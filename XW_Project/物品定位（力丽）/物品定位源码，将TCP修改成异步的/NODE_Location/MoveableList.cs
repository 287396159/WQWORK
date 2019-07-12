using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using CiXinLocation.bean;
 
namespace MoveableListLib
{
    public partial class MoveableList : UserControl
    {
        //public 
        //存储菜单项
        public ArrayList arrList = new ArrayList();
        public MoveableListLib.MListItem.MItemClick onMListItemClick;
        public delegate void MoveBtn(int type);
        MoveBtn mBtn;

        //最大数目
        private int max = 0;
        //单元数目
        private int unit = 0;

        public MoveableList()
        {
            InitializeComponent();       
            //初始化
            initListContent(); 
        }

        public MoveableList(int pMax,int pUnit):this()
        {
            max = pMax;
            unit = pUnit;
        }

        private void iss(int index)
        {
            if (onMListItemClick != null)
            {
                onMListItemClick(index);
            }
        }

        public void btnMoveClick(int type) 
        {
            if (mBtn != null)
                mBtn(type);
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

        /// <summary>
        /// 初始化内容
        /// </summary>
        public  void initListContent()
        {
            for (int i = 0; i < unit; i++)
            {
                MListItem item = new MListItem(i);
                item.Location = new Point(0,item.Height * i);
                item.onMItemClick += onItemClick;
                item.onDeleteMItemClick += deleteData;
                mBtn += item.buttonMove;
                this.Controls.Add(item);
            }
        }

        /// <summary>
        /// 加载新数据
        /// </summary>
        /// <param name="lastPosition">最后一条记录的位置</param>
        public void loadNewData(List<CenJiBean> cenJiFileData)
        {
            List<CenJiBean> cenJiData = cenJiFileData.ToList();
            removeItem();

            int count = cenJiData.Count;
            max = count;
            for (int i = 0; i < count;i++ )
            {
                MListItem item = new MListItem(i,cenJiData[i].CenJiName);
                item.onMItemClick -= onItemClick;
                item.Location = new Point(0, item.Height * i);
                item.onMItemClick += onItemClick;
                item.onDeleteMItemClick += deleteData;
                mBtn += item.buttonMove;
                this.Controls.Add(item);              
            }
        }

        private void removeItem() 
        {
            //this.Controls.Clear();

            if (this.Controls.Count > 0)
            {
                for (int i = 0; i < this.Controls.Count; i++)
                {
                    if (!(this.Controls[i] is MListItem)) continue;
                    MListItem item = this.Controls[i] as MListItem;
                    item.onMItemClick -= onItemClick;
                    item.onDeleteMItemClick -= deleteData;
                    mBtn -= item.buttonMove;

                    //Control C = this.Controls[i];
                    this.Controls.Remove(item);
                    item.Dispose();
                }
            }
        }

        /// <summary>
        /// 加载新数据
        /// </summary>
        /// <param name="lastPosition">最后一条记录的位置</param>
        public void loadNewData(int lastPosition)
        {
            this.Controls.Clear();
            removeItem();
            int currentSize =this.Controls.Count;
            for (int i = 0; i < max; i++)
            {
                MListItem item = new MListItem(currentSize+i);
                item.onMItemClick -= onItemClick;
                item.Location = new Point(0, item.Height *i + lastPosition);
                item.onMItemClick += onItemClick;
                item.onDeleteMItemClick += deleteData;
                this.Controls.Add(item);
                mBtn += item.buttonMove;
            }
        }

        public void deleteData(MListItem item)
        {
            if (item == null) return;

            this.Controls.Remove(item);
            max--;
            loadNewData(0);
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

        public void onItemClick(int index) 
        {
            iss(index);
            foreach (Control conItem in this.Controls)
            {
                if (!(conItem is MListItem)) return;
                MListItem item = (MListItem)conItem;
                item.backGroundChange(index);
            }
        }

        private void MoveableList_Load(object sender, EventArgs e)
        {

        }

    }
}
