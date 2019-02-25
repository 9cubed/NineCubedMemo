using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileInfoManager.DB;
using NineCubed.Common.Controls;
using NineCubed.Common.Utils;
using NineCubed.Memo.Plugins.Interfaces;
using NineCubed.Memo.Plugins;
using NineCubed;
using System.IO;
using NineCubed.Memo.Plugins.TextEditor;
using NineCubed.Common.Files;
using FileInfoManager.Manager;

namespace FileInfoManager.Editor
{
    public partial class FileInfoEditorControl : UserControl
    {
        /// <summary>
        /// プラグイン
        /// </summary>
        IPlugin _plugin;

        /// <summary>
        /// プラグインマネージャー
        /// </summary>
        PluginManager _pluginManager;

        /// <summary>
        /// DB接続用オブジェクト
        /// </summary>
        FileDB _db;

        /// <summary>
        /// メモ欄
        /// </summary>
        TextBoxEx _txtMemo;
        public TextBoxEx GetTextEditor() => _txtMemo;

        /// <summary>
        /// 変更の有無
        /// </summary>
        public  bool Modified {get; set; } //変更の有無 true:変更あり

        /// <summary>
        /// 入力欄の値をファイルデータにして返します
        /// </summary>
        /// <returns></returns>
        public FileData GetFileData() {
            var data = new FileData {
                id      = StringUtils.ToInt(txtId.Text),
                title   = txtTitle.Text,
                memo    = _txtMemo.Text,
                value   = cmbValue.SelectedIndex,
                path    = txtPath.Text,
                size    = StringUtils.ToLong(txtSize.Text),
                created = txtCreated.Text,
                updated = txtUpdated.Text,
                _tags   = txtTag.Text,

                //ファイル種別 1:ファイル 2:フォルダ
                kind = (FileUtils.IsFile(txtPath.Text) ? 1 : 0) +
                       (FileUtils.IsDir (txtPath.Text) ? 2 : 0)
            };
            return data;
        }

        /// <summary>
        /// ファイルデータを入力欄へ設定します
        /// </summary>
        /// <param name="fileData"></param>
        public void SetFileData(FileData fileData) {
            txtId     .Text = fileData.id.ToString();
            txtTitle  .Text = fileData.title;
            _txtMemo  .Text = fileData.memo;
            cmbValue.SelectedIndex = fileData.value;
            txtPath   .Text = fileData.path;
            txtSize   .Text = fileData.size.ToString();
            txtCreated.Text = fileData.created;
            txtUpdated.Text = fileData.updated;
            txtTag    .Text = fileData._tags;

            //パスが存在しない場合は、パスの背景をピンクにします
            CheckFile(fileData.path);

            if (FileUtils.IsFile(fileData.path)) {
                txtSize.Text = new FileInfo(fileData.path).Length.ToString();
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dbPath"></param>
        public FileInfoEditorControl(IPlugin plugin, string dbPath, string id = "")
        {
            InitializeComponent();

            //プラグインを保持します
            _plugin = plugin;

            //プラグインマネージャーを保持します
            _pluginManager = PluginManager.GetInstance();

            //DB接続用オブジェクトを生成します
            _db = new FileDB(dbPath);

            _txtMemo = new TextBoxEx();
            _txtMemo.Parent = this;
            _txtMemo.Dock = DockStyle.Fill;
            _txtMemo.BringToFront();
            _txtMemo.Initialize();
            _txtMemo.TextChanged += _txtMemo_TextChanged;
        }

        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileInfoEditorControl_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// タグボタンクリック
        /// タグ一覧画面を表示します
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTag_Click(object sender, EventArgs e)
        {
            //タグの編集画面を表示します
            using(var form = new TagForm(TagForm.Mode.Select, _db, txtTag.Text)) {
                //画面を表示します
                form.ShowDialog();
                if (form.SelectedTagList == null) return;

                //タグ欄に選択されたタグを設定します
                txtTag.Text = string.Join(" ", form.SelectedTagList);
            }
        }

        /// <summary>
        /// 保存ボタンクリック
        /// ファイルデータを保存します
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        /// <summary>
        /// ファイル情報テーブルからデータを取得します
        /// </summary>
        /// <param name="id"></param>
        public void LoadData(string id)
        {
            using(var connection = _db.GetConnection()) {
                var fileData = FileDataDao.GetData(connection, id);
                if (fileData != null) {
                    fileData._tags = TagDataDao.GetTags(connection, fileData.id);

                    //ファイルデータを入力欄に設定します
                    SetFileData(fileData);
                }
            }

            //変更なしにします
            Modified = false;
        }

        /// <summary>
        /// ファイル情報テーブルにデータを保存します
        /// </summary>
        public void SaveData()
        {
            using(var connection = _db.GetConnection()) {
                FileDB.SaveData(connection, GetFileData());
            }

            //変更なしにします
            Modified = false;
        }

        private void pnlTop_Click               (object sender, EventArgs e) => EnterEvent();
        private void FileInfoEditorControl_Enter(object sender, EventArgs e) => EnterEvent();
        private void EnterEvent() {
            //アクティブプラグインを設定します
            _pluginManager.ActivePlugin = _plugin;
        }

        /// <summary>
        /// 入力欄の変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbValue_SelectedIndexChanged(object sender, EventArgs e) => Changed();
        private void txtTitle_TextChanged(object sender, EventArgs e) => Changed();
        private void   txtTag_TextChanged(object sender, EventArgs e) => Changed();
        private void  txtSize_TextChanged(object sender, EventArgs e) => Changed();
        private void _txtMemo_TextChanged(object sender, EventArgs e) => Changed();
        private void  txtPath_TextChanged(object sender, EventArgs e) {
            //パスが存在しない場合は、パスの背景をピンクにします
            CheckFile(txtPath.Text);

            Changed();
        }
        private void Changed() {
            Modified = true;
        }

        /// <summary>
        /// ファイルが存在するかチェックして、パス欄の背景色を設定します
        /// </summary>
        /// <param name="path"></param>
        /// <returns>true:ファイルが存在する</returns>
        private bool CheckFile(string path) {
            //パスが存在しない場合は、パスの背景をピンクにします
            if (FileUtils.Exists(path)) {
                //ある場合
                txtPath.BackColor = Color.White;
                return true;
            } else {
                //ない場合
                txtPath.BackColor = Color.LightPink;
                return false;
            }
        }

    } //class

}
