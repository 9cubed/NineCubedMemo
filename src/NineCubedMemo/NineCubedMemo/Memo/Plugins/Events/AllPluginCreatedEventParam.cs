using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Events
{
    /// <summary>
    /// プラグインローダーにより、plugin_list.ini の全てのプラグインが生成された際のイベントです。
    /// </summary>
    public class AllPluginCreatedEventParam : EventParam
    {
        //イベント名。イベントの発生、イベントハンドラーの追加、イベントハンドラーのメソッド名として使用します
        public static string Name = "PluginEvent_AllPluginCreated";
    }

}
