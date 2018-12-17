using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Dialogs.SaveDialog
{
    public interface ISaveDialog
    {
        /// <summary>
        /// 名前を付けて保存ダイアログを表示します
        /// </summary>
        /// <returns>選択されたパス</returns>
        string ShowDialog();
    }
}
