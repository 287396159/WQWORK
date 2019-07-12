namespace RfUpdateApp
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.gbSet = new System.Windows.Forms.GroupBox();
            this.lbBinSize = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbBinVersion = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbBinType = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbNeedUpdateVersion = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStartUpdate = new System.Windows.Forms.Button();
            this.cbAskToUpdate = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbFile = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.btnComRefresh = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbCom = new System.Windows.Forms.ComboBox();
            this.gbDongle = new System.Windows.Forms.GroupBox();
            this.lbPercent = new System.Windows.Forms.Label();
            this.lbUpdateProgress = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnStopUpdate = new System.Windows.Forms.Button();
            this.btnReadStatus = new System.Windows.Forms.Button();
            this.pbDongleUpdate = new System.Windows.Forms.ProgressBar();
            this.tbDongleStatus = new System.Windows.Forms.TextBox();
            this.lbDeviceFailNum = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lbDeviceSuccessNum = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lbDeviceNum = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbdateVersion = new System.Windows.Forms.Label();
            this.gbSet.SuspendLayout();
            this.gbDongle.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSet
            // 
            this.gbSet.Controls.Add(this.lbBinSize);
            this.gbSet.Controls.Add(this.label8);
            this.gbSet.Controls.Add(this.lbBinVersion);
            this.gbSet.Controls.Add(this.label7);
            this.gbSet.Controls.Add(this.lbBinType);
            this.gbSet.Controls.Add(this.label9);
            this.gbSet.Controls.Add(this.label4);
            this.gbSet.Controls.Add(this.tbNeedUpdateVersion);
            this.gbSet.Controls.Add(this.label2);
            this.gbSet.Controls.Add(this.btnStartUpdate);
            this.gbSet.Controls.Add(this.cbAskToUpdate);
            this.gbSet.Controls.Add(this.label3);
            this.gbSet.Controls.Add(this.tbFile);
            this.gbSet.Controls.Add(this.btnOpenFile);
            this.gbSet.Controls.Add(this.btnComRefresh);
            this.gbSet.Controls.Add(this.btnConnect);
            this.gbSet.Controls.Add(this.label1);
            this.gbSet.Controls.Add(this.cbCom);
            this.gbSet.Location = new System.Drawing.Point(12, 12);
            this.gbSet.Name = "gbSet";
            this.gbSet.Size = new System.Drawing.Size(593, 188);
            this.gbSet.TabIndex = 0;
            this.gbSet.TabStop = false;
            this.gbSet.Text = "Set";
            // 
            // lbBinSize
            // 
            this.lbBinSize.AutoSize = true;
            this.lbBinSize.Location = new System.Drawing.Point(396, 161);
            this.lbBinSize.Name = "lbBinSize";
            this.lbBinSize.Size = new System.Drawing.Size(41, 12);
            this.lbBinSize.TabIndex = 28;
            this.lbBinSize.Text = "label6";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(301, 161);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 12);
            this.label8.TabIndex = 27;
            this.label8.Text = "Firmware Size:";
            // 
            // lbBinVersion
            // 
            this.lbBinVersion.AutoSize = true;
            this.lbBinVersion.Location = new System.Drawing.Point(171, 161);
            this.lbBinVersion.Name = "lbBinVersion";
            this.lbBinVersion.Size = new System.Drawing.Size(41, 12);
            this.lbBinVersion.TabIndex = 26;
            this.lbBinVersion.Text = "label6";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(58, 161);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 12);
            this.label7.TabIndex = 25;
            this.label7.Text = "Firmware Version:";
            // 
            // lbBinType
            // 
            this.lbBinType.AutoSize = true;
            this.lbBinType.Location = new System.Drawing.Point(171, 138);
            this.lbBinType.Name = "lbBinType";
            this.lbBinType.Size = new System.Drawing.Size(41, 12);
            this.lbBinType.TabIndex = 23;
            this.lbBinType.Text = "label5";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(76, 138);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 12);
            this.label9.TabIndex = 24;
            this.label9.Text = "Firmware Type:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(170, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 22;
            this.label4.Text = "0x";
            // 
            // tbNeedUpdateVersion
            // 
            this.tbNeedUpdateVersion.Location = new System.Drawing.Point(193, 104);
            this.tbNeedUpdateVersion.Name = "tbNeedUpdateVersion";
            this.tbNeedUpdateVersion.Size = new System.Drawing.Size(75, 21);
            this.tbNeedUpdateVersion.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Need Update Version:";
            // 
            // btnStartUpdate
            // 
            this.btnStartUpdate.Location = new System.Drawing.Point(474, 103);
            this.btnStartUpdate.Name = "btnStartUpdate";
            this.btnStartUpdate.Size = new System.Drawing.Size(93, 23);
            this.btnStartUpdate.TabIndex = 20;
            this.btnStartUpdate.Text = "Start Update";
            this.btnStartUpdate.UseVisualStyleBackColor = true;
            this.btnStartUpdate.Click += new System.EventHandler(this.btnStartUpdate_Click);
            // 
            // cbAskToUpdate
            // 
            this.cbAskToUpdate.AutoSize = true;
            this.cbAskToUpdate.Location = new System.Drawing.Point(303, 106);
            this.cbAskToUpdate.Name = "cbAskToUpdate";
            this.cbAskToUpdate.Size = new System.Drawing.Size(102, 16);
            this.cbAskToUpdate.TabIndex = 19;
            this.cbAskToUpdate.Text = "Ask to update";
            this.cbAskToUpdate.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 16;
            this.label3.Text = "File:";
            // 
            // tbFile
            // 
            this.tbFile.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tbFile.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbFile.Location = new System.Drawing.Point(75, 70);
            this.tbFile.Name = "tbFile";
            this.tbFile.ReadOnly = true;
            this.tbFile.Size = new System.Drawing.Size(378, 21);
            this.tbFile.TabIndex = 18;
            this.tbFile.TabStop = false;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(474, 68);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(93, 23);
            this.btnOpenFile.TabIndex = 17;
            this.btnOpenFile.Text = "Open File";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // btnComRefresh
            // 
            this.btnComRefresh.Location = new System.Drawing.Point(172, 29);
            this.btnComRefresh.Name = "btnComRefresh";
            this.btnComRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnComRefresh.TabIndex = 15;
            this.btnComRefresh.Text = "Refresh";
            this.btnComRefresh.UseVisualStyleBackColor = true;
            this.btnComRefresh.Click += new System.EventHandler(this.btnComRefresh_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(268, 29);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 13;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "COM:";
            // 
            // cbCom
            // 
            this.cbCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCom.FormattingEnabled = true;
            this.cbCom.Location = new System.Drawing.Point(75, 30);
            this.cbCom.Name = "cbCom";
            this.cbCom.Size = new System.Drawing.Size(76, 20);
            this.cbCom.TabIndex = 12;
            // 
            // gbDongle
            // 
            this.gbDongle.BackColor = System.Drawing.SystemColors.Control;
            this.gbDongle.Controls.Add(this.lbPercent);
            this.gbDongle.Controls.Add(this.lbUpdateProgress);
            this.gbDongle.Controls.Add(this.label5);
            this.gbDongle.Controls.Add(this.btnStopUpdate);
            this.gbDongle.Controls.Add(this.btnReadStatus);
            this.gbDongle.Controls.Add(this.pbDongleUpdate);
            this.gbDongle.Controls.Add(this.tbDongleStatus);
            this.gbDongle.Location = new System.Drawing.Point(12, 206);
            this.gbDongle.Name = "gbDongle";
            this.gbDongle.Size = new System.Drawing.Size(593, 196);
            this.gbDongle.TabIndex = 1;
            this.gbDongle.TabStop = false;
            this.gbDongle.Text = "Usb Dongle";
            // 
            // lbPercent
            // 
            this.lbPercent.AutoSize = true;
            this.lbPercent.Location = new System.Drawing.Point(472, 128);
            this.lbPercent.Name = "lbPercent";
            this.lbPercent.Size = new System.Drawing.Size(17, 12);
            this.lbPercent.TabIndex = 31;
            this.lbPercent.Text = "0%";
            // 
            // lbUpdateProgress
            // 
            this.lbUpdateProgress.AutoSize = true;
            this.lbUpdateProgress.ForeColor = System.Drawing.Color.Red;
            this.lbUpdateProgress.Location = new System.Drawing.Point(73, 158);
            this.lbUpdateProgress.Name = "lbUpdateProgress";
            this.lbUpdateProgress.Size = new System.Drawing.Size(89, 12);
            this.lbUpdateProgress.TabIndex = 2;
            this.lbUpdateProgress.Text = "UpdateProgress";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 29;
            this.label5.Text = "Progress:";
            // 
            // btnStopUpdate
            // 
            this.btnStopUpdate.Location = new System.Drawing.Point(474, 68);
            this.btnStopUpdate.Name = "btnStopUpdate";
            this.btnStopUpdate.Size = new System.Drawing.Size(93, 23);
            this.btnStopUpdate.TabIndex = 30;
            this.btnStopUpdate.Text = "Stop Update";
            this.btnStopUpdate.UseVisualStyleBackColor = true;
            this.btnStopUpdate.Click += new System.EventHandler(this.btnStopUpdate_Click);
            // 
            // btnReadStatus
            // 
            this.btnReadStatus.Location = new System.Drawing.Point(474, 29);
            this.btnReadStatus.Name = "btnReadStatus";
            this.btnReadStatus.Size = new System.Drawing.Size(93, 23);
            this.btnReadStatus.TabIndex = 29;
            this.btnReadStatus.Text = "Read Status";
            this.btnReadStatus.UseVisualStyleBackColor = true;
            this.btnReadStatus.Click += new System.EventHandler(this.btnReadStatus_Click);
            // 
            // pbDongleUpdate
            // 
            this.pbDongleUpdate.Location = new System.Drawing.Point(75, 123);
            this.pbDongleUpdate.Name = "pbDongleUpdate";
            this.pbDongleUpdate.Size = new System.Drawing.Size(378, 23);
            this.pbDongleUpdate.TabIndex = 1;
            // 
            // tbDongleStatus
            // 
            this.tbDongleStatus.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tbDongleStatus.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbDongleStatus.Location = new System.Drawing.Point(75, 20);
            this.tbDongleStatus.Multiline = true;
            this.tbDongleStatus.Name = "tbDongleStatus";
            this.tbDongleStatus.ReadOnly = true;
            this.tbDongleStatus.Size = new System.Drawing.Size(378, 80);
            this.tbDongleStatus.TabIndex = 0;
            this.tbDongleStatus.TabStop = false;
            // 
            // lbDeviceFailNum
            // 
            this.lbDeviceFailNum.AutoSize = true;
            this.lbDeviceFailNum.Location = new System.Drawing.Point(371, 667);
            this.lbDeviceFailNum.Name = "lbDeviceFailNum";
            this.lbDeviceFailNum.Size = new System.Drawing.Size(41, 12);
            this.lbDeviceFailNum.TabIndex = 24;
            this.lbDeviceFailNum.Text = "label6";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(310, 667);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 23;
            this.label14.Text = "Failure:";
            // 
            // lbDeviceSuccessNum
            // 
            this.lbDeviceSuccessNum.AutoSize = true;
            this.lbDeviceSuccessNum.Location = new System.Drawing.Point(211, 667);
            this.lbDeviceSuccessNum.Name = "lbDeviceSuccessNum";
            this.lbDeviceSuccessNum.Size = new System.Drawing.Size(41, 12);
            this.lbDeviceSuccessNum.TabIndex = 22;
            this.lbDeviceSuccessNum.Text = "label6";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(151, 667);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 21;
            this.label12.Text = "Success:";
            // 
            // lbDeviceNum
            // 
            this.lbDeviceNum.AutoSize = true;
            this.lbDeviceNum.Location = new System.Drawing.Point(57, 667);
            this.lbDeviceNum.Name = "lbDeviceNum";
            this.lbDeviceNum.Size = new System.Drawing.Size(41, 12);
            this.lbDeviceNum.TabIndex = 19;
            this.lbDeviceNum.Text = "label6";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 667);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 20;
            this.label10.Text = "Total:";
            // 
            // lbdateVersion
            // 
            this.lbdateVersion.AutoSize = true;
            this.lbdateVersion.Location = new System.Drawing.Point(540, 667);
            this.lbdateVersion.Name = "lbdateVersion";
            this.lbdateVersion.Size = new System.Drawing.Size(65, 12);
            this.lbdateVersion.TabIndex = 25;
            this.lbdateVersion.Text = "2019-02-20";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 687);
            this.Controls.Add(this.lbdateVersion);
            this.Controls.Add(this.lbDeviceFailNum);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.lbDeviceSuccessNum);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lbDeviceNum);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.gbDongle);
            this.Controls.Add(this.gbSet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wireless Firmware Update";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gbSet.ResumeLayout(false);
            this.gbSet.PerformLayout();
            this.gbDongle.ResumeLayout(false);
            this.gbDongle.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSet;
        private System.Windows.Forms.CheckBox cbAskToUpdate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbFile;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Button btnComRefresh;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbCom;
        private System.Windows.Forms.Button btnStartUpdate;
        private System.Windows.Forms.TextBox tbNeedUpdateVersion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gbDongle;
        private System.Windows.Forms.TextBox tbDongleStatus;
        private System.Windows.Forms.Label lbBinSize;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbBinVersion;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbBinType;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ProgressBar pbDongleUpdate;
        private System.Windows.Forms.Button btnReadStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnStopUpdate;
        private System.Windows.Forms.Label lbUpdateProgress;
        private System.Windows.Forms.Label lbPercent;
        private System.Windows.Forms.Label lbDeviceFailNum;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lbDeviceSuccessNum;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lbDeviceNum;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbdateVersion;
    }
}

