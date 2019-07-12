using CiXinLocation.bean;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CiXinLocation
{
    //警告界面
    public partial class WarnForm : Form
    {
        private Dictionary<string, DrivaceWarnMessage> cardLowEleWarnMsgs;//
        private Dictionary<string, DrivaceWarnMessage> cardUnanswerWranMsgs;//
        private Dictionary<string, DrivaceWarnMessage> nODEUnanswerWranMsgs;//

        public WarnForm()
        {
            InitializeComponent();            
            createCardLowEleWarnMsgs();
            createCardUnanswerWranMsgs();
            createNODEUnanswerWranMsgs();
        }

        private void WarnForm_Load(object sender, EventArgs e)
        {
            FormClosed += warnFormClosedEventHandler;
            WarnMessage.getWarnMessage().warnMsg += wranFormMessage;
            WarnMessage.getWarnMessage().drivaceClear(WarnType.NOTHING);       
            //setLowEleList();
            //setUnAnswerCardList();
            //setUnAnswerNODEList();
        }

        public void warnFormClosedEventHandler(object sender, FormClosedEventArgs e)        
        {
            WarnMessage.getWarnMessage().warnMsg -= wranFormMessage;
            cardLowEleWarnMsgs.Clear();  //
            cardUnanswerWranMsgs.Clear();//
            nODEUnanswerWranMsgs.Clear();//
        }

        public void wranFormMessage(int msgCount)
        {
            if (msgCount < 0) return;
            this.Invoke((EventHandler)(delegate
            { //放入主線程    
                label1.Text = "報警資訊數量:" + msgCount;
                addListInView();
            }));
        }

        private void addListInView() //添加新的数据到列表中
        {
            createCardLowEleWarnMsgs();           
            addListInView(cardLowEleWarnMsgs,listView1);
            setLowEleList();

            createCardUnanswerWranMsgs();
            addListInView(cardUnanswerWranMsgs, listView2);       
            setUnAnswerCardList();
            
            createNODEUnanswerWranMsgs();
            addListInView(nODEUnanswerWranMsgs, listView3);       
            setUnAnswerNODEList();
        }

        private void addListInView(Dictionary<string, DrivaceWarnMessage> warnMsgs,ListView listView) 
        {
            int count = listView.Items.Count;
            for (int i = 0; i < count; i++)
            {
                string driID = (string)listView.Items[i].Tag;
                if (warnMsgs.ContainsKey(driID))
                    warnMsgs.Remove(driID);
            }
        }

        private void setUnAnswerNODEList()
        {
            if (nODEUnanswerWranMsgs.Count > 0)
            {
                List<ListViewItem> lisNODE = new List<ListViewItem>();
                foreach (var item in nODEUnanswerWranMsgs)
                {
                    if (item.Value.IsClear) continue;
                    lisNODE.Add(getUnanswerNODE(item.Value));
                }
                ListViewItem[] listNODE = lisNODE.ToArray();
                listView3.Items.AddRange(listNODE);
            }
        }

        private void setUnAnswerCardList()
        {
            if (cardUnanswerWranMsgs.Count > 0)
            {
                List<ListViewItem> lisCard = new List<ListViewItem>();
                foreach (var item in cardUnanswerWranMsgs)
                {
                    if (item.Value.IsClear) continue;
                    lisCard.Add(getUnanswerCard(item.Value));
                }
                ListViewItem[] listCard = lisCard.ToArray();
                listView2.Items.AddRange(listCard);
            }
        }
        
        private void setLowEleList() 
        {
            if (cardLowEleWarnMsgs.Count > 0)
            {
                List<ListViewItem> lis = new List<ListViewItem>();
                foreach (var item in cardLowEleWarnMsgs)
                {
                    if (item.Value.IsClear) continue;
                    lis.Add(getLowEleItem(item.Value));
                }
                ListViewItem[] listLowEle = lis.ToArray();
                listView1.Items.AddRange(listLowEle);
            }
        }

        private string getTimeFromINT(long time) //time 是时间戳
        {
            if (time == 0)
                return "";
            return XwDataUtils.currentTimeToSe(time);
        }

        private ListViewItem getLowEleItem(DrivaceWarnMessage driMsg) 
        {
            if (driMsg.WarnTp != WarnType.CARD_LOW_ELECTRICITY) return null;
            ListViewItem liViewItem = new ListViewItem();
            liViewItem.Tag = driMsg.DrivaceID;
            liViewItem.SubItems[0].Text = driMsg.DrivaceName;
            liViewItem.SubItems.Add(driMsg.QuYuName);
            liViewItem.SubItems.Add(driMsg.CanKaoDianName);
            liViewItem.SubItems.Add(driMsg.CurrentElectricity.ToString());
            liViewItem.SubItems.Add(getTimeFromINT(driMsg.WarnTime));
            liViewItem.SubItems.Add(getTimeFromINT(driMsg.DealWarnTime));
            liViewItem.SubItems.Add(driMsg.IsDeal?"是":"否");
            return liViewItem;
        }

        private ListViewItem getUnanswerCard(DrivaceWarnMessage driMsg) 
        {
            if (driMsg.WarnTp != WarnType.CARD_UNANSWERED) return null;
            ListViewItem liViewItem = new ListViewItem();
            liViewItem.Tag = driMsg.DrivaceID;
            liViewItem.SubItems[0].Text = driMsg.DrivaceName;
            liViewItem.SubItems.Add(driMsg.QuYuName);
            liViewItem.SubItems.Add(driMsg.CanKaoDianName);
            liViewItem.SubItems.Add(driMsg.SleepTime.ToString());
            liViewItem.SubItems.Add(getTimeFromINT(driMsg.WarnTime));
            liViewItem.SubItems.Add(getTimeFromINT(driMsg.DealWarnTime));
            liViewItem.SubItems.Add(driMsg.IsDeal ? "是" : "否");
            return liViewItem;
        }

        private ListViewItem getUnanswerNODE(DrivaceWarnMessage driMsg)
        {
            if (driMsg.WarnTp != WarnType.NODE_UNANSWERED) return null;
            ListViewItem liViewItem = new ListViewItem();
            liViewItem.Tag = driMsg.DrivaceID;
            liViewItem.SubItems[0].Text = driMsg.DrivaceName;
            liViewItem.SubItems.Add(driMsg.QuYuName);
            liViewItem.SubItems.Add(driMsg.SleepTime.ToString());
            liViewItem.SubItems.Add(getTimeFromINT(driMsg.WarnTime));
            liViewItem.SubItems.Add(getTimeFromINT(driMsg.DealWarnTime));
            liViewItem.SubItems.Add(driMsg.IsDeal ? "是" : "否");
            return liViewItem;
        }

        private void createCardLowEleWarnMsgs()
        {            
            if (cardLowEleWarnMsgs == null)
                cardLowEleWarnMsgs = new Dictionary<string, DrivaceWarnMessage>(WarnMessage.getWarnMessage().CardLowEleWarnMsgs);
            else
            {
                cardLowEleWarnMsgs.Clear();
                cardLowEleWarnMsgs = new Dictionary<string, DrivaceWarnMessage>(WarnMessage.getWarnMessage().CardLowEleWarnMsgs);
            }
        }

        private void createCardUnanswerWranMsgs()
        {
            if (cardUnanswerWranMsgs == null)
                cardUnanswerWranMsgs = new Dictionary<string, DrivaceWarnMessage>(WarnMessage.getWarnMessage().CardUnanswerWranMsgs);
            else 
            {
                cardUnanswerWranMsgs.Clear();
                cardUnanswerWranMsgs = new Dictionary<string, DrivaceWarnMessage>(WarnMessage.getWarnMessage().CardUnanswerWranMsgs);
            }
        }

        private void createNODEUnanswerWranMsgs()
        {
            if (nODEUnanswerWranMsgs == null)
                nODEUnanswerWranMsgs = new Dictionary<string, DrivaceWarnMessage>(WarnMessage.getWarnMessage().NODEUnanswerWranMsgs);
            else
            {
                nODEUnanswerWranMsgs.Clear();
                nODEUnanswerWranMsgs = new Dictionary<string, DrivaceWarnMessage>(WarnMessage.getWarnMessage().NODEUnanswerWranMsgs);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked) return;
            if (tabControl1.SelectedTab == tabPage1) 
            {
                checkAllBox(listView1);
            }
            else if (tabControl1.SelectedTab == tabPage2) 
            {
                checkAllBox(listView2);
            }
            else if (tabControl1.SelectedTab == tabPage3) 
            {
                checkAllBox(listView3);
            }
        }

        public void checkAllBox(ListView listView) 
        {
            int listCount = listView.Items.Count;
            if (listCount < 1) return;
            for (int i = 0; i < listCount; i++)
            {
                listView.Items[i].Checked = true;
            }
        }

        private void button2_Click(object sender, EventArgs e) //处理警告
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                dealWarn(listView1,5);
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                dealWarn(listView2,5);
            }
            else if (tabControl1.SelectedTab == tabPage3)
            {
                dealWarn(listView3,4);
            }
        }

        private void dealWarn(ListView listView, int changeIndex) 
        {
            int listCount = listView.Items.Count;
            if (listCount < 1) return;
            for (int i = 0; i < listCount; i++)
            {
                if (!listView.Items[i].Checked) continue;
                dealWarnInModel((string)listView.Items[i].Tag);
                listView.Items[i].SubItems[changeIndex].Text = XwDataUtils.currentTimeToSe();
                listView.Items[i].SubItems[changeIndex+1].Text = "是";
            }
        }

        private void dealWarnInModel(string driID) 
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                WarnMessage.getWarnMessage().drivaceDeal(driID, WarnType.CARD_LOW_ELECTRICITY);          
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                WarnMessage.getWarnMessage().drivaceDeal(driID, WarnType.CARD_UNANSWERED);          
            }
            else if (tabControl1.SelectedTab == tabPage3)
            {
                WarnMessage.getWarnMessage().drivaceDeal(driID, WarnType.NODE_UNANSWERED);          
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 1) 
            {
                if (listView1.Items.Count > 0 &&　listView1.Items[0].ForeColor == Color.Red) 
                {
                    listView1.Items.Clear();
                    setLowEleList();
                }
                if (listView2.Items.Count > 0 && listView2.Items[0].ForeColor == Color.Red)
                {
                    listView2.Items.Clear();
                    setUnAnswerCardList();
                }
                if (listView3.Items.Count > 0 && listView3.Items[0].ForeColor == Color.Red)
                {
                    listView3.Items.Clear();
                    setUnAnswerNODEList();
                }                               
                return;
            } 
            string searchID = textBox1.Text;
            if (WarnMessage.getWarnMessage().CardLowEleWarnMsgs.ContainsKey(searchID))
            {
                if (tabControl1.SelectedTab != tabPage1)
                    tabControl1.SelectedTab = tabPage1;
                listView1.Items.Clear();
                listView1.Items.Add(getLowEleItem(WarnMessage.getWarnMessage().CardLowEleWarnMsgs[searchID]));
                listView1.Items[0].ForeColor = Color.Red;
            }
            else if (WarnMessage.getWarnMessage().CardUnanswerWranMsgs.ContainsKey(searchID))
            {
                if (tabControl1.SelectedTab != tabPage2)
                    tabControl1.SelectedTab = tabPage2;
                listView2.Items.Clear();
                listView2.Items.Add(getUnanswerCard(WarnMessage.getWarnMessage().CardUnanswerWranMsgs[searchID]));
                listView2.Items[0].ForeColor = Color.Red;
            }
            else if (WarnMessage.getWarnMessage().NODEUnanswerWranMsgs.ContainsKey(searchID))
            {
                if (tabControl1.SelectedTab != tabPage3)
                    tabControl1.SelectedTab = tabPage3;
                listView3.Items.Clear();
                listView3.Items.Add(getUnanswerNODE(WarnMessage.getWarnMessage().NODEUnanswerWranMsgs[searchID]));
                listView3.Items[0].ForeColor = Color.Red;
            }
            else
                MessageBox.Show("搜尋失敗!");
        }

        private void button3_Click(object sender, EventArgs e) //清除
        {

            if (tabControl1.SelectedTab == tabPage1)
            {
                clearWarn(listView1, 6,WarnType.CARD_LOW_ELECTRICITY);
                WarnMessage.getWarnMessage().drivaceClear(WarnType.CARD_LOW_ELECTRICITY);                        
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                clearWarn(listView2, 6,WarnType.CARD_UNANSWERED);
                WarnMessage.getWarnMessage().drivaceClear(WarnType.CARD_UNANSWERED);                
            }
            else if (tabControl1.SelectedTab == tabPage3)
            {
                clearWarn(listView3, 5,WarnType.NODE_UNANSWERED);
                WarnMessage.getWarnMessage().drivaceClear(WarnType.NODE_UNANSWERED);                
            }           
        }

        private void clearWarn(ListView listView,int changeIndex,WarnType wType)
        {           

            int listCount = listView.Items.Count;
            if (listCount < 1) return;
            List<ListViewItem> rListItem = new List<ListViewItem>();
            for (int i = 0; i < listCount; i++)
            {
                try 
                {
                    if (!"是".Equals(listView.Items[i].SubItems[changeIndex].Text)) continue;
                    rListItem.Add(listView.Items[i]);   
                }
                catch (Exception e)
                {
                    Debug.Write("clearWarn.."+e.Message);
                }                
            }
            foreach(var Item in rListItem)
            {
                listView.Items.Remove(Item);
            //    dealDriWarn();
            }            
        }

        private void dealDriWarn(string id,WarnType wType)
        {
            if (wType == WarnType.CARD_LOW_ELECTRICITY)
            {
                if(cardLowEleWarnMsgs != null && cardLowEleWarnMsgs.ContainsKey(id))
                    cardLowEleWarnMsgs.Remove(id);
            }
            else if (wType == WarnType.CARD_UNANSWERED)            
            {
                if (cardUnanswerWranMsgs != null && cardUnanswerWranMsgs.ContainsKey(id))
                    cardUnanswerWranMsgs.Remove(id);
            }
            else if (wType == WarnType.NODE_UNANSWERED)
            {
                if (nODEUnanswerWranMsgs != null && nODEUnanswerWranMsgs.ContainsKey(id))
                    nODEUnanswerWranMsgs.Remove(id);
            }
                    
          //  private Dictionary<string, DrivaceWarnMessage> cardLowEleWarnMsgs;//
          //      private Dictionary<string, DrivaceWarnMessage> cardUnanswerWranMsgs;//
          //      private Dictionary<string, DrivaceWarnMessage> nODEUnanswerWranMsgs;//
        }


    }
}
