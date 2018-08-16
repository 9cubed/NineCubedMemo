using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Utils
{
    /// <summary>
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

        static string[] _hankakuDakutenList = {
            "ｳﾞ",
            "ｶﾞ", "ｷﾞ", "ｸﾞ", "ｹﾞ", "ｺﾞ",
            "ｻﾞ", "ｼﾞ", "ｽﾞ", "ｾﾞ", "ｿﾞ",
            "ﾀﾞ", "ﾁﾞ", "ﾂﾞ", "ﾃﾞ", "ﾄﾞ",
            "ﾊﾞ", "ﾋﾞ", "ﾌﾞ", "ﾍﾞ", "ﾎﾞ",
            "ﾊﾟ", "ﾋﾟ", "ﾌﾟ", "ﾍﾟ", "ﾎﾟ",
        };
        static string[] _zenkakuDakutenList = {
            "ヴ",
            "ガ", "ギ", "グ", "ゲ", "ゴ",
            "ザ", "ジ", "ズ", "ゼ", "ゾ",
            "ダ", "ヂ", "ヅ", "デ", "ド",
            "バ", "ビ", "ブ", "ベ", "ボ",
            "パ", "ピ", "プ", "ペ", "ポ",
        };
        
        /// <summary>
        /// 半角を全角にします。
        /// </summary>
        /// <param name="target">文字列</param>
        /// <returns></returns>
        public static string HankakuToZenkaku(string target) {
            //濁点、半濁点の変換
            for (int i = 0; i < _hankakuDakutenList.Length; i++) {
                target = target.Replace(_hankakuDakutenList[i], _zenkakuDakutenList[i]);
            }

            //上記以外の変換
            target = new string(target.Select(_ => HankakuToZenkaku(_)).ToArray());
            return target;
        }
        private static string _hankaku = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~｡｢｣､･ｦｧｨｩｪｫｬｭｮｯｰｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾉﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾔﾕﾖﾗﾘﾙﾚﾛﾜﾝﾞﾟ";
        private static string _zenkaku = "　！”＃＄％＆’（）＊＋，－．／０１２３４５６７８９：；＜＝＞？＠ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ［￥］＾＿｀ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ｛｜｝￣。「」、・ヲァィゥェォャュョッーアイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワン゛゜";

        /// <summary>
        /// 半角を全角にします。
        /// </summary>
        /// <param name="target">文字</param>
        /// <returns></returns>
        private static char HankakuToZenkaku(char target) {

            var index = _hankaku.IndexOf(target);
            if (index < 0) {
                return target;
            } else {
                return _zenkaku.Substring(index, 1)[0];
            }
        }

        /// <summary>
        /// 全角を半角にします。
        /// </summary>
        /// <param name="target">文字列</param>
        /// <returns></returns>
        public static string ZenkakuToHankaku(string target) {
            //濁点、半濁点の変換
            for (int i = 0; i < _hankakuDakutenList.Length; i++) {
                target = target.Replace(_zenkakuDakutenList[i], _hankakuDakutenList[i]);
            }

            //上記以外の変換
            target = new string(target.Select(_ => ZenkakuToHankaku(_)).ToArray());
            return target;
        }
        /// <summary>
        /// 全角を半角にします。
        /// </summary>
        /// <param name="target">文字</param>
        /// <returns></returns>
        public static char ZenkakuToHankaku(char target) {
            var index = _zenkaku.IndexOf(target);
            if (index < 0) {
                return target;
            } else {
                return _hankaku.Substring(index, 1)[0];
            }
        }

        /// <summary>
        /// 10進数 -> 16進数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DecimalToHex(string s) {
            if (Int64.TryParse(s, out Int64 value) == false) return null;//数値に変換できない場合
            return Convert.ToString(value, 16);
        }

        /// <summary>
        /// 16進数 -> 10進数
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static string HexToDecimal(string hex) {
            string result = null;
            try {
                result = Convert.ToInt64(hex, 16).ToString();
            } catch (Exception) {}
            return result;
        }

    } //class
}
