using NineCubed.Common.Controls.FileList;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.FileList
{
    public class FileListPlugin : FileListGrid, IPlugin
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
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FileListPlugin_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

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

            //ファイルリストで表示するフォルダの画像を設定します
            this.SetImage(Image.FromFile(FileUtils.AppendPath(param.DataPath, "img/opened_folder.png")));

            //ポップアップメニューを設定します
            var popupMenu = new ContextMenuStrip();
            {
                var menu = new ToolStripMenuItem("最新の情報に更新");
                popupMenu.Items.Add(menu); 
                menu.Click += (sender, e) => {
                    ShowFileList();
                };
            }
            this.ContextMenuStrip = popupMenu;

            //イベントハンドラーを登録します
            _pluginManager.GetEventManager().AddEventHandler(DirSelectedEventParam.Name, this);

            return true;
        }
        //プラグイン配置後の初期化処理を行います
        public void InitializePlaced() {
            this.Dock = DockStyle.Fill;
            this.BringToFront();
        } 
        private PluginManager _pluginManager = null;                    //プラグインマネージャー
        public string    PluginId         { get; set; }                 //プラグインID
        public Component GetComponent()   { return this; }              //プラグインのコンポーネントを返します
        public string    Title            { get; set; }                 //プラグインのタイトル
        public bool      CanClosePlugin() { return true; }              //プラグインが終了できるかどうか
        public void      ClosePlugin()    { Parent = null; Dispose(); } //プラグインの終了処理
        public void      SetFocus()       { this.SetFocus(); }          //フォーカスを設定します

        /// <summary>
        /// ファイルリストで選択されたフォルダまたはファイルの選択イベントを発生させます
        /// </summary>
        private void RaiseSelectedEvent(string path)
        {
            if (FileUtils.IsFile(path)) {
                //ファイルの場合、ファイル選択イベントを発生させます
                var param = new FileSelectedEventParam { Path = path };
                _pluginManager.GetEventManager().RaiseEvent(FileSelectedEventParam.Name,  null, param);
            } else {
                //フォルダの場合、フォルダ選択イベントを発生させます
                var param = new DirSelectedEventParam { Path = path };
                _pluginManager.GetEventManager().RaiseEvent(DirSelectedEventParam.Name,  null, param);
            }
        }

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

        private void FileListPlugin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                //エンターキーが押された場合
                //ダブルクリックされた行のパスを取得します。
                var path = this[0, this.CurrentRow.Index].Value.ToString();

                //フォルダまたはファイル選択イベントを発生させます
                RaiseSelectedEvent(path);
            }

            //ATL + 左キー の場合は、上の階層のフォルダ選択イベントを発生させます
            if (e.Alt && e.KeyCode == Keys.Left) {
                if (string.IsNullOrEmpty(this.CurrentPath)) return;
                var dir = new DirectoryInfo(this.CurrentPath);
                if (dir.Parent != null) {
                    //フォルダまたはファイル選択イベントを発生させます
                    RaiseSelectedEvent(dir.Parent.FullName);
                }
            }
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



    } //class
}
