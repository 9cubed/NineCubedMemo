namespace FileInfoManager.Manager
{
    partial class TagForm
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblMsg = new System.Windows.Forms.Label();
            this.btnAddTag = new System.Windows.Forms.Button();
            this.txtTag = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSet = new System.Windows.Forms.Button();
            this.listTag = new System.Windows.Forms.ListBox();
            this.pnlTop.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.lblMsg);
            this.pnlTop.Controls.Add(this.btnAddTag);
            this.pnlTop.Controls.Add(this.txtTag);
            this.pnlTop.Controls.Add(this.label1);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(438, 123);
            this.pnlTop.TabIndex = 0;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMsg.Location = new System.Drawing.Point(10, 17);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(269, 19);
            this.lblMsg.TabIndex = 4;
            this.lblMsg.Text = "追加するタグを入力してください。";
            // 
            // btnAddTag
            // 
            this.btnAddTag.Location = new System.Drawing.Point(197, 79);
            this.btnAddTag.Name = "btnAddTag";
            this.btnAddTag.Size = new System.Drawing.Size(37, 30);
            this.btnAddTag.TabIndex = 3;
            this.btnAddTag.Text = "▲";
            this.btnAddTag.UseVisualStyleBackColor = true;
            this.btnAddTag.Click += new System.EventHandler(this.btnAddTag_Click);
            // 
            // txtTag
            // 
            this.txtTag.Location = new System.Drawing.Point(40, 53);
            this.txtTag.Name = "txtTag";
            this.txtTag.Size = new System.Drawing.Size(380, 19);
            this.txtTag.TabIndex = 1;
            this.txtTag.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTag_KeyDown);
            this.txtTag.Leave += new System.EventHandler(this.txtTag_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "タグ";
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Controls.Add(this.btnSet);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 398);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(438, 52);
            this.pnlBottom.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.Location = new System.Drawing.Point(366, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(64, 36);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(8, 8);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(64, 36);
            this.btnSet.TabIndex = 0;
            this.btnSet.Text = "設定";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // listTag
            // 
            this.listTag.FormattingEnabled = true;
            this.listTag.ItemHeight = 12;
            this.listTag.Location = new System.Drawing.Point(36, 184);
            this.listTag.MultiColumn = true;
            this.listTag.Name = "listTag";
            this.listTag.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listTag.Size = new System.Drawing.Size(198, 124);
            this.listTag.TabIndex = 2;
            this.listTag.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listTag_KeyDown);
            // 
            // TagForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 450);
            this.Controls.Add(this.listTag);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Name = "TagForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "タグ";
            this.Resize += new System.EventHandler(this.TagForm_Resize);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.TextBox txtTag;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListBox listTag;
        private System.Windows.Forms.Button btnAddTag;
        private System.Windows.Forms.Label lblMsg;
    }
}