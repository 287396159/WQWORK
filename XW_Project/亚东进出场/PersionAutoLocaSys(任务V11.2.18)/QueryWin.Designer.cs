namespace PersionAutoLocaSys
{
    partial class QueryWin
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
            this.TagIDorNameTB = new System.Windows.Forms.TextBox();
            this.SearchBtn = new System.Windows.Forms.Button();
            this.tagrb = new System.Windows.Forms.RadioButton();
            this.rfrb = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ndrb = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TagIDorNameTB
            // 
            this.TagIDorNameTB.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TagIDorNameTB.Location = new System.Drawing.Point(23, 79);
            this.TagIDorNameTB.Multiline = true;
            this.TagIDorNameTB.Name = "TagIDorNameTB";
            this.TagIDorNameTB.Size = new System.Drawing.Size(147, 23);
            this.TagIDorNameTB.TabIndex = 1;
            // 
            // SearchBtn
            // 
            this.SearchBtn.BackColor = System.Drawing.Color.Transparent;
            this.SearchBtn.BackgroundImage = global::PersionAutoLocaSys.Properties.Resources.srh_1;
            this.SearchBtn.Location = new System.Drawing.Point(193, 71);
            this.SearchBtn.Margin = new System.Windows.Forms.Padding(0);
            this.SearchBtn.Name = "SearchBtn";
            this.SearchBtn.Size = new System.Drawing.Size(75, 36);
            this.SearchBtn.TabIndex = 3;
            this.SearchBtn.UseVisualStyleBackColor = false;
            this.SearchBtn.Click += new System.EventHandler(this.SearchBtn_Click);
            // 
            // tagrb
            // 
            this.tagrb.AutoSize = true;
            this.tagrb.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.tagrb.BackgroundImage = global::PersionAutoLocaSys.Properties.Resources.selectbg;
            this.tagrb.Checked = true;
            this.tagrb.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tagrb.ForeColor = System.Drawing.Color.OrangeRed;
            this.tagrb.Location = new System.Drawing.Point(25, 8);
            this.tagrb.Margin = new System.Windows.Forms.Padding(0);
            this.tagrb.Name = "tagrb";
            this.tagrb.Size = new System.Drawing.Size(44, 16);
            this.tagrb.TabIndex = 4;
            this.tagrb.Text = "Tag";
            this.tagrb.UseVisualStyleBackColor = false;
            // 
            // rfrb
            // 
            this.rfrb.AutoSize = true;
            this.rfrb.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.rfrb.BackgroundImage = global::PersionAutoLocaSys.Properties.Resources.selectbg;
            this.rfrb.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rfrb.ForeColor = System.Drawing.Color.OrangeRed;
            this.rfrb.Location = new System.Drawing.Point(86, 8);
            this.rfrb.Margin = new System.Windows.Forms.Padding(0);
            this.rfrb.Name = "rfrb";
            this.rfrb.Size = new System.Drawing.Size(58, 16);
            this.rfrb.TabIndex = 5;
            this.rfrb.Text = "Refer";
            this.rfrb.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::PersionAutoLocaSys.Properties.Resources.selectbg;
            this.panel1.Controls.Add(this.ndrb);
            this.panel1.Controls.Add(this.rfrb);
            this.panel1.Controls.Add(this.tagrb);
            this.panel1.Location = new System.Drawing.Point(16, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(231, 30);
            this.panel1.TabIndex = 6;
            // 
            // ndrb
            // 
            this.ndrb.AutoSize = true;
            this.ndrb.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ndrb.BackgroundImage = global::PersionAutoLocaSys.Properties.Resources.selectbg;
            this.ndrb.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ndrb.ForeColor = System.Drawing.Color.OrangeRed;
            this.ndrb.Location = new System.Drawing.Point(161, 8);
            this.ndrb.Margin = new System.Windows.Forms.Padding(0);
            this.ndrb.Name = "ndrb";
            this.ndrb.Size = new System.Drawing.Size(51, 16);
            this.ndrb.TabIndex = 6;
            this.ndrb.Text = "Node";
            this.ndrb.UseVisualStyleBackColor = false;
            // 
            // QueryWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PersionAutoLocaSys.Properties.Resources.srh_bj;
            this.ClientSize = new System.Drawing.Size(304, 139);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.SearchBtn);
            this.Controls.Add(this.TagIDorNameTB);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "QueryWin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "卡片查詢框";
            this.Load += new System.EventHandler(this.QueryWin_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.QueryWin_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.QueryWin_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.QueryWin_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.QueryWin_MouseUp);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.TextBox TagIDorNameTB;
        private System.Windows.Forms.Button SearchBtn;
        private System.Windows.Forms.RadioButton tagrb;
        private System.Windows.Forms.RadioButton rfrb;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton ndrb;
    }
}