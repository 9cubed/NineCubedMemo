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
    public class ValueColumn : AFileListColumn, IFileListColumnForFileInfo
    {
        /// <summary>
        /// DB接続用オブジェクト
        /// </summary>
        FileDB _db;

        /// <summary>
        /// 評価に応じた背景色
        /// </summary>
        public Color[] ValueColor { get; set; } = new Color[11]; //インデックス:評価(0 - 10)

        public ValueColumn(FileDB db) : base()
        {
            _db = db;
            this.ReadOnly = false; //編集可能
            this.HeaderText = "評価";
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.SortMode = DataGridViewColumnSortMode.Automatic; //ヘッダークリック時にソート(プログラム)するようにする
            this.Width = 40;
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) => "";
        public string ToString(FileData file) => file.value.ToString();

        /// <summary>
        /// ソートします
        /// </summary>
        /// <param name="sValue1"></param>
        /// <param name="sValue2"></param>
        /// <returns></returns>
        override
        public int SortCompare(string sValue1, string sValue2)
        {
            var iValue1 = StringUtils.ToInt(sValue1?.ToString());
            var iValue2 = StringUtils.ToInt(sValue2?.ToString());

            //文字列が空の場合(フォルダの場合)は、-1 として扱う
            if (string.IsNullOrEmpty(sValue1)) iValue1 = -1;
            if (string.IsNullOrEmpty(sValue2)) iValue2 = -1;

            return iValue1 > iValue2 ? 1 : -1;
        }

        //編集で値が変更された場合に呼ばれます
        public FileData ValueChanged(FileData orgFileData, string newValue)
        {
            //評価を更新します
            using(var connection = _db.GetConnection()) {
                FileDataDao.UpdateField(connection, orgFileData.id, "value", newValue);
            }

            orgFileData.value = StringUtils.ToInt(newValue);
            return orgFileData;
        }

        // データに応じた背景色を返します
        public Color GetBackColor(FileData fileData) {
            return this.ValueColor[fileData.value];
        }

    } //class
}
