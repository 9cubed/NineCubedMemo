namespace NineCubed.Memo.Plugins.Grep
{
    partial class GrepControl
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
            this.chkRegExp = new System.Windows.Forms.CheckBox();
            this.chkNotIgnoreCase = new System.Windows.Forms.CheckBox();
            this.chkSubDir = new System.Windows.Forms.CheckBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.txtExtension = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.grid = new System.Windows.Forms.DataGridView();
            this.splitResult = new System.Windows.Forms.SplitContainer();
            this.txtResult = new NineCubed.Common.Controls.TextBoxEx();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitResult)).BeginInit();
            this.splitResult.Panel1.SuspendLayout();
            this.splitResult.Panel2.SuspendLayout();
            this.splitResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.chkRegExp);
            this.pnlTop.Controls.Add(this.chkNotIgnoreCase);
            this.pnlTop.Controls.Add(this.chkSubDir);
            this.pnlTop.Controls.Add(this.lblMsg);
            this.pnlTop.Controls.Add(this.txtExtension);
            this.pnlTop.Controls.Add(this.label3);
            this.pnlTop.Controls.Add(this.label2);
            this.pnlTop.Controls.Add(this.label1);
            this.pnlTop.Controls.Add(this.btnCancel);
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.btnSearch);
            this.pnlTop.Controls.Add(this.txtPath);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(876, 111);
            this.pnlTop.TabIndex = 0;
            // 
            // chkRegExp
            // 
            this.chkRegExp.AutoSize = true;
            this.chkRegExp.Location = new System.Drawing.Point(437, 62);
            this.chkRegExp.Name = "chkRegExp";
            this.chkRegExp.Size = new System.Drawing.Size(100, 16);
            this.chkRegExp.TabIndex = 31;
            this.chkRegExp.Text = "正規表現を使う";
            this.chkRegExp.UseVisualStyleBackColor = true;
            // 
            // chkNotIgnoreCase
            // 
            this.chkNotIgnoreCase.AutoSize = true;
            this.chkNotIgnoreCase.Location = new System.Drawing.Point(123, 62);
            this.chkNotIgnoreCase.Name = "chkNotIgnoreCase";
            this.chkNotIgnoreCase.Size = new System.Drawing.Size(154, 16);
            this.chkNotIgnoreCase.TabIndex = 30;
            this.chkNotIgnoreCase.Text = "大文字・小文字を区別する";
            this.chkNotIgnoreCase.UseVisualStyleBackColor = true;
            // 
            // chkSubDir
            // 
            this.chkSubDir.AutoSize = true;
            this.chkSubDir.Checked = true;
            this.chkSubDir.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSubDir.Location = new System.Drawing.Point(292, 62);
            this.chkSubDir.Name = "chkSubDir";
            this.chkSubDir.Size = new System.Drawing.Size(130, 16);
            this.chkSubDir.TabIndex = 29;
            this.chkSubDir.Text = "サブフォルダも検索する";
            this.chkSubDir.UseVisualStyleBackColor = true;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Location = new System.Drawing.Point(90, 86);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(35, 12);
            this.lblMsg.TabIndex = 28;
            this.lblMsg.Text = "*****";
            // 
            // txtExtension
            // 
            this.txtExtension.Location = new System.Drawing.Point(377, 34);
            this.txtExtension.Name = "txtExtension";
            this.txtExtension.Size = new System.Drawing.Size(121, 19);
            this.txtExtension.TabIndex = 27;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(335, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 26;
            this.label3.Text = "拡張子";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(90, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 25;
            this.label2.Text = "検索";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 12);
            this.label1.TabIndex = 24;
            this.label1.Text = "パス";
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(7, 54);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(71, 40);
            this.btnCancel.TabIndex = 23;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(123, 34);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(197, 19);
            this.txtSearch.TabIndex = 22;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(7, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(71, 40);
            this.btnSearch.TabIndex = 21;
            this.btnSearch.Text = "検索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(123, 9);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(375, 19);
            this.txtPath.TabIndex = 20;
            // 
            // grid
            // 
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Location = new System.Drawing.Point(64, 22);
            this.grid.Name = "grid";
            this.grid.RowTemplate.Height = 21;
            this.grid.Size = new System.Drawing.Size(292, 90);
            this.grid.TabIndex = 1;
            this.grid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellDoubleClick);
            this.grid.SelectionChanged += new System.EventHandler(this.grid_SelectionChanged);
            // 
            // splitResult
            // 
            this.splitResult.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitResult.Location = new System.Drawing.Point(123, 196);
            this.splitResult.Name = "splitResult";
            this.splitResult.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitResult.Panel1
            // 
            this.splitResult.Panel1.Controls.Add(this.grid);
            // 
            // splitResult.Panel2
            // 
            this.splitResult.Panel2.Controls.Add(this.txtResult);
            this.splitResult.Size = new System.Drawing.Size(414, 480);
            this.splitResult.SplitterDistance = 138;
            this.splitResult.TabIndex = 2;
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(47, 93);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(248, 179);
            this.txtResult.TabIndex = 1;
            this.txtResult.Text = "";
            // 
            // GrepControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitResult);
            this.Controls.Add(this.pnlTop);
            this.Name = "GrepControl";
            this.Size = new System.Drawing.Size(876, 726);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.splitResult.Panel1.ResumeLayout(false);
            this.splitResult.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitResult)).EndInit();
            this.splitResult.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.CheckBox chkNotIgnoreCase;
        private System.Windows.Forms.CheckBox chkSubDir;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.TextBox txtExtension;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.SplitContainer splitResult;
        private Common.Controls.TextBoxEx txtResult;
        private System.Windows.Forms.CheckBox chkRegExp;
    }
}
