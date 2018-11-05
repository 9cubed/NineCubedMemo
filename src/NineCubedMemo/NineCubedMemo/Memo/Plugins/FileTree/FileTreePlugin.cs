﻿using NineCubed.Common.Controls.FileTree;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.FileTree
{
    public class FileTreePlugin : FileTreeView, IPlugin
    {
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FileTreePlugin
            // 
            this.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FileTreePlugin_AfterSelect);
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

            this.Dock = DockStyle.Fill;

            //ファイルツリービューの設定
            {
                //ノード用の画像の読み込み
                var imgDirPath = "resource/file";
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
                var menu = new ToolStripMenuItem("テスト");
                popupMenu.Items.Add(menu);
                menu.Click += (sender, e) => {
                    //クリックされた時の処理を書きます
                };
            }
            this.ContextMenuStrip = popupMenu;

            return true;
        }
        public void      InitializePlaced() { }                         //プラグイン配置後の初期化処理を行います
        private PluginManager _pluginManager = null;                    //プラグインマネージャー
        public Component GetComponent()   { return this; }              //プラグインのコンポーネントを返します
        public string    Title            { get; set; }                 //プラグインのタイトル
        public bool      CanClosePlugin() { return true; }              //プラグインが終了できるかどうか
        public void      ClosePlugin()    { Parent = null; Dispose(); } //プラグインの終了処理
        public void      SetFocus()       { this.SetFocus(); }          //フォーカスを設定します

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
            //選択されたノードのパスを取得します
            var path = this.GetPath(e.Node);
            
            //フォルダ選択イベントを発生させます
            var param = new DirSelectedEventParam { Path = path };
            _pluginManager.GetEventManager().RaiseEvent(DirSelectedEventParam.Name,  null, param);
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
            //クリックされた位置の近くにあるノードを取得します
            var node = this.GetNodeAt(e.X, e.Y);

            //ノードを選択状態にします
            this.SelectedNode = node;
        }


        /******************************************************************************
         * 
         *  プラグイン用イベントハンドラー
         * 
         ******************************************************************************/




    } //class
}