using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.Menus.HelpMenu
{
    public class HelpMenuPlugin : ToolStripMenuItem, IPlugin
    {
        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //メニューを作成します
            this.Text = "ヘルプ";
            {
                var menu = new ToolStripMenuItem("バージョン情報");
                menu.Click += (sender, e) => {
                    //バージョン情報を取得します
                    var fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
                    string version =
                        fileVersionInfo.ProductName    + " "  + //製品名
                        fileVersionInfo.ProductVersion + "\n" + //製品バージョン
                        fileVersionInfo.LegalCopyright;         //コピーライト

                    //バージョン情報を表示します
                    MessageBox.Show(version);
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
