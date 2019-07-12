using CiXinLocation.bean;
using CiXinLocation.CiXinInterface;
using CiXinLocation.Model;
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

namespace CiXinLocation.Utils
{
    public partial class HistoricalTrackForm : Form, MainViewInterface, QuyuChangeLinster
    {
        public LocationViewFrom locaViewFrom;
        private FromMainModel fromMainModel;
        private PlayStatus pStatus;
        private UInt32 startTime;
        private UInt32 endTime;
        private UInt32 runTime;
        private uint beiLv = 1; // 倍率       
        private QuyuAutoChange quyuChange;
        int totalCount = 0;		//总共接收到多少个封包  
        int lostCount = 0;		//丢包数量
        int index_card = -1;

        public HistoricalTrackForm()
        {
            InitializeComponent();
            fromMainModel = new FromMainModel(this);
            fromMainModel.IsDealHistory = true;
            pStatus = PlayStatus.STOP;
            quyuChange = new QuyuAutoChange();
            quyuChange.setOnChangeLinster(this);
        }

        private void button1_Click(object sender, EventArgs e) //开始
        {
            if (pStatus == PlayStatus.PLAYINT) 
            {
                PStatus = PlayStatus.STOP;
                return;
            }if( pStatus == PlayStatus.PAUSE)
            {
                PStatus = PlayStatus.PLAYINT;
                return;
            }
            locaViewFrom.clearList();
            StartTime = XwDataUtils.GetTimeStamp(dateTimePicker1.Value, true) - 28801;
            EndTime = XwDataUtils.GetTimeStamp(dateTimePicker2.Value, true) - 28800;
            if (startTime > endTime) 
            {
                MessageBox.Show("終止時間不能小於起始時間，請重新選擇時間");
                return;
            }
                 
            runTime = startTime;
            string tagID = getTextBoxName();
            locaViewFrom.locaModel.setHis_tagID(checkBox1.Checked,tagID);
            locaViewFrom.setCardID(tagID);

            if (pStatus == PlayStatus.STOP) 
            {
                new Thread(startListViewThread).Start(tagID);
                new Thread(startViewThread).Start(tagID);
            }
            try 
            {
                Thread.Sleep(10);    
            }
            catch { }                                     
        }

        private string getCardID(string tagName) 
        {
            string tagIDsTR = "";
            List<CardBean> cardBns = FileModel.getFlModel().ChFlBean.Cards.ToList();
            var cardB = cardBns.Where(tag => tag.Name.Equals(tagName));
            if (cardB.Count() > 0) 
            {
                tagIDsTR = cardB.First().Id;               
            }
            return tagIDsTR;
        }


        public void onQuyuChange(String changeText)
        {
            this.Invoke((EventHandler)(delegate
            {
                if (!comboBox2.Text.Equals(changeText)) 
                {
                    comboBox2.Text = changeText;
                    comboBox2_SelectedIndexChanged(null, null);  
                }
            }));
        }

        public void onCengJiChange(String changeText)
        {
            this.Invoke((EventHandler)(delegate
            {
                if (!comboBox1.Text.Equals(changeText))
                {
                    if (!comboBox1.Text.Equals(changeText))
                        comboBox1.Text = changeText;     
                }                 
            }));
        }


        private void startListViewThread(object obj)
        {
            if (!(obj is string)) return;
            createData();
            string cardID = (string)obj;
            PStatus = PlayStatus.PLAYINT;
            List<HistoryFileDataBean> hisFileBns = FileModel.getFlModel().HisFileDBeans.Where(a => isFile(a)).ToList();
            hisFileBns.OrderBy(a => a.StartTime);
            foreach (var item in hisFileBns)
            {
                //Debug.Write("....地址..." + item.FilePath + "\\" + item.FileName + "\r\n\r\n");
                if (pStatus == PlayStatus.STOP) break; //终止操作
                List<byte[]> chcheBy = FileModel.getFlModel().getHisFileListDataData(item.FilePath + "\\" + item.FileName, 1);
                int index = 0;
                int endIndex = chcheBy.Count - 1;
                getIndex(item, chcheBy, ref index, ref endIndex);
                try
                {
                    startSendListData(chcheBy, index, endIndex, cardID);
                }
                catch (Exception e)
                {
                    Console.Write(e.ToString()+"\r\n");
                    //MessageBox.Show(e.Message);
                }
            }
        }


        private void createData() 
        {
            totalCount = 0;		//总共接收到多少个封包  
            lostCount = 0;		//丢包数量
            index_card = -1;
        }


        private void startViewThread(object obj) 
        {
            if (!(obj is string)) return;
            string cardID = (string)obj;
            PStatus = PlayStatus.PLAYINT;
            List<HistoryFileDataBean> hisFileBns = FileModel.getFlModel().HisFileDBeans.Where(a => isFile(a)).ToList();
            hisFileBns.OrderBy(a => a.StartTime);
            foreach (var item in hisFileBns)
            {
                //Debug.Write("....地址..." + item.FilePath + "\\" + item.FileName + "\r\n\r\n");
                if (pStatus == PlayStatus.STOP) break; //终止操作
                List<byte[]> chcheBy = FileModel.getFlModel().getHisFileListDataData(item.FilePath + "\\" +item.FileName,1);
                int index = 0;
                int endIndex = chcheBy.Count - 1;
                getIndex(item,chcheBy, ref index, ref endIndex);
                try
                {
                    startSendData(chcheBy, index, endIndex, cardID);        
                }
                catch(Exception e) 
                {
                    MessageBox.Show(e.Message);
                }                           
            }
            if (PStatus == PlayStatus.STOP) return;
            this.Invoke((EventHandler)(delegate
            {
                PStatus = PlayStatus.STOP;
                MessageBox.Show("播放完畢！");
            }));
        }

        
        private void startSendListData(List<byte[]> sourBts, int index, int endIndex, string cardID)
        {
            if (endIndex < index || sourBts == null) return;

            long timeOut = long.MaxValue;           
            byte[] zeroBt = new byte[2]{0,0};
            List<CardUpDataBean> cardBeans = new List<CardUpDataBean>();
            for (int i = index; i <= endIndex; i++)
            {
                if (sourBts[i].Length < 16) continue;
                string sourCardID = sourBts[i][2].ToString("X2") + sourBts[i][3].ToString("X2");
                if (!sourCardID.Equals(cardID) && !checkBox1.Checked) continue;
                CardUpDataBean cardBean = new CardUpDataBean(sourBts[i]);
                if (cardBean.FirstReceiveTime > endTime) continue;
                if (cardBean.FirstReceiveTime - timeOut > FileModel.getFlModel().ChFlBean.NoReveTime) 
                {
                    totalCount = 0;
                    lostCount = 0;
                }
                if(index_card > -1)
                {
                    if (index_card + 1 != cardBean.Index && index_card != 255 && cardBean.Index > index_card) 
                    {
                        lostCount += cardBean.Index - index_card;
                    }
                    else if (index_card == 255 && cardBean.Index > 0)
                    {
                        lostCount += cardBean.Index;
                    }
                }
                index_card = cardBean.Index;
                if (!XWUtils.byteBTBettow(cardBean.Port1IDHistory, zeroBt)) totalCount += 1;
                cardBean.TotalCount = totalCount;
                cardBean.LostCount = lostCount;
                cardBeans.Add(cardBean);
            }

            locaViewFrom.setHisData(cardBeans,CardUpType.HISTORY_DATA);
        }


        private void startSendData(List<byte[]> sourBts,int index,int endIndex,string cardID) 
        {
            int cardIndex = 0;
            if (endIndex < index || sourBts == null) return;
            for (int i = index; i <= endIndex; i++)
            {
                if (pStatus == PlayStatus.STOP) break; //终止操作
                while (pStatus == PlayStatus.PAUSE)
                {
                    Thread.Sleep(10);
                }
                if (sourBts[i].Length < 16) continue;
                string sourCardID = sourBts[i][2].ToString("X2")+sourBts[i][3].ToString("X2");
                if (!sourCardID.Equals(cardID) && !checkBox1.Checked) continue;

                byte[] node1 = changeSourBt(sourBts[i], cardIndex);

                if (quyuChange != null) 
                {
                    byte[] nodeID = new byte[2] { sourBts[i][8], sourBts[i][9] };
                    quyuChange.setCurrentNODEID(nodeID);//设置一下参数，会去查询区域是否变化
                } 

                cardIndex++;
                UInt32 currentTime = getBtTime(sourBts[i]); //等候时间
                hisTime = currentTime;

                UInt32 writTime = (UInt32)(currentTime - runTime);
                while (writTime > 1 && currentTime > runTime)
                {
                    writTime = (UInt32)(currentTime - runTime);
                    if (PStatus == PlayStatus.STOP) break;
                    Thread.Sleep(100);
                }
                if (fromMainModel != null)  //等完时间，该发送数据出去了
                {
                    byte[] node2 = new byte[16];
                    byte[] node3 = new byte[16];
                    Array.Copy(node1, 0, node2, 0, 16);
                    Array.Copy(node1, 0, node3, 0, 16);
                    node2[4] = sourBts[i][11];
                    node2[5] = sourBts[i][12];
                    node2[6] = sourBts[i][13];
                    node3[4] = sourBts[i][14];
                    node3[5] = sourBts[i][15];
                    node3[6] = sourBts[i][16];
                    List<byte[]> sendLists = new List<byte[]>();
                    sendLists.Add(node1);
                    sendLists.Add(node2);
                    sendLists.Add(node3);
                    try 
                    {
                        threadSendData(sendLists);
                    }
                    catch { }                    
                }
            }
        }

        UInt32 hisTime = 0;
        private void threadSendData(object obj) 
        {
            List<byte[]> sendLists = (List<byte[]>)obj;
            if (locaViewFrom.locaModel != null)
                locaViewFrom.locaModel.setTimeDate(hisTime);
            fromMainModel.setTimeDate(hisTime);

            fromMainModel.reveData(sendLists[0], null);           
            fromMainModel.reveData(sendLists[1], null);
            fromMainModel.reveData(sendLists[2], null);
        }

        private byte[] changeSourBt(byte[] sourBts,int index) 
        {
            byte[] dataBts = new byte[16];
            if (sourBts.Length < 16) return dataBts;           
            dataBts[0] = 0xfe;
            dataBts[1] = 0x04;
            dataBts[2] = sourBts[2];
            dataBts[3] = sourBts[3];
            dataBts[4] = sourBts[8];
            dataBts[5] = sourBts[9];
            dataBts[6] = sourBts[10];
            dataBts[13] = (byte)(index % 0x100);

            if (sourBts.Length > 25) 
            {
                Array.Copy(sourBts, 18, dataBts,7,7);//拷贝18到24的值到index中
                dataBts[13] = sourBts[17];
                dataBts[1] = sourBts[25];
            }
            dataBts[14] = XWUtils.getCheckBit(dataBts);
            dataBts[15] = 0xfd;
            return dataBts;
        }

        private void getIndex(HistoryFileDataBean hisBn,List<byte[]> sourBts,ref int index,ref int endIndex) 
        {
            int start = 0;
            int end = sourBts.Count;
            int mid = end/2;
            if(sourBts.Count == 0) 
            {
                index = -1;
                return;
            }
            if (hisBn.StartTime < StartTime) 
            {
                if (mid + 1 >= sourBts.Count)
                {
                    if (getBtTime(sourBts[mid]) < StartTime) index = -1;
                    else if (getBtTime(sourBts[mid]) >= StartTime) index = sourBts.Count - 1;
                    return;
                }

                while (start <= mid)
                {
                    if (getBtTime(sourBts[mid]) >= StartTime)
                    {
                        end = mid;
                    }
                    else
                    {                        
                        if (getBtTime(sourBts[mid + 1]) >= StartTime)
                        {
                            index = mid;
                            break;
                        }
                        start = mid;
                    }
                    mid = (start + end) / 2;
                }               
            }
            if (hisBn.EndTime > EndTime)
            {
                while (start <= mid)
                {
                    if (getBtTime(sourBts[mid]) >= EndTime)
                    {
                        end = mid;
                    }
                    else
                    {
                        start = mid;
                        if (getBtTime(sourBts[mid + 1]) >= EndTime)
                        {
                            endIndex = mid;
                            break;
                        }
                    }
                    mid = (start + end) / 2;
                }
            }            
        }

        /// <summary>
        /// 数据源中存的时间戳值
        /// </summary>
        /// <param name="dBt">数据源</param>
        /// <returns></returns>
        private UInt32 getBtTime(byte[] dBt) 
        {
            if(dBt.Length < 8) return 0;
            UInt32 bTime = 0;
            bTime += dBt[4] * (UInt32)0x1000000;
            bTime += dBt[5] * (UInt32)0x10000;
            bTime += dBt[6] * (UInt32)0x100;
            bTime += dBt[7];
            return bTime;
        }

        /// <summary>
        /// 速度倍率值
        /// </summary>
        public uint BeiLv
        {
            get { return beiLv; }
            set { beiLv = value; }
        }

        private bool isFile(HistoryFileDataBean hisBean)
        {
            if (hisBean.StartTime > EndTime) return false;
            if (hisBean.EndTime < StartTime) return false;
            return true;
        }

        private void createLocationView() 
        {
            if (locaViewFrom == null) locaViewFrom = new LocationViewFrom();
        }

        //加載界面
        private void loadLocationViewFrom()
        {
            locaViewFrom.serchViewVisi(false);    //隐藏界面的搜索功能  
            locaViewFrom.closePageCtrol();//隐藏上一页下一页的说法
            locaViewFrom.locaModel.IsDealHistory = true;
            locaViewFrom.TopLevel = false;
            locaViewFrom.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            locaViewFrom.WindowState = FormWindowState.Normal;            
            panel1.Controls.Add(locaViewFrom);
            //locaViewFrom.showDataType(3); // 3代表的是这个界面的内容，locaViewFrom中，意思是显示全部信息
            if (fromMainModel != null) 
            {
                fromMainModel.onTagData += locaViewFrom.locaModel.distributionData;//绕的有点多
                fromMainModel.onCanKData += locaViewFrom.locaModel.distributionCKDData;//绕的有点多
                fromMainModel.onNODEData += locaViewFrom.locaModel.changeCanKaiDianIDtIME;
                fromMainModel.onhisTagData += locaViewFrom.locaModel.onHisData;
            }             
            locaViewFrom.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top;
            locaViewFrom.addAllData();
            //locaViewFrom.setCardID(textBox1.Text);
            locaViewFrom.setDataLoad();
            locaViewFrom.Show();
            locaViewFrom.loadDataInAllViewFrom();
        }

        private void HistoricalTrackForm_Load(object sender, EventArgs e)
        {
            FormClosed += HistoricalTrackFormClosedEventHandler;
            createLocationView();
            loadLocationViewFrom();
            loadDataCom(comboBox1,comboBox2);
            if (comboBox1.Items.Count > 0) comboBox1.Text = comboBox1.Items[0].ToString();
            if (comboBox2.Items.Count > 0) comboBox2.Text = comboBox2.Items[0].ToString();
        }

        public void HistoricalTrackFormClosedEventHandler(object sender, FormClosedEventArgs e)
        {
            PStatus = PlayStatus.STOP;

            fromMainModel.onTagData  -= locaViewFrom.locaModel.distributionData;  //绕的有点多
            fromMainModel.onCanKData -= locaViewFrom.locaModel.distributionCKDData;//绕的有点多
            fromMainModel.onNODEData -= locaViewFrom.locaModel.changeCanKaiDianIDtIME;
            locaViewFrom.MFormMian = null;
            locaViewFrom.closeFoem();          

            if (fromMainModel != null) fromMainModel.close();
            endTime = UInt32.MinValue;
            startTime = UInt32.MaxValue;
            timer1.Enabled = false;

        }

        public UInt32 StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public UInt32 EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        private void button2_Click(object sender, EventArgs e) //搜尋
        {
            string cardName = textBox1.Text;
            if (cardName.Length < 1)
            {
                MessageBox.Show("請輸入卡片ID或者名稱");
                return;
            }
            locaViewFrom.setCardID(cardName);
        }

        private string getTextBoxName() 
        {
            string cardName = textBox1.Text;
            if (cardName.Length < 1)
            {
                return "";
            }
            string cardID = getCardID(cardName);
            if (cardID.Length > 0) cardName = cardID;
            else if (FileModel.getFlModel().ChFlBean.CardDic.ContainsKey(cardName))
            {
                cardName = FileModel.getFlModel().ChFlBean.CardDic[cardName].Id;
            }
            else
            {
                cardName = cardName.ToUpper();
            }
            return cardName;
        }

        private void button3_Click(object sender, EventArgs e) //暂停
        {
            PStatus = PlayStatus.PAUSE;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comItemSelect(comboBox1, comboBox2);
        }

        public void comItemSelect(ComboBox comScour, ComboBox com)
        {
            if (comScour.Text == null || comScour.Text.Length < 1) return;

            List<CenJiBean> cLit = FileModel.getFlModel().CenJiData;
            if (cLit == null || cLit.Count < 1) return;
            foreach (CenJiBean cjItem in cLit)
            {
                string comID = getIDFromKuohao(comScour.Text);
                if (!cjItem.ID.Equals(comID)) continue;
                if (cjItem.QuYuBeans == null) break;
                com.Items.Clear();
                foreach (QuYuBean quYuItem in cjItem.QuYuBeans)
                {
                    com.Items.Add(getquyuNameId(quYuItem).ToString());
                }
            }
        }

        /// <summary>
        /// 初始化数据/85285639634
        /// </summary>
        private void loadDataCom(ComboBox cbCenJi2, ComboBox cbQuYu)
        {
            List<CenJiBean> cLit = FileModel.getFlModel().CenJiData;
            if (cLit == null || cLit.Count < 1) return;
            cbCenJi2.Items.Clear();
            cbQuYu.Items.Clear();
            foreach (CenJiBean cjItem in cLit)
            {
                cbCenJi2.Items.Add(getCenJibuder(cjItem).ToString());
                if (cjItem.QuYuBeans == null) continue;
                foreach (QuYuBean quYuItem in cjItem.QuYuBeans)
                {
                    cbQuYu.Items.Add(getquyuNameId(quYuItem));
                }
            }
        }

        private StringBuilder getCenJibuder(CenJiBean cjItem)
        {
            StringBuilder buder = new StringBuilder();
            buder.Append(cjItem.CenJiName);
            buder.Append("(");
            buder.Append(cjItem.ID);
            buder.Append(")");
            return buder;
        }

        private StringBuilder getquyuNameId(QuYuBean quYuItem)
        {
            StringBuilder buder = new StringBuilder();
            buder.Append(quYuItem.QuyuName);
            buder.Append("(");
            buder.Append(quYuItem.QuyuID);
            buder.Append(")");
            return buder;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cenjiID = getIDFromKuohao(comboBox1.Text);
            string quyuID = getIDFromKuohao(comboBox2.Text);
            loadNewImage(cenjiID, quyuID);
            locaViewFrom.SelectBool = checkBox1.Checked;
            locaViewFrom.addAllData();
        }

        /// <summary>
        /// 从括号中提取层级ID号
        /// </summary>
        /// <returns></returns>
        public string getIDFromKuohao(string cejiText)
        {
            if (cejiText.Equals("") || cejiText == null) return "";
            int start = cejiText.IndexOf('(');
            int end = cejiText.IndexOf(')');
            return cejiText.Substring(start + 1, end - start - 1);
        }

        private void loadNewImage(string cenjiID, string quyuID)
        {
            List<CenJiBean> cLit = FileModel.getFlModel().CenJiData;
            var cenji = cLit.Where(cItem => { return cItem.ID.Equals(cenjiID); });
            if (cenji.Count() == 0) return;

            var cenJiItem = cenji.First();
            var quItem = cenJiItem.QuYuBeans.Where(item => item.QuyuID.Equals(quyuID));
            if (quItem.Count() > 0)
            {
                setDataForFrom(cenJiItem.ID, quItem.First());
                locaViewFrom.addLoadData();
            }
        }

        private void setDataForFrom(string cengjiID, QuYuBean quyu)
        {
            if (quyu != null) locaViewFrom.QUYUBean = quyu;
            locaViewFrom.CkdBeans = FileModel.getFlModel().ChFlBean.CanKaoDians;
            locaViewFrom.CengjiID = cengjiID;
        }

        public void onPeopleAllCount(int count){}

        public void onQuyuPeopleCount(List<CenJiBean> cLit){}

        public void onQuYuName(string cenjiID, string quyuID, string changeName) { }

        public void onCenJiName(string cenjiID, string cenJiName) { }

        private void timer1_Tick(object sender, EventArgs e)
        {
            runTime += BeiLv;
            label5.Text = "歷史時間:" + XwDataUtils.currentTimeToSe(runTime, "yyyy-MM-dd HH:mm:ss");
            if (locaViewFrom.locaModel != null)
                locaViewFrom.locaModel.setRunTimeDate(runTime+1);
            if (fromMainModel == null) return;
            fromMainModel.checkShowPoint();
            
            fromMainModel.setRunTimeDate(runTime+1);
        }

        internal PlayStatus PStatus
        {
            get { return pStatus; }
            set 
            {                
                pStatus = value;
                PlayStatusDeal(pStatus);
            }
        }

        private void PlayStatusDeal(PlayStatus pStatus) 
        {
            this.Invoke((EventHandler)(delegate
            {
                if (pStatus == PlayStatus.PLAYINT)
                {
                    timer1.Interval = 1000;
                    timer1.Start();
                    button1.Text = "停止";
                }
                else if (pStatus == PlayStatus.STOP)
                {
                    timer1.Stop();
                    button1.Text = "開始";
                }
                else if (pStatus == PlayStatus.PAUSE)
                {
                    timer1.Stop();
                    button1.Text = "繼續";
                }
            }));            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string beiLvTe = textBox2.Text;
            int bLv = XWUtils.stringToInt1(beiLvTe);
            if (bLv == -1) 
            {
                MessageBox.Show("輸入的倍率值有誤！");
                return;
            }
            BeiLv = (uint)bLv;
        }

        public void message(string msg, int type){}
    }

    enum PlayStatus 
    {
        PLAYINT = 1,
        PAUSE = 2,
        STOP = 3,
    }
}
