using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using NineCubed.Common.Utils;
using NineCubed.Memo.Menus;
using NineCubed.Common.Calculation;
using NineCubed.Memo.Plugins.Interfaces;
using NineCubed.Common.Files;
using NineCubed.Common.Controls;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.SearchForm;

namespace NineCubed.Memo.Plugins.TextEditor
{
    public partial class TextEditorPlugin : UserControl, IPlugin, IFilePlugin, IEditPlugin, ISearchPlugin, INewLinePlugin, IKeyMacroPlugin, IRefreshPlugin
    {
        /// <summary>
        /// プラグインマネージャー
        /// </summary>
        private PluginManager _pluginManager = null;

        /// <summary>
        /// プロパティファイル
        /// </summary>
        private IniFile _property = new IniFile();

        /// <summary>
        /// テキストファイルデータ
        /// </summary>
        public IFile TargetFile { get; set; }

        /// <summary>
        /// キー操作マクロ
        /// </summary>
        private KeyMacro _keyMacro;

        /// <summary>
        /// 検索条件
        /// </summary>
        public SearchData GetSearchData() {
            return (SearchData)_pluginManager.CommonData[CommonDataKeys.SearchData];
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TextEditorPlugin()
        {
            InitializeComponent();
        }

        private void TextEditorPlugin_Load(object sender, EventArgs e) { }

        /// <summary>
        /// 初期処理を行います
        /// 初期化に失敗した場合などは false を返すとプラグインが破棄されます
        /// この段階ではまだ他のプラグインに配置されていないため、コンポーネントのサイズなどは取得できません
        /// </summary>
        /// <returns>false:初期化失敗。プラグインが破棄されます。</returns>
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //プロパティファイルを読み込みます
            _property.Load(param.PropertyPath);

            //テキストボックスを初期化します
            txtMain.Initialize(
                                    _property["font", "name"],
                StringUtils.ToFloat(_property["font", "size"])
            );

            //テキストボックスのドラッグ＆ドロップ対応
            txtMain.AllowDrop = true; //D&Dを許可します
            txtMain.DragEnter += TxtMain_DragEnter;
            txtMain.DragDrop  += TxtMain_DragDrop;
            //(設定したのを忘れなければデザイン時にプロパティで設定しても問題ありません)

            //テキストボックスのテキストを初期化します
            txtMain.Clear(); //Modified は false になる

            //変更なしにします
            txtMain.Modified = false;

            //キー操作マクロの初期設定
            _keyMacro = new KeyMacro(txtMain);

            //ポップアップメニューを追加します
            AddPopupMenuItem();

            //参照用テキストボックスを初期化します
            txtSplit.Initialize(txtMain.Font.Name, txtMain.Font.Size);
            txtSplit.ReadOnly = true; //読込専用
            txtSplit.BackColor = Color.FromArgb(0xf0, 0xf0, 0xf0);

            //見出しリストを初期化します
            titleListbox.TargetTextbox = txtMain;
            titleListbox.TitleCharList = 
                new [] {
                    _property["title_list", "level_1"],
                    _property["title_list", "level_2"],
                    _property["title_list", "level_3"],
                }.ToList<string>();

            //3ペイン用のスプリットコンテナーにコントロールを配置します
            splitContainer.SetControl(titleListbox, txtSplit, txtMain);
            splitContainer.PanelLeftVisible = false;
            splitContainer.PanelTopVisible  = false;
            splitContainer.Dock = DockStyle.Fill;
            titleListbox.Dock   = DockStyle.Fill;
            txtSplit.Dock       = DockStyle.Fill;
            txtMain.Dock        = DockStyle.Fill;

            /****************************************
             * 
             *  ファイル読み込み
             * 
             ****************************************/

            //初期化パラメーターを取得します
            var path      = param.Path;
            var encoding  = param["encoding"] as Encoding;
            var is_binary = param.ToBool("is_binary");

            //ファイルオブジェクトを生成します
            IFile textFile;
            if (is_binary) {
                textFile = new BinaryFile();
            } else {
                textFile = new TextFile();
                ((TextFile)textFile).TextEncoding = encoding;
            }
            
            //テキストファイルを読み込みます
            textFile.Path = path;
            this.LoadFile(textFile);

            //BOMにより自動的に文字コードが変更された場合は、警告を表示します
            if (this.TargetFile is TextFile targetFile) {
                if (encoding != null) {
                    //文字コードが指定されていた場合
                    if (encoding.CodePage != targetFile.TextEncoding.CodePage) {
                        __.ShowWarnMsgBox("自動判別により文字コードを変更しました。");
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// プラグイン配置後の初期化処理を行います
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public void InitializePlaced() {
            //スプリットバーの位置を設定します
            splitContainer.HorizontalDistance = _property.ToInt("three_pane", "horizontal_distance", 150);
            splitContainer.VerticalDistance   = _property.ToInt("three_pane",   "vertical_distance", 150);
        }

        //ポップアップメニューを追加します
        private void AddPopupMenuItem() {

            var convertMenu = new ToolStripMenuItem { Text= "変換" };
            popupMenuForTextbox.Items.Add(convertMenu);
            {
                //小文字にします
                var menu = new ToolStripMenuItem { Text= "A → a" };
                menu.Click += (sender, e) => {
                    txtMain.SelectedText = txtMain.SelectedText.ToLower();
                };
                convertMenu.DropDownItems.Add(menu);
            }

            {
                //大文字にします
                var menu = new ToolStripMenuItem { Text= "a → A" };
                menu.Click += (sender, e) => {
                    txtMain.SelectedText = txtMain.SelectedText.ToUpper();
                };
                convertMenu.DropDownItems.Add(menu);
            }

            {
                //全角にします
                var menu = new ToolStripMenuItem { Text= "半角 → 全角" };
                menu.Click += (sender, e) => {
                    txtMain.SelectedText = StringUtils.HankakuToZenkaku(txtMain.SelectedText);
                };
                convertMenu.DropDownItems.Add(menu);
            }

            {
                //半角にします
                var menu = new ToolStripMenuItem { Text= "全角 → 半角" };
                menu.Click += (sender, e) => {
                    txtMain.SelectedText = StringUtils.ZenkakuToHankaku(txtMain.SelectedText);
                };
                convertMenu.DropDownItems.Add(menu);
            }

            {
                //10進数 -> 16進数
                var menu = new ToolStripMenuItem { Text= "10進数(255) → 16進数(FF)" };
                menu.Click += (sender, e) => {
                    var result = StringUtils.DecimalToHex(txtMain.SelectedText);
                    if (result != null) {
                        txtMain.SelectedText = result;
                    } else {
                        __.ShowErrorMsgBox("変換できませんでした。");
                    }
                };
                convertMenu.DropDownItems.Add(menu);
            }

            {
                //16進数 -> 10進数
                var menu = new ToolStripMenuItem { Text= "16進数(FF) → 10進数(255)" };
                menu.Click += (sender, e) => {
                    var result = StringUtils.HexToDecimal(txtMain.SelectedText);
                    if (result != null) {
                        txtMain.SelectedText = result;
                    } else {
                        __.ShowErrorMsgBox("変換できませんでした。");
                    }

                };
                convertMenu.DropDownItems.Add(menu);
            }

            {
                //16進数 → 文字列
                var menu = new ToolStripMenuItem { Text= "16進数(82 a0) → 文字列(あ)" };
                convertMenu.DropDownItems.Add(menu);
                menu.DropDownItems.Add(new HexToStrMenu("シフトJIS", txtMain, Encoding.GetEncoding(932)));
                menu.DropDownItems.Add(new HexToStrMenu("UTF-8",     txtMain, new UTF8Encoding(false)));
                menu.DropDownItems.Add(new HexToStrMenu("UTF-16",    txtMain, Encoding.Unicode));
                menu.DropDownItems.Add(new HexToStrMenu("EUC-JP",    txtMain, Encoding.GetEncoding(51932)));
            }

            {
                //文字列 -> 16進数
                var menu = new ToolStripMenuItem { Text= "文字列(あ) → 16進数(82 a0)" };
                convertMenu.DropDownItems.Add(menu);
                menu.DropDownItems.Add(new StrToHexMenu("シフトJIS", this, txtMain, Encoding.GetEncoding(932)));
                menu.DropDownItems.Add(new StrToHexMenu("UTF-8",     this, txtMain, new UTF8Encoding(false)));
                menu.DropDownItems.Add(new StrToHexMenu("UTF-16",    this, txtMain, Encoding.Unicode));
                menu.DropDownItems.Add(new StrToHexMenu("EUC-JP",    this, txtMain, Encoding.GetEncoding(51932)));
            }

            {
                //計算します
                var menu = new ToolStripMenuItem { Text= "計算します" };
                menu.Click += (sender, e) => {
                    var result = Calculator.Calc(txtMain.SelectedText);
                    txtMain.SelectionStart = txtMain.SelectionStart + txtMain.SelectedText.Length;
                    txtMain.SelectionLength = 0;
                    txtMain.SelectedText = " = " + result;
                };
                popupMenuForTextbox.Items.Add(menu);
            }
        }

        /// <summary>
        /// メニュー・ファイル・文字コードのメニューに、チェックをつけます
        /// </summary>
        private void CheckedMenu_MenuFile_Encoding(Encoding encoding) {
            menuFile_Encoding_ShiftJIS    .Checked = encoding.CodePage == 932;
            menuFile_Encoding_UTF8        .Checked = encoding.CodePage == 65001 && encoding.GetPreamble().Length == 0;
            menuFile_Encoding_UTF8_BOM    .Checked = encoding.CodePage == 65001 && encoding.GetPreamble().Length >  0;
            menuFile_Encoding_UTF16_LE_BOM.Checked = encoding.CodePage == 1200;
            menuFile_Encoding_UTF16_BE_BOM.Checked = encoding.CodePage == 1201;
            menuFile_Encoding_EucJp       .Checked = encoding.CodePage == 51932;
        }

        /// <summary>
        /// メニュー・ファイル・改行コードのメニューに、チェックをつけます
        /// </summary>
        private void CheckedMenu_MenuFile_NewLine(string newLineCode) {
            //string newLineCode = ((TextFile)_textFile).NewLineCode;
            menuFile_NewLine_CRLF.Checked = newLineCode.Equals("\r\n");
            menuFile_NewLine_CR  .Checked = newLineCode.Equals("\r");
            menuFile_NewLine_LF  .Checked = newLineCode.Equals("\n");
        }


        /// <summary>
        /// ステータスバーにタイトルを設定します
        /// </summary>
        private void SetTitle() {
            if (this.TargetFile == null) return;

            var title = new StringBuilder();

            //読み取り専用の場合は、(読み取り専用) をつける
            if (this.TargetFile.Path != null) {
                title.Append(this.TargetFile.IsReadOnly ? "(読み取り専用)" : "");
            }

            //パスが未設定の場合は「無題」にする
            title.Append(this.TargetFile.Path ?? "無題");

            //テキスト形式で開いている場合は、文字コードを追加します
            if (this.TargetFile is TextFile) {
                title.Append(" [");
                title.Append(((TextFile)this.TargetFile).TextEncoding.EncodingName);

                //BOMの有無を追加します
                title.Append(
                    ( ((TextFile)this.TargetFile).TextEncoding.GetPreamble().Length > 0 ) ? ":BOMあり" : ""
                );

                //改行コードを追加します
                if (((TextFile)this.TargetFile).NewLineCode.Length == 2) {
                    title.Append(":CRLF");
                } else {
                    title.Append(((TextFile)this.TargetFile).NewLineCode.Equals("\r") ? ":CR" : ":LF");
                }
                title.Append("]");
            }
            
            //テキスト形式で開いている場合は、バイナリ形式
            if (this.TargetFile is BinaryFile) {
                title.Append(" [バイナリ形式]");
            }

            //テキストが変更されている場合は、(*) をつける
            title.Append(txtMain.Modified ? "(*)" : "");
            if (txtMain.Modified) {
                statusPath.BackColor = Color.FromKnownColor(KnownColor.Pink);
            } else {
                statusPath.BackColor = Color.FromKnownColor(KnownColor.Control);
            }

            //キー操作の記録中
            if (_keyMacro.IsRecording) {
                title.Append(" 【 REC 】");
            }

            //タイトルを設定します 
            statusPath.Text = title.ToString(); //TODO ステータスバーに長い文字列を設定すると表示されない

            //ファイル名をプラグインのタイトルとして保持します（タブのタイトルになります）
            this.Title = Path.GetFileName(this.TargetFile.Path) + (txtMain.Modified ? "(*)" : "");

            //タイトル変更イベントを発生させます
            var param = new TitleChangedEventParam{ Plugin = this };
            _pluginManager.GetEventManager().RaiseEvent(TitleChangedEventParam.Name, this, param);
        }

        //文字列中の画像のパスを検出します。フルパスのみ対応。
        private string DetectImagePath(string text) {
            text = text.ToLower();
            var match = Regex.Match(text, @"[a-z]:(\\|/).*\.(jpg|jpeg|bmp|gif|png)");

            if (match.Success) return match.Value;
            return null;
        }

        //画像のファイル名を返します
        private string GetImgFileName()
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + ".png";
            return fileName;
        }


        /******************************************************************************
         * 
         *  テキストボックス
         * 
         ******************************************************************************/
        /// <summary>
        /// テキストボックスの Modified の Changedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_ModifiedChanged(object sender, EventArgs e)
        {
            //フォームのタイトルを設定します
            SetTitle();
        }

        /// <summary>
        /// テキストボックスの DragEnter の Changedイベント
        /// ドラッグしてテキストボックスに入った時に発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtMain_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        /// <summary>
        /// テキストボックスの DragDrop の Changedイベント
        /// テキストボックスにドロップされた時に発生します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtMain_DragDrop(object sender, DragEventArgs e)
        {
            //TODO _pluginManager に対して、プラグインの生成依頼を出す
            /*
	        if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
	            foreach (string path in (string[])e.Data.GetData(DataFormats.FileDrop)) {
                    try {
                        //テキストに変更がある場合は、ファイルの保存確認をして、ファイルを保存します
                        ConfirmAndSave();

                        //ファイルを開きます
                        OpenFileSub(path, new TextFile());
                    } catch (Exception) {
                    }
                    return;
	            }
	        }
            */
        }

        /// <summary>
        /// テキストボックスのKeyDownイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_KeyDown(object sender, KeyEventArgs e)
        {
            //F12キーが押された行に画像のパスが含まれる場合は、画像を表示します
            if (e.KeyCode == Keys.F12) {
                //カーソルがある行を取得します
                int lineNo = txtMain.GetLineFromCharIndex(txtMain.SelectionStart);

                //選択されている文字列のパスを開きます
                var selectedText = txtMain.SelectedText.Trim(' ', '\"', '\'', '\n');
                if (selectedText.Length > 0) {
                    _openByNative(selectedText);
                    return;
                }

                //画像パスを検出します
                var path = DetectImagePath(txtMain.Lines[lineNo].ToLower());
                if (path != null) {
                    _openByNative(path);
                    return;
                }
            }

            if (e.KeyCode == Keys.V && e.Control) {

                if (Clipboard.ContainsImage()) {
                    //クリップボードに画像がある場合

                    //作業フォルダがない場合は作成します
                    string dirPath = Path.Combine(__.GetAppDirPath(), "output");
                    if (Directory.Exists(dirPath) == false) Directory.CreateDirectory(dirPath);

                    //クリップボードに画像形式のデータが入っている場合
                    Image img = Clipboard.GetImage();
                    string fileName = GetImgFileName();
                    string path = Path.Combine(dirPath, fileName);

                    //画像をファイルに出力します
                    img.Save(path, ImageFormat.Png);

                    //パスを貼り付けます
                    txtMain.SelectedText = path + "\n";

                    e.Handled = true; //Ctrl+V を処理済みにします
                    return;
                }
            }

            //ファイルを開きます
            void _openByNative(string path) {
                try {
                    if (File.Exists(path) || Directory.Exists(path)) {
                        Process.Start(path);
                        e.Handled = true;
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        } //txtMain_KeyDown()

        /// <summary>
        /// テキストボックスがアクティブになった時に発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_Enter     (object sender, EventArgs e) { _pluginManager.ActivePlugin = this; }
        private void txtSplit_Enter    (object sender, EventArgs e) { _pluginManager.ActivePlugin = this; }
        private void titleListbox_Enter(object sender, EventArgs e) { _pluginManager.ActivePlugin = this; }

        /******************************************************************************
         * 
         *  ポップアップメニュー
         * 
         ******************************************************************************/ 
        private void popupMenuForTextbox_Cut_Click  (object sender, EventArgs e) { Cut();   }
        private void popupMenuForTextbox_Copy_Click (object sender, EventArgs e) { Copy();  }
        private void popupMenuForTextbox_Paste_Click(object sender, EventArgs e) { Paste(); }
        private void popupMenuForTextbox_SetKeyMacro_Click(object sender, EventArgs e) { SetMacro(); }

        /// <summary>
        /// メニュー・文字コード の Clickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFile_Encoding_Sub(object sender, EventArgs e)
        {
            if ((this.TargetFile is TextFile) == false) {
                __.ShowErrorMsgBox("テキスト形式で開いている場合のみ、文字コードを指定できます。");
                return;
            }

            //エンコーディングを設定します
            Encoding encoding = null;
            if (menuFile_Encoding_ShiftJIS     == sender) encoding = Encoding.GetEncoding(932);   //Shift JIS
            if (menuFile_Encoding_UTF8         == sender) encoding = new UTF8Encoding(false);     //UTF-8 BOMなし
            if (menuFile_Encoding_UTF8_BOM     == sender) encoding = new UTF8Encoding(true);      //UTF-8 BOMあり
            if (menuFile_Encoding_UTF16_LE_BOM == sender) encoding = Encoding.GetEncoding(1200);  //UTF-16 LE
            if (menuFile_Encoding_UTF16_BE_BOM == sender) encoding = Encoding.GetEncoding(1201);  //UTF-16 BE
            if (menuFile_Encoding_EucJp        == sender) encoding = Encoding.GetEncoding(51932); //EUC-JP
            ((TextFile)this.TargetFile).TextEncoding = encoding;

            //テキストを変更ありにします
            txtMain.Modified = true;

            //フォームのタイトルを設定します
            SetTitle();

            //メニュー・文字コードのメニューに、チェックをつけます
            CheckedMenu_MenuFile_Encoding(encoding);
        }

        /// <summary>
        /// メニュー・改行コード の Clickイベント
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFile_NewLine_Sub(object sender, EventArgs e)
        {
            if ((this.TargetFile is TextFile) == false) {
                __.ShowErrorMsgBox("テキスト形式で開いている場合のみ、改行コードを指定できます。");
                return;
            }

            //改行コードを設定します
            string newLineCode = null;
            if (menuFile_NewLine_CRLF == sender) newLineCode = "\r\n";
            if (menuFile_NewLine_CR   == sender) newLineCode = "\r";
            if (menuFile_NewLine_LF   == sender) newLineCode = "\n";

            ((TextFile)this.TargetFile).NewLineCode = newLineCode;

            //テキストを変更ありにします
            txtMain.Modified = true;

            //タイトルを設定します
            SetTitle();

            //メニュー・改行コードのメニューに、チェックをつけます
            CheckedMenu_MenuFile_NewLine(newLineCode);
        }

        /// <summary>
        /// 上下分割メニューのクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popupMenuForTextbox_Split_Click(object sender, EventArgs e)
        {
            if (splitContainer.PanelTopVisible) {
                //上下分割している場合、上下分割を解除します
                splitContainer.PanelTopVisible = false;

                //上下分割メニューのチェックをはずします
                popupMenuForTextbox_Split.Checked = false;
               
            } else {
                //上下分割していない場合、上下分割します
                splitContainer.PanelTopVisible = true;

                //メインのテキストを、参照用テキストにコピーします
                txtSplit.Text = txtMain.Text;

                //上下分割メニューのチェックをつけます
                popupMenuForTextbox_Split.Checked = true;
            }
        }

        /// <summary>
        /// 見出しリストメニューのクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popupMenuForTextbox_TitleList_Click(object sender, EventArgs e)
        {
            if (splitContainer.PanelLeftVisible) {
                //見出しリストを表示している場合、見出しリストを非表示にします
                splitContainer.PanelLeftVisible = false;

                //見出しリストメニューのチェックをはずします
                popupMenuForTextbox_TitleList.Checked = false;
               
            } else {
                //見出しリストが非表示の場合、見出しリストを表示します
                splitContainer.PanelLeftVisible = true;

                //見出しリストを更新します
                titleListbox.SetTitleList();

                //見出しリストメニューのチェックをつけます
                popupMenuForTextbox_TitleList.Checked = true;
            }
        }

        /******************************************************************************
         * 
         *  IPlugin
         * 
         ******************************************************************************/ 

        
        public string     PluginId       { get; set; }    //プラグインID
        public IPlugin    ParentPlugin   { get; set; }    //親プラグイン
        public IComponent GetComponent() { return this; } //プラグインのコンポーネントを返します
        public string     Title          { get; set; }    //プラグインのタイトルを返します

        /// <summary>
        /// プラグインが終了できるかどうか
        /// </summary>
        /// <returns>true:終了できる  false:終了できない</returns>
        public bool CanClosePlugin() {
            if (txtMain.Modified) {
                //テキストに変更がある場合、保存の確認を行います
                var result = MsgBoxUtils.ShowMsgBoxToConfirmSaving();
                if (result == DialogResult.Yes) {
                    //ファイルを保存します
                    SaveFile();
                }
                if (result == DialogResult.Cancel) {
                    //終了できないことを返します
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// プラグインの終了処理
        /// </summary>
        public void ClosePlugin() {

            //スプリットバーの位置を取得します
            _property["three_pane", "horizontal_distance"] = splitContainer.HorizontalDistance.ToString();
            _property["three_pane",   "vertical_distance"] = splitContainer.VerticalDistance.ToString();

            //プロパティを保存します
            _property.Save();

            //コントロールを削除します
            this.Parent = null;
            this.Dispose();

        }

        /// <summary>
        /// フォーカスを設定します
        /// </summary>
        public void SetFocus() {
            txtMain.Focus();

            //アクティブプラグインにします
            _pluginManager.ActivePlugin = this;
        }

        /******************************************************************************
         * 
         *  IEditPlugin
         * 
         ******************************************************************************/ 
        public void Cut() {
            txtMain.Cut();
            if (_keyMacro.IsRecording) _keyMacro.AddKey("^{X}"); //キー操作の記録
            //メニューのショートカットキーにキーイベントを奪われるため、
            //キー操作の記録しています
        }
        public void Copy() {
            txtMain.Copy();
            if (_keyMacro.IsRecording) _keyMacro.AddKey("^{C}"); //キー操作の記録
        }
        public void Paste(){
            txtMain.Paste();
            if (_keyMacro.IsRecording) _keyMacro.AddKey("^{V}"); //キー操作の記録
        }
        public void Delete() {
            txtMain.Delete();
            if (_keyMacro.IsRecording) _keyMacro.AddKey("{DEL}"); //キー操作の記録
        }
        public void Undo() {
            txtMain.Undo();
            if (_keyMacro.IsRecording) _keyMacro.AddKey("^{Z}"); //キー操作の記録
        }
        public void Redo(){
            txtMain.Redo();
            if (_keyMacro.IsRecording) _keyMacro.AddKey("^{Y}"); //キー操作の記録
        }
        
        /******************************************************************************
         * 
         *  ISearchPlugin
         * 
         ******************************************************************************/ 

        /// <summary>
        /// 検索する文字列を返します
        /// 検索する際に、現在選択中の文字列を返すのに使用
        /// </summary>
        /// <returns>検索する文字列</returns>
        public string GetSearchString() {
            return txtMain.SelectedText;
        }

        /// <summary>
        /// 前方検索します。テキストの末尾へ向けて検索します。
        /// </summary>
        /// <returns>見つかった位置</returns>
        public int SearchForward()
        {
            //キー操作の記録
            if (_keyMacro.IsRecording) _keyMacro.AddKey("{F3}");

            //フォーカス設定
            txtMain.Focus();

            //検索する
            int index = txtMain.SearchForward(this.GetSearchData().SearchString, this.GetSearchData().IgnoreCase);
            return index;
        }

        /// <summary>
        /// 後方検索します。テキストの先頭へ向けて検索します。
        /// </summary>
        /// <returns>見つかった位置</returns>
        public int SearchBackward()
        {
            //キー操作の記録
            if (_keyMacro.IsRecording) _keyMacro.AddKey("+{F3}");

            //フォーカス設定
            txtMain.Focus();

            //検索する
            int index = txtMain.SearchBackward(this.GetSearchData().SearchString, this.GetSearchData().IgnoreCase);
            return index;
        }

        /// <summary>
        /// 前方置換します。
        /// </summary>
        /// <returns>見つかった位置</returns>
        public int ReplaceForward()
        {
            //キー操作の記録
            if (_keyMacro.IsRecording) _keyMacro.AddKey("^{F3}");

            //フォーカス設定
            txtMain.Focus();

            //前方置換します
            int index = txtMain.ReplaceForward(this.GetSearchData().SearchString, this.GetSearchData().ReplaceString, this.GetSearchData().IgnoreCase);
            return index;
        }

        /// <summary>
        /// 後方置換します。
        /// </summary>
        /// <returns>見つかった位置</returns>
        public int ReplaceBackward()
        {
            //キー操作の記録
            if (_keyMacro.IsRecording) _keyMacro.AddKey("+^{F3}");

            //フォーカス設定
            txtMain.Focus();

            //後方置換します
            int index = txtMain.ReplaceBackward(this.GetSearchData().SearchString, this.GetSearchData().ReplaceString, this.GetSearchData().IgnoreCase);
            return index;
        }

        /// <summary>
        /// 全置換します。
        /// </summary>
        public void ReplaceAll()
        {
            txtMain.ReplaceAll(
                this.GetSearchData().SearchString, 
                this.GetSearchData().ReplaceString, 
                this.GetSearchData().IgnoreCase);
        }

        /******************************************************************************
         * 
         *  INewLinePlugin
         * 
         ******************************************************************************/ 

        /// <summary>
        /// ファイルを開きます
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool LoadFile(IFile file) {

            //ファイルの種類が指定されていない場合には、テキストファイルで再設定します
            if (file is AnyFile) {
                file = new TextFile{ Path = file.Path};
            }

            this.TargetFile = file;
            this.TargetFile.Load();
            if (this.TargetFile is TextFile)   txtMain.Text = ((TextFile)  this.TargetFile).Text;
            if (this.TargetFile is BinaryFile) txtMain.Text = ((BinaryFile)this.TargetFile).Text;

            //変更なしにします
            txtMain.Modified = false;

            //フォームのタイトルを設定します
            SetTitle();

            
            //メニューの設定
            if (this.TargetFile is TextFile textFile) {
                //メニュー・文字コードのメニューに、チェックをつけます
                CheckedMenu_MenuFile_Encoding(textFile.TextEncoding);

                //メニュー・改行コードのメニューに、チェックをつけます
                CheckedMenu_MenuFile_NewLine(((TextFile)this.TargetFile).NewLineCode);
            }
            

            return true;
        }

        /// <summary>
        /// ファイルを保存します。
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool SaveFile(IFile file = null) {

            //ファイルが指定されている場合は、ファイルを保持します
            if (file != null) this.TargetFile = file;

            //ファイルを保存します
            if (this.TargetFile is TextFile)   ((TextFile)  this.TargetFile).Text = txtMain.Text;
            if (this.TargetFile is BinaryFile) ((BinaryFile)this.TargetFile).Text = txtMain.Text;
            this.TargetFile.Save();

            //読み込み専用を解除します
            this.TargetFile.IsReadOnly = false;

            //変更なしにします
            txtMain.Modified = false;

            //フォームのタイトルを設定します
            SetTitle();

            return true;
        }

        /******************************************************************************
         * 
         *  INewLinePlugin
         * 
         ******************************************************************************/ 

        /// <summary>
        /// 改行コード
        /// </summary>
        public string NewLineCode { get; set; }



        /******************************************************************************
         * 
         *  IKeyMacroPlugin
         * 
         ******************************************************************************/ 

        //キー操作の記録を開始します
        public void StartRecording() {
            _keyMacro.StartRecording();

            //タイトルを設定します
            SetTitle();
        }

        //キー操作の記録を停止します
        public void StopRecording() {
            _keyMacro.StopRecording();

            //タイトルを設定します
            SetTitle();
        }

        //キー操作を再生します
        public void Play() {
            _keyMacro.Play();
        }

        //記録中かどうか？
        public bool IsRecording() {
            return _keyMacro.IsRecording;
        }

        ////記録しているマクロを出力します
        public void OutputMacro() {
            var sb = new StringBuilder();
            foreach (var key in _keyMacro.KeyList) {
                sb.Append(key + "\n");
            }
            txtMain.SelectionLength = 0;
            txtMain.SelectedText = sb.ToString();
        }
        
        //マクロを設定します
        public void SetMacro() {
            if (txtMain.SelectionLength == 0) return;
            _keyMacro.KeyList = txtMain.SelectedText.Split('\n').ToList();
        }

        /******************************************************************************
         * 
         *  IRefreshPlugin
         * 
         ******************************************************************************/ 

        /// <summary>
        /// 最新の情報に更新します
        /// </summary>
        public void RefreshData()
        {
            //見出しリストを更新します
            titleListbox.SetTitleList();

            //メインのテキストを、参照用テキストにコピーします
            txtSplit.Text = txtMain.Text;
        }

    } //class
}
