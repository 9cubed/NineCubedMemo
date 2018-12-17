using NineCubed.Memo.Plugins.Dialogs.SaveDialog;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.Menus.FileMenu
{
    /// <summary>
    /// 「上書き保存」のメニュー
    /// </summary>
    public class SaveFileMenu : ToolStripMenuItem
    {
        PluginManager _pluginManager;
        bool _isShowDialog = false; //ダイアログを表示するかどうか true:名前を付けて保存ダイアログを表示

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_pluginManager"></param>
        /// <param name="_pluginManager">true:上書き false:名前を付けて保存</param>
        public SaveFileMenu(PluginManager pluginManager, bool isOverWrite)
        {
            this.Click += MenuClick;
            _pluginManager = pluginManager;

            if (isOverWrite) {
                //上書き
                this.Text = "上書き保存";
                this.ShortcutKeys = Keys.Control | Keys.S;
                _isShowDialog = false;
            } else {
                //名前を付けて保存
                this.Text = "名前を付けて保存";
                _isShowDialog = true;
            }
        }
        
        /// <summary>
        /// メニュークリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuClick(object sender, EventArgs e)
        {
            try {
                //テキストファイルに保存します
                SaveFile();
            } catch (Exception ex) {
                __.ShowErrorMsgBox(ex);
            }
        }

        /// <summary>
        /// テキストファイルを保存します
        /// </summary>
        private void SaveFile()
        {
            if (_pluginManager.ActivePlugin is IFilePlugin plugin) {

                //名前を付けて保存ダイアログを表示します
                if (_isShowDialog) {

                    //保存ダイアログのプラグインを取得します
                    var saveDialogPlugin = (ISaveDialog)_pluginManager.GetPlugin("save_dialog");

                    //保存ダイアログを表示します
                    var path = saveDialogPlugin.ShowDialog();
                    if (path == null) return;

                    //選択されたファイルのパスを保持します
                    plugin.TargetFile.Path = path;
                }

                //アクティブプラグインのファイルを保存します
                plugin.SaveFile();
            }
        }


    } //class
}
