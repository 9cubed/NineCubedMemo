using NineCubed.Common.Collections;
using NineCubed.Common.Files;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;
using NineCubed.Memo.Plugins.PluginLoader;
using NineCubed.Memo.Plugins.TextEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
        //デバッグ用 プラグインの削除漏れのチェック
        public void CheckPluginLeak() {
            if (_pluginList.Count() > 0) Console.WriteLine("プラグインリスト削除漏れ");
        }

        private static PluginManager    _pluginManager = new PluginManager();
        private static EventManager     _eventManager;
        private static PluginDefineData _pluginDefineData;

        /// <summary>
        /// プラグインマネージャーを返します
        /// </summary>
        /// <returns>プラグインマネージャー</returns>
        public static PluginManager GetInstance() => _pluginManager;

        /// <summary>
        /// イベントマネージャーを返します
        /// </summary>
        /// <returns>イベントマネージャー</returns>
        public EventManager GetEventManager() => _eventManager;

        /// <summary>
        /// プラグイン定義データを返します
        /// </summary>
        /// <returns>プラグイン定義データ</returns>
        public PluginDefineData GetPluginDefineData() => _pluginDefineData;

        /// <summary>
        /// プラグインのフルクラス名と型を関連付けるMap
        /// Key  :プラグインのフルクラス名
        /// Value:プラグインのクラスの型
        /// </summary>
        private Map<string, Type> _classTypeMap = new Map<string, Type>();

        /// <summary>
        /// プラグインリスト
        /// 
        /// 生成された全てのプラグインを格納するリスト
        /// プラグイン生成時にリストに追加して、プラグイン終了時にリストから削除します
        /// 
        /// アプリを終了する際に、全てのプラグインに対して、終了できるかどうかを確認するのに使用します
        /// </summary>
        private IList<IPlugin> _pluginList;

        /// <summary>
        /// アクティブプラグイン
        /// 現在フォーカスがあるプラグイン。
        /// アクティブになったプラグイン側から設定します。
        /// </summary>
        public IPlugin _activePlugin;
        public IPlugin ActivePlugin {
            get
            {
                return _activePlugin;
            }
            set
            {
                _activePlugin = value;
            }
        }

        /// <summary>
        /// メインフォーム
        /// </summary>
        public Form MainForm { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PluginManager()
        {    
            //プラグインリストを初期化します
            _pluginList = new List<IPlugin>();

            //イベントマネージャーを生成します
            _eventManager = new EventManager();

            //プラグイン共通データフォルダが存在しない場合は、フォルダを作成します
            //フォルダ : plugins/data/common/
            var dataPath = GetCommonDataPath();
            FileUtils.CreateDir(dataPath);

            //プラグイン定義データを読み込みます
            _pluginDefineData = new PluginDefineData();
            _pluginDefineData.Load();
        }

        //起動します
        public void Startup()
        {
            //startup.ini を読み込み、プラグインローダーのクラス名を取得します
            var startupIni = new IniFile();
            try {
                startupIni.Load(FileUtils.AppendPath(__.GetAppDirPath(), "plugins/startup.ini"));
            } catch (Exception ex) {
                MessageBox.Show("startup.ini の読み込みに失敗しました。" + ex.Message);
                return;
            }

            //最初に起動するプラグイン(プラグインローダー)のクラス名を取得します
            var pluginLoaderClassName = startupIni["startup", "class_name"];
            if (string.IsNullOrEmpty(pluginLoaderClassName)) {
                MessageBox.Show("startup.ini の [startup] の class_name が見つかりません。");
                return;
            }

            //クラス名 -> クラスの型
            var pluginLoaderType = GetPluginType(pluginLoaderClassName);
            if (pluginLoaderType == null) {
                MessageBox.Show(pluginLoaderClassName + "の型の取得に失敗しました。startup.ini のクラス名を確認してください。");
                return;
            }

            //プラグインを生成します
            var param = new PluginCreateParam();
            var plugin = CreatePluginInstance(pluginLoaderType, param, this, null, "plugin_loader");
        }

        /// <summary>
        /// プラグインのインスタンスを生成します
        /// 
        /// 指定したプラグインIDと同じプラグインが既にある場合は、そのプラグインを返します。
        /// </summary>
        /// <param name="pluginType">プラグインのクラスの型</param>
        /// <param name="param">プラグイン生成用パラメーター</param>
        /// <param name="sender">実行元オブジェクト</param>
        /// <param name="parentPlugin">プラグインのコントロールを割り当てるプラグイン</param>
        /// <param name="pluginId">プラグインID。未指定の場合は自動で採番されます。</param>
        /// <returns>生成したプラグイン</returns>
        public IPlugin CreatePluginInstance(Type pluginType, PluginCreateParam param, Object sender, IPlugin parentPlugin = null, string pluginId = null)
        {
            //プラグインID「common」は、プラグイン共通のファイル置き場のため、使用禁止です。
            if ("common".Equals(pluginId)) throw new Exception("common は予約語のため、プラグインID で使用できません。");

            IPlugin plugin;

            //既に同じプラグインIDのプラグインがある場合は、そのプラグインを返します
            if (pluginId != null) {
                plugin = GetPlugin(pluginId);
                if (plugin != null) return plugin;
            }

            //プラグインの型からインスタンス(オブジェクト)を生成します
            plugin = (IPlugin)Activator.CreateInstance(pluginType);

            //プラグインIDを未指定の場合は生成して設定します
            if (pluginId == null) pluginId = GetPluginId();
            plugin.PluginId = pluginId;

            //デフォルトデータフォルダのパスを取得します
            var defineDataPath = GetDefineDataPath(plugin.GetType().FullName);

            //データフォルダのパスを取得します
            var dataPath = GetDataPath(pluginId);

            //データフォルダが存在しない場合は、フォルダを作成します
            if (Directory.Exists(dataPath) == false) {
                Directory.CreateDirectory(dataPath);
            }

            //デフォルトデータフォルダとデータフォルダを比較して、
            //デフォルトデータが新しい場合はデータフォルダへコピーします
            FileUtils.CopyDir(defineDataPath, dataPath, FileUtils.FileCopyMode.CopyIfNewer);

            //プラグインを初期化します
            if (param == null) param = new PluginCreateParam();
            param.DataPath = dataPath; //データパスを設定します
            try {
                var result = plugin.Initialize(param);

                //初期化に失敗した場合は、プラグインを破棄します
                if (result == false) return null; 
            } catch (Exception ex) {
                MessageBox.Show(ex.Message + "\n" + plugin.GetType().FullName); //TODO ただのログ出力にする
                return null;
            }

            //プラグインの割り当て
            //プラグインがコンポーネントを持つ場合は、プラグインのコンポーネントを配置します
            if (plugin.GetComponent() != null) {

                if (parentPlugin == null) {
                    //割当先が未指定の場合
                    //コントロールがフォーム以外か？
                    if ((plugin.GetComponent() is Form) == false) {
                        //フォーム以外の場合
                        //プラグイン生成イベントを発生させて、プラグインの割り当て先を探します
                        var eventParam = new PluginCreatedEventParam { Plugin = plugin };
                        var eventHandledPlugin = _pluginManager.GetEventManager().RaiseEvent(PluginCreatedEventParam.Name, sender, eventParam);

                        //親プラグインを設定します
                        if (eventHandledPlugin != null) plugin.ParentPlugin = eventHandledPlugin;
                    }
                } else {
                    //割当先が指定されている場合
                    if (plugin.GetComponent() is IComponent component) {
                        //指定されたプラグインに対してだけ、プラグイン生成イベントを発生させます
                        var eventParam = new PluginCreatedEventParam { Plugin = plugin };
                        var eventHandledPlugin = _pluginManager.GetEventManager().RaiseEvent(PluginCreatedEventParam.Name, sender, eventParam, parentPlugin);

                        //親プラグインを設定します
                        if (eventHandledPlugin != null) plugin.ParentPlugin = eventHandledPlugin;
                    }
                }
            }

            //プラグイン配置後の初期化を行います
            plugin.InitializePlaced();

            //プラグインをリストに追加します
            _pluginList.Add(plugin);

            return plugin;
        }

        /// <summary>
        /// プラグインを終了します
        /// </summary>
        public bool ClosePlugin(IPlugin plugin) {

            //子プラグインリストを終了します
            //1つでも終了できないプラグインがある場合は、終了を中止します
            var childPluginList = GetChildPluginList(plugin);
            foreach (var childPlugin in childPluginList) {
                if (ClosePlugin(childPlugin) == false) return false;
            }

            //プラグインを終了できるか？
            if (plugin.CanClosePlugin()) {

                //プラグイン終了イベントを発生させます
                var param = new PluginClosedEventParam { Plugin = plugin };
                _pluginManager.GetEventManager().RaiseEvent(PluginClosedEventParam.Name, this, param);

                //終了したプラグインがアクティブプラグインの場合は、アクティブプラグインを未設定にします
                if (this.ActivePlugin == plugin) this.ActivePlugin = null;

                //プラグインを終了します
                plugin.ClosePlugin();

                //イベントhandlerを全て削除します
                _pluginManager.GetEventManager().RemoveEventHandler(plugin);

                //プラグインID が GUID の場合(自動採番された場合)は、フォルダを削除します
                if (IsGuid(plugin.PluginId)) {
                    var dataPath = GetDataPath(plugin.PluginId);
                    FileUtils.DeleteDir(dataPath);
                }

                //プラグインリストからプラグインを削除します
                _pluginList.Remove(plugin);

                return true;
            } else {
                return false;
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
            //プラグインがなくなるまで削除します
            while (_pluginList.Count() > 0) {
                if (ClosePlugin(_pluginList[0]) == false) break;
            }
        }

        /// <summary>
        /// 指定したコンポーネントを保持するプラグインを返します
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public IPlugin GetPlugin(Component component)
        {
            foreach(var plugin in _pluginList) {
                if (plugin.GetComponent() == component) {
                    return plugin;
                }
            }
            return null;
        }

        /// <summary>
        /// 拡張子に対応するプラグインの型を返します
        /// </summary>
        /// <param name="pluginClassName">プラグインのフルクラス名</param>
        /// <returns></returns>
        public Type GetPluginType(string pluginClassName) {

            //クラスと型の関連付けマップから、フルクラス名をキーにしてクラスの型を取得します
            if (_classTypeMap.TryGetValue(pluginClassName, out Type pluginType)) {
                //取得できた場合
                return pluginType;
            }
            //取得できなかった場合
            //EXE または DLL からクラスの型を取得します

            //プラグインのクラスが定義された EXE または DLL のパスを取得します
            var assemblyPath = _pluginDefineData.GetAssemblyPath(pluginClassName);
            if (assemblyPath == null) return null;

            //Assemblyを取得します
            var assembly = Assembly.LoadFrom(assemblyPath);

            //プラグインのクラスの型を取得します
            pluginType = assembly.GetType(pluginClassName);
            if (pluginType != null) {
                //プラグインのフルクラス名をキーにして型を保持します(型のキャッシュ)
                _classTypeMap[pluginClassName] = pluginType;
            } 

            return pluginType;
        }

        /// <summary>
        /// プラグイン用デフォルトデータフォルダのパスを返します
        /// </summary>
        /// <param name="pluginClassName"></param>
        /// <returns></returns>
        public static string GetDefineDataPath(string pluginClassName)
        {
            return FileUtils.AppendPath(__.GetAppDirPath(), "plugins/define/" + pluginClassName + "/data");
        }

        /// <summary>
        /// プラグイン用データフォルダのパスを返します
        /// </summary>
        /// <param name="pluginId">プラグインID</param>
        /// <returns></returns>
        public static string GetDataPath(string pluginId)
        {
            return FileUtils.AppendPath(__.GetAppDirPath(), "plugins/data/" + pluginId + "/");
        }

        /// <summary>
        /// プラグイン用共通データフォルダのパスを返します
        /// </summary>
        /// <returns></returns>
        public static string GetCommonDataPath() => GetDataPath("common");

        /// <summary>
        /// プラグインIDを採番して返します
        /// </summary>
        /// <returns></returns>
        public static string GetPluginId() => Guid.NewGuid().ToString();

        /// <summary>
        /// 指定したプラグインIDのプラグインを返します
        /// </summary>
        /// <param name="pluginId"></param>
        /// <returns></returns>
        public IPlugin GetPlugin(string pluginId)
        {
            foreach (var plugin in _pluginList) {
                if (plugin.PluginId.Equals(pluginId)) {
                    return plugin;
                }
            }
            return null;
        }

        /// <summary>
        /// 子プラグインをリストで返します
        /// </summary>
        /// <returns></returns>
        public IList<IPlugin> GetChildPluginList(IPlugin parentPlugin)
        {
            var list = new List<IPlugin>();

            //全プラグインの親を取得して、親が引数のプラグインの場合は、リストに追加します
            foreach (var plugin in _pluginList) {
                if (parentPlugin == plugin.ParentPlugin) {
                    list.Add(plugin);
                }
            }
            return list;
        }

        /// <summary>
        /// GUIDの形式チェックを行います
        /// </summary>
        /// <param name="guid">true:GUIDです。</param>
        /// <returns></returns>
        public static bool IsGuid(string guid) {
            //事前に桁数チェックをして、桁数が異なるものははじきます
            if ("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx".Length != guid.Length) return false; //GUIDではない(桁数不一致)

            if (Guid.TryParse(guid, out Guid result) == false) return false; //パース失敗
            return true;
        }

        /// <summary>
        /// 共通データ
        /// 
        /// プラグイン間でデータの受け渡しをするための、
        /// 全プラグインでアクセス可能なデータです。
        /// 
        /// </summary>
        public Map<object, object> CommonData { get; } = new Map<object, object>();

    } //class

    /// <summary>
    /// 共通データ用のキー
    /// よく使うものだけ
    /// </summary>
    public enum CommonDataKeys {

                    //CommonData の値
        SearchData, //NineCubed.Memo.Plugins.SearchForm.SearchData  検索データ
        ColorData   //NineCubed.Memo.Plugins.Theme.ColorData        テーマ用カラーデータ
    }

}
