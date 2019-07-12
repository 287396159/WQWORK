namespace PrecisePosition
{
    partial class PortInfor
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
            this.Port_listView = new System.Windows.Forms.ListView();
            this.PortID_Column = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ReportTime_Column = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Ver_Column = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // Port_listView
            // 
            this.Port_listView.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.Port_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PortID_Column,
            this.ReportTime_Column,
            this.Ver_Column});
            this.Port_listView.FullRowSelect = true;
            this.Port_listView.GridLines = true;
            this.Port_listView.Location = new System.Drawing.Point(-1, -1);
            this.Port_listView.Name = "Port_listView";
            this.Port_listView.Size = new System.Drawing.Size(694, 421);
            this.Port_listView.TabIndex = 0;
            this.Port_listView.UseCompatibleStateImageBehavior = false;
            this.Port_listView.View = System.Windows.Forms.View.Details;
            this.Port_listView.SelectedIndexChanged += new System.EventHandler(this.Port_listView_SelectedIndexChanged);
            // 
            // PortID_Column
            // 
            this.PortID_Column.Text = "PortID";
            this.PortID_Column.Width = 139;
            // 
            // ReportTime_Column
            // 
            this.ReportTime_Column.Text = "Report the time";
            this.ReportTime_Column.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ReportTime_Column.Width = 357;
            // 
            // Ver_Column
            // 
            this.Ver_Column.Text = "Version";
            this.Ver_Column.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Ver_Column.Width = 186;
            // 
            // PortInfor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(697, 424);
            this.Controls.Add(this.Port_listView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PortInfor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reference point information";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PortInfor_FormClosed);
            this.Load += new System.EventHandler(this.PortInfor_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView Port_listView;
        private System.Windows.Forms.ColumnHeader PortID_Column;
        private System.Windows.Forms.ColumnHeader ReportTime_Column;
        private System.Windows.Forms.ColumnHeader Ver_Column;
    }
}