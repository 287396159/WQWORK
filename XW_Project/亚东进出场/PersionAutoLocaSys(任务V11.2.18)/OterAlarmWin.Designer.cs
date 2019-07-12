namespace PersionAutoLocaSys
{
    partial class OterAlarmWin
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
            this.SearchBtn = new System.Windows.Forms.Button();
            this.SearchTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.HandlerAlarmBtn = new System.Windows.Forms.Button();
            this.ClearAlarmTagsBtn = new System.Windows.Forms.Button();
            this.PsAlarmAllSelecCB = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BattLowListView = new BuffListView();
            this.TagCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AreaCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RouterCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CurBatteryCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AlarmCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ClearAlarmTimeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AlarmHandlerCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AlarmControlTab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.TagDisListView = new BuffListView();
            this.TagDisCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TagAreaDisCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TagRouterDisCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TagBatDisCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TagAlarmDisCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TagClrAlarmDisCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TagHandlerDisCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.ReferDisListView = new BuffListView();
            this.ReportRouterCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ReportRouterAreaCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ReportRouterSTCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ReportRouterRTCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ReportClearDTCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ReportRouterHandlerCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.NodeDisListView = new BuffListView();
            this.NodeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NodeAreaCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NodeSPCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AlarmTCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AlarmClsCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IsHandleCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AlarmControlTab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // SearchBtn
            // 
            this.SearchBtn.BackColor = System.Drawing.Color.White;
            this.SearchBtn.Location = new System.Drawing.Point(410, 8);
            this.SearchBtn.Name = "SearchBtn";
            this.SearchBtn.Size = new System.Drawing.Size(93, 28);
            this.SearchBtn.TabIndex = 15;
            this.SearchBtn.Text = "Search";
            this.SearchBtn.UseVisualStyleBackColor = false;
            this.SearchBtn.Click += new System.EventHandler(this.SearchBtn_Click);
            // 
            // SearchTB
            // 
            this.SearchTB.Location = new System.Drawing.Point(302, 12);
            this.SearchTB.Name = "SearchTB";
            this.SearchTB.Size = new System.Drawing.Size(100, 21);
            this.SearchTB.TabIndex = 14;
            this.SearchTB.TextChanged += new System.EventHandler(this.SearchTB_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(256, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "查找：";
            // 
            // HandlerAlarmBtn
            // 
            this.HandlerAlarmBtn.BackColor = System.Drawing.Color.White;
            this.HandlerAlarmBtn.Location = new System.Drawing.Point(728, 8);
            this.HandlerAlarmBtn.Name = "HandlerAlarmBtn";
            this.HandlerAlarmBtn.Size = new System.Drawing.Size(93, 28);
            this.HandlerAlarmBtn.TabIndex = 12;
            this.HandlerAlarmBtn.Text = "處理警報";
            this.HandlerAlarmBtn.UseVisualStyleBackColor = false;
            this.HandlerAlarmBtn.Click += new System.EventHandler(this.HandlerAlarmBtn_Click);
            // 
            // ClearAlarmTagsBtn
            // 
            this.ClearAlarmTagsBtn.BackColor = System.Drawing.Color.White;
            this.ClearAlarmTagsBtn.Location = new System.Drawing.Point(827, 8);
            this.ClearAlarmTagsBtn.Name = "ClearAlarmTagsBtn";
            this.ClearAlarmTagsBtn.Size = new System.Drawing.Size(93, 28);
            this.ClearAlarmTagsBtn.TabIndex = 11;
            this.ClearAlarmTagsBtn.Text = "清除警報";
            this.ClearAlarmTagsBtn.UseVisualStyleBackColor = false;
            this.ClearAlarmTagsBtn.Click += new System.EventHandler(this.ClearAlarmTagsBtn_Click);
            // 
            // PsAlarmAllSelecCB
            // 
            this.PsAlarmAllSelecCB.AutoSize = true;
            this.PsAlarmAllSelecCB.Location = new System.Drawing.Point(12, 14);
            this.PsAlarmAllSelecCB.Name = "PsAlarmAllSelecCB";
            this.PsAlarmAllSelecCB.Size = new System.Drawing.Size(48, 16);
            this.PsAlarmAllSelecCB.TabIndex = 10;
            this.PsAlarmAllSelecCB.Text = "全選";
            this.PsAlarmAllSelecCB.UseVisualStyleBackColor = true;
            this.PsAlarmAllSelecCB.CheckedChanged += new System.EventHandler(this.PsAlarmAllSelecCB_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "報警資訊數量：0";
            // 
            // BattLowListView
            // 
            this.BattLowListView.CheckBoxes = true;
            this.BattLowListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TagCol,
            this.AreaCol,
            this.RouterCol,
            this.CurBatteryCol,
            this.AlarmCol,
            this.ClearAlarmTimeCol,
            this.AlarmHandlerCol});
            this.BattLowListView.FullRowSelect = true;
            this.BattLowListView.GridLines = true;
            this.BattLowListView.Location = new System.Drawing.Point(8, 11);
            this.BattLowListView.Name = "BattLowListView";
            this.BattLowListView.Size = new System.Drawing.Size(890, 459);
            this.BattLowListView.TabIndex = 8;
            this.BattLowListView.UseCompatibleStateImageBehavior = false;
            this.BattLowListView.View = System.Windows.Forms.View.Details;
            this.BattLowListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.PersonAlarmListView_ItemChecked);
            // 
            // TagCol
            // 
            this.TagCol.Text = "卡片";
            this.TagCol.Width = 137;
            // 
            // AreaCol
            // 
            this.AreaCol.Text = "所在區域";
            this.AreaCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AreaCol.Width = 151;
            // 
            // RouterCol
            // 
            this.RouterCol.Text = "參考點";
            this.RouterCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RouterCol.Width = 144;
            // 
            // CurBatteryCol
            // 
            this.CurBatteryCol.Text = "當前電量";
            this.CurBatteryCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CurBatteryCol.Width = 80;
            // 
            // AlarmCol
            // 
            this.AlarmCol.Text = "警報產生時間";
            this.AlarmCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AlarmCol.Width = 161;
            // 
            // ClearAlarmTimeCol
            // 
            this.ClearAlarmTimeCol.Text = "清除警報時間";
            this.ClearAlarmTimeCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ClearAlarmTimeCol.Width = 130;
            // 
            // AlarmHandlerCol
            // 
            this.AlarmHandlerCol.Text = "是否處理";
            this.AlarmHandlerCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AlarmHandlerCol.Width = 80;
            // 
            // AlarmControlTab
            // 
            this.AlarmControlTab.Controls.Add(this.tabPage1);
            this.AlarmControlTab.Controls.Add(this.tabPage2);
            this.AlarmControlTab.Controls.Add(this.tabPage3);
            this.AlarmControlTab.Controls.Add(this.tabPage4);
            this.AlarmControlTab.Location = new System.Drawing.Point(12, 40);
            this.AlarmControlTab.Name = "AlarmControlTab";
            this.AlarmControlTab.SelectedIndex = 0;
            this.AlarmControlTab.Size = new System.Drawing.Size(912, 502);
            this.AlarmControlTab.TabIndex = 16;
            this.AlarmControlTab.SelectedIndexChanged += new System.EventHandler(this.AlarmControlTab_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.BattLowListView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(904, 476);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "電量不足報警列表";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.TagDisListView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(904, 476);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "卡片異常列表";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // TagDisListView
            // 
            this.TagDisListView.CheckBoxes = true;
            this.TagDisListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TagDisCol,
            this.TagAreaDisCol,
            this.TagRouterDisCol,
            this.TagBatDisCol,
            this.TagAlarmDisCol,
            this.TagClrAlarmDisCol,
            this.TagHandlerDisCol});
            this.TagDisListView.FullRowSelect = true;
            this.TagDisListView.GridLines = true;
            this.TagDisListView.Location = new System.Drawing.Point(8, 11);
            this.TagDisListView.Name = "TagDisListView";
            this.TagDisListView.Size = new System.Drawing.Size(890, 459);
            this.TagDisListView.TabIndex = 9;
            this.TagDisListView.UseCompatibleStateImageBehavior = false;
            this.TagDisListView.View = System.Windows.Forms.View.Details;
            // 
            // TagDisCol
            // 
            this.TagDisCol.Text = "卡片";
            this.TagDisCol.Width = 124;
            // 
            // TagAreaDisCol
            // 
            this.TagAreaDisCol.Text = "所在區域";
            this.TagAreaDisCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TagAreaDisCol.Width = 156;
            // 
            // TagRouterDisCol
            // 
            this.TagRouterDisCol.Text = "参考点";
            this.TagRouterDisCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TagRouterDisCol.Width = 148;
            // 
            // TagBatDisCol
            // 
            this.TagBatDisCol.Text = "休眠時間";
            this.TagBatDisCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TagBatDisCol.Width = 92;
            // 
            // TagAlarmDisCol
            // 
            this.TagAlarmDisCol.Text = "警告產生时间";
            this.TagAlarmDisCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TagAlarmDisCol.Width = 155;
            // 
            // TagClrAlarmDisCol
            // 
            this.TagClrAlarmDisCol.Text = "清除警告時間";
            this.TagClrAlarmDisCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TagClrAlarmDisCol.Width = 114;
            // 
            // TagHandlerDisCol
            // 
            this.TagHandlerDisCol.Text = "是否處理";
            this.TagHandlerDisCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TagHandlerDisCol.Width = 80;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.ReferDisListView);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(904, 476);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "參考點異常列表";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // ReferDisListView
            // 
            this.ReferDisListView.CheckBoxes = true;
            this.ReferDisListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ReportRouterCol,
            this.ReportRouterAreaCol,
            this.ReportRouterSTCol,
            this.ReportRouterRTCol,
            this.ReportClearDTCol,
            this.ReportRouterHandlerCol});
            this.ReferDisListView.FullRowSelect = true;
            this.ReferDisListView.GridLines = true;
            this.ReferDisListView.Location = new System.Drawing.Point(8, 11);
            this.ReferDisListView.Name = "ReferDisListView";
            this.ReferDisListView.Size = new System.Drawing.Size(890, 459);
            this.ReferDisListView.TabIndex = 10;
            this.ReferDisListView.UseCompatibleStateImageBehavior = false;
            this.ReferDisListView.View = System.Windows.Forms.View.Details;
            this.ReferDisListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.RouterExcepionListView_ItemChecked);
            // 
            // ReportRouterCol
            // 
            this.ReportRouterCol.Text = "參考點";
            this.ReportRouterCol.Width = 124;
            // 
            // ReportRouterAreaCol
            // 
            this.ReportRouterAreaCol.Text = "所在區域";
            this.ReportRouterAreaCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ReportRouterAreaCol.Width = 167;
            // 
            // ReportRouterSTCol
            // 
            this.ReportRouterSTCol.Text = "休眠時間";
            this.ReportRouterSTCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ReportRouterSTCol.Width = 119;
            // 
            // ReportRouterRTCol
            // 
            this.ReportRouterRTCol.Text = "警告時間";
            this.ReportRouterRTCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ReportRouterRTCol.Width = 181;
            // 
            // ReportClearDTCol
            // 
            this.ReportClearDTCol.Text = "警告處理時間";
            this.ReportClearDTCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ReportClearDTCol.Width = 171;
            // 
            // ReportRouterHandlerCol
            // 
            this.ReportRouterHandlerCol.Text = "是否處理";
            this.ReportRouterHandlerCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ReportRouterHandlerCol.Width = 78;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.NodeDisListView);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(904, 476);
            this.tabPage4.TabIndex = 4;
            this.tabPage4.Text = "數據節點";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // NodeDisListView
            // 
            this.NodeDisListView.CheckBoxes = true;
            this.NodeDisListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NodeCol,
            this.NodeAreaCol,
            this.NodeSPCol,
            this.AlarmTCol,
            this.AlarmClsCol,
            this.IsHandleCol});
            this.NodeDisListView.FullRowSelect = true;
            this.NodeDisListView.GridLines = true;
            this.NodeDisListView.Location = new System.Drawing.Point(8, 11);
            this.NodeDisListView.Name = "NodeDisListView";
            this.NodeDisListView.Size = new System.Drawing.Size(890, 459);
            this.NodeDisListView.TabIndex = 12;
            this.NodeDisListView.UseCompatibleStateImageBehavior = false;
            this.NodeDisListView.View = System.Windows.Forms.View.Details;
            // 
            // NodeCol
            // 
            this.NodeCol.Text = "數據節點";
            this.NodeCol.Width = 107;
            // 
            // NodeAreaCol
            // 
            this.NodeAreaCol.Text = "所在區域";
            this.NodeAreaCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NodeAreaCol.Width = 145;
            // 
            // NodeSPCol
            // 
            this.NodeSPCol.Text = "休眠時間";
            this.NodeSPCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NodeSPCol.Width = 110;
            // 
            // AlarmTCol
            // 
            this.AlarmTCol.Text = "警告時間";
            this.AlarmTCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AlarmTCol.Width = 184;
            // 
            // AlarmClsCol
            // 
            this.AlarmClsCol.Text = "警告處理時間";
            this.AlarmClsCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AlarmClsCol.Width = 192;
            // 
            // IsHandleCol
            // 
            this.IsHandleCol.Text = "是否處理";
            this.IsHandleCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.IsHandleCol.Width = 125;
            // 
            // OterAlarmWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 554);
            this.Controls.Add(this.AlarmControlTab);
            this.Controls.Add(this.SearchBtn);
            this.Controls.Add(this.SearchTB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.HandlerAlarmBtn);
            this.Controls.Add(this.ClearAlarmTagsBtn);
            this.Controls.Add(this.PsAlarmAllSelecCB);
            this.Controls.Add(this.label1);
            this.Name = "OterAlarmWin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "電量不足\\未受控報警列表";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OterAlarmWin_FormClosing);
            this.Load += new System.EventHandler(this.OterAlarmWin_Load);
            this.AlarmControlTab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SearchBtn;
        private System.Windows.Forms.TextBox SearchTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button HandlerAlarmBtn;
        private System.Windows.Forms.Button ClearAlarmTagsBtn;
        private System.Windows.Forms.CheckBox PsAlarmAllSelecCB;
        private System.Windows.Forms.Label label1;
        public BuffListView BattLowListView;
        private System.Windows.Forms.ColumnHeader TagCol;
        private System.Windows.Forms.ColumnHeader AreaCol;
        private System.Windows.Forms.ColumnHeader RouterCol;
        private System.Windows.Forms.ColumnHeader AlarmCol;
        private System.Windows.Forms.ColumnHeader AlarmHandlerCol;
        private System.Windows.Forms.TabControl AlarmControlTab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ColumnHeader CurBatteryCol;
        private System.Windows.Forms.TabPage tabPage3;
        public BuffListView ReferDisListView;
        private System.Windows.Forms.ColumnHeader ReportRouterCol;
        private System.Windows.Forms.ColumnHeader ReportRouterAreaCol;
        private System.Windows.Forms.ColumnHeader ReportRouterSTCol;
        private System.Windows.Forms.ColumnHeader ReportRouterRTCol;
        private System.Windows.Forms.ColumnHeader ReportRouterHandlerCol;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ColumnHeader ClearAlarmTimeCol;
        public BuffListView TagDisListView;
        private System.Windows.Forms.ColumnHeader TagDisCol;
        private System.Windows.Forms.ColumnHeader TagAreaDisCol;
        private System.Windows.Forms.ColumnHeader TagRouterDisCol;
        private System.Windows.Forms.ColumnHeader TagBatDisCol;
        private System.Windows.Forms.ColumnHeader TagAlarmDisCol;
        private System.Windows.Forms.ColumnHeader TagClrAlarmDisCol;
        private System.Windows.Forms.ColumnHeader TagHandlerDisCol;
        private System.Windows.Forms.ColumnHeader ReportClearDTCol;
        public BuffListView NodeDisListView;
        private System.Windows.Forms.ColumnHeader NodeCol;
        private System.Windows.Forms.ColumnHeader NodeAreaCol;
        private System.Windows.Forms.ColumnHeader NodeSPCol;
        private System.Windows.Forms.ColumnHeader AlarmTCol;
        private System.Windows.Forms.ColumnHeader AlarmClsCol;
        private System.Windows.Forms.ColumnHeader IsHandleCol;
    }
}