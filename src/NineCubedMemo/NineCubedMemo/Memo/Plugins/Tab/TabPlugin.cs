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
        public TabPlugin() {
            InitializeComponent();

            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            this.Dock = DockStyle.Fill;

            //ポップアップメニューを設定します
            var popupMenu = new ContextMenuStrip();
            {
                var menu = new ToolStripMenuItem("閉じる");
                popupMenu.Items.Add(menu); 
                menu.Click += (sender, e) => {
                    if (this.SelectedTab == null) return;

                    //タブに関連付けられているプラグインを取得します
                    var plugin = _pluginManager.GetPlugin(this.SelectedTab.Controls[0]);

                    //プラグインを終了します
                    _pluginManager.ClosePlugin(plugin);
                };
            }
            this.ContextMenuStrip = popupMenu;

            //イベントハンドラーを登録します
            _pluginManager.GetEventManager().AddEventHandler(PluginCreatedEventParam.Name, this);
            _pluginManager.GetEventManager().AddEventHandler(PluginClosedEventParam.Name, this);
            _pluginManager.GetEventManager().AddEventHandler(TitleChangedEventParam.Name, this);
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

        /// <summary>
        /// プラグインマネージャー
        /// </summary>
        private PluginManager _pluginManager = null;

        /// <summary>
        /// プラグインのコンポーネントを返します
        /// </summary>
        /// <returns></returns>
        public Component GetComponent() { return this; }

        /// <summary>
        /// プラグインのタイトル
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// プラグインが終了できるかどうか
        /// </summary>
        /// <returns>true:終了できる  false:終了できない</returns>
        public bool CanClosePlugin() { return true; }

        /// <summary>
        /// プラグインの終了処理
        /// </summary>
        public void ClosePlugin() {
            //コントロールを削除します
            this.Parent = null;
            this.Dispose();
        }

        /// <summary>
        /// フォーカスを設定します
        /// </summary>
        public void SetFocus() {
            //TODO 現在のタブに対して、SetFocus() を実行します
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
            _pluginManager.ActivePlugin = null; 

            if (this.SelectedTab != null) {
                if (this.SelectedTab.Controls.Count == 0) return;

                //フォーカスを設定します
                var plugin = _pluginManager.GetPlugin(this.SelectedTab.Controls[0]);
                if (plugin != null) {
                    plugin.SetFocus();
                }
            }
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
        public void PluginEvent_PluginCreated(EventParam param, object sender) {

            //生成されたプラグインを取得します
            var plugin = ((PluginCreatedEventParam)param).Plugin;

            //タブを生成します
            var tabPage = new TabPage { Text = plugin.Title };

            //生成されたプラグインのコントロールをタブに設定します
            var cotrol = ((Control)plugin.GetComponent());
            cotrol.Parent = tabPage;
            cotrol.Dock = DockStyle.Fill;
            cotrol.BringToFront();

            //タブを追加します
            this.TabPages.Add(tabPage);
            this.SelectedTab = tabPage; //追加したタブを選択状態にします

            //イベントをキャンセルします。他のプラグインに、生成されたプラグインを横取りされないようにするため
            //param.Cancel = true;

            //プラグインにフォーカスを設定します
            plugin.SetFocus();
        }

        /// <summary>
        /// プラグイン終了イベント
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sender"></param>
        public void PluginEvent_PluginClosed(EventParam param, object sender) {
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
