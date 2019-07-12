namespace PrecisePosition
{
    partial class Current_OnLine
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
            this.Current_OnLineListView = new System.Windows.Forms.ListView();
            this.CardID_Column = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CardName_Column = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CardLocaType_Column = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.GroupColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Battery_Column = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.No_Exe_Time_Column = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LastReceive_Time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DisLastReceiveTime_Column = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Gsensor_Column = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // Current_OnLineListView
            // 
            this.Current_OnLineListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.CardID_Column,
            this.CardName_Column,
            this.CardLocaType_Column,
            this.GroupColumn,
            this.Battery_Column,
            this.No_Exe_Time_Column,
            this.LastReceive_Time,
            this.DisLastReceiveTime_Column,
            this.Gsensor_Column});
            this.Current_OnLineListView.FullRowSelect = true;
            this.Current_OnLineListView.GridLines = true;
            this.Current_OnLineListView.Location = new System.Drawing.Point(1, 1);
            this.Current_OnLineListView.Name = "Current_OnLineListView";
            this.Current_OnLineListView.Size = new System.Drawing.Size(1142, 674);
            this.Current_OnLineListView.TabIndex = 0;
            this.Current_OnLineListView.UseCompatibleStateImageBehavior = false;
            this.Current_OnLineListView.View = System.Windows.Forms.View.Details;
            this.Current_OnLineListView.SelectedIndexChanged += new System.EventHandler(this.Current_OnLineListView_SelectedIndexChanged);
            // 
            // CardID_Column
            // 
            this.CardID_Column.Text = "CardID";
            this.CardID_Column.Width = 55;
            // 
            // CardName_Column
            // 
            this.CardName_Column.Text = "CardName";
            this.CardName_Column.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CardLocaType_Column
            // 
            this.CardLocaType_Column.Text = "CardLocaType";
            this.CardLocaType_Column.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CardLocaType_Column.Width = 97;
            // 
            // GroupColumn
            // 
            this.GroupColumn.Text = "Group";
            this.GroupColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Battery_Column
            // 
            this.Battery_Column.Text = "Battery";
            this.Battery_Column.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Battery_Column.Width = 57;
            // 
            // No_Exe_Time_Column
            // 
            this.No_Exe_Time_Column.Text = "Have no time to move";
            this.No_Exe_Time_Column.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.No_Exe_Time_Column.Width = 152;
            // 
            // LastReceive_Time
            // 
            this.LastReceive_Time.Text = "The last time to accept data of time";
            this.LastReceive_Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.LastReceive_Time.Width = 242;
            // 
            // DisLastReceiveTime_Column
            // 
            this.DisLastReceiveTime_Column.Text = "The last time to receive data";
            this.DisLastReceiveTime_Column.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DisLastReceiveTime_Column.Width = 203;
            // 
            // Gsensor_Column
            // 
            this.Gsensor_Column.Text = "Gsensor";
            this.Gsensor_Column.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Gsensor_Column.Width = 208;
            // 
            // Current_OnLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1155, 676);
            this.Controls.Add(this.Current_OnLineListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Current_OnLine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "The current online card";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Current_OnLine_FormClosed);
            this.Load += new System.EventHandler(this.Current_OnLine_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView Current_OnLineListView;
        private System.Windows.Forms.ColumnHeader CardID_Column;
        private System.Windows.Forms.ColumnHeader CardName_Column;
        private System.Windows.Forms.ColumnHeader CardLocaType_Column;
        private System.Windows.Forms.ColumnHeader Battery_Column;
        private System.Windows.Forms.ColumnHeader No_Exe_Time_Column;
        private System.Windows.Forms.ColumnHeader LastReceive_Time;
        private System.Windows.Forms.ColumnHeader DisLastReceiveTime_Column;
        private System.Windows.Forms.ColumnHeader GroupColumn;
        private System.Windows.Forms.ColumnHeader Gsensor_Column;
    }
}