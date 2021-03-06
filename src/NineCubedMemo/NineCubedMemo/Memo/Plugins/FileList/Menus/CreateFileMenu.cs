﻿using NineCubed.Common.Controls.FileList;
using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.FileList.Menus
{
    public class CreateFileMenu : ToolStripMenuItem
    {
        public CreateFileMenu(FileListGrid fileList)
        {
            this.Text = "新規作成（ファイル）";

            //クリックイベント
            this.Click += (sender, e) => {
                //フォルダ作成
                var path = FileUtils.CreateNewTextFile(fileList.CurrentPath);

                //行データの生成
                var rowData = new object[fileList.ColumnCount];

                //行を追加します
                var rowIndex = fileList.Rows.Add(rowData);

                //行データを設定します
                fileList.SetRowData(rowIndex, path);

                //追加した行にカレントをします
                fileList.CurrentCell = fileList[fileList.CurrentCell.ColumnIndex, rowIndex];
            };
        }

    } //class
}
