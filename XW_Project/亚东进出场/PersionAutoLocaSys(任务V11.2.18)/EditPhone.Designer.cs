namespace PersionAutoLocaSys
{
    partial class EditPhone
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
            this.PersonNumberLV = new System.Windows.Forms.ListView();
            this.IDCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PhoneCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.UpdateLeftBtn = new System.Windows.Forms.Button();
            this.PhoneNumberTB = new System.Windows.Forms.TextBox();
            this.NameTB = new System.Windows.Forms.TextBox();
            this.IDTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.AddBtn = new System.Windows.Forms.Button();
            this.DeleBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PersonNumberLV
            // 
            this.PersonNumberLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IDCol,
            this.NameCol,
            this.PhoneCol});
            this.PersonNumberLV.FullRowSelect = true;
            this.PersonNumberLV.GridLines = true;
            this.PersonNumberLV.Location = new System.Drawing.Point(13, 12);
            this.PersonNumberLV.Name = "PersonNumberLV";
            this.PersonNumberLV.Size = new System.Drawing.Size(232, 283);
            this.PersonNumberLV.TabIndex = 0;
            this.PersonNumberLV.UseCompatibleStateImageBehavior = false;
            this.PersonNumberLV.View = System.Windows.Forms.View.Details;
            this.PersonNumberLV.Click += new System.EventHandler(this.PersonNumberLV_Click);
            // 
            // IDCol
            // 
            this.IDCol.Text = "ID";
            this.IDCol.Width = 38;
            // 
            // NameCol
            // 
            this.NameCol.Text = " 名称";
            this.NameCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NameCol.Width = 48;
            // 
            // PhoneCol
            // 
            this.PhoneCol.Text = "电话号码";
            this.PhoneCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.PhoneCol.Width = 122;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.UpdateLeftBtn);
            this.groupBox1.Controls.Add(this.PhoneNumberTB);
            this.groupBox1.Controls.Add(this.NameTB);
            this.groupBox1.Controls.Add(this.IDTB);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(252, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(181, 148);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "編輯人員信息";
            // 
            // UpdateLeftBtn
            // 
            this.UpdateLeftBtn.BackColor = System.Drawing.Color.White;
            this.UpdateLeftBtn.Location = new System.Drawing.Point(7, 107);
            this.UpdateLeftBtn.Name = "UpdateLeftBtn";
            this.UpdateLeftBtn.Size = new System.Drawing.Size(168, 30);
            this.UpdateLeftBtn.TabIndex = 6;
            this.UpdateLeftBtn.Text = "左側更新列表";
            this.UpdateLeftBtn.UseVisualStyleBackColor = false;
            this.UpdateLeftBtn.Click += new System.EventHandler(this.UpdateLeftBtn_Click);
            // 
            // PhoneNumberTB
            // 
            this.PhoneNumberTB.Location = new System.Drawing.Point(53, 77);
            this.PhoneNumberTB.Name = "PhoneNumberTB";
            this.PhoneNumberTB.Size = new System.Drawing.Size(108, 21);
            this.PhoneNumberTB.TabIndex = 5;
            // 
            // NameTB
            // 
            this.NameTB.Location = new System.Drawing.Point(53, 47);
            this.NameTB.Name = "NameTB";
            this.NameTB.Size = new System.Drawing.Size(108, 21);
            this.NameTB.TabIndex = 4;
            // 
            // IDTB
            // 
            this.IDTB.Enabled = false;
            this.IDTB.Location = new System.Drawing.Point(53, 19);
            this.IDTB.Name = "IDTB";
            this.IDTB.Size = new System.Drawing.Size(108, 21);
            this.IDTB.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "號碼：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "姓名：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID：";
            // 
            // AddBtn
            // 
            this.AddBtn.BackColor = System.Drawing.Color.White;
            this.AddBtn.Location = new System.Drawing.Point(251, 12);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(90, 32);
            this.AddBtn.TabIndex = 2;
            this.AddBtn.Text = "添    加";
            this.AddBtn.UseVisualStyleBackColor = false;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // DeleBtn
            // 
            this.DeleBtn.BackColor = System.Drawing.Color.White;
            this.DeleBtn.Location = new System.Drawing.Point(343, 12);
            this.DeleBtn.Name = "DeleBtn";
            this.DeleBtn.Size = new System.Drawing.Size(90, 32);
            this.DeleBtn.TabIndex = 3;
            this.DeleBtn.Text = "删    除";
            this.DeleBtn.UseVisualStyleBackColor = false;
            this.DeleBtn.Click += new System.EventHandler(this.DeleBtn_Click);
            // 
            // EditPhone
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 306);
            this.Controls.Add(this.DeleBtn);
            this.Controls.Add(this.AddBtn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.PersonNumberLV);
            this.Name = "EditPhone";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "人員信息";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.EditPhone_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView PersonNumberLV;
        private System.Windows.Forms.ColumnHeader IDCol;
        private System.Windows.Forms.ColumnHeader NameCol;
        private System.Windows.Forms.ColumnHeader PhoneCol;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.Button DeleBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button UpdateLeftBtn;
        private System.Windows.Forms.TextBox PhoneNumberTB;
        private System.Windows.Forms.TextBox NameTB;
        private System.Windows.Forms.TextBox IDTB;
    }
}