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
        /// ver1.0.1
        /// </summary>
        public string Path { get; set; }

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
            textBox.SelectionIndent = 6;

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
        /// ver1.0.1
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
        /// ver1.0.1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFile_New_Click(object sender, EventArgs e)
        {
            if (txtMain.Modified) {
                //テキストに変更がある場合
                var result = MessageBox.Show("変更されています。保存しますか？", "確認", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel) {
                    //「キャンセル」の場合、処理を抜けます
                    return;
                }

                if (result == DialogResult.Yes) {
                    //「はい」の場合

                    //保存をします
                    menuFile_Save_Click(sender, e);
                }
            }

            //テキストボックスのテキストを初期化します
            txtMain.Clear(); //Modified は false になる

            //変更なしにします
            txtMain.Modified = false;

            //パスを未設定にします
            this.Path = null;
        }

        /// <summary>
        /// メニュー・ファイル・開く の Clickイベント
        /// ver1.0.1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFile_Open_Click(object sender, EventArgs e)
        {
            if (txtMain.Modified) {
                //テキストに変更がある場合
                var result = MessageBox.Show("変更されています。保存しますか？", "確認", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel) {
                    //「キャンセル」の場合、処理を抜けます
                    return;
                }
                if (result == DialogResult.Yes) {
                    //「はい」の場合

                    //保存します
                    menuFile_Save_Click(sender, e);
                }
            }

            {
                //開くダイアログを表示します
                openFileDialog.FileName = "";
                var result = openFileDialog.ShowDialog();
                if (result == DialogResult.OK) {
                    //「OK」の場合

                    //ファイルの読み込み
                    string path = openFileDialog.FileName;
                    txtMain.LoadFile(path, RichTextBoxStreamType.PlainText);

                    //変更なしにします
                    txtMain.Modified = false;

                    //パスを保持します
                    this.Path = path;
                }
            }
        }

        /// <summary>
        /// メニュー・ファイル・上書き保存 の Clickイベント
        /// ver1.0.1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFile_Save_Click(object sender, EventArgs e)
        {
            //パスが設定されていない場合は、名前を付けて保存処理に移行します
            if (this.Path == null) {
                menuFile_SaveAs_Click(sender, e);
                return;
            }

            //ファイルの保存
            txtMain.SaveFile(this.Path, RichTextBoxStreamType.PlainText);

            //変更なしにします
            txtMain.Modified = false;
        }

        /// <summary>
        /// メニュー・ファイル・名前を付けて保存 の Clickイベント
        /// ver1.0.1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFile_SaveAs_Click(object sender, EventArgs e)
        {
            //保存ダイアログを表示します
            var result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK) {
                //「OK」の場合、選択されたファイルに保存します

                //ファイルの保存
                string path = saveFileDialog.FileName;
                txtMain.SaveFile(path, RichTextBoxStreamType.PlainText);

                //パスを保持します
                this.Path = path;

                //変更なしにします
                txtMain.Modified = false;
            }
        }

    } //class
}
