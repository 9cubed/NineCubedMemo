using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Dialogs.OpenDialog
{
    public interface IOpenDialog
    {
        /// <summary>
        /// 開くダイアログを表示します
        /// </summary>
        /// <returns>選択されたパス</returns>
        string ShowDialog();
    }
}
