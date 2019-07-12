using CiXinLocation.bean;
using CiXinLocation.CiXinInterface;
using CiXinLocation.Utils;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialPortSpace
{
    public class SerialPortUtils : ComDataInterface
    {

        public delegate void DataEvenHander(SendDataType sType); //定义一个委托事件
        public event DataEvenHander onDataReceived;
        private ComReadDataInterface crdInter;

        public SerialPort comm = new SerialPort();
        private bool pointClosing = false;
        private bool Listening = false;

        public long received_count = 0;//接收计数
        public long send_count = 0;//发送计数
        public int intS = 2;

        public SerialPortUtils() { 
            commLoad();
        }

        public SerialPortUtils(ComReadDataInterface crdInter):base()
        {
            this.crdInter = crdInter;
        }

        /// <summary>
        /// 定义委托，输送详细的数据
        /// </summary>
        /// <param name="buf"></param>
        public void issue(SendDataType sType)
        {
            if (!sType.isNoDt) return;
            if (onDataReceived != null){
                onDataReceived(sType);
                
            }
            if (crdInter != null)
            {
                crdInter.revePortsData(sType);
            }
        }


        void commLoad(){
            //初始化SerialPort对象
            comm.NewLine = "\r\n";
            comm.RtsEnable = true;//根据实际情况吧。
            //添加事件注册
            comm.DataReceived += comm_DataReceived;
        }
       

        /// <summary>
        /// 串口关闭或打开状态
        /// </summary>
        /// <returns></returns>
        public bool comType()
        {
            if (comm == null) return false;
            return comm.IsOpen;
        }

        /// <summary>
        /// 关闭端口号
        /// </summary>
        public void close()
        {
            pointClosing = true;
            while (Listening) Application.DoEvents();
            //打开时点击，则关闭串口  
            try {
                comm.DiscardOutBuffer();
            }
            catch { }
            try {
                comm.DiscardInBuffer();
            }
            catch { }
            
            comm.Close();
            pointClosing = false; 
        }       


        /// <summary>
        /// 打开串口 ,失败返回失败消息
        /// </summary>
        /// <param name="portName">端口号</param>
        /// <param name="baudRate">波特率</param>
        /// <returns>失败消息，成功返回 %!%</returns>
        public string open(int baudRate,string portName)
        { //串口打开
            string msg = "%!%";            
            try{
                //关闭时点击，则设置好端口，波特率后打开
                comm.PortName = portName;
                comm.BaudRate = baudRate;
                comm.Open();
            }
            catch (Exception ex){
                //捕获到异常信息，创建一个新的comm对象，之前的不能用了.
                //comm = new SerialPort(); //若要创建新的对象，那么对象就需要重新初始化，否则打开串口，新对象没有委托
                //现实异常信息给客户。
                msg = ex.Message;
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

        /// <summary>
        /// 接收数据的地方
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void comm_DataReceived(object sender, SerialDataReceivedEventArgs e){
            if (pointClosing) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环
            try{
                Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。 
                //Thread.Sleep(intS);
                byte[] buf = commDataRead();//声明一个临时数组存储当前来的串口数据
                SendDataType sendType = new SendDataType(buf, comm.PortName,CommunicationMode.SERIALPORT,SendMode.OUT);
                issue(sendType);    
            }
            finally{
                Listening = false;
            }
        }


        //缓存的数据
        byte[] commDataRead(){
            //int n = comm.BytesToRead;//先记录下来，避免某种原因，人为的原因，操作几次之间时间长，缓存不一致
            int nReviceBytesNum = comm.BytesToRead;// ReadByte(); x++;
            //Thread.Sleep(2);
            //while (nReviceBytesNum != comm.BytesToRead){
                
            //    Thread.Sleep(2);
            //}
            nReviceBytesNum = comm.BytesToRead;// ReadByte(); x++;
            byte[] buf = new byte[nReviceBytesNum];//声明一个临时数组存储当前来的串口数据
            received_count += nReviceBytesNum;//增加接收计数
            comm.Read(buf, 0, nReviceBytesNum);//读取缓冲数据      
            nReviceBytesNum = 0;
            return buf;
        }


        public int hexDataSend(String hexData){ //数据发送处理
            //我们不管规则了。如果写错了一些，我们允许的，只用正则得到有效的十六进制数
            MatchCollection mc = Regex.Matches(hexData, @"(?i)[\da-f]{2}");
            List<byte> buf = new List<byte>();//填充到这个临时列表中
            //依次添加到列表中
            foreach (Match m in mc){
                //buf.Add(byte.Parse(m.Value));
                buf.Add(byte.Parse(m.Value, System.Globalization.NumberStyles.HexNumber));
            }
            if (!comm.IsOpen){
                return 0;
            }
            //commWriteByte(buf.ToArray(),0,buf.Count);
            comm.Write(buf.ToArray(), 0, buf.Count);
            return buf.Count; //返回记录发送的字节数
        }

        public void sendData(byte[] buf, int index, int length, string ip)
        {
            //转换列表为数组后发送
            //根据当前串口对象，来判断操作
            if (!comm.IsOpen)
            {
                return;
            }
            try
            {
                comm.Write(buf, index, length);
            }
            catch { }    
        }

        public void sendData(SendDataType sType)
        {
            if (sType.isNoDt) sendData(sType.Buf, 0, sType.Buf.Length, sType.Ip);
        }

        public void closeComm() {
            if (!comm.IsOpen) return;
            try {
                comm.DiscardOutBuffer();
                //comm.Close();
            }
            catch { }
        }


        /// <summary>
        /// 获取端口号的集合
        /// </summary>
        /// <returns></returns>
        public static string[] getSerialPorts()
        {
            string[] ports = SerialPort.GetPortNames();
            Array.Sort(ports);
            return ports;
        }

        
        public void classClose()
        {
            close();
        }

    }
}
