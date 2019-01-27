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
using FileInfoManager.Editor;
using FileInfoManager.Manager.Columns;
using NineCubed.Common.Controls.FileList;
using FileInfoManager.Manager.Menus;

namespace FileInfoManager.Manager
{
    public partial class FileInfoManagerControl : UserControl
    {
        FileInfoManagerPlugin _plugin;

        /// <summary>
        /// プラグインマネージャー
        /// </summary>
        PluginManager _pluginManager;

        /// <summary>
        /// DB接続用オブジェクト
        /// </summary>
        FileDB _db;

        //列のIndex
        struct ColIndex {
            public int ID;       //ID
            //public int Kind;     //種別
            public int Title;    //タイトル
            public int Value;    //評価
            public int Tag;      //タグ
            public int FileName; //ファイル名
            public int FileExt;  //拡張子
            public int FileSize; //ファイルサイズ
            public int Path;     //パス
            public int Created;  //作成日時
            public int Updated;  //更新日時
        }
        ColIndex _colIndexes;

        DataGridViewEx _grid;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="dbPath"></param>
        public FileInfoManagerControl(FileInfoManagerPlugin plugin, string dbPath)
        {
            InitializeComponent();

            //プラグインを保持します
            _plugin = plugin;

            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //DB接続用オブジェクトを生成します
            _db = new FileDB(dbPath);

            //グリッドを生成します
            _grid = new DataGridViewEx();
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
            _grid.AllowUserToAddRows    = false; //追加禁止
            _grid.AllowUserToDeleteRows = false; //削除禁止
            _grid.AllowUserToResizeRows = false; //リサイズ禁止
            //grid.ReadOnly = true; //読み取り専用
            _grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _grid.RowTemplate.Height = 23;
            //grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; //行選択
            _grid.RowHeadersVisible = false; //行ヘッダの列を非表示にする
            _grid.Dock = DockStyle.Fill;
            _grid.Parent = this;
            _grid.BringToFront();

            //イベントを設定します
            _grid.CellDoubleClick  += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellDoubleClick);
            _grid.CellEndEdit      += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellEndEdit);
            _grid.CellPainting     += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.grid_CellPainting);
            _grid.SelectionChanged += new System.EventHandler(this.grid_SelectionChanged);
            _grid.SortCompare      += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.grid_SortCompare);
            _grid.Enter            += new System.EventHandler(this.grid_Enter);
            _grid.KeyDown          += new System.Windows.Forms.KeyEventHandler(this.grid_KeyDown);
            _grid.MouseMove        += new System.Windows.Forms.MouseEventHandler(this.grid_MouseMove);
            
            //列を追加します
            _colIndexes.ID       = _grid.Columns.Add(new            IDColumn(_db)); //ID
            _colIndexes.Title    = _grid.Columns.Add(new         TitleColumn(_db)); //タイトル
            _colIndexes.Value    = _grid.Columns.Add(new         ValueColumn(_db)); //評価
            _colIndexes.Tag      = _grid.Columns.Add(new           TagColumn(_db)); //タグ
            _colIndexes.FileName = _grid.Columns.Add(new      FileNameColumn(_db)); //ファイル名
            _colIndexes.FileExt  = _grid.Columns.Add(new FileExtensionColumn(_db)); //拡張子
            _colIndexes.FileSize = _grid.Columns.Add(new      FileSizeColumn(_db)); //ファイルサイズ
            _colIndexes.Path     = _grid.Columns.Add(new          PathColumn(_db)); //パス
            _colIndexes.Created  = _grid.Columns.Add(new   CreatedDateColumn(_db)); //作成日時
            _colIndexes.Updated  = _grid.Columns.Add(new   UpdatedDateColumn(_db)); //更新日時

            //評価カラムに背景色を設定します
            var valueColumn = (ValueColumn)_grid.Columns[_colIndexes.Value];
            for (int i = 0; i <= 10; i++) {
                valueColumn.ValueColor[i] = Color.FromArgb((int)((long)0xff000000 + _plugin.Property.ToInt("value", "color_" + i)));
            }

            //列の設定をします
            foreach (DataGridViewColumn column in _grid.Columns) {
                //ヘッダーのタイトルを改行しないようにする
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.False; 
            }

            //ポップアップメニューを設定します
            SetPopupMenuItem();
        }

        /// <summary>
        /// ポップアップメニューを設定します
        /// </summary>
        private void SetPopupMenuItem()
        {
            //フォルダを開く
            _grid.ContextMenuStrip = new ContextMenuStrip();
            {
                var menu = new ToolStripMenuItem("フォルダを開く");
                menu.Click += (sender, e) => {
                    //選択されているセルがある行のパスを取得します
                    var path = GetSelectedPath();
                    if (path == null) return;
                    var dirPath = Path.GetDirectoryName(path);
                    if (Directory.Exists(dirPath) == false) {
                        MsgBoxUtils.ShowErrorMsgBox(path + "が見つかりません。");
                        return;
                    }

                    //ネイティブな方法でフォルダを開きます
                    Process.Start(dirPath);
                };
                _grid.ContextMenuStrip.Items.Add(menu);
            }

            //ファイルを開く
            {
                var menu = new ToolStripMenuItem("ファイルを開く");
                menu.Click += (sender, e) => {
                    //選択されているセルがある行のパスを取得します
                    var path = GetSelectedPath();
                    if (path == null) return;
                    if (FileUtils.Exists(path) == false) {
                        MsgBoxUtils.ShowErrorMsgBox(path + "が見つかりません。");
                        return;
                    } 

                    //フォルダまたはファイル選択イベントを発生させます
                    RaiseSelectedEvent(path);
                };
                _grid.ContextMenuStrip.Items.Add(menu);
            }

            //ネイティブな方法でファイルを開く
            {
                var menu = new ToolStripMenuItem("ネイティブな方法でファイルを開く");
                menu.Click += (sender, e) => {
                    //選択されているセルがある行のパスを取得します
                    var path = GetSelectedPath();
                    if (path == null) return;
                    if (FileUtils.Exists(path) == false) {
                        MsgBoxUtils.ShowErrorMsgBox(path + "が見つかりません。");
                        return;
                    } 

                    //ネイティブな方法でフォルダを開きます
                    Process.Start(path);
                };
                _grid.ContextMenuStrip.Items.Add(menu);
            }

            //管理データの削除
            {
                var menu = new ToolStripMenuItem("管理データの削除");
                menu.Click += (sender, e) => {
                    //現在選択中のIDリストを取得します
                    var idList = GetSelectedIDList();
                    if (idList.Count == 0) return;

                    //管理データを削除します
                    DeleteData(idList);
                };
                _grid.ContextMenuStrip.Items.Add(menu);
            }

            //評価
            {
                var menu = new ToolStripMenuItem { Text= "評価" };
                _grid.ContextMenuStrip.Items.Add(menu);

                //評価(0 - 10)ごとにメニューを作成します
                for (int value = 0; value <= 10; value++) {
                    menu.DropDownItems.Add(new UpdateValueMenu(this, _db, _grid, _colIndexes.Value, value));
                }
            }

            //タグ
            {
                var menu = new ToolStripMenuItem("タグ");
                _grid.ContextMenuStrip.Items.Add(menu);
                
                menu.DropDownItems.Add(new    AddTagMenu(this, _db, _grid, _colIndexes.Tag)); //タグ追加
                menu.DropDownItems.Add(new UpdateTagMenu(this, _db, _grid, _colIndexes.Tag)); //タグ更新
                menu.DropDownItems.Add(new DeleteTagMenu(this, _db, _grid, _colIndexes.Tag)); //タグ削除
            }
        }

        /// <summary>
        /// 管理データを削除します
        /// </summary>
        private void DeleteData(IList<int> idList)
        {
            //セルが選択されていない場合は、処理を抜けます
            if (_grid.SelectedCells.Count == 0) return;

            var result = MessageBox.Show("管理データを削除してもよろしいですか？\nファイルは削除されません。", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes) {
                var deleteIDList = new List<int>(); //グリッドから行を削除するためのIDリスト

                //「はい」の場合、管理データを削除します
                using (var connection = _db.GetConnection()) {
                    //トランザクション開始
                    var transaction = connection.BeginTransaction();

               
                    try {
                        foreach (var id in idList) {
                            //管理データを削除します
                            FileDB.DeleteData(connection, id);

                            //削除用IDリストにIDを追加します
                            deleteIDList.Add(id);
                        }

                        //コミット
                        transaction.Commit();

                    } catch (Exception ex) {
                        //ロールバック
                        transaction.Rollback();
                        MsgBoxUtils.ShowErrorMsgBox("削除に失敗しました。\n" + ex.Message);
                    }
                }

                //一覧を更新します
                foreach(var id in deleteIDList) {
                    //指定したIDの行を取得します
                    int row = GetRowIndex(id);

                    //行を削除します
                    _grid.Rows.RemoveAt(row);
                }
            }
        }

        /// <summary>
        /// 検索ボタン クリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            ShowFileList();
        }

        /// <summary>
        /// キーダウンイベント
        /// エンターキーが押された場合は、検索します
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTag_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) ShowFileList();
        }
        private void txtKeyword_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) ShowFileList();
        }

        /// <summary>
        /// ファイルデータを検索して、結果を一覧で表示します
        /// </summary>
        public void ShowFileList()
        {
            //キーワードの検索モード(and, or, not)を取得します
            Enum.TryParse(cmbKeyword.SelectedItem.ToString(), true, out FileDataDao.SearchMode searchModeKeyword);

            //タグの検索モード(and, or, not)を取得します
            Enum.TryParse(cmbTag.SelectedItem.ToString(), true, out FileDataDao.SearchMode searchModeTag);

            //入力されたキーワードをスペースで分割してリストにします
            var keywordList = ListUtils.Split(" ", txtKeyword.Text.Replace('　', ' '));

            //入力されたタグをスペースで分割してリストにします
            var tagList     = ListUtils.Split(" ", txtTag.Text.Replace('　', ' '));

            //検索結果の表示件数を取得します
            int limit = StringUtils.ToInt(txtDisplayCount.Text, 0);

            //データの件数を取得します
            int count = FileDataDao.GetDataListCount(_db.GetConnection(),
                cmbValueFrom.SelectedIndex,
                cmbValueTo.SelectedIndex,
                keywordList,
                tagList,
                searchModeTag,
                searchModeKeyword,
                chkFile.Checked,
                chkDir.Checked);

            //データを検索します
            var dataList = FileDataDao.GetDataList(_db.GetConnection(),
                cmbValueFrom.SelectedIndex,
                cmbValueTo.SelectedIndex,
                keywordList,
                tagList,
                searchModeTag,
                searchModeKeyword, 
                chkFile.Checked,
                chkDir.Checked,
                limit);

            //列の初期化処理を実行します
            foreach (IFileListColumn column in _grid.Columns) {
                column.ShowFileListBefore();
            }

            //行を全て削除します
            _grid.Rows.Clear();

            //行にデータを追加します
            foreach (var data in dataList) {

                //行データの生成
                var rowData = new object[_grid.ColumnCount];

                //行を追加します
                var rowIndex = _grid.Rows.Add(rowData);

                //行データを設定します
                SetRowData(rowIndex, data);
            }

            //列の解放処理を実行します
            foreach (IFileListColumn column in _grid.Columns) {
                column.ShowFileListAfter();
            }

            //グリッドにフォーカスを移動します
            _grid.Focus();

            //カーソルを一番上に移動します
            if (_grid.RowCount > 0) {
                _grid.CurrentCell = _grid[0, 0];
            }

            //検索結果の表示
            lblResult.Text = "検索結果：" + count + "件";
        }

        //行データを設定します
        public void SetRowData(int rowIndex, FileData fileData)
        {
            //行データの設定
            int colIndex = 0;
            foreach (IFileListColumnForFileInfo column in _grid.Columns) {
                _grid[colIndex, rowIndex].Value = column.ToString(fileData); //値の設定
                _grid[colIndex, rowIndex].Style.BackColor = column.GetBackColor(fileData); //背景色の設定
                colIndex++;
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
        private void       pnlTop_Click(object sender, EventArgs e) => EnterEvent();
        private void EnterEvent() {
            //アクティブプラグインを設定します
            _pluginManager.ActivePlugin = _plugin;
        }

        /// <summary>
        /// グリッドのダブルクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return; //ヘッダーダブルクリックは無視する

            //選択されているパスを取得します
            var path = GetSelectedPath();
            if (path == null) return;
            if (FileUtils.Exists(path) == false) {
                MsgBoxUtils.ShowErrorMsgBox(path + "が見つかりません。");
                return;
            } 
            
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
            if (_grid.CurrentCell          == null) return; //カレントセルが未設定の場合は処理しない
            if (_grid.CurrentCell.RowIndex == -1)   return; //ヘッダーダブルクリックは無視する

            {
                //選択行のIDを取得します
                int id = GetSelectedID();
                if (id >= 1) {
                    //ファイル情報選択中イベントを発生させます
                    var param = new FileInfoSelectingEventParam { ID = id.ToString() };
                    _pluginManager.GetEventManager().RaiseEvent(FileInfoSelectingEventParam.Name, _plugin, param);
                }
            }

            {
                //選択行のパスを取得します
                var path = GetSelectedPath();
                if (StringUtils.IsNotEmpty(path)) {
                    //ファイル選択中イベントを発生させます
                    var param = new FileSelectingEventParam { Path = path };
                    _pluginManager.GetEventManager().RaiseEvent(FileSelectingEventParam.Name, _plugin, param);
                }
            }
        }

        /// <summary>
        /// タグボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTag_Click(object sender, EventArgs e)
        {
            //タグの編集画面を表示します
            using(var form = new TagForm(TagForm.Mode.Select, _db, txtTag.Text)) {
                //画面を表示します
                form.ShowDialog();
                if (form.SelectedTagList == null) return;

                //タグ欄に選択されたタグを設定します
                txtTag.Text = string.Join(" ", form.SelectedTagList);
            }
        }

        /// <summary>
        /// 編集ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_grid.CurrentCell == null) return;
            if (_grid.CurrentCell.RowIndex == -1) return; //セルが選択されていない場合は処理しない

            //現在選択されている行のIDを取得します
            var id = _grid[_colIndexes.ID, _grid.CurrentCell.RowIndex].Value?.ToString();

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
            if (_grid.CurrentRow == null)   return null;
            if (_grid.CurrentRow.Index < 0) return null;

            //選択されている行のパスを取得します
            var path     = _grid[_colIndexes.Path,     _grid.CurrentRow.Index].Value?.ToString();
            var fileName = _grid[_colIndexes.FileName, _grid.CurrentRow.Index].Value?.ToString();

            return FileUtils.AppendPath(path, fileName);
        }

        /// <summary>
        /// 現在選択されている行のIDを返します
        /// </summary>
        /// <returns></returns>
        private int GetSelectedID() {
            if (_grid.CurrentRow == null)   return -1;
            if (_grid.CurrentRow.Index < 0) return -1;

            //ダブルクリックされた行のIDを取得します。
            var id = _grid[_colIndexes.ID, _grid.CurrentRow.Index].Value?.ToString();
            if (id == null) return -1;

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
                if (FileUtils.Exists(path) == false) {
                    MsgBoxUtils.ShowErrorMsgBox(path + "が見つかりません。");
                    return;
                } 

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
                //現在選択中のIDリストを取得します
                var idList = GetSelectedIDList();
                if (idList.Count == 0) return;

                //管理データを削除します
                DeleteData(idList);
            }
        }

        /// <summary>
        /// ソートします
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            //ソートします
            e.SortResult = ((IFileListColumn)e.Column).SortCompare(
                e.CellValue1?.ToString(),
                e.CellValue2?.ToString());
            e.Handled = true;
        }

        /// <summary>
        /// グリッドの手入力後に発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var id       = _grid[0,             e.RowIndex].Value.ToString();  //ID
            var newValue = _grid[e.ColumnIndex, e.RowIndex].Value?.ToString(); //入力値
            if (newValue == null) newValue = "";

            FileData oldFileData;
            using (var connection = _db.GetConnection()) {
                oldFileData = FileDataDao.GetData(connection, id);
                if (oldFileData == null) {
                    MsgBoxUtils.ShowErrorMsgBox("このデータは削除されています。");
                    return;
                }

                //タグを取得します
                oldFileData._tags = TagDataDao.GetTags(connection, StringUtils.ToInt(id));
            }

            //カラムオブジェクトに値の変更を通知します(カラム側ではファイル名の変更などを行う)
            var newFileData = ((IFileListColumnForFileInfo)_grid.Columns[e.ColumnIndex]).ValueChanged(oldFileData, newValue);
            if (newFileData == null) newFileData = oldFileData;
            
            //行の更新します
            SetRowData(e.RowIndex, newFileData);
        }

        /// <summary>
        /// 列を描画します
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            ((AFileListColumn)_grid.Columns[e.ColumnIndex]).CellPainting(sender, e);
        }

        /// <summary>
        /// 選択されているパスリストを返します
        /// </summary>
        /// <returns></returns>
        private IList<string> GetSelectedPathList()
        {
            var list = new List<string>();
            foreach(DataGridViewCell cell in _grid.SelectedCells) {
                //選択されている行のパスを取得します
                var dirPath  = _grid[_colIndexes.Path,     cell.RowIndex].Value?.ToString();
                var fileName = _grid[_colIndexes.FileName, cell.RowIndex].Value?.ToString();
                if (string.IsNullOrEmpty(dirPath) || string.IsNullOrEmpty(fileName)) continue;
                var path = FileUtils.AppendPath(dirPath, fileName);

                //リストに追加されていない場合は、リストに追加します
                if (list.IndexOf(path) == -1)list.Add(path);
            }
            
            return list;
        }

        /// <summary>
        /// 選択されているIDリストを返します
        /// </summary>
        /// <returns></returns>
        public IList<int> GetSelectedIDList()
        {
            var list = new List<int>();
            if (_grid.SelectedCells == null)    return list;
            if (_grid.SelectedCells.Count == 0) return list;

            foreach(DataGridViewCell cell in _grid.SelectedCells) {
                //選択されている行のIDを取得します
                var strId = _grid[_colIndexes.ID, cell.RowIndex].Value?.ToString();
                if (string.IsNullOrEmpty(strId)) continue;

                //リストに追加されていない場合は、リストに追加します
                int id = StringUtils.ToInt(strId);
                if (list.IndexOf(id) == -1) list.Add(id);
            }
            
            return list;
        }

        /// <summary>
        /// グリッドのマウスムーブイベント。
        /// 選択されているファイルのドラッグを開始します。
        /// 誤ってファイルを移動してしまうことを防ぐため、
        /// コピー限定のドラッグにします。
        /// 
        /// [メモ]
        /// DoDragDrop() でドラッグが開始されると、ドロップされるまで、
        /// MouseMove と MouseUp は発生しない。
        /// 
        /// ドラッグ中に頻繁に System.Runtime.InteropServices.COMException 
        /// が発生するが、これは正常な動作で特に問題はない。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_MouseMove(object sender, MouseEventArgs e)
        {
            //マウスボタンが押されていない場合は処理しない
            if (e.Button == MouseButtons.None) return;

            //クリックされた位置のセルを取得します
            var hitTestInfo = _grid.HitTest(e.X, e.Y);
            var row = hitTestInfo.RowIndex;
            if (row == -1) return; //行が選択されている場合は、処理しない

            //選択されているファイルリストを取得します
            var pathList = GetSelectedPathList().ToArray();
            if (pathList.Count() == 0) return;

            //ドラッグを開始します
            var dataObj = new DataObject(DataFormats.FileDrop, pathList);
            var effect = DragDropEffects.Copy; // | DragDropEffects.Move;
            _grid.DoDragDrop(dataObj, effect);
        }

        /// <summary>
        /// 指定したIDのデータがあるRowを取得します
        /// </summary>
        /// <param name="targetID"></param>
        /// <returns></returns>
        public int GetRowIndex(int targetID)
        {
            for (int row = 0; row < _grid.RowCount; row++) {
                int id = StringUtils.ToInt(_grid[_colIndexes.ID, row].Value?.ToString());
                if (targetID == id) {
                    //IDが一致したRowを返します
                    return row;
                }
            }
            
            return -1; //IDが見つからなかった場合
        }



    } //class

    /// <summary>
    /// 複数選択した状態でドラッグをできるようにしたグリッドです。
    /// 
    /// 複数選択した状態でマウスダウンすると、選択状態が解除されるため、
    /// 解除しないようにしています。
    /// </summary>
    public class DataGridViewEx : DataGridView {
        protected override void OnMouseDown(MouseEventArgs e)
        {
            //クリックされた位置のセルを取得します
            var hitTestInfo = this.HitTest(e.X, e.Y);
            var col = hitTestInfo.ColumnIndex;
            var row = hitTestInfo.RowIndex;
            if (col == -1 || row == -1) {
                //既存のマウスダウンの処理を行います
                base.OnMouseDown(e);
                return; //セル以外がクリックされた場合は処理しない
            }
            if (row == 0) {
                //既存のマウスダウンの処理を行います
                base.OnMouseDown(e);
                return; //セル以外がクリックされた場合は処理しない
            }

            //選択中のセルをクリックした場合は、何も処理しない
            if (this.SelectedCells.Contains(this[col, row])) {
                return;
            }

            //既存のマウスダウンの処理を行います
            base.OnMouseDown(e);
        }

    }
}
