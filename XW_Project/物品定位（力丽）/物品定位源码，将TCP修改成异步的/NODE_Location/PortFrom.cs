using CiXinLocation.bean;
using CiXinLocation.Model;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CiXinLocation
{
    public partial class PortFrom : Form
    {
        public delegate void LISTVIEWClickHandle(ListViewItem listviewItem);
        public LISTVIEWClickHandle cardHand;
        private string id; //节点ID
        private string name;
        DoubleBufferListView listView;
        private System.Windows.Forms.ColumnHeader columnHeader22;
        private Dictionary<string, CardUpDataBean> cardDictionarys;

        public PortFrom(string id)
        {
            this.columnHeader22 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader22.Text = "功率";

            if (listView == null) listView = new DoubleBufferListView();
            InitializeComponent();
            this.id = id;
            cardDictionarys = new Dictionary<string, CardUpDataBean>();
        }

        public string NodeName
        {
            get { return name; }
            set { name = value; }
        }

        private void PortFrom_Load(object sender, EventArgs e)
        {
            if (id != null) this.Text = "節點："+NodeName;
            DoubleBufferListViewLoadData();
        }

        private void DoubleBufferListViewLoadData()
        {
            this.Controls.Remove(listView1);
            this.Controls.Add(this.listView);

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
            this.listView.Location = new System.Drawing.Point(1, 1);
            this.listView.Name = "DoubleBufferListView1";
            this.listView.Size = new System.Drawing.Size(823, 573);
            this.listView.SmallImageList = this.imageList1;
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
        }

        public void cardList(Dictionary<string, List<CardUpDataBean>> NodeCardUpBean)
        {
            if(!NodeCardUpBean.ContainsKey(id)) return;
            List<CardUpDataBean> cardBeans = NodeCardUpBean[id].ToList();
            if (cardBeans == null || cardDictionarys == null) return;
            if (cardBeans.Count == 0) 
            {
                cardDictionarys.Clear();
                listView.Items.Clear();
                return;
            }

            Dictionary<string, CardUpDataBean> cardDicti = new Dictionary<string, CardUpDataBean>(cardDictionarys);
            foreach (CardUpDataBean cBean in cardBeans)
            {
                if (!cardDictionarys.ContainsKey(cBean.TagIdStr))
                {
                    cardDictionarys.Add(cBean.TagIdStr, cBean);
                    addItem(cBean);
                }
                else 
                {
                    for (int i = 0; i < listView.Items.Count; i++)
                    {
                        if (!listView.Items[i].SubItems[1].Text.Equals(cBean.TagIdStr)) continue;
                        changeItem(i, cBean);
                        break;
                    }
                    cardDicti.Remove(cBean.TagIdStr);
                }
            }
            if(cardDicti.Count  == 0) return;

            foreach (var item in cardDicti)
            {
                for (int i = 0; i < listView.Items.Count; i++)
                {
                    if (!listView.Items[i].SubItems[1].Text.Equals(item.Key)) continue;
                    listView.Items.RemoveAt(i);
                    break;
                }
                if (cardDictionarys.ContainsKey(item.Key))
                    cardDictionarys.Remove(item.Key);
            }
        }

        //寻找参考点名字  要不先挣他一个亿
        private string arrayCnKaoDianName(byte[] canKaoDianID)
        {
            List<CanKaoDianBean> canKaoDians = FileModel.getFlModel().ChFlBean.CanKaoDians;
            var canDs = canKaoDians.Where(a => XWUtils.byteBTBettow(canKaoDianID, a.CanDianID)).ToList();
            if (canDs.Count() > 0) return canDs.First().Name;
            else return canKaoDianID[0].ToString("X2") + canKaoDianID[1].ToString("X2");
        }

        //寻找卡片名字
        private string arrayTagName(byte[] tagID)
        {
            Dictionary<string, CardBean> cardDic = new Dictionary<string,CardBean>(FileModel.getFlModel().ChFlBean.CardDic);
            if (cardDic != null && cardDic.ContainsKey(tagID[0].ToString("X2") + tagID[1].ToString("X2")))
                return cardDic[tagID[0].ToString("X2") + tagID[1].ToString("X2")].Name;
            else return tagID[0].ToString("X2") + tagID[1].ToString("X2");
        }


        private void addItem(CardUpDataBean cBean)
        {
            ListViewItem lvItem = new ListViewItem();
            lvItem.SubItems[0].Text = arrayTagName(cBean.TagId);// cBean.TagIdStr;
            lvItem.SubItems.Add(cBean.TagIdStr);
            lvItem.SubItems.Add(arrayCnKaoDianName(cBean.Port1IDHistory));
            lvItem.SubItems.Add(cBean.Port1IDHistoryStr);
            lvItem.SubItems.Add(cBean.Port1RssiHistory.ToString());
            lvItem.SubItems.Add(arrayCnKaoDianName(cBean.Port2IDHistory));
            lvItem.SubItems.Add(cBean.Port2IDHistoryStr);
            lvItem.SubItems.Add(cBean.Port2RssiHistory.ToString());
            lvItem.SubItems.Add(arrayCnKaoDianName(cBean.Port3IDHistory));
            lvItem.SubItems.Add(cBean.Port3IDHistoryStr);
            lvItem.SubItems.Add(cBean.Port3RssiHistory.ToString());
 
            string locaType = "普通定位";
            lvItem.SubItems.Add(locaType);
            lvItem.SubItems.Add(cBean.Battery.ToString());
            lvItem.SubItems.Add(cBean.SensorTime.ToString());
            lvItem.SubItems.Add(XwDataUtils.currentTimeToSe(cBean.FirstReceiveTime));
            UInt32 tm = XwDataUtils.GetTimeStamp() - cBean.FirstReceiveTime;
            lvItem.SubItems.Add(tm.ToString());
            lvItem.SubItems.Add(cBean.TotalCount.ToString());
            lvItem.SubItems.Add(cBean.LostCount.ToString());
            string str = "TAG";
            if (cBean.DrivaceType == 0x04)
            {
                str = "pTAG";
            }
            else if (cBean.DrivaceType == 0x05)
            {
                str = "eTAG";
            }
            lvItem.SubItems.Add(str);
            lvItem.SubItems.Add(cBean.SleepTime.ToString());
            lvItem.SubItems.Add(cBean.SendGongLv.ToString());
            if (cBean.DrivaceType == 0x04)
            {
                byte LED_Staus = cBean.LEDStaus;
                if (LED_Staus == 0) str = "滅";
                else if (LED_Staus == 1) str = "亮";
                else if (LED_Staus == 2) str = "閃爍";
                else str = "";
            }
            else if (cBean.DrivaceType == 0x05)
            {
                byte LED_StausRED = (byte)(cBean.LEDStaus & 3);	  // 3  = 二进制：00 00 00 11;
                byte LED_StausGreen = (byte)(cBean.LEDStaus & 12);// 12 = 二进制：00 00 11 00;
                byte LED_StausBlue = (byte)(cBean.LEDStaus & 48); // 48 = 二进制：00 11 00 00;

                str = "R_" + getLEDstatus(LED_StausRED) + " G_" + getLEDstatus(LED_StausGreen) + " B_"
                    + getLEDstatus(LED_StausBlue);
            }
            lvItem.SubItems.Add(str);

            listView.Items.Add(lvItem);
        }

         
        private void changeItem(int item_index, CardUpDataBean cBean)
        {
            if (item_index >= listView.Items.Count) return;
            listView.Items[item_index].SubItems[0].Text = arrayTagName(cBean.TagId);
            listView.Items[item_index].SubItems[2].Text = arrayCnKaoDianName(cBean.Port1IDHistory);
            listView.Items[item_index].SubItems[3].Text = cBean.Port1IDHistoryStr;
            listView.Items[item_index].SubItems[4].Text = cBean.Port1Rssi.ToString();
            listView.Items[item_index].SubItems[5].Text = arrayCnKaoDianName(cBean.Port2IDHistory);
            listView.Items[item_index].SubItems[6].Text = cBean.Port2IDHistoryStr;
            listView.Items[item_index].SubItems[7].Text = cBean.Port2Rssi.ToString();
            listView.Items[item_index].SubItems[8].Text = arrayCnKaoDianName(cBean.Port3IDHistory);
            listView.Items[item_index].SubItems[9].Text = cBean.Port3IDHistoryStr;
            listView.Items[item_index].SubItems[10].Text = cBean.Port3Rssi.ToString();
            string locaType = "普通定位";
            listView.Items[item_index].SubItems[11].Text = locaType;
            listView.Items[item_index].SubItems[12].Text = cBean.Battery.ToString();
            listView.Items[item_index].SubItems[13].Text = cBean.SensorTime.ToString();
            listView.Items[item_index].SubItems[14].Text = XwDataUtils.currentTimeToSe(cBean.FirstReceiveTime);
            UInt32 tm = XwDataUtils.GetTimeStamp() - cBean.FirstReceiveTime;
            listView.Items[item_index].SubItems[15].Text = tm.ToString();
            listView.Items[item_index].SubItems[16].Text = cBean.TotalCount.ToString();
            listView.Items[item_index].SubItems[17].Text = cBean.LostCount.ToString();
            string str = "TAG";
            if (cBean.DrivaceType == 0x04)
            {
                str = "pTAG";
            }
            else if (cBean.DrivaceType == 0x05)
            {
                str = "eTAG";
            }
            listView.Items[item_index].SubItems[18].Text = str;
            listView.Items[item_index].SubItems[19].Text = cBean.SleepTime.ToString();
            listView.Items[item_index].SubItems[20].Text = cBean.SendGongLv.ToString();
            if (cBean.DrivaceType == 0x04)
            {
                byte LED_Staus = cBean.LEDStaus;
                if (LED_Staus == 0) str = "滅";
                else if (LED_Staus == 1) str = "亮";
                else if (LED_Staus == 2) str = "閃爍";
                else str = "";
            }
            else if (cBean.DrivaceType == 0x05)
            {
                byte LED_StausRED = (byte)(cBean.LEDStaus & 3);	  // 3  = 二进制：00 00 00 11;
                byte LED_StausGreen = (byte)(cBean.LEDStaus & 12);// 12 = 二进制：00 00 11 00;
                byte LED_StausBlue = (byte)(cBean.LEDStaus & 48); // 48 = 二进制：00 11 00 00;
                str = "R_" + getLEDstatus(LED_StausRED) + " G_" + getLEDstatus(LED_StausGreen) + " B_"
                    + getLEDstatus(LED_StausBlue);
            }
            listView.Items[item_index].SubItems[21].Text = str;
        }


        private string getLEDstatus(byte status)
        {
            string sta = "滅";
            if (status == 1)
            {
                sta = "亮";
            }
            else if (status == 2)
            {
                sta = "閃爍";
            }
            return sta;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1) return;
            if (cardHand != null) cardHand(listView1.SelectedItems[0]);
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count < 1) return;
            new Thread(liViewSelect).Start(); //本来是不开线程，是在是这个问题太奇葩了。找不到原因，就开一个线程
        }

        private void liViewSelect() //如果不开线程，点击一次，等弹出的界面消失后，立马又跳出一个界面，就是会跑两次，不知道是那个地方膈应到这个事件了
        {
            try
            {
                this.Invoke((EventHandler)(delegate
                { //放入主線程    
                    if (listView.SelectedItems.Count < 1) return;
                    ListViewItem lvItem = listView.SelectedItems[0];
                    if (cardHand != null) cardHand(listView.SelectedItems[0]);
                }));
            }
            catch { }
        }

    }
}
