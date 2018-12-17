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
using NineCubed.Common.Files;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Events;
using System.IO;

namespace NineCubed.Memo.Plugins.PathField
{
    public partial class PathFieldPlugin : UserControl, IPlugin
    {
        public PathFieldPlugin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// プロパティファイル
        /// </summary>
        //private IniFile _property = new IniFile();

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

            //イベントハンドラーを登録します
            _pluginManager.GetEventManager().AddEventHandler(DirSelectedEventParam.Name, this);

            return true;
        }

        //プラグイン配置後の初期化処理を行います
        public void InitializePlaced() {
            this.BringToFront();
        }

        private PluginManager _pluginManager = null;                    //プラグインマネージャー
        public string    PluginId         { get; set; }                 //プラグインID
        public Component GetComponent()   { return this; }              //プラグインのコンポーネントを返します
        public string    Title            { get; set; }                 //プラグインのタイトル
        public bool      CanClosePlugin() { return true; }              //プラグインが終了できるかどうか
        public void      ClosePlugin()    { Parent = null; Dispose();}  //プラグインの終了処理

        //フォーカスを設定します
        public void SetFocus() {
            txtPath.Focus();

            //アクティブプラグインにします
            _pluginManager.ActivePlugin = this;
        }                          

        private void txtPath_KeyDown(object sender, KeyEventArgs e)
        {
            var path = txtPath.Text;

            if (e.KeyCode == Keys.Enter) {
                if (string.IsNullOrEmpty(path)) return; //パスが未指定の場合は処理を抜けます

                if (FileUtils.IsFile(path)) {
                    //ファイルの場合、ファイル選択イベントを発生させます
                    var param = new FileSelectedEventParam { Path = path };
                    _pluginManager.GetEventManager().RaiseEvent(FileSelectedEventParam.Name,  null, param);
                } else {
                    if (Directory.Exists(path) == false) return; //フォルダが存在しない場合は処理を抜けます

                    //フォルダの場合、フォルダ選択イベントを発生させます
                    var param = new DirSelectedEventParam { Path = path };
                    _pluginManager.GetEventManager().RaiseEvent(DirSelectedEventParam.Name,  null, param);
                }
            }
        }

        /// <summary>
        /// 親フォルダへの移動ボタンのクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoveToParentDir_Click(object sender, EventArgs e)
        {
            //パス入力欄のパスを取得します
            var path = txtPath.Text.Trim();

            //パスが空の場合は処理を抜けます
            if (string.IsNullOrEmpty(path)) return;

            //親フォルダのパスを取得します
            var parentDirPath = Directory.GetParent(path)?.FullName;
            if (string.IsNullOrEmpty(parentDirPath) == false) {

                //末尾がコロン(:)の場合は、セパレーターを付けます
                if (parentDirPath.EndsWith(":")) parentDirPath = parentDirPath + Path.DirectorySeparatorChar;

                //パス欄のパスを設定します
                txtPath.Text = parentDirPath;

                //フォルダの場合、フォルダ選択イベントを発生させます
                var param = new DirSelectedEventParam { Path = path };
                _pluginManager.GetEventManager().RaiseEvent(DirSelectedEventParam.Name, this, param);
            }
        }

        /// <summary>
        /// アクティブになった時に発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPath_Enter(object sender, EventArgs e)
        {
            _pluginManager.ActivePlugin = this;
        }


        /******************************************************************************
         * 
         *  プラグイン用イベントハンドラー
         * 
         ******************************************************************************/

        public void PluginEvent_DirSelected(EventParam param, object sender)
        {
            //イベントの発生元が自分の場合は処理を抜けます
            if (sender == this) return;

            //パス欄に選択されたフォルダをパスを設定します
            var path = (param as DirSelectedEventParam).Path;

            //末尾がコロン(:)の場合は、セパレーターを付けます
            if (path.EndsWith(":")) path = path + Path.DirectorySeparatorChar;

            txtPath.Text = path;
        }

    } //class
        
}
