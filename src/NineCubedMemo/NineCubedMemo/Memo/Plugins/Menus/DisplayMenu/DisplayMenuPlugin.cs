using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//NineCubed.Memo.Plugins.Menus.DisplayMenu.DisplayMenuPlugin
namespace NineCubed.Memo.Plugins.Menus.DisplayMenu
{
    public class DisplayMenuPlugin : ToolStripMenuItem, IPlugin
    {
        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //メニューを作成します
            this.Text = "表示";
            {
                var menu = new ToolStripMenuItem("最新の情報に更新");
                menu.ShortcutKeys = Keys.F5;
                menu.Click += (sender, e) => {
                    if (_pluginManager.ActivePlugin is IRefreshPlugin plugin) plugin.RefreshData();
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
