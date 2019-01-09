namespace NineCubed.Memo.Plugins.ImageViewer
{
    partial class ImageViewerControl
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.pic = new System.Windows.Forms.PictureBox();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.chkLockImage = new System.Windows.Forms.CheckBox();
            this.lblRate = new System.Windows.Forms.Label();
            this.chkFit = new System.Windows.Forms.CheckBox();
            this.btnTopLeft = new System.Windows.Forms.Button();
            this.scrBarRate = new System.Windows.Forms.HScrollBar();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.cmbQuality = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pic
            // 
            this.pic.BackColor = System.Drawing.Color.Black;
            this.pic.Location = new System.Drawing.Point(53, 16);
            this.pic.Name = "pic";
            this.pic.Size = new System.Drawing.Size(225, 128);
            this.pic.TabIndex = 0;
            this.pic.TabStop = false;
            this.pic.Click += new System.EventHandler(this.pic_Click);
            this.pic.DoubleClick += new System.EventHandler(this.pic_DoubleClick);
            this.pic.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pic_MouseDown);
            this.pic.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pic_MouseMove);
            this.pic.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pic_MouseUp);
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.Silver;
            this.pnlTop.Controls.Add(this.label1);
            this.pnlTop.Controls.Add(this.cmbQuality);
            this.pnlTop.Controls.Add(this.chkLockImage);
            this.pnlTop.Controls.Add(this.lblRate);
            this.pnlTop.Controls.Add(this.chkFit);
            this.pnlTop.Controls.Add(this.btnTopLeft);
            this.pnlTop.Controls.Add(this.scrBarRate);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(813, 38);
            this.pnlTop.TabIndex = 3;
            // 
            // chkLockImage
            // 
            this.chkLockImage.AutoSize = true;
            this.chkLockImage.Location = new System.Drawing.Point(487, 10);
            this.chkLockImage.Name = "chkLockImage";
            this.chkLockImage.Size = new System.Drawing.Size(72, 16);
            this.chkLockImage.TabIndex = 4;
            this.chkLockImage.Text = "画像固定";
            this.chkLockImage.UseVisualStyleBackColor = true;
            this.chkLockImage.CheckedChanged += new System.EventHandler(this.chkLockImage_CheckedChanged);
            // 
            // lblRate
            // 
            this.lblRate.AutoSize = true;
            this.lblRate.Location = new System.Drawing.Point(276, 12);
            this.lblRate.Name = "lblRate";
            this.lblRate.Size = new System.Drawing.Size(29, 12);
            this.lblRate.TabIndex = 3;
            this.lblRate.Text = "100%";
            // 
            // chkFit
            // 
            this.chkFit.AutoSize = true;
            this.chkFit.Location = new System.Drawing.Point(8, 10);
            this.chkFit.Name = "chkFit";
            this.chkFit.Size = new System.Drawing.Size(86, 16);
            this.chkFit.TabIndex = 0;
            this.chkFit.Text = "枠に合わせる";
            this.chkFit.UseVisualStyleBackColor = true;
            this.chkFit.CheckedChanged += new System.EventHandler(this.chkFit_CheckedChanged);
            // 
            // btnTopLeft
            // 
            this.btnTopLeft.Location = new System.Drawing.Point(565, 4);
            this.btnTopLeft.Name = "btnTopLeft";
            this.btnTopLeft.Size = new System.Drawing.Size(132, 27);
            this.btnTopLeft.TabIndex = 1;
            this.btnTopLeft.Text = "画像を左上に移動";
            this.btnTopLeft.UseVisualStyleBackColor = true;
            this.btnTopLeft.Click += new System.EventHandler(this.btnTopLeft_Click);
            // 
            // scrBarRate
            // 
            this.scrBarRate.Location = new System.Drawing.Point(107, 7);
            this.scrBarRate.Maximum = 300;
            this.scrBarRate.Name = "scrBarRate";
            this.scrBarRate.Size = new System.Drawing.Size(162, 21);
            this.scrBarRate.TabIndex = 2;
            this.scrBarRate.Value = 100;
            this.scrBarRate.ValueChanged += new System.EventHandler(this.scrBarRate_ValueChanged);
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.Black;
            this.pnlMain.Controls.Add(this.pic);
            this.pnlMain.Location = new System.Drawing.Point(107, 108);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(345, 196);
            this.pnlMain.TabIndex = 4;
            this.pnlMain.DoubleClick += new System.EventHandler(this.pnlMain_DoubleClick);
            this.pnlMain.Enter += new System.EventHandler(this.pnlMain_Enter);
            this.pnlMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlMain_MouseDown);
            this.pnlMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlMain_MouseMove);
            this.pnlMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlMain_MouseUp);
            this.pnlMain.Resize += new System.EventHandler(this.pnlMain_Resize);
            // 
            // cmbQuality
            // 
            this.cmbQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbQuality.FormattingEnabled = true;
            this.cmbQuality.Location = new System.Drawing.Point(355, 8);
            this.cmbQuality.Name = "cmbQuality";
            this.cmbQuality.Size = new System.Drawing.Size(126, 20);
            this.cmbQuality.TabIndex = 5;
            this.cmbQuality.SelectedIndexChanged += new System.EventHandler(this.cmbQuality_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(320, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "画質";
            // 
            // ImageViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlTop);
            this.Name = "ImageViewerControl";
            this.Size = new System.Drawing.Size(813, 388);
            ((System.ComponentModel.ISupportInitialize)(this.pic)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pic;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.CheckBox chkFit;
        private System.Windows.Forms.Label lblRate;
        private System.Windows.Forms.Button btnTopLeft;
        private System.Windows.Forms.HScrollBar scrBarRate;
        private System.Windows.Forms.CheckBox chkLockImage;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ComboBox cmbQuality;
        private System.Windows.Forms.Label label1;
    }
}
