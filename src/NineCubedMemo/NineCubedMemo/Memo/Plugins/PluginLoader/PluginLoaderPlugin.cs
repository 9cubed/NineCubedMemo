using NineCubed.Common.Files;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;
using NineCubed.Memo.Plugins.TextEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.PluginLoader
{
    /// <summary>
    /// プラグインを生成するためのプラグインです。
    /// </summary>
    public class PluginLoaderPlugin : IPlugin
    {
        /// <summary>
        /// 拡張子に対応するプラグインの型
        /// Key  :拡張子
        /// Value:プラグインの型
        /// </summary>
        private Dictionary<string, Type> _pluginTypeDict;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PluginLoaderPlugin()
        {
            //拡張子とプラグインの関連付けをします。TODO ファイルで定義するようにする
            _pluginTypeDict = new Dictionary<string, Type>();
            _pluginTypeDict[".txt"]  = typeof(TextEditorPlugin);
            _pluginTypeDict[".bat"]  = typeof(TextEditorPlugin);
            _pluginTypeDict[".log"]  = typeof(TextEditorPlugin);
            _pluginTypeDict[".csv"]  = typeof(TextEditorPlugin);
            _pluginTypeDict[".dat"]  = typeof(TextEditorPlugin);
            _pluginTypeDict[".htm"]  = typeof(TextEditorPlugin);
            _pluginTypeDict[".html"] = typeof(TextEditorPlugin);
            _pluginTypeDict[".xml"]  = typeof(TextEditorPlugin);
            _pluginTypeDict[".js"]   = typeof(TextEditorPlugin);
            _pluginTypeDict[".json"] = typeof(TextEditorPlugin);
            _pluginTypeDict[".php"]  = typeof(TextEditorPlugin);
            _pluginTypeDict[".css"]  = typeof(TextEditorPlugin);
            _pluginTypeDict[".ini"]  = typeof(TextEditorPlugin);


            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //イベントハンドラーを登録します
            _pluginManager.GetEventManager().AddEventHandler( FileSelectedEventParam.Name, this);
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
            if (_pluginTypeDict.TryGetValue(ext, out Type pluginType)) {
                return pluginType;
            } else {
                return null;
            }
        }


        /******************************************************************************
         * 
         *  IPlugin
         * 
         ******************************************************************************/ 
        private PluginManager _pluginManager = null;       //プラグインマネージャー
        public Component GetComponent()   { return null; } //プラグインのコンポーネントを返します
        public string    Title            { get; set; }    //プラグインのタイトル
        public bool      CanClosePlugin() { return true; } //プラグインが終了できるかどうか
        public void      ClosePlugin()    { }              //プラグインの終了処理
        public void      SetFocus()       { }              //フォーカスを設定します
        public bool      Initialize(PluginCreateParam param) => true; //初期処理を行います
        public void      InitializePlaced() { }            //プラグイン配置後の初期化処理を行います

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

            //プラグインの型が未指定の場合は、拡張子に対応するプラグインの型を取得します
            var pluginType = GetPluginType(path);
            if (pluginType == null) {
                //プラグインの型が取得できない場合は、別のアプリで開きます
                Process.Start(path);
                return;
            }

            //プラグイン生成パラメーターを設定します
            var pluginCreateParam = new PluginCreateParam();
            pluginCreateParam["path"] = path; //選択されたパス

            //プラグインを生成します
            var plugin = (IFilePlugin)_pluginManager.CreatePluginInstance(pluginType, pluginCreateParam);
            if (plugin == null) return;

        }

    } //class
}
