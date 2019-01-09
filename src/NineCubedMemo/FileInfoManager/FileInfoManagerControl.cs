using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileInfoManager.DB;
using NineCubed.Common.Utils;
using System.IO;
using FileInfoManager.Events;
using NineCubed.Memo.Plugins;
using NineCubed.Memo.Plugins.Interfaces;
using NineCubed;
using NineCubed.Memo.Plugins.Events;
using System.Diagnostics;

namespace FileInfoManager
{
    public partial class FileInfoManagerControl : UserControl
    {
        IPlugin _plugin;

        /// <summary>
        /// プラグインマネージャー
        /// </summary>
        PluginManager _pluginManager;

        /// <summary>
        /// DB接続用オブジェクト
        /// </summary>
        FileDB _db;

        /// <summary>
        /// グリッドの列の定義
        /// 列タイトルの表示用と、列を特定するためのキーとして使用しています。
        /// 列の index は、grid.Columns[COL_PATH].Index のようにして取得可能。
        /// </summary>
        const string COL_ID        = "ID";
        const string COL_FILE_NAME = "ファイル名";
        const string COL_PATH      = "パス";
        const string COL_TITLE     = "タイトル";
        const string COL_VALUE     = "評価";
        const string COL_TAG       = "タグ";
        const string COL_CREATED   = "作成日時";
        const string COL_UPDATED   = "更新日時";
        string[] _columnsTitle = {COL_ID, COL_FILE_NAME, COL_PATH, COL_TITLE, COL_VALUE, COL_TAG, COL_CREATED, COL_UPDATED};
        int[]    _columnsWidth = {40,     100,           200,      100,       40,        100,     120,         120};

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="dbPath"></param>
        public FileInfoManagerControl(IPlugin plugin, string dbPath)
        {
            InitializeComponent();

            //プラグインを保持します
            _plugin = plugin;

            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //DB接続用オブジェクトを生成します
            _db = new FileDB(dbPath);
        }

        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileInfoManagerControl_Load(object sender, EventArgs e)
        {
            //グリッドを初期化します
            InitializeGrid();

            //評価のコンボボックスを選択します
            cmbValueFrom.SelectedIndex =  0;
            cmbValueTo  .SelectedIndex = 10;

            //コンボボックスのデフォルト値を選択します
            cmbTag    .SelectedIndex = 0;
            cmbKeyword.SelectedIndex = 0;
        }

        /// <summary>
        /// グリッドを初期化します
        /// </summary>
        private void InitializeGrid()
        {
            grid.AllowUserToAddRows    = false; //追加禁止
            grid.AllowUserToDeleteRows = false; //削除禁止
            grid.AllowUserToResizeRows = false; //リサイズ禁止
            grid.ReadOnly = true; //読み取り専用
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid.RowTemplate.Height = 23;
            //grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; //行選択
            grid.RowHeadersVisible = false; //行ヘッダの列を非表示にする
            grid.Dock = DockStyle.Fill;

            //列を追加します
            Array.ForEach(_columnsTitle, (title) => grid.Columns.Add(title, title));
            grid.Columns[COL_ID]   .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns[COL_VALUE].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            //列の設定をします
            for (int i = 0; i < grid.Columns.Count; i++) {
                DataGridViewColumn column = grid.Columns[i];

                //ヘッダーのタイトルを改行しないようにする
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.False; 

                //列の幅を設定します
                column.Width = _columnsWidth[i];
            }

            //ポップアップメニューを設定します
            SetPopupMenuItem();
        }

        /// <summary>
        /// ポップアップメニューを設定します
        /// </summary>
        private void SetPopupMenuItem()
        {
            //最新の情報に更新
            grid.ContextMenuStrip = new ContextMenuStrip();
            {
                var menu = new ToolStripMenuItem("管理データの削除");
                menu.Click += (sender, e) => {
                    //管理データを削除します
                    DeleteData();
                };
                grid.ContextMenuStrip.Items.Add(menu);
            }
        }

        /// <summary>
        /// 管理データを削除します
        /// </summary>
        private void DeleteData()
        {
            //セルが選択されていない場合は、処理を抜けます
            if (grid.SelectedCells.Count == 0) return;

            var result = MessageBox.Show("管理データを削除してもよろしいですか？\nファイルは削除されません。", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes) {
                //「はい」の場合、管理データを削除します
                using (var connection = _db.GetConnection()) {
                    //トランザクション開始
                    var transaction = connection.BeginTransaction();

                    try {
                        foreach (DataGridViewCell cell in grid.SelectedCells) {
                            //選択されているセルがある行のIDを取得します。
                            var id = StringUtils.ToInt(GetCellValue(COL_ID, cell.RowIndex));

                            //管理データを削除します
                            FileDB.DeleteData(connection, id);
                        }

                        //コミット
                        transaction.Commit();

                    } catch (Exception) {
                        //ロールバック
                        transaction.Rollback();
                    }
                }

                //一覧を更新します
                ((IRefreshPlugin)_plugin).RefreshData();
            }
        }

        /// <summary>
        /// 検索ボタン クリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        /// <summary>
        /// キーダウンイベント
        /// エンターキーが押された場合は、検索します
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTag_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) Search();
        }
        private void txtKeyword_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) Search();
        }

        /// <summary>
        /// ファイルデータを検索します
        /// </summary>
        public void Search()
        {
            //キーワードの検索モード(and, or, not)を取得します
            Enum.TryParse(cmbKeyword.SelectedItem.ToString(), true, out FileDataDao.SearchMode searchModeKeyword);

            //タグの検索モード(and, or, not)を取得します
            Enum.TryParse(cmbTag.SelectedItem.ToString(), true, out FileDataDao.SearchMode searchModeTag);

            //入力されたキーワードをスペースで分割してリストにします
            var keywordList = ListUtils.Split(" ", txtKeyword.Text.Replace('　', ' '));

            //入力されたタグをスペースで分割してリストにします
            var tagList     = ListUtils.Split(" ", txtTag.Text.Replace('　', ' '));

            //データを検索します
            var dataList = FileDataDao.GetDataList(_db.GetConnection(),
                cmbValueFrom.SelectedIndex,
                cmbValueTo.SelectedIndex,
                keywordList,
                tagList,
                searchModeTag,
                searchModeKeyword);

            //行を全て削除します
            grid.Rows.Clear();

            //行にデータを追加します
            foreach (var data in dataList) {

                //行データを作成します
                object[] rowData = {
                    data.id,                     //ID
                    Path.GetFileName(data.path), //ファイル名
                    data.path,                   //パス
                    data.title,                  //タイトル
                    data.value,                  //評価
                    data._tags,                  //タグ
                    data.created,                //作成日時
                    data.updated                 //更新日時
                };

                //行を追加します
                var rowIndex = grid.Rows.Add(rowData);

                //ファイルが存在しない場合は背景色をピンクにします
                CheckFile(data.path, rowIndex);
            }
        }

        //評価の背景色の設定
        private void CheckFile(string path, int row)
        {
            int col = grid.Columns[COL_PATH].Index;

            if (FileUtils.Exists(path)) {
                //ファイルがある場合
                grid[col, row].Style.BackColor = Color.White;
            } else {
                //ファイルがない場合
                grid[col, row].Style.BackColor = Color.LightPink;
            }
        }

        /// <summary>
        /// フォーカスを受け取った時のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbValueFrom_Enter(object sender, EventArgs e) => EnterEvent();
        private void   cmbValueTo_Enter(object sender, EventArgs e) => EnterEvent();
        private void       txtTag_Enter(object sender, EventArgs e) => EnterEvent();
        private void       cmbTag_Enter(object sender, EventArgs e) => EnterEvent();
        private void   txtKeyword_Enter(object sender, EventArgs e) => EnterEvent();
        private void         grid_Enter(object sender, EventArgs e) => EnterEvent();
        private void EnterEvent() {
            //アクティブプラグインを設定します
            _pluginManager.ActivePlugin = _plugin;
        }

        /// <summary>
        /// 指定したセルの値を返します
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private string GetCellValue(string colName, int rowIndex)
        {
            int colIndex = grid.Columns[colName].Index;
            return grid[colIndex, rowIndex].Value.ToString();
        }

        /// <summary>
        /// グリッドのダブルクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return; //ヘッダーダブルクリックは無視する

            //ダブルクリックされた行のパスを取得します。
            var path = GetCellValue(COL_PATH, e.RowIndex);

            //フォルダまたはファイル選択イベントを発生させます
            RaiseSelectedEvent(path);
        }

        /// <summary>
        /// ファイルリストで選択されたフォルダまたはファイルの選択イベントを発生させます
        /// </summary>
        public void RaiseSelectedEvent(string path)
        {
            if (FileUtils.IsFile(path)) {
                //ファイルの場合、ファイル選択イベントを発生させます
                var param = new FileSelectedEventParam { Path = path };
                _pluginManager.GetEventManager().RaiseEvent(FileSelectedEventParam.Name, this, param);
            } else {
                //フォルダの場合、フォルダ選択イベントを発生させます
                var param = new DirSelectedEventParam { Path = path };
                _pluginManager.GetEventManager().RaiseEvent(DirSelectedEventParam.Name, this, param);
            }
        }

        /// <summary>
        /// グリッドの選択しているセルが変更された時のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_SelectionChanged(object sender, EventArgs e)
        {
            if (grid.CurrentCell          == null) return; //カレントセルが未設定の場合は処理しない
            if (grid.CurrentCell.RowIndex == -1)   return; //ヘッダーダブルクリックは無視する

            //選択された行からIDを取得します
            var id = GetCellValue(COL_ID, grid.CurrentRow.Index);

            //ファイル情報選択イベントを発生させます
            var param = new FileInfoSelectingEventParam { ID = id };
            _pluginManager.GetEventManager().RaiseEvent(FileInfoSelectingEventParam.Name, _plugin, param);

        }

        /// <summary>
        /// タグボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTag_Click(object sender, EventArgs e)
        {
            __.ShowInfoMsgBox("すみません。まだ実装してしません。");
        }

        /// <summary>
        /// 編集ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (grid.CurrentCell == null) return;
            if (grid.CurrentCell.RowIndex == -1) return; //セルが選択されていない場合は処理しない

            //現在選択されている行のIDを取得します
            var id = GetCellValue(COL_ID, grid.CurrentCell.RowIndex);

            //プラグイン生成パラメーターを設定します
            var pluginCreateParam = new PluginCreateParam { ["id"] = id };

            //ファイル情報エディタープラグイン(プラグインID固定:file_info_editor)を生成します
            //(既に存在する場合は、既存のプラグインを返ります)
            var editorPlugin = (FileInfoEditorPlugin)_pluginManager.CreatePluginInstance(
                typeof(FileInfoEditorPlugin), pluginCreateParam, this, null, 
                "file_info_editor");
            if (editorPlugin == null) return; //TODO プラグイン生成エラー。定義ファイルのミスなど。

            //ファイル情報選択イベントを発生させます
            var param = new FileInfoSelectingEventParam { ID = id };
            _pluginManager.GetEventManager().RaiseEvent(FileInfoSelectingEventParam.Name, _plugin, param);
        }

        /// <summary>
        /// 現在選択されている行のパスを返します
        /// </summary>
        /// <returns></returns>
        private string GetSelectedPath() {
            if (grid.CurrentRow == null)   return null;
            if (grid.CurrentRow.Index < 0) return null;

            //ダブルクリックされた行のパスを取得します。
            return GetCellValue(COL_PATH, grid.CurrentRow.Index);
        }

        /// <summary>
        /// 現在選択されている行のIDを返します
        /// </summary>
        /// <returns></returns>
        private int GetSelectedID() {
            if (grid.CurrentRow == null)   return -1;
            if (grid.CurrentRow.Index < 0) return -1;

            //ダブルクリックされた行のIDを取得します。
            var id = GetCellValue(COL_ID, grid.CurrentRow.Index);
            return StringUtils.ToInt(id);
        }

        /// <summary>
        /// グリッドのキーダウンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_KeyDown(object sender, KeyEventArgs e)
        {
            //CTRL + エンターキーの場合は、ネイティブな方法でファイルを開きます
            if (e.Control && e.KeyCode == Keys.Enter) {
                //選択されているセルがある行のパスを取得します
                var path = GetSelectedPath();
                if (path == null) return;
                if (File.Exists(path) == false) return;

                //ネイティブな方法でファイルを開きます
                Process.Start(path);

                //キー操作を処理済みにします
                e.Handled = true;
                return;
            }

            //エンターキーが押された場合は、フォルダorファイル選択イベントを発生させます
            if (e.KeyCode == Keys.Enter) {
                //選択されているセルがある行のパスを取得します
                var path = GetSelectedPath();
                if (path == null) return;

                //フォルダまたはファイル選択イベントを発生させます
                RaiseSelectedEvent(path);

                //キー操作を処理済みにします
                e.Handled = true;
                return;
            }

            //DELETEキーが押された場合は、管理データを削除します
            if (e.KeyCode == Keys.Delete) {
                //管理データを削除します
                DeleteData();
            }
        }


    } //class
}
