using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Interfaces
{
    //TODO TextEditorPluginでしか使わないので削除するかも。
    public interface IEncodingPlugin
    {
        /// <summary>
        /// エンコーディング(文字コード)
        /// </summary>
        Encoding TextEncoding { get; set; }
    }
}
