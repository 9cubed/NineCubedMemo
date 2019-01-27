using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileInfoManager.DB
{
    public class TagDataDao
    {
        /// <summary>
        /// データを追加します
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tag"></param>
        /// /// <param name="d_file_id"></param>
        public static void Insert(SQLiteConnection connection, string tag, int d_file_id) {
            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = "insert into d_tag (tag, d_file_id) values (@tag, @d_file_id)";
                cmd.Parameters.Add(new SQLiteParameter("@tag"       , tag));
                cmd.Parameters.Add(new SQLiteParameter("@d_file_id" , d_file_id));
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// データを削除します
        /// タグの更新はDelete-Insert方式にするので、その際に使用します
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="d_file_id"></param>
        public static void Delete(SQLiteConnection connection, int d_file_id) {
            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = "delete from d_tag where d_file_id = @d_file_id ";
                cmd.Parameters.Add(new SQLiteParameter("@d_file_id", d_file_id));
                cmd.ExecuteNonQuery();
            }
        }
        public static void Delete(SQLiteConnection connection, string tag, int d_file_id) {
            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = "delete from d_tag where tag = @tag and d_file_id = @d_file_id ";
                cmd.Parameters.Add(new SQLiteParameter("@tag"      , tag));
                cmd.Parameters.Add(new SQLiteParameter("@d_file_id", d_file_id));
                cmd.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// リストで指定された複数のタグをDBに追加します。
        /// 既存のタグと重複するか事前にチェックして、重複する場合は追加しないようにします。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="d_file_id"></param>
        /// <param name="tagList">追加するタグリスト</param>
        public static void AddTags(SQLiteConnection connection, int d_file_id, IList<string> tagList) {
            //既存のタグリストを取得します
            var dbTagList = GetTagList(connection, d_file_id);

            //追加のタグリストと既存のタグリストを比較して、存在しないものだけをリストにします
            var addTagList = tagList.Where(tag => dbTagList.Contains(tag) == false).ToList<string>();

            //DBにタグを追加します
            foreach (var tag in addTagList) {
                Insert(connection, tag, d_file_id);
            }
        }

        /// <summary>
        /// タグデータを取得して、リストで返します
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="d_file_id"></param>
        /// <returns></returns>
        public static IList<string> GetTagList(SQLiteConnection connection, int d_file_id) {
            var list = new List<string>();
            using (var cmd = connection.CreateCommand())
            {
                var sql = "select * from d_tag where d_file_id = @d_file_id";
                cmd.Parameters.Add(new SQLiteParameter("@d_file_id", d_file_id));
                cmd.CommandText = sql;
                cmd.Prepare();
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        string tag = reader["tag"].ToString();
                        list.Add(tag);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// タグデータを取得して、スペース区切りにして返します
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="d_file_id"></param>
        /// <returns></returns>
        public static string GetTags(SQLiteConnection connection, int d_file_id) {
            //タグリストを取得します
            var list = GetTagList(connection, d_file_id);

            //スペース区切りの文字列にして返します
            return string.Join(" ", list);
        }
        
        /// <summary>
        /// 全てのタグを取得します。
        /// タグに割り当てられているファイル数を返します
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sortKind"></param>
        /// <param name="isSortAsc"></param>
        /// <returns></returns>
        public static IList<TagData> GetAllTagList(SQLiteConnection connection, int sortKind = 0, bool isSortAsc = true) {
            string asc_desc = isSortAsc ? " asc" : " desc";

            var list = new List<TagData>();
            using (var cmd = connection.CreateCommand())
            {
                string sql = "select tag, count(*) as file_count from d_tag group by tag ";

                if (sortKind == 1) {
                    //五十音順
                    sql = sql + "order by tag " + asc_desc;
                }
                if (sortKind == 2) {
                    //メモ数順
                    sql = sql + "order by file_count " + asc_desc;
                }

                //SQLの実行
                cmd.CommandText = sql;
                cmd.Prepare();
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        var tagData = new TagData {
                            tag        = reader["tag"].ToString(),
                            _fileCount = StringUtils.ToInt(reader["file_count"].ToString())
                        };
                        list.Add(tagData);
                    }
                }
            }
            return list;
        }

    } //class
}
