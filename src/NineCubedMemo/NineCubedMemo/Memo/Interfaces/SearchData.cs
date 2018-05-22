using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Interfaces
{
    public class SearchData
    {
        /// <summary>
        /// 検索文字列
        /// </summary>
        public string SearchString  { get; set; } = "";

        /// <summary>
        /// 置換文字列
        /// </summary>
        public string ReplaceString { get; set; } = "";

        /// <summary>
        /// 大文字・小文字を無視するかどうか true:無視する(大文字・小文字を区別する)
        /// </summary>
        public bool IgnoreCase      { get; set; } = false;
    }
}
