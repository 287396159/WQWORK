using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;
using CiXinLocation.bean;

namespace MoveableListLib
{
    public partial class MListItem2 : UserControl
    {

        private Point startLocation ;
        private Point endLocation;
        private Color begin_color = Color.FromArgb(104, 75, 59);
        private Color end_color = Color.FromArgb(192, 137, 107);
        private int itemIndex;
        private QuYuBean quYuBean;

        public delegate void MPamelItemClick(int index, QuYuBean quYuBean);
        public MPamelItemClick onMItemClick;


        public MListItem2()
        {
            InitializeComponent();
        }

        public MListItem2(string index)
        {
            InitializeComponent();
            begin_color = Color.FromArgb(104, 75, 59);
            end_color = Color.FromArgb(192, 137, 107);
        }

        public MListItem2(QuYuBean quYuBean)
        {
            InitializeComponent();
            label1.Text = getFiveByteText(quYuBean.QuyuName);
            begin_color = quYuBean.Begin_color;
            end_color = quYuBean.End_color;
            label3.Text = getPeopleCount(quYuBean.PepleCount);//.ToString();
            this.quYuBean = quYuBean;
            labTextStr = label3.Text;
        }

        private string getPeopleCount(int peopleCount) 
        {
            if (peopleCount< 1000 && label3.Font.Size < 32) this.label3.Font = new System.Drawing.Font("宋体", 40F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            else if (peopleCount > 999 && label3.Font.Size > 30) this.label3.Font = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                return peopleCount.ToString();           
        }

        private string getFiveByteText(string name) 
        {
            string getName = name;
            if (name.Length > 3) getName = name.Substring(0,3)+"..";
            return getName;
        }

        public void setLabColor() 
        {
            label1.BackColor = begin_color;
            label3.BackColor = end_color;
            label2.BackColor = end_color;
        }

        public void setPeopleCount(int count)
        {
            //if (getPeopleCount(count).Equals(label3.Text)) return;
            if (count.ToString("").Equals(labTextStr)) return;
            this.Invoke((EventHandler)(delegate
            { //放入主線程    
                label3.Text = count.ToString();
                getPeopleCount(count);
                labTextStr = label3.Text;
            }));            
        }

        private string labTextStr = "";

        public QuYuBean QuYuBean
        {
            get { return quYuBean; }
            set { quYuBean = value; }
        }

        public Color Begin_color
        {
            get { return begin_color; }
            set 
            {
                if (begin_color.R == 0 && begin_color.A == 0 && begin_color.B == 0 && begin_color.G == 0)
                    begin_color = value;
              }
        }

        public int ItemIndex
        {
            get { return itemIndex; }
            set { itemIndex = value; }
        }

        public Color End_color
        {
            get { return end_color; }
            set
            {
                if (end_color.R == 0 && end_color.A == 0 && end_color.B == 0 && end_color.G == 0)
                    end_color = value;
            }
        }

        /// <summary>
        /// 画四个边角的圆角
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="g"></param>
        /// <param name="_radius"></param>
        /// <param name="cusp">是否画尖角</param>
        /// <param name="begin_color"></param>
        /// <param name="end_color"></param>
        private void Draw(Rectangle rectangle, Graphics g, int _radius, bool cusp)
        {
            if (rectangle.Width == 0 || rectangle.Height == 0 ) return;
            int span = 2;
            //抗锯齿
            g.SmoothingMode = SmoothingMode.AntiAlias;
            //渐变填充
            LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush(rectangle, begin_color, begin_color, LinearGradientMode.Vertical);
            //画尖角
            if (cusp)
            {
                span = 10;
                PointF p1 = new PointF(rectangle.Width - 12, rectangle.Y + 10);
                PointF p2 = new PointF(rectangle.Width - 12, rectangle.Y + 30);
                PointF p3 = new PointF(rectangle.Width, rectangle.Y + 20);
                PointF[] ptsArray = { p1, p2, p3 };
                g.FillPolygon(myLinearGradientBrush, ptsArray);
            }
            //填充
            g.FillPath(myLinearGradientBrush, DrawRoundRect(rectangle.X, rectangle.Y, rectangle.Width - span, rectangle.Height - 6, _radius));
            myLinearGradientBrush = new LinearGradientBrush(rectangle, end_color, end_color, LinearGradientMode.Vertical);
            //填充
            g.FillPath(myLinearGradientBrush, DrawRoundRect(rectangle.X, rectangle.Y + 45, rectangle.Width - span, rectangle.Height - 1, _radius,false, false ,true, true));
        }


        public static GraphicsPath DrawRoundRect(int x, int y, int width, int height, int radius)
        {
            //四边圆角
            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(x, y, radius, radius, 180, 90);   //左上角
            gp.AddArc(width - radius, y, radius, radius, 270, 90);//右上角
            gp.AddArc(width - radius, height - radius, radius, radius, 0, 90); //右下角
            gp.AddArc(x, height - radius, radius, radius, 90, 90); //左下角
            gp.CloseAllFigures();
            return gp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="radius"></param>
        /// <param name="UL">up-left</param>
        /// <param name="UR">up-right</param>
        /// <param name="BL">bottom-left</param>
        /// <param name="BR">bottom-right</param>
        /// <returns></returns>
        public static GraphicsPath DrawRoundRect(int x, int y, int width, int height, int radius, bool UL, bool UR, bool BL, bool BR)
        {
            //四边圆角
            int radius_r = radius;
            GraphicsPath gp = new GraphicsPath();
            if (!UL) radius = 1;
            gp.AddArc(x, y, radius, radius, 180, 90);   //左上角
            if (!UR) radius = 1;
            else radius = radius_r;
            gp.AddArc(width - radius, y, radius, radius, 270, 90);//右上角
            if (!BR) radius = 1;
            else radius = radius_r;
            gp.AddArc(width - radius, height - radius, radius, radius, 0, 90); //右下角
            if (!BL) radius = 1;
            else radius = radius_r;
            gp.AddArc(x, height - radius, radius, radius, 90, 90); //左下角
            gp.CloseAllFigures();
            return gp;
        }

        private void wai_si_jiao_paint(object sender, PaintEventArgs e) { }

        private void MListItem_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;           
            int size = this.Parent.Controls.Count;
            startLocation = e.Location;            
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

            MoveThreeAbleList parentCtrl = (MoveThreeAbleList)this.Parent;
            int size = this.Parent.Controls.Count;
            Control lastCtrl = parentCtrl.Controls[size - 1];
            if (!parentCtrl.existOtherData())
            {
                int sumHeight = getSize(size) * lastCtrl.Height;
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
                //parentCtrl.loadNewData(lastCtrl.Location.Y + lastCtrl.Height);
            }
        }


        private int getSize(int size) 
        {
            int tSize = size / 3;
            int pSize = size % 3 == 0?0:1;
            return tSize + pSize;
        }

        /// <summary>
        /// 重新计算位置
        /// </summary>
        /// <param name="xx">变化值</param>
        private void reloadCtrlPosition(int xx)
        {
            if (this.Parent == null || this.Parent.Controls.Count == 0) return;
            foreach (Control ctrl in this.Parent.Controls)
            {
                ctrl.Location = new Point(ctrl.Location.X, ctrl.Location.Y + xx);
            }                      
        }

        private void MListItem2_Load(object sender, EventArgs e) 
        {
            //label1.ForeColor = Color.White;                       
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics pg = panel1.CreateGraphics();
            Draw(panel1.ClientRectangle, pg, 18, false);
        }

        private void panelClick(object sender, EventArgs e)
        {
            if (onMItemClick != null) onMItemClick(itemIndex,quYuBean);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (onMItemClick != null) onMItemClick(itemIndex, quYuBean);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            if (onMItemClick != null) onMItemClick(itemIndex, quYuBean);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (onMItemClick != null) onMItemClick(itemIndex, quYuBean);
        }


    }
}
