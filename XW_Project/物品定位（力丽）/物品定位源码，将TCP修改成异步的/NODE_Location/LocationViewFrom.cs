using CiXinLocation.bean;
using CiXinLocation.Model;
using MoveableListLib;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CiXinLocation
{
    public partial class LocationViewFrom : Form
    {

        private List<CanKaoDianBean> ckdBeans;       
        private QuYuBean quYuBean;
        private string cengjiID;
        public LocationViewModel locaModel;
        private int dataType = 1;//数据显示的模式，< 0 是图像；=1或3 是列表 
        private int pageCount = 30;//列表一页显示的数据量。        
        private FormMain mFormMian;
        DoubleBufferListView listView;
        DoubleBufferListView listView2;
        private bool selectBool = false;       

        public LocationViewFrom()
        {            
            this.columnHeader22 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader22.Text = "功率";

            if(listView == null) listView = new DoubleBufferListView();
            if (listView2 == null) listView2 = new DoubleBufferListView();
            InitializeComponent();           
            ckdBeans = new List<CanKaoDianBean>();

            locaModel = new LocationViewModel(this,panel1, listView, listView2,label8);
            locaModel.moveHandle += moveTagView;
            locaModel.adReView += addOrRemoveTagView;           
        }

        public FormMain MFormMian
        {
            get { return mFormMian; }
            set { mFormMian = value;}
        }

        public string CengjiID
        {
            get { return cengjiID; }
            set { cengjiID = value;
            locaModel.CengjiID = value; }
        }

        public bool SelectBool
        {
            get { return selectBool; }
            set { selectBool = value; }
        }

        public List<CanKaoDianBean> CkdBeans 
        {
            get { return ckdBeans; }
            set { ckdBeans = value.ToList(); }
        }

        public QuYuBean QUYUBean
        {
            get { return quYuBean; }
            set { quYuBean = value;
            locaModel.QUYUBean = value; }
        }

        public int PageCount
        {
            get { return pageCount; }
            set { pageCount = value; }
        }

        private void LocationViewFrom_Load(object sender, EventArgs e)
        {           
            showData(); 
        }


        private void loadNODEinfo() //初始化节点信息到列表
        {
            List<CanKaoDianBean> canKaoDians = FileModel.getFlModel().ChFlBean.CanKaoDians.ToList();
            if (canKaoDians == null || canKaoDians.Count == 0) return;
            locaModel.setListViewCanInfos(canKaoDians);
            locaModel.setPortInfo();
        }

        public void loadDataInAllViewFrom() 
        {
            if (mFormMian == null) return;
            mFormMian.loadOnNOdata();
        }

       private System.Windows.Forms.ColumnHeader columnHeader22;

       private void DoubleBufferListViewLoadData() 
       {
           this.tabPage1.Controls.Remove(listView1);
           this.tabPage1.Controls.Add(this.listView);
 
           this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top 
           | System.Windows.Forms.AnchorStyles.Bottom)
           | System.Windows.Forms.AnchorStyles.Left)
           | System.Windows.Forms.AnchorStyles.Right)));
           
           this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14,
            this.columnHeader15,
            this.columnHeader16,
            this.columnHeader17,
            this.columnHeader18,
            this.columnHeader19,
            this.columnHeader20,
            this.columnHeader22,
            this.columnHeader21});
           this.listView.FullRowSelect = true;
           this.listView.GridLines = true;
           this.listView.Location = new System.Drawing.Point(8, 6);
           this.listView.Name = "DoubleBufferListView1";
           //this.listView.Size = new System.Drawing.Size(783, 488);
           this.listView.Width = 783;
           this.listView.Height = 488;
           this.listView.SmallImageList = this.imageList1;
           this.listView.TabIndex = 0;
           this.listView.UseCompatibleStateImageBehavior = false;
           this.listView.View = System.Windows.Forms.View.Details;
           this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
           this.listView.HScroll += new EventHandler(OnHScroll);
           this.listView.VScroll += new EventHandler(OnVScroll);
       }

       virtual protected void OnHScroll(object sender, EventArgs e)
       {
       }

       public  void OnVScroll(object sender, EventArgs e)
       {
           if (this.listView.Items.Count < 1) return;
           if (locaModel != null) locaModel.setListTopIndex();
       }

       private void loadList() 
       {
           this.tabPage3.Controls.Remove(listView_re);
           this.tabPage3.Controls.Add(this.listView2);

           this.listView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top
           | System.Windows.Forms.AnchorStyles.Bottom)
           | System.Windows.Forms.AnchorStyles.Left)
           | System.Windows.Forms.AnchorStyles.Right)));

           this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader23,
            this.columnHeader24,
            this.columnHeader25,
            this.columnHeader26,
            this.columnHeader27,
            this.columnHeader28});
           this.listView2.FullRowSelect = true;
           this.listView2.GridLines = true;
           this.listView2.Location = new System.Drawing.Point(3, 38);
           this.listView2.Name = "listView2";
           this.listView2.Size = new System.Drawing.Size(788, 492);
           this.listView2.SmallImageList = this.imageList1;
           this.listView2.TabIndex = 0;
           this.listView2.UseCompatibleStateImageBehavior = false;
           this.listView2.View = System.Windows.Forms.View.Details;
       }

       public void clearList() 
       {
           locaModel.loadNewData();
       }

        public void addAllData() 
        {          
            if (tabControl1.SelectedTab == tabPage1)//进行tabpage位置判断  
            {
                if (selectBool) dataType = 3;                    
                else dataType = 1;
            }
            else if (tabControl1.SelectedTab == tabPage2)//进行tabpage位置判断  
            {
                dataType = -100;
            }
        }

        public void setCardID(string serCardID)
        {
            string cardID = serchIDFrom(serCardID);
            locaModel.SerchCardID = cardID == null ? serCardID.ToUpper() : cardID.ToUpper();
        }

        private string serchIDFrom(string cardName) 
        {
            List<CardBean> cardBeans = FileModel.getFlModel().ChFlBean.Cards.ToList();
            var cards = cardBeans.Where(a => a.Name.Equals(cardName));
            if (cards.Count() > 0) return cards.First().Id; ///检测到 serCardID 是名字，改为它的ID再操作吧
            else return null;
        }

        private void loadImage() 
        {
            if (quYuBean == null) return;
            if (quYuBean.MapPath != null && ":".Equals(quYuBean.MapPath.Substring(1, 1)))
            {                
                try
                {
                    Bitmap bitmap = new Bitmap(quYuBean.MapPath, true);
                    bitmap = new Bitmap(bitmap, panel1.Width, panel1.Height);
                    panel1.BackgroundImage = bitmap;
                    panel1.BackgroundImageLayout = ImageLayout.Stretch;
                }
                catch { }
            }
        }

        //供外部初始化
        public void setDataLoad() 
        {
            DoubleBufferListViewLoadData();
            loadList();

            loadData();
            timer1.Interval = 1000;
            timer1.Start();            
        }

        //初始化
        private void loadData() 
        {
            if (cengjiID == null || quYuBean == null || ckdBeans == null) return;
            this.Text = quYuBean.QuyuName;
            loadImage();            
            if (ckdBeans.Count < 1) return;
            locaModel.Ckds.Clear();
            foreach (CanKaoDianBean ckdBean in ckdBeans)
            {
                if (!quYuBean.QuyuID.Equals(ckdBean.QuYuID) || !cengjiID.Equals(ckdBean.CenJiID)) continue;
                CanKaoDianView cView = new CanKaoDianView();
                cView.BackColor = Color.Transparent;
                Point p = new Point(ckdBean.POint.X * panel1.Width / getcolWeiHei(ckdBean.ColWeiHei[0]),
                    ckdBean.POint.Y * panel1.Height / getcolWeiHei(ckdBean.ColWeiHei[1]));
                cView.Click += button1_Click;
                cView.Location = p;
                cView.LabText = ckdBean.Name;
                cView.Tag = ckdBean.Id;
                cView.CkdIDStr = ckdBean.Id;
                locaModel.cardCountHandle += cView.setCardCount;
                panel1.Controls.Add(cView);
                CanKaoDianBean ckdnewBean = new CanKaoDianBean(ckdBean);
                locaModel.Ckds.Add(ckdnewBean);
            }
        }

        
        public void addLoadData() 
        {
            locaModel.loadNewData();
            panel1_Resize(null,null);
        }

        private int getcolWeiHei(int wHei) 
        {
            return wHei == 0 ? 1 : wHei;
        }

        //参考点的点击事件
        private void button1_Click(object sender, EventArgs e) //添加
        {
            if (!(sender is CanKaoDianView)) return;
            CanKaoDianView cView = (CanKaoDianView)sender;
            PortFrom portFrom = new PortFrom((string)cView.Tag);
            portFrom.NodeName = cView.LabText;
            cardHand += portFrom.cardList;
            portFrom.cardHand += setTag_LEDShow;
            portFrom.ShowDialog();
        }

        public delegate void CardHandle(Dictionary<string, List<CardUpDataBean>> NodeCardUpBean);
        CardHandle cardHand;

        private void panel1_Paint(object sender, PaintEventArgs e) { }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (locaModel != null) locaModel.draw(dataType);
            if (cardHand != null) cardHand(locaModel.NodeCardUpBean);
        }

        public void showDataType(int dataType)
        {
            this.dataType = dataType;
            showData();
        }

        public void serchViewVisi(bool visi) //隱藏掉搜索的相關View
        {
            label6.Visible = visi;
            textBox2.Visible = visi;
            button2.Visible = visi;
            Point point = new Point(0,1);
            tabControl1.Location = point;
        //  tabControl1.Height += 33;
        }

        public void showData()
        {
            timer1_Tick(null,null);
            loadDataInAllViewFrom();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            addAllData();
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            loadData();
        }

        public void moveTagView(Control col,Point pot) 
        {
            try {
                this.Invoke((EventHandler)(delegate
                { //放入主線程    
                    col.Location = pot;
                })); 
            }
            catch { }            
        }

        /// <summary>
        /// view显示，或者删除
        /// </summary>
        /// <param name="col"></param>
        /// <param name="viewType">1 = 添加，2 = 删除</param>
        public void addOrRemoveTagView(Control col,int viewType) 
        {
            this.Invoke((EventHandler)(delegate{ //放入主線程    
                try  {
                    if (viewType == 1)
                    {
                        panel1.Controls.Add(col);
                        if (mFormMian != null) col.Click += mFormMian.panpalTag_Click;
                    }
                    else if (viewType == 2) 
                    {
                        panel1.Controls.Remove(col);
                        if (mFormMian != null) col.Click -= mFormMian.panpalTag_Click;
                    } 
                }
                catch { }                
            })); 
        } 

        private void listView1_SelectedIndexChanged(object sender, EventArgs e){ }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count < 1) return;
            new Thread(liViewSelect).Start(); //本来是不开贤臣，是在是这个问题太奇葩了。找不到原因，就开一个线程
        }

        private void liViewSelect() //如果不开线程，点击一次，等弹出的界面消失后，立马又跳出一个界面，就是会跑两次，不知道是那个地方膈应到这个事件了
        {  
            try
            {
                this.Invoke((EventHandler)(delegate
                { //放入主線程    
                    if (listView.SelectedItems.Count < 1) return;
                    ListViewItem lvItem = listView.SelectedItems[0];
                    setTag_LEDShow(lvItem);
                }));
            }
            catch { }            
        }

        private void setTag_LEDShow(ListViewItem lvItem) 
        {
            if (lvItem == null) return;
            TagLocationPointBean tagPointBeab = locaModel.getTagLocationPointBean(lvItem.SubItems[1].Text);
            if (tagPointBeab != null && mFormMian != null) mFormMian.Tag_LEDShow(tagPointBeab);
        }

        private void label1_Click(object sender, EventArgs e) //首页
        {
            setListPage(ListPageType.FIRST);
        }

        private void label3_Click(object sender, EventArgs e)//上一页
        {
            setListPage(ListPageType.UP);
        }

        private void label4_Click(object sender, EventArgs e)//下一页
        {
            setListPage(ListPageType.DOWN);
        }

        private void label2_Click(object sender, EventArgs e)//尾页   
        {
            setListPage(ListPageType.LAST);
        }

        private void setListPage(ListPageType listType) 
        {
            locaModel.changePageInList(listType, pageCount);
            textBox1.Text = locaModel.getPage().ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)//go
        {
            int pages = XWUtils.stringToInt1(textBox1.Text);
            locaModel.setPage(pages);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!PeoplePowerModel.getPeoplePowerModel().isSerchTag())
            {
                MessageBox.Show("您無此權限");
                return;
            }
            string name = textBox2.Text;
            string cardName = serchIDFrom(name);
            string cardID = cardName == null? name.ToUpper():cardName;
            if (locaModel.setSerchCARDID(cardID))
            {
                panel1_Resize(null, null);
                if (dataType < 0 && locaModel != null)
                    locaModel.draw(1);
                MessageBox.Show("搜索成功");                
            }
            else 
            {
                MessageBox.Show("搜索失敗");
            }
        }

        public void closeFoem() 
        {
            if (timer1.Enabled) timer1.Enabled = false;
            cengjiID = null;
            if (mFormMian != null) mFormMian = null;
            if (quYuBean != null) quYuBean = null;
            if (locaModel != null)
            {
                locaModel.close();
                locaModel = null;
            } 
        }

        public void closePageCtrol()
        {
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            textBox1.Visible = false;
            button1.Visible = false;
        }

        public void setHisData(List<CardUpDataBean> showHisCardDatas, CardUpType type)
        {
            switch (type)
            {
                case CardUpType.CURRENT_DATA:
                    break;
                case CardUpType.HISTORY_DATA:
                    showHisCardData(showHisCardDatas);
                    break;
                default:
                    break;
            }
        }

        private void showHisCardData(List<CardUpDataBean> showHisCardDatas) 
        {
            this.Invoke((EventHandler)(delegate 
            {
                locaModel.drawHisList(showHisCardDatas);
            }));
        }

    }
}
