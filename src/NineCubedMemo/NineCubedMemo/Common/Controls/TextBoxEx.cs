using NineCubed.Common.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Common.Controls
{
    [ToolboxItem(true)]
    public class TextBoxEx : RichTextBox
    {
        /// <summary>
        /// テキストボックスの左側の余白のサイズ
        /// </summary>
        private const int TextBoxMarginLeft = 6;

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

    } //class
}
