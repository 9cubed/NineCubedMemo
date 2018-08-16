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
        /// テキスト
        /// 改行コードは \n に統一して保持する
        /// 
        /// 注意：ファイルに保存する時は、NewLineCodeプロパティ(改行コード)を使う
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// ファイルが読み取り専用かどうか
        /// true:読み取り専用
        /// </summary>
        bool IsReadOnly { get; set; }

        /// <summary>
        /// テキストファイルを書き込みます
        /// <param name="path">パス</param>
        /// </summary>
        void Save(string path);

        /// <summary>
        /// テキストファイルを読み込みます
        /// </summary>
        /// <param name="path">パス</param>
        void Load(string path);

    } //interface
}
