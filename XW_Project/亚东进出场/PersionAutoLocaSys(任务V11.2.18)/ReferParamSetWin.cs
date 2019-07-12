using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace PersionAutoLocaSys
{
    public partial class ReferParamSetWin : Form
    {
        NodeMsg mrefer = null;
        Form1 frm = null;
        hexfileinfor curhexmg = null;
        byte[] mbins = null;
        int binlen = 0;
        FileStream mfstrm = null;
        Thread mythread = null;
        public ReferParamSetWin()
        {
            InitializeComponent();
        }
        public ReferParamSetWin(Form1 frm, NodeMsg mrf)
        {
            InitializeComponent();
            mrefer = mrf;
            this.frm = frm;
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == ConstInfor.FORMMESSAGE && (int)m.WParam == ConstInfor.CLOSEMSGPARAM)
            {
                PersonOperation curpersonoper = new PersonOperation(frm.CurPerson.ID, OperType.LeaveReferParam);
                CommonCollection.personOpers.Add(curpersonoper);
            }
            base.WndProc(ref m);
        }
        private void ReferParamSetWin_Load(object sender, EventArgs e)
        {
            //記錄当前人员进入Node参数设置界面的操作
            PersonOperation curpersonoper = new PersonOperation(frm.CurPerson.ID, OperType.EnterReferParam);
            CommonCollection.personOpers.Add(curpersonoper);
            string strrefermsg = "";
            string strid = mrefer.ID[0].ToString("X2") + mrefer.ID[1].ToString("X2");
            string strname = CommonBoxOperation.GetRouterName(strid);
            rstlb.Visible = false;
            if (null == strname || "".Equals(strname))
            {
                strrefermsg = strid;
            }
            else
            {
                strrefermsg = strname + "(" + strid + ")";
            }
            refermsgtxt.Text = strrefermsg;
            eqpttypetxt.Text = ConstInfor.GetDevType(NodeType.ReferNode,mrefer.type);
            vertxt.Text = ((byte)(mrefer.Version >> 24)).ToString("D2") + ((byte)(mrefer.Version >> 16)).ToString("D2") + ((byte)(mrefer.Version >> 8)).ToString("D2") + ((byte)(mrefer.Version)).ToString("X2"); 
            /*读取参数信息
             */
            //读取信号阀值
            referparam mrfpm = null;
            mrfpm = ReadReferParam(mrefer, frm.MyUdpClient, referparm.Sgthreshold);
            if (null == mrfpm)
            {
                sthdlb.Text = "read error";
            }
            else
            {
                sthdlb.Text = "read ok";
                sthdtxt.Text = mrfpm.Sgthreshold + "";
            }
            //读取信号强度系数
            mrfpm = ReadReferParam(mrefer, frm.MyUdpClient, referparm.Sgstrengthfac);
            if (null == mrfpm)
            {
                ssfaclb.Text = "read error";
            }
            else
            {
                ssfaclb.Text = "read ok";
                ssfactxt.Text = string.Format("{0:F2}", (double)mrfpm.Sgstrengthfac / 100);
            }
        }
        private bool SetReferParam(NodeMsg desdev, UdpClient client, referparm type, byte[] mparams)
        {
            byte[] bytes = null;
            //初始化设置参数及发送数组
            switch (type)
            {
                case referparm.Sgthreshold:
                    //PC--->Node:F1 + 20 + ReferID(2byte) + SigThreshold(1byte) + CS +F2
                    bytes = new byte[7];
                    bytes[1] = 0x20;
                    bytes[4] = mparams[0];
                    break;
                case referparm.Sgstrengthfac:
                    //PC--->Node:F1 + 22 + ReferID(2byte) + Sigk(1byte) + CS +F2
                    bytes = new byte[7];
                    bytes[1] = 0x22;
                    bytes[4] = mparams[0];
                    break;
                default:
                    return false;
            }
            //赋值ID
            System.Buffer.BlockCopy(desdev.ID, 0, bytes, 2, 2);
            //设置包头包尾
            bytes[0] = 0xF1; bytes[bytes.Length - 1] = 0xF2;
            //设置校验和
            bytes[bytes.Length - 2] = 0;
            for (int i = 0; i < bytes.Length - 2; i++)
                bytes[bytes.Length - 2] += bytes[i];
            //传送固件数据
            SysParam.cbparm = false;
            SysParam.sendcount = 3;
            while (true)
            {
                SysParam.tickcount = Environment.TickCount;
                SysParam.sendcount--;
                client.Send(bytes, bytes.Length, desdev.mendpoint);
                while (Environment.TickCount - SysParam.tickcount < ConstInfor.recnwtimeout && (!SysParam.cbparm))
                {
                    Thread.Sleep(1);
                }
                if (SysParam.cbparm || SysParam.sendcount<=0)
                {
                    break;
                }
            }
            return SysParam.cbparm;
        }
        private referparam ReadReferParam(NodeMsg desdev, UdpClient client, referparm type)
        {
            byte[] bytes = new byte[6];
            bytes[0] = 0xF1; bytes[bytes.Length - 1] = 0xF2;
            System.Buffer.BlockCopy(desdev.ID, 0, bytes, 2, 2);
            switch (type)
            {
                case referparm.Sgthreshold: bytes[1] = 0x21; break;
                case referparm.Sgstrengthfac: bytes[1] = 0x23; break;
                case referparm.Reset: bytes[1] = 0x27; break;
            }
            bytes[bytes.Length - 2] = 0;
            for (int i = 0; i < bytes.Length - 2; i++)
            {
                bytes[bytes.Length - 2] += bytes[i];
            }
            SysParam.cbparm = false;
            SysParam.sendcount = 3;
            while(true)
            {
                SysParam.tickcount = Environment.TickCount;
                SysParam.sendcount--;
                client.Send(bytes, bytes.Length, desdev.mendpoint);
                while (Environment.TickCount - SysParam.tickcount < ConstInfor.recrefertimeout && (!SysParam.cbparm))
                {
                    Thread.Sleep(1);
                }
                if (SysParam.sendcount <= 0 || SysParam.cbparm)
                {
                    break;
                }
            }
            if (!SysParam.cbparm)
            {
                return null;
            }
            return SysParam.mfrprm;
        }
        //读取信号阀值
        private void readsthdbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                sthdlb.Text = "";
            }));
            //设置信号阀值
            referparam mrfpm = null;
            mrfpm = ReadReferParam(mrefer, frm.MyUdpClient, referparm.Sgthreshold);
            if (null == mrfpm)
            {
                this.Invoke(new Action(() => {
                    sthdlb.Text = "read error";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    sthdlb.Text = "read ok";
                    sthdtxt.Text = mrfpm.Sgthreshold + "";
                }));
            }
        }
        //读取信号系数
        private void readssfacbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                ssfaclb.Text = "";
            }));
            //设置信号强度系数
            referparam mrfpm = null;
            mrfpm = ReadReferParam(mrefer, frm.MyUdpClient, referparm.Sgstrengthfac);
            if (null == mrfpm)
            {
                this.Invoke(new Action(() => {
                    ssfaclb.Text = "read error";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    ssfaclb.Text = "read ok";
                    ssfactxt.Text = string.Format("{0:F2}", (double)mrfpm.Sgstrengthfac / 100);
                }));
            }
        }
        //设置信号阀值
        private void setsthdbtn_Click(object sender, EventArgs e)
        {
            string strsthd = sthdtxt.Text;
            if ("".Equals(strsthd))
            {
                MessageBox.Show("The signal threshold cannot be empty!");
                return;
            }
            byte[] prams = new byte[1];
            try 
            {
                prams[0] = Convert.ToByte(strsthd);
            }catch(Exception)
            {
                MessageBox.Show("The signal strength threshold format is not correct!");
                return;
            }
            this.Invoke(new Action(() =>
            {
                sthdlb.Text = "";
            }));
            if(SetReferParam(mrefer, frm.MyUdpClient, referparm.Sgthreshold, prams))
            {
                this.Invoke(new Action(() => {
                    sthdlb.Text = "set ok";
                }));
            }else
            {
                this.Invoke(new Action(() => {
                    sthdlb.Text = "set error";
                }));
            }
        }
        //设置信号系数
        private void setssfacbtn_Click(object sender, EventArgs e)
        {
            string strssfac = ssfactxt.Text;
            if ("".Equals(strssfac))
            {
                MessageBox.Show("The signal strength coefficient cannot be empty!");
                return;
            }
            double prm = 0.0;
            try 
            {
                prm = Convert.ToDouble(strssfac);
            }catch(Exception)
            {
                MessageBox.Show("The signal strength coefficient format is wrong!");
                return;
            }
            if (prm < 0 || prm > 2.55)
            {
                MessageBox.Show("The range of signal strength is between 0 and 2.55!");
                return;
            }
            this.Invoke(new Action(() =>
            {
                ssfaclb.Text = "";
            }));
            byte[] prams = new byte[1];
            prams[0] = (byte)(prm * 100);
            if (SetReferParam(mrefer, frm.MyUdpClient, referparm.Sgstrengthfac, prams))
            {
                this.Invoke(new Action(() => {
                    ssfaclb.Text = "set ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    ssfaclb.Text = "set error";
                }));
            }
        }
        //选择Hex文件
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
                versiontxt.Text = ((byte)(curhexmg.version >> 24)).ToString("D2") + ((byte)(curhexmg.version >> 16)).ToString("D2") + ((byte)(curhexmg.version >> 8)).ToString("D2") + ((byte)(curhexmg.version)).ToString("X2");
                devtypetxt.Text = curhexmg.GetDev();
            }
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
             * PC --> Node：F1 + 25 + ID(2byte) + Addr(2byte) + Data(64) + CS + F2
             * Node --> PC：FA + 25 + NodeID(2byte) + ID(2byte) + Addr(2byte) + CS + FB
             * --------------------------------------------------------------------------
             * 对节点设备传送固件数据完成，并要求节点做固件更新
             * PC --> Node：F1 + 26 + ID(2byte) + Len(2byte) + CS_FW(4byte) + CS + F2
             * Node --> PC：FA + 26 + NodeID(2byte) + ID + Status(1byte) + CS + FB
             * --------------------------------------------------------------------------
             * */
            byte[] bytes = new byte[72];
            bytes[0] = 0xF1; bytes[1] = 0x25; bytes[bytes.Length - 1] = 0xF2;
            System.Buffer.BlockCopy(mrefer.ID, 0, bytes, 2, 2);
            int startaddr = 0;
            while (true)
            {
                bytes[4] = (byte)(startaddr >> 8); bytes[5] = (byte)(startaddr);
                SysParam.currentaddr = startaddr;
                System.Buffer.BlockCopy(mbins, startaddr, bytes, 6, 64);
                bytes[bytes.Length - 2] = 0;
                for (int i = 0; i < bytes.Length - 2; i++)
                {
                    bytes[bytes.Length - 2] += bytes[i];
                }
                //可以发送数据了
                SysParam.sendcount = 10;
                SysParam.cbparm = false;
                while (true)
                {
                    SysParam.sendcount--;
                    SysParam.tickcount = Environment.TickCount;
                    frm.MyUdpClient.Send(bytes, bytes.Length, mrefer.mendpoint);
                    while (Environment.TickCount - SysParam.tickcount < ConstInfor.recrefertimeout + 200 && !SysParam.cbparm)
                    {
                        Thread.Sleep(1);
                    }
                    //此时若收到回复的数据包，可以跳出循环了
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
                                uppgtxt.Text = string.Format("{0:F0}", (((double)startaddr / binlen)) * 100) + "%";
                            }));
                        }catch(Exception)
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
            //PC --> Node：F1 + 26 + ID(2byte) + Len(2byte) + CS_FW(4byte) + CS + F2
            bytes = new byte[12];
            bytes[0] = 0xF1; bytes[1] = 0x26; bytes[bytes.Length - 1] = 0xF2;
            System.Buffer.BlockCopy(mrefer.ID, 0, bytes, 2, 2);
            bytes[4] = (byte)(binlen >> 8);
            bytes[5] = (byte)(binlen);
            UInt32 sumcs = 0;
            for (int i = 0; i < binlen; i++)
            {
                sumcs += mbins[i];
            }
            bytes[6] = (byte)(sumcs >> 24); bytes[7] = (byte)(sumcs >> 16);
            bytes[8] = (byte)(sumcs >> 8); bytes[9] = (byte)(sumcs);
            bytes[bytes.Length - 2] = 0;
            for (int i = 0; i < bytes.Length - 2; i++)
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
                frm.MyUdpClient.Send(bytes, bytes.Length, mrefer.mendpoint);
                while (Environment.TickCount - SysParam.tickcount < ConstInfor.recrefertimeout + 500 && !SysParam.cbparm)
                {
                    Thread.Sleep(1);
                }
                if (SysParam.cbparm || SysParam.sendcount <= 0)
                {
                    break;
                }
                Thread.Sleep(5);
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
                        mrefer.Version = curhexmg.version;
                    }));
                }
            }
        }
        private void updatebtn_Click(object sender, EventArgs e)
        {
            if (!frm.CurPerson.Name.ToUpper().Equals(ConstInfor.dmatekname.ToUpper()))
            {
                //点击更新时需要输入密码
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
            if (curhexmg.type != (mrefer.type | hexfileinfor.refertype))
            {
                MessageBox.Show("Sorry, the firmware device type cannot be updated!");
                return;
            }
            if (curhexmg.version == mrefer.Version)
            {
                MessageBox.Show("Sorry, the firmware version cannot be updated!");
                return;
            }
            updateimage(strpath);
        }
        //复位
        private void resetbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                rstlb.Text = "";
            }));
            //复位
            referparam mrfpm = null;
            mrfpm = ReadReferParam(mrefer, frm.MyUdpClient, referparm.Reset);
            if (null == mrfpm)
            {
                this.Invoke(new Action(() => {
                    rstlb.Text = "read error";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    rstlb.Text = "read ok";
                }));
            }
            rstlb.Visible = true;
        }

        private void ReferParamSetWin_FormClosing(object sender, FormClosingEventArgs e)
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
    }
    enum referparm
    {
        Sgthreshold,
        Sgstrengthfac,
        Reset
    }
}
