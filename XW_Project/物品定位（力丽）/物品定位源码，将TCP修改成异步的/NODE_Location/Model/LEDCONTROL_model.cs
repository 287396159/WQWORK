using CiXinLocation.Utils;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CiXinLocation.Model
{
    public class LEDCONTROL_model:FromBaseModel
    {
        byte[] ledID;

        public delegate void labTextSetHandle(string text, int type); //修改lAB
        public delegate void labVisiSetHandle(int type, bool visi); //确定是否显示Lab
        public labTextSetHandle labSet;
        public labVisiSetHandle labVisi;     

        private int labVisiComTime;    //时长，单位秒
        private int labVisiSleepTime; //时长，单位秒
        private int labVisiGLTime; //时长，单位秒
        private bool threadComLab = false;
        private bool threadSleepLab = false;
        private bool threadGLLab = false;

        private LedTypeBean comBean;
        private LedTypeBean sleepTimeBean;
        private LedTypeBean GlBean;

        private bool modelClose = false;

        public LEDCONTROL_model() 
        {
            receVeByteHandle = 0xfe;//接收包头
            receVeByteend = 0xfd;   //接收包尾
            comMode = CommunicationMode.SERIALPORT;//是UDP通信，但是此处是为了处理多包功能    

            comBean = new LedTypeBean(1);
            sleepTimeBean = new LedTypeBean(2);
            GlBean = new LedTypeBean(3);
        }

        private void sendText(string text, int type) 
        {
            if (labSet != null) labSet(text, type);
        }

        public override void reveData(byte[] buf, string ipInfo)
        {
            if (modelClose) return;
            reveData(buf);
        }

        public void reveMainModelData(byte[] buf, string ipInfo)
        {
            reveData(buf);
        }

        public override void reveData(byte[] buf) 
        {
            if (modelClose) return;
            if (buf[0] != 0XFE || buf[buf.Length - 1] != 0xFD || buf.Length < 6) return;
            if (buf[1] == 5) return;
            if (ledID == null || ledID.Length != 2) return; 
            byte status = buf[4];
            //int type = buf[1] > 0x11 ? 1 : 2;
            //if (buf[1] == 0x16 || buf[1] == 0x17) type = 3;
            switch (buf[1])
            {
                case 0x10:
                    sleepTimeBean.NodeReca = true;
                    nodeRecv(status, sleepTimeBean.Type);
                    break;
                case 0x12:
                    comBean.NodeReca = true;
                    nodeRecv(status, comBean.Type);
                    break;
                case 0x14:
                    comBean.NodeReca = true;
                    nodeRecv(status, comBean.Type);
                    break;
                case 0x16:
                    GlBean.NodeReca = true;
                    nodeRecv(status, GlBean.Type);
                    break;
                case 0x11:
                    cardRecv(status, sleepTimeBean.Type);
                    break;
                case 0x13:
                    cardRecv(status, comBean.Type);
                    break;
                case 0x15:
                    cardRecv(status, comBean.Type);
                    break;
                case 0x17:
                    cardRecv(status, GlBean.Type);
                    break;
                default:
                    break;
            }
        }

        private void nodeRecv(byte status, int type) 
        {
            if (status == 1) sendText("節點轉發中..", type);
            else if (status == 2) sendText("節點忙，稍後再試", type);//if (labSet != null) labSet("节点忙，稍后再试", 2);
            else sendText("節點拒接操作", type);
        }

        private void cardRecv(byte status, int type) 
        {
            if (type == comBean.Type) comBean.LabVisiTime = 2;
            else if (type == sleepTimeBean.Type) sleepTimeBean.LabVisiTime = 2;
            else GlBean.LabVisiTime = 2;
            if (status == 1) sendText("設置成功", type);
            else if (status == 2) sendText("設置失敗", type);
            else sendText("卡片拒絕回答", type);
        }



        public byte[] LedID
        {
            get { return ledID; }
            set { ledID = value; }
        }

        public void setLEDsleepTime(string SleepTime, string TimeOut) 
        {
            setLED(SleepTime, TimeOut,0x10);
        }


        public void setLED(string Time, string TimeOut, byte packType)
        {
            if (ledID == null || ledID.Length != 2) return;
            int sleepTime = XWUtils.stringToInt1(Time);
            if (sleepTime > 60) 
            {
                MessageBox.Show("最大可設置睡眠時間60秒");
                return;
            }
            int timeOut = XWUtils.stringToInt1(TimeOut);
            if ((packType != 0x16 && !isOnTimeValue(sleepTime)) || !isTimeoutValue(timeOut)) 
            {
                messageShowstr();
                return;
            } 
            byte[] sendColSleepBt = new byte[9];
            sendColSleepBt[0] = 0xf8;
            sendColSleepBt[1] = packType;
            Array.Copy(ledID, 0, sendColSleepBt, 2, 2);
            sendColSleepBt[4] = (byte)sleepTime;
            sendColSleepBt[5] = (byte)(timeOut / 0x100);
            sendColSleepBt[6] = (byte)(timeOut % 0x100);
            sendColSleepBt[7] = XWUtils.getCheckBit(sendColSleepBt, 0, 7);
            sendColSleepBt[8] = 0xf7;
                       
            if (packType == 0x10)
            {
                if (!sleepTimeBean.ThreadLab) new Thread(setVisiLabSleepTimeThread).Start();
                sleepTimeBean.TimeOut = timeOut;
                sleepTimeBean.LabVisiTime = timeOut + 2;
                sleepTimeBean.Count = 0;
                sleepTimeBean.NodeReca = false;
                sleepTimeBean.SendByte = sendColSleepBt;
                if (labVisi != null) labVisi(sleepTimeBean.Type, true);
                sendText("發往節點中", sleepTimeBean.Type);
            }
            else if (packType == 0x16) 
            {
                if (!GlBean.ThreadLab) new Thread(setVisiLabFaSheGLThread).Start();
                GlBean.TimeOut = timeOut;
                GlBean.LabVisiTime = timeOut + 2;
                GlBean.Count = 0;
                GlBean.NodeReca = false;
                GlBean.SendByte = sendColSleepBt;
                if (labVisi != null) labVisi(GlBean.Type, true);
                sendText("發往節點中", GlBean.Type); 
            }
            sendDataInLED(sendColSleepBt, 0, "");               
        }


        public void sendLED7045com(int ledCom, string OnTime, string OffTime, string TotalTime, string TimeOut) 
        {
            if (ledID == null || ledID.Length != 2) return; 

            int timeOut = XWUtils.stringToInt1(TimeOut);
            if (!isTimeoutValue(timeOut)) return;
            byte[] controlBt = new byte[6];
            if (ledCom == 0) controlBt = sendLEDmie();
            else if (ledCom == 1) controlBt = sendLEDliang(OnTime, TotalTime);
            else if (ledCom == 2) controlBt = sendLEDshan(OnTime, OffTime, TotalTime);
            if (controlBt == null) 
            {
                messageShowstr();
                return;
            }

            byte[] sendColBt = new byte[13];
            sendColBt[0] = 0xf8;
            sendColBt[1] = 0x12;
            Array.Copy(ledID, 0, sendColBt,2,2);
            sendColBt[4] = (byte)ledCom;
            Array.Copy(controlBt, 0, sendColBt, 5, 4);
            sendColBt[9]  = (byte)(timeOut / 0x100);
            sendColBt[10] = (byte)(timeOut % 0x100);
            sendColBt[11] = XWUtils.getCheckBit(sendColBt,0,11);
            sendColBt[12] = 0xf7;

            if (!comBean.ThreadLab) new Thread(setVisiLabComThread).Start();
            comBean.TimeOut = timeOut;
            comBean.LabVisiTime = timeOut + 2;
            comBean.Count = 0;
            comBean.NodeReca = false;
            comBean.SendByte = sendColBt;
            if (labVisi != null) labVisi(comBean.Type, true);
            sendText("發往節點中", comBean.Type);

            sendDataInLED(sendColBt, 0, "");
        }

        private void sendDataInLED(byte[] buf, byte tp, string ipInfo) 
        {
            if (PeoplePowerModel.getPeoplePowerModel().Jurisdiction == PeoplePowerModel.getPeoplePowerModel().CongjiValue)
            {
                tp = 2;
            }
            addEndAndsendData(buf, tp, "LEDSENDDATA");
        }

        private bool isTimeoutValue(int timeout)
        {
            if (timeout < 1 || timeout > 65535) return false;          
            return true;
        }

        private bool isOnTimeValue(int OnTime)
        {
            if (OnTime < 1 || OnTime > 255) return false;            
            return true;
        }

        private bool isTotalTimeValue(int TotalTime)
        {
            if (TotalTime < 0 || TotalTime > 65535) return false;
            return true;
        }


        private void messageShowstr() 
        {
            MessageBox.Show("輸入的值超出範圍，請注意下方參考值");
        }


        //14 + ID_k + LEDStatus + RedOnTime + RedOffTime + RedTotalTime + GreenOnTime +GreenOffTime +GreenTotalTime +BlueOnTime +BlueOffTime + BlueTotalTime+ TimeOut + CS + F7
        public void sendLEDE290com(int ledCom, string RedOnTime, string RedOffTime, string RedTotalTime, string GreenOnTime, string GreenOffTime, string GreenTotalTime,
            string BlueOnTime, string BlueOffTime, string BlueTotalTime,string TimeOut)
        {
            if (ledID == null || ledID.Length != 2) return;

            int timeOut = XWUtils.stringToInt1(TimeOut);
            if (!isTimeoutValue(timeOut)) return;

            int redCom = ledCom & 3; //0000 0011  = 3;
            byte[] redBt = getLED290Type(redCom, RedOnTime, RedOffTime, RedTotalTime);            
            int greenCom = (ledCom>>2) & 3; //0000 0011  = 3;
            byte[] greenBt = getLED290Type(greenCom, GreenOnTime, GreenOffTime, GreenTotalTime);
            int blueCom = (ledCom >> 4) & 3; //0000 0011  = 3;
            byte[] blueBt = getLED290Type(blueCom, BlueOnTime, BlueOffTime, BlueTotalTime);

            if (redBt == null || greenBt == null || blueBt == null) 
            {
                messageShowstr();
                return;
            }
            byte[] sendColBy = new byte[21];
            sendColBy[0] = 0xf8;
            sendColBy[1] = 0x14;
            Array.Copy(ledID, 0, sendColBy, 2, 2);
            sendColBy[4] = (byte)ledCom;
            Array.Copy(redBt, 0, sendColBy, 5, 4);
            Array.Copy(greenBt, 0, sendColBy, 9, 4);
            Array.Copy(blueBt, 0, sendColBy, 13, 4);
            sendColBy[17] = (byte)(timeOut / 0x100);
            sendColBy[18] = (byte)(timeOut % 0x100);
            sendColBy[19] = XWUtils.getCheckBit(sendColBy, 0, 19);
            sendColBy[20] = 0xf7;

            if (!comBean.ThreadLab) new Thread(setVisiLabComThread).Start();
            comBean.TimeOut = timeOut;
            comBean.LabVisiTime = timeOut + 2;
            comBean.Count = 0;
            comBean.NodeReca = false;
            comBean.SendByte = sendColBy;
            if (labVisi != null) labVisi(comBean.Type, true);
            sendText("發往節點中", comBean.Type);

            sendDataInLED(sendColBy, 0, "");

        }
        

        private byte[] getLED290Type(int ledCom, string OnTime, string OffTime, string TotalTime) 
        {            
            if (ledCom == 1) return sendLEDliang(OnTime, TotalTime);
            else if (ledCom == 2) return sendLEDshan(OnTime, OffTime, TotalTime);
            else return sendLEDmie();
        }

        public byte[] sendLEDmie() 
        {
            return new byte[4] { 0, 0, 0, 0  };
        }

        public byte[] sendLEDliang(string OnTime, string TotalTime)
        {
            int onTime = 0; //XWUtils.stringToInt1(OnTime);
            int totalTime = XWUtils.stringToInt1(TotalTime);
            if (!isTotalTimeValue(totalTime)) return null;
            byte[] colBt = new byte[4] { (byte)onTime, 0, (byte)(totalTime / 0x100), (byte)(totalTime % 0x100)};
            return colBt;
        }

        public byte[] sendLEDshan(string OnTime, string OffTime, string TotalTime)
        {
            int onTime = XWUtils.stringToInt1(OnTime);
            int offTime = XWUtils.stringToInt1(OffTime);
            int totalTime = XWUtils.stringToInt1(TotalTime);
            if (!isOnTimeValue(offTime) || !isOnTimeValue(onTime) || !isTotalTimeValue(totalTime)) return null;
            byte[] colBt = new byte[4] { (byte)onTime, (byte)offTime, (byte)(totalTime / 0x100), (byte)(totalTime % 0x100)};
            return colBt;
        }


        private void setVisiLabComThread() 
        {
            threadBean(comBean);
        }

        private void threadBean(LedTypeBean ledBean) 
        {
            ledBean.ThreadLab = true;
            while (ledBean.LabVisiTime > 0 && !modelClose)
            {
                if (!ledBean.NodeReca && ledBean.Count < 5)
                {
                    try
                    {
                        Thread.Sleep(500);
                        if (!ledBean.NodeReca)
                        {
                            sendDataInLED(ledBean.SendByte, 0, "");
                            ledBean.Count++;
                            continue;
                        }                                                
                    }
                    catch { }
                }
                ledBean.LabVisiTime -= 1;
                try
                {
                    Thread.Sleep(1000);
                }
                catch { }
            }
            if (labVisi != null && !modelClose) labVisi(ledBean.Type, false);
            //threadComLab = false;
            ledBean.ThreadLab = false;
        }

        private void setVisiLabSleepTimeThread()
        {
            threadBean(sleepTimeBean);
        }

        private void setVisiLabFaSheGLThread()
        {
            threadBean(GlBean);
        }

        public override void close()
        {
            modelClose = true;
        }

        /// <summary>
        /// 返回MODEL的标志位
        /// </summary>
        /// <returns></returns>
        public override string TAG() { return "LEDCONTROL_model"; }
    }

    public class LedTypeBean
    {
        private int labVisiTime;    //时长，单位秒
        private bool threadLab = false;
        private int timeOut;
        private int type;
        private int count = 0;
        private bool nodeReca = false;
        private byte[] sendByte;    //时长，单位秒

        public byte[] SendByte
        {
            get { return sendByte; }
            set { sendByte = value; }
        }

        public bool NodeReca
        {
            get { return nodeReca; }
            set { nodeReca = value; }
        }

        public LedTypeBean(int type) 
        {
            this.type = type;
        }


        public int LabVisiTime
        {
            get { return labVisiTime; }
            set { labVisiTime = value; }
        }


        public bool ThreadLab
        {
            get { return threadLab; }
            set { threadLab = value; }
        }


        public int TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; }
        }

        public int Type
        {
            get { return type; }
        }


        public int Count
        {
            get { return count; }
            set { count = value; }
        }


    }

}
