using NineCubed.Common.Controls;
using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Menus
{
    public class HexToStrMenu : ToolStripMenuItem
    {
        public HexToStrMenu(string text, TextBoxEx textbox, Encoding encoding)
        {
            this.Text = text;

            //16進数 -> 文字列
            this.Click += (sender, e) => {
                try {
                    byte[] byteArray = BinaryUtils.HexToByteArray(textbox.SelectedText);
                    textbox.SelectionStart = textbox.SelectionStart + textbox.SelectionLength;
                    textbox.SelectionLength = 0;
                    textbox.SelectedText = " " + encoding.GetString(byteArray);
                } catch (Exception) {
                    __.ShowErrorMsgBox("変換できませんでした。");
                }
            };
        }
    }
}
