using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Files
{
    /// <summary>
    /// ファイルの種類が特定できない場合に使うクラス
    /// </summary>
    public class AnyFile : IFile
    {
        /// <summary>
        /// ファイルのパス
        /// </summary>
        public string Path { get; set; }

        public string Text { get; set; }
        public bool IsReadOnly { get; set; }
        public void Save(string path = null) { }
        public void Load(string path = null) { }

    }
}
