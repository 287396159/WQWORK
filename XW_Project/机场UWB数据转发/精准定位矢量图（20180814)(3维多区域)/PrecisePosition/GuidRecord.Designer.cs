namespace PrecisePosition
{
    partial class GuidRecord
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
            this.recordtb = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // recordtb
            // 
            this.recordtb.Location = new System.Drawing.Point(7, 9);
            this.recordtb.Multiline = true;
            this.recordtb.Name = "recordtb";
            this.recordtb.Size = new System.Drawing.Size(490, 470);
            this.recordtb.TabIndex = 0;
            // 
            // GuidRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 491);
            this.Controls.Add(this.recordtb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "GuidRecord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GuidRecord";
            this.Load += new System.EventHandler(this.GuidRecord_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox recordtb;
    }
}