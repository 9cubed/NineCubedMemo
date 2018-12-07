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
    /// <summary>
    /// ファイルリスト用グリッド
    /// </summary>
    public class FileListGrid : DataGridView
    {
        /// <summary>
        /// 画像キャッシュ用Dict
        /// </summary>
        Dictionary<string, Icon> _iconDict = new Dictionary<string, Icon>();

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
            //this.ReadOnly = true; //読み取り専用 -> 各列オブジェクトで指定するようにした
            this.RowTemplate.Height = 23;
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
        public void ShowFileList(string dirPath = null)
        {
            //行を全て削除します
            this.Rows.Clear();

            //パスが指定されていない場合は、現在のパスを表示対象のパスにします
            if (dirPath == null) {
                //カレントパスが未指定の場合は処理を抜けます
                if (CurrentPath == null) return;

                //現在のパスを表示対象のパスにします
                dirPath = CurrentPath;
            }

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
                //行データの生成
                var rowData = new object[this.ColumnCount];

                //行を追加します
                var rowIndex = this.Rows.Add(rowData);

                //行データを設定します
                SetRowData(rowIndex, path);
            }
        }

        //行データを設定します
        public void SetRowData(int rowIndex, string path)
        {
            //ファイル情報を取得します
            var fileInfo = new FileInfo(path);

            //行データの設定
            int colIndex = 0;
            foreach (IFileListColumn column in this.Columns) {
                this[colIndex, rowIndex].Value = column.ToString(fileInfo);
                colIndex++;
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

                //ファイルの種類に応じて画像を描画します
                var cellValue = this[e.ColumnIndex, e.RowIndex].Value.ToString();
                if (cellValue.Equals(FileKindColumn.KIND_DIR)) {
                    //フォルダの場合、フォルダの画像を描画します
                    e.Graphics.DrawImage(_imgFolder, e.CellBounds.Left + 1, e.CellBounds.Top + 1);

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
                e.Handled = true; //false のままだとデフォルトの描画処理が行われる
            }
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

        /// <summary>
        /// 編集で値が変更された場合に呼ばれます
        /// ファイル名カラムの場合は、ファイル名の変更などを行います
        /// </summary>
        /// <param name="orgFile"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        FileInfo ValueChanged(FileInfo orgFile, string newValue);
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
        /// <returns></returns>
        public FileInfo IsFile(FileInfo file) => file.Attributes.HasFlag(FileAttributes.Directory) ? null : file;

        //値の比較をします。ソートモードが Programmatic の場合のみ使用します
        public virtual int SortCompare(string value1, string value2) => (value1 ?? "").CompareTo(value2 ?? "");

        //編集で値が変更された場合に呼ばれます
        public virtual FileInfo ValueChanged(FileInfo orgFile, string newValue) => orgFile;
    }



    /// <summary>
    /// ファイル種別カラム
    /// </summary>
    public class FileKindColumn : AFileListColumn
    {
        public FileKindColumn() : base()
        {
            this.ReadOnly = true;
            this.HeaderText = "";
            this.Width = 22;
        }

        public const string KIND_DIR = "*DIR*";

        //表示する値を返します
        override
        public string ToString(FileInfo file) => IsFile(file) == null ? KIND_DIR : file.FullName;
    }



    /// <summary>
    /// パスカラム
    /// 必ず1列目に設定します
    /// </summary>
    public class PathColumn : AFileListColumn
    {
        public PathColumn() : base()
        {
            this.ReadOnly = true;
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
            if (FileUtils.IsFile(orgFile.FullName) == false) return orgFile; 

            //新旧の拡張子を取得します
            var oldExt = Path.GetExtension(orgFile.FullName).Replace(".", "");
            var newExt = newValue.Replace(".", ""); //入力値の先頭に「.」がある場合は削除する

            //変更がない場合は処理を抜けます
            if (oldExt.Equals(newExt)) return orgFile; 

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
                return orgFile;
            }

            return new FileInfo(newPath);

        }
    }



    /// <summary>
    /// ファイルサイズカラム
    /// </summary>
    public class FileSizeColumn : AFileListColumn
    {
        public FileSizeColumn() : base()
        {
            this.ReadOnly = true;
            this.HeaderText = "サイズ";
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.SortMode = DataGridViewColumnSortMode.Automatic; //ヘッダークリック時にソート(プログラム)するようにする
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) {
            if (IsFile(file) != null) {
                return StringUtils.GetStringByteSize(file.Length);
            } else {
                return "";
            }
        }

        override
        public int SortCompare(string sValue1, string sValue2)
        {
            var iValue1 = StringUtils.GetLongByteSize(sValue1?.ToString());
            var iValue2 = StringUtils.GetLongByteSize(sValue2?.ToString());

            //文字列が空の場合(フォルダの場合)は、-1 として扱う
            if (string.IsNullOrEmpty(sValue1)) iValue1 = -1;
            if (string.IsNullOrEmpty(sValue2)) iValue2 = -1;

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
    }

}
