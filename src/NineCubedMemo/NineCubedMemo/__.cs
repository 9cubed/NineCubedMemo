using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed
{
    /// <summary>
    /// ユーティリティークラス
    /// </summary>
    public class __
    {
        /// <summary>
        /// 処理速度を計測します。
        /// 
        /// 計測する処理を Invoke() で呼び出しているため、軽い処理の計測を行う場合は、
        /// 呼び出し処理自体が計測結果に影響を与える可能性があるため要注意。
        /// 
        /// (使用例1)
        /// long time = __.stopWatch(() => {
        ///     //計測する処理
        /// }, 10000);
        /// 
        /// (使用例2)
        /// var sb = new StringBuilder ();
        /// long time = __.stopWatch(() => {
        ///     //計測する処理
        ///     sb.Append("a");
        /// 
        /// }, 10000, 10,  () => {
        ///     //初期化処理
        ///     Console.WriteLine("初期化処理");
        ///     sb = new StringBuilder();
        /// 
        /// },  () => {
        ///     //後処理
        ///     Console.WriteLine("後処理");
        /// });
        /// 
        /// </summary>
        /// <param name="act"></param>
        /// <param name="loopCount">処理を繰り返す回数</param>
        /// <param name="tryCount">計測する回数</param>
        /// <param name="startAct">初期化処理</param>
        /// <param name="endAct">後処理</param>
        /// <returns></returns>
        public static long stopWatch(Action act, long loopCount, int tryCount = 1, Action startAct = null, Action endAct = null) {
            var watch = new Stopwatch();
            long sumTime = 0;
            for (int i = 0; i < tryCount; i++) {

                //初期化処理
                startAct?.Invoke();

                //計測開始
                watch.Restart();

                //計測対象処理の実行
                for (int j = 0; j < loopCount; j++) {
                    act.Invoke();
                }

                //計測停止
                watch.Stop();

                //解放処理
                endAct?.Invoke();

                //計測結果の取得
                long time = watch.ElapsedMilliseconds;
                sumTime += time;
            }

            //平均値を返す
            return sumTime / tryCount;
        }

        /// <summary>
        /// アプリケーションのフォルダのパスを返します
        /// </summary>
        /// <returns></returns>
        public static string GetAppDirPath()
        {
            //return Application.StartupPath;
            return AppDomain.CurrentDomain.BaseDirectory;
        }

    } //class
}
