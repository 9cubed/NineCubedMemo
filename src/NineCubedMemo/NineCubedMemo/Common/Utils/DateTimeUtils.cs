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
        /// 日時を 2000/01/23 01:23:45.678 形式で返します
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dateDelimiter">日付のデリミタ</param>
        /// <returns></returns>
        public static string GetFullDateTimeString(DateTime dt, string dateDelimiter = null) {
            string s = dt.ToString("yyyy/MM/dd HH:mm:ss.fff");
            if (dateDelimiter != null) s = s.Replace("/", dateDelimiter);
            return s;
        }

        /// <summary>
        /// 日時を 2000/01/23 01:23:45 形式で返します
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dateDelimiter">日付のデリミタ</param>
        /// <returns></returns>
        public static string GetDateTimeString(DateTime dt, string dateDelimiter = null) {
            string s = dt.ToString("yyyy/MM/dd HH:mm:ss");
            if (dateDelimiter != null) s = s.Replace("/", dateDelimiter);
            return s;
        }

        /// <summary>
        /// 日付を 2000/01/23 形式で返します
        /// デリミタを空文字にすることにより、yyyyMMdd形式にすることもできます。
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dateDelimiter">日付のデリミタ</param>
        /// <returns></returns>
        public static string GetDateString(DateTime dt, string dateDelimiter = null) {
            string s = dt.ToString("yyyy/MM/dd");
            if (dateDelimiter != null) s = s.Replace("/", dateDelimiter);
            return s;
        }

        /// <summary>
        /// 月初の DateTime を返します 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetFirstDate(DateTime dt) => GetFirstDate(dt.Year, dt.Month);
        public static DateTime GetFirstDate(int year, int month) {
            return new DateTime(year, month, 1);
        }

        /// <summary>
        /// 月末の DateTime を返します
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetEndDate(DateTime dt) => GetEndDate(dt.Year, dt.Month);
        public static DateTime GetEndDate(int year, int month) {
            return new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 指定した日付が第何週目にあたるのかを返します
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetWeekNo(DateTime dt)
        {
            //1日を取得します
            var firstDate = GetFirstDate(dt);

            //1日の曜日を取得します
            int offset = (int)firstDate.DayOfWeek; //0:日曜日 ～ 6:土曜日 

            //第何週かを取得します
            int weekNo = (dt.Day + offset - 1) / 7 + 1;

            return weekNo;
        }

    } //class
}
