using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.ProcessStart
{
    class ProcessStartPlugin : IPlugin
    {
        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //指定されたファイルがある場合は、ファイルを開きます
            if (FileUtils.Exists(param.Path)) {
                //指定されたパスの先頭の1行を読み込みます
                var lines = TextFileUtils.ReadTopLines(param.Path, Encoding.UTF8, 1);
                try {
                    var path = lines[0].Trim();

                    //OSで指定されている方法でファイルを開きます
                    Process.Start(path);
                } catch (Exception ex) {
                    MsgBoxUtils.ShowErrorMsgBox(ex.Message);
                }
            }

            //false を返してプラグインを破棄させます
            return false;
        }

        //プラグイン配置後の初期化処理を行います
        public void InitializePlaced() {}

        private PluginManager _pluginManager = null;        //プラグインマネージャー
        public string     PluginId         { get; set; }    //プラグインID
        public IPlugin    ParentPlugin     { get; set; }    //親プラグイン
        public IComponent GetComponent()   { return null; } //プラグインのコンポーネントを返します
        public string     Title            { get; set; }    //プラグインのタイトル
        public bool       CanClosePlugin() { return true; } //プラグインが終了できるかどうか
        public void       ClosePlugin()    { }              //プラグインの終了処理
        public void       SetFocus()       { }              //フォーカスを設定します

    } //class
}
