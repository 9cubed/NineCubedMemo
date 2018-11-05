﻿using NineCubed.Memo.Interfaces;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;
using NineCubed.Memo.Plugins.TextEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        //デバッグ用 プラグインの削除漏れのチェック
        public void CheckPluginLeak() {
            if (_pluginList.Count() > 0) Console.WriteLine("プラグインリスト削除漏れ");
        }

        private static readonly PluginManager _pluginManager = new PluginManager();
        private static readonly EventManager  _eventManager  = new EventManager();

        /// <summary>
        /// プラグインマネージャーを返します
        /// </summary>
        /// <returns>プラグインマネージャー</returns>
        public static PluginManager GetInstance() {
            return _pluginManager;
        }

        /// <summary>
        /// イベントマネージャーを返します
        /// </summary>
        /// <returns>イベントマネージャー</returns>
        public EventManager GetEventManager() {
            return _eventManager;
        }

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
        public IPlugin ActivePlugin { get; set; }

        /// <summary>
        /// アプリの Config
        /// </summary>
        public AppConfig Config { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PluginManager()
        {    
            //プラグインリストを初期化します
            _pluginList = new List<IPlugin>();
        }

        /// <summary>
        /// プラグインのインスタンスを生成します
        /// </summary>
        /// <returns>生成したプラグイン</returns>
        public IPlugin CreatePluginInstance(Type pluginType, PluginCreateParam param = null, Control parentControl = null)
        {
            //プラグインの型からインスタンス(オブジェクト)を生成します
            var plugin = (IPlugin)Activator.CreateInstance(pluginType);

            //プラグインを初期化します
            var result = plugin.Initialize(param);
            if (result == false) return null;

            //プラグインがコンポーネントを持つ場合は、プラグインのコンポーネントを配置します
            if (plugin.GetComponent() != null) {
                if (parentControl == null) {
                    //割当先が未指定の場合
                    //プラグイン生成イベントを発生させて、プラグインの割り当て先を探します
                    var eventParam = new PluginCreatedEventParam { Plugin = plugin };
                    _pluginManager.GetEventManager().RaiseEvent(PluginCreatedEventParam.Name, null, eventParam);
                } else {
                    //割当先が指定されている場合
                    ((Control)plugin.GetComponent()).Parent = parentControl;
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
            //プラグインを終了できるか？
            if (plugin.CanClosePlugin()) {

                //プラグイン終了イベントを発生させます
                var param = new PluginClosedEventParam { Plugin = plugin };
                _pluginManager.GetEventManager().RaiseEvent(PluginClosedEventParam.Name, null, param);

                //終了したプラグインがアクティブプラグインの場合は、アクティブプラグインを未設定にします
                if (this.ActivePlugin == plugin) this.ActivePlugin = null;

                //プラグインを終了します
                plugin.ClosePlugin();

                //イベントhandlerを全て削除します
                _pluginManager.GetEventManager().RemoveEventHandler(plugin);

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
            int pluginCount = _pluginList.Count(); //リストから削除していくと Count() の値が変わるため、事前にプラグイン数を取得します
            for (int i = 0; i < pluginCount; i++) {
                //プラグインリストの先頭のプラグインを削除します
                //(先頭を削除すると次の要素が先頭になるため、インデックス指定で削除すると、削除漏れが発生します)
                ClosePlugin(_pluginList[0]); 
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



        //TODO 
        //検索条件。全プラグインで共通
        private SearchData _searchData = new SearchData();
        public SearchData GetSearchData() {
            return _searchData;
        }


    } //class
}
