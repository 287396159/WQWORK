using CiXinLocation.bean;
using CiXinLocation.CiXinInterface;
using CiXinLocation.Utils;
using System;
using System.Collections.Generic;
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
        private IPEndPoint servicePoint;
        private ComReadDataInterface crdInter;
        private Thread t2;

        public delegate void UDPReceiveHandle(SendDataType sType);

        public UDPReceiveHandle reveData;

        private bool isOpenUDP = false;
        private void reDate(SendDataType sType)
        {
            if (!sType.isNoDt) return;
            if (reveData != null && isOpenUDP){
                reveData(sType);
            }
            if (crdInter != null)
            {
                crdInter.revePortsData(sType);
            }
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
            t2 = new Thread(ReciveMsg);
            t2.Start();    
            return clientInfo();           
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
            if (client != null) client.Close();
            if (t2 != null) t2.Abort();
        }

        public bool comType()
        {
            return isOpenUDP;
        }

        private string clientInfo(){         
            try {
                if (!isOpenUDP) client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                servicePoint = new IPEndPoint(IPAddress.Parse(serviseIP), servicePort);
                client.Bind(servicePoint);                                                  
            }
            catch (Exception e){
                isOpenUDP = false;
                client = null;
                return e.Message;//
            }                           
             isOpenUDP = true;
             return "打開成功";   
        }
      

        /// <summary>
        /// 向特定ip的主机的端口发送数据报
        /// </summary>
        public void sendMesg(string msg){
            sendMesg(Encoding.UTF8.GetBytes(msg));
        }


        public void sendMesg(byte[] buf){
            if (!isOpenUDP) return;
            //setClientPoint(buf);       
            try {
                if (point != null) client.SendTo(buf, point);
            }catch{ }            
        }


        public void setClientPoint(string ip, int port) {
            try {
                point = new IPEndPoint(IPAddress.Parse(ip), port);   
            }
            catch { }            
        }


        public void sendData(byte[] buf, int index, int length, string ip)
        {
            if (buf.Length < index + length) return;
            sendMesg(subByteArr(buf, index, length));
        }

        public void sendData(SendDataType sType)
        {
            
        }


        public  byte[] subByteArr(byte[] buf, int index, int length){
            if (buf.Length < index + length) return buf;
            byte[] subBuf = new byte[length];
            try {
                Array.Copy(buf, index, subBuf, 0, length);
            }
            catch { }           
            return subBuf;
        }


        EndPoint receivePoint;
        /// <summary>
        /// 接收发送给本机ip对应端口号的数据报
        /// </summary>
        private void ReciveMsg(){
            while (true){
                try {
                    if (receivePoint == null) receivePoint = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号              
                    byte[] buffer = new byte[1024*10];
                    int length = client.ReceiveFrom(buffer, ref receivePoint);//接收数据报    
                    string ipClient = receivePoint.ToString();

                    SendDataType sendType = new SendDataType(subByteArr(buffer, 0, length), ipClient, CommunicationMode.UDP, SendMode.OUT);
                    reDate(sendType);

                   
                    //reDate(buffer);
                    //setIpClient(ipClient, subByteArr(buffer, 0, length));
                }
                catch (Exception e){
                    string msg = e.Message;
                }               
            }
        }


        public void classClose()
        {
            close();
        }

    }
}
