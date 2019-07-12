using SerialportSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiXinLocation.bean
{
    [Serializable]
    public class CardBean
    {
        private string name;
        private string id;
        private byte[] cardID;
        private UInt32 firstReceiveTime;		//第一次接受到数据的时间
        

        public CardBean() 
        {
            cardID = new byte[2];
        }


        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public byte[] CardID
        {
            get 
            {
                if (id == null) return null;
                int i = XWUtils.hexStrToInt1(Id) & 0xffff;
                if (cardID == null) cardID = new byte[2];
                cardID[0] = (byte)(i / 0x100);
                cardID[1] = (byte)(i % 0x100);
                return cardID; }
        }

        public UInt32 FirstReceiveTime
        {
            get { return firstReceiveTime; }
            set { firstReceiveTime = value; }
        }

    }
}
