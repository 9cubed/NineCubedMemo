using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Events
{
    /// <summary>
    /// イベント用パラメーター
    /// </summary>
    public class EventParam
    {
        /// <summary>
        /// true:イベントを処理済みとし、他のプラグインへイベントを通知しないようにする。
        /// </summary>
        public bool Handled = false;

    } //class
}
