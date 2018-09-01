using NineCubed.Common.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Interfaces
{
    public interface IFilePlugin
    {
        /// <summary>
        /// テキストファイルデータ
        /// </summary>
        IFile TargetFile { get; set; }

        /// <summary>
        /// ファイルを開きます
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        bool OpenFile(IFile file); 

        /// <summary>
        /// ファイルを保存します。
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        bool SaveFile(IFile file = null);
    }
}
