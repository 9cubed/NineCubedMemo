using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.Menus.MacroMenu
{
    public class MacroMenuPlugin : ToolStripMenuItem, IPlugin
    {
        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //メニューを作成します
            this.Text = "マクロ";
            {
                var menu = new ToolStripMenuItem("キー操作の記録開始・終了");
                menu.ShortcutKeys = Keys.F9;
                menu.Click += (sender, e) => {
                    if (_pluginManager.ActivePlugin is IKeyMacroPlugin plugin) {
                        if (plugin.IsRecording() == false) {
                            //キー操作の記録を開始します
                            plugin.StartRecording();
                        } else {
                            //キー操作の記録を停止します
                            plugin.StopRecording();
                        }
                    }
                };
                this.DropDownItems.Add(menu);
            }
            {
                var menu = new ToolStripMenuItem("キー操作の再生");
                menu.ShortcutKeys = Keys.F10;
                menu.Click += (sender, e) => {
                    if (_pluginManager.ActivePlugin is IKeyMacroPlugin plugin) plugin.Play();
                };
                this.DropDownItems.Add(menu);
            }
            {
                var menu = new ToolStripMenuItem("キー操作の出力");
                menu.Click += (sender, e) => {
                    if (_pluginManager.ActivePlugin is IKeyMacroPlugin plugin) plugin.OutputMacro();
                };
                this.DropDownItems.Add(menu);
            }
            {
                var menu = new ToolStripMenuItem("キー操作の登録");
                menu.Click += (sender, e) => {
                    if (_pluginManager.ActivePlugin is IKeyMacroPlugin plugin) plugin.SetMacro();
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
