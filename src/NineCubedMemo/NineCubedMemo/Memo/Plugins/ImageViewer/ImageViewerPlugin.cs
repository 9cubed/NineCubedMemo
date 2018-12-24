using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//NineCubed.Memo.Plugins.ImageViewer.ImageViewerPlugin
namespace NineCubed.Memo.Plugins.ImageViewer
{
    public class ImageViewerPlugin : IPlugin
    {
        ImageViewerControl _ImageViewer = null;

        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //画像ビューアを生成します
            _ImageViewer = new ImageViewerControl(this, _pluginManager);
            _ImageViewer.Dock = DockStyle.Fill;

            //画像を読み込みます
            _ImageViewer.LoadImage(param.Path);

            //ファイル名をタイトルとして保持します（タブのタイトルになります）
            this.Title = Path.GetFileName(param.Path);

            //イベントハンドラーを登録します
            _pluginManager.GetEventManager().AddEventHandler(FileSelectingEventParam.Name, this);

            return true;
        }
        
        //プラグイン配置後の初期化処理を行います
        public void InitializePlaced() {
            //画像を枠に合わせます
            _ImageViewer.IsFit = true;
        }

        private PluginManager _pluginManager = null;                    //プラグインマネージャー
        public string    PluginId         { get; set; }                 //プラグインID
        public Component GetComponent()   { return _ImageViewer; }      //プラグインのコンポーネントを返します
        public string    Title            { get; set; }                 //プラグインのタイトル
        public bool      CanClosePlugin() { return true; }              //プラグインが終了できるかどうか
        public void      ClosePlugin()    { _ImageViewer.Parent = null; //プラグインの終了処理
                                            _ImageViewer.Dispose(); }   

        //フォーカスを設定します
        public void SetFocus() {
            _pluginManager.ActivePlugin = this;
            _ImageViewer.Focus();
        } 
        
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
        public void PluginEvent_FileSelecting(EventParam param, object sender) {

            //画像をロックしている場合は処理しない
            if (_ImageViewer.IsLockImage) return;

            //選択中のパスを取得します
            var path = ((FileSelectingEventParam)param).Path;

            //画像を読み込みます
            _ImageViewer.LoadImage(path);

            //ファイル名をタイトルとして保持します（タブのタイトルになります）
            this.Title = Path.GetFileName(path);

            //タイトル変更イベントを発生させます
            var p = new TitleChangedEventParam{ Plugin = this };
            _pluginManager.GetEventManager().RaiseEvent(TitleChangedEventParam.Name, this, p);
        }


    } //class
}
