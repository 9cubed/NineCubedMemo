using NineCubed.Common.Collections;
using NineCubed.Common.Utils;
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

        /// <summary>
        /// プラグインで開くファイルのパス
        /// </summary>
        public string Path {
            get { return (string)this["path"]; }
            set {                this["path"] = value; }
        }

        /// <summary>
        /// データフォルダのパス
        /// plugins/data/{プラグインID}/
        /// </summary>
        public string DataPath {
            get { return (string)this["data_path"]; }
            set {                this["data_path"] = value; }
        }

        /// <summary>
        /// データフォルダ配下のプロパティファイルのパス
        /// </summary>
        public string PropertyPath {
            get { return FileUtils.AppendPath(DataPath, "property.ini"); }
        }
        
    } //class
}
