using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CiXinLocation.bean;

namespace MoveableListLib
{
    public partial class CanKaoDianView : UserControl
    {
        private Point startLocation;
        private Point endLocation;
        private Color baColor;
        private string labText;
        private byte[] ckdID;
        private string ckdIDStr;
        
        public CanKaoDianView()
        {
            InitializeComponent();
        }

        public CanKaoDianView(Color backColor,string lab)
        {
            InitializeComponent();
            baColor = backColor;
            label2.BackColor = backColor;
            label1.Text = lab;
            labText = lab;
            //ControlMoveResize(button1, this);
        }

        public void changeBtnBackColor()
        {
            if (baColor != null) changeBtnBackColor(baColor);
        }

        public void changeBtnBackColor(Color backColor) 
        {
            if (backColor == null) return;
            label2.BackColor = backColor;
        }

        public void setCardCount(Dictionary<string, List<CardUpDataBean>> NodeCardUpBeans) //设置卡片的数量到背景
        {
            if(ckdIDStr == null ) return;
            Dictionary<string, List<CardUpDataBean>> NodeCardUpBean = new Dictionary<string, List<CardUpDataBean>>(NodeCardUpBeans);
            if (NodeCardUpBean.ContainsKey(ckdIDStr)) 
            {               
                int count = NodeCardUpBean[ckdIDStr].Count;
                if (count > 3)
                {                                        
                    label2.Text = (count - 3).ToString();
                }                    
                else if (label2.Text.Length > 0) 
                {
                    label2.Text = "";
                }
            }
            else if (label2.Text.Length > 0) 
            {
                label2.Text = "";
            }         
        }

        public void mainBtnBackColor(Color backColor)
        {
            this.Invoke((EventHandler)(delegate
            { //放入主線程    
                if (backColor == null) return;
                label2.BackColor = backColor;
            }));             
        }

        public Color btnBackColor()
        {
            return label2.BackColor;
        }

        public Point centerPoint()
        {
            Point m_point = new Point();
            m_point.X = label2.Width / 2;
            m_point.Y = (label2.Height / 2) + label1.Height;
            return m_point;
        }

        public string CkdIDStr
        {
            get { return ckdIDStr; }
            set { ckdIDStr = value; }
        }


        public byte[] CkdID
        {
            get { return ckdID; }
            set { ckdID = value; }
        }


        public string LabText
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }


        private void MListItem_MouseDown(object sender, MouseEventArgs e)
        {

        }


        private void MListItem_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void MListItem_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;           
        }

        private void itemMove()
        {
            int xx = endLocation.Y - startLocation.Y;
            int yy = endLocation.X - startLocation.X;
            if (Math.Abs(xx) > 10 || Math.Abs(yy) > 10)
                reloadCtrlPosition(endLocation);
            else return;
            startLocation = endLocation;
        }

        

        /// <summary>
        /// 重新计算位置
        /// </summary>
        /// <param name="xx">变化值</param>
        private void reloadCtrlPosition(Point pt)
        {
            if (pt == null) return;
             this.Location = pt;           
        }

        /// <summary>
        /// 双击控件发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CKDView_mouDouble(object sender, MouseEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            base.OnClick(e);
        }

        private void CanKaoDianView_Load(object sender, EventArgs e)
        {
        }

        private void label2_DoubleClick(object sender, MouseEventArgs e)
        {
            base.OnDoubleClick(e);
        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        private void label2_MouseUp(object sender, MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        public void ViewEnable() 
        {
         //   label1.Enabled = false;
            label2.Enabled = false;
        }
    }
}
