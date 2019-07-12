namespace PersionAutoLocaSys
{
    partial class AccessStatistics
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
            this.startdtpicker = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.tagtb = new System.Windows.Forms.TextBox();
            this.selectbtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.enddtpicker = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.recordlistview = new System.Windows.Forms.ListView();
            this.IDCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TagNameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EnterReferCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.GoOutReferCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EnterTimeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.GoOutTimeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IntervalTimeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label5 = new System.Windows.Forms.Label();
            this.ClearTimeCB = new System.Windows.Forms.ComboBox();
            this.excelbtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "選擇查詢的開始時間：";
            // 
            // startdtpicker
            // 
            this.startdtpicker.Location = new System.Drawing.Point(148, 14);
            this.startdtpicker.Name = "startdtpicker";
            this.startdtpicker.Size = new System.Drawing.Size(189, 21);
            this.startdtpicker.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "人員ID或名稱：";
            // 
            // tagtb
            // 
            this.tagtb.Location = new System.Drawing.Point(107, 51);
            this.tagtb.Name = "tagtb";
            this.tagtb.Size = new System.Drawing.Size(100, 21);
            this.tagtb.TabIndex = 3;
            // 
            // selectbtn
            // 
            this.selectbtn.BackColor = System.Drawing.Color.White;
            this.selectbtn.Location = new System.Drawing.Point(528, 41);
            this.selectbtn.Name = "selectbtn";
            this.selectbtn.Size = new System.Drawing.Size(227, 38);
            this.selectbtn.TabIndex = 4;
            this.selectbtn.Text = "查詢";
            this.selectbtn.UseVisualStyleBackColor = false;
            this.selectbtn.Click += new System.EventHandler(this.selectbtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(375, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "選擇查詢的結束時間：";
            // 
            // enddtpicker
            // 
            this.enddtpicker.Location = new System.Drawing.Point(508, 14);
            this.enddtpicker.Name = "enddtpicker";
            this.enddtpicker.Size = new System.Drawing.Size(189, 21);
            this.enddtpicker.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.recordlistview);
            this.panel1.Location = new System.Drawing.Point(17, 89);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(782, 512);
            this.panel1.TabIndex = 9;
            // 
            // recordlistview
            // 
            this.recordlistview.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IDCol,
            this.TagNameCol,
            this.EnterReferCol,
            this.GoOutReferCol,
            this.EnterTimeCol,
            this.GoOutTimeCol,
            this.IntervalTimeCol});
            this.recordlistview.FullRowSelect = true;
            this.recordlistview.GridLines = true;
            this.recordlistview.Location = new System.Drawing.Point(3, 3);
            this.recordlistview.Name = "recordlistview";
            this.recordlistview.Size = new System.Drawing.Size(775, 505);
            this.recordlistview.TabIndex = 0;
            this.recordlistview.UseCompatibleStateImageBehavior = false;
            this.recordlistview.View = System.Windows.Forms.View.Details;
            // 
            // IDCol
            // 
            this.IDCol.Text = "ID";
            // 
            // TagNameCol
            // 
            this.TagNameCol.Text = "名稱";
            this.TagNameCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TagNameCol.Width = 75;
            // 
            // EnterReferCol
            // 
            this.EnterReferCol.Text = "入口基站";
            this.EnterReferCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.EnterReferCol.Width = 112;
            // 
            // GoOutReferCol
            // 
            this.GoOutReferCol.Text = "出口基站";
            this.GoOutReferCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.GoOutReferCol.Width = 107;
            // 
            // EnterTimeCol
            // 
            this.EnterTimeCol.Text = "進入時間";
            this.EnterTimeCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.EnterTimeCol.Width = 148;
            // 
            // GoOutTimeCol
            // 
            this.GoOutTimeCol.Text = "外出時間";
            this.GoOutTimeCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.GoOutTimeCol.Width = 142;
            // 
            // IntervalTimeCol
            // 
            this.IntervalTimeCol.Text = "間隔時間";
            this.IntervalTimeCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.IntervalTimeCol.Width = 104;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(210, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "清理時間：";
            // 
            // ClearTimeCB
            // 
            this.ClearTimeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ClearTimeCB.FormattingEnabled = true;
            this.ClearTimeCB.Items.AddRange(new object[] {
            "7天",
            "1個月",
            "3個月",
            "6個月",
            "1年"});
            this.ClearTimeCB.Location = new System.Drawing.Point(279, 51);
            this.ClearTimeCB.Name = "ClearTimeCB";
            this.ClearTimeCB.Size = new System.Drawing.Size(110, 20);
            this.ClearTimeCB.TabIndex = 11;
            this.ClearTimeCB.SelectedIndexChanged += new System.EventHandler(this.ClearTimeCB_SelectedIndexChanged);
            // 
            // excelbtn
            // 
            this.excelbtn.BackColor = System.Drawing.Color.White;
            this.excelbtn.ForeColor = System.Drawing.Color.Black;
            this.excelbtn.Location = new System.Drawing.Point(403, 41);
            this.excelbtn.Name = "excelbtn";
            this.excelbtn.Size = new System.Drawing.Size(97, 38);
            this.excelbtn.TabIndex = 12;
            this.excelbtn.Text = "Excel導出";
            this.excelbtn.UseVisualStyleBackColor = false;
            this.excelbtn.Click += new System.EventHandler(this.excelbtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 609);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "Current record number: 0 ";
            // 
            // AccessStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 630);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.excelbtn);
            this.Controls.Add(this.ClearTimeCB);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.enddtpicker);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.selectbtn);
            this.Controls.Add(this.tagtb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.startdtpicker);
            this.Controls.Add(this.label1);
            this.Name = "AccessStatistics";
            this.Text = "人員出入統計";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AccessStatistics_FormClosing);
            this.Load += new System.EventHandler(this.AccessStatistics_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker startdtpicker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tagtb;
        private System.Windows.Forms.Button selectbtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker enddtpicker;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView recordlistview;
        private System.Windows.Forms.ColumnHeader IDCol;
        private System.Windows.Forms.ColumnHeader TagNameCol;
        private System.Windows.Forms.ColumnHeader GoOutReferCol;
        private System.Windows.Forms.ColumnHeader EnterReferCol;
        private System.Windows.Forms.ColumnHeader EnterTimeCol;
        private System.Windows.Forms.ColumnHeader GoOutTimeCol;
        private System.Windows.Forms.ColumnHeader IntervalTimeCol;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox ClearTimeCB;
        private System.Windows.Forms.Button excelbtn;
        private System.Windows.Forms.Label label4;
    }
}