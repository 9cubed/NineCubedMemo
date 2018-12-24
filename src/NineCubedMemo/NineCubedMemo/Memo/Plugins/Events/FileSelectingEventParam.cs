using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Events
{
    public class FileSelectingEventParam : EventParam
    {
        //イベント名
        public static string Name = "PluginEvent_FileSelecting";

        //選択中のファイルのパス
        public string Path { get; set; }
    }
}
