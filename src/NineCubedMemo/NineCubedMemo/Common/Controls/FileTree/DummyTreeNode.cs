using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Common.Controls.FileTree
{
    public class DummyTreeNode : TreeNode
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="text"></param>
        public DummyTreeNode() {
            this.Name = "dummy";
            this.Text = "**dummy**";
        }

    } //class
}
