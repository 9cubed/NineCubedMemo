using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Utils
{
    public class ListUtils
    {
        /// <summary>
        /// リストの空の要素を全て削除します
        /// </summary>
        /// <param name="list"></param>
        public static void RemoveAllEmpty(List<string> list)
        {
            if (list == null) return;
            list.RemoveAll(_ => string.IsNullOrEmpty(_));
            //list = list.Where(_ => string.IsNullOrEmpty(_) == false).ToList();
        }

        /// <summary>
        /// 文字列を分割します。
        /// 空の要素は含みません。StringSplitOptions.RemoveEmptyEntries
        /// </summary>
        /// <param name="separator">セパレーター</param>
        /// <param name="value">対象の文字列</param>
        /// <returns></returns>
        public static IList<string> Split(string separator, string value)
        {
            var list = new List<string>();
            if (StringUtils.IsNotEmpty(value?.Trim())) {
                list = value.Split(new string[]{separator}, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            return list;
        }

    } //class
}
