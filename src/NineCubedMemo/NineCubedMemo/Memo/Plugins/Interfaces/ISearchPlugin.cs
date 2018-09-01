using NineCubed.Memo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Interfaces
{
    public interface ISearchPlugin : ISearchString
    {
        /// <summary>
        /// 検索する文字列を返します
        /// 検索する際に、現在選択中の文字列を返すのに使用
        /// </summary>
        /// <returns>検索する文字列</returns>
        string GetSearchString();
    }
}
