using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.FileList.Menus
{
    public class OpenFileMenu : ToolStripMenuItem
    {
        public OpenFileMenu(FileListPlugin fileList)
        {
            this.Text = "開く";

            //クリックイベント
            this.Click += (sender, e) => {
                if (fileList.CurrentCell.RowIndex == -1) return; //ヘッダーダブルクリックは無視する

                //カレント行のパスを取得します。
                var path = fileList[0, fileList.CurrentCell.RowIndex].Value.ToString();

                //フォルダまたはファイル選択イベントを発生させます
                fileList.RaiseSelectedEvent(path);
            };
        }

    } //class
}
