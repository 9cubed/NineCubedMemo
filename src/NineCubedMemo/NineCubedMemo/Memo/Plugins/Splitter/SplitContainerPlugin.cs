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

namespace NineCubed.Memo.Plugins.Splitter
{
    /// <summary>
    /// スプリットコンテナープラグイン
    /// </summary>
    public class SplitContainerPlugin : SplitContainer, IPlugin
    {
        /******************************************************************************
         * 
         *  IPlugin
         * 
         ******************************************************************************/
        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //イベントハンドラーを登録します
            _pluginManager.GetEventManager().AddEventHandler(PluginCreatedEventParam.Name, this);

            if (param["orientation"] != null) {
                if (param["orientation"].Equals("horizontal")) this.Orientation = Orientation.Horizontal;
                if (param["orientation"].Equals("vertical"))   this.Orientation = Orientation.Vertical;
            }

            this.FixedPanel = FixedPanel.Panel1; //スプリッターを移動した際に、左のパネルをリサイズしないようにする
            this.SplitterWidth = 6;
            this.SplitterDistance = 180; //スプリッターの位置
            this.BackColor        = Color.LightGray; //スプリッターのバーの色
            this.Panel1.BackColor = SystemColors.Control;
            this.Panel2.BackColor = SystemColors.Control;
            return true;
        }

        //プラグイン配置後の初期化処理を行います
        public void InitializePlaced() {
            this.Dock = DockStyle.Fill;
            this.BringToFront();
        }
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

            //パネル１が未割当の場合は、生成されたプラグインをパネル１に割り当てます
            if (this.Panel1.Controls.Count == 0) {
                ((Control)plugin.GetComponent()).Parent = this.Panel1;
                param.Cancel = true; //処理済みとしてイベントをキャンセルします
                return;
            }

            //パネル２が未割当の場合は、生成されたプラグインをパネル２に割り当てます
            if (this.Panel2.Controls.Count == 0) {
                ((Control)plugin.GetComponent()).Parent = this.Panel2;

                //次回からイベントを受け取らないようにするため、
                //イベントハンドラーを削除します
                _pluginManager.GetEventManager().RemoveEventHandler(PluginCreatedEventParam.Name, this);

                param.Cancel = true; //処理済みとしてイベントをキャンセルします
                return;
            }
        }


    } //class
}
