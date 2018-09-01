﻿namespace NineCubed.Memo.Plugins.TextEditor
{
    partial class TextEditorPlugin
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
            this.components = new System.ComponentModel.Container();
            this.popupMenuForTextbox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.popupMenuForTextbox_Cut = new System.Windows.Forms.ToolStripMenuItem();
            this.popupMenuForTextbox_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.popupMenuForTextbox_Paste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.popupMenuForTextbox_SetKeyMacro = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Encoding = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Encoding_ShiftJIS = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Encoding_UTF8 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Encoding_UTF8_BOM = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Encoding_UTF16_LE_BOM = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Encoding_UTF16_BE_BOM = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Encoding_EucJp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_NewLine = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_NewLine_CRLF = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_NewLine_CR = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_NewLine_LF = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusPath = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtMain = new NineCubed.Common.Controls.TextBoxEx();
            this.popupMenuForTextbox.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // popupMenuForTextbox
            // 
            this.popupMenuForTextbox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.popupMenuForTextbox_Cut,
            this.popupMenuForTextbox_Copy,
            this.popupMenuForTextbox_Paste,
            this.toolStripMenuItem1,
            this.popupMenuForTextbox_SetKeyMacro,
            this.menuFile_Encoding,
            this.menuFile_NewLine});
            this.popupMenuForTextbox.Name = "popupMenuForTextbox";
            this.popupMenuForTextbox.Size = new System.Drawing.Size(150, 142);
            // 
            // popupMenuForTextbox_Cut
            // 
            this.popupMenuForTextbox_Cut.Name = "popupMenuForTextbox_Cut";
            this.popupMenuForTextbox_Cut.Size = new System.Drawing.Size(149, 22);
            this.popupMenuForTextbox_Cut.Text = "切り取り";
            this.popupMenuForTextbox_Cut.Click += new System.EventHandler(this.popupMenuForTextbox_Cut_Click);
            // 
            // popupMenuForTextbox_Copy
            // 
            this.popupMenuForTextbox_Copy.Name = "popupMenuForTextbox_Copy";
            this.popupMenuForTextbox_Copy.Size = new System.Drawing.Size(149, 22);
            this.popupMenuForTextbox_Copy.Text = "コピー";
            this.popupMenuForTextbox_Copy.Click += new System.EventHandler(this.popupMenuForTextbox_Copy_Click);
            // 
            // popupMenuForTextbox_Paste
            // 
            this.popupMenuForTextbox_Paste.Name = "popupMenuForTextbox_Paste";
            this.popupMenuForTextbox_Paste.Size = new System.Drawing.Size(149, 22);
            this.popupMenuForTextbox_Paste.Text = "貼り付け";
            this.popupMenuForTextbox_Paste.Click += new System.EventHandler(this.popupMenuForTextbox_Paste_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(146, 6);
            // 
            // popupMenuForTextbox_SetKeyMacro
            // 
            this.popupMenuForTextbox_SetKeyMacro.Name = "popupMenuForTextbox_SetKeyMacro";
            this.popupMenuForTextbox_SetKeyMacro.Size = new System.Drawing.Size(149, 22);
            this.popupMenuForTextbox_SetKeyMacro.Text = "キー操作の登録";
            this.popupMenuForTextbox_SetKeyMacro.Click += new System.EventHandler(this.popupMenuForTextbox_SetKeyMacro_Click);
            // 
            // menuFile_Encoding
            // 
            this.menuFile_Encoding.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile_Encoding_ShiftJIS,
            this.menuFile_Encoding_UTF8,
            this.menuFile_Encoding_UTF8_BOM,
            this.menuFile_Encoding_UTF16_LE_BOM,
            this.menuFile_Encoding_UTF16_BE_BOM,
            this.menuFile_Encoding_EucJp});
            this.menuFile_Encoding.Name = "menuFile_Encoding";
            this.menuFile_Encoding.Size = new System.Drawing.Size(149, 22);
            this.menuFile_Encoding.Text = "文字コード";
            // 
            // menuFile_Encoding_ShiftJIS
            // 
            this.menuFile_Encoding_ShiftJIS.Name = "menuFile_Encoding_ShiftJIS";
            this.menuFile_Encoding_ShiftJIS.Size = new System.Drawing.Size(164, 22);
            this.menuFile_Encoding_ShiftJIS.Text = "Shift JIS";
            this.menuFile_Encoding_ShiftJIS.Click += new System.EventHandler(this.menuFile_Encoding_Sub);
            // 
            // menuFile_Encoding_UTF8
            // 
            this.menuFile_Encoding_UTF8.Name = "menuFile_Encoding_UTF8";
            this.menuFile_Encoding_UTF8.Size = new System.Drawing.Size(164, 22);
            this.menuFile_Encoding_UTF8.Text = "UTF-8";
            this.menuFile_Encoding_UTF8.Click += new System.EventHandler(this.menuFile_Encoding_Sub);
            // 
            // menuFile_Encoding_UTF8_BOM
            // 
            this.menuFile_Encoding_UTF8_BOM.Name = "menuFile_Encoding_UTF8_BOM";
            this.menuFile_Encoding_UTF8_BOM.Size = new System.Drawing.Size(164, 22);
            this.menuFile_Encoding_UTF8_BOM.Text = "UTF-8 BOM付き";
            this.menuFile_Encoding_UTF8_BOM.Click += new System.EventHandler(this.menuFile_Encoding_Sub);
            // 
            // menuFile_Encoding_UTF16_LE_BOM
            // 
            this.menuFile_Encoding_UTF16_LE_BOM.Name = "menuFile_Encoding_UTF16_LE_BOM";
            this.menuFile_Encoding_UTF16_LE_BOM.Size = new System.Drawing.Size(164, 22);
            this.menuFile_Encoding_UTF16_LE_BOM.Text = "UTF-16 BOM (LE)";
            this.menuFile_Encoding_UTF16_LE_BOM.Click += new System.EventHandler(this.menuFile_Encoding_Sub);
            // 
            // menuFile_Encoding_UTF16_BE_BOM
            // 
            this.menuFile_Encoding_UTF16_BE_BOM.Name = "menuFile_Encoding_UTF16_BE_BOM";
            this.menuFile_Encoding_UTF16_BE_BOM.Size = new System.Drawing.Size(164, 22);
            this.menuFile_Encoding_UTF16_BE_BOM.Text = "UTF-16 BOM (BE)";
            this.menuFile_Encoding_UTF16_BE_BOM.Click += new System.EventHandler(this.menuFile_Encoding_Sub);
            // 
            // menuFile_Encoding_EucJp
            // 
            this.menuFile_Encoding_EucJp.Name = "menuFile_Encoding_EucJp";
            this.menuFile_Encoding_EucJp.Size = new System.Drawing.Size(164, 22);
            this.menuFile_Encoding_EucJp.Text = "EUC-JP";
            this.menuFile_Encoding_EucJp.Click += new System.EventHandler(this.menuFile_Encoding_Sub);
            // 
            // menuFile_NewLine
            // 
            this.menuFile_NewLine.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile_NewLine_CRLF,
            this.menuFile_NewLine_CR,
            this.menuFile_NewLine_LF});
            this.menuFile_NewLine.Name = "menuFile_NewLine";
            this.menuFile_NewLine.Size = new System.Drawing.Size(149, 22);
            this.menuFile_NewLine.Text = "改行コード";
            // 
            // menuFile_NewLine_CRLF
            // 
            this.menuFile_NewLine_CRLF.Name = "menuFile_NewLine_CRLF";
            this.menuFile_NewLine_CRLF.Size = new System.Drawing.Size(100, 22);
            this.menuFile_NewLine_CRLF.Text = "CRLF";
            this.menuFile_NewLine_CRLF.Click += new System.EventHandler(this.menuFile_NewLine_Sub);
            // 
            // menuFile_NewLine_CR
            // 
            this.menuFile_NewLine_CR.Name = "menuFile_NewLine_CR";
            this.menuFile_NewLine_CR.Size = new System.Drawing.Size(100, 22);
            this.menuFile_NewLine_CR.Text = "CR";
            this.menuFile_NewLine_CR.Click += new System.EventHandler(this.menuFile_NewLine_Sub);
            // 
            // menuFile_NewLine_LF
            // 
            this.menuFile_NewLine_LF.Name = "menuFile_NewLine_LF";
            this.menuFile_NewLine_LF.Size = new System.Drawing.Size(100, 22);
            this.menuFile_NewLine_LF.Text = "LF";
            this.menuFile_NewLine_LF.Click += new System.EventHandler(this.menuFile_NewLine_Sub);
            // 
            // statusBar
            // 
            this.statusBar.Font = new System.Drawing.Font("Yu Gothic UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusPath});
            this.statusBar.Location = new System.Drawing.Point(0, 290);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(924, 25);
            this.statusBar.TabIndex = 5;
            this.statusBar.Text = "statusStrip1";
            // 
            // statusPath
            // 
            this.statusPath.BackColor = System.Drawing.SystemColors.Control;
            this.statusPath.Name = "statusPath";
            this.statusPath.Size = new System.Drawing.Size(151, 20);
            this.statusPath.Text = "toolStripStatusLabel1";
            // 
            // txtMain
            // 
            this.txtMain.ContextMenuStrip = this.popupMenuForTextbox;
            this.txtMain.Location = new System.Drawing.Point(41, 28);
            this.txtMain.Name = "txtMain";
            this.txtMain.Size = new System.Drawing.Size(262, 171);
            this.txtMain.TabIndex = 4;
            this.txtMain.Text = "";
            this.txtMain.ModifiedChanged += new System.EventHandler(this.txtMain_ModifiedChanged);
            this.txtMain.Enter += new System.EventHandler(this.txtMain_Enter);
            this.txtMain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMain_KeyDown);
            // 
            // TextEditorPlugin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.txtMain);
            this.Name = "TextEditorPlugin";
            this.Size = new System.Drawing.Size(924, 315);
            this.Load += new System.EventHandler(this.TextEditorPlugin_Load);
            this.popupMenuForTextbox.ResumeLayout(false);
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Common.Controls.TextBoxEx txtMain;
        private System.Windows.Forms.ContextMenuStrip popupMenuForTextbox;
        private System.Windows.Forms.ToolStripMenuItem popupMenuForTextbox_Cut;
        private System.Windows.Forms.ToolStripMenuItem popupMenuForTextbox_Copy;
        private System.Windows.Forms.ToolStripMenuItem popupMenuForTextbox_Paste;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem popupMenuForTextbox_SetKeyMacro;
        private System.Windows.Forms.ToolStripMenuItem menuFile_Encoding;
        private System.Windows.Forms.ToolStripMenuItem menuFile_Encoding_ShiftJIS;
        private System.Windows.Forms.ToolStripMenuItem menuFile_Encoding_UTF8;
        private System.Windows.Forms.ToolStripMenuItem menuFile_Encoding_UTF8_BOM;
        private System.Windows.Forms.ToolStripMenuItem menuFile_Encoding_UTF16_LE_BOM;
        private System.Windows.Forms.ToolStripMenuItem menuFile_Encoding_UTF16_BE_BOM;
        private System.Windows.Forms.ToolStripMenuItem menuFile_Encoding_EucJp;
        private System.Windows.Forms.ToolStripMenuItem menuFile_NewLine;
        private System.Windows.Forms.ToolStripMenuItem menuFile_NewLine_CRLF;
        private System.Windows.Forms.ToolStripMenuItem menuFile_NewLine_CR;
        private System.Windows.Forms.ToolStripMenuItem menuFile_NewLine_LF;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel statusPath;
    }
}
