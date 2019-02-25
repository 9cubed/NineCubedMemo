using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.Theme
{
    public class DarkThemePlugin : IPlugin
    {
        /// <summary>
        /// プロパティファイル
        /// </summary>
        PluginProperty _property;

        /// <summary>
        /// 背景色
        /// </summary>
        Color _backColor;

        /// <summary>
        /// 前景色(文字の色)
        /// </summary>
        Color _foreColor;

        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //プロパティファイルを読み込みます
            _property = new PluginProperty();
            _property.Load(param.PropertyPath);

            //プロパティから色を取得して保持します
            _foreColor = Color.FromArgb((int)((long)0xff000000 + _property.ToInt("color", "fore_color", 0xffffff)));
            _backColor = Color.FromArgb((int)((long)0xff000000 + _property.ToInt("color", "back_color", 0x000000)));

            //カラーデータを生成して、共通データとして設定します
            var colorData = new ColorData { ForeColor = Color.White, BackColor = Color.Black };
            _pluginManager.CommonData[CommonDataKeys.ColorData] = colorData;

            //イベントハンドラーを登録します
            _pluginManager.GetEventManager().AddEventHandler(   PluginCreatedEventParam.Name, this);
            _pluginManager.GetEventManager().AddEventHandler(AllPluginCreatedEventParam.Name, this);
            return true;
        }

        public void InitializePlaced() { } //プラグイン配置後の初期化処理を行います
        private PluginManager _pluginManager = null;        //プラグインマネージャー
        public string     PluginId         { get; set; }    //プラグインID
        public IPlugin    ParentPlugin     { get; set; }    //親プラグイン
        public IComponent GetComponent()   { return null; } //プラグインのコンポーネントを返します
        public string     Title            { get; set; }    //プラグインのタイトル
        public bool       CanClosePlugin() { return true; } //プラグインが終了できるかどうか
        public void       ClosePlugin()    { }              //プラグインの終了処理
        public void SetFocus() {} //フォーカスを設定します

        /******************************************************************************
         * 
         *  プラグイン用イベントハンドラー
         * 
         ******************************************************************************/ 

        /// <summary>
        /// プラグイン生成イベント
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sender"></param>
        public void PluginEvent_PluginCreated(EventParam param, object sender)
        {
            //生成されたプラグインを取得します
            var plugin = ((PluginCreatedEventParam)param).Plugin;

            //生成されたプラグインのコントロールをタブに設定します
            var component = plugin.GetComponent();

            //コントロールにテーマを適用します
            ApplyTheme(component);
        }

        /// <summary>
        /// プラグイン生成イベント
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sender"></param>
        public void PluginEvent_AllPluginCreated(EventParam param, object sender)
        {
            //コントロールにテーマを適用します
            ApplyTheme(_pluginManager.MainForm);
        }

        //コントロールにテーマを適用します
        private void ApplyTheme(IComponent component)
        {
            {
                if (component is Form control) {
                    control.ForeColor = _foreColor;
                    control.BackColor = _backColor;
                }
            }
            {
                if (component is Panel control) {
                    control.ForeColor = _foreColor;
                    control.BackColor = _backColor;
                }
            }
            {
                if (component is SplitContainer control) {
                    control       .BackColor = _backColor;
                    control.Panel1.BackColor = _backColor;
                    control.Panel2.BackColor = _backColor;
                }
            }
            {
                if (component is TreeView control) {
                    control.ForeColor = _foreColor;
                    control.BackColor = _backColor;
                }
            }
            {
                if (component is DataGridView control) {
                    control.ForeColor       = _foreColor;
                    control.BackColor       = _backColor;
                    control.BackgroundColor = _backColor;

                    foreach (DataGridViewColumn column in control.Columns) {
                        column.DefaultCellStyle.ForeColor = _foreColor;
                        column.DefaultCellStyle.BackColor = _backColor;
                    }
                }
            }
            {
                if (component is TabControl control) {
                    //TabControl では BackColor が効かない。
                    //Tab側で、タブがない場合は、Tab自体を非表示にするようにした。
                    //control.ForeColor = Color.White;
                    //control.BackColor = Color.Black;
                }
            }
            {
                if (component is RichTextBox control) {
                    control.ForeColor = _foreColor;
                    control.BackColor = _backColor;
                    control.Modified = false;
                }
            }
            {
                if (component is ListBox control) {
                    control.ForeColor = _foreColor;
                    control.BackColor = _backColor;
                }
            }
            {
                if (component is Button control) {
                    control.ForeColor = _backColor;
                }
            }
            {
                if (component is ToolStripStatusLabel control) {
                    control.ForeColor = _backColor;
                }
            }

            {
                //子コントロールにテーマを適用します
                if (component is Control control) {
                    foreach(var childControl in control.Controls) {
                        ApplyTheme((IComponent)childControl);
                    }
                }
            }
        }

    } //class
}
