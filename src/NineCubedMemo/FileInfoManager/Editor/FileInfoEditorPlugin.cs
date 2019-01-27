using FileInfoManager.Events;
using NineCubed.Common.Files;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileInfoManager.Editor
{
    public class FileInfoEditorPlugin : IPlugin, IFilePlugin
    {
        //ファイル情報エディター
        FileInfoEditorControl _editor;

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

            //DBのパスを作成します
            var dbDirPath = FileUtils.AppendPath(PluginManager.GetCommonDataPath(), "file_info/");
            FileUtils.CreateDir(dbDirPath);
            var dbPath = FileUtils.AppendPath(dbDirPath, "file_info.db");

            //ファイル情報エディターを生成します
            _editor = new FileInfoEditorControl(this, dbPath, (string)param["id"]);

            //イベントハンドラーを登録します
            _pluginManager.GetEventManager().AddEventHandler(FileInfoSelectingEventParam.Name, this);
            
            return true;
        }

        public void InitializePlaced() { } //プラグイン配置後の初期化処理を行います
        private PluginManager _pluginManager = null;           //プラグインマネージャー
        public string     PluginId         { get; set; }       //プラグインID
        public IPlugin    ParentPlugin     { get; set; }       //親プラグイン
        public IComponent GetComponent()   { return _editor; } //プラグインのコンポーネントを返します
        public string     Title            { get { return "ファイル情報"; } set{ } } //プラグインのタイトル
        public void       SetFocus()       { _editor.GetTextEditor().Focus(); } //フォーカスを設定します
        public bool       CanClosePlugin() { return true; }    //プラグインが終了できるかどうか
        public void       ClosePlugin()    {                   //プラグインの終了処理
            _editor.Parent = null;
            _editor.Dispose();
        }

        /******************************************************************************
         * 
         *  IFilePlugin
         * 
         ******************************************************************************/

        /// <summary>
        /// テキストファイルデータ
        /// </summary>
        public IFile TargetFile { get; set; }

        /// <summary>
        /// ファイルを開きます
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool LoadFile(IFile file)
        {
            return true;
        }

        /// <summary>
        /// ファイルを保存します。
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool SaveFile(IFile file = null)
        {
            _editor.SaveData();
            return true;
        }

        /******************************************************************************
         * 
         *  プラグイン用イベントハンドラー
         * 
         ******************************************************************************/

        /// <summary>
        /// ファイル情報選択中イベント
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sender"></param>
        public void PluginEvent_FileInfoSelecting(EventParam param, object sender)
        {
            //データが変更されている場合は、確認メッセージを表示します
            if (_editor.Modified) {
                var dialogResult = MsgBoxUtils.ShowMsgBoxToConfirmSaving();
                if (dialogResult == DialogResult.Yes) {
                    //「はい」の場合、保存します
                    _editor.SaveData();
                }
                if (dialogResult == DialogResult.No) {
                    //「いいえ」の場合、何もせずに処理を続けます
                }
                if (dialogResult == DialogResult.Cancel) {
                    //「キャンセル」の場合、処理を抜けます
                    return;
                }
            }

            //ファイル情報を表示します
            var id = ((FileInfoSelectingEventParam)param).ID;
            _editor.LoadData(id);
        }
        
    } //class
}
