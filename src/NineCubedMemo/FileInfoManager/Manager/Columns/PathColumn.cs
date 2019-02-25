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
    public class PathColumn : AFileListColumn, IFileListColumnForFileInfo
    {
        /// <summary>
        /// DB接続用オブジェクト
        /// </summary>
        FileDB _db;

        public PathColumn(FileDB db) : base()
        {
            _db = db;
            this.ReadOnly = true;
            this.HeaderText = "フォルダ";
            this.Width = 200;
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) => file.FullName;
        public string ToString(FileData file) => Path.GetDirectoryName(file.path);

        //編集で値が変更された場合に呼ばれます
        public FileData ValueChanged(FileData orgFileData, string newValue) => null;

        //セルの描画処理
        public override void CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var cell = this.DataGridView[e.ColumnIndex, e.RowIndex];
            
            var path = (string)cell.Value;
            if (string.IsNullOrEmpty(path)) return;

            if (FileUtils.Exists(path)) {
                //ファイルがある場合
                //cell.Style.BackColor = Color.White;
            } else {
                //ファイルがない場合
                cell.Style.ForeColor = Color.Black;
                cell.Style.BackColor = Color.LightPink;
            }
        }

        // データに応じた色を返します
        public (Color, Color) GetColor(FileData fileData) => (Color.Empty, Color.Empty);

    } //class
}
