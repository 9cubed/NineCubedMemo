using NineCubed.Common.Controls.FileList;
using NineCubed.Common.Controls.FileList.Columns;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.FileList.Menus;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.FileList
{
    public class FileListPlugin : FileListGrid, IPlugin, IEditPlugin, IRefreshPlugin, IPathPlugin
    {
        public FileListPlugin() {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();

            // 
            // FileListPlugin
            // 
            this.RowTemplate.Height = 21;
            this.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.FileListPlugin_CellDoubleClick);
            this.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.FileListPlugin_CellEndEdit);
            this.SelectionChanged += new System.EventHandler(this.FileListPlugin_SelectionChanged);
            this.Enter += new System.EventHandler(this.FileListPlugin_Enter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FileListPlugin_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
        }

        //ポップアップメニューを設定します
        private void SetPopupMenuItem()
        {
            //最新の情報に更新
            this.ContextMenuStrip = new ContextMenuStrip();
            {
                var menu = new ToolStripMenuItem("最新の情報に更新");
                menu.Click += (sender, e) => ShowFileList();
                this.ContextMenuStrip.Items.Add(menu);
            }

            //新規作成
            this.ContextMenuStrip.Items.Add( new CreateDirMenu (this) ); //フォルダ
            this.ContextMenuStrip.Items.Add( new CreateFileMenu(this) ); //ファイル
            this.ContextMenuStrip.Items.Add( new OpenFileMenu  (this) ); //開く

            //開く（文字コード指定）
            {
                var menu = new ToolStripMenuItem { Text= "開く（文字コード指定）" };
                menu.DropDownItems.Add(new OpenFileEncodingMenu(this, "Shift JIS"      , Encoding.GetEncoding(932)));
                menu.DropDownItems.Add(new OpenFileEncodingMenu(this, "UTF-8"          , new UTF8Encoding(false)));
                menu.DropDownItems.Add(new OpenFileEncodingMenu(this, "UTF-8 BOM"      , new UTF8Encoding(true)));
                menu.DropDownItems.Add(new OpenFileEncodingMenu(this, "UTF-16 BOM (LE)", Encoding.GetEncoding(1200)));
                menu.DropDownItems.Add(new OpenFileEncodingMenu(this, "UTF-16 BOM (BE)", Encoding.GetEncoding(1201)));
                menu.DropDownItems.Add(new OpenFileEncodingMenu(this, "EUC-JP"         , Encoding.GetEncoding(51932)));
                this.ContextMenuStrip.Items.Add(menu);
            }

            //開く（バイナリ形式）
            this.ContextMenuStrip.Items.Add( new OpenFileBinaryMenu(this) ); //開く（バイナリ形式）

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
                    if (string.IsNullOrEmpty(this.CurrentPath)) return;

                    //ネイティブな方法でフォルダを開きます
                    Process.Start(this.CurrentPath);
                };
                this.ContextMenuStrip.Items.Add(menu);
            }
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
            var parentDirPath = Path.GetDirectoryName(this.CurrentPath);
            if (parentDirPath == null) return;

            //フォルダ一覧を取得します
            var dirList = FileUtils.GetDirList(parentDirPath);
            
            //フォルダ一覧の中の、現在の位置を取得します
            int index = dirList.IndexOf(this.CurrentPath);

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
            if (this.CurrentRow == null)   return null;
            if (this.CurrentRow.Index < 0) return null;

            //現在のセルがある行のパスを取得します。
            return this[0, this.CurrentRow.Index].Value?.ToString();
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
            this.Initialize();

            this.SelectionMode = DataGridViewSelectionMode.CellSelect;      //セル選択モード
            this.EditMode      = DataGridViewEditMode.EditProgrammatically; //シングルクリックで編集中にならないようにした

            //ファイルリストで表示するフォルダの画像を設定します
            this.SetImage(Image.FromFile(FileUtils.AppendPath(param.DataPath, "img/opened_folder.png")));

            //ポップアップメニューを設定します
            SetPopupMenuItem();

            //カラムの設定(FileListPluginExクラスでは、動的にカラムを設定するようになっています)
            this.Columns.Add(new PathColumn          ()); //パスカラム(非表示)
            //this.Columns.Add(new FileKindColumn      ());
            this.Columns.Add(new FileNameColumn      ());
            this.Columns.Add(new FileExtensionColumn ());
            this.Columns.Add(new FileSizeColumn      ());
            this.Columns.Add(new FileUpdateDateColumn());

            //イベントハンドラーを登録します
            _pluginManager.GetEventManager().AddEventHandler(DirSelectedEventParam.Name, this);

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
                string path = FileUtils.AppendPath(CurrentPath, GetTimeFileName() + ".png");

                //画像ファイルを出力します
                img.Save(path, ImageFormat.Png);

                //ファイル一覧を更新します
                ShowFileList();

                __.ShowInfoMsgBox("画像を保存しました。\n" + path);
                return;
            }

            if (Clipboard.ContainsText()) {
                //クリップボードにテキスト形式のデータが入っている場合
                var text = Clipboard.GetText();
                string path = FileUtils.AppendPath(CurrentPath, GetTimeFileName() + ".txt");
                
                //テキストファイルを出力します
                using(var writer = new StreamWriter(path, false, Encoding.GetEncoding(932))) {
                    writer.Write(text);
                }

                //ファイル一覧を更新します
                ShowFileList();
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
            this.ShowFileList();
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
        private void FileListPlugin_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return; //ヘッダーダブルクリックは無視する

            //ダブルクリックされた行のパスを取得します。
            var path = this[0, e.RowIndex].Value.ToString();

            //フォルダまたはファイル選択イベントを発生させます
            RaiseSelectedEvent(path);
        }

        /// <summary>
        /// グリッドの選択変更イベント
        /// カーソルが移動した時に発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileListPlugin_SelectionChanged(object sender, EventArgs e)
        {
            if (this.CurrentCell          == null) return; //カレントセルが未設定の場合は処理しない
            if (this.CurrentCell.RowIndex == -1)   return; //ヘッダーダブルクリックは無視する

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
        private void FileListPlugin_KeyDown(object sender, KeyEventArgs e)
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
                this.BeginEdit(false);

                //キー操作を処理済みにします
                e.Handled = true;
                return;
            }

            //ALT + 左キー の場合は、上の階層のフォルダ選択イベントを発生させます
            if (e.Alt && e.KeyCode == Keys.Left) {
                if (string.IsNullOrEmpty(this.CurrentPath)) return;
                var dir = new DirectoryInfo(this.CurrentPath);
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
            }
        }

        /// <summary>
        /// グリッドの手入力後に発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileListPlugin_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var path     = this[0,             e.RowIndex].Value.ToString(); //パス
            var newValue = this[e.ColumnIndex, e.RowIndex].Value?.ToString(); //入力値
            if (newValue == null) newValue = "";

            var oldFile = new FileInfo(path);

            //カラムオブジェクトに値の変更を通知します(カラム側ではファイル名の変更などを行う)
            var newFile = ((IFileListColumn)this.Columns[e.ColumnIndex]).ValueChanged(new FileInfo(path), newValue);
            if (newFile == null) newFile = oldFile;

            //行の更新します
            this.SetRowData(e.RowIndex, newFile.FullName);
        }

        /// <summary>
        /// アクティブになった時に発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileListPlugin_Enter(object sender, EventArgs e)
        {
            //アクティブプラグインにします
            _pluginManager.ActivePlugin = this;
        }

        /******************************************************************************
         * 
         *  プラグイン用イベントハンドラー
         * 
         ******************************************************************************/

        public void PluginEvent_DirSelected(EventParam param, object sender)
        {
            //ファイル一覧を表示します
            var path = (param as DirSelectedEventParam).Path;
            this.ShowFileList(path);
        }

        /******************************************************************************
         * 
         *  プラグイン用イベントハンドラー
         * 
         ******************************************************************************/

        /// <summary>
        /// プラグイン生成イベント
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sender"></param>
        public void PluginEvent_PluginCreated(EventParam param, object sender)
        {
            //生成されたプラグインを取得します
            var plugin = ((PluginCreatedEventParam)param).Plugin;

            //生成されたプラグインのコントロールがカラムの場合は、カラムを追加します
            var component = plugin.GetComponent();
            if (component is ToolStripMenuItem) {
                //コントロールがメニューだった場合
                this.ContextMenuStrip.Items.Add((ToolStripMenuItem)component);

                //イベントを処理済みにします
                param.Handled = true;
            }
        }



    } //class
}
