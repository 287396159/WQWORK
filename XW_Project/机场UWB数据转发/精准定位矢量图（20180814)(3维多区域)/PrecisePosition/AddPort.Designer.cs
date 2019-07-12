namespace PrecisePosition
{
    partial class AddPort
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
            this.PortID_01_textBox = new System.Windows.Forms.TextBox();
            this.Add_Btn = new System.Windows.Forms.Button();
            this.Dele_Btn = new System.Windows.Forms.Button();
            this.PortID_02_textBox = new System.Windows.Forms.TextBox();
            this.GoBack_Btn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.PortHeightTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(14, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Reference \r\npoint ID：";
            // 
            // PortID_01_textBox
            // 
            this.PortID_01_textBox.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PortID_01_textBox.Location = new System.Drawing.Point(97, 24);
            this.PortID_01_textBox.Name = "PortID_01_textBox";
            this.PortID_01_textBox.Size = new System.Drawing.Size(47, 26);
            this.PortID_01_textBox.TabIndex = 2;
            // 
            // Add_Btn
            // 
            this.Add_Btn.BackColor = System.Drawing.Color.White;
            this.Add_Btn.Location = new System.Drawing.Point(17, 107);
            this.Add_Btn.Name = "Add_Btn";
            this.Add_Btn.Size = new System.Drawing.Size(212, 34);
            this.Add_Btn.TabIndex = 4;
            this.Add_Btn.Text = "setting";
            this.Add_Btn.UseVisualStyleBackColor = false;
            this.Add_Btn.Click += new System.EventHandler(this.Add_Btn_Click);
            // 
            // Dele_Btn
            // 
            this.Dele_Btn.BackColor = System.Drawing.Color.White;
            this.Dele_Btn.Location = new System.Drawing.Point(17, 147);
            this.Dele_Btn.Name = "Dele_Btn";
            this.Dele_Btn.Size = new System.Drawing.Size(212, 34);
            this.Dele_Btn.TabIndex = 5;
            this.Dele_Btn.Text = "Remove";
            this.Dele_Btn.UseVisualStyleBackColor = false;
            this.Dele_Btn.Click += new System.EventHandler(this.Dele_Btn_Click);
            // 
            // PortID_02_textBox
            // 
            this.PortID_02_textBox.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PortID_02_textBox.Location = new System.Drawing.Point(159, 24);
            this.PortID_02_textBox.Name = "PortID_02_textBox";
            this.PortID_02_textBox.Size = new System.Drawing.Size(47, 26);
            this.PortID_02_textBox.TabIndex = 6;
            // 
            // GoBack_Btn
            // 
            this.GoBack_Btn.BackColor = System.Drawing.Color.White;
            this.GoBack_Btn.Location = new System.Drawing.Point(17, 187);
            this.GoBack_Btn.Name = "GoBack_Btn";
            this.GoBack_Btn.Size = new System.Drawing.Size(212, 34);
            this.GoBack_Btn.TabIndex = 7;
            this.GoBack_Btn.Text = "Back";
            this.GoBack_Btn.UseVisualStyleBackColor = false;
            this.GoBack_Btn.Click += new System.EventHandler(this.GoBack_Btn_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(15, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 28);
            this.label2.TabIndex = 8;
            this.label2.Text = "Base station height:";
            // 
            // PortHeightTB
            // 
            this.PortHeightTB.Location = new System.Drawing.Point(97, 65);
            this.PortHeightTB.Name = "PortHeightTB";
            this.PortHeightTB.Size = new System.Drawing.Size(109, 21);
            this.PortHeightTB.TabIndex = 9;
            this.PortHeightTB.Text = "290";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(212, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "CM";
            // 
            // AddPort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 230);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PortHeightTB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.GoBack_Btn);
            this.Controls.Add(this.PortID_02_textBox);
            this.Controls.Add(this.Dele_Btn);
            this.Controls.Add(this.Add_Btn);
            this.Controls.Add(this.PortID_01_textBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "AddPort";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set the reference point";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddPort_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PortID_01_textBox;
        private System.Windows.Forms.Button Add_Btn;
        private System.Windows.Forms.Button Dele_Btn;
        private System.Windows.Forms.TextBox PortID_02_textBox;
        private System.Windows.Forms.Button GoBack_Btn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PortHeightTB;
        private System.Windows.Forms.Label label3;
    }
}