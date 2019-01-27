using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileInfoManager.DB
{
    public class FileData
    {
        //テーブル項目
        public int    id      = 0;  //ID
        public string title   = ""; //タイトル
        public string memo    = ""; //メモ
        public int    value   = 0;  //評価・重要度
        public string path    = ""; //ファイルのフルパス
        public int    kind    = 0;  //ファイル種別  0:存在しない 1:ファイル 2:フォルダ
        public long   size    = 0;  //ファイルサイズ
        public string created = ""; //作成日時
        public string updated = ""; //更新日時

        //テーブル項目ではない
        public string _tags = ""; //スペース区切りのタグ

        /// <summary>
        /// タグをリストで返します
        /// </summary>
        /// <returns></returns>
        public IList<string> GetTagList() {
            //スペース区切りタグをリストにします
            var list = this._tags.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries).ToList();
            return list;
        }

    } //class
}
