using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Utils
{
    /// <summary>
    /// ver1.0.4
    /// ファイル関連のユーティリティークラス
    /// </summary>
    public class FileUtils
    {
        /// <summary>
        /// ファイルを読み込んでバイト配列で返します
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] LoadFileToByteArray(string path) {
            byte[] byteArray = null;
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                byteArray = new byte[stream.Length];
                stream.Read(byteArray, 0, byteArray.Length);
            }
            return byteArray;
        }

        /// <summary>
        /// ファイルが読み取り専用かどうかを返します
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns>true:読み取り専用</returns>
        public static bool IsReadOnly(string path) {
            return File.GetAttributes(path).HasFlag(FileAttributes.ReadOnly);
        }

    } //class
}
