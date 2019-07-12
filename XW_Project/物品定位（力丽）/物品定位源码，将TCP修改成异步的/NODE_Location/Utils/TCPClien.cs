using CiXinLocation.bean;
using CiXinLocation.CiXinInterface;
using CiXinLocation.Model;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CiXinLocation.Utils
{
    class TCPClienUtils : ComDataInterface
    {            
        //创建 1个客户端套接字 和1个负责监听服务端请求的线程  
        Thread threadclient = null;
        Socket socketclient = null;
        private bool isOpenTCPClien = false;
        private bool formClose = false;//界面是否关闭
        private CommunicationMode comm = CommunicationMode.NOCOM;
        private ComReadDataInterface crdInter;//回调数据
        private int port;
        private string serialPort;

        //////////////////////////////////////////////////////////////////////////////////////
        public TCPClienUtils(ComReadDataInterface crdInter)
        {
            this.crdInter = crdInter;
        }

        /// <summary>
        /// 端口号。返回的是失败的信息
        /// </summary>
        public string open() 
        {
            return open(port, serialPort);
        }

        public CommunicationMode getCommunicationMode() 
        {
            return CommunicationMode.TCPClien;
        }

        /// <summary>
        /// 串口开闭状态，开=true,关=false
        /// </summary>
        /// <returns></returns>
        public bool comType() 
        {
            return isOpenTCPClien;
        }

        /// <summary>
        /// 基本参数的设定打开。返回的是失败的信息
        /// </summary>
        /// <param name="port">端口号</param>
        /// <param name="serialPort">串口号，或者IP</param>
        public string open(string port, string serialPort) 
        {
            int pt = XWUtils.stringToInt1(port);
            if (pt != -1)
                return open(pt, serialPort);
            return "請檢查端口";
        }

        /// <summary>
        /// 基本参数的设定打开。返回的是失败的信息
        /// </summary>
        /// <param name="port">波特率</param>
        /// <param name="serialPort">串口号，或者IP</param>
        public string open(int port, string serialPort) 
        {
            //定义一个套接字监听  
            socketclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  
            //获取文本框中的IP地址  
            IPAddress address = IPAddress.Parse(serialPort);  
            //将获取的IP地址和端口号绑定在网络节点上  
            IPEndPoint point = new IPEndPoint(address, port);  
            try
            {
                //客户端套接字连接到网络节点上，用的是Connect  
                socketclient.Connect(point);
            }
            catch (Exception e)
            {
                Debug.WriteLine("连接失败TCPClien\r\n");
                return e.Message;
            }
            this.port = port;
            this.serialPort = serialPort;
            //if (threadclient != null) 
            threadclient = new Thread(recv);
            threadclient.Start();

            isOpenTCPClien = true;
            return "%!%"; 
        }

        private void recv() 
        {
            while (true && !formClose)
            {
                try
                {
                    //定义一个1M的内存缓冲区，用于临时性存储接收到的消息  
                    byte[] arrRecvmsg = new byte[1024 * 1024];
                    //将客户端套接字接收到的数据存入内存缓冲区，并获取长度  
                    int length = socketclient.Receive(arrRecvmsg, SocketFlags.None);
                    if (length == 0)
                    {
                        new Thread(restartBean).Start();                                                                   
                        //    return null;
                        break;
                    }
                    byte[] buf = new byte[length];
                    Array.Copy(arrRecvmsg, 0, buf, 0, length);

                    reDate(buf, null);
                    if (comm != CommunicationMode.UDP)
                    {
                        reDate(buf, null, CommunicationMode.TCPClien);
                    }
                    if (buf != null && comm == CommunicationMode.TCPClien_loca && buf.Length > 4)   //这里面是处理拆包
                    {
                        try 
                        {
                            dealChaiBao(buf);
                        }
                        catch { }                        
                        continue;
                    }                   
                    
                    //将套接字获取到的字符数组转换为人可以看懂的字符串  
                    //string strRevMsg = Encoding.UTF8.GetString(arrRecvmsg, 0, length);
                }
                catch (NullReferenceException ex)
                {
                    FileModel.getFlModel().setErrorData("NullReferenceException");
                    closeTcpClien(ex.ToString());
                    //break;
                }
                catch (SocketException ex)
                {
                    FileModel.getFlModel().setErrorData("SocketException");
                    if (socketclient != null) socketclient.Close();
                    socketclient = null;
                    isOpenTCPClien = false;
                    closeTcpClien(ex.ToString());
                    break;
                }
                catch (Exception ex)
                {
                    FileModel.getFlModel().setErrorData("Exception");
                    closeTcpClien(ex.ToString());
                    //break;
                }
            }
        }

        /// <summary>
        /// 处理拆包
        /// </summary>
        /// <param name="bufs"></param>
        /// <param name="buf"></param>
        private void dealChaiBao(byte[] bufs) 
        {
            int length = bufs.Length;
            if (length < 16) 
            {
                return;
            }           
            long thisPackLength = bufs[0] * 0x1000000 + bufs[1] * 0x10000 + bufs[2] * 0x100 + bufs[3];
            long packLength = 0;
            if (bufs[4] == 0xff && bufs[5] == 0xfd && bufs[6] == 0xff  &&
                bufs[9] == 0xfd && bufs[10] == 0xff && bufs[11] == 0xfd)
            {
                packLength = thisPackLength - 8;
            }
            else return;
            int packType = bufs[8];
            long index = bufs.Length - 12;
            byte[] buf = new byte[packLength];
            if (packLength > index)
                Array.Copy(bufs, 12, buf, 0, index);
            else
                Array.Copy(bufs, 12, buf, 0, buf.Length);
            while (packLength > index && isOpenTCPClien)
            {
                bufs = getDataBt();
                if (bufs.Length + index <= packLength)
                {
                    Array.Copy(bufs, 0, buf, index, bufs.Length);
                    index += bufs.Length;
                }
                else
                {
                    Array.Copy(bufs, 0, buf, index, packLength - index);
                    index = packLength;
                }
            }
            reDate(buf, packType.ToString(), CommunicationMode.TCPClien_loca);
        }

        private void closeTcpClien(string msg)
        {
            PeoplePowerModel.getPeoplePowerModel().IsConnect = false;
            Debug.Write("TCPClien.recvException" + msg);
            FileModel.getFlModel().setErrorData("TCPClien.recvException" + msg);
        }

        /// <summary>
        /// 和主机重连
        /// </summary>
        private void restartBean() 
        {
            isOpenTCPClien = false;
            for (int i = 0; i < 20;i++ )
            {
                Thread.Sleep(2000);
                if (!isOpenTCPClien) open();
                else break;
                if (formClose) return;                
            }
            Debug.Write("连接结果 = " + isOpenTCPClien);
            byte[] bts = new byte[5] { 0xff, 0xff, 0xfe, 0xfd, 0xfc };
            if (!isOpenTCPClien)
            {
                reDate(bts, "closeTcpClien", CommunicationMode.TCPClien);
                closeTcpClien("与主机连接断开");
            } 
            else 
            {
                bts[2] = 0xff;
                reDate(bts, "closeTcpClien", CommunicationMode.TCPClien);
            }           
        }

        private byte[] getDataBt() 
        {
            //定义一个1M的内存缓冲区，用于临时性存储接收到的消息  
            byte[] arrRecvmsg = new byte[1024 * 1024];
            //将客户端套接字接收到的数据存入内存缓冲区，并获取长度  
            int length = socketclient.Receive(arrRecvmsg,SocketFlags.None);   
            if (length == 0)
            {
                byte[] bts = new byte[5] { 0xff, 0xff, 0xfe, 0xfd, 0xfc };
                reDate(bts, "closeTcpClien", CommunicationMode.TCPClien);
                closeTcpClien("与主机连接断开");
            //    return null;
            }
            byte[] buf = new byte[length];
            Array.Copy(arrRecvmsg, 0, buf, 0, length);

            return buf;
        }


        //发送字符信息到服务端的方法  
        public void ClientSendMsg(string sendMsg)
        {
            //将输入的内容字符串转换为机器可以识别的字节数组     
            byte[] arrClientSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            //调用客户端套接字发送字节数组     
            socketclient.Send(arrClientSendMsg);
            //将发送的信息追加到聊天内容文本框中     
        }

        public void close() 
        {
            isOpenTCPClien = false;
            if (socketclient != null) socketclient.Close();
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        public void sendData(byte[] buf, int index, int length)
        {
            sendData(buf, index, length,null);
        }

        public void sendData(byte[] buf, int index, int Length, string ipInfo)
        {
            if (formClose) return;            
            if (socketclient != null && buf != null) 
            {
                editBuf(ref buf, ref index, ref Length, ipInfo);
                if (ipInfo.Equals("255.255.255.255")) comm = CommunicationMode.TCPClien;
                else if (ipInfo.Equals("255.255.255.0")) comm = CommunicationMode.UDP;
                else if (ipInfo.Equals("255.255.255.254")) comm = CommunicationMode.TCPClien_loca;                                 
                try
                {
                    socketclient.Send(buf, index, Length, SocketFlags.None);
                }
                catch (Exception e)
                {
                    FileModel.getFlModel().setErrorData("TCPClien.sendData");
                    closeTcpClien(e.ToString());
                    
                }
            } 
            //将发送的信息追加到聊天内容文本框中     
        }

        private void editBuf(ref byte[] buf, ref int index, ref int Length, string ipInfo) 
        {            
            if ("LEDSENDDATA".Equals(ipInfo)) 
            {
                byte[] cacheBuf = new byte[buf.Length];
                Array.Copy(buf, 0, cacheBuf, 0, buf.Length);
                buf = new byte[cacheBuf.Length + 10];
                buf[0] = (byte)(((buf.Length - 4) / 0x1000000) % 0x100);
                buf[1] = (byte)(((buf.Length - 4) / 0x10000) % 0x100);
                buf[2] = (byte)(((buf.Length - 4) / 0x100) % 0x100);
                buf[3] = (byte)( (buf.Length - 4) % 0x100);
                buf[4] = 0xff;
                buf[5] = 0xfe;
                buf[6] = 0xff;
                Array.Copy(cacheBuf, 0, buf, 7, cacheBuf.Length);
                buf[buf.Length - 3] = 0xfe;
                buf[buf.Length - 2] = 0xff;
                buf[buf.Length - 1] = 0xfe;
                index = 0;
                Length = buf.Length;
            }            
        }

        public void sendMesg(byte[] buf, TCPSendType tcpSend) {}

        /// <summary>
        /// 类关闭调用
        /// </summary>
        public void classClose() 
        {
            isOpenTCPClien = false;
            formClose = true;
            if (socketclient != null) socketclient.Close();
        }

        private void reDate(byte[] buf, string ipInfo)
        {
            if (crdInter != null)
            {
                crdInter.revePortsData(buf, ipInfo);
            }
        }

        private void reDate(byte[] buf, string ipInfo,CommunicationMode comMode)
        {
            if (crdInter != null)
            {
                crdInter.revePortsData(buf, ipInfo,comMode);
            }
        }


    }
}
