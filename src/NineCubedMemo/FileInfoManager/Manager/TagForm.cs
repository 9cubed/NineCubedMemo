using FileInfoManager.DB;
using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileInfoManager.Manager
{
    public partial class TagForm : Form
    {
        //DB
        FileDB _db;

        //表示モード
        Mode _mode;
        public enum Mode {
            Add,
            Update,
            Delete,
            Select
        }

        /// <summary>
        /// 選択されたタグリスト
        /// </summary>
        public IList<string> SelectedTagList;

        /// <summary>
        /// DBのタグリスト
        /// </summary>
        IList<TagData> _dbTagList;

        public TagForm(Mode mode, FileDB db, string tags)
        {
            InitializeComponent();

            //引数を保持します
            _mode = mode;
            _db = db;
            txtTag.Text = tags;

            if (mode == Mode.Add)    lblMsg.Text = "追加するタグを入力してください。";
            if (mode == Mode.Update) lblMsg.Text = "設定するタグを全て入力してください。";
            if (mode == Mode.Delete) lblMsg.Text = "削除するタグを入力してください。";
            if (mode == Mode.Select) lblMsg.Text = "検索するタグを入力してください。";

            //タグリストを全体に広げます
            listTag.Dock = DockStyle.Fill;

            //DBのタグリストを取得します
            using(var connection = _db.GetConnection()) {
                _dbTagList = TagDataDao.GetAllTagList(_db.GetConnection());
            }

            //タグリストを作成します
            SetTagList();
        }

        /// <summary>
        /// タグ用Listboxの要素を設定します
        /// </summary>
        private void SetTagList()
        {
            //テキストのタグをリストにします
            var inputTagList = txtTag.Text.Split(new[]{' '}, StringSplitOptions.RemoveEmptyEntries).ToList();

            //DBのタグリストを、タグ用Listboxに設定します
            listTag.Items.Clear();
            foreach (var tagData in _dbTagList) {
                //テキストのタグに含まりていないタグだけ追加します
                if (inputTagList.IndexOf(tagData.tag) == -1) {
                    var item = new TagItem {
                        Tag  = tagData.tag,
                        Text = tagData.tag + "(" + tagData._fileCount + ")"};
                    listTag.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// タグ反映ボタンクリック
        /// タグリストで選択されているタグを、テキストに反映します。
        /// また、反映したタグを、リストから削除します
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddTag_Click(object sender, EventArgs e)
        {
            //選択されているタグリストを取得します
            var selectedTagItemList = new List<TagItem>();
            foreach(TagItem tagItem in listTag.SelectedItems) {
                selectedTagItemList.Add(tagItem);
            }
            
            //選択されているタグがテキストにない場合は、タグをテキストに追加します
            foreach(var tagItem in selectedTagItemList) {
                if (txtTag.Text.IndexOf(tagItem.Tag) == -1) {
                    //テキストにタグを追加します
                    txtTag.Text += " " + tagItem.Tag;

                    //タグ用Listboxからタグを削除します
                    listTag.Items.Remove(tagItem);
                }
            }
            txtTag.Text = txtTag.Text.Trim();
        }

        /// <summary>
        /// 設定ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSet_Click(object sender, EventArgs e)
        {
            this.SelectedTagList = txtTag.Text.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries).ToList();
            this.Hide();
        }

        /// <summary>
        /// 閉じるボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            //フォームを閉じます
            this.SelectedTagList = null;
            this.Hide();
        }

        /// <summary>
        /// フォームのリサイズイベント
        /// タブに反映するボタンを中央に配置するために使用。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TagForm_Resize(object sender, EventArgs e)
        {
            btnAddTag.Left = (this.Width - btnAddTag.Width) / 2;
        }

        /// <summary>
        /// キーダウンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                //設定ボタンクリックの処理を実行します
                btnSet_Click(sender, e);
            }
        }

        private void txtTag_Leave(object sender, EventArgs e)
        {
            //タグ用Listboxを再設定します
            SetTagList();
        }

        private void listTag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                //▲ボタンクリックの処理を実行します
                btnAddTag_Click(sender, e);
            }
        }
    } //class

    /// <summary>
    /// タグ用Listboxの要素
    /// </summary>
    class TagItem
    {
        public string Tag  { get; set; } //タグ
        public string Text { get; set; } //表示テキスト

        public override string ToString() => this.Text;
    }

}
