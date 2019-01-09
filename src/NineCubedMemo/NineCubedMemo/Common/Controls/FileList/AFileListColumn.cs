using NineCubed.Common.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Common.Controls.FileList
{
    /// <summary>
    /// ファイルリストカラム
    /// </summary>
    public abstract class AFileListColumn : DataGridViewColumn, IFileListColumn
    {
        /// <summary>
        /// ファイルリスト本体
        /// </summary>
        public FileListGrid FileList { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AFileListColumn()
        {
            this.CellTemplate = new DataGridViewTextBoxCell();
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.SortMode                   = DataGridViewColumnSortMode.Automatic; //ヘッダークリック時にソートするようにする
            this.HeaderCell.Style.WrapMode  = DataGridViewTriState.False; //ヘッダーのタイトルを改行しないようにする
        }

        /// <summary>
        /// ファイル一覧表示直前の処理を行います
        /// </summary>
        public virtual void ShowFileListBefore() { }

        /// <summary>
        /// ファイル一覧表示直後の処理を行います
        /// </summary>
        public virtual void ShowFileListAfter() { }

        //表示する値を返します
        public abstract string ToString(FileInfo file);

        /// <summary>
        /// 指定されたファイルがフォルダ出ない場合はファイルをそのまま返します。
        /// フォルダの場合はnullを返します
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public FileInfo IsFile(FileInfo file) => file.Attributes.HasFlag(FileAttributes.Directory) ? null : file;

        //値の比較をします。ソートモードが Programmatic の場合のみ使用します
        public virtual int SortCompare(string value1, string value2) => (value1 ?? "").CompareTo(value2 ?? "");

        //編集で値が変更された場合に呼ばれます
        public virtual FileInfo ValueChanged(FileInfo orgFile, string newValue) => orgFile;

        //セルの描画処理
        public virtual void CellPainting(object sender, DataGridViewCellPaintingEventArgs e) { }

    } //class
}
