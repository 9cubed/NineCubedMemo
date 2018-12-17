using NineCubed.Common.Files;
using NineCubed.Memo.Exceptions;
using NineCubed.Memo.Plugins.Dialogs.OpenDialog;
using NineCubed.Memo.Plugins.Dialogs.SaveDialog;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;
using NineCubed.Memo.Plugins.TextEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.Menus.FileMenu
{
    public class FileMenuPlugin : ToolStripMenuItem, IPlugin
    {
        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //メニューを作成します
            this.Text = "ファイル";

            //開くメニュー
            this.DropDownItems.Add(new OpenFileMenu(_pluginManager) );

            //開く（文字コード指定）メニュー
            this.DropDownItems.Add(new OpenFileWithEncordingMenu(_pluginManager));

            //開く（バイナリ形式）メニュー
            this.DropDownItems.Add(new OpenFileInBinaryMenu(_pluginManager));

            //上書き保存メニュー
            this.DropDownItems.Add(new SaveFileMenu(_pluginManager, true));

            //名前を付けて保存メニュー
            this.DropDownItems.Add(new SaveFileMenu(_pluginManager, false));

            //閉じるメニュー
            {
                var menu = new ToolStripMenuItem("閉じる");
                menu.ShortcutKeys = Keys.Control | Keys.Q;
                menu.Click += (sender, e) => {
                    if (_pluginManager.ActivePlugin != null) {
                        //アクティブプラグインを終了します
                        _pluginManager.ClosePlugin(_pluginManager.ActivePlugin);
                    }
                };
                this.DropDownItems.Add(menu);
            }

            //終了メニュー
            {
                var menu = new ToolStripMenuItem("終了");
                menu.Click += (sender, e) => {
                    _pluginManager.MainForm.Dispose();
                };
                this.DropDownItems.Add(menu);
            }

            return true;
        }
        
        private PluginManager _pluginManager = null;                      //プラグインマネージャー
        public void      InitializePlaced() {}                            //プラグイン配置後の初期化処理を行います
        public string    PluginId           { get; set; }                 //プラグインID
        public Component GetComponent()     { return this; }              //プラグインのコンポーネントを返します
        public string    Title              { get; set; }                 //プラグインのタイトル
        public bool      CanClosePlugin()   { return true; }              //プラグインが終了できるかどうか
        public void      ClosePlugin()      { Parent = null; Dispose(); } //プラグインの終了処理
        public void      SetFocus()         {  }                          //フォーカスを設定します


    } //class
}
