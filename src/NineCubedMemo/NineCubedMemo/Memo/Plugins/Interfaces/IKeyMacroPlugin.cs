using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Interfaces
{
    public interface IKeyMacroPlugin
    {
        //キー操作の記録を開始します
        void StartRecording();

        //キー操作の記録を停止します
        void StopRecording();

        //キー操作を再生します
        void Play();

        //記録中かどうか？
        bool IsRecording();

        //記録しているマクロを出力します
        void OutputMacro();

        //マクロを設定します
        void SetMacro();

    }
}
