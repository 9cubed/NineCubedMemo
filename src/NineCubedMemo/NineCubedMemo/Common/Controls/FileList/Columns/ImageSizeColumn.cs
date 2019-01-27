using NineCubed.Common.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Common.Controls.FileList.Columns
{
    public class ImageSizeColumn : AFileListColumn
    {
        public ImageSizeColumn() : base()
        {
            this.ReadOnly = true;
            this.HeaderText = "画像サイズ";
        }

        /// <summary>
        /// データ取得用スレッド
        /// </summary>
        Thread _thread;

        /// <summary>
        /// データ取得用スレッドの取得対象データのキュー
        /// </summary>
        ConcurrentQueue<FileInfo> _queue;

        /// <summary>
        /// 取得したデータ
        /// マップ[ ファイル名 ] = 表示する値
        /// </summary>
        ConcurrentDictionary<string, string> _cache;

        //初期化します
        public override void ShowFileListBefore() {
            _cache = new ConcurrentDictionary<string, string>();
            _queue = new ConcurrentQueue<FileInfo>();
        }

        //表示する値を返します
        override
        public string ToString(FileInfo file) {

            //ファイルでない場合は、空文字を返します
            if (FileUtils.IsFile(file.FullName) == false) return "";

            //画像ファイルでない場合は、空文字を返します
            if (ImageUtils.IsImageFile(file.Name) == false) return "";

            //キャッシュにある場合は、キャッシュの値を返します
            if (_cache.TryGetValue(file.FullName, out string value)) {
                return value;
            }

            //別スレッドで処理するキューに、処理対象のファイルを追加します
            _queue.Enqueue(file);

            //動いているスレッドがない場合は、スレッドを生成します
            if (_thread == null || _thread.IsAlive == false) {
                //スレッドがない場合

                //画像サイズを取得して、結果をキャッシュに設定します
                _thread = new Thread(GetData);

                //スレッドを実行します
                _thread.Start();
            }

            return "";
        }

        /// <summary>
        /// 画像サイズを取得します。
        /// 別スレッドで動かします。
        /// </summary>
        private void GetData()
        {
            while (_queue.IsEmpty == false) {
                //空でない場合
                while (_queue.TryDequeue(out var targetFile)) {
                    try {
                        //画像ファイルを読み込みます
                        using(var stream = new FileStream(targetFile.FullName, FileMode.Open, FileAccess.Read))
                        using(var image = Image.FromStream(stream, false, false)) { //第3引数を false にすると速くなる
                            //キャッシュに表示する値を設定します
                            _cache[targetFile.FullName] = image.Width + "×" + image.Height;
                        }
                        
                    } catch (Exception ex) {Console.WriteLine(ex.Message + ":" + targetFile.Name); }
                }

                //キューにデータが追加されるのを少し待ちます
                //次のデータがキューに入る前に処理が終わってしまうと、
                //スレッドが破棄され、またスレッドが生成されてしまうため
                Thread.Sleep(500);
            }

            //列のデータを更新します
            ((FileListGrid)this.DataGridView).UpdateColumn(this);
        }

    } // class
}
