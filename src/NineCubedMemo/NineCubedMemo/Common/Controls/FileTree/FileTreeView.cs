using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Common.Controls.FileTree
{
    public class FileTreeView : TreeView
    {
        //特殊フォルダの表示の有無
        public bool SpecialFolderNodeVisible { get; set; } = true;

        //ドライブノードの表示の有無
        public bool DriveNodeVisible         { get; set; } = true;

        /// <summary>
        /// イメージリストのキー
        /// </summary>
        public enum FileImageKey {
            ROOT,          //ルート
            HDD,           //ハードディスク
            CD,            //CD
            REMOVABLE,     //リムーバブルディスク
            FOLDER,        //フォルダ
            CLOSED_FOLDER, //閉じているフォルダ
            OPENED_FOLDER, //開いているフォルダ
            SPCIAL_FOLDER, //特別なフォルダ
            FILE           //ファイル
        }

        /// <summary>
        /// ルートノードプロパティ
        /// </summary>
        public TreeNode RootNode {
            get {
                return this.Nodes[0];
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileTreeView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FileTreeView
            // 
            this.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.FileTreeView_BeforeCollapse);
            this.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.FileTreeView_BeforeExpand);
            this.ResumeLayout(false);
        }

        /// <summary>
        /// ノードの画像イメージを設定します
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="imgFolderOpened"></param>
        /// <param name="imgFolderClosed"></param>
        /// <param name="imgRoot"></param>
        /// <param name="imgCd"></param>
        /// <param name="imgHdd"></param>
        /// <param name="imgRemovable"></param>
        public void SetImage(
            int width,
            int height,
            Image imgFolderOpened,
            Image imgFolderClosed,
            Image imgRoot,
            Image imgCd,
            Image imgHdd,
            Image imgRemovable) {

            this.ImageList = new ImageList();
            this.ImageList.ImageSize = new Size(width, height);
            this.ImageList.Images.Add(FileTreeView.FileImageKey.OPENED_FOLDER.ToString(), imgFolderOpened);
            this.ImageList.Images.Add(FileTreeView.FileImageKey.CLOSED_FOLDER.ToString(), imgFolderClosed);
            this.ImageList.Images.Add(FileTreeView.FileImageKey.ROOT.ToString(),          imgRoot);
            this.ImageList.Images.Add(FileTreeView.FileImageKey.CD.ToString(),            imgCd);
            this.ImageList.Images.Add(FileTreeView.FileImageKey.HDD.ToString(),           imgHdd);
            this.ImageList.Images.Add(FileTreeView.FileImageKey.REMOVABLE.ToString(),     imgRemovable);
        }

        /// <summary>
        /// 初期化処理
        /// ファイルツリービューを使用する際に必ず呼び出す必要があります。
        /// </summary>
        public void Initialize()
        {
            //ルートの線を非表示にします
            this.ShowRootLines = false;

            //ルートノードの追加
            var rootNode = this.Nodes.Add(
                ".", //キー
                "",  //テキスト
                FileImageKey.ROOT.ToString(), //imageKey
                FileImageKey.ROOT.ToString()  //selectedImageKey
            );

            //特別なフォルダノードの追加
            if (this.SpecialFolderNodeVisible) {
                AddSpecialFolderNode();
            }
            
            //ドライブノードの追加
            if (this.DriveNodeVisible) {
                AddDriveNode();
            }
            
            //ルートノードを開く
            this.RootNode.Expand();
        }

        /// <summary>
        /// 特殊フォルダノードの追加
        /// </summary>
        private void AddSpecialFolderNode() {
            AddNode(Environment.SpecialFolder.Desktop    , "デスクトップ");
            AddNode(Environment.SpecialFolder.MyDocuments, "マイドキュメント");
            AddNode(Environment.SpecialFolder.MyPictures , "マイピクチャー");
            return;

            //ノードを追加します
            void AddNode(Environment.SpecialFolder kind, String text) {

                //特殊フォルダのパスを取得します
                var path = Environment.GetFolderPath(kind);

                //フォルダノードを作成します
                var node = new DirTreeNode(path, text);
                this.RootNode.Nodes.Add(node);

                //ダミーノードを追加します
                AddDummyNode(node, path);
            }
        }

        /// <summary>
        /// ツリーにドライブノードを追加します。
        /// </summary>
        private void AddDriveNode() {
            //ドライブ一覧を取得します
            foreach (var driveInfo in DriveInfo.GetDrives()) {
                //ドライブノードを追加します
                var node = new DriveTreeNode(driveInfo);
                this.RootNode.Nodes.Add(node);

                //ダミーノードを追加します
                String path = driveInfo.Name;
                AddDummyNode(node, path);
            }
        }

        /// <summary>
        /// 指定したノード配下にフォルダがない場合は、ダミーノードを追加します
        /// 
        /// 子ノードがない場合は「＋」の記号がつかず、ノードを開くことができないため、
        /// ダミーノードを追加しておきます。
        /// そして、ノードが開かれた際に、ダミーノードを削除して、
        /// 代わりにフォルダ一覧を取得して、フォルダをノードとして追加します。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dirPath"></param>
        private void AddDummyNode(TreeNode node, String dirPath) {
            try {
                //追加したノードの下にフォルダがあるか？
                if (FileUtils.ExistSubDir(dirPath) == true) {
                    //さらに下の階層にフォルダがあった場合
                    //ダミーノードを追加します
                    node.Nodes.Add(new DummyTreeNode());
                }
            } catch (Exception) {
                //アクセスできないパスは無視する
                Console.WriteLine("アクセスできないため無視します。" + dirPath);
            }
        }

        /// <summary>
        /// ノードが開く直前のイベント
        /// 開くノードにダミーノードがある場合は、ダミーノードを削除してフォルダノードを追加する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            //開かれたノードを取得します
            var expandNode = e.Node;

            //開かれたノード配下のノード数が0の場合は処理を抜けます
            if (expandNode.Nodes.Count == 0) return;
        
            //開かれたノードがフォルダの場合はノードの画像を変更します
            if (expandNode is DirTreeNode node) {
                node.SetOpenFolderImage();
            }

            //ダミーノードかどうか？
            if (expandNode.FirstNode is DummyTreeNode) {
                //ダミーの場合

                //ダミーノードを削除します
                expandNode.FirstNode.Remove();

                //パスを取得します
                var path = GetPath(expandNode);

                //フォルダ一覧を取得します
                IList<string> dirList = FileUtils.GetDirList(path);

                //フォルダ一覧のフォルダを１つずつノードとして追加します
                foreach (var dirPath in dirList) {
                    //パスからファイル名を取得します
                    var dirName = Path.GetFileName(dirPath);

                    //フォルダノードを作成します
                    var dirNode = new DirTreeNode(dirName, dirName);

                    //フォルダノードを追加します
                    expandNode.Nodes.Add(dirNode);

                    //追加したノード配下にダミーノードを追加します
                    AddDummyNode(dirNode, dirPath);
                }
            }

        }

        /// <summary>
        /// ノードが閉じる直前のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            //閉じらたれノードがフォルダの場合はノードの画像を変更します
            if (e.Node is DirTreeNode node) node.SetCloseFolderImage();
        }

        /// <summary>
        /// 指定されたノードのパスを取得します
        /// </summary>
        /// <param name="node">ノード</param>
        /// <returns>パス</returns>
        public string GetPath(TreeNode node) {
            if (node == null) return null;

            string path = node.Name;

            //子ノードからルートノードに向かって遡って結合していく
            while (node.Parent != null && node.Parent != this.RootNode) {
                node = node.Parent;
                path = FileUtils.AppendPath(node.Name, path); //パスの先頭に親ノードの名前を追加します
            }

            return path;
        }


    } //class
}
