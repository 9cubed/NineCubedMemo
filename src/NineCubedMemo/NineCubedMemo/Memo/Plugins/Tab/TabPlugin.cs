using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.Tab
{
    public class TabPlugin : TabControl, IPlugin
    {
        /// <summary>
        /// プラグインを受け取るかどうか true:受け取る
        /// </summary>
        private bool _acceptPlugin = true;

        public TabPlugin() {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TabPlugin
            // 
            this.SelectedIndexChanged += new System.EventHandler(this.TabPlugin_SelectedIndexChanged);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TabPlugin_MouseDown);
            this.ResumeLayout(false);
        }

        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            this.Dock = DockStyle.Fill;

            //ポップアップメニューを設定します
            var popupMenu = new ContextMenuStrip();
            popupMenu.Items.Add(CreateCloseMenu());    //    閉じるメニュー
            popupMenu.Items.Add(CreateAllCloseMenu()); //全て閉じるメニュー
            this.ContextMenuStrip = popupMenu;

            //起動時の引数の menu_visible が false の場合は、メニューを非表示にします
            if (param["menu_visible"] != null) {
                if (StringUtils.ToBool(param["menu_visible"].ToString()) == false) {
                    this.ContextMenuStrip = null;
                }
            }

            //動的に生成されたプラグインを受け取るかどうかのフラグを設定します
            if (param["accept_plugin"] != null) {
                _acceptPlugin = StringUtils.ToBool(param["accept_plugin"].ToString());
            }

            //イベントハンドラーを登録します
            if (_acceptPlugin) {
                _pluginManager.GetEventManager().AddEventHandler(PluginCreatedEventParam.Name, this);
            }
            _pluginManager.GetEventManager().AddEventHandler(    PluginClosedEventParam.Name, this);
            _pluginManager.GetEventManager().AddEventHandler(    TitleChangedEventParam.Name, this);
            _pluginManager.GetEventManager().AddEventHandler(AllPluginCreatedEventParam.Name, this);

            return true;
        }

        public void InitializePlaced() { } //プラグイン配置後の初期化処理を行います
        private PluginManager _pluginManager = null;                    //プラグインマネージャー
        public string     PluginId         { get; set; }                 //プラグインID
        public IPlugin    ParentPlugin     { get; set; }                 //親プラグイン
        public IComponent GetComponent()   { return this; }              //プラグインのコンポーネントを返します
        public string     Title            { get; set; }                 //プラグインのタイトル
        public bool       CanClosePlugin() { return true; }              //プラグインが終了できるかどうか
        public void       ClosePlugin()    { Parent = null; Dispose(); } //プラグインの終了処理

        /// <summary>
        /// フォーカスを設定します
        /// </summary>
        public void SetFocus() {
            if (this.SelectedTab != null) {
                //タブに割り当てられているプラグインを取得して、フォーカスを設定します
                var plugin = _pluginManager.GetPlugin( this.SelectedTab.Controls[0] );
                plugin.SetFocus();
            }
        }

        /// <summary>
        /// 指定したプラグインを保持しているタブを返します
        /// </summary>
        /// <param name="plugin"></param>
        /// <returns></returns>
        private TabPage GetTabPage(IPlugin plugin) {
            //プラグインのコンポーネントを保持しているタブを探します
            for (int i = 0; i < this.TabCount; i++) {
                if (this.TabPages[i].Controls.Count == 0) continue;

                if (this.TabPages[i].Controls[0] == plugin.GetComponent()) {
                    return this.TabPages[i];
                }
            }
            return null;
        }

        /// <summary>
        /// タブを閉じます
        /// </summary>
        /// <param name="tabPage"></param>
        private void CloseTab(TabPage tabPage)
        {
            //タブを削除します
            this.TabPages.Remove(tabPage);
        }

        /// <summary>
        /// タブ変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabPlugin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedTab != null) {
                if (this.SelectedTab.Controls.Count == 0) {
                    //アクティブプラグをリセットします
                    _pluginManager.ActivePlugin = null;
                    return;
                }

                //フォーカスを設定します
                var plugin = _pluginManager.GetPlugin(this.SelectedTab.Controls[0]);
                if (plugin != null) {
                    plugin.SetFocus();

                    //選択中のタブに表示されているプラグインが
                    //アクティブプラグインと異なる場合は、
                    //アクティブプラグインとして設定します。
                    if (_pluginManager.ActivePlugin != plugin) {
                        _pluginManager.ActivePlugin = plugin;
                    }
                    return;
                }
            }

            //アクティブプラグをリセットします
            _pluginManager.ActivePlugin = null;
        }

        private void TabPlugin_MouseDown(object sender, MouseEventArgs e)
        {
            //右クリックされた位置のタブを選択します
            if (e.Button == MouseButtons.Right) {
                for (int i = 0; i < this.TabCount; i++) {
                    if (this.GetTabRect(i).Contains(e.X, e.Y)) {
                        this.SelectedIndex = i;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 「閉じる」メニューを作成します
        /// </summary>
        /// <returns></returns>
        private ToolStripMenuItem CreateCloseMenu()
        { 
            var menu = new ToolStripMenuItem("閉じる");
            menu.Click += (sender, e) => {
                if (this.SelectedTab == null) return;

                //タブに関連付けられているプラグインを取得します
                var plugin = _pluginManager.GetPlugin(this.SelectedTab.Controls[0]);

                //プラグインを終了します
                _pluginManager.ClosePlugin(plugin);
            };
            return menu;
        }

        /// <summary>
        /// 「全て閉じる」メニューを作成します
        /// </summary>
        /// <returns></returns>
        private ToolStripMenuItem CreateAllCloseMenu()
        {
            var menu = new ToolStripMenuItem("全て閉じる");
            menu.Click += (sender, e) => {
                if (this.SelectedTab == null) return;

                //タブに関連付けられている子プラグインのリストを取得します
                var childPluginList = _pluginManager.GetChildPluginList(this);

                foreach (var plugin in childPluginList) {
                    //プラグインを終了します
                    _pluginManager.ClosePlugin(plugin);
                }
            };
            return menu;
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

            //タブを生成します
            var tabPage = new TabPage { Text = plugin.Title };

            //生成されたプラグインのコントロールをタブに設定します
            var control = ((Control)plugin.GetComponent());
            control.Parent = tabPage;
            control.Dock = DockStyle.Fill;
            control.BringToFront();

            //タブを追加します
            this.TabPages.Add(tabPage);
            this.SelectedTab = tabPage; //追加したタブを選択状態にします

            //プラグインにフォーカスを設定します
            plugin.SetFocus();

            //タブを表示します
            this.Visible = true;

            //イベントを処理済みにします
            param.Handled = true;
        }

        /// <summary>
        /// プラグイン生成イベント
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sender"></param>
        public void PluginEvent_AllPluginCreated(EventParam param, object sender)
        {
            //タブがある場合は、先頭のタブを選択します
            if (this.TabPages.Count >= 1) this.SelectedTab = this.TabPages[0];

            //タブがある場合は、タブを表示します
            if (this.TabCount > 0) {
                this.Visible = true;
            } else {
                this.Visible = false;
            }
        }

        /// <summary>
        /// プラグイン終了イベント
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sender"></param>
        public void PluginEvent_PluginClosed(EventParam param, object sender)
        {
            //閉じられたプラグインを取得します
            var plugin = ((PluginClosedEventParam)param).Plugin;

            //プラグインに関連付けられているタブを取得します
            var tabPage = GetTabPage(plugin);
            if (tabPage != null) {

                //選択タブのインデックスを保持します
                int index = this.SelectedIndex;

                //タブを閉じます
                CloseTab(tabPage);
                
                //前回のタブの位置と同じ位置のタブを選択します。存在しない場合は、右端のタブを選択します
                if (index > this.TabCount - 1) index = this.TabCount - 1;
                if (index >= 0) {
                    this.SelectedTab = this.TabPages[index];
                }
            }

            //タブが1つもない場合は、タブを非表示にします
            if (this.TabCount == 0) this.Visible = false;
        }

        /// <summary>
        /// プラグインのタイトル変更イベント
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sender"></param>
        public void PluginEvent_TitleChanged(EventParam param, object sender)
        {
            //タイトルが変更されたプラグインを取得します
            var plugin = ((TitleChangedEventParam)param).Plugin;

            //プラグインを保持しているタブを取得します
            var tabPage = GetTabPage(plugin);
            if (tabPage != null) {
                //タブのタイトルを設定します
                tabPage.Text = plugin.Title;
            }
        }

    } //class
}
