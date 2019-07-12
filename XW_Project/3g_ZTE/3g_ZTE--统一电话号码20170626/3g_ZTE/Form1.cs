using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using NPOILib;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace _3g_ZTE
{
    public partial class Form1 :  Form
    {
        int datalen = 0;
        SerialPort sp1 = new SerialPort();
        OpenFileDialog mOpenFileDialog;
        NPOILibs mNpoiLib = null;
        public string XlsName = "CC2530_3G.xls";

        public static int clb_io_select_old = 0;
        public static int clb_io_select_new = 0;                  
        public static char[] PinSustainTime = new char[5];          //1脚的上升沿发生后需要有效的持续时间
        public static string[] PinBindingPhoneNum = new string[5];  //1脚上升沿功能绑定的可控号码
        public static byte PinBindingPhoneNumCount = 0;             //1脚上升沿功能绑定的可控号码的个数
        public static char[] PinMessage = new char[35];             //1脚上升沿功能发送的控制信息

        private bool Listening = false;
        private bool pointClosing = false;

        private Object lockMergeDataOperation = new object();
        
        public Form1()
        {
            InitializeComponent();
            mOpenFileDialog = new OpenFileDialog();
            mOpenFileDialog.Filter = "Execl files (*.xls)|*.xls";//过滤选择文件的类型
            mOpenFileDialog.Multiselect = false;//不允许多选文件
        }
        
        //加载
        private void Form1_Load(object sender, EventArgs e)
        {
            Profile.LoadProfile();
            
            // 预置波特率
            
            switch (Profile.G_BAUDRATE)
            {
                case "300":
                    cbBaudRate.SelectedIndex = 0;
                    break;
                case "600":
                    cbBaudRate.SelectedIndex = 1;
                    break;
                case "1200":
                    cbBaudRate.SelectedIndex = 2;
                    break;
                case "2400":
                    cbBaudRate.SelectedIndex = 3;
                    break;
                case "4800":
                    cbBaudRate.SelectedIndex = 4;
                    break;
                case "9600":
                    cbBaudRate.SelectedIndex = 5;
                    break;
                case "19200":
                    cbBaudRate.SelectedIndex = 6;
                    break;
                case "38400":
                    cbBaudRate.SelectedIndex = 7;
                    break;
                case "115200":
                    cbBaudRate.SelectedIndex = 8;
                    break;
                default:
                    {
                        MessageBox.Show("波特率预置参数错误。");
                        return;
                    }
            }
            
            //预置波特率
            switch (Profile.G_DATABITS)
            {
                case "5":
                    cbDataBits.SelectedIndex = 0;
                    break;
                case "6":
                    cbDataBits.SelectedIndex = 1;
                    break;
                case "7":
                    cbDataBits.SelectedIndex = 2;
                    break;
                case "8":
                    cbDataBits.SelectedIndex = 3;
                    break;
                default:
                    {
                        MessageBox.Show("数据位预置参数错误。");
                        return;
                    }

            }
            //预置停止位
            switch (Profile.G_STOP)
            {
                case "1":
                    cbStop.SelectedIndex = 0;
                    break;
                case "1.5":
                    cbStop.SelectedIndex = 1;
                    break;
                case "2":
                    cbStop.SelectedIndex = 2;
                    break;
                default:
                    {
                        MessageBox.Show("停止位预置参数错误。");
                        return;
                    }
            }

            //预置校验位
            switch (Profile.G_PARITY)
            {
                case "NONE":
                    cbParity.SelectedIndex = 0;
                    break;
                case "ODD":
                    cbParity.SelectedIndex = 1;
                    break;
                case "EVEN":
                    cbParity.SelectedIndex = 2;
                    break;
                default:
                    {
                        MessageBox.Show("校验位预置参数错误。");
                        return;
                    }
            }
            
            //检查是否含有串口
            string[] str = SerialPort.GetPortNames();
            if (str == null)
            {
                MessageBox.Show("本机没有串口！", "Error");
                return;
            }

            //添加串口项目
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {//获取有多少个COM口
                //System.Diagnostics.Debug.WriteLine(s);
                ComPort.Items.Add(s);
            }

            //串口设置默认选择项
            ComPort.SelectedIndex = 0;         //note：获得COM9口，但别忘修改
            cbBaudRate.SelectedIndex = 8;

            Control.CheckForIllegalCrossThreadCalls = false;    //这个类中我们不检查跨线程的调用是否合法(因为.net 2.0以后加强了安全机制,，不允许在winform中直接跨线程访问控件的属性)
            sp1.DataReceived += new SerialDataReceivedEventHandler(sp1_DataReceived);   //???
            //sp1.ReceivedBytesThreshold = 1;
            //dataReceived = new Thread(new ThreadStart(DataReceivedFunc));
            //dataReceived.Start();

            //准备就绪              
            sp1.DtrEnable = true;
            sp1.RtsEnable = true;
            sp1.Close();

            gb_io_in.Enabled = false;
            gb_io_out.Enabled = false;
            bt_enter_config.Enabled = false;
            bt_save_excel.Enabled = false;
            bt_read_excel.Enabled = false;
        }

        void sp1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (pointClosing) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环

            int i = 0,count = 0;
            int length = 0;
            byte sum = 0;
            //Array.Clear(SerialData.serialBuff, 0, SerialData.serialBuff.Length);
            if (sp1.IsOpen)     //此处可能没有必要判断是否打开串口，但为了严谨性，我还是加上
            {                    
                try
                {
                    Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。 
                    while (true)
                    {
                        if (sp1.BytesToRead > 0)
                        {
                            length = sp1.Read(SerialData.serialBuff, datalen, sp1.BytesToRead);
                            datalen += length; count = 0;
                            if (datalen >= 1024)
                            {
                                datalen = 0;
                                break;
                            }
                        }
                        else
                        {
                            if (count >= 5) break;
                            Thread.Sleep(10);
                            Application.DoEvents();
                            count++;
                        }
                    }
                    /*
                    while (sp1.BytesToRead != 0) 
                    {
                        length = sp1.Read(SerialData.serialBuff , 0+ datalen, sp1.BytesToRead);
                        datalen += length;
                        if (datalen >= 1024)
                        {
                            datalen = 0;                             
                            break;
                        }
                        Thread.Sleep(15);
                    }*/
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "串口接收数据出错");
                }
                int start = -1;
                int curdatelen = datalen;
                while (curdatelen >= 8)
                {
                    start = Find(0x5A, 0x83, SerialData.serialBuff);
                    if (start >= 0)
                    {
                        if (SerialData.serialBuff[start + 2] == SerialData.replySetSuccess[2] && SerialData.serialBuff[start + 3] == SerialData.replySetSuccess[3]
                            && SerialData.serialBuff[start + 4] == SerialData.replySetSuccess[4] && SerialData.serialBuff[start + 5] == SerialData.replySetSuccess[5]
                            && SerialData.serialBuff[start + 6] == SerialData.replySetSuccess[6] && SerialData.serialBuff[start + 7] == SerialData.replySetSuccess[7]
                            )
                        {
                            SerialData.CommandOK = true;
                            // System.Buffer.BlockCopy(SerialData.serialBuff, start + 1, SerialData.serialBuff, 0, datalen);
                            curdatelen -= 8;
                        }
                        else if (SerialData.serialBuff[start + 2] == 0x00 && SerialData.serialBuff[start + 3] == 0x5C)
                        {
                            for (i = 4; i < 95; i++)
                            {
                                sum += SerialData.serialBuff[start + i];//计算校验位
                            }
                            if (sum == SerialData.serialBuff[start + 95])
                            {
                                SerialData.CommandOK = true;

                            }
                            //System.Buffer.BlockCopy(SerialData.serialBuff, start + 1, SerialData.serialBuff, 0, datalen);
                            curdatelen -= 98;
                        }
                        else if (SerialData.serialBuff[start + 2] == 0x03 && (SerialData.serialBuff[start + 3] == 0xD1))
                        {
                            for (i = 4; i < 125 + 122 * 7 + 1; i++)
                            {
                                sum += SerialData.serialBuff[start + i];//计算校验位
                            }
                            if (sum == SerialData.serialBuff[start + 125 + 122 * 7 + 1])
                            {
                                SerialData.CommandOK = true;
                            }
                        }
                        //System.Buffer.BlockCopy(SerialData.serialBuff, start + 1, SerialData.serialBuff, 0, datalen);
                        curdatelen -= 983;

                    }
                    else
                        datalen = 0;
                }

                datalen = 0;

            }

            else
            {
                MessageBox.Show("请打开某个串口", "错误提示");
            }
            Listening = false;
        }
        
        private int Find(byte ch1, byte ch2, byte[] chars)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                if (ch1 == chars[i] && ch2 == chars[i + 1])
                {
                    return i;
                }
            }
            return -1;
        }
        //关闭时事件
        private void Form1_FormCloseing(object sender, EventArgs e)
        {
            sp1.Close();
        }

        private void bt_serial_open_close_Click(object sender, EventArgs e)
        {
            //serialPort1.IsOpen
            if (!sp1.IsOpen)
            {
                bt_serial_open_close.Enabled = false;
                try
                {
                    //设置串口号
                    string serialName = ComPort.SelectedItem.ToString();
                    sp1.PortName = serialName;

                    //设置各“串口设置”
                    string strBaudRate = cbBaudRate.Text;
                    string strDateBits = cbDataBits.Text;
                    string strStopBits = cbStop.Text;
                    Int32 iBaudRate = Convert.ToInt32(strBaudRate);
                    Int32 iDateBits = Convert.ToInt32(strDateBits);

                    sp1.BaudRate = iBaudRate;       //波特率
                    sp1.DataBits = iDateBits;       //数据位
                    switch (cbStop.Text)            //停止位
                    {
                        case "1":
                            sp1.StopBits = StopBits.One;
                            break;
                        case "1.5":
                            sp1.StopBits = StopBits.OnePointFive;
                            break;
                        case "2":
                            sp1.StopBits = StopBits.Two;
                            break;
                        default:
                            MessageBox.Show("Error：参数不正确!", "Error");
                            break;
                    }
                    switch (cbParity.Text)             //校验位
                    {
                        case "无":
                            sp1.Parity = Parity.None;
                            break;
                        case "奇校验":
                            sp1.Parity = Parity.Odd;
                            break;
                        case "偶校验":
                            sp1.Parity = Parity.Even;
                            break;
                        default:
                            MessageBox.Show("Error：参数不正确!", "Error");
                            break;
                    }

                    if (sp1.IsOpen == true)//如果打开状态，则先关闭一下
                    {
                        sp1.Close();
                    }

                    //设置必要控件不可用
                    ComPort.Enabled = false;
                    cbBaudRate.Enabled = false;
                    cbDataBits.Enabled = false;
                    cbStop.Enabled = false;
                    cbParity.Enabled = false;                  

                    sp1.Open();     //打开串口

                    bt_enter_config.Enabled = true;
                    SerialData.configuration = false;
                    bt_enter_config.Text = "Enter Config";

                    bt_serial_open_close.Text = "CloseComm";
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message, "Error");
                }
                finally {
                    bt_serial_open_close.Enabled = true;
                }
            }
            else
            {
                pointClosing = true;
                
                //恢复控件功能
                //设置必要控件不可用
                ComPort.Enabled = true;
                cbBaudRate.Enabled = true;
                cbDataBits.Enabled = true;
                cbStop.Enabled = true;
                cbParity.Enabled = true;

                gb_io_in.Enabled = false;
                gb_io_out.Enabled = false;
                bt_enter_config.Enabled = false;
                bt_save_excel.Enabled = false;
                bt_read_excel.Enabled = false;


                while (Listening) Application.DoEvents();

                sp1.Close();                    //关闭串口
                pointClosing = false; 
                bt_serial_open_close.Text = "OpenComm";
            }         
        }

        private void bt_enter_config_Click(object sender, EventArgs e)
        {
            int count = 0;
            SerialData.CommandOK = false;
            this.Text = "正在操作...";
            if (SerialData.configuration == false)
            {
                if (sp1 != null && sp1.IsOpen)
                {
                    sp1.Write(SerialData.inSettingState, 0, 6);
                    Thread.Sleep(200);
                }
                while (!SerialData.CommandOK)
                {
                    sp1.Write(SerialData.inSettingState, 0, 6);
                    System.Threading.Thread.Sleep(200);
                    count++;
                    if (SerialData.CommandOK || count > 3)
                    {
                        break;
                    }
                }
                if (!SerialData.CommandOK)
                {
                    MessageBox.Show("进入配置失败！");
                    this.Text = "";

                }
                else
                {
                    this.Text = "进入配置成功！";
                    bt_enter_config.Text = "Exit Conifg";
                    SerialData.configuration = true;
                    gb_io_in.Enabled = true;
                    gb_io_out.Enabled = true;
                    bt_save_excel.Enabled = true;
                    bt_read_excel.Enabled = true;
                    bt_save_excel.Enabled = true;
                    bt_read_excel.Enabled = true;
                }
            }
            else
            {
                if (sp1 != null && sp1.IsOpen)
                {
                    sp1.Write(SerialData.inSettingState, 0, 6);
                    Thread.Sleep(200);
                }
                while (!SerialData.CommandOK)
                {
                    sp1.Write(SerialData.inSettingState, 0, 6);
                    System.Threading.Thread.Sleep(200);
                    count++;
                    if (SerialData.CommandOK || count > 3)
                    {
                        break;
                    }
                }
                if (!SerialData.CommandOK)
                {
                    MessageBox.Show("退出配置失败！");
                    this.Text = "";                  
                }
                else
                {
                    this.Text = "退出配置成功！";
                    bt_enter_config.Text = "Enter Config";
                    SerialData.configuration = false;
                    gb_io_in.Enabled = false;
                    gb_io_out.Enabled = false;
                    bt_save_excel.Enabled = false;
                    bt_read_excel.Enabled = false;
                }
            }
        }

        public static void StringToByteArray(string s, byte[] b, int len)
        {
            char[] c = new char[len];
            if (s != null)
            {
                c = s.ToCharArray();
                for (int i = 0; i < len; i++)
                {
                    if (i < c.Length)
                    {
                        b[i] = (byte)c[i];
                    }
                    else
                    {
                        b[i] = 0;
                    }
                }
            }
            else
            {
                Array.Clear(b,0, len);//将b全用0x00填充
            }
        }

      
        public void MergeDataOperation(int io_select_old)
        {
            lock (lockMergeDataOperation)
            {
                try
                {
                    SerialData.currentDataBuff_IO_Input[6 + io_select_old * 122] = 0x2F;
                    string s1;
                    //有效持续时间
                    if (tb_MsInput.Text == "")
                    {
                        SerialData.currentDataBuff_IO_Input[7 + io_select_old * 122] = 0x00;
                        SerialData.currentDataBuff_IO_Input[8 + io_select_old * 122] = 0x00;
                    }
                    else
                    {
                        if ((string.Compare("65536", tb_MsInput.Text) < 0) && (tb_MsInput.Text.Length > 5))
                        {
                            MessageBox.Show("设置最大的数值是65535");                          
                        }
                        else
                        {
                            SerialData.currentDataBuff_IO_Input[7 + io_select_old * 122] = (byte)((Convert.ToUInt16(tb_MsInput.Text)) >> 8);
                            SerialData.currentDataBuff_IO_Input[8 + io_select_old * 122] = (byte)((Convert.ToUInt16(tb_MsInput.Text)) & 0x00FF);
                            tb_MsInput.Text = "";
                        }
                    }

                    SerialData.currentDataBuff_IO_Input[9 + io_select_old * 122] = 0x2F;
                    //设置1脚对应的需要发送信息的号码            
                    byte[] b1 = new byte[15];
                    
                    for (int j = 0; j < 5; j++)
                    {
                        if (lb_phonenumber_show.Items.Count > j)
                        {
                            //使用GetBytes函数后，b1的重新申请空间，可能申请的空间会比原来的空间小
                            b1 = Encoding.Default.GetBytes(lb_phonenumber_show.Items[j].ToString());
                        } else {    
                            Array.Clear(b1, 0, b1.Length);
                        }

                        int count = checkedListBoxInput_IO_Setting.Items.Count;
                        for (int i = 0; i < count;i++ ) {
                            setSavePhone(j, b1, i);
                        }                      
                    }
                    int Count = lb_phonenumber_show.Items.Count;
                    for (int i = Count - 1; i >= 0; i--)
                    {
                        removelb_phonenumber(i);
                        //lb_phonenumber_show.Items.RemoveAt(i);//在集合中移除指定的索引位置 
                    }
                    SerialData.currentDataBuff_IO_Input[89 + io_select_old * 122] = 0x2F;   //从0x2C修改为0x2F

                    //设置短信内容
                    byte[] Message = new byte[35];
                    byte temp;           

                    if (string.IsNullOrEmpty(tb_PhoneMessage.Text) == true)
                    {
                        Array.Clear(Message, 0, Message.Length);
                    }
                    else
                    {
                        //Message = Encoding.Default.GetBytes(tb_PhoneMessage.Text);
                        Message = Encoding.Unicode.GetBytes(tb_PhoneMessage.Text);
                        for (int i = 0; i < Message.Length; i += 2)
                        {
                            temp = Message[i];
                            Message[i] = Message[i + 1];
                            Message[i + 1] = temp;
                        }
                        tb_PhoneMessage.Text = "";
                    }
                    for (int i = 90; i < 90 + Message.Length; i++)
                    {
                        SerialData.currentDataBuff_IO_Input[i + io_select_old * 122] = Message[i - 90];
                    }
                    for (int i = 90 + Message.Length; i < 125; i++)
                    {
                        SerialData.currentDataBuff_IO_Input[i + io_select_old * 122] = 0x00;
                    }
                    SerialData.currentDataBuff_IO_Input[125 + io_select_old * 122] = 0x3B;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        /// <summary>
        /// 保存相同的电话号码
        /// </summary>
        /// <param name="j"></param>
        /// <param name="b1"></param>
        /// <param name="io_select_old"></param>
        private void setSavePhone(int j, byte[] b1, int io_select_old)
        {

            for (int i = 10 + 16 * j; i < b1.Length + 10 + 16 * j; i++)
            {
                SerialData.currentDataBuff_IO_Input[i + io_select_old * 122] = b1[i - 10 - 16 * j];
            }
            for (int i = b1.Length + 10 + 16 * j; i < 25 + 16 * j; i++)
            {
                SerialData.currentDataBuff_IO_Input[i + io_select_old * 122] = 0x00;
            }
            SerialData.currentDataBuff_IO_Input[25 + 16 * j + io_select_old * 122] = 0x2C;
        }


        //合并数据
        public int MergeData(int io_select_old)
        {
            try
            {
                //MergeDataPretreatment();
                MergeDataOperation(io_select_old);
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
            
        }

        //拆分数据
        public int SplitData(int io_select_new)
        {
            //clb_io_select_new
            try
            {
                PinSustainTime = new char[5];          //1脚的上升沿发生后需要有效的持续时间
                PinBindingPhoneNum = new string[5];  //1脚上升沿功能绑定的可控号码
                PinBindingPhoneNumCount = 0;             //1脚上升沿功能绑定的可控号码的个数
                PinMessage = new char[35];             //1脚上升沿功能发送的控制信息

                string s1;

                char[] TempChar_BindingPhoneNum = new char[15];
                byte[] Temp_Pin1Rising_FunctionEnable = new byte[2]; //1脚的上升沿功能是否使能
                byte[] Temp_Pin1Rising_SustainTime = new byte[2];//1脚的上升沿发生后需要有效的持续时间                                                                                                                
                byte[] Temp_Pin1Rising_Message = new byte[35];//1脚上升沿功能发送的控制信息

                //获取1脚的上升沿有效持续时间
                //System.Array.Copy(data, mark_head + 7, Temp_Pin1Rising_SustainTime, 0, 2);
                s1 = ((int)SerialData.currentDataBuff_IO_Input[7 + io_select_new * 122] * 256 + (int)SerialData.currentDataBuff_IO_Input[8 + io_select_new * 122]).ToString();
                StringToCharArray(s1, PinSustainTime, PinSustainTime.Length);
                //获取1脚的输入号码
                PinBindingPhoneNumCount = 0;//清空1脚的有效输入号码个数
                for (int i = 0; i < 5; i++)
                {
                    if (SerialData.currentDataBuff_IO_Input[10 + 16 * i + io_select_new * 122] != 0)
                    {
                        PinBindingPhoneNumCount++;
                        System.Array.Copy(SerialData.currentDataBuff_IO_Input, 10 + 16 * i + io_select_new * 122, TempChar_BindingPhoneNum, 0, 15);
                        PinBindingPhoneNum[PinBindingPhoneNumCount - 1] = new string(TempChar_BindingPhoneNum);
                    }
                }

                //获取对应的短消息
                //System.Array.Copy(c2, 0, Pin1Rising_Message, 0, Pin1Rising_Message.Length);
                if ((SerialData.currentDataBuff_IO_Input[10 + 16 * 5 + io_select_new * 122] != 0) || (SerialData.currentDataBuff_IO_Input[10 + 16 * 5 + io_select_new * 122 + 1] != 0))
                {
                    System.Array.Copy(SerialData.currentDataBuff_IO_Input, 10 + 16 * 5 + io_select_new * 122, Temp_Pin1Rising_Message, 0, 35);
                    byte temp;
                    for (int j = 0; j < Temp_Pin1Rising_Message.Length - 1; j += 2)     //交換奇偶數位字节
                    {
                        temp = Temp_Pin1Rising_Message[j];
                        Temp_Pin1Rising_Message[j] = Temp_Pin1Rising_Message[j + 1];
                        Temp_Pin1Rising_Message[j + 1] = temp;
                        //if(Temp_Pin1Rising_Message[j + 2] == 0x00
                    }
                    for (int i = 0,j = 0; j < 34; j += 2) 
                    {
                        //避免4e00（一）,0031（数字1）的情况
                        if ((Temp_Pin1Rising_Message[j] != 0) || (Temp_Pin1Rising_Message[j + 1] != 0))  
                        {
                            PinMessage[i] = (char)((char)Temp_Pin1Rising_Message[j + 1] << 8);
                            PinMessage[i] = (char)(PinMessage[i] + (char)Temp_Pin1Rising_Message[j]);
                            i++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }

        public static void StringToCharArray(string s, char[] c, int len)
        {
            char[] c1 = new char[len];
            if (s.Equals("") == false)
            {
                c1 = s.ToCharArray();
                for (int i = 0; i < len; i++)
                {
                    if (i < c1.Length)
                    {
                        c[i] = c1[i];
                    }
                    else
                    {
                        c[i] = '\0';
                    }
                }
            }
            else
            {
                Array.Clear(c,0, len);//将c全用0x00填充
            }
        }
        public void MergeDataPretreatment()
        {
            int count = 0;

            //保存上一次的内容
            //if (tb_MsInput.Text.Equals(""))
            if(tb_MsInput.Text == "")
            {
                //System.Array.Copy(c2, 0, PinSustainTime, 0, PinSustainTime.Length);
                Array.Clear(PinSustainTime, 0, PinSustainTime.Length);
            }
            else
            {
                PinSustainTime = tb_MsInput.Text.ToCharArray(0, tb_MsInput.Text.Length);
                tb_MsInput.Text = "";
            }
            for (int i = 0; i < 5; i++)
            {
                PinBindingPhoneNum[i] = "";
            }
            count = lb_phonenumber_show.Items.Count;
            PinBindingPhoneNumCount = (byte)(count & 0x00FF);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    PinBindingPhoneNum[i] = lb_phonenumber_show.Items[i].ToString();
                }
                for (int i = count - 1; i >= 0; i--)
                {
                    lb_phonenumberDelAll(i);
                    //lb_phonenumber_show.Items.RemoveAt(i);//在集合中移除指定的索引位置 
                }
            }
            //if (tb_PhoneMessage.Text.Equals(""))
            if(tb_PhoneMessage.Text == "")
            {
                //System.Array.Copy(c2, 0, PinMessage, 0, PinMessage.Length);
                Array.Clear(PinMessage, 0, PinMessage.Length);
            }
            else
            {
                PinMessage = tb_PhoneMessage.Text.ToCharArray(0, tb_PhoneMessage.Text.Length);
                tb_PhoneMessage.Text = "";
            }
        }

        //显示本次的内容
        public void DisplayCurrentSelectedIndex(int io_select_new)
        {
            try
            {
                if ((io_select_new >= 8) || (io_select_new < 0))
                {
                    throw new ArgumentException("io_select_new is error");
                }
                SplitData(io_select_new);
                string s1 = new string(PinSustainTime);
                if (s1[0] == '0')
                {
                    tb_MsInput.Text = "";
                }
                else
                {
                    tb_MsInput.Text = s1;
                }
                //bt_del_all_Click(bt_del_all, e);
                for (int i = 0; i < PinBindingPhoneNumCount; i++)
                {
                    //if (PinBindingPhoneNum[i].Equals("") == false)
                    if (PinBindingPhoneNum[i] != null && PinBindingPhoneNum[i] != "")
                    {
                        lb_phonenumberAddItem(PinBindingPhoneNum[i]);                       
                        //lb_phonenumber_show.Items.Add(PinBindingPhoneNum[i]);
                    }
                }
                string s2 = new string(PinMessage);
                {
                    tb_PhoneMessage.Text = s2;
                }

                if (SerialData.currentDataBuff_IO_Input[5 + io_select_new * 122] == 0x01)
                {
                    //checkedListBoxInput_IO_Setting.SetItemCheckState(io_select_new, CheckState.Checked);
                }
                else
                {
                    //checkedListBoxInput_IO_Setting.SetItemCheckState(io_select_new, CheckState.Unchecked);
                }
            }
            catch
            {
                return;
            }
        }

        private void checkedListBoxInput_IO_Setting_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            //保存上一次的内容
            MergeData(clb_io_select_old);
            
            //显示本次的内容
            clb_io_select_new = checkedListBoxInput_IO_Setting.SelectedIndex;
            //当checklistbox没有被选中时，clb_io_select_new = -1
            if((clb_io_select_new >= 8) || (clb_io_select_new < 0))
            {
                clb_io_select_new = 0;
            }
            
            DisplayCurrentSelectedIndex(clb_io_select_new);
            
            //判断各引脚的上升沿和下降沿功能是否使能
            if (checkedListBoxInput_IO_Setting.GetItemChecked(clb_io_select_old))
            {
                SerialData.currentDataBuff_IO_Input[4 + clb_io_select_old * 122] = (byte)(1 + clb_io_select_old / 2);
                SerialData.currentDataBuff_IO_Input[5 + clb_io_select_old * 122] = 0x01;
            }
            else
            {
                SerialData.currentDataBuff_IO_Input[4 + clb_io_select_old * 122] = (byte)(1 + clb_io_select_old / 2);
                SerialData.currentDataBuff_IO_Input[5 + clb_io_select_old * 122] = 0x00;
            }

            clb_io_select_old = clb_io_select_new;
           
        }

        private void bt_add_Click(object sender, EventArgs e) {
            int count = 0;
            count = lb_phonenumber_show.Items.Count;
            if (count == 5)
            {
                MessageBox.Show("手機號碼個數超過5個！");
                tb_phonenumberinput.Text = "";
                return;
            }
            //if (tb_phonenumberinput.Text.Equals(""))
            if(tb_phonenumberinput.Text == "")
            {
                MessageBox.Show("請輸入手機號！");
                return;
            }

            lb_phonenumberAddItem(tb_phonenumberinput.Text);
            
        }

        private void lb_phonenumberAddItem(String itemText)
        {
            lb_phonenumber_showAddItem(itemText);
            lb_phonenumber_showSelectItem();
            lb_phonenumber_show2AddItem(itemText);
            lb_phonenumber_show2SelectItem();
        }


        private void lb_phonenumber_showAddItem(String itemText) {
            lb_phonenumber_show.Items.Add(itemText);
            tb_phonenumberinput.Text = "";            
        }

        private void lb_phonenumber_showSelectItem() {
            int count = lb_phonenumber_show.Items.Count;
            lb_phonenumber_show.SetSelected(count - 1, true);
        }

            
        private void bt_delete_Click(object sender, EventArgs e)
        {
            if ((lb_phonenumber_show.SelectedItems != null) && (lb_phonenumber_show.SelectedIndex >= 0))
            {
                removelb_phonenumber(this.lb_phonenumber_show.SelectedIndex);//在集合中移除指定的索引位置 
                //this.lb_phonenumber_show.Items.RemoveAt(this.lb_phonenumber_show.SelectedIndex);                
            }
        }


        private void removelb_phonenumber_show(int SelectedIndex) {
            try {
                this.lb_phonenumber_show.Items.RemoveAt(SelectedIndex);//在集合中移除指定的索引位置 
            }
            catch { }          
        }


        private void removelb_phonenumber(int Index)
        {
            removelb_phonenumber_show(Index);
            removelb_phonenumber_show2(Index);
        }

        private void lb_phonenumberDelAll(int type){
            int Count = lb_phonenumber_show.Items.Count;
            for (int i = Count - 1; i >= 0; i--){
                //this.lb_phonenumber_show.Items.RemoveAt(i);//在集合中移除指定的索引位置 
                if(type != 3) removelb_phonenumber_show(i);
                if (type != 4) removelb_phonenumber_show2(i);
            }
        }


        private void bt_del_all_Click(object sender, EventArgs e){
            lb_phonenumberDelAll(2);
        }


        private void bt_clear_message_Click(object sender, EventArgs e)
        {
            tb_PhoneMessage.Text = "";
        }

        private void bt_set_io_input_Click(object sender, EventArgs e)
        {
            btnSetInput();
            outPutSet();
        }


        private void btnSetInput() {
            //保存上一次的内容
            MergeData(clb_io_select_old);
            //判断各引脚的上升沿和下降沿功能是否使能
            if (checkedListBoxInput_IO_Setting.GetItemChecked(clb_io_select_old))
            {
                SerialData.currentDataBuff_IO_Input[4 + clb_io_select_old * 122] = (byte)(1 + clb_io_select_old / 2);
                SerialData.currentDataBuff_IO_Input[5 + clb_io_select_old * 122] = 0x01;
            }
            else
            {
                SerialData.currentDataBuff_IO_Input[4 + clb_io_select_old * 122] = (byte)(1 + clb_io_select_old / 2);
                SerialData.currentDataBuff_IO_Input[5 + clb_io_select_old * 122] = 0x00;
            }

            //显示本次的内容
            clb_io_select_new = checkedListBoxInput_IO_Setting.SelectedIndex;
            if ((clb_io_select_new >= 8) || (clb_io_select_new < 0))
            {
                clb_io_select_new = 0;
            }
            clb_io_select_old = clb_io_select_new;
            DisplayCurrentSelectedIndex(clb_io_select_new);

            int count = 0;
            SerialData.CommandOK = false;
            //FillClearBuffer(1, Cmd.cmd_all, Cmd.cmd_all.Length);
            this.Text = "正在操作...";

            Array.Copy(SerialData.inputSet, 0, SerialData.currentDataBuff_IO_Input, 0, SerialData.inputSet.Length);
            SerialData.currentDataBuff_IO_Input[125 + 122 * 7 + 1] = 0;
            for (int i = 4; i < 125 + 122 * 7 + 1; i++)
            {
                SerialData.currentDataBuff_IO_Input[125 + 122 * 7 + 1] += SerialData.currentDataBuff_IO_Input[i];//计算校验位
            }
            SerialData.currentDataBuff_IO_Input[125 + 122 * 7 + 1 + 1] = 0x0D;
            SerialData.currentDataBuff_IO_Input[125 + 122 * 7 + 1 + 1 + 1] = 0x0A;
            if (sp1 != null && sp1.IsOpen)
            {
                sp1.Write(SerialData.currentDataBuff_IO_Input, 0, 125 + 122 * 7 + 4);
            }
            Thread.Sleep(300);
            count = 0;
            while (!SerialData.CommandOK)
            {
                if (sp1 != null && sp1.IsOpen)
                {
                    sp1.Write(SerialData.currentDataBuff_IO_Input, 0, 125 + 122 * 7 + 4);
                }
                Thread.Sleep(300);
                count++;
                if (SerialData.CommandOK || count > 3)
                {
                    break;
                }
            }
            if (!SerialData.CommandOK)
            {
                MessageBox.Show("設置失敗！");
                this.Text = "";
            }
            else
            {
                this.Text = "設置成功！";
            }
        }



        private void bt_get_io_input_Click(object sender, EventArgs e)
        {
            int count = 0;
            SerialData.CommandOK = false;
            this.Text = "正在操作...";
            if (sp1 != null && sp1.IsOpen)
            {
                sp1.Write(SerialData.readFlashInputSet, 0, SerialData.readFlashInputSet.Length);
            }
            Thread.Sleep(300);
            count = 0;
            while (!SerialData.CommandOK)
            {
                if (sp1 != null && sp1.IsOpen)
                {
                    sp1.Write(SerialData.readFlashInputSet, 0, SerialData.readFlashInputSet.Length);
                }
                Thread.Sleep(300);
                count++;
                if (SerialData.CommandOK || count > 3)
                {
                    break;
                }
            }
            if (!SerialData.CommandOK)
            {
                MessageBox.Show("獲取IO口輸入設置信息失敗！");
                this.Text = "";
            }
            else
            {
                this.Text = "獲取IO口輸入設置信息成功！";
                Array.Copy(SerialData.serialBuff, 0, SerialData.currentDataBuff_IO_Input, 0, SerialData.currentDataBuff_IO_Input.Length);
                tb_MsInput.Text = "";
                count = lb_phonenumber_show.Items.Count;
                for (int i = count - 1; i >= 0; i--)
                {
                    removelb_phonenumber(i);
                    //this.lb_phonenumber_show.Items.RemoveAt(i);//在集合中移除指定的索引位置 
                } 
                tb_PhoneMessage.Text = "";
                for (int i = 0; i < 8; i++)
                {
                    if (SerialData.currentDataBuff_IO_Input[5 + i * 122] == 0x01)
                    {
                        checkedListBoxInput_IO_Setting.SetItemCheckState(i, CheckState.Checked);
                    }
                    else
                    {
                        checkedListBoxInput_IO_Setting.SetItemCheckState(i, CheckState.Unchecked);
                    }
                }
                DisplayCurrentSelectedIndex(clb_io_select_new);
            }
        }

        private void bt_add2_Click(object sender, EventArgs e)
        {
            int count = 0;
            count = lb_phonenumber_show2.Items.Count;
            if (count == 5)
            {
                MessageBox.Show("手機號碼個數超過5個！");
                tb_phonenumberinput2.Text = "";
                return;
            }
            //if (tb_phonenumberinput2.Text.Equals(""))
            if (tb_phonenumberinput2.Text == "")
            {
                MessageBox.Show("請輸入手機號！");
                return;
            }
            lb_phonenumberAddItem(tb_phonenumberinput2.Text);
        }


        private void lb_phonenumber_show2AddItem(String itemText)
        {
            lb_phonenumber_show2.Items.Add(itemText);
            tb_phonenumberinput2.Text = "";
        }

        private void lb_phonenumber_show2SelectItem()
        {
            int count = lb_phonenumber_show2.Items.Count;
            lb_phonenumber_show2.SetSelected(count - 1, true);
        }



        private void bt_delete2_Click(object sender, EventArgs e)
        {
            if ((lb_phonenumber_show2.SelectedItems != null) && (lb_phonenumber_show2.SelectedIndex >= 0))
            {
                removelb_phonenumber(this.lb_phonenumber_show2.SelectedIndex);
                //this.lb_phonenumber_show2.Items.RemoveAt();//在集合中移除指定的索引位置 
            }
        }


        private void removelb_phonenumber_show2(int SelectedIndex)
        {
            try {
                this.lb_phonenumber_show2.Items.RemoveAt(SelectedIndex);//在集合中移除指定的索引位置 
            }
            catch { }           
        }



        private void bt_del_all2_Click(object sender, EventArgs e) {
            lb_phonenumberDelAll(1);
        }

        public void Update_IO_OutputBuff()
        {
            int count = 0;
            string[] OutPutControl_BindingPhoneNum = new string[5];//可以控制5-8脚输出的的电话号码
            byte OutPutControl_BindingPhoneNumCount = 0;

            Array.Clear(SerialData.currentDataBuff_IO_Output, 0, SerialData.currentDataBuff_IO_Output.Length);
            Array.Copy(SerialData.outputSet, 0, SerialData.currentDataBuff_IO_Output, 0, SerialData.outputSet.Length);

            SerialData.currentDataBuff_IO_Output[4] = 0x2F;
            //设置5-8脚的初始化电平状态
            SerialData.currentDataBuff_IO_Output[5] = 0x05;
            SerialData.currentDataBuff_IO_Output[6] = (byte)cb_Pin5_State.SelectedIndex;
            SerialData.currentDataBuff_IO_Output[7] = 0x06;
            SerialData.currentDataBuff_IO_Output[8] = (byte)cb_Pin6_State.SelectedIndex;
            SerialData.currentDataBuff_IO_Output[9] = 0x07;
            SerialData.currentDataBuff_IO_Output[10] = (byte)cb_Pin7_State.SelectedIndex;
            SerialData.currentDataBuff_IO_Output[11] = 0x08;
            SerialData.currentDataBuff_IO_Output[12] = (byte)cb_Pin8_State.SelectedIndex;

            SerialData.currentDataBuff_IO_Output[13] = 0x2F;

            //设置能够控制IO口的电话号码  
            for (int i = 0; i < 5; i++)
            {
                OutPutControl_BindingPhoneNum[i] = "";
            }
            count = lb_phonenumber_show2.Items.Count;
            OutPutControl_BindingPhoneNumCount = (byte)(count & 0x00FF);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    OutPutControl_BindingPhoneNum[i] = lb_phonenumber_show2.Items[i].ToString();
                }
            }
            byte[] b1 = new byte[15];
            StringToByteArray(OutPutControl_BindingPhoneNum[0], b1, b1.Length);
            for (int i = 14; i < 29; i++)
            {
                SerialData.currentDataBuff_IO_Output[i] = b1[i - 14];
            }
            SerialData.currentDataBuff_IO_Output[29] = 0x2C;
            StringToByteArray(OutPutControl_BindingPhoneNum[1], b1, b1.Length);
            for (int i = 30; i < 45; i++)
            {
                SerialData.currentDataBuff_IO_Output[i] = b1[i - 30];
            }
            SerialData.currentDataBuff_IO_Output[45] = 0x2C;
            StringToByteArray(OutPutControl_BindingPhoneNum[2], b1, b1.Length);
            for (int i = 46; i < 61; i++)
            {
                SerialData.currentDataBuff_IO_Output[i] = b1[i - 46];
            }
            SerialData.currentDataBuff_IO_Output[61] = 0x2C;
            StringToByteArray(OutPutControl_BindingPhoneNum[3], b1, b1.Length);
            for (int i = 62; i < 77; i++)
            {
                SerialData.currentDataBuff_IO_Output[i] = b1[i - 62];
            }
            SerialData.currentDataBuff_IO_Output[77] = 0x2C;
            StringToByteArray(OutPutControl_BindingPhoneNum[4], b1, b1.Length);
            for (int i = 78; i < 93; i++)
            {
                SerialData.currentDataBuff_IO_Output[i] = b1[i - 78];
            }

            SerialData.currentDataBuff_IO_Output[93] = 0x2F;

            if (cb_IsAckMessage.Checked == true)
            {
                SerialData.currentDataBuff_IO_Output[94] = 1;
            }
            else
            {
                SerialData.currentDataBuff_IO_Output[94] = 0;
            }
            for (int i = 4; i < 95; i++)
            {
                SerialData.currentDataBuff_IO_Output[95] += SerialData.currentDataBuff_IO_Output[i];//计算校验位
            }
            SerialData.currentDataBuff_IO_Output[96] = 0x0D;
            SerialData.currentDataBuff_IO_Output[97] = 0x0A;

        }

        private void bt_set_io_output_Click(object sender, EventArgs e)
        {
            outPutSet();
            btnSetInput();
        }


        private void outPutSet() {
            int count = 0;
            SerialData.CommandOK = false;
            try
            {
                this.Text = "正在操作...";
                Update_IO_OutputBuff();
                if (sp1 != null && sp1.IsOpen)
                {
                    sp1.Write(SerialData.currentDataBuff_IO_Output, 0, 98);
                }
                Thread.Sleep(300);
                count = 0;
                while (!SerialData.CommandOK)
                {
                    if (sp1 != null && sp1.IsOpen)
                    {
                        sp1.Write(SerialData.currentDataBuff_IO_Output, 0, 98);
                    }
                    Thread.Sleep(300);
                    count++;
                    if (SerialData.CommandOK || count > 3)
                    {
                        break;
                    }
                }
                if (!SerialData.CommandOK)
                {
                    MessageBox.Show("設置失敗！");
                    this.Text = "";
                }
                else
                {
                    this.Text = "設置成功！";
                }
            }
            catch
            {
                return;
            }
        }


        private void bt_get_io_output_Click(object sender, EventArgs e)
        {
            int count = 0;
            char[] TempChar_BindingPhoneNum = new char[15];
            string[] OutPutControl_BindingPhoneNum = new string[5];//可以控制5-8脚输出的的电话号码
            byte OutPutControl_BindingPhoneNumCount = 0;
            try
            {
                this.Text = "正在操作...";
                if (sp1 != null && sp1.IsOpen)
                {
                    sp1.Write(SerialData.readFlashOutputSet, 0, SerialData.readFlashOutputSet.Length);
                }
                Thread.Sleep(300);
                count = 0;
                while (!SerialData.CommandOK)
                {
                    if (sp1 != null && sp1.IsOpen)
                    {
                        sp1.Write(SerialData.readFlashOutputSet, 0, SerialData.readFlashOutputSet.Length);
                    }
                    Thread.Sleep(300);
                    count++;
                    if (SerialData.CommandOK || count > 3)
                    {
                        break;
                    }
                }
                if (!SerialData.CommandOK)
                {
                    MessageBox.Show("獲取IO口輸出設置信息失敗！");
                    this.Text = "";

                }
                else
                {
                    this.Text = "獲取IO口輸出設置信息成功！";
                    //清除上一次所设置的残留字符串
                    cb_Pin5_State.SelectedIndex = 1;//输出脚5脚的初始化电平状态  默认为高
                    cb_Pin5_State.SelectedIndex = 1; ;//输出脚6脚的初始化电平状态  默认为高
                    cb_Pin5_State.SelectedIndex = 1; ;//输出脚7脚的初始化电平状态  默认为高
                    cb_Pin5_State.SelectedIndex = 1; ;//输出脚8脚的初始化电平状态  默认为高
                    OutPutControl_BindingPhoneNum = new string[5];//可以控制5-8脚输出的的电话号码
                    cb_IsAckMessage.Checked = false;//当设置IO口时3G模组是否需要回复短信
                    SerialData.CommandOK = false;
                    //FillClearBuffer(1, Cmd.cmd_all, Cmd.cmd_all.Length);
                    //读取之前先清除窗体中的号码
                    count = lb_phonenumber_show2.Items.Count;
                    OutPutControl_BindingPhoneNumCount = (byte)(count & 0x00FF);
                    if (count > 0)
                    {
                        for (int i = count - 1; i >= 0; i--)
                        {
                            removelb_phonenumber(i);
                            //this.lb_phonenumber_show2.Items.RemoveAt(i);//在集合中移除指定的索引位置 
                        }
                    }
                    //更新SerialData.currentDataBuff_IO_Output
                    Array.Copy(SerialData.serialBuff, 0, SerialData.currentDataBuff_IO_Output, 0, SerialData.currentDataBuff_IO_Output.Length);

                    //显示白名单号码
                    OutPutControl_BindingPhoneNumCount = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        if (SerialData.currentDataBuff_IO_Output[14 + 16 * i] != 0)
                        {
                            OutPutControl_BindingPhoneNumCount++;
                            System.Array.Copy(SerialData.currentDataBuff_IO_Output, 14 + 16 * i, TempChar_BindingPhoneNum, 0, 15);
                            OutPutControl_BindingPhoneNum[i] = new string(TempChar_BindingPhoneNum);
                        }
                    }
                    for (int i = 0; i < OutPutControl_BindingPhoneNumCount; i++)
                    {
                        //if (OutPutControl_BindingPhoneNum[i].Equals("") == false)
                        if (OutPutControl_BindingPhoneNum[i] != "")
                        {
                            lb_phonenumberAddItem(OutPutControl_BindingPhoneNum[i]);
                            //lb_phonenumber_show2.Items.Add(OutPutControl_BindingPhoneNum[i]);
                        }
                    }

                    if (SerialData.currentDataBuff_IO_Output[94] == 1)
                    {
                        cb_IsAckMessage.Checked = true;
                    }
                    else
                    {
                        cb_IsAckMessage.Checked = false;
                    }
                    cb_Pin5_State.SelectedIndex = SerialData.currentDataBuff_IO_Output[6];
                    cb_Pin6_State.SelectedIndex = SerialData.currentDataBuff_IO_Output[8];
                    cb_Pin7_State.SelectedIndex = SerialData.currentDataBuff_IO_Output[10];
                    cb_Pin8_State.SelectedIndex = SerialData.currentDataBuff_IO_Output[12];
                }
            }
            catch
            {
                return;
            }

        }

        private void bt_save_excel_Click(object sender, EventArgs e)
        {
            try
            {
                //保存上一次的内容
                MergeData(clb_io_select_old);
                //判断各引脚的上升沿和下降沿功能是否使能
                if (checkedListBoxInput_IO_Setting.GetItemChecked(clb_io_select_old))
                {
                    SerialData.currentDataBuff_IO_Input[4 + clb_io_select_old * 122] = (byte)(1 + clb_io_select_old / 2);
                    SerialData.currentDataBuff_IO_Input[5 + clb_io_select_old * 122] = 0x01;
                }
                else
                {
                    SerialData.currentDataBuff_IO_Input[4 + clb_io_select_old * 122] = (byte)(1 + clb_io_select_old / 2);
                    SerialData.currentDataBuff_IO_Input[5 + clb_io_select_old * 122] = 0x00;
                }
                Update_IO_OutputBuff();

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.CreatePrompt = true;
                saveFileDialog.Title = "导出Excel文件到";

                DateTime now = DateTime.Now;
                saveFileDialog.FileName = now.Year.ToString().PadLeft(2)
                + now.Month.ToString().PadLeft(2, '0')
                + now.Day.ToString().PadLeft(2, '0') + "-"
                + now.Hour.ToString().PadLeft(2, '0')
                + now.Minute.ToString().PadLeft(2, '0')
                + now.Second.ToString().PadLeft(2, '0');
                saveFileDialog.ShowDialog();

                string name = saveFileDialog.FileName;
                NPOILibs mNp = new NPOILibs(name);

                mNp.hssfworkbook.SetSheetName(0, "3G");

                for (int i = 0; i < 60; i++)
                {
                    for (int j = 0; j < 15; j++)
                    {
                        mNp.writeToCell(mNp.sheet1, i, j, NPOILibs.TYPE_string, "");
                    }
                }

                mNp.sheet1.SetColumnWidth(0, 12 * 256);
                mNp.sheet1.SetColumnWidth(1, 8 * 256);
                mNp.sheet1.SetColumnWidth(2, 12 * 256);
                mNp.sheet1.SetColumnWidth(3, 18 * 256);
                mNp.sheet1.SetColumnWidth(4, 35 * 256);
                mNp.sheet1.SetColumnWidth(5, 12 * 256);
                mNp.sheet1.SetColumnWidth(6, 16 * 256);

                for (int i = 0; i < 8; i++)
                {
                    mNp.sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1 + 6 * i, 5 + 6 * i, 0, 0));
                    mNp.sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1 + 6 * i, 5 + 6 * i, 1, 1));
                    mNp.sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1 + 6 * i, 5 + 6 * i, 2, 2));
                    mNp.sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1 + 6 * i, 5 + 6 * i, 4, 4));
                    /*
                    IRow row = mNp.sheet1.CreateRow(1 + 6 * i);

                    ICell cell = row.CreateCell(0);

                    ICellStyle cellstyle = mNp.hssfworkbook.CreateCellStyle();//设置垂直居中格式

                    cellstyle.VerticalAlignment = VerticalAlignment.CENTER;

                    cell.CellStyle = cellstyle;

                    row = mNp.sheet1.CreateRow(1+ 6 * i);

                    cell = row.CreateCell(1);

                    cellstyle = mNp.hssfworkbook.CreateCellStyle();//设置垂直居中格式

                    cellstyle.VerticalAlignment = VerticalAlignment.CENTER;

                    cell.CellStyle = cellstyle;

                    row = mNp.sheet1.CreateRow(1 + 6 * i);
                     * 
                    cell = row.CreateCell(3);

                    cellstyle = mNp.hssfworkbook.CreateCellStyle();//设置垂直居中格式

                    cellstyle.VerticalAlignment = VerticalAlignment.CENTER;

                    cell.CellStyle = cellstyle;
                    */
                }
                mNp.sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(49, 53, 5, 5));

                mNp.writeToCell(mNp.sheet1, 0, 0, NPOILibs.TYPE_string, "引腳摸式");
                mNp.writeToCell(mNp.sheet1, 0, 1, NPOILibs.TYPE_string, "是否使能");
                mNp.writeToCell(mNp.sheet1, 0, 2, NPOILibs.TYPE_string, "有效持續時間");
                mNp.writeToCell(mNp.sheet1, 0, 3, NPOILibs.TYPE_string, "白名單號碼");
                mNp.writeToCell(mNp.sheet1, 0, 4, NPOILibs.TYPE_string, "短消息內容");
                mNp.writeToCell(mNp.sheet1, 0, 5, NPOILibs.TYPE_string, "是否回復短信");
                mNp.writeToCell(mNp.sheet1, 0, 6, NPOILibs.TYPE_string, "輸出初始狀態");
                mNp.writeToCell(mNp.sheet1, 49, 6, NPOILibs.TYPE_string, "(以下對應腳5-腳8)");

                mNp.writeToCell(mNp.sheet1, 1, 0, NPOILibs.TYPE_string, "PIN1 Rising");
                mNp.writeToCell(mNp.sheet1, 1 + 6, 0, NPOILibs.TYPE_string, "PIN1 Falling");
                mNp.writeToCell(mNp.sheet1, 1 + 12, 0, NPOILibs.TYPE_string, "PIN2 Rising");
                mNp.writeToCell(mNp.sheet1, 1 + 18, 0, NPOILibs.TYPE_string, "PIN2 Falling");
                mNp.writeToCell(mNp.sheet1, 1 + 24, 0, NPOILibs.TYPE_string, "PIN3 Rising");
                mNp.writeToCell(mNp.sheet1, 1 + 30, 0, NPOILibs.TYPE_string, "PIN3 Falling");
                mNp.writeToCell(mNp.sheet1, 1 + 36, 0, NPOILibs.TYPE_string, "PIN4 Rising");
                mNp.writeToCell(mNp.sheet1, 1 + 42, 0, NPOILibs.TYPE_string, "PIN4 Falling");

                mNp.sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(49, 53, 0, 0));
                mNp.writeToCell(mNp.sheet1, 1 + 48, 0, NPOILibs.TYPE_string, "PIN5-PIN8");

                //设置1脚的上升沿功能是否使能
                for (int i = 0; i < 8; i++)
                {
                    SplitData(i);

                    if (SerialData.currentDataBuff_IO_Input[5 + i * 122] == 1)
                    {
                        mNp.writeToCell(mNp.sheet1, 1 + 6 * i, 1, NPOILibs.TYPE_string, "使能");
                    }
                    else
                    {
                        mNp.writeToCell(mNp.sheet1, 1 + 6 * i, 1, NPOILibs.TYPE_string, "失能");
                    }

                    //设置1脚上升沿的有效持续时间
                    if (PinSustainTime[0] != '\0')
                    {
                        string s1 = new string(PinSustainTime);
                        mNp.writeToCell(mNp.sheet1, 1 + 6 * i, 2, NPOILibs.TYPE_string, s1);
                    }

                    //设置1脚对应的需要发送信息的号码            
                    for (int j = 0; j < PinBindingPhoneNumCount; j++)
                    {
                        mNp.writeToCell(mNp.sheet1, j + 1 + 6 * i, 3, NPOILibs.TYPE_string, PinBindingPhoneNum[j]);
                    }

                    //设置短信内容           
                    if (PinMessage[0] != '\0')
                    {
                        string s1 = new string(PinMessage);
                        mNp.writeToCell(mNp.sheet1, 1 + 6 * i, 4, NPOILibs.TYPE_string, s1);
                    }
                }

                //设置5-8脚对应的白名单号码  
                char[] TempChar_BindingPhoneNum = new char[15];
                string[] OutPutControl_BindingPhoneNum = new string[5];
                byte OutPutControl_BindingPhoneNumCount = 0;
                for (int i = 0; i < 5; i++)
                {
                    if (SerialData.currentDataBuff_IO_Output[14 + 16 * i] != 0)
                    {
                        OutPutControl_BindingPhoneNumCount++;
                        System.Array.Copy(SerialData.currentDataBuff_IO_Output, 14 + 16 * i, TempChar_BindingPhoneNum, 0, 15);
                        OutPutControl_BindingPhoneNum[i] = new string(TempChar_BindingPhoneNum);
                    }
                }
                for (int i = 0; i < OutPutControl_BindingPhoneNumCount; i++)
                {
                    mNp.writeToCell(mNp.sheet1, i + 49, 3, NPOILibs.TYPE_string, OutPutControl_BindingPhoneNum[i]);
                }
                //短信是否回复
                if (SerialData.currentDataBuff_IO_Output[94] == 1)
                {
                    mNp.writeToCell(mNp.sheet1, 49, 5, NPOILibs.TYPE_string, "回復");
                }
                else
                {
                    mNp.writeToCell(mNp.sheet1, 49, 5, NPOILibs.TYPE_string, "不回復");
                }

                for (int i = 0; i < 4; i++)
                {
                    if (SerialData.currentDataBuff_IO_Output[6 + 2 * i] == 1)
                    {
                        mNp.writeToCell(mNp.sheet1, 49 + 1 + i, 6, NPOILibs.TYPE_string, "High");
                    }
                    else
                    {
                        mNp.writeToCell(mNp.sheet1, 49 + 1 + i, 6, NPOILibs.TYPE_string, "Low");
                    }
                }

                mNp.WriteToFile(name);

                //显示当前索引内容
                clb_io_select_new = checkedListBoxInput_IO_Setting.SelectedIndex;
                if ((clb_io_select_new >= 8) || (clb_io_select_new < 0))
                {
                    clb_io_select_new = 0;
                }
                clb_io_select_old = clb_io_select_new;
                DisplayCurrentSelectedIndex(clb_io_select_new);
            }
             catch
            {
                return;
            }
        }

        private void bt_read_excel_Click(object sender, EventArgs e)
        {
            
            try
            {
                mOpenFileDialog.ShowDialog();
                if (mOpenFileDialog.FileNames.Length == 1)
                {

                    StartReadConfig();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                //MessageBox.Show("文件被佔用，請關閉");
            }           
        }

        private void StartReadConfig()
        {
            XlsName = mOpenFileDialog.FileName;
            mNpoiLib = new NPOILibs(XlsName);
            //int count = mNpoiLib.getCount(mNpoiLib.sheet1);
            string s1, s2;
            //clb_io_select.SetSelected(0, true);
            int count = lb_phonenumber_show.Items.Count;
            if (count > 0)
            {
                for (int i = count - 1; i >= 0; i--)
                {
                    this.lb_phonenumber_show.Items.RemoveAt(i);//在集合中移除指定的索引位置 
                }
            }
            count = lb_phonenumber_show2.Items.Count;
            if (count > 0)
            {
                for (int i = count - 1; i >= 0; i--)
                {
                    this.lb_phonenumber_show2.Items.RemoveAt(i);//在集合中移除指定的索引位置 
                }
            }
            System.Threading.Thread thread = new System.Threading.Thread(() =>
            {

                for (int i = 0; i < 8; i++)
                {
                    //从EXCEL表格中获取PIN1 Rising的对应信息
                    s1 = Convert.ToString(mNpoiLib.readToCell(mNpoiLib.sheet1, 1 + 6 * i, 1, NPOILibs.TYPE_string));
                    //if (s1.Equals("使能"))
                    if(s1 == "使能")
                    {
                        SerialData.currentDataBuff_IO_Input[5 + i * 122] = 1;
                    }
                    else
                    {
                        SerialData.currentDataBuff_IO_Input[5 + i * 122] = 0;
                    }

                    s1 = Convert.ToString(mNpoiLib.readToCell(mNpoiLib.sheet1, 1 + 6 * i, 2, NPOILibs.TYPE_string));
                    if (string.IsNullOrEmpty(s1) == false)
                    {
                        if ((string.Compare("65536", s1) < 0) && (s1.Length > 5))
                        {
                            MessageBox.Show("设置最大的数值是65535");
                        }
                        else
                        {
                            SerialData.currentDataBuff_IO_Input[7 + i * 122] = (byte)((Convert.ToUInt16(s1)) >> 8);
                            SerialData.currentDataBuff_IO_Input[8 + i * 122] = (byte)((Convert.ToUInt16(s1)) & 0x00FF);
                            tb_MsInput.Text = "";
                        }
                    }
                    else
                    {
                        SerialData.currentDataBuff_IO_Input[7 + i * 122] = 0x00;
                        SerialData.currentDataBuff_IO_Input[8 + i * 122] = 0x00;
                    }

                    PinBindingPhoneNumCount = 0;
                    PinBindingPhoneNum = new string[5];
                    for (byte j = 1; j < 6; j++)
                    {
                        s1 = Convert.ToString(mNpoiLib.readToCell(mNpoiLib.sheet1, j + 6 * i, 3, NPOILibs.TYPE_string));
                        //if (s1.Equals("") == false)
                        if (string.IsNullOrEmpty(s1) == false)
                        {
                            PinBindingPhoneNum[j - 1] = s1;
                            PinBindingPhoneNumCount = j;
                        }
                        else
                        {
                            break;
                        }                      
                    }

                    SerialData.currentDataBuff_IO_Input[6 + i * 122] = 0x2F;
                    SerialData.currentDataBuff_IO_Input[9 + i * 122] = 0x2F;
                    //设置1脚对应的需要发送信息的号码            
                    byte[] b1 = new byte[15];
                    byte[] Message = new byte[35];
                    if (string.IsNullOrEmpty(PinBindingPhoneNum[0]) == false)
                    {
                        b1 = Encoding.Default.GetBytes(PinBindingPhoneNum[0]);
                    }
                    else
                    {
                        Array.Clear(b1, 0, b1.Length);
                    }
                    for (int j = 10; j < 25; j++)
                    {
                        if (j - 10 >= b1.Length)
                        {
                            SerialData.currentDataBuff_IO_Input[j + i * 122] = 0;
                        }
                        else
                        {
                            SerialData.currentDataBuff_IO_Input[j + i * 122] = b1[j - 10];
                        }
                    }
                    SerialData.currentDataBuff_IO_Input[25 + i * 122] = 0x2C;
                    
                    if (string.IsNullOrEmpty(PinBindingPhoneNum[1]) == false)
                    {
                        b1 = Encoding.Default.GetBytes(PinBindingPhoneNum[1]);
                    }
                    else
                    {
                        Array.Clear(b1, 0, b1.Length);
                    }
                    for (int j = 26; j < 41; j++)
                    {
                        if (j - 26 >= b1.Length)
                        {
                            SerialData.currentDataBuff_IO_Input[j + i * 122] = 0;
                        }
                        else
                        {
                            SerialData.currentDataBuff_IO_Input[j + i * 122] = b1[j - 26];
                        }
                    }
                    SerialData.currentDataBuff_IO_Input[41 + i * 122] = 0x2C;
                    
                    if (string.IsNullOrEmpty(PinBindingPhoneNum[2]) == false)
                    {
                        b1 = Encoding.Default.GetBytes(PinBindingPhoneNum[2]);
                    }
                    else
                    {
                        Array.Clear(b1, 0, b1.Length);
                    }
                    for (int j = 42; j < 57; j++)
                    {
                        if (j - 42 >= b1.Length)
                        {
                            SerialData.currentDataBuff_IO_Input[j + i * 122] = 0;
                        }
                        else
                        {
                            SerialData.currentDataBuff_IO_Input[j + i * 122] = b1[j - 42];
                        }

                    }
                    SerialData.currentDataBuff_IO_Input[57 + i * 122] = 0x2C;
                    
                    if (string.IsNullOrEmpty(PinBindingPhoneNum[3]) == false)
                    {
                        b1 = Encoding.Default.GetBytes(PinBindingPhoneNum[3]);
                    }
                    else
                    {
                        Array.Clear(b1, 0, b1.Length);
                    }
                    for (int j = 58; j < 73; j++)
                    {
                        if (j - 58 >= b1.Length)
                        {
                            SerialData.currentDataBuff_IO_Input[j + i * 122] = 0;
                        }
                        else
                        {
                            SerialData.currentDataBuff_IO_Input[j + i * 122] = b1[j - 58];
                        }
                    }
                    SerialData.currentDataBuff_IO_Input[73 + i * 122] = 0x2C;
                    
                    if (string.IsNullOrEmpty(PinBindingPhoneNum[4]) == false)
                    {
                        b1 = Encoding.Default.GetBytes(PinBindingPhoneNum[4]);
                    }
                    else
                    {
                        Array.Clear(b1, 0, b1.Length);
                    }
                    for (int j = 74; j < 89; j++)
                    {
                        if (j - 74 >= b1.Length)
                        {
                            SerialData.currentDataBuff_IO_Input[j + i * 122] = 0;
                        }
                        else
                        {
                            SerialData.currentDataBuff_IO_Input[j + i * 122] = b1[j - 74];
                        }
                    }

                    SerialData.currentDataBuff_IO_Input[89 + i * 122] = 0x2F;
                    //设置短信内容
                    s1 = Convert.ToString(mNpoiLib.readToCell(mNpoiLib.sheet1, 1 + 6 * i, 4, NPOILibs.TYPE_string));
                    Message = Encoding.Unicode.GetBytes(s1);
                    if (string.IsNullOrEmpty(s1) == false)
                    {
                        //Message = Encoding.Default.GetBytes(s1);
                        Message = Encoding.Unicode.GetBytes(s1);

                        byte temp;
                        for (int m = 0; m < Message.Length; m += 2)
                        {
                            temp = Message[m];
                            Message[m] = Message[m + 1];
                            Message[m + 1] = temp;
                        }

                    }
                    else
                    {
                        Array.Clear(Message, 0, Message.Length);
                    }
                    int k = Message.Length;
                    for (int j = 90; j < 125; j++)
                    {
                        if (j - 90 >= Message.Length)
                        {
                            SerialData.currentDataBuff_IO_Input[j + i * 122] = 0;
                        }
                        else
                        {
                            SerialData.currentDataBuff_IO_Input[j + i * 122] = Message[j - 90];
                        }
                    }
                    SerialData.currentDataBuff_IO_Input[125 + i * 122] = 0x3B;
                }


                for (int i = 0; i < 8; i++)
                {
                    if (SerialData.currentDataBuff_IO_Input[5 + i * 122] == 1)
                    {
                        checkedListBoxInput_IO_Setting.SetItemCheckState(i, CheckState.Checked);
                    }
                    else
                    {
                        checkedListBoxInput_IO_Setting.SetItemCheckState(i, CheckState.Unchecked);
                    }
                }

                //显示当前索引的内容
                clb_io_select_new = checkedListBoxInput_IO_Setting.SelectedIndex;
                if ((clb_io_select_new >= 8) || (clb_io_select_new < 0))
                {
                    clb_io_select_new = 0;
                }
                clb_io_select_old = clb_io_select_new;
                DisplayCurrentSelectedIndex(clb_io_select_new);

                //从EXCEL表格中获取PIN3 Falling的对应信息
                s1 = Convert.ToString(mNpoiLib.readToCell(mNpoiLib.sheet1, 49, 5, NPOILibs.TYPE_string));
                //if (s1.Equals("回復"))
                if(s1 == "回復")
                {
                    cb_IsAckMessage.Checked = true;
                }
                else
                {
                    cb_IsAckMessage.Checked = false;
                }

                //设置5-8脚对应的白名单号码  
                char[] TempChar_BindingPhoneNum = new char[15];
                string[] OutPutControl_BindingPhoneNum = new string[5];
                byte OutPutControl_BindingPhoneNumCount = 0;
                OutPutControl_BindingPhoneNumCount = 0;
                for (byte i = 1; i < 6; i++)
                {
                    s1 = Convert.ToString(mNpoiLib.readToCell(mNpoiLib.sheet1, i + 48, 3, NPOILibs.TYPE_string));
                    if (string.IsNullOrEmpty(s1) == false)
                    {
                        OutPutControl_BindingPhoneNum[i - 1] = s1;
                        OutPutControl_BindingPhoneNumCount = i;
                    }
                    else
                    {
                        break;
                    }
                }

                for (int i = 0; i < OutPutControl_BindingPhoneNumCount; i++)
                {
                    //if (OutPutControl_BindingPhoneNum[i].Equals("") == false)
                    if (OutPutControl_BindingPhoneNum[i] != "")
                    {
                        lb_phonenumber_show2.Items.Add(OutPutControl_BindingPhoneNum[i]);
                    }
                }

                s1 = Convert.ToString(mNpoiLib.readToCell(mNpoiLib.sheet1, 51, 6, NPOILibs.TYPE_string));
                //if (s1.Equals("High"))
                if(s1 == "High")
                {
                    cb_Pin6_State.Text = "High";
                }
                else
                {
                    cb_Pin6_State.Text = "Low";
                }
                s1 = Convert.ToString(mNpoiLib.readToCell(mNpoiLib.sheet1, 52, 6, NPOILibs.TYPE_string));
                //if (s1.Equals("High"))
                if (s1 == "High")
                {
                    cb_Pin7_State.Text = "High";
                }
                else
                {
                    cb_Pin7_State.Text = "Low";
                }
                s1 = Convert.ToString(mNpoiLib.readToCell(mNpoiLib.sheet1, 53, 6, NPOILibs.TYPE_string));
                //if (s1.Equals("High"))
                if (s1 == "High")
                {
                    cb_Pin8_State.Text = "High";
                }
                else
                {
                    cb_Pin8_State.Text = "Low";
                }

            });
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
