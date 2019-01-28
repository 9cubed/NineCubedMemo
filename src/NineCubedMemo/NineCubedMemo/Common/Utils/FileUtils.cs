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
            
            //パスが存在しない場合は処理を抜けます
            if (Directory.Exists(path) == false) return new List<string>();

            //サブディレクトリを含むかどうかの検索条件を設定します
            var option = subDir ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            //フォルダ一覧を取得します
            var dirList = Directory.GetDirectories(path, searchPattern, option).ToList();

            return dirList;
        }

        /// <summary>
        /// 指定されたパス配下のファイル一覧を返します。
        /// </summary>
        /// <param name="path">パス</param>
        /// <param name="subDir">true:サブディレクトリを検索対象にする</param>
        /// <param name="searchPattern">ワイルドカード</param>
        /// <returns>ファイル一覧(文字列)</returns>
        public static IList<string> GetFileList(String path, bool subDir = false, string searchPattern = "*") {
            if (string.IsNullOrEmpty(path)) throw new ArgumentException(); //パスが指定されていないためエラーとする

            //パスにドライブだけ指定されている場合には、\マークをつけます
            if (path.EndsWith(":")) path = path + Path.DirectorySeparatorChar; 

            //パスが存在しない場合は処理を抜けます
            if (Directory.Exists(path) == false) return new List<string>();

            //サブディレクトリを含むかどうかの検索条件を設定します
            var option = subDir ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            //ファイル一覧を取得します
            var fileList = Directory.GetFiles(path, searchPattern, option).ToList();

            return fileList;
        }

        /// <summary>
        /// 指定されたパス配下のフォルダとファイルの一覧を返します。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="subDir"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static IList<string> GetDirFileList(String path, bool subDir = false, string searchPattern = "*") {
            var resultList = new List<string>();
            resultList.AddRange(FileUtils.GetDirList (path, subDir, searchPattern));
            resultList.AddRange(FileUtils.GetFileList(path, subDir, searchPattern));
            return resultList;
        }

        /// <summary>
        /// 指定されたパス配下のファイル一覧を取得します。
        /// 1ファイル毎に、引数の関数(Func)にパスを渡して呼び出します。
        /// 関数の戻り値が false の場合は、処理をキャンセルします
        /// </summary>
        /// <param name="func"></param>
        /// <param name="dirPath"></param>
        /// <param name="subDir"></param>
        public static bool GetFileList(Func<string, bool> func, string dirPath, bool subDir = false)
        {
            foreach (var path in Directory.EnumerateFiles(dirPath)) {
                func(path);
            }

            if (subDir) {
                foreach (var path in Directory.EnumerateDirectories(dirPath)) {
                    GetFileList(func, path, subDir);
                }
            }
            return true;
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
        public static string AppendPath(string path1, string path2, string path3) {
            return AppendPath(AppendPath(path1, path2), path3);
        }
        public static string AppendPath(string path1, string path2, string path3, string path4) {
            return AppendPath(AppendPath(AppendPath(path1, path2), path3), path4);
        }

        /// <summary>
        /// ファイルかどうかの判定をします。
        /// </summary>
        /// <param name="path"></param>
        /// <returns>true:ファイル false:ファイル以外(フォルダ or 不正なパス)</returns>
        public static bool IsFile(string path)
        {
            var file = new FileInfo(path);
            return file.Exists;
            //return !( file.Attributes.HasFlag(FileAttributes.Directory) );
        }
        public static bool IsDir(string path) {
            var dir = new DirectoryInfo(path);
            return dir.Exists;
        }

        /// <summary>
        /// フォルダまたはファイルが存在するかチェックします
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns>true:存在する</returns>
        public static bool Exists(string path)
        {
            bool exists;
            if (FileUtils.IsFile(path)) {
                exists = File.Exists(path);
            } else {
                exists = Directory.Exists(path);
            }
            return exists;
        }

        /// <summary>
        /// ファイルコピーモード
        /// 同じファイルがある場合の動作です。
        /// </summary>
        public enum FileCopyMode {
            CopyAlways,   //常にコピー
            CopyIfNewer,  //新しい時にコピー
            DontCopy      //コピーしない
        }

        /// <summary>
        /// フォルダをコピーします
        /// 指定したフォルダ配下のフォルダとファイルを、指定したフォルダ配下へ全てコピーします
        /// </summary>
        /// <param name="fromDirPath">コピー元フォルダのパス</param>
        /// <param name="toDirPath">コピー先フォルダのパス</param>
        /// <param name="copyMode">コピーモード</param>
        public static void CopyDir(string fromDirPath, string toDirPath, FileCopyMode copyMode) {
            //コピー先がコピー元のフォルダ配下にある場合は、無限ループとなるためエラーとします(実際には無限ループになる前に、パスの文字列長のエラーが発生する)
            if (ContainsDir(fromDirPath, toDirPath)) {
                throw new IOException("CopyDir() コピー先がコピー元に含まれています");
            }
            
            //フォルダを丸ごとコピーします
            CopyDirSub(fromDirPath, toDirPath, copyMode);
        }
        private static void CopyDirSub(string fromDirPath, string toDirPath, FileCopyMode copyMode) {
            
            //コピー元のフォルダが存在しない場合は処理しない
            if (Directory.Exists(fromDirPath) == false) return;

            //フォルダ配下のファイルを全てコピーします
            foreach (var fromPath in Directory.GetFiles(fromDirPath)) {
                //コピー先のパス
                var toPath = AppendPath(toDirPath, Path.GetFileName(fromPath));

                //コピーモードに応じてファイルコピー可能かどうかをチェックします
                if (CanFileCopy(fromPath, toPath, copyMode)) {
                    //ファイルコピー可能な場合、ファイルコピーします
                    File.Copy(fromPath, toPath, true);
                }
            }

            //フォルダ配下のファイルを全てコピーします
            foreach (var dir in Directory.GetDirectories(fromDirPath)) {

                //フォルダ配下のフォルダのパスを作成します
                var toChildDirPath = AppendPath(toDirPath, Path.GetFileName(dir));

                //フォルダがない場合は、フォルダを作成します
                if (Directory.Exists(toChildDirPath) == false) {
                    Directory.CreateDirectory(toChildDirPath);
                }

                //フォルダ配下のフォルダとファイルを再起呼び出しでコピーします
                CopyDirSub(dir, toChildDirPath, copyMode);
            }
        }

        /// <summary>
        /// ファイルコピー可能かどうかを返します
        /// </summary>
        /// <param name="fromPath">コピー元のパス</param>
        /// <param name="toPath"  >コピー先のパス</param>
        /// <param name="copyMode">コピーモード</param>
        /// <returns></returns>
        private static bool CanFileCopy(string fromPath, string toPath, FileCopyMode copyMode) {

            switch (copyMode) {
                case FileCopyMode.CopyAlways:
                    return true; //コピー可

                case FileCopyMode.DontCopy:
                    //コピー先に同じファイルがあるか？
                    if (File.Exists(toPath)) {
                        //ある場合
                        return false; //コピー不可
                    } else {
                        //ない場合
                        return true;  //コピー可
                    }
                
                case FileCopyMode.CopyIfNewer:
                    //コピー先に同じファイルがない場合はコピー可
                    if (File.Exists(toPath) == false) return true; //コピー可

                    //更新日付を比較して、コピー元の方が新しい場合はコピー可とします
                    var fromDateTime = File.GetLastWriteTime(fromPath);
                    var   toDateTime = File.GetLastWriteTime(  toPath);
                    if (fromDateTime > toDateTime) {
                        //コピー元が新しい場合
                        return true;  //コピー可
                    } else {
                        return false; //コピー不可
                    }

                default:
                    //コーディングミス:コピーモードを増やして、case の追加漏れがあった場合に来ます
                    throw new Exception(); 
            }
        }
        
        /// <summary>
        /// 親フォルダに指定した子フォルダが含まれているかどうかを返します。
        /// </summary>
        /// <param name="parentDirPath"></param>
        /// <param name="childDirPath"></param>
        /// <returns></returns>
        public static bool ContainsDir(string parentDirPath, string childDirPath)
        {
            //パスを絶対パスに統一します(セパレーターは￥に統一されます)
            parentDirPath = (new DirectoryInfo(parentDirPath)).FullName;
             childDirPath = (new DirectoryInfo( childDirPath)).FullName;

            //末尾にセパレーターを付けて統一します
            var separator = Path.DirectorySeparatorChar.ToString();
            if (parentDirPath.EndsWith(separator) == false) parentDirPath = parentDirPath + separator;
            if ( childDirPath.EndsWith(separator) == false)  childDirPath =  childDirPath + separator;

            //子フォルダのパスに親フォルダのパスが含まれるか確認します
            return childDirPath.IndexOf(parentDirPath) >= 0;
        }
        
        /// <summary>
        /// フォルダを削除します
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteDir(String path) {
            try {
                Directory.Delete(path, true);
            } catch (IOException) {
                //エクスプローラーを開いていると、ファイルは削除されても例外が発生する場合がある
                //なおかつ指定したフォルダだけ残る場合があるので、もう一度削除する
                try {
                    Directory.Delete(path);
                } catch (IOException) { }
            }
        }

        /// <summary>
        /// フォルダを作成します。
        /// </summary>
        /// <param name="path">フォルダを作成するフォルダのパス</param>
        /// <returns>作成したフォルダのパス</returns>
        public static string CreateNewDir(string path) {
            string dirPath = path + "/新しいフォルダ";

            if (Directory.Exists(dirPath) == false) {
                //同じフォルダがない場合
                Directory.CreateDirectory(dirPath);
                return dirPath;
            }

            for (int i = 2; i <= 9999; i++) {
                dirPath = path + "/新しいフォルダ(" + i.ToString() + ")";

                if (Directory.Exists(dirPath) == false) {
                    //同じフォルダがない場合
                    Directory.CreateDirectory(dirPath);
                    return dirPath;
                }
            }

            return null;
        }

        /// <summary>
        /// テキストファイルを作成します。
        /// </summary>
        /// <param name="path">テキストファイルを作成するフォルダのパス</param>
        /// <returns>作成したファイルのパス</returns>
        public static string CreateNewTextFile(string path) {
            string filePath = path + "/新しいファイル.txt";

            if (File.Exists(filePath) == false) {
                //同じフォルダがない場合
                File.Create(filePath).Close();
                return filePath;
            }

            for (int i = 2; i <= 9999; i++) {
                filePath = path + "/新しいファイル(" + i.ToString() + ").txt";

                if (File.Exists(filePath) == false) {
                    //同じファイルがない場合
                    File.Create(filePath).Close();
                    return filePath;
                }
            }

            return null;
        }

        /// <summary>
        /// フォルダが存在しない場合は、途中のフォルダを含めて全てのフォルダを作成します
        /// </summary>
        /// <param name="dirPath"></param>
        public static void CreateDir(string dirPath)
        {
            if (Directory.Exists(dirPath) == false) {
                Directory.CreateDirectory(dirPath);
            }
        }

    } //class
}
