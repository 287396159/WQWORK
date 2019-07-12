using SerialportSample;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CiXinLocation.bean
{
    /// <summary>
    /// 单点定位时，卡片位置参考
    /// </summary>
    class SingleTagBean
    {
        private byte[] canID;
        private Dictionary<string, byte[]> cardS = new Dictionary<string, byte[]>();//卡片待在參考點的倒計次數        
        private Dictionary<string, int[]> caedRemoveCount = new Dictionary<string, int[]>();//卡片待在參考點的倒計次數    
        public SingleTagBean() 
        {
            canID = new byte[] { 0,0};
        }

        /// <summary>
        /// 单点定位，获取参考点周围可用的位置
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        public Point getPint(byte[] cardID) 
        {  
            int tagCount = cardS.Count;
            int index = 0;
            foreach (var cardItem in cardS)
            {
                if (!XWUtils.byteBTBettow(cardID, cardItem.Value, cardID.Length)) 
                {
                    index++;
                    continue;
                } 
                return getCaPoint(index);
            }
            return new Point(0,0);
        }

        /// <summary>
        /// 8个方向,上、下、左、右、左上、左下、右上、右下，，获取不同位置的点，以顺时针反向
        /// </summary>
        /// <param name="index_c"></param>
        /// <returns></returns>
        public Point getCaPoint(int index_c) 
        {
            int x = 40;
            int y = 20;
            Point point; 
            if  (index_c % 3 == 0)     point = new Point(x+10,-y);
            else if (index_c % 3 == 1) point = new Point(x + 10, 15);
            else if (index_c % 3 == 2) point = new Point(x + 10, y + 30);
            else point = new Point(0,0);
            return point;
        }

        public static Point getStaticCaPoint(int index_c)
        {
            int x = 30;
            int y = 10;
            Point point;
            if (index_c % 3 == 0) point = new Point(x + 10, -y+10);
            else if (index_c % 3 == 1) point = new Point(x + 10, 20);
            else if (index_c % 3 == 2) point = new Point(x + 10, y + 29);
            else point = new Point(0, 0);
            return point;
        }

        public void addCard(byte[] cardID,int index) 
        {
            if (cardS == null) cardS = new Dictionary<string, byte[]>();
            if (!cardS.ContainsKey(cardID[0].ToString("X2") + cardID[1].ToString("X2"))) 
            {
                cardS.Add(cardID[0].ToString("X2") + cardID[1].ToString("X2"),cardID);
                int[] singInt = { 0, index };
                CaedRemoveCount.Add(cardID[0].ToString("X2") + cardID[1].ToString("X2"), singInt);//Contains(ID[0].ToString("X2")+ID[1].ToString("X2"))
            }
        }

        public void deleCard(byte[] cardID)
        {
            if (cardS == null) cardS = new Dictionary<string, byte[]>();

            if (cardS.ContainsKey(cardID[0].ToString("X2") + cardID[1].ToString("X2")))
            {
                cardS.Remove(cardID[0].ToString("X2") + cardID[1].ToString("X2"));
                CaedRemoveCount.Remove(cardID[0].ToString("X2") + cardID[1].ToString("X2"));
            }               
            
        }

        /// <summary>
        /// 是否存在Tag
        /// </summary>
        /// <returns></returns>
        public bool tagHave(byte[] cardID) 
        {
            return cardS.ContainsKey(cardID[0].ToString("X2") + cardID[1].ToString("X2"));
        }

        /*public List<byte[]> CardS
        {
            get { return cardS; }
            set { cardS = value; }
        }*/

        private Dictionary<string, byte[]> CardS
        {
            get { return cardS; }
            set { cardS = value; }
        }

        public byte[] CanID
        {
            get { return canID; }
            set { canID = value; }
        }

        public Dictionary<string, int[]> CaedRemoveCount
        {
            get { return caedRemoveCount; }
            set { caedRemoveCount = value; }
        }

    }
}
