using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiXinLocation.bean
{
    /// <summary>
    /// 应用重启前保存的状态
    /// </summary>
    [Serializable]
    public class RestartBean
    {
        public RestartBean(){ }
        private int pcType = 3; //主机还是从机，1 = 主机，2 = 从机。
        private string count;  //账号       
        private string password;  //密码    
        private int powerValue = 0;  // 人员权限值       
        private int jurisdiction = 50;//值越大，权限越小；  
        private long restartTime = 0;//重启的时间

        private List<CardUpDataBean> dealUpCardDatas;
        private Dictionary<string, DrivaceWarnMessage> cardLowEleWarnMsgs;//
        private Dictionary<string, DrivaceWarnMessage> cardUnanswerWranMsgs;//
        private Dictionary<string, DrivaceWarnMessage> nODEUnanswerWranMsgs;//
        private bool uDPopen = false;
        private bool tCPopen = false;
        private bool tCPClienopen = false;

        public bool TCPClienopen
        {
            get { return tCPClienopen; }
            set { tCPClienopen = value; }
        }

        public long RestartTime
        {
            get { return restartTime; }
            set { restartTime = value; }
        }

        public int Jurisdiction
        {
            get { return jurisdiction; }
            set { jurisdiction = value; }
        }

        public int PowerValue
        {
            get { return powerValue; }
            set { powerValue = value; }
        }

        
        public bool TCPopen
        {
            get { return tCPopen; }
            set { tCPopen = value; }
        }

        public bool UDPopen
        {
            get { return uDPopen; }
            set { uDPopen = value; }
        }

        /// <summary>
        /// //主机还是从机，1 = 主机，2 = 从机。
        /// </summary>
        public int PcType
        {
            get { return pcType; }
            set { pcType = value; }
        }

        public string Count
        {
            get { return count; }
            set { count = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public List<CardUpDataBean> DealUpCardDatas
        {
            get { return dealUpCardDatas; }
            set { dealUpCardDatas = value; }
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


    }
}
