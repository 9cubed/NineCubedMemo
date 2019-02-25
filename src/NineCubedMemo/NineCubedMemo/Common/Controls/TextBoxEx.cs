using NineCubed.Common.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Common.Controls
{
    [ToolboxItem(true)]
    public class TextBoxEx : RichTextBox
    {
        //入力コンテキストの取得
        [DllImport("Imm32.dll")]
        private static extern IntPtr ImmGetContext(IntPtr hWnd);

        //漢字変換に関する情報の取得
        //https://msdn.microsoft.com/ja-jp/library/cc447983.aspx
        [DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
        private static extern int ImmGetCompositionString(IntPtr hIMC, int dwIndex, byte[] lpBuf, int dwBufLen);

        //入力コンテキストなどの解放
        [DllImport("Imm32.dll")]
        private static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);

        /// <summary>
        /// IMEで漢字変換が確定した時のイベント
        /// </summary>
        public delegate void ImeConvertedEventHandler(object sender, string selectedString);
        public event ImeConvertedEventHandler ImeConverted = null;


        /// <summary>
        /// テキストボックスの左側の余白のサイズ
        /// </summary>
        private const int TextBoxMarginLeft = 6;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TextBoxEx() {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TextBoxEx
            // 
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxEx_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxEx_KeyPress);
            this.ResumeLayout(false);
        }

        /// <summary>
        /// テキストボックスを初期化します
        /// </summary>
        public void Initialize(string fontName = "ＭＳ ゴシック", float fontSize = 12, int tabSize = 4)
        {
            //折り返さないようにします
            this.WordWrap = false;

            //タブを入力できるようにします。false にすると、別のコントロールにフォーカスが移動してしまい、タブを入力できません
            this.AcceptsTab = true;

            //文字の幅が同じするため、等幅フォントの「ＭＳ ゴシック」にします
            this.Font = new Font(fontName, fontSize);

            //デフォルトでは、半角英数字は英語フォント、それ以外は日本語フォントを使う DualFont になっているため、AutoFont だけを再設定します
            //この設定をしないと、半角の「i」の幅がものすごく狭くなってしまう
            this.LanguageOption = RichTextBoxLanguageOptions.AutoFont;

            //テキストボックスの左の内側に余白を設定します
            this.SelectionIndent = TextBoxMarginLeft;

            //左の余白がリセットされる不具合があるため、コメントアウトしました
            //URLが入力された時に書式が変わらないようにする
            //textBox.DetectUrls = false; //true にすると、LinkClickedイベントでクリックされたURL(e.LinkText)が取得できる

            //タブの位置を設定します
            SetSelectionTabs(tabSize);

            //変更なしにします
            this.Modified = false;
        }

        /// <summary>
        /// テキストボックスにタブの停止位置を設定します
        /// 
        /// タブの停止位置は、1タブ＝半角スペース4個分、などのように指定できません
        /// 実際に描画される1文字分の幅を取得して、そこからタブが止まる位置を計算して設定しています
        /// </summary>
        /// <param name="tabSize">1タブあたりの文字数</param>
        private void SetSelectionTabs(int tabSize)
        {
            //1文字の幅のサイズを取得します
            int charWidth = 0;
            using (Graphics g = this.CreateGraphics()) {
                Size size = TextRenderer.MeasureText(g, "A", this.Font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding);
                charWidth = size.Width;
            }

            //タブで止まる位置を配列に設定します
            int[] tabArray = new int[32]; //32より多く設定するとエラーとなる
            for (int i = 0; i < tabArray.Count(); i++)
            {
                tabArray[i] = (charWidth * tabSize * (i + 1) + this.SelectionIndent); //textBox.SelectionIndent はテキスト内の左側の余白サイズ
            }

            //タブで止まる位置を設定します
            this.SelectionTabs = tabArray;
        }

        /// <summary>
        /// 選択されている文字列を削除します
        /// </summary>
        public void Delete() {
            this.SelectedText = "";
        }

        public int SearchForward(string searchString, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(searchString)) return -1; //検索文字列が空の場合は処理しない

            //選択文字列が検索文字列と同じ場合は、次の文字列から検索します
            int offset = this.SelectedText.Equals(searchString, getStringComparison(ignoreCase)) ? 1 : 0;
            int index = this.Text.IndexOf(searchString, this.SelectionStart + offset, getStringComparison(ignoreCase));
            if (index >= 0) {
                //見つかった場合
                this.Select(index, searchString.Length);
            }

            return index;
        }

        public int SearchBackward(string searchString, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(searchString)) return -1; //検索文字列が空の場合は処理しない

            //検索開始位置の設定
            int searchStartIndex = (this.SelectionStart + searchString.Length - 1) - 1;
            if (searchStartIndex < 0) return -1; //先頭の場合は処理を抜けます

            //検索開始位置が末尾以降になる場合は、検索開始位置を末尾にします
            if (searchStartIndex > this.Text.Length) searchStartIndex = this.Text.Length;

            //検索
            int index = this.Text.LastIndexOf(searchString, searchStartIndex, getStringComparison(ignoreCase));
            if (index >= 0) {
                //見つかった場合
                this.Select(index, searchString.Length);
            }
            return index;
        }

        public int ReplaceForward(string searchString, string replaceString, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(searchString)) return -1; //検索文字列が空の場合は処理しない

            //選択文字列が検索文字列と同じ場合は置換します
            if (this.SelectedText.Equals(searchString, getStringComparison(ignoreCase))) {
                //同じ場合
                this.SelectedText = replaceString;
            }

            //前方検索をします
            int index = SearchForward(searchString, ignoreCase);
            return index;
        }

        public int ReplaceBackward(string searchString, string replaceString, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(searchString)) return -1; //検索文字列が空の場合は処理しない

            //選択文字列が検索文字列と同じ場合は置換します
            if (this.SelectedText.Equals(searchString, getStringComparison(ignoreCase))) {
                //同じ場合

                //カーソルの位置を保存します(SelectedText に設定をすると、カーソルが後ろに移動するため)
                int oldSelectionStart = this.SelectionStart;

                //置換します
                this.SelectedText = replaceString;

                //カーソルの位置を復元します
                this.SelectionStart = oldSelectionStart;
            }

            //後方検索をします
            int index = SearchBackward(searchString, ignoreCase);
            return index;
        }

        public void ReplaceAll(string searchString, string replaceString, bool ignoreCase)
        {
            this.SelectionStart = 0;  //カーソルを先頭に移動します
            this.SelectionLength = 0; //文字列を未選択にします
            int index = 0;
            while (index >= 0) {
                index = ReplaceForward(searchString, replaceString, ignoreCase);
            }
        }

        /// <summary>
        /// 大文字・小文字の条件を取得します
        /// </summary>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        private StringComparison getStringComparison(bool ignoreCase) {
            return ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        }

        /// <summary>
        /// 書式なしのテキストフォーマットでペーストします。
        /// </summary>
        public new void Paste() {
            //テキスト形式でペーストします
            var textFormat = DataFormats.GetFormat(DataFormats.Text);
            this.Paste(textFormat);
        }

        /// <summary>
        /// KeyDownイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxEx_KeyDown(object sender, KeyEventArgs e)
        {
            //デフォルトのペーストは書式付きになるため、独自のペーストで処理します
            if (e.Control && e.KeyCode.Equals(Keys.V)) {
                e.Handled = true; //Ctrl+V のKeyDownを処理済みにします。デフォルトのペーストが実行されなくなる。
                this.Paste();     //ペーストします
                return;
            }
            
            //タブの場合はインデントします
            if (e.KeyCode == Keys.Tab) {
                Indent(e);
                return;
            }

            //移動できない方向のカーソルキーが押された場合は、
            //エラー音が鳴るためキー操作を無効にします
            int currentLine = this.GetLineFromCharIndex(this.SelectionStart); //先頭行
            if (currentLine == 0) {
                //先頭行が上が押された場合
                if (e.KeyCode == Keys.Up) {
                    e.Handled = true; return;
                }
                //テキストの先頭で左 or BSキーが押された場合
                if ((e.KeyCode == Keys.Left || e.KeyCode == Keys.Back) 
                    && this.SelectionStart == 0) {
                    e.Handled = true; return;
                }
            }
            int lastLine = this.GetLineFromCharIndex(this.TextLength); //最終行
            if (currentLine == lastLine) {
                //最終行で下が押された場合
                if (e.KeyCode == Keys.Down && currentLine == lastLine) {
                    e.Handled = true; return;
                }
                //テキストの最後で右が押された場合
                if (e.KeyCode == Keys.Right && this.SelectionStart == this.TextLength) {
                    e.Handled = true; return;
                }
            }
        }

        /// <summary>
        /// KeyPressイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxEx_KeyPress(object sender, KeyPressEventArgs e)
        {
            //タブが押された場合は、タブが入力されないようにするため、処理済みにします
            if (e.KeyChar == 9) {
                e.Handled = true; //処理済み
                return;
            }
        }

        /// <summary>
        /// インデントします
        /// </summary>
        /// <param name="e"></param>
        private void Indent(KeyEventArgs e) {

            //文字列が選択されていない場合は、タブの挿入または削除を行う
            if (this.SelectedText.Length == 0) {

                if (e.Shift) {
                    if (this.SelectionStart == 0) return; //テキストの先頭の場合は処理しない

                    //カーソルの前にタブがあれば、そのタブを削除する
                    if (this.Text.Substring(this.SelectionStart - 1, 1).Equals("\t") || 
                        this.Text.Substring(this.SelectionStart - 1, 1).Equals(" ")) {
                        this.SelectionStart = this.SelectionStart - 1;
                        this.SelectionLength = 1;
                        this.SelectedText = "";
                    }

                } else {
                    //カーソルの位置にタブを挿入する
                    this.SelectedText = "\t";
                }

                return;
            }

            //選択されている行の範囲を取得します
            int startLineNo = this.GetLineFromCharIndex(this.SelectionStart);
            int endLineNo   = this.GetLineFromCharIndex(this.SelectionEnd);
            
            if (e.Shift) {
                //Shift押下 -> インデント解除
                
                //タブを置換するため、対象の行全体を選択します
                SelecteLine(startLineNo, endLineNo);

                //インデント解除処理(先頭)
                //選択文字列の先頭がタブかスペースの場合は削除します
                //下の一括置換で最初のタブとスペースは置換できないので個別に処理しています
                string target = this.SelectedText;
                if (target.StartsWith("\t") || target.StartsWith(" ")) target = this.SelectedText.Substring(1);

                //インデント解除処理
                //行の先頭のスペースとタブを削除します
                //タブだけで置換すると、行の先頭以外のタブも置換されてしまうため、\nを付けています
                target = target.Replace("\n " , "\n");
                target = target.Replace("\n\t", "\n");
                this.SelectedText = target;

            } else {
                //Shift押下していない -> インデント

                //行全体が選択されるように再選択します
                SelecteLine(startLineNo, endLineNo);

                //選択文字列の改行を「改行 + タブ」にして、さらに先頭にタブを追加します
                string target = this.SelectedText;
                this.SelectedText = 
                    "\t" +                        //先頭は個別にタブ追加
                    target.Replace("\n", "\n\t"); //先頭以外は改行を「改行+タブ」に置換
            }

            //行全体が選択されるように再選択します
            SelecteLine(startLineNo, endLineNo);
        }

        //指定された行を選択します
        private void SelecteLine(int startLineNo, int endLineNo) {
            //選択されている行の先頭のIndexを取得します
            int startIndex = this.GetFirstCharIndexFromLine(startLineNo);
            int endIndex   = this.GetFirstCharIndexFromLine(endLineNo);

            //最終行の末尾のIndexを取得します
            endIndex = endIndex + this.Lines[endLineNo].Length;

            //行全体が選択されるように選択します
            this.SelectionStart = startIndex;
            this.SelectionLength = endIndex - startIndex;
        }

        /// <summary>
        /// 選択されている文字列の末尾のインデックスを返します
        /// </summary>
        /// <returns></returns>
        public int SelectionEnd {
            get {
                return this.SelectionStart + this.SelectionLength - 1;
            }
        }

        /// <summary>
        /// 現在カーソルがある行を返します
        /// </summary>
        /// <returns></returns>
        public int GetCurrntLine()
        {
               int currentLine = this.GetLineFromCharIndex(this.SelectionStart) + 1;
            return currentLine;
        }

        /// <summary>
        /// 現在カーソルがある列を返します
        /// </summary>
        /// <returns></returns>
        public int GetCurrntColumn()
        {
            //現在の行を取得します
            int currentLineIndex = GetCurrntLine() - 1;

            //行の先頭のIndexを取得します
            int firstCharIndex = this.GetFirstCharIndexFromLine(currentLineIndex);

            //現在のIndex - 行の先頭のIndex で列を取得します
               int column = this.SelectionStart - firstCharIndex;
            return column;
        }

        /// <summary>
        /// ウィンドウプロシージャー
        /// Windowsメッセージを処理します
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m) {

            const int WM_IME_COMPOSITION = 0x10F;  //IMEに関するメッセージ
            const int GCS_RESULTSTR      = 0x0800; //漢字変換した文字列の取得

            if (m.Msg == WM_IME_COMPOSITION) {
                //テキストボックスの入力コンテキストを取得します
                IntPtr hIMC = ImmGetContext(this.Handle);

                try {
                    //漢字変換した文字列のバッファサイズを取得します (nullは含んでいない)
                    int length = ImmGetCompositionString(hIMC, GCS_RESULTSTR, null, 0);
                    if (length > 0) {
                        //バッファ確保
                        byte[] lpBuf = new byte[length];

                        //漢字変換した文字列を取得します
                        ImmGetCompositionString(hIMC, GCS_RESULTSTR, lpBuf, length);

                        //UNICODEのバイト配列 -> 文字列
                        string selectedString = Encoding.Unicode.GetString(lpBuf);
                        
                        //漢字変換確定イベントを発生させます
                        if (this.ImeConverted != null) ImeConverted(this, selectedString);
                    }

                } finally {
                    //入力コンテキストなどを解放します
                    ImmReleaseContext(this.Handle, hIMC);
                }
            }

            //Windows メッセージを処理します
            base.WndProc(ref m);
        }

    } //class
}
