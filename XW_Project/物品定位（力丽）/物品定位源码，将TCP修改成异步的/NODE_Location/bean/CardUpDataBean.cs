using SerialportSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiXinLocation.bean
{

    [Serializable]
    public class CardUpDataBean
    {
        private bool receivData;			//是否接受到资料
        private byte type;					//定位信息类型
        private byte drivaceType;			//设备类型
        private byte[] tagId = new byte[2];			//
        private byte[] port1ID = new byte[2]{0,0};		//保存最近的一个Router的ID
        private byte port1Rssi = 0xff;				//保存最近的一个Router接受数据时候的信号强度
        private byte[] port2ID = new byte[2] { 0, 0 };
        private byte port2Rssi = 0xff;	
        private byte[] port3ID = new byte[2] { 0, 0 };
        private byte port3Rssi = 0xff;	
        private byte battery;		//电池电量
        private UInt16 sensorTime;
        private byte index;					//本次资料的序列号	
        private UInt32 firstReceiveTime;		//第一次接受到数据的时间
        private bool isUpdate;				//数据是否有更新（有新的定位数据到来，需要更新到列表中），True为更新
        private int totalCount;		//总共接收到多少个封包
        private int lostCount;		//丢包数量
        private byte sendGongLv;	//发射功率       
        private byte ledStaus;		//1Byte，当前的LED灯的状态， 0 = 灭；1 = 亮；2 = 闪烁
        private byte sleepTime;		   
        private int tagID_Int;
        private int port1ID_Int;
        private byte[] portID = new byte[2] { 0, 0 };
        private int changeIndex = 0;
        private byte[] port1IDHistory = new byte[2] { 0, 0 };		//保存最近的一个Router的ID        
        private byte port1RssiHistory = 0xff;				//保存最近的一个Router接受数据时候的信号强度        
        private byte[] port2IDHistory = new byte[2] { 0, 0 };
        private byte port2RssiHistory = 0xff;
        private byte[] port3IDHistory = new byte[2] { 0, 0 };
        private byte port3RssiHistory = 0xff;

        public byte Port1RssiHistory
        {
            get { return port1RssiHistory; }
            set { port1RssiHistory = value; }
        }

        public string Port2IDHistoryStr
        {
            get { return port2IDHistory[0].ToString("X2") + port2IDHistory[1].ToString("X2"); }
        }

        public byte[] Port2IDHistory
        {
            get { return port2IDHistory; }
            set { port2IDHistory = value; }
        }
        
        public byte Port2RssiHistory
        {
            get { return port2RssiHistory; }
            set { port2RssiHistory = value; }
        }

        public string Port3IDHistoryStr
        {
            get { return port3IDHistory[0].ToString("X2") + port3IDHistory[1].ToString("X2"); }
        }

        public byte[] Port3IDHistory
        {
            get { return port3IDHistory; }
            set { port3IDHistory = value; }
        }       

        public byte Port3RssiHistory
        {
            get { return port3RssiHistory; }
            set { port3RssiHistory = value; }
        }

        public byte[] Port1IDHistory
        {
            get { return port1IDHistory; }
            set { port1IDHistory = value; }
        }

        public string Port1IDHistoryStr
        {
            get { return port1IDHistory[0].ToString("X2") + port1IDHistory[1].ToString("X2"); }
        }


        public CardUpDataBean() { }
        public CardUpDataBean(CardUpDataBean cardb)
        {
            ReceivData = cardb.ReceivData;			//是否接受到资料
            MType = cardb.MType;					//定位信息类型
            DrivaceType = cardb.DrivaceType;			//设备类型
            TagId = cardb.TagId;			//
            Port1ID = cardb.Port1ID;		//保存最近的一个Router的ID
            Port1Rssi = cardb.Port1Rssi;				//保存最近的一个Router接受数据时候的信号强度
            Port2ID = cardb.Port2ID;
            Port2Rssi = cardb.Port2Rssi;
            Port3ID = cardb.Port3ID;
            Port3Rssi = cardb.Port3Rssi;

            Port1IDHistory = cardb.Port1IDHistory;
            Port1RssiHistory = cardb.Port1RssiHistory;
            Port2IDHistory = cardb.Port2IDHistory;
            Port2RssiHistory = cardb.Port2RssiHistory;
            Port3IDHistory = cardb.Port3IDHistory;
            Port3RssiHistory = cardb.Port3RssiHistory;

            Battery = cardb.Battery;		//电池电量
            SensorTime = cardb.SensorTime;
            Index = cardb.Index;					//本次资料的序列号	
            FirstReceiveTime = cardb.FirstReceiveTime;		//第一次接受到数据的时间
            IsUpdate = cardb.IsUpdate;				//数据是否有更新（有新的定位数据到来，需要更新到列表中），True为更新
            TotalCount = cardb.TotalCount;		//总共接收到多少个封包
            LostCount = cardb.LostCount;		//丢包数量
            LEDStaus = cardb.LEDStaus;			//1Byte，当前的LED灯的状态， 0 = 灭；1 = 亮；2 = 闪烁
            SleepTime = cardb.SleepTime;		//1Byte，当前的LED灯的状态， 0 = 灭；1 = 亮；2 = 闪烁
            sendGongLv = cardb.sendGongLv;
            TagID_Int = cardb.TagID_Int;
            Port1ID_Int = cardb.Port1ID_Int;
        }

        public void text(int i)
        {
            ReceivData  = false;			//是否接受到资料
            MType = 1;					//定位信息类型
            DrivaceType = 1;			//设备类型            
            Port1ID = new byte[2] { (byte)(i / 0x100), (byte)(i % 0x100) };		//保存最近的一个Router的ID
            Port1Rssi = 2;				//保存最近的一个Router接受数据时候的信号强度
            Port2ID = new byte[2]{0,0};
            Port2Rssi = 2;
            Port3ID = new byte[2] { 0, 0 };
            Port3Rssi = (byte)i;
            Battery = (byte)i;		//电池电量
            SensorTime = (byte)i;
            Index = (byte)i;						//本次资料的序列号	
            FirstReceiveTime = (UInt32)i;		//第一次接受到数据的时间
            IsUpdate = false;				//数据是否有更新（有新的定位数据到来，需要更新到列表中），True为更新
            TotalCount = i;		//总共接收到多少个封包
            LostCount = i;		//丢包数量
            LEDStaus = (byte)i;			//1Byte，当前的LED灯的状态， 0 = 灭；1 = 亮；2 = 闪烁
            SleepTime = (byte)i;			//1Byte，当前的LED灯的状态， 0 = 灭；1 = 亮；2 = 闪烁
        }

        public void changeStr(CardUpDataBean cardb) 
        {
            ReceivData = cardb.ReceivData;			//是否接受到资料
            MType = cardb.MType;					//定位信息类型
            DrivaceType = cardb.DrivaceType;			//设备类型            
            Port1ID = cardb.Port1ID;		//保存最近的一个Router的ID
            Port1Rssi = cardb.Port1Rssi;				//保存最近的一个Router接受数据时候的信号强度
            Port2ID = cardb.Port2ID;
            Port2Rssi = cardb.Port2Rssi;
            Port3ID = cardb.Port3ID;
            Port3Rssi = cardb.Port3Rssi;
            Battery = cardb.Battery;		//电池电量
            SensorTime = cardb.SensorTime;
            Index = cardb.Index;					//本次资料的序列号	
            FirstReceiveTime = cardb.FirstReceiveTime;		//第一次接受到数据的时间
            IsUpdate = cardb.IsUpdate;				//数据是否有更新（有新的定位数据到来，需要更新到列表中），True为更新
            TotalCount = cardb.TotalCount;		//总共接收到多少个封包
            LostCount = cardb.LostCount;		//丢包数量
            LEDStaus = cardb.LEDStaus;			//1Byte，当前的LED灯的状态， 0 = 灭；1 = 亮；2 = 闪烁
            SleepTime = cardb.SleepTime;			//1Byte，当前的LED灯的状态， 0 = 灭；1 = 亮；2 = 闪烁
            sendGongLv = cardb.sendGongLv;
            TagID_Int = cardb.TagID_Int;
            Port1ID_Int = cardb.Port1ID_Int;
        }

        public byte[] getCardUpByte(byte packData) //将CardUpDataBean对象换成二进制对象，string类型的，不需要
        {
            byte[] cInfor = new byte[27];

            cInfor[0] = 0xfd;
            cInfor[1] = packData;
            Array.Copy(TagId, 0, cInfor, 2, 2);
            Array.Copy(FirstTimeByte, 0, cInfor, 4, 4);
            Array.Copy(Port1IDHistory, 0, cInfor, 8, 2);
            cInfor[10] = Port1RssiHistory;
            Array.Copy(Port2IDHistory, 0, cInfor, 11, 2);
            cInfor[13] = Port2RssiHistory;
            Array.Copy(Port3IDHistory, 0, cInfor, 14, 2);
            cInfor[16] = Port3RssiHistory;
            cInfor[17] = Index;

            cInfor[18] = (byte)(SensorTime / 0x100);
            cInfor[19] = (byte)(SensorTime % 0x100);
            cInfor[20] = Battery;
            cInfor[21] = sleepTime;
            cInfor[22] = LEDStaus ;
            cInfor[23] = sendGongLv;
            cInfor[24] = DrivaceType;

            cInfor[cInfor.Length - 2] = XWUtils.getCheckBit(cInfor, 0, cInfor.Length - 2);
            cInfor[cInfor.Length - 1] = 0xfb;
            return cInfor;
        }


        public CardUpDataBean(byte[] data)
        {
            Array.Copy(data, 2, TagId, 0, 2);
            firstReceiveTime = (uint)(data[4] << 24 | data[5] << 16 | data[6] << 8 | data[7]);
            Array.Copy(data, 4, FirstTimeByte, 0, 4);
            Array.Copy(data, 8, Port1ID, 0, 2);
            Port1Rssi = data[10];
            Array.Copy(data, 11, Port2ID, 0, 2);
            Port2Rssi = data[13];
            Array.Copy(data, 14, Port3ID, 0, 2);
            Port3Rssi = data[16];
            Array.Copy(data, 8, Port1IDHistory, 0, 2);
            Port1RssiHistory = data[10];
            Array.Copy(data, 11, Port2IDHistory, 0, 2);
            Port2RssiHistory = data[13];
            Array.Copy(data, 14, Port3IDHistory, 0, 2);
            Port3RssiHistory = data[16];
            Index = data[17];
            SensorTime = (ushort)(data[18] << 8 | data[19]);
            Battery = data[20];
            sleepTime = data[21];
            LEDStaus = data[22];
            sendGongLv = data[23];
            DrivaceType = data[24];
        }


        //是否有参考点ID
        public bool isHaceCanKaoDianID(string canKaoID)
        {
            if (!canKaoID.Equals(Port1IDStr) && !canKaoID.Equals(Port2IDStr) && !canKaoID.Equals(Port3IDStr)) return false;
            return true;
        }

        public string PortIDStr
        {
            get { return PortID[0].ToString("X2") + PortID[1].ToString("X2"); }
        }

        public byte[] PortID
        {
            get { return portID; }
            set { portID = value; }
        }

        public int TagID_Int
        {
            get 
            {
                if (tagID_Int == 0 && tagId != null && tagId.Length == 2) return tagId[0] * 0x100 + tagId[1];
                return tagID_Int; }
            set { tagID_Int = value; }
        }

        public int ChangeIndex
        {
            get { return changeIndex; }
            set { changeIndex = value; }
        }

        public int Port1ID_Int
        {
            get {
                if (port1ID_Int == 0 && port1ID != null && port1ID.Length == 2) return port1ID[0] * 0x100 + port1ID[1];
                return port1ID_Int; }
            set { port1ID_Int = value; }
        }

        public byte LEDStaus
        {
            get { return ledStaus; }
            set { ledStaus = value; }
        }

        public byte SleepTime
        {
            get { return sleepTime; }
            set { sleepTime = value; }
        }

        public int LostCount
        {
            get { return lostCount; }
            set { lostCount = value; }
        }

        public int TotalCount
        {
            get { return totalCount; }
            set { totalCount = value; }
        }

        public bool IsUpdate
        {
            get { return isUpdate; }
            set { isUpdate = value; }
        }

        public UInt32 FirstReceiveTime
        {
            get { return firstReceiveTime; }
            set { firstReceiveTime = value; }
        }

        public byte[] FirstTimeByte
        {
            get 
            {
                byte[] timeByte = new byte[4];
                timeByte[0] = (byte)(firstReceiveTime / 0x1000000);
                timeByte[1] = (byte)(firstReceiveTime % 0x1000000 / 0x10000);
                timeByte[2] = (byte)(firstReceiveTime % 0x10000 / 0x100);
                timeByte[3] = (byte)(firstReceiveTime % 0x100 );
                return timeByte;
            }
        }

        public byte Index
        {
            get { return index; }
            set { index = value; }
        }

        public UInt16 SensorTime
        {
            get { return sensorTime; }
            set { sensorTime = value; }
        }

        public byte Battery
        {
            get { return battery; }
            set { battery = value; }
        }

        public byte Port3Rssi
        {
            get { return port3Rssi; }
            set { port3Rssi = value; }
        }

        public string Port3IDStr
        {
            get { return port3ID[0].ToString("X2") + port3ID[1].ToString("X2"); }
        }

        public byte[] Port3ID
        {
            get { return port3ID; }
            set { port3ID = value; }
        }

        public byte Port2Rssi
        {
            get { return port2Rssi; }
            set { port2Rssi = value; }
        }

        public string Port2IDStr
        {
            get { return port2ID[0].ToString("X2") + port2ID[1].ToString("X2"); }
        }

        public byte[] Port2ID
        {
            get { return port2ID; }
            set { port2ID = value; }
        }

        public byte Port1Rssi
        {
            get { return port1Rssi; }
            set { port1Rssi = value; }
        }

        public string Port1IDStr
        {
            get { return port1ID[0].ToString("X2") + port1ID[1].ToString("X2"); }
        }

        public byte[] Port1ID
        {
            get { return port1ID; }
            set { port1ID = value; }
        }


        public string TagIdStr
        {
            get { return tagId[0].ToString("X2") + tagId[1].ToString("X2"); }
        }


        public byte[] TagId
        {
            get { return tagId; }
            set { tagId = value; }
        }

        public byte DrivaceType
        {
            get { return drivaceType; }
            set { drivaceType = value; }
        }

        /// <summary>
        /// 1是普通定位，2是紧急定位
        /// </summary>
        public byte MType
        {
            get { return type; }
            set { type = value; }
        }

        public bool ReceivData 
        {
            get { return receivData; }
            set { receivData = value; }
        }

        public byte SendGongLv
        {
            get { return sendGongLv; }
            set { sendGongLv = value; }
        }

    }
}
