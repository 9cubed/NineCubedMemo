using NineCubed.Common.Controls.FileList.Columns;
using NineCubed.Common.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FileListGrid_MouseMove);
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

            /* //この処理はコントロールの利用者側から行う
            //カラムの設定
            this.Columns.Add(new PathColumn          ()); //パスカラム(非表示)
            this.Columns.Add(new FileKindColumn      ());
            this.Columns.Add(new FileNameColumn      ());
            this.Columns.Add(new FileExtensionColumn ());
            this.Columns.Add(new FileSizeColumn      ());
            this.Columns.Add(new FileUpdateDateColumn());
            this.Columns.Add(new ImageSizeColumn     ());

            //各カラムに FileListGrid 本体を設定します
            foreach (var colums in this.Columns) ((IFileListColumn)colums).FileList = this;
            */

            //ファイル一覧の取得メソッドを設定します
            this.GetFileList = (dirPath) => { 
                //フォルダとファイルの一覧を取得します
                IList<string> pathList = null;
                try {
                    pathList = FileUtils.GetDirFileList(dirPath);
                } catch (Exception) {
                    //アクセス権限がない場合
                    return new List<string>();
                }
                return pathList ;
            };
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
        internal Image _imgFolder;

        /// <summary>
        /// 現在のフォルダのパス
        /// </summary>
        public string CurrentPath {
            get;
            private set;
        }

        /// <summary>
        /// マウスダウン処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            //クリックされた位置のセルを取得します
            var hitTestInfo = this.HitTest(e.X, e.Y);
            var col = hitTestInfo.ColumnIndex;
            var row = hitTestInfo.RowIndex;

            //セル以外がクリックされた場合は、
            //既存のマウスダウンの処理を行います
            if (col == -1 || row == -1) {
                base.OnMouseDown(e);
                return;
            }

            //選択中のセルをクリックした場合は、何も処理しない
            if (this.SelectedCells.Contains(this[col, row])) {
                return;
            }

            //既存のマウスダウンの処理を行います
            base.OnMouseDown(e);
        }

        /// <summary>
        /// ファイル一覧を返すメソッド
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public delegate IList<string> GetFileListMethod(string dirPath); //型
        public GetFileListMethod GetFileList { get; set; }               //実体

        /// <summary>
        /// ファイルリストを表示します
        /// </summary>
        /// <param name="dirPath">フォルダのパス</param>
        public void ShowFileList(string dirPath = null)
        {
            //行を全て削除します
            this.Rows.Clear();

            //列の初期化処理を実行します
            foreach (IFileListColumn column in this.Columns) {
                column.ShowFileListBefore();
            }

            //パスが指定されていない場合は、現在のパスを表示対象のパスにします
            if (dirPath == null) {
                //カレントパスが未指定の場合は処理を抜けます
                if (this.CurrentPath == null) return;

                //現在のパスを表示対象のパスにします
                dirPath = this.CurrentPath;
            } else {
                //パスが指定されている場合は、パスを保持します
                this.CurrentPath = dirPath;
            }

            //フォルダとファイルの一覧を取得します
            var pathList = this.GetFileList(this.CurrentPath);

            //ファイル一覧をループして、行を追加します
            foreach (var path in pathList) {
                //行データの生成
                var rowData = new object[this.ColumnCount];

                //行を追加します(データはまだ空)
                var rowIndex = this.Rows.Add(rowData);

                //行データを設定します
                SetRowData(rowIndex, path);
            }

            //列の解放処理を実行します
            foreach (IFileListColumn column in this.Columns) {
                column.ShowFileListAfter();
            }

            //カーソルを一番上に移動します
            if (this.RowCount >= 1 && this.ColumnCount >= 2) {
                //編集可能なカラムのインデックスを取得します
                var colIndex = GetEditableColumnIndex();
                try {
                    if (colIndex >= 0) {
                        this.CurrentCell = this[colIndex, 0];
                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
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
        /// 列データを更新します。
        /// 
        /// 列クラスの別スレッドから呼ばれます。
        /// メインスレッドで ShowFileList() を実行中に本メソッドで呼ばれると、
        /// RowCount の不一致やデータの整合性が取れなくなるため、
        /// Invoke() を使って、メインスレッドに実行させています。
        /// これで、ShowFileList() の実行中には、本メソッドの処理は実行されなくなります。
        /// </summary>
        /// <param name="column"></param>
        public void UpdateColumn(IFileListColumn column)
        {
            this.Invoke((MethodInvoker)(() => {
                        
                var colIndex = GetColumnIndex(column);
                if (colIndex == -1) return;

                for (int rowIndex = 0; rowIndex < this.RowCount; rowIndex++) {

                    //ファイル情報を取得します
                    var path = this[0, rowIndex].Value.ToString();
                    var fileInfo = new FileInfo(path);

                    //セルに値を設定します
                    this[colIndex, rowIndex].Value = column.ToString(fileInfo);
                }

            }));
        }

        /// <summary>
        /// 指定されたカラムの index を返します
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private int GetColumnIndex(IFileListColumn column)
        {
            for (int colIndex = 0; colIndex < this.ColumnCount; colIndex++) {
                if (this.Columns[colIndex] == column) {
                    return colIndex;
                }
            }
            return -1;
        }

        /// <summary>
        /// 編集可能な最初のカラムの index を返します
        /// </summary>
        /// <returns></returns>
        private int GetEditableColumnIndex()
        {
            for (int colIndex = 0; colIndex < this.ColumnCount; colIndex++) {
                if (this.Columns[colIndex].ReadOnly == false) {
                    return colIndex;
                }
            }
            return -1;
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
            ((AFileListColumn)this.Columns[e.ColumnIndex]).CellPainting(sender, e);
        }

        /// <summary>
        /// マウスムーブイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileListGrid_MouseMove(object sender, MouseEventArgs e)
        {
            //マウスボタンが押されていない場合は処理しない
            if (e.Button == MouseButtons.None) return;

            //クリックされた位置のセルを取得します
            var hitTestInfo = this.HitTest(e.X, e.Y);
            var row = hitTestInfo.RowIndex;
            if (row == -1) return; //行が選択されている場合は、処理しない

            //選択されているファイルリストを取得します
            var pathList = GetSelectedPathList().ToArray();
            if (pathList.Count() == 0) return;

            //ドラッグを開始します
            var dataObj = new DataObject(DataFormats.FileDrop, pathList);
            var effect = DragDropEffects.Copy; // | DragDropEffects.Move;
            this.DoDragDrop(dataObj, effect);
        }

        /// <summary>
        /// 選択されているパスリストを返します
        /// </summary>
        /// <returns></returns>
        private List<string> GetSelectedPathList()
        {
            var list = new List<string>();
            foreach(DataGridViewCell cell in this.SelectedCells) {
                //選択されている行のパスを取得します
                var path = this[0, cell.RowIndex].Value?.ToString();
                if (string.IsNullOrEmpty(path)) continue;

                //リストに追加されていない場合は、リストに追加します
                if (list.IndexOf(path) == -1)list.Add(path);
            }
            
            return list;
        }

    } //class 
}
