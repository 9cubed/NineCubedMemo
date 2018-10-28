using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Common.Controls.FileTree
{
    public class DriveTreeNode : TreeNode
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="driveInfo"></param>
        public DriveTreeNode(DriveInfo driveInfo) {
            //ドライブのイメージキーを設定します
            var imageKey = FileTreeView.FileImageKey.HDD.ToString();
            if (driveInfo.DriveType == DriveType.CDRom)     imageKey = FileTreeView.FileImageKey.CD.ToString();
            if (driveInfo.DriveType == DriveType.Removable) imageKey = FileTreeView.FileImageKey.REMOVABLE.ToString();
                
            //ボリュームラベルを取得します
            var volumeLabel = "";
            if (driveInfo.IsReady) {
                if (driveInfo.VolumeLabel.Trim().Length >= 1) {
                    volumeLabel = "(" + driveInfo.VolumeLabel.Trim() + ")";
                }
            }
            
            this.Name = driveInfo.Name.Trim('\\');    //キー
            this.Text = driveInfo.Name + volumeLabel; //テキスト
            this.ImageKey         = imageKey;
            this.SelectedImageKey = imageKey;
        }

    } //class
}
