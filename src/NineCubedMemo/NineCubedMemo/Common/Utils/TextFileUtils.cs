using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Utils
{
    public class TextFileUtils
    {

        /// <summary>
        /// テキストファイルを先頭から指定された行数だけ読み込みます
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <param name="lineCount"></param>
        /// <returns></returns>
        public static IList<string> ReadTopLines(string path, Encoding encoding, int lineCount)
        {
            var list = new List<string>();

            using(var reader = new StreamReader(path, encoding)) {
                for (int i = 0; i < lineCount; i++) {
                    if (reader.EndOfStream) break;

                    //1行読み込みます
                    list.Add(reader.ReadLine());
                }
            }

            return list;
        }

    } //class
}
