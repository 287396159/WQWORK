namespace PrecisePosition
{
    partial class AlarmInfor
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
            this.AlarmInfor_textBox = new System.Windows.Forms.TextBox();
            this.ClearBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AlarmInfor_textBox
            // 
            this.AlarmInfor_textBox.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AlarmInfor_textBox.ForeColor = System.Drawing.Color.Red;
            this.AlarmInfor_textBox.Location = new System.Drawing.Point(0, -1);
            this.AlarmInfor_textBox.Multiline = true;
            this.AlarmInfor_textBox.Name = "AlarmInfor_textBox";
            this.AlarmInfor_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.AlarmInfor_textBox.Size = new System.Drawing.Size(931, 590);
            this.AlarmInfor_textBox.TabIndex = 0;
            // 
            // ClearBtn
            // 
            this.ClearBtn.BackColor = System.Drawing.Color.White;
            this.ClearBtn.Location = new System.Drawing.Point(0, 595);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(931, 32);
            this.ClearBtn.TabIndex = 1;
            this.ClearBtn.Text = "Clear";
            this.ClearBtn.UseVisualStyleBackColor = false;
            this.ClearBtn.Click += new System.EventHandler(this.ClearBtn_Click);
            // 
            // AlarmInfor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(932, 629);
            this.Controls.Add(this.ClearBtn);
            this.Controls.Add(this.AlarmInfor_textBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AlarmInfor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "The alarm information";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AlarmInfor_FormClosed);
            this.Load += new System.EventHandler(this.AlarmInfor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox AlarmInfor_textBox;
        private System.Windows.Forms.Button ClearBtn;
    }
}