using CiXinLocation.bean;
using CiXinLocation.CiXinInterface;
using CiXinLocation.Model;
using CiXinLocation.Utils;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CiXinLocation
{
    public partial class MainForm : FormBase, MainFromInterface, LabelChangeInterface
    {
        private DrivaceSetOtherInterface dSetInterface;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DefaultState();
        }

        /// <summary>
        /// 默认设置一些值
        /// </summary>
        private void DefaultState()
        {
            string ip4 = XWUtils.GetAddressIP();
            textBox2.Text = ip4;
            SetDataResuleView.getInstance().setLanINterface(this);
            if (comboBox2.Items.Count > 0) comboBox2.SelectedItem = comboBox2.Items[0];
            if (comboBox3.Items.Count > 0) comboBox3.SelectedItem = comboBox3.Items[0];
        }

        private void createMainModel()
        {
            MainFromModel mfModel = new MainFromModel(this);
            dSetInterface = mfModel;
            createFromBaseModel(mfModel);
        }

        public override void commOverCallBack(string msg) {
            if (isOpenCom)
            {
                btnOpen();
                closeComBtn();
                createMainModel();
            }
            else
            {
                btnClose();
                openComBtn();
                closeMainModel();
            }
            label24.Visible = false;
            button3.Enabled = true;
            if (msg == null || msg.Length < 1 || "%!%".Equals(msg)) return;
            MessageBox.Show(StringUtils.commError + msg);
            
        }

        private void btnClose() 
        {
            closeEnableBtn(button1);
            closeEnableBtn(button4);
            closeEnableBtn(button6);
            closeEnableBtn(button7);
        }

        private void btnOpen()
        {
            openEnableBtn(button1);
            openEnableBtn(button4);
            openEnableBtn(button6);
            openEnableBtn(button7);
        }

        private void closeEnableBtn(Button btn)
        {
            if (btn.Enabled) btn.Enabled = false;
        }

        private void openEnableBtn(Button btn)
        {
            if (!btn.Enabled) btn.Enabled = true;
        }

        private void closeMainModel()
        {
            closeFromBaseModel();
        }

        private void closeComBtn()
        { //串口按鈕關閉
            closeComBtn(button3);
        }

        private void closeComBtn(Button btn)
        {
            if (btn.Text != StringUtils.close)
            {
                btn.Text = StringUtils.close;
                btn.FlatStyle = FlatStyle.Popup;
            }
        }

        private void openComBtn()
        {//串口按鈕打開
            openComBtn(button3);
        }

        private void openComBtn(Button btn)
        {
            if (btn.Text != StringUtils.open)
            {
                btn.Text = StringUtils.open;
                btn.FlatStyle = FlatStyle.Standard;
            }
        }

        private void number_TB_KeyPress(object sender, KeyPressEventArgs e)
        {
            justNumberInput(sender,e);
        }

        private void code_TB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (Char) 8 && e.KeyChar != '.')
            {//
                e.Handled = true;
            } 
        }

        /// <summary>
        /// 只允许数字的输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void justNumberInput(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (Char)8 )
            {//
                e.Handled = true;
            } 
        }

        /// <summary>
        /// 设置结果显示
        /// </summary>
        private void setViewVisi(DataPacketType dpType, Label lab)
        {
            DP_TYPE_LABEL dpLabel = new DP_TYPE_LABEL(dpType, lab);
            SetDataResuleView.getInstance().insertSet(dpLabel);
        }

        /// <summary>
        /// ID是否为空，并且显示一个MessageBox，作为提示,并且接口也不能为空
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true为空，false不为空</returns>
        private bool IDnullMessage(byte[] id, TextBox tbox)
        {
            return IDnullMessage(id, StringUtils.errorID, tbox);
        }
        private bool IDnullMessage(byte[] id)
        {
            return IDnullMessage(id, null);
        }

        private bool IDnullMessage(byte[] id, string message, TextBox tbox)
        {
            if (tbox != null) tbox.Text = "";
            if (id == null)
            {
                MessageBox.Show(message);
                return true;
            }
            return false;
        }

        /// <summary>
        /// IP类的判空处理。主要是对接口对象，ID和IP进行判空，并且提示给用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        private bool IP_IDNULL(byte[] id, byte[] ip)
        {
            if (IDnullMessage(id)) return true;
            if (IPnullMessage(ip)) return true;
            return false;
        }

        private bool IPnullMessage(byte[] ip)
        {
            if (ip == null)
            {
                MessageBox.Show(StringUtils.errorIP);
                return true;
            }
            return false;
        }

        /// <summary>
        /// port类的判空处理。主要是对接口对象，ID和IP进行判空，并且提示给用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        private bool Port_IDNULL(byte[] id, byte[] port)
        {
            if (IDnullMessage(id)) return true;
            if (PortNullMessage(port)) return true;
            return false;
        }

        /// <summary>
        /// port是否为空，并且显示一个MessageBox，作为提示
        /// </summary>
        /// <param name="port"></param>
        /// <returns>true为空，false不为空</returns>
        private bool PortNullMessage(byte[] port)
        {
            return PortNullMessage(port, StringUtils.errorPort);
        }

        private bool PortNullMessage(byte[] port, string msg)
        {
            if (port == null)
            {
                MessageBox.Show(msg);
                return true;
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e) //設置ServerIP
        {          
            byte[] ip = XWUtils.getIP4byte(textBox2.Text);
            byte[] port = XWUtils.getComTime(textBox1.Text);
            if (PortNullMessage(port)) return;
            if (IPnullMessage(ip)) return;
            textBox4.Text = "";
            textBox3.Text = "";
            setViewVisi(DataPacketType.SET_SERVISE_IP, label33);
            dSetInterface.onSetJieDianServiseIP(ip, port);
        }


        private void button6_Click(object sender, EventArgs e) //設置波特率
        {
            byte port = 0xff;
            if (comboBox2.Text.Equals("115200")) port = 0;
            else if (comboBox2.Text.Equals("57600")) port = 1;
            else if (comboBox2.Text.Equals("38400")) port = 2;
            else if (comboBox2.Text.Equals("19200")) port = 3;
            else if (comboBox2.Text.Equals("9600")) port = 4;
            textBox6.Text = "";
            setViewVisi(DataPacketType.SET_BOTELV, label3);
            dSetInterface.onSetBoTeLvPort(port);
        }

        private void button4_Click(object sender, EventArgs e)  //讀取serverIP
        {
            textBox4.Text = "";
            textBox3.Text = "";
            dSetInterface.onReadJieDianServiseIP(null,null);
        }

        private void button7_Click(object sender, EventArgs e) //讀取波特率
        {
            textBox6.Text = "";
            dSetInterface.onReadBoTeLvPort(0);
        }

        private void button3_Click_1(object sender, EventArgs e) //打開串口
        {
            label24.Visible = true;
            int ban = XWUtils.stringToInt1(comboBox3.Text);
            openCloseComm(comboBox1.Text, ban);
        }

        private void openCloseComm(string comPortName, int baudRates)
        {//開關串口操作            
            if (comPortName.Length < 1)
            {
                MessageBox.Show(StringUtils.comNothing);
                return;
            }
            button3.Enabled = false;
            portName = comPortName;
            baudRate = baudRates;
            comOpenClose();
        }

        /// <summary>
        /// 成功返回结果显示
        /// </summary>
        private void retuViewVisi(DataPacketType dpType, Label lab)
        {
            DP_TYPE_LABEL dpLabel = new DP_TYPE_LABEL(dpType, lab);
            SetDataResuleView.getInstance().returnSuccess(dpLabel);
        }

        /// <summary>
        /// 设置节点的ServiseIP
        /// </summary>
        /// <param name="IP">设置成功的IP 4个Byte</param>
        public void onSetJieDianServiseIP(byte[] IP, byte[] port)
        {
            if (port[1] == 0) return;
            retuViewVisi(DataPacketType.SET_SERVISE_IP, label33);
        }

        /// <summary>
        /// 读取节点的ServiseIP
        /// </summary>
        /// <param name="IP">读取到的IP 4个Byte</param>
        public void onReadJieDianServiseIP(byte[] IP, byte[] port)
        {
            setTextToMain(textBox4, IP[0] + "." + IP[1] + "." + IP[2] + "." + IP[3]);
            setTextToMain(textBox3, (port[0] * 0x100 + port[1]).ToString());
        }

        /// <summary>
        /// 设置节点的Servise Port
        /// </summary>
        /// <param name="port">设置节点的端口 2个Byte</param>
        public void onSetBoTeLvPort(byte port)
        {
            if (port == 0) return;
            retuViewVisi(DataPacketType.SET_BOTELV, label3);
        }

        /// <summary>
        /// 读取节点的Servise Port
        /// </summary>
        /// <param name="port">读取节点的端口 2个Byte</param>
        public void onReadBoTeLvPort(byte port)
        {
            StringBuilder sbuder = new StringBuilder();
            if (0 == port) sbuder.Append("115200");
            else if (1 == port) sbuder.Append("57600");
            else if (2 == port) sbuder.Append("38400");
            else if (3 == port) sbuder.Append("19200");
            else if (4 == port) sbuder.Append("9600");
            setTextToMain(textBox6, sbuder.ToString());
        }

        private void setTextToMain(TextBox textBox, string text)
        {
            this.Invoke((EventHandler)(delegate
            { //放入主線程
                textBox.Text = text;
            }));
        }

        public void setUpdataResultLabelMain(Label lab, string text)
        {
            this.Invoke((EventHandler)(delegate
            { //放入主線程
                if (lab == null || text == null) return;
                if (!lab.Visible) lab.Visible = true;
                setUpdataResultLabel(lab, text);
            }));
        }

        private void setUpdataResultLabel(Label lab, string text)
        {
            lab.Text = text;
        }

        public void enableFalse(Label lab)
        {
            if (lab == null) return;
            try
            {
                this.Invoke((EventHandler)(delegate
                { //放入主線程                
                    if (lab.Visible) lab.Visible = false;
                }));
            }
            catch { }
        }

        public override void commPortsCallBack(string[] msg)
        {
            if (msg == null || msg.Length < 1) return;
            comBoxItemsAdd(comboBox1, msg);
        }

        private void comBoxItemsAdd(ComboBox comboBox, string[] ports)
        {
            comboBox.Items.Clear();
            comboBox.Items.AddRange(ports);
            comboBox.SelectedIndex = comboBox.Items.Count > 0 ? 0 : -1;
        }

        private void button14_Click(object sender, EventArgs e) //刷新
        {
            getCommPorts();
        }


    }
}
