namespace PersionAutoLocaSys
{
    partial class RegInfoWin
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
            this.ShowModeTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.AreaTaglistView = new BuffListView();
            this.TagIDCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TagNameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RouterCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SigStrenCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TagBatteryCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NoExeTimeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ReportTimeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ImagePanel = new System.Windows.Forms.Panel();
            this.ShowModeTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ShowModeTabControl
            // 
            this.ShowModeTabControl.Controls.Add(this.tabPage1);
            this.ShowModeTabControl.Controls.Add(this.tabPage2);
            this.ShowModeTabControl.Location = new System.Drawing.Point(0, 0);
            this.ShowModeTabControl.Name = "ShowModeTabControl";
            this.ShowModeTabControl.SelectedIndex = 0;
            this.ShowModeTabControl.Size = new System.Drawing.Size(939, 558);
            this.ShowModeTabControl.TabIndex = 0;
            this.ShowModeTabControl.SelectedIndexChanged += new System.EventHandler(this.ShowModeTabControl_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.AreaTaglistView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(931, 532);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "列表模式";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "人員當前總數：0";
            // 
            // AreaTaglistView
            // 
            this.AreaTaglistView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TagIDCol,
            this.TagNameCol,
            this.RouterCol,
            this.SigStrenCol,
            this.TagBatteryCol,
            this.NoExeTimeCol,
            this.ReportTimeCol});
            this.AreaTaglistView.FullRowSelect = true;
            this.AreaTaglistView.GridLines = true;
            this.AreaTaglistView.Location = new System.Drawing.Point(10, 33);
            this.AreaTaglistView.Name = "AreaTaglistView";
            this.AreaTaglistView.Size = new System.Drawing.Size(915, 493);
            this.AreaTaglistView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.AreaTaglistView.TabIndex = 0;
            this.AreaTaglistView.UseCompatibleStateImageBehavior = false;
            this.AreaTaglistView.View = System.Windows.Forms.View.Details;
            this.AreaTaglistView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.AreaTaglistView_ColumnClick);
            // 
            // TagIDCol
            // 
            this.TagIDCol.Text = "卡片ID(↓)";
            this.TagIDCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TagIDCol.Width = 92;
            // 
            // TagNameCol
            // 
            this.TagNameCol.Text = "卡片名稱";
            this.TagNameCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TagNameCol.Width = 174;
            // 
            // RouterCol
            // 
            this.RouterCol.Text = "參考點";
            this.RouterCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RouterCol.Width = 121;
            // 
            // SigStrenCol
            // 
            this.SigStrenCol.Text = "信號強度";
            this.SigStrenCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SigStrenCol.Width = 67;
            // 
            // TagBatteryCol
            // 
            this.TagBatteryCol.Text = "卡片電量%";
            this.TagBatteryCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TagBatteryCol.Width = 83;
            // 
            // NoExeTimeCol
            // 
            this.NoExeTimeCol.Text = "未移動時間sec";
            this.NoExeTimeCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NoExeTimeCol.Width = 116;
            // 
            // ReportTimeCol
            // 
            this.ReportTimeCol.Text = "數據上報時間";
            this.ReportTimeCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ReportTimeCol.Width = 239;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ImagePanel);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(931, 532);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "地圖模式";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ImagePanel
            // 
            this.ImagePanel.BackColor = System.Drawing.Color.White;
            this.ImagePanel.Location = new System.Drawing.Point(8, 6);
            this.ImagePanel.Name = "ImagePanel";
            this.ImagePanel.Size = new System.Drawing.Size(917, 520);
            this.ImagePanel.TabIndex = 0;
            this.ImagePanel.Click += new System.EventHandler(this.ImagePanel_Click);
            this.ImagePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.ImagePanel_Paint);
            // 
            // RegInfoWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 565);
            this.Controls.Add(this.ShowModeTabControl);
            this.Name = "RegInfoWin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "當前區域為：办公室(0001)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RegInfoWin_FormClosing);
            this.Load += new System.EventHandler(this.RegInfoWin_Load);
            this.ShowModeTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TabControl ShowModeTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private BuffListView AreaTaglistView;
        private System.Windows.Forms.ColumnHeader TagIDCol;
        private System.Windows.Forms.ColumnHeader RouterCol;
        private System.Windows.Forms.ColumnHeader SigStrenCol;
        private System.Windows.Forms.ColumnHeader TagBatteryCol;
        private System.Windows.Forms.ColumnHeader NoExeTimeCol;
        private System.Windows.Forms.ColumnHeader ReportTimeCol;
        private System.Windows.Forms.Panel ImagePanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ColumnHeader TagNameCol;

    }
}