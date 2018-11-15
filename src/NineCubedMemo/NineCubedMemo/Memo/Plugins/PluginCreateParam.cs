using NineCubed.Common.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins
{
    /// <summary>
    /// プラグイン生成用パラメーター
    /// </summary>
    public class PluginCreateParam : Map<string, object>
    {
        //よく使うものだけ定義しておきます

        public string Path {
            get { return (string)this["path"]; }
            set {                this["path"] = value; }
        }

        public string DataPath {
            get { return (string)this["data_path"]; }
            set {                this["data_path"] = value; }
        }

        //値を取得します
        private T ToValue<T>(string key) => this.TryGetValue(key, out object value) ? (T)value : default(T);
        public object ToObject(string key) => ToValue<object>(key);
        public string ToString(string key) => ToValue<string>(key);
        public int    ToInt   (string key) => ToValue<int>   (key);
        public long   ToLong  (string key) => ToValue<long>  (key);
        public bool   ToBool  (string key) => ToValue<bool>  (key);

    } //class
}
