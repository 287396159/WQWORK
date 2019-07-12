using CiXinLocation.bean;
using CiXinLocation.CiXinInterface;
using CiXinLocation.Utils;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace CiXinLocation.Model
{
    public class FileUpdataModel : FromBaseModel, FileUpdataModelInterface
    {

        FileUpdataInterface mFileUpDataInterface;
        HexFileRead hfRead;
        HexFileBean hfModelInfor;
        private int bin_dataLength = 64;//发送数据的长度
        private byte[] ID;
        private byte[] index = new byte[1]{1};
        private bool isUpdata = true;//是否正在更新中..重发线程机制中使用
        private bool isCheck = false; 
        private bool isClose = false;   //界面关闭，或者此类关闭
        private bool stopUpData = false;//是否停止更新
        private DrivaceTypeAll drType;
        private string ipinfo;
        private CanKaoDianUpInfo udpCanKaoDianUpInfo;

       
        public FileUpdataModel(FileUpdataInterface fileUpDataInterface)
        {
            receVeByteHandle = 0xfe; //接收包头
            receVeByteend = 0xfd;    //接收包尾
            CreateFileModel();
            if (fileUpDataInterface == null) return;
            mFileUpDataInterface = fileUpDataInterface;
        }

        private void CreateFileModel() { //创建读取file对象，用于读取文件
            if (hfRead != null) return;
            hfRead = new HexFileRead();
            hfRead.sendDataEventHandle += fileInfor;            
        }
         
        public void fileInformation(HexFileBean hfInfor)
        {
            //if (hfModelInfor != null) fileInfor(hfModelInfor, 0);
            //else {
                CreateFileModel();
                hfRead.readHexFile(hfInfor.Path);    
            //}            
        }
            
        public void fileInfor(HexFileBean hfInfor, int type) //委托回调，创建
        {
            //if (hfInfor.mainDrivaceType == DrivaceType.TAG) index[0] = hfInfor.Index;
            if (type != 0)  hfModelInfor = hfInfor;
            mFileUpDataInterface.fileInformation(hfInfor);         
        }

        public void sendData(byte type,byte[] data) {
            byte[] sendBt = new byte[4 + data.Length];
            sendBt[0] = 0xfa;
            sendBt[1] = type;           
            sendBt[sendBt.Length - 1] = 0xf9;
            Array.Copy(data, 0, sendBt,2,data.Length);
            sendBt[sendBt.Length - 2] = XWUtils.getCheckBit(sendBt, 0, sendBt.Length - 2);

            byte[] handData = new byte[6 + sendBt.Length];
            handData[0] = 0xf8;
            handData[1] = 0x5A;
            Array.Copy(data, 0, handData, 2, 2);
            Array.Copy(sendBt, 0, handData, 4, sendBt.Length);
            handData[handData.Length - 2] = XWUtils.getCheckBit(handData, 0, handData.Length - 2);
            handData[handData.Length - 1] = 0xf7;

            Debug.Write("sendData:" + Ipinfo +"\r\n");
            addEndAndsendData(handData, 0, Ipinfo);            
        }

        private byte[] Addr = new byte[2];
        public void sendBinData(DrivaceTypeAll dType,byte[] ID, byte[] Addr)
        {
            if (stopUpData || hfModelInfor == null || hfModelInfor.G_bin == null || ID == null) return;
            if (Addr[0] == 0 && Addr[1] == 0) {
                isUpdata = true;
                drType = dType; 
                if (dType != DrivaceTypeAll.USB_DANGLE) new Thread(sendData_BinThread).Start();
                this.ID = ID;                             
            }
            this.Addr = Addr;
            new Thread(sendData_Bin).Start();     
        }

        int time = 0;
        bool sleep200(int tm){         
            if (Environment.TickCount - time > tm) {
                return true;
            }
            return false;
        }

        int sendCount = 0;
        private void sendData_BinThread() {
            time = Environment.TickCount;
            int threadTime = time;
            int overTime = (int)(hfModelInfor.FileLength * 4 / 64) * 200+time;
            while (isUpdata) {
                try{
                    for (int i = 0; i < 50;i++ )
                    {
                        Thread.Sleep(10);
                        if (!isUpdata) 
                            break;
                    }
                } catch { }
                if (sleep200(500)) {
                    if (sendCount > 4) {
                        updataResult(ID,0);
                        break;
                    }                    
                    if (isCheck) new Thread(checkBinData).Start();
                    else new Thread(sendData_Bin).Start();
                    Debug.Write("sendData_BinThread" + sendCount.ToString());
                    sendCount++;
                }
                if (Environment.TickCount - threadTime > overTime - (threadTime - time)*4) 
                    return;
            }
        }

        public string Ipinfo
        {
            get { return ipinfo; }
            set { ipinfo = value; }
        }

        public CanKaoDianUpInfo UdpCanKaoDianUpInfo
        {
            get { return udpCanKaoDianUpInfo; }
            set { udpCanKaoDianUpInfo = value; }
        }       

        private void sendData_Bin(){
            int addrLength = Addr[0] * 256 + Addr[1];
            if (addrLength >= hfModelInfor.FileLength) return;

            int copy_length = hfModelInfor.FileLength - addrLength > bin_dataLength ?
                bin_dataLength : (int)(hfModelInfor.FileLength - addrLength);

            byte[] id_index;
            if (drType == DrivaceTypeAll.CARD)
            {
                id_index = index;
            }
            else if (drType == DrivaceTypeAll.USB_DANGLE) {
                id_index = new byte[0];
            } else {
                id_index = ID;                
            }            

            byte[] bin_data = Enumerable.Repeat((byte)0xff, bin_dataLength + Addr.Length + id_index.Length).ToArray();
            Array.Copy(id_index, 0, bin_data, 0, id_index.Length);
            Array.Copy(Addr, 0, bin_data, id_index.Length, Addr.Length);
            Array.Copy(hfModelInfor.G_bin, addrLength, bin_data, Addr.Length + id_index.Length, copy_length);
                       
            time = Environment.TickCount;
            sendData(getPackByte(), bin_data);           
        }        

        private byte getPackByte() {
            byte type = 0;
            if (drType == DrivaceTypeAll.NODE) type = 0x3;
            if (drType == DrivaceTypeAll.CANKAODIAN) type = 0x43;
            if (drType == DrivaceTypeAll.CARD) type = 0x83;
            if (drType == DrivaceTypeAll.USB_DANGLE) type = 0xe2;
            return type;
        }
         
        public void backBinData(byte[] ID, byte[] Addr){}

        public HexFileBean HfModelInfor {
            get { return hfModelInfor; }
        }
        
        public void checkBinData(byte[] ID){
            checkBinData();
        }

        public void checkBinData() {
            // this.ID = new byte[2] { 1, 1 };
            // if (hfModelInfor == null || hfModelInfor.G_bin == null) return;// || this.ID == null
            if (hfModelInfor == null || hfModelInfor.G_bin == null || this.ID == null) return;
            isCheck = true;
            UInt32 filesize = hfModelInfor.FileLength;
            byte[] id_index;
            if (drType == DrivaceTypeAll.CARD)
            {
                id_index = index;
            }
            else
            {
                id_index = ID;
            }
            int id_indexlength = id_index.Length;
            int sendByte_length = 10 + id_indexlength;
            if (drType == DrivaceTypeAll.CARD) sendByte_length += 1;
            byte[] sendByte = new byte[sendByte_length];
            if (drType == DrivaceTypeAll.CARD) sendByte[sendByte.Length - 1] = hfModelInfor.TypeByte[0];

            Array.Copy(id_index, 0, sendByte, 0, id_index.Length);

            sendByte[id_indexlength] = (byte)((filesize >> 8) & 0xFF);
            sendByte[id_indexlength + 1] = (byte)(filesize & 0xFF);
            Array.Copy(getCheckSum(), 0, sendByte, id_indexlength + 2,4);
            Array.Copy(hfModelInfor.TimeVersion, 0, sendByte, id_indexlength + 6, 4);

            time = Environment.TickCount;
            sendData((byte)(getPackByte()+1), sendByte);
        }

        private byte[] getCheckSum() {
            UInt32 filesize = hfModelInfor.FileLength;
            UInt32 cs = 0;
            //Console.Write("========\r\n");
            for (int j = 0; j < filesize; j++) 
            {
                cs += hfModelInfor.G_bin[j];
            //    Console.Write(hfModelInfor.G_bin[j].ToString("X2")+" ");
            }
            //Console.Write("========\r\n");
            byte[] checkSum = new byte[4];

            checkSum[0] = (byte)(cs >> 24);
            checkSum[1] = (byte)((cs >> 16) & 0xFF);
            checkSum[2] = (byte)((cs >> 8) & 0xFF);
            checkSum[3] = (byte)(cs & 0xFF);

            return checkSum;
        }

        public void backCheckBinData(byte[] ID, byte status) { }        
        
        public DrivaceTypeAll getType()
        {
            if (hfModelInfor == null ) return DrivaceTypeAll.CARD;
            return hfModelInfor.mainDrivaceType;
        }

        public byte sunDeviceType()
        {
            if (hfModelInfor == null) return 0xff;
            return hfModelInfor.TypeByte[0];
        }

        public void updataResult(byte[] ID, byte status)
        {
            isCheck = false;
            isUpdata = false;
            if (mFileUpDataInterface != null) mFileUpDataInterface.updataResult(ID, status);
        }

        private byte[] refeData;

        public override void reveData(byte[] buf,string ipInfo) {

            if (isClose) return;

            if (buf[1] != 0x5a || buf.Length < 10) return;
            byte[] recvID = new byte[2];
            Array.Copy(buf, 2, recvID, 0, 2);
           // if (udpCanKaoDianUpInfo == null) return;
            byte[] recvData = new byte[buf.Length - 6];
            Array.Copy(buf, 4, recvData, 0, recvData.Length);
            if (!isCheckPacket(buf)) return;
            if (recvData[0] != 0xfa || recvData[recvData.Length - 1] != 0xf9) return;

            byte[] ID = new byte[2];
            Array.Copy(recvData, 2, ID, 0, 2);
           
            sendCount = 0;
            switch (recvData[1])
            {
                case 3:
                    if (drType == DrivaceTypeAll.NODE) backUpdataData(buf, ID);
                    break;
                case 0x43:
                    //if (drType == DrivaceType.CANKAODIAN) 
                    backUpdataData(recvData, ID);
                    break;
                case 4:
                    if (drType == DrivaceTypeAll.NODE) backCheckUpdata(buf[4]);          
                    break;
                case 0x44:
                    //if (drType == DrivaceType.CANKAODIAN)
                    backCheckUpdata(recvData[0]);          
                    break;                
                case 0x83:
                    if (drType != DrivaceTypeAll.CARD) break;
                    byte[] AddrTag = new byte[2];
                    Array.Copy(buf, 3, AddrTag, 0, AddrTag.Length);
                    if (mFileUpDataInterface != null) mFileUpDataInterface.backBinData(this.ID, AddrTag);
                    break;
                case 0x84:
                    if (drType != DrivaceTypeAll.CARD) break;
                    byte statusTag = buf[3];
                    updataResult(ID, statusTag);
                    isUpdata = false;
                    if (mFileUpDataInterface == null) return;
                    mFileUpDataInterface.backCheckBinData(ID, statusTag);
                    break;
                case 0x85:
                    if (drType != DrivaceTypeAll.CARD) break;
                    if (mFileUpDataInterface != null) mFileUpDataInterface.upDataTag(buf[2]);
                    break;
                case 0x86:
                    if (drType != DrivaceTypeAll.CARD) break;
                    byte[] seAddrTag = new byte[2];
                    byte[] lenTag = new byte[2];
                    Array.Copy(buf, 4, seAddrTag, 0, seAddrTag.Length);
                    Array.Copy(buf, 6, lenTag, 0, lenTag.Length);
                    upDataTag(ID,seAddrTag,lenTag);
                    break;
                case 0x87:
                    if (drType != DrivaceTypeAll.CARD) break;
                    if (mFileUpDataInterface != null) mFileUpDataInterface.clearTag();
                    break;
                case 0xe2:
                    //if (drType != DrivaceType.USB_DANGLE) break;
                    byte[] AddrUSB_dangle = new byte[2];
                    Array.Copy(buf, 2, AddrUSB_dangle, 0, AddrUSB_dangle.Length);
                    if (mFileUpDataInterface == null) break;                    
                    mFileUpDataInterface.backBinData(this.ID, AddrUSB_dangle);
                    sendBinData(DrivaceTypeAll.USB_DANGLE, new byte[] { 0, 0 }, AddrUSB_dangle);
                    break;
                default:
                    break;
            }
        }

        private void backUpdataData(byte[] buf,byte[] id) {

            byte[] Addr = new byte[2];
            Array.Copy(buf, 4, Addr, 0, Addr.Length);
            if (mFileUpDataInterface != null) mFileUpDataInterface.backBinData(id, Addr);
        }


        private void backCheckUpdata(byte status)
        {
            updataResult(ID, status);
            if (mFileUpDataInterface == null) return;
            mFileUpDataInterface.backCheckBinData(ID, status);       
        }

        public override void close()
        {
            isCheck = false;
            isUpdata = false;
            isClose = true;
            mFileUpDataInterface = null;
        }

        public void upDataTag(byte Enable)
        {
            byte[] bt = new byte[1] { Enable };
            sendData(0x85, bt);
        }

        public void upDataTag(byte[] ID, byte[] Addr, byte[] len)
        {
            if (mFileUpDataInterface != null) mFileUpDataInterface.upDataTag(ID,Addr,len);
        }

        public void clearTag()
        {          
            sendData(0x87, new byte[0]);
        }

        /// <summary>
        /// USB_dANGLE通知更新
        /// </summary>
        /// <param name="len"></param>
        /// <param name="CheckSum"></param>
        public void askUSB_dangleUpData()
        {
            if (hfModelInfor == null) return;
            if (hfModelInfor.mainDrivaceType != DrivaceTypeAll.USB_DANGLE) return;

            byte[] sendBt = new byte[6];
            sendBt[0] = (byte)((hfModelInfor.FileLength >> 8) & 0xFF);
            sendBt[1] = (byte)(hfModelInfor.FileLength & 0xFF);
            Array.Copy(getCheckSum(), 0, sendBt, 2, 4);
            sendData(0xe1, sendBt);
        }

        public void stop()
        {
            stopUpData = true;
            isUpdata = false;
        }

        public void start()
        {
            stopUpData = false;
            isUpdata = true;
        }

        public void start(byte Index)
        {
            start();
            this.index[0] = Index;
        }

        /// <summary>
        /// 每个对象的地址应该是唯一的，故地址作为标志位
        /// </summary>
        /// <returns></returns>
        public override string TAG()
        {
            return "FileUpdataModel";
        }
    }
}
