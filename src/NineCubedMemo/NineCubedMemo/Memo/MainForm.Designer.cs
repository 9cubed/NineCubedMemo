namespace NineCubed.Memo
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtMain = new System.Windows.Forms.RichTextBox();
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_New = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Close = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_End = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit_Undo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit_Redo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit_Cut = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit_Paste = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp_version = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolFile_New = new System.Windows.Forms.ToolStripButton();
            this.toolFile_Open = new System.Windows.Forms.ToolStripButton();
            this.toolFile_Save = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuBar.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtMain
            // 
            this.txtMain.Location = new System.Drawing.Point(172, 96);
            this.txtMain.Name = "txtMain";
            this.txtMain.Size = new System.Drawing.Size(297, 152);
            this.txtMain.TabIndex = 0;
            this.txtMain.Text = "";
            // 
            // menuBar
            // 
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuEdit,
            this.menuHelp});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size(579, 24);
            this.menuBar.TabIndex = 1;
            this.menuBar.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile_New,
            this.menuFile_Open,
            this.menuFile_Save,
            this.menuFile_SaveAs,
            this.menuFile_Close,
            this.menuFile_End});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(53, 20);
            this.menuFile.Text = "ファイル";
            // 
            // menuFile_New
            // 
            this.menuFile_New.Name = "menuFile_New";
            this.menuFile_New.Size = new System.Drawing.Size(180, 22);
            this.menuFile_New.Text = "新規作成";
            this.menuFile_New.Click += new System.EventHandler(this.menuFile_New_Click);
            // 
            // menuFile_Open
            // 
            this.menuFile_Open.Name = "menuFile_Open";
            this.menuFile_Open.Size = new System.Drawing.Size(180, 22);
            this.menuFile_Open.Text = "開く";
            this.menuFile_Open.Click += new System.EventHandler(this.menuFile_Open_Click);
            // 
            // menuFile_Save
            // 
            this.menuFile_Save.Name = "menuFile_Save";
            this.menuFile_Save.Size = new System.Drawing.Size(180, 22);
            this.menuFile_Save.Text = "上書き保存";
            this.menuFile_Save.Click += new System.EventHandler(this.menuFile_Save_Click);
            // 
            // menuFile_SaveAs
            // 
            this.menuFile_SaveAs.Name = "menuFile_SaveAs";
            this.menuFile_SaveAs.Size = new System.Drawing.Size(180, 22);
            this.menuFile_SaveAs.Text = "名前を付けて保存";
            this.menuFile_SaveAs.Click += new System.EventHandler(this.menuFile_SaveAs_Click);
            // 
            // menuFile_Close
            // 
            this.menuFile_Close.Name = "menuFile_Close";
            this.menuFile_Close.Size = new System.Drawing.Size(161, 22);
            this.menuFile_Close.Text = "閉じる";
            // 
            // menuFile_End
            // 
            this.menuFile_End.Name = "menuFile_End";
            this.menuFile_End.Size = new System.Drawing.Size(161, 22);
            this.menuFile_End.Text = "終了";
            // 
            // menuEdit
            // 
            this.menuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEdit_Undo,
            this.menuEdit_Redo,
            this.menuEdit_Cut,
            this.menuEdit_Copy,
            this.menuEdit_Paste,
            this.menuEdit_Delete});
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size(43, 20);
            this.menuEdit.Text = "編集";
            // 
            // menuEdit_Undo
            // 
            this.menuEdit_Undo.Name = "menuEdit_Undo";
            this.menuEdit_Undo.Size = new System.Drawing.Size(117, 22);
            this.menuEdit_Undo.Text = "元に戻す";
            // 
            // menuEdit_Redo
            // 
            this.menuEdit_Redo.Name = "menuEdit_Redo";
            this.menuEdit_Redo.Size = new System.Drawing.Size(117, 22);
            this.menuEdit_Redo.Text = "やり直す";
            // 
            // menuEdit_Cut
            // 
            this.menuEdit_Cut.Name = "menuEdit_Cut";
            this.menuEdit_Cut.Size = new System.Drawing.Size(117, 22);
            this.menuEdit_Cut.Text = "切り取り";
            // 
            // menuEdit_Copy
            // 
            this.menuEdit_Copy.Name = "menuEdit_Copy";
            this.menuEdit_Copy.Size = new System.Drawing.Size(117, 22);
            this.menuEdit_Copy.Text = "コピー";
            // 
            // menuEdit_Paste
            // 
            this.menuEdit_Paste.Name = "menuEdit_Paste";
            this.menuEdit_Paste.Size = new System.Drawing.Size(117, 22);
            this.menuEdit_Paste.Text = "貼り付け";
            // 
            // menuEdit_Delete
            // 
            this.menuEdit_Delete.Name = "menuEdit_Delete";
            this.menuEdit_Delete.Size = new System.Drawing.Size(117, 22);
            this.menuEdit_Delete.Text = "削除";
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHelp_version});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(48, 20);
            this.menuHelp.Text = "ヘルプ";
            // 
            // menuHelp_version
            // 
            this.menuHelp_version.Name = "menuHelp_version";
            this.menuHelp_version.Size = new System.Drawing.Size(142, 22);
            this.menuHelp_version.Text = "バージョン情報";
            this.menuHelp_version.Click += new System.EventHandler(this.menuHelp_version_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolFile_New,
            this.toolFile_Open,
            this.toolFile_Save});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(579, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolFile_New
            // 
            this.toolFile_New.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolFile_New.Image = ((System.Drawing.Image)(resources.GetObject("toolFile_New.Image")));
            this.toolFile_New.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolFile_New.Name = "toolFile_New";
            this.toolFile_New.Size = new System.Drawing.Size(23, 22);
            this.toolFile_New.Text = "toolStripButton1";
            // 
            // toolFile_Open
            // 
            this.toolFile_Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolFile_Open.Image = ((System.Drawing.Image)(resources.GetObject("toolFile_Open.Image")));
            this.toolFile_Open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolFile_Open.Name = "toolFile_Open";
            this.toolFile_Open.Size = new System.Drawing.Size(23, 22);
            this.toolFile_Open.Text = "toolStripButton1";
            // 
            // toolFile_Save
            // 
            this.toolFile_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolFile_Save.Image = ((System.Drawing.Image)(resources.GetObject("toolFile_Save.Image")));
            this.toolFile_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolFile_Save.Name = "toolFile_Save";
            this.toolFile_Save.Size = new System.Drawing.Size(23, 22);
            this.toolFile_Save.Text = "toolStripButton1";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 344);
            this.Controls.Add(this.txtMain);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuBar);
            this.MainMenuStrip = this.menuBar;
            this.Name = "MainForm";
            this.Text = "メモ帳";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtMain;
        private System.Windows.Forms.MenuStrip menuBar;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuFile_New;
        private System.Windows.Forms.ToolStripMenuItem menuFile_Open;
        private System.Windows.Forms.ToolStripMenuItem menuFile_Save;
        private System.Windows.Forms.ToolStripMenuItem menuFile_SaveAs;
        private System.Windows.Forms.ToolStripMenuItem menuFile_Close;
        private System.Windows.Forms.ToolStripMenuItem menuFile_End;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolFile_New;
        private System.Windows.Forms.ToolStripButton toolFile_Open;
        private System.Windows.Forms.ToolStripButton toolFile_Save;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        private System.Windows.Forms.ToolStripMenuItem menuEdit_Undo;
        private System.Windows.Forms.ToolStripMenuItem menuEdit_Redo;
        private System.Windows.Forms.ToolStripMenuItem menuEdit_Cut;
        private System.Windows.Forms.ToolStripMenuItem menuEdit_Copy;
        private System.Windows.Forms.ToolStripMenuItem menuEdit_Paste;
        private System.Windows.Forms.ToolStripMenuItem menuEdit_Delete;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuHelp_version;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}