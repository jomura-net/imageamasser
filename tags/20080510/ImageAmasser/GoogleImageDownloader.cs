using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Windows.Forms;
using System.Web;
using System.ComponentModel;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Threading;
using ImageAmasser;
using System.Net;

namespace ImageAmasser
{
    sealed class GoogleImageDownloader : IDisposable
    {
        public Collection<WebClient> Queue
        {
            get { return m_queue; }
        }
        Collection<WebClient> m_queue = new Collection<WebClient>();

        #region 過去にダウンロードした画像URLの履歴管理

        DownloadHistoryDS.HistoryDataTable historytable = new DownloadHistoryDS.HistoryDataTable();

        static string HistoryFilePath
        {
            get
            {
                string dirname = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
                return dirname + "\\history.xml";
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GoogleImageDownloader()
        {
            LoadHistoryTable();
        }

        void LoadHistoryTable()
        {
            if (File.Exists(HistoryFilePath))
            {
                historytable.ReadXml(HistoryFilePath);
            }
        }

        void SaveHistoryTable()
        {
            lock (historytable)
            {
                historytable.WriteXml(HistoryFilePath);
            }
        }

        #region IDisposable メンバ

        public void Dispose()
        {
            SaveHistoryTable();
            historytable.Dispose();
        }

        #endregion

        #endregion

        #region statics

        public static Uri BuildHtmlUri(string searchWords)
        {
            string encodeSearchWords = HttpUtility.UrlEncode(searchWords);
            string safe = GetSafeSearchFilteringValue(
                Properties.Settings.Default.SafeSearchFiltering);
            string imgsz = Properties.Settings.Default.ImageSize;

            StringBuilder urlb = new StringBuilder("http://images.google.co.jp/images");
            urlb.Append("?ie=UTF-8&oe=Shift_JIS&filter=0");
            urlb.Append("&q=" + encodeSearchWords);
            if (!string.IsNullOrEmpty(safe))
            {
                urlb.Append("&safe=" + safe);
            }
            if (!string.IsNullOrEmpty(imgsz))
            {
                urlb.Append("&imgsz=" + imgsz);
            }
            return new Uri(urlb.ToString());
        }

        static Regex regex = new Regex(@"imgurl=(?<imgurl>https?:\/\/[^&]+)&imgrefurl=(?<imgrefurl>https?:\/\/[^&]+)&start");
        
        public static Dictionary<string, string> ParseHtml(Uri url)
        {
            Dictionary<string, string> imgurls = new Dictionary<string, string>();
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.Headers.Add("User-Agent: HTTP");
                wc.Headers.Add("Cookie: PREF=ID=de8c67910902bc10:TM=1206815195:LM=1206815195:S=gXYrewusW-tZFgq-");
                using (Stream st = wc.OpenRead(url.AbsoluteUri))
                using (StreamReader sr = new StreamReader(st, Encoding.GetEncoding(932)))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        MatchCollection matches = regex.Matches(line);
                        foreach (Match match in matches)
                        {
                            //Debug.WriteLine("0 : " + match.Groups["imgurl"].Value);
                            //Debug.WriteLine("1 : " + match.Groups["imgrefurl"].Value);
                            imgurls.Add(match.Groups["imgurl"].Value, match.Groups["imgrefurl"].Value);
                        }
                    }
                }
            }
            return imgurls;
        }

        static string GetSafeSearchFilteringValue(string name)
        {
            string val = string.Empty;
            switch (name)
            {
                case "strict":
                    val = "active";
                    break;
                case "do not filter":
                    val = "off";
                    break;
                default:
                    break;
            }
            return val;
        }

        #endregion

        public void GetImageFile(string imgurl, string imgrefurl, string searchWords)
        {
            if (null != historytable.FindByURL(imgurl)) return;

            string dirname = Properties.Settings.Default.BaseSaveFolder + "\\"
                + searchWords.Replace("/", string.Empty).Replace("\\", string.Empty)
                .Replace(":", string.Empty).Replace("*", string.Empty)
                .Replace("?", string.Empty).Replace("\"", string.Empty).Replace("<", string.Empty)
                .Replace(">", string.Empty).Replace("|", string.Empty);
            if (!Directory.Exists(dirname))
            {
                Directory.CreateDirectory(dirname);
            }

            char replaceChar = '-';
            string filepath = dirname + "\\" + imgurl.Replace("http://", string.Empty)
                .Replace("https://", string.Empty).Replace('/', replaceChar)
                .Replace('\\', replaceChar).Replace(':', replaceChar).Replace('*', replaceChar)
                .Replace('?', replaceChar).Replace('"', replaceChar).Replace('<', replaceChar)
                .Replace('>', replaceChar).Replace('|', replaceChar);

            //FileInfoクラス利用時のPathTooLongExceptionを防止
            if (filepath.Length > 255) filepath = filepath.Substring(0, 255);

            if (File.Exists(filepath)) return;

            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("Referer: " + imgrefurl);
                wc.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted_Callback);
                m_queue.Add(wc);
                wc.DownloadFileAsync(new Uri(imgurl), filepath, new string[] { imgurl, filepath });
            }
        }

        void DownloadFileCompleted_Callback(object sender, AsyncCompletedEventArgs e)
        {
            WebClient wc = sender as WebClient;
            m_queue.Remove(wc);

            string[] imgurls = e.UserState as string[];
            string imgurl = imgurls[0];
            string filepath = imgurls[1];

            FileInfo file = new FileInfo(filepath);

            if (e.Error != null)
            {
                file.Delete();
                Exception ex = e.Error;
                string message = ex.Message;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    message += " " + ex.Message;
                }
                Debug.WriteLine("Download Error : " + message);
            }
            else if (e.Cancelled)
            {
                file.Delete();
                Debug.WriteLine("Download Canceled : " + imgurl);
            }
            else if (!wc.ResponseHeaders[HttpResponseHeader.ContentType]
                .StartsWith("image/", StringComparison.CurrentCulture))
            {
                file.Delete();
                Debug.WriteLine("Download Not-a-image : " + imgurl);
            }
            else
            {
                Debug.WriteLine("Download Complete ("
                    + (file.Length / 1024) + ") : " + imgurl);
                lock (historytable)
                {
                    if (null == historytable.FindByURL(imgurl))
                    {
                        historytable.AddHistoryRow(imgurl, DateTime.Now);
                    }
                }
            }

            if (file.Exists && file.Length == 0)
            {
                file.Delete();
            }
        }

    }//end class
}//end namespace
