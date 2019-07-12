using CiXinLocation.bean;
using CiXinLocation.CiXinInterface;
using CiXinLocation.Utils;
using MoveableListLib.Bean;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace CiXinLocation.Model
{
    /// <summary>
    /// 通过tcpclien传输文件
    /// 通过tcpclien同步一些数据
    /// </summary>
    public class FileTcpClienModel
    {
        private static FileTcpClienModel fileTcpClenModel;
        public delegate void TCPFileReturnHandle(string msg);
        public TCPFileReturnHandle tcpHandle;
        public delegate void CardUpDataHandle(List<CardUpDataBean> dealUpCardDatas);
        public CardUpDataHandle cardHandle;

        private  FileTcpClienModel() { }
        private static object obk = new object();
        //创建 1个客户端套接字 和1个负责监听服务端请求的线程  
        Thread threadclient = null;
        Socket socketclient = null;
        private bool isOpenTCPClien = false;
        private bool isCloseForm = false;
        private string path;
        private byte tongbuTp = 0; // 同步結果的類型

        public static FileTcpClienModel getFileTcpClienMidel() 
        {
            if (fileTcpClenModel != null) return fileTcpClenModel;
            
            lock(obk)
            {
                if (fileTcpClenModel == null)
                {
                    fileTcpClenModel = new FileTcpClienModel();
                }
            }           
            return fileTcpClenModel;
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        private void returnMessage(string msg) 
        {
            if (tcpHandle != null) tcpHandle(msg);
        }

        private void cardHandleMessage(List<CardUpDataBean> dealUpCardDatas) 
        {
            if (cardHandle != null) cardHandle(dealUpCardDatas);
        }

        /// <summary>
        /// 开始同步数据
        /// </summary>
        public void startTongbuData()
        {
            createSocketclient();
            new Thread(tongBU).Start();
        }

        /// <summary>
        /// 同步資訊
        /// </summary>
        private void tongBU()
        {
            sendConjiWarnEleData();
            Thread.Sleep(200);
            sendConjiWarnCardData();
            Thread.Sleep(200);
            sendConjiWarnNODEData();
            Thread.Sleep(200);
            sendConjiTongBu();               
        }

        public void startFile() 
        {
            createSocketclient();            
            if (!isOpenTCPClien) return;
            byte[] buf = getCountBtAddHandle(getTCPServerType(0x00, 0x07));
            tongbuTp = 0;
            new Thread(tongbuThread).Start();
            sendData(buf,0,buf.Length);
        }

        private void createSocketclient() 
        {
            if (socketclient != null && isOpenTCPClien) return;
            else if (socketclient != null && !isOpenTCPClien)
                socketclient.Close();

            //socketclient.Close();
            //socketclient = null;
            //isOpenTCPClien = false;

            string locaIp = FileModel.getFlModel().ChFlBean.ServerIP_TCP;
            if (locaIp == null) locaIp = XWUtils.GetAddressIP();
            int poet = FileModel.getFlModel().ChFlBean.ServerPort_TCP;
            poet = poet == 0 ? 51234 : poet;
            string retuMsg = open(poet, locaIp);
        }

        /// <summary>
        /// 串口开闭状态，开=true,关=false
        /// </summary>
        /// <returns></returns>
        public bool comType() 
        {
            return isOpenTCPClien;
        }

        private void tongbuThread() 
        {
            for (int i = 0; i < 20;i++ )
            {
                Thread.Sleep(100);
                if (tongbuTp != 0) break;
            }
            Thread.Sleep(2000);
            string msg = tongbuTp != 0 ? "" : "同步失敗";
            returnMessage(msg);
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
            if (isOpenTCPClien) return "%!%"; 
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
                Debug.WriteLine("连接失败\r\n");
                return e.Message;
            }            
            threadclient = new Thread(recv);
            threadclient.Start();
            isOpenTCPClien = true;
            return "%!%"; 
        }

        private void recv() 
        {
            while (!isCloseForm)
            {
                try
                {
                    byte[] buf = getDataBt();
                    if (buf == null) break;
                    dealChaiBao(buf);                    
                    //将套接字获取到的字符数组转换为人可以看懂的字符串  
                    //string strRevMsg = Encoding.UTF8.GetString(arrRecvmsg, 0, length);
                }
                catch (NullReferenceException ex)
                {
                    closeTcpClien(ex.ToString());
                    break;
                }
                catch (SocketException ex)
                {
                    closeTcpClien(ex.ToString());
                    break;
                }
                catch (Exception ex)
                {
                    closeTcpClien(ex.ToString());
                    break;
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
            while (packLength > index && !isCloseForm)
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
            if (packType > 2 && packType < 7) //3,4,5,6处理的事情
            {
                dealTCPClien_loca(packType.ToString(), buf);
                return;
            }
            bool tongbuResult = isSaveDataInFile(buf, packType);
            //数据接收完成后，进行存储，放入到内存中
            if (packType == 0x07) 
            {
                //FileModel.getFlModel().getData();  //存储对象
                FileModel.getFlModel().getdata(FileModel.COUNTADD);//放入到内存中
                byte[] bufD = getCountBtAddHandle(getTCPServerType(0x00, 0x08));//要接收cacheData.dat文件数据的命令
                sendData(bufD, 0, bufD.Length);
                if (tongbuResult) returnMessage("同步data.dat成功");
                else returnMessage("同步data.dat失敗");
                tongbuTp = 1;
            }
            else 
            {
             //   FileModel.getFlModel().getCFData(); //存储对象
                socketclient.Close();               //要的数据都要完了，是时候关闭了
                isOpenTCPClien = false;
                if (tongbuResult) returnMessage("所有文件同步完成");
                else returnMessage("同步cacheData.dat失敗");
                tongbuTp = 2;
            }
            //reDate(buf, packType.ToString(), CommunicationMode.TCPClien_loca);
        }

        /// <summary>
        /// 确认一下数据是否是文件数据
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="packType"></param>
        /// <returns></returns>
        private bool isSaveDataInFile(byte[] buf, int packType) 
        {
            if (packType < 0x07 || packType > 0x08) return false;
            object obj = null;
            fileDeserialize(ref obj, buf);
            if (null == obj) return false;

            if (packType == 0x07)
            {
                path = "data.dat";
                List<CenJiBean> cenJiData = obj as List<CenJiBean>;
                if (cenJiData == null) return false;
                FileModel.getFlModel().changeData(cenJiData);
                FileModel.getFlModel().setData();
            }
            else if (packType == 0x08) 
            {
                path = "cacheData.dat";
                CacheFileBean chFlBean = obj as CacheFileBean;
                if (chFlBean == null) return false;
                FileModel.getFlModel().changeCacheData(chFlBean);
                FileModel.getFlModel().setCFData();
            }
            return true;
        }

        private void fileDeserialize(ref object obj, byte[] buf) 
        {
            Stream fs = null;
            BinaryFormatter bf = null;
            try
            {
                fs = new MemoryStream(buf);
                bf = new BinaryFormatter();
                obj = bf.Deserialize(fs);
            }
            catch { }
            finally 
            {
                if (null != fs) 
                {
                    fs.Close();
                    fs = null;
                }                 
            }                      
        }

        /// <summary>
        /// 处理，来自TCPClien_loca的消息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="buf"></param>
        private void dealTCPClien_loca(string type, byte[] buf)
        {  
            if ("3".Equals(type)) dealDataToCongjiLoca(buf);
            else if ("4".Equals(type)) dealWarnEleDataToCongji(buf);
            else if ("5".Equals(type)) dealWarnCardDataToCongji(buf);
            else if ("6".Equals(type)) dealWarnNODEDataToCongji(buf);
            WarnMessage.getWarnMessage().warnMsgCallBack();
        //    sendConjiReveLocaData();
        }

        /// <summary>
        /// 处理一下同步到的定位数据
        /// </summary>
        private void dealDataToCongjiLoca(byte[] buf)
        {
            if (buf.Length < 4) return;
            Object obj = null;
            XWUtils.DeserializeObject(buf, ref obj);
            if (obj == null || !(obj is List<CardUpDataBean>)) return;
            List<CardUpDataBean> dealUpCardDatas = (List<CardUpDataBean>)obj;
            new Thread(dealDataToCongjiLocaThread).Start(dealUpCardDatas);            
        }

        private void dealDataToCongjiLocaThread(object obj) 
        {
            List<CardUpDataBean> dealUpCardDatas = obj as List<CardUpDataBean>;
            cardHandleMessage(dealUpCardDatas);
        }

        /// <summary>
        /// 处理一下同步到的电量异常数据
        /// </summary>
        private void dealWarnEleDataToCongji(byte[] buf)
        {
            if (buf.Length < 4) return;
            Object obj = null;
            XWUtils.DeserializeObject(buf, ref obj);
            if (obj == null || !(obj is Dictionary<string, DrivaceWarnMessage>)) return;
            WarnMessage.getWarnMessage().CardLowEleWarnMsgs = (Dictionary<string, DrivaceWarnMessage>)obj;
        }

        /// <summary>
        /// 处理一下同步到的卡片异常数据
        /// </summary>
        private void dealWarnCardDataToCongji(byte[] buf)
        {
            if (buf.Length < 4) return;
            Object obj = null;
            XWUtils.DeserializeObject(buf, ref obj);
            if (obj == null || !(obj is Dictionary<string, DrivaceWarnMessage>)) return;
            WarnMessage.getWarnMessage().CardUnanswerWranMsgs = (Dictionary<string, DrivaceWarnMessage>)obj;
        }

        /// <summary>
        /// 处理一下同步到的节点异常数据
        /// </summary>
        private void dealWarnNODEDataToCongji(byte[] buf)
        {
            if (buf.Length < 4) return;
            Object obj = null;
            XWUtils.DeserializeObject(buf, ref obj);
            if (obj == null || !(obj is Dictionary<string, DrivaceWarnMessage>)) return;
            WarnMessage.getWarnMessage().NODEUnanswerWranMsgs = (Dictionary<string, DrivaceWarnMessage>)obj;
        }

        private void closeTcpClien(string msg) 
        {           
            if (socketclient != null) socketclient.Close();
            socketclient = null;
            isOpenTCPClien = false;
            Debug.Write("TCPClien.recvException" + msg);
        //    FileModel.getFlModel().setErrorData("FileTcpClienModel.recvException" + msg);
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
                return null;
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
            isCloseForm = true;
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
            if (!isOpenTCPClien) return;
            if (socketclient != null && buf != null) 
            {
                try 
                {
                    socketclient.Send(buf, index, Length, SocketFlags.None);
                }
                catch(Exception e)
                {
                    closeTcpClien(e.ToString());
                }
                
            } 
            //将发送的信息追加到聊天内容文本框中     
        }

        /// <summary>
        /// TCPServer相关的一些协议
        /// </summary>
        /// <param name="bt1"></param>
        /// <param name="bt2">,</param>
        /// <returns></returns>
        private byte[] getTCPServerType(byte bt1, byte bt2)
        {
            byte[] value = new byte[8] { 0xff, 0xfd, 0xff, bt1, bt2, 0xfd, 0xff, 0xfd };
            return value;
        }

        /// <summary>
        /// 类关闭调用
        /// </summary>
        public void classClose() 
        {
            isCloseForm = true;
            isOpenTCPClien = false;
            if (socketclient != null) socketclient.Close();
        }

        /// <summary>
        /// 发送同步命令
        /// </summary>
        public void sendConjiTongBu()
        {
            if (PeoplePowerModel.getPeoplePowerModel().IsConnect)
                sendDataToZhuju(getCountTongBuBt(), "255.255.255.254");
        }

        /// <summary>
        /// 發送從機同步接收定位数据命令
        /// </summary>
        //public void sendConjiReveLocaData()
        //{
        //    sendDataToZhuju(getTCPServerType(0x00, 0x03), "255.255.255.255");
        //}

        /// <summary>
        /// 發送從機同步接收电量异常信息命令
        /// </summary>
        public void sendConjiWarnEleData()
        {
            sendDataToZhuju(getTCPServerType(0x00, 0x04), "255.255.255.254");
        }

        /// <summary>
        /// 发送同步接收卡片异常信息命令
        /// </summary>
        public void sendConjiWarnCardData()
        {
            sendDataToZhuju(getTCPServerType(0x00, 0x05), "255.255.255.254");
        }

        /// <summary>
        /// 发送节点异常信息命令
        /// </summary>
        public void sendConjiWarnNODEData()
        {
            sendDataToZhuju(getTCPServerType(0x00, 0x06), "255.255.255.254");
        }

        /// <summary>
        /// 发送数据到主机
        /// </summary>
        /// <param name="type">0是發送從機身份驗證，1是主机数据同步命令</param>
        private void sendDataToZhuju(byte[] buf, string msg)
        {
            createSocketclient();
            if (!isOpenTCPClien) return;
            byte[] bufs = getCountBtAddHandle(buf);
            if (bufs != null)
            {
                sendData(bufs, 0, bufs.Length);
            }
        }

        /// <summary>
        /// 加上数据包头getCountBt() ；
        /// </summary>
        /// <returns></returns>
        private byte[] getCountBtAddHandle(byte[] bufs)
        {
            if (bufs == null) return new byte[0];

            byte[] buf = new byte[bufs.Length + 4];
            Array.Copy(bufs, 0, buf, 4, bufs.Length);
            buf[0] = (byte)((bufs.Length / 0x1000000) % 0x100);
            buf[1] = (byte)((bufs.Length / 0x10000) % 0x100);
            buf[2] = (byte)((bufs.Length / 0x100) % 0x100);
            buf[3] = (byte)(bufs.Length % 0x100);
            return buf;
        }

        /// <summary>
        /// 获取主机同步命令
        /// </summary>
        /// <returns></returns>
        private byte[] getCountTongBuBt()
        {
            byte[] errValue = getTCPServerType(0x00, 0x02);
            return errValue;
        }

        /// <summary>
        /// 获取账号字节码或者关闭连接字节码
        /// </summary>
        /// <returns></returns>
        private byte[] getCountBt()
        {
            if (PeoplePowerModel.getPeoplePowerModel().Jurisdiction != PeoplePowerModel.getPeoplePowerModel().CongjiValue)
            {
                return null;
            }
            if (PeoplePowerModel.getPeoplePowerModel().IsConnect)  //关闭
            {
                byte[] errValue = new byte[8] { 0xff, 0xfd, 0xff, 0x00, 0x00, 0xfd, 0xff, 0xfd };
                return errValue;
            }
            string count = PeoplePowerModel.getPeoplePowerModel().Count;
            string power = PeoplePowerModel.getPeoplePowerModel().Password;
            byte[] countBt = Encoding.UTF8.GetBytes(count); //Encoding.UTF8.GetString(arrRecvmsg, 0, length);
            byte[] powerBt = Encoding.UTF8.GetBytes(power);
            byte[] bufs = new byte[6 + countBt.Length + powerBt.Length];// { 0xff, 0xfd, 0xff, 0x00, 0, 0, 0, 0xfd, 0xff, 0xfd };//发一个身份验证
            bufs[0] = 0xff;
            bufs[1] = 0xfd;
            bufs[2] = 0xff;
            Array.Copy(countBt, 0, bufs, 3, countBt.Length);
            Array.Copy(powerBt, 0, bufs, 3 + countBt.Length, powerBt.Length);
            bufs[bufs.Length - 3] = 0xfd;
            bufs[bufs.Length - 2] = 0xff;
            bufs[bufs.Length - 1] = 0xfd;
            return bufs;
        }

    }
}
