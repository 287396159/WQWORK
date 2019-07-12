using CiXinLocation.Properties;
namespace MoveableListLib
{
    partial class MListItem
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.mainPanel = new System.Windows.Forms.Panel();
            this.lblContent = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.BackgroundImage = Resources.manu_a;
            this.mainPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.mainPanel.Controls.Add(this.lblContent);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(199, 46);
            this.mainPanel.TabIndex = 1;
            this.mainPanel.Click += new System.EventHandler(this.mainPanel_Click);
            this.mainPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MListItem_MouseDown);
            this.mainPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MListItem_MouseMove);
            this.mainPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MListItem_MouseUp);
            // 
            // lblContent
            // 
            this.lblContent.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblContent.AutoSize = true;
            this.lblContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblContent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblContent.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblContent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(219)))), ((int)(((byte)(222)))));
            this.lblContent.Location = new System.Drawing.Point(39, 13);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(93, 20);
            this.lblContent.TabIndex = 0;
            this.lblContent.Text = "描述信息";
            this.lblContent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblContent.Click += new System.EventHandler(this.lblContent_Click);
            this.lblContent.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MListItem_MouseDown);
            this.lblContent.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MListItem_MouseMove);
            this.lblContent.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MListItem_MouseUp);
            // 
            // MListItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainPanel);
            this.Name = "MListItem";
            this.Size = new System.Drawing.Size(202, 46);
            this.Click += new System.EventHandler(this.mlistItemClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MListItem_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MListItem_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MListItem_MouseUp);
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Label lblContent;
    }
}
