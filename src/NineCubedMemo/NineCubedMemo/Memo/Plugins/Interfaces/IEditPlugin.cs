using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Interfaces
{
    public interface IEditPlugin
    {
        void Cut();
        void Copy();
        void Paste();
        void Delete();
        void Undo();
        void Redo();
    }
}
