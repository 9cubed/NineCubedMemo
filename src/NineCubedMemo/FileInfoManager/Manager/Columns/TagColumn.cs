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
    public class TagColumn : AFileListColumn, IFileListColumnForFileInfo
    {
        /// <summary>
        /// DB接続用オブジェクト
        /// </summary>
        FileDB _db;

        public TagColumn(FileDB db) : base()
        {
            _db = db;
            this.ReadOnly = false; //編集可能
            this.HeaderText = "タグ";
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.SortMode = DataGridViewColumnSortMode.Automatic; //ヘッダークリック時にソート(プログラム)するようにする
            this.Width = 100;
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) => "";
        public string ToString(FileData file) => file._tags;

        /// <summary>
        /// 編集で値が変更された場合に呼ばれます
        /// </summary>
        /// <param name="orgFileData"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public FileData ValueChanged(FileData orgFileData, string newValue) {

            //タグを更新します
            using(var connection = _db.GetConnection()) {
                orgFileData._tags = newValue;
                FileDB.UpdateTag(connection, orgFileData);
            }

            return orgFileData;
        }

        // データに応じた背景色を返します
        public Color GetBackColor(FileData fileData) => Color.White;

    } //class
}
