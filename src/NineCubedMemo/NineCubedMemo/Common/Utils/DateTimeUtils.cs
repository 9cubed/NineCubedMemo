using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Utils
{
    /// <summary>
    /// ファイル関連のユーティリティークラス
    /// </summary>
    public class DateTimeUtils
    {
        /// <summary>
        /// 日時を 2001-01-23 01:23:45.678 形式で返します
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dateDelimiter">日付のデリミタ</param>
        /// <returns></returns>
        public static string GetFullDateTimeString(DateTime dt, string dateDelimiter = null) {
            string s = dt.ToString("yyyy/MM/dd HH:mm:ss.fff");
            if (dateDelimiter != null) s = s.Replace("/", dateDelimiter);
            return s;
        }
    }
}
