using NineCubed.Common.Files;
using NineCubed.Memo.Exceptions;
using NineCubed.Memo.Interfaces;
using NineCubed.Memo.Plugins;
using NineCubed.Memo.Plugins.Interfaces;
using NineCubed.Memo.Plugins.TextEditor;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace NineCubed.Memo
{
    public partial class MainForm : Form, ISearchString
    {
        /// <summary>
        /// プラグイン管理クラス
        /// </summary>
        private PluginManager _pluginManager;

        /// <summary>
        /// Config
        /// </summary>
        private AppConfig _config;

        /// <summary>
        /// 検索条件
        /// </summary>
        public SearchData GetSearchData() {
            return _pluginManager.GetSearchData();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            //Configを読み込みます
            _config = AppConfig.Load(this);

            //ConfigをPluginManagerに設定します
            _pluginManager = PluginManager.GetPluginManager();
            _pluginManager.Config = _config;

            //TODO タブで実装する
            _pluginManager.TitleChanged += PluginTitleChanged;
        }

        //TODO タブで実装する
        //TitleChange のテスト
        private void PluginTitleChanged(IPlugin plugin)
        {
            //TODO タブの中からタイトルが変更されたプラグインを探して、タブのタイトルを設定する
            this.Text = plugin.Title;
        }

        /// <summary>
        /// MainForm の Load イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1) {
                //コマンドライン引数がある場合
                //(EXEにファイルをD&D、送るメニューでファイル指定、ショートカットで引数指定による起動)

                //ファイルを開きます
                try {
                    var file = new AnyFile();
                    file.Path = args[1];
                    OpenFile(null, file);
                } catch (Exception ex) {
                    //例外が発生した場合は、アプリを終了させます
                    __.ShowErrorMsgBox(ex);
                    this.Dispose();
                    return;
                }

            } else {
                //コマンドライン引数がない場合(通常起動)
            }

            //ダイアログの初期設定
            openFileDialog.Filter = 
                //"テキストファイル(*.txt)|*.txt" + "|" + 
                "すべてのファイル(*.*)|*.*";
            saveFileDialog.Filter = openFileDialog.Filter;

        }

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
                //全てのプラグインを終了します
                _pluginManager.CloseAllPlugins();

                //Configを保存します
                _config.Save();
            } catch (Exception) {
            }
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
                //テキストファイルを開きます
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
                //テキストファイルを開きます
                var newFile = new TextFile();
                if (encoding != null) newFile.TextEncoding = encoding;
                var plugin = OpenFile(null, newFile);
                if (plugin == null) return; //開けなかった場合

                //BOMにより自動的に文字コードが変更された場合は、警告を表示します
                var targetFile = (TextFile)((IFilePlugin)plugin).TargetFile;
                if (encoding.CodePage != targetFile.TextEncoding.CodePage) {
                    __.ShowWarnMsgBox("自動判別により文字コードを変更しました。");
                }

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
        /// テキストファイルを開きます
        /// </summary>
        /// <param name="pluginType">プラグインの型</param>
        /// <param name="file">ファイル</param>
        private IPlugin OpenFile(Type pluginType, IFile file)
        {
            //パスが未設定の場合は、開くダイアログを表示します
            if (file.Path == null) {
                openFileDialog.FileName = "";
                var dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK) {
                    //選択されたパスを取得します
                    file.Path = openFileDialog.FileName;
                } else {
                    return null;
                }
            }

            //ファイルが存在しない場合は処理しない
            if (File.Exists(file.Path) == false) return null;

            //プラグインを生成します
            var plugin = (IFilePlugin)_pluginManager.CreatePluginInstance(pluginType, file.Path);
            if (plugin == null) {
                //プラグインが生成できなかった場合、他のアプリで開きます
                Process.Start(file.Path);
                return null;
            }

            //ファイルを開きます
            if (plugin.OpenFile(file)) {
                //ファイルが開けた場合

                //TODO タブができるまでの暫定処理。現在のプラグインを強制的に閉じます。
                if (_pluginManager.ActivePlugin != null) {
                    _pluginManager.ClosePlugin(_pluginManager.ActivePlugin);
                }

                //TODO タブにプラグインを追加します
                var control = ((Control)((IPlugin)plugin).GetComponent());
                control.Parent = this;
                control.Dock = DockStyle.Fill;
                control.BringToFront();
            }

            return (IPlugin)plugin;
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
                //テキストエディターでバイナリ形式でファイルを開きます
                OpenFile(typeof(TextEditorPlugin), new BinaryFile());
            } catch (CancelException) {
                //キャンセル時
            } catch (Exception ex) {
                __.ShowErrorMsgBox(ex);
            }
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

    } //class
}
