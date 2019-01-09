using NineCubed.Common.Collections;
using NineCubed.Common.Files;
using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.PluginLoader
{
    /// <summary>
    /// プラグイン定義データ
    /// 
    /// plugins/define/{フルクラス名}/フルクラス名.ini の値を保持します
    /// 
    /// </summary>
    public class PluginDefineData
    {
        /// <summary>
        /// プラグイン定義データ
        /// 
        /// キー:プラグインのフルクラス名
        /// 値  :iniファイルデータ
        /// </summary>
        private readonly Map<string, IniFile> _pluginDefineData = new Map<string, IniFile>();

        /// <summary>
        /// プラグインの定義データを全て読み込みます
        /// plugins/{フルクラス名}/フルクラス名.ini を全て読み込みます
        /// </summary>
        public void Load()
        {
            //pluginsフォルダ配下のフォルダ一覧を取得します
            var dirList = FileUtils.GetDirList(FileUtils.AppendPath(__.GetAppDirPath(), "plugins/define"));

            //フォルダ一覧でループして、iniファイルを読み込みます
            foreach (var dirPath in dirList) {

                //iniファイルのパスを作成します
                var pluginClassName = Path.GetFileName(dirPath);
                var iniFileName = "plugin.ini"; //パスが長くなるため廃止:pluginClassName + ".ini";
                var iniPath = FileUtils.AppendPath(dirPath, iniFileName);

                //ファイルチェック
                if (File.Exists(iniPath)) {
                    //iniファイルを読み込みます
                    var iniFile = new IniFile();
                    iniFile.Load(iniPath);

                    //iniファイルをプラグイン名をキーにして保持します
                    _pluginDefineData[pluginClassName] = iniFile;

                } else {
                    //TODO warningをログとして残す。残骸ファイルがあっても、動作上は問題ないため。
                    //throw new Exception(iniPath + "が見つかりません。");
                }
            }
        }

        /// <summary>
        /// プラグインが含まれているEXEまたはDLLのパスを返します
        /// </summary>
        /// <param name="pluginClassName">プラグインのフルクラス名</param>
        /// <returns></returns>
        public string GetAssemblyPath(string pluginClassName)
        {
            var iniFile = _pluginDefineData[pluginClassName];
            if (iniFile == null) return null;

            var path = iniFile["assembly", "path"];
            return path;
        }

    } //class
}
