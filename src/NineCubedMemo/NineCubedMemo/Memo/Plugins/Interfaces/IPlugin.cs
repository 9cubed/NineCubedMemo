using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.Interfaces
{
    public interface IPlugin
    {
        /// <summary>
        /// プラグインのコンポーネントを返します
        /// </summary>
        /// <returns></returns>
        Component GetComponent();

        /// <summary>
        /// プラグインのタイトル
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// プラグインが終了できるかどうか
        /// </summary>
        /// <returns>true:終了できる  false:終了できない</returns>
        bool CanClosePlugin();

        /// <summary>
        /// プラグインの終了処理
        /// </summary>
        void ClosePlugin();
    }
}
