using NineCubed.Common.Files;
using NineCubed.Common.Utils;
using NineCubed.Memo.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// テキストファイルデータ
        /// </summary>
        private TextFile _textFile { get; set; }

        /// <summary>
        /// テキストボックスの左側の余白のサイズ
        /// </summary>
        private const int TextBoxMarginLeft = 6;

        /// <summary>
        /// Config
        /// </summary>
        private AppConfig _config;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            //Configを読み込みます
            LoadConfig();
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
                _textFile = new TextFile();
                try {
                    OpenFileSub(args[1], null);
                } catch (Exception ex) {
                    //例外が発生した場合は、アプリを終了させます
                    ShowErrorMsgBox(ex);
                    this.Dispose();
                    return;
                }

            } else {
                //コマンドライン引数がない場合(通常起動)
          
                //新規作成時の状態にします
                CreateNewFile();
            }

            //テキストボックスを初期化します
            InitTextBox(txtMain);

            //隙間がなくなるようにテキストボックスを配置します
            txtMain.Dock = DockStyle.Fill;

            //ダイアログの初期設定
            openFileDialog.Filter = 
                "テキストファイル(*.txt)|*.txt" + "|" + 
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
                //テキストファイルを閉じます
                CloseFile();
            } catch (CancelException) {
                //キャンセルされた場合、フォームが閉じるのを中止します
                e.Cancel = true;
            } catch (Exception ex) {
                e.Cancel = true;
                ShowErrorMsgBox(ex);
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
            //Configを保存します
            SaveConfig();
        }

        /// <summary>
        /// テキストボックスを初期化します
        /// </summary>
        /// <param name="textBox">初期化するテキストボックス</param>
        private void InitTextBox(RichTextBox textBox, string fontName = "ＭＳ ゴシック", float fontSize = 12, int tabSize = 4)
        {
            //折り返さないようにします
            textBox.WordWrap = false;

            //タブを入力できるようにします。false にすると、別のコントロールにフォーカスが移動してしまい、タブを入力できません
            textBox.AcceptsTab = true;

            //文字の幅が同じするため、等幅フォントの「ＭＳ ゴシック」にします
            textBox.Font = new Font(fontName, fontSize);

            //デフォルトでは、半角英数字は英語フォント、それ以外は日本語フォントを使う DualFont になっているため、AutoFont だけを再設定します
            //この設定をしないと、半角の「i」の幅がものすごく狭くなってしまう
            textBox.LanguageOption = RichTextBoxLanguageOptions.AutoFont;

            //テキストボックスの左の内側に余白を設定します
            textBox.SelectionIndent = TextBoxMarginLeft;

            //左の余白がリセットされる不具合があるため、コメントアウトしました
            //URLが入力された時に書式が変わらないようにする
            //textBox.DetectUrls = false; //true にすると、LinkClickedイベントでクリックされたURL(e.LinkText)が取得できる

            //タブの位置を設定します
            SetSelectionTabs(textBox, tabSize);

            //変更なしにします
            textBox.Modified = false;
        }

        /// <summary>
        /// テキストボックスにタブの停止位置を設定します
        /// 
        /// タブの停止位置は、1タブ＝半角スペース4個分、などのように指定できません
        /// 実際に描画される1文字分の幅を取得して、そこからタブが止まる位置を計算して設定しています
        /// </summary>
        /// <param name="textBox">テキストボックス</param>
        /// <param name="tabSize">1タブあたりの文字数</param>
        private void SetSelectionTabs(RichTextBox textBox, int tabSize)
        {
            //1文字の幅のサイズを取得します
            int charWidth = 0;
            using (Graphics g = this.CreateGraphics()) {
                Size size = TextRenderer.MeasureText(g, "A", textBox.Font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding);
                charWidth = size.Width;
            }

            //タブで止まる位置を配列に設定します
            int[] tabArray = new int[32]; //32より多く設定するとエラーとなる
            for (int i = 0; i < tabArray.Count(); i++)
            {
                tabArray[i] = (charWidth * tabSize * (i + 1) + textBox.SelectionIndent); //textBox.SelectionIndent はテキスト内の左側の余白サイズ
            }

            //タブで止まる位置を設定します
            textBox.SelectionTabs = tabArray;
        }

        /// <summary>
        /// Configを読み込みます
        /// </summary>
        private void LoadConfig() {
            _config = AppConfig.Load();
            if (_config != null) {
                //Configが読み込めた場合

                //フォームの位置とサイズを設定します
                this.StartPosition = FormStartPosition.Manual;
                this.Left   = _config.form_left;
                this.Top    = _config.form_top;
                this.Width  = _config.form_width;
                this.Height = _config.form_height;
            } else {
                //Configが読み込めなかった場合

                //デフォルト値を設定します
                _config = new AppConfig();

                //フォームを中央に表示します
                var screenBounds = Screen.PrimaryScreen.Bounds;
                this.Width  = (int)(screenBounds.Width  * 0.6);
                this.Height = (int)(screenBounds.Height * 0.6);
                this.StartPosition = FormStartPosition.CenterScreen;
            }
        }

        /// <summary>
        /// Configを保存します
        /// </summary>
        private void SaveConfig() {
            //ウィンドウが最小化、最大化されている場合は、標準に戻します
            //(Configに保存する際のサイズがわからないため)
            if (this.WindowState != FormWindowState.Normal) {
                this.WindowState = FormWindowState.Normal;
            }

            //Configに現在の状態を設定します
            _config.form_left   = this.Left;
            _config.form_top    = this.Top;
            _config.form_width  = this.Width;
            _config.form_height = this.Height;

            //Configを保存します
            _config.Save();
        }

        /// <summary>
        /// メニュー・ヘルプ・バージョン情報 の Clickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuHelp_version_Click(object sender, EventArgs e)
        {
            //バージョン情報を取得します
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            string version =
                fileVersionInfo.ProductName    + " "  + //製品名
                fileVersionInfo.ProductVersion + "\n" + //製品バージョン
                fileVersionInfo.LegalCopyright;         //コピーライト

            //バージョン情報を表示します
            MessageBox.Show(version);
        }

        /// <summary>
        /// メニュー・ファイル・新規作成 の Clickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFile_New_Click(object sender, EventArgs e)
        {
            try {
                //テキストファイルを新規作成します
                CreateNewFile();
            } catch (CancelException) {
                //キャンセル時
            } catch (Exception ex) {
                ShowErrorMsgBox(ex);
            }
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
                OpenFile();
            } catch (CancelException) {
                //キャンセル時
            } catch (Exception ex) {
                ShowErrorMsgBox(ex);
            }
        }

        /// <summary>
        /// メニュー・ファイル・開く（文字コード指定） の Clickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFile_Open_Encoding_Sub_Click(object sender, EventArgs e)
        {
            Encoding encoding = null;
            if (menuFile_Open_ShiftJIS     == sender) encoding = Encoding.GetEncoding(932);   //Shift JIS
            if (menuFile_Open_UTF8         == sender) encoding = new UTF8Encoding(false);     //UTF-8 BOMなし
            if (menuFile_Open_UTF8_BOM     == sender) encoding = new UTF8Encoding(true);      //UTF-8 BOMあり
            if (menuFile_Open_UTF16_LE_BOM == sender) encoding = Encoding.GetEncoding(1200);  //UTF-16 LE
            if (menuFile_Open_UTF16_BE_BOM == sender) encoding = Encoding.GetEncoding(1201);  //UTF-16 BE
            if (menuFile_Open_EucJp        == sender) encoding = Encoding.GetEncoding(51932); //EUC-JP

            try {
                //テキストファイルを開きます
                OpenFile(encoding);

                //BOMにより自動的に文字コードが変更された場合は、警告を表示します
                if (encoding.CodePage != _textFile.TextEncoding.CodePage) {
                    MessageBox.Show("自動判別により文字コードを変更しました。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            } catch (CancelException) {
                //キャンセル時
            } catch (Exception ex) {
                ShowErrorMsgBox(ex);
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
                ShowErrorMsgBox(ex);
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
                ShowErrorMsgBox(ex);
            }
        }

        /// <summary>
        /// メニュー・ファイル・文字コード の Clickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFile_Encoding_Sub(object sender, EventArgs e)
        {
            //エンコーディングを設定します
            Encoding encoding = null;
            if (menuFile_Encoding_ShiftJIS     == sender) encoding = Encoding.GetEncoding(932);   //Shift JIS
            if (menuFile_Encoding_UTF8         == sender) encoding = new UTF8Encoding(false);     //UTF-8 BOMなし
            if (menuFile_Encoding_UTF8_BOM     == sender) encoding = new UTF8Encoding(true);      //UTF-8 BOMあり
            if (menuFile_Encoding_UTF16_LE_BOM == sender) encoding = Encoding.GetEncoding(1200);  //UTF-16 LE
            if (menuFile_Encoding_UTF16_BE_BOM == sender) encoding = Encoding.GetEncoding(1201);  //UTF-16 BE
            if (menuFile_Encoding_EucJp        == sender) encoding = Encoding.GetEncoding(51932); //EUC-JP
            _textFile.TextEncoding = encoding;

            //テキストを変更ありにします
            txtMain.Modified = true;

            //フォームのタイトルを設定します
            SetFormTitle();

            //メニュー・文字コードのメニューに、チェックをつけます
            CheckedMenu_MenuFile_Encoding();
        }

        /// <summary>
        /// メニュー・ファイル・改行コード の Clickイベント
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFile_NewLine_Sub(object sender, EventArgs e)
        {
            //改行コードを設定します
            string newLineCode = null;
            if (menuFile_NewLine_CRLF == sender) newLineCode = "\r\n";
            if (menuFile_NewLine_CR   == sender) newLineCode = "\r";
            if (menuFile_NewLine_LF   == sender) newLineCode = "\n";
            _textFile.NewLineCode = newLineCode;

            //テキストを変更ありにします
            txtMain.Modified = true;

            //フォームのタイトルを設定します
            SetFormTitle();

            //メニュー・改行コードのメニューに、チェックをつけます
            CheckedMenu_MenuFile_NewLine();
        }

        //メニュー・編集の Clickイベント
        private void menuEdit_Undo_Click  (object sender, EventArgs e) { txtMain.Undo(); }
        private void menuEdit_Redo_Click  (object sender, EventArgs e) { txtMain.Redo(); }
        private void menuEdit_Cut_Click   (object sender, EventArgs e) { txtMain.Cut(); }
        private void menuEdit_Copy_Click  (object sender, EventArgs e) { txtMain.Copy(); }
        private void menuEdit_Paste_Click (object sender, EventArgs e) { txtMain.Paste(); }
        private void menuEdit_Delete_Click(object sender, EventArgs e) { txtMain.SelectedText = ""; }

        /// <summary>
        /// ツールバー・ファイル・新規作成 の Clickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolFile_New_Click(object sender, EventArgs e)
        {
            menuFile_New_Click(sender, e);
        }

        /// <summary>
        /// ツールバー・ファイル・開く の Clickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolFile_Open_Click(object sender, EventArgs e)
        {
            menuFile_Open_Click(sender, e);
        }

        /// <summary>
        /// ツールバー・ファイル・保存 の Clickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolFile_Save_Click(object sender, EventArgs e)
        {
            menuFile_Save_Click(sender, e);
        }

        /// <summary>
        /// テキストボックスの Modified の Changedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_ModifiedChanged(object sender, EventArgs e)
        {
            //フォームのタイトルを設定します
            SetFormTitle();
        }

        /// <summary>
        /// テキストファイルを新規作成します
        /// </summary>
        private void CreateNewFile()
        {
            //テキストに変更がある場合は、ファイルの保存確認をして、ファイルを保存します
            ConfirmAndSave();

            {
                //テキストボックスのテキストを初期化します
                txtMain.Clear(); //Modified は false になる

                //変更なしにします
                txtMain.Modified = false;

                //テキストファイルを生成して設定します
                _textFile = new TextFile();

                //文字コードをシフトJISにします
                _textFile.SetEncodingShiftJIS();

                //改行コードは \r\n にします
                _textFile.NewLineCode = "\r\n";

                //フォームのタイトルを設定します
                SetFormTitle();

                //メニュー・文字コードのメニューに、チェックをつけます
                CheckedMenu_MenuFile_Encoding();

                //メニュー・改行コードのメニューに、チェックをつけます
                CheckedMenu_MenuFile_NewLine();
            }
        }

        /// <summary>
        /// テキストファイルを開きます
        /// 保存確認と開くダイアログの表示を行います。
        /// </summary>
        /// <param name="encoding"></param>
        private void OpenFile(Encoding encoding = null)
        {
            //テキストに変更がある場合は、ファイルの保存確認をして、ファイルを保存します
            ConfirmAndSave();

            {
                //開くダイアログを表示します
                openFileDialog.FileName = "";
                var dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK) {
                    //「OK」の場合

                    //ファイルの読み込み
                    string path = openFileDialog.FileName;
                    OpenFileSub(path, encoding);
                }
            }
        }

        /// <summary>
        /// テキストファイルを開きます
        /// 保存確認と開くダイアログの表示は行いません
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        private void OpenFileSub(string path, Encoding encoding = null) {
            _textFile.TextEncoding = encoding; //文字コード
            _textFile.NewLineCode = null;      //改行コード(自動判別)
            _textFile.Load(path);
            txtMain.Text = _textFile.Text;

            //変更なしにします
            txtMain.Modified = false;

            //フォームのタイトルを設定します
            SetFormTitle();

            //メニュー・文字コードのメニューに、チェックをつけます
            CheckedMenu_MenuFile_Encoding();

            //メニュー・改行コードのメニューに、チェックをつけます
            CheckedMenu_MenuFile_NewLine();
        }

        /// <summary>
        /// テキストファイルを閉じます
        /// </summary>
        private void CloseFile()
        {
            //テキストに変更がある場合は、ファイルの保存確認をして、ファイルを保存します
            ConfirmAndSave();
        }

        /// <summary>
        /// ファイルを必要に応じて保存します
        /// テキストに変更がある場合は、ファイルの保存確認をして、「はい」の場合にはファイルを保存します
        /// </summary>
        /// <returns></returns>
        private bool ConfirmAndSave()
        {
            if (txtMain.Modified)
            {
                //ファイルの保存確認をします
                var dialogResult = ShowConfirmSavingMsgBox();
                if (dialogResult == DialogResult.Cancel) throw new CancelException();
                if (dialogResult == DialogResult.Yes)
                {
                    //「はい」の場合、ファイルを保存します
                    SaveFile();
                }
            }
            return true;
        }

        /// <summary>
        /// テキストファイルを保存します
        /// </summary>
        /// <param name="showDialog">true:ファイル選択ダイアログを表示します</param>
        private void SaveFile(bool showDialog = false)
        {
            string path = _textFile.Path;

            //パスが未設定、ダイアログを表示、読み取り専用、の場合は、保存ダイアログを表示します
            if (path == null || showDialog == true || _textFile.IsReadOnly)
            {
                //パスが未設定の場合
                //保存ダイアログを表示します
                var result = saveFileDialog.ShowDialog();
                if (result == DialogResult.Cancel) throw new CancelException();
                if (result == DialogResult.No) return;

                //選択されたファイルのパスを保持します
                path = saveFileDialog.FileName;
            }

            //ファイルを保存します
            _textFile.Text = txtMain.Text;
            _textFile.Save(path);

            //読み込み専用を解除します
            _textFile.IsReadOnly = false;

            //変更なしにします
            txtMain.Modified = false;

            //フォームのタイトルを設定します
            SetFormTitle();
        }

        /// <summary>
        /// エラーメッセージを表示します
        /// </summary>
        /// <param name="e">発生した例外</param>
        private void ShowErrorMsgBox(Exception e)
        {
            MessageBox.Show("エラーが発生しました。\n" + e.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 保存確認メッセージを表示します
        /// </summary>
        /// <returns>押されたボタン</returns>
        private DialogResult ShowConfirmSavingMsgBox() {
            var dialogResult = MessageBox.Show("変更されています。保存しますか？", "確認", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            return dialogResult;
        }

        /// <summary>
        /// フォームのタイトルを設定します
        /// </summary>
        private void SetFormTitle() {
            
            var title = new StringBuilder();

            //読み取り専用の場合は、(読み取り専用) をつける
            if (_textFile.Path != null) {
                title.Append(_textFile.IsReadOnly ? "(読み取り専用)" : "");
            }

            //パスが未設定の場合は「無題」にする
            title.Append(_textFile.Path ?? "無題");

            //文字コードを追加します
            title.Append(" [" + _textFile.TextEncoding.EncodingName);

            //BOMの有無を追加します
            title.Append( ((_textFile.TextEncoding.GetPreamble().Length > 0) ? ":BOMあり" : ""));

            //改行コードを追加します
            if (_textFile.NewLineCode.Length == 2) {
                title.Append(":CRLF");
            } else {
                title.Append(_textFile.NewLineCode.Equals("\r") ? ":CR" : ":LF");
            }
            title.Append("]");
            
            //テキストが変更されている場合は、(*) をつける
            title.Append(txtMain.Modified ? "(*)" : "");

            //フォームのタイトルを設定します
            this.Text = title.ToString();
        }

        /// <summary>
        /// メニュー・ファイル・文字コードのメニューに、チェックをつけます
        /// </summary>
        private void CheckedMenu_MenuFile_Encoding() {
            Encoding encoding = _textFile.TextEncoding;
            menuFile_Encoding_ShiftJIS.Checked     = encoding.CodePage == 932;
            menuFile_Encoding_UTF8.Checked         = encoding.CodePage == 65001 && encoding.GetPreamble().Length == 0;
            menuFile_Encoding_UTF8_BOM.Checked     = encoding.CodePage == 65001 && encoding.GetPreamble().Length >  0;
            menuFile_Encoding_UTF16_LE_BOM.Checked = encoding.CodePage == 1200;
            menuFile_Encoding_UTF16_BE_BOM.Checked = encoding.CodePage == 1201;
            menuFile_Encoding_EucJp.Checked        = encoding.CodePage == 51932;
        }

        /// <summary>
        /// メニュー・ファイル・改行コードのメニューに、チェックをつけます
        /// </summary>
        private void CheckedMenu_MenuFile_NewLine() {
            string newLineCode = _textFile.NewLineCode;
            menuFile_NewLine_CRLF.Checked = newLineCode.Equals("\r\n");
            menuFile_NewLine_CR.Checked   = newLineCode.Equals("\r");
            menuFile_NewLine_LF.Checked   = newLineCode.Equals("\n");
        }

        /// <summary>
        /// ポップアップメニュー・切り取り の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popupMenuForTextbox_Cut_Click(object sender, EventArgs e)
        {
            txtMain.Cut();
        }

        /// <summary>
        /// ポップアップメニュー・コピー の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popupMenuForTextbox_Copy_Click(object sender, EventArgs e)
        {
            txtMain.Copy();
        }

        /// <summary>
        /// ポップアップメニュー・貼り付け の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popupMenuForTextbox_Paste_Click(object sender, EventArgs e)
        {
            txtMain.Paste();
        }

    } //class
}
