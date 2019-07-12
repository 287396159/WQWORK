using CiXinLocation.bean;
using CiXinLocation.CiXinInterface;
using CiXinLocation.Model;
using CiXinLocation.Utils;
using SerialportSample;
using System;
using System.Collections;
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
    public partial class MainForm : FormBase, DrivaceSetInterface, FileUpdataInterface, LabelChangeInterface
    {
        private DrivaceSetOtherInterface dSetInterface;
        private FileUpdataModelInterface fUpdataInterface;
        private byte[] ID;
        private byte sunType = 0;
        private DrivaceType updataDTyp;
        private DrivaceType setDType;
        private int idPath_flag = -1;
        private readonly int IDLIST = 0;
        private readonly int PATHLIST = 1;
        private UInt32 fileLength = 0;
        FileUpdataModel fileModel;
        List<ID_Version> id_Versions = new List<ID_Version>();
        ListViewItem selectOne;
        DmatekFileModel dFileModel;
        private byte[] usb_readVersion;
        private byte[] usb_fileVersion;
        private Hashtable listIDtable;
        /// <summary>
        /// 卡片浏览按钮的编号，分别为1，2，3
        /// </summary>
        private int tagBtnIndex = 0;//
        private TagUpDataFlag tagUpFlag;

        public MainForm(){
            InitializeComponent();
            tagUpFlag = new TagUpDataFlag(3);//3 = Tag界面的选项卡的文件地址可选项。
            setDType = DrivaceType.NODE;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }       


        /////////////////////////////////////串口基本相關操作////////////////////////////


        private void button3_Click(object sender, EventArgs e) {
            /*btnClose(button3);
            label24.Visible = true;
            openCloseComm(comboBox1.Text);*/
            commRemoveCallBack();
        }

        public override void commRemoveCallBack()
        {
            btnClose(button3);
            label24.Visible = true;
            openCloseComm(comboBox1.Text);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            getCommPorts();
        }

        private void openCloseComm(string comPortName){//開關串口操作            
            if (comPortName.Length < 1){
                commOverCallBack(StringUtils.comNothing);
                return;
            }
            portName = comPortName;
            comOpenClose();
        }

        public override void commOverCallBack(string msg) { //串口打開按鈕點擊后，結果處理
            if (isOpenCom)
            {
                closeComBtn();
                createMainModel();
                findBtnOpen();
                if (dSetInterface != null) new Thread(cheak_USBdabgleThread).Start();
            } else {
                openComBtn();
                closeMainModel();
                findBtnClose();
                setBtnClose();
                clearTextBox();
                closeData();
            }
            btnOpen(button3);
            ctrlVisiClose();
            if (msg == null || msg.Length < 1 || "%!%".Equals(msg)) return;
            MessageBox.Show(StringUtils.commError+msg);
        }

        private void closeData() {
            usb_readVersion = null;
            usb_fileVersion = null;
            fileLength = 0;
            string fileInfo = "";
            string versionMessage = "";
            setTextForHexLabel(label16, fileInfo, label6, versionMessage);
            setTextForHexLabel(label17, fileInfo, label18, versionMessage);
            setTextForHexLabel(label19, fileInfo, label20, versionMessage);
            setTextForHexLabel(label28, fileInfo, label29, versionMessage);
        }

        private void cheak_USBdabgleThread() {
            dSetInterface.onCheckUSB_DANGLE(null);//查找USB_Dangle
        }

        private void clearTextBox() {
            textBox1.Clear();
            textBox10.Clear();
            textBox18.Clear();
            textBox27.Clear();
            textBox11.Clear();
            textBox20.Clear();
        }

        private void ctrlVisiOpen() {
            ctrlVisiOpen(label24);
        }
        private void ctrlVisiClose() {
            ctrlVisiClose(label24);
        }
        private void ctrlVisiOpen(Control ctrol) {
            if (ctrol.Visible != true) ctrol.Visible = true;
        }
        private void ctrlVisiClose(Control ctrol)
        {
            if (ctrol.Visible != false) ctrol.Visible = false;
        }

        //打开搜索按钮
        private void findBtnOpen() {
            btnOpen(button1);
            btnOpen(button25); 
            btnOpen(button38);
            btnOpen(button36);
            btnOpen(button45);
            btnOpen(button46);
            refeshBtnClose();
        }

        private void refeshBtnClose() {
            btnClose(button14); //关闭刷新
        }

        private void btnOpen(Button button) {
            if (!button.Enabled) button.Enabled = true;
        }

        //关闭搜索按钮
        private void findBtnClose() {
            btnClose(button1);
            btnClose(button25);
            btnClose(button38);
            btnClose(button36);            
            refeshBtnOpen();   
        }

        private void setBtnClose() {
            btnClose(button5);
            btnClose(button6);
            btnClose(button7);
            btnClose(button8);
            btnClose(button9);
            btnClose(button10);
            btnClose(button11);
            btnClose(button12);
            btnClose(button13);
            btnClose(button19);
            btnClose(button30);
            btnClose(button31);
            btnClose(button33);
            btnClose(button34);
            btnClose(button22);         
            btnClose(button27);
            btnClose(button28);
            btnClose(button29);
            btnClose(button32);
            btnClose(button41);
            btnClose(button42);
            btnClose(button35);
            btnClose(button40);
            btnClose(button43); 

            btnClose(button26);
            btnClose(button20);
            btnClose(button15);
            btnClose(button16);
        }

        private void refeshBtnOpen() {
            btnOpen(button14);//打开刷新
        }

        private void btnClose(Button button)
        {
            if (button.Enabled) button.Enabled = false;
        }

        private void createMainModel() {
            MainFromModel mfModel = new MainFromModel(this);
            dSetInterface = mfModel;
            createFromBaseModel(mfModel);
        }

        private void closeMainModel() {
            if (dSetInterface != null) dSetInterface = null;
            if (fUpdataInterface != null) fUpdataInterface = null;
            closeFromBaseModel();
        }

        private void closeComBtn()  { //串口按鈕關閉
            closeComBtn(button3);
        }

        private void closeComBtn(Button btn) {
            if (btn.Text != StringUtils.close){
                btn.Text = StringUtils.close;
                btn.FlatStyle = FlatStyle.Popup;
            }
        }

        private void comBoxTextSet() {
            int index = tabControl1.SelectedIndex;
            //if (0 == index) comBoxTextSet(comboBox1, comboBox2, comboBox1);
            //if (1 == index) comBoxTextSet(comboBox2, comboBox1, comboBox3);
            //if (1 == index) comBoxTextSet(comboBox1, comboBox2, comboBox1);
        }

        private void comBoxTextSet(ComboBox sourceBox, ComboBox dexBox1, ComboBox dexBox2){
            dexBox1.Text = sourceBox.Text;
            dexBox2.Text = sourceBox.Text;
        }

        private void openComBtn()
        {//串口按鈕打開
            openComBtn(button3);
        }

        private void openComBtn(Button btn){
            if (btn.Text != StringUtils.open){
                btn.Text = StringUtils.open;
                btn.FlatStyle = FlatStyle.Standard;
            }
        }

        public override void commPortsCallBack(string[] msg) {
            if (msg == null || msg.Length < 1) return;
            comBoxItemsAdd(comboBox1,msg);
        }

        private void comBoxItemsAdd(ComboBox comboBox, string[] ports) {
            comboBox.Items.Clear();
            comboBox.Items.AddRange(ports);
            comboBox.SelectedIndex = comboBox.Items.Count > 0 ? 0 : -1;
        }


////////////////////////////////////////设置模块/////////////////////////////////////////////////////////

        private void setTextToMain(TextBox textBox,string text) {
            this.Invoke((EventHandler)(delegate
            { //放入主線程
                textBox.Text =  text;
            }));  
        }

        private void addVersion(DrivaceType drivaceType, byte[] ID, byte[] version, byte type)
        {
            addID_Version(drivaceType,ID);
            if (dFileModel != null) dFileModel.writeVersion(drivaceType, ID, version, type);            
            if (setDType != drivaceType) return;
            addVersionMain(drivaceType, ID, version,type);                         
        }

        private void addVersionMain(DrivaceType drivaceType, byte[] ID, byte[] version, byte type)
        {
            if (id_Versions == null) return;
            ID_Version id_Version = new ID_Version(ID);
            id_Version.DrivaceType = drivaceType;
            id_Version.typeDrivaceType = drivaceType;            
            foreach (ID_Version id_VersionItem in id_Versions)
            {
                if (id_Version.idEqulse(id_VersionItem))
                {
                    id_VersionItem.Version = version;
                    if (id_VersionItem.Type != null) id_VersionItem.Type[0] = type;
                    changeVersion(getListView(drivaceType, 0), id_VersionItem);
                    return;
                }
            }
        }

        /// <summary>
        /// 修改ID列表的版本信息
        /// </summary>
        /// <param name="lView">要修改的</param>
        /// <param name="id_Version"></param>
        private void changeVersion(ListView lView, ID_Version id_Version)
        {
            this.Invoke((EventHandler)(delegate
            { //放入主線程            
                if (id_Version == null || id_Version.Version == null || id_Version.Version.Length != 4) return;
                foreach (ListViewItem lvItem in lView.Items)
                {
                    if (lvItem.SubItems[0].Text.Equals(id_Version.IDtostring())) {
                        lvItem.SubItems[1].Text = id_Version.getVersion() + id_Version.getSunType;
                        return;
                    }
                }
            }));  
        }

        /// <summary>
        /// 返回listView的对象 listViewTipe = 0 ID列表   listViewTipe = 0 地址列表
        /// </summary>
        /// <param name="driType"> 设备类别 </param>
        /// <param name="listViewTipe"> ID列表或者地址列表 </param>
        /// <returns></returns>
        private ListView getListView(DrivaceType driType,int listViewTipe){
            ListView lView = null;
            switch (driType)
            {
                case DrivaceType.NODE://ID_Version.NODE:
                    if (listViewTipe == 0) lView = listView1;
                    if (listViewTipe == 1) lView = listView6;
                    break;
                case DrivaceType.LOCATION://ID_Version.LOCATION:                   
                    if (listViewTipe == 0) lView = listView2;
                    if (listViewTipe == 1) lView = listView5;
                    break;
                case DrivaceType.TAG://ID_Version.TAG:
                    if (listViewTipe == 0) lView = listView3;
                    break;
                default:
                    break;
            }
            return lView;
        }

        private void addID_Version(DrivaceType drivaceType, byte[] ID)
        {
            if (drivaceType != setDType) return;

            ID_Version id_Version = new ID_Version(ID);
            id_Version.DrivaceType = drivaceType;
            id_Version.typeDrivaceType = drivaceType;

            if (id_Versions == null) id_Versions = new List<ID_Version>();
            for (int i = 0; i < id_Versions.Count; i++)
            {
                if (id_Versions[i].idEqulse(id_Version)) {
              //      new Thread(checkVersion).Start();
                    return;
                } 
            }
            ID_Version versionBt = null;
            if (dFileModel != null)  versionBt = dFileModel.getVersion(drivaceType, ID);
              
            lock (obj)
            {
                id_Versions.Add(id_Version);
            }
            this.Invoke((EventHandler)(delegate{ //放入主線程
                if (setDType != drivaceType) return;
                addID_VersionMain(drivaceType, id_Version);                
            }));
            if (versionBt != null) changeVersion(getListView(drivaceType, 0), versionBt);
            //new Thread(checkVersion).Start();
        }


        private void addID_VersionMain(DrivaceType drivaceType, ID_Version id_Version)
        {           
            ListViewItem newIDitem = id_Version.getlvItem();
            id_Version = null;
            if (newIDitem != null) getListView(drivaceType,0).Items.Add(newIDitem);
        }

        object obj = new object();
        bool isCheckVersion = false;
        private void checkVersion() {
            if (isCheckVersion) return;
            isCheckVersion = true;
            if (id_Versions == null) return;
            lock (obj)
            {
                foreach (ID_Version id_versionItem in id_Versions)
                {
                    if (id_versionItem.Version != null) continue;
                    checkVersion(id_versionItem.DrivaceType, id_versionItem.Id);
                    try{
                        Thread.Sleep(50);
                    }
                    catch { }
                }
            }           
            isCheckVersion = false;
        }

        public void onCheckVersionBack() 
        {
            checkVersion();
        }

        private void checkVersion(DrivaceType drivaceType,byte[] ID) { 
            if (dSetInterface == null) return;
            if (setDType == DrivaceType.NODE) dSetInterface.onCheckJieDianVersion(ID, 0x00,null);
            if (setDType == DrivaceType.LOCATION) dSetInterface.onCheckCanKaoDianVersion(ID,0x00, null);
            if (setDType == DrivaceType.TAG) dSetInterface.onCheckTagVersion(ID, 0x00,null);
        }      

        public void onCheckJieDianID(byte[] ID){
            addID_Version(DrivaceType.NODE, ID);
        }

        public void onCheckUSB_DANGLE(byte[] version)
        {
            if (version == null || version.Length != 4) return;
            string msg = CiXinUtils.getVersion(version);
            usb_readVersion = version;
            mainThreadSetText(label30, msg);           
        }

        /// <summary>
        /// 开始USB_Dangle的更新
        /// </summary>
        void startUSB_DangleFileUpdata(){
            if (usb_readVersion == null || usb_fileVersion == null) return;
            if (XWUtils.byteBTBettow(usb_readVersion, usb_fileVersion, 4)) btnClose(button19);
            else btnOpen(button19);
        }

        private void mainThreadSetText(Label lab, string text)
        {
            this.Invoke((EventHandler)(delegate
            { //放入主線程
                setUpdataResultLabel(label30, StringUtils.USB_DangleInfor + text);
                startUSB_DangleFileUpdata();
            }));              
        }

        public void onCheckJieDianVersion(byte[] ID,byte type,byte[] version)
        {
            addVersion(DrivaceType.NODE, ID, version, type);
        }

        /// <summary>
        /// 成功返回结果显示
        /// </summary>
        private void retuViewVisi(byte[] ID,DataPacketType dpType, Label lab)
        {
            if (!XWUtils.byteBTBettow(ID, this.ID, this.ID.Length)) return;
            DP_TYPE_LABEL dpLabel = new DP_TYPE_LABEL(dpType, lab);
            SetDataResuleView.getInstance().returnSuccess(dpLabel);
        }

        public void onSetJieDianServiseIP(byte[] ID, byte[] IP) {

            retuViewVisi(ID,DataPacketType.SET_SERVISE_IP, label33);
        }


        public void onReadJieDianServiseIP(byte[] ID, byte[] IP)
        {
            setTextIp(ID, IP, textBox5);
        }

        public void onSetJieDianServisePort(byte[] ID, byte[] port) {

            retuViewVisi(ID,DataPacketType.SET_SERVISE_PORT, label34);
        }

        public void onReadJieDianServisePort(byte[] ID, byte[] port)
        {
            if (this.ID == null) return;
            if (ID[0] != this.ID[0] || ID[1] != this.ID[1]) return;
            setTextToMain(textBox6, (port[0] *256 + port[1]).ToString());
        }

        public void onSetJieDianWifiName(byte[] ID, byte[] name) {
            retuViewVisi(ID,DataPacketType.SET_WIFI_NAME, label35);
        }

        public void onReadJieDianWifiName(byte[] ID, byte[] name)
        {
            if (this.ID == null) return;
            if (ID[0] != this.ID[0] || ID[1] != this.ID[1]) return;
            string wifiName = System.Text.Encoding.ASCII.GetString(name);
            setTextToMain(textBox7, wifiName);
        }

        public void onSetJieDianWifiPassWord(byte[] ID, byte[] password){
            retuViewVisi(ID, DataPacketType.SET_WIFI_PASSWORD, label36);
        }

        public void onReadJieDianWifiPassWord(byte[] ID, byte[] password)
        {
            if (this.ID == null) return;
            if (ID[0] != this.ID[0] || ID[1] != this.ID[1]) return;
            string wifiPassword = System.Text.Encoding.ASCII.GetString(password);
            setTextToMain(textBox8, wifiPassword);
        }
        public void onCheckCanKaoDianID(byte[] ID)
        {
            addID_Version(DrivaceType.LOCATION, ID);
        }
        public void onCheckCanKaoDianVersion(byte[] ID, byte type, byte[] version)
        {
            addVersion(DrivaceType.LOCATION, ID, version, type);
        }
        public void onCheckTagID(byte[] ID)
        {
             addID_Version(DrivaceType.TAG, ID);         
        }
        public void onCheckTagVersion(byte[] ID, byte type, byte[] version)
        {
            addVersion(DrivaceType.TAG, ID, version, type);
        }
        public void onSetTagUpTime(byte[] ID, byte[] upTime) {
            retuViewVisi(ID, DataPacketType.SET_CARD_UPTIME, label41);
        }
        public void onSetTagUpTime(byte[] ID, byte upTime)
        {
            retuViewVisi(ID, DataPacketType.SET_CARD_UPTIME, label41);
        }

        public void onReadTagUpTime(byte[] ID, byte[] upTime)
        {
            if (this.ID == null) return;
            if (ID[0] != this.ID[0] || ID[1] != this.ID[1]) return;
            setTextToMain(textBox23, (upTime[0]*256 + upTime[1]).ToString());
        }
        public void onReadTagUpTime(byte[] ID, byte upTime)
        {
            if (this.ID == null) return;
            if (ID[0] != this.ID[0] || ID[1] != this.ID[1]) return;
            setTextToMain(textBox23, upTime.ToString());
        }

        public void onSetTagRF(byte[] ID, byte RF){
            retuViewVisi(ID, DataPacketType.SET_CARD_POWER, label42);
        }

        public void onReadTagRF(byte[] ID, byte RF)
        {
            if (this.ID == null) return;
            if (ID[0] != this.ID[0] || ID[1] != this.ID[1]) return;
            setTextToMain(textBox22, RF.ToString());
        }

        public void onNODEfuwei(byte[] ID) {
            retuViewVisi(ID, DataPacketType.RESET, label46);
        }

        public void onSetNODEmodel(byte[] ID, byte model) {
            retuViewVisi(ID,DataPacketType.SET_NODE_MODEL, label37);
        }

        public void onReadNODEmodel(byte[] ID, byte model) {
            if (model == 0) setTextToMain(textBox13, StringUtils.staticModel);
            if (model == 1) setTextToMain(textBox13, StringUtils.dynmicModel);
        }

        public void onSetNODE_IP(byte[] ID, byte[] IP) {
            retuViewVisi(ID,DataPacketType.SET_NODE_IP, label38);
        }

        public void onReadNODE_IP(byte[] ID, byte[] IP) {
            setTextIp(ID, IP, textBox14);
        }

        private void setTextIp(byte[] ID, byte[] IP, TextBox textBox)
        {
            if (this.ID == null) return;
            if (ID[0] != this.ID[0] || ID[1] != this.ID[1]) return;
            setTextToMain(textBox, IP[0] + "." + IP[1] + "." + IP[2] + "." + IP[3]);
        }

        public void onSetNODESubMask(byte[] ID, byte[] IP) {
            retuViewVisi(ID,DataPacketType.SET_SUBMASK, label39);
        }

        public void onReadNODESubMask(byte[] ID, byte[] IP) {
            setTextIp(ID, IP, textBox15);
        }

        public void onSetNODEGateWay(byte[] ID, byte[] IP) {
            retuViewVisi(ID,DataPacketType.SET_GATEWAY, label40);
        }

        public void onReadNODEGateWay(byte[] ID, byte[] IP) {
            setTextIp(ID, IP, textBox16);
        }

        public void onSet_XinHaoQiangdu_(byte[] ID, byte Threshold)
        {
            retuViewVisi(ID, DataPacketType.SET_CARD_XinHaoQiangdu, label15);
        }

        public void onRead_XinHaoQiangdu_(byte[] ID, byte Threshold)
        {
            if (this.ID == null) return;
            if (ID[0] != this.ID[0] || ID[1] != this.ID[1]) return;
            setTextToMain(textBox25, Threshold.ToString());
        }

        public void onSet_XinHaoQiangduXiShu_(byte[] ID, byte k1)
        {
            retuViewVisi(ID, DataPacketType.SET_CARD_XinHaoQiangduXiShu, label10);
        }

        public void onRead_XinHaoQiangduXiShu(byte[] ID, byte k1)
        {
            if (this.ID == null) return;
            if (ID[0] != this.ID[0] || ID[1] != this.ID[1]) return;
            int kDoubleTen = k1 / 100;
            int kDoubleDian = k1 % 100;
            StringBuilder sDer = new StringBuilder();
            sDer.Append(kDoubleTen.ToString(""));
            sDer.Append(".");
            sDer.Append(kDoubleDian.ToString(""));
            setTextToMain(textBox28, sDer.ToString());
        }


        /// <summary>
        /// USB_dANGLE更新数据回调
        /// </summary>
        /// <param name="len"></param>
        /// <param name="CheckSum"></param>
        public void askUSB_dangleUpData(){ }

        private void button1_Click(object sender, EventArgs e) //节点搜索
        {
            setDType = DrivaceType.NODE;
            if (dSetInterface != null) dSetInterface.onCheckJieDianID(null);
        }

        private void button6_Click(object sender, EventArgs e) { //设置IP
            byte[] ip = XWUtils.getIP4byte(textBox2.Text);
            if (IP_IDNULL(ID, ip)) return;
            setViewVisi(DataPacketType.SET_SERVISE_IP, label33);
            if (IDnullMessage(ID, textBox5)) return;
            dSetInterface.onSetJieDianServiseIP(ID, ip);
            //threadSleep(sleep_sendReadData);          
            //dSetInterface.onReadJieDianServiseIP(ID, null);           
        }

        /// <summary>
        /// 设置结果显示
        /// </summary>
        private void setViewVisi(DataPacketType dpType, Label lab)
        {
            DP_TYPE_LABEL dpLabel = new DP_TYPE_LABEL(dpType, lab);
            SetDataResuleView.getInstance().insertSet(dpLabel);
        }

        /// <summary>
        /// IP类的判空处理。主要是对接口对象，ID和IP进行判空，并且提示给用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        private bool IP_IDNULL(byte[] id, byte[] ip)
        {
            if (IDnullMessage(id)) return true;
            if (IPnullMessage(ip)) return true;
            return false;
        }

        private bool IPnullMessage(byte[] ip) {
            if (ip == null)
            {
                MessageBox.Show(StringUtils.errorIP);
                return true;
            }
            return false;
        }

        /// <summary>
        /// ID是否为空，并且显示一个MessageBox，作为提示,并且接口也不能为空
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true为空，false不为空</returns>
        private bool IDnullMessage(byte[] id,TextBox tbox) {
            
            if (dSetInterface == null) return false;
            return IDnullMessage(id, StringUtils.errorID, tbox);
        }
        private bool IDnullMessage(byte[] id)
        {
            return IDnullMessage(id,null);
        }


        private bool IDnullMessage(byte[] id, string message, TextBox tbox)
        {
            if (tbox != null) tbox.Text = "";
            if (id == null)
            {
                MessageBox.Show(message);
                return true;
            }
            return false;
        }

        /// <summary>
        /// port类的判空处理。主要是对接口对象，ID和IP进行判空，并且提示给用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        private bool Port_IDNULL(byte[] id, byte[] port)
        {
            if (IDnullMessage(id)) return true;
            if (PortNullMessage(port)) return true;
            return false;
        }

        /// <summary>
        /// port是否为空，并且显示一个MessageBox，作为提示
        /// </summary>
        /// <param name="port"></param>
        /// <returns>true为空，false不为空</returns>
        private bool PortNullMessage(byte[] port) {
            return PortNullMessage(port, StringUtils.errorPort);
        }
        private bool PortNullMessage(byte[] port,string msg)
        {
            if (port == null)
            {
                MessageBox.Show(msg);
                return true;
            }
            return false;
        }

        private void button9_Click(object sender, EventArgs e) { //去读IP
            if (IDnullMessage(ID,textBox5)) return;
            dSetInterface.onReadJieDianServiseIP(ID, null);
        }


        private void button7_Click(object sender, EventArgs e) {          
            byte[] port = XWUtils.getComTime(textBox3.Text);
            if(Port_IDNULL(ID,port))return;
            if (IDnullMessage(ID, textBox6)) return;
            setViewVisi(DataPacketType.SET_SERVISE_PORT,label34);                       
            dSetInterface.onSetJieDianServisePort(ID,port);
        }


        private void button10_Click(object sender, EventArgs e){
            if (IDnullMessage(ID,textBox6)) return;
            dSetInterface.onReadJieDianServisePort(ID, null);
        }


        private void button8_Click(object sender, EventArgs e) {
            if (IDnullMessage(ID)) return;
            byte[] sendWifi = getWifi(textBox4.Text);
            if (sendWifi == null) return;
            if (IDnullMessage(ID, textBox7)) return;
            setViewVisi(DataPacketType.SET_WIFI_NAME, label35);                      
            dSetInterface.onSetJieDianWifiName(ID, sendWifi);
        }


        private byte[] getWifi(string data) {
            byte[] sendWifi = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                sendWifi[i] = 0;
            } try
            {
                byte[] wifiByte = System.Text.Encoding.ASCII.GetBytes(data);
                if (wifiByte.Length > 32)
                {
                    MessageBox.Show(StringUtils.wifiLength);
                    return null;
                }
                Array.Copy(wifiByte, 0, sendWifi, 0, wifiByte.Length);
            } catch
            {
                return null;
            }
            return sendWifi;
        }

        private void button11_Click(object sender, EventArgs e) {
            if (IDnullMessage(ID,textBox7)) return;           
            dSetInterface.onReadJieDianWifiName(ID,null);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (IDnullMessage(ID)) return;
            byte[] sendWifi = getWifi(textBox9.Text);
            if (sendWifi == null) return;
            if (IDnullMessage(ID, textBox8)) return;            
            setViewVisi(DataPacketType.SET_WIFI_PASSWORD, label36);
            dSetInterface.onSetJieDianWifiPassWord(ID,sendWifi);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (IDnullMessage(ID,textBox8)) return;
            dSetInterface.onReadJieDianWifiPassWord(ID,null);
        }

        private void button25_Click(object sender, EventArgs e) //参考点搜索
        {
            setDType = DrivaceType.LOCATION;
            if (dSetInterface != null) dSetInterface.onCheckCanKaoDianID(null);
        }

        private void button38_Click(object sender, EventArgs e) 
        {
            setDType = DrivaceType.TAG;
            if (dSetInterface != null) dSetInterface.onCheckTagID(null);
        }

        private void button34_Click(object sender, EventArgs e)
        {
            //byte[] upTime = XWUtils.getComTime(textBox21.Text); //检测等待时间
            //if (Port_IDNULL(ID, upTime)) return;

            int dTime = XWUtils.stringToInt1(textBox26.Text);
            if (dTime > 255 || dTime < 1) {
                MessageBox.Show(StringUtils.errorPort);
                return;
            }
            //if (Port_IDNULL(ID, upTime)) return;
            //if (IDnullMessage(ID, textBox23)) return;           
            setViewVisi(DataPacketType.SET_CARD_UPTIME, label41);
            dSetInterface.onSetTagUpTime(ID, (byte)dTime);
        }


        private void button31_Click(object sender, EventArgs e)
        {
            if (IDnullMessage(ID, textBox23)) return;            
            dSetInterface.onReadTagUpTime(ID,null);
        }


        private void button33_Click(object sender, EventArgs e)
        {            
            int RFbt = (byte)XWUtils.stringToInt1(comboBox4.Text);
            if (RFbt == -1 || IDnullMessage(ID)) return;
            if (IDnullMessage(ID, textBox22)) return;           
            setViewVisi(DataPacketType.SET_CARD_POWER, label42);
            dSetInterface.onSetTagRF(ID,(byte)RFbt);
        }


        private void button30_Click(object sender, EventArgs e){
            if (IDnullMessage(ID, textBox22)) return;
            dSetInterface.onReadTagRF(ID,0);
        }

        
        private void listView1_SelectedIndexChanged(object sender, EventArgs e) {
            idPath_flag = 0;
            if (listView1.SelectedItems.Count <= 0  )  return;           
            try {
                selectOne = listView1.SelectedItems[0];
                if (!button1.Enabled) return;       
                string selectMsg = selectOne.SubItems[0].Text;
                ID = XWUtils.getByteID(selectMsg);
                string tpBtStr = XWUtils.getSplTypeEnd(selectOne.SubItems[1].Text,'-');
                sunType = CiXinUtils.getSunType("-"+tpBtStr);
                if (ID == null) return;
                
                label3.Text = "0%";
                label4.Text = StringUtils.changeDrivace + ":" + ID[0].ToString("X2") + ID[1].ToString("X2") + " " + tpBtStr;
                label5.Text = StringUtils.currentDrivace + ":" + ID[0].ToString("X2") + ID[1].ToString("X2") + " " + tpBtStr;
                if (progressBar1.Value != 0) progressBar1.Value = 0;
                if (button5.Enabled) {
                    setBtnOpen(sunType);
                    readAllcanShu(DrivaceType.NODE, ID);
                    return;
                } 
                setBtnClose();
                setBtnOpen(sunType);
                setBouthOpen();
                readAllcanShu(DrivaceType.NODE, ID);
            }
            catch { }           
        }

        /// <summary>
        ///  一键读取所有参数
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ID"></param>
        private void readAllcanShu(DrivaceType dt,byte[] ID) {
            return;
            clearReadTextBox();
            if (dSetInterface != null) dSetInterface.readAllParameter(dt, ID);
        }

        /// <summary>
        /// 清空读取框中数值
        /// </summary>
        private void clearReadTextBox() {
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";

            textBox13.Text = "";
            textBox14.Text = "";
            textBox15.Text = "";
            textBox16.Text = "";
            textBox25.Text = "";
            textBox28.Text = "";
        }

        /// <summary>
        /// 根据不同子类设备类型，打开和关闭读取按钮
        /// </summary>
        /// <param name="subType"></param>
        private void setBtnOpen(byte subType) {
            if (subType == 1) {
                setBtnLanOpene();
                closeBtnWifiPa();
            } else if (subType == 2) {
                setBtnWifiPaOpen();
                setBtnLanClose();
            }          
        }
        private void setBouthOpen() {
            btnOpen(button5);
            btnOpen(button6);
            btnOpen(button7);
            btnOpen(button9);
            btnOpen(button10);
            btnOpen(button43);
            btnOpen(button26);
            btnOpen(button20);
            btnOpen(button15);
            btnOpen(button16);
        }
        private void setBtnWifiPaOpen() {
            btnOpen(button27);
            btnOpen(button28);
            btnOpen(button29);
            btnOpen(button32);
            btnOpen(button41);
            btnOpen(button42);
            btnOpen(button35);
            btnOpen(button40);            
        }
        private void closeBtnWifiPa() {
            btnClose(button27);
            btnClose(button28);
            btnClose(button29);
            btnClose(button32);
            btnClose(button41);
            btnClose(button42);
            btnClose(button35);
            btnClose(button40);
        }

        private void setBtnLanOpene()
        {
            btnOpen(button8);
            btnOpen(button11);
            btnOpen(button12);
            btnOpen(button13);
        }

        private void setBtnLanClose()
        {
            btnClose(button8);
            btnClose(button11);
            btnClose(button12);
            btnClose(button13);
        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e) {
            idPath_flag = 0;
            if (listView3.SelectedItems.Count <= 0 ) return;
            try {
                selectOne = listView3.SelectedItems[0];
                if (!button38.Enabled) return;
                string selectMsg = selectOne.SubItems[0].Text;

                string tpBtStr = XWUtils.getSplTypeEnd(selectOne.SubItems[1].Text, '-');

                ID = XWUtils.getByteID(selectMsg);
                if (ID == null) return;
               
                label11.Text = StringUtils.currentDrivace + ":" + ID[0].ToString("X2") + ID[1].ToString("X2") + " " + tpBtStr;

                if (button30.Enabled) {
                    readAllcanShu(DrivaceType.TAG, ID);
                    return;
                } 
                setBtnClose();
                settAGBtnOpen();
                readAllcanShu(DrivaceType.TAG, ID);
            }
            catch { }    
        }

        private void settAGBtnOpen() {            
            btnOpen(button30);
            btnOpen(button31);
            btnOpen(button33);
            btnOpen(button34);
        }
 
        private void listView2_SelectedIndexChanged(object sender, EventArgs e) {
            idPath_flag = 0;
            if (listView2.SelectedItems.Count <= 0 ) return;
            try {
                selectOne = listView2.SelectedItems[0];
                if (!button25.Enabled) return;
                string selectMsg = selectOne.SubItems[0].Text;
                byte[]  Id = XWUtils.getByteID(selectMsg);

                string tpBtStr = XWUtils.getSplTypeEnd(selectOne.SubItems[1].Text, '-');
                sunType = CiXinUtils.getSunType("-" + tpBtStr);

                if (Id == null) return;
                if (ID != null && Id[0] == ID[0] && Id[1] == ID[1]) return;
                ID = Id;
                label8.Text = "0%";
                label7.Text = StringUtils.changeDrivace + ":" + ID[0].ToString("X2") + ID[1].ToString("X2") + " " + tpBtStr;
                if (progressBar2.Value != 0) progressBar2.Value = 0;

                if (button22.Enabled) return;
                setBtnClose();
                setCanKaoDianBtnOpen();
            }
            catch { }    
        }

        private void setCanKaoDianBtnOpen() {
            label7.Text = StringUtils.changeDrivace + ":" + ID[0].ToString("X2") + ID[1].ToString("X2");

            btnOpen(button22);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label4.Text = StringUtils.changeDrivace + ":" ;
            clearBtnDeal(DrivaceType.NODE, 0, true);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            clearBtnDeal(DrivaceType.LOCATION, 0, true);
        }

        private void button37_Click(object sender, EventArgs e)
        {
            clearBtnDeal(DrivaceType.TAG, 0, true);
        }

        private void clearBtnDeal(DrivaceType dtype, int lvTip,bool isSet)
        {
            clearBtnDeal(dtype,lvTip);
            if (!isSet) return;
            if (dtype == DrivaceType.NODE) label5.Text = StringUtils.currentDrivace + ":";
            if (dtype == DrivaceType.LOCATION) label7.Text = StringUtils.changeDrivace + ":";
            if (dtype == DrivaceType.TAG) label11.Text = StringUtils.currentDrivace + ":";         
        }

        private void clearBtnDeal(DrivaceType dtype, int lvTip)
        {
            try {
                getListView(dtype, lvTip).Items.Clear();//getType()
                if (lvTip == 0) clearListBtn(dtype);
            }
            catch { }           
        }

        private void clearListBtn(DrivaceType dtype) {
            List<ID_Version> deleteId_Versions = new List<ID_Version>();
            foreach (ID_Version idVerItem in id_Versions)
            {
                if (idVerItem.typeDrivaceType == dtype) deleteId_Versions.Add(idVerItem);
            }
            if (deleteId_Versions.Count > 0) {
                lock(obj){
                    foreach (ID_Version idVerItem in deleteId_Versions)
                    {
                        id_Versions.Remove(idVerItem);
                    }
                }                
            }
            setBtnClose();
            if (listIDtable == null) return;
            listIDtable.Clear();
        }
        
        private void MainForm_Load(object sender, EventArgs e){
             //dFileModel = new DmatekFileModel();
             StringUtils.PrintCtrlName(this);
             DefaultState();
             Disposed += OnRecordingPanelDisposed;
             //tabControl1.TabPages.Remove(tabPage2);//隐藏掉选项卡第二项，参考点
             tabPage2.Parent = null;
             tabPage4.Parent = null;
        }

        void OnRecordingPanelDisposed(object sender, EventArgs e)
        {
            SetDataResuleView.getInstance().clear();
        }

        /// <summary>
        /// 默认设置一些值
        /// </summary>
        private void DefaultState() {
            string ip4 = XWUtils.GetAddressIP();
            textBox2.Text = ip4;
            SetDataResuleView.getInstance().setLanINterface(this);
        }

////////////////////////////////////////////固件更新模块////////////////////////////////////////////////////////////

        
        public DrivaceType getType()
        {
            if (tabControl1 == null) return DrivaceType.NODRIVACE;
            int select_index = tabControl1.SelectedIndex;
            if (select_index == 0) return DrivaceType.NODE;
            else if (select_index == 1) return DrivaceType.TAG;///去掉了Location,所以就改成卡片了
            else if (select_index == 2) return DrivaceType.TAG;
            else return DrivaceType.NODRIVACE;
        }

        public byte sunDeviceType()
        {
            return 0xff;
        }

        public void fileInformation(HexFileBean hfInfor)
        {
            if (hfInfor == null) return;
            fileLength = hfInfor.FileLength;
            if (setFUpdataInterface(hfInfor)) {
                setTextBtnForHexInfor(hfInfor.FileInfor, hfInfor.VersionMessage);        
                if (addFromBaseModel(fileModel))
                    fUpdataInterface = fileModel;  
            }            
           if (updataDTyp == DrivaceType.USB_DANGLE)
           {
               if (hfInfor.mainDrivaceType != DrivaceType.USB_DANGLE) 
                    MessageBox.Show(StringUtils.hexFileNoType);
                else {
                    usb_fileVersion = hfInfor.TimeVersion;
                    pathSet(textBox10, hfInfor.Path); //tb.Text = path;
                    startUSB_DangleFileUpdata();
                    setTextForHexLabel(label28, hfInfor.FileInfor, label29, hfInfor.VersionMessage);
                    if (addFromBaseModel(fileModel))
                        fUpdataInterface = fileModel;  
                }              
                updataDTyp = DrivaceType.NODRIVACE;
            }                                       
        }        

        /// <summary>
        /// 浏览完目录后，将文件添加到文件列表
        /// </summary>
        /// <param name="hfInfor"></param>
        private bool setFUpdataInterface(HexFileBean hfInfor) { 
            
            if (updataDTyp == DrivaceType.USB_DANGLE) return false;
            DrivaceType dt = fUpdataInterface.getType();
            if (setDType != dt) {
                MessageBox.Show(StringUtils.hexFileNoType);
                return false;
            }
            if (setDType == DrivaceType.NODE){
                pathSet(textBox1, hfInfor.Path); //textBox1.Text = path;
                return true;
            } 
            else if (setDType == DrivaceType.LOCATION){
                pathSet(textBox18, hfInfor.Path); //tb.Text = path;
                return true;
            } 
            else if (dt == DrivaceType.TAG) {
                byte sunType = fUpdataInterface.sunDeviceType();
                tagPathSet(hfInfor.Path, tagBtnIndex, sunType);
                return true;
            }
            return false;
        }


        private void listViewADDChange(DrivaceType dt,HexFileBean hfInfor)
        {
            ListView lview = getListView(dt, 1);
            if (lview == null) return;
            int lviewCount = lview.Items.Count;
            for (int i = 0; i < lviewCount; i++) {
                ListViewItem lItem = lview.Items[i];
                string pathItem = getPath(lItem);
                FileUpdataModel fuModel;
                try { 
                    fuModel = (FileUpdataModel)getFromBaseModel(pathItem);
                } catch {
                    continue;
                }
                if (fuModel == null) continue;
                if (hfInfor.mainDrivaceType != fuModel.HfModelInfor.mainDrivaceType || hfInfor.Index != fuModel.HfModelInfor.Index) continue;
                closeFromBaseModel(fuModel);
                lview.Items.Remove(lItem);
                lview.Items.Add(hfInfor.getlvItem());
                return;
            }
            getListView(dt, 1).Items.Add(hfInfor.getlvItem());
        }


        private void tagPathSet(TextBox tb, List<TextBox> tbPath, string path, byte sunType)
        {
            if (tb == null) return;
            for (int i = 0; i < tbPath.Count;i++ )
            {
                TextBox tbItem = tbPath[i];
                if (tbItem == null) continue;
                if (tbItem.Text.Length < 1 ) continue;
                if ((i + 1) == tagBtnIndex && tb.Text.Equals(tbItem.Text)) continue;
                FileUpdataInterface fileUpdataInter = getFileUpdataBaseModel(tbItem.Text);
                if (fileUpdataInter == null || fileUpdataInter.getType() != DrivaceType.TAG) continue;
                int textBoxSunType = getSunByte(fileUpdataInter);
                if (textBoxSunType == -1) continue;
                if (sunType == textBoxSunType )
                {
                    messageBoxShow(StringUtils.addHandle + (i + 1) + StringUtils.addressWei);                 
                    return;
                 }
            }
            try {
                pathSet(tb, path);
            }
            catch { }            
        }

        /// <summary>
        /// messageBox样式显示，点击确定返回True,否则False
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool messageBoxShow(string msg) {
            DialogResult MsgBoxResult;
            MsgBoxResult = MessageBox.Show(msg,//对话框的显示内容
            StringUtils.prompt,//对话框的标题
            MessageBoxButtons.YesNo,//定义对话框的按钮，这里定义了YSE和NO两个按钮
            MessageBoxIcon.Exclamation,//定义对话框内的图表式样，这里是一个黄色三角型内加一个感叹号
            MessageBoxDefaultButton.Button2);//定义对话框的按钮式样);
            if (MsgBoxResult == DialogResult.Yes) return true;
            return false;
        }


        private void tagPathSet(string path, int index, byte sunType)
        {
            List<TextBox> tbPath = new List<TextBox>();
            tbPath.Add(textBox27);
            tbPath.Add(textBox11);
            tbPath.Add(textBox20);

            if (index == 1) tagPathSet(textBox27, tbPath, path, sunType);
            else if (index == 2) tagPathSet(textBox11, tbPath, path, sunType);
            else if (index == 3) tagPathSet(textBox20, tbPath, path, sunType);
        }

        private FileUpdataModel getFileUpdataBaseModel(string path) {
            FromBaseModel fModel = getFromBaseModel(path);
            if (fModel is FileUpdataModel) return (FileUpdataModel)fModel;
            return null;
        }

        private int getSunByte(string path)
        {
            return getSunByte(getFileUpdataBaseModel(path));
        }

        private int getSunByte(FileUpdataInterface fileUpdataInter)
        {
            if (fileUpdataInter == null) return -1;
            return fileUpdataInter.sunDeviceType();
        }

        /// <summary>
        /// TAG浏览，选择文件后，发送数据到Dangle
        /// </summary>
        /// <param name="btType"></param>
        private void TagUpdataAfterSelect(DrivaceType hexDrivaceType)
        {
            if (setDType == DrivaceType.TAG)
            {
                if (hexDrivaceType != DrivaceType.TAG) return;
                if (ID == null)
                {
                    ID = new byte[2];
                    ID[0] = 0;
                    ID[1] = 0;
                }
               
                startRefech(DrivaceType.TAG);
                label14.Text = StringUtils.updataSpeed;
            }
        }

        private void setTextBtnForHexInfor(string fileInfo, string versionMessage)//, byte type
        {            
            if (setDType == DrivaceType.NODE) setTextForHexLabel(label16, fileInfo, label6, versionMessage);
            else if (setDType == DrivaceType.LOCATION) setTextForHexLabel(label17, fileInfo, label18, versionMessage);
            else if (setDType == DrivaceType.TAG) setTextForHexLabel(label19, fileInfo, label20, versionMessage);
            //if (setDType == DrivaceType.USB_DANGLE) 
        }

        private void setTextForHexLabel(Label lab, string fileInfo, Label lab2, string versionMessage)
        {
            lab.Text = StringUtils.hexFileInfor + ":" + fileInfo;
            lab2.Text = StringUtils.hexFileSize + ":" + versionMessage;
        }


        public void sendBinData(DrivaceType dType, byte[] ID, byte[] Addr)
        {
            if (fUpdataInterface == null || this.ID == null) return;
            fUpdataInterface.sendBinData(dType,this.ID, Addr);
        }


        public void backBinData(byte[] ID, byte[] Addr)
        {
            if (fromClose) return;
            int addrLength = Addr[0] * 256 + Addr[1];
            otherDrivaceProgress(ID,addrLength);
            usbDangleProgress(addrLength);
            send_DealBinData(ID, Addr,addrLength);                       
        }

        private void send_DealBinData(byte[] ID, byte[] Addr, int addrLength) //发送下一包64字节数据准备，或者发送校验
        {
            if (fUpdataInterface.getType() == DrivaceType.USB_DANGLE) return;
            if (addrLength + 64 >= fileLength)
            {
                checkBinData(ID);
                return;
            }
            Addr[1] = (byte)((addrLength + 64) % 256);
            Addr[0] = (byte)((addrLength + 64) / 256);
            sendBinData(setDType, ID, Addr);   
        }


        private void usbDangleProgress(int addrLength) //USB_dangle显示
        {
            if (fUpdataInterface.getType() != DrivaceType.USB_DANGLE) return;
            int pe_int = (int)((addrLength + 64) * 100 / fileLength);
            if (pe_int > 100) pe_int = 100;
            this.Invoke((EventHandler)(delegate
            { //放入主線程
                label12.Text = pe_int + "%";
            }));   
        }

        private void otherDrivaceProgress(byte[] ID, int addrLength)//节点，参考点，卡片更新进度条显示
        {
            if (fUpdataInterface.getType() == DrivaceType.USB_DANGLE) return;

            if (this.ID != null && this.ID[0] == ID[0] && this.ID[1] == ID[1])
            {
                  setProgress(addrLength, fileLength);            
            }
            if (setDType != DrivaceType.TAG)
            {
                string listID = ID[0].ToString("X2") + ID[1].ToString("X2");
                setListView(listID, addrLength, (UInt32)fileLength);
            }
        }


        private void setProgress(int addrLength, UInt32 fileLength)
        {
            try {
                this.Invoke((EventHandler)(delegate
                { //放入主線程
                    if (fromClose) return;
                    setProgressMain(addrLength, fileLength);
                }));   
            }
            catch { }
            
        }

        private void setProgressMain(int addrLength, UInt32 fileLength)
        {
            int pe_int = (int)((addrLength + 64) * 100 / fileLength);
            if (pe_int > 100) pe_int = 100;
            if (updataDTyp == DrivaceType.NODE) setProgressValue(progressBar1, pe_int, label3);
            else if (updataDTyp == DrivaceType.LOCATION) setProgressValue(progressBar2, pe_int, label8);
            else if (updataDTyp == DrivaceType.TAG)
            {
                setProgressValue(progressBar3, pe_int, null);//label13
                if (pe_int == 100) pe_int = 99;
                setUSB_Dangle_TagListView(pe_int);
            }
            else if (setDType == DrivaceType.USB_DANGLE) setProgressValue(null, pe_int, label12);   
        }


        /// <summary>
        /// 在列表中显示更新进度
        /// </summary>
        /// <param name="value">代表值0到100</param>
        private void setUSB_Dangle_TagListView(int value)
        {
            if (tagBtnIndex == 1) label13.Text = value + "%";
            if (tagBtnIndex == 2) label44.Text = value + "%";
            if (tagBtnIndex == 3) label45.Text = value + "%";
        }

        private void setListView(string listID,int addrLength, UInt32 fileLength)
        {
            this.Invoke((EventHandler)(delegate
            { //放入主線程
                ID_DType idType = new ID_DType(listID, setDType);
                listViewUpdata(idType);
                if (!listIDtable.ContainsKey(idType)) return;
                int indexView = (int)listIDtable[idType];
                int pe_int = (int)((addrLength + 64) * 100 / fileLength);
                if (pe_int > 100) pe_int = 100;
                ListViewItem lvItem = getListView(idType.MDType,0).Items[indexView];
                try {
                    lvItem.SubItems[2].Text = pe_int + "%";
                }
                catch { }                
            }));
        } 

        private void setProgressValue(ProgressBar pBar,int value,Label lab) {
            if (pBar!= null) pBar.Value = value;
            if (lab != null) lab.Text = value + "%";
        }
         
        public void checkBinData(byte[] ID){
            if (fUpdataInterface == null || ID == null) return;
            fUpdataInterface.checkBinData(ID);
        }

        public void backCheckBinData(byte[] ID, byte status){ }

        public void updataResult(byte[] ID, byte status)
        {
            if (fUpdataInterface != null && fUpdataInterface.getType() == DrivaceType.TAG) fUpdataInterface.upDataTag(1);
            try {
                updataResultMain(status);
            }
            catch { }          
        }

        public void updataResultMain( byte status)
        {
            this.Invoke((EventHandler)(delegate
            { //放入主線程
                string msg;
                if (status != 0) msg = StringUtils.updataSuccess;
                else msg = StringUtils.updataFile;
                if (setDType == DrivaceType.NODE) setUpdataResultLabel(label2, msg);
                if (setDType == DrivaceType.LOCATION) setUpdataResultLabel(label9, msg);
                if (setDType == DrivaceType.TAG)
                {
                    setUpdataResultLabel(label14, msg);
                    if (status != 0) setUSB_Dangle_TagListView(100);
                    try
                    {
                        tagUpFlag.Tags[tagBtnIndex - 1].isPath = false;
                    }
                    catch { }
                    for (int i = 0; i < 3; i++)
                    {
                        if (tagUpFlag.Tags[i].isPath && tagBtnIndex < 3)
                        {
                            tagBtnIndex = i + 1;
                            TagUpdataAfterSelect(DrivaceType.TAG);
                            return;
                        }
                    }
                    btnOpen(button36);
                    btnOpen(button45);
                    btnOpen(button46);
                    btnOpen(button17);
                }
            }));
        }

        public void upDataTag(byte Enable)
        {
            if (Enable == 0) {
                retuViewVisi(ID, DataPacketType.SET_CARD_CLOSE_UPDATA, label47);
                upDataCard(button17, button21);
            } 
            if (Enable == 1) upDataCard(button21, button17);
        }

        public void upDataTag(byte[] ID, byte[] Addr, byte[] len)
        {
            if (setDType != DrivaceType.TAG) return;
            int addrLength = Addr[0] * 256 + Addr[1];
            int fileLength = len[0]*256 +len[1]; 
       
            string listID = ID[0].ToString("X2") + ID[1].ToString("X2");      
            setListView(listID, addrLength, (UInt32)fileLength);
        }

        private void listViewUpdata(ID_DType idType)
        {
            startHashTable();
            if (idType == null ) return;
            bool isHave = listIDtable.ContainsKey(idType);
            if (isHave) return;
            ListView lv = getListView(setDType,0);
            for (int i = 0; i < lv.Items.Count;i++ )
            {
                if (idType.Id.Equals(lv.Items[i].SubItems[0].Text))
                {
                    listTableAddItem(idType, i);
                }
            }
        }

        private void listTableAddItem(ID_DType idType, int index)
        {
            listIDtable.Add(idType, index);
        }

        
        private void startHashTable() {
            if (listIDtable == null) listIDtable = new Hashtable();
        }

        private void upDataCard(Button btn_Open,Button btn_Close) {
            this.Invoke((EventHandler)(delegate { //放入主線程
                btnOpen(btn_Open);
                btnClose(btn_Close);
            }));
        }

        public void clearTag(){ }

        private void setUpdataResultLabel(Label lab,string text) {
            lab.Text =  text;
        }

        public void setUpdataResultLabelMain(Label lab, string text)
        {
            this.Invoke((EventHandler)(delegate
            { //放入主線程
                if (lab == null || text == null) return;
                if (!lab.Visible) lab.Visible = true;
                setUpdataResultLabel(lab,text);
            }));
        }

        public void enableFalse(Label lab)
        {
            if (lab == null) return;
            try {
                this.Invoke((EventHandler)(delegate
                { //放入主線程                
                    if (lab.Visible) lab.Visible = false;
                }));
            }
            catch {}          
        }

        private void button4_Click(object sender, EventArgs e) {
            openFile();
        }

        private void openFile() {
            //if (openFileDialog == null) openFileDialog = new OpenFileDialog();
            if (!isOpenCom) {
                MessageBox.Show(StringUtils.commOpen);
                return;
            }
            openFileDialog.Filter = "hex|*.hex|all|*.*";
            ///openFileDialog.ShowDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                addFileUpDataModel();
                string g_hexfilepath = openFileDialog.FileName;
                
                fileInforma(g_hexfilepath);
            }
        }

        private void pathSet(TextBox tb,string path) {
            tb.Text = path;
        }
       
        private void addFileUpDataModel() {
            fileModel = new FileUpdataModel(this);
            fUpdataInterface = fileModel;
        }

        private string getPath() {
            if (tabControl1.SelectedIndex == 0) return textBox1.Text;
            //else if (tabControl1.SelectedIndex == 1) return textBox18.Text; //tb.Text = path;
            else if (tabControl1.SelectedIndex == 1 && tagUpFlag.Tags[0].isPath) return textBox27.Text; //tb.Text = path;
            else if (tabControl1.SelectedIndex == 1 && tagUpFlag.Tags[1].isPath) return textBox11.Text; //tb.Text = path;
            else if (tabControl1.SelectedIndex == 1 && tagUpFlag.Tags[2].isPath) return textBox20.Text; //tb.Text = path;
            return "";
        }

        private void fileInforma(string path) {
            if (path == null || path.Length < 1) return;
            HexFileBean hfBean = new HexFileBean(path);
            if (fUpdataInterface != null) fUpdataInterface.fileInformation(hfBean);          
        }

        private void button5_Click(object sender, EventArgs e)
        {
            startRefech(DrivaceType.NODE);
            label2.Text = StringUtils.updataSpeed;
        }

        private void startRefech(DrivaceType dtype) {
            
            if (ID == null && dtype != DrivaceType.TAG)
            {
                MessageBox.Show(StringUtils.errorID);
                return;
            }
            if (!selectFileUpdataInterface(dtype)) {
                MessageBox.Show(StringUtils.hexFileAddErr);
                return;
            }
            if (fUpdataInterface == null) {
                MessageBox.Show(StringUtils.operationFailed);
                return;
            }            
            if (fUpdataInterface.getType() != dtype)
            {
                MessageBox.Show(StringUtils.errorHexFile);
                return;
            }
            if (sunType != fUpdataInterface.sunDeviceType() && dtype != DrivaceType.TAG)
            {
                MessageBox.Show(StringUtils.hexFileNoType);
                return;
            }
            if (dtype != DrivaceType.TAG)
            {
                foreach (ID_Version id_versin in id_Versions)
                {
                    if (id_versin.Version == null) continue;
                    if (!XWUtils.byteBTBettow(id_versin.Id, ID, ID.Length)) continue;
                    if (!XWUtils.byteBTBettow(id_versin.Version, fileModel.HfModelInfor.TimeVersion, id_versin.Version.Length)) break;
                    MessageBox.Show(StringUtils.isNewVersin);
                    return;
                }
            }
            setDType = dtype;
            updataDTyp = dtype;
            byte[] Addr = { 0, 0 };
            fUpdataInterface.start((byte)tagBtnIndex);
            sendBinData(setDType,ID, Addr);
        }

        private bool selectFileUpdataInterface(DrivaceType dtype)
        {
            return selectFileModel(getPath());
        }

        private bool selectFileModel(string path)
        {
            fileModel = (FileUpdataModel)getFromBaseModel(path);
            if (fileModel == null) return false;
            
            fileLength = fileModel.HfModelInfor.FileLength;
            fUpdataInterface = fileModel;
            return true;
        }

        private void button23_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void button36_Click(object sender, EventArgs e)
        {
            tagBtnIndex = 1;
            openFile();
        }

        private void button22_Click(object sender, EventArgs e)
        {           
            label9.Text = StringUtils.updataSpeed;
            startRefech(DrivaceType.LOCATION);            
        }

        private void button17_Click(object sender, EventArgs e)
        {
            btnClose(button36);
            btnClose(button45);
            btnClose(button46);
            btnClose(button17);
            btnOpen(button21);
            if (textBox27.Text.Length > 1) tagUpFlag.Tags[0].isPath = true;
            if (textBox11.Text.Length > 1) tagUpFlag.Tags[1].isPath = true;
            if (textBox20.Text.Length > 1) tagUpFlag.Tags[2].isPath = true;
            tagBtnIndex = 1;
            TagUpdataAfterSelect(DrivaceType.TAG);
        }

        private void groupBox4_Enter(object sender, EventArgs e) { }

        private void button18_Click(object sender, EventArgs e)
        {
            updataDTyp = DrivaceType.USB_DANGLE;
            openFile();
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)//进行tabpage位置判断  
            {
                setDType = DrivaceType.NODE;
            }
            if (tabControl1.SelectedTab == tabPage2)//进行tabpage位置判断  
            {
                setDType = DrivaceType.LOCATION;
            }
            if (tabControl1.SelectedTab == tabPage3)//进行tabpage位置判断  
            {
                setDType = DrivaceType.TAG;
            }
        } 

        private void button21_Click(object sender, EventArgs e)
        {
            btnOpen(button36);
            btnOpen(button45);
            btnOpen(button46);
            btnOpen(button17);
            setViewVisi(DataPacketType.SET_CARD_CLOSE_UPDATA, label47);
            if (fUpdataInterface != null) {
                fUpdataInterface.stop();
                try {
                    Thread.Sleep(50);
                }
                catch { }               
                fUpdataInterface.upDataTag(0);
                try
                {
                    Thread.Sleep(50);
                }
                catch { }   
                dSetInterface.clearTag();
            }   
        }

        private void groupBox10_Enter(object sender, EventArgs e) { }

        private string getPath(ListViewItem selectOne)
        {
            if (selectOne == null) return null;
            string selectMsg = selectOne.SubItems[0].Text;
            string fileName = XWUtils.getSplFirst(selectMsg) + ".hex";
            string filePathAndName = XWUtils.getSplFirst(selectMsg, ')');
            string filePath = XWUtils.getSplpart(filePathAndName, '(', 1);
            if (fileName == null || filePath == null) return null;
            string path = "";
            try{
                path = System.IO.Path.Combine(filePath, fileName);
            } catch{
                return null;
            }
            return path;
        }

        private void dealPathListView(ListViewItem selectOne)
        {
            string path = getPath(selectOne);
            if(!selectFileModel(path)) return;
            fileInforma(path);
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteItem();
            //id_Versions.Remove(idVerItem);
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (idPath_flag != IDLIST) return;
            clearItem();
            clearBtnDeal(getType(), idPath_flag, true);
            idPath_flag = IDLIST;
        }

        /// <summary>
        /// 删除ListView中一项Item
        /// </summary>
        private void deleteItem() {
            if (selectOne == null || idPath_flag == -1) return;
            getListView(setDType, idPath_flag).Items.Remove(selectOne);
            if (idPath_flag == 1) closeFromBaseModel(getPath(selectOne));
            selectOne = null;
        }

        /// <summary>
        ///  清除缓存的文件数据
        /// </summary>
        private void clearItem() {
            if (idPath_flag != 1) return;
            foreach (ListViewItem lvitem in getListView(setDType, idPath_flag).Items)
            {
                closeFromBaseModel(getPath(lvitem));
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            setDType = getType();
            ToolStripItemCollection tsItem = contextMenuStrip1.Items;            
        }

        private void listView6_SelectedIndexChanged(object sender, EventArgs e)
        {
            idPath_flag = 1;
        }

        private void listView5_SelectedIndexChanged(object sender, EventArgs e)
        {
            idPath_flag = 1;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteItem();
        }

        private void upDataToolStripMenuItem_Click(object sender, EventArgs e) //清空
        {
            clearItem();
            clearBtnDeal(getType(), PATHLIST, true);
        }

        private void updataToolStripMenuItem1_Click(object sender, EventArgs e) //更新
        {
            if (idPath_flag != 1) return;
            TagUpdataAfterSelect(setDType);
        }

        private void groupBox5_Enter(object sender, EventArgs e) { }

        private void label5_Click(object sender, EventArgs e){ }

        private void button27_Click(object sender, EventArgs e) // 设置节点模式
        {
            if (IDnullMessage(ID)) return;
            if (IDnullMessage(ID, textBox13)) return;           
            setViewVisi(DataPacketType.SET_NODE_MODEL, label37);
            byte model = getNODEmodel(comboBox5.Text);
            if (model == 2) MessageBox.Show(StringUtils.errorPort);
            else {
                dSetInterface.onSetNODEmodel(ID, getNODEmodel(comboBox5.Text));
            }             
        }

        private byte getNODEmodel(string modelStr)
        { 
            byte model = 2;
            if (StringUtils.staticModel.Equals(modelStr)) model = 0;
            else if (StringUtils.dynmicModel.Equals(modelStr)) model = 1;
            return model;
        }

        private void button29_Click(object sender, EventArgs e) // 读取节点模式
        {
            if (IDnullMessage(ID, textBox13)) return;
            dSetInterface.onReadNODEmodel(ID,0);
        }

        private void button28_Click(object sender, EventArgs e) // 设置节点IP
        {
            byte[] ip = XWUtils.getIP4byte(textBox12.Text);
            if (IP_IDNULL(ID, ip)) return;
            if (IDnullMessage(ID, textBox14)) return;
            
            setViewVisi(DataPacketType.SET_NODE_IP, label38);
            dSetInterface.onSetNODE_IP(ID,ip);
        }

        private void button42_Click(object sender, EventArgs e) // 设置subMask
        {
            byte[] ip = XWUtils.getIP4byte(textBox19.Text);
            if (IP_IDNULL(ID, ip)) return;
            if (IDnullMessage(ID, textBox15)) return;
            
            setViewVisi(DataPacketType.SET_SUBMASK, label39);
            dSetInterface.onSetNODESubMask(ID, ip);
        }

        private void button35_Click(object sender, EventArgs e) // 设置gateWay
        {
            byte[] ip = XWUtils.getIP4byte(textBox17.Text);
            if (IP_IDNULL(ID, ip)) return;
            if (IDnullMessage(ID, textBox16)) return;        
            setViewVisi(DataPacketType.SET_GATEWAY, label40);
            dSetInterface.onSetNODEGateWay(ID, ip);
        }

        private void button32_Click(object sender, EventArgs e) // 读取节点IP
        {
            if (IDnullMessage(ID, textBox14)) return;
            dSetInterface.onReadNODE_IP(ID, null);
        }

        private void button41_Click(object sender, EventArgs e) // 读取SubMask
        {
            if (IDnullMessage(ID, textBox15)) return;
            dSetInterface.onReadNODESubMask(ID, null);
        }

        private void button40_Click(object sender, EventArgs e) // 读取gateWay
        {
            if (IDnullMessage(ID, textBox16)) return;
            dSetInterface.onReadNODEGateWay(ID, null);
        }

        private void textBox14_TextChanged(object sender, EventArgs e){}

        private void button19_Click(object sender, EventArgs e)
        {
            label12.Text = "0%";
            if (fUpdataInterface != null) fUpdataInterface.askUSB_dangleUpData();
            else MessageBox.Show(StringUtils.operationFailed);
        }

        private void button43_Click(object sender, EventArgs e)
        {
            if (IDnullMessage(ID)) return;
            setViewVisi(DataPacketType.RESET, label46);
            dSetInterface.onNODEfuwei(ID);            
        }

        private void button44_Click(object sender, EventArgs e)
        {
            if (dSetInterface != null) dSetInterface.clearTag();
        }

        private void button45_Click(object sender, EventArgs e)
        {
            tagBtnIndex = 2;
            openFile();           
        }

        private void button46_Click(object sender, EventArgs e)
        {
            tagBtnIndex = 3;
            openFile();
        }

        private void textBox27_Click(object sender, EventArgs e)
        {
            string path = textBox27.Text;
            fileInforma(path);
        }

        private void textBox11_Click(object sender, EventArgs e)
        {
            string path = textBox11.Text;
            fileInforma(path);
        }

        private void textBox20_Click(object sender, EventArgs e)
        {
            string path = textBox20.Text;
            fileInforma(path);
        }


        private void button15_Click_1(object sender, EventArgs e)
        {
            int faZhi = XWUtils.stringToInt1(textBox21.Text);
            if (IDnullMessage(ID)) return;
            if (faZhi > 255 || faZhi < 1)
            {
                MessageBox.Show(StringUtils.quZhiFanWei);
                return;
            }
            setViewVisi(DataPacketType.SET_CARD_XinHaoQiangdu, label15);
            dSetInterface.onSet_XinHaoQiangdu_(ID, (byte)faZhi);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (IDnullMessage(ID, textBox25)) return;
            dSetInterface.onRead_XinHaoQiangdu_(ID, 0);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            double xiShu = XWUtils.stringToDouble1(textBox24.Text);
            if (xiShu == -1 || IDnullMessage(ID)) return;
            if (xiShu > 2.55 || xiShu <= 0)
            {
                MessageBox.Show(StringUtils.quZhiFanWei2);
                return;
            }
            setViewVisi(DataPacketType.SET_CARD_XinHaoQiangduXiShu, label10);
            int xiShuBaiBei = (int)((xiShu + 0.00005) * 100);
            dSetInterface.onSet_XinHaoQiangduXiShu_(ID, (byte)xiShuBaiBei);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            if (IDnullMessage(ID, textBox28)) return;
            dSetInterface.onRead_XinHaoQiangduXiShu(ID, 0);
        }


        private void button39_Click_1(object sender, EventArgs e)
        {
            fUpdataInterface.checkBinData(null);
        }


    }
}
