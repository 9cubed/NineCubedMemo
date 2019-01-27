using NineCubed.Common.Files;
using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace NineCubed.Memo.Plugins.Grep
{
    public class GrepThread
    {
        GrepControl _control;
        public string Path          {get; set;} //検索対象フォルダのパス
        public string Extension     {get; set;} //検索対象拡張子(カンマ区切り)
        public string Keyword       {get; set;} //検索キーワード
        public bool   SubDir        {get; set;} //true:サブフォルダを含む
        public bool   NotIgnoreCase {get; set;} //true:大文字・小文字を区別しない  false:区別する
        public bool   RegExp        {get; set;} //true:正規表現を使う

        //キャンセルフラグ trueになったらGrep処理を中止します
        public bool CancelFlg { get; set; } = false;

        //検索件数
        public int MatchCount     { get; private set; } //検索該当数(検索で引っかかった数)
        public int MatchFileCount { get; private set; } //検索該当ファイル数(検索で引っかかった数)
        public int AllFileCount   { get; private set; } //検索対象ファイル数

        //進捗状況を10ファイルにつき1回表示するためのカウンター
        private int _progressCount = 0;

        //コンストラクタ
        public GrepThread(GrepControl control, string path, string extension, string keyword, bool subDir, bool notIgnoreCase, bool regExp)
        {
            _control = control;
            this.Path       = path;
            this.Extension  = extension;
            this.Keyword    = keyword;
            this.SubDir     = subDir;
            this.NotIgnoreCase = notIgnoreCase;
            this.RegExp     = regExp;
        }

        //スレッド実行用メソッド
        public void Invoke()
        {
            Grep();
        }

        //スレッドメイン処理
        //Grepします
        private void Grep()
        {
            //検索件数をクリアします
            MatchCount = 0;
            MatchFileCount = 0;
            AllFileCount = 0;

            string msg;
            try {
                //Grepします
                FileUtils.GetFileList(CheckFile, this.Path, this.SubDir);

                msg = "完了しました。" + 
                    "該当数:" + MatchCount + " 該当ファイル数:" + MatchFileCount + " 検索対象ファイル数:" + AllFileCount;
                
            } catch (GrepCancelException) {
                msg = "キャンセルしました。"+ 
                    "該当数:" + MatchCount + " 該当ファイル数:" + MatchFileCount + " 検索対象ファイル数:" + AllFileCount;

            } catch (Exception ex) {
                msg = "エラーが発生しました。" + ex.Message;
            }

            //Grep完了を通知します
            _control.GrepFinished(msg);
        }

        /// <summary>
        /// 引数のファイルが検索条件に一致するかチェックして、
        /// 一致する場合には、メインコントロール側の AddResult() で結果を返します
        /// 本メソッドは、Grepの対象ファイル毎に、
        /// FileUtils.GetFileList()のコールバックで呼ばれ、
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool CheckFile(string path)
        {
            if (this.CancelFlg) {
                throw new GrepCancelException();
            }

            //処理中のパスを画面に表示します
            if (_progressCount == 0) {
                _control.GrepProgress(path);
            }
            //進捗状況の表示は重いため、10回に1回表示するようにする
            if (++_progressCount >= 10) _progressCount = 0;

            //Grepの条件に拡張子が指定されている場合は、拡張子をチェックします
            if (StringUtils.IsNotEmpty(this.Extension)) {
                if (CheckExtensions(path) == false) return false; //不一致
            }

            //検索対象ファイル数を +1 します
            AllFileCount++;

            //ファイルの本文に指定されたキーワードが含まれるかチェックします
            CheckText(path);

            return true;
        }

        /// <summary>
        /// Grepの条件に拡張子とファイルの拡張子が一致するかチェックします
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool CheckExtensions(string path)
        {
            //対象ファイルの拡張子を取得します
            var targetExt = System.IO.Path.GetExtension(path); //.txt など、先頭に「.」がつく

            //拡張子のチェック
            var                 extensions = this.Extension.Split(new[]{',', ' '});
            foreach (var ext in extensions) {
                //入力された拡張子の先頭に「.」が付くように統一します
                var extComp = ext.StartsWith(".") ? ext : "." + ext;

                //拡張子を比較します
                if (targetExt.Equals(extComp)) return true; //一致した場合
            }

            return false; //不一致
        }

        /// <summary>
        /// ファイルの本文にキーワードが含まれているかチェックします
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private void CheckText(string path)
        {
            //ファイルを読み込みます
            var textFile = new TextFile();
            textFile.Load(path);
            var list = textFile.Text.Split('\n');
            
            bool isMatch = false; //true:検索で引っかかった場合

            //テキストファイルを1行ずつループしてチェックします
            for (int i = 0; i < list.Count(); i++) {

                if (this.RegExp) {
                    //正規表現で検索する
                    
                    //大文字・小文字の区別
                    var options = this.NotIgnoreCase ?  RegexOptions.None : RegexOptions.IgnoreCase;
                    
                    //行にキーワードが含まれているか？
                    var matches = Regex.Matches(list[i], this.Keyword, options);
                    foreach (Match match in matches) {
                        //含まれている場合、結果をメインコントロールに返します
                        _control.AddGrepResult(path, i + 1, match.Index, match.Value, list[i], textFile.TextEncoding.WebName);

                        //検索該当数を +1 します
                        MatchCount++;
                        isMatch = true;
                    }
                    
                } else {
                    //indexOf()で検索する

                    //大文字・小文字の区別
                    var comp = this.NotIgnoreCase ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;

                    //行にキーワードが含まれているか？
                    int index = -1;
                    while((index = list[i].IndexOf(this.Keyword, index + 1, comp)) >= 0) {
                        
                        //マッチした文字列を取得します
                        var keyword = this.Keyword;
                        if (this.NotIgnoreCase == false) {
                            keyword = list[i].Substring(index, keyword.Length);
                        }

                        //含まれている場合、結果をメインコントロールに返します
                        _control.AddGrepResult(path, i + 1, index, keyword, list[i], textFile.TextEncoding.WebName);

                        //検索該当行数を +1 します
                        MatchCount++;
                        isMatch = true;
                    }
                }
            }

            if (isMatch) MatchFileCount++; //検索該当ファイル数を +1 します
        }

    } //class

    //Grepキャンセル用の例外
    class GrepCancelException : Exception{}

}
