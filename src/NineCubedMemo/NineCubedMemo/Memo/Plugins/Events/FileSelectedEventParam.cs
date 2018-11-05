using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Events
{
    public class FileSelectedEventParam : EventParam
    {
        //イベント名
        public static string Name = "PluginEvent_FileSelected";

        //選択されたファイルのパス
        public string Path { get; set; }
    }
}
