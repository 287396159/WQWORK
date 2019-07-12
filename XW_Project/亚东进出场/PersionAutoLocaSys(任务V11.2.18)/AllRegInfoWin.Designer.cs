namespace PersionAutoLocaSys
{
    partial class AllRegInfoWin
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
            this.label1 = new System.Windows.Forms.Label();
            this.AreaSelectCB = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AllImageSearchTX = new System.Windows.Forms.TextBox();
            this.MapPsSearchBtn = new System.Windows.Forms.Button();
            this.IsTailCB = new System.Windows.Forms.CheckBox();
            this.AllPanel = new System.Windows.Forms.Panel();
            this.TrackBtn = new System.Windows.Forms.Button();
            this.AllRegTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ListModeSearchTX = new System.Windows.Forms.TextBox();
            this.ListModeSearchBt = new System.Windows.Forms.Button();
            this.AllTagListView = new PersionAutoLocaSys.BuffListView();
            this.TagCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TagNameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TagAreaCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RouterCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SigStrenCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TagBatteryCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NoExeTimeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ReportTimeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.GroupComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.totaltxt = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.refernumtxt = new System.Windows.Forms.Label();
            this.nodenumtxt = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.RoutersLV = new PersionAutoLocaSys.BuffListView();
            this.NodeColum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NodeNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NodeTypeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SleepTimeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VersionColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CnnStatusColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ReportTimeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.admissionLab = new System.Windows.Forms.Label();
            this.onLineLab = new System.Windows.Forms.Label();
            this.exitLab = new System.Windows.Forms.Label();
            this.daTagListView = new System.Windows.Forms.ListView();
            this.TagIDCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SDTimeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.workIDCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.WarmMsgBtn = new System.Windows.Forms.Button();
            this.selectpersonbtn = new System.Windows.Forms.Button();
            this.NodeTreePanal = new System.Windows.Forms.TreeView();
            this.label9 = new System.Windows.Forms.Label();
            this.AllRegTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(176, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "當前區域：";
            // 
            // AreaSelectCB
            // 
            this.AreaSelectCB.BackColor = System.Drawing.Color.White;
            this.AreaSelectCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AreaSelectCB.FormattingEnabled = true;
            this.AreaSelectCB.Location = new System.Drawing.Point(243, 10);
            this.AreaSelectCB.Name = "AreaSelectCB";
            this.AreaSelectCB.Size = new System.Drawing.Size(106, 20);
            this.AreaSelectCB.TabIndex = 1;
            this.AreaSelectCB.SelectedIndexChanged += new System.EventHandler(this.AreaSelectCB_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(373, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "人員查找：";
            // 
            // AllImageSearchTX
            // 
            this.AllImageSearchTX.Location = new System.Drawing.Point(445, 10);
            this.AllImageSearchTX.Name = "AllImageSearchTX";
            this.AllImageSearchTX.Size = new System.Drawing.Size(70, 21);
            this.AllImageSearchTX.TabIndex = 5;
            this.AllImageSearchTX.TextChanged += new System.EventHandler(this.AllImageSearchTX_TextChanged);
            // 
            // MapPsSearchBtn
            // 
            this.MapPsSearchBtn.BackColor = System.Drawing.Color.White;
            this.MapPsSearchBtn.Location = new System.Drawing.Point(523, 7);
            this.MapPsSearchBtn.Name = "MapPsSearchBtn";
            this.MapPsSearchBtn.Size = new System.Drawing.Size(93, 28);
            this.MapPsSearchBtn.TabIndex = 7;
            this.MapPsSearchBtn.Text = "Search";
            this.MapPsSearchBtn.UseVisualStyleBackColor = false;
            this.MapPsSearchBtn.Click += new System.EventHandler(this.MapPsSearchBtn_Click);
            // 
            // IsTailCB
            // 
            this.IsTailCB.AutoSize = true;
            this.IsTailCB.BackColor = System.Drawing.Color.Transparent;
            this.IsTailCB.ForeColor = System.Drawing.Color.Black;
            this.IsTailCB.Location = new System.Drawing.Point(626, 12);
            this.IsTailCB.Name = "IsTailCB";
            this.IsTailCB.Size = new System.Drawing.Size(72, 16);
            this.IsTailCB.TabIndex = 8;
            this.IsTailCB.Text = "是否跟踪";
            this.IsTailCB.UseVisualStyleBackColor = false;
            this.IsTailCB.CheckedChanged += new System.EventHandler(this.IsTailCB_CheckedChanged);
            // 
            // AllPanel
            // 
            this.AllPanel.BackColor = System.Drawing.Color.White;
            this.AllPanel.ForeColor = System.Drawing.Color.Black;
            this.AllPanel.Location = new System.Drawing.Point(6, 34);
            this.AllPanel.Name = "AllPanel";
            this.AllPanel.Size = new System.Drawing.Size(837, 478);
            this.AllPanel.TabIndex = 9;
            this.AllPanel.Click += new System.EventHandler(this.AllPanel_Click);
            this.AllPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.AllPanel_Paint);
            // 
            // TrackBtn
            // 
            this.TrackBtn.BackColor = System.Drawing.Color.White;
            this.TrackBtn.Location = new System.Drawing.Point(854, 24);
            this.TrackBtn.Name = "TrackBtn";
            this.TrackBtn.Size = new System.Drawing.Size(120, 34);
            this.TrackBtn.TabIndex = 10;
            this.TrackBtn.Text = "軌跡分析";
            this.TrackBtn.UseVisualStyleBackColor = false;
            this.TrackBtn.Click += new System.EventHandler(this.TrackBtn_Click);
            // 
            // AllRegTabControl
            // 
            this.AllRegTabControl.Controls.Add(this.tabPage1);
            this.AllRegTabControl.Controls.Add(this.tabPage2);
            this.AllRegTabControl.Controls.Add(this.tabPage3);
            this.AllRegTabControl.Controls.Add(this.tabPage4);
            this.AllRegTabControl.Location = new System.Drawing.Point(0, 3);
            this.AllRegTabControl.Name = "AllRegTabControl";
            this.AllRegTabControl.SelectedIndex = 0;
            this.AllRegTabControl.Size = new System.Drawing.Size(852, 546);
            this.AllRegTabControl.TabIndex = 0;
            this.AllRegTabControl.SelectedIndexChanged += new System.EventHandler(this.AllRegTabControl_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.ListModeSearchTX);
            this.tabPage1.Controls.Add(this.ListModeSearchBt);
            this.tabPage1.Controls.Add(this.AllTagListView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(844, 520);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "列表模式";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "人員當前總數：0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(307, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "人員查找：";
            // 
            // ListModeSearchTX
            // 
            this.ListModeSearchTX.Location = new System.Drawing.Point(379, 9);
            this.ListModeSearchTX.Name = "ListModeSearchTX";
            this.ListModeSearchTX.Size = new System.Drawing.Size(70, 21);
            this.ListModeSearchTX.TabIndex = 9;
            this.ListModeSearchTX.TextChanged += new System.EventHandler(this.ListModeSearchTX_TextChanged);
            // 
            // ListModeSearchBt
            // 
            this.ListModeSearchBt.BackColor = System.Drawing.Color.White;
            this.ListModeSearchBt.Location = new System.Drawing.Point(469, 6);
            this.ListModeSearchBt.Name = "ListModeSearchBt";
            this.ListModeSearchBt.Size = new System.Drawing.Size(93, 28);
            this.ListModeSearchBt.TabIndex = 11;
            this.ListModeSearchBt.Text = "Search";
            this.ListModeSearchBt.UseVisualStyleBackColor = false;
            this.ListModeSearchBt.Click += new System.EventHandler(this.ListModeSearchBt_Click);
            // 
            // AllTagListView
            // 
            this.AllTagListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TagCol,
            this.TagNameCol,
            this.TagAreaCol,
            this.RouterCol,
            this.SigStrenCol,
            this.TagBatteryCol,
            this.NoExeTimeCol,
            this.ReportTimeCol});
            this.AllTagListView.FullRowSelect = true;
            this.AllTagListView.GridLines = true;
            this.AllTagListView.Location = new System.Drawing.Point(10, 37);
            this.AllTagListView.Name = "AllTagListView";
            this.AllTagListView.Size = new System.Drawing.Size(826, 472);
            this.AllTagListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.AllTagListView.TabIndex = 1;
            this.AllTagListView.UseCompatibleStateImageBehavior = false;
            this.AllTagListView.View = System.Windows.Forms.View.Details;
            this.AllTagListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.AllTagListView_ColumnClick);
            // 
            // TagCol
            // 
            this.TagCol.Text = "卡片ID↓";
            this.TagCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TagCol.Width = 76;
            // 
            // TagNameCol
            // 
            this.TagNameCol.Text = "卡片名稱";
            this.TagNameCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TagNameCol.Width = 90;
            // 
            // TagAreaCol
            // 
            this.TagAreaCol.Text = "所在區域";
            this.TagAreaCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TagAreaCol.Width = 88;
            // 
            // RouterCol
            // 
            this.RouterCol.Text = "參考點";
            this.RouterCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RouterCol.Width = 75;
            // 
            // SigStrenCol
            // 
            this.SigStrenCol.Text = "信號強度";
            this.SigStrenCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SigStrenCol.Width = 88;
            // 
            // TagBatteryCol
            // 
            this.TagBatteryCol.Text = "卡片電量%";
            this.TagBatteryCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TagBatteryCol.Width = 90;
            // 
            // NoExeTimeCol
            // 
            this.NoExeTimeCol.Text = "未移動時間sec";
            this.NoExeTimeCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NoExeTimeCol.Width = 114;
            // 
            // ReportTimeCol
            // 
            this.ReportTimeCol.Text = "數據上報時間";
            this.ReportTimeCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ReportTimeCol.Width = 220;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.GroupComboBox);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.AreaSelectCB);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.AllPanel);
            this.tabPage2.Controls.Add(this.AllImageSearchTX);
            this.tabPage2.Controls.Add(this.IsTailCB);
            this.tabPage2.Controls.Add(this.MapPsSearchBtn);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(844, 520);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "圖形模式";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // GroupComboBox
            // 
            this.GroupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GroupComboBox.FormattingEnabled = true;
            this.GroupComboBox.Location = new System.Drawing.Point(61, 9);
            this.GroupComboBox.Name = "GroupComboBox";
            this.GroupComboBox.Size = new System.Drawing.Size(89, 20);
            this.GroupComboBox.TabIndex = 11;
            this.GroupComboBox.SelectedIndexChanged += new System.EventHandler(this.GroupComboBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "當前組：";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.totaltxt);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.refernumtxt);
            this.tabPage3.Controls.Add(this.nodenumtxt);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.RoutersLV);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(844, 520);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "參考點/數據節點信息";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // totaltxt
            // 
            this.totaltxt.AutoSize = true;
            this.totaltxt.Location = new System.Drawing.Point(85, 12);
            this.totaltxt.Name = "totaltxt";
            this.totaltxt.Size = new System.Drawing.Size(29, 12);
            this.totaltxt.TabIndex = 6;
            this.totaltxt.Text = "0 個";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 5;
            this.label8.Text = "總設備數量：";
            // 
            // refernumtxt
            // 
            this.refernumtxt.AutoSize = true;
            this.refernumtxt.Location = new System.Drawing.Point(424, 12);
            this.refernumtxt.Name = "refernumtxt";
            this.refernumtxt.Size = new System.Drawing.Size(29, 12);
            this.refernumtxt.TabIndex = 4;
            this.refernumtxt.Text = "0 個";
            // 
            // nodenumtxt
            // 
            this.nodenumtxt.AutoSize = true;
            this.nodenumtxt.Location = new System.Drawing.Point(262, 12);
            this.nodenumtxt.Name = "nodenumtxt";
            this.nodenumtxt.Size = new System.Drawing.Size(29, 12);
            this.nodenumtxt.TabIndex = 3;
            this.nodenumtxt.Text = "0 個";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(355, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "參考點數量：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(181, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "數據節點數量：";
            // 
            // RoutersLV
            // 
            this.RoutersLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NodeColum,
            this.NodeNameColumn,
            this.NodeTypeColumn,
            this.SleepTimeColumn,
            this.VersionColumn,
            this.CnnStatusColumn,
            this.ReportTimeColumn});
            this.RoutersLV.FullRowSelect = true;
            this.RoutersLV.GridLines = true;
            this.RoutersLV.Location = new System.Drawing.Point(4, 36);
            this.RoutersLV.Name = "RoutersLV";
            this.RoutersLV.Size = new System.Drawing.Size(830, 473);
            this.RoutersLV.TabIndex = 0;
            this.RoutersLV.UseCompatibleStateImageBehavior = false;
            this.RoutersLV.View = System.Windows.Forms.View.Details;
            this.RoutersLV.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.RoutersLV_ColumnClick);
            this.RoutersLV.Click += new System.EventHandler(this.RoutersLV_Click);
            // 
            // NodeColum
            // 
            this.NodeColum.Text = "節點ID";
            this.NodeColum.Width = 68;
            // 
            // NodeNameColumn
            // 
            this.NodeNameColumn.Text = "節點名稱";
            this.NodeNameColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NodeNameColumn.Width = 118;
            // 
            // NodeTypeColumn
            // 
            this.NodeTypeColumn.Text = "節點類型";
            this.NodeTypeColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NodeTypeColumn.Width = 89;
            // 
            // SleepTimeColumn
            // 
            this.SleepTimeColumn.Text = "休眠時間";
            this.SleepTimeColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SleepTimeColumn.Width = 65;
            // 
            // VersionColumn
            // 
            this.VersionColumn.Text = "版本信息";
            this.VersionColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.VersionColumn.Width = 114;
            // 
            // CnnStatusColumn
            // 
            this.CnnStatusColumn.Text = "狀態";
            this.CnnStatusColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CnnStatusColumn.Width = 85;
            // 
            // ReportTimeColumn
            // 
            this.ReportTimeColumn.Text = "上報時間";
            this.ReportTimeColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ReportTimeColumn.Width = 231;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button1);
            this.tabPage4.Controls.Add(this.checkBox3);
            this.tabPage4.Controls.Add(this.checkBox2);
            this.tabPage4.Controls.Add(this.checkBox1);
            this.tabPage4.Controls.Add(this.admissionLab);
            this.tabPage4.Controls.Add(this.onLineLab);
            this.tabPage4.Controls.Add(this.exitLab);
            this.tabPage4.Controls.Add(this.daTagListView);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(844, 520);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "進出廠記錄";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(741, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "進出廠歷史記錄";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(501, 11);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(84, 16);
            this.checkBox3.TabIndex = 18;
            this.checkBox3.Text = "在廠記錄：";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(283, 8);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(84, 16);
            this.checkBox2.TabIndex = 17;
            this.checkBox2.Text = "出廠記錄：";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(40, 10);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(84, 16);
            this.checkBox1.TabIndex = 16;
            this.checkBox1.Text = "入廠記錄：";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // admissionLab
            // 
            this.admissionLab.AutoSize = true;
            this.admissionLab.Location = new System.Drawing.Point(130, 11);
            this.admissionLab.Name = "admissionLab";
            this.admissionLab.Size = new System.Drawing.Size(29, 12);
            this.admissionLab.TabIndex = 15;
            this.admissionLab.Text = "0 個";
            // 
            // onLineLab
            // 
            this.onLineLab.AutoSize = true;
            this.onLineLab.Location = new System.Drawing.Point(585, 12);
            this.onLineLab.Name = "onLineLab";
            this.onLineLab.Size = new System.Drawing.Size(29, 12);
            this.onLineLab.TabIndex = 13;
            this.onLineLab.Text = "0 個";
            // 
            // exitLab
            // 
            this.exitLab.AutoSize = true;
            this.exitLab.Location = new System.Drawing.Point(367, 10);
            this.exitLab.Name = "exitLab";
            this.exitLab.Size = new System.Drawing.Size(29, 12);
            this.exitLab.TabIndex = 12;
            this.exitLab.Text = "0 個";
            // 
            // daTagListView
            // 
            this.daTagListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TagIDCol,
            this.columnHeader1,
            this.SDTimeCol,
            this.columnHeader2,
            this.workIDCol});
            this.daTagListView.FullRowSelect = true;
            this.daTagListView.GridLines = true;
            this.daTagListView.Location = new System.Drawing.Point(6, 38);
            this.daTagListView.Name = "daTagListView";
            this.daTagListView.Size = new System.Drawing.Size(833, 477);
            this.daTagListView.TabIndex = 9;
            this.daTagListView.UseCompatibleStateImageBehavior = false;
            this.daTagListView.View = System.Windows.Forms.View.Details;
            // 
            // TagIDCol
            // 
            this.TagIDCol.Text = "ID";
            this.TagIDCol.Width = 76;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "名稱";
            this.columnHeader1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader1.Width = 105;
            // 
            // SDTimeCol
            // 
            this.SDTimeCol.Text = "入廠時間";
            this.SDTimeCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SDTimeCol.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "出廠時間";
            this.columnHeader2.Width = 150;
            // 
            // workIDCol
            // 
            this.workIDCol.Text = "打卡ID";
            this.workIDCol.Width = 234;
            // 
            // WarmMsgBtn
            // 
            this.WarmMsgBtn.BackColor = System.Drawing.Color.White;
            this.WarmMsgBtn.Location = new System.Drawing.Point(853, 64);
            this.WarmMsgBtn.Name = "WarmMsgBtn";
            this.WarmMsgBtn.Size = new System.Drawing.Size(120, 34);
            this.WarmMsgBtn.TabIndex = 13;
            this.WarmMsgBtn.Text = "警报资讯";
            this.WarmMsgBtn.UseVisualStyleBackColor = false;
            this.WarmMsgBtn.Click += new System.EventHandler(this.WarmMsgBtn_Click);
            // 
            // selectpersonbtn
            // 
            this.selectpersonbtn.BackColor = System.Drawing.Color.White;
            this.selectpersonbtn.Location = new System.Drawing.Point(854, 106);
            this.selectpersonbtn.Name = "selectpersonbtn";
            this.selectpersonbtn.Size = new System.Drawing.Size(120, 34);
            this.selectpersonbtn.TabIndex = 14;
            this.selectpersonbtn.Text = "查看人員操作";
            this.selectpersonbtn.UseVisualStyleBackColor = false;
            this.selectpersonbtn.Click += new System.EventHandler(this.selectpersonbtn_Click);
            // 
            // NodeTreePanal
            // 
            this.NodeTreePanal.Location = new System.Drawing.Point(853, 173);
            this.NodeTreePanal.Name = "NodeTreePanal";
            this.NodeTreePanal.Size = new System.Drawing.Size(121, 372);
            this.NodeTreePanal.TabIndex = 13;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(858, 152);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(122, 12);
            this.label9.TabIndex = 15;
            this.label9.Text = "參考點連接樹狀圖：";
            // 
            // AllRegInfoWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(980, 553);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.NodeTreePanal);
            this.Controls.Add(this.selectpersonbtn);
            this.Controls.Add(this.WarmMsgBtn);
            this.Controls.Add(this.AllRegTabControl);
            this.Controls.Add(this.TrackBtn);
            this.Name = "AllRegInfoWin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "總區域觀察窗口";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AllRegInfoWin_FormClosing);
            this.Load += new System.EventHandler(this.AllRegInfoWin_Load);
            this.AllRegTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox AreaSelectCB;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox AllImageSearchTX;
        private System.Windows.Forms.Button MapPsSearchBtn;
        public System.Windows.Forms.CheckBox IsTailCB;
        private System.Windows.Forms.Panel AllPanel;
        private System.Windows.Forms.Button TrackBtn;

        public System.Windows.Forms.TabControl AllRegTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private BuffListView AllTagListView;
        private System.Windows.Forms.ColumnHeader TagCol;
        private System.Windows.Forms.ColumnHeader TagAreaCol;
        private System.Windows.Forms.ColumnHeader RouterCol;
        private System.Windows.Forms.ColumnHeader SigStrenCol;
        private System.Windows.Forms.ColumnHeader TagBatteryCol;
        private System.Windows.Forms.ColumnHeader NoExeTimeCol;
        private System.Windows.Forms.ColumnHeader ReportTimeCol;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ListModeSearchTX;
        private System.Windows.Forms.Button ListModeSearchBt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.ComboBox GroupComboBox;
  
        private System.Windows.Forms.Button WarmMsgBtn;
        private System.Windows.Forms.TabPage tabPage3;
        private BuffListView RoutersLV;
        private System.Windows.Forms.ColumnHeader NodeColum;
        private System.Windows.Forms.ColumnHeader NodeTypeColumn;
        private System.Windows.Forms.ColumnHeader SleepTimeColumn;
        private System.Windows.Forms.ColumnHeader VersionColumn;
        private System.Windows.Forms.ColumnHeader CnnStatusColumn;
        private System.Windows.Forms.ColumnHeader ReportTimeColumn;
        private System.Windows.Forms.ColumnHeader TagNameCol;
        private System.Windows.Forms.ColumnHeader NodeNameColumn;
        private System.Windows.Forms.Button selectpersonbtn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label refernumtxt;
        private System.Windows.Forms.Label nodenumtxt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label totaltxt;
        private System.Windows.Forms.TreeView NodeTreePanal;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label admissionLab;
        private System.Windows.Forms.Label onLineLab;
        private System.Windows.Forms.Label exitLab;
        public System.Windows.Forms.ListView daTagListView;
        public System.Windows.Forms.ColumnHeader TagIDCol;
        public System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader SDTimeCol;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader workIDCol;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Button button1;
    }
}