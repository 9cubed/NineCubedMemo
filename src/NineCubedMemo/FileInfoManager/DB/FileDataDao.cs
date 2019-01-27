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
                    "  title,  memo,  value,  path,  kind,  size,  created,  updated) values " + 
                    "(@title, @memo, @value, @path, @kind, @size, @created, @updated)";
                
                cmd.Parameters.Add(new SQLiteParameter("@title"  , data.title));
                cmd.Parameters.Add(new SQLiteParameter("@memo"   , data.memo));
                cmd.Parameters.Add(new SQLiteParameter("@value"  , data.value));
                cmd.Parameters.Add(new SQLiteParameter("@path"   , data.path));
                cmd.Parameters.Add(new SQLiteParameter("@kind"   , data.kind));
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
                        "kind      = @kind,      " +
                        "size      = @size,      " +
                        "created   = @created,   " +
                        "updated   = @updated    " +
                    " where id = @id";

                cmd.Parameters.Add(new SQLiteParameter("@title"     , data.title));
                cmd.Parameters.Add(new SQLiteParameter("@memo"      , data.memo));
                cmd.Parameters.Add(new SQLiteParameter("@value"     , data.value));
                cmd.Parameters.Add(new SQLiteParameter("@path"      , data.path));
                cmd.Parameters.Add(new SQLiteParameter("@kind"      , data.kind));
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
            data.kind    = StringUtils.ToInt(reader["kind"].ToString(), 0);
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

        /// <summary>
        /// データを検索します
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="valueFrom"></param>
        /// <param name="valueTo"></param>
        /// <param name="keywordList"></param>
        /// <param name="tagList"></param>
        /// <param name="searchModeTag"></param>
        /// <param name="searchModeKeyword"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static IList<FileData> GetDataList(SQLiteConnection connection, 
            int valueFrom,
            int valueTo,
            IList<String> keywordList,
            IList<String> tagList,
            SearchMode searchModeTag,
            SearchMode searchModeKeyword,
            bool fileVisible,
            bool dirVisible,
            int limit
            ) {

            var list = new List<FileData>();

            using (var cmd = connection.CreateCommand()) {
                SetCommandForGetDataList(
                    cmd, valueFrom, valueTo, keywordList, tagList, 
                    searchModeTag, searchModeKeyword, fileVisible, dirVisible, limit, false);

                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        FileData memoData = ReadData(reader, true);
                        list.Add(memoData);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// データの件数を取得します
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="valueFrom"></param>
        /// <param name="valueTo"></param>
        /// <param name="keywordList"></param>
        /// <param name="tagList"></param>
        /// <param name="searchModeTag"></param>
        /// <param name="searchModeKeyword"></param>
        /// <returns></returns>
        public static int GetDataListCount(SQLiteConnection connection, 
            int valueFrom,
            int valueTo,
            IList<String> keywordList,
            IList<String> tagList,
            SearchMode searchModeTag,
            SearchMode searchModeKeyword,
            bool fileVisible,
            bool dirVisible
            ) {

            using (var cmd = connection.CreateCommand()) {
                SetCommandForGetDataList(
                    cmd, valueFrom, valueTo, keywordList, tagList, 
                    searchModeTag, searchModeKeyword, fileVisible, dirVisible, 0, true);

                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        return StringUtils.ToInt(reader["count"].ToString(), 0);
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// 検索用のSQLの設定とパラメーターの設定を行います。
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="valueFrom"></param>
        /// <param name="valueTo"></param>
        /// <param name="keywordList"></param>
        /// <param name="tagList"></param>
        /// <param name="searchModeTag"></param>
        /// <param name="searchModeKeyword"></param>
        /// <param name="limit"></param>
        /// <param name="isCount"></param>
        private static void SetCommandForGetDataList(
            SQLiteCommand cmd,
            int valueFrom,
            int valueTo,
            IList<String> keywordList,
            IList<String> tagList,
            SearchMode searchModeTag,
            SearchMode searchModeKeyword,
            bool fileVisible,
            bool dirVisible,
            int limit,
            bool isCount) //true:件数だけ返す
        {

            string sql;
            if (isCount) {
                sql = "select count(*) as count from d_file T1 ";
            } else {
                sql = "select *, " +
                             "(select group_concat(tag, ' ') from (select tag from d_tag T2 where T2.d_file_id = T1.id order by tag)) as tags " + 
                       " from d_file T1 ";
            }

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
                    where = where + and_or_not + " (select d_file_id from d_tag where lower(tag) = lower(" + paramName + ")) ";
                    cmd.Parameters.Add(new SQLiteParameter(paramName, tagList[i]));
                }

                whereList.Add("(" + where.Substring(4) + ")");
            }

            //ファイル種別
            if (fileVisible == false && dirVisible == false) {
                whereList.Add("kind = 0");
            }
            if (fileVisible == true  && dirVisible == false) {
                whereList.Add("kind = 1");
            }
            if (fileVisible == false && dirVisible == true) {
                whereList.Add("kind = 2");
            }
            if (fileVisible == true  && dirVisible == true) {
                whereList.Add("(kind = 1 or kind = 2)");
            }

            //検索条件の追加
            if (whereList.Count > 0) {
                sql = sql + " where " + string.Join( " and ", whereList.ToArray());
            }

            //検索結果数の制限
            if (limit > 0) sql = sql + " limit " + limit;

            cmd.CommandText = sql;
            cmd.Prepare();
        }

    } //class
}
