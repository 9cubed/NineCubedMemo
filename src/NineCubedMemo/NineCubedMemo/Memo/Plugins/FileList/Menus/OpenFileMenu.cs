using NineCubed.Common.Controls.FileList;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Events;
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
        public OpenFileMenu(FileListGrid fileList)
        {
            this.Text = "開く";

            //クリックイベント
            this.Click += (sender, e) => {
                if (fileList.CurrentCell.RowIndex == -1) return; //ヘッダーダブルクリックは無視する

                //カレント行のパスを取得します。
                var path = fileList[0, fileList.CurrentCell.RowIndex].Value.ToString();

                //フォルダまたはファイル選択イベントを発生させます
                RaiseSelectedEvent(path);
            };
        }

        /// <summary>
        /// ファイルリストで選択されたフォルダまたはファイルの選択イベントを発生させます
        /// </summary>
        public void RaiseSelectedEvent(string path)
        {
            var pluginManager = PluginManager.GetInstance();

            if (FileUtils.IsFile(path)) {
                //ファイルの場合、ファイル選択イベントを発生させます
                var param = new FileSelectedEventParam { Path = path };
                pluginManager.GetEventManager().RaiseEvent(FileSelectedEventParam.Name, this, param);
            } else {
                //フォルダの場合、フォルダ選択イベントを発生させます
                var param = new DirSelectedEventParam { Path = path };
                pluginManager.GetEventManager().RaiseEvent(DirSelectedEventParam.Name, this, param);
            }
        }

    } //class
}
