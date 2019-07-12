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
    public partial class MoveThreeAbleList : UserControl
    {

        public delegate void MoveBtn(int type);
        MoveBtn mBtn;
        List<Color[]> colors;
        //最大数目
        private int max = 0;
        //单元数目
        private int unit = 0;


        public delegate void MPamelItemClickList(int index, QuYuBean quYuBean);
        public MPamelItemClickList onMItemClickBack;
        public MoveThreeAbleList(){ }


        public MoveThreeAbleList(List<Color[]> colors)
        {

            InitializeComponent();
            //初始化
            initListContent();
            this.colors = colors;
        }

        public MoveThreeAbleList(int pMax, int pUnit, List<Color[]> colors) 
        {
            
            max = pMax;
            unit = pUnit;
            this.colors = colors;
        }


        /// <summary>
        /// 设置大小
        /// </summary>
        /// <param name="pMax">最大记录数</param>
        /// <param name="pUnit">加载单位记录数</param>
        public void setItemsSize(int pMax, int pUnit)
        {
            max = pMax;
            unit = pUnit;
        }

        public void btnMoveClick(int type)
        {
            if (mBtn != null)
                mBtn(type);
        }

        public List<Color[]> Colors 
        {
            get { return colors; }
            set { colors = value;}
        }

        /// <summary>
        /// 初始化内容
        /// </summary>
        public void initListContent()
        {
            for (int i = 0; i < unit; i++)
            {
                MListItem2 item = new MListItem2(i.ToString());
                item.ItemIndex = i;
                item.Location = new Point(i % 3 * item.Width+10 , item.Height * (i / 3));
                //this.Controls.Add(item);
                mBtn += item.buttonMove;
            }         
        }

        /// <summary>
        /// 加载新数据
        /// </summary>
        /// <param name="lastPosition">最后一条记录的位置</param>
        public void loadNewData(List<QuYuBean> fileQuYuBeans)
        {
            List<QuYuBean> QuYuBeans = fileQuYuBeans.ToList();
            if (QuYuBeans == null) return;
            int count = QuYuBeans.Count;
            if (count < 0) return; 
            this.Controls.Clear(); 
            //removeItem();
            max = count;

            for (int i = 0; i < count; i++)
            {
                MListItem2 item = new MListItem2(QuYuBeans[i]);
                item.Begin_color = colors[i % 9][0];
                item.End_color = colors[i % 9][1];
                item.onMItemClick += onMItemClick;
                item.ItemIndex = i;
                item.setLabColor();

                item.Location = new Point(i % 3 * item.Width + 10, item.Height * (i / 3));
                try {
                    this.Controls.Add(item);
                }
                catch {
                    continue;
                }               
                mBtn += item.buttonMove;
            }
        }


        private void removeItem()
        {
            if (this.Controls.Count > 0)
            {
                for (int i = 0; i < this.Controls.Count; i++)
                {
                    if (!(this.Controls[i] is MListItem2)) continue;
                    MListItem2 item = this.Controls[i] as MListItem2;
                    item.onMItemClick -= onMItemClick;
                    mBtn -= item.buttonMove;
                    this.Controls.Remove(item);
                    item.Dispose();
                }
            }
        }


        public void onMItemClick(int index, QuYuBean quYuBean)
        {
            if (onMItemClickBack != null) onMItemClickBack(index, quYuBean);
        }

        /// <summary>
        /// 是否还存在其他数据
        /// </summary>
        /// <returns></returns>
        public bool existOtherData()
        {
            if (this.Controls.Count < max)
                return true;
            return false;
        }

        private void MoveThreeAbleList_Load(object sender, EventArgs e)
        {

        }

        //int count = 0;
        public void changePeopleCount(List<QuYuBean> QuYuBeans)
        {
            if (QuYuBeans == null || QuYuBeans.Count == 0) return;
            foreach(Control col in this.Controls)
            {
                if (!(col is MListItem2)) continue;
                MListItem2 item2 = (MListItem2)col;
                foreach (QuYuBean qy in QuYuBeans)
                {
                    if (item2.QuYuBean.QuyuID.Equals(qy.QuyuID))
                        item2.setPeopleCount(qy.PepleCount);//count);
                }
            }
            //count++;
            //if (count > 1200) count = 1;
        }

    }
}
