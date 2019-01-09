using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Controls.FileList.Columns
{
    /// <summary>
    /// 更新日時カラム
    /// </summary>
    public class FileUpdateDateColumn : AFileListColumn
    {
        public FileUpdateDateColumn() : base()
        {
            this.ReadOnly = false;
            this.HeaderText = "更新日時";
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) {
            return 
                file.LastWriteTime.ToShortDateString() + " " + 
                file.LastWriteTime.ToString("HH:mm");
        }

        //編集で値が変更された場合に呼ばれます
        override
        public FileInfo ValueChanged(FileInfo orgFile, string newValue)
        {
            if (DateTime.TryParse(newValue, out DateTime dt)) {
                try {
                    //更新日時を変更します
                    if (FileUtils.IsFile(orgFile.FullName)) {
                             File.SetLastWriteTime(orgFile.FullName, dt);
                    } else {
                        Directory.SetLastWriteTime(orgFile.FullName, dt);
                    }
                } catch (Exception ex) {
                    __.ShowErrorMsgBox(ex.Message);
                    return orgFile;
                }
            }
            
            return orgFile;
        }
    } //class

}
