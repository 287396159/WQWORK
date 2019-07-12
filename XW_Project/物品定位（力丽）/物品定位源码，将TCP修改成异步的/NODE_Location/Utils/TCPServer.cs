using CiXinLocation.bean;
using CiXinLocation.CiXinInterface;
using CiXinLocation.Model;
using MoveableListLib.Bean;
using MyTCPServerUtils.TcpUtils;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using XGCServerSocketAsyncEventArgs.TcpUtils;

namespace CiXinLocation.Utils
{
    class TCPServer : ComDataInterface
    {

        SocketManager m_socket;

        private bool fromClose = false;
        private bool isOpenTCPServer = false;
        //IPAddress ip;
        //IPEndPoint point = new IPEndPoint(IPAddress.Any, 0);
        private ComReadDataInterface crdInter;

        Thread threadWatch = null; // 负责监听客户端连接请求的 线程；
        Socket socketWatch = null;
        Dictionary<string, Socket> dict = new Dictionary<string, Socket>();
        Dictionary<string, Socket> forwardDict = new Dictionary<string, Socket>();//转发从机的地址
        Dictionary<string, Thread> dictThread = new Dictionary<string, Thread>();
        Dictionary<string, byte> lbOnline = new Dictionary<string, byte>();
        Dictionary<string, byte> lbOnlineFour = new Dictionary<string, byte>();//未实时发送数据，建一个新的，以免出问题
        private object objLocak = new object();
         
        public TCPServer(){ }

        public TCPServer(ComReadDataInterface crdInter) 
        {
            this.crdInter = crdInter;
        }

         /// <summary>
        /// 端口号。返回的是失败的信息
        /// </summary>
        public string open() 
        {
            return "";
        }

        /// <summary>
        /// 串口开闭状态，开=true,关=false
        /// </summary>
        /// <returns></returns>
        public bool comType() 
        {
            return isOpenTCPServer;
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
        /// <param name="serialPort">串口号</param>
        public string open(int port, string serialPort) 
        {

            m_socket = new SocketManager(200, 1048576); // 1048576 = 1024*1024
            m_socket.Init();
            ///IPAddress.Any
            IPAddress address = IPAddress.Parse(serialPort.Trim());
            isOpenTCPServer = m_socket.Start(new IPEndPoint(address, port));
            m_socket.ReceiveClientData += ReceiveClientData;
            String msg = "%!%";
            return isOpenTCPServer ? msg : "開啟失敗";

            // 创建负责监听的套接字，注意其中的参数；
            socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // 获得文本框中的IP对象；
            //IPAddress address = IPAddress.Parse(serialPort.Trim());
            // 创建包含ip和端口号的网络节点对象；
            IPEndPoint endPoint = new IPEndPoint(address, port);
            try
            {
                // 将负责监听的套接字绑定到唯一的ip和端口上；
                socketWatch.Bind(endPoint);
                TcpListener tLinster = new TcpListener(endPoint);
            }
            catch (SocketException se)
            {
                //MessageBox.Show("异常：" + se.Message);
                return se.Message.ToString();
            }
            // 设置监听队列的长度；
            socketWatch.Listen(10);
            // 创建负责监听的线程；
            threadWatch = new Thread(WatchConnecting);
            //threadWatch.IsBackground = true;
            threadWatch.Start();
            isOpenTCPServer = true;
           
            return "%!%";
        }


        /// <summary>
        /// 最新異步接受數據
        /// </summary>
        /// <param name="token"></param>
        /// <param name="buff"></param>
        public void ReceiveClientData(AsyncUserToken token, byte[] buffer)
        {
            //string msg = Encoding.UTF8.GetString(buff);
            //Console.Write(token.Remote.ToString() + "发来贺电：" + msg + "\r\n");
            Socket sokClient = token.Socket;
            int length = buffer.Length;
            addLbOnline(sokClient, buffer[1], buffer[4]);
            subcontract(sokClient, buffer, length);
            if (buffer[1] != 0x04) reDate(subByteArr(buffer, 0, length), sokClient.RemoteEndPoint.ToString(), CommunicationMode.TCPServer_OUT);
            else reDate(subByteArrFour(buffer, 0, length, sokClient.RemoteEndPoint.ToString()),
                sokClient.RemoteEndPoint.ToString(), CommunicationMode.TCPServer_OUT);
            reDate(subByteArr(buffer, 0, length), sokClient.RemoteEndPoint.ToString(), CommunicationMode.TCPServer_IP);
        }


        /// <summary>
        /// 监听客户端请求的方法；
        /// </summary>
        void WatchConnecting()
        {
            // 开始监听客户端连接请求，Accept方法会阻断当前的线程；
            while (true && !fromClose)
            {
                try
                {
                    Socket sokConnection = socketWatch.Accept(); // 一旦监听到一个客户端的请求，就返回一个与该客户端通信的 套接字；
                    // 想列表控件中添加客户端的IP信息；
                    //lbOnline.Items.Add(sokConnection.RemoteEndPoint.ToString());
                    // 将与客户端连接的 套接字 对象添加到集合中；
                    dict.Add(sokConnection.RemoteEndPoint.ToString(), sokConnection);

                    //ShowMsg("客户端连接成功！");
                    Thread thr = new Thread(RecMsg);
                    //thr.IsBackground = true;
                    thr.Start(sokConnection);
                    dictThread.Add(sokConnection.RemoteEndPoint.ToString(), thr);  //  将新建的线程 添加 到线程的集合中去。          \
                }
                catch
                {
                    lbOnline.Clear();
                    //dictThread.Clear();
                    forwardDict.Clear();
                    //dict.Clear();
                    lbOnlineFour.Clear();
                    break;
                }
            }            
        }


        void RecMsg(object sokConnectionparn)
        {
            Socket sokClient = sokConnectionparn as Socket;
            while (true && !fromClose)
            {
                // 定义一个2M的缓存区；
                byte[] buffer = new byte[1024  * 2];
                // 将接受到的数据存入到输入  arrMsgRec中；
                int length = -1;
                try
                {
                    length = sokClient.Receive(buffer); // 接收数据，并返回数据的长度；     
                    if (length == 0) 
                    {
                        removeException(sokClient);
                        break;
                    }
                }
                catch (SocketException se)
                {
                    removeException(sokClient);
                    break;
                } 
                catch (Exception e)
                {
                    removeException(sokClient);
                    break;
                }

                addLbOnline(sokClient, buffer[1], buffer[4]);
                subcontract(sokClient, buffer, length);
                if (buffer[1] != 0x04) reDate(subByteArr(buffer, 0, length), sokClient.RemoteEndPoint.ToString(), CommunicationMode.TCPServer_OUT);
                else reDate(subByteArrFour(buffer, 0, length, sokClient.RemoteEndPoint.ToString()),
                    sokClient.RemoteEndPoint.ToString(), CommunicationMode.TCPServer_OUT);
                reDate(subByteArr(buffer, 0, length), sokClient.RemoteEndPoint.ToString(),CommunicationMode.TCPServer_IP);
                /*if (arrMsgRec[0] == 1) // 表示接收到的是文件；
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    if (sfd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {// 在上边的 sfd.ShowDialog（） 的括号里边一定要加上 this 否则就不会弹出 另存为 的对话框，而弹出的是本类的其他窗口，，这个一定要注意！！！【解释：加了this的sfd.ShowDialog(this)，“另存为”窗口的指针才能被SaveFileDialog的对象调用，若不加thisSaveFileDialog 的对象调用的是本类的其他窗口了，当然不弹出“另存为”窗口。】

                        string fileSavePath = sfd.FileName;// 获得文件保存的路径；
                        // 创建文件流，然后根据路径创建文件；
                        using (FileStream fs = new FileStream(fileSavePath, FileMode.Create))
                        {
                            fs.Write(arrMsgRec, 1, length - 1);
                            ShowMsg("文件保存成功：" + fileSavePath);
                        }
                    }
                }*/
            }
        }


        /// <summary>
        /// 分包
        /// </summary>
        private void subcontract(Socket sokClient, byte[] value, int length)
        {                      
            int packIndex = 0;
            while (length > packIndex && !fromClose)
            {
                if (length < packIndex + 3) break;
                int packLength = value[packIndex] * 0x1000000 + value[packIndex + 1] * 0x10000 + value[packIndex+2] * 0x100 + value[packIndex + 3];
                if (packLength <= 0 || packIndex + 4 + packLength > length) break;

                byte[] buf = new byte[packLength + 4];
                Array.Copy(value, packIndex, buf, 0, buf.Length);
                addForwardDict(sokClient, buf, buf.Length);
                packIndex += packLength + 4;
            }
        }


        /// <summary>
        /// 连接Tcp，装发数据命令
        /// </summary>
        private void addForwardDict(Socket sokClient, byte[] value,int length) 
        {            
            if (value[4] == 0xff && value[5] == 0xfd && value[6] == 0xff && 
                value[length - 3] == 0xfd && value[length - 2] == 0xff && value[length - 1] == 0xfd) 
            {
                //returnCongjiLogin(sokClient, value, length);
                loginCongjiCount(sokClient, value, length);
                congjiTongBuData(sokClient, value, length);
            }
            else if (value[4] == 0xff && value[5] == 0xfe && value[6] == 0xff &&
               value[length - 3] == 0xfe && value[length - 2] == 0xff && value[length - 1] == 0xfe) 
            {
                byte[] bufClien = new byte[value.Length - 9];
                bufClien[0] = value[5];
                Array.Copy(value, 7, bufClien, 1, value.Length - 10);//  7 = 4字节包头长+包协议头3字节（ff,fd,ff...  10 = 7+尾6字节（...fd,ff,fd）
                reDate(bufClien, sokClient.RemoteEndPoint.ToString(), CommunicationMode.TCPServer);
            }           
        }


        /// <summary>
        /// 登錄從機賬號 
        /// </summary>
        /// <param name="sokClient"></param>
        /// <param name="value"></param>
        /// <param name="length"></param>
        private void loginCongjiCount(Socket sokClient, byte[] value, int length) 
        {
            //if ((value[7] == 0 && length == 12)&&(value[8] == 0 || value[8] == 2 ||value[8] == 3 ||
            //    value[8] == 4 || value[8] == 5 ||value[8] == 6)) return;
            if ((value[7] == 0 && length == 12)&&(value[8] != 1)) return;

            byte[] namePasword = new byte[length - 10];  //10 = 4字节包头长+包协议头尾6字节（ff,fd,ff,...fd,ff,fd）
            Array.Copy(value, 7, namePasword, 0, namePasword.Length); //7 = 4字节包头长+包协议头3字节（ff,fd,ff）
            string namePasw = Encoding.UTF8.GetString(namePasword);
            List<PeopleBean> peoples = FileModel.getFlModel().ChFlBean.Peoples;
            foreach (PeopleBean ppItem in peoples)
            {
                if (ppItem.Jurisdiction == 3) continue;
                string ppNameWord = ppItem.Name + ppItem.PassWord;
                if (!ppNameWord.Equals(namePasw)) continue;
                if (!forwardDict.ContainsKey(sokClient.RemoteEndPoint.ToString()))
                    m_socket.setOnline(sokClient.RemoteEndPoint.ToString(), true);
                    //forwardDict.Add(sokClient.RemoteEndPoint.ToString(), sokClient);
                sokClient.Send(value, 0, length, SocketFlags.None);                
                return;
            }
            byte[] errValue = new byte[12] { 0x00,0x00,0x00, 0x08, 0xff, 0xfd, 0xff, 0x00, 0x01, 0xfd, 0xff, 0xfd };
            sokClient.Send(errValue, 0, errValue.Length, SocketFlags.None);
        }


        /// <summary>
        /// 从机同步数据
        /// </summary>
        private void congjiTongBuData(Socket sokClient, byte[] value, int length) 
        {
            if (value[7] == 0 &&  length == 12)
            {
                if (value[8] == 0)
                {
                    m_socket.setOnline(sokClient.RemoteEndPoint.ToString(), false);
                    //if (forwardDict.ContainsKey(sokClient.RemoteEndPoint.ToString()))
                    //{                       
                    //    forwardDict.Remove(sokClient.RemoteEndPoint.ToString());
                    //}
                    sokClient.Send(value, 0, value.Length, SocketFlags.None);
                }
                else if (value[8] == 2) 
                {
                    byte[] sendDataLo = new byte[3] { 0xfe, 0x02, 0xfd };
                    reDate(sendDataLo, sokClient.RemoteEndPoint.ToString(), CommunicationMode.TCPServer_loca);
                }
                else if (value[8] == 3)
                {
                    m_socket.setOnline(sokClient.RemoteEndPoint.ToString(), true);
                    //if (!forwardDict.ContainsKey(sokClient.RemoteEndPoint.ToString()))
                    //{
                    //    forwardDict.Add(sokClient.RemoteEndPoint.ToString(), sokClient);
                    //}
                    sokClient.Send(value, 0, length, SocketFlags.None);
                }
                else if (value[8] == 4) 
                {
                    byte[] sendDataLo = new byte[3] { 0xfe, 0x04, 0xfd };
                    reDate(sendDataLo, sokClient.RemoteEndPoint.ToString(), CommunicationMode.TCPServer_loca);
                }
                else if (value[8] == 5)
                {
                    byte[] sendDataLo = new byte[3] { 0xfe, 0x05, 0xfd };
                    reDate(sendDataLo, sokClient.RemoteEndPoint.ToString(), CommunicationMode.TCPServer_loca);
                }
                else if (value[8] == 6)
                {
                    byte[] sendDataLo = new byte[3] { 0xfe, 0x06, 0xfd };
                    reDate(sendDataLo, sokClient.RemoteEndPoint.ToString(), CommunicationMode.TCPServer_loca);
                }
                else if (value[8] == 7)
                {
                    byte[] sendDataLo = new byte[3] { 0xfe, 0x07, 0xfd };
                    reDate(sendDataLo, sokClient.RemoteEndPoint.ToString(), CommunicationMode.TCPServer_File);
                }
                else if (value[8] == 8)
                {
                    byte[] sendDataLo = new byte[3] { 0xfe, 0x08, 0xfd };
                    reDate(sendDataLo, sokClient.RemoteEndPoint.ToString(), CommunicationMode.TCPServer_File);
                } 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sokClient"></param>
        /// <param name="value"></param>
        /// <param name="getType">实时发送数据的一个状态 0 = 实时发送，！0 = 不发送或者停止</param>
        private void addLbOnline(Socket sokClient, byte value, byte getType) 
        {          
            string key = sokClient.RemoteEndPoint.ToString();
            if(value == 0x04)
            {
               /* if (lbOnlineFour.ContainsKey(key))
                    if (getType != 0) lbOnlineFour[key] = value;
                    else lbOnlineFour[key] = 0;
                else 
                {
                    lbOnlineFour.Add(key, value);
                } */
                return;
            }

            lock (objLocak)
            {
                if (lbOnline.ContainsKey(key))
                    lbOnline[key] = value;
                else if (lbOnline.ContainsKey(key))
                {
                    lbOnline[key] = 0;
                }
                else lbOnline.Add(key, value);
            }            
        }


        private void removeException(Socket sokClient) 
        {
            lock (objLocak)
            {
                try
                {                    
                    //ShowMsg("异常：" + e.Message);
                    // 从 通信套接字 集合中删除被中断连接的通信套接字；
                    dict.Remove(sokClient.RemoteEndPoint.ToString());
                }
                catch { }  
            }                     
            try
            {
                // 从通信线程集合中删除被中断连接的通信线程对象；
                dictThread.Remove(sokClient.RemoteEndPoint.ToString());
            }
            catch { }            
            try
            {
                // 从列表中移除被中断的连接IP
                lbOnline.Remove(sokClient.RemoteEndPoint.ToString());
            }
            catch { }
            try
            {
                // 从列表中移除被中断的连接IP
                if (lbOnlineFour != null && lbOnlineFour.ContainsKey(sokClient.RemoteEndPoint.ToString()))
                    lbOnlineFour.Remove(sokClient.RemoteEndPoint.ToString());
            }
            catch { }
            try {
                byte[] fourBt = new byte[7] { 0xfe, 0x04, 0, 0, 1, 0x3, 0xfc };
                reDate(subByteArrFour(fourBt, 0, fourBt.Length, sokClient.RemoteEndPoint.ToString()), sokClient.RemoteEndPoint.ToString());
            }
            catch { }
            try 
            {
                m_socket.setOnline(sokClient.RemoteEndPoint.ToString(), false);
                //forwardDict.Remove(sokClient.RemoteEndPoint.ToString());
            }
            catch { }
        }


        public void close() 
        {
            if (m_socket != null) m_socket.Stop();

            isOpenTCPServer = false;
            Dictionary<string, Socket> dictCache = new Dictionary<string, Socket>(dict);
            foreach (Socket socketItem in dictCache.Values)
            {
                closeSocket(socketItem);
            }
            dict.Clear();
            Dictionary<string, Thread> dictCacheThread = new Dictionary<string, Thread>(dictThread);
            dictThread.Clear();
            foreach (Thread threadItem in dictCacheThread.Values)
            {
                threadClose(threadItem);
            }
            
            closeSocket(socketWatch);
            threadClose(threadWatch);
        }


        private void threadClose(Thread threadItem) 
        {
            try 
            {
                if (threadItem != null)
                {
                    threadItem.Abort();
                }
            }
            catch { }
            
        }

        /// <summary>
        /// 关闭Socket
        /// </summary>
        /// <param name="socketItem"></param>
        private void closeSocket(Socket socketItem)        
        {
            try
            {
                if (socketItem != null) socketItem.Shutdown(SocketShutdown.Both);                                  
            }
            catch (Exception exp)
            {
                Debug.Write("TCPServer.closeSocket.." + exp.ToString());//处理异常
            }
            if (socketItem != null) socketItem.Close();            
        }

        public void sendData(byte[] buf, int index, int length,string ipInfo)
        {
            if (buf == null || fromClose) return;
            if (m_socket != null) m_socket.SendMessage(ipInfo, subByteArr(buf, index, length));

            if (dict.ContainsKey(ipInfo))
            {
                try 
                {
                    dict[ipInfo].Send(subByteArr(buf, index, length)); 
                }
                catch(Exception e)
                {
                    try
                    {
                        //ShowMsg("异常：" + e.Message);
                        // 从 通信套接字 集合中删除被中断连接的通信套接字；
                        dict.Remove(ipInfo);
                    }
                    catch { }  
                }                              
            }else 
                sendData(buf, index, length);        
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        public void sendData(byte[] buf, int index, int length) 
        {
            if (buf.Length < index + length) return;
            lock(objLocak)
            {
                sendMesg(subByteArr(buf, index, length));
            }            
        }

        public byte[] subByteArr(byte[] buf, int index, int length)
        {
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
        /// 项目专案，独特方法，为了对付专属协议，实时转发数据
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private byte[] subByteArrFour(byte[] buf, int index, int length,string key)
        {
            if (buf.Length < index + length) return buf;
            byte[] keyInfo = Encoding.UTF8.GetBytes(key);//crBean.Name

            byte[] subBuf = new byte[length + 4 + keyInfo.Length];                   
            subBuf[0] = 0xfe;
            subBuf[1] = 0x04;
            try
            {
                Array.Copy(buf, index, subBuf, 2, length);
                Array.Copy(keyInfo, 0, subBuf, length + 2, keyInfo.Length);
                subBuf[subBuf.Length - 2] = XWUtils.getCheckBit(subBuf, 0, subBuf.Length - 2);
                subBuf[subBuf.Length - 1] = 0xfc;
            }          
            catch { }
            return subBuf;
        }


        /// <summary>
        /// 向特定ip的主机的端口发送数据报
        /// </summary>
        public void sendMesg(string msg)
        {
            lock (objLocak)
            {
                sendMesg(Encoding.UTF8.GetBytes(msg));
            }           
        }


        public void sendMesg(byte[] buf)
        {
            if (!isOpenTCPServer) return;

            if (buf[1] == 0x04 || buf[1] == 0x14) 
            {
                if (buf[0] != 0xfd) return;
                int length = buf[2];
                if (length >= buf.Length || buf[length] != 0xfd) return;
                byte[] keybt = new byte[length - 3];
                byte[] sendByte = new byte[buf.Length - length];
                Array.Copy(buf, 3, keybt, 0, length - 3);
                Array.Copy(buf, length, sendByte, 0, buf.Length - length);
                string key = Encoding.UTF8.GetString(keybt);

                if (m_socket != null) m_socket.SendMessage(key, sendByte);

                //if (dict.ContainsKey(key))
                //    dict[key].Send(sendByte);    
                return;
            }
            List<string> onLineKeys = new List<string>();
            //3.0以上版本
            foreach (var item in lbOnline)
            {
                if (item.Value != buf[1] && item.Value + 0x10 != buf[1]) continue;

                if (string.IsNullOrEmpty(item.Key)) continue; // 判断是不是选择了发送的对象；
                try 
                {
                    if (m_socket != null) m_socket.SendMessage(item.Key, buf);
                    //dict[item.Key].Send(buf);       // 解决了 sokConnection是局部变量，不能再本函数中引用的问题；
                    if (item.Value != 0x14 && item.Value > 0x10) onLineKeys.Add(item.Key);                        
                }
                catch { }                                                 
            }
            foreach (string key in onLineKeys) lbOnline[key] = 0;                
       }

        public void sendMesg(byte[] buf,TCPSendType tcpSend)
        {
            if (tcpSend == TCPSendType.PUTONG) 
            {
                sendMesg(buf);
            }
            else if (tcpSend == TCPSendType.FORWORD) 
            {
                try 
                {
                    if (m_socket != null) m_socket.SendOnlineMessage(buf);
                    return;

                    Dictionary<string, Socket> forwardDictCache = new Dictionary<string, Socket>(forwardDict); //缓存一个地址出来
                    foreach (var Item in forwardDictCache)
                    {
                        try
                        {
                            Item.Value.Send(buf);
                        }
                        catch (SocketException e)
                        {
                            break;
                        }  
                    }   
                }
                catch(Exception e) 
                {
                    Debug.Write("TCPServer.sendMesg.."+e.Message);
                    FileModel.getFlModel().setErrorData("<<<<TCPServer.sendMesg.." + e.Message + ">>>>\r\n");
                }                            
            }
        }

        /// <summary>
        /// 类关闭调用
        /// </summary>
        public void classClose() 
        {                      
            close();
            fromClose = true;
        }

        private void reDate(byte[] buf,string ipInfo)
        {
            if (crdInter != null)
            {
                crdInter.revePortsData(buf, ipInfo);
            }
        }

        private void reDate(byte[] buf, string ipInfo, CommunicationMode comMode)
        {
            if (crdInter != null)
            {
                crdInter.revePortsData(buf, ipInfo, comMode);
            }
        }

        public CommunicationMode getCommunicationMode()
        {
            return CommunicationMode.TCPServer;
        }
    }


}
