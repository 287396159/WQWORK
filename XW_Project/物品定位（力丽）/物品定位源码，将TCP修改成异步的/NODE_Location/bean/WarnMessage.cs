using CiXinLocation.Model;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CiXinLocation.bean
{
    public class WarnMessage //这是一个单例模式
    {
        public delegate void WarnMessageHandle(int msgCount);
        public WarnMessageHandle warnMsg;
        private UInt32 sendTime = 0;
        private Timer timer = null;

        private WarnMessage()
        {
            createCardLowEleWarnMsgs();
            createCardUnanswerWranMsgs();
            createNODEUnanswerWranMsgs();
            createCardLowEleWarnHisMsgs();
            createCardUnanswerWranHisMsgs();
            createNODEUnanswerWranHisMsgs();
            creatTime();
        }

        private static WarnMessage warnMessage;
        private Dictionary<string, DrivaceWarnMessage> cardLowEleWarnMsgs;//
        private Dictionary<string, DrivaceWarnMessage> cardUnanswerWranMsgs;//
        private Dictionary<string, DrivaceWarnMessage> nODEUnanswerWranMsgs;//

        private Dictionary<string, DrivaceWarnMessage> cardLowEleWarnHisMsgs;  //处理过的历史数据
        private Dictionary<string, DrivaceWarnMessage> cardUnanswerWranHisMsgs;//处理过的历史数据
        private Dictionary<string, DrivaceWarnMessage> nODEUnanswerWranHisMsgs;//处理过的历史数据

        public void warnMsgCallBack()
        {
            warnMsgCallBack(nODEUnanswerWranMsgs.Count + cardLowEleWarnMsgs.Count + cardUnanswerWranMsgs.Count);
        }

        private void warnMsgCallBack(int msgCount)
        {
            /*Console.Write("\r\n警告时间" + XwDataUtils.GetLongTimeStamp());
            if (sendTime == 0 || sendTime + 2 > XwDataUtils.GetTimeStamp()) 
            {
                Console.Write("\r\n进入线程");
                if (timer == null) creatTime();
                if (!timer.Enabled) timer.Enabled = true;
                return;
            }
            sendTime = XwDataUtils.GetTimeStamp(); */
            if (warnMsg != null) warnMsg(msgCount);
        }


        private void creatTime() 
        {
            Console.Write("\r\n进入创建");
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new System.EventHandler(this.timer1_Tick);
            
        }

        //private void createWarnMessa

        private void timer1_Tick(object sender, EventArgs e)
        {
            Console.Write("\r\n跑了一次，时间" + XwDataUtils.currentTimeToSe());
            if (sendTime + 10 <= XwDataUtils.GetTimeStamp())
            {
                Console.Write("\r\n跑了一次，时间" + XwDataUtils.currentTimeToSe());
                warnMsgCallBack();
                timer.Enabled = false;
            }            
        }

       
        internal Dictionary<string, DrivaceWarnMessage> CardLowEleWarnMsgs
        {
            get { return cardLowEleWarnMsgs; }
            set { cardLowEleWarnMsgs = value; }
        }      

        internal Dictionary<string, DrivaceWarnMessage> CardUnanswerWranMsgs
        {
            get { return cardUnanswerWranMsgs; }
            set { cardUnanswerWranMsgs = value; }
        }        

        internal Dictionary<string, DrivaceWarnMessage> NODEUnanswerWranMsgs
        {
            get { return nODEUnanswerWranMsgs; }
            set { nODEUnanswerWranMsgs = value; }
        }
        private static object obje = new object();        

        public static WarnMessage getWarnMessage() 
        {
            if (warnMessage == null)
            {
                lock (obje)
                {
                    if (warnMessage == null) warnMessage = new WarnMessage();
                }
            }
            return warnMessage;
        }

        public void getEleWarnBt(ref List<byte[]> eleBt)
        {
            if(cardLowEleWarnMsgs == null) return;
            Dictionary<string, DrivaceWarnMessage> cardLowEleWarnMsgsCache = new Dictionary<string, DrivaceWarnMessage>(cardLowEleWarnMsgs);
            foreach (var item in cardLowEleWarnMsgsCache)
            {
                byte[] tagID = XWUtils.getByteID(item.Value.DrivaceID);
                byte[] canKaoID = XWUtils.getByteID(item.Value.CanKaoDianID);
                byte[] warnTime = XwDataUtils.firstTimeByte(item.Value.WarnTime);
                byte[] dealTime = XwDataUtils.firstTimeByte(item.Value.DealWarnTime);
                if (tagID == null) tagID = new byte[2];
                if (canKaoID == null) canKaoID = new byte[2];

                byte[] eleByte = new byte[17];
                eleByte[0] = 0xfd;
                eleByte[1] = 0x05;
                eleByte[2] = tagID[0];
                eleByte[3] = tagID[1];
                eleByte[4] = canKaoID[0];
                eleByte[5] = canKaoID[1];
                eleByte[6] = (byte)item.Value.CurrentElectricity;
                Array.Copy(warnTime, 0, eleByte, 7, 4);
                Array.Copy(dealTime, 0, eleByte, 11, 4);
                eleByte[16] = 0xfb;
                eleBt.Add(eleByte);
            }            
        }

        public void getCardWarnBt(ref List<byte[]> cardBt)
        {
            if (cardUnanswerWranMsgs == null) return;
            Dictionary<string, DrivaceWarnMessage> cardUnanswerWranMsgsCache = new Dictionary<string, DrivaceWarnMessage>(cardUnanswerWranMsgs);
            foreach (var item in cardUnanswerWranMsgsCache)
            {
                byte[] tagID = XWUtils.getByteID(item.Value.DrivaceID);
                byte[] canKaoID = XWUtils.getByteID(item.Value.CanKaoDianID);
                byte[] warnTime = XwDataUtils.firstTimeByte(item.Value.WarnTime);
                byte[] dealTime = XwDataUtils.firstTimeByte(item.Value.DealWarnTime);
                if (tagID == null) tagID = new byte[2];
                if (canKaoID == null) canKaoID = new byte[2];

                byte[] eleByte = new byte[18];
                eleByte[0] = 0xfd;
                eleByte[1] = 0x06;
                eleByte[2] = tagID[0];
                eleByte[3] = tagID[1];
                eleByte[4] = canKaoID[0];
                eleByte[5] = canKaoID[1];
                eleByte[6] = (byte)(item.Value.SleepTime / 0x100);
                eleByte[7] = (byte)(item.Value.SleepTime % 0x100);
                Array.Copy(warnTime, 0, eleByte, 8, 4);
                Array.Copy(dealTime, 0, eleByte, 12, 4);
                eleByte[17] = 0xfb;
                cardBt.Add(eleByte);
            }            
        }

        public void getNODEWarnBt(ref List<byte[]> nodeBt)
        {
            if (cardUnanswerWranMsgs == null) return;
            Dictionary<string, DrivaceWarnMessage> nODEUnanswerWranMsgsCache = new Dictionary<string, DrivaceWarnMessage>(nODEUnanswerWranMsgs);
            foreach (var item in nODEUnanswerWranMsgsCache)
            {
                byte[] canKaoID = XWUtils.getByteID(item.Value.DrivaceID);
                byte[] warnTime = XwDataUtils.firstTimeByte(item.Value.WarnTime);
                byte[] dealTime = XwDataUtils.firstTimeByte(item.Value.DealWarnTime);
                if (canKaoID == null) canKaoID = new byte[2];

                byte[] eleByte = new byte[16];
                eleByte[0] = 0xfd;
                eleByte[1] = 0x07;
                eleByte[2] = canKaoID[0];
                eleByte[3] = canKaoID[1];
                eleByte[4] = (byte)(item.Value.SleepTime / 0x100);
                eleByte[5] = (byte)(item.Value.SleepTime % 0x100);
                Array.Copy(warnTime, 0, eleByte, 6, 4);
                Array.Copy(dealTime, 0, eleByte, 10, 4);
                eleByte[15] = 0xfb;
                nodeBt.Add(eleByte);
            }         
        }

        private void createCardLowEleWarnMsgs() 
        {
            if (cardLowEleWarnMsgs == null) 
                cardLowEleWarnMsgs = new Dictionary<string, DrivaceWarnMessage>();
        }

        private void createCardUnanswerWranMsgs()
        {
            if (cardUnanswerWranMsgs == null)
                cardUnanswerWranMsgs = new Dictionary<string, DrivaceWarnMessage>();
        }

        private void createNODEUnanswerWranMsgs()
        {
            if (nODEUnanswerWranMsgs == null)
                nODEUnanswerWranMsgs = new Dictionary<string, DrivaceWarnMessage>();
        }

        private void createCardLowEleWarnHisMsgs()
        {
            if (cardLowEleWarnHisMsgs == null)
                cardLowEleWarnHisMsgs = new Dictionary<string, DrivaceWarnMessage>();
        }

        private void createCardUnanswerWranHisMsgs()
        {
            if (cardUnanswerWranHisMsgs == null)
                cardUnanswerWranHisMsgs = new Dictionary<string, DrivaceWarnMessage>();
        }

        private void createNODEUnanswerWranHisMsgs()
        {
            if (nODEUnanswerWranHisMsgs == null)
                nODEUnanswerWranHisMsgs = new Dictionary<string, DrivaceWarnMessage>();
        }

        public void addcardLowEleMessage(string cardId, int currentElectricity, string quYuID, string canKaoDianID, long drivaceUpTime)         
        {
            createCardLowEleWarnMsgs();
            DrivaceWarnMessage dreMsg = new DrivaceWarnMessage();
            dreMsg.setLowEle(cardId, currentElectricity, quYuID, canKaoDianID, drivaceUpTime);
            addDrivaceWrimMsg(cardId, cardLowEleWarnMsgs,cardLowEleWarnHisMsgs, dreMsg);
        }

        public void removeCardLowEleMessage(int lowEle,string cardId)
        {
            if (lowEle > FileModel.getFlModel().ChFlBean.TagLow)
            {
                removeCardLowEleMessage(cardId);
            }
        }

        /// <summary>
        /// 移除低电量报警
        /// </summary>
        /// <param name="cardId"></param>
        public void removeCardLowEleMessage(string cardId)
        {
            removeMessage(cardLowEleWarnMsgs, cardLowEleWarnHisMsgs, cardId);
        }

        private void removeMessage(Dictionary<string, DrivaceWarnMessage> WarnMsgs, Dictionary<string, DrivaceWarnMessage> hisWarnMsgs, String cardId)
        {
            if (WarnMsgs.ContainsKey(cardId))
            {
                WarnMsgs.Remove(cardId);
                warnMsgCallBack();
            }
            if (hisWarnMsgs.ContainsKey(cardId))
            {
                hisWarnMsgs.Remove(cardId);
            }          
        }

        private object cardLock = new object();
        public void addCardUnanswerMessage(string cardId, uint sleepTime, string quYuID, string canKaoDianID, long drivaceUpTime)
        {
            lock (cardLock)
            {
                createCardUnanswerWranMsgs();
                DrivaceWarnMessage dreMsg = new DrivaceWarnMessage();
                dreMsg.setCardUnanswer(cardId, sleepTime, quYuID, canKaoDianID, drivaceUpTime);
                addDrivaceWrimMsg(cardId, cardUnanswerWranMsgs, cardUnanswerWranHisMsgs,dreMsg);
            }            
        }

        /// <summary>
        /// 移除卡片报警
        /// </summary>
        /// <param name="cardId"></param>
        public void removeCardUnanswerMessage(string cardId)
        {
            removeMessage(cardUnanswerWranMsgs, cardUnanswerWranHisMsgs, cardId);
        }


        public void addNODEUnanswerWranMsgs(string NODEId, uint sleepTime, string quYuID, string canKaoDianID, long drivaceUpTime)
        {            
            createNODEUnanswerWranMsgs();
            DrivaceWarnMessage dreMsg = new DrivaceWarnMessage();
            dreMsg.setNODEUnanswer(NODEId, sleepTime, quYuID, canKaoDianID, drivaceUpTime);
            addDrivaceWrimMsg(NODEId, nODEUnanswerWranMsgs, nODEUnanswerWranHisMsgs, dreMsg);
        }

        /// <summary>
        /// 移除节点报警
        /// </summary>
        /// <param name="cardId"></param>
        public void removeNODEUnanswerMessage(string cardId)
        {
            removeMessage(nODEUnanswerWranMsgs, nODEUnanswerWranHisMsgs, cardId);
        }

        /// <summary>
        /// 将报警信息添加相应的信息中存储
        /// </summary>
        /// <param name="drivaceID"></param>
        /// <param name="msgs"></param>
        /// <param name="dreMsg"></param>
        private void addDrivaceWrimMsg(string drivaceID, Dictionary<string, DrivaceWarnMessage> msgs, Dictionary<string, DrivaceWarnMessage> hisMsgs, DrivaceWarnMessage dreMsg) 
        {
            if (drivaceID == null || "".Equals(drivaceID)) return;
            if (hisMsgs.ContainsKey(drivaceID))
            {
                if (dreMsg.DrivaceUpTime <= hisMsgs[drivaceID].DrivaceUpTime)
                    return;
            } 
            if (msgs.ContainsKey(drivaceID))
            {               
                if (dreMsg.WarnTp != WarnType.CARD_LOW_ELECTRICITY)
                    msgs[drivaceID].WarnTime = XwDataUtils.GetTimeStamp();
                msgs[drivaceID].IsClear = false;
                msgs[drivaceID].IsDeal = false;
            }
            else 
            {
                msgs.Add(drivaceID, dreMsg);
                warnMsgCallBack(nODEUnanswerWranMsgs.Count + cardLowEleWarnMsgs.Count + cardUnanswerWranMsgs.Count);
            }                           
            if (dreMsg.WarnTp == WarnType.CARD_LOW_ELECTRICITY && msgs[drivaceID].CurrentElectricity > dreMsg.CurrentElectricity + 9) 
            {
                msgs[drivaceID].CurrentElectricity = dreMsg.CurrentElectricity;
                msgs[drivaceID].WarnTime = XwDataUtils.GetTimeStamp();
                warnMsgCallBack(nODEUnanswerWranMsgs.Count + cardLowEleWarnMsgs.Count + cardUnanswerWranMsgs.Count);
            }                           
        }

        public void drivaceDeal(string drivaceID,WarnType wType) //处理设备
        {
            DrivaceWarnMessage drevaceMsg = null;
            if (wType == WarnType.CARD_LOW_ELECTRICITY) 
            {
                if (cardLowEleWarnMsgs.ContainsKey(drivaceID)) drevaceMsg = cardLowEleWarnMsgs[drivaceID];
            }
            else if (wType == WarnType.CARD_UNANSWERED) 
            {
                if (cardUnanswerWranMsgs.ContainsKey(drivaceID)) drevaceMsg = cardUnanswerWranMsgs[drivaceID];//.DealWarnTime = XwDataUtils.GetTimeStamp();
            }
            else if (wType == WarnType.NODE_UNANSWERED) 
            {
                if (nODEUnanswerWranMsgs.ContainsKey(drivaceID)) drevaceMsg = nODEUnanswerWranMsgs[drivaceID];//.DealWarnTime = XwDataUtils.GetTimeStamp();            
            }                
            if(drevaceMsg != null)
            {
                drevaceMsg.DealWarnTime = XwDataUtils.GetTimeStamp();
                drevaceMsg.IsDeal = true;
            }                
        }

        public void drivaceClear(WarnType wType) // 设备清理
        {
            if (wType == WarnType.CARD_LOW_ELECTRICITY)
            {
                drivaceClear(cardLowEleWarnMsgs, cardLowEleWarnHisMsgs);
            }else if(wType == WarnType.CARD_UNANSWERED)
                drivaceClear(cardUnanswerWranMsgs,cardUnanswerWranHisMsgs);
            else if(wType == WarnType.NODE_UNANSWERED)
                drivaceClear(nODEUnanswerWranMsgs, nODEUnanswerWranHisMsgs);
            warnMsgCallBack(nODEUnanswerWranMsgs.Count + cardLowEleWarnMsgs.Count + cardUnanswerWranMsgs.Count);
        }

        private void drivaceClear(Dictionary<string, DrivaceWarnMessage> msgs, Dictionary<string, DrivaceWarnMessage> hisMsgs) 
        {
            Dictionary<string, DrivaceWarnMessage> msgInfo = new Dictionary<string, DrivaceWarnMessage>(msgs);
            foreach (var item in msgInfo)
            {
                if (item.Value.IsDeal)
                {
                    msgs.Remove(item.Key);
                    if (hisMsgs.ContainsKey(item.Key)) 
                    {
                        hisMsgs[item.Key] = item.Value;
                    }
                    else
                        hisMsgs.Add(item.Key, item.Value);
                } 
            }
            msgInfo.Clear();
        }
    }

    [Serializable]
    class DrivaceWarnMessage
    {
        public DrivaceWarnMessage() { }
        private DrivaceTypeAll drivace;
        private WarnType warnTp;      
        private string drivaceName;
        private string drivaceID;        
        private string quYuName;
        private string quYuID;
        private string canKaoDianName;
        private string canKaoDianID;        
        private int currentElectricity; //当前电量
        private long warnTime;  //报警时间
        private uint sleepTime; //休眠时间
        private long dealWarnTime; //处理警报时间
        private bool isDeal = false; //是否处理
        private long drivaceUpTime;//设备最后上报的时间
        private bool isClear = false; //是否清理

        public void setLowEle(string cardId, int currentElectricity, string quYuID, string canKaoDianID, long drivaceUpTime) 
        {
            drivace = DrivaceTypeAll.CARD;
            warnTp = WarnType.CARD_LOW_ELECTRICITY;
            drivaceID = cardId;
            drivaceName = getFileCardName(cardId);
            this.quYuID = quYuID;
            this.drivaceUpTime = drivaceUpTime;
            quYuName = getFilequYuName(canKaoDianID);
            this.canKaoDianID = canKaoDianID;
            canKaoDianName = getFileNODEName(canKaoDianID);
            warnTime = XwDataUtils.GetTimeStamp();
            this.currentElectricity = currentElectricity;
            dealWarnTime = 0;
        }

        public void setCardUnanswer(string cardId, uint sleepTime, string quYuID, string canKaoDianID, long drivaceUpTime)
        {
            setLowEle(cardId, 0, quYuID, canKaoDianID, drivaceUpTime);
            warnTp = WarnType.CARD_UNANSWERED;
            this.sleepTime = sleepTime;
        }

        public void setNODEUnanswer(string NODEId, uint sleepTime, string quYuID, string canKaoDianID, long drivaceUpTime)
        {
            setLowEle(NODEId, 0, quYuID, canKaoDianID, drivaceUpTime);
            drivaceName = getFileNODEName(NODEId);
            drivace = DrivaceTypeAll.NODE;
            warnTp = WarnType.NODE_UNANSWERED;            
            this.sleepTime = sleepTime;
        }

        public string getFileCardName(string cardID) //获取保存的卡片名称
        {
            Dictionary<string, CardBean> cardDic = FileModel.getFlModel().ChFlBean.CardDic;
            if (cardDic != null && cardDic.ContainsKey(cardID))
                return cardDic[cardID].Name;
            return cardID;
        }

        public string getFilequYuName(string cankaoDianID) //获取保存的区域名称
        {
            List<CanKaoDianBean> cankaodianData = FileModel.getFlModel().ChFlBean.CanKaoDians.ToList();
            if (cankaodianData == null) return "";
            foreach (var Item in cankaodianData) 
            {
                if (cankaoDianID.Equals(Item.Id))
                    return Item.QuYuname;
            }
            return "";
        }

        public string getFileNODEName(string NODEID) //获取保存的节点名称
        {
            List<CanKaoDianBean> canKaoDians = FileModel.getFlModel().ChFlBean.CanKaoDians;
            if (NODEID == null) return NODEID;
            foreach (var Item in canKaoDians)
            {
                if (Item.Id.Equals(NODEID))
                    return Item.Name;
            }
            return NODEID;
        }

        public bool IsClear
        {
            get { return isClear; }
            set { isClear = value; }
        }

        public long DrivaceUpTime
        {
            get { return drivaceUpTime; }
            set { drivaceUpTime = value; }
        }

        public WarnType WarnTp
        {
            get { return warnTp; }
            set { warnTp = value; }
        }

        public string CanKaoDianID
        {
            get { return canKaoDianID; }
            set { canKaoDianID = value; }
        }

        public string QuYuID
        {
            get { return quYuID; }
            set { quYuID = value; }
        }

        public string DrivaceID
        {
            get { return drivaceID; }
            set { drivaceID = value; }
        }

        public bool IsDeal
        {
            get { return isDeal; }
            set { isDeal = value; }
        }

        public long DealWarnTime
        {
            get { return dealWarnTime; }
            set { dealWarnTime = value; }
        }

        public uint SleepTime
        {
            get { return sleepTime; }
            set { sleepTime = value; }
        }

        public long WarnTime
        {
            get { return warnTime; }
            set { warnTime = value; }
        }

        public int CurrentElectricity
        {
            get { return currentElectricity; }
            set { currentElectricity = value; }
        }

        public string CanKaoDianName
        {
            get { return canKaoDianName; }
            set { canKaoDianName = value; }
        }

        public string QuYuName
        {
            get { return quYuName; }
            set { quYuName = value; }
        }

        public string DrivaceName
        {
            get { return drivaceName; }
            set { drivaceName = value; }
        }
        
        public DrivaceTypeAll Drivace
        {
            get { return drivace; }
            set { drivace = value; }
        }
    }

    public enum WarnType
    {
        NOTHING = 0, //什么都不是
        NODE_UNANSWERED = 1,//节点未回
        CARD_UNANSWERED = 2,//卡片未回
        CARD_LOW_ELECTRICITY = 3, //低电量
        HELP_SOS = 4 //求救
    }

}
