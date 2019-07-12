namespace PersionAutoLocaSys
{
    partial class WarmMsg
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.xlsRB = new System.Windows.Forms.RadioButton();
            this.txtRB = new System.Windows.Forms.RadioButton();
            this.AlarmMsgOutBtn = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.AlarmTypeCB = new System.Windows.Forms.ComboBox();
            this.StartBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.eMinitueCB = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.eHourCB = new System.Windows.Forms.ComboBox();
            this.EndDTPicker = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.sMinitueCb = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.sHourCb = new System.Windows.Forms.ComboBox();
            this.StartDTPicker = new System.Windows.Forms.DateTimePicker();
            this.WarmListView = new System.Windows.Forms.ListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.xlsRB);
            this.groupBox3.Controls.Add(this.txtRB);
            this.groupBox3.Controls.Add(this.AlarmMsgOutBtn);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.AlarmTypeCB);
            this.groupBox3.Controls.Add(this.StartBtn);
            this.groupBox3.Location = new System.Drawing.Point(13, 69);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(675, 51);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "基本操作";
            // 
            // xlsRB
            // 
            this.xlsRB.AutoSize = true;
            this.xlsRB.Location = new System.Drawing.Point(526, 23);
            this.xlsRB.Name = "xlsRB";
            this.xlsRB.Size = new System.Drawing.Size(47, 16);
            this.xlsRB.TabIndex = 12;
            this.xlsRB.TabStop = true;
            this.xlsRB.Text = ".xls";
            this.xlsRB.UseVisualStyleBackColor = true;
            // 
            // txtRB
            // 
            this.txtRB.AutoSize = true;
            this.txtRB.Checked = true;
            this.txtRB.Location = new System.Drawing.Point(473, 23);
            this.txtRB.Name = "txtRB";
            this.txtRB.Size = new System.Drawing.Size(47, 16);
            this.txtRB.TabIndex = 11;
            this.txtRB.TabStop = true;
            this.txtRB.Text = ".txt";
            this.txtRB.UseVisualStyleBackColor = true;
            // 
            // AlarmMsgOutBtn
            // 
            this.AlarmMsgOutBtn.BackColor = System.Drawing.Color.White;
            this.AlarmMsgOutBtn.Location = new System.Drawing.Point(368, 13);
            this.AlarmMsgOutBtn.Name = "AlarmMsgOutBtn";
            this.AlarmMsgOutBtn.Size = new System.Drawing.Size(90, 31);
            this.AlarmMsgOutBtn.TabIndex = 10;
            this.AlarmMsgOutBtn.Text = "记录导出";
            this.AlarmMsgOutBtn.UseVisualStyleBackColor = false;
            this.AlarmMsgOutBtn.Click += new System.EventHandler(this.AlarmMsgOutBtn_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 9;
            this.label7.Text = "警報類型：";
            // 
            // AlarmTypeCB
            // 
            this.AlarmTypeCB.DisplayMember = "0";
            this.AlarmTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AlarmTypeCB.FormattingEnabled = true;
            this.AlarmTypeCB.Items.AddRange(new object[] {
            "全部",
            "低電量",
            "人員求救",
            "區域管制",
            "人員未移動",
            "卡片異常",
            "參考點異常",
            "數據節點異常"});
            this.AlarmTypeCB.Location = new System.Drawing.Point(73, 19);
            this.AlarmTypeCB.Name = "AlarmTypeCB";
            this.AlarmTypeCB.Size = new System.Drawing.Size(121, 20);
            this.AlarmTypeCB.TabIndex = 8;
            this.AlarmTypeCB.SelectedIndexChanged += new System.EventHandler(this.AlarmTypeCB_SelectedIndexChanged);
            // 
            // StartBtn
            // 
            this.StartBtn.BackColor = System.Drawing.Color.White;
            this.StartBtn.Location = new System.Drawing.Point(263, 13);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(90, 31);
            this.StartBtn.TabIndex = 0;
            this.StartBtn.Text = "開始搜索";
            this.StartBtn.UseVisualStyleBackColor = false;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.eMinitueCB);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.eHourCB);
            this.groupBox2.Controls.Add(this.EndDTPicker);
            this.groupBox2.Location = new System.Drawing.Point(356, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(332, 83);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "結束時間";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(294, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "分";
            // 
            // eMinitueCB
            // 
            this.eMinitueCB.FormattingEnabled = true;
            this.eMinitueCB.Location = new System.Drawing.Point(240, 20);
            this.eMinitueCB.Name = "eMinitueCB";
            this.eMinitueCB.Size = new System.Drawing.Size(49, 20);
            this.eMinitueCB.TabIndex = 3;
            this.eMinitueCB.SelectedIndexChanged += new System.EventHandler(this.eMinitueCB_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(140, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "日";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(218, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "時";
            // 
            // eHourCB
            // 
            this.eHourCB.FormattingEnabled = true;
            this.eHourCB.Location = new System.Drawing.Point(162, 21);
            this.eHourCB.Name = "eHourCB";
            this.eHourCB.Size = new System.Drawing.Size(49, 20);
            this.eHourCB.TabIndex = 1;
            this.eHourCB.SelectedIndexChanged += new System.EventHandler(this.eHourCB_SelectedIndexChanged);
            // 
            // EndDTPicker
            // 
            this.EndDTPicker.Location = new System.Drawing.Point(13, 20);
            this.EndDTPicker.Name = "EndDTPicker";
            this.EndDTPicker.Size = new System.Drawing.Size(121, 21);
            this.EndDTPicker.TabIndex = 0;
            this.EndDTPicker.ValueChanged += new System.EventHandler(this.EndDTPicker_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.sMinitueCb);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.sHourCb);
            this.groupBox1.Controls.Add(this.StartDTPicker);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(330, 83);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "開始時間";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(294, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "分";
            // 
            // sMinitueCb
            // 
            this.sMinitueCb.FormattingEnabled = true;
            this.sMinitueCb.Location = new System.Drawing.Point(240, 20);
            this.sMinitueCb.Name = "sMinitueCb";
            this.sMinitueCb.Size = new System.Drawing.Size(49, 20);
            this.sMinitueCb.TabIndex = 3;
            this.sMinitueCb.SelectedIndexChanged += new System.EventHandler(this.sMinitueCb_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(140, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "日";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(218, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "時";
            // 
            // sHourCb
            // 
            this.sHourCb.FormattingEnabled = true;
            this.sHourCb.Location = new System.Drawing.Point(162, 21);
            this.sHourCb.Name = "sHourCb";
            this.sHourCb.Size = new System.Drawing.Size(49, 20);
            this.sHourCb.TabIndex = 1;
            this.sHourCb.SelectedIndexChanged += new System.EventHandler(this.sHourCb_SelectedIndexChanged);
            // 
            // StartDTPicker
            // 
            this.StartDTPicker.Location = new System.Drawing.Point(13, 20);
            this.StartDTPicker.Name = "StartDTPicker";
            this.StartDTPicker.Size = new System.Drawing.Size(121, 21);
            this.StartDTPicker.TabIndex = 0;
            this.StartDTPicker.ValueChanged += new System.EventHandler(this.StartDTPicker_ValueChanged);
            // 
            // WarmListView
            // 
            this.WarmListView.FormattingEnabled = true;
            this.WarmListView.HorizontalScrollbar = true;
            this.WarmListView.ItemHeight = 12;
            this.WarmListView.Location = new System.Drawing.Point(12, 150);
            this.WarmListView.Name = "WarmListView";
            this.WarmListView.ScrollAlwaysVisible = true;
            this.WarmListView.Size = new System.Drawing.Size(676, 424);
            this.WarmListView.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 129);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(101, 12);
            this.label8.TabIndex = 11;
            this.label8.Text = "當前記錄數：0 條";
            // 
            // WarmMsg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 583);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.WarmListView);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "WarmMsg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "警告资讯";
            this.Load += new System.EventHandler(this.WarmMsg_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox eMinitueCB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox eHourCB;
        private System.Windows.Forms.DateTimePicker EndDTPicker;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox sMinitueCb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox sHourCb;
        private System.Windows.Forms.DateTimePicker StartDTPicker;
        private System.Windows.Forms.ListBox WarmListView;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox AlarmTypeCB;
        private System.Windows.Forms.Button AlarmMsgOutBtn;
        private System.Windows.Forms.RadioButton txtRB;
        private System.Windows.Forms.RadioButton xlsRB;
        private System.Windows.Forms.Label label8;
    }
}