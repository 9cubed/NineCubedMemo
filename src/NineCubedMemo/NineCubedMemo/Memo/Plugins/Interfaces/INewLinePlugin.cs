using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Interfaces
{
    //TODO TextEditorPluginでしか使わないので削除するかも。
    public interface INewLinePlugin
    {
        string NewLineCode { get; set; }
    }
}
