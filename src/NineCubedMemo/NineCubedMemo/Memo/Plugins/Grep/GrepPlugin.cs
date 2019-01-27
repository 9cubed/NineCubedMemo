using NineCubed.Common.Files;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Grep
{
    public class GrepPlugin : IPlugin
    {
        /// <summary>
        /// Grepコントロール
        /// </summary>
        GrepControl _control;

        /// <summary>
        /// プロパティファイル
        /// </summary>
        PluginProperty _property;

        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //プロパティファイルを読み込みます
            _property = new PluginProperty();
            _property.Load(param.PropertyPath);

            bool notIgnoreCase = _property.ToBool  ("input", "not_ignore_case");
            bool includeSubDir = _property.ToBool  ("input", "include_sub_dir");
            bool useRegExp     = _property.ToBool  ("input", "use_reg_exp");
            var  extension     = _property.ToString("input", "extension");
            var  fontName      = _property.ToString("font",  "name");
            var  fontSize      = _property.ToDouble("font",  "size");

            //プラグイン生成時のパスを取得します
            var path = param.ToString("path");

            //Grepコントロールを生成します
            _control = new GrepControl(this, _pluginManager, path, notIgnoreCase, includeSubDir, useRegExp, extension, fontName, (float)fontSize);
            return true;
        }

        public void InitializePlaced() { } //プラグイン配置後の初期化処理を行います
        private PluginManager _pluginManager = null;           //プラグインマネージャー
        public string     PluginId         { get; set; }        //プラグインID
        public IPlugin    ParentPlugin     { get; set; }        //親プラグイン
        public IComponent GetComponent()   { return _control; } //プラグインのコンポーネントを返します
        public string     Title            { get{return "Grep";} set{} } //プラグインのタイトル
        public void       SetFocus()       { _control.Focus();} //フォーカスを設定します
        public bool       CanClosePlugin() { return true; }     //プラグインが終了できるかどうか

        /// <summary>
        /// プラグインの終了処理
        /// </summary>
        public void ClosePlugin() {

            //入力値を保存します
            _property["input", "not_ignore_case"] = _control.NotIgnoreCase.ToString();
            _property["input", "include_sub_dir"] = _control.IncludeSubDir.ToString();
            _property["input", "use_reg_exp"]     = _control.UseRegExp    .ToString();
            _property["input", "extension"]       = _control.Extension;

            //defineのプロパティに保存します
            PluginProperty.SaveToDefine(this, _property);

            //コントロールを削除します
            _control.Parent = null;
            _control.Dispose();
        }

    } //class
}

