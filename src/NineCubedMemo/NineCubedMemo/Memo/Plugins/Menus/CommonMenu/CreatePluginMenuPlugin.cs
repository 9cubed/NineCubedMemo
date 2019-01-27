using NineCubed.Common.Files;
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
    /// <summary>
    /// プラグインを生成するためのメニュープラグイン
    /// </summary>
    public class CreatePluginMenuPlugin : ToolStripMenuItem, IPlugin
    {
        
        /// <summary>
        /// プロパティファイル
        /// </summary>
        PluginProperty _property;

        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //プロパティファイルを読み込みます
            _property = new PluginProperty();
            _property.Load(param.PropertyPath);

            var text      = _property[PluginProperty.NO_SECTION, "text"];
            var className = _property[PluginProperty.NO_SECTION, "create_plugin_class_name"];

            //メニューを作成します
            this.Text = text;
            this.Click += (sender, e) => {

                //プラグイン生成パラメーター
                var pluginCreateParam = new PluginCreateParam();

                //親プラグインがパスを保持している場合は、
                //パスをプラグイン生成パラメーターに設定します
                if (this.ParentPlugin is IPathPlugin parentPlugin) {
                    var path = parentPlugin.GetPath();
                    if (string.IsNullOrEmpty(path)) return; //TODO 要検討。パスが取得できない場合に、プラグインを生成するか？

                    //プラグイン生成パラメーターを設定します
                    pluginCreateParam["path"] = path; //選択されたパス
                }

                //プラグインを生成します
                var type = _pluginManager.GetPluginType(className);
                var plugin = (IPlugin)_pluginManager.CreatePluginInstance(type, pluginCreateParam, this);
            };

            return true;
        }
        
        private PluginManager _pluginManager = null;                       //プラグインマネージャー
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
