namespace NineCubed.Memo.Plugins.FileList
{
    partial class FileListPluginEx
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.cmbPattern = new System.Windows.Forms.ComboBox();
            this.btnDisplay = new System.Windows.Forms.Button();
            this.chkFileVisible = new System.Windows.Forms.CheckBox();
            this.chkDirVisible = new System.Windows.Forms.CheckBox();
            this.fileListGrid = new NineCubed.Common.Controls.FileList.FileListGrid();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileListGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.cmbPattern);
            this.pnlTop.Controls.Add(this.btnDisplay);
            this.pnlTop.Controls.Add(this.chkFileVisible);
            this.pnlTop.Controls.Add(this.chkDirVisible);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(685, 31);
            this.pnlTop.TabIndex = 1;
            this.pnlTop.Click += new System.EventHandler(this.pnlTop_Click);
            // 
            // cmbPattern
            // 
            this.cmbPattern.FormattingEnabled = true;
            this.cmbPattern.Location = new System.Drawing.Point(280, 5);
            this.cmbPattern.Name = "cmbPattern";
            this.cmbPattern.Size = new System.Drawing.Size(120, 20);
            this.cmbPattern.TabIndex = 4;
            this.cmbPattern.SelectedValueChanged += new System.EventHandler(this.cmbPattern_SelectedValueChanged);
            this.cmbPattern.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbPattern_KeyDown);
            // 
            // btnDisplay
            // 
            this.btnDisplay.Location = new System.Drawing.Point(411, 3);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(55, 24);
            this.btnDisplay.TabIndex = 3;
            this.btnDisplay.Text = "表示";
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // chkFileVisible
            // 
            this.chkFileVisible.AutoSize = true;
            this.chkFileVisible.Checked = true;
            this.chkFileVisible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFileVisible.Location = new System.Drawing.Point(140, 8);
            this.chkFileVisible.Name = "chkFileVisible";
            this.chkFileVisible.Size = new System.Drawing.Size(58, 16);
            this.chkFileVisible.TabIndex = 1;
            this.chkFileVisible.Text = "ファイル";
            this.chkFileVisible.UseVisualStyleBackColor = true;
            this.chkFileVisible.CheckedChanged += new System.EventHandler(this.chkFileVisible_CheckedChanged);
            // 
            // chkDirVisible
            // 
            this.chkDirVisible.AutoSize = true;
            this.chkDirVisible.Checked = true;
            this.chkDirVisible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDirVisible.Location = new System.Drawing.Point(8, 8);
            this.chkDirVisible.Name = "chkDirVisible";
            this.chkDirVisible.Size = new System.Drawing.Size(59, 16);
            this.chkDirVisible.TabIndex = 0;
            this.chkDirVisible.Text = "フォルダ";
            this.chkDirVisible.UseVisualStyleBackColor = true;
            this.chkDirVisible.CheckedChanged += new System.EventHandler(this.chkDirVisible_CheckedChanged);
            // 
            // fileListGrid
            // 
            this.fileListGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fileListGrid.GetFileList = null;
            this.fileListGrid.Location = new System.Drawing.Point(103, 96);
            this.fileListGrid.Name = "fileListGrid";
            this.fileListGrid.RowTemplate.Height = 21;
            this.fileListGrid.Size = new System.Drawing.Size(374, 227);
            this.fileListGrid.TabIndex = 0;
            this.fileListGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.fileListGrid_CellDoubleClick);
            this.fileListGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.fileListGrid_CellEndEdit);
            this.fileListGrid.SelectionChanged += new System.EventHandler(this.fileListGrid_SelectionChanged);
            this.fileListGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fileListGrid_KeyDown);
            this.fileListGrid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fileListGrid_KeyPress);
            this.fileListGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.fileListGrid_MouseDown);
            // 
            // FileListPluginEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.fileListGrid);
            this.Controls.Add(this.pnlTop);
            this.Name = "FileListPluginEx";
            this.Size = new System.Drawing.Size(685, 450);
            this.Enter += new System.EventHandler(this.FileListPluginEx_Enter);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileListGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Controls.FileList.FileListGrid fileListGrid;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.CheckBox chkFileVisible;
        private System.Windows.Forms.CheckBox chkDirVisible;
        private System.Windows.Forms.Button btnDisplay;
        private System.Windows.Forms.ComboBox cmbPattern;
    }
}
