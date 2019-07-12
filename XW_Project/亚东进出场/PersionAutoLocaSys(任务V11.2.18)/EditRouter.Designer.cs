namespace PersionAutoLocaSys
{
    partial class EditRouter
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
            this.RID_1TB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.RID_2TB = new System.Windows.Forms.TextBox();
            this.SetBt = new System.Windows.Forms.Button();
            this.DeleBt = new System.Windows.Forms.Button();
            this.BackBt = new System.Windows.Forms.Button();
            this.RouterVisibleCB = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.NodeTypeCB = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ReferNameTB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // RID_1TB
            // 
            this.RID_1TB.Location = new System.Drawing.Point(67, 18);
            this.RID_1TB.Name = "RID_1TB";
            this.RID_1TB.Size = new System.Drawing.Size(66, 21);
            this.RID_1TB.TabIndex = 0;
            this.RID_1TB.Text = "00";
            this.RID_1TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(28, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "ID：";
            // 
            // RID_2TB
            // 
            this.RID_2TB.Location = new System.Drawing.Point(152, 18);
            this.RID_2TB.Name = "RID_2TB";
            this.RID_2TB.Size = new System.Drawing.Size(66, 21);
            this.RID_2TB.TabIndex = 2;
            this.RID_2TB.Text = "00";
            this.RID_2TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SetBt
            // 
            this.SetBt.BackColor = System.Drawing.Color.White;
            this.SetBt.Location = new System.Drawing.Point(24, 141);
            this.SetBt.Name = "SetBt";
            this.SetBt.Size = new System.Drawing.Size(194, 26);
            this.SetBt.TabIndex = 3;
            this.SetBt.Text = "設   置";
            this.SetBt.UseVisualStyleBackColor = false;
            this.SetBt.Click += new System.EventHandler(this.SetBt_Click);
            // 
            // DeleBt
            // 
            this.DeleBt.BackColor = System.Drawing.Color.White;
            this.DeleBt.Location = new System.Drawing.Point(24, 175);
            this.DeleBt.Name = "DeleBt";
            this.DeleBt.Size = new System.Drawing.Size(194, 26);
            this.DeleBt.TabIndex = 4;
            this.DeleBt.Text = "刪    除";
            this.DeleBt.UseVisualStyleBackColor = false;
            this.DeleBt.Click += new System.EventHandler(this.DeleBt_Click);
            // 
            // BackBt
            // 
            this.BackBt.BackColor = System.Drawing.Color.White;
            this.BackBt.Location = new System.Drawing.Point(24, 209);
            this.BackBt.Name = "BackBt";
            this.BackBt.Size = new System.Drawing.Size(194, 26);
            this.BackBt.TabIndex = 5;
            this.BackBt.Text = "返   回";
            this.BackBt.UseVisualStyleBackColor = false;
            this.BackBt.Click += new System.EventHandler(this.BackBt_Click);
            // 
            // RouterVisibleCB
            // 
            this.RouterVisibleCB.AutoSize = true;
            this.RouterVisibleCB.Checked = true;
            this.RouterVisibleCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RouterVisibleCB.Location = new System.Drawing.Point(80, 114);
            this.RouterVisibleCB.Name = "RouterVisibleCB";
            this.RouterVisibleCB.Size = new System.Drawing.Size(72, 16);
            this.RouterVisibleCB.TabIndex = 7;
            this.RouterVisibleCB.Text = "是否可見";
            this.RouterVisibleCB.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "類型：";
            // 
            // NodeTypeCB
            // 
            this.NodeTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.NodeTypeCB.FormattingEnabled = true;
            this.NodeTypeCB.Items.AddRange(new object[] {
            "參考點",
            "數據節點"});
            this.NodeTypeCB.Location = new System.Drawing.Point(67, 83);
            this.NodeTypeCB.Name = "NodeTypeCB";
            this.NodeTypeCB.Size = new System.Drawing.Size(151, 20);
            this.NodeTypeCB.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "Name：";
            // 
            // ReferNameTB
            // 
            this.ReferNameTB.Location = new System.Drawing.Point(67, 51);
            this.ReferNameTB.Name = "ReferNameTB";
            this.ReferNameTB.Size = new System.Drawing.Size(151, 21);
            this.ReferNameTB.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(23, 242);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(202, 30);
            this.label4.TabIndex = 13;
            this.label4.Text = "注：在添加或修改參考點時，不允許參考點和數據節點的ID相同";
            // 
            // EditRouter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 272);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ReferNameTB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.NodeTypeCB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RouterVisibleCB);
            this.Controls.Add(this.BackBt);
            this.Controls.Add(this.DeleBt);
            this.Controls.Add(this.SetBt);
            this.Controls.Add(this.RID_2TB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RID_1TB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "EditRouter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "編輯參考點/數據節點";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditRouter_FormClosing);
            this.Load += new System.EventHandler(this.EditRouter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox RID_1TB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox RID_2TB;
        private System.Windows.Forms.Button SetBt;
        private System.Windows.Forms.Button DeleBt;
        private System.Windows.Forms.Button BackBt;
        private System.Windows.Forms.CheckBox RouterVisibleCB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox NodeTypeCB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ReferNameTB;
        private System.Windows.Forms.Label label4;
    }
}