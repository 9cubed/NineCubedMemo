using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Events
{
    /*
    public interface ITitleChangedEvent
    {
        void PluginEvent_TitleChanged(EventParam param, object sender);
    }
    */

    public class TitleChangedEventParam : EventParam
    {
        //イベント名。イベントの発生、イベントハンドラーの追加、イベントハンドラーのメソッド名として使用します
        public static string Name = "PluginEvent_TitleChanged";

        //タイトルが変更されたプラグイン
        public IPlugin Plugin { get; set; }
    }
}
