using NineCubed.Memo.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo
{
    public partial class SearchForm : Form
    {
        /// <summary>
        /// 検索ターゲット
        /// </summary>
        ISearchString _target= null;

        /// <summary>
        /// フォーム。シングルトン。
        /// </summary>
        static SearchForm _form = null;

        /// <summary>
        /// 検索条件
        /// </summary>
        public SearchData Data { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target"></param>
        private SearchForm(ISearchString target)
        {
            InitializeComponent();

            _target = target;
        }

        /// <summary>
        /// フォームを表示します
        /// </summary>
        /// <param name="target"></param>
        /// <param name="searchData"></param>
        public static void ShowForm(ISearchString target, SearchData searchData) {
            if (_form == null || _form.IsDisposed) {
                _form = new SearchForm(target);
            }

            //検索条件を保持します
            _form.Data = searchData;

            //入力欄に検索条件を反映します
            _form.txtSearch.Text  = searchData.SearchString;
            _form.txtReplace.Text = searchData.ReplaceString;
            _form.chkCase.Checked = !searchData.IgnoreCase;

            _form.Show();
        }

        /// <summary>
        /// 後方検索ボタン の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchBackward_Click(object sender, EventArgs e)
        {
            _target.SearchBackward(txtSearch.Text, !chkCase.Checked);
        }

        /// <summary>
        /// 前方検索ボタン の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchForward_Click(object sender, EventArgs e)
        {
            _target.SearchForward(txtSearch.Text, !chkCase.Checked);
        }

        /// <summary>
        /// 後方置換ボタン の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReplaceBackward_Click(object sender, EventArgs e)
        {
            _target.ReplaceBackward(txtSearch.Text, txtReplace.Text, !chkCase.Checked);
        }

        /// <summary>
        /// 前方置換ボタン の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReplaceForward_Click(object sender, EventArgs e)
        {
            _target.ReplaceForward(txtSearch.Text, txtReplace.Text, !chkCase.Checked);
        }

        /// <summary>
        /// 全置換ボタン の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            _target.ReplaceAll(txtSearch.Text, txtReplace.Text, !chkCase.Checked);
        }

        /// <summary>
        /// 閉じるボタン の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// フォーム の  KeyDown イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchForm_KeyDown(object sender, KeyEventArgs e)
        {
            //ESCが押されたら画面を閉じます
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        /// <summary>
        /// 検索文字列欄 の KeyDown イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            //エンターが押された場合は、前方検索を行って画面を閉じます
            if (e.KeyCode == Keys.Enter) {
                btnSearchForward_Click(sender, null);
                this.Close();
            }
        }

        /// <summary>
        /// 置換文字列欄 の KeyDown イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtReplace_KeyDown(object sender, KeyEventArgs e)
        {
            //エンターが押された場合は、前方置換を行って画面を閉じます
            if (e.KeyCode == Keys.Enter) {
                btnReplaceForward_Click(sender, null);
                this.Close();
            }
        }

        /// <summary>
        /// 検索文字列 の TextChanged イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _form.Data.SearchString = txtSearch.Text;
        }

        /// <summary>
        /// 置換文字列 の TextChanged イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtReplace_TextChanged(object sender, EventArgs e)
        {
            _form.Data.ReplaceString = txtReplace.Text;
        }

        /// <summary>
        /// 大文字・小文字を区別するチェックボックス の CheckedChanged イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkCase_CheckedChanged(object sender, EventArgs e)
        {
            _form.Data.IgnoreCase = !chkCase.Checked;
        }

    } //class
}
