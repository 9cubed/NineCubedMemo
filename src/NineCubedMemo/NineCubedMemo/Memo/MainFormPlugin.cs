using NineCubed.Common.Collections;
using NineCubed.Common.Controls.FileList;
using NineCubed.Common.Files;
using NineCubed.Common.Utils;
using NineCubed.Memo.Exceptions;
using NineCubed.Memo.Interfaces;
using NineCubed.Memo.Plugins;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.FileList;
using NineCubed.Memo.Plugins.FileTree;
using NineCubed.Memo.Plugins.Interfaces;
using NineCubed.Memo.Plugins.PluginLoader;
using NineCubed.Memo.Plugins.Tab;
using NineCubed.Memo.Plugins.Test;
using NineCubed.Memo.Plugins.TextEditor;
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
    public partial class MainFormPlugin : Form, IPlugin, ISearchString
    {
        /// <summary>
        /// プロパティファイル
        /// </summary>
        private IniFile _property = new IniFile();

        /// <summary>
        /// 検索条件
        /// </summary>
        public SearchData GetSearchData() {
            return _pluginManager.GetSearchData();
        }

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
            //スプリッターの初期化
            splitVertical.Parent = this;
            splitVertical.Dock = DockStyle.Fill;
            splitVertical.FixedPanel = FixedPanel.Panel1; //スプリッターを移動した際に、左のパネルをリサイズしないようにする
            splitVertical.SplitterWidth = 6;
            splitVertical.SplitterDistance = 180; //スプリッターの位置
            splitVertical.BackColor        = Color.LightGray; //スプリッターのバーの色
            splitVertical.Panel1.BackColor = SystemColors.Control;
            splitVertical.Panel2.BackColor = SystemColors.Control;

            splitHorizon.Parent = splitVertical.Panel1;
            splitHorizon.Dock = DockStyle.Fill;
            splitHorizon.FixedPanel = FixedPanel.Panel1; 
            splitHorizon.SplitterWidth = 6;
            splitHorizon.SplitterDistance = splitHorizon.Height / 2; //スプリッターの位置
            splitHorizon.BackColor        = Color.LightGray; //スプリッターのバーの色
            splitHorizon.Panel1.BackColor = SystemColors.Control;
            splitHorizon.Panel2.BackColor = SystemColors.Control;

            //ファイルツリービュープラグインの初期化
            var fileTreePlugin = (FileTreePlugin)_pluginManager.CreatePluginInstance(typeof(FileTreePlugin), null, splitHorizon.Panel1);

            //ファイルリストプラグインの初期化
            var fileListPlugin = (FileListPlugin)_pluginManager.CreatePluginInstance(typeof(FileListPlugin), null, splitHorizon.Panel2);
            fileListPlugin.ShowFileList(@"D:\");


            //タブプラグインを生成します
            //プラグインを生成します
            var tabPlugin = (TabPlugin)_pluginManager.CreatePluginInstance(typeof(TabPlugin), null, splitVertical.Panel2);
            //tabPlugin.Parent = splitVertical.Panel2;
            tabPlugin.Dock = DockStyle.None;
            tabPlugin.Dock = DockStyle.Fill;
            tabPlugin.BringToFront();
            */

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

            //ConfigをPluginManagerに設定します
            _pluginManager = PluginManager.GetInstance();

            //ダイアログの初期設定
            openFileDialog.Filter = 
                //"テキストファイル(*.txt)|*.txt" + "|" + 
                "すべてのファイル(*.*)|*.*";
            saveFileDialog.Filter = openFileDialog.Filter;

            return true;
        }

        public void      InitializePlaced() { } //プラグイン配置後の初期化処理を行います
        private PluginManager _pluginManager = null;                    //プラグインマネージャー
        public string    PluginId         { get; set; }                 //プラグインID
        public Component GetComponent()   { return this; }              //プラグインのコンポーネントを返します
        public string    Title            { get; set; }                 //プラグインのタイトル
        public bool      CanClosePlugin() { return true; }              //プラグインが終了できるかどうか
        public void      ClosePlugin()    { Parent = null; Dispose(); } //プラグインの終了処理
        public void      SetFocus()       {  }                          //フォーカスを設定します

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



        /// <summary>
        /// メニュー・ヘルプ・バージョン情報 の Clickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuHelp_version_Click(object sender, EventArgs e)
        {
            //バージョン情報を取得します
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            string version =
                fileVersionInfo.ProductName    + " "  + //製品名
                fileVersionInfo.ProductVersion + "\n" + //製品バージョン
                fileVersionInfo.LegalCopyright;         //コピーライト

            //バージョン情報を表示します
            MessageBox.Show(version);
        }

        /// <summary>
        /// メニュー・ファイル・開く の Clickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFile_Open_Click(object sender, EventArgs e)
        {
            try {
                //ファイルをプラグインで開きます
                OpenFile(null, new AnyFile());

            } catch (CancelException) {
                //キャンセル時
            } catch (Exception ex) {
                __.ShowErrorMsgBox(ex);
            }
        }

        /// <summary>
        /// メニュー・ファイル・開く（文字コード指定） の Clickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFile_Open_Encoding_Sub_Click(object sender, EventArgs e)
        {
            //クリックされたメニューによってエンコーディングを設定します
            Encoding encoding = null;
            if (menuFile_Open_ShiftJIS     == sender) encoding = Encoding.GetEncoding(932);   //Shift JIS
            if (menuFile_Open_UTF8         == sender) encoding = new UTF8Encoding(false);     //UTF-8 BOMなし
            if (menuFile_Open_UTF8_BOM     == sender) encoding = new UTF8Encoding(true);      //UTF-8 BOMあり
            if (menuFile_Open_UTF16_LE_BOM == sender) encoding = Encoding.GetEncoding(1200);  //UTF-16 LE
            if (menuFile_Open_UTF16_BE_BOM == sender) encoding = Encoding.GetEncoding(1201);  //UTF-16 BE
            if (menuFile_Open_EucJp        == sender) encoding = Encoding.GetEncoding(51932); //EUC-JP

            try {
                //ファイルを開くダイアログを表示します
                var path = ShowOpenFileDialog();
                if (path == null) return;
                
                //プラグイン生成パラメーターを設定します
                var pluginCreateParam = new PluginCreateParam {
                    ["path"]      = path,     //選択されたパス
                    ["encoding"]  = encoding, //文字コード
                    ["is_binary"] = false     //テキストモード
                };

                //テキストエディタープラグインを生成します
                var plugin = (IFilePlugin)_pluginManager.CreatePluginInstance(typeof(TextEditorPlugin), pluginCreateParam);

            } catch (CancelException) {
                //キャンセル時
            } catch (Exception ex) {
                __.ShowErrorMsgBox(ex);
            }
        }

        /// <summary>
        /// メニュー・ファイル・上書き保存 の Clickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFile_Save_Click(object sender, EventArgs e)
        {
            try {
                //テキストファイルに保存します
                SaveFile();
            } catch (CancelException) {
                //キャンセル時
            } catch (Exception ex) {
                __.ShowErrorMsgBox(ex);
            }
        }

        /// <summary>
        /// メニュー・ファイル・名前を付けて保存 の Clickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFile_SaveAs_Click(object sender, EventArgs e)
        {
            try {
                //テキストファイルに保存します
                SaveFile(true);
            } catch (CancelException) {
                //キャンセル時
            } catch (Exception ex) {
                __.ShowErrorMsgBox(ex);
            }
        }

        /// <summary>
        /// メニュー・ファイル・閉じる の Clickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFile_Close_Click(object sender, EventArgs e)
        {
            if (_pluginManager.ActivePlugin != null) {
                //アクティブプラグインを終了します
                _pluginManager.ClosePlugin( _pluginManager.ActivePlugin );
            }
        }

        //メニュー・編集の Clickイベント
        private void menuEdit_Undo_Click  (object sender, EventArgs e) {
            if (_pluginManager.ActivePlugin is IEditPlugin plugin) plugin.Undo();
        }
        private void menuEdit_Redo_Click  (object sender, EventArgs e) {
            if (_pluginManager.ActivePlugin is IEditPlugin plugin) plugin.Redo();
        }
        private void menuEdit_Cut_Click   (object sender, EventArgs e) {
            if (_pluginManager.ActivePlugin is IEditPlugin plugin) plugin.Cut();
        }
        private void menuEdit_Copy_Click  (object sender, EventArgs e) {
            if (_pluginManager.ActivePlugin is IEditPlugin plugin) plugin.Copy();
        }
        private void menuEdit_Paste_Click (object sender, EventArgs e) {
            if (_pluginManager.ActivePlugin is IEditPlugin plugin) plugin.Paste();
        }
        private void menuEdit_Delete_Click(object sender, EventArgs e) {
            if (_pluginManager.ActivePlugin is IEditPlugin plugin) plugin.Delete();
        }

        /// <summary>
        /// メニュー・検索・検索 の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSearch_Search_Click(object sender, EventArgs e)
        {
            if (_pluginManager.ActivePlugin is TextEditorPlugin plugin) {
                //検索画面を表示します
                var searchString = (plugin.GetSearchString().Length > 0) ? plugin.GetSearchString() : _pluginManager.GetSearchData().SearchString;
                _pluginManager.GetSearchData().SearchString = searchString;
                SearchForm.ShowForm(this, _pluginManager.GetSearchData());
            }
        }

        //メニュー・検索・前方検索 の click イベント
        private void menuSearch_SearchForward_Click(object sender, EventArgs e) { SearchForward(); }

        //メニュー・検索・後方検索 の click イベント
        private void menuSearch_SearchBackward_Click(object sender, EventArgs e) { SearchBackward(); }

        //メニュー・検索・前方置換 の click イベント
        private void menuSearch_ReplaceForward_Click(object sender, EventArgs e) { ReplaceForward(); }

        //メニュー・検索・後方置換 の click イベント
        private void menuSearch_ReplaceBackward_Click(object sender, EventArgs e) { ReplaceBackward(); }

        //メニュー・検索・全て置換 の click イベント
        private void menuSearch_ReplaceAll_Click(object sender, EventArgs e) { ReplaceAll(); }

        // ツールバー の Clickイベント
        private void toolFile_Open_Click(object sender, EventArgs e) { menuFile_Open_Click(sender, e); }
        private void toolFile_Save_Click(object sender, EventArgs e) { menuFile_Save_Click(sender, e); }


        /// <summary>
        /// ファイルをプラグインで開きます
        /// </summary>
        /// <param name="pluginType">プラグインの型</param>
        /// <param name="file">ファイル</param>
        private IPlugin OpenFile(Type pluginType, IFile file)
        {
            //パスが未設定の場合は、開くダイアログを表示します
            if (file.Path == null) {
                file.Path = ShowOpenFileDialog();
                if (file.Path == null) return null;
            }

            //ファイルが存在しない場合は処理しない
            if (File.Exists(file.Path) == false) return null;

            //ファイル選択イベントを発生させます
            var param = new FileSelectedEventParam { Path = file.Path };
            _pluginManager.GetEventManager().RaiseEvent(FileSelectedEventParam.Name,  null, param);

            return null;
        }

        /// <summary>
        /// ファイルを開くダイアログを表示します
        /// </summary>
        /// <returns>選択されたファイルのパス。未選択の場合は null。</returns>
        private string ShowOpenFileDialog()
        {
            openFileDialog.FileName = "";
            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK) {
                //選択されたパスを取得します
                return openFileDialog.FileName;
            } else {
                return null;
            }
        }

        /// <summary>
        /// テキストファイルを保存します
        /// </summary>
        /// <param name="showDialog">true:ファイル選択ダイアログを表示します</param>
        private void SaveFile(bool showDialog = false)
        {
            if (_pluginManager.ActivePlugin is IFilePlugin plugin) {

                //保存ダイアログを表示します
                if (showDialog) {
                    //保存ダイアログを表示します
                    var result = saveFileDialog.ShowDialog();
                    if (result == DialogResult.Cancel) throw new CancelException();
                    if (result == DialogResult.No) return;

                    //選択されたファイルのパスを保持します
                    plugin.TargetFile.Path = saveFileDialog.FileName;
                }

                //アクティブプラグインのファイルを保存します
                plugin.SaveFile();
            }
        }

        //キー操作の記録開始・終了
        private void menuMacro_StartRec_Click(object sender, EventArgs e)
        {
            if (_pluginManager.ActivePlugin is IKeyMacroPlugin plugin) {
                if (plugin.IsRecording() == false) {
                    //キー操作の記録を開始します
                    plugin.StartRecording();
                } else {
                    //キー操作の記録を停止します
                    plugin.StopRecording();
                }
            }
        }

        //キー操作の再生
        private void menuMacro_Play_Click(object sender, EventArgs e)
        {
            if (_pluginManager.ActivePlugin is IKeyMacroPlugin plugin) {
                //キー操作を再生します
                plugin.Play();
            }
        }

        //キー操作をテキストに出力します
        private void menuMacro_List_Click(object sender, EventArgs e)
        {
            if (_pluginManager.ActivePlugin is IKeyMacroPlugin plugin) {
                plugin.OutputMacro();
            }
        }

        //キー操作の登録
        private void menuMacro_Set_Click(object sender, EventArgs e)
        {
            if (_pluginManager.ActivePlugin is IKeyMacroPlugin plugin) {
                plugin.SetMacro();
            }
        }

        /// <summary>
        /// メニュー・ファイル・開く(バイナリー形式) の Clickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFile_Open_Binary_Click(object sender, EventArgs e)
        {
            try {
                //ファイルを開くダイアログを表示します
                var path = ShowOpenFileDialog();
                if (path == null) return;

                //プラグイン生成パラメーターを設定します
                var pluginCreateParam = new PluginCreateParam {
                    ["path"]      = path, //選択されたパス
                    ["encoding"]  = null, //文字コード
                    ["is_binary"] = true  //バイナリーモード
                };

                //テキストエディタープラグインを生成します
                var plugin = (IFilePlugin)_pluginManager.CreatePluginInstance(typeof(TextEditorPlugin), pluginCreateParam);

            } catch (CancelException) {
                //キャンセル時
            } catch (Exception ex) {
                __.ShowErrorMsgBox(ex);
            }
        }

        private void menuFile_End_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// テストプラグインを生成します
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuDebug_createTestPlugin_Click(object sender, EventArgs e)
        {
            //プラグインを生成します
            var plugin = (IPlugin)_pluginManager.CreatePluginInstance(typeof(TestPlugin));
        }


        /// <summary>
        /// 前方検索します。テキストの末尾へ向けて検索します。
        /// </summary>
        /// <returns>見つかった位置</returns>
        public int SearchForward()
        {
            if (_pluginManager.ActivePlugin is ISearchPlugin plugin) {
                return plugin.SearchForward();
            }
            return -1;
        }

        /// <summary>
        /// 後方検索します。テキストの先頭へ向けて検索します。
        /// </summary>
        /// <returns>見つかった位置</returns>
        public int SearchBackward()
        {
            if (_pluginManager.ActivePlugin is ISearchPlugin plugin) {
                return plugin.SearchBackward();
            }
            return -1;
        }

        /// <summary>
        /// 前方置換します。
        /// </summary>
        /// <returns>見つかった位置</returns>
        public int ReplaceForward()
        {
            if (_pluginManager.ActivePlugin is ISearchPlugin plugin) {
                return plugin.ReplaceForward();
            }
            return -1;
        }

        /// <summary>
        /// 後方置換します。
        /// </summary>
        /// <returns>見つかった位置</returns>
        public int ReplaceBackward()
        {
            if (_pluginManager.ActivePlugin is ISearchPlugin plugin) {
                return plugin.ReplaceBackward();
            }
            return -1;
        }

        /// <summary>
        /// 全置換します。
        /// </summary>
        public void ReplaceAll()
        {
            if (_pluginManager.ActivePlugin is ISearchPlugin plugin) {
                plugin.ReplaceAll();
            }
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

            //プラグインのコントロールを受け取っていない場合は、受け取る
            if (_isReceivedControl == false && plugin.GetComponent() != null) {
                _isReceivedControl = true; //受取済みにします

                //コントロールをフォームに配置します
                var ctl = ((Control)plugin.GetComponent());
                ctl.Parent = this;
                ctl.Dock = DockStyle.Fill;
                param.Cancel = true; //処理済みとしてイベントをキャンセルします
                return;
            }

            //TODO ツールバーとメニューを受け取るようにする
        }

    } //class
}
