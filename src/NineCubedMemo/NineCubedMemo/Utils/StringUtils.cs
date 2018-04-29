using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Utils
{
    /// <summary>
    /// ver1.0.4
    /// 文字列操作用ユーティリティークラス
    /// </summary>
    public class StringUtils
    {
        /// <summary>
        /// 文字列中の特定の文字を数えます
        /// </summary>
        /// <param name="target"></param>
        /// <param name="countChar">数える文字</param>
        /// <returns></returns>
        public static int CountChar(string target, char countChar) {
            if (target == null) return 0;
            //return target.Where(_ => _ == countChar).Count(); //LINQだと遅い。

            int count = 0;
            for (int i = 0; i < target.Length; i++) {
                if (target[i] == countChar) count++;
            }
            return count;
        }

        /// <summary>
        /// 文字列中に特定の文字があるかどうかを返します
        /// </summary>
        /// <param name="target"></param>
        /// <param name="searchChar"></param>
        /// <returns></returns>
        public static bool ExistsChar(string target, char searchChar) {
            if (target == null) return false;
            for (int i = 0; i < target.Length; i++) {
                if (target[i] == searchChar) return true;
            }
            return false;
        }

    } //class
}
