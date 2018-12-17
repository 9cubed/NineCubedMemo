using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Dialogs.OpenDialog;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.Tools.FileTool
{
    public class OpenFileToolPlugin : ToolStripButton, IPlugin
    {

        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //クリックイベントを設定します
            this.Click += ToolClick;

            //ツールの画像を設定します
            this.Image = Image.FromFile(FileUtils.AppendPath(param.DataPath, "img/opened_folder.png"));

            return true;
        }
        
        private PluginManager _pluginManager = null;                      //プラグインマネージャー
        public void      InitializePlaced() {}                            //プラグイン配置後の初期化処理を行います
        public string    PluginId           { get; set; }                 //プラグインID
        public Component GetComponent()     { return this; }              //プラグインのコンポーネントを返します
        public string    Title              { get; set; }                 //プラグインのタイトル
        public bool      CanClosePlugin()   { return true; }              //プラグインが終了できるかどうか
        public void      ClosePlugin()      { Parent = null; Dispose(); } //プラグインの終了処理
        public void      SetFocus()         {  }                          //フォーカスを設定します


        /// <summary>
        /// ツールボタンクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolClick(object sender, EventArgs e)
        {
            try {
                //ファイルをプラグインで開きます
                OpenFile();
            } catch (Exception ex) {
                __.ShowErrorMsgBox(ex);
            }
        }

        /// <summary>
        /// ファイルをプラグインで開きます
        /// </summary>
        private void OpenFile()
        {
            //開くダイアログのプラグインを取得します
            var openDialogPlugin = (IOpenDialog)_pluginManager.GetPlugin("open_dialog");

            //ファイルを開くダイアログを表示します
            var path = openDialogPlugin.ShowDialog();
            if (path == null) return;

            //ファイルが存在しない場合は処理しない
            if (File.Exists(path) == false) return;

            //ファイル選択イベントを発生させます
            var param = new FileSelectedEventParam { Path = path };
            _pluginManager.GetEventManager().RaiseEvent(FileSelectedEventParam.Name,  null, param);
        }


    } //class
}
