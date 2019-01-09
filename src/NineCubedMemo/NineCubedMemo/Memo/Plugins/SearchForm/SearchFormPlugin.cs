using NineCubed.Common.Files;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.SearchForm
{
    public class SearchFormPlugin : IPlugin, ISearchForm
    {
        /// <summary>
        /// プロパティファイル
        /// </summary>
        private IniFile _property = new IniFile();

        /// <summary>
        /// 検索画面
        /// </summary>
        private SearchInputForm _searchForm = null;

        /******************************************************************************
         * 
         *  IPlugin
         * 
         ******************************************************************************/

        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //共通データに検索データを生成して設定します
            _pluginManager.CommonData[CommonDataKeys.SearchData] = new SearchData();

            //プロパティファイルを読み込みます
            _property.Load(param.PropertyPath);

            return true;
        }

        //プラグイン配置後の初期化処理を行います
        public void InitializePlaced() {}

        private PluginManager _pluginManager = null;                    //プラグインマネージャー
        public string     PluginId         { get; set; }                 //プラグインID
        public IPlugin    ParentPlugin     { get; set; }                 //親プラグイン
        public IComponent GetComponent()   { return null; }              //プラグインのコンポーネントを返します
        public string     Title            { get; set; }                 //プラグインのタイトル
        public bool       CanClosePlugin() { return true; }              //プラグインが終了できるかどうか
        public void       SetFocus()       {  }                          //フォーカスを設定します

        /// <summary>
        /// プラグインの終了処理
        /// </summary>
        public void ClosePlugin()
        {
            //フォームを閉じます
            if (_searchForm != null && _searchForm.IsDisposed == false) {
                _searchForm.Dispose();
            }

            //プロパティファイルを保存します
            _property.Save();
        }

        /******************************************************************************
         * 
         *  ISearchForm
         * 
         ******************************************************************************/
        
        /// <summary>
        /// 検索画面を表示します
        /// </summary>
        public void ShowForm()
        {
            //フォームを生成します
            if (_searchForm == null || _searchForm.IsDisposed) {
                _searchForm = new SearchInputForm();
            }

            //フォームを表示します
            _searchForm.ShowForm(_pluginManager, _property);

            //フォームの位置を設定します
            _searchForm.Left = StringUtils.ToInt(_property["location", "left"]  , 0);
            _searchForm.Top  = StringUtils.ToInt(_property["location", "top"]   , 0);
        }

        /******************************************************************************
         * 
         *  その他
         * 
         ******************************************************************************/ 

    } //class
}
