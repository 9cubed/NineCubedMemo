using NineCubed.Memo.Plugins.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileInfoManager.Events
{
    public class FileInfoSelectingEventParam : EventParam
    {
        //イベント名
        public static string Name = "PluginEvent_FileInfoSelecting";

        //選択されたファイルの ID
        public string ID { get; set; }

    }
}
