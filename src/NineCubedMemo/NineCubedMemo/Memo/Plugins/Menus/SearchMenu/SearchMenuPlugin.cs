using NineCubed.Memo.Plugins.Interfaces;
using NineCubed.Memo.Plugins.SearchForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.Menus.SearchMenu
{
    public class SearchMenuPlugin : ToolStripMenuItem, IPlugin
    {
        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //メニューを作成します
            this.Text = "検索";
            {
                var menu = new ToolStripMenuItem("検索");
                menu.ShortcutKeys = Keys.Control | Keys.F;
                menu.Click += (sender, e) => {
                    if (_pluginManager.ActivePlugin is ISearchPlugin plugin) {
                        //プラグイン共通データから、検索データを取得します
                        var searchData = (SearchData)_pluginManager.CommonData[CommonDataKeys.SearchData];
                        var searchString = searchData.SearchString;

                        //テキストが選択されている場合は、それを検索文字列とします
                        if (plugin.GetSearchString().Length > 0) {
                            searchString = plugin.GetSearchString();
                        }

                        //検索文字列を設定します
                        searchData.SearchString = searchString;

                        //検索画面プラグインを取得します
                        var searchFormPlugin = (ISearchForm)_pluginManager.GetPlugin("search_form");

                        //検索画面を表示します
                        searchFormPlugin.ShowForm();
                    }
                };
                this.DropDownItems.Add(menu);
            }
            {
                var menu = new ToolStripMenuItem("前方検索");
                menu.ShortcutKeys = Keys.F3;
                menu.Click += (sender, e) => {
                    if (_pluginManager.ActivePlugin is ISearchPlugin plugin) plugin.SearchForward();
                };
                this.DropDownItems.Add(menu);
            }
            {
                var menu = new ToolStripMenuItem("後方検索");
                menu.ShortcutKeys = Keys.Shift | Keys.F3;
                menu.Click += (sender, e) => {
                    if (_pluginManager.ActivePlugin is ISearchPlugin plugin) plugin.SearchBackward();
                };
                this.DropDownItems.Add(menu);
            }
            {
                var menu = new ToolStripMenuItem("前方置換");
                menu.ShortcutKeys = Keys.Control | Keys.F3;
                menu.Click += (sender, e) => {
                    if (_pluginManager.ActivePlugin is ISearchPlugin plugin) plugin.ReplaceForward();
                };
                this.DropDownItems.Add(menu);
            }
            {
                var menu = new ToolStripMenuItem("後方置換");
                menu.ShortcutKeys = Keys.Control | Keys.Shift | Keys.F3;
                menu.Click += (sender, e) => {
                    if (_pluginManager.ActivePlugin is ISearchPlugin plugin) plugin.ReplaceBackward();
                };
                this.DropDownItems.Add(menu);
            }
            {
                var menu = new ToolStripMenuItem("全て置換");
                menu.Click += (sender, e) => {
                    if (_pluginManager.ActivePlugin is ISearchPlugin plugin) plugin.ReplaceAll();
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
