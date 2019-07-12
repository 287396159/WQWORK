namespace PersionAutoLocaSys
{
    partial class ViewOperation
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
            this.label2 = new System.Windows.Forms.Label();
            this.startdtpk = new System.Windows.Forms.DateTimePicker();
            this.enddtpk = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.persontxt = new System.Windows.Forms.TextBox();
            this.selectbtn = new System.Windows.Forms.Button();
            this.peroperlv = new System.Windows.Forms.ListView();
            this.personidcol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.opertimecol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.opertypecol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.namecol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label4 = new System.Windows.Forms.Label();
            this.clearopertimecb = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "開始時間：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(326, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "結束時間：";
            // 
            // startdtpk
            // 
            this.startdtpk.Location = new System.Drawing.Point(95, 23);
            this.startdtpk.Name = "startdtpk";
            this.startdtpk.Size = new System.Drawing.Size(200, 21);
            this.startdtpk.TabIndex = 2;
            // 
            // enddtpk
            // 
            this.enddtpk.Location = new System.Drawing.Point(397, 23);
            this.enddtpk.Name = "enddtpk";
            this.enddtpk.Size = new System.Drawing.Size(200, 21);
            this.enddtpk.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.enddtpk);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.startdtpk);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(709, 60);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "選擇時間";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "選擇人員：";
            // 
            // persontxt
            // 
            this.persontxt.Location = new System.Drawing.Point(84, 87);
            this.persontxt.Name = "persontxt";
            this.persontxt.Size = new System.Drawing.Size(100, 21);
            this.persontxt.TabIndex = 6;
            // 
            // selectbtn
            // 
            this.selectbtn.BackColor = System.Drawing.Color.White;
            this.selectbtn.Location = new System.Drawing.Point(255, 82);
            this.selectbtn.Name = "selectbtn";
            this.selectbtn.Size = new System.Drawing.Size(111, 30);
            this.selectbtn.TabIndex = 7;
            this.selectbtn.Text = "確   定";
            this.selectbtn.UseVisualStyleBackColor = false;
            this.selectbtn.Click += new System.EventHandler(this.selectbtn_Click);
            // 
            // peroperlv
            // 
            this.peroperlv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.personidcol,
            this.namecol,
            this.opertimecol,
            this.opertypecol});
            this.peroperlv.GridLines = true;
            this.peroperlv.Location = new System.Drawing.Point(11, 123);
            this.peroperlv.Name = "peroperlv";
            this.peroperlv.Size = new System.Drawing.Size(710, 441);
            this.peroperlv.TabIndex = 8;
            this.peroperlv.UseCompatibleStateImageBehavior = false;
            this.peroperlv.View = System.Windows.Forms.View.Details;
            // 
            // personidcol
            // 
            this.personidcol.Text = "人員ID";
            // 
            // opertimecol
            // 
            this.opertimecol.Text = "操作時間";
            this.opertimecol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.opertimecol.Width = 224;
            // 
            // opertypecol
            // 
            this.opertypecol.Text = "操作類型";
            this.opertypecol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.opertypecol.Width = 315;
            // 
            // namecol
            // 
            this.namecol.Text = "名稱";
            this.namecol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.namecol.Width = 105;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(484, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "操作記錄清理時間：";
            // 
            // clearopertimecb
            // 
            this.clearopertimecb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clearopertimecb.FormattingEnabled = true;
            this.clearopertimecb.Items.AddRange(new object[] {
            "7天",
            "1個月",
            "3個月",
            "6個月",
            "1年",
            "永不"});
            this.clearopertimecb.Location = new System.Drawing.Point(598, 86);
            this.clearopertimecb.Name = "clearopertimecb";
            this.clearopertimecb.Size = new System.Drawing.Size(121, 20);
            this.clearopertimecb.TabIndex = 10;
            this.clearopertimecb.SelectedIndexChanged += new System.EventHandler(this.clearopertimecb_SelectedIndexChanged);
            // 
            // ViewOperation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 576);
            this.Controls.Add(this.clearopertimecb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.peroperlv);
            this.Controls.Add(this.selectbtn);
            this.Controls.Add(this.persontxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Name = "ViewOperation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "查看人員操作";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewOperation_FormClosing);
            this.Load += new System.EventHandler(this.ViewOperation_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker startdtpk;
        private System.Windows.Forms.DateTimePicker enddtpk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox persontxt;
        private System.Windows.Forms.Button selectbtn;
        private System.Windows.Forms.ListView peroperlv;
        private System.Windows.Forms.ColumnHeader personidcol;
        private System.Windows.Forms.ColumnHeader opertimecol;
        private System.Windows.Forms.ColumnHeader opertypecol;
        private System.Windows.Forms.ColumnHeader namecol;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox clearopertimecb;



    }
}