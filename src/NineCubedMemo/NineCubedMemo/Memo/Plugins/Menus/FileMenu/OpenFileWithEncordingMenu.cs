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
    public class OpenFileWithEncordingMenu : ToolStripMenuItem
    {
        PluginManager _pluginManager;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_pluginManager"></param>
        public OpenFileWithEncordingMenu(PluginManager pluginManager)
        {
            _pluginManager = pluginManager;

            this.Text = "開く（文字コード指定）";

            //サブメニューの追加
            this.DropDownItems.AddRange(
                new ToolStripItem[] {
                    CreateOpenWithEncMenu("Shift JIS"     , Encoding.GetEncoding(932)),  //Shift JIS
                    CreateOpenWithEncMenu("UTF-8"         , new UTF8Encoding(false)),    //UTF-8 BOMなし
                    CreateOpenWithEncMenu("UTF-8 BOM"     , new UTF8Encoding(true )),    //UTF-8 BOMあり
                    CreateOpenWithEncMenu("UTF-16 BOM(LE)", Encoding.GetEncoding(1200)), //UTF-16 LE
                    CreateOpenWithEncMenu("UTF-16 BOM(BE)", Encoding.GetEncoding(1201)), //UTF-16 BE
                    CreateOpenWithEncMenu("EUC-JP"        , Encoding.GetEncoding(51932)) //EUC-JP
                });
        }

        /// <summary>
        /// 開く（文字コード指定）メニューの作成
        /// </summary>
        /// <param name="text"></param>
        /// <param name="shortcutKeys"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private ToolStripMenuItem CreateOpenWithEncMenu(string text, Encoding encoding)
        {
            var menu = new ToolStripMenuItem(text);
                menu.Click += (sender, e) => { OpenWithEnc(encoding); };

            return menu;
        }

        /// <summary>
        /// 開く（文字コード指定） の Click された時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenWithEnc(Encoding encoding)
        {
            try {
                //開くダイアログのプラグインを取得します
                var openDialogPlugin = (IOpenDialog)_pluginManager.GetPlugin("open_dialog");

                //ファイルを開くダイアログを表示します
                var path = openDialogPlugin.ShowDialog();
                if (path == null) return;
                
                //プラグイン生成パラメーターを設定します
                var pluginCreateParam = new PluginCreateParam {
                    ["path"]      = path,     //選択されたパス
                    ["encoding"]  = encoding, //文字コード
                    ["is_binary"] = false     //テキストモード
                };

                //テキストエディタープラグインを生成します
                var plugin = (IFilePlugin)_pluginManager.CreatePluginInstance(typeof(TextEditorPlugin), pluginCreateParam, this);

            } catch (Exception ex) {
                __.ShowErrorMsgBox(ex);
            }
        }

    } //class
}
