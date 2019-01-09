using FileInfoManager.DB;
using NineCubed.Common.Controls.FileList;
using NineCubed.Common.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileInfoManager
{
    public class FileValueColumn : AFileListColumn
    {
        Thread _thread;
        ConcurrentQueue<FileInfo> _queue;

        //キャッシュ key:ファイル名 value:表示する値
        ConcurrentDictionary<string, string> _cache;

        /// <summary>
        /// DB接続用オブジェクト
        /// </summary>
        FileDB _db;

        public FileValueColumn(string dbPath) : base()
        {
            this.ReadOnly = false;
            this.HeaderText = "評価";

            //DB接続用オブジェクトを生成します
            _db = new FileDB(dbPath);
        }

        /// <summary>
        /// ファイル一覧表示直前の処理を行います
        /// </summary>
        override
        public void ShowFileListBefore()
        {
            _cache = new ConcurrentDictionary<string, string>();
            _queue = new ConcurrentQueue<FileInfo>();
        }

        /// <summary>
        /// ファイル一覧表示直後の処理を行います
        /// </summary>
        override
        public void ShowFileListAfter()
        {
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) {
            //キャッシュにある場合は、キャッシュの値を返します
            if (_cache.TryGetValue(file.FullName, out string value)) {
                return value;
            }

            //別スレッドで処理するキューに、処理対象のファイルを追加します
            _queue.Enqueue(file);

            //動いているスレッドがない場合は、スレッドを生成します
            if (_thread == null || _thread.IsAlive == false) {
                //スレッドがない場合

                //画像サイズを取得して、結果をキャッシュに設定します
                _thread = new Thread(GetData);

                //スレッドを実行します
                _thread.Start();
            }

            return "";
        }

        //編集で値が変更された場合に呼ばれます
        override
        public FileInfo ValueChanged(FileInfo orgFile, string newValue)
        {
            //スレッドが動いている場合は、処理しない
            if (_thread != null && _thread.IsAlive == true) return orgFile;

            var path = orgFile.FullName;

            using (var connection = _db.GetConnection()) {
                var data = FileDataDao.GetDataByPath(connection, orgFile.FullName);
                if (data == null) {
                    //データがない場合
                    data = new FileData();
                    data.title = orgFile.Name;
                    data.value = StringUtils.ToInt(newValue, 0); //評価・重要度
                    data.path  = path;           //ファイルのフルパス
                    data.size  = orgFile.Length; //ファイルサイズ 
                } else {
                    //データがある場合
                    data.value = StringUtils.ToInt(newValue, 0); //評価・重要度
                }

                //データを更新します
                FileDB.SaveData(connection, data);

                //キャッシュに反映します
                _cache[orgFile.FullName] = data.value.ToString();
            }

            return orgFile;
        }

        /// <summary>
        /// 画像サイズを取得します
        /// </summary>
        private void GetData()
        {
            using (var connection = _db.GetConnection()) {
                while (_queue.IsEmpty == false) {
                    //空でない場合
                    while (_queue.TryDequeue(out var targetFile)) {
                        var data = FileDataDao.GetDataByPath(connection, targetFile.FullName);
                        if (data != null) {
                            //キャッシュに表示する値を設定します
                            _cache[targetFile.FullName] = data.value.ToString();
                        }
                    }

                    //キューにデータが追加されるのを少し待ちます
                    //次のデータがキューに入る前に処理が終わってしまうと、
                    //スレッドが破棄され、またスレッドが生成されてしまうため
                    Thread.Sleep(100);
                }
            }

            //列のデータを更新します
            FileList.UpdateColumn(this);
        }

        override
        public int SortCompare(string sValue1, string sValue2)
        {
            var iValue1 = StringUtils.ToInt(sValue1, 0);
            var iValue2 = StringUtils.ToInt(sValue2, 0);

            //文字列が空の場合(フォルダの場合)は、-1 として扱う
            if (string.IsNullOrEmpty(sValue1)) iValue1 = -1;
            if (string.IsNullOrEmpty(sValue2)) iValue2 = -1;

            return iValue1 > iValue2 ? 1 : -1;
        }

    } //class
}
