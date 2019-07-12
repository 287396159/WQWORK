namespace PersionAutoLocaSys
{
    partial class AlarmInfoWin
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
            this.PersonAlarmListView = new BuffListView();
            this.TagCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AreaCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RouterCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AlarmCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ClearAlarmCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AlarmHandlerCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.PsAlarmAllSelecCB = new System.Windows.Forms.CheckBox();
            this.ClearAlarmTagsBtn = new System.Windows.Forms.Button();
            this.HandlerAlarmBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SearchTB = new System.Windows.Forms.TextBox();
            this.SearchBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PersonAlarmListView
            // 
            this.PersonAlarmListView.CheckBoxes = true;
            this.PersonAlarmListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TagCol,
            this.AreaCol,
            this.RouterCol,
            this.AlarmCol,
            this.ClearAlarmCol,
            this.AlarmHandlerCol});
            this.PersonAlarmListView.FullRowSelect = true;
            this.PersonAlarmListView.GridLines = true;
            this.PersonAlarmListView.Location = new System.Drawing.Point(12, 37);
            this.PersonAlarmListView.Name = "PersonAlarmListView";
            this.PersonAlarmListView.Size = new System.Drawing.Size(912, 505);
            this.PersonAlarmListView.TabIndex = 0;
            this.PersonAlarmListView.UseCompatibleStateImageBehavior = false;
            this.PersonAlarmListView.View = System.Windows.Forms.View.Details;
            this.PersonAlarmListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.PersonAlarmListView_ItemChecked);
            // 
            // TagCol
            // 
            this.TagCol.Text = "卡片";
            this.TagCol.Width = 125;
            // 
            // AreaCol
            // 
            this.AreaCol.Text = "所在區域";
            this.AreaCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AreaCol.Width = 142;
            // 
            // RouterCol
            // 
            this.RouterCol.Text = "參考點";
            this.RouterCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RouterCol.Width = 106;
            // 
            // AlarmCol
            // 
            this.AlarmCol.Text = "警報產生時間";
            this.AlarmCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AlarmCol.Width = 233;
            // 
            // ClearAlarmCol
            // 
            this.ClearAlarmCol.Text = "警報消除時間";
            this.ClearAlarmCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ClearAlarmCol.Width = 222;
            // 
            // AlarmHandlerCol
            // 
            this.AlarmHandlerCol.Text = "是否處理";
            this.AlarmHandlerCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AlarmHandlerCol.Width = 63;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "報警資訊數量：0";
            // 
            // PsAlarmAllSelecCB
            // 
            this.PsAlarmAllSelecCB.AutoSize = true;
            this.PsAlarmAllSelecCB.Location = new System.Drawing.Point(16, 12);
            this.PsAlarmAllSelecCB.Name = "PsAlarmAllSelecCB";
            this.PsAlarmAllSelecCB.Size = new System.Drawing.Size(48, 16);
            this.PsAlarmAllSelecCB.TabIndex = 2;
            this.PsAlarmAllSelecCB.Text = "全選";
            this.PsAlarmAllSelecCB.UseVisualStyleBackColor = true;
            this.PsAlarmAllSelecCB.CheckedChanged += new System.EventHandler(this.PsAlarmAllSelecCB_CheckedChanged);
            // 
            // ClearAlarmTagsBtn
            // 
            this.ClearAlarmTagsBtn.BackColor = System.Drawing.Color.White;
            this.ClearAlarmTagsBtn.Location = new System.Drawing.Point(825, 6);
            this.ClearAlarmTagsBtn.Name = "ClearAlarmTagsBtn";
            this.ClearAlarmTagsBtn.Size = new System.Drawing.Size(93, 28);
            this.ClearAlarmTagsBtn.TabIndex = 3;
            this.ClearAlarmTagsBtn.Text = "清除警報";
            this.ClearAlarmTagsBtn.UseVisualStyleBackColor = false;
            this.ClearAlarmTagsBtn.Click += new System.EventHandler(this.ClearAlarmTagsBtn_Click);
            // 
            // HandlerAlarmBtn
            // 
            this.HandlerAlarmBtn.BackColor = System.Drawing.Color.White;
            this.HandlerAlarmBtn.Location = new System.Drawing.Point(726, 6);
            this.HandlerAlarmBtn.Name = "HandlerAlarmBtn";
            this.HandlerAlarmBtn.Size = new System.Drawing.Size(93, 28);
            this.HandlerAlarmBtn.TabIndex = 4;
            this.HandlerAlarmBtn.Text = "處理警告";
            this.HandlerAlarmBtn.UseVisualStyleBackColor = false;
            this.HandlerAlarmBtn.Click += new System.EventHandler(this.HandlerAlarmBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(267, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "查找：";
            // 
            // SearchTB
            // 
            this.SearchTB.Location = new System.Drawing.Point(315, 9);
            this.SearchTB.Name = "SearchTB";
            this.SearchTB.Size = new System.Drawing.Size(100, 21);
            this.SearchTB.TabIndex = 6;
            this.SearchTB.TextChanged += new System.EventHandler(this.SearchTB_TextChanged);
            // 
            // SearchBtn
            // 
            this.SearchBtn.BackColor = System.Drawing.Color.White;
            this.SearchBtn.Location = new System.Drawing.Point(421, 5);
            this.SearchBtn.Name = "SearchBtn";
            this.SearchBtn.Size = new System.Drawing.Size(93, 28);
            this.SearchBtn.TabIndex = 7;
            this.SearchBtn.Text = "Search";
            this.SearchBtn.UseVisualStyleBackColor = false;
            this.SearchBtn.Click += new System.EventHandler(this.SearchBtn_Click);
            // 
            // AlarmInfoWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 554);
            this.Controls.Add(this.SearchBtn);
            this.Controls.Add(this.SearchTB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.HandlerAlarmBtn);
            this.Controls.Add(this.ClearAlarmTagsBtn);
            this.Controls.Add(this.PsAlarmAllSelecCB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PersonAlarmListView);
            this.Name = "AlarmInfoWin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "人員求救資訊";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AlarmInfoWin_FormClosing);
            this.Load += new System.EventHandler(this.AlarmInfoWin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public BuffListView PersonAlarmListView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox PsAlarmAllSelecCB;
        private System.Windows.Forms.ColumnHeader TagCol;
        private System.Windows.Forms.ColumnHeader AreaCol;
        private System.Windows.Forms.ColumnHeader RouterCol;
        private System.Windows.Forms.ColumnHeader AlarmCol;
        private System.Windows.Forms.ColumnHeader ClearAlarmCol;
        private System.Windows.Forms.Button ClearAlarmTagsBtn;
        private System.Windows.Forms.Button HandlerAlarmBtn;
        private System.Windows.Forms.ColumnHeader AlarmHandlerCol;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SearchTB;
        private System.Windows.Forms.Button SearchBtn;
    }
}