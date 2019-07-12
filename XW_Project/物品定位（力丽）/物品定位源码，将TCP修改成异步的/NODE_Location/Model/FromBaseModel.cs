using CiXinLocation.Utils;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CiXinLocation.Model
{
    public class FromBaseModel {

        public delegate void SendDataHandle(byte[] buf,string ipInfo);
        public SendDataHandle sendDataHandle;

        public byte sendByteHandle = 0xf2;  //发送包头
        public byte sendByteend = 0xf1;     //发送包尾
        public byte receVeByteHandle = 0xfc;//接收包头
        public byte receVeByteend = 0xfb;   //接收包尾
        public int locationPack = 16;
        public CommunicationMode comMode;//
        private UInt32 openUDPtime = 0; //打开UDP连接按妞的时间
        private UInt32 closeUDPtime = 0;//关闭UDP连接按妞的时间
        
        public FromBaseModel() { }

        public void sendDataModel(byte[] buf, string ipInfo)
        {
            if (sendDataHandle != null)
                sendDataHandle(buf, ipInfo);                        
        }

        int i_send = 0;
        /// <summary>
        /// 计算校验，并且发送数据
        /// </summary>
        /// <param name="buf"></param>
        public void addCSAndsendData(byte[] buf, string ipInfo)
        { 
            if(buf == null || buf.Length < 2) return;
            buf[buf.Length - 2] = XWUtils.getCheckBit(buf,0,buf.Length -2);
            sendDataModel(buf, ipInfo);       
        }


        /// <summary>
        /// 加上尾部标志，并且发送数据，
        /// 尾部 0 = UDP，尾部 1 = TCPServer , 2 = TCPClien
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="tp">尾部 0 = UDP，尾部 1 = TCPServer</param> 
        public void addEndAndsendData(byte[] buf, byte tp, string ipInfo)
        {
            if (buf == null || buf.Length < 2) return;
            byte[] sendBy = new byte[buf.Length + 1];
            Array.Copy(buf, 0, sendBy, 0, buf.Length);            
            sendBy[sendBy.Length - 1] = tp;
            sendDataModel(sendBy, ipInfo);
        }      

        private int index_buf = 0;
        private byte[] bytesBuf = new byte[28*1024];

        public UInt32 CloseUDPtime
        {
            get { return closeUDPtime; }
            set { closeUDPtime = value; }
        }
        public UInt32 OpenUDPtime
        {
            get { return openUDPtime; }
            set { openUDPtime = value; }
        }        

        /// <summary>
        /// 接收数据开始处
        /// </summary>
        /// <param name="buf"></param>
        public void revePortsData(byte[] buf,string ipInfo) {
            if (buf.Length > 19 && TAG().Equals("FromMainModel") &&PeoplePowerModel.getPeoplePowerModel().Jurisdiction != PeoplePowerModel.getPeoplePowerModel().CongjiValue)
            {
                if (checkPacket(buf, ipInfo)) return;
            //    return;
            }
            else if (buf.Length != locationPack && (buf[0] != 0XFE || buf[buf.Length - 1] != 0xFD)) //为了将TCP数据放进来
            {
                if (checkPacket(buf, ipInfo)) return;
            //    return;
            }
            
            if (buf == null || buf.Length < 1) return;
            //if (index_buf == 0 && buf[0] != receVeByteHandle) return;

            if (buf[0] == receVeByteHandle && buf[buf.Length - 1] == receVeByteend)
            {
                if (buf[buf.Length - 2] == XWUtils.getCheckBit(buf, 0, buf.Length - 2))
                {
                    index_buf = 0;
                    reveData(buf, ipInfo);
                    return;
                }
            } 
            if(index_buf == 0) Array.Clear(bytesBuf, 0, bytesBuf.Length);//先清理，后面才好添加数据
            if (bytesBuf.Length <= buf.Length + 38) {
                index_buf = 0;
                Array.Clear(bytesBuf, 0, bytesBuf.Length);
                if (bytesBuf.Length < buf.Length) return;
            }
            else if (index_buf + buf.Length >= bytesBuf.Length) {
                Array.Copy(bytesBuf, index_buf - 38, bytesBuf, 0, 38);
                index_buf = 38;
                Array.Clear(bytesBuf, 38, bytesBuf.Length - 38);
            }
            Array.Copy(buf, 0, bytesBuf, index_buf, buf.Length);
            findPack(bytesBuf,ipInfo);
                    
            index_buf += buf.Length;
            if (index_buf >= bytesBuf.Length) index_buf = 0;
        }

        /// <summary>
        /// 接收数据开始处
        /// </summary>
        /// <param name="buf"></param>
        public void revePortsData(byte[] buf, string ipInfo, CommunicationMode comMode)
        {
            if (comMode == CommunicationMode.TCPServer_OUT)
                reveTCPServerOutDate(buf, ipInfo, comMode);
            else
                reveData(buf, ipInfo, comMode);            
        }

        /// <summary>
        /// TCPServer数据格式专属协议，专属神器
        /// </summary>
        private void reveTCPServerOutDate(byte[] buf, string ipInfo, CommunicationMode comMode) 
        {
            if (buf[0] == 0XFE && buf[buf.Length - 1] == 0xFC) //为了将TCP数据放进来
            {
                checkPacket(buf, ipInfo);
                return;
            }
        }


        /// <summary>
        /// 将缓存的数据中符合要求的数据包解析出来
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="index"></param>
        private void findPack(byte[] buf, string ipInfo)
        {            
            for (int i = 0; i < buf.Length;i++ ) 
            {
                if (buf[i] != receVeByteHandle) continue; //只要包头
                for (int j = i; j < buf.Length; j++)
                {
                    if (j > i + 64) break;
                    if (buf[j] != receVeByteend) continue;//只要包尾
                    byte[] packData = new byte[j - i + 1];
                    Array.Copy(buf, i, packData, 0, packData.Length);//截取一段包头与包尾                  
                    if (checkPacket(packData, ipInfo)) Array.Clear(buf, i, j - i);
                }
            }         
        }


        //检查校验位
        public bool checkPacket(byte[] checkBuf,string ipInfo)
        {
            if (checkBuf.Length < 2) return false;
            byte check = XWUtils.getCheckBit(checkBuf, 0, checkBuf.Length-2);
            if (checkBuf[checkBuf.Length - 2] == check)
            {
                reveData(checkBuf, ipInfo);
                return true;
            }
            return false;
        }

        //检查校验位
        public bool isCheckPacket(byte[] checkBuf)
        {
            if (checkBuf.Length < 2) return false;
            byte check = XWUtils.getCheckBit(checkBuf, 0, checkBuf.Length - 2);
            if (checkBuf[checkBuf.Length - 2] == check)
            {
                return true;
            }
            return false;
        }


        public virtual void reveData(byte[] buf) {  }
        public virtual void reveData(byte[] buf,string ipInfo) { }
        public virtual void reveData(byte[] buf, string ipInfo,CommunicationMode comMode) { }
        public virtual void close() { }

        /// <summary>
        /// 返回MODEL的标志位
        /// </summary>
        /// <returns></returns>
        public virtual string TAG() { return tag; }
        public void setTag(string tags) 
        {
            tag = tags;
        }
        public string tag = "FromBaseModel";
    }
}
  