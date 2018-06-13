using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo
{
    public class AppConfig
    {
        /// <summary>
        /// Config ファイルのパス
        /// </summary>
        const string PATH = "config.xml";

        /// <summary>
        /// メインフォーム
        /// </summary>
        MainForm _mainForm = null;

        public void Save() {
            //ウィンドウが最小化、最大化されている場合は、標準に戻します
            //(Configに保存する際のサイズがわからないため)
            if (_mainForm.WindowState != FormWindowState.Normal) {
                _mainForm.WindowState = FormWindowState.Normal;
            }

            //Configに現在の状態を設定します
            this.form_left   = _mainForm.Left;
            this.form_top    = _mainForm.Top;
            this.form_width  = _mainForm.Width;
            this.form_height = _mainForm.Height;

            //Configを保存します
            SerializeUtils.SerializeToFile(this, PATH);
        }

        /// <summary>
        /// Configを読み込みます
        /// </summary>
        /// <returns></returns>
        public static AppConfig Load(MainForm mainForm) {
            AppConfig config = null;

            if (File.Exists(PATH)) {
                //Configファイルがある場合、Configファイルを読み込みます
                config = (AppConfig)SerializeUtils.DeserializeFromFile(typeof(AppConfig), PATH);

                //フォームの位置とサイズを設定します
                mainForm.StartPosition = FormStartPosition.Manual;
                mainForm.Left   = config.form_left;
                mainForm.Top    = config.form_top;
                mainForm.Width  = config.form_width;
                mainForm.Height = config.form_height;

            } else {
                //Configファイルがない場合

                //デフォルト値を設定します
                config = new AppConfig();

                //フォームを中央に表示します
                var screenBounds = Screen.PrimaryScreen.Bounds;
                mainForm.Width  = (int)(screenBounds.Width  * 0.6);
                mainForm.Height = (int)(screenBounds.Height * 0.6);
                mainForm.StartPosition = FormStartPosition.CenterScreen;
            }

            //フォームを保持します
            config._mainForm = mainForm;

            return config;
        }

        //フォームの位置とサイズ
        public int form_left   = 0;
        public int form_top    = 0;
        public int form_width  = 0;
        public int form_height = 0;

        //フォント
        public string memo_font_name = "ＭＳ ゴシック";
        public float  memo_font_size = 12;

    } //class
}
