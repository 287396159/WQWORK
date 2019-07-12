namespace PersionAutoLocaSys
{
    partial class SelecNodeWin
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
            this.nodelistview = new PersionAutoLocaSys.BuffListView();
            this.idcol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.namecol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Areacol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dtcol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // nodelistview
            // 
            this.nodelistview.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.idcol,
            this.namecol,
            this.Areacol,
            this.dtcol});
            this.nodelistview.FullRowSelect = true;
            this.nodelistview.GridLines = true;
            this.nodelistview.Location = new System.Drawing.Point(0, 0);
            this.nodelistview.Name = "nodelistview";
            this.nodelistview.Size = new System.Drawing.Size(615, 388);
            this.nodelistview.TabIndex = 0;
            this.nodelistview.UseCompatibleStateImageBehavior = false;
            this.nodelistview.View = System.Windows.Forms.View.Details;
            this.nodelistview.Click += new System.EventHandler(this.nodelistview_Click);
            // 
            // idcol
            // 
            this.idcol.Text = "ID";
            // 
            // namecol
            // 
            this.namecol.Text = "Name";
            this.namecol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.namecol.Width = 149;
            // 
            // Areacol
            // 
            this.Areacol.Text = "Area";
            this.Areacol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Areacol.Width = 184;
            // 
            // dtcol
            // 
            this.dtcol.Text = "Report Time";
            this.dtcol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.dtcol.Width = 211;
            // 
            // SelecNodeWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 393);
            this.Controls.Add(this.nodelistview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SelecNodeWin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select the node to set";
            this.Load += new System.EventHandler(this.SelecNodeWin_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private BuffListView nodelistview;
        private System.Windows.Forms.ColumnHeader idcol;
        private System.Windows.Forms.ColumnHeader namecol;
        private System.Windows.Forms.ColumnHeader Areacol;
        private System.Windows.Forms.ColumnHeader dtcol;
    }
}