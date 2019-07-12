using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CiXinLocation.Properties;

namespace MoveableListLib
{
    public partial class MListItem : UserControl
    {
        private Point startLocation;
        private Point endLocation;
        private int item_index;

        public delegate void MItemClick(int index);
        public delegate void MDeleteItemClick(MListItem item);
        public MItemClick onMItemClick;
        public MDeleteItemClick onDeleteMItemClick;


        private void iss(int index) 
        {
            if (onMItemClick != null) 
            {
                onMItemClick(index);
            }
        }

        public MListItem(int index)
        {
            InitializeComponent();
            item_index = index;
            this.lblContent.Text += index;
        }

        public MListItem(int index,string name)
        {
            InitializeComponent();
            item_index = index;
            this.lblContent.Text = name;
        }


        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this.lblContent.Text +"的详细信息！");
            if (onDeleteMItemClick != null) onDeleteMItemClick(this);
        }

        private void MListItem_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int size = this.Parent.Controls.Count;
                startLocation = e.Location;
            }
        }

         
        private void MListItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || startLocation == null) return;

            endLocation = e.Location;
            itemMove();
        }


        private void MListItem_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            itemMoveOver();                          
        }


        public void buttonMove(int type)
        {
            if (type == 1)
            {
                startLocation = new Point(0, 0);
                endLocation = new Point(0, 1);
            }
            else if (type == 2)
            {
                startLocation = new Point(0, 2);
                endLocation = new Point(0, 1);
            }

            itemMove();
            itemMoveOver();
        }


        private void itemMove()
        {
            int xx = endLocation.Y - startLocation.Y;
            if (Math.Abs(xx) > 0.1)
                reloadCtrlPosition(xx);
        }


        /// <summary>
        /// 移动计算
        /// </summary>
        private void itemMoveOver()
        {
            if (startLocation == null || this.Parent == null) return;

            //考虑第一条记录
            Control firstCtrl = this.Parent.Controls[0];
            int firstPosition = firstCtrl.Location.Y;
            if (firstPosition > 0)
            {
                reloadCtrlPosition(-firstCtrl.Location.Y);
                return;
            }

            MoveableList parentCtrl = (MoveableList)this.Parent;
            int size = this.Parent.Controls.Count;
            Control lastCtrl = parentCtrl.Controls[size - 1];
            if (!parentCtrl.existOtherData())
            {
                int sumHeight = size * lastCtrl.Height;
                int maxHeight = parentCtrl.Height;
                if (sumHeight < parentCtrl.Height)
                {
                    maxHeight = sumHeight;
                }
                int xx = maxHeight - lastCtrl.Height - lastCtrl.Location.Y;
                if (xx > 0)
                    reloadCtrlPosition(xx);
            }
            else
            {
                parentCtrl.loadNewData(lastCtrl.Location.Y + lastCtrl.Height);
            }
        }


        /// <summary>
        /// 重新计算位置
        /// </summary>
        /// <param name="xx">变化值</param>
        private void reloadCtrlPosition(int xx)
        {
            if (this.Parent == null || this.Parent.Controls.Count < 1) return;
            foreach (Control ctrl in this.Parent.Controls)
            {
                ctrl.Location = new Point(0, ctrl.Location.Y + xx);
            }
        }
       

        private void mainPanel_Click(object sender, EventArgs e)
        {
            lblContent_Click(sender, e);
        }


        private void mlistItemClick(object sender, EventArgs e)
        {         
            iss(item_index);
        }

        private void lblContent_Click(object sender, EventArgs e)
        {
            if (mainPanel.Tag == null || !(bool)(mainPanel.Tag)) 
            {
                mainPanel.BackgroundImage = Resources.manu_b;
                //lblContent.BackColor = Color.FromArgb(0,0,0,0);
            }                
            mainPanel.Tag = true;
            iss(item_index);
        }


        public void backGroundChange(int index)
        {
            if (index == item_index) 
            {
                if (mainPanel.BackgroundImage != Resources.manu_b) mainPanel.BackgroundImage = Resources.manu_b;
                return;
            }             
            if (mainPanel.BackgroundImage != Resources.manu_a) mainPanel.BackgroundImage = Resources.manu_a;
            if (mainPanel.Tag == null || !(bool)(mainPanel.Tag)) return;
            mainPanel.Tag = false;
        }


    }
}
