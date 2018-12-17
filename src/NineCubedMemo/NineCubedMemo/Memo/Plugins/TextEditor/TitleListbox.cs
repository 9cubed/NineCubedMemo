using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.TextEditor
{
    public class TitleListbox : ListBox
    {
        //見出し用文字リスト
        public IList<string> TitleCharList { get; set; } = new List<string>();

        //ターゲットのテキストボックス
        public TextBoxBase TargetTextbox { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TitleListbox()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TitleListbox
            // 
            this.SelectedIndexChanged += new System.EventHandler(this.TitleListbox_SelectedIndexChanged);
            this.ResumeLayout(false);
        }

        //見出しリストを設定します
        public void SetTitleList()
        {
            //見出しリストをクリアします
            this.Items.Clear();

            //TODO
            //1行が一定の文字数を超えると自動的に改行されるため、Lines のインデックスの行がずれる
            //文字数で改行された数をカウントして、LineNo を指定する際に、オフセットとして加算する必要がある。

            //テキストを行単位で取得します
            string[] lines = this.TargetTextbox.Lines;

            //1行ずつループ
            for (int i = 0; i < lines.Count(); i++) {

                //レベルでループ
                for (int level = 0; level < TitleCharList.Count(); level++) {

                    //先頭に見出し文字がない場合は、次のループへ
                    if (lines[i].StartsWith(TitleCharList[level]) == false) continue;
                    //見出しがある場合

                    //見出し文字列の作成します
                    var title = 
                        StringUtils.RepeatChar('　', level) + //インデント
                        TitleCharList[level] +                //見出し記号
                        lines[i].Substring(1);                //見出し文字列(見出し記号の以降の文字列)

                    //見出し要素をリストに追加します
                    this.Items.Add( new TitleData { Title = title, LineNo = i } );
                }
            }
        }

        /// <summary>
        /// リストボックスの要素が選択された時のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitleListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //選択アイテムを取得する
            var item = (TitleData)this.SelectedItem;
            if (item == null) return;

            //指定した行のインデックスを取得する
            int index = this.TargetTextbox.GetFirstCharIndexFromLine(item.LineNo);

            //カーソルを移動する
            this.TargetTextbox.Select(index, 0);

            //テキストボックスをスクロールさせるため、1度テキストにフォーカスを与える
            this.TargetTextbox.Focus();

            //フォーカスを見出しリストに戻す
            this.Focus();
        }

    } //class

    //見出しリストボックスの要素
    public class TitleData {
        public string Title  { get; set; } //見出し
        public int    LineNo { get; set; } //見出しがある行番号

        //リストボックスの表示用
        public override string ToString() { return Title; }
    }

}
