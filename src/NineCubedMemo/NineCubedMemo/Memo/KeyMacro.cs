using NineCubed.Common.Controls;
using NineCubed.Memo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo
{
    public class KeyMacro
    {
        //テキストボックス
        TextBoxEx _textBox;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="textBox"></param>
        public KeyMacro(TextBoxEx textBox) {
            _textBox = textBox;

            //イベントの設定
            _textBox.ImeConverted += TextBoxEx_ImeConverted; //IMEで漢字変換が確定した時のイベント
            _textBox.KeyDown     += this.TextBoxEx_KeyDown;
            _textBox.KeyPress    += this.TextBoxEx_KeyPress;
            _textBox.KeyUp       += this.TextBoxEx_KeyUp;
        }

        //キー操作の記録中かどうか true:記録中
        public bool IsRecording = false;

        //キー操作リスト
        public IList<string> KeyList = new List<string>();

        //キー操作の記録を開始します
        public void StartRecording() {
            this.IsRecording = true;
            KeyList = new List<string>();
        }

        //キー操作の記録を停止します
        public void StopRecording() {
            this.IsRecording = false;
        }

        //キー操作を再生します
        //
        //SendKeys.Send() で {^} を送ると「&」になる問題がある
        public void Play() {
            if (this.IsRecording) return; //記録中の場合は再生しない

            //CapsLock の状態の取得
            bool isCapsLock = Control.IsKeyLocked(Keys.CapsLock);

            foreach (var key in KeyList) {
                string sendKey = key;
                try {
                    // {^} を送ると「&」になるため、全角に置換します。TODO
                    if (sendKey.Equals("{^}")) sendKey = "{＾}";

                    if (isCapsLock && sendKey.IndexOf("{") == -1) {
                        if (('z' >= key[0] && key[0] >= 'a') ||
                            ('Z' >= key[0] && key[0] >= 'A')) {
                            sendKey = "{CAPSLOCK}" + sendKey;
                        }
                    }

                    //キーを送信します
                    SendKeys.Send(sendKey);

                } catch (Exception) {
                    Console.WriteLine("ERROR:" + key);
                }
            }
        }

        /// <summary>
        /// IMEで漢字変換が確定した時のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="selectedString">漢字変換された文字列</param>
        private void TextBoxEx_ImeConverted(object sender, string selectedString)
        {
            if (this.IsRecording == false) return; //記録中でない場合は処理を抜けます

            //変換された漢字をキー操作リストに追加します
            KeyList.Add(selectedString);
        }

        //Ctrlキー の状態
        bool _isControl = false;

        /// <summary>
        /// KeyDownイベント
        /// ショートカットキーをキー操作リストに追加します。
        /// ここでは正確な文字(大文字・小文字・記号)がわからないため、文字の追加はしません。
        /// 文字は KeyPressイベントで追加します。
        /// また、KeyPressイベントでは Ctrl と Alt の状態がわからないため、
        /// KeyDown で状態を保持して、KeyPress で解放しています。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxEx_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.IsRecording == false) return; //記録中でない場合は処理を抜けます

            //Ctrlキーの状態を保持します
            _isControl = e.Control;

            //キー操作の記録中の場合は、キーをキー操作リストに追加します
            AddKey(e);
        }

        /// <summary>
        /// KeyPressイベント
        /// 入力された文字をリストに追加します
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxEx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.IsRecording == false) return; //記録中でない場合は処理を抜けます

            //Ctrl が押されていなければ、キー操作リストにキーを追加します
            if (_isControl == false) {
                string key = e.KeyChar.ToString();
                if (key == "\r") key = "{ENTER}";
                if (key == "\t") key = "{TAB}";
                if (key == "\b") key = "{BS}";

                //SendKeys.Send() の予約文字は、{ } を付けてエスケープします
                if ("^%~+(){}".IndexOf(key) >= 0) key = "{" + key + "}";
                
                //キー操作リストに追加します
                AddKey(key);
            }
        }

        /// <summary>
        /// KeyUpイベント
        /// Ctrl と Alt の状態をリセットします
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxEx_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.IsRecording == false) return; //記録中でない場合は処理を抜けます

            //Ctrlキーの状態をリセットします
            _isControl = e.Control;
        }

        //操作の記録最大キー数。超えた分は破棄する
        public int MaxKeyCount = 256;

        /// <summary>
        /// キーをリストに追加します
        /// キーイベントではなく、直接任意のキーをリストに追加する場合に使います
        /// </summary>
        /// <param name="key"></param>
        public void AddKey(string key) {
            if (KeyList.Count >= MaxKeyCount) return;
            KeyList.Add(key);
        }

        /// <summary>
        /// キーをキー操作リストに追加します
        /// 
        /// SendKeys.Send() のキー一覧
        /// https://msdn.microsoft.com/en-us/library/system.windows.forms.sendkeys.send(v=vs.110).aspx
        /// </summary>
        /// <param name="e"></param>
        private void AddKey(KeyEventArgs e) {

            string specialKey = ""; //特別なキー
            if (e.Shift)   specialKey = specialKey + "+";
            if (e.Control) specialKey = specialKey + "^";
            if (e.Alt)     specialKey = specialKey + "%";
            
            string key = "";

            if (e.KeyCode == Keys.Up)     key = "{UP}";
            if (e.KeyCode == Keys.Down)   key = "{DOWN}";
            if (e.KeyCode == Keys.Left)   key = "{LEFT}";
            if (e.KeyCode == Keys.Right)  key = "{RIGHT}";

            if (e.KeyCode == Keys.Home)   key = "{HOME}";
            if (e.KeyCode == Keys.End)    key = "{END}";
            if (e.KeyCode == Keys.Delete) key = "{DEL}";
            if (e.KeyCode == Keys.Back)   return; //KeyPress で処理する
            if (e.KeyCode == Keys.Enter)  return; //KeyPress で処理する
            if (e.KeyCode == Keys.Tab)    return; //KeyPress で処理する
            //if (e.KeyCode == Keys.F3) key = "{F3}"; //メニューのショートカットキーが処理されるため、ここには来ない

            if (e.Control && e.KeyCode == Keys.Z) key = "{Z}";
            if (e.Control && e.KeyCode == Keys.X) key = "{X}";
            if (e.Control && e.KeyCode == Keys.C) key = "{C}";
            if (e.Control && e.KeyCode == Keys.V) key = "{V}";
            if (e.Control && e.KeyCode == Keys.Y) key = "{Y}";

            if (string.IsNullOrEmpty(key) == false) {
                AddKey(specialKey + key);
            }
        }

    } //class

}
