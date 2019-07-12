using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

namespace PersionAutoLocaSys
{
    public partial class NodeParamSetWin : Form
    {
        byte[] mbins = null;
        int binlen = 0;
        NodeMsg mnode = null;
        Form1 frm = null;
        FileStream mfstrm = null;
        hexfileinfor curhexmg = null;
        Thread mythread = null;
        public NodeParamSetWin()
        {
            InitializeComponent();
        }
        public NodeParamSetWin(Form1 frm,NodeMsg node)
        {
            InitializeComponent();
            this.mnode = node;
            this.frm = frm;
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == ConstInfor.FORMMESSAGE && (int)m.WParam == ConstInfor.CLOSEMSGPARAM)
            {
                PersonOperation curpersonoper = new PersonOperation(frm.CurPerson.ID, OperType.LeaveNodeParam);
                CommonCollection.personOpers.Add(curpersonoper);
            }
            base.WndProc(ref m);
        }
        private void NodeParamSetWin_Load(object sender, EventArgs e)
        {
            PersonOperation curpersonoper = new PersonOperation(frm.CurPerson.ID, OperType.EnterNodeParam);
            CommonCollection.personOpers.Add(curpersonoper);
            string strrefermsg = "";
            string strid = mnode.ID[0].ToString("X2") + mnode.ID[1].ToString("X2");
            string strname = CommonBoxOperation.GetNodeName(strid);
            if (null == strname || "".Equals(strname))
            {
                strrefermsg = strid;
            }
            else
            {
                strrefermsg = strname + "(" + strid + ")";
            }
            ndmsgtxt.Text = strrefermsg;
            eqpttypetxt.Text = ConstInfor.GetDevType(NodeType.DataNode,mnode.type);
            vertxt.Text = ((byte)(mnode.Version >> 24)).ToString("D2") + ((byte)(mnode.Version >> 16)).ToString("D2") + ((byte)(mnode.Version >> 8)).ToString("D2") + ((byte)(mnode.Version)).ToString("X2");// mnode.Version.ToString("X2");
            //可以读取设备信息了
            nodenwparam mdp = null;
            if (mnode.type != 0x02 && mnode.type != 0x03)
            {   //Wifi参数设置
                label15.Visible = true; wifinametxt.Visible = true; wifinamesetbtn.Visible = true;
                wifinamereadbtn.Visible = true; wifinamestatustxt.Visible = true;
                label16.Visible = true; wifipswtxt.Visible = true;
                wifipswsetbtn.Visible = true; wifipswreadbtn.Visible = true; wifipswstatustxt.Visible = true;
                nodeparambox.Visible = false;
                label18.Visible = true; wifiipmodecb.Visible = true; setwifiipmode.Visible = true; readwifiipmode.Visible = true;
                label19.Visible = true; staticiptx.Visible = true; setstaticip.Visible = true; readstaticip.Visible = true;
                label20.Visible = true; wifisigtx.Visible = true; searchwifisigbtn.Visible = true;
                wifiipmodestatus.Visible = false; staticipstatus.Visible = false; wifisigstatus.Visible = false;
                sigvaluetx.Visible = true;

                rstbtn.Location = new Point(266, 334); rststatustxt.Location = new Point(457, 340);
                setparmtab.Height = 393;
                this.Height = 542;
                mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.wifiname);
                if (null == mdp)
                {
                    wifinamestatustxt.Text = "read error";
                }
                else
                {

                    wifinametxt.Text = "" + System.Text.ASCIIEncoding.Default.GetString(mdp.wifiname, 0, 32);
                    wifinamestatustxt.Text = "read ok";
                }
                mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.wifipsw);
                if (null == mdp)
                {
                    wifipswstatustxt.Text = "read error";
                }
                else
                {

                    wifipswtxt.Text = "" + System.Text.ASCIIEncoding.Default.GetString(mdp.wifipsw, 0, 32);
                    wifipswstatustxt.Text = "read ok";
                }
                mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.wifiipmode);
                if (null == mdp)
                {
                    //读取WIfi的IP模式
                    wifiipmodestatus.Visible = true;
                    wifiipmodestatus.Text = "read error";
                }
                else
                {
                    wifiipmodestatus.Visible = true;
                    wifiipmodestatus.Text = "read ok";
                    wifiipmodecb.SelectedIndex = mdp.ipmode;
                }

                mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.wifistaticip);
                if (null == mdp)
                {
                    staticipstatus.Visible = true;
                    staticipstatus.Text = "read error";
                }
                else
                {
                    staticipstatus.Visible = true;
                    staticipstatus.Text = "read ok";
                    staticiptx.Text = mdp.nodeip[0].ToString() + "." + mdp.nodeip[1].ToString() + "." + mdp.nodeip[2].ToString() + "." + mdp.nodeip[3].ToString();
                }
            }
            else
            {   //RJ45参数设置
                label15.Visible = false; wifinametxt.Visible = false; wifinamesetbtn.Visible = false;
                wifinamereadbtn.Visible = false; wifinamestatustxt.Visible = false;
                label16.Visible = false; wifipswtxt.Visible = false;
                wifipswsetbtn.Visible = false; wifipswreadbtn.Visible = false; wifipswstatustxt.Visible = false;
                label18.Visible = false; wifiipmodecb.Visible = false; setwifiipmode.Visible = false; readwifiipmode.Visible = false;
                label19.Visible = false; staticiptx.Visible = false; setstaticip.Visible = false; readstaticip.Visible = false;
                label20.Visible = false; wifisigtx.Visible = false; searchwifisigbtn.Visible = false;
                wifiipmodestatus.Visible = false; staticipstatus.Visible = false; wifisigstatus.Visible = false;
                sigvaluetx.Visible = false;
                nodeparambox.Location = new Point(17, 128);
                setparmtab.Height = 422;
                this.Height = 585;

                rstbtn.Location = new Point(266, 282);
                rststatustxt.Location = new Point(457, 288);

                mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.nodemode);
                if (null == mdp)
                {
                    ipmodestatustxt.Text = "read error";
                }
                else
                {
                    ipmodestatustxt.Text = "read ok";
                    ipmodecb.SelectedIndex = mdp.ipmode;
                }

                mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.nodeip);
                if (null == mdp)
                {
                    nodeipstatustxt.Text = "read error";

                }
                else
                {
                    nodeipstatustxt.Text = "read ok";
                    ndiptxt.Text = mdp.nodeip[0].ToString() + "." + mdp.nodeip[1].ToString() + "." + mdp.nodeip[2].ToString() + "." + mdp.nodeip[3].ToString();
                }

                mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.submask);
                if (null == mdp)
                {
                    maskstatustxt.Text = "read error";

                }
                else
                {
                    maskstatustxt.Text = "read ok";
                    smtxt.Text = mdp.submask[0].ToString() + "." + mdp.submask[1].ToString() + "." + mdp.submask[2].ToString() + "." + mdp.submask[3].ToString();
                }

                mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.gateway);
                if (null == mdp)
                {
                    gatestatustxt.Text = "read error";
                }
                else
                {
                    gatestatustxt.Text = "read ok";
                    gwtxt.Text = mdp.gateway[0].ToString() + "." + mdp.gateway[1].ToString() + "." + mdp.gateway[2].ToString() + "." + mdp.gateway[3].ToString();
                }

            }
            //读取ServerIp
            mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.serverip);
            if (null == mdp)
            {
                serveripstatustxt.Text = "read error";
            }
            else
            {
                serveripstatustxt.Text = "read ok";
                iptxt.Text = mdp.serverip[0].ToString() + "." + mdp.serverip[1].ToString() + "." + mdp.serverip[2].ToString() + "." + mdp.serverip[3].ToString();
            }
            //读取Server端口
            mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.serverport);
            if (null == mdp)
            {
                serverportstatustxt.Text = "read error";
            }
            else
            {
                serverportstatustxt.Text = "read ok";
                porttxt.Text = mdp.port + "";
            }

            //读取Node信道
            mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.nodechannel);
            if (null == mdp)
            {
                chantxt.Text = "read error";
            }
            else
            {
                chantxt.Text = "read ok";
                chantb.Text = mdp.channel + "";
            }

            rststatustxt.Visible = false;
        }

        private string GetWifiNameOrPsw(byte[] bytes,int len)
        {
            string str = "";
            int index = -1,i;
            for (i = len - 1; i > 0;i--)
            {
                if (bytes[i] != 0)
                {
                    index = i;
                }
            }
            for (i = 0; i < index;i++)
            {
                str = "" + bytes[i];
            }
            return str;
        }
        
        //读取Node的参数
        private nodenwparam ReadNodeParam(NodeMsg desdev, UdpClient client, nodeparm type)
        {
            byte[] ips = new byte[4];
            byte[] bytes = new byte[6];
            bytes[0] = 0xF1; bytes[bytes.Length - 1] = 0xF2;
            System.Buffer.BlockCopy(desdev.ID, 0, bytes,2,2);
            switch (type)
            {
                case nodeparm.serverip: bytes[1] = 0x02; break;
                case nodeparm.serverport: bytes[1] = 0x04; break;
                case nodeparm.nodechannel: bytes[1] = 0x14;break;
                case nodeparm.wifiname: bytes[1] = 0x06; break;
                case nodeparm.wifipsw: bytes[1] = 0x08; break;
                case nodeparm.wifiipmode: bytes[1] = 0x17; break;
                case nodeparm.wifistaticip: bytes[1] = 0x19; break;
                case nodeparm.nodemode: bytes[1] = 0x0A; break;
                case nodeparm.nodeip: bytes[1] = 0x0C; break;
                case nodeparm.submask: bytes[1] = 0x0E; break;
                case nodeparm.gateway: bytes[1] = 0x10; break;
            }
            bytes[bytes.Length - 2] = 0;
            for (int i = 0; i < bytes.Length - 2;i++)
            {
                bytes[bytes.Length - 2] += bytes[i];
            }
            SysParam.cbparm = false;
            SysParam.sendcount = 3;

            while (true)
            {
                SysParam.tickcount = Environment.TickCount;
                SysParam.sendcount--;
                client.Send(bytes, bytes.Length, desdev.mendpoint);
                while (Environment.TickCount - SysParam.tickcount < ConstInfor.recnwtimeout + 300 && (!SysParam.cbparm))
                {
                    Thread.Sleep(1);
                }
                if (SysParam.cbparm || SysParam.sendcount <= 0)
                {
                    break;
                }
            }
            if (!SysParam.cbparm)
            {
                return null;
            }
            return SysParam.mntparm;
        }
        //读取Node的参数
        private bool SetNodeParam(NodeMsg desdev, UdpClient client, nodeparm type,byte[] mparams)
        {
            byte[] bytes = null;
            //初始化设置参数及发送数组
            #region
            switch (type)
            {
                case nodeparm.serverip:
                    /*Server设置IP
                     * PC-->Node：F1 + 01 + ID(2byte) + IP(4byte) + CS + F2
                     */
                    bytes = new byte[10];bytes[1] = 0x01;
                    System.Buffer.BlockCopy(mparams, 0, bytes,4,4);
                    break;
                case nodeparm.serverport:
                    /*Server设置端口
                     * PC --> Node：F1 + 03 + ID(2byte) + Port(2byte) + CS + F2
                     */
                    bytes = new byte[8];bytes[1] = 0x03;
                    System.Buffer.BlockCopy(mparams, 0, bytes,4,2);
                    break;
                case nodeparm.wifiname:
                    /*Server设置WiFI名称
                     * PC --> Node：F1 + 05 + ID(2byte) + Name(32byte) + CS + F2
                     */
                    bytes = new byte[38];bytes[1] = 0x05;
                    System.Buffer.BlockCopy(mparams, 0, bytes,4,32);
                    break;
                case nodeparm.wifipsw:
                    /*Server设置WIFI密码
                     * PC --> Node：F1 + 07 + ID(2byte) + PSW(32byte) + CS + F2
                     */
                    bytes = new byte[38];bytes[1] = 0x07;
                    System.Buffer.BlockCopy(mparams, 0, bytes,4,32);
                    break;
                case nodeparm.wifiipmode:
                    /*设置Wifi的IP模式
                     * PC --> Node: F1 + 16 + ID(2byte) + IPmode(1byte) + CS + F2
                     */
                    bytes = new byte[7];bytes[1] = 0x16;
                    bytes[4] = mparams[0];
                    break;
                case nodeparm.wifistaticip:
                    /*
                     * 设置Wifi的IP
                     * PC --> Node: F1 + 18 + ID(2byte) + IP(4byte) + CS + F2 
                     */
                    bytes = new byte[10];bytes[1] = 0x18;
                    System.Buffer.BlockCopy(mparams, 0, bytes, 4, 4);
                    break;
                case nodeparm.nodemode:
                    /*Server设置IP模式（动态/静态）
                     * PC --> Node：F1 + 09 + ID(2byte) + Mode(1byte) + CS + F2
                     */
                    bytes = new byte[7];bytes[1] = 0x09;
                    bytes[4] = mparams[0];
                    break;
                case nodeparm.nodeip:
                    /*Server设置节点IP
                     * PC --> Node：F1 + 0B + ID(2byte) + IP(4byte) + CS + F2
                     */
                    bytes = new byte[10];bytes[1] = 0x0B;
                    System.Buffer.BlockCopy(mparams, 0, bytes,4,4);
                    break;
                case nodeparm.submask:
                    /*Server设置节点掩码
                     * PC --> Node：F1 + 0D + ID(2byte) + Submask(4byte) + CS + F2
                     * */
                    bytes = new byte[10];bytes[1] = 0x0D;
                    System.Buffer.BlockCopy(mparams, 0, bytes,4,4);
                    break;
                case nodeparm.gateway:
                    /*Server设置节点网关
                     * PC --> Node：F1 + 0F + ID(2byte) + GateWay(4byte) + CS + F2
                     */
                    bytes = new byte[10];bytes[1] = 0x0F;
                    System.Buffer.BlockCopy(mparams, 0, bytes,4,4);
                    break;
                case nodeparm.rst:
                    /*Server复位节点
                     * PC --> Node：F1 + 00 + ID + CS + F2
                     */
                    bytes = new byte[6];bytes[1] = 0x00;
                    break;
                default:
                    return false;
            }
            #endregion
            //赋值ID
            System.Buffer.BlockCopy(desdev.ID, 0, bytes,2,2);
            //设置包头包尾
            bytes[0] = 0xF1; bytes[bytes.Length - 1] = 0xF2;
            //设置校验和
            bytes[bytes.Length - 2] = 0;
            for (int i = 0; i < bytes.Length - 2;i++)
                bytes[bytes.Length - 2] += bytes[i];
            //传送固件数据
            SysParam.cbparm = false;
            SysParam.sendcount = 3;
            while (true)
            {
                SysParam.tickcount = Environment.TickCount;
                SysParam.sendcount--;
                client.Send(bytes, bytes.Length, desdev.mendpoint);
                while (Environment.TickCount - SysParam.tickcount < ConstInfor.recnwtimeout + 300 && (!SysParam.cbparm))
                {
                    Thread.Sleep(1);
                }
                if (SysParam.cbparm || SysParam.sendcount >= 0)
                {
                    break;
                }
            }
            return SysParam.cbparm;
        }
        //点击读取Node的Server IP
        private void readipbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                serveripstatustxt.Text = "";
            }));
            rststatustxt.Visible = false;
            nodenwparam mdp = null;
            //读取ServerIp
            mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.serverip);
            if (null == mdp)
            {
                this.Invoke(new Action(() => {
                    serveripstatustxt.Text = "read error";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    serveripstatustxt.Text = "read ok";
                    iptxt.Text = mdp.serverip[0].ToString() + "." + mdp.serverip[1].ToString() + "." + mdp.serverip[2].ToString() + "." + mdp.serverip[3].ToString();
                }));
            }
        }
        //点击设置Node的Server IP
        private void setipbtn_Click(object sender, EventArgs e)
        {
            rststatustxt.Visible = false;
            string strip = iptxt.Text;
            if ("".Equals(strip))
            {
                MessageBox.Show("Sorry IP address cannot be empty!");
                return;
            }
            IPAddress ipaddr = null;
            if (!IPAddress.TryParse(strip, out ipaddr))
            {
                MessageBox.Show("Sorry, the IP format is wrong!");
                return;
            }
            DialogResult mrs = MessageBox.Show("Changing server IP causes the current network connection to disconnect. Are you sure you want to change?", "warning", MessageBoxButtons.OKCancel);
            if (mrs == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            this.Invoke(new Action(() =>
            {
                serveripstatustxt.Text = "";
            }));
            //获取IP地址的byte数组
            byte[] ips = ipaddr.GetAddressBytes();
            //设置IP
            if (SetNodeParam(mnode, frm.MyUdpClient, nodeparm.serverip, ips))
            {
                this.Invoke(new Action(() => {
                    serveripstatustxt.Text = "set ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    serveripstatustxt.Text = "set error";
                }));
            }
        }
        //读取Server端的端口
        private void readportbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                serverportstatustxt.Text = "";
            }));
            rststatustxt.Visible = false;
            nodenwparam mdp = null;
            //读取ServerIp
            mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.serverport);
            if (null == mdp)
            {
                this.Invoke(new Action(() => {
                    serverportstatustxt.Text = "read error";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    serverportstatustxt.Text = "read ok";
                }));
                porttxt.Text = mdp.port+"";
            }
        }
        //设置Server端口
        private void setportbtn_Click(object sender, EventArgs e)
        {
            rststatustxt.Visible = false;
            string strport = porttxt.Text;
            if ("".Equals(strport))
            {
                MessageBox.Show("Sorry, the port cannot be empty!");
                return;
            }
            int port = 0;
            try 
            {
                port = Convert.ToInt32(strport);
            }catch(Exception)
            {
                MessageBox.Show("Sorry, the port format is wrong!");
                return;
            }
            if (port < 1024 || port > 65535)
            {
                MessageBox.Show("Sorry, the port range is 1024 to 65535!");
                return;
            }
            DialogResult mrs = MessageBox.Show("Changing the port causes the current network connection to disconnect. Are you sure you want to change?", "warning", MessageBoxButtons.OKCancel);
            if (mrs == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            this.Invoke(new Action(() =>
            {
                serverportstatustxt.Text = "";
            }));
            byte[] mpts = new byte[2];
            mpts[0] = (byte)(port >> 8);
            mpts[1] = (byte) port;
            if (SetNodeParam(mnode, frm.MyUdpClient, nodeparm.serverport, mpts))
            {
                this.Invoke(new Action(() => {
                    serverportstatustxt.Text = "set ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    serverportstatustxt.Text = "set error";
                }));
            }

        }
        //读取ip模式
        private void readipmodebtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                ipmodestatustxt.Text = "";
            }));
            rststatustxt.Visible = false;
            nodenwparam mdp = null;
            mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.nodemode);
            if (null == mdp)
            {
                this.Invoke(new Action(() => {
                    ipmodestatustxt.Text = "read error";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    ipmodestatustxt.Text = "read ok";
                    ipmodecb.SelectedIndex = mdp.ipmode;
                }));
            }
        }
        //设置ip模式
        private void setipmodebtn_Click(object sender, EventArgs e)
        {
            rststatustxt.Visible = false;
            if (ipmodecb.SelectedIndex < 0 || ipmodecb.SelectedIndex > 1)
            {
                MessageBox.Show("Sorry, the choice of Ip mode is wrong!");
                return;
            }
            this.Invoke(new Action(() =>
            {
                ipmodestatustxt.Text = "";
            }));
            byte[] ipmodes = new byte[1];
            ipmodes[0] = (byte)ipmodecb.SelectedIndex;
            if (SetNodeParam(mnode, frm.MyUdpClient, nodeparm.nodemode, ipmodes))
            {
                this.Invoke(new Action(() => {
                    ipmodestatustxt.Text = "set ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    ipmodestatustxt.Text = "set error";
                }));
            }
        }
        //读取Node自身ip
        private void readndipbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                nodeipstatustxt.Text = "";
            }));
            rststatustxt.Visible = false;
            nodenwparam mdp = null;
            //读取ServerIp
            mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.nodeip);
            if (null == mdp)
            {
                this.Invoke(new Action(() => { 
                    nodeipstatustxt.Text = "read error";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    nodeipstatustxt.Text = "read ok";
                    ndiptxt.Text = mdp.nodeip[0].ToString() + "." + mdp.nodeip[1].ToString() + "." + mdp.nodeip[2].ToString() + "." + mdp.nodeip[3].ToString();
                }));
            }
        }
        //设置自身Ip
        private void setndipbtn_Click(object sender, EventArgs e)
        {
            rststatustxt.Visible = false;
            string strip = ndiptxt.Text;
            if ("".Equals(strip))
            {
                MessageBox.Show("Sorry IP address cannot be empty!");
                return;
            }
            IPAddress ipaddr = null;
            if (!IPAddress.TryParse(strip, out ipaddr))
            {
                MessageBox.Show("Sorry, the IP format is wrong!");
                return;
            }
            DialogResult mrs = MessageBox.Show("Sorry, the IP change of Node itself may cause the network to be blocked, can you make sure that you want to change it?", "warning", MessageBoxButtons.OKCancel);
            if (mrs == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            this.Invoke(new Action(() =>
            {
                nodeipstatustxt.Text = "";
            }));
            //获取IP地址的byte数组
            byte[] ips = ipaddr.GetAddressBytes();
            //设置IP
            if (SetNodeParam(mnode, frm.MyUdpClient, nodeparm.nodeip, ips))
            {
                this.Invoke(new Action(() => {
                    nodeipstatustxt.Text = "set ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    nodeipstatustxt.Text = "set error";
                }));
            }
        }
        //读取掩码
        private void readsmbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                maskstatustxt.Text = "";
            }));
            rststatustxt.Visible = false;
            nodenwparam mdp = null;
            mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.submask);
            if (null == mdp)
            {
                this.Invoke(new Action(() => {
                    maskstatustxt.Text = "read error";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    maskstatustxt.Text = "read ok";
                    smtxt.Text = mdp.submask[0].ToString() + "." + mdp.submask[1].ToString() + "." + mdp.submask[2].ToString() + "." + mdp.submask[3].ToString();
                }));
            }
        }
        //设置掩码
        private void setsmbtn_Click(object sender, EventArgs e)
        {
            rststatustxt.Visible = false;
            string strsm = smtxt.Text;
            if ("".Equals(strsm))
            {
                MessageBox.Show("Sorry, the subnet mask cannot be empty!");
                return;
            }
            IPAddress smaddr = null;
            if (!IPAddress.TryParse(strsm, out smaddr))
            {
                MessageBox.Show("Sorry, the subnet mask format is wrong!");
                return;
            }
            DialogResult mrs = MessageBox.Show("Modifying the subnet mask can cause the network to be blocked, to make sure that it needs to be modified?", "warning", MessageBoxButtons.OKCancel);
            if (mrs == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            this.Invoke(new Action(() =>
            {
                maskstatustxt.Text = "";
            }));
            byte[] submasks = smaddr.GetAddressBytes();
           
            if (SetNodeParam(mnode, frm.MyUdpClient, nodeparm.submask, submasks))
            {
                this.Invoke(new Action(() => {
                    maskstatustxt.Text = "set ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    maskstatustxt.Text = "set error";
                }));
            }
        }
        //读取网关
        private void readgwbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                gatestatustxt.Text = "";
            }));
            rststatustxt.Visible = false;
            nodenwparam mdp = null;
            mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.gateway);
            if (null == mdp)
            {
                this.Invoke(new Action(() => {
                    gatestatustxt.Text = "read error";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    gatestatustxt.Text = "read ok";
                    gwtxt.Text = mdp.gateway[0].ToString() + "." + mdp.gateway[1].ToString() + "." + mdp.gateway[2].ToString() + "." + mdp.gateway[3].ToString();
                }));
            }
        }
        //设置网关
        private void setgwbtn_Click(object sender, EventArgs e)
        {
            rststatustxt.Visible = false;
            string strgw = gwtxt.Text;
            if ("".Equals(strgw))
            {
                MessageBox.Show("Sorry, the gateway cannot be empty!");
                return;
            }
            IPAddress ipgw = null;
            if (!IPAddress.TryParse(strgw, out ipgw))
            {
                MessageBox.Show("Sorry, the gateway format is wrong!");
                return;
            }
            DialogResult mrs = MessageBox.Show("Modifying the gateway could cause the current network connection to disconnect. Do you want to change it?", "warning", MessageBoxButtons.OKCancel);
            if (mrs == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            this.Invoke(new Action(() =>
            {
                gatestatustxt.Text = "";
            }));
            //获取IP地址的byte数组
            byte[] gws = ipgw.GetAddressBytes();
            //设置IP
            if (SetNodeParam(mnode, frm.MyUdpClient, nodeparm.gateway, gws))
            {
                this.Invoke(new Action(() => {
                    gatestatustxt.Text = "set ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    gatestatustxt.Text = "set error";
                }));
            }
        }
        //复位
        private void rstbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                rststatustxt.Text = "";
            }));
            if (SetNodeParam(mnode, frm.MyUdpClient, nodeparm.rst, null))
            {
                this.Invoke(new Action(() => {
                    rststatustxt.Text = "reset ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    rststatustxt.Text = "reset err";
                }));
            }
            rststatustxt.Visible = true;
        }
        //选择固件
        private void selectbtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog mofd = new OpenFileDialog();
            mofd.Filter = "all|*.hex";
            if (mofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                updatefiletxt.Text = mofd.FileName;

                ReadHexInfor.getfileinfor(mofd.FileName, out curhexmg);
                //更新固件信息
                sizetxt.Text = curhexmg.filesize + " byte";
                versiontxt.Text = ((Byte)(curhexmg.version >> 24)).ToString("D2") + ((Byte)(curhexmg.version >> 16)).ToString("D2") + ((Byte)(curhexmg.version >> 8)).ToString("D2") + ((Byte)(curhexmg.version)).ToString("X2");
                devtypetxt.Text = curhexmg.GetDev();
            }
        }
        //固件更新
        private void updatebtn_Click(object sender, EventArgs e)
        {
            //点击更新时需要输入密码

            if (!frm.CurPerson.Name.ToUpper().Equals(ConstInfor.dmatekname.ToUpper()))
            {//当前登陆的是公司内部人员，不需要密码验证
                UpdateVerifyWin Myverywin = new UpdateVerifyWin();
                if (Myverywin.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    MessageBox.Show("對不起,你沒有更新權限,如需更新請聯繫本公司工作人員...");
                    return;
                }
            }
			//判断固件是否可以更新
            //检验设备类型
            string strpath = updatefiletxt.Text;
            if ("".Equals(strpath))
            {
                MessageBox.Show("Sorry, the firmware path cannot be empty!");
                return;
            }
            if (null == curhexmg)
            {
                MessageBox.Show("Sorry, the firmware path is wrong!");
                return;
            }
            //判断固件是否可以更新
            //检验设备类型
            if (curhexmg.type != (hexfileinfor.nodetype | mnode.type))
            {
                MessageBox.Show("Sorry, the firmware device type cannot be updated!");
                return;
            }
            if (curhexmg.version == mnode.Version)
            {
                MessageBox.Show("Sorry, the firmware version cannot be updated!");
                return;
            }
            updateimage(strpath);
        }
        //更新固件
        private void updateimage(string filepath)
        {
            try
            {
                mfstrm = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            }catch(Exception)
            {
                MessageBox.Show("Sorry, the firmware path is wrong!");
                return;
            }
            byte[] bytes = new byte[mfstrm.Length];
            try
            {
                mfstrm.Read(bytes, 0, bytes.Length);
            }
            catch (Exception)
            {
            }
            finally
            {
                if (null != mfstrm)
                    mfstrm.Close();
                mfstrm = null;
            }
            //开始创建线程
            int result = ReadHexInfor.HexToBin(bytes, out mbins, out binlen);
            if (result < 0)
            {
                #region
                if (result == -1)
                {
                    MessageBox.Show("Sorry, check hex failed!");
                    return;
                }
                else if (result == -2)
                {
                    MessageBox.Show("The length of the error");
                    return;
                }
                #endregion
            }
            else
            {
                //可以开始传送固件数据了
                mythread = new Thread(updateimage);
                mythread.Start();
            }
        }
        //开始传送固件数据
        private void updateimage()
        {
            /*
             * byte[] mbins = null;
             * int binlen = 0;
             * --------------------------------------------------------------------------
             * USB Dangle对节点设备传送固件数据
             * PC --> Node：F1 + 12 + ID(2byte) + Addr(2byte) + Data(64) + CS + F2
             * Node --> PC：FA + 12 + ID + Addr + CS + FB
             * --------------------------------------------------------------------------
             * 对节点设备传送固件数据完成，并要求节点做固件更新
             * PC --> Node：F1 + 13 + ID(2byte) + Len(2byte) + CS_FW(4byte) + CS + F2
             * Node --> PC：FA + 13 + ID + Status(1byte) + CS + FB
             * --------------------------------------------------------------------------
             * */
            byte[] bytes = new byte[72];
            bytes[0] = 0xF1; bytes[1] = 0x12; bytes[bytes.Length - 1] = 0xF2;
            System.Buffer.BlockCopy(mnode.ID, 0, bytes,2,2);
            int startaddr = 0;
            while(true)
            { 
                bytes[4] = (byte)(startaddr >> 8);bytes[5] = (byte)(startaddr);
                SysParam.currentaddr = startaddr;
                System.Buffer.BlockCopy(mbins, startaddr, bytes,6,64);
                bytes[bytes.Length - 2] = 0;
                for (int i = 0; i < bytes.Length - 2;i++)
                {
                    bytes[bytes.Length - 2] += bytes[i];
                }
                //可以发送数据了
                SysParam.sendcount = 5;
                SysParam.cbparm = false;
                while (true)
                {
                    SysParam.sendcount--;
                    SysParam.tickcount = Environment.TickCount;
                    frm.MyUdpClient.Send(bytes, bytes.Length, mnode.mendpoint);
                    while (Environment.TickCount - SysParam.tickcount < ConstInfor.recnwtimeout + 100 && !SysParam.cbparm)
                    {
                        Thread.Sleep(1);
                    }
                    if (SysParam.cbparm || SysParam.sendcount <= 0)
                    {
                        break;
                    }
                }
                if (!SysParam.cbparm)
                {
                    break;
                }
                else
                {
                    startaddr += 64;
                    if (startaddr >= binlen)
                    {
                        try
                        {
                            this.Invoke(new Action(() =>
                            {
                                uppgtxt.Text = "100%";
                            }));
                        }catch(Exception)
                        {
                        
                        }
                        break;
                    }
                    else
                    {
                        try
                        {
                            this.Invoke(new Action(() =>
                            {
                                if (!uppgtxt.IsDisposed)
                                    uppgtxt.Text = string.Format("{0:F0}", (((double)startaddr / binlen)) * 100) + "%";
                            }));
                        }
                        catch (Exception)
                        { 
                        
                        }
                        
                    }
                }
            }

            if (startaddr < binlen && !SysParam.cbparm)
            {
                this.Invoke(new Action(() =>
                {
                    uppgtxt.Text = "Failed to transmit firmware！";
                }));
                return;
            }

            //传送完成，请求更新
            //PC --> Node：F1 + 13 + ID(2byte) + Len(2byte) + CS_FW(4byte) + CS + F2
            bytes = new byte[12];
            bytes[0] = 0xF1; bytes[1] = 0x13; bytes[bytes.Length - 1] = 0xF2;
            System.Buffer.BlockCopy(mnode.ID, 0, bytes, 2, 2);
            bytes[4] = (byte)(binlen >> 8);
            bytes[5] = (byte)(binlen);
            UInt32 sumcs = 0;
            for (int i = 0; i < binlen;i++)
            {
                sumcs += mbins[i];
            }
            bytes[6] = (byte)(sumcs >> 24); bytes[7] = (byte)(sumcs >> 16);
            bytes[8] = (byte)(sumcs >> 8); bytes[9] = (byte)(sumcs);
            bytes[bytes.Length - 2] = 0;
            for (int i = 0; i < bytes.Length - 2;i++)
            {
                bytes[bytes.Length - 2] += bytes[i];
            }
            //可以发送数据了
            SysParam.tickcount = Environment.TickCount;
            SysParam.sendcount = 3;
            SysParam.cbparm = false;
            SysParam.ischeckcs = false;
            while (true)
            {
                SysParam.sendcount--;
                SysParam.tickcount = Environment.TickCount;
                frm.MyUdpClient.Send(bytes, bytes.Length, mnode.mendpoint);
                while (Environment.TickCount - SysParam.tickcount < ConstInfor.recnwtimeout + 200 && !SysParam.cbparm)
                {
                    Thread.Sleep(1);
                }
                if (SysParam.cbparm || SysParam.sendcount <= 0)
                {
                    break;
                }
            }
            if (!SysParam.cbparm)
            {
                this.Invoke(new Action(() =>
                {
                    uppgtxt.Text = "Firmware update failed！";
                }));
            }
            else
            {
                if (!SysParam.ischeckcs)
                {

                    this.Invoke(new Action(() =>
                    {
                        uppgtxt.Text = "Failure to verify the underlying firmware！";
                    }));
                }
                else
                {
                    this.Invoke(new Action(() =>
                    {
                        uppgtxt.Text = "Firmware update success！";
                        vertxt.Text = "" + ((byte)(curhexmg.version >> 24)).ToString("D2") + ((byte)(curhexmg.version >> 16)).ToString("D2") + ((byte)(curhexmg.version >> 8)).ToString("D2") + ((byte)(curhexmg.version)).ToString("X2");
                        mnode.Version = curhexmg.version;
                    }));
                }
            }
        }
        private void wifinamereadbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                wifinamestatustxt.Text = "";
            }));
            rststatustxt.Visible = false;
            nodenwparam mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.wifiname);
            if (null == mdp)
            {
                this.Invoke(new Action(() => {
                    wifinamestatustxt.Text = "read error";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    wifinametxt.Text = "" + System.Text.ASCIIEncoding.Default.GetString(mdp.wifiname, 0, 32);
                    wifinamestatustxt.Text = "read ok";
                }));
            }
        }
        private void wifipswreadbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                wifipswstatustxt.Text = "";
            }));
            rststatustxt.Visible = false;
            nodenwparam mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.wifipsw);
            if (null == mdp)
            {
                this.Invoke(new Action(() => {
                    wifipswstatustxt.Text = "read error";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    wifipswtxt.Text = "" + System.Text.ASCIIEncoding.Default.GetString(mdp.wifipsw, 0, 32);
                    wifipswstatustxt.Text = "read ok";
                }));
            }
        }
        //设置Wifi名称
        private void wifinamesetbtn_Click(object sender, EventArgs e)
        {
            string strwifiname = wifinametxt.Text;
            rststatustxt.Visible = false;
            if ("".Equals(strwifiname))
            {
                MessageBox.Show("I'm sorry, but the Wifi name cannot be empty!");
                return;
            }
            if (strwifiname.Length >= 32)
            {
                MessageBox.Show("Sorry, the length of the Wifi name cannot exceed 32 bits");
                return;
            }
            this.Invoke(new Action(() =>
            {
                wifinamestatustxt.Text = "";
            }));
            byte[] bytes = new byte[32];
            byte[] chs = System.Text.Encoding.ASCII.GetBytes(strwifiname);
            System.Buffer.BlockCopy(chs, 0, bytes, 0, chs.Length);
            if (SetNodeParam(mnode, frm.MyUdpClient, nodeparm.wifiname, bytes))
            {
                this.Invoke(new Action(() => {
                    wifinamestatustxt.Text = "set ok";
                }));
                
            }
            else
            { 
                this.Invoke(new Action(() => {
                    wifinamestatustxt.Text = "set error";
                }));
            }
        }

        private void wifipswsetbtn_Click(object sender, EventArgs e)
        {
            string strpsw = wifipswtxt.Text;
            rststatustxt.Visible = false;
            if ("".Equals(strpsw))
            {
                MessageBox.Show("I'm sorry, but the Wifi password cannot be empty!");
                return;
            }
            if (strpsw.Length >= 32)
            {
                MessageBox.Show("Sorry, the length of the Wifi password cannot exceed 32 bits");
                return;
            }
            this.Invoke(new Action(() =>
            {
                wifipswstatustxt.Text = "";
            }));
            byte[] bytes = new byte[32];
            byte[] chs = System.Text.Encoding.ASCII.GetBytes(strpsw);
            System.Buffer.BlockCopy(chs, 0, bytes, 0, chs.Length);
            if (SetNodeParam(mnode, frm.MyUdpClient, nodeparm.wifipsw, bytes))
            {
                this.Invoke(new Action(() => {
                    wifipswstatustxt.Text = "set ok";
                }));
            }
            else
            { 
                this.Invoke(new Action(() => {
                    wifipswstatustxt.Text = "set error";
                }));
            }
        }

        private void chanreadbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                chantxt.Text = "";
            }));
            rststatustxt.Visible = false;
            nodenwparam mdp = null;
            //读取ServerIp
            mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.nodechannel);
            if (null == mdp)
            {
                this.Invoke(new Action(() => {
                    chantxt.Text = "read error";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    chantxt.Text = "read ok";
                    chantb.Text = mdp.channel + "";
                }));
            }
        }

        private void NodeParamSetWin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != mythread)
            {
                try
                {
                    if (mythread.IsAlive)
                    {
                        mythread.Abort();
                    }
                    mythread = null;
                }catch(Exception)
                {
                }
            }
        }
        //读取wifi节点的IP 模式
        private void readwifiipmode_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() => {
                wifiipmodestatus.Text = "";
            }));
            rststatustxt.Visible = false;
            nodenwparam mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.wifiipmode);
            if (null == mdp)
            {
                //读取WIfi的IP模式
                this.Invoke(new Action(() => {
                    wifiipmodestatus.Visible = true;
                    wifiipmodestatus.Text = "read error";
                }));
            }
            else
            {
                this.Invoke(new Action(() =>
                {
                    wifiipmodestatus.Visible = true;
                    wifiipmodestatus.Text = "read ok";
                    wifiipmodecb.SelectedIndex = mdp.ipmode;
                }));
            }
        }
        /// <summary>
        /// 读取wifi的IP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void readstaticip_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() => {
                staticipstatus.Text = "";
            }));
            rststatustxt.Visible = false;
            nodenwparam mdp = ReadNodeParam(mnode, frm.MyUdpClient, nodeparm.wifistaticip);
            if (null == mdp)
            {
                this.Invoke(new Action(() => {
                    staticipstatus.Visible = true;
                    staticipstatus.Text = "read error";
                }));
            }
            else
            {
                this.Invoke(new Action(() =>
                {
                    staticipstatus.Visible = true;
                    staticipstatus.Text = "read ok";
                    staticiptx.Text = mdp.nodeip[0].ToString() + "." + mdp.nodeip[1].ToString() + "." + mdp.nodeip[2].ToString() + "." + mdp.nodeip[3].ToString();
                }));
            }
        }
        //设置Wifi节点的ip模式
        private void setwifiipmode_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() => {
                wifiipmodestatus.Text = "";
            }));
            rststatustxt.Visible = false;
            byte[] ipmode = new byte[1];

            ipmode[0] = (byte)(wifiipmodecb.SelectedIndex == 0 ? 0x01:0x02);
            if (SetNodeParam(mnode, frm.MyUdpClient, nodeparm.wifiipmode, ipmode))
            {
                this.Invoke(new Action(() => {
                    wifiipmodestatus.Visible = true;
                    wifiipmodestatus.Text = "set ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() =>
                {
                    wifiipmodestatus.Visible = true;
                    wifiipmodestatus.Text = "set error";
                }));
            }

        }

        private void setstaticip_Click(object sender, EventArgs e)
        {

            String strip = staticiptx.Text;
            if ("".Equals(strip))
            {
                MessageBox.Show("Sorry IP address cannot be empty!");
                return;
            }
            IPAddress ipaddr = null;
            if (!IPAddress.TryParse(strip, out ipaddr))
            {
                MessageBox.Show("Sorry, the IP format is wrong!");
                return;
            }
            this.Invoke(new Action(() =>
            {
                staticipstatus.Text = "";
            }));
            rststatustxt.Visible = false;
            //获取IP地址的数组
            byte[] ips = ipaddr.GetAddressBytes();
            if (SetNodeParam(mnode, frm.MyUdpClient, nodeparm.wifistaticip, ips))
            {
                this.Invoke(new Action(() => {
                    staticipstatus.Visible = true;
                    staticipstatus.Text = "set ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    staticipstatus.Visible = true;
                    staticipstatus.Text = "set error";
                }));
            }
        }
        //搜索指定的Wifi信号强度
        private void searchwifisigbtn_Click(object sender, EventArgs e)
        {
            String strapid = wifisigtx.Text;
            if ("".Equals(strapid))
            {
                MessageBox.Show("The name of the WIFI cannot be empty！");
                return;
            }
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(strapid);
            if (bytes.Length <= 0)
            {
                MessageBox.Show("The name of the WIFI cannot be empty！");
                return;
            }
            this.Invoke(new Action(() => {
                rststatustxt.Visible = false;
                wifisigstatus.Text = "";
            }));
            
            if (SearchWifiSig(mnode, frm.MyUdpClient, bytes))
            {
                this.Invoke(new Action(() => {
                    wifisigstatus.Visible = true;
                    sigvaluetx.Text = "The signal value: " + ((SysParam.mntparm.wifisig != 0xFF) ? SysParam.mntparm.wifisig.ToString() : "error");
                    wifisigstatus.Text = "search ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() =>
                {
                    wifisigstatus.Visible = true;
                    sigvaluetx.Text = "The signal value: error" ;
                    wifisigstatus.Text = "search error";
                }));
            }
        }
        /// <summary>
        /// 搜索Wifi的信号强度
        /// </summary>
        /// <param name="desdev"></param>
        /// <param name="client"></param>
        /// <param name="mparams"></param>
        /// <returns></returns>
        private bool SearchWifiSig(NodeMsg desdev, UdpClient client, byte[] mparams)
        { 
            /* 查询节点周围指定wifi的信号强度
             * PC --> Node: F1 + 15 + ID + WIFISSID[32 byte] + CS + F2
             */
            byte[] bytes = new byte[38];
            bytes[0] = 0xF1;
            bytes[1] = 0x15;
            System.Buffer.BlockCopy(desdev.ID, 0, bytes, 2, 2);
            System.Buffer.BlockCopy(mparams, 0, bytes, 4, mparams.Length);
            bytes[bytes.Length - 2] = 0;
            for (int i = 0; i < bytes.Length - 2; i++ )
            {
                bytes[bytes.Length - 2] += bytes[i];
            }
            bytes[bytes.Length - 1] = 0xF2;
            SysParam.cbparm = false;

            int tickcount = Environment.TickCount;
            client.Send(bytes, bytes.Length, desdev.mendpoint);
            while (Environment.TickCount - tickcount < ConstInfor.recnwtimeout * 100 && !SysParam.cbparm)
            {
                Thread.Sleep(1);
            }
            if (!SysParam.cbparm)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    enum nodeparm
    { 
        serverip,
        serverport,
        nodechannel,//node信道
        wifiname,
        wifipsw,
        wifiipmode,
        wifistaticip,
        nodemode,
        nodeip,
        submask,
        gateway,
        rst
    }
    //Hex文件讯息
    class hexfileinfor
    {
        public string strfilepath;
        public long filesize;
        public UInt32 version;
        public UInt32 type;

        public const UInt32 nodetype = 0x18160100;
        public const UInt32 refertype = 0x180F0200;

        public string GetDev()
        {
            string strdev = "";
            switch (type)
            {
                case 0x18160101:
                    strdev = "ZB2530-01PA/02PA/WIFI_V1.0(NODE)";
                    break;
                case 0x18160102:
                    strdev = "ZB2530-Lan_V02.02(NODE)";
                    break;
                case 0x18160103:
                    strdev = "ZB2530-LAN-04_V01.01(NODE)";
                    break;
                case 0x18160104:
                    strdev = "ZB2530-WIFI-04_V01.01(NODE)";
                    break;
                case 0x180F0201:
                    strdev = "ZB2530-01PA/02PA/WIFI_V1.00(REFER)";
                    break;
                case 0x180F0203:
                    strdev = "ZB2530-LAN/WIFI-04_V01.01(REFER)";
                    break;
                case 0x180F0202:
                    strdev = "ZB2530-03_V1.0(REFER)";
                    break;
                case 0x180F0204:
                    strdev = "ZB2530-04_V1.1(REFER)";
                    break;
                default:
                    break;
            }
            return strdev;
        }
    }
    //更新固件
    class ReadHexInfor
    {
        //获取文件讯息
        public static void getfileinfor(string filepath, out hexfileinfor mhf)
        {
            mhf = new hexfileinfor();
            //获取文件路径
            mhf.strfilepath = filepath;
            FileStream mfstrm = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            //获取文件长度
            mhf.filesize = mfstrm.Length;
            //读取文件缓存到buff
            byte[] bytes = new byte[mhf.filesize];
            try
            {
                mfstrm.Read(bytes, 0, (int)mhf.filesize);
            }
            catch (Exception)
            {

            }
            finally
            {
                if (null != mfstrm)
                    mfstrm.Close();
                mfstrm = null;
            }
            //找到保存文件类型位置
            int start, len, index;
            findplace(bytes, ConstInfor.imgplace, out start, out len, out index);
            if (start < 0 || len <= 0 || index < 0)
            {
                MessageBox.Show("Sorry, file type error!");
                return;
            }
            int sp = 0;
            byte type1, type2, type3, type4;
            int baseplace = index + 8 + (ConstInfor.imgplace - start) * 2;
            if (baseplace < index + 8 + len * 2)
            {
                type1 = (byte)(HexToInt(bytes[baseplace + 1]) << 4 | HexToInt(bytes[baseplace + 2]));
            }
            else
            {
                sp = len * 2 - 2 * (ConstInfor.imgplace - start);
                type1 = (byte)(HexToInt(bytes[index + 8 + 2 + len * 2 + 12 + (0 - sp)]) << 4 | HexToInt(bytes[index + 8 + 3 + len * 2 + 12 + (0 - sp)]));
            }
            if (baseplace + 2 < index + 8 + len * 2)
            {
                type2 = (byte)(HexToInt(bytes[baseplace + 3]) << 4 | HexToInt(bytes[baseplace + 4]));
            }
            else
            {
                sp = len * 2 - 2 * (ConstInfor.imgplace - start);
                type2 = (byte)(HexToInt(bytes[index + 8 + 2 + len * 2 + 12 + (2 - sp)]) << 4 | HexToInt(bytes[index + 8 + 3 + len * 2 + 12 + (2 - sp)]));
            }

            if (baseplace + 4 < index + 8 + len * 2)
            {
                type3 = (byte)(HexToInt(bytes[baseplace + 5]) << 4 | HexToInt(bytes[baseplace + 6]));
            }
            else
            {
                sp = len * 2 - 2 * (ConstInfor.imgplace - start);
                type3 = (byte)(HexToInt(bytes[index + 8 + 2 + len * 2 + 12 + (4 - sp)]) << 4 | HexToInt(bytes[index + 8 + 3 + len * 2 + 12 + (4 - sp)]));
            }

            if (baseplace + 6 < index + 8 + len * 2)
            {
                type4 = (byte)(HexToInt(bytes[baseplace + 7]) << 4 | HexToInt(bytes[baseplace + 8]));
            }
            else
            {
                sp = len * 2 - 2 * (ConstInfor.imgplace - start);

                type4 = (byte)(HexToInt(bytes[index + 8 + 2 + len * 2 + 12 + (6 - sp)]) << 4 | HexToInt(bytes[index + 8 + 3 + len * 2 + 12 + (6 - sp)]));
            }
            mhf.type = (UInt32)(type4 << 24 | type3 << 16 | type2 << 8 | type1);
            //找到保存文件的版本位置
            findplace(bytes, ConstInfor.verplace, out start, out len, out index);
            if (start < 0 || len <= 0)
            {
                MessageBox.Show("Sorry, file version error");
                return;
            }
            baseplace = index + 8 + (ConstInfor.verplace - start) * 2;
            if (baseplace < index + 8 + len * 2)
            {
                type1 = (byte)(HexToInt(bytes[baseplace + 1]) << 4 | HexToInt(bytes[baseplace + 2]));
            }
            else
            {
                sp = len * 2 - 2 * (ConstInfor.verplace - start);
                type1 = (byte)(HexToInt(bytes[index + 8 + 2 + len * 2 + 12 + (0 - sp)]) << 4 | HexToInt(bytes[index + 8 + 3 + len * 2 + 12 + (0 - sp)]));
            }
            if (baseplace + 2 < index + 8 + len * 2)
            {
                type2 = (byte)(HexToInt(bytes[baseplace + 3]) << 4 | HexToInt(bytes[baseplace + 4]));
            }
            else
            {
                sp = len * 2 - 2 * (ConstInfor.verplace - start);
                type2 = (byte)(HexToInt(bytes[index + 8 + 2 + len * 2 + 12 + (2 - sp)]) << 4 | HexToInt(bytes[index + 8 + 3 + len * 2 + 12 + (2 - sp)]));
            }

            if (baseplace + 4 < index + 8 + len * 2)
            {
                type3 = (byte)(HexToInt(bytes[baseplace + 5]) << 4 | HexToInt(bytes[baseplace + 6]));
            }
            else
            {
                sp = len * 2 - 2 * (ConstInfor.verplace - start);
                type3 = (byte)(HexToInt(bytes[index + 8 + 2 + len * 2 + 12 + (4 - sp)]) << 4 | HexToInt(bytes[index + 8 + 3 + len * 2 + 12 + (4 - sp)]));
            }

            if (baseplace + 6 < index + 8 + len * 2)
            {
                type4 = (byte)(HexToInt(bytes[baseplace + 7]) << 4 | HexToInt(bytes[baseplace + 8]));
            }
            else
            {
                sp = len * 2 - 2 * (ConstInfor.verplace - start);

                type4 = (byte)(HexToInt(bytes[index + 8 + 2 + len * 2 + 12 + (6 - sp)]) << 4 | HexToInt(bytes[index + 8 + 3 + len * 2 + 12 + (6 - sp)]));
            }
            mhf.version = (UInt32)(type4 << 24 | type3 << 16 | type2 << 8 | type1);
        }
        //查找版本及类型在数组中的位置
        public static void findplace(byte[] buff, int place, out int start, out int len, out int index)
        {
            start = -1;
            len = -1;
            index = -1;
            for (int i = 0; i < buff.Length; i++)
            {
                if (buff[i] == ':')
                {
                    //判断后面一个地址
                    len = (HexToInt(buff[i + 1]) << 4 | HexToInt(buff[i + 2]));
                    start = (int)HexToInt(buff[i + 3]) << 12 | HexToInt(buff[i + 4]) << 8 | HexToInt(buff[i + 5]) << 4 | HexToInt(buff[i + 6]);
                    if (place >= start && place <= start + len)
                    {
                        index = i;
                        return;
                    }
                }
            }
        }
        public static int HexToInt(byte ch)
        {
            switch (ch)
            {
                case 0x30: return 0;
                case 0x31: return 1;
                case 0x32: return 2;
                case 0x33: return 3;
                case 0x34: return 4;
                case 0x35: return 5;
                case 0x36: return 6;
                case 0x37: return 7;
                case 0x38: return 8;
                case 0x39: return 9;
                case 0x41: return 10;
                case 0x42: return 11;
                case 0x43: return 12;
                case 0x44: return 13;
                case 0x45: return 14;
                case 0x46: return 15;
            }
            return -1;
        }

        public static void datatohex(byte[] bytes, out byte[] rowhexdatas)
        {
            rowhexdatas = new byte[bytes.Length / 2];
            for (int i = 0; i < bytes.Length; i += 2)
            {
                rowhexdatas[i / 2] = (byte)(HexToInt(bytes[i]) << 4 | HexToInt(bytes[i + 1]));
            }
        }
        //校验hex数据,rowhexs不包含":"
        public static bool checkhex(byte[] rowhexs)
        {
            byte chs = 0;
            //先计算它们的校验和
            byte[] bytes = new byte[rowhexs.Length / 2];
            for (int i = 0; i < rowhexs.Length; i += 2)
            {
                chs = (byte)(HexToInt(rowhexs[i]) << 4 | HexToInt(rowhexs[i + 1]));
                //计算校验和
                bytes[i / 2] = chs;
            }
            int cssum = 0;
            for (int i = 0; i < bytes.Length - 1; i++)
            {
                cssum += bytes[i];
            }
            byte cs = 0;
            //计算校验值
            cs = (byte)(0x100 - (cssum % 256));
            if (cs == bytes[bytes.Length - 1])
            {
                return true;
            }
            return false;
        }
        public static int HexToBin(byte[] bytes, out byte[] mbins, out int mbinslen)
        {
            mbins = new byte[bytes.Length];
            for (int i = 0; i < mbins.Length; i++)
            {
                mbins[i] = 0xFF;
            }
            mbinslen = 0;
            byte[] rows = null, rowdatas = null, rowhexdatas = null;
            UInt16 rowlen = 0, rowplace = 0, rowtype = 0;
            int baseaddr = 0, curaddr = 0;
            //先获取纯数据长度
            for (int i = 0; i < bytes.Length; i++)
            {
                //判断是一行开头
                if (':' == bytes[i])
                {   //获取行数据长度
                    rowlen = (byte)(HexToInt(bytes[i + 1]) << 4 | HexToInt(bytes[i + 2]));
                    //获取行起始位置，可能是偏移位置
                    rowplace = (UInt16)(HexToInt(bytes[i + 3]) << 12 | HexToInt(bytes[i + 4]) << 8 | HexToInt(bytes[i + 5]) << 4 | HexToInt(bytes[i + 6]));
                    //获取行类型
                    rowtype = (byte)(HexToInt(bytes[i + 7]) << 4 | HexToInt(bytes[i + 8]));
                    //校验行
                    rows = new byte[(rowlen + 5) * 2];
                    System.Buffer.BlockCopy(bytes, i + 1, rows, 0, rows.Length);
                    if (!checkhex(rows))
                    {
                        return -1;
                    }
                    if (bytes[i + 8 + rowlen * 2 + 2 + 1] != 0x0d || bytes[i + 8 + rowlen * 2 + 2 + 2] != 0x0a)
                    {
                        return -2;
                    }
                    if (rowtype == 0x00)
                    {
                        //将数据拷贝到缓存中mbins
                        rowdatas = new byte[rowlen * 2];
                        for (int j = 0; j < rowdatas.Length; j++)
                            rowdatas[j] = 0xFF;

                        System.Buffer.BlockCopy(bytes, i + 9, rowdatas, 0, rowdatas.Length);
                        datatohex(rowdatas, out rowhexdatas);
                        //拷贝到总数组中
                        curaddr = baseaddr + rowplace - ConstInfor.baseaddr;
                        if (curaddr < 0)
                            continue;
                        //mbins
                        System.Buffer.BlockCopy(rowhexdatas, 0, mbins, curaddr, rowhexdatas.Length);
                        mbinslen = curaddr + rowhexdatas.Length;
                    }
                    else if (rowtype == 0x01)
                    {
                        break;
                    }
                    else if (rowtype == 0x02)
                    {
                    }
                    else if (rowtype == 0x03)
                    {
                    }
                    else if (rowtype == 0x04)
                    {
                        //基地址左移16位
                        baseaddr = HexToInt(bytes[i + 12]) << 16;
                    }
                    else if (rowtype == 0x05)
                    {

                    }
                }
            }
            return 1;
        }
    }
}
