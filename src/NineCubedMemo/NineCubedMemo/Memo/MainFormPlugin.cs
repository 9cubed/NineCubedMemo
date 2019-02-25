using NineCubed.Common.Collections;
using NineCubed.Common.Controls.FileList;
using NineCubed.Common.Files;
using NineCubed.Common.Utils;
using NineCubed.Memo.Exceptions;
using NineCubed.Memo.Plugins;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.FileList;
using NineCubed.Memo.Plugins.FileTree;
using NineCubed.Memo.Plugins.Interfaces;
using NineCubed.Memo.Plugins.PluginLoader;
using NineCubed.Memo.Plugins.SearchForm;
using NineCubed.Memo.Plugins.Tab;
using NineCubed.Memo.Plugins.Test;
using NineCubed.Memo.Plugins.TextEditor;
using NineCubed.Memo.Plugins.Theme;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace NineCubed.Memo
{
    public partial class MainFormPlugin : Form, IPlugin
    {
        /// <summary>
        /// プロパティファイル
        /// </summary>
        private IniFile _property = new IniFile();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainFormPlugin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// MainForm の Load イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            /*
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1) {
                //コマンドライン引数がある場合
                //(EXEにファイルをD&D、送るメニューでファイル指定、ショートカットで引数指定による起動)

                //ファイルを開きます
                try {
                    var file = new AnyFile();
                    file.Path = args[1];
                    var plugin = OpenFile(null, file);
                    if (plugin != null) {
                        //プラグイン生成イベントを発生させます
                        var param = new PluginCreatedEventParam { Plugin = plugin };
                        _pluginManager.GetEventManager().RaiseEvent(PluginCreatedEventParam.Name, null, param);
                    }

                } catch (Exception ex) {
                    //例外が発生した場合は、アプリを終了させます
                    __.ShowErrorMsgBox(ex);
                    this.Dispose();
                    return;
                }

            } else {
                //コマンドライン引数がない場合(通常起動)
            }
            */

        }

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

            //メインフォームとして設定します
            _pluginManager.MainForm = this;

            //プロパティファイルを読み込みます
            LoadProperty(param.PropertyPath);

            //共通カラーデータを取得します
            //未設定の場合は、生成して共通データに設定します
            var colorData = (ColorData)_pluginManager.CommonData[CommonDataKeys.ColorData];
            if (colorData == null) {
                colorData = new ColorData();
                _pluginManager.CommonData[CommonDataKeys.ColorData] = colorData;
            }

            return true;
        }

        private PluginManager _pluginManager = null;                    //プラグインマネージャー
        public void       InitializePlaced() { }                         //プラグイン配置後の初期化処理を行います
        public string     PluginId         { get; set; }                 //プラグインID
        public IPlugin    ParentPlugin     { get; set; }                 //親プラグイン
        public IComponent GetComponent()   { return this; }              //プラグインのコンポーネントを返します
        public string     Title            { get; set; }                 //プラグインのタイトル
        public bool       CanClosePlugin() { return true; }              //プラグインが終了できるかどうか
        public void       ClosePlugin()    { Parent = null; Dispose(); } //プラグインの終了処理
        public void       SetFocus()       {  }                          //フォーカスを設定します

        /// <summary>
        /// MainForm の FormClosing イベント
        /// フォームが閉じられる直前に呼ばれます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try {
                //全てのプラグインが終了できるか？
                if (_pluginManager.CanCloseAllPlugins() == false) {
                    //終了できないプラグインがある場合、フォームが閉じられるのをキャンセルします
                    e.Cancel = true;
                }

            } catch (CancelException) {
                //キャンセルされた場合、フォームが閉じるのを中止します
                e.Cancel = true;
            } catch (Exception ex) {
                e.Cancel = true;
                __.ShowErrorMsgBox(ex);
            }
        }

        /// <summary>
        /// MainForm の FormClosed イベント
        /// フォームが閉じられた時に呼ばれます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try {
                //プロパティを保存します
                SaveProperty();

                //全てのプラグインを終了します
                _pluginManager.CloseAllPlugins();

                //デバッグ用チェック処理
                _pluginManager.CheckPluginLeak();
                _pluginManager.GetEventManager().CheckEventLeak();
                
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// プロパティファイルを読み込みます
        /// </summary>
        private void LoadProperty(string path)
        {
            //プロパティファイルを読み込みます
            _property.Load(path);

            if (_property["size", "width"] == null || _property["size", "height"] == null) {
                //幅と高さが指定されていない場合
                //フォームを中央に表示します
                var screenBounds = Screen.PrimaryScreen.Bounds;
                this.Width  = (int)(screenBounds.Width  * 0.6);
                this.Height = (int)(screenBounds.Height * 0.6);
                this.StartPosition = FormStartPosition.CenterScreen;
            } else {
                //幅と高さが指定されている場合
                this.StartPosition = FormStartPosition.Manual;
                this.Left   = StringUtils.ToInt(_property["location", "left"]  , 0);
                this.Top    = StringUtils.ToInt(_property["location", "top"]   , 0);
                this.Width  = StringUtils.ToInt(_property["size"    , "width"] , 600);
                this.Height = StringUtils.ToInt(_property["size"    , "height"], 400);
            }
        }

        /// <summary>
        /// プロパティファイルに保存します
        /// </summary>
        private void SaveProperty()
        {
            //ウィンドウが最小化、最大化されている場合は、標準に戻します
            //(Configに保存する際のサイズがわからないため)
            if (this.WindowState != FormWindowState.Normal) {
                this.WindowState  = FormWindowState.Normal;
            }

            //Configに現在の状態を設定します
            _property["location", "left"]   = this.Left  .ToString();
            _property["location", "top"]    = this.Top   .ToString();
            _property["size"    , "width"]  = this.Width .ToString();
            _property["size"    , "height"] = this.Height.ToString();

            //プロパティを保存します
            _property.Save();
        }

        /******************************************************************************
         * 
         *  プラグイン用イベントハンドラー
         * 
         ******************************************************************************/ 

        //生成されたプラグインのコントロールを受け取ったかどうか
        bool _isReceivedControl = false;

        /// <summary>
        /// プラグイン生成イベント
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sender"></param>
        public void PluginEvent_PluginCreated(EventParam param, object sender) {
            //生成されたプラグインを取得します
            var plugin = ((PluginCreatedEventParam)param).Plugin;

            //生成されたプラグインがメニューの場合は受けとる
            if (plugin.GetComponent() is MenuStrip)
            {
                //コントロールをフォームに配置します
                var ctl = ((MenuStrip)plugin.GetComponent());
                ctl.Parent = this;
                ctl.Dock = DockStyle.Top;
                param.Handled = true; //処理済みとしてイベントをキャンセルします
                return;
            }
            
            //生成されたプラグインがツールバーの場合は受けとる
            if (plugin.GetComponent() is ToolStrip)
            {
                //ツールバーをフォームに配置します
                var ctl = ((ToolStrip)plugin.GetComponent());
                ctl.Parent = this;
                ctl.Dock = DockStyle.Top;
                param.Handled = true; //処理済みとしてイベントをキャンセルします
                return;
            }

            //プラグインのコントロールを受け取っていない場合は、受け取る
            if (_isReceivedControl == false && plugin.GetComponent() is Control control) {
                _isReceivedControl = true; //受取済みにします

                //コントロールをフォームに配置します
                control.Parent = this;
                control.Dock = DockStyle.Fill;
                param.Handled = true; //処理済みとしてイベントをキャンセルします
                return;
            }
        }

    } //class
}
