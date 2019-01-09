using NineCubed.Common.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Common.Controls.FileList.Columns
{
    
    /// <summary>
    /// ファイルサイズカラム
    /// </summary>
    public class FileSizeColumn : AFileListColumn
    {
        public FileSizeColumn() : base()
        {
            this.ReadOnly = true;
            this.HeaderText = "サイズ";
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.SortMode = DataGridViewColumnSortMode.Automatic; //ヘッダークリック時にソート(プログラム)するようにする
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) {
            if (IsFile(file) != null) {
                return StringUtils.GetStringByteSize(file.Length);
            } else {
                return "";
            }
        }

        override
        public int SortCompare(string sValue1, string sValue2)
        {
            var iValue1 = StringUtils.GetLongByteSize(sValue1?.ToString());
            var iValue2 = StringUtils.GetLongByteSize(sValue2?.ToString());

            //文字列が空の場合(フォルダの場合)は、-1 として扱う
            if (string.IsNullOrEmpty(sValue1)) iValue1 = -1;
            if (string.IsNullOrEmpty(sValue2)) iValue2 = -1;

            return iValue1 > iValue2 ? 1 : -1;
        }
    } // class
}
