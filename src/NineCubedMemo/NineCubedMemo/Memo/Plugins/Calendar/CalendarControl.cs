using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NineCubed.Common.Collections;
using NineCubed.Common.Utils;
using System.IO;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;

namespace NineCubed.Memo.Plugins.Calendar
{
    public partial class CalendarControl : UserControl
    {
        /// <summary>
        /// 現在表示している日付
        /// </summary>
        public DateTime _currentDate = DateTime.Now;

        /// <summary>
        /// 日記ファイル用フォルダ
        /// </summary>
        string _dataDir;

        //プラグインマネージャー
        private PluginManager _pluginManager; 

        //プラグイン
        private IPlugin _plugin;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CalendarControl(IPlugin plugin, PluginManager pluginManager, string dataDir)
        {
            InitializeComponent();

            //引数を保持します
            _pluginManager = pluginManager;
            _plugin = plugin;

            //グリッドを初期化します
            InitGrid();

            //メニューを設定します(タブ閉じるメニューが出るのを防ぐ)
            this.ContextMenuStrip = new ContextMenuStrip();

            //引数を保持します
            _dataDir = dataDir;

            //カレンダーを表示します
            ShowCalendar(_currentDate);
        }

        /// <summary>
        /// グリッドを初期化します
        /// </summary>
        private void InitGrid()
        {
            grid.AllowUserToAddRows    = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid.ReadOnly = true;
            grid.RowTemplate.Height = 21;
            grid.MultiSelect = false;

            string[] headers = { "日", "月", "火", "水", "木", "金", "土" };

            for (int i = 0; i < headers.Length; i++) {
                grid.Columns.Add("header_" + i, headers[i]);

                //ヘッダーを改行しないようにする
                grid.Columns[i].HeaderCell.Style.WrapMode = DataGridViewTriState.False;
            }

            //列の設定
            for (int col = 0; col <= 6; col++) {
                grid.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                grid.Columns[col].DefaultCellStyle.Padding = new Padding(2); //セル内の余白
                grid.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
                grid.Columns[col].Width = 140;
                grid.Columns[col].DefaultCellStyle.WrapMode = DataGridViewTriState.True; //折り返す
            }

            //grid.RowHeadersWidth = 20; //行ヘッダの幅を狭くする
            grid.RowHeadersVisible = false; //行ヘッダの列を非表示にする

            grid.RowCount = 7 * 2;

            //行の高さを設定します
            for (int row = 0; row <= 6; row++) {
                grid.Rows[row * 2 + 1].Height = 80;
            }

            //日付部分のスタイルを設定します
            for (int row = 0; row <= 6; row++) {
                for (int col = 0; col <= 6; col++) {
                    //文字の位置
                    grid[col, row * 2].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    //背景色
                    grid[col, row * 2].Style.BackColor = Color.LightGray;
                }
            }

            //フォントの設定
            grid.Font = new Font("ＭＳ ゴシック", 8);

            grid.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// 指定された年月のカレンダーを表示します
        /// </summary>
        /// <param name="currentDate"></param>
        public void ShowCalendar() => ShowCalendar(_currentDate);
        public void ShowCalendar(DateTime date)
        {
            //日付を保持します
            _currentDate = date;

            //年月を取得します
            int year  = _currentDate.Year;
            int month = _currentDate.Month;

            //グリッドをクリアします
            ClearGrid();

            //年月の設定
            txtYearMonth.Text = year + "/" + month.ToString("00");

            //データの取得
            var map = GetDiaryData(year, month);

            //1日から1日ずつ加算していき、月が変わるまでループします
            var dt = new DateTime(year, month, 1);
            while (dt.Month == month) {
                //カレンダーの位置の取得
                int col = (int)dt.DayOfWeek;               //曜日
                int row = DateTimeUtils.GetWeekNo(dt) - 1; //第何週か

                //日付の設定
                grid[col, row * 2].Value = dt.Day;

                //値の設定
                var yyyyMMdd = DateTimeUtils.GetDateString(dt, "");
                grid[col, row * 2 + 1].Value       = StringUtils.Left(map[yyyyMMdd], 128);
                grid[col, row * 2 + 1].ToolTipText = map[yyyyMMdd];

                //1日加算します
                dt = dt.AddDays(1);
            }
        }

        /// <summary>
        /// 指定された月の日記ファイルを読み込んで、
        /// 日付をキーにした連想配列に入れて返します。
        /// 読み込む量は、先頭の10行、または1024文字まで。
        /// 
        ///   連想配列["yyyyMMdd"] = テキスト
        /// 
        /// 日記ファイルのパス
        /// diary/{yyyy}/{MM}/{yyyyMMdd}.txt  
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private Map<string, string> GetDiaryData(int year, int month)
        {
            var map = new Map<string, string>(); //キー:yyyyMMdd 値:先頭の10行
            var enc = Encoding.GetEncoding(932);
            
            //データフォルダのパスを作成します。{_dataDir}/yyyy/MM
            var dataDirPath = GetDataDirPath(year, month);

            //月末を取得します
            int endDay = DateTimeUtils.GetEndDate(year, month).Day;

            //1日 ～ 月末でループします
            for (int day = 1; day <= endDay; day++) {
                //日記ファイルの有無を確認します 
                var yyyyMMdd = year + month.ToString("00") + day.ToString("00");
                var path = FileUtils.AppendPath(dataDirPath, yyyyMMdd + ".txt");
                if (File.Exists(path) == false) continue;

                //ファイルが存在する場合、先頭の10行を読み込みます
                var list = TextFileUtils.ReadTopLines(path, enc, 10);

                //読み込んだテキストを連想配列に設定します
                map[yyyyMMdd] = StringUtils.Left(string.Join("\n", list).Trim(), 1024);
            }

            return map;
        }

        /// <summary>
        /// 日記のデータフォルダのパスを返します
        /// {_dataDir}/yyyy/MM
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private string GetDataDirPath(int year, int month) {
            return FileUtils.AppendPath(_dataDir, year.ToString(), month.ToString("00"));
        }

        /// <summary>
        /// グリッドの位置から日付を返します
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private string GetDateInGrid(int col, int row)
        {
            int year  = _currentDate.Year;
            int month = _currentDate.Month;

            DateTime dt = new DateTime(year, month, 1);

            int day = (row / 2) * 7 + col - (int)dt.DayOfWeek + 1;
            if (day <= 0 || day > DateTimeUtils.GetEndDate(dt).Day) return null;

            return DateTimeUtils.GetDateString(new DateTime(year, month, day), "-");
        }

        /// <summary>
        /// カレンダーをクリアします
        /// </summary>
        private void ClearGrid()
        {
            for (int row = 0; row < grid.RowCount; row++) {
                for (int col = 0; col < grid.ColumnCount; col++) {
                    grid[col, row].Value = "";
                }
            }
        }

        //最新の情報に更新
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //カレンダーを表示します
            ShowCalendar();

            //アクティブプラグとして設定します
            SetActivePlugin();
        }

        //グリッドダブルクリック
        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OpenMemo(e.ColumnIndex, e.RowIndex);
        }

        //キーダウン・イベント
        private void grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OpenMemo(grid.CurrentCell.ColumnIndex, grid.CurrentCell.RowIndex);
                e.Handled = true;
            }
        }

        /// <summary>
        /// 前月ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, EventArgs e)
        {
            //先月のカレンダーを表示します
            ShowCalendar(_currentDate.AddMonths(-1));

            //アクティブプラグとして設定します
            SetActivePlugin();
        }

        /// <summary>
        /// 先月ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            //翌月のカレンダーを表示します
            ShowCalendar(_currentDate.AddMonths(1));

            //アクティブプラグとして設定します
            SetActivePlugin();
        }

        /// <summary>
        /// 日記ファイルをテキストエディターで開きます
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        private void OpenMemo(int col, int row)
        {
            //行が偶数の場合は、日付行がクリックされているので、+1 します。
            if (row % 2 == 0) row++;

            int year  = _currentDate.Year;
            int month = _currentDate.Month;
            int day   = StringUtils.ToInt(grid[col, row - 1].Value.ToString());
            if (day == 0) return; //セルから日付が取得できない場合は処理しない

            //データフォルダのパスを作成します
            var dataDirPath = GetDataDirPath(year, month);
            var yyyyMMdd = year + month.ToString("00") + day.ToString("00");
            var path = FileUtils.AppendPath(dataDirPath, yyyyMMdd + ".txt");

            //フォルダがない場合は、フォルダを作成します
            FileUtils.CreateDir(dataDirPath);

            //ファイルがない場合は、ファイルを作成します
            if (File.Exists(path) == false) {
                File.Create(path).Close();
            }

            //ファイル選択イベントを発生させます
            var param = new FileSelectedEventParam { Path = path };
            _pluginManager.GetEventManager().RaiseEvent(FileSelectedEventParam.Name, this, param);
        }

        /// <summary>
        /// アクティブプラグインとして設定します
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetActivePlugin()
        {
            _pluginManager.ActivePlugin = _plugin;
        }
        private void grid_Enter           (object sender, EventArgs e) => SetActivePlugin();
        private void pnlTop_Click         (object sender, EventArgs e) => SetActivePlugin();
        private void CalendarControl_Enter(object sender, EventArgs e) => SetActivePlugin();

    } //class
}
