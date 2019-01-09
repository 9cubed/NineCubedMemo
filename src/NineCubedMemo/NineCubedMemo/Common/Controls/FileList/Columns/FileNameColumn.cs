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
    /// ファイル名カラム
    /// </summary>
    public class FileNameColumn : AFileListColumn
    {
        public FileNameColumn() : base()
        {
            this.ReadOnly = false;
            this.HeaderText = "ファイル名";
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) => file.Name;

        //編集で値が変更された場合に呼ばれます
        override
        public FileInfo ValueChanged(FileInfo orgFile, string newValue)
        {
            var oldPath = orgFile.FullName;
            var newPath = FileUtils.AppendPath(orgFile.DirectoryName, newValue);

            //ファイル名の変更がない場合は処理を抜けます
            if (orgFile.Name.Equals(newValue)) return orgFile;

            try {
                if (FileUtils.IsFile(oldPath)) {
                    //ファイルの場合
                    File.Move(oldPath, newPath);
                } else {
                    //フォルダの場合
                    Directory.Move(oldPath, newPath);
                }
            } catch (Exception ex) {
                __.ShowErrorMsgBox(ex.Message);
                return orgFile;
            }

            return new FileInfo(newPath);
        }

    } //class
}
