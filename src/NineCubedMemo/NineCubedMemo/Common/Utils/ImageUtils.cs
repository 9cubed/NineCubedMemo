using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Utils
{
    public class ImageUtils
    {
        /// <summary>
        /// 画像ファイルかどうかを返します。
        /// 判定は、拡張子のみで行います。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsImageFile(string path)
        {
            var ext = Path.GetExtension(path).ToLower();
            if ((ext.EndsWith(".jpeg") || 
                 ext.EndsWith(".jpg")  || 
                 ext.EndsWith(".png")  ||
                 ext.EndsWith(".bmp")  ||
                 ext.EndsWith(".gif"))) {
                return true;  //画像ファイル
            } else {
                return false; //画像ファイル以外
            }
        }

    } //class
}
