using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Common.Controls.FileList
{
    public class FileListGrid : DataGridView
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileListGrid()
        {
            InitializeComponent();
        }

        /// <summary>
        /// (自動生成)
        /// </summary>
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // FileListGrid
            // 
            this.RowTemplate.Height = 21;
            this.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.FileListGrid_CellPainting);
            this.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.FileListGrid_SortCompare);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            this.AllowUserToAddRows    = false; //追加禁止
            this.AllowUserToDeleteRows = false; //削除禁止
            this.AllowUserToResizeRows = false; //リサイズ禁止
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ReadOnly = true; //読み取り専用
            this.RowTemplate.Height = 21;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;  //行選択
            this.RowHeadersVisible = false; //行ヘッダの列を非表示にする
            //grid.RowHeadersWidth = 20; //行ヘッダの幅を狭くする

            //カラムの設定
            this.Columns.Add(new PathColumn { Visible = false }); //非表示カラム
            this.Columns.Add(new FileKindColumn());
            this.Columns.Add(new FileNameColumn());
            this.Columns.Add(new FileExtensionColumn());
            this.Columns.Add(new FileSizeColumn());
            this.Columns.Add(new FileUpdateDateColumn());
        }

        /// <summary>
        /// 画像を設定します
        /// </summary>
        /// <param name="imgFolder"></param>
        public void SetImage(Image imgFolder)
        {
            _imgFolder = imgFolder;
        }

        /// <summary>
        /// フォルダ画像
        /// </summary>
        Image _imgFolder;

        /// <summary>
        /// 現在のフォルダのパス
        /// </summary>
        public string CurrentPath {
            get;
            private set;
        }

        /// <summary>
        /// ファイルリストを表示します
        /// </summary>
        /// <param name="dirPath">フォルダのパス</param>
        public void ShowFileList(string dirPath)
        {
            //行を全て削除します
            this.Rows.Clear();

            //パスを保持します
            CurrentPath = dirPath;

            //フォルダとファイルの一覧を取得します
            IList<string> pathList = null;
            try {
                pathList = FileUtils.GetDirFileList(dirPath);
            } catch (Exception) {
                //アクセス権限がない場合
                return;
            }

            //ファイル一覧をループして、行を追加します
            foreach (var path in pathList) {

                //ファイル情報を取得します
                var fileInfo = new FileInfo(path);

                //行データの生成
                var rowData = new object[this.ColumnCount];

                //行データの設定
                int i = 0;
                foreach (IFileListColumn column in this.Columns) {
                    rowData[i] = column.ToString(fileInfo);
                    i++;
                }

                //行を追加します
                this.Rows.Add(rowData);
            }
        }


        /// <summary>
        /// グリッドのソート時に、値の比較をするセル毎に呼ばれます
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileListGrid_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            //ソートします
            e.SortResult = ((IFileListColumn)e.Column).SortCompare(
                e.CellValue1?.ToString(),
                e.CellValue2?.ToString());
            e.Handled = true;
        }

        /// <summary>
        /// 描画イベント
        /// 再描画が必要な場合に呼ばれます
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileListGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (this.Columns[e.ColumnIndex] is FileKindColumn && e.RowIndex >= 0) {
                //再描画の必要な場所がファイル種別カラムの場合

                //セル全体を白で描画します
                e.Graphics.FillRectangle(Brushes.White, e.CellBounds.X + 0, e.CellBounds.Y - 1, e.CellBounds.Width, e.CellBounds.Height);

                //フォルダの場合はフォルダの画像を描画します
                if (this[e.ColumnIndex, e.RowIndex].Value.ToString().Equals("D")) {
                    e.Graphics.DrawImage(_imgFolder, e.CellBounds.Left + 1, e.CellBounds.Top + 1);
                }

                //罫線を描画します
                e.Graphics.DrawRectangle(Pens.Gray, e.CellBounds.X - 1, e.CellBounds.Y - 1, e.CellBounds.Width, e.CellBounds.Height);

                //描画済みにします
                e.Handled = true; //false のままだとデフォルトの描画処理が行われる
            }
        }

    } //class 

    /// <summary>
    /// ファイルリスト用のカラムインターフェース
    /// </summary>
    public interface IFileListColumn
    {
        /// <summary>
        /// 指定されたファイル情報を元にして、表示する値を返します
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        string ToString(FileInfo file);

        /// <summary>
        /// ソート時に値の比較をします
        /// ソートモードが Programmatic の場合のみ使用します
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        int SortCompare(string value1, string value2);
    }



    /// <summary>
    /// ファイルリストカラム
    /// </summary>
    public abstract class AFileListColumn : DataGridViewColumn, IFileListColumn
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AFileListColumn()
        {
            this.CellTemplate = new DataGridViewTextBoxCell();
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.SortMode = DataGridViewColumnSortMode.Automatic; //ヘッダークリック時にソートするようにする
        }

        //表示する値を返します
        public abstract string ToString(FileInfo file);

        /// <summary>
        /// 指定されたファイルがフォルダ出ない場合はファイルをそのまま返します。
        /// フォルダの場合はnullを返します
        /// </summary>
        /// <param name="file"></param>
        /// <returns>true:フォルダ false:ファイル</returns>
        public FileInfo IsFile(FileInfo file) => file.Attributes.HasFlag(FileAttributes.Directory) ? null : file;

        //値の比較をします。ソートモードが Programmatic の場合のみ使用します
        public virtual int SortCompare(string value1, string value2) => (value1 ?? "").CompareTo(value2 ?? "");
    }



    /// <summary>
    /// ファイル種別カラム
    /// </summary>
    public class FileKindColumn : AFileListColumn
    {
        public FileKindColumn() : base()
        {
            this.HeaderText = "";
            this.Width = 22;
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) => IsFile(file) == null ? "D" : "";
    }



    /// <summary>
    /// パスカラム
    /// 必ず1列目に設定します
    /// </summary>
    public class PathColumn : AFileListColumn
    {
        public PathColumn() : base()
        {
            this.HeaderText = "パス";
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) => file.FullName;
    }



    /// <summary>
    /// ファイル名カラム
    /// </summary>
    public class FileNameColumn : AFileListColumn
    {
        public FileNameColumn() : base()
        {
            this.HeaderText = "ファイル名";
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) => file.Name;
    }



    /// <summary>
    /// 拡張子カラム
    /// </summary>
    public class FileExtensionColumn : AFileListColumn
    {
        public FileExtensionColumn() : base()
        {
            this.HeaderText = "拡張子";
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) => IsFile(file)?.Extension;
    }



    /// <summary>
    /// ファイルサイズカラム
    /// </summary>
    public class FileSizeColumn : AFileListColumn
    {
        public FileSizeColumn() : base()
        {
            this.HeaderText = "サイズ";
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.SortMode = DataGridViewColumnSortMode.Automatic; //ヘッダークリック時にソート(プログラム)するようにする
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) => IsFile(file)?.Length.ToString();

        override
        public int SortCompare(string sValue1, string sValue2)
        {
            long.TryParse(sValue1?.ToString(), out long iValue1);
            long.TryParse(sValue2?.ToString(), out long iValue2);
            return iValue1 > iValue2 ? 1 : -1;
        }
    }



    /// <summary>
    /// 更新日時カラム
    /// </summary>
    public class FileUpdateDateColumn : AFileListColumn
    {
        public FileUpdateDateColumn() : base()
        {
            this.HeaderText = "更新日時";
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) {
            return 
                file.LastWriteTime.ToShortDateString() + " " + 
                file.LastWriteTime.ToShortTimeString();
        }
    }

}
