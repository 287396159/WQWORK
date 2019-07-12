namespace PersionAutoLocaSys
{
    partial class UpdateVerifyWin
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
            this.okbtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.pswtxt = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(13, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "請輸入驗證密碼:";
            // 
            // okbtn
            // 
            this.okbtn.BackColor = System.Drawing.Color.White;
            this.okbtn.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.okbtn.Location = new System.Drawing.Point(252, 15);
            this.okbtn.Name = "okbtn";
            this.okbtn.Size = new System.Drawing.Size(85, 34);
            this.okbtn.TabIndex = 2;
            this.okbtn.Text = "確  定";
            this.okbtn.UseVisualStyleBackColor = false;
            this.okbtn.Click += new System.EventHandler(this.okbtn_Click);
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(13, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(307, 27);
            this.label2.TabIndex = 3;
            this.label2.Text = "注：如需更新固件請聯繫本公司工作人員，取得相應固件及密碼后才可進行更新";
            // 
            // pswtxt
            // 
            this.pswtxt.Location = new System.Drawing.Point(118, 23);
            this.pswtxt.Name = "pswtxt";
            this.pswtxt.PromptChar = '*';
            this.pswtxt.Size = new System.Drawing.Size(124, 21);
            this.pswtxt.TabIndex = 4;
            this.pswtxt.UseSystemPasswordChar = true;
            // 
            // UpdateVerifyWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 89);
            this.Controls.Add(this.pswtxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.okbtn);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "UpdateVerifyWin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "密碼驗證";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button okbtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox pswtxt;
    }
}