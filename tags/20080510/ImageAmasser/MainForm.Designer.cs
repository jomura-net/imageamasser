namespace ImageAmasser
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tbSearchWord = new System.Windows.Forms.TextBox();
            this.lSearchWord = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpGoogle = new System.Windows.Forms.TabPage();
            this.gbImageSize = new System.Windows.Forms.GroupBox();
            this.clbImageSize = new System.Windows.Forms.CheckedListBox();
            this.gbSafeSearchFiltering = new System.Windows.Forms.GroupBox();
            this.rbStrict = new System.Windows.Forms.RadioButton();
            this.rbNotFilter = new System.Windows.Forms.RadioButton();
            this.rbModerate = new System.Windows.Forms.RadioButton();
            this.tpCommon = new System.Windows.Forms.TabPage();
            this.bSave = new System.Windows.Forms.Button();
            this.lSaveTo = new System.Windows.Forms.Label();
            this.tbSaveFolder = new System.Windows.Forms.TextBox();
            this.fbdSaveFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.pbBar = new System.Windows.Forms.ProgressBar();
            this.bStart = new System.Windows.Forms.Button();
            this.pMain = new System.Windows.Forms.Panel();
            this.bwDownload = new System.ComponentModel.BackgroundWorker();
            this.tabControl1.SuspendLayout();
            this.tpGoogle.SuspendLayout();
            this.gbImageSize.SuspendLayout();
            this.gbSafeSearchFiltering.SuspendLayout();
            this.tpCommon.SuspendLayout();
            this.pMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbSearchWord
            // 
            this.tbSearchWord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearchWord.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbSearchWord.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbSearchWord.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tbSearchWord.Location = new System.Drawing.Point(83, 1);
            this.tbSearchWord.MaxLength = 255;
            this.tbSearchWord.Name = "tbSearchWord";
            this.tbSearchWord.Size = new System.Drawing.Size(89, 20);
            this.tbSearchWord.TabIndex = 0;
            this.tbSearchWord.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearchWord_KeyDown);
            // 
            // lSearchWord
            // 
            this.lSearchWord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lSearchWord.AutoSize = true;
            this.lSearchWord.Location = new System.Drawing.Point(3, 4);
            this.lSearchWord.Name = "lSearchWord";
            this.lSearchWord.Size = new System.Drawing.Size(81, 12);
            this.lSearchWord.TabIndex = 1;
            this.lSearchWord.Text = "Search Words :";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpGoogle);
            this.tabControl1.Controls.Add(this.tpCommon);
            this.tabControl1.Location = new System.Drawing.Point(0, 22);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(223, 163);
            this.tabControl1.TabIndex = 2;
            // 
            // tpGoogle
            // 
            this.tpGoogle.Controls.Add(this.gbImageSize);
            this.tpGoogle.Controls.Add(this.gbSafeSearchFiltering);
            this.tpGoogle.Location = new System.Drawing.Point(4, 21);
            this.tpGoogle.Name = "tpGoogle";
            this.tpGoogle.Padding = new System.Windows.Forms.Padding(3);
            this.tpGoogle.Size = new System.Drawing.Size(215, 138);
            this.tpGoogle.TabIndex = 0;
            this.tpGoogle.Text = "Google";
            this.tpGoogle.UseVisualStyleBackColor = true;
            // 
            // gbImageSize
            // 
            this.gbImageSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbImageSize.Controls.Add(this.clbImageSize);
            this.gbImageSize.Location = new System.Drawing.Point(0, 0);
            this.gbImageSize.Name = "gbImageSize";
            this.gbImageSize.Size = new System.Drawing.Size(216, 70);
            this.gbImageSize.TabIndex = 14;
            this.gbImageSize.TabStop = false;
            this.gbImageSize.Text = "Image Size";
            // 
            // clbImageSize
            // 
            this.clbImageSize.BackColor = System.Drawing.SystemColors.Control;
            this.clbImageSize.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clbImageSize.CheckOnClick = true;
            this.clbImageSize.ColumnWidth = 60;
            this.clbImageSize.FormattingEnabled = true;
            this.clbImageSize.Items.AddRange(new object[] {
            "huge",
            "xxlarge",
            "xlarge",
            "large",
            "medium",
            "small",
            "icon"});
            this.clbImageSize.Location = new System.Drawing.Point(2, 11);
            this.clbImageSize.MultiColumn = true;
            this.clbImageSize.Name = "clbImageSize";
            this.clbImageSize.Size = new System.Drawing.Size(125, 56);
            this.clbImageSize.TabIndex = 10;
            this.clbImageSize.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox1_ItemCheck);
            // 
            // gbSafeSearchFiltering
            // 
            this.gbSafeSearchFiltering.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSafeSearchFiltering.Controls.Add(this.rbStrict);
            this.gbSafeSearchFiltering.Controls.Add(this.rbNotFilter);
            this.gbSafeSearchFiltering.Controls.Add(this.rbModerate);
            this.gbSafeSearchFiltering.Location = new System.Drawing.Point(1, 71);
            this.gbSafeSearchFiltering.Name = "gbSafeSearchFiltering";
            this.gbSafeSearchFiltering.Size = new System.Drawing.Size(214, 67);
            this.gbSafeSearchFiltering.TabIndex = 14;
            this.gbSafeSearchFiltering.TabStop = false;
            this.gbSafeSearchFiltering.Text = "SafeSearch Filtering";
            // 
            // rbStrict
            // 
            this.rbStrict.AutoSize = true;
            this.rbStrict.Location = new System.Drawing.Point(3, 12);
            this.rbStrict.Name = "rbStrict";
            this.rbStrict.Size = new System.Drawing.Size(50, 16);
            this.rbStrict.TabIndex = 11;
            this.rbStrict.Text = "strict";
            this.rbStrict.UseVisualStyleBackColor = true;
            this.rbStrict.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbNotFilter
            // 
            this.rbNotFilter.AutoSize = true;
            this.rbNotFilter.Location = new System.Drawing.Point(3, 44);
            this.rbNotFilter.Name = "rbNotFilter";
            this.rbNotFilter.Size = new System.Drawing.Size(83, 16);
            this.rbNotFilter.TabIndex = 13;
            this.rbNotFilter.Text = "do not filter";
            this.rbNotFilter.UseVisualStyleBackColor = true;
            this.rbNotFilter.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbModerate
            // 
            this.rbModerate.AutoSize = true;
            this.rbModerate.Checked = true;
            this.rbModerate.Location = new System.Drawing.Point(3, 28);
            this.rbModerate.Name = "rbModerate";
            this.rbModerate.Size = new System.Drawing.Size(70, 16);
            this.rbModerate.TabIndex = 12;
            this.rbModerate.TabStop = true;
            this.rbModerate.Text = "moderate";
            this.rbModerate.UseVisualStyleBackColor = true;
            this.rbModerate.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // tpCommon
            // 
            this.tpCommon.Controls.Add(this.bSave);
            this.tpCommon.Controls.Add(this.lSaveTo);
            this.tpCommon.Controls.Add(this.tbSaveFolder);
            this.tpCommon.Location = new System.Drawing.Point(4, 21);
            this.tpCommon.Name = "tpCommon";
            this.tpCommon.Padding = new System.Windows.Forms.Padding(3);
            this.tpCommon.Size = new System.Drawing.Size(215, 138);
            this.tpCommon.TabIndex = 1;
            this.tpCommon.Text = "Common";
            this.tpCommon.UseVisualStyleBackColor = true;
            // 
            // bSave
            // 
            this.bSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bSave.Location = new System.Drawing.Point(186, 3);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(26, 19);
            this.bSave.TabIndex = 2;
            this.bSave.Text = "...";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // lSaveTo
            // 
            this.lSaveTo.AutoSize = true;
            this.lSaveTo.Location = new System.Drawing.Point(2, 5);
            this.lSaveTo.Name = "lSaveTo";
            this.lSaveTo.Size = new System.Drawing.Size(50, 12);
            this.lSaveTo.TabIndex = 1;
            this.lSaveTo.Text = "Save to :";
            // 
            // tbSaveFolder
            // 
            this.tbSaveFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSaveFolder.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ImageAmasser.Properties.Settings.Default, "BaseSaveFolder", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbSaveFolder.Location = new System.Drawing.Point(51, 3);
            this.tbSaveFolder.Name = "tbSaveFolder";
            this.tbSaveFolder.Size = new System.Drawing.Size(133, 19);
            this.tbSaveFolder.TabIndex = 0;
            this.tbSaveFolder.Text = global::ImageAmasser.Properties.Settings.Default.BaseSaveFolder;
            // 
            // pbBar
            // 
            this.pbBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbBar.Location = new System.Drawing.Point(0, 188);
            this.pbBar.Name = "pbBar";
            this.pbBar.Size = new System.Drawing.Size(223, 20);
            this.pbBar.TabIndex = 3;
            // 
            // bStart
            // 
            this.bStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bStart.Location = new System.Drawing.Point(173, 1);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(48, 20);
            this.bStart.TabIndex = 4;
            this.bStart.Text = "Start";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.bStart_Click);
            // 
            // pMain
            // 
            this.pMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pMain.Controls.Add(this.lSearchWord);
            this.pMain.Controls.Add(this.bStart);
            this.pMain.Controls.Add(this.tbSearchWord);
            this.pMain.Controls.Add(this.tabControl1);
            this.pMain.Location = new System.Drawing.Point(0, 0);
            this.pMain.Name = "pMain";
            this.pMain.Size = new System.Drawing.Size(223, 185);
            this.pMain.TabIndex = 3;
            // 
            // bwDownload
            // 
            this.bwDownload.WorkerReportsProgress = true;
            this.bwDownload.WorkerSupportsCancellation = true;
            this.bwDownload.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwDownload_DoWork);
            this.bwDownload.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwDownload_RunWorkerCompleted);
            this.bwDownload.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwDownload_ProgressChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 208);
            this.Controls.Add(this.pMain);
            this.Controls.Add(this.pbBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(160, 92);
            this.Name = "MainForm";
            this.Text = "Image Amasser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tpGoogle.ResumeLayout(false);
            this.gbImageSize.ResumeLayout(false);
            this.gbSafeSearchFiltering.ResumeLayout(false);
            this.gbSafeSearchFiltering.PerformLayout();
            this.tpCommon.ResumeLayout(false);
            this.tpCommon.PerformLayout();
            this.pMain.ResumeLayout(false);
            this.pMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbSearchWord;
        private System.Windows.Forms.Label lSearchWord;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpGoogle;
        private System.Windows.Forms.TabPage tpCommon;
        private System.Windows.Forms.FolderBrowserDialog fbdSaveFolder;
        private System.Windows.Forms.Label lSaveTo;
        private System.Windows.Forms.TextBox tbSaveFolder;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.ProgressBar pbBar;
        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.Panel pMain;
        private System.Windows.Forms.GroupBox gbSafeSearchFiltering;
        private System.Windows.Forms.GroupBox gbImageSize;
        private System.Windows.Forms.RadioButton rbStrict;
        private System.Windows.Forms.RadioButton rbNotFilter;
        private System.Windows.Forms.RadioButton rbModerate;
        private System.Windows.Forms.CheckedListBox clbImageSize;
        private System.ComponentModel.BackgroundWorker bwDownload;
    }
}

