using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Common.Controls
{
    /// <summary>
    /// ３ペイン用スプリットコンテナー
    /// 
    /// 使用例
    /// threePaneContainer1.SetControl(textBox1, textBox2, textBox3);
    /// threePaneContainer1.PanelLeftVisible = true;
    /// threePaneContainer1.PanelTopVisible  = false;
    /// threePaneContainer1.SetVisible();
    /// </summary>
    public partial class ThreePaneContainer : UserControl
    {
        //スプリットバーの位置
        public int HorizontalDistance {
            get => splitHorizontal.SplitterDistance;
            set {
                splitHorizontal.SplitterDistance = value;
            }
        }
        public int VerticalDistance {
            get => splitVertical.SplitterDistance;
            set {
                splitVertical.SplitterDistance = value;
            }
        }

        /// <summary>
        /// パネルの Visible
        /// </summary>
        private bool _panelLeftVisible;
        public bool PanelLeftVisible {
            get => _panelLeftVisible;
            set {
                _panelLeftVisible = value;
                SetVisible();
            }
        }
        private bool _panelTopVisible;
        public bool PanelTopVisible {
            get => _panelTopVisible;
            set {
                _panelTopVisible = value;
                SetVisible();
            }
        }

        /// <summary>
        /// パネル
        /// </summary>
        public SplitterPanel PanelLeft { get => splitVertical  .Panel1; }
        public SplitterPanel PanelTop  { get => splitHorizontal.Panel1; }
        public SplitterPanel PanelMain { get => splitHorizontal.Panel2; }

        //メインパネルに割り当てられたコントロール
        private Control MainControl { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ThreePaneContainer()
        {
            InitializeComponent();
            
            //上下分割のスプリッターを左右分割スプリッターの右側に配置します
            splitHorizontal.Parent = splitVertical.Panel2;

            //スプリットバーの幅を設定します
            splitHorizontal.SplitterWidth = 6;
            splitVertical  .SplitterWidth = 6;

            //スプリットバーの位置を設定します
            splitHorizontal.SplitterDistance = 150;
            splitVertical  .SplitterDistance = 150;

            //スプリッターを全面に表示します
            splitHorizontal.Dock = DockStyle.Fill;
            splitVertical  .Dock = DockStyle.Fill;
        }

        /// <summary>
        /// 各ペインにコントロールを割り当てます
        /// </summary>
        /// <param name="ctlLeft"></param>
        /// <param name="ctlTop"></param>
        /// <param name="ctlMain"></param>
        public void SetControl(Control ctlLeft, Control ctlTop, Control ctlMain)
        {
            if (ctlLeft == null) ctlLeft = new Panel { Dock = DockStyle.Fill };
            if (ctlTop  == null) ctlTop  = new Panel { Dock = DockStyle.Fill };
            if (ctlMain == null) ctlMain = new Panel { Dock = DockStyle.Fill };

            ctlLeft.Parent = splitVertical  .Panel1;
            ctlTop .Parent = splitHorizontal.Panel1;
            ctlMain.Parent = splitHorizontal.Panel2;

            //メインコントロールを保持します
            MainControl = ctlMain;
        }

        /// <summary>
        /// パネルの Visible を設定します
        /// メインパネルは常に表示するため、変更不可。
        /// </summary>
        /// <param name="visible1">左側のパネル</param>
        /// <param name="visible2">上側のパネル</param>
        private void SetVisible()
        {
            if (_panelTopVisible  == true && 
                _panelLeftVisible == true) {
                //３ペイン表示

                //コントロールの配置変更
                splitVertical  .Parent = this;
                splitHorizontal.Parent = splitVertical  .Panel2;
                if (MainControl != null) MainControl.Parent = splitHorizontal.Panel2;

                //Visibleの設定
                splitVertical  .Visible = true; //左右分割
                splitHorizontal.Visible = true; //上下分割
            }

            if (_panelTopVisible  == false &&
                _panelLeftVisible == true  ) {
                //２ペイン表示(左・中央)

                //コントロールの配置変更
                splitVertical  .Parent = this;
                splitHorizontal.Parent = null;
                if (MainControl != null) MainControl.Parent = splitVertical.Panel2;

                //Visibleの設定
                splitVertical  .Visible = true;  //左右分割
                splitHorizontal.Visible = false; //上下分割
            }

            if (_panelTopVisible  == true  && 
                _panelLeftVisible == false ) {
                //２ペイン表示(上・中央)

                //コントロールの配置変更
                splitVertical  .Parent = null;
                splitHorizontal.Parent = this;
                if (MainControl != null) MainControl.Parent = splitHorizontal.Panel2;

                //Visibleの設定
                splitVertical  .Visible = false; //左右分割
                splitHorizontal.Visible = true;  //上下分割
            }

            if (_panelTopVisible  == false &&
                _panelLeftVisible == false ) {
                //１ペイン表示(中央)

                //コントロールの配置変更
                splitVertical  .Parent = null;
                splitHorizontal.Parent = null;
                if (MainControl != null) MainControl.Parent = this;

                //Visibleの設定
                splitHorizontal.Visible = false; //上下分割
                splitVertical  .Visible = false; //左右分割
            }
        }

    } //class
}
