namespace PersionAutoLocaSys
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
            this.MainTitle_Panel = new System.Windows.Forms.Panel();
            this.Sig_panel = new System.Windows.Forms.Panel();
            this.SigP2 = new System.Windows.Forms.Panel();
            this.SigP1 = new System.Windows.Forms.Panel();
            this.MainCenter_Panel = new System.Windows.Forms.Panel();
            this.MainTitle_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTitle_Panel
            // 
            this.MainTitle_Panel.BackColor = System.Drawing.Color.White;
            this.MainTitle_Panel.Controls.Add(this.Sig_panel);
            this.MainTitle_Panel.Controls.Add(this.SigP2);
            this.MainTitle_Panel.Controls.Add(this.SigP1);
            this.MainTitle_Panel.Location = new System.Drawing.Point(1, 0);
            this.MainTitle_Panel.Name = "MainTitle_Panel";
            this.MainTitle_Panel.Size = new System.Drawing.Size(916, 124);
            this.MainTitle_Panel.TabIndex = 0;
            this.MainTitle_Panel.Paint += new System.Windows.Forms.PaintEventHandler(this.MainTitle_Panel_Paint);
            this.MainTitle_Panel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MainTitle_Panel_MouseDoubleClick);
            this.MainTitle_Panel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainTitle_Panel_MouseDown);
            this.MainTitle_Panel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainTitle_Panel_MouseMove);
            this.MainTitle_Panel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainTitle_Panel_MouseUp);
            // 
            // Sig_panel
            // 
            this.Sig_panel.BackColor = System.Drawing.Color.White;
            this.Sig_panel.Location = new System.Drawing.Point(427, 54);
            this.Sig_panel.Name = "Sig_panel";
            this.Sig_panel.Size = new System.Drawing.Size(126, 8);
            this.Sig_panel.TabIndex = 2;
            // 
            // SigP2
            // 
            this.SigP2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.SigP2.Location = new System.Drawing.Point(611, 41);
            this.SigP2.Name = "SigP2";
            this.SigP2.Size = new System.Drawing.Size(13, 40);
            this.SigP2.TabIndex = 1;
            // 
            // SigP1
            // 
            this.SigP1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.SigP1.Location = new System.Drawing.Point(572, 41);
            this.SigP1.Name = "SigP1";
            this.SigP1.Size = new System.Drawing.Size(13, 40);
            this.SigP1.TabIndex = 0;
            // 
            // MainCenter_Panel
            // 
            this.MainCenter_Panel.Location = new System.Drawing.Point(1, 130);
            this.MainCenter_Panel.Name = "MainCenter_Panel";
            this.MainCenter_Panel.Size = new System.Drawing.Size(914, 486);
            this.MainCenter_Panel.TabIndex = 1;
            this.MainCenter_Panel.Paint += new System.Windows.Forms.PaintEventHandler(this.MainCenter_Panel_Paint);
            this.MainCenter_Panel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainCenter_Panel_MouseClick);
            this.MainCenter_Panel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainCenter_Panel_MouseDown);
            this.MainCenter_Panel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainCenter_Panel_MouseMove);
            this.MainCenter_Panel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainCenter_Panel_MouseUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 616);
            this.Controls.Add(this.MainCenter_Panel);
            this.Controls.Add(this.MainTitle_Panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "人員定位系統V11.2.19";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MainTitle_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MainTitle_Panel;
        public System.Windows.Forms.Panel MainCenter_Panel;
        public System.Windows.Forms.Panel SigP1;
        public System.Windows.Forms.Panel SigP2;
        public System.Windows.Forms.Panel Sig_panel;
    }
}

