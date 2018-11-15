using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Collections
{
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

    } //class
}
