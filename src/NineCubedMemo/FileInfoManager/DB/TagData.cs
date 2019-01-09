using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileInfoManager.DB
{
    public class TagData
    {
        //テーブル項目
        public string tag = "";      //タグ
        public int    d_file_id = 0; //ファイルデータのID

        //テーブル項目ではない
        public int _fileCount = 0;   //タグに割り当てられているファイル数
    }
}
