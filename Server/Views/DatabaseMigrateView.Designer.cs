namespace Server.Views
{
    partial class DatabaseMigrateView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.LogTextBox = new DevExpress.XtraEditors.MemoEdit();
            this.SourceDBPathTextBox = new DevExpress.XtraEditors.TextEdit();
            this.SelectSourceDBButton = new DevExpress.XtraEditors.SimpleButton();
            this.StartMigrateButton = new DevExpress.XtraEditors.SimpleButton();
            this.CancelButton = new DevExpress.XtraEditors.SimpleButton();
            this.CustomPrefixTextBox = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogTextBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SourceDBPathTextBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomPrefixTextBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.ribbon.SearchEditItem,
            this.barButtonItem1});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 2;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbon.Size = new System.Drawing.Size(898, 158);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 1;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "数据库迁移";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "操作";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 553);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(898, 24);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.LogTextBox);
            this.layoutControl1.Controls.Add(this.SourceDBPathTextBox);
            this.layoutControl1.Controls.Add(this.SelectSourceDBButton);
            this.layoutControl1.Controls.Add(this.StartMigrateButton);
            this.layoutControl1.Controls.Add(this.CancelButton);
            this.layoutControl1.Controls.Add(this.CustomPrefixTextBox);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 158);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(898, 395);
            this.layoutControl1.TabIndex = 2;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // LogTextBox
            // 
            this.LogTextBox.Location = new System.Drawing.Point(12, 98);
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.Properties.ReadOnly = true;
            this.LogTextBox.Size = new System.Drawing.Size(874, 285);
            this.LogTextBox.StyleController = this.layoutControl1;
            this.LogTextBox.TabIndex = 4;
            // 
            // SourceDBPathTextBox
            // 
            this.SourceDBPathTextBox.Location = new System.Drawing.Point(77, 12);
            this.SourceDBPathTextBox.Name = "SourceDBPathTextBox";
            this.SourceDBPathTextBox.Properties.ReadOnly = true;
            this.SourceDBPathTextBox.Size = new System.Drawing.Size(728, 20);
            this.SourceDBPathTextBox.StyleController = this.layoutControl1;
            this.SourceDBPathTextBox.TabIndex = 5;
            // 
            // SelectSourceDBButton
            // 
            this.SelectSourceDBButton.Location = new System.Drawing.Point(809, 12);
            this.SelectSourceDBButton.Name = "SelectSourceDBButton";
            this.SelectSourceDBButton.Size = new System.Drawing.Size(77, 22);
            this.SelectSourceDBButton.StyleController = this.layoutControl1;
            this.SelectSourceDBButton.TabIndex = 6;
            this.SelectSourceDBButton.Text = "选择";
            this.SelectSourceDBButton.Click += new System.EventHandler(this.SelectSourceDBButton_Click);
            // 
            // StartMigrateButton
            // 
            this.StartMigrateButton.Location = new System.Drawing.Point(718, 72);
            this.StartMigrateButton.Name = "StartMigrateButton";
            this.StartMigrateButton.Size = new System.Drawing.Size(87, 22);
            this.StartMigrateButton.StyleController = this.layoutControl1;
            this.StartMigrateButton.TabIndex = 7;
            this.StartMigrateButton.Text = "开始迁移";
            this.StartMigrateButton.Click += new System.EventHandler(this.StartMigrateButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(809, 72);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(77, 22);
            this.CancelButton.StyleController = this.layoutControl1;
            this.CancelButton.TabIndex = 8;
            this.CancelButton.Text = "取消";
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // CustomPrefixTextBox
            // 
            this.CustomPrefixTextBox.Location = new System.Drawing.Point(77, 36);
            this.CustomPrefixTextBox.Name = "CustomPrefixTextBox";
            this.CustomPrefixTextBox.Properties.AdvancedModeOptions.Label = "自定义前缀";
            this.CustomPrefixTextBox.Size = new System.Drawing.Size(809, 20);
            this.CustomPrefixTextBox.StyleController = this.layoutControl1;
            this.CustomPrefixTextBox.TabIndex = 9;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(898, 395);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.LogTextBox;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 86);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(878, 289);
            this.layoutControlItem1.Text = "日志信息";
            this.layoutControlItem1.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(62, 14);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.SourceDBPathTextBox;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(797, 24);
            this.layoutControlItem2.Text = "源数据库：";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(62, 14);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.SelectSourceDBButton;
            this.layoutControlItem3.Location = new System.Drawing.Point(797, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(81, 24);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.StartMigrateButton;
            this.layoutControlItem4.Location = new System.Drawing.Point(706, 60);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(91, 26);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.CancelButton;
            this.layoutControlItem5.Location = new System.Drawing.Point(797, 60);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(81, 26);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.CustomPrefixTextBox;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(878, 36);
            this.layoutControlItem6.Text = "自定义前缀：";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(62, 14);
            // 
            // DatabaseMigrateView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 577);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.Name = "DatabaseMigrateView";
            this.Ribbon = this.ribbon;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "数据库迁移工具";
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LogTextBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SourceDBPathTextBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomPrefixTextBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.MemoEdit LogTextBox;
        private DevExpress.XtraEditors.TextEdit SourceDBPathTextBox;
        private DevExpress.XtraEditors.SimpleButton SelectSourceDBButton;
        private DevExpress.XtraEditors.SimpleButton StartMigrateButton;
        private DevExpress.XtraEditors.SimpleButton CancelButton;
        private DevExpress.XtraEditors.TextEdit CustomPrefixTextBox;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
    }
}