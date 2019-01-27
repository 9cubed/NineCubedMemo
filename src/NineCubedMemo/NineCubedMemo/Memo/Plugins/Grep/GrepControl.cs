using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NineCubed.Memo.Plugins.Interfaces;
using System.Threading;
using NineCubed.Common.Files;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Events;

namespace NineCubed.Memo.Plugins.Grep
{
    public partial class GrepControl : UserControl
    {
        /// <summary>
        /// プラグインマネージャー
        /// </summary>
        private PluginManager _pluginManager; 

        /// <summary>
        /// プラグイン
        /// </summary>
        private IPlugin _plugin;

        /// <summary>
        /// Grep用スレッド
        /// </summary>
        GrepThread _grepThread;

        public bool NotIgnoreCase { get => chkNotIgnoreCase.Checked; }
        public bool IncludeSubDir { get => chkSubDir.Checked; }
        public bool UseRegExp     { get => chkRegExp.Checked; }
        public string Extension   { get => txtExtension.Text; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GrepControl(IPlugin plugin, PluginManager pluginManager, string path, bool notIgnoreCase, bool includeSubDir, bool useRegExp, string extension, string fontName, float fontSize)
        {
            InitializeComponent();

            //引数を保持します
            _pluginManager = pluginManager;
            _plugin = plugin;

            //画面の初期値を設定します
            txtPath.Text = path;
            chkNotIgnoreCase.Checked = notIgnoreCase;
            chkSubDir       .Checked = includeSubDir;
            chkRegExp       .Checked = useRegExp;
            txtExtension.Text = extension;
            lblMsg      .Text = "";
            txtResult.WordWrap = false; //折り返ししない(折り返すと行番号がずれるため)

            //フォントを設定します
            txtResult.Font = new Font(fontName, fontSize);

            //Dockを設定します
            splitResult  .Dock = DockStyle.Fill;
            grid         .Dock = DockStyle.Fill;
            txtResult    .Dock = DockStyle.Fill;

            //グリッドの初期化をします
            InitGrid();

            //メニューを設定します(タブ閉じるメニューが出るのを防ぐ)
            this.ContextMenuStrip = new ContextMenuStrip();
            /*{
                var menu = new ToolStripMenuItem("");
                menu.Click += (sender, e) => { };
            }*/
        }

        //カラムのIndex
        const int COL_PATH   = 0; //パス
        const int COL_LINENO = 1; //行番号
        const int COL_COL    = 2; //列
        const int COL_MATCH  = 3; //該当文字列
        const int COL_LINE   = 4; //行の内容
        const int COL_ENCORD = 5; //文字コード

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
            grid.RowHeadersVisible = false; //行ヘッダーの列を非表示にします

            string[] headers = { "パス", "行番号", "列", "文字列", "内容", "文字コード" };

            for (int i = 0; i < headers.Length; i++) {
                grid.Columns.Add("header_" + i, headers[i]);

                //ヘッダーを改行しないようにする
                grid.Columns[i].HeaderCell.Style.WrapMode  = DataGridViewTriState.False;
                grid.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            //列の幅を設定します
            grid.Columns[COL_PATH  ].Width = 300;
            grid.Columns[COL_LINENO].Width = 50;
            grid.Columns[COL_COL   ].Width = 50;
            grid.Columns[COL_MATCH ].Width = 100;
            grid.Columns[COL_LINE  ].Width = 400;
            grid.Columns[COL_ENCORD].Width = 80;

            //右揃えにします
            grid.Columns[COL_LINENO].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns[COL_COL]   .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        /// <summary>
        /// 検索ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            Grep();
        }

        /// <summary>
        /// キャンセルボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //キャンセルフラグを設定します
            _grepThread.CancelFlg = true;
        }

        private void Grep()
        {
            //パス、キーワードが空の場合は処理しない
            if (string.IsNullOrEmpty(  txtPath.Text)) return;
            if (string.IsNullOrEmpty(txtSearch.Text)) return;

            //進捗状況を設定します
            lblMsg.Text = "検索中・・・";

            //ボタンのEnabledの設定
            btnSearch.Enabled = false; //検索ボタン       -> 使用不可
            btnCancel.Enabled = true;  //キャンセルボタン -> 使用可

            //行を全て削除します
            grid.Rows.Clear();

            //Grep用スレッドを実行します
            _grepThread = new GrepThread(
                this,
                txtPath     .Text,
                txtExtension.Text,
                txtSearch   .Text,
                chkSubDir       .Checked,
                chkNotIgnoreCase.Checked,
                chkRegExp       .Checked);
            Thread workerThread = new Thread(_grepThread.Invoke);
            workerThread.Start();
        }

        /// <summary>
        /// グリッドに検索結果を1行追加します。
        /// スレッド側からGrepの結果を1ファイルずつ返してもらう際に呼ばれます。
        /// </summary>
        /// <param name="path">パス</param>
        /// <param name="lineNo">行番号</param>
        /// <param name="lineNo">列</param>
        /// <param name="matchString">該当文字列</param>
        /// <param name="line">1行分のテキスト</param>
        /// <param name="encode">文字コード</param>
        public void AddGrepResult(string path, int lineNo, int col, string matchString, string line, string encode)
        {
            if (this.IsDisposed) return; //スレッド実行中にフォームが閉じられた場合

            //Invoke() を使う必要があるかどうか？ -> スレッドからしか呼ばれないため今回は不要
            //if (this.InvokeRequired) {}

            this.Invoke((MethodInvoker)(() => {
                var data = new String[grid.ColumnCount];
                data[COL_PATH]   = path;
                data[COL_LINENO] = lineNo.ToString();
                data[COL_COL]    = col   .ToString();
                data[COL_MATCH]  = matchString;
                data[COL_LINE]   = line;
                data[COL_ENCORD] = encode;
                grid.Rows.Add(data);
            }));
        }

        /// <summary>
        /// Grep完了メソッド
        /// Grepスレッドの処理が完了した際に呼ばれます。
        /// </summary>
        /// <param name="path">処理中のパス</param>
        public void GrepProgress(string path)
        {
            if (this.IsDisposed) return; //スレッド実行中にフォームが閉じられた場合

            this.Invoke((MethodInvoker)(() => {
                //進捗状況を設定します
                lblMsg.Text = path;
            }));
        }

        /// <summary>
        /// Grep完了メソッド
        /// Grepスレッドの処理が完了した際に呼ばれます。
        /// </summary>
        public void GrepFinished(string msg)
        {
            if (this.IsDisposed) return; //スレッド実行中にフォームが閉じられた場合
            try {
                this.Invoke((MethodInvoker)(() => {

                    //進捗状況を設定します
                    lblMsg.Text = msg;

                    //ボタンのEnabledの設定
                    btnSearch.Enabled = true;  //検索ボタン       -> 使用可
                    btnCancel.Enabled = false; //キャンセルボタン -> 使用不可
                }));
            } catch (Exception) { }
        }

        /// <summary>
        /// セルダブルクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return; //ヘッダーは無視する

            //ダブルクリックされたパスと行番号を取得します
            var path = grid[COL_PATH, grid.CurrentCell.RowIndex].Value.ToString();

            //ファイル選択イベントを発生させます
            var param = new FileSelectedEventParam { Path = path };
            _pluginManager.GetEventManager().RaiseEvent(FileSelectedEventParam.Name, this, param);
        }

        private string _oldPath = ""; //前回開いていたパス

        /// <summary>
        /// セル選択イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_SelectionChanged(object sender, EventArgs e)
        {
            if (grid.CurrentCell          == null) return; //セルが未選択の場合は処理しない
            if (grid.CurrentCell.RowIndex == -1)   return; //ヘッダーは無視する

            //ダブルクリックされたパスと行番号を取得します
            int gridRow = grid.CurrentCell.RowIndex;
            var path        =                   grid[COL_PATH  , gridRow].Value.ToString();  //検索ファイルのパス
            var lineNo      = StringUtils.ToInt(grid[COL_LINENO, gridRow].Value.ToString()); //テキストの行番号
            var col         = StringUtils.ToInt(grid[COL_COL,    gridRow].Value.ToString()); //テキストの列インデックス
            var matchString =                   grid[COL_MATCH,  gridRow].Value.ToString();  //該当文字列(色を付ける際の文字列長の取得に使用)

            //現在開いているファイルとは異なる場合は、ファイルを開きます
            if (path.Equals(_oldPath) == false) {
                //異なる場合、ファイルを読み込みます
                _oldPath = path;
                var textFile = new TextFile();
                try {
                    textFile.Load(path);
                } catch (Exception ex) {
                    MsgBoxUtils.ShowErrorMsgBox(ex.Message);
                    return;
                }
                txtResult.Text = textFile.Text;
            }

            //該当箇所にカーソルを移動する
            int index = txtResult.GetFirstCharIndexFromLine(lineNo - 1);

            //該当箇所に色をつけます
            txtResult.Select(index + col, matchString.Length);
            txtResult.SelectionBackColor = Color.Yellow;

            //フォーカスを移動しないと色がつかないので、フォーカスを移動して元に戻します
            txtResult.Focus();
            grid.Focus();
        }

    } //class
}
