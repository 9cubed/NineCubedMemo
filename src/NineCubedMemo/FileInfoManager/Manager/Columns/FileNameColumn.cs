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
    public class FileNameColumn : AFileListColumn, IFileListColumnForFileInfo
    {
        /// <summary>
        /// DB接続用オブジェクト
        /// </summary>
        FileDB _db;

        public FileNameColumn(FileDB db) : base()
        {
            _db = db;
            this.ReadOnly = false;
            this.HeaderText = "ファイル名";
            this.Width = 100;
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) => file.Name;
        public string ToString(FileData file) => Path.GetFileName(file.path);

        /// <summary>
        /// 編集で値が変更された場合に呼ばれます
        /// </summary>
        /// <param name="orgFileData"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public FileData ValueChanged(FileData orgFileData, string newValue)
        {
            var oldPath     = orgFileData.path;
            var oldDir      = Path.GetDirectoryName(oldPath);
            var oldFileName = Path.GetFileName(oldPath);
            var newPath     = FileUtils.AppendPath(oldDir, newValue);

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

        // データに応じた背景色を返します
        public Color GetBackColor(FileData fileData) {
            if (FileUtils.Exists(fileData.path)) {
                return Color.White;
            } else {
                return Color.LightPink; //ファイルが存在しない
            }
        }

    } //class
}
