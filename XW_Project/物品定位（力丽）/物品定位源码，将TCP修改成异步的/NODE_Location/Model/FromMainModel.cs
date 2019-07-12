using CiXinLocation.bean;
using CiXinLocation.CiXinInterface;
using CiXinLocation.Utils;
using SerialportSample;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace CiXinLocation.Model
{
    class FromMainModel:FromBaseModel
    {
        public delegate void LEDDataHandle(byte[] buf, string ipInfo);
        public LEDDataHandle LedData;
        public delegate void ShowTagDataHandle(List<CardUpDataBean> showUpCardDatas);
        public ShowTagDataHandle onTagData;
        public delegate void ShowHistoryTagDataHandle(CardUpDataBean showUpCardData,int type);//历史记录的数据委托
        public ShowHistoryTagDataHandle onhisTagData;
        public delegate void ShowCanKaoDianDataHandle(Dictionary<string, CanKaoDianBean> canDictionarys);
        public ShowCanKaoDianDataHandle onCanKData;
        public MainViewInterface mainInterface;
        public delegate void NODEDataHandle(string nodeID,Dictionary<string, CanKaoDianUpInfo> canKaoDianInfor);
        public NODEDataHandle onNODEData;
        private object objLock = new object();
        private object objipBeansLock = new object();
        private UInt32 hisTime = 0;
        private UInt32 runTime = 0;

        private Dictionary<string, byte[]> keyBys;                  //= new Dictionary<string, byte[]>();
        private Dictionary<string, IPIDRessBean> ipBeanDictionarys; //= new Dictionary<string, byte[]>();
        private Dictionary<string, string> showIpBeanDictionarys;   //= new Dictionary<string, byte[]>();

        private Dictionary<string, CanKaoDianBean> canKaoDianDictionarys;    //参考点信息
        private Dictionary<string, HistoryCardBean[]> cardHistoryDictionarys;//参考点信息
        private Dictionary<string, CanKaoDianUpInfo> canKaoDianInfors;       //= new Dictionary<string, byte[]>();
        private bool isTongbuData = false;
        private bool isFileTcpClienModelTongbuData = false;

        /// <summary>
        /// 此时是历史轨迹功能使用时刻
        /// false不是，true是         
        /// </summary>
        private bool isDealHistory = false;
        
        public FromMainModel(MainViewInterface mainInterface) 
        {            
            setTag("FromMainModel");
            comMode = CommunicationMode.UDP;
            receVeByteHandle = 0xfe;//接收包头
            receVeByteend = 0xfd;   //接收包尾

            dealUpCardDatas = new List<CardUpDataBean>();
            oldUpCardDatas = new List<CardUpDataBean>();
            this.mainInterface = mainInterface;
            keyBys = new Dictionary<string, byte[]>();
            ipBeanDictionarys = new Dictionary<string, IPIDRessBean>();
            showIpBeanDictionarys = new Dictionary<string, string>();
            canKaoDianDictionarys = new Dictionary<string, CanKaoDianBean>();
            canKaoDianInfors = new Dictionary<string, CanKaoDianUpInfo>();
            cardHistoryDictionarys = new Dictionary<string, HistoryCardBean[]>();
            // setListViewCanInfos();
            noMoveTime = FileModel.getFlModel().ChFlBean.NoReveTime;            

            FileTcpClienModel.getFileTcpClienMidel().cardHandle += dealDataToCongjiLoca;
        }

        private List<CardUpDataBean> dealUpCardDatas;       
        private List<CardUpDataBean> oldUpCardDatas;
        
        public override void reveData(byte[] buf,string ipInfo)
        {
            if (isFileTcpClienModelTongbuData) return;
            if (LedData != null) LedData(buf, ipInfo);
            packJieXi(buf, ipInfo);
            if (buf.Length != locationPack && (buf[0] != 0XFE || buf[buf.Length - 1] != 0xFD))  //TCPServer处理
            {
                addTcpServerReveData(buf);
                return;
            }
            dealTagLoca(buf);
        }


        private void onHisData(CardUpDataBean showUpCardData, int type) 
        {
            if (onhisTagData != null)
                onhisTagData(showUpCardData, type);
        }

        public void setTimeDate(UInt32 time) 
        {
            hisTime = time;            
        }

        public void setRunTimeDate(UInt32 time) 
        {
            runTime = time;            
        }

        public bool IsTongbuData
        {
            get { return isTongbuData; }
            set { isTongbuData = value; }
        }

        public List<CardUpDataBean> DealUpCardDatas
        {
            get { return dealUpCardDatas; }
            set { dealUpCardDatas = value; }
        }

        public override void reveData(byte[] buf, string ipInfo, CommunicationMode comMode)
        {
            switch (comMode)
            {
                case CommunicationMode.TCPClien:
                    dealTCPClienMessage(buf, ipInfo);
                    break;
                case CommunicationMode.TCPServer:
                    byte[] sendBuf = new byte[buf.Length - 1];
                    Array.Copy(buf, 1, sendBuf, 0, sendBuf.Length);
                    if (buf[0] == 0xfe) 
                    {
                        addEndAndsendData(sendBuf, 0, "");
                    }
                    break;
                case CommunicationMode.TCPServer_loca:
                    if (buf[0] != 0xfe ||  buf[2] != 0xfd)  break;                    
                    if (buf[1] == 0x02)
                        sendDataToCongji(ipInfo);
                    else if (buf[1] == 0x04)
                        sendWarnEleDataToCongji(ipInfo);
                    else if (buf[1] == 0x05)
                        sendWarnCardDataToCongji(ipInfo);
                    else if (buf[1] == 0x06)
                        sendWarnNODEDataToCongji(ipInfo);
                    break;
                case CommunicationMode.TCPClien_loca:
                //    dealTCPClien_loca(ipInfo, buf);
                    break;
                case CommunicationMode.TCPServer_IP:
                    dealTCPServer_IPData(ipInfo, buf);
                    break;
                case CommunicationMode.TCPServer_File:
                    if (buf[0] != 0xfe || buf[2] != 0xfd) break;
                    if (buf[1] == 7) getLocaFileData(ipInfo);
                    else if (buf[1] == 8) getLocaFileCacheData(ipInfo);
                    break;
                default:
                    break;
            }
        }

        private void getLocaFileData(string ipInfo) 
        {
            lock(FileModel.getFlModel().FileNameLock)
            {
                DataFileUtils dfUtils = new DataFileUtils();
                byte[] by = dfUtils.getDataFromFile("data.dat");
                sendDataToTcpServer(by, ipInfo, getTCPServerType(0x00, 0x07));
            }            
        }

        private void getLocaFileCacheData(string ipInfo)
        {
            lock (FileModel.getFlModel().FileName2Lock)
            {
                DataFileUtils dfUtils = new DataFileUtils();
                byte[] by = dfUtils.getDataFromFile("cacheData.dat");
                sendDataToTcpServer(by, ipInfo, getTCPServerType(0x00, 0x08));
            }            
        }

        public void sendTcpType() 
        {
            addEndAndsendData(null, 2, "255.255.255.0");
        }

        /// <summary>
        /// 处理，来自TCPServer_IP的消息
        /// </summary>
        private void dealTCPServer_IPData(string ipInfo, byte[] buf) 
        {
            if (buf.Length != 4) return;                          //数据包长度验证
            if (buf[0] != 0xfe || buf[3] != 0xfc) return;         //包头包尾验证
            if (buf[2] != XWUtils.getCheckBit(buf, 0, 2)) return; //校验验证
            List<byte[]> eleBt = new List<byte[]>();
            switch(buf[1])
            {
                case 0x05:                  
                    WarnMessage.getWarnMessage().getEleWarnBt(ref eleBt);
                    break;
                case 0x06:
                    WarnMessage.getWarnMessage().getCardWarnBt(ref eleBt);
                    break;
                case 0x07:
                    WarnMessage.getWarnMessage().getNODEWarnBt(ref eleBt);
                    break;
                default:
                    return;
                    break;
            }
            sendDataToTcpServer(eleBt, ipInfo);
            byte[] overBt = new byte[4]{0xfd,0x00,0x00,0xfb};
            overBt[1] = (byte)(buf[1] + 0x10);
            overBt[2] = (byte)(0xfd + overBt[1]);
            addEndAndsendData(overBt, 1, ipInfo); //发送结束标志
        }


        private void sendDataToTcpServer(List<byte[]> eleBt,string ipInfo)
        {
            int sendCount = (eleBt.Count / 1000)+1;
            int btCount = eleBt.Count > 0 ? eleBt[0].Length : 0;
            for (int i = 0; i < sendCount;i++ )
            {                
                ///count = 1000，当不够1000，剩下多少是多少
                int count = eleBt.Count > (i + 1) * 1000 ? 1000 : eleBt.Count % 1000;
                byte[] sendDt = new byte[count * btCount];
                for (int j = 0; j < count; j++)
                {
                    int index = i * 1000 + j;
                    if (sendDt.Length < j * btCount) break;
                    Array.Copy(eleBt[index], 0, sendDt, j * btCount, btCount);
                }
                addEndAndsendData(sendDt, 1, ipInfo);
            }
        }


        private void sendDataToTcpServer(byte[] sendData, string ipInfo, byte[] bufType) 
        {
            //封装一包该数据的说明，表明这是一包发送现有的卡片定位数据
            byte[] sendCountData = new byte[bufType.Length + sendData.Length];
            Array.Copy(bufType, 0, sendCountData, 0, bufType.Length);
            Array.Copy(sendData, 0, sendCountData, bufType.Length, sendData.Length);

            addEndAndsendData(getCountBtAddHandle(sendCountData), 1, ipInfo);
        }

        /// <summary>
        /// 发送一份定位数据到从机，让从机同步一下
        /// </summary>
        private void sendDataToCongji(string ipInfo) 
        {
            if (dealUpCardDatas == null) return;
            byte[] sendData = null;
            XWUtils.SerializeObject(dealUpCardDatas, ref sendData);
            if (sendData == null) return;

            sendDataToTcpServer(sendData, ipInfo,getTCPServerType(0x00, 0x03));
        }


        public void dealDataToCongjiLoca(List<CardUpDataBean> dealUpCardDatas) 
        {
            isFileTcpClienModelTongbuData = true;
            Thread.Sleep(100);
            if (dealUpCardDatas == null || dealUpCardDatas.Count == 0) 
            {
                isFileTcpClienModelTongbuData = false;
                return;
            } 
            this.dealUpCardDatas = dealUpCardDatas.ToList();
            isFileTcpClienModelTongbuData = false;
        }

         /// <summary>
        /// 发送一份电量异常数据到从机，让从机同步一下
        /// </summary>
        private void sendWarnEleDataToCongji(string ipInfo)
        {
            Dictionary<string, DrivaceWarnMessage> cardLowEleWarnMsgs = WarnMessage.getWarnMessage().CardLowEleWarnMsgs;
            byte[] sendData = null;
            XWUtils.SerializeObject(cardLowEleWarnMsgs, ref sendData);
            if (sendData == null) return;

            sendDataToTcpServer(sendData, ipInfo, getTCPServerType(0x00, 0x04));
        }



        /// <summary>
        /// 发送一份卡片异常数据到从机，让从机同步一下
        /// </summary>
        private void sendWarnCardDataToCongji(string ipInfo)
        {
            Dictionary<string, DrivaceWarnMessage> cardUnanswerWranMsgs = WarnMessage.getWarnMessage().CardUnanswerWranMsgs;
            byte[] sendData = null;
            XWUtils.SerializeObject(cardUnanswerWranMsgs, ref sendData);
            if (sendData == null) return;

            sendDataToTcpServer(sendData, ipInfo, getTCPServerType(0x00, 0x05));
        }

        /// <summary>
        /// 发送一份节点异常数据到从机，让从机同步一下
        /// </summary>
        private void sendWarnNODEDataToCongji(string ipInfo)
        {
            Dictionary<string, DrivaceWarnMessage> nODEUnanswerWranMsgs = WarnMessage.getWarnMessage().NODEUnanswerWranMsgs;
            byte[] sendData = null;
            XWUtils.SerializeObject(nODEUnanswerWranMsgs, ref sendData);
            if (sendData == null) return;
            sendDataToTcpServer(sendData, ipInfo, getTCPServerType(0x00, 0x06));
        }

        /// <summary>
        /// 处理一下TCPlien一些消息
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="ipInfo"></param>
        private void dealTCPClienMessage(byte[] buf, string ipInfo) 
        {
            if ("closeTcpClien".Equals(ipInfo))
            {
                if (buf.Length == 5 && (buf[0] == 0xff && buf[1] == 0xff && buf[2] == 0xff && buf[3] == 0xfd && buf[4] == 0xfc)) 
                {//断线重连了
                    Debug.Write("斷線重連了\r\n");
                    sendDataToZhuju(tongbuYanZhaneg(), "255.255.255.255"); 
                    return;
                }
                Debug.Write("與主機連接斷開\r\n");
                if (buf.Length != 5 && (buf[0] != 0xff || buf[1] != 0xff || buf[2] != 0xfe || buf[3] != 0xfd || buf[4] != 0xfc)) return;
                if (mainInterface != null) mainInterface.message("與主機連接斷開", 2);
            }
            else
            {
                try 
                { 
                    subcontract(buf);
                }
                catch (Exception e)
                {
                    Debug.Write("FromMainModel.reveData" + e.Message);
                }
            }       
        }

        /// <summary>
        /// 分包
        /// </summary>
        private void subcontract( byte[] value)
        {
            int length = value.Length;
            int packIndex = 0;
            while (length > packIndex)
            {
                if (length < packIndex + 4) break;
                int packLength = value[packIndex] * 0x1000000 + value[packIndex + 1] * 0x10000 + value[packIndex + 2] * 0x100 + value[packIndex + 3];
                if (packLength <= 0 || packIndex + 4 + packLength > length) break;

                byte[] buf = new byte[packLength + 4];
                Array.Copy(value, packIndex, buf, 0, buf.Length);
                try { dealSubcontract(buf); }
                catch (Exception e)
                {
                    Debug.Write("FromMainModel.subcontract" + e.Message);
                }                   
                packIndex += packLength + 4;
            }            
        }

        /// <summary>
        /// 处理一下粘包情况
        /// </summary>
        /// <param name="buf"></param>
        private void dealSubcontract(byte[] buf) 
        {
            byte[] bufs = getCountBtAddHandle(getCountBt());
            if (XWUtils.byteBTBettow(bufs, buf, buf.Length))
            {
                PeoplePowerModel.getPeoplePowerModel().IsConnect = !PeoplePowerModel.getPeoplePowerModel().IsConnect;
                if (mainInterface != null && PeoplePowerModel.getPeoplePowerModel().IsConnect) mainInterface.message("操作成功", 1);
                else if (mainInterface != null && !PeoplePowerModel.getPeoplePowerModel().IsConnect) mainInterface.message("操作成功", 3);
            }
            else if (buf.Length == 12)
            {
                int length = buf.Length;
                if (buf[4] == 0xff && buf[5] == 0xfd && buf[6] == 0xff && buf[7] == 0 && buf[9] == 0xfd && buf[10] == 0xff && buf[11] == 0xfd)
                {
                    if (buf[8] == 1 && mainInterface != null) mainInterface.message("連接失敗，請聯繫主機人員", 0);
                    //else if (buf[8] == 3) addEndAndsendData(null, 2, "255.255.255.0");
                }
            }
            else return;
            addEndAndsendData(null, 2, "255.255.255.0");
        }

        public override void reveData(byte[] buf) {}
 
        public void setListViewCanInfos()   
        {
            List<CanKaoDianBean> ckdBean = FileModel.getFlModel().ChFlBean.CanKaoDians;
            if (ckdBean == null || ckdBean.Count == 0) return;
            if (canKaoDianInfors == null) canKaoDianInfors = new Dictionary<string, CanKaoDianUpInfo>();
            foreach (var item in ckdBean)
            {
                if (!canKaoDianInfors.ContainsKey(item.Id))
                {
                    CanKaoDianUpInfo cInro = new CanKaoDianUpInfo();
                    cInro.CkdId = item.Id;
                    Array.Copy(item.CanDianID, 0, cInro.CID, 0, 2);
                    cInro.Name = item.Name;
                    cInro.UpTime = XwDataUtils.GetTimeStamp();
                    canKaoDianInfors.Add(item.Id, cInro);
                }
            }
        }

        /// <summary>
        /// 此时是历史轨迹功能使用时刻
        /// false不是，true是
        /// </summary>
        public bool IsDealHistory
        {
            get { return isDealHistory; }
            set { isDealHistory = value; }
        }


        private void packJieXi(byte[] buf,string ipInfo) 
        {
            if (buf.Length < locationPack || (buf[0] != 0XFE || buf[buf.Length - 1] != 0xFD)) 
            {
                dealNode(buf);
                return; //TCPServer处理 
            }
            if (buf.Length < locationPack || buf[0] != 0XFE || buf[buf.Length - 1] != 0xFD || ipInfo == null)
            {
                return;
            }
            byte[] tagID = new byte[2];
            Array.Copy(buf, 2, tagID, 0, 2);
            byte[] idRess = new byte[3];
            Array.Copy(buf, 4, idRess, 0, 3);
            IPIDRessBean ipIdBean = new IPIDRessBean();
            ipIdBean.IpKey = ipInfo;
            ipIdBean.IpKeyBt = Encoding.UTF8.GetBytes(ipInfo);
            ipIdBean.TagID = tagID;
            ipIdBean.PortIDRes = idRess;         
            addUdpIp(ipIdBean);            
        }

        private void onCanKDataISS(Dictionary<string, CanKaoDianBean> canDictionarys) 
        {
            if (onCanKData != null) onCanKData(canDictionarys);
        }

        private void dealNode(byte[] buf) 
        {
            if (buf.Length != 14 || buf[1] != 1 || buf[0] != 0XFE || buf[buf.Length - 1] != 0xFD) return;
            string canID = buf[2].ToString("X2") + buf[3].ToString("X2");
            if (canKaoDianInfors.ContainsKey(canID))
            {
                if (canKaoDianInfors[canID].UpTime + 120 < XwDataUtils.GetTimeStamp())
                {
                    WarnMessage.getWarnMessage().removeNODEUnanswerMessage(canID);
                }
                canKaoDianInfors[canID].UpTime = XwDataUtils.GetTimeStamp();
                Array.Copy(buf, 4, canKaoDianInfors[canID].NodeType, 0, 4);
                Array.Copy(buf, 8, canKaoDianInfors[canID].Version, 0, 4);
            }
            else
            {
                CanKaoDianUpInfo canDianInfo = new CanKaoDianUpInfo();
                byte[] NODEid = new byte[2];
                NODEid[0] = buf[2];
                NODEid[1] = buf[3];
                Array.Copy(NODEid, 0, canDianInfo.CID, 0, 2);
                canDianInfo.CkdId = canID;
                canDianInfo.UpTime = XwDataUtils.GetTimeStamp();
                Array.Copy(buf, 4, canDianInfo.NodeType, 0, 4);
                Array.Copy(buf, 8, canDianInfo.Version, 0, 4);
                List<CanKaoDianBean> canKaoDians = FileModel.getFlModel().ChFlBean.CanKaoDians;
                var canS = canKaoDians.Where(a => a.Id.Equals(canID));
                if (canS.Count() > 0) canDianInfo.Name = canS.First().Name;
                else canDianInfo.Name = canID;
                canKaoDianInfors.Add(canID, canDianInfo);

                WarnMessage.getWarnMessage().removeNODEUnanswerMessage(canID);
            }
            onNOData(canID);
        }

        public void onNOData(string nodeID)  //节点信息
        {
            if (onNODEData != null && canKaoDianInfors != null)  //將節點信息發送出去
            {
                onNODEData(nodeID,canKaoDianInfors);
            }
        }

        private void dealNoAnswerNODE(uint timeOver) 
        {
            if(canKaoDianInfors == null || canKaoDianInfors.Count == 0) return;
            foreach (var Item in canKaoDianInfors)
            {
                if (CloseUDPtime > OpenUDPtime) Item.Value.UpTime++;
                if (Item.Value.UpTime + timeOver < XwDataUtils.GetTimeStamp() && !IsDealHistory)
                    WarnMessage.getWarnMessage().addNODEUnanswerWranMsgs(Item.Value.CkdId, timeOver, "", Item.Value.CkdId, Item.Value.UpTime);
            }
        }

        private void dealTagLoca(byte[] buf) 
        {
            if (buf.Length != locationPack) return;
            ///////////////UDP数据处理
            if (dealCardData(buf)) return;           
            CardUpDataBean cardBean = addNewCard(buf);
            cardBean.Port1IDHistory = new byte[2] { buf[4], buf[5] };
            cardBean.Port1RssiHistory = buf[6];
            cardBean.TotalCount = 1;
            cardBean.LostCount = 0;
            if (dealUpCardDatas != null) 
            {
                int index = getCardUpDataIndex(cardBean);
                if (index == -1) dealUpCardDatas.Add(cardBean);
                else if (index > -1) dealUpCardDatas.Insert(index, cardBean);                     
            }
            if (dealUpCardDatas.Count > 5000)
            {
                if (dealUpCardDatas.Count > 10000) dealUpCardDatas.Clear();
                if (dealUpCardDatas.Count % 5000 == 0) FileModel.getFlModel().setErrorAppData("FromMainModel.dealTagLoca() at dealUpCardDatas > 5000");
            }

            WarnMessage.getWarnMessage().removeCardUnanswerMessage(cardBean.TagIdStr);
        }


        /// <summary>
        /// 利用二分法，插入数据时，将数据插入
        /// </summary>
        /// <param name="cardBean"></param>
        /// <returns></returns>
        private int getCardUpDataIndex(CardUpDataBean cardBean)
        {
            if(dealUpCardDatas.Count < 1) return 0; //没有值
            if(dealUpCardDatas[dealUpCardDatas.Count - 1].TagID_Int < cardBean.TagID_Int) return -1;//比最大值大
            if (dealUpCardDatas[0].TagID_Int > cardBean.TagID_Int) return 0;//比最小值小
            int hei = dealUpCardDatas.Count;
            int low = 0;
            while (hei > low) 
            {
                int mid = (hei + low)/2;
                if (cardBean.TagID_Int == dealUpCardDatas[mid].TagID_Int) return -3;
                else if (cardBean.TagID_Int < dealUpCardDatas[mid].TagID_Int)
                {
                    if (dealUpCardDatas[mid - 1].TagID_Int < cardBean.TagID_Int) return mid; //就是它
                    hei = mid;
                } else 
                {
                    if (dealUpCardDatas[mid - 1].TagID_Int > cardBean.TagID_Int) return mid;
                    low = mid;
                }
            }
            return -2;
        }


        private void addUdpIp(IPIDRessBean ipIdBean) 
        {         
            addUpIpDictionary(ipIdBean);
        }

        private void addUpIpDictionary(IPIDRessBean ipIdBean) 
        {
            if (ipIdBean.TagIDStr.Length != 0 && !ipBeanDictionarys.ContainsKey(ipIdBean.TagIDStr)) 
            {
                ipBeanDictionarys.Add(ipIdBean.TagIDStr, ipIdBean);
                if (ipBeanDictionarys.Count > 10000) ipBeanDictionarys.Clear();
                if (ipBeanDictionarys.Count % 1000 == 0) 
                    FileModel.getFlModel().setErrorAppData("FromMainModel.addUpIpDictionary() at ipBeanDictionarys > 1000");
            }
            string portID = ipIdBean.PortIDRes[0].ToString("X2") + ipIdBean.PortIDRes[1].ToString("X2");
            if (!showIpBeanDictionarys.ContainsKey(portID))
            {
                showIpBeanDictionarys.Add(portID, ipIdBean.IpKey);
                if (showIpBeanDictionarys.Count > 10000) showIpBeanDictionarys.Clear();
                if (showIpBeanDictionarys.Count % 1000 == 0)
                    FileModel.getFlModel().setErrorAppData("FromMainModel.addUpIpDictionary() at showIpBeanDictionarys > 1000");
            }            
        }

        private void addCanKaoDianInfo(string portID) 
        {
            if (portID.Equals("0000") || portID.Length != 4) return;
         
            if (canKaoDianDictionarys.ContainsKey(portID))
                canKaoDianDictionarys[portID].TimeReceive = XwDataUtils.GetTimeStamp();
            else
            {
                CanKaoDianBean canBean = new CanKaoDianBean();
                canBean.TimeReceive = XwDataUtils.GetTimeStamp();
                canBean.Id = portID;
                canKaoDianDictionarys.Add(portID, canBean);
            }
        }

        public void addTcpServerReveData(byte[] buf)
        {
            if (buf[0] != 0XFE || buf[buf.Length - 1] != 0xFC || buf.Length < 4) return;
            byte[] cardID = new byte[2] { buf[2], buf[3] };
            switch(buf[1])
            {
                case 0x01:
                    if (buf.Length != 6) return;
                    sendCnKaoDian(cardID);
                    break;
                case 0x02:
                    if(buf.Length != 6) return;                   
                    sendCardData(cardID);
                    break;
                case 0x03:
                    if (buf.Length != 14) return;
                    long startTime = getTime(buf[4],buf[5],buf[6],buf[7]);
                    long endTime = getTime(buf[8], buf[9], buf[10], buf[11]);
                    dealHistoryCardData(cardID, startTime, endTime);
                    break;
                case 0x04:
                    if (buf.Length < 11) return;
                    dealFourData(buf);
                    break;
            }
        }

        private void dealFourData(byte[] buf) 
        {
            if (buf[2] != 0xfe || buf[3] != 0x04) return;
            byte[] fourByte = new byte[7];
            Array.Copy(buf, 2, fourByte,0,7);   //去读原始数据
            byte[] cardID = new byte[2] { fourByte[2], fourByte[3] };

            if (!checkPacket(fourByte,null)) return;
            byte[] keyBy = new byte[buf.Length - 11]; //去读key字节
            Array.Copy(buf, 9, keyBy, 0, buf.Length - 11);
            lock(objLock)
            {
                addKey(keyBy, cardID, fourByte[4]);
            }            
        }

        private void addKey(byte[] keyBy, byte[] cardID, byte getType) 
        {
            if (keyBys == null) keyBys = new Dictionary<string, byte[]>();
            string key = Encoding.UTF8.GetString(keyBy);
            if (!keyBys.ContainsKey(key) && getType == 0) keyBys.Add(key, cardID);
            else if (keyBys.ContainsKey(key) && getType == 0) keyBys[key] = cardID;
            else if (keyBys.ContainsKey(key) && getType != 0) keyBys.Remove(key);
            if (keyBys.Count % 1000 == 0 ) 
            {
                if (keyBys.Count > 10000) keyBys.Clear();
                if (keyBys.Count > 0) FileModel.getFlModel().setErrorAppData("FromMainModel.addKey() at keyBys > 1000");
            }
        }

        //4个字节的时间，转换成时间戳
        private long getTime(long time1, long time2, long time3, long time4) 
        {
            long time = 0;
            time += time1 * 0x1000000;
            time += time2 * 0x10000;
            time += time3 * 0x100;
            time += time4;
            return time;
        }

        private void dealHistoryCardData(byte[] cardID,long startTime,long endTime) 
        {
            if (endTime < startTime || endTime < 1 || cardID == null) return;
            List<HistoryFileDataBean> hisFileDBeans = FileModel.getFlModel().HisFileDBeans.ToList();
            if (hisFileDBeans == null) return;
            foreach (HistoryFileDataBean hfItems in hisFileDBeans)
            {
                if (hfItems.StartTime > endTime || (hfItems.EndTime < startTime && hfItems.EndTime != 0)) continue;
                List<byte[]> chcheBy = FileModel.getFlModel().getHisFileListDataData(hfItems.FilePath + "\\" + hfItems.FileName,0);
                sendCardHisData(cardID, startTime, endTime, chcheBy);
            }            
            sendOverByte(cardID, 0x13);  
        }

        private void sendCardHisData(byte[] cardID, long startTime, long endTime, List<byte[]> chcheBy) 
        {
            if (chcheBy == null) return;
            if (cardID == null && cardID.Length != 2) return;   
            List<byte[]> canKaoDianS = new List<byte[]>();
            string cID = cardID[0].ToString("X2") + cardID[1].ToString("X2");
            foreach (byte[] cBean in chcheBy)
            {
                if ((cardID[0] != 0x00 || cardID[1] != 0x00) && (cardID[0] != cBean[2] || cardID[1] != cBean[3])) continue;
                long cBeanTime = getTime(cBean[4], cBean[5], cBean[6], cBean[7]);
                if (startTime > cBeanTime) continue;
                if (endTime < cBeanTime) break; //约往后循环，cBeanTime值越大，这个值是递增的，故没必要遍历下去
                canKaoDianS.Add(cBean);
            }
            sendTcpServerData(canKaoDianS, cardID);
        }


        private void sendTcpServerData(List<byte[]> byteS, byte[] cardID) 
        {
            if (byteS == null || byteS.Count == 0) return;
            sendCardData(byteS,2);            
        }


        private byte[] getCardUpByte(CardUpDataBean cBean) 
        {
            byte[] cInfor = new byte[19];

            cInfor[0] = 0xfd;
            cInfor[1] = 0x03;
            Array.Copy(cBean.TagId, 0, cInfor, 2, 2);
            Array.Copy(cBean.FirstTimeByte, 0, cInfor, 4, 4);
            Array.Copy(cBean.Port1ID, 0, cInfor, 8, 2);
            cInfor[10] = cBean.Port1Rssi;
            Array.Copy(cBean.Port2ID, 0, cInfor, 11, 2);
            cInfor[13] = cBean.Port2Rssi;
            Array.Copy(cBean.Port3ID, 0, cInfor, 14, 2);
            cInfor[16] = cBean.Port2Rssi;

            cInfor[cInfor.Length - 2] = XWUtils.getCheckBit(cInfor, 0, cInfor.Length - 2);
            cInfor[cInfor.Length - 1] = 0xfb;
            return cInfor;
        } 


        private void sendCnKaoDian(byte[] canKaoDianID) 
        {
            if (canKaoDianID == null && canKaoDianID.Length != 2) return;
            List<CanKaoDianBean> canKaoDianBeans = FileModel.getFlModel().ChFlBean.CanKaoDians;
            List<byte[]> canKaoDianS = new List<byte[]>();
            string canKaoID = canKaoDianID[0].ToString("X2") + canKaoDianID[1].ToString("X2");

            foreach (CanKaoDianBean cBean in canKaoDianBeans)
            {
                if (canKaoDianID[0] == 0x00 && canKaoDianID[1] == 0x00)
                    canKaoDianS.Add(getCanKaoDdianData(cBean));
                else if (cBean.Id.Equals(canKaoID))
                {
                    canKaoDianS.Add(getCanKaoDdianData(cBean));
                    break;
                }
            }
            sendCardData(canKaoDianS,20);
            sendOverByte(canKaoDianID,0x11);  
        }


        /// <summary>
        /// 回传卡片资料
        /// </summary>
        /// <param name="cardID"></param>
        private void sendCardData(byte[] cardID) 
        {
            if(cardID == null && cardID.Length != 2) return;           
            string cardIDStr = cardID[0].ToString("X2") + cardID[1].ToString("X2");
            Dictionary<string, CardBean> cardDic = FileModel.getFlModel().ChFlBean.CardDic;
            List<Byte[]> cardBs = new List<byte[]>();
            if (cardDic != null && cardDic.ContainsKey(cardIDStr)) 
            {
                var crBean = cardDic[cardIDStr];
                if (cardID[0] == 0x00 && cardID[1] == 0x00)
                    cardBs.Add(getCardIDInfor(crBean));
                else if (crBean.Id.Equals(cardIDStr))
                {
                    cardBs.Add(getCardIDInfor(crBean));
                    //break;
                }  
            }
            if (cardID[0] == 0x00 && cardID[1] == 0x00)
            {
                List<CardBean> cardBns = FileModel.getFlModel().ChFlBean.Cards.ToList();
                foreach (CardBean crBean in cardBns)
                {
                    if (cardID[0] == 0x00 && cardID[1] == 0x00)
                        cardBs.Add(getCardIDInfor(crBean));
                }         
            }                     
            sendCardData(cardBs,20);
            sendOverByte(cardID,0x12);           
        }
        

        /// <summary>
        /// 准备好数据后，发送
        /// </summary>
        /// <param name="cardBs"></param>
        private void sendCardData(List<Byte[]> cardBs,int toLangth) 
        {
            if (cardBs == null|| cardBs.Count == 0) return;

            int byteLength = (cardBs[0].Length + toLangth) * 1000;

            int countThouend = cardBs.Count / 1000;
            int length = 0;
            for (int i = 0; i < countThouend + 1;i++ )
            {
                byte[] mByte = new byte[byteLength];
                int count = cardBs.Count - i * 1000 >= 1000 ? 1000 : cardBs.Count - i * 1000;
                for (int i_c = 0; i_c < count;i_c++ )
                {
                    Array.Copy(cardBs[i_c + i*1000], 0, mByte, length, cardBs[i_c].Length);
                    length += cardBs[i_c].Length;
                }
                byte[] mByte2 = new byte[length];
                Array.Copy(mByte, 0, mByte2, 0, length);
                length = 0;
                addEndAndsendData(mByte2,1,"");
            }                                  
        }

        //发送 结束包
        private void sendOverByte(byte[] cardID,byte packType) 
        {
            byte[] overByte = new byte[6] { 0xfd, packType, cardID[0], cardID[1], 0x00, 0xfb };
            overByte[4] = XWUtils.getCheckBit(overByte, 0, overByte.Length - 2);
            try
            {
                Thread.Sleep(5);
            }
            catch { }
            addEndAndsendData(overByte,1,"");
        }

        //将卡片资料转换成
        private byte[] getCardIDInfor(CardBean crBean) 
        {
            byte[] nameInfo = Encoding.UTF8.GetBytes(crBean.Name);//crBean.Name
            byte[] cardInfor = new byte[6 + nameInfo.Length];
            cardInfor[0] = 0xfd;
            cardInfor[1] = 0x02;
            Array.Copy(crBean.CardID, 0, cardInfor,2,2);
            Array.Copy(nameInfo, 0, cardInfor, 4, nameInfo.Length);

            cardInfor[cardInfor.Length - 2] = XWUtils.getCheckBit(cardInfor, 0, cardInfor.Length - 2);
            cardInfor[cardInfor.Length - 1] = 0xfb;
            return cardInfor;
        }

        /// <summary>
        /// 将参考点资料转换成byte数组
        /// </summary>
        /// <param name="cBean"></param>
        /// <returns></returns>
        private byte[] getCanKaoDdianData(CanKaoDianBean cBean) 
        {
            byte[] nameInfo = Encoding.UTF8.GetBytes(cBean.Name);//crBean.Name
            byte[] cInfor = new byte[10 + nameInfo.Length];

            cInfor[0] = 0xfd;
            cInfor[1] = 0x01;
            Array.Copy(cBean.CanDianID, 0, cInfor, 2, 2);
            cInfor[4] = (byte)(cBean.POint.X / 0x100);
            cInfor[5] = (byte)(cBean.POint.X % 0x100);
            cInfor[6] = (byte)(cBean.POint.Y / 0x100);
            cInfor[7] = (byte)(cBean.POint.Y % 0x100);

            Array.Copy(nameInfo, 0, cInfor, 8, nameInfo.Length);

            cInfor[cInfor.Length - 2] = XWUtils.getCheckBit(cInfor, 0, cInfor.Length - 2);
            cInfor[cInfor.Length - 1] = 0xfb;
            return cInfor;
        }


        /// <summary>
        /// 新來一張卡片的信息
        /// </summary>
        /// <param name="buf"></param>
        private CardUpDataBean addNewCard(byte[] buf) 
        {
            CardUpDataBean cardBean = new CardUpDataBean();
            cardBean.TagId = new byte[2] { buf[2], buf[3] };
            cardBean.TagID_Int = buf[2] * 0x100 + buf[3];
            cardBean.MType = getTypeFromPack(buf[1]);
            cardBean.DrivaceType = buf[1];
            cardBean.Port1ID = new byte[2] { buf[4], buf[5] };
            cardBean.Port1ID_Int = buf[4] * 0x100 + buf[5];
            cardBean.Port1Rssi = buf[6];
            UInt16 heiSTime = (UInt16)(buf[7] << 8);
            cardBean.SensorTime = (UInt16)(heiSTime + buf[8]);
            cardBean.Battery = buf[9];
            cardBean.SleepTime = buf[10];
            cardBean.LEDStaus = buf[11];
            cardBean.IsUpdate = true;
            cardBean.ReceivData = true;
            cardBean.SendGongLv = buf[12];
            cardBean.Index = buf[13];
            cardBean.FirstReceiveTime = getCurrenHisTime();

            cardBean.TotalCount = 0;
            cardBean.LostCount = 0;
            return cardBean;          
        }

        private byte getTypeFromPack(byte bufType) 
        {
            if (bufType == 0x00 || bufType == 0x02 || bufType == 0x04 || bufType == 0x06 || bufType == 0x08)
                return 1;
            else if (bufType == 0x01 || bufType == 0x03 || bufType == 0x05 || bufType == 0x07 || bufType == 0x09)
                return 2;
            return 0;
        }

        private bool dealCardData(byte[] buf) 
        {
            byte[] TagID = new byte[2]{buf[2],buf[3]};
            int index = getCardBeanIndex(buf[2] * 0x100 + buf[3]);
            if (index < 0) return false;   
       
            dealCardInList(buf, dealUpCardDatas[index]);
            return true;
        }


        private int getCardBeanIndex(int tagIDIndex) 
        {
            if (dealUpCardDatas == null || dealUpCardDatas.Count == 0) return -1;
            //int count = 0;
            int low = 0;
            int hei = dealUpCardDatas.Count;
            while(hei > low)
            {
                int mid = (low + hei) / 2;
                if (tagIDIndex == dealUpCardDatas[mid].TagID_Int) return mid;
                else if (tagIDIndex > dealUpCardDatas[mid].TagID_Int)
                {
                    if (low != mid)
                        low = mid;
                    else low++;
                } else 
                {
                    if (hei != mid)
                        hei = mid;
                    else hei--;
                }               
            }
            return -2;
        }


        private void distriHandle(object obj) 
        {
            List<CardUpDataBean> showUpCardDataCaches = (List<CardUpDataBean>)obj;
            if (onTagData != null)
            {
                try 
                {
                    onTagData(showUpCardDataCaches);   //dealUpCardDatas.ToList()
                }
                catch { }                                            
            }
            if(!IsDealHistory) onCanKDataISS(canKaoDianDictionarys);
        }


        public void dirHandle() 
        {
            distriHandle(dealUpCardDatas.ToList());
        }


        /// <summary>
        /// 通過查到的卡片，返回對應的參考點，通過參考點信息獲取其他信息
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public byte[] sercahCard(string card) 
        {
            string tagIDsTR = "";
            List<CardBean> cardBns = FileModel.getFlModel().ChFlBean.Cards.ToList();
            var cardB = cardBns.Where(tag => tag.Name.Equals(card));
            if (cardB.Count() > 0) tagIDsTR = cardB.First().Id;

            byte[] tagID = sercahCardInID(card.ToUpper(),tagIDsTR);
            return tagID;
        }

        private byte[] sercahCardInID(string card1,string card2) 
        {
            byte[] cardID = null;
            try 
            {
                List<CardUpDataBean> dealUpCardDatasCache = dealUpCardDatas.ToList();
                var cardItem = dealUpCardDatasCache.Where(item => item.TagIdStr.Equals(card1) || item.TagIdStr.Equals(card2));
                if (cardItem.Count() > 0)
                {
                    cardID = new byte[2];
                    cardID[0] = cardItem.First().Port1ID[0];
                    cardID[1] = cardItem.First().Port1ID[1];
                }
            }
            catch { }            
            return cardID;
        }

        private object objShowTag = new object();
        private int noMoveTime;

        private int cout = 0;
        //检查tag状况
        public void checkShowPoint()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(dealShowPoint));    //参数可选                
        }

        //周期性处理卡片的资讯
        private void dealShowPoint(object state) 
        {
            dealTimeOverNOED();
            if (dealUpCardDatas.Count < 0) return;   
            try
            { 
                dealTimeOverCard();
                var cardItem = dealUpCardDatas.ToList();
                dataCallBack(cardItem);
                if (cardItem.Count() < 1) return;
                ThreadPool.QueueUserWorkItem(new WaitCallback(distriHandle), cardItem);    //参数可选      
            }
            catch (Exception e) 
            {                
                Debug.Write("======"+e.ToString());
            }            
        }


        /// <summary>
        /// 处理节点超时
        /// </summary>
        private void dealTimeOverNOED()
        {
            if (IsDealHistory || OpenUDPtime < CloseUDPtime) return;
            if (cout >= 10)
            {
                uint timeOver = 300;
                if (PeoplePowerModel.getPeoplePowerModel().Jurisdiction == PeoplePowerModel.getPeoplePowerModel().CongjiValue)
                {
                    timeOver = 600;
                }
                dealNoAnswerNODE(timeOver);
                cout = 0;
            }
            else
                cout++;     
        }


        /// <summary>
        /// 处理卡片超时
        /// </summary>
        private void dealTimeOverCard() 
        {
            var cardItem = dealUpCardDatas.ToList();
            try 
            {
                foreach (var item in cardItem)
                {
                    if (CloseUDPtime > OpenUDPtime) item.FirstReceiveTime++;
                    if (item.FirstReceiveTime + FileModel.getFlModel().ChFlBean.NoReveTime > getCurrenHisTime()) continue;
                    int index = getCardBeanIndex(item.TagID_Int);
                    CardUpDataBean cardBean = new CardUpDataBean(dealUpCardDatas[index]); 
                    ThreadPool.QueueUserWorkItem(new WaitCallback(warnCardUnAnswerMsg), cardBean);    //报警处理放入线程池
                    dealUpCardDatas.RemoveAt(index);                    
                }   
            }
            catch (Exception e) 
            {
                Debug.Write("dealTimeOverCard() >> " + e.ToString());
            }          
        }

        private UInt32 getCurrenHisTime()
        {
            if (IsDealHistory)
                return hisTime;
                
            else return XwDataUtils.GetTimeStamp();
        }

        private UInt32 getCurrenRunTime()
        {
            if (IsDealHistory)
                return runTime;               
            else return XwDataUtils.GetTimeStamp();
        }

        /// <summary>
        /// 卡片超时未上报，报警处理
        /// </summary>
        /// <param name="cardBean"></param>
        private void warnCardUnAnswerMsg(Object obj) 
        {
            if (IsDealHistory) return;
            CardUpDataBean cardBean = (CardUpDataBean)obj;
            if (!(obj is CardUpDataBean)  || cardBean == null) return;
            long upTime = (long)cardBean.FirstReceiveTime;
            if (isDealHistory) return;
            WarnMessage.getWarnMessage()
                .addCardUnanswerMessage(cardBean.TagIdStr, cardBean.SleepTime, "", cardBean.Port1IDHistoryStr, upTime);
        }

        private List<CardUpDataBean> getDealUpCardDatas() 
        {
            List<CardUpDataBean> cardItem = new List<CardUpDataBean>();
            for (int i = 0; i < dealUpCardDatas.Count;i++ )
            {
                cardItem.Add(dealUpCardDatas[i]);
            }
            return cardItem;
        }

        /// <summary>
        /// 实时回传人数等一些数据
        /// </summary>
        private void dataCallBack(List<CardUpDataBean> showUpCardDataCaches) 
        {
            if (IsDealHistory) return;
            ThreadPool.QueueUserWorkItem(new WaitCallback(dataCallBackThread), showUpCardDataCaches);    //参数可选
        }

        private void dataCallBackThread(object obj) //線程,返回正确的首页人数统计，计算循环
        {
            if (obj == null || !(obj is List<CardUpDataBean>)) return;
            List<CardUpDataBean> showUpCardDataCache = (List<CardUpDataBean>)obj;
            if (mainInterface != null && showUpCardDataCache != null) mainInterface.onPeopleAllCount(showUpCardDataCache.Count);
            if (mainInterface != null && showUpCardDataCache != null) 
                mainInterface.onQuyuPeopleCount(getCanKaobeans(showUpCardDataCache));             
        }

        /// <summary>
        /// 寻找合适的CanKaoDianBean集合，这个集合是每个区域中挑选的有且一个参考点作为代表
        /// </summary>
        /// <returns></returns>
        private List<CenJiBean> getCanKaobeans(List<CardUpDataBean> showUpCardDataCache) 
        {
            setPeopleCount(showUpCardDataCache);      
            List<CanKaoDianBean> canKaoBns = FileModel.getFlModel().ChFlBean.CanKaoDians.ToList();
            List<CenJiBean> cLit = FileModel.getFlModel().CenJiData.ToList();         
            foreach (CenJiBean cjItem in cLit)  ///这个嵌套循环，是为了找出区域中的一个随机代表参考点
            {
                if (cjItem.QuYuBeans == null) continue;
                foreach (QuYuBean quYuItem in cjItem.QuYuBeans)
                {
                    quYuItem.PepleCount = 0;
                    foreach (CanKaoDianBean cItem in canKaoBns)
                    {
                        if(quYuItem.QuyuID.Equals(cItem.QuYuID) && cjItem.ID.Equals(cItem.CenJiID))
                        {
                            quYuItem.PepleCount += cItem.PeopleCount;
                        }                            
                    }
                }
            }
            return cLit;
        }

        /// <summary>
        /// 为每个参考点的人数记录一下,记录之前会先将人数清零
        /// </summary>
        /// <returns></returns>
        private void setPeopleCount(List<CardUpDataBean> showUpCardDataCache) 
        {           
            List<CanKaoDianBean> canKaoBns = FileModel.getFlModel().ChFlBean.CanKaoDians;
            foreach (CanKaoDianBean ckBean in canKaoBns)
            {
                ckBean.PeopleCount = 0;
                foreach (CardUpDataBean cupBean in showUpCardDataCache)
                {
                    if (cupBean == null) continue;
                    if (XWUtils.byteBTBettow(cupBean.Port1IDHistory, ckBean.CanDianID, ckBean.CanDianID.Length))
                    {
                        ckBean.PeopleCount++;
                    }
                }//  foreach  showUpCardDatas                
            }          
        }

        //文件存储
        private void cacheData() 
        {
            if (PeoplePowerModel.getPeoplePowerModel().Jurisdiction == PeoplePowerModel.getPeoplePowerModel().CongjiValue) return;
            if (oldUpCardDatas != null && oldUpCardDatas.Count < 10000 && XwDataUtils.GetTimeStamp() - oldUpCardDatas[0].FirstReceiveTime < 60) return;

            List<CardUpDataBean> oldUpCardDataCache = oldUpCardDatas.ToList();
            oldUpCardDatas.Clear();

            new Thread(cacheDataThread).Start(oldUpCardDataCache);           
        }

        //文件存储线程
        private void cacheDataThread(object obj) 
        {
            if(obj == null) return;
            if (!(obj is List<CardUpDataBean>)) return;
            List<CardUpDataBean> oldUpCardData = (List<CardUpDataBean>)obj;
            List<CardUpDataBean> oldUpCardDataCache = oldUpCardData.ToList();
            //oldUpCardData.Clear();
            if (FileModel.getFlModel().OldUpCardDatas.Count != 0) FileModel.getFlModel().OldUpCardDatas.Clear();
            FileModel.getFlModel().OldUpCardDatas.AddRange(oldUpCardDataCache);
            int dataCount = FileModel.getFlModel().OldUpCardDatas.Count;
            if (dataCount == 0) return;            
            long starTime = FileModel.getFlModel().OldUpCardDatas[0].FirstReceiveTime;            
            List<HistoryFileDataBean> hisFileDBeans = FileModel.getFlModel().HisFileDBeans;
            bool isHaveItem = false;
            if (hisFileDBeans != null) 
            {
                List<HistoryFileDataBean> hisFileDBeanForch = hisFileDBeans.ToList();
                foreach (HistoryFileDataBean hfItems in hisFileDBeanForch)
                {
                    if (!hfItems.IsCache) continue;
                    isHaveItem = true;
                    try 
                    {
                        hfItems.EndTime = FileModel.getFlModel().OldUpCardDatas[dataCount - 1].FirstReceiveTime;
                        cahceCardData(hfItems, dataCount, starTime);     
                    }
                    catch { }                                  
                }
            } //else            
            if (!isHaveItem) 
            {
                HistoryFileDataBean hBean = new HistoryFileDataBean(FileModel.getFlModel().OldUpCardDatas[0].FirstReceiveTime,
                    XwDataUtils.currentMonthTime(), FileModel.getFlModel().OldUpCardDatas[dataCount - 1].FirstReceiveTime);               
                hisFileDBeans.Add(hBean);
                cahceCardData(hBean, dataCount, starTime);
            }
        }

        //存储卡片信息
        private void cahceCardData(HistoryFileDataBean hfItems,int cacheCount,long firstTime) 
        {
            if (hfItems == null) return;
            FileModel.getFlModel().setHisFileData(hfItems.FilePath + "\\" + hfItems.FileName);
            hfItems.FileSize += cacheCount;
            if (hfItems.FileSize > 90000 || !XwDataUtils.oneDayTime(hfItems.StartTime, firstTime)) hfItems.IsCache = false;
            FileModel.getFlModel().setHisFileInfore();
        //    Debug.Write("....FileSize..." + hfItems.FileSize);
        //    Debug.Write("....地址..." + hfItems.FilePath + "\\" + hfItems.FileName+"\r\n\r\n");
        }

        //处理UDP消息
        private void dealCardInList(byte[] buf,CardUpDataBean cItem) 
        {
            cItem.TotalCount++;
            if (cItem.Index != buf[13]) 
            {
                if (cItem.Index + 1 != buf[13]) 
                {
                    if (buf[13] < cItem.Index)
                        cItem.LostCount += buf[13] + 255 - cItem.Index;
                    else
                        cItem.LostCount += buf[13] - cItem.Index - 1;
                }
                if (isDealHistory) 
                {
                    CardUpDataBean cardNewHisBean = new CardUpDataBean(cItem);
                    onHisData(cardNewHisBean, 1);
                }                   
                int titleCount = cItem.TotalCount;
                int lostCount = cItem.LostCount;
                try 
                {                    
                    sendTcpServerInfor(cItem);
                    saveHisCardData(cItem);
                    cardJump(cItem);
                }
                catch(Exception e)
                {
                    Debug.Write("FromMainModel.dealCardInList"+e.Message);
                }
                CardUpDataBean cardNewBean = addNewCard(buf);
                cItem.changeStr(cardNewBean);   //展示新的数据 

                addCanKaoDianInfo(cItem.Port1IDStr);
                addCanKaoDianInfo(cItem.Port2IDStr);
                addCanKaoDianInfo(cItem.Port3IDStr);  
                cItem.TotalCount = titleCount;
                cItem.LostCount = lostCount;
                showNewIp(cItem);
                if (cItem.Battery <= FileModel.getFlModel().ChFlBean.TagLow && !isDealHistory)//报警处理放入线程池
                    ThreadPool.QueueUserWorkItem(new WaitCallback(lowEleWarn), cItem);
                else {
                    WarnMessage.getWarnMessage().removeCardLowEleMessage(cItem.Battery, cItem.TagIdStr);
                }
                return;
            }
            byte ress = buf[6];
            byte[] ID = new byte[2] { buf[4], buf[5] };
            if (ress < cItem.Port1Rssi) 
            {
                changeRess(cItem, 1);
                cItem.Port1ID = ID;
                cItem.Port1Rssi = ress;
            }
            else if (ress < cItem.Port2Rssi) 
            {
                changeRess(cItem, 2);
                cItem.Port2ID = ID;
                cItem.Port2Rssi = ress;
            }
            else if (ress < cItem.Port3Rssi) 
            {
                cItem.Port3ID = ID;
                cItem.Port3Rssi = ress;
            }                            
        }

        /// <summary>
        /// 报警系统之低电量报警
        /// </summary>
        /// <param name="obj"></param>
        private void lowEleWarn(object obj) 
        {
            CardUpDataBean cardNewBean = (CardUpDataBean)obj;
            CardUpDataBean cardBean = new CardUpDataBean(cardNewBean);
            cardBean.changeStr(cardNewBean);   //展示新的数据 

            if (!(obj is CardUpDataBean) || cardBean == null) return;
            // if(cardBean.Port1IDHistory[0] == 0 && cardBean.Port1IDHistory[1] == 0) return;
            // string quyuName = getquYuName(cardBean.Port1IDHistoryStr);
            // if("".Equals(quyuName)) return;
            WarnMessage.getWarnMessage().addcardLowEleMessage(cardBean.TagIdStr, cardBean.Battery,
                "", cardBean.Port1IDHistoryStr, (long)cardBean.FirstReceiveTime);
        }


        private String getquYuName(string canKaiID) 
        {
            List<CanKaoDianBean> CanKaoDians = FileModel.getFlModel().ChFlBean.CanKaoDians.ToList();
            var can = CanKaoDians.Where(a => a.Id.Equals(canKaiID));
            if (can.Count() > 0) 
            {
                return can.First().QuYuname;
            }
            return "";
        }

        //卡片跳点，符合客户设置的核查次数，就跳点  青玉，白玉
        private void cardJump(CardUpDataBean cItem) 
        {
            if (cItem.PortID[0] == 0 && cItem.PortID[1] == 0)
            {
                cItem.PortID = cItem.Port1ID;
            }                
            else if (cItem.PortID[0] != cItem.Port1ID[0] || cItem.PortID[1] != cItem.Port1ID[1])
            {
                cItem.ChangeIndex++;
                if (cItem.ChangeIndex >= FileModel.getFlModel().ChFlBean.CheckC + 1)
                {
                    cItem.ChangeIndex = 1;
                    cItem.PortID = cItem.Port1ID;
                    changeHisData(cItem);
                }
            }
            else 
            {
                cItem.ChangeIndex = 1;
                changeHisData(cItem);
            }        
        }

        private void changeHisData(CardUpDataBean cItem) 
        {
            if (cardHistoryDictionarys.ContainsKey(cItem.TagIdStr))
            {
                for (int i = 0; i < 10; i++)
                {
                    HistoryCardBean his = cardHistoryDictionarys[cItem.TagIdStr][i];
                    if (his == null || his.Port1Rssi == 0xff) break;
                    if (cItem.Port1RssiHistory == 0xff)
                    {
                        setHisData(his, cItem);
                    }
                    else if (XWUtils.byteBTBettow(cItem.PortID, his.Port1ID, 2))
                    {
                        setHisData(his, cItem);
                        break;
                    }
                }
            }
        }

        //设置历史数据
        private void setHisData(HistoryCardBean his, CardUpDataBean cItem) 
        {
            if (his.Port1ID != null)
            {
                Array.Copy(his.Port1ID, 0, cItem.Port1IDHistory, 0, 2);
                cItem.Port1RssiHistory = his.Port1Rssi;
            }
            else cItem.Port1RssiHistory = 0xff;
            if (his.Port2ID != null)
            {
                Array.Copy(his.Port2ID, 0, cItem.Port2IDHistory, 0, 2);
                cItem.Port2RssiHistory = his.Port2Rssi;
            }
            else cItem.Port2RssiHistory = 0xff;
            if (his.Port3ID != null) 
            {
                Array.Copy(his.Port3ID, 0, cItem.Port3IDHistory, 0, 2);
                cItem.Port3RssiHistory = his.Port3Rssi;
            }
            else cItem.Port3RssiHistory = 0xff;
        }

        //保存10组历史数据，不保存到本地文件中
        private void saveHisCardData(CardUpDataBean cItem) 
        {
            if (cardHistoryDictionarys.ContainsKey(cItem.TagIdStr))
            {
                HistoryCardBean[] hisBeans = cardHistoryDictionarys[cItem.TagIdStr];
                HistoryCardBean his = new HistoryCardBean(cItem);
                for (int i = 9; i > 0;i-- )
                {
                    hisBeans[i] = hisBeans[i - 1];
                }
                //Array.Copy(hisBeans, 0, hisBeans, 1, 9);
                hisBeans[0] = his;
            }
            else
            {
                HistoryCardBean[] hisBeans = new HistoryCardBean[10];
                HistoryCardBean his = new HistoryCardBean(cItem);
                hisBeans[0] = his;
                cardHistoryDictionarys.Add(cItem.TagIdStr, hisBeans);
            }
        }                 

        private void showNewIp(CardUpDataBean cItem) 
        {
            try 
            {
                showNewIpFromDictionary(cItem);
            }
            catch { }         
        }

        private void showNewIpFromDictionary(CardUpDataBean cItem) 
        {
            if (cItem.TagIdStr.Length == 0 || !ipBeanDictionarys.ContainsKey(cItem.TagIdStr)) return;
             
            var iiRe = ipBeanDictionarys[cItem.TagIdStr];
            byte[] iiReNODEID = new byte[2];
            Array.Copy(iiRe.PortIDRes, 0, iiReNODEID,0,2);

            if (XWUtils.byteBTBettow(iiReNODEID, cItem.Port1IDHistory)) return;
            string nodeKey = cItem.Port1IDHistory[0].ToString("X2") + cItem.Port1IDHistory[1].ToString("X2");

            if(showIpBeanDictionarys.ContainsKey(nodeKey))
            {
                Array.Copy(cItem.Port1IDHistory,0,iiRe.PortIDRes, 0, 2);

                iiRe.PortIDRes[2] = cItem.Port1RssiHistory;
                iiRe.IpKey = showIpBeanDictionarys[nodeKey];
                byte[] keyInfo = Encoding.UTF8.GetBytes(showIpBeanDictionarys[nodeKey]);//crBean.Name
                iiRe.IpKeyBt = keyInfo;
                sendIpShow(iiRe);
            }                     
        }

        private void sendIpShow(IPIDRessBean idReItem) 
        {                           
            byte[] Reveip = new byte[idReItem.IpKeyBt.Length+4];
            Reveip[0] = 0xff;
            Reveip[1] = 0xff;
            Array.Copy(idReItem.TagID, 0, Reveip,2,2);
            Array.Copy(idReItem.IpKeyBt, 0, Reveip, 4, idReItem.IpKeyBt.Length);
            addEndAndsendData(Reveip, 0, idReItem.IpKey);
        }

        private long ipChangeTime = 0;
        //修改发送Ip
        private void sendTcpServerInforThread(object obj) 
        {           
            CardUpDataBean oldItem = (CardUpDataBean)obj;
            try 
            {
                if (oldUpCardDatas != null && PeoplePowerModel.getPeoplePowerModel().Jurisdiction != PeoplePowerModel.getPeoplePowerModel().CongjiValue)                            oldUpCardDatas.Add(oldItem); //保存一份到旧数据  //此處調用，是為了方便在線程中運行，而不會阻塞其他線程導致耗時
                if (XwDataUtils.GetLongTimeStamp() - ipChangeTime > 500) 
                {                
                    ipChangeTime = XwDataUtils.GetLongTimeStamp();                                        
                    cacheData();//此處調用，是為了方便在線程中運行，而不會阻塞其他線程導致耗時                                                            
                }
            }
            catch { }    
            lock (objLock) 
            {
                foreach (var item in keyBys)
                {
                    byte[] ID = item.Value;
                    if (ID[0] == 0 && ID[1] == 0)
                    {
                        byte[] keyInfo = Encoding.UTF8.GetBytes(item.Key);//crBean.Name
                        getFourByte(oldItem.getCardUpByte(0x04), keyInfo);
                    }
                    else if (XWUtils.byteBTBettow(ID, oldItem.TagId, ID.Length))
                    {
                        byte[] keyInfo = Encoding.UTF8.GetBytes(item.Key);//crBean.Name
                        getFourByte(oldItem.getCardUpByte(0x04), keyInfo);
                    }
                }
            }            
        }

        private void sendTcpServerInfor(CardUpDataBean oldItem) 
        {
            if (isDealHistory) return;
            CardUpDataBean oldItem2 = new CardUpDataBean(oldItem);
            ThreadPool.QueueUserWorkItem(new WaitCallback(sendTcpServerInforThread), oldItem2);    //参数可选           
        }

        private void getFourByte(byte[] scourBytes,byte[] key) 
        {
            byte[] scourByte = FileModel.getFlModel().getChongZuData(scourBytes);

            byte[] sendBt = new byte[scourByte.Length + key.Length +3];
            sendBt[0] = 0xfd;
            sendBt[1] = 0x04;
            sendBt[2] = (byte)(key.Length + 3); //scourByte首位下标
            Array.Copy(key, 0, sendBt, 3, key.Length);
            Array.Copy(scourByte, 0, sendBt, sendBt[2], scourByte.Length);
            addEndAndsendData(sendBt,1,"");
        }

        private void changeRess(CardUpDataBean cItem,int ressID) 
        {
            if (ressID == 1) 
            {
                cItem.Port3ID = cItem.Port2ID;
                cItem.Port3Rssi = cItem.Port2Rssi;
                cItem.Port2ID = cItem.Port1ID;
                cItem.Port2Rssi = cItem.Port1Rssi;
            }
            else if (ressID == 2) 
            {
                cItem.Port2ID = cItem.Port1ID;
                cItem.Port2Rssi = cItem.Port1Rssi;
            }
        }

        public override void close()
        {
            mainInterface = null;
        }

        /// <summary>
        /// 返回MODEL的标志位
        /// </summary>
        /// <returns></returns>
        public override string TAG() 
        {
            return "FromMainModel";
        }

        /// <summary>
        /// 發送從機身份驗證
        /// btnType = 連接按鈕的狀態
        /// </summary>
        public void sendConjiYanzheng(string btnType) 
        {
            PeoplePowerModel.getPeoplePowerModel().IsConnect = true;
            sendDataToZhuju(getCountBt(), "255.255.255.255");
            Thread.Sleep(300);

            if ("green".Equals(btnType)) PeoplePowerModel.getPeoplePowerModel().IsConnect = false;
            else PeoplePowerModel.getPeoplePowerModel().IsConnect = true;
            sendDataToZhuju(getCountBt(), "255.255.255.255"); 
        }

        /// <summary>
        /// 发送同步命令
        /// </summary>
        public void sendConjiTongBu()
        {
            if (PeoplePowerModel.getPeoplePowerModel().IsConnect)
                sendDataToZhuju(getCountTongBuBt(), "255.255.255.254");
        }

        /// <summary>
        /// 發送從機同步接收定位数据命令
        /// </summary>
        public void sendConjiReveLocaData()
        {
            sendDataToZhuju(getTCPServerType(0x00, 0x03), "255.255.255.255");
        }

        /// <summary>
        /// 发送数据到主机
        /// </summary>
        /// <param name="type">0是發送從機身份驗證，1是主机数据同步命令</param>
        private void sendDataToZhuju(byte[] buf,string msg) 
        {
            byte[] bufs = getCountBtAddHandle(buf);
            if (bufs != null)
            {
                addEndAndsendData(bufs, 2, msg);
            }   
        }

        /// <summary>
        /// 加上数据包头getCountBt() ；
        /// </summary>
        /// <returns></returns>
        private byte[] getCountBtAddHandle(byte[] bufs) 
        {
            if (bufs == null) return new byte[0];     

            byte[] buf = new byte[bufs.Length + 4];
            Array.Copy(bufs, 0, buf, 4, bufs.Length);
            buf[0] = (byte)((bufs.Length / 0x1000000) % 0x100);
            buf[1] = (byte)((bufs.Length / 0x10000) % 0x100);
            buf[2] = (byte)((bufs.Length / 0x100) % 0x100);
            buf[3] = (byte)(bufs.Length % 0x100);
            return buf;
        }

        /// <summary>
        /// 获取主机同步命令
        /// </summary>
        /// <returns></returns>
        private byte[] getCountTongBuBt()
        {
            byte[] errValue = getTCPServerType(0x00,0x02);
            return errValue;
        }

        /// <summary>
        /// TCPServer相关的一些协议
        /// bt1 =00时，bt2 = 00 关闭TCPClien接收数据，bt2 = 01 消息失败，bt2 = 2 获取主机同步命令,bt2 = 3主机同步命令完成
        /// 
        /// </summary>
        /// <param name="bt1"></param>
        /// <param name="bt2">,</param>
        /// <returns></returns>
        private byte[] getTCPServerType(byte bt1,byte bt2) 
        {
            byte[] value = new byte[8] { 0xff, 0xfd, 0xff, bt1, bt2, 0xfd, 0xff, 0xfd };
            return value;
        }
        
        /// <summary>
        /// 获取账号字节码或者关闭连接字节码
        /// </summary>
        /// <returns></returns>
        private byte[] getCountBt() 
        {
            if (PeoplePowerModel.getPeoplePowerModel().Jurisdiction != PeoplePowerModel.getPeoplePowerModel().CongjiValue)
            {
                return null;
            }
            if (PeoplePowerModel.getPeoplePowerModel().IsConnect)  //关闭
            {
                byte[] errValue = new byte[8] {0xff, 0xfd, 0xff, 0x00, 0x00, 0xfd, 0xff, 0xfd };
                return errValue;
            }

            return tongbuYanZhaneg();
        }

        private byte[] tongbuYanZhaneg() 
        {
            string count = PeoplePowerModel.getPeoplePowerModel().Count;
            string power = PeoplePowerModel.getPeoplePowerModel().Password;
            byte[] countBt = Encoding.UTF8.GetBytes(count); //Encoding.UTF8.GetString(arrRecvmsg, 0, length);
            byte[] powerBt = Encoding.UTF8.GetBytes(power);
            byte[] bufs = new byte[6 + countBt.Length + powerBt.Length];// { 0xff, 0xfd, 0xff, 0x00, 0, 0, 0, 0xfd, 0xff, 0xfd };//发一个身份验证
            bufs[0] = 0xff;
            bufs[1] = 0xfd;
            bufs[2] = 0xff;
            Array.Copy(countBt, 0, bufs, 3, countBt.Length);
            Array.Copy(powerBt, 0, bufs, 3 + countBt.Length, powerBt.Length);
            bufs[bufs.Length - 3] = 0xfd;
            bufs[bufs.Length - 2] = 0xff;
            bufs[bufs.Length - 1] = 0xfd;
            return bufs;
        }

    }
}
