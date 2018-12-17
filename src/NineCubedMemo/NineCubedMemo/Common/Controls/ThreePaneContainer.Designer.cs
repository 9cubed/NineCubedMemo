namespace NineCubed.Common.Controls
{
    partial class ThreePaneContainer
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
            this.splitVertical = new System.Windows.Forms.SplitContainer();
            this.splitHorizontal = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitVertical)).BeginInit();
            this.splitVertical.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitHorizontal)).BeginInit();
            this.splitHorizontal.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitVertical
            // 
            this.splitVertical.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitVertical.Location = new System.Drawing.Point(40, 55);
            this.splitVertical.Name = "splitVertical";
            this.splitVertical.Size = new System.Drawing.Size(173, 180);
            this.splitVertical.SplitterDistance = 57;
            this.splitVertical.TabIndex = 0;
            // 
            // splitHorizontal
            // 
            this.splitHorizontal.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitHorizontal.Location = new System.Drawing.Point(289, 55);
            this.splitHorizontal.Name = "splitHorizontal";
            this.splitHorizontal.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitHorizontal.Size = new System.Drawing.Size(173, 180);
            this.splitHorizontal.SplitterDistance = 57;
            this.splitHorizontal.TabIndex = 1;
            // 
            // ThreePaneContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitHorizontal);
            this.Controls.Add(this.splitVertical);
            this.Name = "ThreePaneContainer";
            this.Size = new System.Drawing.Size(796, 543);
            ((System.ComponentModel.ISupportInitialize)(this.splitVertical)).EndInit();
            this.splitVertical.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitHorizontal)).EndInit();
            this.splitHorizontal.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitVertical;
        private System.Windows.Forms.SplitContainer splitHorizontal;
    }
}
