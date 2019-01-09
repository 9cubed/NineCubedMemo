namespace FileInfoManager
{
    partial class FileInfoEditorControl
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
            this.btnSave = new System.Windows.Forms.Button();
            this.txtCreated = new System.Windows.Forms.TextBox();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.btnTag = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUpdated = new System.Windows.Forms.TextBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTag = new System.Windows.Forms.TextBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbValue = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnSave);
            this.pnlTop.Controls.Add(this.txtCreated);
            this.pnlTop.Controls.Add(this.txtSize);
            this.pnlTop.Controls.Add(this.btnTag);
            this.pnlTop.Controls.Add(this.label7);
            this.pnlTop.Controls.Add(this.label5);
            this.pnlTop.Controls.Add(this.txtUpdated);
            this.pnlTop.Controls.Add(this.txtPath);
            this.pnlTop.Controls.Add(this.label6);
            this.pnlTop.Controls.Add(this.label8);
            this.pnlTop.Controls.Add(this.label3);
            this.pnlTop.Controls.Add(this.txtTag);
            this.pnlTop.Controls.Add(this.txtTitle);
            this.pnlTop.Controls.Add(this.label1);
            this.pnlTop.Controls.Add(this.txtId);
            this.pnlTop.Controls.Add(this.label2);
            this.pnlTop.Controls.Add(this.cmbValue);
            this.pnlTop.Controls.Add(this.label4);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(903, 63);
            this.pnlTop.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(801, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(56, 45);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtCreated
            // 
            this.txtCreated.Location = new System.Drawing.Point(675, 7);
            this.txtCreated.Name = "txtCreated";
            this.txtCreated.ReadOnly = true;
            this.txtCreated.Size = new System.Drawing.Size(116, 19);
            this.txtCreated.TabIndex = 7;
            this.txtCreated.Text = "2019-01-01 00:00:00";
            this.txtCreated.Enter += new System.EventHandler(this.txtCreated_Enter);
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(415, 32);
            this.txtSize.Name = "txtSize";
            this.txtSize.ReadOnly = true;
            this.txtSize.Size = new System.Drawing.Size(74, 19);
            this.txtSize.TabIndex = 6;
            this.txtSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSize.TextChanged += new System.EventHandler(this.txtSize_TextChanged);
            this.txtSize.Enter += new System.EventHandler(this.txtSize_Enter);
            // 
            // btnTag
            // 
            this.btnTag.Location = new System.Drawing.Point(324, 30);
            this.btnTag.Name = "btnTag";
            this.btnTag.Size = new System.Drawing.Size(40, 24);
            this.btnTag.TabIndex = 5;
            this.btnTag.Text = "タグ";
            this.btnTag.UseVisualStyleBackColor = true;
            this.btnTag.Click += new System.EventHandler(this.btnTag_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(640, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 23;
            this.label7.Text = "更新";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(375, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 12);
            this.label5.TabIndex = 19;
            this.label5.Text = "サイズ";
            // 
            // txtUpdated
            // 
            this.txtUpdated.Location = new System.Drawing.Point(675, 32);
            this.txtUpdated.Name = "txtUpdated";
            this.txtUpdated.ReadOnly = true;
            this.txtUpdated.Size = new System.Drawing.Size(116, 19);
            this.txtUpdated.TabIndex = 8;
            this.txtUpdated.Text = "2019-01-01 00:00:00";
            this.txtUpdated.Enter += new System.EventHandler(this.txtUpdated_Enter);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(414, 7);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(215, 19);
            this.txtPath.TabIndex = 2;
            this.txtPath.TextChanged += new System.EventHandler(this.txtPath_TextChanged);
            this.txtPath.Enter += new System.EventHandler(this.txtPath_Enter);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(640, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 22;
            this.label6.Text = "作成";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(117, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(22, 12);
            this.label8.TabIndex = 25;
            this.label8.Text = "タグ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(384, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "パス";
            // 
            // txtTag
            // 
            this.txtTag.Location = new System.Drawing.Point(143, 32);
            this.txtTag.Name = "txtTag";
            this.txtTag.Size = new System.Drawing.Size(175, 19);
            this.txtTag.TabIndex = 4;
            this.txtTag.TextChanged += new System.EventHandler(this.txtTag_TextChanged);
            this.txtTag.Enter += new System.EventHandler(this.txtTag_Enter);
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(143, 7);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(221, 19);
            this.txtTitle.TabIndex = 1;
            this.txtTitle.TextChanged += new System.EventHandler(this.txtTitle_TextChanged);
            this.txtTitle.Enter += new System.EventHandler(this.txtTitle_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(100, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "タイトル";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(39, 7);
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(50, 19);
            this.txtId.TabIndex = 0;
            this.txtId.Enter += new System.EventHandler(this.txtId_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "ID";
            // 
            // cmbValue
            // 
            this.cmbValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbValue.FormattingEnabled = true;
            this.cmbValue.Items.AddRange(new object[] {
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
            this.cmbValue.Location = new System.Drawing.Point(39, 32);
            this.cmbValue.Name = "cmbValue";
            this.cmbValue.Size = new System.Drawing.Size(50, 20);
            this.cmbValue.TabIndex = 3;
            this.cmbValue.SelectedIndexChanged += new System.EventHandler(this.cmbValue_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 17;
            this.label4.Text = "評価";
            // 
            // FileInfoEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.Name = "FileInfoEditorControl";
            this.Size = new System.Drawing.Size(903, 383);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbValue;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.TextBox txtUpdated;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCreated;
        private System.Windows.Forms.TextBox txtTag;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnTag;
        private System.Windows.Forms.Button btnSave;
    }
}
