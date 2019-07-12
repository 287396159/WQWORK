using CiXinLocation.bean;
using CiXinLocation.CiXinInterface;
using CiXinLocation.Model;
using CiXinLocation.Utils;
using SerialportSample;
using SerialPortSpace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using 亚东定位;

namespace CiXinLocation{

    public partial class FormBase : Form,ComReadDataInterface  {

        public List<FromBaseModel> fbModels;
        //private SerialPortUtils serPort;
        private ComDataInterface cdInterface;
        public bool isOpenCom = false; //打开串口了
        public string portName = "COM1";
        public int baudRate = 115200;
        public FromBaseModel selectfileModel;

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
            closeFromBaseModel();
            closeSerPortUtil();
        }     

        private void createSerPortUtil(CommunicationMode comMode) {
            if (cdInterface != null) return;
            if (comMode == CommunicationMode.SERIALPORT) cdInterface = new SerialPortUtils(this);
            else if (comMode == CommunicationMode.UDP) cdInterface = new UDPUtils(this);
        }

        private void closeSerPortUtil() {
            SetDataResuleView.getInstance().clear();
            if (cdInterface == null) return;
            cdInterface.close();
            cdInterface.classClose();
            cdInterface = null;
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
        /// 添加一个Model对象,重复的对象不添加
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

        public void comOpenClose()//.UDP
        { //串口打开或者关闭
            new Thread(comOpenCloseThread).Start();
        }

        private void createSerPortUtil()
        {
            if (cdInterface != null) return;
            SerialPortUtils ser = new SerialPortUtils();
            ser.onDataReceived += revePortsData;

            cdInterface = ser;
        }

        public void comOpenCloseThread()
        {
            createSerPortUtil();
            string msg = "%!%";
            if (!isOpenCom)
                msg = cdInterface.open(baudRate, portName);
            else
                cdInterface.close();
            isOpenCom = cdInterface.comType();

            this.Invoke((EventHandler)(delegate
            { //放入主線程
                commOverCallBack(msg);
            }));
        }



        /// <summary>
        /// 獲取串口號，初始化下拉串口名称列表框
        /// </summary>
        public void getCommPorts() {
            string[] ports = SerialPortUtils.getSerialPorts();
            commPortsCallBack(ports);
        }
        private void sendPortsData(SendDataType sType)
        {
            if (cdInterface != null) new Thread(sendPortsDataThread).Start(sType);// serPort.commWriteByte(buf, 0, buf.Length);
        }
        private void sendPortsData(byte[] buf, string ip, CommunicationMode comMode, SendMode sendMode)
        {
            SendDataType sType = new SendDataType(buf, ip, comMode, sendMode);
            if (cdInterface != null) new Thread(sendPortsDataThread).Start(sType);// serPort.commWriteByte(buf, 0, buf.Length);
        }

        private void sendPortsDataThread(object obj)
        {
            if (!(obj is SendDataType)) return;
            SendDataType sType = (SendDataType)obj;
            if (cdInterface != null) cdInterface.sendData(sType);
        }

        /// <summary>
        /// 接收线程
        /// </summary>
        /// <param name="buf"></param>
        public void revePortsData(byte[] buf, string ip)
        {
            foreach (FromBaseModel fbModel in fbModels) //遍历出主要的设置Model,去除不需要的更新的Model
            {
                if (fbModel == null) continue;
                fbModel.revePortsData(buf, ip);
            }  
        }

        public void revePortsData(SendDataType sType)
        {
            foreach (FromBaseModel fbModel in fbModels) //遍历出主要的设置Model,去除不需要的更新的Model
            {
                if (fbModel == null) continue;
                fbModel.revePortsData(sType);
            }  
        }


        public virtual void commOverCallBack(string msg) { }
        public virtual void commPortsCallBack(string[] msg) { }      

    }
}
