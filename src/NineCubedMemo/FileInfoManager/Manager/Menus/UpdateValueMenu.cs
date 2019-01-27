using FileInfoManager.DB;
using FileInfoManager.Manager.Columns;
using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileInfoManager.Manager.Menus
{
    public class UpdateValueMenu : ToolStripMenuItem
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
        /// グリッドの評価のColインデックス
        /// </summary>
        int _valueColIndex;

        /// <summary>
        /// クリックされた際に設定する評価
        /// </summary>
        int _value;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UpdateValueMenu(FileInfoManagerControl control, FileDB db, DataGridView grid, int valueColIndex, int value) {
            this.Text = value.ToString();
            this.Click += MenuClick;

            //引数を保持します
            _control = control;
            _db = db;
            _grid = grid;
            _valueColIndex = valueColIndex;
            _value = value;
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

            //評価を一括更新します
            var result = UpdateValue(idList);
            if (result == false) return;

            //変更内容をグリッドに反映します
            ReflectToGrid(idList);
        }

        /// <summary>
        /// 評価を一括更新します
        /// </summary>
        private bool UpdateValue(IList<int> idList)
        {
            using (var connection = _db.GetConnection()) {
                //トランザクション開始
                var transaction = connection.BeginTransaction();

                try {
                    foreach (var id in idList) {
                        //評価を更新します
                        FileDataDao.UpdateField(connection, id, "value", _value.ToString());
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
        private void ReflectToGrid(IList<int> idList)
        {
            foreach (var id in idList) {
                //IDのデータがあるRowを取得します
                int row = _control.GetRowIndex(id);

                //検索データ一覧を更新します
                _grid[_valueColIndex, row].Value = _value.ToString();
                _grid[_valueColIndex, row].Style.BackColor = 
                    ((ValueColumn)_grid.Columns[_valueColIndex]).GetBackColor(new FileData{ value = _value });

            }
        }

    } //class
}
