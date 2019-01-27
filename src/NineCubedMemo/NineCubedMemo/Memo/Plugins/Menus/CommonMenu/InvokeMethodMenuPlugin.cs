using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//NineCubed.Memo.Plugins.Menus.CommonMenu.InvokeMethodMenuPlugin
namespace NineCubed.Memo.Plugins.Menus.CommonMenu
{
    public class InvokeMethodMenuPlugin : ToolStripMenuItem, IPlugin
    {
        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プロパティファイルを読み込みます
            var property = new PluginProperty();
                property.Load(param.PropertyPath);

            //プロパティから値を取得します
            var text      = property[PluginProperty.NO_SECTION, "text"];
            var className = property[PluginProperty.NO_SECTION, "create_plugin_class_name"];
            var method    = property[PluginProperty.NO_SECTION, "method"];

            //メニューを作成します
            this.Text        = text;
            //this.ShortcutKeys = Keys.F3; //TODO プロパティで指定できるようにする
            this.Click += (sender, e) => {
                var targetPlugin = PluginManager.GetInstance().ActivePlugin;
                if (targetPlugin == null) return;

                //リフレクションで指定されたメソッドを取得します
                var methodInfo = targetPlugin.GetType().GetMethod(method);
                
                //メソッドが取得できない場合は、処理を中止します
                if (methodInfo == null) return; //TODO ログ出力
                
                //メソッドを実行します
                methodInfo.Invoke(targetPlugin, null);
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
