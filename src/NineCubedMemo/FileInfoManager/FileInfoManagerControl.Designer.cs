namespace FileInfoManager
{
    partial class FileInfoManagerControl
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
            this.cmbKeyword = new System.Windows.Forms.ComboBox();
            this.btnTag = new System.Windows.Forms.Button();
            this.cmbTag = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtTag = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbValueTo = new System.Windows.Forms.ComboBox();
            this.cmbValueFrom = new System.Windows.Forms.ComboBox();
            this.grid = new System.Windows.Forms.DataGridView();
            this.btnEdit = new System.Windows.Forms.Button();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnEdit);
            this.pnlTop.Controls.Add(this.cmbKeyword);
            this.pnlTop.Controls.Add(this.btnTag);
            this.pnlTop.Controls.Add(this.cmbTag);
            this.pnlTop.Controls.Add(this.btnSearch);
            this.pnlTop.Controls.Add(this.txtTag);
            this.pnlTop.Controls.Add(this.label4);
            this.pnlTop.Controls.Add(this.txtKeyword);
            this.pnlTop.Controls.Add(this.label3);
            this.pnlTop.Controls.Add(this.label2);
            this.pnlTop.Controls.Add(this.label1);
            this.pnlTop.Controls.Add(this.cmbValueTo);
            this.pnlTop.Controls.Add(this.cmbValueFrom);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(735, 63);
            this.pnlTop.TabIndex = 0;
            // 
            // cmbKeyword
            // 
            this.cmbKeyword.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKeyword.FormattingEnabled = true;
            this.cmbKeyword.Items.AddRange(new object[] {
            "AND",
            "OR",
            "NOT"});
            this.cmbKeyword.Location = new System.Drawing.Point(420, 34);
            this.cmbKeyword.Name = "cmbKeyword";
            this.cmbKeyword.Size = new System.Drawing.Size(55, 20);
            this.cmbKeyword.TabIndex = 8;
            // 
            // btnTag
            // 
            this.btnTag.Location = new System.Drawing.Point(374, 6);
            this.btnTag.Name = "btnTag";
            this.btnTag.Size = new System.Drawing.Size(40, 22);
            this.btnTag.TabIndex = 3;
            this.btnTag.Text = "タグ";
            this.btnTag.UseVisualStyleBackColor = true;
            this.btnTag.Click += new System.EventHandler(this.btnTag_Click);
            // 
            // cmbTag
            // 
            this.cmbTag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTag.FormattingEnabled = true;
            this.cmbTag.Items.AddRange(new object[] {
            "AND",
            "OR",
            "NOT"});
            this.cmbTag.Location = new System.Drawing.Point(420, 7);
            this.cmbTag.Name = "cmbTag";
            this.cmbTag.Size = new System.Drawing.Size(55, 20);
            this.cmbTag.TabIndex = 4;
            this.cmbTag.Enter += new System.EventHandler(this.cmbTag_Enter);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(483, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(55, 39);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "検索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtTag
            // 
            this.txtTag.Location = new System.Drawing.Point(228, 7);
            this.txtTag.Name = "txtTag";
            this.txtTag.Size = new System.Drawing.Size(140, 19);
            this.txtTag.TabIndex = 2;
            this.txtTag.Enter += new System.EventHandler(this.txtTag_Enter);
            this.txtTag.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTag_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "評価";
            // 
            // txtKeyword
            // 
            this.txtKeyword.Location = new System.Drawing.Point(68, 35);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(346, 19);
            this.txtKeyword.TabIndex = 5;
            this.txtKeyword.Enter += new System.EventHandler(this.txtKeyword_Enter);
            this.txtKeyword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKeyword_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "キーワード";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(204, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "タグ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(122, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "～";
            // 
            // cmbValueTo
            // 
            this.cmbValueTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbValueTo.FormattingEnabled = true;
            this.cmbValueTo.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cmbValueTo.Location = new System.Drawing.Point(144, 7);
            this.cmbValueTo.Name = "cmbValueTo";
            this.cmbValueTo.Size = new System.Drawing.Size(47, 20);
            this.cmbValueTo.TabIndex = 1;
            this.cmbValueTo.Enter += new System.EventHandler(this.cmbValueTo_Enter);
            // 
            // cmbValueFrom
            // 
            this.cmbValueFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbValueFrom.FormattingEnabled = true;
            this.cmbValueFrom.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cmbValueFrom.Location = new System.Drawing.Point(68, 7);
            this.cmbValueFrom.Name = "cmbValueFrom";
            this.cmbValueFrom.Size = new System.Drawing.Size(47, 20);
            this.cmbValueFrom.TabIndex = 0;
            this.cmbValueFrom.Enter += new System.EventHandler(this.cmbValueFrom_Enter);
            // 
            // grid
            // 
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Location = new System.Drawing.Point(85, 195);
            this.grid.Name = "grid";
            this.grid.RowTemplate.Height = 21;
            this.grid.Size = new System.Drawing.Size(349, 104);
            this.grid.TabIndex = 8;
            this.grid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellDoubleClick);
            this.grid.SelectionChanged += new System.EventHandler(this.grid_SelectionChanged);
            this.grid.Enter += new System.EventHandler(this.grid_Enter);
            this.grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_KeyDown);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(544, 10);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(55, 39);
            this.btnEdit.TabIndex = 7;
            this.btnEdit.Text = "編集";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // FileInfoManagerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grid);
            this.Controls.Add(this.pnlTop);
            this.Name = "FileInfoManagerControl";
            this.Size = new System.Drawing.Size(735, 472);
            this.Load += new System.EventHandler(this.FileInfoManagerControl_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTag;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbValueTo;
        private System.Windows.Forms.ComboBox cmbValueFrom;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbTag;
        private System.Windows.Forms.Button btnTag;
        private System.Windows.Forms.ComboBox cmbKeyword;
        private System.Windows.Forms.Button btnEdit;
    }
}
