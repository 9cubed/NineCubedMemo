using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NineCubed.Common.Utils
{
    public class SerializeUtils
    {
        /// <summary>
        /// オブジェクトをシリアライズ化してXMLファイルとして出力します
        /// </summary>
        /// <param name="obj">オブジェクト</param>
        /// <param name="path">出力先のパス</param>
        /// <param name="encoding">出力時のエンコーディング</param>
        public static void SerializeToFile(object obj, string path, Encoding encoding = null) {
            if (encoding == null) encoding = new UTF8Encoding(false);

            var serializer = new XmlSerializer(obj.GetType());
            using (var stream = new FileStream(path, FileMode.Create))
            using (var writer = new StreamWriter(stream, encoding)) {
                serializer.Serialize(writer, obj);
            }
        }

        /// <summary>
        /// XMLファイルからオブジェクトを復元します。
        /// </summary>
        /// <param name="type">オブジェクトの型</param>
        /// <param name="path">入力元のパス</param>
        /// <param name="encoding">入力時のエンコーディング</param>
        /// <returns>XMLファイルから復元したオブジェクト</returns>
        public static object DeserializeFromFile(Type type, string path, Encoding encoding = null) {
            if (encoding == null) encoding = new UTF8Encoding(false);

            object obj = null;
            var serializer = new XmlSerializer(type);
            using (var stream = new FileStream(path, FileMode.Open))
            using (var reader = new StreamReader(stream, encoding)) {
                obj = serializer.Deserialize(reader);
            }
            return obj;
        }

        /// <summary>
        /// オブジェクトをシリアライズ化して文字列として返します
        /// </summary>
        /// <param name="obj">オブジェクト</param>
        /// <param name="path">出力先のパス</param>
        /// <param name="encoding">出力時のエンコーディング</param>
        /// <returns>XML形式の文字列</returns>
        public static string SerializeToString(object obj, Encoding encoding = null) {
            if (encoding == null) encoding = new UTF8Encoding(false);
            
            string result = null;
            var serializer = new XmlSerializer(obj.GetType());
            using (var stream = new MemoryStream()) {
                //シリアライズ化
                serializer.Serialize(stream, obj);

                //シリアライズの結果をバイト配列で取得して文字列に変換します
                var byteArray = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(byteArray, 0, (int)stream.Length);
                result = encoding.GetString(byteArray);
                
            }
            return result;
        }

        /// <summary>
        /// 文字列からオブジェクトを復元します。
        /// </summary>
        /// <param name="type">オブジェクトの型</param>
        /// <param name="strXml">XML形式の文字列</param>
        /// <param name="encoding">入力時のエンコーディング</param>
        /// <returns></returns>
        public static object DeserializeFromString(Type type, string strXml, Encoding encoding = null) {
            if (encoding == null) encoding = new UTF8Encoding(false);

            object obj = null;
            var serializer = new XmlSerializer(type);
            using (Stream stream = new MemoryStream(encoding.GetBytes(strXml))) {
                obj = serializer.Deserialize(stream);
            }
            return obj;
        }

    } //class
}
