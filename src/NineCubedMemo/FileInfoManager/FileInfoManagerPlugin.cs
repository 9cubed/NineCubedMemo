﻿using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileInfoManager
{
    public class FileInfoManagerPlugin : IPlugin, IRefreshPlugin
    {

        FileInfoManagerControl _control;

        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //DBのパスを作成します
            var dbDirPath = FileUtils.AppendPath(_pluginManager.GetCommonDataPath(), "file_info/");
            FileUtils.CreateDir(dbDirPath);
            var dbPath = FileUtils.AppendPath(dbDirPath, "file_info.db");

            _control = new FileInfoManagerControl(this, dbPath);

            return true;
        }

        public void InitializePlaced() { } //プラグイン配置後の初期化処理を行います
        private PluginManager _pluginManager = null;           //プラグインマネージャー
        public string     PluginId         { get; set; }        //プラグインID
        public IPlugin    ParentPlugin     { get; set; }        //親プラグイン
        public IComponent GetComponent()   { return _control; } //プラグインのコンポーネントを返します
        public string     Title            { get{return "ファイル管理";} set{} } //プラグインのタイトル
        public void       SetFocus()       { }                  //フォーカスを設定します
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
            //検索します
            _control.Search();
        }


    } //class

}
