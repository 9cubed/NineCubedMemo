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
    public class UpdatedDateColumn : AFileListColumn, IFileListColumnForFileInfo
    {
        /// <summary>
        /// DB接続用オブジェクト
        /// </summary>
        FileDB _db;

        public UpdatedDateColumn(FileDB db) : base()
        {
            _db = db;
            this.ReadOnly = true;
            this.HeaderText = "更新日時";
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.SortMode = DataGridViewColumnSortMode.Automatic; //ヘッダークリック時にソート(プログラム)するようにする
            this.Width = 120;
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) => "";
        public string ToString(FileData file) => file.updated;

        //編集で値が変更された場合に呼ばれます
        public FileData ValueChanged(FileData orgFileData, string newValue) => null;

        // データに応じた背景色を返します
        public Color GetBackColor(FileData fileData) => Color.White;
    }
}
