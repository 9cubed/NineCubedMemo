using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using NineCubed.Memo.Plugins.Interfaces;
using NineCubed.Common.Utils;

namespace NineCubed.Memo.Plugins.ImageViewer
{
    public partial class ImageViewerControl : UserControl
    {
        /// <summary>
        /// パス
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// 画像を枠に合わせるかどうか？ true:合わせる
        /// </summary>
        public bool IsFit {
            get => this.chkFit.Checked;
            set {
                this.chkFit.Checked = value;
            }
        }

        /// <summary>
        /// 画像を固定するかどうか？
        /// (ファイル選択中イベントを受け取るかどうか)
        /// </summary>
        public bool IsLockImage {get => this.chkLockImage.Checked; }

        /// <summary>
        /// オリジナル画像
        /// </summary>
        private Image _orgImage;

        /// <summary>
        /// 画像の拡大縮小率
        /// </summary>
        public double Rate { get; set; } = 1;

        //プラグインマネージャー
        private PluginManager _pluginManager; 

        //プラグイン
        private IPlugin _plugin;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ImageViewerControl(IPlugin plugin, PluginManager pluginManager)
        {
            InitializeComponent();

            //プラグインとプラグインマネージャーを保持します
            _plugin        = plugin;
            _pluginManager = pluginManager;

            //マウスホイールイベントを追加します
            this.MouseWheel += new MouseEventHandler(this.pic_MouseWheel);
             pic.MouseWheel += new MouseEventHandler(this.pic_MouseWheel);

            //画質のコンボボックスを設定します
            cmbQuality.Items.Add("Low");                 //低品質
            cmbQuality.Items.Add("High");                //高品質
            cmbQuality.Items.Add("Bilinear");            //双一次補間
            cmbQuality.Items.Add("Bicubic");             //双三次補間
            cmbQuality.Items.Add("NearestNeighbor");     //ニアレスト ネイバーの補間
            cmbQuality.Items.Add("HighQualityBilinear"); //高品質で双一次補間
            cmbQuality.Items.Add("HighQualityBicubic");  //最高品質  高品質の双三次補間
            cmbQuality.SelectedIndex = 4; //NearestNeighbor をデフォルトにする

            //画像の表示領域の位置を設定します
            pic.Left = 0;
            pic.Top  = 0;

            //メインパネルを全体に広げます
            pnlMain.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// 画像を読み込みます
        /// </summary>
        /// <param name="path"></param>
        public bool LoadImage(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentException();
            if (File.Exists(path) == false) return false;

            //拡張子が画像ファイル以外の場合は処理しない
            if (ImageUtils.IsImageFile(path) == false) return false;

            //画像ファイルを読み込みます
            //var image = Image.FromFile(path); //ファイルがロックされるため FileStream に変更
            Image image;
            using(var stream = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                image = Image.FromStream(stream);
            }

            //画像を設定します
            SetImage(image);

            //パスを保持します
            this.Path = path;

            return true;
        }

        /// <summary>
        /// 画像を設定します
        /// </summary>
        /// <param name="image"></param>
        private void SetImage(Image image)
        {
            //現在設定されている画像を解放します
            if (_orgImage != null) _orgImage.Dispose();

            //新しいオリジナル画像を保持します
            _orgImage = image;

            //画像を表示します
            ShowImage();

        }

        /// <summary>
        /// 画像を表示します
        /// </summary>
        public void ShowImage()
        {
            //画像が設定されていない場合は処理しない
            if (_orgImage == null) return;

            //前回設定されていた画像を解放します
            var tmpImage = pic.Image; //現在の表示画像を退避します
            pic.Image = null;
            tmpImage?.Dispose();

            //画像を枠に合わせるかどうか？
            if (this.IsFit) {
                //画像を枠に合わせた場合の、画像の拡大縮小率を算出します
                this.Rate = GetRate(_orgImage, pic.Width, pic.Height);
            }

            //拡大縮小画像を取得して、画像の表示領域に設定します
            pic.Image = GetResizeImage(_orgImage,
                            (int)((double)_orgImage.Width  * this.Rate),
                            (int)((double)_orgImage.Height * this.Rate));

            //画像の表示領域を画像サイズに合わせます
            pic.Width  = pic.Image.Width;
            pic.Height = pic.Image.Height;

            //拡大縮小率を表示します
            ShowRate();
        }

        /// <summary>
        /// アスペクト比を維持して、指定したサイズに合う縮小率を算出します。
        /// 元画像が指定したサイズに収まる場合は、1 を返します。
        /// </summary>
        /// <param name="image">元画像</param>
        /// <param name="newWidth" >縮小後の幅</param>
        /// <param name="newHeight">縮小後の高さ</param>
        /// <returns></returns>
        public double GetRate(Image image, int newWidth, int newHeight)
        {
            int oldWidth  = image.Width;
            int oldHeight = image.Height;
            double rateWidth  = 1; //幅の縮小率
            double rateHeight = 1; //高さの縮小率

            //幅を縮小する必要がある場合は、幅の縮小率を求めます
            if (newWidth < oldWidth) rateWidth = (double)newWidth / oldWidth;

            //高さを縮小する必要がある場合は、高さの縮小率を求めます
            if (newHeight < oldHeight) rateHeight = (double)newHeight / oldHeight;

            //高さの縮小率と、幅の縮小率の、小さい方を返します
            return (rateWidth > rateHeight) ? rateHeight : rateWidth;
        }

        //
        /// <summary>
        /// 拡大縮小した画像を返します
        /// </summary>
        /// <param name="orgImage">元画像</param>
        /// <param name="width" >拡大縮小後の幅</param>
        /// <param name="height">拡大縮小後の高さ</param>
        /// <returns></returns>
        private Bitmap GetResizeImage(Image orgImage, int width, int height)
        {
            //新しい Bitmap を生成します
            if (width  < 10) width  = 10;
            if (height < 10) height = 10;
            var image = new Bitmap(width, height);

            try {
                using (var g = Graphics.FromImage(image)) {
                    //新しい Bitmap に、拡大縮小したイメージを描画します

                    //拡大縮小アルゴリズムを設定します(画質・処理速度が異なる)
                    g.InterpolationMode = GetInterpolationMode();

                    //描画します
                    g.DrawImage(orgImage, 0, 0, width, height);
                }
                return image;

            } catch (Exception) {
                return image;
            }
        }

        /// <summary>
        /// 画質のコンボボックスで選択されている InterpolationMode を返します
        /// </summary>
        /// <returns></returns>
        private InterpolationMode GetInterpolationMode()
        {
            if (Enum.TryParse(cmbQuality.SelectedItem.ToString(), out InterpolationMode mode)) {
                return mode;
            }
            return InterpolationMode.Default;
        }

        /// <summary>
        /// 拡大縮小率を表示します
        /// </summary>
        public void ShowRate()
        {
            int rate = (int)Math.Ceiling(this.Rate * 100);
            lblRate.Text = rate + "%"; //ラベル
        }

        /******************************************************************************
         * 
         *  コントロールのイベント
         * 
         ******************************************************************************/ 

        /// <summary>
        /// パネルのリサイズイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pnlMain_Resize(object sender, EventArgs e)
        {
            if (_orgImage == null) return;

            //最小化時のエラー回避
            if (pic.Width == 0 || pic.Height == 0) return;

            //枠に合わせない場合は、処理しない
            if (this.IsFit == false) return;

            //画像を表示します
            ShowImage();
        }

        /// <summary>
        /// 画像のダブルクリックイベント
        /// 「枠に合わせる」のチェックボックスのチェックを反転させます
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pic_DoubleClick(object sender, EventArgs e)
        {
            //チェックボックスのチェックを反転させます
            chkFit.Checked = !chkFit.Checked;
        }
        //パネルのダブルクリックイベント
        private void pnlMain_DoubleClick(object sender, EventArgs e){ pic_DoubleClick(sender, e); }

        /// <summary>
        /// 「枠に合わせる」チェックボックスにチェックの変更があった場合のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkFit_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFit.Checked) {
                //枠に合わせる場合

                //画像の表示エリアを枠に合わせます
                pic.Dock = DockStyle.Fill;

                //拡大縮小率のスクロールバーを使用不可にします
                scrBarRate.Enabled = false;

            } else {
                //枠に合わせない場合

                //画像の表示エリアを枠に合わせます
                pic.Dock = DockStyle.None;

                //拡大縮小率のスクロールバーを使用可にします
                scrBarRate.Enabled = true;

                //スクロールバーの値を設定します
                scrBarRate.Value = (int)Math.Ceiling(this.Rate * 100);
            }

            //画像を表示します
            ShowImage();

            //アクティブプラグインと設定します
            SetActivePlugin();
        }

        /// <summary>
        /// マウスホイールイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pic_MouseWheel(object sender, MouseEventArgs e) {
            //ホイール量の取得 e.Delta は 120単位なので 120で割ってます
            double rate = Rate + 0.01 * (e.Delta / 120);
            if (rate < 0.1) rate = 0.1;
            if (rate > 2)   rate = 2;
            Rate = rate;

            //画像を表示します
            ShowImage();
        }

        /// <summary>
        /// 横スクロールバーの値の変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scrBarRate_ValueChanged(object sender, EventArgs e)
        {
            if (IsFit) return; //枠に合わせる場合は処理しない

            //拡大縮小率を%から
            double rate = (double)scrBarRate.Value / 100;
            if (rate < 0.01) rate = 0.01;

            this.Rate = rate;

            //画像を表示します
            ShowImage();

            //アクティブプラグインと設定します
            SetActivePlugin();
        }

        /// <summary>
        /// 「画像を左上に移動」ボタンのクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTopLeft_Click(object sender, EventArgs e)
        {
            pic.Left = 0;
            pic.Top  = 0;

            //アクティブプラグインと設定します
            SetActivePlugin();
        }

        /// <summary>
        /// 画質コンボボックスの選択イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
            //画像を表示します
            ShowImage();
        }

        /******************************************************************************
         * 
         *  アクティブプラグインの設定用イベント
         * 
         ******************************************************************************/ 

        /// <summary>
        /// アクティブプラグインにします
        /// </summary>
        private void SetActivePlugin() { _pluginManager.ActivePlugin = _plugin; }

        //画像のダブルクリックイベント
        private void pic_Click(object sender, EventArgs e) { SetActivePlugin(); }

        //画像をロックチェックボックスの変更イベント
        private void chkLockImage_CheckedChanged(object sender, EventArgs e) { SetActivePlugin(); }
        
        //メインパネルがフォーカスを受けた時のイベント
        private void pnlMain_Enter(object sender, EventArgs e) { SetActivePlugin(); }

        /******************************************************************************
         * 
         *  画像の移動制御
         * 
         ******************************************************************************/ 

        /// <summary>
        /// 画像の移動中かどうかのフラグ true:移動中
        /// </summary>
        bool _isMoving = false;

        /// <summary>
        /// 移動開始時のクリックされた位置
        /// </summary>
        Point _mouseXY;

        /// <summary>
        /// 移動開始時の画像の位置
        /// </summary>
        Point _picXY;

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            //枠に合わせる場合は処理しない
            if (chkFit.Checked) return;

            //移動中の場合は処理しない
            if (_isMoving) return;

            //移動モードを開始します
            _isMoving = true;

            //移動開始時の画像の位置とクリックされた位置を保持します
            _picXY   = new Point(pic.Location.X, pic.Location.Y);
            _mouseXY = new Point(Cursor.Position.X, Cursor.Position.Y);
        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            //枠に合わせる場合は処理しない
            if (chkFit.Checked) return;

            //移動中でない場合は処理しない
            if (_isMoving == false) return;

            pic.Location = new Point(
                _picXY.X + (Cursor.Position.X - _mouseXY.X),
                _picXY.Y + (Cursor.Position.Y - _mouseXY.Y)
            );
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            //枠に合わせる場合は処理しない
            if (chkFit.Checked) return;

            //移動中でない場合は処理しない
            if (_isMoving == false) return;

            //移動モードを終了します
            _isMoving = false;
        }
        private void pnlMain_MouseDown(object sender, MouseEventArgs e){ pic_MouseDown(sender, e); }
        private void pnlMain_MouseMove(object sender, MouseEventArgs e){ pic_MouseMove(sender, e); }
        private void pnlMain_MouseUp  (object sender, MouseEventArgs e){ pic_MouseUp  (sender, e); }

    } //class
}
