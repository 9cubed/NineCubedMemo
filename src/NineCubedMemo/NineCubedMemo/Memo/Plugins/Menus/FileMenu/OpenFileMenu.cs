using NineCubed.Common.Files;
using NineCubed.Memo.Plugins.Dialogs.OpenDialog;
using NineCubed.Memo.Plugins.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.Menus.FileMenu
{
    public class OpenFileMenu : ToolStripMenuItem
    {
        PluginManager _pluginManager;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_pluginManager"></param>
        public OpenFileMenu(PluginManager pluginManager)
        {
            this.Text = "開く";
                                
            this.ShortcutKeys = Keys.Control | Keys.O;
            this.Click += MenuClick;

            _pluginManager = pluginManager;
        }

        /// <summary>
        /// メニュークリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuClick(object sender, EventArgs e)
        {
            try {
                //ファイルをプラグインで開きます
                OpenFile();
            } catch (Exception ex) {
                __.ShowErrorMsgBox(ex);
            }
        }

        /// <summary>
        /// ファイルをプラグインで開きます
        /// </summary>
        private void OpenFile()
        {
            //開くダイアログのプラグインを取得します
            var openDialogPlugin = (IOpenDialog)_pluginManager.GetPlugin("open_dialog");

            //ファイルを開くダイアログを表示します
            var path = openDialogPlugin.ShowDialog();
            if (path == null) return;

            //ファイルが存在しない場合は処理しない
            if (File.Exists(path) == false) return;

            //ファイル選択イベントを発生させます
            var param = new FileSelectedEventParam { Path = path };
            _pluginManager.GetEventManager().RaiseEvent(FileSelectedEventParam.Name, this, param);
        }

    } //class
}
