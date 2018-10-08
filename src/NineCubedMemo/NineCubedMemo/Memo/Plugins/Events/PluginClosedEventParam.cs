using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Events
{
    /*
    public interface IPluginClosedEvent
    {
        void PluginEvent_PluginClosed(EventParam param, object sender);
    }
    */

    /// <summary>
    /// プラグイン終了時に発生するイベントのパラメーターです。
    /// 
    /// メニューの「閉じる」などでプラグインを終了された時に発生するイベントです。
    /// このイベントの中では、まだ終了するプラグインにアクセスすることができます。
    /// 
    /// タブを削除した際に、紐づくプラグインを閉じるのではなく、
    /// プラグインを閉じられたら、タブがそれをイベントで検知して、紐づくタブを削除するようにしています。
    /// プラグインを閉じるのは、タブだけでなく、メインフォームのメニューの「閉じる」などでも行えるため、
    /// このような流れてしています。
    /// </summary>
    public class PluginClosedEventParam : EventParam
    {
        //イベント名。イベントの発生、イベントハンドラーの追加、イベントハンドラーのメソッド名として使用します
        public static string Name = "PluginEvent_PluginClosed";

        //クローズされたプラグイン
        public IPlugin Plugin { get; set; }
    }

}
