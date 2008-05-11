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
        #region �����ݒ�

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            //���[�U�[�ݒ�̕���
            RestoreProperties();
#if USE_CUI
            //�����t���ŋN�������ꍇ�́A
            //�������W�J�n����B
            GetCommandLineArgs();
#endif
        }

        void GetCommandLineArgs()
        {
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                tbSearchWord.Text = string.Join(" ", Environment.GetCommandLineArgs(),
                    1, Environment.GetCommandLineArgs().Length - 1);
                //�摜�擾 �����J�n
                this.bStart_Click(bStart, new EventArgs());
            }
        }

        void RestoreProperties()
        {
            //�ݒ�l�����F�摜�T�C�Y�̑I��
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

            //�ݒ�l�����G�t�B���^
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

        #region UI�ł̐ݒ�ۑ�

        /// <summary>
        /// �A�v���P�[�V�����I��
        /// 
        /// �ݒ�l��XML�t�@�C���ɕۑ�����B
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// �摜�ۑ��t�H���_�̑I���{�^���N���b�N
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
        /// �t�B���^�I�����W�I�{�^���̃`�F�b�N��ԕύX
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
        /// �摜�T�C�Y�I���`�F�b�N�{�b�N�X�̃`�F�b�N��ԕύX
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

        #region ���W�����֘A�C�x���g

        /// <summary>
        /// �ۑ��J�n�{�^���N���b�N
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
                    //�ݒ�`�F�b�N
                    if (string.IsNullOrEmpty(tbSearchWord.Text)) return;
                    if (!Directory.Exists(tbSaveFolder.Text))
                    {
                        Directory.CreateDirectory(tbSaveFolder.Text);
                    }

                    // �_�E�����[�h����
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

                // �摜URL�̎��W
                //Text = "Collecting Image URLs... - " + APP_NAME;
                Invoke(textDlg, new object[]{"Collecting Image URLs... - " + APP_NAME});
                Uri url = GoogleImageDownloader.BuildHtmlUri(searchString);
                Dictionary<string, string> imgurls = new Dictionary<string, string>();
                for (int i = 0; ; i = i + 20)
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }

                    Dictionary<string, string> newImgurls = loader.ParseHtml(new Uri(url.AbsoluteUri + "&start=" + i));
                    if (newImgurls == null)
                    {
                        break;
                    }
                    foreach (string newImgurl in newImgurls.Keys)
                    {
                        if (!imgurls.ContainsKey(newImgurl))
                        {
                            imgurls.Add(newImgurl, newImgurls[newImgurl]);
                            if (imgurls.Count >= maxCount) goto COLLECT_URL_END;
                        }
                    }
                    worker.ReportProgress(10 * imgurls.Count / maxCount);
                    Debug.WriteLine("Target image count : " + imgurls.Count + "(" + i + ")");
                }
            COLLECT_URL_END:

                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                
                //�_�E�����[�h�Ώۂ̉摜��
                int imgCount = imgurls.Count;

                //�摜�̃_�E�����[�h
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

                //���s�����ҋ@
                int _poolCount = loader.Queue.Count; //�{���_�ł�Queue��
                int poolCount = loader.Queue.Count;
                int wait = 30000;
                bool isAlreadyCanceled = false;
                while (loader.Queue.Count != 0)
                {
                    if ((worker.CancellationPending || wait < 0) && !isAlreadyCanceled)
                    {
                        e.Cancel = true;

                        //�L�����Z������
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

            //e.Result = "���ׂĊ���";
        }

        static void CancelQueueAll(Collection<WebClient> collection)
        {
            //�L�����Z������
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
            // �i���傭�󋵂̕\��
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
