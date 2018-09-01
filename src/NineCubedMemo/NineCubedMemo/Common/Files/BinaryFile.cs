using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Files
{
    public class BinaryFile : IFile
    {
        /// <summary>
        /// テキストファイル情報
        /// </summary>
        private FileInfo _fileInfo;

        /// <summary>
        /// テキスト
        /// 改行コードは \n に統一して保持する
        /// 
        /// 注意：ファイルに保存する時は、NewLineCodeプロパティ(改行コード)を使う
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// ファイル保存時の改行コード
        /// 
        /// 注意：Textプロパティの改行コードは \n で統一する
        /// </summary>
        //public string NewLineCode { get; set; }

        /// <summary>
        /// ファイルのパス
        /// </summary>
        public string Path {
            get {
                return _fileInfo?.FullName;
            }
            set {
                _fileInfo = null;
                if (value != null) _fileInfo = new FileInfo(value);
            }
        }

        /// <summary>
        /// ファイルが読み取り専用かどうか
        /// true:読み取り専用
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BinaryFile() {
        }

        /// <summary>
        /// テキストファイルを書き込みます
        /// </summary>
        /// <param name="path">パス</param>
        public void Save(string path = null)
        {
            if (path == null) path = this.Path;

            //テキスト -> バイト配列
            byte[] byteArray = BinaryUtils.HexToByteArray(this.Text);

            //ファイル出力
            using(var writer = new FileStream(path, FileMode.Create, FileAccess.Write)) {
                writer.Write(byteArray, 0, byteArray.Length);
            }

            //パスを保存します
            this.Path = path;
        }

        /// <summary>
        /// テキストファイルを読み込みます
        /// TextEncodingプロパティが null の場合は、文字コードの判別を行います
        /// NewLineCode プロパティが null の場合は、改行コードの判別を行います
        /// 引数で自動判別のフラグを持たさせたいが、I/F合わなくなるため、追加していません。要検討。
        /// </summary>
        /// <param name="path">パス</param>
        public void Load(string path = null)
        {
            if (path == null) path = this.Path;

            if ((new FileInfo(path)).Length >= 1024 * 1024 * 50) {
                //ファイルサイズが50MBを超える場合はエラーとする。TODO:Configで設定させる
                throw new Exception("ファイルサイズが50MBを越えています。");
            }

            byte[] byteArray;
            using(var stream = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                byteArray = new byte[stream.Length];
                stream.Read(byteArray, 0, (int)stream.Length);
            }

            //16進数の文字列をテキストとして設定します
            this.Text = BinaryUtils.ByteArrayToHex(byteArray, 16);

            //パス、読み込み専用のフラグの保持
            this.Path = path;
            this.IsReadOnly = FileUtils.IsReadOnly(path);
        }

    } //class
}
