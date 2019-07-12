using CiXinLocation.bean;
using CiXinLocation.CiXinInterface;
using CiXinLocation.Model;
using CiXinLocation.Utils;
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
    public partial class NODESearchForm : Form, RecvCallBack, FileUpdataInterface, LabelChangeInterface
    {
        private Dictionary<string, CanKaoDianUpInfo> canKaoDianInfors;
        private Dictionary<string, CanKaoDianUpInfo> maincanKaoDianInfors;
        public NODESearchModel nodesearchModel;
        //private byte[] selectNodeID;
        private FileUpdataModel fileModel;
        private FileUpdataModelInterface fUpdataInterface;
        private UInt32 fileLength = 0;
        private DrivaceTypeAll updataDTyp;
        private DrivaceTypeAll setDType;
        private bool fromClose = false;
        private byte[] selectNodeID;
        byte sunType = 0;
        private int tagBtnIndex = 0;//
        private string nodeID;

        public NODESearchForm()
        {
            InitializeComponent();           
        }

        public NODESearchForm(string nodeID, Dictionary<string, CanKaoDianUpInfo> canKaoDianInfors)
        {
            InitializeComponent();
            this.Text = "透傳設備" + nodeID;
            this.nodeID = nodeID;
            createCanKaoDianUpInfo();
            maincanKaoDianInfors = new Dictionary<string, CanKaoDianUpInfo>(canKaoDianInfors);
            string ipInfo = "";
            if (canKaoDianInfors.ContainsKey(nodeID))
            {
                nodesearchModel = new NODESearchModel(canKaoDianInfors[nodeID], this);
                ipInfo = canKaoDianInfors[nodeID].IpInfo;
            } 
            else nodesearchModel = new NODESearchModel(null,this);
            addFileUpDataModel(ipInfo);
            setDType = DrivaceTypeAll.NODE;
            updataDTyp = DrivaceTypeAll.NODE;
        }

        private void createCanKaoDianUpInfo() 
        {
            if (this.canKaoDianInfors == null)
            {
                this.canKaoDianInfors = new Dictionary<string, CanKaoDianUpInfo>();
            }
        }

        private void addCanKaoDianUpInfoID(Dictionary<string, CanKaoDianUpInfo> canKaoDianInfors) 
        {
            createCanKaoDianUpInfo();
            Dictionary<string, CanKaoDianUpInfo> cacheCanKaoDianInfors = new Dictionary<string, CanKaoDianUpInfo>(canKaoDianInfors);
            foreach (var item in cacheCanKaoDianInfors)
            {                                
                addCanKaoDianUpInfoID(item.Key,item.Value);
            }
        }

        public void addCanKaoDianUpInfoID(string canID, CanKaoDianUpInfo canKaoDianInfor)
        {
            createCanKaoDianUpInfo();
            canKaoDianInfor.removeVersionType();
            if (!canKaoDianInfors.ContainsKey(canID)) canKaoDianInfors.Add(canID, canKaoDianInfor);
        }

        public void changeCanKaoDianUpInfo(string canID, CanKaoDianUpInfo canKaoDianInfor)
        {
            createCanKaoDianUpInfo();
            if (canKaoDianInfors.ContainsKey(canID)) canKaoDianInfors[canID] = canKaoDianInfor;
            else canKaoDianInfors.Add(canID, canKaoDianInfor);
        }

        public void changeCanKaoDianUpInfo(string canID, string ip)
        {
            if (this.canKaoDianInfors == null) return;
            if (canKaoDianInfors.ContainsKey(canID)) canKaoDianInfors[canID].IpInfo = ip;
        }

        private void NODESearchForm_Load(object sender, EventArgs e)
        {
            addData();
            string ip4 = XWUtils.GetAddressIP();
            textBox2.Text = ip4;
            SetDataResuleView.getInstance().setLanINterface(this);
        }

        private void addData() 
        {
            if (canKaoDianInfors == null || canKaoDianInfors.Count < 1) return;
            Dictionary<string, CanKaoDianUpInfo> cacheInfors = new Dictionary<string,CanKaoDianUpInfo>(canKaoDianInfors);
            foreach (var item in cacheInfors)
            {
                addData(item.Key,item.Value.Name,"");
            }
        }


        public void setUpdataResultLabelMain(Label lab, string text)
        {
            this.Invoke((EventHandler)(delegate
            { //放入主線程
                if (lab == null || text == null) return;
                if (!lab.Visible) lab.Visible = true;
                setUpdataResultLabel(lab, text);
            }));
        }

        public void enableFalse(Label lab)
        {
            if (lab == null) return;
            try
            {
                this.Invoke((EventHandler)(delegate
                { //放入主線程                
                    if (lab.Visible) lab.Visible = false;
                }));
            }
            catch { }
        }

        public FileUpdataModel FileModel
        {
            get { return fileModel; }
            set { fileModel = value; }
        }

        private void addData(string id,string name,string version) 
        {
            if (nodeID.Equals(id)) return;
            ListViewItem lvItem = new ListViewItem();
            lvItem.SubItems[0].Text = id;
            lvItem.SubItems.Add(name);
            lvItem.SubItems.Add("");
            //lvItem.SubItems.Add(Loca_NodeUtils.getDrivaceNODEType(item.Value.Version));
            listView1.Items.Add(lvItem);
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

        private bool IPnullMessage(byte[] ip)
        {
            if (ip == null)
            {
                MessageBox.Show("IP格式不正確，格式參考：192.168.1.101");
                return true;
            }
            return false;
        }

        /// <summary>
        /// ID是否为空，并且显示一个MessageBox，作为提示,并且接口也不能为空
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true为空，false不为空</returns>
        private bool IDnullMessage(byte[] id, TextBox tbox)
        {
            if (nodesearchModel == null) return false;
            return IDnullMessage(id, "未選擇ID", tbox);
        }
        private bool IDnullMessage(byte[] id)
        {
            return IDnullMessage(id, null);
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
        private bool PortNullMessage(byte[] port)
        {
            return PortNullMessage(port, "格式錯誤");
        }
        private bool PortNullMessage(byte[] port, string msg)
        {
            if (port == null)
            {
                MessageBox.Show(msg);
                return true;
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e) //搜索
        {
            //nodesearchModel.searchID(selectNodeID);
            button1.Enabled = false;
            new Thread(searchID).Start();
        }

        private int searchIDCount = 5;//搜寻的次数
        private void searchID() 
        {
            searchIDCount = 5;
            while (searchIDCount > 0) 
            {
                if (canKaoDianInfors == null) nodesearchModel.searchID(null);
                else if (canKaoDianInfors.Count == 0) nodesearchModel.searchID(null);
                else
                {
                    List<CanKaoDianUpInfo> mList = canKaoDianInfors.Values.ToList();
                    byte[] IDs = new byte[mList.Count * 2];
                    for (int j = 0; j < mList.Count; j++)
                    {
                        Array.Copy(mList[j].CID, 0, IDs, j * 2, 2);
                    }
                    nodesearchModel.searchID(IDs);
                }
                Thread.Sleep(200);
                searchIDCount--;
            }
                          
            Dictionary<string, CanKaoDianUpInfo> cacheCanKaoDianInfors = new Dictionary<string, CanKaoDianUpInfo>(canKaoDianInfors);
            foreach (var item in cacheCanKaoDianInfors)
            {
                for (int k = 0; k < 5;k++ )
                {
                    if (item.Value.Version != null) 
                    {
                        if (item.Value.Version[0] != 0 || item.Value.Version[1] != 0 || item.Value.Version[2] != 0 || item.Value.Version[3] != 0)
                            break;
                    }
                    nodesearchModel.checkVersion(item.Value.CID);
                    Thread.Sleep(100);
                }
            }

            this.Invoke((EventHandler)(delegate
            { //放入主線程
                button1.Enabled = true;
            }));
           
            //nodesearchModel.checkVersion(ID);
        }

        private void button2_Click(object sender, EventArgs e) //清除
        {
            listView1.Items.Clear();
            canKaoDianInfors.Clear();
            addData();
        }

        private void checkVersion() { }

        private void button6_Click(object sender, EventArgs e) //设置ServerIP
        {
            byte[] ip = XWUtils.getIP4byte(textBox2.Text);
            if (IP_IDNULL(selectNodeID, ip)) return;
            setViewVisi(DataPacketType.SET_SERVISE_IP, label33);
            if (IDnullMessage(selectNodeID, textBox5)) return;
            nodesearchModel.SetServerIP(selectNodeID, ip);
        }

        private void button9_Click(object sender, EventArgs e) //读取ServerIP
        {
            if (IDnullMessage(selectNodeID, textBox5)) return;
            nodesearchModel.readServerIP(selectNodeID);
        }

        private void button7_Click(object sender, EventArgs e) //设置ServerPort
        {
            byte[] port = XWUtils.getComTime(textBox3.Text);
            if (Port_IDNULL(selectNodeID, port)) return;
            if (IDnullMessage(selectNodeID, textBox6)) return;
            setViewVisi(DataPacketType.SET_SERVISE_PORT, label34);
            nodesearchModel.setServerPort(selectNodeID, port);
        }

        private void button10_Click(object sender, EventArgs e)//读取ServerPort
        {
            if (IDnullMessage(selectNodeID, textBox6)) return;
            nodesearchModel.readServerPort(selectNodeID);
        }

        private void button8_Click(object sender, EventArgs e) //设置wifi名称
        {
            if (IDnullMessage(selectNodeID)) return;
            byte[] sendWifi = getWifi(textBox4.Text);
            if (sendWifi == null) return;
            if (IDnullMessage(selectNodeID, textBox7)) return;
            setViewVisi(DataPacketType.SET_WIFI_NAME, label35);
            nodesearchModel.setWifiName(selectNodeID, sendWifi);
        }

        private byte[] getWifi(string data)
        {
            byte[] sendWifi = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                sendWifi[i] = 0;
            } try
            {
                byte[] wifiByte = System.Text.Encoding.ASCII.GetBytes(data);
                if (wifiByte.Length > 32)
                {
                    MessageBox.Show("WiFi只能在32個字符內");
                    return null;
                }
                Array.Copy(wifiByte, 0, sendWifi, 0, wifiByte.Length);
            }
            catch
            {
                return null;
            }
            return sendWifi;
        }

        private void button11_Click(object sender, EventArgs e)//读取wifi名称
        {
            if (IDnullMessage(selectNodeID, textBox7)) return;
            nodesearchModel.readWifiName(selectNodeID);
        }

        private void button13_Click(object sender, EventArgs e) //設置wifi密碼
        {
            if (IDnullMessage(selectNodeID)) return;
            byte[] sendWifi = getWifi(textBox9.Text);
            if (sendWifi == null) return;
            if (IDnullMessage(selectNodeID, textBox8)) return;
            setViewVisi(DataPacketType.SET_WIFI_PASSWORD, label36);
            nodesearchModel.setWifiPassWord(selectNodeID, sendWifi);
        }

        private void button12_Click(object sender, EventArgs e) //讀取wifi密碼
        {
            if (IDnullMessage(selectNodeID, textBox8)) return;
            nodesearchModel.readWifiPassWord(selectNodeID);
        }

        private void button27_Click(object sender, EventArgs e) //設置節點模式
        {
            if (IDnullMessage(selectNodeID)) return;
            if (IDnullMessage(selectNodeID, textBox13)) return;
            setViewVisi(DataPacketType.SET_NODE_MODEL, label37);
            byte model = getNODEmodel(comboBox5.Text);
            if (model == 2) MessageBox.Show("格式錯誤");
            else
            {
                nodesearchModel.setNODEModel(selectNodeID, getNODEmodel(comboBox5.Text));
            }             
        }

        private byte getNODEmodel(string modelStr)
        {
            byte model = 2;
            if ("靜態模式".Equals(modelStr)) model = 0;
            else if ("動態模式".Equals(modelStr)) model = 1;
            return model;
        }

        private void button29_Click(object sender, EventArgs e) //读取节点模式
        {
            if (IDnullMessage(selectNodeID, textBox13)) return;
            nodesearchModel.readNODEModel(selectNodeID);
        }

        private void button28_Click(object sender, EventArgs e) //设置节点IP
        {
            byte[] ip = XWUtils.getIP4byte(textBox12.Text);
            if (IP_IDNULL(selectNodeID, ip)) return;
            if (IDnullMessage(selectNodeID, textBox14)) return;

            setViewVisi(DataPacketType.SET_NODE_IP, label38);
            nodesearchModel.SetNODEIP(selectNodeID, ip);
        }

        private void button32_Click(object sender, EventArgs e) //读取节点IP
        {
            if (IDnullMessage(selectNodeID, textBox14)) return;
            nodesearchModel.readNODEIP(selectNodeID);
        }

        private void button42_Click(object sender, EventArgs e) //设置submask
        {
            byte[] ip = XWUtils.getIP4byte(textBox19.Text);
            if (IP_IDNULL(selectNodeID, ip)) return;
            if (IDnullMessage(selectNodeID, textBox15)) return;

            setViewVisi(DataPacketType.SET_SUBMASK, label39);
            nodesearchModel.SetSubmask(selectNodeID, ip);            
        }

        private void button41_Click(object sender, EventArgs e)//读取submask
        {
            if (IDnullMessage(selectNodeID, textBox15)) return;
            nodesearchModel.readSubmask(selectNodeID);
        }

        private void button35_Click(object sender, EventArgs e) //设置gateWay
        {
            byte[] ip = XWUtils.getIP4byte(textBox17.Text);
            if (IP_IDNULL(selectNodeID, ip)) return;
            if (IDnullMessage(selectNodeID, textBox16)) return;
            setViewVisi(DataPacketType.SET_GATEWAY, label40);
            nodesearchModel.SetGateWay(selectNodeID, ip);
        }

        private void button40_Click(object sender, EventArgs e) //读取gateWay
        {
            if (IDnullMessage(selectNodeID, textBox16)) return;
            nodesearchModel.ReadGateWay(selectNodeID);
        }

        private void button15_Click(object sender, EventArgs e) //设置接收阀值
        {
            int faZhi = XWUtils.stringToInt1(textBox21.Text);
            textBox25.Text = "";
            if (IDnullMessage(selectNodeID)) return;
            if (faZhi > 255 || faZhi < 1)
            {
                MessageBox.Show("取值范围：1~255");
                return;
            }
            setViewVisi(DataPacketType.SET_CARD_XinHaoQiangdu, label15);
            nodesearchModel.SetFazhi(selectNodeID, (byte)faZhi);
        }

        private void button20_Click(object sender, EventArgs e) //读取接收阀值
        {
            if (IDnullMessage(selectNodeID, textBox25)) return;
            nodesearchModel.ReadFazhi(selectNodeID);
        }

        private void button16_Click(object sender, EventArgs e) // 设置强度系数
        {
            double xiShu = XWUtils.stringToDouble1(textBox24.Text);
            if (xiShu == -1 || IDnullMessage(selectNodeID)) return;
            textBox28.Text = "";
            if (xiShu > 2.55 || xiShu <= 0)
            {
                MessageBox.Show("取值範圍：0.01~2.55");
                return;
            }
            setViewVisi(DataPacketType.SET_CARD_XinHaoQiangduXiShu, label10);
            int xiShuBaiBei = (int)((xiShu + 0.00005) * 100);
            nodesearchModel.SetRssiValue(selectNodeID, (byte)xiShuBaiBei);
        }

        private void button26_Click(object sender, EventArgs e) // 读取强度系数
        {
            if (IDnullMessage(selectNodeID, textBox28)) return;
            nodesearchModel.ReadRssiValue(selectNodeID);
        }

        private void button43_Click(object sender, EventArgs e) //复位
        {
            setViewVisi(DataPacketType.RESET, label46);
            nodesearchModel.onNODEfuwei(selectNodeID);
        }

        /// <summary>
        /// 成功返回结果显示
        /// </summary>
        private void retuViewVisi(byte[] ID, DataPacketType dpType, Label lab)
        {
            DP_TYPE_LABEL dpLabel = new DP_TYPE_LABEL(dpType, lab);
            SetDataResuleView.getInstance().returnSuccess(dpLabel);
        }

        public void onSetJieDianServiseIP(byte[] ID, byte[] IP)
        {
            retuViewVisi(ID, DataPacketType.SET_SERVISE_IP, label33);
        }

        private void setTextIp(byte[] ID, byte[] IP, TextBox textBox)
        {
            setTextToMain(textBox, IP[0] + "." + IP[1] + "." + IP[2] + "." + IP[3]);
        }

        private void setTextToMain(TextBox textBox, string text)
        {
            this.Invoke((EventHandler)(delegate
            { //放入主線程
                textBox.Text = text;
            }));
        }

        /// <summary>
        /// buf为对应的数据块，type为数据包的类型
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="type">值参考文档的透传协议</param>
        public void callBack(byte[] buff, int type,string ipInfo)
        {
            if (buff.Length < 2) return;
            byte[] ID = new byte[2];
            Array.Copy(buff, 0, ID, 0, 2);
            byte[] reData = new byte[buff.Length - 2];
            Array.Copy(buff, 2, reData, 0, reData.Length);
            switch (type)
            {
                case 0x41:
                    dealSearchID(ID);
                    searchIDCount = 5;
                    //nodesearchModel.checkVersion(ID);
                    break;
                case 0x42:
                    addVersion(ID, reData);
                    break;
                case 0x43:
                    break;
                case 0x44:
                    break;
                case 0x45:
                    retuViewVisi(ID, DataPacketType.SET_SERVISE_IP, label33);
                    setTextToMain(textBox5,"");
                    nodesearchModel.readServerIP(selectNodeID);
                    break;
                case 0x46:
                    if (buff.Length < 6) break;
                    setTextIp(ID, reData, textBox5);
                    break;
                case 0x47:
                    retuViewVisi(ID, DataPacketType.SET_SERVISE_PORT, label34);
                    setTextToMain(textBox6, "");
                    nodesearchModel.readServerPort(selectNodeID);
                    break;
                case 0x48:
                    if (buff.Length < 4) break;
                    setTextToMain(textBox6, (reData[0] * 256 + reData[1]).ToString());
                    break;
                case 0x49:
                    retuViewVisi(ID, DataPacketType.SET_WIFI_NAME, label35);
                    setTextToMain(textBox7, "");
                    nodesearchModel.readWifiName(selectNodeID);
                    break;
                case 0x4a:
                    string wifiName = System.Text.Encoding.ASCII.GetString(reData);
                    setTextToMain(textBox7, wifiName);
                    break;
                case 0x4b:
                    retuViewVisi(ID, DataPacketType.SET_WIFI_PASSWORD, label36);
                    setTextToMain(textBox8, "");
                    nodesearchModel.readWifiPassWord(selectNodeID);
                    break;
                case 0x4c:
                    string wifiPassword = System.Text.Encoding.ASCII.GetString(reData);
                    setTextToMain(textBox8, wifiPassword);
                    break;
                case 0x4d:
                    retuViewVisi(ID, DataPacketType.RESET, label46);
                    break;
                case 0x4e:
                    retuViewVisi(ID, DataPacketType.SET_NODE_MODEL, label37);
                    setTextToMain(textBox13, "");
                    nodesearchModel.readNODEModel(selectNodeID);
                    break;
                case 0x4f:
                    byte model = reData[0];
                    if (model == 0) setTextToMain(textBox13, "靜態模式");
                    if (model == 1) setTextToMain(textBox13, "動態模式");                    
                    break;
                case 0x50:
                    retuViewVisi(ID, DataPacketType.SET_NODE_IP, label38);
                    setTextToMain(textBox14, "");
                    nodesearchModel.readNODEIP(selectNodeID);
                    break;
                case 0x51:
                    if (buff.Length < 6) break;
                    setTextIp(ID, reData, textBox14);
                    break;
                case 0x52:
                    retuViewVisi(ID, DataPacketType.SET_SUBMASK, label39);
                    setTextToMain(textBox15, "");
                    nodesearchModel.readSubmask(selectNodeID);
                    break;
                case 0x53:
                    if (buff.Length < 6) break;
                    setTextIp(ID, reData, textBox15);
                    break;
                case 0x54:
                    retuViewVisi(ID, DataPacketType.SET_GATEWAY, label40);
                    setTextToMain(textBox16, "");
                    nodesearchModel.ReadGateWay(selectNodeID);
                    break;
                case 0x55:
                    if (buff.Length < 6) break;
                    setTextIp(ID, reData, textBox16);
                    break;
                case 0x56:
                    retuViewVisi(ID, DataPacketType.SET_CARD_XinHaoQiangdu, label15);
                    setTextToMain(textBox25, "");
                    nodesearchModel.ReadFazhi(selectNodeID);
                    break;
                case 0x57:
                    setTextToMain(textBox25, reData[0].ToString());
                    break;
                case 0x58:
                    retuViewVisi(ID, DataPacketType.SET_CARD_XinHaoQiangduXiShu, label10);
                    setTextToMain(textBox28,"");
                    nodesearchModel.ReadRssiValue(selectNodeID);
                    break;
                case 0x59:
                    double bd = reData[0];
                    bd  /= 100;
                    setTextToMain(textBox28, bd.ToString("F2"));
                    break;
                default:
                    break;
            }
        }

        private void dealSearchID(byte[] ID) 
        {
            string nodeID = ID[0].ToString("X2") + ID[1].ToString("X2");
            if (canKaoDianInfors == null) canKaoDianInfors = new Dictionary<string, CanKaoDianUpInfo>();
            else if (canKaoDianInfors.ContainsKey(nodeID)) return;
            CanKaoDianUpInfo ckInfo = new CanKaoDianUpInfo();
            Array.Copy(ID, 0, ckInfo.CID, 0, 2);
            canKaoDianInfors.Add(nodeID, ckInfo);
            string name = maincanKaoDianInfors.ContainsKey(nodeID) ? maincanKaoDianInfors[nodeID].Name : nodeID;
            this.Invoke((EventHandler)(delegate
            { //放入主線程
                addData(nodeID, name, "");
            }));                    
        }

        private void addVersion(byte[] ID, byte[] data) 
        {
            if(data.Length != 8) return;
            string IDStr = ID[0].ToString("X2") + ID[1].ToString("X2");
            byte[] type = new byte[4];
            byte[] version = new byte[4];
            Array.Copy(data, 0, type, 0, 4);
            Array.Copy(data, 4, version, 0, 4);
            if (canKaoDianInfors.ContainsKey(IDStr)) 
            {
                canKaoDianInfors[IDStr].NodeType = type;
                canKaoDianInfors[IDStr].Version = version;
            }
            //string name = maincanKaoDianInfors.ContainsKey(IDStr) ? maincanKaoDianInfors[IDStr].Name : nodeID;
            this.Invoke((EventHandler)(delegate
            { //放入主線程
                changeListItem(ID,type, version);
            }));            
        }

        private void changeListItem(byte[] ID, byte[] type, byte[] version) 
        {
            string nodeID = ID[0].ToString("X2") + ID[1].ToString("X2");
            int listCount = listView1.Items.Count;
            for (int i = 0; i < listCount; i++)
            {
                if (nodeID.Equals(listView1.Items[i].SubItems[0].Text))
                {
                    listView1.Items[i].SubItems[2].Text = CiXinUtils.getVersion(version) + getSunTypeName(type[2], type[3]);
                    return;
                }
            }
            addData(nodeID, nodeID, CiXinUtils.getVersion(version) + getSunTypeName(type[2], type[3]));
        }


        /// <summary>
        /// 获取子设备类型的字符串
        /// </summary>
        /// <param name="mainType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string getSunTypeName(byte mainType, byte type)
        {
            if (mainType == 0x01) return CiXinUtils.getSunNODETypeName(0x01, type);
            else if (mainType == 0x02) return CiXinUtils.getSunCankaodianTypeName(0x02, type);
            else if (mainType == 0x03) return CiXinUtils.getSunCardTypeName(0x03, type);
            else return "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void openFile()
        {
             openFileDialog.Filter = "hex|*.hex|all|*.*";
            ///openFileDialog.ShowDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //addFileUpDataModel();
                string g_hexfilepath = openFileDialog.FileName;
                fileInforma(g_hexfilepath);
            }
        }

        private void addFileUpDataModel(string ipInfo)
        {
            if (fileModel == null) 
            {
                fileModel = new FileUpdataModel(this);
                if (nodesearchModel != null) fileModel.Ipinfo = nodesearchModel.UdpCanKaoDianUpInfo.IpInfo;
                fUpdataInterface = fileModel;
                fileModel.Ipinfo = ipInfo;
            }             
        }

        private void fileInforma(string path)
        {
            if (path == null || path.Length < 1) return;
            HexFileBean hfBean = new HexFileBean(path);
            if (fUpdataInterface != null) fUpdataInterface.fileInformation(hfBean);
        }

        public void fileInformation(HexFileBean hfInfor)
        {
            if (hfInfor == null) return;
            fileLength = hfInfor.FileLength;
            if (setFUpdataInterface(hfInfor))
            {
                setTextBtnForHexInfor(hfInfor.FileInfor, hfInfor.VersionMessage);
            }
            /*if (updataDTyp == DrivaceType.USB_DANGLE)
            {
                if (hfInfor.mainDrivaceType != DrivaceType.USB_DANGLE)
                    MessageBox.Show(StringUtils.hexFileNoType);
                else
                {
                    usb_fileVersion = hfInfor.TimeVersion;
                    pathSet(textBox10, hfInfor.Path); //tb.Text = path;
                    startUSB_DangleFileUpdata();
                    setTextForHexLabel(label28, hfInfor.FileInfor, label29, hfInfor.VersionMessage);
                    if (addFromBaseModel(fileModel))
                        fUpdataInterface = fileModel;
                }
                updataDTyp = DrivaceType.NOTHING;
            }*/
        }


        /// <summary>
        /// 浏览完目录后，将文件添加到文件列表
        /// </summary>
        /// <param name="hfInfor"></param>
        private bool setFUpdataInterface(HexFileBean hfInfor)
        {

            if (updataDTyp == DrivaceTypeAll.USB_DANGLE) return false;
            DrivaceTypeAll dt = fUpdataInterface.getType();
            if (setDType != dt)
            {
                MessageBox.Show(StringUtils.hexFileNoType);
                return false;
            }
            if (setDType == DrivaceTypeAll.NODE)
            {
                pathSet(textBox1, hfInfor.Path); //textBox1.Text = path;
                return true;
            }
            /*else if (setDType == DrivaceType.CANKAODIAN)
            {
                pathSet(textBox18, hfInfor.Path); //tb.Text = path;
                return true;
            }
            else if (dt == DrivaceType.CARD)
            {
                byte sunType = fUpdataInterface.sunDeviceType();
                tagPathSet(hfInfor.Path, tagBtnIndex, sunType);
                return true;
            }*/
            return false;
        }

        private void pathSet(TextBox tb, string path)
        {
            tb.Text = path;
        }

        private void setTextBtnForHexInfor(string fileInfo, string versionMessage)//, byte type
        {
            if (setDType == DrivaceTypeAll.NODE) setTextForHexLabel(label16, fileInfo, label6, versionMessage);
            //else if (setDType == DrivaceType.CANKAODIAN) setTextForHexLabel(label17, fileInfo, label18, versionMessage);
            //else if (setDType == DrivaceType.CARD) setTextForHexLabel(label19, fileInfo, label20, versionMessage);
            //if (setDType == DrivaceType.USB_DANGLE) 
        }

        private void setTextForHexLabel(Label lab, string fileInfo, Label lab2, string versionMessage)
        {
            lab.Text = StringUtils.hexFileInfor + ":" + fileInfo;
            lab2.Text = StringUtils.hexFileSize + ":" + versionMessage;
        }

        public byte sunDeviceType()
        {
            return 0xff;
        }

        public DrivaceTypeAll getType()
        {
            return DrivaceTypeAll.NODE;
        }

        public void sendBinData(DrivaceTypeAll dType, byte[] ID, byte[] Addr)
        {
            if (fUpdataInterface == null || this.selectNodeID == null) return;
            fUpdataInterface.sendBinData(dType, this.selectNodeID, Addr);
        }

        public void backBinData(byte[] ID, byte[] Addr)
        {
            if (fromClose) return;
            int addrLength = Addr[0] * 256 + Addr[1];
            otherDrivaceProgress(ID,addrLength);
        //    usbDangleProgress(addrLength);
            send_DealBinData(ID, Addr,addrLength);                       
        }

        private void send_DealBinData(byte[] ID, byte[] Addr, int addrLength) //发送下一包64字节数据准备，或者发送校验
        {
            if (fUpdataInterface.getType() == DrivaceTypeAll.USB_DANGLE) return;
            if (addrLength + 64 >= fileLength)
            {
                checkBinData(ID);
                return;
            }
            Addr[1] = (byte)((addrLength + 64) % 256);
            Addr[0] = (byte)((addrLength + 64) / 256);
            sendBinData(setDType, ID, Addr);
        }

        private void otherDrivaceProgress(byte[] ID, int addrLength)//节点，参考点，卡片更新进度条显示
        {
            if (fUpdataInterface.getType() == DrivaceTypeAll.USB_DANGLE) return;

            if (this.selectNodeID != null && this.selectNodeID[0] == ID[0] && this.selectNodeID[1] == ID[1])
            {
                setProgress(addrLength, fileLength);
            }
            /*if (setDType != DrivaceType.TAG)
            {
                string listID = ID[0].ToString("X2") + ID[1].ToString("X2");
                setListView(listID, addrLength, (UInt32)fileLength);
            }*/
        }


        private void setProgress(int addrLength, UInt32 fileLength)
        {
            try
            {
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
            if (updataDTyp == DrivaceTypeAll.NODE) setProgressValue(progressBar1, pe_int, label3);
        }

        private void setProgressValue(ProgressBar pBar, int value, Label lab)
        {
            if (pBar != null) pBar.Value = value;
            if (lab != null) lab.Text = value + "%";
        }

        public void checkBinData(byte[] ID)
        {
            if (fUpdataInterface == null || ID == null) return;
            fUpdataInterface.checkBinData(ID);
        }

        public void backCheckBinData(byte[] ID, byte status) { }

        public void updataResultMain(byte status)
        {
            this.Invoke((EventHandler)(delegate
            { //放入主線程
                string msg;
                if (status != 0) msg = StringUtils.updataSuccess;
                else msg = StringUtils.updataFile;
                if (setDType == DrivaceTypeAll.NODE) setUpdataResultLabel(label2, msg);
            }));
        }

        private void setUpdataResultLabel(Label lab, string text)
        {
            lab.Text = text;
        }

        public void upDataTag(byte Enable){}

        public void upDataTag(byte[] ID, byte[] Addr, byte[] len){}

        public void clearTag() { }

        public void updataResult(byte[] ID, byte status)
        {
            if (fUpdataInterface != null && fUpdataInterface.getType() == DrivaceTypeAll.CARD) fUpdataInterface.upDataTag(1);
            try
            {
                updataResultMain(status);
            }
            catch { }
        }

        /// <summary>
        /// USB_dANGLE更新数据回调
        /// </summary>
        /// <param name="len"></param>
        /// <param name="CheckSum"></param>
        public void askUSB_dangleUpData() { }

        public void close() 
        {
            fromClose = true;
        }
       
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0) return;
            try
            {
                ListViewItem selectOne = listView1.SelectedItems[0];
                if (!button1.Enabled) return;
                string selectMsg = selectOne.SubItems[0].Text;
                selectNodeID = XWUtils.getByteID(selectMsg);
                string tpBtStr = XWUtils.getSplTypeEnd(selectOne.SubItems[2].Text, '-');
                sunType = CiXinUtils.getSunType("-" + tpBtStr);
                if (selectNodeID == null) return;

                label3.Text = "0%";
                label4.Text = StringUtils.changeDrivace + ":" + selectNodeID[0].ToString("X2") + selectNodeID[1].ToString("X2") + " " + tpBtStr;
                label5.Text = StringUtils.currentDrivace + ":" + selectNodeID[0].ToString("X2") + selectNodeID[1].ToString("X2") + " " + tpBtStr;
                if (progressBar1.Value != 0) progressBar1.Value = 0;
                if (button5.Enabled)
                {
                    setBtnOpen(sunType);
                    readAllcanShu(DrivaceTypeAll.NODE, selectNodeID);
                    return;
                }
                setBtnClose();
                setBtnOpen(sunType);
                setBouthOpen();
                readAllcanShu(DrivaceTypeAll.NODE, selectNodeID);
            }
            catch { }   
        }

        /// <summary>
        ///  一键读取所有参数
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ID"></param>
        private void readAllcanShu(DrivaceTypeAll dt, byte[] ID)
        {
            clearReadTextBox();
            nodesearchModel.readAllParameter(dt, ID);
        }

        /// <summary>
        /// 清空读取框中数值
        /// </summary>
        private void clearReadTextBox()
        {
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

        private void btnOpen(Button button)
        {
            if (!button.Enabled) button.Enabled = true;
        }

        private void btnClose(Button button)
        {
            if (button.Enabled) button.Enabled = false;
        }

        /// <summary>
        /// 根据不同子类设备类型，打开和关闭读取按钮
        /// </summary>
        /// <param name="subType"></param>
        private void setBtnOpen(byte subType)
        {
            if (subType == 1)
            {
                setBtnLanOpene();
                closeBtnWifiPa();
            }
            else if (subType == 2)
            {
                setBtnWifiPaOpen();
                setBtnLanClose();
            }
            else if (subType == 3)
            {
                setBtnWifiPaOpen();
                setBtnLanOpene();
            }
        }

        private void setBouthOpen()
        {
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

        private void setBtnWifiPaOpen()
        {
            btnOpen(button27);
            btnOpen(button28);
            btnOpen(button29);
            btnOpen(button32);
            btnOpen(button41);
            btnOpen(button42);
            btnOpen(button35);
            btnOpen(button40);
        }

        private void closeBtnWifiPa()
        {
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

        private void setBtnClose()
        {
            btnClose(button5);
            btnClose(button6);
            btnClose(button7);
            btnClose(button8);
            btnClose(button9);
            btnClose(button10);
            btnClose(button11);
            btnClose(button12);
            btnClose(button13);
            //btnClose(button19);
            //btnClose(button30);
            //btnClose(button31);
            //btnClose(button33);
            //btnClose(button34);
            //btnClose(button22);
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

        private void button5_Click(object sender, EventArgs e)
        {
            startRefech(DrivaceTypeAll.NODE);
            label2.Text = StringUtils.updataSpeed;
        }

        private void startRefech(DrivaceTypeAll dtype)
        {

            if (selectNodeID == null && dtype != DrivaceTypeAll.CARD)
            {
                MessageBox.Show(StringUtils.errorID);
                return;
            }
            if (!selectFileUpdataInterface(dtype))
            {
                MessageBox.Show(StringUtils.hexFileAddErr);
                return;
            }
            if (fUpdataInterface == null)
            {
                MessageBox.Show(StringUtils.operationFailed);
                return;
            }
            if (fUpdataInterface.getType() != dtype)
            {
                MessageBox.Show(StringUtils.errorHexFile);
                return;
            }
            if (sunType != fUpdataInterface.sunDeviceType() && dtype != DrivaceTypeAll.CARD)
            {
                MessageBox.Show(StringUtils.hexFileNoType);
                return;
            }
            if (dtype != DrivaceTypeAll.CARD)
            {
                if (canKaoDianInfors.ContainsKey(selectNodeID[0].ToString("X2") + selectNodeID[1].ToString("X2"))) 
                {
                    byte[] version = canKaoDianInfors[selectNodeID[0].ToString("X2") + selectNodeID[1].ToString("X2")].Version;
                    if (XWUtils.byteBTBettow(version, fileModel.HfModelInfor.TimeVersion, version.Length)) 
                    {
                        MessageBox.Show(StringUtils.isNewVersin);
                        return;
                    }                   
                }
            }
            setDType = dtype;
            updataDTyp = dtype;
            byte[] Addr = { 0, 0 };
            fUpdataInterface.start((byte)tagBtnIndex);
            sendBinData(setDType, selectNodeID, Addr);
        }

        private bool selectFileUpdataInterface(DrivaceTypeAll dtype)
        {
            return selectFileModel(textBox1.Text);
        }

        private bool selectFileModel(string path)
        {
            if (fileModel == null) return false;
            fileLength = fileModel.HfModelInfor.FileLength;
            fUpdataInterface = fileModel;
            return true;
        }


    }

}
