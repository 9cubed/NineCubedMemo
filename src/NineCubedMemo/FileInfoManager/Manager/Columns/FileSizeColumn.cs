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
    public class FileSizeColumn : AFileListColumn, IFileListColumnForFileInfo
    {
        /// <summary>
        /// DB接続用オブジェクト
        /// </summary>
        FileDB _db;

        public FileSizeColumn(FileDB db) : base()
        {
            _db = db;
            this.ReadOnly = true;
            this.HeaderText = "サイズ";
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.SortMode = DataGridViewColumnSortMode.Automatic; //ヘッダークリック時にソート(プログラム)するようにする
            this.Width = 65;
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) {
            if (file.Exists) {
                return StringUtils.GetStringByteSize(file.Length);
            } else {
                return "";
            }
        }
        public string ToString(FileData file) => ToString(new FileInfo(file.path));

        /// <summary>
        /// ソートします
        /// </summary>
        /// <param name="sValue1"></param>
        /// <param name="sValue2"></param>
        /// <returns></returns>
        override
        public int SortCompare(string sValue1, string sValue2)
        {
            var iValue1 = StringUtils.GetLongByteSize(sValue1?.ToString());
            var iValue2 = StringUtils.GetLongByteSize(sValue2?.ToString());

            int result = (iValue1 > iValue2) ? 1 : -1;
            return result;
        }

        //編集で値が変更された場合に呼ばれます
        public FileData ValueChanged(FileData orgFileData, string newValue) => null;

        // データに応じた背景色を返します
        public Color GetBackColor(FileData fileData) => Color.White;

    } //class
}
