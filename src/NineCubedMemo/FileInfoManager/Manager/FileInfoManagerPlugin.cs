using FileInfoManager.DB;
using NineCubed.Common.Files;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileInfoManager.Manager
{
    public class FileInfoManagerPlugin : IPlugin, IRefreshPlugin
    {
        /// <summary>
        /// メインコントロール
        /// </summary>
        FileInfoManagerControl _control;

        /// <summary>
        /// DB
        /// </summary>
        FileDB _db;

        /// <summary>
        /// プロパティファイル
        /// </summary>
        public IniFile Property;

        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //DBのパスを作成します
            var dbDirPath = FileUtils.AppendPath(PluginManager.GetCommonDataPath(), "file_info/");
            FileUtils.CreateDir(dbDirPath);
            var dbPath = FileUtils.AppendPath(dbDirPath, "file_info.db");

            //プロパティファイルを読み込みます
            Property = new IniFile();
            Property.Load(param.PropertyPath);

            //DB接続用オブジェクトを生成します
            _db = new FileDB(dbPath);

            //DBのバックアップを作成します
            File.Copy(dbPath, dbPath + "_old", true);

            //ビューのコントロールを生成します
            _control = new FileInfoManagerControl(this, dbPath);

            //イベントハンドラーを登録します
            _pluginManager.GetEventManager().AddEventHandler(FileNameChangedEventParam.Name, this);

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
            _control.ShowFileList();
        }

        /******************************************************************************
         * 
         *  プラグイン用イベントハンドラー
         * 
         ******************************************************************************/ 

        /// <summary>
        /// ファイル名変更イベント
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sender"></param>
        public void PluginEvent_FileNameChanged(EventParam param, object sender) {
            //変更前と変更後のパスを取得します
            var oldPath = ((FileNameChangedEventParam)param).OldPath;
            var newPath = ((FileNameChangedEventParam)param).NewPath;

            using(var connection = _db.GetConnection()) {
                var fileData = FileDataDao.GetData(connection, oldPath, "path");
                if (fileData != null) {
                    FileDataDao.UpdateField(connection, fileData.id, "path", newPath);
                }
            }
        }

    } //class

}
