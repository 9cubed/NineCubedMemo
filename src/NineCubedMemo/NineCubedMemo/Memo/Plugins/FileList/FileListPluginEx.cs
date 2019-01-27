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
using System.Diagnostics;
using System.IO;
using NineCubed.Common.Controls.FileList;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Common.Utils;
using System.Drawing.Imaging;
using NineCubed.Memo.Plugins.FileList.Menus;
using NineCubed.Common.Controls.FileList.Columns;

namespace NineCubed.Memo.Plugins.FileList
{
    public partial class FileListPluginEx : UserControl, IPlugin, IEditPlugin, IRefreshPlugin, IPathPlugin
    {
        public FileListPluginEx()
        {
            InitializeComponent();

            fileListGrid.Dock = DockStyle.Fill;
        }

        //ポップアップメニューを設定します
        private void SetPopupMenuItem()
        {
            //最新の情報に更新
            this.ContextMenuStrip = new ContextMenuStrip();
            {
                var menu = new ToolStripMenuItem("最新の情報に更新");
                menu.Click += (sender, e) => fileListGrid.ShowFileList();
                this.ContextMenuStrip.Items.Add(menu);
            }

            //新規作成
            this.ContextMenuStrip.Items.Add( new CreateDirMenu (fileListGrid) ); //フォルダ
            this.ContextMenuStrip.Items.Add( new CreateFileMenu(fileListGrid) ); //ファイル
            this.ContextMenuStrip.Items.Add( new OpenFileMenu  (fileListGrid) ); //開く

            //開く（文字コード指定）
            {
                var menu = new ToolStripMenuItem { Text= "開く（文字コード指定）" };
                menu.DropDownItems.Add(new OpenFileEncodingMenu(fileListGrid, "Shift JIS"      , Encoding.GetEncoding(932)));
                menu.DropDownItems.Add(new OpenFileEncodingMenu(fileListGrid, "UTF-8"          , new UTF8Encoding(false)));
                menu.DropDownItems.Add(new OpenFileEncodingMenu(fileListGrid, "UTF-8 BOM"      , new UTF8Encoding(true)));
                menu.DropDownItems.Add(new OpenFileEncodingMenu(fileListGrid, "UTF-16 BOM (LE)", Encoding.GetEncoding(1200)));
                menu.DropDownItems.Add(new OpenFileEncodingMenu(fileListGrid, "UTF-16 BOM (BE)", Encoding.GetEncoding(1201)));
                menu.DropDownItems.Add(new OpenFileEncodingMenu(fileListGrid, "EUC-JP"         , Encoding.GetEncoding(51932)));
                this.ContextMenuStrip.Items.Add(menu);
            }

            //開く（バイナリ形式）
            this.ContextMenuStrip.Items.Add( new OpenFileBinaryMenu(fileListGrid) ); //開く（バイナリ形式）

            //ネイティブな方法で開く
            {
                var menu = new ToolStripMenuItem("ネイティブな方法で開く");
                menu.Click += (sender, e) => {
                    //選択されているセルがある行のパスを取得します
                    var path = GetSelectedPath();
                    if (path == null) return;

                    //ネイティブな方法でファイルを開きます
                    Process.Start(path);
                };
                this.ContextMenuStrip.Items.Add(menu);
            }

            //現在のフォルダを開く
            {
                var menu = new ToolStripMenuItem("現在のフォルダを開く");
                menu.Click += (sender, e) => {
                    //選択されているセルがある行のパスを取得します
                    if (string.IsNullOrEmpty(fileListGrid.CurrentPath)) return;

                    //ネイティブな方法でフォルダを開きます
                    Process.Start(fileListGrid.CurrentPath);
                };
                this.ContextMenuStrip.Items.Add(menu);
            }

            //パスのコピー
            {
                var menu = new ToolStripMenuItem("パスのコピー");
                menu.Click += (sender, e) => {
                    var data = new Dictionary<int, string>();
                    foreach (DataGridViewCell cell in fileListGrid.SelectedCells) {
                        data[cell.RowIndex] = fileListGrid[0, cell.RowIndex].Value.ToString();
                    }

                    var text = "";
                    foreach (var keyValue in data) {
                        text += keyValue.Value + "\n";
                    }
                    text = StringUtils.RemoveRight(text, 1); //最後の改行を削除

                    if (StringUtils.IsNotEmpty(text)) {
                        //クリップボードに文字列を設定します
                        Clipboard.SetText(text);
                    }
                };
                this.ContextMenuStrip.Items.Add(menu);
            }
            /*
            //NG:わざわざ作らなくても、標準でこの機能ついてる。。。。。。。。。。
            //選択範囲のコピー
            {
                var menu = new ToolStripMenuItem("選択範囲のコピー");
                menu.Click += (sender, e) => {
                    //選択されているセルの値をタブ区切りの文字列で取得します
                    var text = GetSelectedCellValues();
                    if (StringUtils.IsNotEmpty(text)) {
                        //クリップボードに文字列を設定します
                        Clipboard.SetText(text);
                    }
                };
                this.ContextMenuStrip.Items.Add(menu);
            }*/
        }
        /*
        /// <summary>
        /// 選択されているセルの値を、タブ区切りの文字列で返します。
        /// 行は改行コードで区切ります。
        /// </summary>
        /// <returns></returns>
        private string GetSelectedCellValues()
        {
            //選択範囲の上下左右の端のindexを取得します
            int minCol = fileListGrid.ColumnCount; //選択範囲の左端
            int minRow = fileListGrid.RowCount;    //選択範囲の上端
            int maxCol = 0;                        //選択範囲の右端
            int maxRow = 0;                        //選択範囲の下端
            foreach (DataGridViewCell cell in fileListGrid.SelectedCells) {
                if (cell.ColumnIndex < minCol) minCol = cell.ColumnIndex;
                if (cell.RowIndex    < minRow) minRow = cell.RowIndex;
                if (cell.ColumnIndex > maxCol) maxCol = cell.ColumnIndex;
                if (cell.RowIndex    > maxRow) maxRow = cell.RowIndex;
            }

            //選択されているセルの値だけを、連想配列にコピーします。キーは「列_行」
            var data = new Dictionary<string, string>();
            foreach (DataGridViewCell cell in fileListGrid.SelectedCells) {
                var key = cell.ColumnIndex.ToString() + "_" + cell.RowIndex.ToString();
                data[key] = cell.Value?.ToString().Trim();
            }

            //連想配列の値を結合して文字列にします
            var text = new StringBuilder();
            for (int row = minRow; row <= maxRow; row++) {
                string line = "";
                for (int col = minCol; col <= maxCol; col++) {
                    var key = col.ToString() + "_" + row.ToString();
                    if (data.TryGetValue(key, out string value)) {
                        line += value;
                    }
                    line += "\t";
                }

                //末尾がタブの場合は、削除します
                if (line.EndsWith("\t")) line = StringUtils.RemoveRight(line, 1);

                if (StringUtils.IsNotEmpty(line.Trim('\t'))){
                    //空行でない場合、戻り値用文字列に追加します
                    text.Append(line + "\n");
                }
            }
            return text.ToString();
        }*/

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
        /// ファイルリストで選択中のフォルダまたはファイルの、選択中イベントを発生させます
        /// 
        /// </summary>
        public void RaiseSelectingEvent(string path)
        {
            if (FileUtils.IsFile(path)) {
                //ファイルの場合、ファイル選択イベントを発生させます
                var param = new FileSelectingEventParam { Path = path };
                _pluginManager.GetEventManager().RaiseEvent(FileSelectingEventParam.Name, this, param);
            } else {
                //フォルダの場合、フォルダ選択イベントを発生させます
                var param = new DirSelectingEventParam { Path = path };
                _pluginManager.GetEventManager().RaiseEvent(DirSelectingEventParam.Name, this, param);
            }
        }

        //日時をファイル名にしたファイル名を返します
        private string GetTimeFileName() {
            return DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
        }

        /// <summary>
        /// 同じ階層の隣のフォルダへ移動します
        /// </summary>
        /// <param name="isMovePrevious">true:1つ前のフォルダへ移動 false:1つ後ろのフォルダへ移動</param>
        private void MoveDir(bool isMovePrevious)
        {
            //親フォルダのパスを取得します
            var parentDirPath = Path.GetDirectoryName(fileListGrid.CurrentPath);
            if (parentDirPath == null) return;

            //フォルダ一覧を取得します
            var dirList = FileUtils.GetDirList(parentDirPath);
            
            //フォルダ一覧の中の、現在の位置を取得します
            int index = dirList.IndexOf(fileListGrid.CurrentPath);

            //位置を移動します
            //インデックスの範囲を超えた場合は、処理を抜けます
            if (isMovePrevious) {
                index--;
                if (index < 0) return;
            } else {
                index++;
                if (index > dirList.Count - 1) return;
            }

            //フォルダ選択イベントを発生させます
            RaiseSelectedEvent(dirList[index]);
        }

        /// <summary>
        /// 現在選択されている行のパスを返します
        /// </summary>
        /// <returns></returns>
        private string GetSelectedPath() {
            if (fileListGrid.CurrentRow == null)   return null;
            if (fileListGrid.CurrentRow.Index < 0) return null;

            //現在のセルがある行のパスを取得します。
            return fileListGrid[0, fileListGrid.CurrentRow.Index].Value?.ToString();
        }

        /******************************************************************************
         * 
         *  IPlugin
         * 
         ******************************************************************************/
        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //コントロールの初期化をします
            fileListGrid.Initialize();
            
            //カラムの設定
            fileListGrid.Columns.Add(new PathColumn          ()); //パスカラム(非表示)
            /*
            fileListGrid.Columns.Add(new FileKindColumn      ());
            fileListGrid.Columns.Add(new FileNameColumn      ());
            fileListGrid.Columns.Add(new FileExtensionColumn ());
            fileListGrid.Columns.Add(new FileSizeColumn      ());
            fileListGrid.Columns.Add(new FileUpdateDateColumn());
            fileListGrid.Columns.Add(new ImageSizeColumn     ());

            //各カラムに FileListGrid 本体を設定します
            foreach (var colums in fileListGrid.Columns) ((IFileListColumn)colums).FileList = fileListGrid;
            */

            fileListGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;      //セル選択モード
            fileListGrid.EditMode      = DataGridViewEditMode.EditProgrammatically; //シングルクリックで編集中にならないようにした

            //ファイルリストで表示するフォルダの画像を設定します
            fileListGrid.SetImage(Image.FromFile(FileUtils.AppendPath(param.DataPath, "img/opened_folder.png")));

            //ファイル一覧の取得メソッドを設定します
            fileListGrid.GetFileList = (dirPath) => {

                //ファイルのパターンを取得します。未指定の場合は、「*」にします。
                var searchPattern = cmbPattern.Text.Trim();
                if (string.IsNullOrEmpty(searchPattern)) searchPattern = "*";

                //フォルダとファイルの一覧を取得します
                List<string> pathList = new List<string>();
                try {
                    //フォルダにチェックが入っている場合は、フォルダ一覧を取得します
                    if (chkDirVisible.Checked) {
                        var list = FileUtils.GetDirList(dirPath, false, searchPattern);
                        pathList.AddRange(list);
                        chkDirVisible.Text = "フォルダ（" + list.Count + "個）";
                    } else {
                        chkDirVisible.Text = "フォルダ";
                    }

                    //ファイルにチェックが入っている場合は、フォルダ一覧を取得します
                    if (chkFileVisible.Checked == true) {
                        var list = FileUtils.GetFileList(dirPath, false, searchPattern);
                        pathList.AddRange(list);
                        chkFileVisible.Text = "ファイル（" + list.Count + "個）";
                    } else {
                        chkFileVisible.Text = "ファイル";
                    }
                    
                } catch (Exception) {
                    //アクセス権限がない場合
                    return new List<string>();
                }
                return pathList ;
            };

            //ポップアップメニューを設定します
            SetPopupMenuItem();

            //イベントハンドラーを登録します
            _pluginManager.GetEventManager().AddEventHandler(  DirSelectedEventParam.Name, this);
            _pluginManager.GetEventManager().AddEventHandler(PluginCreatedEventParam.Name, this);

            return true;
        }
        //プラグイン配置後の初期化処理を行います
        public void InitializePlaced() {
            this.Dock = DockStyle.Fill;
            this.BringToFront();
        }
        private PluginManager _pluginManager = null;                     //プラグインマネージャー
        public string     PluginId         { get; set; }                 //プラグインID
        public IPlugin    ParentPlugin     { get; set; }                 //親プラグイン
        public IComponent GetComponent()   { return this; }              //プラグインのコンポーネントを返します
        public string     Title            { get { return "ファイル一覧";} set{} } //プラグインのタイトル
        public bool       CanClosePlugin() { return true; }              //プラグインが終了できるかどうか
        public void       ClosePlugin()    { Parent = null; Dispose(); } //プラグインの終了処理

        //フォーカスを設定します
        public void SetFocus() {
            this.Focus();

            //アクティブプラグインにします
            _pluginManager.ActivePlugin = this;
        }

        /******************************************************************************
         * 
         *  IEditPlugin
         * 
         ******************************************************************************/ 
        public void Cut()    {  }
        public void Copy()   {  }
        public void Paste()
        {
            if (Clipboard.ContainsImage()) {
                //クリップボードに画像形式のデータが入っている場合
                var img = Clipboard.GetImage();
                string path = FileUtils.AppendPath(fileListGrid.CurrentPath, GetTimeFileName() + ".png");

                //画像ファイルを出力します
                img.Save(path, ImageFormat.Png);

                //ファイル一覧を更新します
                fileListGrid.ShowFileList();

                __.ShowInfoMsgBox("画像を保存しました。\n" + path);
                return;
            }

            if (Clipboard.ContainsText()) {
                //クリップボードにテキスト形式のデータが入っている場合
                var text = Clipboard.GetText();
                string path = FileUtils.AppendPath(fileListGrid.CurrentPath, GetTimeFileName() + ".txt");
                
                //テキストファイルを出力します
                using(var writer = new StreamWriter(path, false, Encoding.GetEncoding(932))) {
                    writer.Write(text);
                }

                //ファイル一覧を更新します
                fileListGrid.ShowFileList();
                __.ShowInfoMsgBox("テキストを保存しました。\n" + path);
                return;
            }
        }
        public void Delete() {  }
        public void Undo()   {  }
        public void Redo()   {  }

        /******************************************************************************
         * 
         *  IRefreshPlugin
         * 
         ******************************************************************************/ 

        /// <summary>
        /// 最新の情報に更新します
        /// </summary>
        public void RefreshData()
        {
            //ファイルリストを更新します
            fileListGrid.ShowFileList();
        }

        /******************************************************************************
         * 
         *  IPathPlugin
         * 
         ******************************************************************************/

        /// <summary>
        /// パスを返します
        /// </summary>
        /// <returns></returns>
        public string GetPath() => GetSelectedPath();

        /******************************************************************************
         * 
         *  グリッドのイベント
         * 
         ******************************************************************************/

        /// <summary>
        /// グリッドのダブルクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileListGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return; //ヘッダーダブルクリックは無視する

            //ダブルクリックされた行のパスを取得します。
            var path = fileListGrid[0, e.RowIndex].Value.ToString();

            //フォルダまたはファイル選択イベントを発生させます
            RaiseSelectedEvent(path);
        }

        /// <summary>
        /// グリッドの選択変更イベント
        /// カーソルが移動した時に発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileListGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (fileListGrid.CurrentCell          == null) return; //カレントセルが未設定の場合は処理しない
            if (fileListGrid.CurrentCell.RowIndex == -1)   return; //ヘッダーダブルクリックは無視する

            //ダブルクリックされた行のパスを取得します
            var path = GetSelectedPath();
            if (path == null) return;

            //フォルダまたはファイル選択中イベントを発生させます
            RaiseSelectingEvent(path);
        }

        /// <summary>
        /// グリッドのキーダウンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileListGrid_KeyDown(object sender, KeyEventArgs e)
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

            //F2が押された場合は、編集中にします
            if (e.KeyCode == Keys.F2) {
                //選択されているセルがある行のパスを取得します
                var path = GetSelectedPath();
                if (path == null) return;

                //編集中にします
                fileListGrid.BeginEdit(false);

                //キー操作を処理済みにします
                e.Handled = true;
                return;
            }

            //ALT + 左キー の場合は、上の階層のフォルダ選択イベントを発生させます
            if (e.Alt && e.KeyCode == Keys.Left) {
                if (string.IsNullOrEmpty(fileListGrid.CurrentPath)) return;
                var dir = new DirectoryInfo(fileListGrid.CurrentPath);
                if (dir.Parent != null) {
                    //フォルダまたはファイル選択イベントを発生させます
                    RaiseSelectedEvent(dir.Parent.FullName);
                }

                //キー操作を処理済みにします
                e.Handled = true;
                return;
            }

            //ALT + 上キー の場合は、同じ階層の次のフォルダのフォルダ選択イベントを発生させます
            if (e.Alt && e.KeyCode == Keys.Up) {
                //同じ階層の前のフォルダへ移動します
                MoveDir(true);

                //キー操作を処理済みにします
                e.Handled = true;
                return;
            }

            //ALT + 下キー の場合は、同じ階層の次のフォルダのフォルダ選択イベントを発生させます
            if (e.Alt && e.KeyCode == Keys.Down) {
                //同じ階層の次のフォルダへ移動します
                MoveDir(false);

                //キー操作を処理済みにします
                e.Handled = true;
                return;
            }

            //Ctrl + V の場合は、貼り付けをします
            if (e.Control && e.KeyCode == Keys.V) {
                Paste();

                //キー操作を処理済みにします
                e.Handled = true;
                return;
            }
        }

        /// <summary>
        /// グリッドのキープレスイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileListGrid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (fileListGrid.CurrentCell == null) return;
            if (fileListGrid.CurrentCell.ColumnIndex == -1) return;
            
            //編集可能なセルの場合は、編集モードにします
            if (fileListGrid.Columns[fileListGrid.CurrentCell.ColumnIndex].ReadOnly == false) {
                //編集中にします
                fileListGrid.BeginEdit(true);

                //入力されたキーを送信します
                SendKeys.Send(e.KeyChar.ToString());
            }
        }

        /// <summary>
        /// グリッドの手入力後に発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileListGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var path     = fileListGrid[0,             e.RowIndex].Value.ToString(); //パス
            var newValue = fileListGrid[e.ColumnIndex, e.RowIndex].Value?.ToString(); //入力値
            if (newValue == null) newValue = "";

            var oldFile = new FileInfo(path);

            //カラムオブジェクトに値の変更を通知します(カラム側ではファイル名の変更などを行う)
            var newFile = ((IFileListColumn)fileListGrid.Columns[e.ColumnIndex]).ValueChanged(oldFile, newValue);
            if (newFile == null) newFile = oldFile;
            
            //行の更新します
            fileListGrid.SetRowData(e.RowIndex, newFile.FullName);
        }

        /// <summary>
        /// アクティブになった時に発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileListGrid_Enter(object sender, EventArgs e)
        {
            //アクティブプラグインにします
            _pluginManager.ActivePlugin = this;
        }

        //フォルダやファイルのチェックが変更されたり、表示ボタンがクリックされたら、
        //ファイルリストを更新します。
        private void btnDisplay_Click             (object sender, EventArgs e) => RefreshData();
        private void chkDirVisible_CheckedChanged (object sender, EventArgs e) => RefreshData();
        private void chkFileVisible_CheckedChanged(object sender, EventArgs e) => RefreshData();

        /// <summary>
        /// ファイルパターンのキーダウンイベント
        /// 
        /// エンターキーが押されたら、ファイルリストを更新します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPattern_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                RefreshData();

                //コンボボックスのリストに、パターンが追加されていない場合は、パターンを追加します
                if (cmbPattern.Items.IndexOf(cmbPattern.Text) == -1) {
                    cmbPattern.Items.Add(cmbPattern.Text);
                }
            }
        }

        /// <summary>
        /// ファイルパターンの選択イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPattern_SelectedValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        /******************************************************************************
         * 
         *  プラグイン用イベントハンドラー
         * 
         ******************************************************************************/

        /// <summary>
        /// フォルダ選択イベント
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sender"></param>
        public void PluginEvent_DirSelected(EventParam param, object sender)
        {
            //ファイル一覧を表示します
            var path = (param as DirSelectedEventParam).Path;
            fileListGrid.ShowFileList(path);
        }

        /// <summary>
        /// プラグイン生成イベント
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sender"></param>
        public void PluginEvent_PluginCreated(EventParam param, object sender) {

            //生成されたプラグインを取得します
            var plugin = ((PluginCreatedEventParam)param).Plugin;

            //生成されたプラグインのコンポーネントを取得します
            var component = plugin.GetComponent();

            //生成されたプラグインのコンポーネントがカラムの場合
            if (component is DataGridViewColumn column) {
                //カラムを追加します
                fileListGrid.Columns.Add(column);

                //イベントを処理済みにします
                param.Handled = true;
                return;
            }

            //生成されたプラグインのコンポーネントがメニューの場合
            if (component is ToolStripMenuItem) {
                //メニューを追加します
                this.ContextMenuStrip.Items.Add((ToolStripMenuItem)component);

                //イベントを処理済みにします
                param.Handled = true;
            }
        }

    } //class
}
