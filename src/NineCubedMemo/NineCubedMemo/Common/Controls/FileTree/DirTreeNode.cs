using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Common.Controls.FileTree
{
    public class DirTreeNode : TreeNode
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        public DirTreeNode(String name, String text) {
            this.Name = name;
            this.Text = text;
            SetCloseFolderImage(); //画像の設定
        }

        //ノードの画像を開いたフォルダにします
        public void SetOpenFolderImage() {
            this.ImageKey         = FileTreeView.FileImageKey.OPENED_FOLDER.ToString();
            this.SelectedImageKey = FileTreeView.FileImageKey.OPENED_FOLDER.ToString();
        }

        //ノードの画像を閉じたフォルダにします
        public void SetCloseFolderImage() {
            this.ImageKey         = FileTreeView.FileImageKey.CLOSED_FOLDER.ToString(); 
            this.SelectedImageKey = FileTreeView.FileImageKey.CLOSED_FOLDER.ToString();
        }
        
    }
}
