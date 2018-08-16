using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Utils
{
    /// <summary>
    /// バイナリ関連のユーティリティークラス
    /// </summary>
    public class BinaryUtils
    {
        /// <summary>
        /// スペース区切りの16進数の文字列をバイト配列にして返します
        /// 
        /// "A0 A1 A2"  ->  [0]=0xa0  [1] = 0xa1  [2] = 0xa2
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] HexToByteArray(string hex) {
            //テキスト -> 16進数の配列
            string[] hexArray = hex.Split(new char[]{' ', '\t', '\r', '\n'});

            //16進数の配列 -> 数値のリスト
            var byteList = new List<byte>();
            for (int i = 0; i < hexArray.Length; i++) {
                if (string.IsNullOrEmpty(hexArray[i]) == false) {
                    //文字列(16進数) -> 数値
                    byte value = Convert.ToByte(hexArray[i], 16);

                    //リストに数値を追加します
                    byteList.Add(value);
                }
            } //TODO: hexArrayの空要素の数を数えて、byteArrayを確保する。byteListを使わないようにする

            //数値のリスト -> バイトの配列
            byte[] byteArray = byteList.ToArray();
            return byteArray;
        }

        /// <summary>
        /// バイト配列をスペース区切りの16進数の文字列にして返します
        /// 
        /// [0]=0xa0  [1] = 0xa1  [2] = 0xa2  ->  "A0 A1 A2"
        /// </summary>
        /// <param name="byteArray"></param>
        /// <param name="maxCol"></param>
        /// <returns></returns>
        public static string ByteArrayToHex(byte[] byteArray, int maxCol = 0) {
            if (byteArray == null || byteArray.Length == 0) return "";

            //バイト配列 -> 16進数の文字列
            var sb = new StringBuilder();
            for (int i = 0; i < byteArray.Length; i++) {
                //10進数 -> 16進数
                var hex = Convert.ToString(byteArray[i], 16);
                if (byteArray[i] < 16) hex = "0" + hex; //先頭0埋め

                //テキストに追加
                sb.Append(hex);

                //指定された数で改行を追加します
                //行の途中はスペースを追加します
                if (maxCol > 0 && ((i + 1) % maxCol == 0)) {
                    sb.Append("\n");
                } else {
                    //区切らない場合は毎回スペースを追加します
                    sb.Append(" ");
                }
            }

            return sb.ToString().Trim();
        }

    } //class
}
