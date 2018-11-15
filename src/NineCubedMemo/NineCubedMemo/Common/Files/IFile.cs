using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Files
{

    /// <summary>
    /// ファイル用インターフェース
    /// </summary>
    public interface IFile
    {
        /// <summary>
        /// ファイルのパス
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// ファイルが読み取り専用かどうか
        /// true:読み取り専用
        /// </summary>
        bool IsReadOnly { get; set; }

        /// <summary>
        /// テキストファイルを書き込みます
        /// <param name="path">パス</param>
        /// </summary>
        void Save(string path = null);

        /// <summary>
        /// テキストファイルを読み込みます
        /// </summary>
        /// <param name="path">パス</param>
        void Load(string path = null);

    } //interface
}
