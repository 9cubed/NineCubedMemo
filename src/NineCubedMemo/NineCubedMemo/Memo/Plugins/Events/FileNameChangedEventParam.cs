using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Events
{
    public class FileNameChangedEventParam : EventParam
    {
        //イベント名
        public static string Name = "PluginEvent_FileNameChanged";

        //変更前のパス
        public string OldPath { get; set; }

        //変更後のパス
        public string NewPath { get; set; }
    }
}
