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
using System.Windows.Forms;
using 亚东定位;


namespace CiXinLocation{

    public partial class FormBase : Form,ComReadDataInterface  {

        public List<FromBaseModel> fbModels;
        private ComDataInterface cdInterface;
        private ComDataInterface cdInterface2;
        private ComDataInterface cdInterface3;
        
        public string portName = "COM1";
        public int baudRate = 115200;
        public FromBaseModel selectfileModel;
        private bool isForwordInTcoServer = false; //是否通过TcpServer转发数据
        private object obj = new object();

        public FormBase()
        {
            InitializeComponent();           
        }

        private void FormBase_Load(object sender, EventArgs e)
        {          
            getCommPorts();
            Disposed += OnRecordingPanelDisposed;
        }

        public void OnRecordingPanelDisposed(object sender, EventArgs e)
        {
            closeForm();
            closeFromBaseModel();
            closeSerPortUtil();
        }     

        private void createSerPortUtil(CommunicationMode comMode) {
            if (cdInterface != null) return;
            if (comMode == CommunicationMode.SERIALPORT) cdInterface = new SerialPortUtils(this);
            else if (comMode == CommunicationMode.UDP) cdInterface = new UDPUtils(this);
            else if (comMode == CommunicationMode.TCPServer) cdInterface = new TCPServer(this);
            else if (comMode == CommunicationMode.TCPClien) cdInterface = new TCPClienUtils(this);
        }

        private void addSerPortUtil(CommunicationMode comMode)
        {
            if (cdInterface2 != null) return;
            if (comMode == CommunicationMode.SERIALPORT) cdInterface2 = new SerialPortUtils(this);
            else if (comMode == CommunicationMode.UDP) cdInterface2 = new UDPUtils(this);
            else if (comMode == CommunicationMode.TCPServer) cdInterface2 = new TCPServer(this);
            else if (comMode == CommunicationMode.TCPClien) cdInterface2 = new TCPClienUtils(this);
        }

        private void addSerPortUtil3(CommunicationMode comMode)
        {
            if (cdInterface3 != null) return;
            if (comMode == CommunicationMode.SERIALPORT) cdInterface3 = new SerialPortUtils(this);
            else if (comMode == CommunicationMode.UDP) cdInterface3 = new UDPUtils(this);
            else if (comMode == CommunicationMode.TCPServer) cdInterface3 = new TCPServer(this);
            else if (comMode == CommunicationMode.TCPClien) cdInterface3 = new TCPClienUtils(this);
        }

        private void closeSerPortUtil() {
            if (cdInterface != null)
            {
                cdInterface.close();
                cdInterface = null;
            }
            if (cdInterface2 != null)
            {
                cdInterface2.classClose();
                cdInterface2 = null;
            }
            if (cdInterface3 != null)
            {
                cdInterface3.classClose();
                cdInterface3 = null;
            }
        }

        public bool IsForwordInTcoServer
        {
            get { return isForwordInTcoServer; }
            set { isForwordInTcoServer = value; }
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

            if (isHaveModel(fbModel)) return false;
            fbModel.sendDataHandle += sendPortsData;
            //lock (obj)
            //{
                fbModels.Add(fbModel);
            //}            
            return true;
        }

        public bool getcdInterfaceOpen() 
        {
            if (cdInterface == null) return false;
            return cdInterface.comType();
        }

        public bool getcdInterface2Open()
        {
            if (cdInterface2 == null) return false;
            return cdInterface2.comType();
        }

        public bool getcdInterface3Open()
        {
            if (cdInterface3 == null) return false;
            return cdInterface3.comType();
        }
        public CommunicationMode getCommunicationMode()
        {
            if (cdInterface == null) return CommunicationMode.NOCOM;
            return cdInterface.getCommunicationMode();
        }

        public CommunicationMode getCommunicationMode2()
        {
            if (cdInterface2 == null) return CommunicationMode.NOCOM;
            return cdInterface2.getCommunicationMode();
        }

        public CommunicationMode getCommunicationMode3()
        {
            if (cdInterface3 == null) return CommunicationMode.NOCOM;
            return cdInterface3.getCommunicationMode();
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
                if (path.Equals(itemModel.TAG())) 
                {
                    selectfileModel = itemModel;
                    return itemModel;
                }
            }
            return null;
        }

        public void closeFromBaseModel() 
        {
            lock (obj)
            {
                if (fbModels == null ) return;
                for (int i = 0; i < fbModels.Count;i++ )
                {
                    closeFromBaseModel(fbModels[i]);
                }            
                fbModels.Clear();
            }            
        }

        public void closeFromBaseModel(FromBaseModel fbModel)
        {
            if (fbModel == null || fbModel.sendDataHandle == null) return;
            fbModel.sendDataHandle -= sendPortsData;
            fbModel.close();
            //lock (obj)
            //{
                fbModels.Remove(fbModel);
            //}            
        }

        public void closeFromBaseModel(string path)
        {
            closeFromBaseModel(getFromBaseModel(path));
        }

        public void comOpenClose(CommunicationMode comMode)//.UDP
        { //串口打开或者关闭
            new Thread(comOpenCloseThread).Start(comMode);
        }

        public void comOpenClose2(CommunicationMode comMode)//.UDP
        { //串口打开或者关闭
            new Thread(comOpenCloseThread2).Start(comMode);
        }

        public void comOpenClose3(CommunicationMode comMode)//.UDP
        { //串口打开或者关闭
            new Thread(comOpenCloseThread3).Start(comMode);
        }

        public void comOpenCloseThread3(object obj)
        {
            if (!(obj is CommunicationMode)) return;
            addSerPortUtil3((CommunicationMode)obj);
            openClose(cdInterface3, (CommunicationMode)obj);
        }

        public void comOpenCloseThread2(object obj)
        {
            if (!(obj is CommunicationMode)) return;
            addSerPortUtil((CommunicationMode)obj);
            openClose(cdInterface2,(CommunicationMode)obj);
        }

        public  void comOpenCloseThread(object obj)
        {
            if (!(obj is CommunicationMode)) return;
            createSerPortUtil((CommunicationMode)obj);
            openClose(cdInterface,(CommunicationMode)obj);
        }

        private void openClose(ComDataInterface cdInterface, CommunicationMode comMode) 
        {
            string msg = "%!%";
            if (!cdInterface.comType())
                msg = cdInterface.open(baudRate, portName);
            else
                cdInterface.close();
            try 
            {
                this.Invoke((EventHandler)(delegate
                { //放入主線程
                    commOverCallBack(msg, comMode);
                }));   
            }
            catch { }                    
        }

        /// <summary>
        /// 獲取串口號，初始化下拉串口名称列表框
        /// </summary>
        public void getCommPorts()
        {
            string[] ports = SerialPortUtils.getSerialPorts();
            commPortsCallBack(ports);
        }

        /// <summary>
        /// 尾部 0是cdInterface，1 = cdInterface2，2 = cdInterface3
        /// </summary>
        /// <param name="sendBuf"></param>
        private void sendPortsData(byte[] sendBuf) 
        {
            byte[] buf = new byte[sendBuf.Length -1];
            Array.Copy(sendBuf, 0, buf, 0, buf.Length);
            if (sendBuf[sendBuf.Length - 1] == 0)
            {
                if (PeoplePowerModel.getPeoplePowerModel().Jurisdiction == PeoplePowerModel.getPeoplePowerModel().CongjiValue )
                    sendPortsDataThread3(buf);
                else sendPortsDataThread(buf);
            }              
            else if (sendBuf[sendBuf.Length - 1] == 1)
                sendPortsDataThread2(buf);
            else if (sendBuf[sendBuf.Length - 1] == 2)
                sendPortsDataThread3(buf);
        }

        private void sendPortsData(byte[] sendBuf,string ipInfo)
        {
            if (ipInfo == null) sendPortsData(sendBuf);
            else 
            {
                byte[] buf = new byte[sendBuf.Length -1];
                Array.Copy(sendBuf, 0, buf, 0, buf.Length);
                ComDataInterface cdInterf = null;
                if (sendBuf[sendBuf.Length - 1] == 0)
                {
                    if (PeoplePowerModel.getPeoplePowerModel().Jurisdiction == PeoplePowerModel.getPeoplePowerModel().CongjiValue)
                        cdInterf = cdInterface3;
                    else cdInterf = cdInterface;
                } 
                else if (sendBuf[sendBuf.Length - 1] == 1) cdInterf = cdInterface2;
                else if (sendBuf[sendBuf.Length - 1] == 2) cdInterf = cdInterface3;

                if (cdInterf != null) cdInterf.sendData(buf, 0, buf.Length, ipInfo);
            }
        }

        private void sendPortsDataThread(object obj)
        {
            byte[] buf = (byte[])obj;
            if (cdInterface != null) cdInterface.sendData(buf, 0, buf.Length);
        }

        private void sendPortsDataThread2(object obj)
        {
            byte[] buf = (byte[])obj;
            if (cdInterface2 != null) cdInterface2.sendData(buf, 0, buf.Length);
        }

        private void sendPortsDataThread3(object obj)
        {
            byte[] buf = (byte[])obj;
            if (cdInterface3 != null) cdInterface3.sendData(buf, 0, buf.Length);
        }

        /// <summary>
        /// 接收线程
        /// </summary>
        /// <param name="buf"></param>
        public void revePortsData(byte[] buf)
        {
            revePortsData(buf,null);        
        }

        public void revePortsData(byte[] buf,string ipInfo)
        {
            try
            {
                forwordData(buf, ipInfo);
            }
            catch (Exception e)
            {
                FileModel.getFlModel().setErrorData("<<<<FormBase.revePortsData.." + e.Message + ">>>>\r\n");
            }
            try
            {
                lock (obj)
                {
                    foreach (FromBaseModel fbModel in fbModels) //遍历出主要的设置Model,去除不需要的更新的Model
                    {
                        if (fbModel == null) continue;
                        fbModel.revePortsData(buf, ipInfo);
                    }
                }
            } catch{}    
        }

        public void revePortsData(byte[] buf, string ipInfo, CommunicationMode comMode)
        {
            try 
            {
             if(comMode == CommunicationMode.UDP)  forwordData(buf, ipInfo);
            }
            catch (Exception e)
            {
                FileModel.getFlModel().setErrorData("<<<<FormBase.revePortsData.." + e.Message + ">>>>\r\n");
            }
            try             
            {
                lock (obj)
                {
                    foreach (FromBaseModel fbModel in fbModels) //遍历出主要的设置Model,去除不需要的更新的Model
                    {
                        if (fbModel == null) continue;
                        fbModel.revePortsData(buf, ipInfo, comMode);
                    }
                }
            }
            catch
            {
                
            }
            
        }

        public void forwordData(byte[] buf, string ipInfo) 
        {            
            if (cdInterface2!= null && cdInterface2.getCommunicationMode() == CommunicationMode.TCPServer) 
            {
                cdInterface2.sendMesg(buf,bean.TCPSendType.FORWORD);
            }
            else if (cdInterface != null && cdInterface.getCommunicationMode() == CommunicationMode.TCPServer)
            {
                cdInterface.sendMesg(buf, bean.TCPSendType.FORWORD);
            }
            else if (cdInterface3 != null && cdInterface3.getCommunicationMode() == CommunicationMode.TCPServer)
            {
                cdInterface3.sendMesg(buf, bean.TCPSendType.FORWORD);
            }
        }

        public virtual void commOverCallBack(string msg, CommunicationMode comMode) { }
        public virtual void commPortsCallBack(string[] msg) { }
        public virtual void closeForm() { }    
    }
}
