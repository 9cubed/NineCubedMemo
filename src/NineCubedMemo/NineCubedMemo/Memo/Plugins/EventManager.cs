using NineCubed.Memo.Plugins.Events;
using NineCubed.Memo.Plugins.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins
{
    //イベントを管理するクラス
    public class EventManager
    {
        //デバッグ用 イベントの削除漏れのチェック
        public void CheckEventLeak() {
            foreach (var dict in _eventDict) {
                if (dict.Value.Count() > 0) Console.WriteLine("イベント削除漏れ。" + dict.Key.ToString());
            }
        }

        //キー:イベント名(発生側と受取側で一致していれば任意)
        //値  :イベントハンドラーリストの Dict (イベントを受け取るオブジェクトリストのマップ)
        //用途:イベント発生依頼があった場合、イベント名をキーにしてイベントハンドラーリストを取得して、
        //     各イベントハンドラーのイベントメソッドを呼ぶ際に使用します。
        private Dictionary<String, IList<IPlugin>> _eventDict = new Dictionary<String, IList<IPlugin>>();

        //イベント名をキーにして、イベントハンドラーリストを取得します
        //リストがまだない場合には、リストを生成して返します
        private IList<IPlugin> GetEventHandlerList(string eventName) {

            if (_eventDict.TryGetValue(eventName, out IList<IPlugin> list) == false) {
                //リストが生成されていない場合は、リストを生成して Dictionary に追加します
                list = new List<IPlugin>();
                _eventDict.Add(eventName, list);
            }

            return list;
        }

        /// <summary>
        /// イベントハンドラーをリストに追加します
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventHandler"></param>
        public void AddEventHandler(string eventName, IPlugin eventHandler) {
            //イベント名をキーにして、イベントハンドラーリストを取得します
            var list = GetEventHandlerList(eventName);

            //イベントハンドラーを追加します
            list.Add(eventHandler);
        }

        /// <summary>
        /// イベントハンドラーをリストから削除します
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventHandler"></param>
        public void RemoveEventHandler(string eventName, IPlugin eventHandler) {
            //イベント名をキーにして、イベントハンドラーリストを取得します
            var list = GetEventHandlerList(eventName);

            //イベントハンドラーを削除します
            list.Remove(eventHandler);
        }

        /// <summary>
        /// 指定したイベントハンドラーを全てのイベントハンドラーリストから削除します。
        /// </summary>
        /// <param name="plugin"></param>
        public void RemoveEventHandler(IPlugin eventHandler) {
            foreach (var item in _eventDict) {
                //イベントハンドラーを削除します
                var list = item.Value;
                list.Remove(eventHandler);
            }
        }

        /// <summary>
        /// イベントを発生させます
        /// 
        /// イベントハンドラーリストにあるイベントハンドラーに対しては、イベントのメソッドを実行します。
        /// 引数の EventParam.Cancel が true にされた場合、その時点でイベント処理を中止します。
        /// </summary>
        /// <param name="eventName">イベント名</param>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="param">イベントパラメーター</param>
        /// <return>イベントを処理済みにしたプラグイン</return>
        public IPlugin RaiseEvent(string eventName, object sender, EventParam param, IPlugin plugin = null) {

            //イベントを発生させるプラグインが指定されている場合は、
            //イベントハンドラーリストを使わずに、すぐにイベントメソッドを実行します
            if (plugin != null) {
                InvokeEventMethod(eventName, sender, param, plugin);
                return plugin;
            }

            //イベント名をキーにして、イベントハンドラーリストを取得します
            if (_eventDict.TryGetValue(eventName, out IList<IPlugin> tmpEventHandlerList)) {

                //イベント処理中にイベントハンドラーリストに追加されるとエラーになるため、
                //リストのコピーを作ります。イベント処理のループはこれを使います。
                var eventHandlerList = tmpEventHandlerList.ToList();

                foreach(var eventHandler in eventHandlerList) {
                    //イベントメソッドを実行します
                    InvokeEventMethod(eventName, sender, param, eventHandler);

                    //イベントがプラグインにて処理済みとなった場合には、処理を抜けます
                    if (param.Handled) return eventHandler;
                }
            }

            return null;
        }

        /// <summary>
        /// イベント用のメソッドを実行します。
        /// 引数のイベント名(eventName)を、そのままメソッド名として実行します。
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="sender"></param>
        /// <param name="param"></param>
        /// <param name="eventHandler"></param>
        private void InvokeEventMethod(string eventName, object sender, EventParam param, IPlugin eventHandler)
        {
            //TODO Console.WriteLine("■" + eventName + ":" + eventHandler.PluginId);

            //イベントの受け取り先のメソッド情報を取得します
            var methodInfo = eventHandler.GetType().GetMethod(eventName);
            if (methodInfo == null) {
                //エラー。メソッドが定義されていません
                throw new MissingMethodException(eventHandler.GetType().FullName, eventName); 
            }

            try {
                //イベントのメソッドを実行します
                object[] parameters = { param, sender }; //メソッドの第1引数、第2引数として渡される
                methodInfo.Invoke(eventHandler, parameters);
            } catch (Exception ex) {
                throw new Exception("イベントメソッド実行時にエラーが発生しました。" + eventName, ex);
            }
        }

    } //class
}
