using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Utils
{
    /// <summary>
    /// ファイル関連のユーティリティークラス
    /// </summary>
    public class FileUtils
    {
        /// <summary>
        /// ファイルを読み込んでバイト配列で返します
        /// </summary>
        /// <param name="path"></param>
        /// /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] LoadFileToByteArray(string path, int length = int.MaxValue) {
            if (string.IsNullOrEmpty(path)) throw new ArgumentException(nameof(path));

            byte[] byteArray = null;
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                long readLength = stream.Length > length ? length : stream.Length;
                byteArray = new byte[readLength];
                stream.Read(byteArray, 0, (int)readLength);
            }
            return byteArray;
        }

        /// <summary>
        /// バイト配列をファイルに書き込みます
        /// </summary>
        /// <param name="path"></param>
        /// <param name="byteArray"></param>
        public static void SaveFileFromByteArray(string path, byte[] byteArray) {
            if (string.IsNullOrEmpty(path)) throw new ArgumentException(nameof(path));
            if (byteArray == null) throw new ArgumentException(nameof(byteArray));

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write)) {
                stream.Write(byteArray, 0, byteArray.Length);
            }
        }

        /// <summary>
        /// ファイルが読み取り専用かどうかを返します
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns>true:読み取り専用</returns>
        public static bool IsReadOnly(string path) {
            if (string.IsNullOrEmpty(path)) throw new ArgumentException(nameof(path));

            return File.GetAttributes(path).HasFlag(FileAttributes.ReadOnly);
        }

    } //class
}
