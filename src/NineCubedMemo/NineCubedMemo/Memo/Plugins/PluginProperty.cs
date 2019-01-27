using NineCubed.Common.Collections;
using NineCubed.Common.Files;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins
{
    public class PluginProperty : IniFile
    {
        public const string PropertyFileName = "property.ini";

        /// <summary>
        /// defineのプロパティファイルに差分プロパティを上書きして保存します
        /// </summary>
        /// <param name="plugin">defineの対象プラグイン</param>
        /// <param name="property">プロパティの差分のみ</param>
        public static void SaveToDefine(IPlugin plugin, IniFile property)
        {
            var dirPath = PluginManager.GetDefineDataPath(plugin.GetType().FullName);
            var iniPath = FileUtils.AppendPath(dirPath, PropertyFileName);

            //defineのプロパティファイルを読み込みます
            var defineProp = new IniFile();
            defineProp.Load(iniPath);

            //引数の差分プロパティを、defineのプロパティに上書きします
            defineProp.AddData(property);

            //defineのプロパティファイルを保存します
            defineProp.Save();
        }

    } //class
}
