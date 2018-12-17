using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Collections
{
    /// <summary>
    /// TryGetValue() を使わなくてもアクセスできるようにした Dictionary です。
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class Map<TKey, TValue> : Dictionary<TKey, TValue>
    {
        new
        public TValue this[TKey key] {

            get {
                this.TryGetValue(key, out TValue value);
                return value;
            }

            set {
                base[key] = value; //this は new で隠したので base でアクセスする
            }
        }

        //値を取得します
        public string ToString(TKey key, string defaultValue = null)  => this[key]?.ToString();
        public int    ToInt   (TKey key, int    defaultValue = 0)     =>    int.TryParse(this[key]?.ToString(), out int    value) ? value : 0;
        public long   ToLong  (TKey key, long   defaultValue = 0)     =>   long.TryParse(this[key]?.ToString(), out long   value) ? value : 0;
        public double ToDouble(TKey key, double defaultValue = 0)     => double.TryParse(this[key]?.ToString(), out double value) ? value : 0;
        public bool   ToBool  (TKey key, bool   defaultValue = false) =>   bool.TryParse(this[key]?.ToString(), out bool   value) ? value : false;

    } //class
}
