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
using NineCubed.Memo.Plugins.Events;

namespace NineCubed.Memo.Plugins.Test
{
    public partial class TestPlugin : UserControl, IPlugin
    {
        public TestPlugin()
        {
            InitializeComponent();
        }

        private void TestPlugin_Load(object sender, EventArgs e) { }

        /// <summary>
        /// プラグインマネージャー
        /// </summary>
        private PluginManager _pluginManager = null;

        //初期処理を行います
        public bool Initialize(PluginCreateParam param) {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //イベントハンドラーを登録します
            _pluginManager.GetEventManager().AddEventHandler(PluginCreatedEventParam.Name, this);
            _pluginManager.GetEventManager().AddEventHandler( PluginClosedEventParam.Name, this);
            _pluginManager.GetEventManager().AddEventHandler( TitleChangedEventParam.Name, this);
            _pluginManager.GetEventManager().AddEventHandler(  DirSelectedEventParam.Name, this);
            _pluginManager.GetEventManager().AddEventHandler( FileSelectedEventParam.Name, this);

            return true;
        }

        //プラグイン配置後の初期化処理を行います
        public void InitializePlaced()
        {
            //コントロールを配置します
            pnlTop.Dock = DockStyle.Top;
            txtLog.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// プラグインID
        /// </summary>
        public string PluginId { get; set; } 

        //親プラグイン
        public IPlugin ParentPlugin { get; set; }

        /// <summary>
        /// プラグインのコンポーネントを返します
        /// </summary>
        /// <returns></returns>
        public IComponent GetComponent() { return this; }

        //タイトル用の連番
        static int no = 1;

        /// <summary>
        /// プラグインのタイトル
        /// </summary>
        public string Title {
            get {
                return "テスト" + no++;
            }
            set {}
        }

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
            txtLog.Focus();
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

            //ログ出力
            AddLog(plugin.GetType().FullName + " が生成されました。");
        }

        /// <summary>
        /// プラグイン終了イベント
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sender"></param>
        public void PluginEvent_PluginClosed(EventParam param, object sender) {
            //閉じられたプラグインを取得します
            var plugin = ((PluginClosedEventParam)param).Plugin;

            //ログ出力
            AddLog(plugin.GetType().FullName + " が削除されました。");
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

            //ログ出力
            AddLog(plugin.GetType().FullName + " のタイトルが、" + plugin.Title + " に変更されました。");
        }

        /// <summary>
        /// ディレクトリ選択イベント
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sender"></param>
        public void PluginEvent_DirSelected(EventParam param, object sender)
        {
            //ログ出力
            AddLog(((DirSelectedEventParam)param).Path + " が選択されました。");
        }

        /// <summary>
        /// ファイル選択イベント
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sender"></param>
        public void PluginEvent_FileSelected(EventParam param, object sender)
        {
            //ログ出力
            AddLog(((FileSelectedEventParam)param).Path + " が選択されました。");
        }

        //テキストボックスにログを出力します
        private void AddLog(string log) {
            txtLog.Text += log + "\n";
        }

        /******************************************************************************
         * 
         *  イベント
         * 
         ******************************************************************************/ 

        /// <summary>
        /// テキストがアクティブになった時のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLog_Enter(object sender, EventArgs e)
        {
            //アクティブプラグインにします
            _pluginManager.ActivePlugin = this;
        }

    } //class
}
