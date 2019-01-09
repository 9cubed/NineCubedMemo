using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Common.Utils
{
    public class MsgBoxUtils
    {
        /// <summary>
        /// 保存確認のメッセージボックスを表示します
        /// </summary>
        /// <returns></returns>
        public static DialogResult ShowMsgBoxToConfirmSaving() {
            return MessageBox.Show("変更されています。保存しますか？", "確認", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }


    } //class
} 
