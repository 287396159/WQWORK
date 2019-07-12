using CiXinLocation.bean;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CiXinLocation.Model
{
    public class NODESearchModel : FromBaseModel
    {

        private CanKaoDianUpInfo udpCanKaoDianUpInfo;       
        private RecvCallBack recvInter;
        private byte[] NODEid;

        public NODESearchModel(CanKaoDianUpInfo udpCanKaoDianUpInfo, RecvCallBack recvInter) 
        {
            receVeByteHandle = 0xfe; //接收包头
            receVeByteend = 0xfd;    //接收包尾
            this.udpCanKaoDianUpInfo = udpCanKaoDianUpInfo;
            this.recvInter = recvInter;
        }

        public override void reveData(byte[] buf, string ipInfo)
        {
            if (buf[1] != 0x5a) return;
            if(buf.Length < 10) return;
            byte[] recvID = new byte[2];
            Array.Copy(buf, 2, recvID, 0, 2);
            if (udpCanKaoDianUpInfo == null) return;           
            byte[] recvData = new byte[buf.Length - 6];
            Array.Copy(buf, 4, recvData, 0, recvData.Length);
            if (!isCheckPacket(buf)) return;
            if (recvData[0] != 0xfa || recvData[recvData.Length - 1] != 0xf9) return;
            byte[] cvData = new byte[recvData.Length - 4];
            Array.Copy(recvData, 2, cvData, 0, cvData.Length);
            recvInter.callBack(cvData, recvData[1], ipInfo);
        }

        private void setTouChuanData(byte type, byte[] data) 
        {
            int dataLength = data == null ? 0 : data.Length;
            byte[] sendData = new byte[4 + dataLength];
            sendData[0] = 0xfa;
            sendData[1] = type;
            if (data != null) Array.Copy(data, 0, sendData, 2, dataLength);
            sendData[2 + dataLength] = XWUtils.getCheckBit(sendData, 0, 2 + dataLength);
            sendData[3 + dataLength] = 0xf9;

            byte[] handData = new byte[6 + sendData.Length];
            handData[0] = 0xf8;
            handData[1] = 0x5A;
            if (udpCanKaoDianUpInfo != null)
                Array.Copy(data, 0, handData, 2, 2);
            Array.Copy(sendData, 0, handData, 4, sendData.Length);
            handData[handData.Length - 2] = XWUtils.getCheckBit(handData, 0, handData.Length - 2);
            handData[handData.Length - 1] = 0xf7;
            //data = handData;
            addEndAndsendData(handData, 0, udpCanKaoDianUpInfo.IpInfo);
        }

        private void pingjieData(byte type,byte[] nodeID, byte[] data) 
        {
            byte[] idIp = new byte[nodeID.Length + data.Length];
            Array.Copy(nodeID, 0, idIp, 0, nodeID.Length);
            Array.Copy(data, 0, idIp, 2, data.Length);
            setTouChuanData(type, idIp);
        }

        private void pingjieData(byte type, byte[] nodeID, byte data)
        {
            byte[] dt = new byte[1]{data};
            pingjieData(type, nodeID, dt);
        }

        public CanKaoDianUpInfo UdpCanKaoDianUpInfo
        {
            get { return udpCanKaoDianUpInfo; }
            set { udpCanKaoDianUpInfo = value; }
        }
        /// <summary>
        /// 搜寻卡片ID
        /// </summary>
        public void searchID(byte[] ID) 
        {
            new Thread(a =>
            {
                searchIDThread(ID);
            }).Start();
        }

        private void searchIDThread(byte[] ID)
        {
            int IDLength = ID == null ? 0 : ID.Length;
            byte[] nodeData = new byte[5 + IDLength];// { 0xfa, 0x01, 0x00, ID[0], ID[1], 0x00, 0xf9 };
            nodeData[0] = 0xfa;
            nodeData[1] = 0x01;
            nodeData[2] = (byte)(IDLength / 2);
            if (ID != null) Array.Copy(ID, 0, nodeData, 3, IDLength);
            nodeData[nodeData.Length - 2] = XWUtils.getCheckBit(nodeData, 0, nodeData.Length - 2);
            nodeData[nodeData.Length - 1] = 0xf9;

            nodeData[nodeData.Length - 2] = XWUtils.getCheckBit(nodeData, 0, nodeData.Length - 2);
            byte[] handData = new byte[6 + nodeData.Length];
            handData[0] = 0xf8;
            handData[1] = 0x5a;
            handData[2] = 0xff;
            handData[3] = 0xff;
            Array.Copy(nodeData, 0, handData, 4, nodeData.Length);
            handData[handData.Length - 2] = XWUtils.getCheckBit(handData, 0, handData.Length - 2);
            handData[handData.Length - 1] = 0xf7;

            addEndAndsendData(handData, 0, udpCanKaoDianUpInfo.IpInfo);
        }

        public void checkVersion(byte[] nodeID) 
        {
            new Thread(a =>
            {
                setTouChuanData(0x02, nodeID);
            }).Start();            
        }

        public void SetServerIP(byte[] nodeID,byte[] ip) 
        {
            pingjieData(0x05, nodeID, ip);
        }

        public void readServerIP(byte[] nodeID)
        {
            setTouChuanData(0x06, nodeID);
        }

        public void setServerPort(byte[] nodeID,byte[] port)
        {
            pingjieData(0x07, nodeID, port);
        }

        public void readServerPort(byte[] nodeID)
        {
            setTouChuanData(0x08, nodeID);
        }

        public void setWifiName(byte[] nodeID, byte[] wiifName)
        {
            pingjieData(0x09, nodeID, wiifName);
        }

        public void readWifiName(byte[] nodeID)
        {
            setTouChuanData(0x0a, nodeID);
        }

        public void setWifiPassWord(byte[] nodeID, byte[] wiifName)
        {
            pingjieData(0x0b, nodeID, wiifName);
        }

        public void readWifiPassWord(byte[] nodeID)
        {
            setTouChuanData(0x0c, nodeID);
        }

        public void setNODEModel(byte[] nodeID, byte model)
        {
            pingjieData(0x0e, nodeID, model);
        }

        public void readNODEModel(byte[] nodeID)
        {
            setTouChuanData(0x0f, nodeID);
        }

        public void SetNODEIP(byte[] nodeID, byte[] ip)
        {
            pingjieData(0x10, nodeID, ip);
        }
        public void readNODEIP(byte[] nodeID)
        {
            setTouChuanData(0x11, nodeID);
        }

        public void SetSubmask(byte[] nodeID, byte[] ip)
        {
            pingjieData(0x12, nodeID, ip);
        }
        public void readSubmask(byte[] nodeID)
        {
            setTouChuanData(0x13, nodeID);
        }

        public void SetGateWay(byte[] nodeID, byte[] ip)
        {
            pingjieData(0x14, nodeID, ip);
        }

        public void ReadGateWay(byte[] nodeID)
        {
            setTouChuanData(0x15, nodeID);
        }

        public void SetFazhi(byte[] nodeID, byte fazhi)
        {
            pingjieData(0x16, nodeID, fazhi);
        }

        public void ReadFazhi(byte[] nodeID)
        {
            setTouChuanData(0x17, nodeID);
        }

        public void SetRssiValue(byte[] nodeID, byte rssi)
        {
            pingjieData(0x18, nodeID, rssi);
        }

        public void ReadRssiValue(byte[] nodeID)
        {
            setTouChuanData(0x19, nodeID);
        }

        public void onNODEfuwei(byte[] ID)
        {
            setTouChuanData(0x0d, ID);
        }

        public override void close() {}

        /// <summary>
        /// 返回MODEL的标志位
        /// </summary>
        /// <returns></returns>
        public override string TAG()
        {
            return "NODESearchModel";
        }

        public void readAllParameter(DrivaceTypeAll driType, byte[] ID)
        {
            if (driType == DrivaceTypeAll.NODE) new Thread(readNODEParameter).Start(ID);
        }

        private int sleepTime = 100;
        private void readNODEParameter(object obj)
        {
            if (obj is byte[])
            {
                byte[] locaByteID = (byte[])obj;
                if (locaByteID == null) return;
                NODEid = locaByteID;

                readServerIP(NODEid);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;

                readServerPort(NODEid);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;

                readWifiName(NODEid);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;

                readWifiPassWord(NODEid);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;

                readNODEModel(NODEid);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;

                readNODEIP(NODEid);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;

                readSubmask(NODEid);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;

                ReadGateWay(NODEid);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;

                ReadFazhi(NODEid);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;

                ReadRssiValue(NODEid);
            }
        }

    }

    public interface RecvCallBack
    {
        void callBack(byte[] buff, int type, string ipInfo);
    }

}
