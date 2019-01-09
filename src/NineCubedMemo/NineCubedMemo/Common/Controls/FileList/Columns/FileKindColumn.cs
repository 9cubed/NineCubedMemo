using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Common.Controls.FileList.Columns
{
    /// <summary>
    /// ファイル種別カラム
    /// </summary>
    public class FileKindColumn : AFileListColumn
    {
        /// <summary>
        /// 画像キャッシュ用Dict
        /// </summary>
        Dictionary<string, Icon> _iconDict = new Dictionary<string, Icon>();

        public FileKindColumn() : base()
        {
            this.ReadOnly = true;
            this.HeaderText = "";
            this.Width = 22;

            //文字を透明にします
            this.DefaultCellStyle.SelectionForeColor = Color.Transparent;
        }

        public const string KIND_DIR = "*DIR*";

        //表示する値を返します
        override
        public string ToString(FileInfo file) => IsFile(file) == null ? KIND_DIR : file.FullName;

        //セルの描画処理
        public override void CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var paintParts = e.PaintParts & DataGridViewPaintParts.Background;
            e.Paint(e.ClipBounds, paintParts);
            
            //セル全体を描画します
            e.Graphics.FillRectangle(Brushes.White, e.CellBounds.X + 0, e.CellBounds.Y - 1, e.CellBounds.Width, e.CellBounds.Height);

            //ファイルの種類に応じて画像を描画します
            var cellValue = FileList[e.ColumnIndex, e.RowIndex].Value.ToString();
            if (cellValue.Equals(FileKindColumn.KIND_DIR)) {
                //フォルダの場合、フォルダの画像を描画します
                e.Graphics.DrawImage(FileList._imgFolder, e.CellBounds.Left + 1, e.CellBounds.Top + 1);

            } else {
                //ファイルの場合、拡張子に対応する画像を描画します

                //ファイルの画像を取得します
                var fileIcon = GetFileIcon(cellValue);

                //セル内の描画サイズを、幅と高さの小さい方で揃えます
                int minSize = Math.Min(e.CellBounds.Width, e.CellBounds.Height) - 2;

                //ファイルの画像を描画します
                e.Graphics.DrawIcon(fileIcon, new Rectangle(e.CellBounds.Left + 1, e.CellBounds.Top + 1, minSize, minSize));
            }

            //罫線を描画します
            e.Graphics.DrawRectangle(Pens.Gray, e.CellBounds.X - 1, e.CellBounds.Y - 1, e.CellBounds.Width, e.CellBounds.Height);

            //描画済みにします
            e.Handled = true; //false:デフォルトの描画処理が行われる
        }

        /// <summary>
        /// 拡張子に対応する画像を取得します
        /// EXEの場合はEXEのアイコンを取得します
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private Icon GetFileIcon(string path)
        {
            //拡張子をキーにしてDictから画像を取得します
            var ext = Path.GetExtension(path);
            _iconDict.TryGetValue(ext, out Icon fileIcon);
            if (fileIcon == null) {
                //取得できなかった場合(まだキャッシュされていない場合)

                //関連するアプリケーションのアイコンを取得します
                fileIcon = Icon.ExtractAssociatedIcon(path);

                //Dictで保持します
                if (_iconDict.Count < 256) { //念のため256個までに制限します
                    _iconDict[ext] = fileIcon;
                }
            }
            return fileIcon;
        }

    } //class
}
