using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.Menus.CommonMenu
{
    public class GroupMenuPlugin : ToolStripMenuItem, IPlugin
    {
        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プロパティファイルを読み込みます
            var property = new PluginProperty();
                property.Load(param.PropertyPath);

            var text = property[PluginProperty.NO_SECTION, "text"];

            //メニューを作成します
            this.Text = text;
            //this.ShortcutKeys = Keys.F3; //TODO プロパティで指定できるようにする

            return true;
        }
        
        public void       InitializePlaced() {}                            //プラグイン配置後の初期化処理を行います
        public string     PluginId           { get; set; }                 //プラグインID
        public IPlugin    ParentPlugin       { get; set; }                 //親プラグイン
        public IComponent GetComponent()     { return this; }              //プラグインのコンポーネントを返します
        public string     Title              { get; set; }                 //プラグインのタイトル
        public bool       CanClosePlugin()   { return true; }              //プラグインが終了できるかどうか
        public void       ClosePlugin()      { Parent = null; Dispose(); } //プラグインの終了処理
        public void       SetFocus()         {  }                          //フォーカスを設定します

        /// <summary>
        /// プラグイン生成イベント
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sender"></param>
        public void PluginEvent_PluginCreated(EventParam param, object sender) {

            //生成されたプラグインを取得します
            var plugin = ((PluginCreatedEventParam)param).Plugin;

            //生成されたプラグインのコンポーネントを取得します
            var component = plugin.GetComponent();

            //生成されたプラグインのコンポーネントがメニューの場合
            if (component is ToolStripMenuItem) {
                //メニューを追加します
                this.DropDownItems.Add((ToolStripMenuItem)component);

                //イベントを処理済みにします
                param.Handled = true;
            }
        }


    } //class
}
