using NineCubed;
using NineCubed.Memo;
using NineCubed.Memo.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubedMemo
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainFormPlugin());
            
            //簡単なプログラムの動作確認用フォームの表示
            #if DEBUG
            bool isTest = false;
            if (isTest) {
                Application.Run(new frmTest());
                return;
            }
            #endif

            var pluginManager = PluginManager.GetInstance();
                pluginManager.Startup();

            if (pluginManager.MainForm != null) {
                Application.Run(pluginManager.MainForm);
            }
        }
    }
}
