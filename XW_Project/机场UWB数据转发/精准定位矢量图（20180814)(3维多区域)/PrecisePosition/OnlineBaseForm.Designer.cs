namespace PrecisePosition
{
    partial class OnlineBaseForm
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
            this.PortListView = new System.Windows.Forms.ListView();
            this.IDCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.GroupCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IntervalCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ReportCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VersionCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // PortListView
            // 
            this.PortListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IDCol,
            this.GroupCol,
            this.IntervalCol,
            this.ReportCol,
            this.VersionCol});
            this.PortListView.FullRowSelect = true;
            this.PortListView.GridLines = true;
            this.PortListView.Location = new System.Drawing.Point(0, 0);
            this.PortListView.Name = "PortListView";
            this.PortListView.Size = new System.Drawing.Size(992, 646);
            this.PortListView.TabIndex = 0;
            this.PortListView.UseCompatibleStateImageBehavior = false;
            this.PortListView.View = System.Windows.Forms.View.Details;
            // 
            // IDCol
            // 
            this.IDCol.Text = "PortID";
            this.IDCol.Width = 134;
            // 
            // GroupCol
            // 
            this.GroupCol.Text = "Group";
            this.GroupCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.GroupCol.Width = 145;
            // 
            // IntervalCol
            // 
            this.IntervalCol.Text = "Interval";
            this.IntervalCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.IntervalCol.Width = 176;
            // 
            // ReportCol
            // 
            this.ReportCol.Text = "ReportTime";
            this.ReportCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ReportCol.Width = 317;
            // 
            // VersionCol
            // 
            this.VersionCol.Text = "Version";
            this.VersionCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.VersionCol.Width = 204;
            // 
            // OnlineBaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 648);
            this.Controls.Add(this.PortListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OnlineBaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Online base station statistics";
            this.Load += new System.EventHandler(this.OnlineBaseForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView PortListView;
        private System.Windows.Forms.ColumnHeader IDCol;
        private System.Windows.Forms.ColumnHeader ReportCol;
        private System.Windows.Forms.ColumnHeader VersionCol;
        private System.Windows.Forms.ColumnHeader IntervalCol;
        private System.Windows.Forms.ColumnHeader GroupCol;
    }
}