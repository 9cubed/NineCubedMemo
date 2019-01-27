using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Controls.FileList.Columns
{
    /// <summary>
    /// パスカラム
    /// 必ず1列目に設定します
    /// </summary>
    public class PathColumn : AFileListColumn
    {
        public PathColumn() : base()
        {
            this.Visible = false;
            this.ReadOnly = true;
            this.HeaderText = "パス";
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) => file.FullName;


    }
}
