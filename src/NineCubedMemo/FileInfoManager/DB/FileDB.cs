using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileInfoManager.DB
{
    public class FileDB
    {
        //DBファイルのパス
        private String DbPath;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dbPath"></param>
        public FileDB(string dbPath)
        {
            this.DbPath = dbPath;

            //DBファイルが存在しない場合は、DBを作成します
            if (File.Exists(dbPath) == false) {
                CreateDB();
            }
        }

        /// <summary>
        /// DBをオープンします
        /// </summary>
        /// <returns></returns>
        public SQLiteConnection GetConnection() {
            if (string.IsNullOrEmpty(this.DbPath)) return null;
            var connection = new SQLiteConnection("Data Source=" + this.DbPath);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// テーブルを作成します
        /// </summary>
        /// <param name="dbPath"></param>
        public void CreateDB()
        {
            //テーブルを作成します
            using (var connection = GetConnection())
            using (var cmd = connection.CreateCommand()) {

                //テーブル作成
                cmd.CommandText = "create table d_file(" +
                    "id INTEGER PRIMARY KEY, " + //ID
                    "title   TEXT, "    + //タイトル
                    "memo    TEXT, "    + //メモ
                    "value   INTEGER, " + //評価・重要度
                    "path    TEXT, "    + //ファイルのフルパス
                    "kind    INTEGER, " + //ファイル種別 1:ファイル 2:フォルダ
                    "size    INTEGER, " + //ファイルサイズ
                    "created TEXT, "    + //作成日時
                    "updated TEXT  "    + //更新日時
                ");";
                cmd.ExecuteNonQuery();

                //テーブル作成
                cmd.CommandText = "create table d_tag(" + 
                    "tag TEXT, " +
                    "d_file_id INTEGER, " + 
                    "primary key(tag, d_file_id));";
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 参照系のSQLを実行します。
        /// 1レコードのデータをstring配列にして、それをリストにしたものを返します。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<string[]> ExecuteQuery(SQLiteConnection connection, string sql) {

            var dataList = new List<string[]>();

            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = sql;
                cmd.Prepare();

                //SQLを実行します
                using (var reader = cmd.ExecuteReader()) {

                    //データを読み込みます。データがなくなるまでループ
                    while (reader.Read()) {
                        var data = new string[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++) {
                            data[i] = reader[i].ToString();
                        }
                        dataList.Add(data);
                    }
                }
            }

            return dataList;
        }

        /// <summary>
        /// 最後に追加したデータの id を返します
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        static public int GetLastId(SQLiteConnection connection, string tableName) {
            int id = 0;
            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = "select id from " + tableName + " where ROWID = last_insert_rowid()";
                cmd.Prepare();
                using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                    if (reader.Read()) {
                        id = StringUtils.ToInt(reader["id"].ToString(), 0);
                    }
                }
            }
            return id;
        }

        /// <summary>
        /// ファイルデータとタグデータを保存します
        /// タグデータの更新は、Delete Insert です。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="fileData"></param>
        public static void SaveData(SQLiteConnection connection, FileData fileData)
        {
            //既存のファイルデータを取得します
            FileData tempData = null;
            if (fileData.id != 0) {
                tempData = FileDataDao.GetData(connection, fileData.id.ToString());
            }

            //ファイルデータを保存します
            if (tempData == null) {
                //ファイルデータがないため、Insert します
                FileDataDao.Insert(connection, fileData);

                //追加した詳細データを取得します(タグの追加でidを使用するため)
                tempData = FileDataDao.GetLastData(connection);
                fileData.id = tempData.id; //IDを設定します

            } else {
                //ファイルデータがあるため、Update します
                FileDataDao.Update(connection, fileData);
            }

            //タグデータを更新します
            UpdateTag(connection, fileData);
        }

        /// <summary>
        /// タグデータを更新します
        /// </summary>
        public static void UpdateTag(SQLiteConnection connection, FileData fileData)
        {
            //タグデータを削除します
            TagDataDao.Delete(connection, fileData.id);

            //タグデータを追加します
            foreach (var tag in fileData.GetTagList()) {
                TagDataDao.Insert(connection, tag, fileData.id);
            }
        }

        /// <summary>
        /// ファイルデータとタグデータを削除します
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="FileData_id"></param>
        public static void DeleteData(SQLiteConnection connection, int d_file_id)
        {
            //ファイルデータを削除します
            FileDataDao.Delete(connection, d_file_id);

            //タグデータを削除します
            TagDataDao.Delete(connection, d_file_id);
        }

        /// <summary>
        /// ファイルデータとタグデータを削除します [複数レコード版] 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="d_file_id_list"></param>
        public static void DeleteData(SQLiteConnection connection, IList<int> d_file_id_list)
        {
            foreach (var FileData_id in d_file_id_list) {
                DeleteData(connection, FileData_id);
            }
        }

        /// <summary>
        /// ファイルデータとタグデータを取得します。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="d_file_id"></param>
        /// <returns></returns>
        public static FileData LoadData(SQLiteConnection connection, int d_file_id)
        {
            //ファイルデータを取得します
            var fileData = FileDataDao.GetData(connection, d_file_id.ToString());
            if (fileData == null) return null;

            //タグデータを取得します
            fileData._tags = TagDataDao.GetTags(connection, d_file_id);

            return fileData;
        }


    } //class
}
