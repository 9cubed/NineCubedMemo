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
        /// <param name="data"></param>
        public static void Insert(SQLiteConnection connection, TagData data) {
            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = "insert into d_tag (tag, d_file_id) values (@tag, @d_file_id)";
                cmd.Parameters.Add(new SQLiteParameter("@tag"       , data.tag));
                cmd.Parameters.Add(new SQLiteParameter("@d_file_id" , data.d_file_id));
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

        /// <summary>
        /// タグデータを取得して、スペース区切りにして返します
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="d_file_id"></param>
        /// <returns></returns>
        public static string GetTags(SQLiteConnection connection, int d_file_id) {
            var list = new List<string>();
            using (var cmd = connection.CreateCommand())
            {
                var sql = "select * from d_tag where d_file_id = @d_file_id";
                cmd.Parameters.Add(new SQLiteParameter("@d_file_id", d_file_id));
                cmd.CommandText = sql;
                cmd.Prepare();
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        string tag = reader["tag"].ToString().ToLower();
                        list.Add(tag);
                    }
                }
            }

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
        public static IList<TagData>GetAllTagList(SQLiteConnection connection, int sortKind = 0, bool isSortAsc = true) {
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
