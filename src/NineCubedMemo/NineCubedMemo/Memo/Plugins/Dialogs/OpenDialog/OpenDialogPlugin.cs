using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.Dialogs.OpenDialog
{
    public class OpenDialogPlugin : IPlugin, IOpenDialog
    {
        OpenFileDialog _dialog = null;

        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            return true;
        }
        
        private PluginManager _pluginManager = null;            //プラグインマネージャー
        public void      InitializePlaced() {}                  //プラグイン配置後の初期化処理を行います
        public string    PluginId           { get; set; }       //プラグインID
        public Component GetComponent()     { return _dialog; } //プラグインのコンポーネントを返します
        public string    Title              { get; set; }       //プラグインのタイトル
        public bool      CanClosePlugin()   { return true; }    //プラグインが終了できるかどうか
        public void      ClosePlugin()      {  }                //プラグインの終了処理
        public void      SetFocus()         {  }                //フォーカスを設定します

        /// <summary>
        /// 開くダイアログを表示します
        /// </summary>
        /// <returns>選択されたパス</returns>
        public string ShowDialog()
        {
            if (_dialog == null) {
                _dialog = new OpenFileDialog{
                    Filter = "すべてのファイル(*.*)|*.*"
                };
            } 

            _dialog.FileName = "";
            var dialogResult = _dialog.ShowDialog();
            if (dialogResult == DialogResult.OK) {
                //選択されたパスを取得します
                return _dialog.FileName;
            } else {
                return null;
            }
        }

        /// <summary>
        /// 選択されたファイル名
        /// </summary>
        public string FileName { get; set; }

    } //class
}

