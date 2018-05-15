using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo
{
    public class AppConfig
    {
        const string PATH = "config.xml";

        public void Save() {
            SerializeUtils.SerializeToFile(this, PATH);
        }

        /// <summary>
        /// Configを読み込みます
        /// </summary>
        /// <returns></returns>
        public static AppConfig Load() {
            AppConfig config = null;
            if (File.Exists(PATH)) {
                //ファイルがある場合
                config = (AppConfig)SerializeUtils.DeserializeFromFile(typeof(AppConfig), PATH);
            }
            return config;
        }

        //フォームの位置とサイズ
        public int form_left   = 0;
        public int form_top    = 0;
        public int form_width  = 0;
        public int form_height = 0;

    } //class
}
