namespace NineCubed.Memo.Plugins.PathField
{
    partial class PathFieldPlugin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PathFieldPlugin));
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnMoveToParentDir = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtPath
            // 
            this.txtPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPath.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtPath.Location = new System.Drawing.Point(30, 2);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(322, 22);
            this.txtPath.TabIndex = 0;
            this.txtPath.Enter += new System.EventHandler(this.txtPath_Enter);
            this.txtPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPath_KeyDown);
            // 
            // btnMoveToParentDir
            // 
            this.btnMoveToParentDir.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnMoveToParentDir.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveToParentDir.Image")));
            this.btnMoveToParentDir.Location = new System.Drawing.Point(2, 2);
            this.btnMoveToParentDir.Name = "btnMoveToParentDir";
            this.btnMoveToParentDir.Size = new System.Drawing.Size(28, 23);
            this.btnMoveToParentDir.TabIndex = 1;
            this.btnMoveToParentDir.UseVisualStyleBackColor = true;
            this.btnMoveToParentDir.Click += new System.EventHandler(this.btnMoveToParentDir_Click);
            // 
            // PathFieldPlugin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.btnMoveToParentDir);
            this.Name = "PathFieldPlugin";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(354, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnMoveToParentDir;
    }
}
