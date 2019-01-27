using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Interfaces
{
    public interface IPathPlugin
    {
        /// <summary>
        /// パスを返します
        /// </summary>
        /// <returns></returns>
        string GetPath();
    }
}
