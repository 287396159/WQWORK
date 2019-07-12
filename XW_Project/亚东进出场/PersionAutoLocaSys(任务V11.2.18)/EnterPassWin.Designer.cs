namespace PersionAutoLocaSys
{
    partial class EnterPassWin
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
            this.UserName_TB = new System.Windows.Forms.TextBox();
            this.Pass_TB = new System.Windows.Forms.TextBox();
            this.Enter_Btn = new System.Windows.Forms.Button();
            this.Cancel_Btn = new System.Windows.Forms.Button();
            this.ModifyUser_LB = new System.Windows.Forms.LinkLabel();
            this.PWAutherBox = new System.Windows.Forms.Panel();
            this.ModifyPWBox = new System.Windows.Forms.Panel();
            this.NewPassTB = new System.Windows.Forms.TextBox();
            this.NewUserNameTB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ModifyCancelBtn = new System.Windows.Forms.Button();
            this.ModifyEnterBtn = new System.Windows.Forms.Button();
            this.OldPassTB = new System.Windows.Forms.TextBox();
            this.OldUserNameTB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.logoutcb = new System.Windows.Forms.CheckBox();
            this.quitappcb = new System.Windows.Forms.CheckBox();
            this.exittypebox = new System.Windows.Forms.Panel();
            this.PWAutherBox.SuspendLayout();
            this.ModifyPWBox.SuspendLayout();
            this.exittypebox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用戶名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "密碼：";
            // 
            // UserName_TB
            // 
            this.UserName_TB.Location = new System.Drawing.Point(66, 5);
            this.UserName_TB.Name = "UserName_TB";
            this.UserName_TB.Size = new System.Drawing.Size(100, 21);
            this.UserName_TB.TabIndex = 2;
            // 
            // Pass_TB
            // 
            this.Pass_TB.Location = new System.Drawing.Point(66, 36);
            this.Pass_TB.MaxLength = 10;
            this.Pass_TB.Name = "Pass_TB";
            this.Pass_TB.Size = new System.Drawing.Size(100, 21);
            this.Pass_TB.TabIndex = 3;
            this.Pass_TB.UseSystemPasswordChar = true;
            // 
            // Enter_Btn
            // 
            this.Enter_Btn.Location = new System.Drawing.Point(9, 71);
            this.Enter_Btn.Name = "Enter_Btn";
            this.Enter_Btn.Size = new System.Drawing.Size(75, 23);
            this.Enter_Btn.TabIndex = 4;
            this.Enter_Btn.Text = "確定";
            this.Enter_Btn.UseVisualStyleBackColor = true;
            this.Enter_Btn.Click += new System.EventHandler(this.Enter_Btn_Click);
            // 
            // Cancel_Btn
            // 
            this.Cancel_Btn.Location = new System.Drawing.Point(91, 71);
            this.Cancel_Btn.Name = "Cancel_Btn";
            this.Cancel_Btn.Size = new System.Drawing.Size(75, 23);
            this.Cancel_Btn.TabIndex = 5;
            this.Cancel_Btn.Text = "取消";
            this.Cancel_Btn.UseVisualStyleBackColor = true;
            this.Cancel_Btn.Click += new System.EventHandler(this.Cancel_Btn_Click);
            // 
            // ModifyUser_LB
            // 
            this.ModifyUser_LB.AutoSize = true;
            this.ModifyUser_LB.Enabled = false;
            this.ModifyUser_LB.Location = new System.Drawing.Point(11, 104);
            this.ModifyUser_LB.Name = "ModifyUser_LB";
            this.ModifyUser_LB.Size = new System.Drawing.Size(101, 12);
            this.ModifyUser_LB.TabIndex = 6;
            this.ModifyUser_LB.TabStop = true;
            this.ModifyUser_LB.Text = "修改用戶名及密碼";
            this.ModifyUser_LB.Visible = false;
            this.ModifyUser_LB.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ModifyUser_LB_LinkClicked);
            // 
            // PWAutherBox
            // 
            this.PWAutherBox.Controls.Add(this.UserName_TB);
            this.PWAutherBox.Controls.Add(this.ModifyUser_LB);
            this.PWAutherBox.Controls.Add(this.label1);
            this.PWAutherBox.Controls.Add(this.Cancel_Btn);
            this.PWAutherBox.Controls.Add(this.label2);
            this.PWAutherBox.Controls.Add(this.Enter_Btn);
            this.PWAutherBox.Controls.Add(this.Pass_TB);
            this.PWAutherBox.Location = new System.Drawing.Point(12, 18);
            this.PWAutherBox.Name = "PWAutherBox";
            this.PWAutherBox.Size = new System.Drawing.Size(173, 123);
            this.PWAutherBox.TabIndex = 7;
            // 
            // ModifyPWBox
            // 
            this.ModifyPWBox.Controls.Add(this.NewPassTB);
            this.ModifyPWBox.Controls.Add(this.NewUserNameTB);
            this.ModifyPWBox.Controls.Add(this.label5);
            this.ModifyPWBox.Controls.Add(this.label6);
            this.ModifyPWBox.Controls.Add(this.ModifyCancelBtn);
            this.ModifyPWBox.Controls.Add(this.ModifyEnterBtn);
            this.ModifyPWBox.Controls.Add(this.OldPassTB);
            this.ModifyPWBox.Controls.Add(this.OldUserNameTB);
            this.ModifyPWBox.Controls.Add(this.label4);
            this.ModifyPWBox.Controls.Add(this.label3);
            this.ModifyPWBox.Location = new System.Drawing.Point(267, 18);
            this.ModifyPWBox.Name = "ModifyPWBox";
            this.ModifyPWBox.Size = new System.Drawing.Size(195, 166);
            this.ModifyPWBox.TabIndex = 8;
            // 
            // NewPassTB
            // 
            this.NewPassTB.Location = new System.Drawing.Point(79, 98);
            this.NewPassTB.MaxLength = 10;
            this.NewPassTB.Name = "NewPassTB";
            this.NewPassTB.Size = new System.Drawing.Size(100, 21);
            this.NewPassTB.TabIndex = 10;
            this.NewPassTB.UseSystemPasswordChar = true;
            // 
            // NewUserNameTB
            // 
            this.NewUserNameTB.Location = new System.Drawing.Point(79, 67);
            this.NewUserNameTB.Name = "NewUserNameTB";
            this.NewUserNameTB.Size = new System.Drawing.Size(100, 21);
            this.NewUserNameTB.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 102);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "新密碼：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "新用戶名：";
            // 
            // ModifyCancelBtn
            // 
            this.ModifyCancelBtn.Location = new System.Drawing.Point(103, 131);
            this.ModifyCancelBtn.Name = "ModifyCancelBtn";
            this.ModifyCancelBtn.Size = new System.Drawing.Size(75, 23);
            this.ModifyCancelBtn.TabIndex = 6;
            this.ModifyCancelBtn.Text = "取消";
            this.ModifyCancelBtn.UseVisualStyleBackColor = true;
            this.ModifyCancelBtn.Click += new System.EventHandler(this.Cancel_Btn_Click);
            // 
            // ModifyEnterBtn
            // 
            this.ModifyEnterBtn.Location = new System.Drawing.Point(22, 131);
            this.ModifyEnterBtn.Name = "ModifyEnterBtn";
            this.ModifyEnterBtn.Size = new System.Drawing.Size(75, 23);
            this.ModifyEnterBtn.TabIndex = 5;
            this.ModifyEnterBtn.Text = "確定";
            this.ModifyEnterBtn.UseVisualStyleBackColor = true;
            this.ModifyEnterBtn.Click += new System.EventHandler(this.ModifyEnterBtn_Click);
            // 
            // OldPassTB
            // 
            this.OldPassTB.Location = new System.Drawing.Point(79, 36);
            this.OldPassTB.MaxLength = 10;
            this.OldPassTB.Name = "OldPassTB";
            this.OldPassTB.Size = new System.Drawing.Size(100, 21);
            this.OldPassTB.TabIndex = 4;
            this.OldPassTB.UseSystemPasswordChar = true;
            // 
            // OldUserNameTB
            // 
            this.OldUserNameTB.Location = new System.Drawing.Point(79, 5);
            this.OldUserNameTB.Name = "OldUserNameTB";
            this.OldUserNameTB.Size = new System.Drawing.Size(100, 21);
            this.OldUserNameTB.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "原密碼：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "原用戶名：";
            // 
            // logoutcb
            // 
            this.logoutcb.AutoSize = true;
            this.logoutcb.Checked = true;
            this.logoutcb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.logoutcb.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.logoutcb.Location = new System.Drawing.Point(9, 8);
            this.logoutcb.Name = "logoutcb";
            this.logoutcb.Size = new System.Drawing.Size(76, 16);
            this.logoutcb.TabIndex = 9;
            this.logoutcb.Text = "退出登陆";
            this.logoutcb.UseVisualStyleBackColor = true;
            this.logoutcb.CheckedChanged += new System.EventHandler(this.logoutcb_CheckedChanged);
            // 
            // quitappcb
            // 
            this.quitappcb.AutoSize = true;
            this.quitappcb.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.quitappcb.Location = new System.Drawing.Point(87, 8);
            this.quitappcb.Name = "quitappcb";
            this.quitappcb.Size = new System.Drawing.Size(76, 16);
            this.quitappcb.TabIndex = 10;
            this.quitappcb.Text = "退出應用";
            this.quitappcb.UseVisualStyleBackColor = true;
            this.quitappcb.CheckedChanged += new System.EventHandler(this.quitappcb_CheckedChanged);
            // 
            // exittypebox
            // 
            this.exittypebox.Controls.Add(this.quitappcb);
            this.exittypebox.Controls.Add(this.logoutcb);
            this.exittypebox.Location = new System.Drawing.Point(277, 209);
            this.exittypebox.Name = "exittypebox";
            this.exittypebox.Size = new System.Drawing.Size(173, 31);
            this.exittypebox.TabIndex = 11;
            // 
            // EnterPassWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 303);
            this.Controls.Add(this.exittypebox);
            this.Controls.Add(this.ModifyPWBox);
            this.Controls.Add(this.PWAutherBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EnterPassWin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "                       密碼驗證";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EnterPassWin_FormClosed);
            this.Load += new System.EventHandler(this.EnterPassWin_Load);
            this.PWAutherBox.ResumeLayout(false);
            this.PWAutherBox.PerformLayout();
            this.ModifyPWBox.ResumeLayout(false);
            this.ModifyPWBox.PerformLayout();
            this.exittypebox.ResumeLayout(false);
            this.exittypebox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox UserName_TB;
        private System.Windows.Forms.TextBox Pass_TB;
        private System.Windows.Forms.Button Enter_Btn;
        private System.Windows.Forms.Button Cancel_Btn;
        private System.Windows.Forms.LinkLabel ModifyUser_LB;
        private System.Windows.Forms.Panel PWAutherBox;
        private System.Windows.Forms.Panel ModifyPWBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox OldPassTB;
        private System.Windows.Forms.TextBox OldUserNameTB;
        private System.Windows.Forms.Button ModifyCancelBtn;
        private System.Windows.Forms.Button ModifyEnterBtn;
        private System.Windows.Forms.TextBox NewPassTB;
        private System.Windows.Forms.TextBox NewUserNameTB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox logoutcb;
        private System.Windows.Forms.CheckBox quitappcb;
        private System.Windows.Forms.Panel exittypebox;
    }
}