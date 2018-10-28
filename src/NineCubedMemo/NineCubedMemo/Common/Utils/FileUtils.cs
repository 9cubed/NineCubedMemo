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

                /// <summary>
        /// 指定したパス配下のフォルダの有無を返します。
        /// 
        /// パスにドライブを指定する場合は、c:\ のように、円マークを付けないと、
        /// 動く場合と動かない場合があるので要注意。
        /// </summary>
        /// <returns>true:有 false:無</returns>
        public static bool ExistSubDir(String path) {
            //指定されたパス配下の先頭のフォルダのパスを取得します。ない場合はnull。
            var subDirPath = Directory.EnumerateDirectories(path).FirstOrDefault();
            return (subDirPath != null) ? true : false;
        }

        /// <summary>
        /// 指定されたパス配下のフォルダ一覧を返します。
        /// </summary>
        /// <param name="path">パス</param>
        /// <param name="subDir">true:サブディレクトリを検索対象にする</param>
        /// <param name="searchPattern">ワイルドカード</param>
        /// <returns>フォルダ一覧(文字列)</returns>
        public static IList<string> GetDirList(String path, bool subDir = false, string searchPattern = "*") {
            if (string.IsNullOrEmpty(path)) throw new ArgumentException(); //パスが指定されていないためエラーとする

            //パスにドライブだけ指定されている場合には、\マークをつけます
            if (path.EndsWith(":")) path = path + Path.DirectorySeparatorChar; 

            //サブディレクトリを含むかどうかの検索条件を設定します
            var option = subDir ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            //フォルダ一覧を取得します
            var dirList = Directory.GetDirectories(path, searchPattern, option).ToList();

            return dirList;
        }

        // パスを結合します。
        // Path.Combine() は以下の動作になるので使わない。
        // Path.Combine("c:", "test.txt")    -> c:test.txt  ドライブの前に \ がついてくれない。
        // Path.Combine("c:", @"\test.txt")  -> \test.txt   
        public static string AppendPath(String path1, String path2) {

            //パスが指定されていない場合は空を返します
            if (string.IsNullOrEmpty(path1) && string.IsNullOrEmpty(path2)) return "";

            //末尾のセパレータを削除します
            path1 = path1.TrimEnd(new[]{'\\', '/'});
            
            //先頭のセパレータを削除します
            path2 = path2.TrimStart(new[]{'\\', '/'});

            //片方が空の場合は、もう片方を返します
            if (string.IsNullOrEmpty(path1)) return path2;
            if (string.IsNullOrEmpty(path2)) return path1;

            //パスを結合して返します
            return path1 + Path.DirectorySeparatorChar.ToString() + path2;
        }

    } //class
}
