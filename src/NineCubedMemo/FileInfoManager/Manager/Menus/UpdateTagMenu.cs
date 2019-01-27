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
    public class UpdateTagMenu : ToolStripMenuItem
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
        public UpdateTagMenu(FileInfoManagerControl control, FileDB db, DataGridView grid, int tagColIndex) {
            this.Text = "タグ更新";
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
            //現在選択中のIDリストを取得します
            var idList = _control.GetSelectedIDList();
            if (idList.Count == 0) return;

            //タグ編集画面を表示します
            var selectedTagList = ShowTagForm(idList[0]);
            if (selectedTagList == null) return;

            //DBにタグを追加します
            var result = UpdateTag(idList, selectedTagList);
            if (result == false) return;

            //変更内容をグリッドに反映します
            ReflectToGrid(idList, selectedTagList);
        }

        /// <summary>
        /// タグ編集画面を表示します
        /// </summary>
        /// <returns></returns>
        private IList<string> ShowTagForm(int topID) {
            //1番最初に選択されているデータのタグリストを取得します(子画面のデフォルト値として使用)
            string tags = "";
            using(var connection = _db.GetConnection()) {
                tags = TagDataDao.GetTags(connection, topID);
            }

            //タグの編集画面を表示します
            using(var form = new TagForm(TagForm.Mode.Add, _db, tags)) {
                //画面を表示します
                form.ShowDialog();

                //選択されたタグリストを取得します
                return form.SelectedTagList;
            }
        }

        /// <summary>
        /// DBのタグを更新します
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        private bool UpdateTag(IList<int> idList, IList<string> selectedTagList)
        {
            //DBのタグを更新します
            using(var connection = _db.GetConnection()) {
                //トランザクション開始
                var transaction = connection.BeginTransaction();
                try {
                    foreach (var id in idList) {
                        //タグを全て削除します
                        TagDataDao.Delete(connection, id);

                        //タグを追加します
                        foreach (var tag in selectedTagList) {
                            TagDataDao.Insert(connection, tag, id);
                        }
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
                //検索データ一覧を更新します
                int row = _control.GetRowIndex(id);
                _grid[_tagColIndex, row].Value = string.Join(" ", selectedTagList);
            }
        }


    } //class
}
