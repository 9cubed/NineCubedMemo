using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.Menus.EditMenu
{
    public class EditMenuPlugin : ToolStripMenuItem, IPlugin
    {
        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //メニューを作成します
            this.Text = "編集";
            {
                var menu = new ToolStripMenuItem("元に戻す");
                menu.Click += (sender, e) => {
                    if (_pluginManager.ActivePlugin is IEditPlugin plugin) plugin.Undo();
                };
                this.DropDownItems.Add(menu);
            }
            {
                var menu = new ToolStripMenuItem("やり直す");
                menu.Click += (sender, e) => {
                    if (_pluginManager.ActivePlugin is IEditPlugin plugin) plugin.Redo();
                };
                this.DropDownItems.Add(menu);
            }
            {
                var menu = new ToolStripMenuItem("切り取り");
                menu.Click += (sender, e) => {
                    if (_pluginManager.ActivePlugin is IEditPlugin plugin) plugin.Cut();
                };
                this.DropDownItems.Add(menu);
            }
            {
                var menu = new ToolStripMenuItem("コピー");
                menu.Click += (sender, e) => {
                    if (_pluginManager.ActivePlugin is IEditPlugin plugin) plugin.Copy();
                };
                this.DropDownItems.Add(menu);
            }
            {
                var menu = new ToolStripMenuItem("貼り付け");
                //menu.ShortcutKeys = Keys.Control | Keys.V;
                menu.Click += (sender, e) => {
                    if (_pluginManager.ActivePlugin is IEditPlugin plugin) plugin.Paste();
                };
                this.DropDownItems.Add(menu);
            }
            {
                var menu = new ToolStripMenuItem("削除");
                menu.Click += (sender, e) => {
                    if (_pluginManager.ActivePlugin is IEditPlugin plugin) plugin.Delete();
                };
                this.DropDownItems.Add(menu);
            }

            return true;
        }
        
        private PluginManager _pluginManager = null;                      //プラグインマネージャー
        public void       InitializePlaced() {}                            //プラグイン配置後の初期化処理を行います
        public string     PluginId           { get; set; }                 //プラグインID
        public IPlugin    ParentPlugin       { get; set; }                 //親プラグイン
        public IComponent GetComponent()     { return this; }              //プラグインのコンポーネントを返します
        public string     Title              { get; set; }                 //プラグインのタイトル
        public bool       CanClosePlugin()   { return true; }              //プラグインが終了できるかどうか
        public void       ClosePlugin()      { Parent = null; Dispose(); } //プラグインの終了処理
        public void       SetFocus()         {  }                          //フォーカスを設定します

    } //class
}
