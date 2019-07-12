using CiXinLocation.Model;
using CiXinLocation.Utils;
using SerialportSample;
using SerialPortSpace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CiXinLocation{

    public partial class FormBase : Form  {

        public bool fromClose = false;

        public FormBase()
        {            
            InitializeComponent();           
        }

        private void FormBase_Load(object sender, EventArgs e)
        {          
            getCommPorts();
            Disposed += OnRecordingPanelDisposed;
        }


        void OnRecordingPanelDisposed(object sender, EventArgs e)
        {
            fromClose = true;
            closeFromBaseModel();
            closeSerPortUtil();
        }

        public List<FromBaseModel> fbModels;
        private  SerialPortUtils serPort;
        public  bool isOpenCom = false; //打开串口了
        public string portName = "COM1";
        int baudRate = 115200;
        FromBaseModel selectfileModel;

        private void createSerPortUtil() {
            if (serPort != null) return;
            serPort = new SerialPortUtils();
            serPort.onDataReceived += revePortsData;
        }

        private void closeSerPortUtil()
        {
            if (serPort == null) return;
            serPort.onDataReceived -= revePortsData;
            serPort.closeComm();
            serPort = null;
        }


        public void createFromBaseModel(FromBaseModel fbModel)
        {
            addFromBaseModel(fbModel);
        }
         

        private bool isHaveModel(FromBaseModel fbModel)
        {
            foreach (FromBaseModel itemModel in fbModels)
            {
                if (fbModel.TAG().Equals(itemModel.TAG())) return true;
            }
            return false;
        }


        /// <summary>
        /// 添加一个Model对象
        /// </summary>
        /// <param name="fbModel"></param>
        /// <returns>添加失败</returns>
        public bool addFromBaseModel(FromBaseModel fbModel)
        {
            if (fbModels == null) {
                this.fbModels = new List<FromBaseModel>();
            }
            selectfileModel = fbModel;
            if (isHaveModel(fbModel)) return false;
            fbModel.sendDataHandle += sendPortsData;
            fbModels.Add(fbModel);
            return true;
        }

        /// <summary>
        /// 返回存储的MODEL，没有返回null
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FromBaseModel getFromBaseModel(string path)
        {
            if (fbModels == null || path == null || path.Length <= 1) return null;
            foreach (FromBaseModel itemModel in fbModels)
            {
                if (path.Equals(itemModel.TAG()))  {
                    selectfileModel = itemModel;
                    return itemModel;
                }
            }
            return null;
        }


        public void closeFromBaseModel() {
            if (fbModels == null ) return;
            for (int i = 0; i < fbModels.Count;i++ )
            {
                closeFromBaseModel(fbModels[i]);
            }
            fbModels.Clear();
        }

        public void closeFromBaseModel(FromBaseModel fbModel)
        {
            if (fbModel == null || fbModel.sendDataHandle == null) return;
            fbModel.sendDataHandle -= sendPortsData;
            fbModel.close();
            fbModels.Remove(fbModel);
        }

        public void closeFromBaseModel(string path) {
            closeFromBaseModel(getFromBaseModel(path));
        }

        public  void comOpenClose()
        { //串口打开或者关闭
            new Thread(comOpenCloseThread).Start();
        }


        public  void comOpenCloseThread()
        {
            createSerPortUtil();
            string msg = "%!%";
            if (!isOpenCom)
                msg = serPort.commOpen(portName, baudRate);
            else
                serPort.commClose();
            isOpenCom = serPort.comm.IsOpen;


            this.Invoke((EventHandler)(delegate{ //放入主線程
                commOverCallBack(msg);
            }));           
        }


        /// <summary>
        /// 獲取串口號，初始化下拉串口名称列表框
        /// </summary>
        public void getCommPorts() {
            createSerPortUtil();            
            string[] ports = serPort.getSerialPorts();
            commPortsCallBack(ports);
        }

        private void sendPortsData(byte[] buf) {
            if (serPort != null) new Thread(sendPortsDataThread).Start(buf);// serPort.commWriteByte(buf, 0, buf.Length);
        }

        private void sendPortsDataThread(object obj)
        {
            byte[] buf = (byte[])obj;
            if (serPort != null) serPort.commWriteByte(buf, 0, buf.Length);
        }
        /// <summary>
        /// 接收线程
        /// </summary>
        /// <param name="buf"></param>
        private void revePortsData(byte[] buf) {
            if (selectfileModel != null) selectfileModel.revePortsData(buf);

            foreach (FromBaseModel fbModel in fbModels) //遍历出主要的设置Model,去除不需要的更新的Model
            {
                if (fbModel == null ) continue;
                if (fbModel.TAG().Equals("MainFromModel")) {
                    if (!(selectfileModel != null && selectfileModel.TAG().Equals("MainFromModel")))
                        fbModel.revePortsData(buf);
                    return;
                } 
            }            
        }
 
        public void sendONEData() {
            byte[] buf = new byte[9] { 0x00, 0xFC, 0xE0, 0x11, 0x08, 0x0F, 0x12, 0x16, 0xFB };
            revePortsData(buf);
        }

        public virtual void commOverCallBack(string msg) { }
        public virtual void commPortsCallBack(string[] msg) { }
        public virtual void commRemoveCallBack() { }//检测到串口被拔出


       // usb消息定义
       // public const int WM_DEVICE_CHANGE = 0x219;
       // public const int DBT_DEVICEARRIVAL = 0x8000;
       // public const int DBT_DEVICE_REMOVE_COMPLETE = 0x8004;
        public const UInt32 DBT_DEVTYP_PORT = 0x00000003;

        [StructLayout(LayoutKind.Sequential)]
        struct DEV_BROADCAST_HDR
        {
            public UInt32 dbch_size;
            public UInt32 dbch_devicetype;
            public UInt32 dbch_reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct DEV_BROADCAST_PORT_Fixed
        {
            public uint dbcp_size;
            public uint dbcp_devicetype;
            public uint dbcp_reserved;
            // Variable?length field dbcp_name is declared here in the C header file.
        }

        public enum DeviceEvent : int
        {
            DBT_CONFIGCHANGECANCELED = 0x0019,

            DBT_CONFIGCHANGED = 0x0018,

            DBT_CUSTOMEVENT = 0x8006,

            DBT_DEVICEARRIVAL = 0x8000,//USB Insert DEvice Statu

            DBT_DEVICEQUERYREMOVE = 0x8001,

            DBT_DEVICEQUERYREMOVEFAILED = 0x8002,

            DBT_DEVICEREMOVEPENDING = 0x8003,//USB Revoing.

            DBT_DEVICEREMOVECOMPLETE = 0x8004,//USB Remove Completed

            DBT_DEVICETYPESPECIFIC = 0x8005,

            DBT_DEVNODES_CHANGED = 0x0007,//Device List _Changed
            DBT_QUERYCHANGECONFIG = 0x0017,

            DBT_USERDEFINED = 0xFFFF
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            DeviceEvent lEvent;
            lEvent = (DeviceEvent)m.WParam.ToInt32();
            switch (lEvent)
            {
                case DeviceEvent.DBT_DEVICEARRIVAL://[Insert]
                    DEV_BROADCAST_HDR dbhdr = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_HDR));
                    if (dbhdr.dbch_devicetype == DBT_DEVTYP_PORT)      
                    {
                        string portName = Marshal.PtrToStringUni((IntPtr)(m.LParam.ToInt32() + Marshal.SizeOf(typeof(DEV_BROADCAST_PORT_Fixed))));
                        Console.WriteLine("Port '" + portName + "' arrived.");
                    }
                    break;
                case DeviceEvent.DBT_DEVICEREMOVECOMPLETE://[REmove]
                    DEV_BROADCAST_HDR dbhdr2 = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_HDR));
                    if (dbhdr2.dbch_devicetype == DBT_DEVTYP_PORT)
                    {
                        string portName2 = Marshal.PtrToStringUni((IntPtr)(m.LParam.ToInt32() + Marshal.SizeOf(typeof(DEV_BROADCAST_PORT_Fixed))));
                        Console.WriteLine("Port '" + portName2 + "' arrived.");
                        if (!portName.Equals(portName2)) break;
                        commRemoveCallBack();
                        getCommPorts();
                        StringBuilder strBu = new StringBuilder();
                        strBu.Append(StringUtils.findCom);
                        strBu.Append(portName2);
                        strBu.Append(StringUtils.Com_Remove);
                        MessageBox.Show(strBu.ToString());                     
                    }
                    break;
                case DeviceEvent.DBT_DEVNODES_CHANGED://[Device List Have Changed]
                    break;

                default:
                    break;

            }
        }

    }
}
