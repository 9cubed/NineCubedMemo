using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileInfoManager.DB
{
    public class FileDataDao
    {
        /// <summary>
        /// データを追加します
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="data"></param>
        public static void Insert(SQLiteConnection connection, FileData data)
        {
            //作成日時を設定します
            data.created = DateTimeUtils.GetDateTimeString(DateTime.Now, "-");

            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = "insert into d_file (" + 
                    "  title,  memo,  value,  path,  size,  created,  updated) values " + 
                    "(@title, @memo, @value, @path, @size, @created, @updated)";
                
                cmd.Parameters.Add(new SQLiteParameter("@title"  , data.title));
                cmd.Parameters.Add(new SQLiteParameter("@memo"   , data.memo));
                cmd.Parameters.Add(new SQLiteParameter("@value"  , data.value));
                cmd.Parameters.Add(new SQLiteParameter("@path"   , data.path));
                cmd.Parameters.Add(new SQLiteParameter("@size"   , data.size));
                cmd.Parameters.Add(new SQLiteParameter("@created", data.created));
                cmd.Parameters.Add(new SQLiteParameter("@updated", data.updated));

                //SQLを実行します
                cmd.ExecuteNonQuery();

                //引数のデータに、採番された id を設定します
                data.id = FileDataDao.GetLastId(connection);
            }
        }

        /// <summary>
        /// データを更新します
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="data"></param>
        public static void Update(SQLiteConnection connection, FileData data)
        {
            //更新日時を設定します
            data.updated = DateTimeUtils.GetDateTimeString(DateTime.Now, "-");

            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = 
                    "update d_file set " +
                        "title     = @title,     " +
                        "memo      = @memo,      " +
                        "value     = @value,     " +
                        "path      = @path,      " +
                        "size      = @size,      " +
                        "created   = @created,   " +
                        "updated   = @updated    " +
                    " where id = @id";

                cmd.Parameters.Add(new SQLiteParameter("@title"     , data.title));
                cmd.Parameters.Add(new SQLiteParameter("@memo"      , data.memo));
                cmd.Parameters.Add(new SQLiteParameter("@value"     , data.value));
                cmd.Parameters.Add(new SQLiteParameter("@path"      , data.path));
                cmd.Parameters.Add(new SQLiteParameter("@size"      , data.size));
                cmd.Parameters.Add(new SQLiteParameter("@created"   , data.created));
                cmd.Parameters.Add(new SQLiteParameter("@updated"   , data.updated));
                cmd.Parameters.Add(new SQLiteParameter("@id"        , data.id));

                //SQLを実行します
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 指定された項目を更新します
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="id"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void UpdateField(SQLiteConnection connection, int id, string fieldName, string value) {
            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = 
                    "update d_file set " + fieldName + " = @value,  " +
                                                "updated = @updated " +
                    " where id = @id";

                cmd.Parameters.Add(new SQLiteParameter("@value"  , value));
                cmd.Parameters.Add(new SQLiteParameter("@updated", DateTimeUtils.GetDateTimeString(DateTime.Now, "-")));
                cmd.Parameters.Add(new SQLiteParameter("@id"     , id));
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// データを削除します
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="id"></param>
        public static void Delete(SQLiteConnection connection, int id) {
            using (SQLiteCommand cmd = connection.CreateCommand()) {
                cmd.CommandText = "delete from d_file where id = @id";
                cmd.Parameters.Add(new SQLiteParameter("@id", id));
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 最後に追加したデータの id を返します
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static int GetLastId(SQLiteConnection connection) {
            return FileDB.GetLastId(connection, "d_file");
        }
 
        /// <summary>
        /// 最後に追加したデータを返します
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static FileData GetLastData(SQLiteConnection connection) {
            FileData fileData = null;
            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = "select * from d_file where ROWID = last_insert_rowid()";
                cmd.Prepare();
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        fileData = ReadData(reader);
                    }
                }
            }
            return fileData;
        }
 
        /// <summary>
        /// データを検索します
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="value"></param>
        /// <param name="fieldName">フィールド名</param>
        /// <returns></returns>
        public static FileData GetData(SQLiteConnection connection, string value, string fieldName = "id") {
            FileData data = null;
            using (var cmd = connection.CreateCommand())
            {
                string sql = "select * from d_file where " + fieldName + " = @value";
                cmd.Parameters.Add(new SQLiteParameter("@value", value));

                cmd.CommandText = sql;
                cmd.Prepare();

                using (var reader = cmd.ExecuteReader()) {
                    if (reader.Read()) {
                        data = ReadData(reader);
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// パスをキーにしてデータを検索します
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileData GetDataByPath(SQLiteConnection connection, string path) 
            => GetData(connection, path, "path");

        /// <summary>
        /// SQLiteDataReader を FileData に変換して返します
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static FileData ReadData(SQLiteDataReader reader, bool containsTags = false)
        {
            var data = new FileData();
            data.id      = StringUtils.ToInt(reader["id"].ToString(), 0);
            data.title   = reader["title"  ].ToString();
            data.memo    = reader["memo"   ].ToString();
            data.value   = StringUtils.ToInt(reader["value"].ToString(), 0);
            data.path    = reader["path"   ].ToString();
            data.size    = StringUtils.ToLong(reader["size"].ToString(), 0);
            data.created = reader["created"].ToString();
            data.updated = reader["updated"].ToString();

            if (containsTags) {
                data._tags = reader["tags"].ToString();
            }
            
            return data;
        }

        /// <summary>
        /// タグの検索モード
        /// </summary>
        public enum SearchMode {
            AND, //条件に全て一致するデータ
            OR,  //条件のどれかに一致するデータ
            NOT  //条件の全てに一致しないデータ(AND の逆)
        }

        //データを検索します
        public static IList<FileData> GetDataList(SQLiteConnection connection, 
            int valueFrom,
            int valueTo,
            IList<String> keywordList,
            IList<String> tagList,
            SearchMode searchModeTag,
            SearchMode searchModeKeyword
            ) {

            var list = new List<FileData>();

            using (var cmd = connection.CreateCommand()) {
                string sql = 
                    "select *, " + 
                        "(select group_concat(tag, ' ') from d_tag T2 where T2.d_file_id = T1.id group by d_file_id) as tags " + 
                    " from d_file T1 ";

                //where句リスト。1つの条件につき、1要素。最後に "and" で結合して SQL に追加します
                var whereList = new List<string>(); 

                //評価 from
                if (valueFrom >= 0) {
                    whereList.Add("value >= @value_from");
                    cmd.Parameters.Add(new SQLiteParameter("@value_from", valueFrom));
                }

                //評価 to
                if (valueTo >= 0) {
                    whereList.Add("@value_to >= value");
                    cmd.Parameters.Add(new SQLiteParameter("@value_to", valueTo));
                }

                //キーワード
                if (keywordList != null && keywordList.Count > 0) {
                    string and_or_not = null;
                    if (searchModeKeyword.Equals(SearchMode.AND)) and_or_not = " and ";
                    if (searchModeKeyword.Equals(SearchMode.OR))  and_or_not = " or  ";
                    if (searchModeKeyword.Equals(SearchMode.NOT)) and_or_not = " and not ";

                    var where = "";
                    for (int i = 0; i < keywordList.Count; i++) {
                        var paramName = "@keyword_" + i.ToString();
                        where = where + and_or_not + "(" +
                            "title like " + paramName + " or " +
                            "memo  like " + paramName + " or " +
                            "path  like " + paramName + ")";
                        cmd.Parameters.Add(new SQLiteParameter(paramName, "%" + keywordList[i] + "%"));
                    }

                    whereList.Add("(" + where.Substring(4) + ")");
                }

                //タグ
                if (tagList != null && tagList.Count > 0) {
                    
                    string and_or_not = null;
                    if (searchModeTag.Equals(SearchMode.AND)) and_or_not = " and id in ";
                    if (searchModeTag.Equals(SearchMode.OR))  and_or_not = " or  id in ";
                    if (searchModeTag.Equals(SearchMode.NOT)) and_or_not = " and id not in ";

                    var where = "";
                    for (int i = 0; i < tagList.Count; i++) {
                        var paramName = "@tag" + i.ToString();
                        where = where + and_or_not + " (select d_file_id from d_tag where tag = " + paramName + ") ";
                        cmd.Parameters.Add(new SQLiteParameter(paramName, tagList[i]));
                    }

                    whereList.Add("(" + where.Substring(4) + ")");
                }

                //検索条件の追加
                if (whereList.Count > 0) {
                    sql = sql + " where " + string.Join( " and ", whereList.ToArray());
                }

                cmd.CommandText = sql;
                cmd.Prepare();

                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        FileData memoData = ReadData(reader, true);
                        list.Add(memoData);
                    }
                }
            }

            return list;
        } //getDataList()

    } //class
}
