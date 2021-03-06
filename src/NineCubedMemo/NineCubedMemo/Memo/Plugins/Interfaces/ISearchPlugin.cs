﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Interfaces
{
    public interface ISearchPlugin
    {
        /// <summary>
        /// 検索する文字列を返します
        /// 検索する際に、現在選択中の文字列を返すのに使用します。
        /// </summary>
        /// <returns>検索する文字列</returns>
        string GetSearchString();

        /// <summary>
        /// 前方検索します。テキストの末尾へ向けて検索します。
        /// </summary>
        /// <param name="searchString">検索文字列</param>
        /// <param name="ignoreCase">大文字・小文字を無視します</param>
        /// <returns>見つかった位置</returns>
        int SearchForward();

        /// <summary>
        /// 後方検索します。テキストの先頭へ向けて検索します。
        /// </summary>
        /// <param name="searchString">検索文字列</param>
        /// <param name="ignoreCase">大文字・小文字を無視します</param>
        /// /// <returns>見つかった位置</returns>
        int SearchBackward();

        /// <summary>
        /// 前方置換します。
        /// </summary>
        /// <param name="searchString">検索文字列</param>
        /// <param name="replaceString">置換文字列</param>
        /// <param name="ignoreCase">大文字・小文字を無視します</param>
        /// <returns>見つかった位置</returns>
        int ReplaceForward();

        /// <summary>
        /// 後方置換します。
        /// </summary>
        /// <param name="searchString">検索文字列</param>
        /// <param name="replaceString">置換文字列</param>
        /// <param name="ignoreCase">大文字・小文字を無視します</param>
        /// <returns>見つかった位置</returns>
        int ReplaceBackward();

        /// <summary>
        /// 全置換します。
        /// </summary>
        /// <param name="searchString">検索文字列</param>
        /// <param name="replaceString">置換文字列</param>
        /// <param name="ignoreCase">大文字・小文字を無視します</param>
        void ReplaceAll();

    } //interface
}
