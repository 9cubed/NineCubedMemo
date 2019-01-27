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
    /// 拡張子カラム
    /// </summary>
    public class FileExtensionColumn : AFileListColumn
    {
        public FileExtensionColumn() : base()
        {
            this.ReadOnly = false;
            this.HeaderText = "拡張子";
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) => IsFile(file)?.Extension;

        //編集で値が変更された場合に呼ばれます
        override
        public FileInfo ValueChanged(FileInfo orgFile, string newValue)
        {
            //フォルダの場合は処理しない
            if (FileUtils.IsFile(orgFile.FullName) == false) return null; 

            //新旧の拡張子を取得します
            var oldExt = Path.GetExtension(orgFile.FullName).Replace(".", "");
            var newExt = newValue.Replace(".", ""); //入力値の先頭に「.」がある場合は削除する

            //変更がない場合は処理を抜けます
            if (oldExt.Equals(newExt)) return null; 

            var oldDirPath  = orgFile.DirectoryName;                          //フルパス
            var oldFileName = Path.GetFileNameWithoutExtension(orgFile.Name); //拡張子を除いたファイル名
            var newPath     = FileUtils.AppendPath(oldDirPath, oldFileName + "." + newExt);

            try {
                if (FileUtils.IsFile(orgFile.FullName)) {
                    //ファイルの場合
                    File.Move(orgFile.FullName, newPath);
                } else {
                    //フォルダの場合
                    Directory.Move(orgFile.FullName, newPath);
                }
            } catch (Exception ex) {
                __.ShowErrorMsgBox(ex.Message);
                return null;
            }

            return new FileInfo(newPath);

        }

    } //class
}
