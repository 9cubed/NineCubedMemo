using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Interfaces
{
    public interface IRefreshPlugin
    {
        /// <summary>
        /// 最新の情報に更新します
        /// </summary>
        void RefreshData();
    }
}
