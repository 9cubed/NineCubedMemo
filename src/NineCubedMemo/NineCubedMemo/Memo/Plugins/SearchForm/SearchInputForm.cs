using NineCubed.Common.Files;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.SearchForm
{
    public partial class SearchInputForm : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SearchInputForm()
        {
            InitializeComponent();
        }

        //プラグインマネージャー
        private PluginManager _pluginManager = null;

        // プロパティファイル
        private IniFile _property;

        /******************************************************************************
         * 
         *  イベント
         * 
         ******************************************************************************/
        
        /// <summary>
        /// フォームを表示します
        /// </summary>
        public void ShowForm(PluginManager pluginManager, IniFile property) {
            _pluginManager = pluginManager;
            _property = property;

            //入力欄に検索条件を反映します
            var searchData = GetSearchData();
            txtSearch.Text  = searchData.SearchString;
            txtReplace.Text = searchData.ReplaceString;
            chkCase.Checked = searchData.IgnoreCase;

            //フォームを表示します
            this.Show();
            this.Activate();
        }

        /// <summary>
        /// 検索データを返します
        /// </summary>
        /// <returns></returns>
        private SearchData GetSearchData()
        {
            return ((SearchData)_pluginManager.CommonData[CommonDataKeys.SearchData]);
        }


        /// <summary>
        /// 後方検索ボタン の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchBackward_Click(object sender, EventArgs e)
        {
            if (_pluginManager.ActivePlugin is ISearchPlugin plugin) {
                plugin.SearchBackward();
            }
        }

        /// <summary>
        /// 前方検索ボタン の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchForward_Click(object sender, EventArgs e)
        {
            if (_pluginManager.ActivePlugin is ISearchPlugin plugin) {
                plugin.SearchForward();
            }
        }

        /// <summary>
        /// 後方置換ボタン の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReplaceBackward_Click(object sender, EventArgs e)
        {
            if (_pluginManager.ActivePlugin is ISearchPlugin plugin) {
                plugin.ReplaceBackward();
            }
            
        }

        /// <summary>
        /// 前方置換ボタン の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReplaceForward_Click(object sender, EventArgs e)
        {
            if (_pluginManager.ActivePlugin is ISearchPlugin plugin) {
                plugin.ReplaceForward();
            }
        }

        /// <summary>
        /// 全置換ボタン の click イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            if (_pluginManager.ActivePlugin is ISearchPlugin plugin) {
                plugin.ReplaceAll();
            }
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
            GetSearchData().SearchString = txtSearch.Text;
        }

        /// <summary>
        /// 置換文字列 の TextChanged イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtReplace_TextChanged(object sender, EventArgs e)
        {
            GetSearchData().ReplaceString = txtReplace.Text;
        }

        /// <summary>
        /// 大文字・小文字を区別するチェックボックス の CheckedChanged イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkCase_CheckedChanged(object sender, EventArgs e)
        {
            GetSearchData().IgnoreCase = chkCase.Checked;
        }

        /// <summary>
        /// フォーム終了イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchInputForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //ウィンドウが最小化、最大化されている場合は、標準に戻します
            if (this.WindowState != FormWindowState.Normal) {
                this.WindowState  = FormWindowState.Normal;
            }

            //フォームの位置を保存します
            _property["location", "left"] = this.Left  .ToString();
            _property["location", "top"]  = this.Top   .ToString();
        }
    } //class
}
