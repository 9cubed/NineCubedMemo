﻿namespace NineCubed.Memo
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.popupMenuForTextbox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.popupMenuForTextbox_Cut = new System.Windows.Forms.ToolStripMenuItem();
            this.popupMenuForTextbox_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.popupMenuForTextbox_Paste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.popupMenuForTextbox_SetKeyMacro = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_New = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Open_Encoding = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Open_ShiftJIS = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Open_UTF8 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Open_UTF8_BOM = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Open_UTF16_LE_BOM = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Open_UTF16_BE_BOM = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Open_EucJp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Open_Binary = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFile_Close = new System.Windows.Forms.ToolStripMenuItem();
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
            this.menuFile_End = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit_Undo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit_Redo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit_Cut = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit_Paste = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSearch_Search = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSearch_SearchForward = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSearch_SearchBackward = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSearch_ReplaceForward = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSearch_ReplaceBackward = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMacro = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMacro_StartRec = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMacro_Play = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMacro_List = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMacro_Set = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp_version = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolFile_New = new System.Windows.Forms.ToolStripButton();
            this.toolFile_Open = new System.Windows.Forms.ToolStripButton();
            this.toolFile_Save = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.txtMain = new NineCubed.Common.Controls.TextBoxEx();
            this.popupMenuForTextbox.SuspendLayout();
            this.menuBar.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // popupMenuForTextbox
            // 
            this.popupMenuForTextbox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.popupMenuForTextbox_Cut,
            this.popupMenuForTextbox_Copy,
            this.popupMenuForTextbox_Paste,
            this.toolStripMenuItem1,
            this.popupMenuForTextbox_SetKeyMacro});
            this.popupMenuForTextbox.Name = "popupMenuForTextbox";
            this.popupMenuForTextbox.Size = new System.Drawing.Size(150, 98);
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
            // menuBar
            // 
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuEdit,
            this.menuSearch,
            this.menuMacro,
            this.menuHelp});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size(869, 24);
            this.menuBar.TabIndex = 1;
            this.menuBar.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile_New,
            this.menuFile_Open,
            this.menuFile_Open_Encoding,
            this.menuFile_Open_Binary,
            this.menuFile_Save,
            this.menuFile_SaveAs,
            this.menuFile_Close,
            this.menuFile_Encoding,
            this.menuFile_NewLine,
            this.menuFile_End});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(53, 20);
            this.menuFile.Text = "ファイル";
            // 
            // menuFile_New
            // 
            this.menuFile_New.Name = "menuFile_New";
            this.menuFile_New.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.menuFile_New.Size = new System.Drawing.Size(189, 22);
            this.menuFile_New.Text = "新規作成";
            this.menuFile_New.Click += new System.EventHandler(this.menuFile_New_Click);
            // 
            // menuFile_Open
            // 
            this.menuFile_Open.Name = "menuFile_Open";
            this.menuFile_Open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuFile_Open.Size = new System.Drawing.Size(189, 22);
            this.menuFile_Open.Text = "開く";
            this.menuFile_Open.Click += new System.EventHandler(this.menuFile_Open_Click);
            // 
            // menuFile_Open_Encoding
            // 
            this.menuFile_Open_Encoding.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile_Open_ShiftJIS,
            this.menuFile_Open_UTF8,
            this.menuFile_Open_UTF8_BOM,
            this.menuFile_Open_UTF16_LE_BOM,
            this.menuFile_Open_UTF16_BE_BOM,
            this.menuFile_Open_EucJp});
            this.menuFile_Open_Encoding.Name = "menuFile_Open_Encoding";
            this.menuFile_Open_Encoding.Size = new System.Drawing.Size(189, 22);
            this.menuFile_Open_Encoding.Text = "開く（文字コード指定）";
            // 
            // menuFile_Open_ShiftJIS
            // 
            this.menuFile_Open_ShiftJIS.Name = "menuFile_Open_ShiftJIS";
            this.menuFile_Open_ShiftJIS.Size = new System.Drawing.Size(164, 22);
            this.menuFile_Open_ShiftJIS.Text = "Shift JIS";
            this.menuFile_Open_ShiftJIS.Click += new System.EventHandler(this.menuFile_Open_Encoding_Sub_Click);
            // 
            // menuFile_Open_UTF8
            // 
            this.menuFile_Open_UTF8.Name = "menuFile_Open_UTF8";
            this.menuFile_Open_UTF8.Size = new System.Drawing.Size(164, 22);
            this.menuFile_Open_UTF8.Text = "UTF-8";
            this.menuFile_Open_UTF8.Click += new System.EventHandler(this.menuFile_Open_Encoding_Sub_Click);
            // 
            // menuFile_Open_UTF8_BOM
            // 
            this.menuFile_Open_UTF8_BOM.Name = "menuFile_Open_UTF8_BOM";
            this.menuFile_Open_UTF8_BOM.Size = new System.Drawing.Size(164, 22);
            this.menuFile_Open_UTF8_BOM.Text = "UTF-8 BOM";
            this.menuFile_Open_UTF8_BOM.Click += new System.EventHandler(this.menuFile_Open_Encoding_Sub_Click);
            // 
            // menuFile_Open_UTF16_LE_BOM
            // 
            this.menuFile_Open_UTF16_LE_BOM.Name = "menuFile_Open_UTF16_LE_BOM";
            this.menuFile_Open_UTF16_LE_BOM.Size = new System.Drawing.Size(164, 22);
            this.menuFile_Open_UTF16_LE_BOM.Text = "UTF-16 BOM (LE)";
            this.menuFile_Open_UTF16_LE_BOM.Click += new System.EventHandler(this.menuFile_Open_Encoding_Sub_Click);
            // 
            // menuFile_Open_UTF16_BE_BOM
            // 
            this.menuFile_Open_UTF16_BE_BOM.Name = "menuFile_Open_UTF16_BE_BOM";
            this.menuFile_Open_UTF16_BE_BOM.Size = new System.Drawing.Size(164, 22);
            this.menuFile_Open_UTF16_BE_BOM.Text = "UTF-16 BOM (BE)";
            this.menuFile_Open_UTF16_BE_BOM.Click += new System.EventHandler(this.menuFile_Open_Encoding_Sub_Click);
            // 
            // menuFile_Open_EucJp
            // 
            this.menuFile_Open_EucJp.Name = "menuFile_Open_EucJp";
            this.menuFile_Open_EucJp.Size = new System.Drawing.Size(164, 22);
            this.menuFile_Open_EucJp.Text = "EUC-JP";
            this.menuFile_Open_EucJp.Click += new System.EventHandler(this.menuFile_Open_Encoding_Sub_Click);
            // 
            // menuFile_Open_Binary
            // 
            this.menuFile_Open_Binary.Name = "menuFile_Open_Binary";
            this.menuFile_Open_Binary.Size = new System.Drawing.Size(189, 22);
            this.menuFile_Open_Binary.Text = "開く（バイナリ形式）";
            this.menuFile_Open_Binary.Click += new System.EventHandler(this.menuFile_Open_Binary_Click);
            // 
            // menuFile_Save
            // 
            this.menuFile_Save.Name = "menuFile_Save";
            this.menuFile_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuFile_Save.Size = new System.Drawing.Size(189, 22);
            this.menuFile_Save.Text = "上書き保存";
            this.menuFile_Save.Click += new System.EventHandler(this.menuFile_Save_Click);
            // 
            // menuFile_SaveAs
            // 
            this.menuFile_SaveAs.Name = "menuFile_SaveAs";
            this.menuFile_SaveAs.Size = new System.Drawing.Size(189, 22);
            this.menuFile_SaveAs.Text = "名前を付けて保存";
            this.menuFile_SaveAs.Click += new System.EventHandler(this.menuFile_SaveAs_Click);
            // 
            // menuFile_Close
            // 
            this.menuFile_Close.Name = "menuFile_Close";
            this.menuFile_Close.Size = new System.Drawing.Size(189, 22);
            this.menuFile_Close.Text = "閉じる";
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
            this.menuFile_Encoding.Size = new System.Drawing.Size(189, 22);
            this.menuFile_Encoding.Text = "文字コード";
            this.menuFile_Encoding.Click += new System.EventHandler(this.menuFile_Encoding_Click);
            // 
            // menuFile_Encoding_ShiftJIS
            // 
            this.menuFile_Encoding_ShiftJIS.Name = "menuFile_Encoding_ShiftJIS";
            this.menuFile_Encoding_ShiftJIS.Size = new System.Drawing.Size(180, 22);
            this.menuFile_Encoding_ShiftJIS.Text = "Shift JIS";
            this.menuFile_Encoding_ShiftJIS.Click += new System.EventHandler(this.menuFile_Encoding_Sub);
            // 
            // menuFile_Encoding_UTF8
            // 
            this.menuFile_Encoding_UTF8.Name = "menuFile_Encoding_UTF8";
            this.menuFile_Encoding_UTF8.Size = new System.Drawing.Size(180, 22);
            this.menuFile_Encoding_UTF8.Text = "UTF-8";
            this.menuFile_Encoding_UTF8.Click += new System.EventHandler(this.menuFile_Encoding_Sub);
            // 
            // menuFile_Encoding_UTF8_BOM
            // 
            this.menuFile_Encoding_UTF8_BOM.Name = "menuFile_Encoding_UTF8_BOM";
            this.menuFile_Encoding_UTF8_BOM.Size = new System.Drawing.Size(180, 22);
            this.menuFile_Encoding_UTF8_BOM.Text = "UTF-8 BOM付き";
            this.menuFile_Encoding_UTF8_BOM.Click += new System.EventHandler(this.menuFile_Encoding_Sub);
            // 
            // menuFile_Encoding_UTF16_LE_BOM
            // 
            this.menuFile_Encoding_UTF16_LE_BOM.Name = "menuFile_Encoding_UTF16_LE_BOM";
            this.menuFile_Encoding_UTF16_LE_BOM.Size = new System.Drawing.Size(180, 22);
            this.menuFile_Encoding_UTF16_LE_BOM.Text = "UTF-16 BOM (LE)";
            this.menuFile_Encoding_UTF16_LE_BOM.Click += new System.EventHandler(this.menuFile_Encoding_Sub);
            // 
            // menuFile_Encoding_UTF16_BE_BOM
            // 
            this.menuFile_Encoding_UTF16_BE_BOM.Name = "menuFile_Encoding_UTF16_BE_BOM";
            this.menuFile_Encoding_UTF16_BE_BOM.Size = new System.Drawing.Size(180, 22);
            this.menuFile_Encoding_UTF16_BE_BOM.Text = "UTF-16 BOM (BE)";
            this.menuFile_Encoding_UTF16_BE_BOM.Click += new System.EventHandler(this.menuFile_Encoding_Sub);
            // 
            // menuFile_Encoding_EucJp
            // 
            this.menuFile_Encoding_EucJp.Name = "menuFile_Encoding_EucJp";
            this.menuFile_Encoding_EucJp.Size = new System.Drawing.Size(180, 22);
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
            this.menuFile_NewLine.Size = new System.Drawing.Size(189, 22);
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
            // menuFile_End
            // 
            this.menuFile_End.Name = "menuFile_End";
            this.menuFile_End.Size = new System.Drawing.Size(189, 22);
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
            this.menuEdit_Undo.Click += new System.EventHandler(this.menuEdit_Undo_Click);
            // 
            // menuEdit_Redo
            // 
            this.menuEdit_Redo.Name = "menuEdit_Redo";
            this.menuEdit_Redo.Size = new System.Drawing.Size(117, 22);
            this.menuEdit_Redo.Text = "やり直す";
            this.menuEdit_Redo.Click += new System.EventHandler(this.menuEdit_Redo_Click);
            // 
            // menuEdit_Cut
            // 
            this.menuEdit_Cut.Name = "menuEdit_Cut";
            this.menuEdit_Cut.Size = new System.Drawing.Size(117, 22);
            this.menuEdit_Cut.Text = "切り取り";
            this.menuEdit_Cut.Click += new System.EventHandler(this.menuEdit_Cut_Click);
            // 
            // menuEdit_Copy
            // 
            this.menuEdit_Copy.Name = "menuEdit_Copy";
            this.menuEdit_Copy.Size = new System.Drawing.Size(117, 22);
            this.menuEdit_Copy.Text = "コピー";
            this.menuEdit_Copy.Click += new System.EventHandler(this.menuEdit_Copy_Click);
            // 
            // menuEdit_Paste
            // 
            this.menuEdit_Paste.Name = "menuEdit_Paste";
            this.menuEdit_Paste.Size = new System.Drawing.Size(117, 22);
            this.menuEdit_Paste.Text = "貼り付け";
            this.menuEdit_Paste.Click += new System.EventHandler(this.menuEdit_Paste_Click);
            // 
            // menuEdit_Delete
            // 
            this.menuEdit_Delete.Name = "menuEdit_Delete";
            this.menuEdit_Delete.Size = new System.Drawing.Size(117, 22);
            this.menuEdit_Delete.Text = "削除";
            this.menuEdit_Delete.Click += new System.EventHandler(this.menuEdit_Delete_Click);
            // 
            // menuSearch
            // 
            this.menuSearch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSearch_Search,
            this.menuSearch_SearchForward,
            this.menuSearch_SearchBackward,
            this.menuSearch_ReplaceForward,
            this.menuSearch_ReplaceBackward});
            this.menuSearch.Name = "menuSearch";
            this.menuSearch.Size = new System.Drawing.Size(43, 20);
            this.menuSearch.Text = "検索";
            // 
            // menuSearch_Search
            // 
            this.menuSearch_Search.Name = "menuSearch_Search";
            this.menuSearch_Search.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.menuSearch_Search.Size = new System.Drawing.Size(199, 22);
            this.menuSearch_Search.Text = "検索";
            this.menuSearch_Search.Click += new System.EventHandler(this.menuSearch_Search_Click);
            // 
            // menuSearch_SearchForward
            // 
            this.menuSearch_SearchForward.Name = "menuSearch_SearchForward";
            this.menuSearch_SearchForward.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.menuSearch_SearchForward.Size = new System.Drawing.Size(199, 22);
            this.menuSearch_SearchForward.Text = "前方検索";
            this.menuSearch_SearchForward.Click += new System.EventHandler(this.menuSearch_SearchForward_Click);
            // 
            // menuSearch_SearchBackward
            // 
            this.menuSearch_SearchBackward.Name = "menuSearch_SearchBackward";
            this.menuSearch_SearchBackward.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F3)));
            this.menuSearch_SearchBackward.Size = new System.Drawing.Size(199, 22);
            this.menuSearch_SearchBackward.Text = "後方検索";
            this.menuSearch_SearchBackward.Click += new System.EventHandler(this.menuSearch_SearchBackward_Click);
            // 
            // menuSearch_ReplaceForward
            // 
            this.menuSearch_ReplaceForward.Name = "menuSearch_ReplaceForward";
            this.menuSearch_ReplaceForward.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F3)));
            this.menuSearch_ReplaceForward.Size = new System.Drawing.Size(199, 22);
            this.menuSearch_ReplaceForward.Text = "前方置換";
            this.menuSearch_ReplaceForward.Click += new System.EventHandler(this.menuSearch_ReplaceForward_Click);
            // 
            // menuSearch_ReplaceBackward
            // 
            this.menuSearch_ReplaceBackward.Name = "menuSearch_ReplaceBackward";
            this.menuSearch_ReplaceBackward.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F3)));
            this.menuSearch_ReplaceBackward.Size = new System.Drawing.Size(199, 22);
            this.menuSearch_ReplaceBackward.Text = "後方置換";
            this.menuSearch_ReplaceBackward.Click += new System.EventHandler(this.menuSearch_ReplaceBackward_Click);
            // 
            // menuMacro
            // 
            this.menuMacro.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMacro_StartRec,
            this.menuMacro_Play,
            this.menuMacro_List,
            this.menuMacro_Set});
            this.menuMacro.Name = "menuMacro";
            this.menuMacro.Size = new System.Drawing.Size(46, 20);
            this.menuMacro.Text = "マクロ";
            // 
            // menuMacro_StartRec
            // 
            this.menuMacro_StartRec.Name = "menuMacro_StartRec";
            this.menuMacro_StartRec.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.menuMacro_StartRec.Size = new System.Drawing.Size(222, 22);
            this.menuMacro_StartRec.Text = "キー操作の記録開始・終了";
            this.menuMacro_StartRec.Click += new System.EventHandler(this.menuMacro_StartRec_Click);
            // 
            // menuMacro_Play
            // 
            this.menuMacro_Play.Name = "menuMacro_Play";
            this.menuMacro_Play.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.menuMacro_Play.Size = new System.Drawing.Size(222, 22);
            this.menuMacro_Play.Text = "キー操作の再生";
            this.menuMacro_Play.Click += new System.EventHandler(this.menuMacro_Play_Click);
            // 
            // menuMacro_List
            // 
            this.menuMacro_List.Name = "menuMacro_List";
            this.menuMacro_List.Size = new System.Drawing.Size(222, 22);
            this.menuMacro_List.Text = "キー操作の出力";
            this.menuMacro_List.Click += new System.EventHandler(this.menuMacro_List_Click);
            // 
            // menuMacro_Set
            // 
            this.menuMacro_Set.Name = "menuMacro_Set";
            this.menuMacro_Set.Size = new System.Drawing.Size(222, 22);
            this.menuMacro_Set.Text = "キー操作の登録";
            this.menuMacro_Set.Click += new System.EventHandler(this.menuMacro_Set_Click);
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
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolFile_New,
            this.toolFile_Open,
            this.toolFile_Save});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(869, 27);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolFile_New
            // 
            this.toolFile_New.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolFile_New.Image = ((System.Drawing.Image)(resources.GetObject("toolFile_New.Image")));
            this.toolFile_New.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolFile_New.Name = "toolFile_New";
            this.toolFile_New.Size = new System.Drawing.Size(24, 24);
            this.toolFile_New.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.toolFile_New.ToolTipText = "新規作成";
            this.toolFile_New.Click += new System.EventHandler(this.toolFile_New_Click);
            // 
            // toolFile_Open
            // 
            this.toolFile_Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolFile_Open.Image = ((System.Drawing.Image)(resources.GetObject("toolFile_Open.Image")));
            this.toolFile_Open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolFile_Open.Name = "toolFile_Open";
            this.toolFile_Open.Size = new System.Drawing.Size(24, 24);
            this.toolFile_Open.ToolTipText = "開く";
            this.toolFile_Open.Click += new System.EventHandler(this.toolFile_Open_Click);
            // 
            // toolFile_Save
            // 
            this.toolFile_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolFile_Save.Image = ((System.Drawing.Image)(resources.GetObject("toolFile_Save.Image")));
            this.toolFile_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolFile_Save.Name = "toolFile_Save";
            this.toolFile_Save.Size = new System.Drawing.Size(24, 24);
            this.toolFile_Save.Text = "toolStripButton1";
            this.toolFile_Save.ToolTipText = "上書き保存";
            this.toolFile_Save.Click += new System.EventHandler(this.toolFile_Save_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // txtMain
            // 
            this.txtMain.ContextMenuStrip = this.popupMenuForTextbox;
            this.txtMain.Location = new System.Drawing.Point(74, 79);
            this.txtMain.Name = "txtMain";
            this.txtMain.Size = new System.Drawing.Size(262, 171);
            this.txtMain.TabIndex = 3;
            this.txtMain.Text = "";
            this.txtMain.ModifiedChanged += new System.EventHandler(this.txtMain_ModifiedChanged);
            this.txtMain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMain_KeyDown);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 344);
            this.Controls.Add(this.txtMain);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuBar;
            this.Name = "MainForm";
            this.Text = "メモ帳";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.popupMenuForTextbox.ResumeLayout(false);
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
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
        private System.Windows.Forms.ToolStripMenuItem menuFile_Open_Encoding;
        private System.Windows.Forms.ToolStripMenuItem menuFile_Open_ShiftJIS;
        private System.Windows.Forms.ToolStripMenuItem menuFile_Open_UTF8;
        private System.Windows.Forms.ToolStripMenuItem menuFile_Open_UTF8_BOM;
        private System.Windows.Forms.ToolStripMenuItem menuFile_Open_UTF16_LE_BOM;
        private System.Windows.Forms.ToolStripMenuItem menuFile_Open_UTF16_BE_BOM;
        private System.Windows.Forms.ToolStripMenuItem menuFile_Open_EucJp;
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
        private System.Windows.Forms.ContextMenuStrip popupMenuForTextbox;
        private System.Windows.Forms.ToolStripMenuItem popupMenuForTextbox_Cut;
        private System.Windows.Forms.ToolStripMenuItem popupMenuForTextbox_Copy;
        private System.Windows.Forms.ToolStripMenuItem popupMenuForTextbox_Paste;
        private System.Windows.Forms.ToolStripMenuItem menuSearch;
        private System.Windows.Forms.ToolStripMenuItem menuSearch_SearchForward;
        private System.Windows.Forms.ToolStripMenuItem menuSearch_SearchBackward;
        private System.Windows.Forms.ToolStripMenuItem menuSearch_ReplaceForward;
        private System.Windows.Forms.ToolStripMenuItem menuSearch_ReplaceBackward;
        private System.Windows.Forms.ToolStripMenuItem menuSearch_Search;
        private Common.Controls.TextBoxEx txtMain;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuMacro;
        private System.Windows.Forms.ToolStripMenuItem menuMacro_StartRec;
        private System.Windows.Forms.ToolStripMenuItem menuMacro_Play;
        private System.Windows.Forms.ToolStripMenuItem menuMacro_List;
        private System.Windows.Forms.ToolStripMenuItem menuMacro_Set;
        private System.Windows.Forms.ToolStripMenuItem popupMenuForTextbox_SetKeyMacro;
        private System.Windows.Forms.ToolStripMenuItem menuFile_Open_Binary;
    }
}