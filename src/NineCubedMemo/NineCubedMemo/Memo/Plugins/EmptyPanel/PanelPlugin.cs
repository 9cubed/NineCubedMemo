using NineCubed.Common.Files;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.EmptyPanel
{
    public class PanelPlugin : Panel, IPlugin
    {
        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            this.Dock = DockStyle.Fill;
            return true;
        }

        //プラグイン配置後の初期化処理を行います
        public void InitializePlaced() {}

        private PluginManager _pluginManager = null;                    //プラグインマネージャー
        public string     PluginId         { get; set; }                 //プラグインID
        public IPlugin    ParentPlugin     { get; set; }                 //親プラグイン
        public IComponent GetComponent()   { return this; }              //プラグインのコンポーネントを返します
        public string     Title            { get; set; }                 //プラグインのタイトル
        public bool       CanClosePlugin() { return true; }              //プラグインが終了できるかどうか
        public void       ClosePlugin()    { Parent = null; Dispose(); } //プラグインの終了処理
        public void       SetFocus()       {  }                          //フォーカスを設定します

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

            //生成されたプラグインのコントロールをタブに設定します
            var control = ((Control)plugin.GetComponent());
            control.Parent = this;
            control.Dock = DockStyle.Fill;
            control.BringToFront();

            //プラグインにフォーカスを設定します
            plugin.SetFocus();
        }

    } //class
}
