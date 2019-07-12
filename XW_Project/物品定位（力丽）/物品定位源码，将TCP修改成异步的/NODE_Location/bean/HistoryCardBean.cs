using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiXinLocation.bean
{
    class HistoryCardBean
    {
        private byte[] tagId;			//
        private byte[] port1ID = new byte[2] { 0, 0 };		//保存最近的一个Router的ID
        private byte port1Rssi = 0xff;				//保存最近的一个Router接受数据时候的信号强度
        private byte[] port2ID = new byte[2] { 0, 0 };
        private byte port2Rssi = 0xff;
        private byte[] port3ID = new byte[2] { 0, 0 };
        private byte port3Rssi = 0xff;
        private UInt32 firstReceiveTime;		//第一次接受到数据的时间

        public HistoryCardBean() 
        { }

        public HistoryCardBean(CardUpDataBean cardb)
        {
            TagId = cardb.TagId;			//
            Port1ID = cardb.Port1ID;		//保存最近的一个Router的ID
            Port1Rssi = cardb.Port1Rssi;				//保存最近的一个Router接受数据时候的信号强度
            Port2ID = cardb.Port2ID;
            Port2Rssi = cardb.Port2Rssi;
            Port3ID = cardb.Port3ID;
            Port3Rssi = cardb.Port3Rssi;
            FirstReceiveTime = cardb.FirstReceiveTime;		//第一次接受到数据的时间
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

        public UInt32 FirstReceiveTime
        {
            get { return firstReceiveTime; }
            set { firstReceiveTime = value; }
        }


    }
}
