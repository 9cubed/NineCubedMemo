using NineCubed.Memo.Interfaces;
using NineCubed.Memo.Plugins.Interfaces;
using NineCubed.Memo.Plugins.TextEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins
{
    /// <summary>
    /// プラグインマネージャー
    /// プラグインを管理するクラス。
    /// </summary>
    public class PluginManager
    {
        private static readonly PluginManager _pluginManager = new PluginManager();

        /// <summary>
        /// プラグインマネージャーを返します
        /// </summary>
        public static PluginManager GetPluginManager() {
            return _pluginManager;
        }

        /// <summary>
        /// タイトル変更イベント
        /// </summary>
        /// <param name="plugin"></param>
        public delegate void TitleChangedEventHandler(IPlugin plugin);
        public event TitleChangedEventHandler TitleChanged = null;

        /// <summary>
        /// タイトル変更イベントを発生させます。
        /// プラグインから呼び出します。
        /// </summary>
        /// <param name="plugin"></param>
        public void RaiseTitleChangedEvent(IPlugin plugin) {
            if (this.TitleChanged != null) TitleChanged(plugin);
        }

        /// <summary>
        /// プラグインリスト
        /// 
        /// 生成された全てのプラグインを格納するリスト
        /// プラグイン生成時にリストに追加して、プラグイン終了時にリストから削除します
        /// 
        /// アプリを終了する際に、全てのプラグインに対して、終了できるかどうかを確認するのに使用します
        /// </summary>
        public IList<IPlugin> _pluginList;

        /// <summary>
        /// アクティブプラグイン
        /// 現在フォーカスがあるプラグイン。
        /// アクティブになったプラグイン側から設定します。
        /// </summary>
        public IPlugin ActivePlugin { get; set; }

        /// <summary>
        /// 拡張子に対応するプラグインの型
        /// Key  :拡張子
        /// Value:プラグインの型
        /// </summary>
        private Dictionary<string, Type> _PluginTypeDict;


        /// <summary>
        /// AppConfig
        /// </summary>
        public AppConfig Config { get; set; }



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PluginManager() {
            _PluginTypeDict = new Dictionary<string, Type>();
            _PluginTypeDict[".txt"] = typeof(TextEditorPlugin);

            //プラグインリストを初期化します
            _pluginList = new List<IPlugin>();
        }


        /// <summary>
        /// 拡張子に対応するプラグインの型を返します
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Type GetPluginType(string path) {
            //拡張子を取得します
            var ext = Path.GetExtension(path).ToLower();

            //拡張子に対応するプラグインがある場合は、プラグインを生成します
            if (_PluginTypeDict.TryGetValue(ext, out Type pluginType)) {
                return pluginType;
            } else {
                return null;
            }
        }

        /// <summary>
        /// パスに対応するプラグインのインスタンスを生成します
        /// </summary>
        /// <param name="path"></param>
        /// <returns>生成したプラグイン</returns>
        public IPlugin CreatePluginInstance(Type pluginType, string path) {

            //プラグインの型が未指定の場合は、拡張子に対応するプラグインの型を取得します
            if (pluginType == null) {
                pluginType = GetPluginType(path);
                if (pluginType == null) return null; //生成失敗
            }

            //プラグインの型からインスタンス(オブジェクト)を生成します
            var plugin = (IPlugin)Activator.CreateInstance(pluginType);

            //プラグインをリストに追加します
            _pluginList.Add(plugin);

            return plugin;
        }

        /// <summary>
        /// プラグインをリストから削除します
        /// </summary>
        /// <param name="plugin"></param>
        public void RemovePlugin(IPlugin plugin) {
            _pluginList.Remove(plugin);
        }

        /// <summary>
        /// プラグインを終了します
        /// </summary>
        public void ClosePlugin(IPlugin plugin) {
            if (plugin.CanClosePlugin()) {
                plugin.ClosePlugin();
            }
        }

        /// <summary>
        /// 全てのプラグインが終了できるか？
        /// </summary>
        /// <returns></returns>
        public bool CanCloseAllPlugins() {
            foreach(var plugin in _pluginList) {
                if (plugin.CanClosePlugin() == false) return false;
            }
            return true;
        }

        /// <summary>
        /// 全てのプラグインを終了します
        /// </summary>
        public void CloseAllPlugins() {
            foreach(var plugin in _pluginList) {
                plugin.ClosePlugin();
            }
        }


        //TODO 
        //検索条件。全プラグインで共通
        private SearchData _searchData = new SearchData();
        public SearchData GetSearchData() {
            return _searchData;
        }


    } //class
}
