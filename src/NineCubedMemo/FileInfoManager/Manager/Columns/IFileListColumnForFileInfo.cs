using FileInfoManager.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileInfoManager.Manager.Columns
{
    public interface IFileListColumnForFileInfo
    {
        string ToString(FileData fileData);
        FileData ValueChanged(FileData orgFileData, string newValue);

        /// <summary>
        /// データに応じた色を返します
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns>(前景色, 背景色)</returns>
        (Color, Color) GetColor(FileData fileData);
    }
}
