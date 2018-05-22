using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Interfaces
{
    public interface ISearchString
    {
        /// <summary>
        /// 前方検索します。テキストの末尾へ向けて検索します。
        /// </summary>
        /// <param name="searchString">検索文字列</param>
        /// <param name="ignoreCase">大文字・小文字を無視します</param>
        /// <returns>見つかった位置</returns>
        int SearchForward(string searchString, bool ignoreCase);

        /// <summary>
        /// 後方検索します。テキストの先頭へ向けて検索します。
        /// </summary>
        /// <param name="searchString">検索文字列</param>
        /// <param name="ignoreCase">大文字・小文字を無視します</param>
        /// /// <returns>見つかった位置</returns>
        int SearchBackward(string searchString, bool ignoreCase);

        /// <summary>
        /// 前方置換します。
        /// </summary>
        /// <param name="searchString">検索文字列</param>
        /// <param name="replaceString">置換文字列</param>
        /// <param name="ignoreCase">大文字・小文字を無視します</param>
        /// <returns>見つかった位置</returns>
        int ReplaceForward(string searchString, string replaceString, bool ignoreCase);

        /// <summary>
        /// 後方置換します。
        /// </summary>
        /// <param name="searchString">検索文字列</param>
        /// <param name="replaceString">置換文字列</param>
        /// <param name="ignoreCase">大文字・小文字を無視します</param>
        /// <returns>見つかった位置</returns>
        int ReplaceBackward(string searchString, string replaceString, bool ignoreCase);

        /// <summary>
        /// 全置換します。
        /// </summary>
        /// <param name="searchString">検索文字列</param>
        /// <param name="replaceString">置換文字列</param>
        /// <param name="ignoreCase">大文字・小文字を無視します</param>
        void ReplaceAll(string searchString, string replaceString, bool ignoreCase);

    } //interface
}
