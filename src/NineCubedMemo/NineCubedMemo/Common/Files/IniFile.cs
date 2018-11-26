using NineCubed.Common.Collections;
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
    /// iniファイルデータクラス
    /// iniファイルを使うための汎用クラスです。
    /// ファイルの入出力とデータの保持をします。
    /// 
    /// 使用例
    /// var iniFile IniFile();
    /// iniFile.Load(パス);
    /// var value = iniFile["セクション名", "キー"];
    /// </summary>
    public class IniFile : IFile
    {
        /// <summary>
        /// iniファイルの入出力とデータを保持するクラスです。
        /// 
        /// ----------------------------------------
        /// title = Test
        /// 
        /// [location]
        /// top  = 10
        /// left = 20
        /// 
        /// [size]
        /// width  = 100
        /// height = 200
        /// ----------------------------------------
        /// と言う iniファイルの場合、
        /// 
        /// var iniFile = new IniFile();
        /// iniFile.Load("config.ini");
        /// var top    = iniFile["location", "top"]
        /// var height = iniFile["size", "height"]
        /// のようにして値を取得できます。
        /// 
        /// セクションがない ini ファイルは、
        /// iniFile["", "title"] のようにセクション名に "" を指定して取得できます。
        /// または iniFile[IniFile.NO_SECTION, "title"]
        /// 
        /// </summary>
        private readonly Map<string, Map<string, string>> _data = new Map<string, Map<string, string>>();

        /// <summary>
        /// セクション未指定の空文字
        /// </summary>
        public const string NO_SECTION = "";

        /// <summary>
        /// iniファイルデータのアクセサー
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string section, string key] {
            get {
                var subData = _data[section];
                if (subData != null) return subData[key];

                //取得失敗
                return null;
            }
            set {
                var subData = _data[section];
                if (subData == null) {
                    //取得できなかった場合は、生成してMapに追加します
                    subData = new Map<string, string>();
                    _data[section] = subData;
                }

                subData[key] = value;
            }
        }

        /// <summary>
        /// 指定されたセクション配下のデータを Map で返します
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public Map<string, string> GetSubData(string section) => _data[section];
        
        /// <summary>
        /// ファイルのパス
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// ファイルが読み取り専用かどうか
        /// true:読み取り専用
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// 文字コード
        /// </summary>
        private Encoding encoding = new UTF8Encoding(false);

        /// <summary>
        /// iniファイルを出力します
        /// <param name="path">パス</param>
        /// </summary>
        public void Save(string path = null)
        {
            if (path != null) this.Path = path;

            using (var writer = new StreamWriter(this.Path, false, encoding)) {
                //セクション名が未指定の Map を先にファイル出力します
                WriteKeyValue(writer, _data[""]);

                foreach (var keyValue in _data) {
                    if (keyValue.Key.Equals("")) continue; //セクション名が未指定の場合は処理済みのためスキップします

                    //セクションをファイル出力します
                    writer.WriteLine("[" + keyValue.Key + "]");

                    //Map をファイル出力します
                    WriteKeyValue(writer, keyValue.Value);
                }
            }

            return;
            //以下、ローカル関数(メソッド内の関数)

            //ファイルに Map の内容を1行ずつ出力します
            void WriteKeyValue(StreamWriter writer, Map<string, string> map) {
                if (map != null) {
                    //ファイルに1行ずつ出力します
                    foreach (var keyValue in map) {
                        writer.WriteLine(keyValue.Key + "=" + keyValue.Value);
                    }

                    writer.WriteLine(); //空行を追加します
                }
            }
        }

        /// <summary>
        /// iniファイルを読み込みます
        /// </summary>
        /// <param name="path">パス</param>
        public void Load(string path = null)
        {
            if (path != null) this.Path = path; //パスが指定されている場合は、パスを保持します
            if (File.Exists(this.Path) == false) return; //ファイルか存在しない場合

            string section = ""; //セクション名

            using(var reader = new StreamReader(this.Path, encoding)) {
                while (reader.EndOfStream == false) {
                    //ファイルから1行読み込みます
                    var line = reader.ReadLine().Trim();

                    //コメントの場合は無視します
                    if (line.StartsWith(";") || line.StartsWith("#")) continue;

                    // [ ] で挟まている値をセクション名として取得します
                    if (line.StartsWith("[") && line.EndsWith("]")) {
                        section = line.Substring(1, line.Length - 2);
                        continue;
                    }

                    //「=」が含まれる場合は、キーと値に分割して、値を保持します
                    var (key, value) = StringUtils.GetKeyValue(line);
                    if (string.IsNullOrEmpty(key) == false) {
                        this[section, key] = value;
                    }
                }
            }
        }

    } //class
}
