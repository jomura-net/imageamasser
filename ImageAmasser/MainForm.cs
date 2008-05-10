#define USE_CUI

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using System.Net;
using System.Collections.ObjectModel;
using System.Globalization;

namespace ImageAmasser
{
    public partial class MainForm : Form
    {
        const string APP_NAME = "Image Amasser";
        #region 初期設定

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            //ユーザー設定の復元
            RestoreProperties();
#if USE_CUI
            //引数付きで起動した場合は、
            //自動収集開始する。
            GetCommandLineArgs();
#endif
        }

        void GetCommandLineArgs()
        {
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                tbSearchWord.Text = string.Join(" ", Environment.GetCommandLineArgs(),
                    1, Environment.GetCommandLineArgs().Length - 1);
                //画像取得 自動開始
                this.bStart_Click(bStart, new EventArgs());
            }
        }

        void RestoreProperties()
        {
            //設定値復元：画像サイズの選択
            string imgsz = Properties.Settings.Default.ImageSize;
            string[] imgszs = imgsz.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> imgszList = new List<string>(imgszs);
            for (int i = 0, max = clbImageSize.Items.Count; i < max; i++)
            {
                if (imgszList.Count == 0 || imgszList.Contains(clbImageSize.Items[i] as string))
                {
                    clbImageSize.SetItemChecked(i, true);
                }
            }

            //設定値復元；フィルタ
            string selectedFilterButton = Properties.Settings.Default.SafeSearchFiltering;
            foreach (RadioButton button in this.gbSafeSearchFiltering.Controls)
            {
                if (selectedFilterButton == button.Text)
                {
                    button.Checked = true;
                }
            }
        }

        #endregion

        #region UIでの設定保存

        /// <summary>
        /// アプリケーション終了
        /// 
        /// 設定値をXMLファイルに保存する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// 画像保存フォルダの選択ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bSave_Click(object sender, EventArgs e)
        {
            if (fbdSaveFolder.ShowDialog() == DialogResult.OK)
            {
                this.tbSaveFolder.Text = fbdSaveFolder.SelectedPath;
            }
        }

        /// <summary>
        /// フィルタ選択ラジオボタンのチェック状態変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if (button.Checked)
            {
                Properties.Settings.Default.SafeSearchFiltering = button.Text;
            }
        }

        /// <summary>
        /// 画像サイズ選択チェックボックスのチェック状態変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox clbImageSizes = (CheckedListBox)sender;
            string seleteditem = clbImageSizes.SelectedItem as string;
            if (string.IsNullOrEmpty(seleteditem)) return;

            List<string> imgszList = new List<string>();
            foreach (string checkedItem in clbImageSizes.CheckedItems)
            {
                imgszList.Add(checkedItem);
            }

            if (CheckState.Checked == e.NewValue)
            {
                imgszList.Add(seleteditem);
            }
            else
            {
                imgszList.Remove(seleteditem);
            }

            if (clbImageSizes.Items.Count != imgszList.Count)
            {
                string imgsz = string.Join("|", imgszList.ToArray());
                Properties.Settings.Default.ImageSize = imgsz;
            }
        }

        void Enable(bool enabled)
        {
            bStart.Text = enabled ? "Start" : "Cancel";
            tbSearchWord.Enabled =
            tabControl1.Enabled = enabled;

            if (enabled)
            {
                bStart.Enabled = true;
                Text = APP_NAME;
            }
        }

        #endregion

        #region 収集処理関連イベント

        /// <summary>
        /// 保存開始ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bStart_Click(object sender, EventArgs e)
        {
            switch (bStart.Text)
            {
                case "Cancel":
                    bwDownload.CancelAsync();
                    bStart.Enabled = false;
                    break;

                default:
                    //設定チェック
                    if (string.IsNullOrEmpty(tbSearchWord.Text)) return;
                    if (!Directory.Exists(tbSaveFolder.Text))
                    {
                        Directory.CreateDirectory(tbSaveFolder.Text);
                    }

                    // ダウンロード処理
                    if (!bwDownload.IsBusy)
                    {
                        Enable(false);
                        bwDownload.RunWorkerAsync(tbSearchWord.Text);
                    }
                    break;
            }
        }

        delegate void TextDelegate(string text);
        void SetText(string text) { Text = text; }

        void bwDownload_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            worker.ReportProgress(0);
            string searchString = e.Argument as string;

            using (GoogleImageDownloader loader = new GoogleImageDownloader())
            {
                string maxCountStr = ConfigurationManager.AppSettings["MaxCount"];
                int maxCount = 1000;
                if (!int.TryParse(maxCountStr, out maxCount))
                {
                    Debug.WriteLine("Invalid value : AppSettings[\"MaxCount\"]");
                }

                TextDelegate textDlg = new TextDelegate(SetText);

                // 画像URLの収集
                //Text = "Collecting Image URLs... - " + APP_NAME;
                Invoke(textDlg, new object[]{"Collecting Image URLs... - " + APP_NAME});
                Uri url = GoogleImageDownloader.BuildHtmlUri(searchString);
                Dictionary<string, string> imgurls = new Dictionary<string, string>();
                for (int i = 0; i < maxCount; i = i + 20)
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }

                    Dictionary<string, string> newImgurls = GoogleImageDownloader.ParseHtml(new Uri(url.AbsoluteUri + "&start=" + i));
                    foreach (string newImgurl in newImgurls.Keys)
                    {
                        if (!imgurls.ContainsKey(newImgurl))
                        {
                            imgurls.Add(newImgurl, newImgurls[newImgurl]);
                        }
                    }
                    worker.ReportProgress(10 * i / maxCount);
                    Debug.WriteLine("Target image count : " + imgurls.Count);
                }

                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                
                //ダウンロード対象の画像個数
                int imgCount = imgurls.Count;

                //画像のダウンロード
                //Text = string.Format("Getting {0} Images... - " + APP_NAME, imgCount);
                Invoke(textDlg, new object[] { string.Format(CultureInfo.CurrentCulture,
                    "Getting {0} Images... - " + APP_NAME, imgCount) });
                int index = 0;
                foreach (string imgurl in imgurls.Keys)
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }

                    loader.GetImageFile(imgurl, imgurls[imgurl], searchString);

                    worker.ReportProgress(10 + (50 * ++index / imgCount));
                }

                //実行完了待機
                int _poolCount = loader.Queue.Count; //本時点でのQueue数
                int poolCount = loader.Queue.Count;
                int wait = 30000;
                bool isAlreadyCanceled = false;
                while (loader.Queue.Count != 0)
                {
                    if ((worker.CancellationPending || wait < 0) && !isAlreadyCanceled)
                    {
                        e.Cancel = true;

                        //キャンセル処理
                        CancelQueueAll(loader.Queue);
                        isAlreadyCanceled = true;
                    }

                    worker.ReportProgress(60 + (40 * (_poolCount - poolCount) / _poolCount));
                    //Text = string.Format("remain {0} files... - " + APP_NAME, loader.Queue.Count);
                    Invoke(textDlg, new object[] { string.Format(CultureInfo.CurrentCulture,
                        "remain {0} files... - " + APP_NAME, loader.Queue.Count) });

                    Thread.Sleep(1000);

                    if (poolCount == loader.Queue.Count)
                    {
                        wait -= 1000;
                        Debug.WriteLine("remaining queue : " + poolCount);
                    }
                    else
                    {
                        poolCount = loader.Queue.Count;
                        wait = 30000;
                    }
                }
            }

            //e.Result = "すべて完了";
        }

        static void CancelQueueAll(Collection<WebClient> collection)
        {
            //キャンセル処理
            lock (collection)
            {
                WebClient[] wcs = new WebClient[collection.Count];
                collection.CopyTo(wcs, 0);

                foreach (WebClient wc in wcs)
                {
                    wc.CancelAsync();
                }
            }
        }

        void bwDownload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbBar.Value = 0;
            Enable(true);
#if USE_CUI
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                Dispose();
            }
#endif
        }

        private void bwDownload_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // 進ちょく状況の表示
            //this.Text = e.ProgressPercentage + "% Complete";
            pbBar.Value = e.ProgressPercentage;
        }

        #endregion

        private void tbSearchWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.bStart_Click(bStart, new EventArgs());
            }
        }

    }//end class
}//end namespace
