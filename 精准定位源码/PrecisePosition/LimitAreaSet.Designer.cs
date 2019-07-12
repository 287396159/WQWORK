namespace PrecisePosition
{
    partial class LimitAreaSet
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
            this.id0tx = new System.Windows.Forms.TextBox();
            this.nametb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.id1tx = new System.Windows.Forms.TextBox();
            this.deleteBtn = new System.Windows.Forms.Button();
            this.setbtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID：";
            // 
            // id0tx
            // 
            this.id0tx.Location = new System.Drawing.Point(81, 22);
            this.id0tx.Name = "id0tx";
            this.id0tx.Size = new System.Drawing.Size(51, 21);
            this.id0tx.TabIndex = 1;
            // 
            // nametb
            // 
            this.nametb.Location = new System.Drawing.Point(81, 66);
            this.nametb.Name = "nametb";
            this.nametb.Size = new System.Drawing.Size(116, 21);
            this.nametb.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Name：";
            // 
            // id1tx
            // 
            this.id1tx.Location = new System.Drawing.Point(146, 22);
            this.id1tx.Name = "id1tx";
            this.id1tx.Size = new System.Drawing.Size(51, 21);
            this.id1tx.TabIndex = 6;
            // 
            // deleteBtn
            // 
            this.deleteBtn.BackColor = System.Drawing.Color.White;
            this.deleteBtn.Location = new System.Drawing.Point(122, 104);
            this.deleteBtn.Name = "deleteBtn";
            this.deleteBtn.Size = new System.Drawing.Size(75, 34);
            this.deleteBtn.TabIndex = 7;
            this.deleteBtn.Text = "Remove";
            this.deleteBtn.UseVisualStyleBackColor = false;
            this.deleteBtn.Click += new System.EventHandler(this.deleteBtn_Click);
            // 
            // setbtn
            // 
            this.setbtn.BackColor = System.Drawing.Color.White;
            this.setbtn.Location = new System.Drawing.Point(42, 104);
            this.setbtn.Name = "setbtn";
            this.setbtn.Size = new System.Drawing.Size(74, 34);
            this.setbtn.TabIndex = 8;
            this.setbtn.Text = "Set";
            this.setbtn.UseVisualStyleBackColor = false;
            this.setbtn.Click += new System.EventHandler(this.setbtn_Click);
            // 
            // LimitAreaSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 161);
            this.Controls.Add(this.setbtn);
            this.Controls.Add(this.deleteBtn);
            this.Controls.Add(this.id1tx);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nametb);
            this.Controls.Add(this.id0tx);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximumSize = new System.Drawing.Size(272, 197);
            this.MinimumSize = new System.Drawing.Size(272, 197);
            this.Name = "LimitAreaSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LimitAreaSet";
            this.Load += new System.EventHandler(this.LimitAreaSet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox id0tx;
        private System.Windows.Forms.TextBox nametb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox id1tx;
        private System.Windows.Forms.Button deleteBtn;
        private System.Windows.Forms.Button setbtn;
    }
}