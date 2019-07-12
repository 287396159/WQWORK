using CiXinLocation.bean;
using CiXinLocation.CiXinInterface;
using CiXinLocation.Model;
using CiXinLocation.Utils;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace 亚东定位{

    class UDPUtils : ComDataInterface
    {
        private Socket client;
        private string serviseIP = "";
        private int servicePort = 0;
        private EndPoint point = new IPEndPoint(IPAddress.Parse("192.168.1.50"), 5000);
        private EndPoint zhuanfaPoint = new IPEndPoint(IPAddress.Parse("192.168.1.50"), 53142);
        private IPEndPoint servicePoint;
        private ComReadDataInterface crdInter;
        private Thread t2;

        Dictionary<string, EndPoint> dict = new Dictionary<string, EndPoint>();
        Dictionary<string, string> tagID_IP = new Dictionary<string, string>();
        byte[] buffer = new byte[1024 * 1024];
        private int index_re = 0;

        public delegate void UDPReceiveHandle(byte[] buf, string ipInfo);
        public UDPReceiveHandle reveData;
        private bool isOpenUDP = false;

        private void reDate(byte[] buf,string ipInfo){
            if (reveData != null && isOpenUDP){
                reveData(buf,ipInfo);
            }
            if (crdInter != null)
            {
                crdInter.revePortsData(buf, ipInfo);
            }
            try 
            {
                client.SendTo(buf, zhuanfaPoint);
            }
            catch { }            
        }

        public UDPUtils(){                         
            clientInfo();
        }

        public UDPUtils(ComReadDataInterface crdInter): base()
        {
            this.crdInter = crdInter;
        }

        public string open(int servicePort,string serviseIP)
        {
            this.serviseIP = serviseIP;
            this.servicePort = servicePort;
            string msg = clientInfo();
            if (isOpenUDP) 
            {
                t2 = new Thread(ReciveMsg);
                t2.Start();
            }            
            return msg;           
        }

        public string open()
        { //串口打开
            return "";
        }

        public string open(string baudRate, string portName)
        { //串口打开
            return "";
        }

        public void close()
        {
            isOpenUDP = false;
            if (dict != null) dict.Clear();
            if (tagID_IP != null) tagID_IP.Clear();
            if (client != null) client.Close();
            if (t2 != null) t2.Abort();
        }

        public bool comType()
        {
            return isOpenUDP;
        }

        private string clientInfo()
        {         
            try 
            {
                if (!isOpenUDP) client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                servicePoint = new IPEndPoint(IPAddress.Parse(serviseIP), servicePort);
                int zhuanFaPort = servicePort >= 65535 ? 5321:servicePort + 1;
                zhuanfaPoint = new IPEndPoint(IPAddress.Parse(serviseIP), zhuanFaPort);
                client.Bind(servicePoint);                                                  
            }
            catch (Exception e)
            {
                isOpenUDP = false;
                client = null;
                return e.Message;//
            }                           
             isOpenUDP = true;
             return "%!%";   //%!%代表打開成功
        }      

        /// <summary>
        /// 向特定ip的主机的端口发送数据报
        /// </summary>
        public void sendMesg(string msg)
        {
            sendMesg(Encoding.UTF8.GetBytes(msg));
        }

        public void sendMesg(byte[] buf){
            if (!isOpenUDP) return;
            if (buf[0] == 0xff && buf[1] == 0xff) 
            {
                string id = buf[2].ToString("X2")+buf[3].ToString("X2");

                byte[] keybt = new byte[buf.Length - 4];
                Array.Copy(buf, 4, keybt, 0, buf.Length - 4);
                string key = Encoding.UTF8.GetString(keybt);

                if (tagID_IP.ContainsKey(id))
                {
                    if (!tagID_IP[id].Equals(key))
                        tagID_IP[id] = key;
                }
                else tagID_IP.Add(id, key);
                return;
            }  
            try {
                string id = buf[2].ToString("X2") + buf[3].ToString("X2");
                if (tagID_IP.ContainsKey(id))
                {
                    string keyEndPoint = tagID_IP[id];
                    if (dict.ContainsKey(keyEndPoint)) client.SendTo(buf, dict[keyEndPoint]);//client.SendTo(buf, point);
                    else sendNoPoint(buf);
                }
                else sendNoPoint(buf);
            }catch{ }            
        }


        private void sendNoPoint(byte[] buf) 
        {
            if (dict.Count > 0)
            {
                foreach (var item in dict)
                {
                    client.SendTo(buf, item.Value);//client.SendTo(buf, point);
                    return;
                }
            }
        }


        public void setClientPoint(string ip, int port) {
            try {
                point = new IPEndPoint(IPAddress.Parse(ip), port);   
            }
            catch { }            
        }


        public void sendData(byte[] buf, int index, int length)
        {
            sendData(buf, index, length,null);
        }

        public void sendData(byte[] buf, int index, int length,string ipInfo)
        {
            if (buf.Length < index + length) return;
            sendMesg(subByteArr(buf, index, length));
        }

        public  byte[] subByteArr(byte[] buf, int index, int length){
            if (buf.Length < index + length) return buf;
            byte[] subBuf = new byte[length];
            try 
            {
                Array.Copy(buf, index, subBuf, 0, length);
            }
            catch { }           
            return subBuf;
        }
        
        /// <summary>
        /// 接收发送给本机ip对应端口号的数据报
        /// </summary>
        private void ReciveMsg()
        {
            if (UDPDATA_IPs == null) UDPDATA_IPs = new List<UDPDATA_IP>();
            while (true && isOpenUDP)
            {
                if (UDPDATA_IPs == null) UDPDATA_IPs = new List<UDPDATA_IP>();
                try
                {
                    EndPoint receivePoint = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号              
                    byte[] buf = new byte[1024*10];
                    int length = client.ReceiveFrom(buf, ref receivePoint);//接收数据报      
                    string ipClient = receivePoint.ToString();
                    try
                    {
                        if (!dict.ContainsKey(ipClient))
                        {
                            dict.Add(ipClient, receivePoint);
                        }
                    }
                    catch { }
                    reDate(subByteArr(buf, 0, length), ipClient);
                }
                catch (Exception e){
                    Debug.WriteLine("UDPUtils.ReciveMsg" + e.Message);
                //    FileModel.getFlModel().setErrorData("<<<<UDPUtils.ReciveMsg" + e.ToString()+">>>>\r\n");
                }
            }
        }

        private long timeReve = 0;
        private List<UDPDATA_IP> UDPDATA_IPs;
        private void reciveDataDeal(byte[] buf, int length, string ipClient) //处理一下，接收数据之后，缓存一下隔一段时间处理
        {
            Array.Copy(buf, 0, buffer, index_re, length);            
            index_re += length;
            
            UDPDATA_IP udpData = new UDPDATA_IP();
            udpData.Length = length;
            udpData.Ip = ipClient;
            UDPDATA_IPs.Add(udpData);
            if (timeReve + 500 < XwDataUtils.GetLongTimeStamp() || index_re > buffer.Length) 
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(dealDataThread), UDPDATA_IPs.ToList()); //参数可选
                timeReve = XwDataUtils.GetLongTimeStamp();
            }                                         
        }

        private object objReveUDP = new object();
        private void dealDataThread(object obj) 
        {
            if (!(obj is List<UDPDATA_IP>) || obj == null) return;
            byte[] buff = null;
            try {
                if (index_re > buffer.Length) index_re = buffer.Length;
                buff = new byte[index_re + 1];
                Array.Copy(buffer, 0, buff, 0, index_re);
                Array.Clear(buffer, 0, buffer.Length);
                index_re = 0;
            }
            catch { }                           
            if (buff == null) return;
            List<UDPDATA_IP> UDPDATAips = (List<UDPDATA_IP>)obj;
            int index_udp = 0;
            foreach (var item in UDPDATAips)
            {
                int length = item.Length;
                string ipClient = item.Ip;

                if (length != 16)
                    reDate(subByteArr(buff, index_udp, length), ipClient);
                else reDate(subByteArrFour(buff, index_udp, length, ipClient), ipClient);
                index_udp += length;
                if (index_udp > buff.Length) return;
            }              
        }

        /// <summary>
        /// 项目专案，独特方法，为了对付专属协议，实时转发数据
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private byte[] subByteArrFour(byte[] buf, int index, int length, string key)
        {
            if (buf.Length < index + length) return buf;
            byte[] keyInfo = Encoding.UTF8.GetBytes(key);//crBean.Name

            byte[] subBuf = new byte[length + 5 + keyInfo.Length];
            subBuf[0] = 0xfe;
            subBuf[1] = 0xff;
            subBuf[2] = 0xff;
            try
            {
                Array.Copy(buf, index, subBuf, 3, length);
                Array.Copy(keyInfo, 0, subBuf, length + 3, keyInfo.Length);
                subBuf[subBuf.Length - 2] = XWUtils.getCheckBit(subBuf, 0, subBuf.Length - 2);
                subBuf[subBuf.Length - 1] = 0xfd;
            }
            catch { }
            return subBuf;
        }

        public void sendMesg(byte[] buf, TCPSendType tcpSend)
        {
        
        }

        public CommunicationMode getCommunicationMode() 
        {
            return CommunicationMode.UDP;
        }

        public void classClose()
        {
            close();
        }

    }
}
