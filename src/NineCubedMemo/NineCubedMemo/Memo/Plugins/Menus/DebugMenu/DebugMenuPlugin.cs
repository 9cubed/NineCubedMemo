using NineCubed.Memo.Plugins.Interfaces;
using NineCubed.Memo.Plugins.Test;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.Menus.DebugMenu
{
    public class DebugMenuPlugin : ToolStripMenuItem, IPlugin
    {
        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //メニューを作成します
            this.Text = "デバッグ";
            {
                var menu = new ToolStripMenuItem("テストプラグインの生成");
                menu.Click += (sender, e) => {
                    //プラグインを生成します
                    var plugin = (IPlugin)_pluginManager.CreatePluginInstance(typeof(TestPlugin), null, this);
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
