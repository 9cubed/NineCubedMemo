using FileInfoManager.DB;
using NineCubed;
using NineCubed.Common.Controls.FileList;
using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileInfoManager.Manager.Columns
{
    public class FileExtensionColumn : AFileListColumn, IFileListColumnForFileInfo
    {
        /// <summary>
        /// DB接続用オブジェクト
        /// </summary>
        FileDB _db;

        public FileExtensionColumn(FileDB db) : base()
        {
            _db = db;
            this.ReadOnly = false;
            this.HeaderText = "拡張子";
            this.Width = 50;
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) => file.Name.ToLower();
        public string ToString(FileData file) {
            if (file.kind == 2) return "(フォルダ)";

            return Path.GetExtension(file.path).ToLower();
        }

        /// <summary>
        /// 編集で値が変更された場合に呼ばれます
        /// </summary>
        /// <param name="orgFileData"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public FileData ValueChanged(FileData orgFileData, string newValue)
        {
            if (FileUtils.IsDir(orgFileData.path)) return null;

            //新旧の拡張子を取得します
            var oldExt = Path.GetExtension(orgFileData.path).Replace(".", "");
            var newExt = newValue.Replace(".", ""); //入力値の先頭に「.」がある場合は削除する

            //変更がない場合は処理を抜けます
            if (oldExt.Equals(newExt)) return null; 

            var oldPath = orgFileData.path;
            var oldDirPath  = Path.GetDirectoryName(oldPath); //フォルダのパス
            var oldFileName = Path.GetFileNameWithoutExtension(orgFileData.path); //拡張子を除いたファイル名
            var newPath     = FileUtils.AppendPath(oldDirPath, oldFileName + "." + newExt);

            //ファイル名の変更がない場合は処理を抜けます
            if (oldFileName.Equals(newValue)) return null;

            //ファイル名を変更します
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
                return null;
            }

            //パスを更新します
            using(var connection = _db.GetConnection()) {
                FileDataDao.UpdateField(connection, orgFileData.id, "path", newPath);
            }

            orgFileData.path = newPath;
            return orgFileData;
        }

        // データに応じた色を返します
        public (Color, Color) GetColor(FileData fileData) {
            if (fileData.kind == 2) {
                return (Color.Black, Color.LightGoldenrodYellow); //フォルダ
            } else {
                return (Color.Empty, Color.Empty);                //フォルダ以外(ファイル or 存在しない)
            }
        }

    } //class
}
