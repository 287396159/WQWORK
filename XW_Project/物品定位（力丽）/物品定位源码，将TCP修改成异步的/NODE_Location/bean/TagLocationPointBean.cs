using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CiXinLocation.bean
{
    public class TagLocationPointBean
    {
        string name;
        byte[] cardID;       
        Point point;
        LocationType lcType;
        private int color = 1;
        private int[] colWeiHei;
        private byte type;
        private int countIndex = 0;
        private int sleepTime;
        private byte gonglv = 0xff;

        

        private long showTime = 0;

        /*private bool showCanKaoDian = true;//显示参考点
        private bool showJingJiTag = false;//显示紧急定位
        private bool showBlackTag = true;//长时间不移动，显示黑点
        private bool noShowTag = true;  //长时间未显示信息，不显示黑点
        private int blackTime = 60; //长时间不移动时间
        private int noReveTime = 60; //长时间未收到信息时间*/

       public int SleepTime
       {
           get { return sleepTime; }
           set { sleepTime = value; }
       }

       public byte Gonglv
       {
           get { return gonglv; }
           set { gonglv = value; }
       }
        

        public byte[] CardID
        {
            get { return cardID; }
            set { cardID = value; }
        }

        public int CountIndex
        {
            get { return countIndex; }
            set { countIndex = value; }
        }

        public string CardName
        {
            get {
                if (CardID == null || cardID.Length != 2) return "";
                return cardID[0].ToString("X2") + cardID[1].ToString("X2"); }            
        }

        public TagLocationPointBean()
        { 
            colWeiHei = new int[2];
        }

        public long ShowTime
        {
            get { return showTime; }
            set { showTime = value; }
        }


        public byte MType
        {
            get { return type; }
            set { type = value; }
        }


        /// <summary>
        /// 背景颜色代表，1代表黑色，2代表蓝色，3代表红色，4代表粉红色
        /// </summary>
        public int MColor
        {
            get { return color; }
            set { color = value; }
        }

        public int[] ColWeiHei
        {
            get { return colWeiHei; }
            set { colWeiHei = value; }
        }

        /*public bool ShowCanKaoDian
        {
            get { return showCanKaoDian; }
            set { showCanKaoDian = value; }
        }

        public bool ShowJingJiTag
        {
            get { return showJingJiTag; }
            set { showJingJiTag = value; }
        }

        public bool ShowBlackTag
        {
            get { return showBlackTag; }
            set { showBlackTag = value; }
        }

        public bool NoShowTag
        {
            get { return noShowTag; }
            set { noShowTag = value; }
        }

        public int BlackTime
        {
            get { return blackTime; }
            set { blackTime = value; }
        }

        public int NoReveTime
        {
            get { return noReveTime; }
            set { noReveTime = value; }
        }*/


        public string Name 
        {
            get { return name; }
            set { name = value; }
        }

        public Point Point
        {
            get { return point; }
            set { point = value; }
        }

        public LocationType LcType
        {
            get { return lcType; }
            set { lcType = value; }
        }

    }
}
