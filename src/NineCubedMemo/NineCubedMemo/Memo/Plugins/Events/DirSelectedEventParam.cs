using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Events
{

    public class DirSelectedEventParam : EventParam
    {
        //イベント名
        public static string Name = "PluginEvent_DirSelected";

        //選択されたディレクトリのパス
        public string Path { get; set; }
    }

}
