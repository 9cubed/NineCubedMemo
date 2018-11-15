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
        /// プラグインID
        /// </summary>
        string PluginId { get; set; }

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

        /// <summary>
        /// フォーカスを設定します
        /// </summary>
        void SetFocus();

        /// <summary>
        /// 初期処理を行います
        /// 初期化に失敗した場合などは false を返すとプラグインが破棄されます
        /// この段階ではまだ他のプラグインに配置されていないため、コンポーネントのサイズなどは取得できません
        /// </summary>
        /// <returns>false:初期化失敗。プラグインが破棄されます。</returns>
        bool Initialize(PluginCreateParam param);

        /// <summary>
        /// プラグイン配置後の初期化処理を行います
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        void InitializePlaced();


    }
}
