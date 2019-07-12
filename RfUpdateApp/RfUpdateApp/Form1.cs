using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RfUpdateApp
{
    public partial class Form1 : Form
    {
        public enum UpdateStatusEnum : byte
        {
            CanBeUpdated = 0,               //可以更新
            UpdateSuccessful,               //更新成功
            UpdateFailed,                   //更新失败
            UpdateDisableOrNoBin,          //dongle没使能更新或不存在固件
            VersionSame,                    //版本号相同
            IllegallyFirmware,              //非法固件
            NotSameWithNeedUpdateVersion,   //设备版本和指定的需要更新的版本不一样
            FirmwareTooLarge,               //固件太大
        }

        public class ImageMarkConf
        {
            public UInt32 ImageMark;
            public string Type;

            public ImageMarkConf(UInt32 imageMark, string type)
            {
                ImageMark = imageMark;
                Type = type;
            }
        }

        public class DeviceUpdateConf
        {
            public byte[] Id = new byte[2];
            public byte[] Version = new byte[4];
            public UpdateStatusEnum UpdateStatus;
            public string Type = "Unknown";
        }

        public const UInt32 DeviceBinMaxSize = 80 * 1024;
        public const UInt32 DongleBinMaxSize = 40 * 1024;

        public List<ImageMarkConf> DeviceImageMarkConf = new List<ImageMarkConf> { 
        new ImageMarkConf(0x12121212, "Locate_Anchor-V1"), new ImageMarkConf(0x34343434, "Locate_TD-V1"),
        new ImageMarkConf(0x80808080, "Locate_Tag_UTAG-9056-V1.3"),new ImageMarkConf(0x81818181, "Locate_Tag_UTAG-8141-V1.3/V1.4"), new ImageMarkConf(0x82828282, "Locate_Tag_UTAG-7045-V1.4/V2.0"), 
        new ImageMarkConf(0x83838383, "Locate_Tag_UTAG-H02-V1.2"),new ImageMarkConf(0x84848484, "Locate_Tag_UTAG-9056-V1.01"),  new ImageMarkConf(0x85858585, "Locate_Tag_UTAG-5136-V1.1/V1.3"), 
        new ImageMarkConf(0x86868686, "Locate_Tag_UTAG-9060WPC-V1.2"),new ImageMarkConf(0x87878787, "Locate_Tag_UTAG-H03-V1.0"), new ImageMarkConf(0x88888888, "Locate_Tag_UTAG-M02-V1.0/Locate_Tag_UTAG-4937-V1.0"),
        new ImageMarkConf(0x89898989, "Locate_Tag_UTAG-7045-V2.2/V2.3/V3.0"),new ImageMarkConf(0x8A8A8A8A, "Locate_Tag_UTAG-9056-V2.1/V2.2"),
        
        new ImageMarkConf(0x19011220, "India_NormalAlarmCar_UTAG-7045-V2.2/V2.3/V3.0"),
        new ImageMarkConf(0x19011221, "India_NormalAlarmCar_UTAG-SL90-V1.0/V1.2"),
        new ImageMarkConf(0x19011240, "India_RelayAlarmCar_UTAG-7045-V2.2/V2.3/V3.0"),
        new ImageMarkConf(0x19011260, "India_PersonCounter_Anchor-V1"),
        new ImageMarkConf(0x19011280, "India_Tag_UTAG-5136-V1.1/V1.3"),

        new ImageMarkConf(0x19012520, "Alarm_FixedAlarm_Anchor-V1"),
        new ImageMarkConf(0x19012521, "Alarm_FixedAlarm_UTAG-SL90-V1.0/V1.2"),
        new ImageMarkConf(0x19012540, "Alarm_ForkliftAlarm_Anchor-V1"),
        new ImageMarkConf(0x19012541, "Alarm_ForkliftAlarm_UTAG-7045-V1.4/V2.0"),
        new ImageMarkConf(0x19012542, "Alarm_ForkliftAlarm_UTAG-7045-V2.2/V2.3/V3.0"),
        new ImageMarkConf(0x19012543, "Alarm_ForkliftAlarm_UTAG-SL90-V1.0/V1.2"),
        new ImageMarkConf(0x19012560, "Alarm_Tag_UTAG-5136-V1.0"),
        new ImageMarkConf(0x19012561, "Alarm_Tag_UTAG-5136-V1.1/V1.3"),
        new ImageMarkConf(0x19012562, "Alarm_Tag_UTAG-9056-V1.3"),
        new ImageMarkConf(0x19012563, "Alarm_Tag_UTAG-9056-V2.1/V2.2"),
        new ImageMarkConf(0x19012564, "Alarm_Tag_UTAG-H02-V1.2"),
        };

        public List<ImageMarkConf> DongleImageMarkConf = new List<ImageMarkConf> {
        new ImageMarkConf(0x70707070, "Locate_Dongle_UWB-USB-01-V01.00"), 
        new ImageMarkConf(0x19011200, "India_Dongle_UWB-USB-01-V01.00"), 
        new ImageMarkConf(0x19012500, "Alarm_Dongle_UWB-USB-01-V01.00"), 
        };

        public delegate void DeMessageBox(Form form, string text, string title);
        public DeMessageBox EvMessageBox;

        public MySerialPort MySerial = new MySerialPort();

        public byte[] BinBuffer = new byte[100 * 1024];
        public UInt32 BinSize = 0;
        public UInt32 BinImageMark = 0;
        public UInt32 BinVersion = 0;
        public UInt32 BinCheckSum = 0;
        public string BinType = "";
        bool IsDongleImage = false;                     //固件是否为dongle固件            
        public UInt32 NeedUpdateVersion = 0;            //为0表示所有版本都需要更新
        public byte UpdateProgressPercent = 0;          //更新进度百分比
        public string UpdateProgress = "";              //更新进度

        private Thread ThUpdate = null;

        public delegate void DeFlush();
        public DeFlush EvUpdateProgressFlush;
        public DeFlush EvLvDeviceFlush;

        private object LockUpdateProgressPercent = new object();
        private object LockDeviceUpdateConfList = new object();

        List<DeviceUpdateConf> DeviceUpdateConfList = new List<DeviceUpdateConf>();

        DoubleBufferListView doubleBufferLvDevice = new DoubleBufferListView();
        private int LvDeviceSelectIndex = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnOpenFile.Enabled = false;
            btnStartUpdate.Enabled = false;
            gbDongle.Enabled = false;

            lbBinSize.Text = "";
            lbBinType.Text = "";
            lbBinVersion.Text = "";
            
            pbDongleUpdate.Maximum = 100;
            pbDongleUpdate.Minimum = 0;
            pbDongleUpdate.Value = 0;
            lbPercent.Text = "0%";
            lbUpdateProgress.Text = "";

            doubleBufferLvDevice.FullRowSelect = true;
            doubleBufferLvDevice.Location = new System.Drawing.Point(12, 408);
            doubleBufferLvDevice.Name = "doubleBufferLvDevice";
            doubleBufferLvDevice.Size = new System.Drawing.Size(593, 242);
            doubleBufferLvDevice.UseCompatibleStateImageBehavior = false;
            doubleBufferLvDevice.View = System.Windows.Forms.View.Details;
            doubleBufferLvDevice.Scrollable = true;
            doubleBufferLvDevice.GridLines = true;
            doubleBufferLvDevice.Clear();
            doubleBufferLvDevice.Columns.Add("Device ID", 80, System.Windows.Forms.HorizontalAlignment.Center);
            doubleBufferLvDevice.Columns.Add("Type", 150, System.Windows.Forms.HorizontalAlignment.Center);
            doubleBufferLvDevice.Columns.Add("Firmware Version", 150, System.Windows.Forms.HorizontalAlignment.Center);
            doubleBufferLvDevice.Columns.Add("Update Status", 150, System.Windows.Forms.HorizontalAlignment.Center);
            ImageList i1 = new ImageList();
            //设置高度
            i1.ImageSize = new Size(1, 20);
            //绑定listView控件
            doubleBufferLvDevice.SmallImageList = i1;
            doubleBufferLvDevice.ItemSelectionChanged += lvDevice_ItemSelectionChanged;
            this.Controls.Add(this.doubleBufferLvDevice);
            doubleBufferLvDevice.Enabled = false;

            foreach (string name in MySerialPort.GetPortName())
            {
                cbCom.Items.Add(name);
            }
            if (cbCom.Items.Count == 0)
            {
                cbCom.Text = "";
            }
            else
            {
                //如果父类中装的是子类的对象，则可以转换
                cbCom.Text = cbCom.Items[0] as string;
            }

            MySerial.SerialReceive += MySerial_SerialReceive;

            EvMessageBox = new DeMessageBox(MyMessageBox);
            EvUpdateProgressFlush = new DeFlush(UpdateProgressFlush);
            EvLvDeviceFlush = new DeFlush(LvDeviceFlush);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ThUpdate != null)
            {
                ThUpdate.Abort();
            }
        }

        public void MyMessageBox(Form form, string text, string title)
        {
            MessageBox.Show(form, text, title);
        }

        private void btnComRefresh_Click(object sender, EventArgs e)
        {
            cbCom.Items.Clear();
            foreach (string name in MySerialPort.GetPortName())
            {
                cbCom.Items.Add(name);
            }
            if (cbCom.Items.Count == 0)
            {
                cbCom.Text = "";
            }
            else
            {
                //如果父类中装的是子类的对象，则可以转换
                cbCom.Text = cbCom.Items[0] as string;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text == "Connect")
            {
                MySerial.MyPortName = cbCom.Text;
                MySerial.MyBaudRates = SerialPortBaudRates.BaudRate_115200;
                MySerial.MyDataBits = SerialPortDataBits.EightBits;
                MySerial.MyParity = Parity.None;
                MySerial.MyStopBits = StopBits.One;

                if (MySerial.OpenPort() == true)
                {
                    btnConnect.Text = "Disconnect";
                    cbCom.Enabled = false;
                    btnComRefresh.Enabled = false;

                    btnOpenFile.Enabled = true;
                    gbDongle.Enabled = true;

                    tbDongleStatus.Text = "";
                    tbFile.Text = "";

                    lbBinSize.Text = "";
                    lbBinType.Text = "";
                    lbBinVersion.Text = "";
                    lbPercent.Text = "0%";
                    lbUpdateProgress.Text = "";
                    pbDongleUpdate.Value = 0;

                    MySerial.ListDataFormat.Clear();
                    MySerial.ListDataFormat.Add(new DataFormat(0xE9, 0x9E, 0x01, 5));
                    MySerial.ListDataFormat.Add(new DataFormat(0xE9, 0x9E, 0x02, 7));
                    MySerial.ListDataFormat.Add(new DataFormat(0xE9, 0x9E, 0x03, 5));
                    MySerial.ListDataFormat.Add(new DataFormat(0xE9, 0x9E, 0x21, 15));
                    MySerial.ListDataFormat.Add(new DataFormat(0xE9, 0x9E, 0x22, 15));
                    MySerial.ListDataFormat.Add(new DataFormat(0xE9, 0x9E, 0x41, 26));
                    MySerial.ListDataFormat.Add(new DataFormat(0xE9, 0x9E, 0x42, 4));
                }
                else
                {
                    MessageBox.Show(this, "A serial port open failure!", "Attention");
                }
            }
            else
            {
                if (MySerial.ClosePort() == true)
                {
                    if (ThUpdate != null)
                    {
                        ThUpdate.Abort();
                    }

                    btnConnect.Text = "Connect";
                    cbCom.Enabled = true;
                    btnComRefresh.Enabled = true;

                    btnOpenFile.Enabled = false;
                    btnStartUpdate.Enabled = false;
                    gbDongle.Enabled = false;
                    doubleBufferLvDevice.Enabled = false;
                }
                else
                {
                    MessageBox.Show(this, "A serial port close failure!", "Attention");
                }
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select the hex file or bin file to open";
            ofd.Multiselect = false;
            ofd.Filter = "hex file|*.hex|bin file|*.bin";
            ofd.ShowDialog();

            if (ofd.FileName != "")
            {
                bool index = false;
                string ext = Path.GetExtension(ofd.FileName);
                if (ext == ".bin")
                {
                    using (FileStream fsRead = new FileStream(ofd.FileName, FileMode.OpenOrCreate, FileAccess.Read))
                    {
                        int realRead = fsRead.Read(BinBuffer, 0, BinBuffer.Length);
                        if (realRead > DeviceBinMaxSize || realRead == 0)
                        {
                            MessageBox.Show(this, "The bin file is too large or the bin file is empty!", "Attention");
                            lbBinType.Text = "";
                            lbBinVersion.Text = "";
                            lbBinSize.Text = "";
                            tbFile.Text = "";
                            btnStartUpdate.Enabled = false;
                            return;
                        }
                        index = true;
                        BinSize = (UInt32)realRead;
                    }
                }
                else if (ext == ".hex")
                {
                    using (FileStream fsRead = new FileStream(ofd.FileName, FileMode.OpenOrCreate, FileAccess.Read))
                    {
                        //hex文件最大不能超过DeviceBinMaxSize*4K
                        byte[] buffer = new byte[DeviceBinMaxSize * 4];
                        int realRead = fsRead.Read(buffer, 0, buffer.Length);
                        if (realRead == DeviceBinMaxSize * 4 || realRead == 0)
                        {
                            MessageBox.Show(this, "The hex file is too large or the hex file is empty!", "Attention");
                            lbBinType.Text = "";
                            lbBinVersion.Text = "";
                            lbBinSize.Text = "";
                            tbFile.Text = "";
                            btnStartUpdate.Enabled = false;
                            return;
                        }
                        HexToBin hexToBin = new HexToBin();
                        ConvertStatus res = hexToBin.HexFileToBinFile(Encoding.Default.GetString(buffer), ref BinBuffer, ref BinSize);
                        if (res == ConvertStatus.ConvertOk)
                        {
                            index = true;
                        }
                    }
                }
                if (index == true)
                {
                    int imageMarkNum = -1;
                    UInt32 imageMark = ((UInt32)BinBuffer[8195] << 24) + ((UInt32)BinBuffer[8194] << 16) + ((UInt32)BinBuffer[8193] << 8) + ((UInt32)BinBuffer[8192]);
                    //Console.WriteLine("imageMark" + imageMark);
                    for (int i = 0; i < DeviceImageMarkConf.Count; i++)
                    {
                        if (imageMark == DeviceImageMarkConf[i].ImageMark)
                        {
                            imageMarkNum = i;
                            IsDongleImage = false;
                            break;
                        }
                    }
                    if (imageMarkNum == -1)
                    {
                        for (int i = 0; i < DongleImageMarkConf.Count; i++)
                        {
                            if (imageMark == DongleImageMarkConf[i].ImageMark)
                            {
                                imageMarkNum = i;
                                IsDongleImage = true;
                                break;
                            }
                        }
                    }
                    if (imageMarkNum != -1)
                    {
                        UInt32 maxBinSize = 0;
                        if (IsDongleImage == true)
                        {
                            maxBinSize = DongleBinMaxSize;
                        }
                        else
                        {
                            maxBinSize = DeviceBinMaxSize;
                        }

                        if (BinSize > maxBinSize)
                        {
                            MessageBox.Show(this, "The file is too large!", "Attention");
                            lbBinType.Text = "";
                            lbBinVersion.Text = "";
                            lbBinSize.Text = "";
                            tbFile.Text = "";
                            btnStartUpdate.Enabled = false; ;
                            return;
                        }

                        BinImageMark = imageMark;
                        BinVersion = ((UInt32)BinBuffer[8199] << 24) + ((UInt32)BinBuffer[8198] << 16) + ((UInt32)BinBuffer[8197] << 8) + ((UInt32)BinBuffer[8196]);
                        //Console.WriteLine("BinVersion" +((byte)(BinVersion>>24)).ToString("X2"));
                        if (IsDongleImage == true)
                            BinType = DongleImageMarkConf[imageMarkNum].Type;
                        else
                            BinType = DeviceImageMarkConf[imageMarkNum].Type;
                        lbBinType.Text = BinType;
                        lbBinVersion.Text = "20" + ((byte)(BinVersion >> 24)).ToString("x2") + "-" + ((byte)(BinVersion >> 16)).ToString("x") + "-" + ((byte)(BinVersion >> 8)).ToString("x") +
                                            " V" + ((byte)BinVersion).ToString("x2").Substring(0, 1).ToUpper() + "." + ((byte)BinVersion).ToString("x2").Substring(1, 1).ToUpper();
                        lbBinSize.Text = ((double)BinSize / 1000.0).ToString("f3") + "KB";
                    }
                    else
                    {
                        MessageBox.Show(this, "Illegally firmware!", "Attention");
                        lbBinType.Text = "";
                        lbBinVersion.Text = "";
                        lbBinSize.Text = "";
                        tbFile.Text = "";
                        btnStartUpdate.Enabled = false;
                        return;
                    }
                   
                    //获取bin文件的校验和
                    BinCheckSum = 0;
                    for (int i = 0; i < BinSize; i++)
                    {
                        //Console.WriteLine("BinBuffer["+i+"]" + BinBuffer[i]);
                        BinCheckSum += BinBuffer[i];
                    }

                    tbFile.Text = "";
                    tbFile.Text = ofd.FileName;
                    btnStartUpdate.Enabled = true;
                }
                else
                {
                    MessageBox.Show(this, "Illegally firmware!", "Attention");
                    lbBinType.Text = "";
                    lbBinVersion.Text = "";
                    lbBinSize.Text = "";
                    tbFile.Text = "";
                    btnStartUpdate.Enabled = false;
                    return;
                }
            }
        }

        void MySerial_SerialReceive(object sender, MySerialPort.SerialReceiveEventArgs e)
        {
            if (btnStartUpdate.Text == "Stop Update" && IsDongleImage == false)
            {
                if (e.DataLen >= 4 && e.DataBuf[0] == 0xE9 && (e.DataBuf[1] == 0x21 || e.DataBuf[1] == 0x22) && e.DataBuf[e.DataLen - 1] == 0x9E)
                {
                    byte sum = 0;
                    for (int i = 0; i < e.DataLen - 2; i++)
                    {
                        sum += e.DataBuf[i];
                    }
                    if (sum == e.DataBuf[e.DataLen - 2])
                    {
                        //dongle上传设备上报消息
                        if (e.DataLen == 15 && e.DataBuf[1] == 0x21 && ((UInt32)e.DataBuf[5] << 24) + ((UInt32)e.DataBuf[6] << 16) + ((UInt32)e.DataBuf[7] << 8) + ((UInt32)e.DataBuf[8]) == BinImageMark)
                        {
                            bool isExist = false;
                            lock (LockDeviceUpdateConfList)
                            {
                                foreach (DeviceUpdateConf item in DeviceUpdateConfList)
                                {
                                    if (item.Id[0] == e.DataBuf[2] && item.Id[1] == e.DataBuf[3])
                                    {
                                        isExist = true;
                                        //信息与之前的一样，无需刷新列表
                                        if (item.Version[0] != e.DataBuf[9] || item.Version[1] != e.DataBuf[10] ||
                                            item.Version[2] != e.DataBuf[11] || item.Version[3] != e.DataBuf[12] || e.DataBuf[4] != (byte)item.UpdateStatus)
                                        {
                                            item.Version[0] = e.DataBuf[9];
                                            item.Version[1] = e.DataBuf[10];
                                            item.Version[2] = e.DataBuf[11];
                                            item.Version[3] = e.DataBuf[12];
                                            item.UpdateStatus = (UpdateStatusEnum)e.DataBuf[4];
                                            BeginInvoke(EvLvDeviceFlush);
                                        }
                                        break;
                                    }
                                }
                                if (isExist == false)
                                {
                                    DeviceUpdateConf deviceUpdateConf = new DeviceUpdateConf();
                                    deviceUpdateConf.Id[0] = e.DataBuf[2];
                                    deviceUpdateConf.Id[1] = e.DataBuf[3];
                                    deviceUpdateConf.UpdateStatus = (UpdateStatusEnum)e.DataBuf[4];
                                    deviceUpdateConf.Version[0] = e.DataBuf[9];
                                    deviceUpdateConf.Version[1] = e.DataBuf[10];
                                    deviceUpdateConf.Version[2] = e.DataBuf[11];
                                    deviceUpdateConf.Version[3] = e.DataBuf[12];
                                    deviceUpdateConf.Type = lbBinType.Text;

                                    DeviceUpdateConfList.Add(deviceUpdateConf);
                                    BeginInvoke(EvLvDeviceFlush);
                                }
                            }
                        }
                        //dongle上传设备更新状态
                        else if (e.DataLen == 15 && e.DataBuf[1] == 0x22 && ((UInt32)e.DataBuf[5] << 24) + ((UInt32)e.DataBuf[6] << 16) + ((UInt32)e.DataBuf[7] << 8) + ((UInt32)e.DataBuf[8]) == BinImageMark)
                        { 
                            bool isExist = false;
                            lock (LockDeviceUpdateConfList)
                            {
                                foreach (DeviceUpdateConf item in DeviceUpdateConfList)
                                {
                                    if (item.Id[0] == e.DataBuf[2] && item.Id[1] == e.DataBuf[3])
                                    {
                                        isExist = true;
                                        item.Version[0] = e.DataBuf[9];
                                        item.Version[1] = e.DataBuf[10];
                                        item.Version[2] = e.DataBuf[11];
                                        item.Version[3] = e.DataBuf[12];
                                        if (e.DataBuf[4] == 0)
                                            item.UpdateStatus = UpdateStatusEnum.UpdateSuccessful;
                                        else
                                            item.UpdateStatus = UpdateStatusEnum.UpdateFailed;
                                        BeginInvoke(EvLvDeviceFlush);
                                        
                                        break;
                                    }
                                }
                                if (isExist == false)
                                {
                                    DeviceUpdateConf deviceUpdateConf = new DeviceUpdateConf();
                                    deviceUpdateConf.Id[0] = e.DataBuf[2];
                                    deviceUpdateConf.Id[1] = e.DataBuf[3];
                                    deviceUpdateConf.Version[0] = e.DataBuf[9];
                                    deviceUpdateConf.Version[1] = e.DataBuf[10];
                                    deviceUpdateConf.Version[2] = e.DataBuf[11];
                                    deviceUpdateConf.Version[3] = e.DataBuf[12];

                                    if (e.DataBuf[4] == 0)
                                        deviceUpdateConf.UpdateStatus = UpdateStatusEnum.UpdateSuccessful;
                                    else
                                        deviceUpdateConf.UpdateStatus = UpdateStatusEnum.UpdateFailed;

                                    deviceUpdateConf.Type = lbBinType.Text;
                                    
                                    DeviceUpdateConfList.Add(deviceUpdateConf);
                                    BeginInvoke(EvLvDeviceFlush);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnStartUpdate_Click(object sender, EventArgs e)
        {
            if (btnStartUpdate.Text == "Start Update")
            {
                try
                {
                    if (tbNeedUpdateVersion.Text == "")
                        NeedUpdateVersion = 0;
                    else
                    {
                        NeedUpdateVersion = Convert.ToUInt32(tbNeedUpdateVersion.Text, 16);
                        tbNeedUpdateVersion.Text = NeedUpdateVersion.ToString("x08");
                    }
                    //指定需要更新的版本与固件版本相同，返回错误
                    if (NeedUpdateVersion == BinVersion)
                    {
                        BeginInvoke(EvMessageBox, this, "NeedUpdateVersion can not be the same as Firmware Version!", "Attention"); 
                        return;
                    }
                }
                catch
                {
                    BeginInvoke(EvMessageBox, this, "Need Update Version input format is wrong!", "Attention");
                    return;
                }

                if (IsDongleImage == false)
                {
                    doubleBufferLvDevice.Enabled = true;
                }

                btnConnect.Enabled = false;
                btnOpenFile.Enabled = false;
                tbNeedUpdateVersion.Enabled = false;
                cbAskToUpdate.Enabled = false;
                btnStopUpdate.Enabled = false;
                btnStartUpdate.Text = "Stop Update";

                if (ThUpdate != null)
                {
                    ThUpdate.Abort();
                }
                ThUpdate = new Thread(FirmwareUpdate);
                ThUpdate.IsBackground = false;
                ThUpdate.Start(); 
            }
            else
            {
                
                if (ThUpdate != null)
                {
                    ThUpdate.Abort();
                }
                bool isStopUpdate = false;
                //更新的是设备的固件，手动停止dongle更新
                if (IsDongleImage == false)
                {
                    isStopUpdate = DongleStopUpdate();
                }
                else
                {
                    isStopUpdate = true;
                }

                if (isStopUpdate == true)
                {
                    lock (LockUpdateProgressPercent)
                    {
                        UpdateProgress = "Update has stopped...";
                    }
                    BeginInvoke(EvUpdateProgressFlush);
                }

                doubleBufferLvDevice.Enabled = false;
                btnConnect.Enabled = true;
                btnOpenFile.Enabled = true;
                tbNeedUpdateVersion.Enabled = true;
                cbAskToUpdate.Enabled = true;
                btnStopUpdate.Enabled = true;
                btnStartUpdate.Text = "Start Update"; 
            }
        }

        private void UpdateProgressFlush()
        {
            lock (LockUpdateProgressPercent)
            {
                if (UpdateProgressPercent <= 100)
                {
                    pbDongleUpdate.Value = UpdateProgressPercent;
                    lbPercent.Text = UpdateProgressPercent.ToString() + "%";
                }
                lbUpdateProgress.Text = UpdateProgress;
            }
        }

        private void LvDeviceFlush()
        {
            int deviceSuccessNum = 0;
            int deviceFailNum = 0;
            
            lock (LockDeviceUpdateConfList)
            {
                List<DeviceUpdateConf> deviceUpdateConfList = new List<DeviceUpdateConf>();

                foreach (DeviceUpdateConf item in DeviceUpdateConfList)
                {
                    deviceUpdateConfList.Add(item);
                }

                if (deviceUpdateConfList.Count > 0)
                {
                    for (int i = deviceUpdateConfList.Count - 1; i > 0; i--)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            if ((UInt16)(((UInt16)deviceUpdateConfList[j].Id[0] << 8) + deviceUpdateConfList[j].Id[1]) > (UInt16)(((UInt16)deviceUpdateConfList[j + 1].Id[0] << 8) + deviceUpdateConfList[j + 1].Id[1]))
                            {
                                DeviceUpdateConf deviceUpdateConf = deviceUpdateConfList[j];
                                deviceUpdateConfList[j] = deviceUpdateConfList[j + 1];
                                deviceUpdateConfList[j + 1] = deviceUpdateConf;
                            }
                        }
                    }
                }

                int index = 0;
                try
                {
                    index = doubleBufferLvDevice.TopItem.Index;            //获取第一行的索引
                }
                catch
                { }

                doubleBufferLvDevice.BeginUpdate();
                doubleBufferLvDevice.Items.Clear();

                foreach (DeviceUpdateConf item in deviceUpdateConfList)
                {
                    ListViewItem lv = new ListViewItem();
                    lv.Text = item.Id[0].ToString("x2").ToUpper() + item.Id[1].ToString("x2").ToUpper();
                    lv.SubItems.Add(item.Type);
                    lv.SubItems.Add("20" + item.Version[0].ToString("x2") + "-" + item.Version[1].ToString("x") + "-" +
                                    item.Version[2].ToString("x") + " V" + item.Version[3].ToString("x2").Substring(0, 1) + "." + item.Version[3].ToString("x2").Substring(1, 1));
                    switch (item.UpdateStatus)
                    {
                        case UpdateStatusEnum.CanBeUpdated: lv.SubItems.Add("No Update"); break;
                        case UpdateStatusEnum.UpdateSuccessful: lv.SubItems.Add("Update Successful"); break;
                        case UpdateStatusEnum.UpdateFailed: lv.SubItems.Add("Update Failed"); break;
                        case UpdateStatusEnum.UpdateDisableOrNoBin: lv.SubItems.Add("Update Disable Or No Firmware"); break;
                        case UpdateStatusEnum.VersionSame: lv.SubItems.Add("Version Same"); break;
                        case UpdateStatusEnum.IllegallyFirmware: lv.SubItems.Add("Illegally Firmware"); break;
                        case UpdateStatusEnum.NotSameWithNeedUpdateVersion: lv.SubItems.Add("Not Same With NeedUpdateVersion"); break;
                        case UpdateStatusEnum.FirmwareTooLarge: lv.SubItems.Add("Firmware Too Large"); break;
                        default: lv.SubItems.Add("Unknown Status"); break;            
                    }
                    this.doubleBufferLvDevice.Items.Add(lv);
                    if(item.UpdateStatus ==  UpdateStatusEnum.UpdateSuccessful || item.UpdateStatus == UpdateStatusEnum.VersionSame)
                        deviceSuccessNum ++;
                    else
                        deviceFailNum ++;
                }

                try
                {
                    Point a = doubleBufferLvDevice.Items[0].Position;
                    Point b = doubleBufferLvDevice.Items[1].Position;      //（利用用户区的高度-表头高度）/ 行高 - 1 = 需要显示的项
                    Size size = doubleBufferLvDevice.ClientSize;
                    doubleBufferLvDevice.Items[index + (size.Height - a.Y) / (b.Y - a.Y) - 1].EnsureVisible();
                }
                catch { }
                try
                {
                    //如果之前选中某一项，则继续选中
                    if (LvDeviceSelectIndex >= 0)
                    {
                        doubleBufferLvDevice.Items[LvDeviceSelectIndex].Selected = true;
                        doubleBufferLvDevice.Items[LvDeviceSelectIndex].Focused = true;
                    }
                }
                catch { }

                doubleBufferLvDevice.EndUpdate();
                
                lbDeviceNum.Text = deviceUpdateConfList.Count.ToString();
                lbDeviceSuccessNum.Text = deviceSuccessNum.ToString();
                lbDeviceFailNum.Text = deviceFailNum.ToString();
            }
        }

        private void FirmwareUpdate()
        {
            MySerial.ClearSerialReceData();

            lock (LockUpdateProgressPercent)
            {
                if (IsDongleImage == true)
                {
                    UpdateProgress = "Dongle's firmware is ready for update...";
                }
                else
                {
                    UpdateProgress = lbBinType.Text + "'s firmware is ready for transmit...";
                }
                UpdateProgressPercent = 0;
            }
            BeginInvoke(EvUpdateProgressFlush);

            //更新设备固件前，清空列表
            if (IsDongleImage == false)
            {
                lock (LockDeviceUpdateConfList)
                {
                    DeviceUpdateConfList.Clear();
                }
                BeginInvoke(EvLvDeviceFlush);
            }

            //请求传输固件
            byte res = RequestTransFirmWare();  
            if (res == 0)
            {
                lock (LockUpdateProgressPercent)
                {
                    if (IsDongleImage == true) UpdateProgress = "Dongle's firmware is being updated...";
                    else UpdateProgress = lbBinType.Text + "'s firmware is being transmitted...";
                }
                BeginInvoke(EvUpdateProgressFlush);

                bool index = false;
                if (StartTransFirmWare() == true)
                {
                    if (TransFirmWareComplete() == true)
                    {
                        index = true;
                    }
                }
                lock (LockUpdateProgressPercent)
                {
                    if (index == true)
                    {
                        if (IsDongleImage == true) UpdateProgress = "Dongle's firmware update successful...";
                        else UpdateProgress = lbBinType.Text + "'s firmware transmit successful,"+ "\r\n" + "and it is being updated...";
                    }
                    else
                    {
                        if (IsDongleImage == true) UpdateProgress = "Dongle's firmware update failed !";
                        else UpdateProgress = lbBinType.Text + "'s firmware transmit failed !";
                    }
                }
                BeginInvoke(EvUpdateProgressFlush);
            }
            else
            {
                lock (LockUpdateProgressPercent)
                {
                    switch (res)
                    {
                        case 0x01:  if (IsDongleImage == true) UpdateProgress = "Version same. Dongle's firmware update successful...";
                                    else UpdateProgress = "Firmware already exists and it is being updated...";
                                    break;
                        case 0x02:  UpdateProgress = "Firmware too large";
                                    break;
                        case 0x03:  UpdateProgress = "No enable updates";
                                    break;
                        case 0x04:  UpdateProgress = "NeedUpdateVersion can not be the same as Firmware Version";
                                    break;
                        default:    UpdateProgress = "Unknown error";
                                    break;
                    }
                }
                BeginInvoke(EvUpdateProgressFlush);
            }
        }

        //请求传输固件给usb_dongle
        private byte RequestTransFirmWare()
        {
            byte[] sendBuf = new byte[128];
            List<byte> receList = new List<byte>();
            int txLen = 0;
            byte index = 0xFF;
            Console.WriteLine("BinImageMark:" + BinImageMark + "  BinVersion:" + BinVersion + "  BinSize:" + BinSize + "  BinCheckSum:" + BinCheckSum + "  NeedUpdateVersion:" + NeedUpdateVersion);
            sendBuf[txLen++] = 0xF9;
            sendBuf[txLen++] = 0x01;
            sendBuf[txLen++] = (byte)(BinImageMark >> 24);
            sendBuf[txLen++] = (byte)(BinImageMark >> 16);
            sendBuf[txLen++] = (byte)(BinImageMark >> 8);
            sendBuf[txLen++] = (byte)(BinImageMark);
            sendBuf[txLen++] = (byte)(BinVersion >> 24);
            sendBuf[txLen++] = (byte)(BinVersion >> 16);
            sendBuf[txLen++] = (byte)(BinVersion >> 8);
            sendBuf[txLen++] = (byte)(BinVersion);
            sendBuf[txLen++] = (byte)(BinSize >> 16);
            sendBuf[txLen++] = (byte)(BinSize >> 8);
            sendBuf[txLen++] = (byte)(BinSize);
            sendBuf[txLen++] = (byte)(BinCheckSum >> 24);
            sendBuf[txLen++] = (byte)(BinCheckSum >> 16);
            sendBuf[txLen++] = (byte)(BinCheckSum >> 8);
            sendBuf[txLen++] = (byte)(BinCheckSum);
            sendBuf[txLen++] = (byte)(NeedUpdateVersion >> 24);
            sendBuf[txLen++] = (byte)(NeedUpdateVersion >> 16);
            sendBuf[txLen++] = (byte)(NeedUpdateVersion >> 8);
            sendBuf[txLen++] = (byte)(NeedUpdateVersion);
            if (cbAskToUpdate.Checked == true)
                sendBuf[txLen++] = 0x55;
            else
                sendBuf[txLen++] = 0;
            sendBuf[txLen++] = 0x55;

            sendBuf[txLen] = 0;
            for (int i = 0; i < txLen; i++)
            {
                sendBuf[txLen] += sendBuf[i];
            }
            sendBuf[++txLen] = 0x9F;
            for (int i = 0; i < sendBuf.Length; i++)
            {
                Console.WriteLine(sendBuf[i]);

            }
            int sendCount = 0;
            for (sendCount = 0; sendCount < 5; sendCount++)
            {
                if (MySerial.WriteData(sendBuf, 0, txLen + 1) == true)
                {
                    int time = Environment.TickCount;
                    while (Environment.TickCount - time < 200)
                    {
                        if (MySerial.ReadData(ref receList) == true)
                        {
                            if (receList.Count == 5 && receList[0] == 0xE9 && receList[1] == 0x01 && receList[receList.Count - 1] == 0x9E)
                            {
                                byte sum = 0;
                                for (int i = 0; i < receList.Count - 2; i++)
                                {
                                    sum += receList[i];
                                }
                                if (sum == receList[receList.Count - 2])
                                {
                                    index = receList[2];
                                    sendCount = 100;
                                    break;
                                }
                            }
                        }
                        Thread.Sleep(1);
                    }
                }
            }

            return index;
        }


        //开始传输固件
        private bool StartTransFirmWare()
        {
            byte[] sendBuf = new byte[256];
            List<byte> receList = new List<byte>();
            int txLen = 0;
            UInt32 addr = 0;
            int dataNum = 0;
            string readStr = "";
            while (addr < BinSize)
            {
                if (BinSize - addr > 128)
                    dataNum = 128;
                else
                    dataNum = (int)(BinSize - addr);

                txLen = 0;
                sendBuf[txLen++] = 0xF9;
                sendBuf[txLen++] = 0x02;
                sendBuf[txLen++] = (byte)(addr >> 16);
                sendBuf[txLen++] = (byte)(addr >> 8);
                sendBuf[txLen++] = (byte)(addr);
                for (int i = 0; i < dataNum; i++)
                {
                    sendBuf[txLen++] = BinBuffer[addr + i];
                }
                if (dataNum != 128)
                {
                    for (int i = 0; i < 128 - dataNum; i++)
                    {
                        sendBuf[txLen++] = 0xFF;
                    }
                }
                sendBuf[txLen] = 0;
                for (int i = 0; i < txLen; i++)
                {
                    sendBuf[txLen] += sendBuf[i];
                }
                sendBuf[++txLen] = 0x9F;

                string senStr = "";

                for (int i = 0; i < 131; i++)
                {
                    senStr += "|"+sendBuf[i+2];
                }
                Console.WriteLine("I/WirelessFirmwareUpdateActivity: addr:"+addr+"  sendTransFirmWareBuf:" + senStr);
                int sendCount = 0;
                for (sendCount = 0; sendCount < 5; sendCount++)
                {
                    if (MySerial.WriteData(sendBuf, 0, txLen + 1) == true)
                    {
                        int time = Environment.TickCount;
                        
                        while (Environment.TickCount - time < 200)
                        {
                            if (MySerial.ReadData(ref receList) == true)
                            {
                                //for (int j = 0; j < 7;j++ )
                                //{
                                //    readStr += "|"+receList[j];
                                //}
                                if (receList.Count == 7 && receList[0] == 0xE9 && receList[1] == 0x02 && ((UInt32)receList[2] << 16) + ((UInt32)receList[3] << 8) + ((UInt32)receList[4]) == addr && 
                                    receList[receList.Count - 1] == 0x9E)
                                {
                                    byte sum = 0;
                                    for (int i = 0; i < receList.Count - 2; i++)
                                    {
                                        sum += receList[i];
                                    }
                                    if (sum == receList[receList.Count - 2])
                                    {
                                        addr += 128;
                                        sendCount = 100;
                                        lock (LockUpdateProgressPercent)
                                        {
                                            if (UpdateProgressPercent != (addr * 100 / BinSize))
                                            {
                                                UpdateProgressPercent = (byte)(addr * 100 / BinSize);
                                            }
                                        }
                                        BeginInvoke(EvUpdateProgressFlush);
                                        break;
                                    }
                                }
                            }
                            Thread.Sleep(1);
                        }
                        
                    }
                }
                if (sendCount == 5) return false;
            }
            Console.WriteLine("readStr" + readStr);
            return true;
        }

        //固件传输完成
        private bool TransFirmWareComplete()
        {
            byte[] sendBuf = new byte[128];
            List<byte> receList = new List<byte>();
            int txLen = 0;
            byte index = 0xFF;

            sendBuf[txLen++] = 0xF9;
            sendBuf[txLen++] = 0x03;
            sendBuf[txLen++] = (byte)(BinImageMark >> 24);
            sendBuf[txLen++] = (byte)(BinImageMark >> 16);
            sendBuf[txLen++] = (byte)(BinImageMark >> 8);
            sendBuf[txLen++] = (byte)(BinImageMark);
            sendBuf[txLen++] = (byte)(BinVersion >> 24);
            sendBuf[txLen++] = (byte)(BinVersion >> 16);
            sendBuf[txLen++] = (byte)(BinVersion >> 8);
            sendBuf[txLen++] = (byte)(BinVersion);
            sendBuf[txLen++] = (byte)(BinSize >> 16);
            sendBuf[txLen++] = (byte)(BinSize >> 8);
            sendBuf[txLen++] = (byte)(BinSize);
            sendBuf[txLen++] = (byte)(BinCheckSum >> 24);
            sendBuf[txLen++] = (byte)(BinCheckSum >> 16);
            sendBuf[txLen++] = (byte)(BinCheckSum >> 8);
            sendBuf[txLen++] = (byte)(BinCheckSum);
            sendBuf[txLen++] = (byte)(NeedUpdateVersion >> 24);
            sendBuf[txLen++] = (byte)(NeedUpdateVersion >> 16);
            sendBuf[txLen++] = (byte)(NeedUpdateVersion >> 8);
            sendBuf[txLen++] = (byte)(NeedUpdateVersion);
            if (cbAskToUpdate.Checked == true)
                sendBuf[txLen++] = 0x55;
            else
                sendBuf[txLen++] = 0;
            sendBuf[txLen++] = 0x55;

            sendBuf[txLen] = 0;
            for (int i = 0; i < txLen; i++)
            {
                sendBuf[txLen] += sendBuf[i];
            }
            sendBuf[++txLen] = 0x9F;

            string sendBufString = "";
            for (int i = 0; i < 21;i++ )
            {
                sendBufString += "|"+sendBuf[i + 2];
            }

            Console.WriteLine("sendBufString"+sendBufString);
            int sendCount = 0;
            for (sendCount = 0; sendCount < 5; sendCount++)
            {
                if (MySerial.WriteData(sendBuf, 0, txLen + 1) == true)
                {
                    int time = Environment.TickCount;
                    while (Environment.TickCount - time < 200)
                    {
                        if (MySerial.ReadData(ref receList) == true)
                        {
                            if (receList.Count == 5 && receList[0] == 0xE9 && receList[1] == 0x03 && receList[receList.Count - 1] == 0x9E)
                            {
                                byte sum = 0;
                                for (int i = 0; i < receList.Count - 2; i++)
                                {
                                    sum += receList[i];
                                }
                                if (sum == receList[receList.Count - 2])
                                {
                                    index = receList[2];
                                    sendCount = 100;
                                    break;
                                }
                            }
                        }
                        Thread.Sleep(1);
                    }
                }
            }

            if (index == 0) return true;
            else return false;
        }

        private bool ReadDongleStatus()
        {
            byte[] sendBuf = new byte[128];
            List<byte> receList = new List<byte>();
            int txLen = 0;

            sendBuf[txLen++] = 0xF9;
            sendBuf[txLen++] = 0x41;
            sendBuf[txLen] = 0;
            for (int i = 0; i < txLen; i++)
            {
                sendBuf[txLen] += sendBuf[i];
            }

            //Console.WriteLine("CCS"+sendBuf[2]);
            sendBuf[++txLen] = 0x9F;

            int sendCount = 0;
            for (sendCount = 0; sendCount < 5; sendCount++)
            {
                if (MySerial.WriteData(sendBuf, 0, txLen + 1) == true)
                {
                    int time = Environment.TickCount;
                    while (Environment.TickCount - time < 200)
                    {
                        if (MySerial.ReadData(ref receList) == true)
                        {
                            if (receList.Count == 26 && receList[0] == 0xE9 && receList[1] == 0x41 && receList[receList.Count - 1] == 0x9E)
                            {
                                byte sum = 0;
                                for (int i = 0; i < receList.Count - 2; i++)
                                {
                                    sum += receList[i];
                                }
                                if (sum == receList[receList.Count - 2])
                                {
                                    tbDongleStatus.Text = "";
                                    tbDongleStatus.Text += ("Dongle's Firmware Version:" + receList[2].ToString("x02") + receList[3].ToString("x02") + receList[4].ToString("x02") + receList[5].ToString("x02") + "\r\n");
                                    tbDongleStatus.Text += "AskToUpdate En:";
                                    if (receList[6] == 0x55) tbDongleStatus.Text += "Enable";
                                    else tbDongleStatus.Text += "Disable";
                                    tbDongleStatus.Text += "    WaitForUpdate En:";
                                    if (receList[7] == 0x55) tbDongleStatus.Text += "Enable";
                                    else tbDongleStatus.Text += "Disable";
                                    tbDongleStatus.Text += "\r\n";
                                    if (receList[8] != 0x55)
                                    {
                                        tbDongleStatus.Text += ("No device firmware!!" + "\r\n");
                                    }
                                    else
                                    {
                                        tbDongleStatus.Text += "Firmware Type:";
                                        bool isExist = false;
                                        UInt32 imageMark = ((UInt32)receList[9] << 24) + ((UInt32)receList[10] << 16) + ((UInt32)receList[11] << 8) + ((UInt32)receList[12]);
                                        foreach (ImageMarkConf item in DeviceImageMarkConf)
                                        {
                                            if (item.ImageMark == imageMark)
                                            {
                                                tbDongleStatus.Text += item.Type;
                                                isExist = true;
                                                break;
                                            }
                                        }
                                        if (isExist == false)
                                        {
                                            tbDongleStatus.Text += "Unknown Type";
                                        }
                                        tbDongleStatus.Text += "\r\n";
                                        tbDongleStatus.Text += "Firmware Version:";
                                        tbDongleStatus.Text += (receList[13].ToString("x02") + receList[14].ToString("x02") + receList[15].ToString("x02") + receList[16].ToString("x02"));
                                        UInt32 binSize = ((UInt32)receList[17] << 16) + ((UInt32)receList[18] << 8) + ((UInt32)receList[19]);
                                        tbDongleStatus.Text += ("    " + "Firmware Size:" + ((double)binSize / 1000.0).ToString("f3") + "KB\r\n");
                                        if (receList[20] == 0 && receList[21] == 0 && receList[22] == 0 && receList[23] == 0)
                                        {
                                            tbDongleStatus.Text += ("Need Update Version:" + "None");
                                        }
                                        else
                                        {
                                            tbDongleStatus.Text += ("Need Update Version:" + receList[20].ToString("x02") + receList[21].ToString("x02") + receList[22].ToString("x02") + receList[23].ToString("x02"));
                                        }
                                    }

                                    sendCount = 100;
                                    break;
                                }
                            }
                        }
                        Thread.Sleep(1);
                    }
                }
            }
            if (sendCount == 5)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool DongleStopUpdate()
        {
            byte[] sendBuf = new byte[128];
            List<byte> receList = new List<byte>();
            int txLen = 0;

            sendBuf[txLen++] = 0xF9;
            sendBuf[txLen++] = 0x42;
            sendBuf[txLen] = 0;
            for (int i = 0; i < txLen; i++)
            {
                sendBuf[txLen] += sendBuf[i];
            }
            sendBuf[++txLen] = 0x9F;

            int sendCount = 0;
            for (sendCount = 0; sendCount < 5; sendCount++)
            {
                if (MySerial.WriteData(sendBuf, 0, txLen + 1) == true)
                {
                    int time = Environment.TickCount;
                    while (Environment.TickCount - time < 200)
                    {
                        if (MySerial.ReadData(ref receList) == true)
                        {
                            if (receList.Count == 4 && receList[0] == 0xE9 && receList[1] == 0x42 && receList[receList.Count - 1] == 0x9E)
                            {
                                byte sum = 0;
                                for (int i = 0; i < receList.Count - 2; i++)
                                {
                                    sum += receList[i];
                                }
                                if (sum == receList[receList.Count - 2])
                                {
                                    sendCount = 100;
                                    break;
                                }
                            }
                        }
                        Thread.Sleep(1);
                    }
                }
            }
            if (sendCount == 5)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnReadStatus_Click(object sender, EventArgs e)
        {
            if (ReadDongleStatus() == false)
            {
                tbDongleStatus.Text = "";
                BeginInvoke(EvMessageBox, this, "Dongle status read failed!", "Attention");
            }
        }

        private void btnStopUpdate_Click(object sender, EventArgs e)
        {
            if (DongleStopUpdate() == true)
            {
                lock (LockUpdateProgressPercent)
                {
                    UpdateProgress = "Update has stopped...";
                }
                BeginInvoke(EvUpdateProgressFlush);
                BeginInvoke(EvMessageBox, this, "Dongle Stop update successful!", "Attention");
                ReadDongleStatus();
            }
            else
            {
                BeginInvoke(EvMessageBox, this, "Dongle Stop update failed!", "Attention");
            }
            
        }

        private void lvDevice_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                ListViewItem lva = e.Item;
                LvDeviceSelectIndex = lva.Index;
            }
        }

        

    }

   
}
