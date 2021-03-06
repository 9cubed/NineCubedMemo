﻿using NineCubed.Common.Controls.FileList;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Interfaces;
using NineCubed.Memo.Plugins.TextEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineCubed.Memo.Plugins.FileList.Menus
{
    public class OpenFileBinaryMenu : ToolStripMenuItem
    {
        public OpenFileBinaryMenu(FileListGrid fileList) 
        {
            this.Text = "開く（バイナリ形式）";

            //クリックイベント
            this.Click += (sender, e) => {
                //カレント行のパスを取得します。
                var path = fileList[0, fileList.CurrentCell.RowIndex].Value.ToString();

                //フォルダの場合は処理しない
                if (FileUtils.IsFile(path) == false) return;

                //プラグイン生成パラメーターを設定します
                var pluginCreateParam = new PluginCreateParam {
                    ["path"]      = path,  //選択されたパス
                    ["encoding"]  = null,  //文字コード
                    ["is_binary"] = true   //バイナリーモード
                };

                //テキストエディタープラグインを生成します
                var plugin = (IFilePlugin)PluginManager.GetInstance().CreatePluginInstance(typeof(TextEditorPlugin), pluginCreateParam, this);
            };
        }
    }
}
