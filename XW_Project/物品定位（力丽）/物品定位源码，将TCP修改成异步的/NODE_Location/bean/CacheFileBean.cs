using CiXinLocation.bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoveableListLib.Bean
{
    [Serializable]
    public class CacheFileBean
    {
        private string locaIP;
        private int locaPort;
        private string locaIP_TCP;
        private int locaPort_TCP;
        private string serverIP_TCP;
        private int serverPort_TCP;       
        private bool showCanKaoDian = true;//显示参考点
        private bool showJingJiTag = false;//显示紧急定位
        private bool showBlackTag = true;//长时间不移动，显示黑点
        private bool noShowTag = true;  //长时间未显示信息，不显示黑点
        private bool showTagLow = true;  //长时间未显示信息，不显示黑点

        private int blackTime = 60; //长时间不移动时间
        private int noReveTime = 60; //长时间未收到信息时间
        private int checkC = 5;
        private int tagLow = 10;  //长时间未显示信息，不显示黑
        private int blackTimeText = 60; //长时间不移动时间
        private int noReveTimeText = 60; //长时间未收到信息时间        
        private int checkCText = 5;
        private int tagLowText = 10;  //长时间未显示信息，不显示黑点

        private List<PeopleBean> peoples;
        private List<CardBean> cards;
        private List<CanKaoDianBean> canKaoDians;
        private List<NODEBean> nodes;  //当前力丽不需要这个数据，只是干放着，没有任何数据
        private Dictionary<string, CardBean> cardDic;//Dictionary        

        public CacheFileBean()
        {
            peoples = new List<PeopleBean>();
            cards = new List<CardBean>();
            canKaoDians = new List<CanKaoDianBean>();
            nodes = new List<NODEBean>();
            cardDic = new Dictionary<string, CardBean>();
            addChaojiYonhu();
        }

        public void addCardValues(List<CardBean> cardBns) 
        {
            if (cardBns == null || cardBns.Count == 0) return;
            if (cardDic == null) cardDic = new Dictionary<string, CardBean>();
            foreach (var item in cardBns)
            {
                if (!cardDic.ContainsKey(item.Id))
                    cardDic.Add(item.Id, item);
            }
        }

        public void setCardValues(CardBean cardbn)
        {
            setCardValues(cardbn.Id, cardbn.Name);                 
        }

        public void setCardValues(string cardID,string name)//修改卡片
        {
            if (cardDic.ContainsKey(cardID))
            {
                if (cardDic[cardID].Name.Equals(name)) return;
                foreach (var cardItem in cards)
                {
                    if (!cardID.Equals(cardItem.Id)) continue;
                    if (!cardItem.Name.Equals(name))
                    {
                        cardItem.Name = name;
                        cardDic[cardID].Name = name;
                    }
                    return;
                }
            }               
        }


        public int TagLow
        {
            get { return tagLow; }
            set { tagLow = value; }
        }

        public bool ShowTagLow
        {
            get { return showTagLow; }
            set { showTagLow = value; }
        }

        public int TagLowText
        {
            get { return tagLowText; }
            set { tagLowText = value; }
        }

        public void deleteCardValues(string cardID)//删除卡片
        {
            if (cardDic.ContainsKey(cardID))
            {
                cardDic.Remove(cardID);
            }
        }

        public void addCardValues(string cardID, string name)//添加卡片或者修改卡片
        {
            if (cardDic == null) cardDic = new Dictionary<string, CardBean>();
            if ( cardDic.ContainsKey(cardID)) 
            {
                setCardValues(cardID, name);
            }
            else
            {
                CardBean cardBean = new CardBean();
                cardBean.Id = cardID;
                cardBean.Name = name;
                cardDic.Add(cardID, cardBean);
                cards.Add(cardBean);
            }  
        }

        public Dictionary<string, CardBean> CardDic
        {
            get { return cardDic; }
            set { cardDic = value; }
        }

        public int CheckCText
        {
            get { return checkCText; }
            set { checkCText = value; }
        }   

        public int NoReveTimeText
        {
            get { return noReveTimeText; }
            set { noReveTimeText = value; }
        }

        public int BlackTimeText
        {
            get { return blackTimeText; }
            set { blackTimeText = value; }
        }

        public int CheckC
        {
            get { return checkC; }
            set { checkC = value; }
        }

        /// <summary>
        /// 添加超级用户
        /// </summary>
        private void addChaojiYonhu() 
        {
            PeopleBean pb = new PeopleBean();
            pb.Name = "007";
            pb.PassWord = "xw2017";
            pb.Id = "fg123456";
            pb.Jurisdiction = 0;
            pb.PowerValue = 0xffffff;
            addPeopeo(pb);
        }

        public bool ShowCanKaoDian
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

        /// <summary>
        /// 卡片变黑的时间
        /// </summary>
        public int BlackTime
        {
            get { return blackTime; }
            set { blackTime = value; }
        }

        /// <summary>
        /// 卡片没有移动时间的设定或读取
        /// </summary>
        public int NoReveTime
        {
            get { return noReveTime; }
            set { noReveTime = value; }
        }

        public string LocaIP_TCP
        {
            get { return locaIP_TCP; }
            set { locaIP_TCP = value; }
        }

        public int LocaPort_TCP
        {
            get { return locaPort_TCP; }
            set { locaPort_TCP = value; }
        }

        public string ServerIP_TCP
        {
            get { return serverIP_TCP; }
            set { serverIP_TCP = value; }
        }

        public int ServerPort_TCP
        {
            get { return serverPort_TCP; }
            set { serverPort_TCP = value; }
        }

        public string LocaIP
        {
            get { return locaIP; }
            set { locaIP = value; }
        }

        public int LocaPort
        {
            get { return locaPort; }
            set { locaPort = value; }
        }

        public List<PeopleBean> Peoples
        {
            get { return peoples; }
            set { peoples = value; }
        }

        public List<CardBean> Cards
        {
            get { return cards; }
            set { cards = value; }
        }

        public List<CanKaoDianBean> CanKaoDians
        {
            get { return canKaoDians; }
            set { canKaoDians = value; }
        }

        public List<NODEBean> NODES
        {
            get { return nodes; }
            set { nodes = value; }
        }

        public void addPeopeo(PeopleBean pleBn) 
        {
            peoples.Add(pleBn);
        }

        /// <summary>
        /// 添加設備 CardBean、CanKaoDianBean、NODEBean這三種
        /// </summary>
        public void addDrivace(object obj) 
        {
            if (obj is CardBean)
            {
                CardBean card = (CardBean)obj;
                Cards.Add(card);
                cardDic.Add(card.Id,(CardBean)obj);
            } 
            else if (obj is CanKaoDianBean) CanKaoDians.Add((CanKaoDianBean)obj);
            else if (obj is NODEBean) nodes.Add((NODEBean)obj);
        }

    }
}
