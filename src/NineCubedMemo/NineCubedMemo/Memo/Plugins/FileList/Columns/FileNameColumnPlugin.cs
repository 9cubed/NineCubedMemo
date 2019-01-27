using NineCubed.Common.Controls.FileList.Columns;
using NineCubed.Common.Files;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.FileList.Columns
{
    public class FileNameColumnPlugin : FileNameColumn, IPlugin
    {
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

            //列の幅を設定します
            this.Width = _property.ToInt(IniFile.NO_SECTION, "width", 140);

            return true;
        }

        //プラグイン配置後の初期化処理を行います
        public void InitializePlaced() {}

        private PluginManager _pluginManager = null;        //プラグインマネージャー
        public string     PluginId         { get; set; }    //プラグインID
        public IPlugin    ParentPlugin     { get; set; }    //親プラグイン
        public IComponent GetComponent()   { return this; } //プラグインのコンポーネントを返します
        public string     Title            { get { return this.HeaderText;} set{ } } //プラグインのタイトル
        public bool       CanClosePlugin() { return true; } //プラグインが終了できるかどうか
        public void       SetFocus()       {  }             //フォーカスを設定します

        //プラグインの終了処理
        public void ClosePlugin()
        {
            //プロパティファイルを読み込みます
            _property[IniFile.NO_SECTION, "width"] = this.Width.ToString();
            _property.Save();

            Dispose();
        }

        /// <summary>
        /// セルの値が変更された時の処理
        /// </summary>
        /// <param name="orgFile"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        override
        public FileInfo ValueChanged(FileInfo orgFile, string newValue)
        {
            var newFile = base.ValueChanged(orgFile, newValue);
            if (newFile == null) return null;

            //ファイル名変更イベントを発生させます
            var param = new FileNameChangedEventParam { OldPath = orgFile.FullName, NewPath = newFile.FullName };
            _pluginManager.GetEventManager().RaiseEvent(FileNameChangedEventParam.Name, this, param);

            return newFile;
        }

    } //class
}
