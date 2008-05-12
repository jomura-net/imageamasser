using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ImageAmasser
{
    static class Program
    {
        static System.Threading.Mutex _mutex;

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Mutexクラスの作成
            //"MyName"の部分を適当な文字列に変えてください
            _mutex = new System.Threading.Mutex(false, "ImageAmasser");
            //ミューテックスの所有権を要求する
            if (_mutex.WaitOne(0, false) == false)
            {
                //すでに起動していると判断して終了
                MessageBox.Show("多重起動はできません。");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}