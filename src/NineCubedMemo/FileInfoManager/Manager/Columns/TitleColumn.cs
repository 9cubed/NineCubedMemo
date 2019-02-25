using FileInfoManager.DB;
using NineCubed.Common.Controls.FileList;
using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileInfoManager.Manager.Columns
{
    public class TitleColumn : AFileListColumn, IFileListColumnForFileInfo
    {
        /// <summary>
        /// DB接続用オブジェクト
        /// </summary>
        FileDB _db;

        public TitleColumn(FileDB db) : base()
        {
            _db = db;
            this.ReadOnly = false; //編集可能
            this.HeaderText = "タイトル";
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.SortMode = DataGridViewColumnSortMode.Automatic; //ヘッダークリック時にソート(プログラム)するようにする
            this.Width = 100;
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) => "";
        public string ToString(FileData file) => file.title;

        //編集で値が変更された場合に呼ばれます
        public FileData ValueChanged(FileData orgFileData, string newValue) {
            //タイトルを更新します
            using(var connection = _db.GetConnection()) {
                FileDataDao.UpdateField(connection, orgFileData.id, "title", newValue);
            }

            orgFileData.title = newValue;
            return orgFileData;
        }

        // データに応じた色を返します
        public (Color, Color) GetColor(FileData fileData) => (Color.Empty, Color.Empty);
    }
}
