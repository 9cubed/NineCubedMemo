using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Common.Controls.FileTree
{
    public class SpecialTreeNode : TreeNode
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        public SpecialTreeNode(String name, String text) {
            this.Name = name;
            this.Text = text;
            this.ImageKey         = FileTreeView.FileImageKey.SPCIAL_FOLDER.ToString();
            this.SelectedImageKey = FileTreeView.FileImageKey.SPCIAL_FOLDER.ToString();
        }

    } //class
}
