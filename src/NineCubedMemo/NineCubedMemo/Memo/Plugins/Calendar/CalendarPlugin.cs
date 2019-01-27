using NineCubed.Common.Files;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Calendar
{
    public class CalendarPlugin : IPlugin, IRefreshPlugin
    {
        /// <summary>
        /// カレンダーコントロール
        /// </summary>
        CalendarControl _control;

        /// <summary>
        /// プロパティファイル
        /// </summary>
        IniFile _property;

        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //プロパティファイルを読み込みます
            _property = new IniFile();
            _property.Load(param.PropertyPath);

            //日記用のデータフォルダのパスを取得します
            var dataDir = _property.ToString(IniFile.NO_SECTION, "data_dir", "diary");

            //カレンダーコントロールを生成します
            _control = new CalendarControl(this, _pluginManager, dataDir);

            return true;
        }

        public void InitializePlaced() { } //プラグイン配置後の初期化処理を行います
        private PluginManager _pluginManager = null;           //プラグインマネージャー
        public string     PluginId         { get; set; }        //プラグインID
        public IPlugin    ParentPlugin     { get; set; }        //親プラグイン
        public IComponent GetComponent()   { return _control; } //プラグインのコンポーネントを返します
        public string     Title            { get{return "カレンダー";} set{} } //プラグインのタイトル
        public void       SetFocus()       { _control.Focus();} //フォーカスを設定します
        public bool       CanClosePlugin() { return true; }     //プラグインが終了できるかどうか
        public void       ClosePlugin()    {                    //プラグインの終了処理
            //_textbox.Parent = null;
            //_textbox.Dispose();
        }

        /// <summary>
        /// 最新の情報に更新します
        /// </summary>
        public void RefreshData()
        {
            //カレンダーを表示します
            _control.ShowCalendar();
        }

    } //class
}
