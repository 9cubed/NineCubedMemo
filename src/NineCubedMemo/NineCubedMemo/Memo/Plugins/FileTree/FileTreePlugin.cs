﻿using NineCubed.Common.Controls.FileTree;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.FileTree
{
    public class FileTreePlugin : FileTreeView, IPlugin, IRefreshPlugin, IPathPlugin
    {
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FileTreePlugin
            // 
            this.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FileTreePlugin_AfterSelect);
            this.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.FileTreePlugin_NodeMouseClick);
            this.Enter += new System.EventHandler(this.FileTreePlugin_Enter);
            this.Leave += new System.EventHandler(this.FileTreePlugin_Leave);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FileTreePlugin_MouseDown);
            this.ResumeLayout(false);

        }

        public FileTreePlugin() {
            InitializeComponent();
        }

        /******************************************************************************
         * 
         *  IPlugin
         * 
         ******************************************************************************/ 

        //初期処理を行います
        public bool Initialize(PluginCreateParam param)
        {
            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //ファイルツリービューの設定
            {
                //ノード用の画像の読み込み
                var imgDirPath = FileUtils.AppendPath(param.DataPath, "img");
                var imgOpenedFolder  = Image.FromFile(FileUtils.AppendPath(imgDirPath, "opened_folder.png"));
                var imgClosedFolder  = Image.FromFile(FileUtils.AppendPath(imgDirPath, "closed_folder.png"));
                var imgRoot          = Image.FromFile(FileUtils.AppendPath(imgDirPath, "root.png"));
                var imgCd            = Image.FromFile(FileUtils.AppendPath(imgDirPath, "cd.png"));
                var imgHdd           = Image.FromFile(FileUtils.AppendPath(imgDirPath, "hdd.png"));
                var imgRemovable     = Image.FromFile(FileUtils.AppendPath(imgDirPath, "removable.png"));

                //ノード用の画像の設定
                SetImage(20, 20, imgOpenedFolder, imgClosedFolder, imgRoot, imgCd, imgHdd, imgRemovable);

                //ファイルツリービューを初期化します
                Initialize();
                this.Font = new Font("ＭＳゴシック", 9);
            }

            //ポップアップメニューを設定します
            var popupMenu = new ContextMenuStrip();
            {
                var menu = new ToolStripMenuItem("最新の情報に更新");
                popupMenu.Items.Add(menu);
                menu.Click += (sender, e) => {
                    this.Refresh(this.SelectedNode);
                };
            }
            {
                var menu = new ToolStripMenuItem("フォルダを開く");
                popupMenu.Items.Add(menu);
                menu.Click += (sender, e) => {
                    //選択されているノードのパスを取得します
                    var path = GetPath(this.SelectedNode);
                    if (string.IsNullOrEmpty(path)) return;

                    //ネイティブな方法でフォルダを開きます
                    Process.Start(path);
                };
            }
            this.ContextMenuStrip = popupMenu;

            return true;
        }
        //プラグイン配置後の初期化処理を行います
        public void InitializePlaced() {
            this.Dock = DockStyle.Fill;
            this.BringToFront();
        } 
        private PluginManager _pluginManager = null;                    //プラグインマネージャー
        public string     PluginId         { get; set; }                 //プラグインID
        public IPlugin    ParentPlugin     { get; set; }                 //親プラグイン
        public IComponent GetComponent()   { return this; }              //プラグインのコンポーネントを返します
        public string     Title            { get; set; }                 //プラグインのタイトル
        public bool       CanClosePlugin() { return true; }              //プラグインが終了できるかどうか
        public void       ClosePlugin()    { Parent = null; Dispose(); } //プラグインの終了処理

        //フォーカスを設定します
        public void SetFocus() {
            this.Focus();

            //アクティブプラグインにします
            _pluginManager.ActivePlugin = this;
        }

        /// <summary>
        /// ノードが選択された時に呼ばれます
        ///   FileTreePlugin_AfterSelect()
        ///   FileTreePlugin_NodeMouseClick()
        /// </summary>
        private void NodeSelected(TreeNode node)
        {
            //選択されたノードのパスを取得します
            var path = this.GetPath(node);
            
            if (path.Equals(_oldSelectedPath) == false) {
                //フォルダ選択イベントを発生させます
                var param = new DirSelectedEventParam { Path = path };
                _pluginManager.GetEventManager().RaiseEvent(DirSelectedEventParam.Name, this, param);

                //選択されたパスを保持します
                _oldSelectedPath = path;
            }
        }

        /// <summary>
        /// 前回選択されたノードのパス
        /// ノード選択時に設定されます。
        /// また、ファイルツリーからフォーカスがはずれた時にクリアされます。
        /// </summary>
        private string _oldSelectedPath;

        /******************************************************************************
         * 
         *  IRefreshPlugin
         * 
         ******************************************************************************/ 

        /// <summary>
        /// 最新の情報に更新します
        /// </summary>
        public void RefreshData()
        {
            this.Refresh(this.SelectedNode);
        }

        /******************************************************************************
         * 
         *  IPathPlugin
         * 
         ******************************************************************************/

        /// <summary>
        /// パスを返します
        /// </summary>
        /// <returns></returns>
        public string GetPath() => GetPath(this.SelectedNode);

        /******************************************************************************
         * 
         *  ファイルツリービューのイベント
         * 
         ******************************************************************************/

        /// <summary>
        /// ノード選択直後に発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileTreePlugin_AfterSelect(object sender, TreeViewEventArgs e)
        {
            NodeSelected(e.Node);
        }

        /// <summary>
        /// ノードクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileTreePlugin_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            NodeSelected(e.Node);
        }

        /// <summary>
        /// フォーカスを失った時のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileTreePlugin_Leave(object sender, EventArgs e)
        {
            //選択されたパスをクリアします
            _oldSelectedPath = null;
        }

        /// <summary>
        /// マウスダウンイベント
        /// 右クリックした場合に自動的にノードが選択されないため、
        /// クリックされた位置の近くにあるノードを選択します
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileTreePlugin_MouseDown(object sender, MouseEventArgs e)
        {
            /*
            //クリックされた位置の近くにあるノードを取得します
            var node = this.GetNodeAt(e.X, e.Y);

            //ノードを選択状態にします
            this.SelectedNode = node;
            */
        }

        /// <summary>
        /// アクティブになった時に発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileTreePlugin_Enter(object sender, EventArgs e)
        {
            _pluginManager.ActivePlugin = this;
        }


        /******************************************************************************
         * 
         *  プラグイン用イベントハンドラー
         * 
         ******************************************************************************/

        /// <summary>
        /// プラグイン生成イベント
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sender"></param>
        public void PluginEvent_PluginCreated(EventParam param, object sender)
        {
            //生成されたプラグインを取得します
            var plugin = ((PluginCreatedEventParam)param).Plugin;

            //生成されたプラグインのコンポーネントがメニューの場合
            var component = plugin.GetComponent();
            if (component is ToolStripMenuItem) {
                //メニューを追加します
                this.ContextMenuStrip.Items.Add((ToolStripMenuItem)component);

                //イベントを処理済みにします
                param.Handled = true;
            }
        }

    } //class
}
