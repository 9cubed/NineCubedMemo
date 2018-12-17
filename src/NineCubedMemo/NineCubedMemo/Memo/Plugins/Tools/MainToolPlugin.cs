using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.Tools
{
    public class MainToolPlugin : ToolStrip, IPlugin
    {
        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //イベントハンドラーを登録します
            _pluginManager.GetEventManager().AddEventHandler(PluginCreatedEventParam.Name, this);

            //ツールバーのボタンの画像サイズを設定します
            this.ImageScalingSize = new Size(20, 20);
            return true;
        }

        //プラグイン配置後の初期化処理を行います
        public void InitializePlaced() {}

        private PluginManager _pluginManager = null;                    //プラグインマネージャー
        public string    PluginId         { get; set; }                 //プラグインID
        public Component GetComponent()   { return this; }              //プラグインのコンポーネントを返します
        public string    Title            { get; set; }                 //プラグインのタイトル
        public bool      CanClosePlugin() { return true; }              //プラグインが終了できるかどうか
        public void      ClosePlugin()    { Parent = null; Dispose(); } //プラグインの終了処理
        public void      SetFocus()       {  }                          //フォーカスを設定します

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

            //生成されたプラグインのコンポーネントがツールバーのボタンの場合は、
            //ボタンをツールバーに追加します。
            if (plugin.GetComponent() is ToolStripButton tool) {
                this.Items.Add(tool);
                param.Cancel = true;
            }
        }

    } //class
}
