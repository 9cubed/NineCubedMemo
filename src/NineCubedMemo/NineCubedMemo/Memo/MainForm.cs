using NineCubed.Common.Controls;
using NineCubed.Common.Files;
using NineCubed.Common.Utils;
using NineCubed.Memo.Exceptions;
using NineCubed.Memo.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo
{
    public partial class MainForm : Form, ISearchString
    {
        /// <summary>
        /// テキストファイルデータ
        /// </summary>
        private TextFile _textFile { get; set; }

        /// <summary>
        /// Config
        /// </summary>
        private AppConfig _config;

        /// <summary>
        /// 検索条件
        /// </summary>
        private SearchData _searchData = new SearchData();

        /// <summary>
        /// 検索条件
        /// </summary>
        public SearchData GetSearchData() {
            return _searchData;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            //Configを読み込みます
            _config = AppConfig.Load(this);
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
            txtMain.Initialize(_config.memo_font_name, _config.memo_font_size);

            //テキストボックスのドラッグ＆ドロップ対応 
            txtMain.AllowDrop = true; //D&Dを許可します
            txtMain.DragEnter += TxtMain_DragEnter;
            txtMain.DragDrop  += TxtMain_DragDrop;
            //(設定したのを忘れなければプロパティで設定しても問題ありません)
            
            //隙間がなくなるようにテキストボックスを配置します
            txtMain.Dock = DockStyle.Fill;

            //ダイアログの初期設定
            openFileDialog.Filter = 
                "テキストファイル(*.txt)|*.txt" + "|" + 
                "すべてのファイル(*.*)|*.*";
            saveFileDialog.Filter = openFileDialog.Filter;

            //ポップアップメニューを追加します
            addPopupMenuItem();
        }

        //ポップアップメニューを追加します
        private void addPopupMenuItem() {

            {
                //小文字にします
                var menu = new ToolStripMenuItem { Text= "A → a" };
                menu.Click += (sender, e) => {
                    txtMain.SelectedText = txtMain.SelectedText.ToLower();
                };
                popupMenuForTextbox.Items.Add(menu);
            }

            {
                //大文字にします
                var menu = new ToolStripMenuItem { Text= "a → A" };
                menu.Click += (sender, e) => {
                    txtMain.SelectedText = txtMain.SelectedText.ToUpper();
                };
                popupMenuForTextbox.Items.Add(menu);
            }

            {
                //全角にします
                var menu = new ToolStripMenuItem { Text= "半角 → 全角" };
                menu.Click += (sender, e) => {
                    txtMain.SelectedText = StringUtils.HankakuToZenkaku(txtMain.SelectedText);
                };
                popupMenuForTextbox.Items.Add(menu);
            }

            {
                //半角にします
                var menu = new ToolStripMenuItem { Text= "全角 → 半角" };
                menu.Click += (sender, e) => {
                    txtMain.SelectedText = StringUtils.ZenkakuToHankaku(txtMain.SelectedText);
                };
                popupMenuForTextbox.Items.Add(menu);
            }
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
            try {
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
        private void menuEdit_Undo_Click  (object sender, EventArgs e) { txtMain.Undo();   }
        private void menuEdit_Redo_Click  (object sender, EventArgs e) { txtMain.Redo();   }
        private void menuEdit_Cut_Click   (object sender, EventArgs e) { txtMain.Cut();    }
        private void menuEdit_Copy_Click  (object sender, EventArgs e) { txtMain.Copy();   }
        private void menuEdit_Paste_Click (object sender, EventArgs e) { txtMain.Paste();  }
        private void menuEdit_Delete_Click(object sender, EventArgs e) { txtMain.Delete(); }

        /// <summary>
        /// メニュー・検索・検索 の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSearch_Search_Click(object sender, EventArgs e)
        {
            //検索画面を表示します
            string searchString = (txtMain.SelectedText.Length > 0) ? txtMain.SelectedText : _searchData.SearchString;
            _searchData.SearchString = searchString;
            SearchForm.ShowForm(this, _searchData);
        }

        /// <summary>
        /// メニュー・検索・前方検索 の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSearch_SearchForward_Click(object sender, EventArgs e)
        {
            SearchForward();
        }

        /// <summary>
        /// メニュー・検索・後方検索 の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSearch_SearchBackward_Click(object sender, EventArgs e)
        {
            SearchBackward();
        }

        /// <summary>
        /// メニュー・検索・前方置換 の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSearch_ReplaceForward_Click(object sender, EventArgs e)
        {
            ReplaceForward();
        }

        /// <summary>
        /// メニュー・検索・後方置換 の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSearch_ReplaceBackward_Click(object sender, EventArgs e)
        {
            ReplaceBackward();
        }

        // ツールバー の Clickイベント
        private void toolFile_New_Click(object sender, EventArgs e)  { menuFile_New_Click(sender, e);  }
        private void toolFile_Open_Click(object sender, EventArgs e) { menuFile_Open_Click(sender, e); }
        private void toolFile_Save_Click(object sender, EventArgs e) { menuFile_Save_Click(sender, e); }

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
	        if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
	            foreach (string path in (string[])e.Data.GetData(DataFormats.FileDrop)) {
                    try {
                        //テキストに変更がある場合は、ファイルの保存確認をして、ファイルを保存します
                        ConfirmAndSave();

                        //ファイルを開きます
                        OpenFileSub(path);
                    } catch (Exception) {
                    }
                    return;
	            }
	        }
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
            title.Append(" [");
            title.Append(_textFile.TextEncoding.EncodingName);

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

        // ポップアップメニュー の click イベント
        private void popupMenuForTextbox_Cut_Click  (object sender, EventArgs e) { txtMain.Cut();   }
        private void popupMenuForTextbox_Copy_Click (object sender, EventArgs e) { txtMain.Copy();  }
        private void popupMenuForTextbox_Paste_Click(object sender, EventArgs e) { txtMain.Paste(); }

        /// <summary>
        /// 前方検索します。テキストの末尾へ向けて検索します。
        /// </summary>
        /// <returns>見つかった位置</returns>
        public int SearchForward()
        {
            //フォーカス設定
            txtMain.Focus();

            //検索する
            int index = txtMain.SearchForward(_searchData.SearchString, _searchData.IgnoreCase);
            return index;
        }

        /// <summary>
        /// 後方検索します。テキストの先頭へ向けて検索します。
        /// </summary>
        /// <returns>見つかった位置</returns>
        public int SearchBackward()
        {
            //フォーカス設定
            txtMain.Focus();

            //検索する
            int index = txtMain.SearchBackward(_searchData.SearchString, _searchData.IgnoreCase);
            return index;
        }

        /// <summary>
        /// 前方置換します。
        /// </summary>
        /// <returns>見つかった位置</returns>
        public int ReplaceForward()
        {
            //フォーカス設定
            txtMain.Focus();

            //前方置換します
            int index = txtMain.ReplaceForward(_searchData.SearchString, _searchData.ReplaceString, _searchData.IgnoreCase);
            return index;
        }

        /// <summary>
        /// 後方置換します。
        /// </summary>
        /// <returns>見つかった位置</returns>
        public int ReplaceBackward()
        {
            //フォーカス設定
            txtMain.Focus();

            //後方置換します
            int index = txtMain.ReplaceBackward(_searchData.SearchString, _searchData.ReplaceString, _searchData.IgnoreCase);
            return index;
        }

        /// <summary>
        /// 全置換します。
        /// </summary>
        public void ReplaceAll()
        {
            txtMain.ReplaceAll(_searchData.SearchString, _searchData.ReplaceString, _searchData.IgnoreCase);
        }

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

    } //class
}
