using NineCubed.Common.Collections;
using NineCubed.Common.Files;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;
using NineCubed.Memo.Plugins.TextEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.PluginLoader
{
    /// <summary>
    /// プラグインを生成するためのプラグインです。
    /// </summary>
    public class PluginLoaderPlugin : IPlugin
    {
        /// <summary>
        /// 拡張子とプラグインのフルクラス名を関連付けるIniファイル
        /// Key1 :IniFile.NO_SECTION
        /// Key2 :拡張子
        /// Value:プラグインのフルクラス名
        /// </summary>
        private readonly IniFile _pluginExtIni = new IniFile();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PluginLoaderPlugin() { }

        /// <summary>
        /// 拡張子に対応するプラグインの型を返します
        /// </summary>
        /// <param name="path">ファイルのパス</param>
        /// <returns></returns>
        public Type GetPluginType(string path) {
            //拡張子を取得します
            var ext = Path.GetExtension(path).ToLower();

            //拡張子に対応するプラグインのフルクラス名を取得します
            Type pluginType = null;
            var pluginClassName = _pluginExtIni[IniFile.NO_SECTION, ext];
            if (pluginClassName != null) {
                //取得できた場合
                //クラス名をキーより、プラグインの型を取得します
                pluginType = _pluginManager.GetPluginType(pluginClassName);
            }

            //拡張子に対応するプラグインがなかった場合
            return pluginType;
        }

        /******************************************************************************
         * 
         *  IPlugin
         * 
         ******************************************************************************/
        private PluginManager _pluginManager = null;       //プラグインマネージャー
        public string     PluginId         { get; set; }    //プラグインID
        public IPlugin    ParentPlugin     { get; set; }    //親プラグイン
        public IComponent GetComponent()   { return null; } //プラグインのコンポーネントを返します
        public string     Title            { get; set; }    //プラグインのタイトル
        public bool       CanClosePlugin() { return true; } //プラグインが終了できるかどうか
        public void       ClosePlugin()    { }              //プラグインの終了処理
        public void       SetFocus()       { }              //フォーカスを設定します
        public void       InitializePlaced() { }            //プラグイン配置後の初期化処理を行います

        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //イベントハンドラーを登録します
            _pluginManager.GetEventManager().AddEventHandler( FileSelectedEventParam.Name, this);

            //plugin_extension.ini を読み込みます
            _pluginExtIni.Load( FileUtils.AppendPath(param.DataPath, "plugin_extension.ini") );

            //plugin_list.ini を読み込みます
            var pluginListIni = new PluginListIni(); //new IniFile();
                pluginListIni.Load( FileUtils.AppendPath(param.DataPath, "plugin_list.ini") );

            //plugin_list.ini で指定されているプラグインを全て生成します
            CreatePlugin(pluginListIni);

            //全てのプラグインの生成が完了したことを通知するイベントを発生させます
            var eventParam = new AllPluginCreatedEventParam();
            _pluginManager.GetEventManager().RaiseEvent(AllPluginCreatedEventParam.Name, this, eventParam);

            return true;
        }

        //plugin_list.iniファイルに定義されているプラグインを全て生成します
        private void CreatePlugin(IniFile pluginListIni)
        {
            int no = 1; //pluginセクションの連番
            Map<string, string> subData;
            //セクションが見つからなくなるまでループします
            while ((subData = pluginListIni.GetSubData("plugin_" + no++)) != null) {
                var pluginClassName = subData["class"];
                var pluginId        = subData["id"];
                var pluginParam     = subData["param"];
                var pluginParentId  = subData["parent_id"]; //割当先の親プラグイン
                var pluginDock      = subData["dock"];      //割当先の DockStyle
                
                //iniファイルのチェック
                if (string.IsNullOrEmpty(pluginClassName)) {
                    throw new Exception("plugin_list.ini の [" + "plugin_" + no +"] に class が定義されていません。");
                }

                //プラグインの型を取得します
                var pluginType = _pluginManager.GetPluginType(pluginClassName);
                if (pluginType == null) {
                    throw new Exception("plugin_list.ini の " + pluginClassName + " の型の取得に失敗しました。\ndefine配下のプラグイン定義ファイル(フルクラス名.ini)のプロパティが、「コピーする」になっていることを確認してください。");
                }

                //プラグインの生成パラメーターを生成します
                var param = new PluginCreateParam();
                if (string.IsNullOrEmpty(pluginParam) == false) {
                    //csv 形式の ini(キー = 値) を分解してパラメーターに設定します
                    //(例)「param = orientation = horizontal, width = 320, height = 100」を解析して、param に設定する
                    var list = pluginParam.Split(',');
                    foreach (var item in list) {
                        var (key, value) = StringUtils.GetKeyValue(item);
                        param[key] = value;
                    }
                }

                //割当先が指定されている場合は、割当先のプラグインを取得します
                IPlugin parentPlugin = null;
                if (pluginParentId != null) {
                    //親プラグインIDをキーにして、親プラグインを取得します
                    parentPlugin = _pluginManager.GetPlugin(pluginParentId);
                }
                
                //プラグインを生成します
                var plugin = _pluginManager.CreatePluginInstance(pluginType, param, this, parentPlugin, pluginId);

                //Dockが指定されている場合は、Dockを設定します
                if (pluginDock != null) {
                    if (plugin.GetComponent() is Control control) {
                        //文字列を DockStyle の Enum に変換して Dock に設定します
                        if (Enum.TryParse(pluginDock, true, out DockStyle dock)) {
                            control.Dock = dock;
                        }
                    }
                }
            }
        }


        /******************************************************************************
         * 
         *  プラグイン用イベントハンドラー
         * 
         ******************************************************************************/ 
        
        /// <summary>
        /// ファイル選択イベント
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sender"></param>
        public void PluginEvent_FileSelected(EventParam param, object sender)
        {
            //選択されたファイルのパスを取得します
            var path = ((FileSelectedEventParam)param).Path;

            //プラグインの型が未指定の場合は、拡張子に対応するプラグインのフルクラス名を取得します
            var pluginType = GetPluginType(path);
            if (pluginType == null) {
                //プラグインの型が取得できない場合は、別のアプリで開きます
                Process.Start(path);
                return;
            }

            //プラグイン生成パラメーターを設定します
            var pluginCreateParam = new PluginCreateParam();
            pluginCreateParam.Path = path; //選択されたパス

            //プラグインを生成します
            var plugin = _pluginManager.CreatePluginInstance(pluginType, pluginCreateParam, this);
        }

    } //class
}
