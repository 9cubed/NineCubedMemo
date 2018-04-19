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
        /// テキストファイルのパス
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// ver1.0.3
        /// ファイルが読み取り専用かどうか
        /// true:読み取り専用
        /// </summary>
        private bool IsReadOnly { get; set; }

        /// <summary>
        /// テキストボックスの左側の余白のサイズ
        /// </summary>
        private const int TextBoxMarginLeft = 6;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm()
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
            //テキストボックスを初期化します
            InitTextBox(txtMain);

            //隙間がなくなるようにテキストボックスを配置します
            txtMain.Dock = DockStyle.Fill;

            //ダイアログの初期設定 ver1.0.1
            openFileDialog.Filter = 
                "テキストファイル(*.txt)|*.txt" + "|" + 
                "すべてのファイル(*.*)|*.*";
            saveFileDialog.Filter = openFileDialog.Filter;
        }

        /// <summary>
        /// ver1.0.2
        /// MainForm の FormClosing イベント
        /// フォームが閉じられる直前に呼ばれます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //テキストファイルを閉じます
                CloseFile();
            } catch (CancelException) {
                //キャンセルされた場合、フォームが閉じるのを中止します
                e.Cancel = true;
            } catch (Exception ex) {
                //ver1.0.3
                e.Cancel = true;
                ShowErrorMsgBox(ex);
            }
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

            //URLが入力された時に書式が変わらないようにする
            textBox.DetectUrls = false; //true にすると、LinkClickedイベントでクリックされたURL(e.LinkText)が取得できる

            //タブの位置を設定します
            SetSelectionTabs(textBox, tabSize);

            //変更なしにします ver1.0.1
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
        /// ver1.0.3
        /// ツールバー・ファイル・新規作成 の Clickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolFile_New_Click(object sender, EventArgs e)
        {
            menuFile_New_Click(sender, e);
        }

        /// <summary>
        /// ver1.0.3
        /// ツールバー・ファイル・開く の Clickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolFile_Open_Click(object sender, EventArgs e)
        {
            menuFile_Open_Click(sender, e);
        }

        /// <summary>
        /// ver1.0.3
        /// ツールバー・ファイル・保存 の Clickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolFile_Save_Click(object sender, EventArgs e)
        {
            menuFile_Save_Click(sender, e);
        }

        /// <summary>
        /// ver1.0.3
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
        /// ver1.0.3
        /// テキストボックスの TextChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMain_TextChanged(object sender, EventArgs e)
        {
            //テキストボックスの左の内側に余白のサイズが初期化された場合は、再設定します
            //(テキストボックスの DetectUrls を false にしてテキストを全て削除すると、SelectionIndent が初期化されるため)
            if (txtMain.SelectionIndent != TextBoxMarginLeft) {
                txtMain.SelectionIndent = TextBoxMarginLeft;
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

                //パスを未設定にします
                this.Path = null;

                //読み取り専用を解除します
                this.IsReadOnly = false;

                //フォームのタイトルを設定します
                SetFormTitle();
            }
        }

        /// <summary>
        /// テキストファイルを開きます
        /// </summary>
        private void OpenFile()
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
                    txtMain.LoadFile(path, RichTextBoxStreamType.PlainText);

                    //変更なしにします
                    txtMain.Modified = false;

                    //パスを保持します
                    this.Path = path;

                    //読み取り専用かどうかを保持します
                    this.IsReadOnly = isReadOnly(this.Path);

                    //フォームのタイトルを設定します
                    SetFormTitle();
                }
            }
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
            //パスが未設定、ダイアログを表示、読み取り専用、の場合は、保存ダイアログを表示します
            if (this.Path == null || showDialog == true || this.IsReadOnly)
            {
                //パスが未設定の場合
                //保存ダイアログを表示します
                var result = saveFileDialog.ShowDialog();
                if (result == DialogResult.Cancel) throw new CancelException();
                if (result == DialogResult.No) return;

                //選択されたファイルのパスを保持します
                this.Path = saveFileDialog.FileName;
            }

            //ファイルを保存します
            txtMain.SaveFile(this.Path, RichTextBoxStreamType.PlainText);

            //変更なしにします
            txtMain.Modified = false;

            //読み取り専用を解除します
            this.IsReadOnly = false;

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
        /// ver1.0.3
        /// フォームのタイトルを設定します
        /// </summary>
        private void SetFormTitle() {
            
            string title = "";

            //読み取り専用の場合は、(読み取り専用) をつける
            if (this.Path != null) {
                title += (isReadOnly(this.Path) ? "(読み取り専用)" : "");
            }

            //パスが未設定の場合は「無題」にする
            title += this.Path ?? "無題";

            //テキストが変更されている場合は、(*) をつける
            title += (txtMain.Modified ? "(*)" : "");

            //フォームのタイトルを設定します
            this.Text = title;
        }

        /// <summary>
        /// ver1.0.3
        /// ファイルが読み取り専用かどうかを返します
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns>true:読み取り専用</returns>
        private bool isReadOnly(string path) {
            return ((File.GetAttributes(path) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly);
        }



    } //class
}
