using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace RfUpdateApp
{
    #region 枚举
    public enum SerialPortDataBits : int
    {
        FiveBits = 5,
        SixBits = 6,
        SevenBits = 7,
        EightBits = 8,
    }
    public enum SerialPortBaudRates : int
    {
        BaudRate_75 = 75,
        BaudRate_110 = 110,
        BaudRate_150 = 150,
        BaudRate_300 = 300,
        BaudRate_600 = 600,
        BaudRate_1200 = 1200,
        BaudRate_2400 = 2400,
        BaudRate_4800 = 4800,
        BaudRate_9600 = 9600,
        BaudRate_14400 = 14400,
        BaudRate_19200 = 19200,
        BaudRate_28800 = 28800,
        BaudRate_38400 = 38400,
        BaudRate_56000 = 56000,
        BaudRate_57600 = 57600,
        BaudRate_115200 = 115200,
        BaudRate_128000 = 128000,
        BaudRate_230400 = 230400,
        BaudRate_256000 = 256000,
    }
    #endregion

    public class DataFormat
    {
        public byte DataHead
        {
            get;
            set;
        }
        public byte DataEnd
        {
            get;
            set;
        }
        public byte DataType
        {
            get;
            set;
        }
        public int DataLen
        {
            get;
            set;
        }

        public DataFormat(byte dataHead, byte dataEnd, byte dataType, int dataLen)
        {
            this.DataHead = dataHead;
            this.DataEnd = dataEnd;
            this.DataType = dataType;
            this.DataLen = dataLen;
        }
    }
    
    public class MySerialPort
    {
        public class SerialReceiveEventArgs : EventArgs
        {
            public List<byte> DataBuf;
            public int DataLen = 0;

            public SerialReceiveEventArgs(List<byte> m_DataReceived, int datalen)
            {
                this.DataBuf = m_DataReceived;
                this.DataLen = datalen;
            }
        }
        
        
        public delegate void SerialReceiveEventHandler(object sender, SerialReceiveEventArgs e);
        public event SerialReceiveEventHandler SerialReceive;

        private SerialPort MserialPort = new SerialPort(); 
        private List<List<byte>> SerialDataList = new List<List<byte>>();

        private object LockSerialReceData = new object();
        private object LockSerialSendData = new object();

        #region 字段和属性
        private string _myPortName = null;
        private SerialPortBaudRates _myBaudRates;
        private SerialPortDataBits _myDataBits;
        private Parity _myParity;
        private StopBits _myStopBits;

        public string MyPortName
        {
            get { return _myPortName; }
            set
            {
                try
                {
                    if (value.Substring(0, 3) == "COM")
                    {
                        _myPortName = value;
                    }
                }
                catch { }
            }
        }
        public SerialPortBaudRates MyBaudRates
        { 
            get { return _myBaudRates; }
            set
            {
                if (value == SerialPortBaudRates.BaudRate_75 || value == SerialPortBaudRates.BaudRate_110 || value == SerialPortBaudRates.BaudRate_150 ||
                    value == SerialPortBaudRates.BaudRate_300 || value == SerialPortBaudRates.BaudRate_600 || value == SerialPortBaudRates.BaudRate_1200 ||
                    value == SerialPortBaudRates.BaudRate_2400 || value == SerialPortBaudRates.BaudRate_4800 || value == SerialPortBaudRates.BaudRate_9600 ||
                    value == SerialPortBaudRates.BaudRate_14400 || value == SerialPortBaudRates.BaudRate_19200 || value == SerialPortBaudRates.BaudRate_28800 ||
                    value == SerialPortBaudRates.BaudRate_38400 || value == SerialPortBaudRates.BaudRate_56000 || value == SerialPortBaudRates.BaudRate_57600 ||
                    value == SerialPortBaudRates.BaudRate_115200 || value == SerialPortBaudRates.BaudRate_128000 || value == SerialPortBaudRates.BaudRate_230400 ||
                    value == SerialPortBaudRates.BaudRate_256000)
                {
                    _myBaudRates = value;
                }
            }
        }
        public SerialPortDataBits MyDataBits
        {
            get { return _myDataBits; }
            set
            {
                if (value == SerialPortDataBits.FiveBits || value == SerialPortDataBits.SixBits || value == SerialPortDataBits.SevenBits || value == SerialPortDataBits.EightBits)
                {
                    _myDataBits = value;
                }
            }
        }
        public Parity MyParity
        {
            get { return _myParity; }
            set
            {
                if (value == Parity.Even || value == Parity.Mark || value == Parity.None || value == Parity.Odd || value == Parity.Space)
                {
                    _myParity = value;
                }
            }
        }
        public StopBits MyStopBits
        {
            get { return _myStopBits; }
            set
            {
                if (value == StopBits.None || value == StopBits.One || value == StopBits.OnePointFive || value == StopBits.Two)
                {
                    _myStopBits = value;
                }
            }
        }

        public List<DataFormat> ListDataFormat
        {
            get;
            set;
        }

        #endregion


        #region 构造函数
        public MySerialPort()
        {
            this.MyPortName = "COM1";
            this.MyBaudRates = SerialPortBaudRates.BaudRate_115200;
            this.MyDataBits = SerialPortDataBits.EightBits;
            this.MyParity = Parity.None;
            this.MyStopBits = StopBits.One;

            MserialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);     //将SerialPort_DataReceived方法委托到串口接收事件
            MserialPort.ReceivedBytesThreshold = 1;

            ListDataFormat = new List<DataFormat>();
            //直接使用这一句就可以将串口的编码改为GB2312 即可以使用中文发送
            //serialPort.Encoding = System.Text.Encoding.GetEncoding("gb2312");
        }
        public MySerialPort(string port, SerialPortBaudRates baud, SerialPortDataBits databits, Parity parity, StopBits stopbits)
        {
            this.MyPortName = port;
            this.MyBaudRates = baud;
            this.MyDataBits = databits;
            this.MyParity = parity;
            this.MyStopBits = stopbits;

            MserialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);     //将SerialPort_DataReceived方法委托到串口接收事件
            MserialPort.ReceivedBytesThreshold = 1;

            ListDataFormat = new List<DataFormat>();

            //直接使用这一句就可以将串口的编码改为GB2312 即可以使用中文发送
            //serialPort.Encoding = System.Text.Encoding.GetEncoding("gb2312");
        }
        #endregion

        public bool IsOpen()
        {
            return MserialPort.IsOpen;
        }

        public bool OpenPort()
        {
            if (IsOpen() == true)
                return true;
            try
            {
                MserialPort.PortName = MyPortName;
                MserialPort.BaudRate = (int)MyBaudRates;
                MserialPort.DataBits = (int)MyDataBits;
                MserialPort.Parity = MyParity;
                MserialPort.StopBits = MyStopBits;

                MserialPort.Open();
            }
            catch
            { }
            if (IsOpen() == true)
                return true;
            return false;
        }

        public bool ClosePort()
        {
            if (IsOpen() == false)
                return true;
            try
            {
                DiscardBuffer();
                MserialPort.Close();
                ClearSerialReceData();
            }
            catch
            { }
            if (IsOpen() == false)
                return true;
            return false;
        }

        public void DiscardBuffer()
        {
            MserialPort.DiscardInBuffer();
            MserialPort.DiscardOutBuffer();
        }

        static public string[] GetPortName()
        {
            return SerialPort.GetPortNames();
        }

        List<byte> ListRece = new List<byte>();

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buf = new byte[1024];
            bool index = false;
            List<byte> serialDatalist = null;

            try
            {
                if (MserialPort.BytesToRead > 0)
                {
                    while (true)
                    {
                        int  count = MserialPort.BytesToRead;
                        MserialPort.Read(buf, 0, count);

                        for (int i = 0; i < count; i++)
                        {
                            ListRece.Add(buf[i]);
                        }

                        //Thread.Sleep(1);

                        if (MserialPort.BytesToRead == 0)
                        {
                            while (ListRece.Count > 1)
                            {
                                index = false;
                                foreach (DataFormat item in ListDataFormat)
                                {
                                    //找到对应的数据头和类型
                                    if (item.DataHead == ListRece[0] && item.DataType == ListRece[1])
                                    {
                                        //长度是否足够
                                        if (ListRece.Count >= item.DataLen)
                                        {
                                            //找到对应的数据尾
                                            if (ListRece[item.DataLen - 1] == item.DataEnd)
                                            {
                                                index = true;
                                                //list每次都申请一块内存，这样子放进SerialDataList集合中才不会公用其内存
                                                serialDatalist = new List<byte>();
                                                for (int i = 0; i < item.DataLen; i++)
                                                {
                                                    serialDatalist.Add(ListRece[0]);
                                                    ListRece.RemoveAt(0);
                                                }
                                                lock (LockSerialReceData)
                                                {
                                                    if (SerialDataList.Count > 100)
                                                    {
                                                        SerialDataList.RemoveAt(0);
                                                    }
                                                    SerialDataList.Add(serialDatalist);
                                                }
                                                break;
                                            }
                                            //数据尾不对
                                            else
                                            {
                                                //continue;
                                                break;
                                            }
                                        }
                                        //长度不足，直接退出
                                        else
                                        {
                                            return;
                                        }
                                    }
                                }
                                //找不到符合的格式，移除第一位后继续找
                                if (index == false)
                                {
                                    ListRece.RemoveAt(0);
                                }
                            }

                            if (SerialReceive != null)
                            {
                                this.SerialReceive(this, new SerialReceiveEventArgs(serialDatalist, serialDatalist.Count)); 
                            }
                            break;
                        }
                    }
                }
            }
            catch { };
        }
        
        /// <summary>
        /// 读取串口接收数据
        /// </summary>
        /// <param name="buf"></param>
        public bool ReadData(ref List<byte> buf)
        {
            lock (LockSerialReceData)
            {
                if (SerialDataList.Count > 0)
                {
                    buf.Clear();
                    buf.AddRange(SerialDataList[0]);
                    SerialDataList.RemoveAt(0);
                    return true;
                }
            }
            return false;
        }

        public void ClearSerialReceData()
        {
            lock (LockSerialReceData)
            {
                SerialDataList.Clear();
            }
        }

        public bool WriteData(string str)
        {
            try
            {
                MserialPort.Write(str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool WriteData(byte[] buf, int offset, int count)
        {
            try
            {
                lock (LockSerialSendData)
                {
                    MserialPort.Write(buf, offset, count);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
