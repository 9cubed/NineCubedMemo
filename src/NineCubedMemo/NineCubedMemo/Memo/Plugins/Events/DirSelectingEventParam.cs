using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Events
{
    public class DirSelectingEventParam : EventParam
    {
        //イベント名
        public static string Name = "PluginEvent_DirSelecting";

        //選択中のディレクトリのパス
        public string Path { get; set; }
    }
}
