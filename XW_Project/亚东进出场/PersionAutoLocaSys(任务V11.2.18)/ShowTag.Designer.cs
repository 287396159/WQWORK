namespace PersionAutoLocaSys
{
    partial class ShowTag
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
            this.RouterTaglistView = new BuffListView();
            this.TagCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SigStrenCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TagBatteryCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NoExeTimeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ReportTimeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // RouterTaglistView
            // 
            this.RouterTaglistView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TagCol,
            this.SigStrenCol,
            this.TagBatteryCol,
            this.NoExeTimeCol,
            this.ReportTimeCol});
            this.RouterTaglistView.FullRowSelect = true;
            this.RouterTaglistView.GridLines = true;
            this.RouterTaglistView.Location = new System.Drawing.Point(7, 7);
            this.RouterTaglistView.Name = "RouterTaglistView";
            this.RouterTaglistView.Size = new System.Drawing.Size(743, 393);
            this.RouterTaglistView.TabIndex = 1;
            this.RouterTaglistView.UseCompatibleStateImageBehavior = false;
            this.RouterTaglistView.View = System.Windows.Forms.View.Details;
            // 
            // TagCol
            // 
            this.TagCol.Text = "卡片";
            this.TagCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TagCol.Width = 131;
            // 
            // SigStrenCol
            // 
            this.SigStrenCol.Text = "信號強度";
            this.SigStrenCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SigStrenCol.Width = 87;
            // 
            // TagBatteryCol
            // 
            this.TagBatteryCol.Text = "卡片電量";
            this.TagBatteryCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TagBatteryCol.Width = 93;
            // 
            // NoExeTimeCol
            // 
            this.NoExeTimeCol.Text = "未移動時間";
            this.NoExeTimeCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NoExeTimeCol.Width = 120;
            // 
            // ReportTimeCol
            // 
            this.ReportTimeCol.Text = "數據上報時間";
            this.ReportTimeCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ReportTimeCol.Width = 245;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 406);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tag的總數量為：0";
            // 
            // ShowTag
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 421);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RouterTaglistView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ShowTag";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "當前的參考點：";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShowTag_FormClosing);
            this.Load += new System.EventHandler(this.ShowTag_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BuffListView RouterTaglistView;
        private System.Windows.Forms.ColumnHeader TagCol;
        private System.Windows.Forms.ColumnHeader SigStrenCol;
        private System.Windows.Forms.ColumnHeader TagBatteryCol;
        private System.Windows.Forms.ColumnHeader NoExeTimeCol;
        private System.Windows.Forms.ColumnHeader ReportTimeCol;
        private System.Windows.Forms.Label label1;

    }
}