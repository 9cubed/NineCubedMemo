using NineCubed.Memo.Plugins;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sample
{
    /// <summary>
    /// サンプルプラグイン
    /// 
    /// 参照設定で以下を追加
    ///  1. アセンブリ   : System.Windows.Forms
    ///  2. プロジェクト : NineCubedMemo
    /// </summary>
    public class SamplePlugin : IPlugin
    {
        //画面に表示するテキストボックス
        private TextBox _textbox;

        //初期処理を行います
        public bool Initialize(PluginCreateParam param) {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //ラベルを生成します
            _textbox = new TextBox();
            _textbox.Text = "サンプルプラグインです。";
            _textbox.Dock = DockStyle.Fill;
            _textbox.KeyDown += _textbox_KeyDown;

            return true;
        }

        private void _textbox_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("test");
        }

        public void InitializePlaced() { } //プラグイン配置後の初期化処理を行います
        private PluginManager _pluginManager = null;           //プラグインマネージャー
        public string     PluginId         { get; set; }        //プラグインID
        public IPlugin    ParentPlugin     { get; set; }        //親プラグイン
        public IComponent GetComponent()   { return _textbox; } //プラグインのコンポーネントを返します
        public string     Title            { get; set; }        //プラグインのタイトル
        public void       SetFocus()       { _textbox.Focus();} //フォーカスを設定します
        public bool       CanClosePlugin() { return true; }     //プラグインが終了できるかどうか
        public void       ClosePlugin()    {                    //プラグインの終了処理
            _textbox.Parent = null;
            _textbox.Dispose();
        }

    } //class
}
