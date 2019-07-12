using CiXinLocation.bean;
using CiXinLocation.CiXinInterface;
using CiXinLocation.Model;
using CiXinLocation.Properties;
using CiXinLocation.Utils;
using MoveableListLib.Bean;
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
    public partial class FormMain : FormBase, MainViewInterface, IMessageFilter
    {
        public static String getMainName() 
        {
            if (MainModel == 1) return "主機";
            else return "從機";
        }
        public static int MainModel = 1;
        
        public static int MainVersion = 2;
        public static int subVersion = 01;
        private const int move_down = 1;
        private const int move_up = 2;
        private CenJiBean selectCenJiBean;      //當前選中的層級
        private FromMainModel fromMainModel;
        private delegate void TCPServerHandle(bool isOpen, CommunicationMode comMode);
        private TCPServerHandle tcpHandle;
        private delegate void TAGSERCHResultHandle(bool isOpen);
        private TAGSERCHResultHandle tagSerchHandle;
        private int iOperCount = 0;
        private long appOpenCount = 0; //应用未使用状态
        private long runTimeCount = 0; //应用跑动的时间
        private ushort timeTickCount = 0;
        private loginForm loginForm;
        private bool isfromClose = false;

        private void te(ref int x) 
        {
            Console.Write(x.ToString()+"\r\n");
            x = 100;
            Console.Write(x.ToString() + "\r\n");
        }

        private void text() 
        {
            FileModel.getFlModel().setErrorAppData("");
        }

        private int serchErFen(CardUpDataBean c,List<CardUpDataBean> addInt) 
        {
            if (c == null || addInt == null) return 0;
            int low = 0;
            int height = addInt.Count;
            while (low < height)
            {
                int mid = (low+height)/2;                
                if(addInt[mid].Port1IDStr.Equals(c.Port1IDStr)) return mid;
                else if (getIDLength(c.Port1ID) > getIDLength(addInt[mid].Port1ID))
                {
                    if (low != mid) low = mid;
                    else low++;
                }
                else 
                {
                    if (height != mid) height = mid;
                    else height--;
                } 
            }
            return -1;
        }

        private int getIDLength(byte[] byteID) 
        {
            if (byteID.Length < 2) return 0;
            return byteID[0] * 0x100 + byteID[1];
        }

        public FormMain()
        {
            text();
            InitializeComponent();
            WarnMessage.getWarnMessage().warnMsg += wranMessage;
            FileModel.getFlModel().start();
            FileModel.getFlModel().getdata += newDataChange;
            fromMainModel = new FromMainModel(this);
            Application.AddMessageFilter(this);
            IsForwordInTcoServer = true;            
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.Text += FormMain.getMainName() + FormMain.MainVersion + "." + FormMain.subVersion;
            FormClosing += mainFormClosingEventHandler;
            moveThreeAbleList1.Colors = Loca_NodeUtils.getColors().ToList();
            moveableList1.onMListItemClick += onItemClick;
            FileModel.getFlModel().getData();
            moveThreeAbleList1.onMItemClickBack += onMItemClick;            
            ThreadPool.SetMaxThreads(20, 20);        
           
            timer2.Interval = 1000;
            if (!timer2.Enabled) timer2.Start();

            moveableList1.loadNewData(FileModel.getFlModel().CenJiData);
            moveableList1.onItemClick(0);

            new Thread(showLoginFormThread).Start();
            new Thread(pcRestart_Load).Start();

        }

        private void pcRestart_Load() 
        {
            RestartBean resBean = FileModel.getFlModel().getRestartData();          
            if (resBean == null) return;
            Thread.Sleep(200);
            if (resBean.PcType == 1)
            {
                if (resBean.CardLowEleWarnMsgs != null)
                    WarnMessage.getWarnMessage().CardLowEleWarnMsgs = new Dictionary<string, DrivaceWarnMessage>(resBean.CardLowEleWarnMsgs);
                if (resBean.CardUnanswerWranMsgs != null)
                    WarnMessage.getWarnMessage().CardUnanswerWranMsgs = new Dictionary<string, DrivaceWarnMessage>(resBean.CardUnanswerWranMsgs);
                if (resBean.NODEUnanswerWranMsgs != null)
                    WarnMessage.getWarnMessage().NODEUnanswerWranMsgs = new Dictionary<string, DrivaceWarnMessage>(resBean.NODEUnanswerWranMsgs);
                if ((resBean.CardLowEleWarnMsgs != null && resBean.CardLowEleWarnMsgs.Count > 0) ||
                    (resBean.CardUnanswerWranMsgs != null && resBean.CardUnanswerWranMsgs.Count > 0) ||
                    (resBean.NODEUnanswerWranMsgs != null && resBean.NODEUnanswerWranMsgs.Count > 0))
                    WarnMessage.getWarnMessage().warnMsgCallBack();
                if (resBean.DealUpCardDatas != null)
                    fromMainModel.DealUpCardDatas = resBean.DealUpCardDatas.ToList();
                PeoplePowerModel.getPeoplePowerModel().PowerValue = resBean.PowerValue;
                PeoplePowerModel.getPeoplePowerModel().Jurisdiction = resBean.Jurisdiction;
                if (!getcdInterface2Open() && resBean.TCPopen) canshuButton19_Click(null, null);
                if (!getcdInterfaceOpen() && resBean.UDPopen) UDPOpen();
                FileModel.getFlModel().deleteRestartData();
            }
            else if (resBean.PcType == 2) 
            {
                Thread.Sleep(300);
                PeoplePowerModel.getPeoplePowerModel().Password = resBean.Password;
                PeoplePowerModel.getPeoplePowerModel().Count = resBean.Count;
                PeoplePowerModel.getPeoplePowerModel().PowerValue = resBean.PowerValue;
                PeoplePowerModel.getPeoplePowerModel().Jurisdiction = resBean.Jurisdiction;
                if (!getcdInterface3Open() && resBean.TCPClienopen) canshuButton24_Click(null, null);
                if (resBean.TCPClienopen) new Thread(congjiOpenDataThread).Start();
                FileModel.getFlModel().deleteRestartData();
            }
        }

        /// <summary>
        /// 从机打开主机的数据
        /// </summary>
        private void congjiOpenDataThread() 
        {
            for (int i = 0; i < 10; i++)
            {
                if (!getcdInterface3Open()) canshuButton24_Click(null, null);
                else break;
                Thread.Sleep(3000);
            }     

            for (int i = 0; i < 10; i++) 
            {
                if (getcdInterface3Open()) 
                {
                    string btnColor = (string)button1.Tag;
                    if (!"green".Equals(btnColor)) break;
                    fromMainModel.sendConjiYanzheng("green");                   
                }
                //else canshuButton24_Click(null, null);    
                Thread.Sleep(1000);
            }
                  
            /*for (int i = 0; i < 10; i++)
            {
                if (PeoplePowerModel.getPeoplePowerModel().IsConnect) break;
                if (getcdInterface3Open())
                {
                    fromMainModel.sendConjiYanzheng("green");
                    break;
                }
                Thread.Sleep(100);
            }*/
        }

        private void mainFormClosingEventHandler(object sender, FormClosingEventArgs e)
        {
            if (!PeoplePowerModel.getPeoplePowerModel().isHaveExeOC())
            {
                MessageBox.Show("您無權關閉軟體！");
                e.Cancel = true;
            }
            isfromClose = true;
            FileTcpClienModel.getFileTcpClienMidel().classClose();
        }//        

        private void showLoginFormThread() 
        {
            Thread.Sleep(500);
            this.Invoke((EventHandler)(delegate
            { //放入主線程    
                showLoginForm();
            }));            
        }
        
        private void createLoginForm() 
        {
            if(loginForm == null)
                loginForm = new loginForm();
        }

        private void showLoginForm() 
        {
            createLoginForm();
            if (loginForm.Visible) return;
            DialogResult dResult = loginForm.ShowDialog();
            if (dResult != DialogResult.OK)
            {
                closeFromBaseModel();
                Application.Exit();
            }
            else 
            {
                label6.Text = "[退出]";
                if (loginForm != null)
                    label7.Text = loginForm.getAccount();
                if (PeoplePowerModel.getPeoplePowerModel().Jurisdiction != PeoplePowerModel.getPeoplePowerModel().CongjiValue)
                {
                    if (!button10.Visible)  button10.Visible = true;
                    if (button11.Visible) button11.Visible = false;
                    if (!getcdInterface2Open()) canshuButton19_Click(null, null);
                }
                else 
                {
                    if (button10.Visible) button10.Visible = false;
                    if (!button11.Visible) button11.Visible = true;
                    if (!getcdInterface3Open()) canshuButton24_Click(null, null);
                }
            }
        }

        private void tcpServerHandle(bool isOpen, CommunicationMode comMode) 
        {
            if (tcpHandle != null) tcpHandle(isOpen, comMode);
        }

        /// <summary>
        /// 层级或区域数据发生了变化
        /// </summary>
        private void newDataChange(int type) 
        {
            try 
            {
                this.Invoke((EventHandler)(delegate
                { //放入主線程    
                
                //    if (moveThreeAbleList1.Controls.Count > 0) moveThreeAbleList1.Controls.Clear();
                    onItemClick(0);//只能这样，给个默认值
                }));                
            }
            catch { }         
        }

        //鼠标进入控件时,背景变化
        //开始设置，参数设置和查询按钮
        private void button_mouse(object sender, EventArgs e)
        {
            Button btnSen = (Button)sender;
            if (btnSen.Tag.Equals("green"))
            {
                btnSen.BackgroundImage = Resources.start_2;
            }
            else if (btnSen.Tag.Equals("yellow"))
                btnSen.BackgroundImage = Resources.start_4;
        }

        //鼠标进入控件时,背景变化
        //开始设置，参数设置和查询按钮
        private void button_mouse_over(object sender, EventArgs e)
        {
            Button btnSen = (Button)sender;
            if (btnSen.Tag.Equals("green"))
            {
                btnSen.BackgroundImage = Resources.start_1;
            }
            else if (btnSen.Tag.Equals("yellow"))
                btnSen.BackgroundImage = Resources.start_3;           
        }

        //中心位置的上下移动按钮
        private void button_mouseDown(object sender, MouseEventArgs e)
        {
            Button btnSen = (Button)sender;
            if (btnSen.Tag == "quyu_up")
                btnSen.BackgroundImage = Resources.quyu_up2;
            else if (btnSen.Tag == "quyu_down")
                btnSen.BackgroundImage = Resources.quyu_down2;
            else if (btnSen.Tag == "CHANGQU_UP")
                btnSen.BackgroundImage = Resources.manu_up2;
             else if (btnSen.Tag == "CHANGQU_DOWN")
                btnSen.BackgroundImage = Resources.manu_down2;
        }

        private void button_mouseUp(object sender, MouseEventArgs e)
        {
            Button btnSen = (Button)sender;
            if (btnSen.Tag == "quyu_up")
                btnSen.BackgroundImage = Resources.quyu_up1;
            else if (btnSen.Tag == "quyu_down")
                btnSen.BackgroundImage = Resources.quyu_down1;
            else if (btnSen.Tag == "CHANGQU_UP")
                btnSen.BackgroundImage = Resources.manu_up1;
            else if (btnSen.Tag == "CHANGQU_DOWN")
                btnSen.BackgroundImage = Resources.manu_down1;
        }

        public void onItemClick(int index)
        {
            List<QuYuBean> quyuBeans = new List<QuYuBean>();
            try {
                quyuBeans = FileModel.getFlModel().CenJiData[index].QuYuBeans;
                selectCenJiBean = FileModel.getFlModel().CenJiData[index];              
            }
            catch { }
            moveThreeAbleList1.loadNewData(quyuBeans);          
        }

        private void button8_Click(object sender, EventArgs e)
        {
            moveThreeAbleList1.btnMoveClick(move_down);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            moveThreeAbleList1.btnMoveClick(move_up);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            moveableList1.btnMoveClick(move_up);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            moveableList1.btnMoveClick(move_down);
        }

        private void button2_Click(object sender, EventArgs e) //参数设置
        {
            if (!PeoplePowerModel.getPeoplePowerModel().isCanshuChange())
            {
                MessageBox.Show("您無此權限");
                return;
            }
            CanShuSetFrom mCanShuSet = new CanShuSetFrom();
            mCanShuSet.button19.Click += canshuButton19_Click;
            mCanShuSet.button24.Click += canshuButton24_Click;
            tcpHandle += mCanShuSet.tcpServerHandle;           
            mCanShuSet.Show();
            tcpServerHandle(getcdInterface2Open(),getCommunicationMode2());
            tcpServerHandle(getcdInterface3Open(), getCommunicationMode3());

            tcpServerHandle(button1.Tag.Equals("yellow"), CommunicationMode.TCPClien_File);
        }

        public void onMItemClick(int index, QuYuBean quYuBean)
        {
            if (!PeoplePowerModel.getPeoplePowerModel().isLocationShow())
            {
                MessageBox.Show("您無此權限");
                return;
            }
            LocationViewFrom locaViewFrom = new LocationViewFrom();
            locaViewFrom.QUYUBean = quYuBean;
            locaViewFrom.CkdBeans = FileModel.getFlModel().ChFlBean.CanKaoDians;
            locaViewFrom.CengjiID = selectCenJiBean.ID;
            locaViewFrom.MFormMian = this;
            locaViewFrom.setDataLoad();
            fromMainModel.onTagData += locaViewFrom.locaModel.distributionData;//绕的有点多
            fromMainModel.onCanKData += locaViewFrom.locaModel.distributionCKDData;//绕的有点多
            fromMainModel.onNODEData += locaViewFrom.locaModel.changeCanKaiDianIDtIME;
            locaViewFrom.locaModel.CloseUDPtime = fromMainModel.CloseUDPtime;
            locaViewFrom.locaModel.OpenUDPtime = fromMainModel.OpenUDPtime;

            fromMainModel.dirHandle();
            locaViewFrom.FormClosed += locaFormClosedEventHandler;
            timer1_Tick(null, null);           
            locaViewFrom.ShowDialog();
          //  loadOnNOdata();
        }

        public void loadOnNOdata() 
        {
            if (fromMainModel != null) fromMainModel.onNOData(null);
        }

        private void locaFormClosedEventHandler(object sender, FormClosedEventArgs e)
        {
            if (!(sender is LocationViewFrom)) return;
            LocationViewFrom lFrom = (LocationViewFrom)sender;
            if (lFrom.locaModel != null) 
            {
                fromMainModel.onTagData -= lFrom.locaModel.distributionData;//绕的有点多
                fromMainModel.onCanKData -= lFrom.locaModel.distributionCKDData;//绕的有点多
                fromMainModel.onNODEData -= lFrom.locaModel.changeCanKaiDianIDtIME;
                //lFrom.locaModel.close();
            }
            lFrom.closeFoem();
            lFrom = null;
        }

        private void button1_Click(object sender, EventArgs e) //開始接受數據
        {
            if (!PeoplePowerModel.getPeoplePowerModel().isMonitor()) 
            {
                show(10,"您無此權限");
                return;
            }
            if (PeoplePowerModel.getPeoplePowerModel().Jurisdiction != PeoplePowerModel.getPeoplePowerModel().CongjiValue) UDPOpen();
            else if (fromMainModel != null) 
            {
                if (!getcdInterface3Open()) 
                {
                    if (button1.Tag.Equals("green")) show(30,"請在設置界面->網絡管理->開啟TCPClien");
                    else btnChangeYellow();
                    return;
                }
                fromMainModel.sendConjiYanzheng((string)button1.Tag);
            }
        }

        private void UDPOpen() 
        {
            string locaIp = FileModel.getFlModel().ChFlBean.LocaIP;
            if (locaIp == null) locaIp = XWUtils.GetAddressIP();
            portName = locaIp;
            int poet = FileModel.getFlModel().ChFlBean.LocaPort;
            baudRate = poet == 0 ? 51234 : poet;
            comOpenClose(CommunicationMode.UDP);
        }

        private void canshuButton19_Click(object sender, EventArgs e) //開始接受數據
        {
            string locaIp = FileModel.getFlModel().ChFlBean.LocaIP_TCP;
            if (locaIp == null) locaIp = XWUtils.GetAddressIP();
            portName = locaIp;
            int poet = FileModel.getFlModel().ChFlBean.LocaPort_TCP;
            baudRate = poet == 0 ? 51234 : poet;
            comOpenClose2(CommunicationMode.TCPServer);
        }

        private void canshuButton24_Click(object sender, EventArgs e) //開始接受數據
        {
            string locaIp = FileModel.getFlModel().ChFlBean.ServerIP_TCP;
            if (locaIp == null) locaIp = XWUtils.GetAddressIP();
            portName = locaIp;
            int poet = FileModel.getFlModel().ChFlBean.ServerPort_TCP;
            baudRate = poet == 0 ? 51234 : poet;
            comOpenClose3(CommunicationMode.TCPClien);
        }

        public override void commOverCallBack(string msg, CommunicationMode comMode)
        {
            if (comMode == CommunicationMode.TCPServer) tcpServerHandle(getcdInterface2Open(), comMode);
            else if (comMode == CommunicationMode.TCPClien) tcpServerHandle(getcdInterface3Open(), comMode);

            if (!msg.Equals("%!%")) 
            {
                timer1.Stop();
                if (comMode == CommunicationMode.UDP)
                {
                    MessageBox.Show(msg);
                }
                else if (comMode == CommunicationMode.TCPServer)
                    return;
                else if (comMode == CommunicationMode.TCPClien)
                    return;
            }
            addFromBaseModel(fromMainModel);
            
            if (comMode == CommunicationMode.TCPServer) return;          

            timer1.Interval = 500;
            timer1.Start();
            if (button1.Tag.Equals("green") && getcdInterfaceOpen()) 
            {
                btnChangeGreen();
                fromMainModel.setListViewCanInfos();
                fromMainModel.OpenUDPtime = XwDataUtils.GetTimeStamp();
                                     
            }
            else if (button1.Tag.Equals("yellow") && !getcdInterfaceOpen()) 
            {
                btnChangeYellow();
            }          
        }

        private void btnChangeGreen() 
        {
            panel6.BackgroundImage = Resources.tag_2;
            panel7.BackgroundImage = Resources.pc_2;
            button1.BackgroundImage = Resources.start_3;
            button1.Tag = "yellow";
            button1.Text = "關閉連接";
        } 


        /// <summary>
        /// 將Buttn字樣先關掉
        /// </summary>
        private void btnChangeYellow() 
        {
            panel6.BackgroundImage = Resources.tag_1;
            panel7.BackgroundImage = Resources.pc_1;
            fromMainModel.CloseUDPtime = XwDataUtils.GetTimeStamp();
            button1.BackgroundImage = Resources.start_1;
            button1.Tag = "green";
            button1.Text = "開始連接監控";
        }

        public void onPeopleAllCount(int count) 
        {
            try 
            {
                this.Invoke((EventHandler)(delegate
                { //放入主線程    
                    button4.Text = count.ToString();
                }));
            }
            catch(Exception e)
            {
                Debug.Write("onPeopleAllCount.."+e.Message);
            }                      
        }        

        public void onQuyuPeopleCount(List<CenJiBean> cLit)
        {
            if(selectCenJiBean == null) return;
            foreach (CenJiBean cjb in cLit)
            {
                if (cjb.ID.Equals(selectCenJiBean.ID))  moveThreeAbleList1.changePeopleCount(cjb.QuYuBeans);               
            }
        }

        public void onQuYuName(string cenjiID, string quyuID, string changeName) { }
        public void onCenJiName(string cenjiID, string cenJiName) { }

        public void panpalTag_Click(object sender, EventArgs e) //開始接受數據
        {
            if (!(sender is Panel)) return;
            Panel tagPel = (Panel)sender;
            if (!(tagPel.Tag is TagLocationPointBean)) return;

            TagLocationPointBean tagPointBeab = (TagLocationPointBean)tagPel.Tag;
            Tag_LEDShow(tagPointBeab);
        }

        public void Tag_LEDShow(TagLocationPointBean tagPointBeab) //開始接受數據
        {
            LEDCONTROLForm LEDForm = new LEDCONTROLForm();
            fromMainModel.LedData += LEDForm.LEDmodel.reveMainModelData;
            LEDForm.LEDmodel.sendDataHandle += fromMainModel.sendDataModel;
            //addFromBaseModel(LEDForm.LEDmodel);
            LEDForm.FormClosing += OnLedViewDisposed;
            LEDForm.CardID = tagPointBeab.CardID;
            LEDForm.SleepTime = tagPointBeab.SleepTime;
            LEDForm.LedNmae = tagPointBeab.Name;
            LEDForm.MType = tagPointBeab.MType;
            LEDForm.SleepTime = tagPointBeab.SleepTime;
            LEDForm.GongLv = tagPointBeab.Gonglv;
            LEDForm.ShowDialog();
        }

        void OnLedViewDisposed(object sender, EventArgs e)
        {
            if (!(sender is LEDCONTROLForm)) return;
            LEDCONTROLForm ledForm = (LEDCONTROLForm)sender;
            ledForm.LEDmodel.close();
            fromMainModel.LedData -= ledForm.LEDmodel.reveMainModelData;
            ledForm.LEDmodel.sendDataHandle -= fromMainModel.sendDataModel;
            //closeFromBaseModel(ledForm.LEDmodel);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {            
            if (fromMainModel == null) return;            
            fromMainModel.checkShowPoint();
        }

        private void label4_Click(object sender, EventArgs e){}

        private void button4_Click(object sender, EventArgs e)
        {
            if (!PeoplePowerModel.getPeoplePowerModel().isLocationShow()) 
            {
                MessageBox.Show("您無此權限");
                return;
            }
            loadAllViewShow(null,null);
        }
         
        AllViewShow aShow;
        private void loadAllViewShow(CanKaoDianBean canDian, string cardID) 
        {
            if (aShow == null) 
            {
                aShow = new AllViewShow();
                fromMainModel.onTagData += aShow.locaViewFrom.locaModel.distributionData;//绕的有点多
                fromMainModel.onCanKData += aShow.locaViewFrom.locaModel.distributionCKDData;//绕的有点多
                fromMainModel.onNODEData += aShow.locaViewFrom.locaModel.changeCanKaiDianIDtIME;
                aShow.locaViewFrom.locaModel.CloseUDPtime = fromMainModel.CloseUDPtime;
                aShow.locaViewFrom.locaModel.OpenUDPtime = fromMainModel.OpenUDPtime;
            }
            fromMainModel.dirHandle();
            aShow.locaViewFrom.MFormMian = this;
            
            tagSerchHandle += aShow.tagSerchHandle;
            aShow.serchCardAllView += serchCardId;
            if (canDian != null && cardID != null) 
            {
                setSerchResult(canDian, cardID);
            }
            aShow.FormClosed += AllViewShowFormClosedEventHandler;
            loadOnNOdata();
            if (!aShow.Visible) aShow.ShowDialog();            
        }

        private void setSerchResult(CanKaoDianBean canDian, string cardID) 
        {
            if (aShow == null || (canDian == null && cardID == null)) return;
            aShow.CanKaoDian = canDian;
            aShow.TagID = cardID;
            aShow.serchData(canDian, cardID);
            aShow.serchCard();
            aShow.locaViewFrom.setCardID(cardID);
        }

        private void AllViewShowFormClosedEventHandler(object sender, FormClosedEventArgs e) 
        {
            if (!(sender is AllViewShow)) return;
            AllViewShow aShows = (AllViewShow)sender;
            aShows.serchCardAllView -= serchCardId;
            if (aShows.locaViewFrom != null) 
            {
                fromMainModel.onTagData  -= aShows.locaViewFrom.locaModel.distributionData;  //绕的有点多
                fromMainModel.onCanKData -= aShow.locaViewFrom.locaModel.distributionCKDData;//绕的有点多
                fromMainModel.onNODEData -= aShows.locaViewFrom.locaModel.changeCanKaiDianIDtIME;
                aShows.locaViewFrom.MFormMian = null;
                aShows.locaViewFrom.closeFoem();
                aShows.locaViewFrom.Close();
            }
            aShows.close();
            aShows.locaViewFrom = null;
            aShow = null;
            aShows = null;
        }

        private void button3_Click(object sender, EventArgs e) // 查询
        {

            if (!PeoplePowerModel.getPeoplePowerModel().isSerchTag())
            {
                MessageBox.Show("您無此權限");
                return;
            }
            serchCardFrom serFrom = new serchCardFrom();
            serFrom.serchCard += serchCardId;
            tagSerchHandle += serFrom.tagSerchHandle;
            serFrom.Show();
        }

        private void serchCardId(string cardID) 
        {
            byte[] nodeID = fromMainModel.sercahCard(cardID);
            if (nodeID == null) 
            {
                if (tagSerchHandle != null) tagSerchHandle(false);
                return;
            }            
            List<CanKaoDianBean> canKaoDians =  FileModel.getFlModel().ChFlBean.CanKaoDians;
            var canKaoDianItems = canKaoDians.Where(cDianItem => XWUtils.byteBTBettow(nodeID, cDianItem.CanDianID));
            if (canKaoDianItems.Count() == 0) 
            {
                MessageBox.Show("請在設置界面，設置相關節點，否則圖形無法顯示卡片位置");
                return;
            }
            if (tagSerchHandle != null) tagSerchHandle(true);
            if (aShow == null) loadAllViewShow(canKaoDianItems.First(), cardID);
            else  {
                setSerchResult(canKaoDianItems.First(), cardID);
            } 
        }

        private void button9_Click(object sender, EventArgs e){ }

        public void wranMessage(int msgCount) 
        {
            if (msgCount < 0) return;            
            this.Invoke((EventHandler)(delegate{ //放入主線程    
                button9.Text = msgCount.ToString();
            })); 
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            if (!PeoplePowerModel.getPeoplePowerModel().isWarnDeal())
            {
                MessageBox.Show("您無此權限");
                return;
            }
            WarnForm warForm = new WarnForm();
            warForm.ShowDialog();
        }

        public bool PreFilterMessage(ref Message m)
        {
            //如果检测到有鼠标或则键盘的消息，则使计数为0.....  
            if (m.Msg == 0x0200 || m.Msg == 0x0201 || m.Msg == 0x0204 || m.Msg == 0x0207)
            {
                iOperCount = 0;
                appOpenCount = 0;
            }
            return false;
        }

        public override void closeForm()
        {
            WarnMessage.getWarnMessage().warnMsg -= wranMessage;
        }

        private void timer2_Tick(object sender, EventArgs e) //长时间未操作，程序退出
        {
            if (PeoplePowerModel.getPeoplePowerModel().Jurisdiction == PeoplePowerModel.getPeoplePowerModel().CongjiValue)
            {
                timeTickCount++;
                if (timeTickCount > 21 && timeTickCount < 24) conjiWarnEleData();
                else if (timeTickCount > 44 && timeTickCount < 47) conjiWarnCardData();
                else if (timeTickCount > 66 && timeTickCount < 69) conjiWarnNODEData();
                else if (timeTickCount > 88) conjiTongBu();
                if (timeTickCount >= 90) timeTickCount = 0;                                   
            }
            else 
            {                
                iOperCount++;
                if (iOperCount > 300)
                {
                    showLoginForm();
                    iOperCount = 0;
                }
            }
            if (runTimeCount > 21600 && appOpenCount > 300) 
            {
                appOpenCount = 0;
                appRestart();
            }
            appOpenCount++;
            runTimeCount++;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            label6.Text = "[登錄]";
            label7.Text = "...";
            showLoginForm();
        }

        private void button10_Click(object sender, EventArgs e) //歷史信息
        {
            HistoricalTrackForm hisTraForm = new HistoricalTrackForm();
            hisTraForm.ShowDialog();
        }

        private void show(int time,string msg)
        {
            MessageAutoClose ms = new MessageAutoClose(time);
            ms.show(msg);
            ms.ShowDialog();
        }

        public void message(string msg, int type)
        {
            if (isfromClose) return;
            if (type == 0) 
            {
                this.Invoke((EventHandler)(delegate
                { //放入主線程    
                    show(10,msg);
                }));
            }
            else if (type == 1) 
            {
                this.Invoke((EventHandler)(delegate
                { //放入主線程      
                    commOver3CallBack("%!%", CommunicationMode.TCPClien);//暂时借用一下UDP    
                    fromMainModel.sendConjiTongBu();
                }));
            }
            else if (type == 2)
            {
                this.Invoke((EventHandler)(delegate
                { //放入主線程    
                //    new Thread(appRestart).Start();                                      
                    commOver3CallBack("%!%", CommunicationMode.TCPClien);
                    //if (getcdInterface3Open()) canshuButton24_Click(null, null);
                    FileModel.getFlModel().setErrorAppData(msg);                       
                }));
                //MessageBox.Show(msg);
            }
            else if (type == 3)  //关闭
            {
                this.Invoke((EventHandler)(delegate
                { //放入主線程    
                    commOver3CallBack("%!%", CommunicationMode.TCPClien);//暂时借用一下UDP    
                    fromMainModel.sendConjiTongBu();
                }));
            }
        }


        public  void commOver3CallBack(string msg, CommunicationMode comMode)
        {
            addFromBaseModel(fromMainModel);
            timer1.Interval = 500;
            timer1.Start();

            if (button1.Tag.Equals("green") && getcdInterface3Open() && PeoplePowerModel.getPeoplePowerModel().IsConnect)
            {
                panel6.BackgroundImage = Resources.tag_2;
                panel7.BackgroundImage = Resources.pc_2;
                button1.BackgroundImage = Resources.start_3;
                button1.Tag = "yellow";
                button1.Text = "關閉連接";
                fromMainModel.setListViewCanInfos();
                fromMainModel.sendConjiReveLocaData();
                new Thread(tongBuAllData).Start();
                if (!timer3.Enabled) timer3.Start();
                fromMainModel.OpenUDPtime = XwDataUtils.GetTimeStamp();
            //    timer2.Interval = 1000;
            //    if (!timer2.Enabled) timer2.Start();    
            }
            else if (button1.Tag.Equals("yellow") && getcdInterface3Open() && !PeoplePowerModel.getPeoplePowerModel().IsConnect)
            {
                panel6.BackgroundImage = Resources.tag_1;
                panel7.BackgroundImage = Resources.pc_1;
                button1.BackgroundImage = Resources.start_1;
                button1.Tag = "green";
                button1.Text = "開始連接監控";
                fromMainModel.CloseUDPtime = XwDataUtils.GetTimeStamp();
            }
        }


        private void conjiTongBu() //同步数据
        {
         //   fromMainModel.IsTongbuData = true;
            FileTcpClienModel.getFileTcpClienMidel().sendConjiTongBu();
        //    new Thread(setTongbuFalse).Start();
        }

        private void conjiWarnEleData() //异常一
        {
         //   fromMainModel.IsTongbuData = true;
            FileTcpClienModel.getFileTcpClienMidel().sendConjiWarnEleData();
        //    new Thread(setTongbuFalse).Start();
        }

        private void conjiWarnCardData() //异常二
        {
        //    fromMainModel.IsTongbuData = true;
            FileTcpClienModel.getFileTcpClienMidel().sendConjiWarnCardData();
        //    new Thread(setTongbuFalse).Start();
        }

        private void conjiWarnNODEData() //异常三
        {
        //    fromMainModel.IsTongbuData = true;
            FileTcpClienModel.getFileTcpClienMidel().sendConjiWarnNODEData();
         //   new Thread(setTongbuFalse).Start();
        }

        private void setTongbuFalse() 
        {
            Thread.Sleep(100);
            if (fromMainModel.IsTongbuData) 
            {
                fromMainModel.IsTongbuData = false;
                fromMainModel.sendTcpType();
            } 
        }

        private void button11_Click(object sender, EventArgs e) //同步資訊
        {
            new Thread(handTongBU).Start();
        }

        private void tongBuAllData() 
        {
            FileTcpClienModel.getFileTcpClienMidel().startTongbuData();
            Thread.Sleep(900);
            FileTcpClienModel.getFileTcpClienMidel().startFile();           
        }

        /// <summary>
        /// 手動同步資訊
        /// </summary>
        private void handTongBU() 
        {
            this.Invoke((EventHandler)(delegate
            { //放入主線程    
                if (!label8.Visible) label8.Visible = true;
                label8.Text = "正在同步中...";
            }));
            tongBuAllData();
            Thread.Sleep(200);
            this.Invoke((EventHandler)(delegate
            { //放入主線程    
                if (!label8.Visible) label8.Visible = true;
                label8.Text = "同步結束";
            }));
            Thread.Sleep(1000);
            this.Invoke((EventHandler)(delegate
            { //放入主線程    
                if (label8.Visible) label8.Visible = false;
            }));
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            FileTcpClienModel.getFileTcpClienMidel().startFile();         
        }


        private void button12_Click(object sender, EventArgs e)
        {
            new Thread(threadStartFile).Start();
        }

        private void threadStartFile() 
        {
            while(true)
            {
                 FileTcpClienModel.getFileTcpClienMidel().startFile();
                 Thread.Sleep(2000);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            XWUtils.ClearMemory();
        }

        private void appRestart() 
        {
            Thread.Sleep(5000);

            RestartBean resBean = new RestartBean();
            resBean.UDPopen = getcdInterfaceOpen();
            resBean.TCPopen = getcdInterface2Open();
            resBean.TCPClienopen = getcdInterface3Open();
            resBean.CardLowEleWarnMsgs = new Dictionary<string, DrivaceWarnMessage>(WarnMessage.getWarnMessage().CardLowEleWarnMsgs);
            resBean.CardUnanswerWranMsgs = new Dictionary<string, DrivaceWarnMessage>(WarnMessage.getWarnMessage().CardUnanswerWranMsgs);
            resBean.NODEUnanswerWranMsgs = new Dictionary<string, DrivaceWarnMessage>(WarnMessage.getWarnMessage().NODEUnanswerWranMsgs);
            resBean.PcType = PeoplePowerModel.getPeoplePowerModel().Jurisdiction == PeoplePowerModel.getPeoplePowerModel().CongjiValue ? 2 : 1;
            resBean.Count = loginForm.getAccount();
            resBean.Password = PeoplePowerModel.getPeoplePowerModel().Password;
            resBean.PowerValue = PeoplePowerModel.getPeoplePowerModel().PowerValue;
            resBean.Jurisdiction = PeoplePowerModel.getPeoplePowerModel().Jurisdiction;
            resBean.DealUpCardDatas = fromMainModel.DealUpCardDatas.ToList();
            resBean.RestartTime = XwDataUtils.GetTimeStamp();
            FileModel.getFlModel().setRestartData(resBean);
            new Thread(ApplicationRestart).Start();
        }


        private void ApplicationRestart() 
        {
            for (int i = 0; i < 100;i++ )
            {
                if (getcdInterfaceOpen()) UDPOpen(); ;
                if (getcdInterface2Open()) canshuButton19_Click(null, null);
                if (getcdInterface3Open()) canshuButton24_Click(null, null);
                else 
                {                    
                    break;
                }
                Thread.Sleep(500);
            }

            closeTimer(timer1);
            closeTimer(timer2);
            closeTimer(timer3);
            Thread.Sleep(500);

            if (!getcdInterface2Open()) System.Windows.Forms.Application.Restart();
            else FileModel.getFlModel().setErrorAppData(XwDataUtils.currentTimeToSe() + "=== Restart File+\r\n");
        }

        private void closeTimer(System.Windows.Forms.Timer timer) 
        {
            if (!timer.Enabled) return;
            timer.Enabled = false;
            timer.Dispose();
        }

        private void button12_Click_1(object sender, EventArgs e)
        {           
            new Thread(mesShow).Start();
            MessageBox.Show("1231");            
        }


        private void mesShow()
        {
            Thread.Sleep(2000);
            appRestart();
        }

        private void button12_Click_2(object sender, EventArgs e)
        {
            appRestart();
        }

        /*private void button12_Click(object sender, EventArgs e)
        {
            new Thread(su).Start();
        }

        private void su() 
        {
            int j = 0;
            int k = 1;
            int p = k / j;
        }*/

    }    
}
