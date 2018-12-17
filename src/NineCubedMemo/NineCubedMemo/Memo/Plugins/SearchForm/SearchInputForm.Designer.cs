namespace NineCubed.Memo.Plugins.SearchForm
{
    partial class SearchInputForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.txtReplace = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSearchBackward = new System.Windows.Forms.Button();
            this.btnSearchForward = new System.Windows.Forms.Button();
            this.chkCase = new System.Windows.Forms.CheckBox();
            this.btnReplaceForward = new System.Windows.Forms.Button();
            this.btnReplaceBackward = new System.Windows.Forms.Button();
            this.btnReplaceAll = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "検索";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(49, 12);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(230, 19);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // txtReplace
            // 
            this.txtReplace.Location = new System.Drawing.Point(329, 12);
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.Size = new System.Drawing.Size(230, 19);
            this.txtReplace.TabIndex = 3;
            this.txtReplace.TextChanged += new System.EventHandler(this.txtReplace_TextChanged);
            this.txtReplace.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtReplace_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(294, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "置換";
            // 
            // btnSearchBackward
            // 
            this.btnSearchBackward.Location = new System.Drawing.Point(49, 37);
            this.btnSearchBackward.Name = "btnSearchBackward";
            this.btnSearchBackward.Size = new System.Drawing.Size(32, 24);
            this.btnSearchBackward.TabIndex = 4;
            this.btnSearchBackward.Text = "<";
            this.btnSearchBackward.UseVisualStyleBackColor = true;
            this.btnSearchBackward.Click += new System.EventHandler(this.btnSearchBackward_Click);
            // 
            // btnSearchForward
            // 
            this.btnSearchForward.Location = new System.Drawing.Point(87, 37);
            this.btnSearchForward.Name = "btnSearchForward";
            this.btnSearchForward.Size = new System.Drawing.Size(32, 24);
            this.btnSearchForward.TabIndex = 5;
            this.btnSearchForward.Text = ">";
            this.btnSearchForward.UseVisualStyleBackColor = true;
            this.btnSearchForward.Click += new System.EventHandler(this.btnSearchForward_Click);
            // 
            // chkCase
            // 
            this.chkCase.AutoSize = true;
            this.chkCase.Location = new System.Drawing.Point(125, 42);
            this.chkCase.Name = "chkCase";
            this.chkCase.Size = new System.Drawing.Size(154, 16);
            this.chkCase.TabIndex = 6;
            this.chkCase.Text = "大文字・小文字を無視する";
            this.chkCase.UseVisualStyleBackColor = true;
            this.chkCase.CheckedChanged += new System.EventHandler(this.chkCase_CheckedChanged);
            // 
            // btnReplaceForward
            // 
            this.btnReplaceForward.Location = new System.Drawing.Point(367, 37);
            this.btnReplaceForward.Name = "btnReplaceForward";
            this.btnReplaceForward.Size = new System.Drawing.Size(32, 24);
            this.btnReplaceForward.TabIndex = 8;
            this.btnReplaceForward.Text = ">";
            this.btnReplaceForward.UseVisualStyleBackColor = true;
            this.btnReplaceForward.Click += new System.EventHandler(this.btnReplaceForward_Click);
            // 
            // btnReplaceBackward
            // 
            this.btnReplaceBackward.Location = new System.Drawing.Point(329, 37);
            this.btnReplaceBackward.Name = "btnReplaceBackward";
            this.btnReplaceBackward.Size = new System.Drawing.Size(32, 24);
            this.btnReplaceBackward.TabIndex = 7;
            this.btnReplaceBackward.Text = "<";
            this.btnReplaceBackward.UseVisualStyleBackColor = true;
            this.btnReplaceBackward.Click += new System.EventHandler(this.btnReplaceBackward_Click);
            // 
            // btnReplaceAll
            // 
            this.btnReplaceAll.Location = new System.Drawing.Point(405, 37);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(64, 24);
            this.btnReplaceAll.TabIndex = 9;
            this.btnReplaceAll.Text = "全置換";
            this.btnReplaceAll.UseVisualStyleBackColor = true;
            this.btnReplaceAll.Click += new System.EventHandler(this.btnReplaceAll_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(495, 37);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(64, 24);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // SearchInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(572, 71);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReplaceAll);
            this.Controls.Add(this.btnReplaceForward);
            this.Controls.Add(this.btnReplaceBackward);
            this.Controls.Add(this.chkCase);
            this.Controls.Add(this.btnSearchForward);
            this.Controls.Add(this.btnSearchBackward);
            this.Controls.Add(this.txtReplace);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SearchInputForm";
            this.Text = "検索・置換";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SearchInputForm_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.TextBox txtReplace;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSearchBackward;
        private System.Windows.Forms.Button btnSearchForward;
        private System.Windows.Forms.CheckBox chkCase;
        private System.Windows.Forms.Button btnReplaceForward;
        private System.Windows.Forms.Button btnReplaceBackward;
        private System.Windows.Forms.Button btnReplaceAll;
        private System.Windows.Forms.Button btnClose;
    }
}