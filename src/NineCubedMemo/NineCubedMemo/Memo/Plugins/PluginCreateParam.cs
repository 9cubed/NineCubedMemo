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
    public class PluginCreateParam : Dictionary<string, object>
    {
        //値を取得します
        private T ToValue<T>(string key) 
        {
            return this.TryGetValue(key, out object value) ? (T)value : default(T);
        }

        //値を取得します
        public object ToObject(string key) => ToValue<object>(key);
        public string ToString(string key) => ToValue<string>(key);
        public int    ToInt   (string key) => ToValue<int>   (key);
        public long   ToLong  (string key) => ToValue<long>  (key);
        public bool   ToBool  (string key) => ToValue<bool>  (key);

    } //class
}
