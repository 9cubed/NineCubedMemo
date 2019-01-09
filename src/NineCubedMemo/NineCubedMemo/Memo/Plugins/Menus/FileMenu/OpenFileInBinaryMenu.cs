using NineCubed.Memo.Plugins.Dialogs.OpenDialog;
using NineCubed.Memo.Plugins.Interfaces;
using NineCubed.Memo.Plugins.TextEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.Menus.FileMenu
{
    /// <summary>
    /// 「開く（バイナリ形式）」のメニュー
    /// </summary>
    public class OpenFileInBinaryMenu : ToolStripMenuItem
    {
        PluginManager _pluginManager;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_pluginManager"></param>
        public OpenFileInBinaryMenu(PluginManager pluginManager)
        {
            this.Text = "開く（バイナリ形式）";
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
                //開くダイアログのプラグインを取得します
                var openDialogPlugin = (IOpenDialog)_pluginManager.GetPlugin("open_dialog");

                //ファイルを開くダイアログを表示します
                var path = openDialogPlugin.ShowDialog();
                if (path == null) return;

                //プラグイン生成パラメーターを設定します
                var pluginCreateParam = new PluginCreateParam {
                    ["path"]      = path, //選択されたパス
                    ["encoding"]  = null, //文字コード
                    ["is_binary"] = true  //バイナリーモード
                };

                //テキストエディタープラグインを生成します
                var plugin = (IFilePlugin)_pluginManager.CreatePluginInstance(typeof(TextEditorPlugin), pluginCreateParam, this);

            } catch (Exception ex) {
                __.ShowErrorMsgBox(ex);
            }
        }

    } //class
}
