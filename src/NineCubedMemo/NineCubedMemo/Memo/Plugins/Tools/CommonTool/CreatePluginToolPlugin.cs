using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.Tools.CommonTool
{
    public class CreatePluginToolPlugin : ToolStripButton, IPlugin
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
            var toolTip   = _property[PluginProperty.NO_SECTION, "tool_tip_text"];
            var className = _property[PluginProperty.NO_SECTION, "create_plugin_class_name"];
            var imgPath   = _property[PluginProperty.NO_SECTION, "img_path"]; //img/calendar.png

            //ツールの画像を設定します
            if (StringUtils.IsNotEmpty(imgPath)) {
                this.Image = System.Drawing.Image.FromFile(FileUtils.AppendPath(param.DataPath, imgPath));
            }

            //メニューを作成します
            this.Text        = text;
            this.ToolTipText = toolTip;
            this.Click += (sender, e) => {
                //プラグインを生成します
                var type = _pluginManager.GetPluginType(className);
                var plugin = (IPlugin)_pluginManager.CreatePluginInstance(type, new PluginCreateParam(), this);
            };

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
