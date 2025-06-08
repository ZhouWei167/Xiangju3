namespace PatchManager
{
    partial class PMain
    {
        /// <summary>
        /// 必需的设计器变量
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，则为true；否则为false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// 设计器支持所需的方法-不要修改
        /// 此方法的内容和代码编辑器。
        /// </summary>
        private void InitializeComponent()
        {
            this.pcClientPathLabel = new System.Windows.Forms.Label();
            this.pcClientPathTextBox = new System.Windows.Forms.TextBox();
            this.pcPatchSavePathLabel = new System.Windows.Forms.Label();
            this.labelControl6 = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.labelControl8 = new System.Windows.Forms.Label();
            this.SaveSizeLabel = new System.Windows.Forms.Label();
            this.TotalProgressBar = new System.Windows.Forms.ProgressBar();
            this.SaveSpeedLabel = new System.Windows.Forms.Label();
            this.labelControl10 = new System.Windows.Forms.Label();
            this.FolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.mobileLabel = new System.Windows.Forms.Label();
            this.mobileCheckBox = new System.Windows.Forms.CheckBox();
            this.apkLabel = new System.Windows.Forms.Label();
            this.apkVersionLabel = new System.Windows.Forms.Label();
            this.apkVersionValueLabel = new System.Windows.Forms.Label();
            this.apkFIleTextBox = new System.Windows.Forms.TextBox();
            this.openAPKButton = new System.Windows.Forms.Button();
            this.mobClientPathTextBox = new System.Windows.Forms.TextBox();
            this.mobClientPathLabel = new System.Windows.Forms.Label();
            this.mobPatchSavePathLabel = new System.Windows.Forms.Label();
            this.mobUploadPatchButton = new System.Windows.Forms.Button();
            this.openBaseZipButton = new System.Windows.Forms.Button();
            this.baseZipTextBox = new System.Windows.Forms.TextBox();
            this.baseZipLabel = new System.Windows.Forms.Label();
            this.pcPatchSavePathTextBox = new System.Windows.Forms.TextBox();
            this.mobPatchSavePathTextBox = new System.Windows.Forms.TextBox();
            this.pcUploadPatchButton = new System.Windows.Forms.Button();
            this.directorySearcher1 = new System.DirectoryServices.DirectorySearcher();
            this.SuspendLayout();
            // 
            // pcClientPathLabel
            // 
            this.pcClientPathLabel.AutoEllipsis = true;
            this.pcClientPathLabel.AutoSize = true;
            this.pcClientPathLabel.Location = new System.Drawing.Point(13, 18);
            this.pcClientPathLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pcClientPathLabel.Name = "pcClientPathLabel";
            this.pcClientPathLabel.Size = new System.Drawing.Size(105, 15);
            this.pcClientPathLabel.TabIndex = 0;
            this.pcClientPathLabel.Text = "端游补丁目录:";
            // 
            // pcClientPathTextBox
            // 
            this.pcClientPathTextBox.Location = new System.Drawing.Point(15, 47);
            this.pcClientPathTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.pcClientPathTextBox.Name = "pcClientPathTextBox";
            this.pcClientPathTextBox.Size = new System.Drawing.Size(311, 25);
            this.pcClientPathTextBox.TabIndex = 1;
            this.pcClientPathTextBox.TextChanged += new System.EventHandler(this.pcClientPathTextBox_TextChanged);
            // 
            // pcPatchSavePathLabel
            // 
            this.pcPatchSavePathLabel.AutoEllipsis = true;
            this.pcPatchSavePathLabel.AutoSize = true;
            this.pcPatchSavePathLabel.Location = new System.Drawing.Point(13, 82);
            this.pcPatchSavePathLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pcPatchSavePathLabel.Name = "pcPatchSavePathLabel";
            this.pcPatchSavePathLabel.Size = new System.Drawing.Size(105, 15);
            this.pcPatchSavePathLabel.TabIndex = 2;
            this.pcPatchSavePathLabel.Text = "端游保存目录:";
            // 
            // labelControl6
            // 
            this.labelControl6.AutoEllipsis = true;
            this.labelControl6.AutoSize = true;
            this.labelControl6.Location = new System.Drawing.Point(53, 377);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(45, 15);
            this.labelControl6.TabIndex = 11;
            this.labelControl6.Text = "状态:";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoEllipsis = true;
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(108, 377);
            this.StatusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(38, 15);
            this.StatusLabel.TabIndex = 12;
            this.StatusLabel.Text = "<空>";
            // 
            // labelControl8
            // 
            this.labelControl8.AutoEllipsis = true;
            this.labelControl8.AutoSize = true;
            this.labelControl8.Location = new System.Drawing.Point(51, 398);
            this.labelControl8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(45, 15);
            this.labelControl8.TabIndex = 13;
            this.labelControl8.Text = "保存:";
            // 
            // SaveSizeLabel
            // 
            this.SaveSizeLabel.AutoEllipsis = true;
            this.SaveSizeLabel.AutoSize = true;
            this.SaveSizeLabel.Location = new System.Drawing.Point(108, 398);
            this.SaveSizeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SaveSizeLabel.Name = "SaveSizeLabel";
            this.SaveSizeLabel.Size = new System.Drawing.Size(38, 15);
            this.SaveSizeLabel.TabIndex = 14;
            this.SaveSizeLabel.Text = "<空>";
            // 
            // TotalProgressBar
            // 
            this.TotalProgressBar.Location = new System.Drawing.Point(56, 417);
            this.TotalProgressBar.Margin = new System.Windows.Forms.Padding(4);
            this.TotalProgressBar.Name = "TotalProgressBar";
            this.TotalProgressBar.Size = new System.Drawing.Size(551, 20);
            this.TotalProgressBar.TabIndex = 15;
            // 
            // SaveSpeedLabel
            // 
            this.SaveSpeedLabel.AutoEllipsis = true;
            this.SaveSpeedLabel.AutoSize = true;
            this.SaveSpeedLabel.Location = new System.Drawing.Point(384, 398);
            this.SaveSpeedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SaveSpeedLabel.Name = "SaveSpeedLabel";
            this.SaveSpeedLabel.Size = new System.Drawing.Size(38, 15);
            this.SaveSpeedLabel.TabIndex = 17;
            this.SaveSpeedLabel.Text = "<空>";
            // 
            // labelControl10
            // 
            this.labelControl10.AutoEllipsis = true;
            this.labelControl10.AutoSize = true;
            this.labelControl10.Location = new System.Drawing.Point(331, 398);
            this.labelControl10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(45, 15);
            this.labelControl10.TabIndex = 16;
            this.labelControl10.Text = "速度:";
            // 
            // FolderDialog
            // 
            this.FolderDialog.Description = "请选择补丁目录";
            this.FolderDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // mobileLabel
            // 
            this.mobileLabel.AutoSize = true;
            this.mobileLabel.Location = new System.Drawing.Point(343, 154);
            this.mobileLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.mobileLabel.Name = "mobileLabel";
            this.mobileLabel.Size = new System.Drawing.Size(45, 15);
            this.mobileLabel.TabIndex = 19;
            this.mobileLabel.Text = "手游:";
            // 
            // mobileCheckBox
            // 
            this.mobileCheckBox.AutoSize = true;
            this.mobileCheckBox.Location = new System.Drawing.Point(398, 152);
            this.mobileCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.mobileCheckBox.Name = "mobileCheckBox";
            this.mobileCheckBox.Size = new System.Drawing.Size(18, 17);
            this.mobileCheckBox.TabIndex = 18;
            this.mobileCheckBox.CheckedChanged += new System.EventHandler(this.mobileCheckBox_CheckedChanged);
            // 
            // apkLabel
            // 
            this.apkLabel.AutoEllipsis = true;
            this.apkLabel.AutoSize = true;
            this.apkLabel.Location = new System.Drawing.Point(351, 182);
            this.apkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.apkLabel.Name = "apkLabel";
            this.apkLabel.Size = new System.Drawing.Size(39, 15);
            this.apkLabel.TabIndex = 20;
            this.apkLabel.Text = "APK:";
            // 
            // apkVersionLabel
            // 
            this.apkVersionLabel.AutoEllipsis = true;
            this.apkVersionLabel.AutoSize = true;
            this.apkVersionLabel.Location = new System.Drawing.Point(319, 217);
            this.apkVersionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.apkVersionLabel.Name = "apkVersionLabel";
            this.apkVersionLabel.Size = new System.Drawing.Size(69, 15);
            this.apkVersionLabel.TabIndex = 23;
            this.apkVersionLabel.Text = "APK版本:";
            // 
            // apkVersionValueLabel
            // 
            this.apkVersionValueLabel.AutoEllipsis = true;
            this.apkVersionValueLabel.AutoSize = true;
            this.apkVersionValueLabel.Location = new System.Drawing.Point(395, 217);
            this.apkVersionValueLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.apkVersionValueLabel.Name = "apkVersionValueLabel";
            this.apkVersionValueLabel.Size = new System.Drawing.Size(38, 15);
            this.apkVersionValueLabel.TabIndex = 24;
            this.apkVersionValueLabel.Text = "<空>";
            // 
            // apkFIleTextBox
            // 
            this.apkFIleTextBox.Enabled = false;
            this.apkFIleTextBox.Location = new System.Drawing.Point(398, 177);
            this.apkFIleTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.apkFIleTextBox.Name = "apkFIleTextBox";
            this.apkFIleTextBox.Size = new System.Drawing.Size(268, 25);
            this.apkFIleTextBox.TabIndex = 21;
            // 
            // openAPKButton
            // 
            this.openAPKButton.BackColor = System.Drawing.Color.LightGray;
            this.openAPKButton.Location = new System.Drawing.Point(668, 177);
            this.openAPKButton.Margin = new System.Windows.Forms.Padding(4);
            this.openAPKButton.Name = "openAPKButton";
            this.openAPKButton.Size = new System.Drawing.Size(41, 26);
            this.openAPKButton.TabIndex = 22;
            this.openAPKButton.Text = "...";
            this.openAPKButton.UseVisualStyleBackColor = false;
            this.openAPKButton.Click += new System.EventHandler(this.openAPKButton_Click);
            // 
            // mobClientPathTextBox
            // 
            this.mobClientPathTextBox.Location = new System.Drawing.Point(360, 45);
            this.mobClientPathTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.mobClientPathTextBox.Name = "mobClientPathTextBox";
            this.mobClientPathTextBox.Size = new System.Drawing.Size(311, 25);
            this.mobClientPathTextBox.TabIndex = 26;
            this.mobClientPathTextBox.TextChanged += new System.EventHandler(this.mobClientPathTextBox_TextChanged);
            // 
            // mobClientPathLabel
            // 
            this.mobClientPathLabel.AutoEllipsis = true;
            this.mobClientPathLabel.AutoSize = true;
            this.mobClientPathLabel.Location = new System.Drawing.Point(358, 18);
            this.mobClientPathLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.mobClientPathLabel.Name = "mobClientPathLabel";
            this.mobClientPathLabel.Size = new System.Drawing.Size(105, 15);
            this.mobClientPathLabel.TabIndex = 25;
            this.mobClientPathLabel.Text = "手游补丁目录:";
            // 
            // mobPatchSavePathLabel
            // 
            this.mobPatchSavePathLabel.AutoEllipsis = true;
            this.mobPatchSavePathLabel.AutoSize = true;
            this.mobPatchSavePathLabel.Location = new System.Drawing.Point(358, 82);
            this.mobPatchSavePathLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.mobPatchSavePathLabel.Name = "mobPatchSavePathLabel";
            this.mobPatchSavePathLabel.Size = new System.Drawing.Size(105, 15);
            this.mobPatchSavePathLabel.TabIndex = 27;
            this.mobPatchSavePathLabel.Text = "手游保存目录:";
            // 
            // mobUploadPatchButton
            // 
            this.mobUploadPatchButton.Location = new System.Drawing.Point(360, 317);
            this.mobUploadPatchButton.Margin = new System.Windows.Forms.Padding(4);
            this.mobUploadPatchButton.Name = "mobUploadPatchButton";
            this.mobUploadPatchButton.Size = new System.Drawing.Size(247, 34);
            this.mobUploadPatchButton.TabIndex = 35;
            this.mobUploadPatchButton.Text = "生成手游补丁";
            this.mobUploadPatchButton.Click += new System.EventHandler(this.mobUploadPatchButton_Click);
            // 
            // openBaseZipButton
            // 
            this.openBaseZipButton.BackColor = System.Drawing.Color.LightGray;
            this.openBaseZipButton.Location = new System.Drawing.Point(668, 242);
            this.openBaseZipButton.Margin = new System.Windows.Forms.Padding(4);
            this.openBaseZipButton.Name = "openBaseZipButton";
            this.openBaseZipButton.Size = new System.Drawing.Size(41, 26);
            this.openBaseZipButton.TabIndex = 38;
            this.openBaseZipButton.Text = "...";
            this.openBaseZipButton.UseVisualStyleBackColor = false;
            this.openBaseZipButton.Click += new System.EventHandler(this.openBaseZipButton_Click);
            // 
            // baseZipTextBox
            // 
            this.baseZipTextBox.Enabled = false;
            this.baseZipTextBox.Location = new System.Drawing.Point(398, 242);
            this.baseZipTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.baseZipTextBox.Name = "baseZipTextBox";
            this.baseZipTextBox.Size = new System.Drawing.Size(268, 25);
            this.baseZipTextBox.TabIndex = 37;
            // 
            // baseZipLabel
            // 
            this.baseZipLabel.AutoEllipsis = true;
            this.baseZipLabel.AutoSize = true;
            this.baseZipLabel.Location = new System.Drawing.Point(327, 247);
            this.baseZipLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseZipLabel.Name = "baseZipLabel";
            this.baseZipLabel.Size = new System.Drawing.Size(60, 15);
            this.baseZipLabel.TabIndex = 36;
            this.baseZipLabel.Text = "基础包:";
            // 
            // pcPatchSavePathTextBox
            // 
            this.pcPatchSavePathTextBox.Location = new System.Drawing.Point(15, 112);
            this.pcPatchSavePathTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.pcPatchSavePathTextBox.Name = "pcPatchSavePathTextBox";
            this.pcPatchSavePathTextBox.Size = new System.Drawing.Size(311, 25);
            this.pcPatchSavePathTextBox.TabIndex = 40;
            this.pcPatchSavePathTextBox.TextChanged += new System.EventHandler(this.pcPatchSavePathTextBox_TextChanged);
            // 
            // mobPatchSavePathTextBox
            // 
            this.mobPatchSavePathTextBox.Location = new System.Drawing.Point(360, 110);
            this.mobPatchSavePathTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.mobPatchSavePathTextBox.Name = "mobPatchSavePathTextBox";
            this.mobPatchSavePathTextBox.Size = new System.Drawing.Size(311, 25);
            this.mobPatchSavePathTextBox.TabIndex = 42;
            this.mobPatchSavePathTextBox.TextChanged += new System.EventHandler(this.mobPatchSavePathTextBox_TextChanged);
            // 
            // pcUploadPatchButton
            // 
            this.pcUploadPatchButton.Location = new System.Drawing.Point(16, 317);
            this.pcUploadPatchButton.Margin = new System.Windows.Forms.Padding(4);
            this.pcUploadPatchButton.Name = "pcUploadPatchButton";
            this.pcUploadPatchButton.Size = new System.Drawing.Size(247, 34);
            this.pcUploadPatchButton.TabIndex = 10;
            this.pcUploadPatchButton.Text = "生成端游补丁";
            this.pcUploadPatchButton.Click += new System.EventHandler(this.pcUploadPatchButton_Click);
            // 
            // directorySearcher1
            // 
            this.directorySearcher1.ClientTimeout = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerPageTimeLimit = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerTimeLimit = System.TimeSpan.Parse("-00:00:01");
            // 
            // PMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 451);
            this.Controls.Add(this.mobPatchSavePathTextBox);
            this.Controls.Add(this.openBaseZipButton);
            this.Controls.Add(this.baseZipTextBox);
            this.Controls.Add(this.baseZipLabel);
            this.Controls.Add(this.mobUploadPatchButton);
            this.Controls.Add(this.mobClientPathTextBox);
            this.Controls.Add(this.mobClientPathLabel);
            this.Controls.Add(this.apkVersionValueLabel);
            this.Controls.Add(this.apkVersionLabel);
            this.Controls.Add(this.openAPKButton);
            this.Controls.Add(this.apkFIleTextBox);
            this.Controls.Add(this.apkLabel);
            this.Controls.Add(this.mobileLabel);
            this.Controls.Add(this.mobileCheckBox);
            this.Controls.Add(this.SaveSpeedLabel);
            this.Controls.Add(this.labelControl10);
            this.Controls.Add(this.TotalProgressBar);
            this.Controls.Add(this.SaveSizeLabel);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.pcUploadPatchButton);
            this.Controls.Add(this.pcClientPathTextBox);
            this.Controls.Add(this.pcClientPathLabel);
            this.Controls.Add(this.pcPatchSavePathTextBox);
            this.Controls.Add(this.pcPatchSavePathLabel);
            this.Controls.Add(this.mobPatchSavePathLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "补丁管理器";
            this.Load += new System.EventHandler(this.PMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label pcClientPathLabel;
        private System.Windows.Forms.TextBox pcClientPathTextBox;
        private System.Windows.Forms.Label pcPatchSavePathLabel;
        private System.Windows.Forms.Button pcUploadPatchButton;
        private System.Windows.Forms.Label labelControl6;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Label labelControl8;
        private System.Windows.Forms.Label SaveSizeLabel;
        private System.Windows.Forms.ProgressBar TotalProgressBar;
        private System.Windows.Forms.Label SaveSpeedLabel;
        private System.Windows.Forms.Label labelControl10;
        private System.Windows.Forms.FolderBrowserDialog FolderDialog;
        private System.Windows.Forms.Label mobileLabel;
        private System.Windows.Forms.CheckBox mobileCheckBox;
        private System.Windows.Forms.Label apkLabel;
        private System.Windows.Forms.Label apkVersionLabel;
        private System.Windows.Forms.Label apkVersionValueLabel;
        private System.Windows.Forms.TextBox apkFIleTextBox;
        private System.Windows.Forms.Button openAPKButton;
        private System.Windows.Forms.TextBox mobClientPathTextBox;
        private System.Windows.Forms.Label mobClientPathLabel;
        private System.Windows.Forms.Label mobPatchSavePathLabel;
        private System.Windows.Forms.Button mobUploadPatchButton;
        private System.Windows.Forms.Button openBaseZipButton;
        private System.Windows.Forms.TextBox baseZipTextBox;
        private System.Windows.Forms.Label baseZipLabel;
        private System.Windows.Forms.TextBox pcPatchSavePathTextBox;
        private System.Windows.Forms.TextBox mobPatchSavePathTextBox;
        private System.DirectoryServices.DirectorySearcher directorySearcher1;
    }
}

