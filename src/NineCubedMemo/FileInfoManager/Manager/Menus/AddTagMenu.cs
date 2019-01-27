using FileInfoManager.DB;
using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileInfoManager.Manager.Menus
{
    public class AddTagMenu : ToolStripMenuItem
    {
        /// <summary>
        /// メインコントロール
        /// </summary>
        FileInfoManagerControl _control;

        /// <summary>
        /// DB
        /// </summary>
        FileDB _db;

        /// <summary>
        /// 一覧表用グリッド
        /// </summary>
        DataGridView _grid;

        /// <summary>
        /// グリッドのタグのColインデックス
        /// </summary>
        int _tagColIndex;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AddTagMenu(FileInfoManagerControl control, FileDB db, DataGridView grid, int tagColIndex) {
            this.Text = "タグ追加";
            this.Click += MenuClick;

            //引数を保持します
            _control = control;
            _db = db;
            _grid = grid;
            _tagColIndex = tagColIndex;
        }

        /// <summary>
        /// メニュークリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuClick(object sender, EventArgs e)
        {
            //タグ編集画面を表示します
            var selectedTagList = ShowTagForm();
            if (selectedTagList == null) return;

            //現在選択中のIDリストを取得します
            var idList = _control.GetSelectedIDList();
            if (idList.Count == 0) return;

            //DBにタグを追加します
            var result = AddTag(idList, selectedTagList);
            if (result == false) return;

            //変更内容をグリッドに反映します
            ReflectToGrid(idList, selectedTagList);
        }

        /// <summary>
        /// タグ編集画面を表示します
        /// </summary>
        /// <returns></returns>
        private IList<string> ShowTagForm() {
            //タグの編集画面を表示します
            using(var form = new TagForm(TagForm.Mode.Add, _db, tags:"")) {
                //画面を表示します
                form.ShowDialog();

                //選択されたタグリストを取得します
                return form.SelectedTagList;
            }
        }

        /// <summary>
        /// DBにタグを追加します
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        private bool AddTag(IList<int> idList, IList<string> selectedTagList) {
            //DBにタグを追加します
            using(var connection = _db.GetConnection()) {
                //トランザクション開始
                var transaction = connection.BeginTransaction();

                try {
                    //選択中のIDのデータに対して、選択されたタグを追加します
                    foreach (var id in idList) {
                        TagDataDao.AddTags(connection, id, selectedTagList);
                    }

                    //コミット
                    transaction.Commit();
                    return true;

                } catch (Exception ex) {
                    //ロールバック
                    transaction.Rollback();
                    MsgBoxUtils.ShowErrorMsgBox("更新に失敗しました。\n" + ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// 変更内容をグリッドに反映します
        /// </summary>
        /// <param name="idList"></param>
        private void ReflectToGrid(IList<int> idList, IList<string> selectedTagList)
        {
            foreach (var id in idList) {
                //IDのデータがあるRowを取得します
                int row = _control.GetRowIndex(id);

                //グリッドのタグをリストにします
                var tags = _grid[_tagColIndex, row].Value?.ToString();
                var tagList = tags.Split(new []{" "}, StringSplitOptions.RemoveEmptyEntries).ToList();

                //追加のタグリストと既存のタグリストを比較して、存在しないものだけをリストにします
                var addTagList = selectedTagList.Where(tag => tagList.Contains(tag) == false).ToList();

                //グリッドにタグを追加します
                _grid[_tagColIndex, row].Value += " " + string.Join(" ", addTagList);
            }
        }

    } //class
}
