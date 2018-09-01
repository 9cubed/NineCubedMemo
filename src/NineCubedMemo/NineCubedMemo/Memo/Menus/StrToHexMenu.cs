using NineCubed.Common.Controls;
using NineCubed.Common.Files;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Menus
{
    public class StrToHexMenu : ToolStripMenuItem
    {
        IPlugin _plugin; //操作対象プラグイン。改行コードを持つプラグインの場合に、改行コードを取得するのに使用。

        public StrToHexMenu(string text, IPlugin plugin, TextBoxEx textbox, Encoding encoding)
        {
            this.Text = text;
            _plugin = plugin;

            //文字列 -> 16進数
            this.Click += (sender, e) => {
                try {
                    //改行コードを変更します
                    string newLineCode = "\r\n";
                    if (_plugin is INewLinePlugin) {
                        newLineCode = ((INewLinePlugin)_plugin).NewLineCode;
                    }
                    string targetText = textbox.SelectedText.Replace("\n", newLineCode);

                    byte[] byteArray = encoding.GetBytes(targetText);
                    textbox.SelectionStart = textbox.SelectionStart + textbox.SelectionLength;
                    textbox.SelectionLength = 0;

                    textbox.SelectedText = " " + BinaryUtils.ByteArrayToHex(byteArray);
                } catch (Exception) { }
            };
        }
    }
}
