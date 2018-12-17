using NineCubed.Memo.Exceptions;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.Dialogs.SaveDialog
{
    public class SaveDialogPlugin : IPlugin, ISaveDialog
    {
        SaveFileDialog _dialog = null;

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
        /// 名前を付けて保存ダイアログを表示します
        /// </summary>
        /// <returns>選択されたパス</returns>
        public string ShowDialog()
        {
            if (_dialog == null) {
                _dialog = new SaveFileDialog();
                _dialog.Filter = "すべてのファイル(*.*)|*.*";
            }

            //保存ダイアログを表示します
            var result = _dialog.ShowDialog();
            if (result == DialogResult.Cancel) return null; //throw new CancelException();
            if (result == DialogResult.No)     return null;

            //選択されたファイルのパスを返します
            return _dialog.FileName;
        }


    } //class
}

