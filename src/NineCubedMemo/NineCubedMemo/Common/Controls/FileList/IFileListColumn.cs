using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Controls.FileList
{
    /// <summary>
    /// ファイルリスト用のカラムインターフェース
    /// </summary>
    public interface IFileListColumn
    {
        /// <summary>
        /// ファイルリスト本体
        /// </summary>
        FileListGrid FileList { get; set; }

        /// <summary>
        /// ファイル一覧表示直前の処理を行います
        /// キャッシュの初期化やDBのコネクションの取得などを行います。
        /// </summary>
        void ShowFileListBefore();

        /// <summary>
        /// ファイル一覧表示直後の処理を行います
        /// 解放処理などを行います。
        /// </summary>
        void ShowFileListAfter();

        /// <summary>
        /// 指定されたファイル情報を元にして、表示する値を返します
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        string ToString(FileInfo file);

        /// <summary>
        /// ソート時に値の比較をします
        /// ソートモードが Programmatic の場合のみ使用します
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        int SortCompare(string value1, string value2);

        /// <summary>
        /// 編集で値が変更された場合に呼ばれます
        /// ファイル名カラムの場合は、ファイル名の変更などを行います
        /// </summary>
        /// <param name="orgFile"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        FileInfo ValueChanged(FileInfo orgFile, string newValue);

    } //interface

}
