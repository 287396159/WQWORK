namespace PersionAutoLocaSys
{
    partial class AdmissionExitHistoryForm
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
            this.label66 = new System.Windows.Forms.Label();
            this.EndTP = new System.Windows.Forms.DateTimePicker();
            this.startTP = new System.Windows.Forms.DateTimePicker();
            this.startTimeLb = new System.Windows.Forms.Label();
            this.endTimeLb = new System.Windows.Forms.Label();
            this.daTagListView = new System.Windows.Forms.ListView();
            this.TagIDCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SDTimeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.workIDCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Location = new System.Drawing.Point(248, 16);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(23, 12);
            this.label66.TabIndex = 32;
            this.label66.Text = "---";
            // 
            // EndTP
            // 
            this.EndTP.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.EndTP.Location = new System.Drawing.Point(349, 12);
            this.EndTP.Name = "EndTP";
            this.EndTP.ShowUpDown = true;
            this.EndTP.Size = new System.Drawing.Size(158, 21);
            this.EndTP.TabIndex = 31;
            this.EndTP.Value = new System.DateTime(2017, 5, 15, 18, 0, 0, 0);
            // 
            // startTP
            // 
            this.startTP.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startTP.Location = new System.Drawing.Point(67, 12);
            this.startTP.Name = "startTP";
            this.startTP.ShowUpDown = true;
            this.startTP.Size = new System.Drawing.Size(159, 21);
            this.startTP.TabIndex = 30;
            this.startTP.Value = new System.DateTime(2017, 5, 15, 8, 0, 0, 0);
            // 
            // startTimeLb
            // 
            this.startTimeLb.AutoSize = true;
            this.startTimeLb.Location = new System.Drawing.Point(11, 16);
            this.startTimeLb.Name = "startTimeLb";
            this.startTimeLb.Size = new System.Drawing.Size(53, 12);
            this.startTimeLb.TabIndex = 33;
            this.startTimeLb.Text = "開始時間";
            // 
            // endTimeLb
            // 
            this.endTimeLb.AutoSize = true;
            this.endTimeLb.Location = new System.Drawing.Point(292, 16);
            this.endTimeLb.Name = "endTimeLb";
            this.endTimeLb.Size = new System.Drawing.Size(53, 12);
            this.endTimeLb.TabIndex = 34;
            this.endTimeLb.Text = "結束時間";
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
            this.daTagListView.Location = new System.Drawing.Point(6, 49);
            this.daTagListView.Name = "daTagListView";
            this.daTagListView.Size = new System.Drawing.Size(722, 454);
            this.daTagListView.TabIndex = 35;
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
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(251, 16);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(84, 16);
            this.checkBox2.TabIndex = 40;
            this.checkBox2.Text = "出場記錄：";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(8, 17);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(84, 16);
            this.checkBox1.TabIndex = 39;
            this.checkBox1.Text = "入場記錄：";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(98, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 12);
            this.label10.TabIndex = 38;
            this.label10.Text = "0 個";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(335, 17);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 36;
            this.label13.Text = "0 個";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(513, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 41;
            this.button1.Text = "查詢記錄";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(652, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 42;
            this.button2.Text = "匯出Excel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 39);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(741, 532);
            this.tabControl1.TabIndex = 43;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.daTagListView);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.checkBox2);
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(733, 506);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "進出廠記錄";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button3);
            this.tabPage2.Controls.Add(this.listView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(733, 506);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "所有記錄";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(652, 8);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 43;
            this.button3.Text = "匯出Excel";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(8, 37);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(722, 463);
            this.listView1.TabIndex = 36;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "ID";
            this.columnHeader3.Width = 76;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "名稱";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 105;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "進出場模式";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 150;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "進出場時間";
            this.columnHeader6.Width = 150;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "打卡ID";
            this.columnHeader7.Width = 234;
            // 
            // AdmissionExitHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 577);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.endTimeLb);
            this.Controls.Add(this.startTimeLb);
            this.Controls.Add(this.label66);
            this.Controls.Add(this.EndTP);
            this.Controls.Add(this.startTP);
            this.Name = "AdmissionExitHistoryForm";
            this.Text = "進出廠歷史數據";
            this.Load += new System.EventHandler(this.AdmissionExitHistoryForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.DateTimePicker EndTP;
        private System.Windows.Forms.DateTimePicker startTP;
        private System.Windows.Forms.Label startTimeLb;
        private System.Windows.Forms.Label endTimeLb;
        public System.Windows.Forms.ListView daTagListView;
        public System.Windows.Forms.ColumnHeader TagIDCol;
        public System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader SDTimeCol;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader workIDCol;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        public System.Windows.Forms.ListView listView1;
        public System.Windows.Forms.ColumnHeader columnHeader3;
        public System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Button button3;
    }
}