using NineCubed.Memo;
using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Files
{
    /// <summary>
    /// ver1.0.4
    /// テキストファイルクラス
    /// </summary>
    public class TextFile : IFile
    {
        /// <summary>
        /// テキストファイル情報
        /// </summary>
        private FileInfo _fileInfo;

        /// <summary>
        /// エンコーディング(文字コード)
        /// </summary>
        public Encoding TextEncoding { get; set; }
        public void SetEncodingUtf8(bool hasBom)   { this.TextEncoding = new UTF8Encoding(hasBom); }
        public void SetEncodingShiftJIS()          { this.TextEncoding = Encoding.GetEncoding(932); }

        /// <summary>
        /// テキスト
        /// 改行コードは \n に統一して保持する
        /// 
        /// 注意：ファイルに保存する時は、NewLineCodeプロパティ(改行コード)を使う
        /// </summary>
        public string Text { get; set; }

        /*
        /// <summary>
        /// テキストの行単位の配列
        /// 
        /// get は Split() をしているため、繰り返し使用する際は要注意。
        /// </summary>
        public string[] Lines {
            get {
                return this.Text.Split('\n');
            }
            set {
                this.Text = string.Join("\n", value);
            }
        }
        */

        /// <summary>
        /// ファイル保存時の改行コード
        /// 
        /// 注意：Textプロパティの改行コードは \n で統一する
        /// </summary>
        public string NewLineCode { get; set; }

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
        public TextFile() {
        }

        /// <summary>
        /// テキストファイルを書き込みます
        /// </summary>
        /// <param name="path">パス</param>
        public void Save(string path)
        {
            using(var writer = new FileStream(path, FileMode.Create, FileAccess.Write)) {
                //改行コードを変更します
                string text = this.Text.Replace("\n", this.NewLineCode);

                {
                    //BOMがある場合は、BOMコードを書き込みます
                    if (this.TextEncoding != null) {
                        byte[] byteArray = this.TextEncoding.GetPreamble();
                        if (byteArray.Length > 0) {
                            writer.Write(byteArray, 0, byteArray.Length);
                        }
                    }
                }

                {
                    //エンコードします
                    if (this.TextEncoding == null) SetEncodingShiftJIS(); //文字コードが指定されていない場合は、シフトJISで設定します
                    byte[] byteArray = this.TextEncoding.GetBytes(text);

                    //ファイルに書き込みます
                    writer.Write(byteArray, 0, byteArray.Length);
                }
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
        public void Load(string path)
        {
            //文字コードと改行コードの判別のため、
            //先頭の8KBだけ読み込んでバイト配列にいれます。8KBに根拠なし。なんとなく。
            byte[] textByteArray = null;
            if (this.TextEncoding == null || this.NewLineCode == null) {
                textByteArray = FileUtils.LoadFileToByteArray(path, 1024 * 8);
            }

            //----------------------------------------
            //  文字コード判別
            //----------------------------------------
            //文字コードが指定されていない場合は、文字コードの判別を行います
            if (this.TextEncoding == null) {
                this.TextEncoding = DetectEncoding(textByteArray);
            }
            
            //----------------------------------------
            //  バイト配列 -> 文字列リスト
            //----------------------------------------
            //バイト配列から1行単位で読み込み、リストに追加します
            var list = new List<string>();
            using(var reader = new StreamReader(path, this.TextEncoding)) {
                this.Text = reader.ReadToEnd();
            }

            //Textで保持する改行コードを \n で統一します
            this.Text = this.Text.Replace("\r\n", "\n");
            this.Text = this.Text.Replace('\r'  , '\n');

            //----------------------------------------
            //  改行コード判別
            //----------------------------------------
            //改行コードが指定されていない場合は、改行コードの判別を行います
            if (this.NewLineCode == null) {
                this.NewLineCode = DetectNewLineCode(textByteArray);
            }

            //----------------------------------------
            //  プロパティの設定
            //  最初に設定すると、例外発生時に誤った値が保持されるため、最後に設定しています
            //----------------------------------------
            this.Path = path;
            this.IsReadOnly = FileUtils.IsReadOnly(path);
        }

        /// <summary>
        /// (簡易版)文字コード判別
        /// バイト配列から文字コードを判別して Encoding を返します
        /// </summary>
        /// <param name="byteArray">バイト配列</param>
        /// <returns>判別された Encoding</returns>
        private Encoding DetectEncoding(byte[] byteArray)
        {
            //----------------------------------------
            //  BOMコードにより判別
            //----------------------------------------

            //UTF-8
            if (byteArray.Length >= 3) {
                if (byteArray[0] == 0xEF && byteArray[1] == 0xBB && byteArray[2] == 0xBF) return new UTF8Encoding(true); //UTF-8
            }

            //UTF-32
            if (byteArray.Length >= 4) {
                if (byteArray[0] == 0xFF && byteArray[1] == 0xFE && byteArray[2] == 0x00 && byteArray[3] == 0x00) return new UTF32Encoding(false, true); //UTF-32
                if (byteArray[0] == 0x00 && byteArray[1] == 0x00 && byteArray[2] == 0xFE && byteArray[3] == 0xFF) return new UTF32Encoding(true , true); //UTF-32BE
            }
            
            //UTF-16
            if (byteArray.Length >= 2) {
                if (byteArray[0] == 0xFF && byteArray[1] == 0xFE) return Encoding.GetEncoding(1200); //UTF-16 
                if (byteArray[0] == 0xFE && byteArray[1] == 0xFF) return Encoding.GetEncoding(1201); //UTF-16BE
            }
            
            //----------------------------------------
            //  文字化けの文字により判別
            //----------------------------------------

            //シフトJIS -> UTF-8     「�」 がたくさん出る
            //EUC-JP    -> UTF-8     「�」 がたくさん出る
            //UTF-8     -> シフトJIS 「縺」「繝」「繧」 がたくさん出る
            //EUC-JP    -> シフトJIS 「､」「･」がたくさん出る
            //シフトJIS -> EUC-JP    「・」がたくさん出る
            //UTF-8     -> EUC-JP    「・」がたくさん出る

            //文字判別のためにエンコードするサイズ
            const int MaxEncodeSize = 1024 * 8; //先頭の8KBで判別する
            int encodeSize = Math.Min(byteArray.Length, MaxEncodeSize);
            bool removeEnd = false; //true:末尾を削除する
            if (byteArray.Length > MaxEncodeSize) {
                removeEnd = true; //文字列の途中で切った場合は、末尾が文字化けするため、末尾を削除します
            }

            //バイト配列 -> シフトJISの文字列
            string strShiftJis = Encoding.GetEncoding(932).GetString(byteArray, 0, encodeSize);
            if (strShiftJis.Length > 0 && removeEnd == true) strShiftJis = strShiftJis.Substring(0, strShiftJis.Length - 1); //途中で切っているため、最後の文字は文字化けする可能性があるため破棄する

            //バイト配列 -> UTF-8の文字列
            string strUtf8 = Encoding.UTF8.GetString(byteArray, 0, encodeSize);
            if (strUtf8.Length > 0 && removeEnd == true) strUtf8 = strUtf8.Substring(0, strUtf8.Length - 1);


            //UTF-8 か判別します
            if (strUtf8.IndexOf('�') == -1) {
                //シフトJIS や EUC-JP から UTF-8 に変換した時によく出る文字化けの文字が出ない

                if (StringUtils.CountChar(strShiftJis, '縺') + 
                    StringUtils.CountChar(strShiftJis, '繧') + 
                    StringUtils.CountChar(strShiftJis, '繝') + 
                    StringUtils.CountChar(strShiftJis, '｡')> 0) {
                    //UTF-8 から ShiftJIS に変換した時によく出る文字化けの文字が出る
                    return new UTF8Encoding(false); //BOMなし
                }
            }

            //EUC-JP か判別します
            if (StringUtils.CountChar(strShiftJis, '､') + 
                StringUtils.CountChar(strShiftJis, '･') +
                StringUtils.CountChar(strShiftJis, '｣') > 0) {
                //EUC-JP から シフトJIS に変換した時によく出る文字化けの文字が出る
                
                //バイト配列 -> EUC-JP の文字列
                string strEucJp = Encoding.GetEncoding(51932).GetString(byteArray, 0, encodeSize);

                if (StringUtils.CountChar(strEucJp, '・') == 0) {
                    //シフトJIS から EUC-JP に変換した時によく出る文字化けの文字が出ない
                    return Encoding.GetEncoding(51932);
                }
            }

            //文字化けなし、半角のみの場合は、シフトJISとします
            return Encoding.GetEncoding(932);
        }

        /// <summary>
        /// (簡易版)改行コードを判別して返します
        /// CRとLFが混在する場合は正常に判別できません。CRLF扱いにします。
        /// \r と \n が 1バイトで表せない UTF-16 などは、正常に判別できません。
        /// </summary>
        /// <param name="byteData"></param>
        /// <returns></returns>
        private string DetectNewLineCode(byte[] byteData)
        {
            int cr = Array.IndexOf(byteData, (byte)0x0d);
            int lf = Array.IndexOf(byteData, (byte)0x0a);
            if (cr >= 0 && lf >= 0) return "\r\n";
            if (cr >= 0) return "\r";
            if (lf >= 0) return "\n";

            //改行コードがなかった場合、デフォルトの改行コードを返します
            return Environment.NewLine;
        }
        
        /// <summary>
        /// テキストファイルの最後が改行かどうかを返します
        /// 
        /// テキストファイルを ReadLine() で読み込むと、最後の改行の有無がわからないため、最後に改行があるか確認します。
        /// </summary>
        /// <param name="byteArray"></param>
        /// <param name="newLineCode">改行コード</param>
        /// <returns></returns>
        private bool IsNewLineCodeAtTail(byte[] byteArray, string newLineCode) {

            //改行をバイト配列にします(UTF-16、UTF-32 はサイズが異なるため)
            byte[] newLineByteArray = this.TextEncoding.GetBytes(newLineCode);

            //対象バイト配列が改行のバイト配列よりも短い場合は改行なしとする
            if (byteArray.Length < newLineByteArray.Length) return false;
            
            //対象バイト配列の最後が、改行コードと一致するか確認する
            //1つでも異なれば改行なしとする
            for (int i = 0; i < newLineByteArray.Length; i++) {
                if (byteArray[byteArray.Length - i - 1] != newLineByteArray[newLineByteArray.Length - i - 1]) return false;
            }

            return true;
        }

    } //class
}
