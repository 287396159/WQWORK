using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Timer = System.Windows.Forms.Timer;
using System.Net;
namespace PersionAutoLocaSys
{
    public partial class NodeSettingDevWin : Form
    {
        private Form1 frm = null;
        private NodeMsg nodemsg = null;
        private Timer updatetimer = null;
        private nodenwparam ndparam = null;
        private referparam referparam = null;
        private NodeType CurSelectType = NodeType.DataNode;
        public NodeSettingDevWin()
        {
            InitializeComponent();
        }
        public NodeSettingDevWin(Form1 frm, NodeMsg node)
        {
            this.frm = frm;
            this.nodemsg = node;
            InitializeComponent();
        }
        private void NodeSettingDevWin_Load(object sender, EventArgs e)
        {
            PersonOperation curpersonoper = new PersonOperation(frm.CurPerson.ID, OperType.DebugSetAroundDeviceParam);
            CommonCollection.personOpers.Add(curpersonoper);

            StringBuilder strbuilder = new StringBuilder("Node ID：");
            strbuilder.Append(nodemsg.ID[0].ToString("X2"));
            strbuilder.Append(nodemsg.ID[1].ToString("X2"));
            nodeidlb.Text = strbuilder.ToString();
            strbuilder.Clear();
            strbuilder.Append("Version：");
            //
            strbuilder.Append(((byte)(nodemsg.Version >> 24)).ToString("D2") + ((byte)(nodemsg.Version >> 16)).ToString("D2") + ((byte)(nodemsg.Version >> 8)).ToString("D2") + ((byte)(nodemsg.Version)).ToString("X2"));
            verlb.Text = strbuilder.ToString();
            strbuilder.Clear();
            strbuilder.Append("Type：");
            strbuilder.Append(ConstInfor.GetDevType(NodeType.DataNode, nodemsg.type));
            typelb.Text = strbuilder.ToString();
            selecttypecb.SelectedIndex = 0;
            CurSelectType = NodeType.DataNode;
            updatetimer = new Timer();
            updatetimer.Interval = 800;
            updatetimer.Tick += updatetimer_Tick;
            updatetimer.Start();
            nodepanel.Visible = true;
            nodepanel.Visible = true;
            //查询Wifi信号强度的区域不可见
            wifisigpanel.Visible = false;

            referpanal.Visible = false;
            nodepanel.Enabled = false;
            referpanal.Enabled = false;
            nodepanel.Location = new Point(285, 12);
            nodeparambox.Location = new Point(7, 215);
            rstbtn.Location = new Point(254, 373);

            nodeparambox.Visible = true;
            referpanal.Visible = false;
            if (!isSearchingNodeDev)
            {
                System.Threading.Tasks.Task.Factory.StartNew(SearchNodeDevices);
            }
        }

        private void updatetimer_Tick(Object obj,EventArgs args)
        {
            if (CurSelectType == NodeType.DataNode)
            {
                List<KeyValuePair<string, nodenwparam>> nodedevices = SysParam.mdevices.nodedevs.OrderBy(k => k.Key).ToList<KeyValuePair<string, nodenwparam>>();
                foreach (KeyValuePair<string, nodenwparam> refparam in nodedevices)
                {
                    if (devlistview.Items.ContainsKey(refparam.Key))
                    {
                        ListViewItem[] items = devlistview.Items.Find(refparam.Key, false);
                        if (items.Length <= 0)
                        {
                            continue;
                        }
                        if (refparam.Value.type != byte.MaxValue)
                        {
                            items[0].SubItems[1].Text = ConstInfor.GetDevType(NodeType.DataNode, refparam.Value.type);
                        }
                        else
                        {
                            items[0].SubItems[1].Text = "****";
                        }
                        if (refparam.Value.version != UInt32.MaxValue)
                        {
                            items[0].SubItems[2].Text = ((byte)(refparam.Value.version >> 24)).ToString("D2") + ((byte)(refparam.Value.version >> 16)).ToString("D2") + ((byte)(refparam.Value.version >> 8)).ToString("D2") + ((byte)(refparam.Value.version)).ToString("X2");
                        }
                        else
                        {
                            items[0].SubItems[2].Text = "****";
                        }
                        continue;
                    }
                    ListViewItem item = new ListViewItem();
                    item.Text = refparam.Key;
                    item.Name = refparam.Key;
                    if (refparam.Value.type != byte.MaxValue)
                    {
                        item.SubItems.Add(ConstInfor.GetDevType(NodeType.DataNode, refparam.Value.type));
                    }
                    else
                    {
                        item.SubItems.Add("****");
                    }
                    if (refparam.Value.version != UInt32.MaxValue)
                    {
                        item.SubItems.Add(((byte)(refparam.Value.version >> 24)).ToString("D2") + ((byte)(refparam.Value.version >> 16)).ToString("D2") + ((byte)(refparam.Value.version >> 8)).ToString("D2") + ((byte)(refparam.Value.version)).ToString("X2"));
                    }
                    else
                    {
                        item.SubItems.Add("****");
                    }
                    devlistview.Items.Add(item);
                }
            }
            else
            {
                List<KeyValuePair<string,referparam>> referparams =  SysParam.mdevices.referdevs.OrderBy(k => k.Key).ToList<KeyValuePair<string, referparam>>();
                foreach (KeyValuePair<string, referparam> refparams in referparams)
                {
                    if (devlistview.Items.ContainsKey(refparams.Key))
                    {
                        ListViewItem[] items = devlistview.Items.Find(refparams.Key, false);
                        if (items.Length <= 0)
                        {
                            continue;
                        }
                        if (refparams.Value.type != byte.MaxValue)
                        {
                            items[0].SubItems[1].Text = ConstInfor.GetDevType(NodeType.ReferNode, refparams.Value.type);
                        }
                        else
                        {
                            items[0].SubItems[1].Text = "****";
                        }
                        if (refparams.Value.version != UInt32.MaxValue)
                        {
                            items[0].SubItems[2].Text = ((byte)(refparams.Value.version >> 24)).ToString("D2") + ((byte)(refparams.Value.version >> 16)).ToString("D2") + ((byte)(refparams.Value.version >> 8)).ToString("D2") + ((byte)(refparams.Value.version)).ToString("X2");
                        }
                        else
                        {
                            items[0].SubItems[2].Text = "****";
                        }
                        continue;
                    }
                    ListViewItem item = new ListViewItem();
                    item.Text = refparams.Key;
                    item.Name = refparams.Key;
                    if (refparams.Value.type != byte.MaxValue)
                    {
                        item.SubItems.Add(ConstInfor.GetDevType(NodeType.ReferNode, refparams.Value.type));
                    }
                    else
                    {
                        item.SubItems.Add("****");
                    }
                    if (refparams.Value.version != UInt32.MaxValue)
                    {
                        item.SubItems.Add(((byte)(refparams.Value.version >> 24)).ToString("D2") + ((byte)(refparams.Value.version >> 16)).ToString("D2") + ((byte)(refparams.Value.version >> 8)).ToString("D2") + ((byte)(refparams.Value.version)).ToString("X2"));
                    }
                    else
                    {
                        item.SubItems.Add("****");
                    }
                    devlistview.Items.Add(item);
                }
            }
        }
        /// <summary>
        /// 搜索设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchbtn_Click(object sender, EventArgs e)
        {
            if (selecttypecb.SelectedIndex == 0)
            {//搜索数据节点
                SysParam.mdevices.nodedevs.Clear();
                devlistview.Items.Clear();
                if (!isSearchingNodeDev)
                {//是否正在搜索数据节点设备
                    System.Threading.Tasks.Task.Factory.StartNew(SearchNodeDevices);
                }
            }
            else
            {//搜索参考点 
                SysParam.mdevices.referdevs.Clear();
                devlistview.Items.Clear();
                if (!isSearchingReferDev)
                {//是否正在搜索参考点设备
                    System.Threading.Tasks.Task.Factory.StartNew(SearchReferDevices);
                }
            }
        }
        private Boolean isSearchingReferDev = false;
        /// <summary>
        /// 搜索参考点设备
        /// </summary>
        private void SearchReferDevices()
        { /*查询周围的参考点设备ID
           * PC --> USB：F2 + 41 + CS + F1
           * */
           isSearchingReferDev = true;
           byte[] buff = new byte[4];
           buff[1] = 0x41;
           buff[0] = 0xF2;
           buff[buff.Length - 1] = 0xF1;
           buff[buff.Length - 2] = 0;
           for (int i = 0; i < buff.Length - 2; i++)
           {
               buff[buff.Length - 2] += buff[i];
           }
           SysParam.readmark = false;
           int tick = Environment.TickCount;
           try
           {
               frm.MyUdpClient.Send(buff, buff.Length, nodemsg.mendpoint);
               while (Environment.TickCount - tick < ConstInfor.recnwtimeout * 50 && !SysParam.readmark)
               {
                   Thread.Sleep(1);
               }
               if (!SysParam.readmark)
               {
                   return;
               }
               if (SysParam.mdevices.referdevs.Count > 0)
               {
                   referparam[] referdevices = SysParam.mdevices.referdevs.Values.ToArray<referparam>();
                   foreach (referparam refer in referdevices)
                   {
                       ReadDev(operationtype.referreadreferinfor, refer.id, 0);
                   }
               }
           }
           catch (Exception ex)
           {
               Console.WriteLine("搜索周围参考点时出现异常，异常原因:" + ex.ToString());
           }
           finally
           {
               isSearchingReferDev = false;
           }
        }
        private bool isSearchingNodeDev = false;
        /// <summary>
        /// 搜索数据节点设备
        /// </summary>
        private void SearchNodeDevices()
        {   
            isSearchingNodeDev = true;
            byte[] buff = new byte[4];
            buff[1] = 0x01;
            buff[0] = 0xF2;
            buff[buff.Length - 1] = 0xF1;
            buff[buff.Length - 2] = 0;
            for (int i = 0; i < buff.Length - 2; i++)
            {
                buff[buff.Length - 2] += buff[i];
            }
            SysParam.readmark = false;
            //搜索设备只发送一次即可
            int tick = Environment.TickCount;
            try
            {
                frm.MyUdpClient.Send(buff, buff.Length, nodemsg.mendpoint);
                //搜索时可以等长一点时间
                while (Environment.TickCount - tick < ConstInfor.recnwtimeout * 50 && !SysParam.readmark)
                {
                    Thread.Sleep(1);
                }
                //说明搜索到设备
                if (!SysParam.readmark)
                {
                    return;
                }
                if (SysParam.mdevices.nodedevs.Count > 0)
                {//搜索完成以后，重新查看设备类型
                    nodenwparam[] nodedevices = SysParam.mdevices.nodedevs.Values.ToArray<nodenwparam>();
                    for (int i = 0; i < nodedevices.Length; i++)
                    {
                        ReadDev(operationtype.nodereadnodeinfor, nodedevices[i].id, nodedevices[i].channel);
                    }
                }
            }
            catch (Exception ex)
            {
                FileOperation.WriteLog("搜索周围数据节点时出现异常!异常原因:" + ex.ToString());
            }
            finally
            {
                isSearchingNodeDev = false;
            }
        }
        /// <summary>
        /// 设置设备讯息
        /// </summary>
        /// <param name="curtype"></param>
        /// <param name="id"></param>
        /// <param name="bytes"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        private bool SetDev(operationtype curtype,byte[] id,byte[] bytes,byte channel)
        {
            byte[] buff = null;
            switch (curtype)
            {
                case operationtype.nodesetserverip:
                    /*设置节点的Server IP
                     * PC --> USB：F2 + 05 + ID  + channel+ IP+ CS + F1
                     */
                    buff = new byte[11];
                    buff[1] = 0x05;
                    System.Buffer.BlockCopy(id, 0, buff,2,2);
                    buff[4] = channel;
                    System.Buffer.BlockCopy(bytes, 0, buff, 5, 4);
                    break;
                case operationtype.nodesetserverport:
                    /*设置节点的Server Port
                     * PC --> USB：F2 + 07 + ID  + channel+ Port + CS + F1
                    */
                    buff = new byte[9];
                    buff[1] = 0x07;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    buff[4] = channel;
                    System.Buffer.BlockCopy(bytes, 0, buff, 5, 2);
                    break;
                case operationtype.nodesetselfipmode:
                    /*设置节点的IP模式
                     * PC --> USB：F2 + 0E + ID  + channel+ MODE + CS + F1
                    */
                    buff = new byte[8];
                    buff[1] = 0x0E;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    buff[4] = channel;
                    buff[5] = bytes[0];
                    break;
                case operationtype.nodesetselfip:
                    /*设置节点的IP
                     * PC --> USB：F2 + 10 + ID  + channel+ IP + CS + F1
                     * */
                    buff = new byte[11];
                    buff[1] = 0x10;
                    System.Buffer.BlockCopy(id, 0, buff,2,2);
                    buff[4] = channel;
                    System.Buffer.BlockCopy(bytes, 0, buff, 5, 4);
                    break;
                case operationtype.nodesetselfmask:
                    /*设置节点的SubMask
                     * PC --> USB：F2 + 12 + ID  + channel+ IP + CS + F1
                    * */
                    buff = new byte[11];
                    buff[1] = 0x12;
                    System.Buffer.BlockCopy(id, 0, buff,2,2);
                    buff[4] = channel;
                    System.Buffer.BlockCopy(bytes, 0, buff, 5, 4);
                    break;
                case operationtype.nodesetselfgateway:
                    /*设置节点的GateWay
                     * PC --> USB：F2 + 14 + ID  + channel+ IP + CS + F1
                    * */
                    buff = new byte[11];
                    buff[1] = 0x14;
                    System.Buffer.BlockCopy(id, 0, buff,2,2);
                    buff[4] = channel;
                    System.Buffer.BlockCopy(bytes, 0, buff, 5, 4);
                    break;
                case operationtype.nodesetwifiname:
                    /*设置节点的Wifi AP名称
                     * PC --> USB：F2 + 09 + ID + channel + Name + CS + F1
                    * */
                    buff = new byte[39];
                    buff[1] = 0x09;
                    System.Buffer.BlockCopy(id, 0, buff,2,2);
                    buff[4] = channel;
                    System.Buffer.BlockCopy(bytes, 0, buff, 5, bytes.Length);
                    break;
                case operationtype.nodesetwifipsw:
                    /*设置节点的Wifi AP密码
                     * PC --> USB：F2 + 0B + ID  + channel+ Password + CS + F1
                    * */
                    buff = new byte[39];
                    buff[1] = 0x0B;
                    System.Buffer.BlockCopy(id, 0, buff,2,2);
                    buff[4] = channel;
                    System.Buffer.BlockCopy(bytes, 0, buff, 5, bytes.Length);
                    break;
                case operationtype.refersetThreshold:
                    /*设置参考点接收Tag定位信号强度阀值
                     * PC--->USB：F2 + 45 + ID + Threshold + CS  + F1
                     * */
                    buff = new byte[7];
                    buff[1] = 0x45;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    buff[4] = bytes[0];
                    break;
                case operationtype.refersetk1:
                    /*设置参考点发送Tag定位包的信号强度系数k
                     * PC--->USB：F2 + 47 + ID + k1 + CS  + F1
                     * */
                    buff = new byte[7];
                    buff[1] = 0x47;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    buff[4] = bytes[0];
                    break;
                case operationtype.referrest:
                    /*控制参考点复位
                     * PC --> USB：F2 + 49 + ID + CS + F1
                     * */
                    buff = new byte[6];
                    buff[1] = 0x49;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    break;
                case operationtype.nodesetwifiipmode:
                    /*设置Wifi的IP模式
                     * PC -->USB: F2 + 18 + ID + Channel + Mode + CS + F1
                     */
                    buff = new byte[8];
                    buff[1] = 0x18;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    buff[4] = channel;
                    buff[5] = bytes[0];
                    break;
                case operationtype.nodesetwifistaticip:
                    /*设置WIFI的静态IP
                     * F2 + 1A + ID + Channel + IP + CS + F1
                     */
                    buff = new byte[11];
                    buff[1] = 0x1A;
                    System.Buffer.BlockCopy(id, 0, buff,2,2);
                    buff[4] = channel;
                    System.Buffer.BlockCopy(bytes, 0, buff, 5, 4);
                    break;
                default:
                    return false;
            }
            try
            {
                buff[0] = 0xF2;
                buff[buff.Length - 1] = 0xF1;
                buff[buff.Length - 2] = 0;
                for (int i = 0; i < buff.Length - 2; i++)
                {
                    buff[buff.Length - 2] += buff[i];
                }
                int count = 3;
                int tick = 0;
                SysParam.readmark = false;
                while (count > 0)
                {
                    tick = Environment.TickCount;
                    frm.MyUdpClient.Send(buff, buff.Length, nodemsg.mendpoint);
                    while (Environment.TickCount - tick < ConstInfor.recnwtimeout * 3 && !SysParam.readmark)
                    {
                        Thread.Sleep(1);
                    }
                    if (SysParam.readmark)
                    {
                        break;
                    }
                    else
                    {
                        count--;
                    }
                }
            }catch(Exception ex)
            {
                FileOperation.WriteLog("设置设备参数出现异常！异常原因:"+ex.ToString());
            }
            if (SysParam.readmark)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 读取设备讯息
        /// </summary>
        /// <param name="curtype"></param>
        /// <param name="id"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        private bool ReadDev(operationtype curtype, byte[] id,byte channel)
        {
            byte[] buff = null;
            #region 构建发送的数据包
            switch (curtype)
            {
                case operationtype.nodereadnodeinfor:
                    /*查询节点设备的固件版本号
                     * PC --> USB：F2 + 02 + ID + channel + CS + F1
                     * */
                    buff = new byte[7];
                    buff[1] = 0x02;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    buff[4] = channel;
                    break;
                case operationtype.referreadreferinfor:
                    /*查询参考点设备的固件版本号
                     * PC --> USB：F2 + 42 + ID + CS + F1
                     * */
                    buff = new byte[6];
                    buff[1] = 0x42;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    break;
                case operationtype.nodereadserverip:
                    /*
                     * 读取节点的Server IP
                     * PC --> USB：F2 + 06 + ID  + channel+ CS + F1
                     * */
                    buff = new byte[7];
                    buff[1] = 0x06;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    buff[4] = channel;
                    break;
                case operationtype.nodereadserverport:
                    /*读取节点的Server Port
                     * PC --> USB：F2 + 08 + ID + channel + CS + F1
                     * */
                    buff = new byte[7];
                    buff[1] = 0x08;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    buff[4] = channel;
                    break;
                case operationtype.nodereadselfipmode:
                    /*读取节点的IP模式
                     * PC --> USB：F2 + 0F + ID  + channel+ CS + F1
                     * */
                    buff = new byte[7];
                    buff[1] = 0x0F;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    buff[4] = channel;
                    break;
                case operationtype.nodereadselfip:
                    /*读取节点的IP模式
                     * PC --> USB：F2 + 11 + ID  + channel+ CS + F1
                     * */
                    buff = new byte[7];
                    buff[1] = 0x11;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    buff[4] = channel;
                    break;
                case operationtype.nodereadselfgateway:
                    /*读取节点的GateWay
                     * PC --> USB：F2 + 15 + ID + channel + CS + F1
                     * */

                    buff = new byte[7];
                    buff[1] = 0x15;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    buff[4] = channel;

                    break;
                case operationtype.nodereadselfmask:
                    /*读取节点的SubMask
                     * PC --> USB：F2 + 13 + ID  + channel+ CS + F1
                     * */
                    buff = new byte[7];
                    buff[1] = 0x13;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    buff[4] = channel;
                    break;
                case operationtype.nodereset:
                    /*控制节点复位
                     * PC --> USB：F2 + 0D + ID + channel + CS + F1
                     * */
                    buff = new byte[7];
                    buff[1] = 0x0D;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    buff[4] = channel;
                    break;
                case operationtype.nodereadwifiname:
                    /*读取节点的Wifi AP名称
                     * PC --> USB：F2 + 0A + ID  + channel + CS + F1
                     * */
                    buff = new byte[7];
                    buff[1] = 0x0A;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    buff[4] = channel;
                    break;
                case operationtype.nodereadwifipsw:
                    /*读取节点的Wifi AP密码
                     * PC --> USB：F2 + 0C + ID + channel + CS + F1
                     * */
                    buff = new byte[7];
                    buff[1] = 0x0C;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    buff[4] = channel;
                    break;
                case operationtype.referreadThreshold:
                    /*读取参考点接收Tag定位信号强度阀值
                     * PC--->USB：F2 + 46 + ID + CS + F1
                     * */
                    buff = new byte[6];
                    buff[1] = 0x46;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    break;
                case operationtype.referreadk1:
                    /*读取参考点发送Tag定位包的信号强度系数k
                     * PC--->USB：
                     * F2 + 48 + ID + CS+ F1
                     * */
                    buff = new byte[6];
                    buff[1] = 0x48;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    break;
                case operationtype.referreadsig:
                    /*查询节点周围指定参考点的信号强度
                     * PC --> USB：F2 + 4A + ID + CS + F1
                     * */
                    buff = new byte[6];
                    buff[1] = 0x4A;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    break;
                case operationtype.nodereadwifiipmode:
                    /*读取Wifi的IP模式
                     * PC -->USB: F2 + 19 + ID + Channel + CS + F1
                     */
                    buff = new byte[7];
                    buff[1] = 0x19;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    buff[4] = channel;
                    break;
                case operationtype.nodereadwifistaticip:
                    /*读取节点WIFI的静态IP
                     * PC -->USB: F2 + 1B + ID + Channel + CS + F1
                     */
                    buff = new byte[7];
                    buff[1] = 0x1B;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    buff[4] = channel;
                    break;
                case operationtype.nodeselectcnnstatus:
                   /*查询节点最后一次网络连接状态
                    * PC -->USB: F2 + 17 + ID + Channel + CS + F1
                    */
                    buff = new byte[7];
                    buff[1] = 0x17;
                    System.Buffer.BlockCopy(id, 0, buff, 2, 2);
                    buff[4] = channel;
                    break;
                default:
                    return false;
            }
            buff[0] = 0xF2;
            buff[buff.Length - 1] = 0xF1;
            buff[buff.Length - 2] = 0;
            for (int i = 0; i < buff.Length - 2; i++)
            {
                buff[buff.Length - 2] += buff[i];
            }
            #endregion
            int count = 3;
            int tick = 0;
            SysParam.readmark = false;
            try
            {
                while (count > 0)
                {
                    tick = Environment.TickCount;
                    frm.MyUdpClient.Send(buff, buff.Length, nodemsg.mendpoint);
                    while (Environment.TickCount - tick < ConstInfor.recnwtimeout * 3 && !SysParam.readmark)
                    {
                        Thread.Sleep(1);
                    }
                    if (SysParam.readmark)
                    {
                        break;
                    }
                    else
                    {
                        count--;
                    }
                }
                if (SysParam.readmark)
                {
                    return true;
                }
            }catch(Exception ex)
            {
                FileOperation.WriteLog("读取设备参数时出现异常!异常原因："+ex.ToString());
            }
            return false;
        }
        private void NodeSettingDevWin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != updatetimer)
            {
                updatetimer.Stop();
                updatetimer = null;
            }
            SysParam.mdevices.nodedevs.Clear();
            SysParam.mdevices.referdevs.Clear();
        }
        //点击列表中的项，选择指定列表,并可以进行设置
        private void devlistview_Click(object sender, EventArgs e)
        {
            if (devlistview.SelectedItems.Count <= 0)
            {
                return;
            }
            #region 选择设备类型,更新设备讯息到面板上
            rststatustxt.Visible = false;
            rstlb.Visible = false;
            string strid = devlistview.SelectedItems[0].Text;
            NodeType type = NodeType.UnKnown;
            if (SysParam.mdevices.nodedevs.TryGetValue(strid, out ndparam))
            {//说明当前是数据节点
                #region 显示节点基本讯息
                nodepanel.Visible = true; referpanal.Visible = false;
                nodepanel.Enabled = true;
                referpanal.Enabled = false;
                nodepanel.Location = new Point(285, 12);
                type = NodeType.DataNode;
                string strname = CommonBoxOperation.GetNodeName(strid);
                StringBuilder strbuilder = new StringBuilder("the current data node:");
                if (null == strname || "".Equals(strname))
                {
                    strbuilder.Append(strid);
                }
                else
                {
                    strbuilder.Append(strname);
                    strbuilder.Append("(");
                    strbuilder.Append(strid);
                    strbuilder.Append(")");
                }
                devicelb.Text = strbuilder.ToString();
                strbuilder.Clear();
                strbuilder.Append("type:");
                strbuilder.Append(ConstInfor.GetDevType(NodeType.DataNode, ndparam.type));
                nodetypelb.Text = strbuilder.ToString();
                strbuilder.Clear();
                strbuilder.Append("version:");
                strbuilder.Append(((byte)(ndparam.version >> 24)).ToString("D2") + ((byte)(ndparam.version >> 16)).ToString("D2") + ((byte)(ndparam.version >> 8)).ToString("D2") + ((byte)(ndparam.version)).ToString("X2"));
                nodeverlb.Text = strbuilder.ToString();
                strbuilder.Clear();
                strbuilder.Append("channel:");
                strbuilder.Append(ndparam.channel.ToString());
                channellb.Text = strbuilder.ToString();
                if (ndparam.type != 0x02 && ndparam.type != 0x03)
                {//Wifi参数设置
                    label15.Visible = true; wifinametxt.Visible = true; wifinamesetbtn.Visible = true;
                    wifinamereadbtn.Visible = true; wifinamestatustxt.Visible = true;
                    label16.Visible = true; wifipswtxt.Visible = true;
                    wifipswsetbtn.Visible = true; wifipswreadbtn.Visible = true; wifipswstatustxt.Visible = true;
                    nodeparambox.Visible = false;
                    rstbtn.Location = new Point(254, 657);
                    rststatustxt.Location = new Point(445, 659);
                    wifisigpanel.Location = new Point(16, 275);
                    wifisigpanel.Visible = true;
                }
                else
                {//RJ45参数设置
                    label15.Visible = false; wifinametxt.Visible = false; wifinamesetbtn.Visible = false;
                    wifinamereadbtn.Visible = false; wifinamestatustxt.Visible = false;
                    label16.Visible = false; wifipswtxt.Visible = false;
                    wifipswsetbtn.Visible = false; wifipswreadbtn.Visible = false; wifipswstatustxt.Visible = false;
                    nodeparambox.Visible = true;
                    nodeparambox.Location = new Point(7, 215);
                    rstbtn.Location = new Point(254, 421);
                    rststatustxt.Location = new Point(445, 426);
                    wifisigpanel.Visible = false;
                }
                #endregion
            }
            else if (SysParam.mdevices.referdevs.TryGetValue(strid, out referparam))
            { //参考点
                #region 显示参考点基本讯息
                referpanal.Visible = true; nodepanel.Visible = false;
                referpanal.Location = new Point(285, 12);
                nodepanel.Enabled = false;
                referpanal.Enabled = true;
                type = NodeType.ReferNode;
                string strname = CommonBoxOperation.GetRouterName(strid);
                StringBuilder strbuilder = new StringBuilder("the current data refer:");
                if (null == strname || "".Equals(strname))
                {
                    strbuilder.Append(strid);
                }
                else
                {
                    strbuilder.Append(strname);
                    strbuilder.Append("(");
                    strbuilder.Append(strid);
                    strbuilder.Append(")");
                }
                referinfotx.Text = strbuilder.ToString();
                strbuilder.Clear();
                strbuilder.Append("type:");
                strbuilder.Append(ConstInfor.GetDevType(NodeType.ReferNode, referparam.type));
                refertypetx.Text = strbuilder.ToString();
                strbuilder.Clear();
                strbuilder.Append("version:");
                //((byte)(rt.Value.Version >> 24)).ToString("D2") + ((byte)(rt.Value.Version >> 16)).ToString("D2") + ((byte)(rt.Value.Version >> 8)).ToString("D2") + ((byte)(rt.Value.Version)).ToString("X2");
                strbuilder.Append(((byte)(referparam.version >> 24)).ToString("D2") + ((byte)(referparam.version >> 16)).ToString("D2") + ((byte)(referparam.version >> 8)).ToString("D2") + ((byte)(referparam.version)).ToString("X2"));
                referversiontx.Text = strbuilder.ToString();
                #endregion
            }
            #endregion
            #region 获取设备讯息
            if (type == NodeType.DataNode)
            {//获取到数据节点，开始获取数据节点讯息
                StringBuilder strbr = new StringBuilder();
                #region 读取数据节点的相关参数
                if (ReadDev(operationtype.nodereadserverip, ndparam.id, ndparam.channel))
                { //读取Server IP
                    strbr.Append(ndparam.serverip[0].ToString());
                    strbr.Append(".");
                    strbr.Append(ndparam.serverip[1].ToString());
                    strbr.Append(".");
                    strbr.Append(ndparam.serverip[2].ToString());
                    strbr.Append(".");
                    strbr.Append(ndparam.serverip[3].ToString());
                    iptxt.Text = strbr.ToString();
                    serveripstatustxt.Text = "read ok";
                }
                else
                {
                    iptxt.Text = "";
                    serveripstatustxt.Text = "read error";
                }
                if (ReadDev(operationtype.nodereadserverport, ndparam.id,ndparam.channel))
                {//读取Server Port 
                    porttxt.Text = ndparam.port.ToString();
                    serverportstatustxt.Text = "read ok";
                }
                else
                {
                    porttxt.Text = "";
                    serverportstatustxt.Text = "read error";
                }
                if (ndparam.type != 0x02 && ndparam.type != 0x03)
                {//Wifi
                    if (ReadDev(operationtype.nodereadwifiname, ndparam.id,ndparam.channel))
                    {
                        wifinametxt.Text = System.Text.ASCIIEncoding.Default.GetString(ndparam.wifiname, 0, 32);
                        wifinamestatustxt.Text = "read ok";

                    }
                    else
                    {
                        wifinametxt.Text = "";
                        wifinamestatustxt.Text = "read error";
                    }
                    if (ReadDev(operationtype.nodereadwifipsw, ndparam.id,ndparam.channel))
                    {
                        wifipswtxt.Text = System.Text.ASCIIEncoding.Default.GetString(ndparam.wifipsw, 0, 32);
                        wifipswstatustxt.Text = "read ok";

                    }
                    else
                    {
                        wifipswtxt.Text = "";
                        wifipswstatustxt.Text = "read error";
                    }
                    if (ReadDev(operationtype.nodereadwifiipmode, ndparam.id, ndparam.channel))
                    {
                        wifiipmodecb.SelectedIndex = ndparam.ipmode;
                        wifiipmodestatustx.Text = "read ok";
                    }
                    else
                    {
                        wifiipmodestatustx.Text = "read error";
                    }
                    if (ReadDev(operationtype.nodereadwifistaticip, ndparam.id, ndparam.channel))
                    {
                        wifistaticiptx.Text = ndparam.nodeip[0].ToString() + "." + ndparam.nodeip[1].ToString() + "." + ndparam.nodeip[2].ToString() + "." + ndparam.nodeip[3].ToString();
                        wifistaticipstatustx.Text = "read ok";
                    }
                    else
                    {
                        wifistaticipstatustx.Text = "read error";
                    }
                    if (ReadDev(operationtype.nodeselectcnnstatus, ndparam.id, ndparam.channel))
                    {
                        string str = "";
                        switch (ndparam.status)
                        {
                            case 0:
                                str = "Wifi connection successful";
                                break;
                            case 1:
                                str = "AT + RST fails";
                                break;
                            case 2:
                                str = "AT + CWMODE_CUR fails";
                                break;
                            case 3:
                                str = "AT + CWDHCP_CUR fails";
                                break;
                            case 4:
                                str = "AT + CIPSTA_CUR fails";
                                break;
                            case 5:
                                str = "AT + CWJAP_CUR failed to respond";
                                break;
                            case 6:
                                str = "AT + PING fails";
                                break;
                            case 7:
                                str = "AT + CIPSEND fails";
                                break;
                            case 8:
                                str = "AT+CWJAP_CUR failed connection timeout";
                                break;
                            case 9:
                                str = "AT+CWJAP_CUR failed password error";
                                break;
                            case 10:
                                str = "AT+CWJAP_CUR failed to find the target AP";
                                break;
                            case 11:
                                str = "AT+CWJAP_CUR failed connection failed";
                                break;
                            case 12:
                                str = "AT+CWJAP_CUR failure is another reason";
                                break;
                            default:
                                str = "Unknown reason";
                                break;
                        }
                        lastcnnstatustx.Text = str;
                        wifilastcnnstatustx.Text = "read ok";
                    }
                    else
                    {
                        wifilastcnnstatustx.Text = "read error";
                    }
                }
                else
                { //RJ45
                    if (ReadDev(operationtype.nodereadselfipmode, ndparam.id,ndparam.channel))
                    {//读取自身的IP模式
                        ipmodecb.SelectedIndex = ndparam.ipmode;
                        ipmodestatustxt.Text = "read ok";
                    }
                    else
                    {
                        ipmodecb.Text = "";
                        ipmodestatustxt.Text = "read error";
                    }
                    if (ReadDev(operationtype.nodereadselfip, ndparam.id,ndparam.channel))
                    {//读取自身的IP
                        strbr.Clear();
                        strbr.Append(ndparam.nodeip[0].ToString());
                        strbr.Append(".");
                        strbr.Append(ndparam.nodeip[1].ToString());
                        strbr.Append(".");
                        strbr.Append(ndparam.nodeip[2].ToString());
                        strbr.Append(".");
                        strbr.Append(ndparam.nodeip[3].ToString());
                        ndiptxt.Text = strbr.ToString();
                        nodeipstatustxt.Text = "read ok";
                    }
                    else
                    {
                        ndiptxt.Text = "";
                        nodeipstatustxt.Text = "read error";
                    }
                    if (ReadDev(operationtype.nodereadselfmask, ndparam.id,ndparam.channel))
                    {//读取自身的掩码
                        strbr.Clear();
                        strbr.Append(ndparam.submask[0].ToString());
                        strbr.Append(".");
                        strbr.Append(ndparam.submask[1].ToString());
                        strbr.Append(".");
                        strbr.Append(ndparam.submask[2].ToString());
                        strbr.Append(".");
                        strbr.Append(ndparam.submask[3].ToString());
                        smtxt.Text = strbr.ToString();
                        maskstatustxt.Text = "read ok";
                    }
                    else
                    {
                        smtxt.Text = "";
                        maskstatustxt.Text = "read error";
                    }
                    if (ReadDev(operationtype.nodereadselfgateway, ndparam.id,ndparam.channel))
                    {//读取自身的网关
                        strbr.Clear();
                        strbr.Append(ndparam.gateway[0].ToString());
                        strbr.Append(".");
                        strbr.Append(ndparam.gateway[1].ToString());
                        strbr.Append(".");
                        strbr.Append(ndparam.gateway[2].ToString());
                        strbr.Append(".");
                        strbr.Append(ndparam.gateway[3].ToString());
                        gwtxt.Text = strbr.ToString();
                        gatestatustxt.Text = "read ok";
                    }
                    else
                    {
                        gwtxt.Text = "";
                        gatestatustxt.Text = "read error";
                    }
                    if (ReadDev(operationtype.nodeselectcnnstatus, ndparam.id, ndparam.channel))
                    {
                        string str = "";
                        switch (ndparam.status)
                        {
                            case 0:
                                str = "Successful network connection";
                                break;
                            case 1:
                                str = "IC initialization failed";
                                break;
                            case 2:
                                str = "Physical connection failed";
                                break;
                            case 3:
                                str = "Get ARP failed";
                                break;
                            case 4:
                                str = "DHCP failure";
                                break;
                            case 5:
                                str = "IP conflict";
                                break;

                        }
                        nodeselectlaststatictx.Text = str;

                        nodeselectstatustxt.Text = "read ok";
                    }
                    else
                    {

                        nodeselectstatustxt.Text = "read error";
                    }
                }
                #endregion

            }else if(type == NodeType.ReferNode)
            {
                #region 读取参考点的相关参数
                if (ReadDev(operationtype.referreadThreshold, referparam.id, 0))
                {
                    sthdlb.Text = "read ok";
                    sthdtxt.Text = referparam.Sgthreshold.ToString();
                }
                else
                {
                    sthdlb.Text = "read error";
                }
                if (ReadDev(operationtype.referreadk1, referparam.id, 0))
                {//读取Refer的信号强度系数 
                    ssfaclb.Text = "read ok";
                    ssfactxt.Text = string.Format("{0:F2}", (double)referparam.Sgstrengthfac / 100);
                }
                else
                {
                    ssfaclb.Text = "read error";
                }
                if (ReadDev(operationtype.referreadsig, referparam.id, 0))
                {//读取Refer与节点的信号强度
                    nodetorefersigtb.Text = referparam.nodetorefersig.ToString();
                    refertonodesigtb.Text = referparam.refertonodesig.ToString();
                    readnfsigstatustx.Text = "read ok";
                    readfnsigstatustx.Text = "read ok";
                }
                else
                {
                    readnfsigstatustx.Text = "read error";
                    readfnsigstatustx.Text = "read error";
                }
                #endregion
            }
            #endregion

        }
        /// <summary>
        /// 读取server 的IP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void readipbtn_Click(object sender, EventArgs e)
        {
            StringBuilder strbr = new StringBuilder();
            rststatustxt.Visible = false;
            this.Invoke(new Action(() => {
                serveripstatustxt.Text = "";
            }));
            if (ReadDev(operationtype.nodereadserverip, ndparam.id,ndparam.channel))
            { //读取Server IP
                strbr.Append(ndparam.serverip[0].ToString());
                strbr.Append(".");
                strbr.Append(ndparam.serverip[1].ToString());
                strbr.Append(".");
                strbr.Append(ndparam.serverip[2].ToString());
                strbr.Append(".");
                strbr.Append(ndparam.serverip[3].ToString());
                this.Invoke(new Action(() => {
                    iptxt.Text = strbr.ToString();
                    serveripstatustxt.Text = "read ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    iptxt.Text = "";
                    serveripstatustxt.Text = "read error";
                }));
            }
        }
        /// <summary>
        /// 读取Server的端口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void readportbtn_Click(object sender, EventArgs e)
        {
            rststatustxt.Visible = false;
            this.Invoke(new Action(() =>
            {
                porttxt.Text = ndparam.port.ToString();
                serverportstatustxt.Text = "";
            }));
            if (ReadDev(operationtype.nodereadserverport, ndparam.id,ndparam.channel))
            {//读取Server Port 
                this.Invoke(new Action(() => {
                    porttxt.Text = ndparam.port.ToString();
                    serverportstatustxt.Text = "read ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    porttxt.Text = "";
                    serverportstatustxt.Text = "read error";
                }));
            }
        }
        /// <summary>
        /// 读取IP模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void readipmodebtn_Click(object sender, EventArgs e)
        {
            rststatustxt.Visible = false;
            this.Invoke(new Action(() =>
            {
                ipmodestatustxt.Text = "";
            }));
            if (ReadDev(operationtype.nodereadselfipmode, ndparam.id,ndparam.channel))
            {//读取自身的IP模式
                this.Invoke(new Action(() => {
                    ipmodecb.SelectedIndex = ndparam.ipmode;
                    ipmodestatustxt.Text = "read ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    ipmodecb.Text = "";
                    ipmodestatustxt.Text = "read error";
                }));
            }
        }
        /// <summary>
        /// 读取自身ip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void readndipbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                nodeipstatustxt.Text = "";
            }));
            rststatustxt.Visible = false;
            StringBuilder strbr = new StringBuilder();
            if (ReadDev(operationtype.nodereadselfip, ndparam.id,ndparam.channel))
            {//读取自身的IP
                strbr.Append(ndparam.nodeip[0].ToString());
                strbr.Append(".");
                strbr.Append(ndparam.nodeip[1].ToString());
                strbr.Append(".");
                strbr.Append(ndparam.nodeip[2].ToString());
                strbr.Append(".");
                strbr.Append(ndparam.nodeip[3].ToString());
                this.Invoke(new Action(() => {
                    ndiptxt.Text = strbr.ToString();
                    nodeipstatustxt.Text = "read ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    ndiptxt.Text = "";
                    nodeipstatustxt.Text = "read error";
                }));
            }
        }
        /// <summary>
        /// 读取掩码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void readsmbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                maskstatustxt.Text = "";
            }));
            rststatustxt.Visible = false;
            StringBuilder strbr = new StringBuilder();
            if (ReadDev(operationtype.nodereadselfmask, ndparam.id,ndparam.channel))
            {//读取自身的掩码
                strbr.Append(ndparam.submask[0].ToString());
                strbr.Append(".");
                strbr.Append(ndparam.submask[1].ToString());
                strbr.Append(".");
                strbr.Append(ndparam.submask[2].ToString());
                strbr.Append(".");
                strbr.Append(ndparam.submask[3].ToString());
                this.Invoke(new Action(() => {
                    smtxt.Text = strbr.ToString();
                    maskstatustxt.Text = "read ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    smtxt.Text = "";
                    maskstatustxt.Text = "read error"; 
                }));
            }
        }
        /// <summary>
        /// 读取网关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void readgwbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                gatestatustxt.Text = "";
            }));
            rststatustxt.Visible = false;
            StringBuilder strbr = new StringBuilder();
            if (ReadDev(operationtype.nodereadselfgateway, ndparam.id,ndparam.channel))
            {//读取自身的网关
                strbr.Append(ndparam.gateway[0].ToString());
                strbr.Append(".");
                strbr.Append(ndparam.gateway[1].ToString());
                strbr.Append(".");
                strbr.Append(ndparam.gateway[2].ToString());
                strbr.Append(".");
                strbr.Append(ndparam.gateway[3].ToString());
                this.Invoke(new Action(() => {
                    gwtxt.Text = strbr.ToString();
                    gatestatustxt.Text = "read ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    gwtxt.Text = "";
                    gatestatustxt.Text = "read error";
                }));
            }
        }
        /// <summary>
        /// 复位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rstbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                rststatustxt.Text = "";
            }));
            rststatustxt.Visible = true;
            if (ReadDev(operationtype.nodereset, ndparam.id,ndparam.channel))
            {
                this.Invoke(new Action(() => {
                    rststatustxt.Text = "reset ok";
                }));
                MessageBox.Show("The data node channel may change after reset and we need to research the equipment...", "Remind", MessageBoxButtons.OK);
                searchbtn_Click(null, null);
            }
            else
            {
                this.Invoke(new Action(() => {
                    rststatustxt.Text = "reset error";
                }));
            }
        }
        /// <summary>
        /// 读取Wifi Name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wifinamereadbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                wifinamestatustxt.Text = "";
            }));
            rststatustxt.Visible = false;
            if (ReadDev(operationtype.nodereadwifiname, ndparam.id,ndparam.channel))
            {
                this.Invoke(new Action(() => {
                    wifinametxt.Text = System.Text.ASCIIEncoding.Default.GetString(ndparam.wifiname, 0, 32);
                    wifinamestatustxt.Text = "read ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    wifinamestatustxt.Text = "read error";
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
            if (ReadDev(operationtype.nodereadwifipsw, ndparam.id,ndparam.channel))
            {
                this.Invoke(new Action(() => {
                    wifipswtxt.Text = System.Text.ASCIIEncoding.Default.GetString(ndparam.wifipsw, 0, 32);
                    wifipswstatustxt.Text = "read ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    wifipswstatustxt.Text = "read error";
                }));
            }
        }
        //设置节点ip
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
            this.Invoke(new Action(() => {
                serveripstatustxt.Text = "";
            }));
            //获取IP地址的byte数组
            byte[] ips = ipaddr.GetAddressBytes();
            if (SetDev(operationtype.nodesetserverip, ndparam.id, ips, ndparam.channel))
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
        /// <summary>
        /// 设置节点的Server Port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            }
            catch (Exception)
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
            mpts[1] = (byte)port;
            if (SetDev(operationtype.nodesetserverport, ndparam.id, mpts, ndparam.channel))
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
        /// <summary>
        /// 设置Wifi Name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wifinamesetbtn_Click(object sender, EventArgs e)
        {
            string strwifiname = wifinametxt.Text;
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
            if (SetDev(operationtype.nodesetwifiname, ndparam.id, chs, ndparam.channel))
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
        /// <summary>
        /// 设置Wifi的PSW
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wifipswsetbtn_Click(object sender, EventArgs e)
        {
            string strpsw = wifipswtxt.Text;
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
            if (SetDev(operationtype.nodesetwifipsw, ndparam.id, chs, ndparam.channel))
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
        /// <summary>
        /// 设置ip 模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            if (SetDev(operationtype.nodesetselfipmode, ndparam.id, ipmodes, ndparam.channel))
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
            if (SetDev(operationtype.nodesetselfip, ndparam.id, ips, ndparam.channel))
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

            if (SetDev(operationtype.nodesetselfmask, ndparam.id, submasks, ndparam.channel))
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
            if (SetDev(operationtype.nodesetselfgateway, ndparam.id, gws, ndparam.channel))
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
        //读取信号阀值
        private void readsthdbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                sthdlb.Text = "";
            }));
            rstlb.Visible = false;
            if (ReadDev(operationtype.referreadThreshold, referparam.id, 0))
            {
                this.Invoke(new Action(() => {
                    sthdlb.Text = "read ok";
                    sthdtxt.Text = referparam.Sgthreshold.ToString();
                }));

            }
            else
            {
                this.Invoke(new Action(() => {
                    sthdlb.Text = "read error";
                }));
            }
        }
        /// <summary>
        /// 读取信号强度系数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void readssfacbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                ssfaclb.Text = "";
            }));
            rstlb.Visible = false;
            if (ReadDev(operationtype.referreadk1, referparam.id, 0))
            {//读取Refer的信号强度系数 
                this.Invoke(new Action(() => {
                    ssfaclb.Text = "read ok";
                    ssfactxt.Text = string.Format("{0:F2}", (double)referparam.Sgstrengthfac / 100);
                }));
   
            }
            else
            {
                this.Invoke(new Action(() => {
                    ssfaclb.Text = "read error";
                }));
            }
        }
        /// <summary>
        /// 读取信号强度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void readsigbtn_Click(object sender, EventArgs e)
        {
            rstlb.Visible = false;
            this.Invoke(new Action(() =>
            {
                readnfsigstatustx.Text = "";
                readfnsigstatustx.Text = "";
            }));
            if (ReadDev(operationtype.referreadsig, referparam.id, 0))
            {//读取Refer与节点的信号强度
                this.Invoke(new Action(() => {
                    nodetorefersigtb.Text = referparam.nodetorefersig.ToString();
                    refertonodesigtb.Text = referparam.refertonodesig.ToString();
                    readnfsigstatustx.Text = "read ok";
                    readfnsigstatustx.Text = "read ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    readnfsigstatustx.Text = "read error";
                    readfnsigstatustx.Text = "read error";
                }));
            }

        }
        /// <summary>
        /// 给参考点复位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                rstlb.Text = "";
            }));
            if (SetDev(operationtype.referrest, referparam.id, null, 0))
            {
                this.Invoke(new Action(() => {
                    rstlb.Text = "read ok";
                }));
               
            }
            else
            {
                this.Invoke(new Action(() => {
                    rstlb.Text = "read error";
                }));
                
            }
            rstlb.Visible = true;
        }
        /// <summary>
        /// 设置参考点信号强度阀值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            }
            catch (Exception)
            {
                MessageBox.Show("The signal strength threshold format is not correct!");
                return;
            }
            this.Invoke(new Action(() =>
            {
                sthdlb.Text = "";
            }));
            if (SetDev(operationtype.refersetThreshold, referparam.id, prams,0))
            {
                this.Invoke(new Action(() => {
                    sthdlb.Text = "set ok";
                }));
        
            }
            else
            {
                this.Invoke(new Action(() => {
                    sthdlb.Text = "set error";
                }));
               
            }
        }
        /// <summary>
        /// 设置信号强度系数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            }
            catch (Exception)
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
            if (SetDev(operationtype.refersetk1, referparam.id, prams, 0))
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
        private void selecttypecb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selecttypecb.SelectedIndex == 0)
            {
                CurSelectType = NodeType.DataNode;
                nodepanel.Visible = true;
                referpanal.Visible = false;
            }
            else
            {
                CurSelectType = NodeType.ReferNode;
                nodepanel.Visible = false;
                referpanal.Visible = true;
            }
            devlistview.Items.Clear();
        }
        //开始搜索Wifi的信号强度
        private void searchwifisigbtn_Click(object sender, EventArgs e)
        {
            string strwifiname = searchwifinametb.Text;
            this.Invoke(new Action(() => {
                readwifisigstatustxt.Text = "";
            }));
            if ("".Equals(strwifiname))
            {
                MessageBox.Show("Sorry, the name of the Wifi to search cannot be empty!");
                return;
            }
            byte[] chs = System.Text.Encoding.ASCII.GetBytes(strwifiname);
            if (Searchwifisig(chs, ndparam.channel, ndparam.id))
            {
                this.Invoke(new Action(() => {
                    readwifisigstatustxt.Text = "read ok";
                    wifisigstrtb.Text = SysParam.WifiSsd + "";
                }));
            }
            else
            {
                this.Invoke(new Action(() =>{
                    readwifisigstatustxt.Text = "read error";
                }));
                
            }
            
        }
        private bool Searchwifisig(byte[] wifiname,byte channel,byte[] id)
        { 
            //F2 + 16(1) + ID(2-3) + channel(4) + WifiSsid[32] + cs + F1  ======>len = 39
            byte[] bytes = new byte[39];
            bytes[0] = 0xF2; bytes[1] = 0x16;
            System.Buffer.BlockCopy(id,0,bytes,2,2);
            bytes[4] = channel;
            System.Buffer.BlockCopy(wifiname, 0, bytes, 5, wifiname.Length);
            bytes[37] = 0;
            for (int i = 0; i < 37;i ++)
            {
                bytes[37] += bytes[i];
            }
            bytes[38] = 0xF1;
            SysParam.readmark = false;
            int tickcount = Environment.TickCount;
            frm.MyUdpClient.Send(bytes, bytes.Length, nodemsg.mendpoint);
            while (Environment.TickCount - tickcount < ConstInfor.recnwtimeout * 150  && !SysParam.readmark)
            {
                Thread.Sleep(1);
            }
            if (!SysParam.readmark)
            {
                return false;
            }
            else
            {
                return (SysParam.WifiSsd == 0xFF) ? false : true;
            }
        }
        private void readwifimodebtn_Click(object sender, EventArgs e)
        {
            //读取wifi的连接模式
            this.Invoke(new Action(() =>
            {
                wifiipmodestatustx.Text = "";
            }));
            rststatustxt.Visible = false;
            if (ReadDev(operationtype.nodereadwifiipmode, ndparam.id, ndparam.channel))
            {
                this.Invoke(new Action(() =>
                {
                    wifiipmodecb.SelectedIndex = ndparam.ipmode;
                    wifiipmodestatustx.Text = "read ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() =>
                {
                    wifiipmodestatustx.Text = "read error";
                }));

            }

        }

        private void readstaticipbtn_Click(object sender, EventArgs e)
        {
            //读取静态IP
            this.Invoke(new Action(() =>
            {
                wifistaticipstatustx.Text = "";
            }));
            rststatustxt.Visible = false;

            if (ReadDev(operationtype.nodereadwifistaticip, ndparam.id, ndparam.channel))
            {
                this.Invoke(new Action(() =>
                {
                    wifistaticiptx.Text = ndparam.nodeip[0].ToString() + "." + ndparam.nodeip[1].ToString() + "." + ndparam.nodeip[2].ToString() + "." + ndparam.nodeip[3].ToString();
                    wifistaticipstatustx.Text = "read ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() =>
                {
                    wifistaticipstatustx.Text = "read error";
                }));
            }
        }

        private void setwifimodebtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                wifiipmodestatustx.Text = "";
            }));
            byte[] bytes = new byte[1];
            bytes[0] = (byte)(wifiipmodecb.SelectedIndex == 0 ? 0x01 : 0x02);
            if (SetDev(operationtype.nodesetwifiipmode, ndparam.id, bytes, ndparam.channel))
            {
                this.Invoke(new Action(() =>
                {
                    wifiipmodestatustx.Text = "set ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() =>
                {
                    wifiipmodestatustx.Text = "set error";
                }));

            }
        }

        private void setstaticipbtn_Click(object sender, EventArgs e)
        {
            String strip = wifistaticiptx.Text;
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
                wifistaticipstatustx.Text = "";
            }));
            byte[] ips = ipaddr.GetAddressBytes();
            if (SetDev(operationtype.nodesetwifistaticip, ndparam.id, ips, ndparam.channel))
            {
                this.Invoke(new Action(() =>
                {
                    wifistaticipstatustx.Text = "set ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() =>
                {
                    wifistaticipstatustx.Text = "set error";
                }));
            }
        }

        private void selectlastcnnstatusbtn_Click(object sender, EventArgs e)
        {
            // 查询节点最后一次网络连接状态
            this.Invoke(new Action(() =>
            {
                wifilastcnnstatustx.Text = "";
            }));
            rststatustxt.Visible = false;

            if (ReadDev(operationtype.nodeselectcnnstatus, ndparam.id, ndparam.channel))
            {
                this.Invoke(new Action(() =>
                {
                    string str = "";
                    switch (ndparam.status)
                    { 
                        case 0:
                            str = "Wifi connection successful";
                            break;
                        case 1:
                            str = "AT + RST fails";
                            break;
                        case 2:
                            str = "AT + CWMODE_CUR fails";
                            break;
                        case 3:
                            str = "AT + CWDHCP_CUR fails";
                            break;
                        case 4:
                            str = "AT + CIPSTA_CUR fails";
                            break;
                        case 5:
                            str = "AT + CWJAP_CUR failed to respond";
                            break;
                        case 6:
                            str = "AT + PING fails";
                            break;
                        case 7:
                            str = "AT + CIPSEND fails";
                            break;
                        case 8:
                            str = "AT+CWJAP_CUR failed connection timeout";
                            break;
                        case 9:
                            str = "AT+CWJAP_CUR failed password error";
                            break;
                        case 10:
                            str = "AT+CWJAP_CUR failed to find the target AP";
                            break;
                        case 11:
                            str = "AT+CWJAP_CUR failed connection failed";
                            break;
                        case 12:
                            str = "AT+CWJAP_CUR failure is another reason";
                            break;
                        default:
                            str = "Unknown reason";
                            break;
                    }
                    lastcnnstatustx.Text = str;
                    wifilastcnnstatustx.Text = "read ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() =>
                {
                    wifilastcnnstatustx.Text = "read error";
                }));
            }
        }

        private void nodeselectbtn_Click(object sender, EventArgs e)
        {
            // 查询节点最后一次网络连接状态
            this.Invoke(new Action(() =>
            {
                nodeselectstatustxt.Text = "";
            }));
            rststatustxt.Visible = false;
            if (ReadDev(operationtype.nodeselectcnnstatus, ndparam.id, ndparam.channel))
            {
                this.Invoke(new Action(() =>
                {
                    string str = "";
                    switch (ndparam.status)
                    {
                        case 0:
                            str = "Successful network connection";
                            break;
                        case 1:
                            str = "IC initialization failed";
                            break;
                        case 2:
                            str = "Physical connection failed";
                            break;
                        case 3:
                            str = "Get ARP failed";
                            break;
                        case 4:
                            str = "DHCP failure";
                            break;
                        case 5:
                            str = "IP conflict";
                            break;
          
                    }
                    nodeselectlaststatictx.Text = str;

                    nodeselectstatustxt.Text = "read ok";
                }));
            }
            else
            {
                this.Invoke(new Action(() =>
                {

                    nodeselectstatustxt.Text = "read error";
                }));
            }
        }
        private void readwifimodulbtn_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() => {
                readwifimodulestatus.Text = "";
            }));
            String strtimeout = timeouttx.Text;
            if ("".Equals(strtimeout))
            {
                MessageBox.Show("The timeout cannot be set empty!");
                return;
            }
            int timeout = 0;
            try 
            {
                timeout = Convert.ToInt32(strtimeout);
            }catch(Exception)
            {
                MessageBox.Show("The timeout format is incorrect!");
                return;
            }
            if (timeout < 0 || timeout > 20000)
            {
                MessageBox.Show("The timeout period ranges from 0 to 20000!");
                return;
            }
            String strmodelorder = wifimodulopertx.Text;
            if ("".Equals(strmodelorder))
            {
                MessageBox.Show("The AT command of the Wifi module cannot be empty!");
                return;
            }
            if (strmodelorder.LastIndexOf("\r\n") < 0)
            {
                strmodelorder += "\r\n";
            }
            byte[] cmds = System.Text.Encoding.ASCII.GetBytes(strmodelorder);

            if (ReadWifiModuleOrder(ndparam.id, ndparam.channel, timeout, cmds))
            {
                this.Invoke(new Action(() => {
                    readwifimodulestatus.Text = "read ok";
                    //SysParam.backcmds
                    //System.Text.ASCIIEncoding.Default.GetString
                    wifimodulbacktx.Text = System.Text.ASCIIEncoding.Default.GetString(SysParam.backcmds, 0, SysParam.backcmds.Length);
                }));
            }
            else
            {
                this.Invoke(new Action(() =>
                {
                    readwifimodulestatus.Text = "read error";
                }));
            }
        }
        //读取Wifi模块
        private bool ReadWifiModuleOrder(byte[] id, byte channel,int timeout,byte[] cmds)
        {
            //PC => USB: F2 1C ID + Channel + Timeout[2 byte] + ATCommond[90 byte] + CS + F1
            byte[] bytes = new byte[99];
            bytes[0] = 0xF2; bytes[1] = 0x1C;
            System.Buffer.BlockCopy(id, 0, bytes,2,2);
            bytes[4] = channel;
            bytes[5] = (byte)(timeout >> 8);
            bytes[6] = (byte)timeout;
            System.Buffer.BlockCopy(cmds, 0, bytes, 7, cmds.Length);
            bytes[bytes.Length - 2] = 0;
            for (int i = 0; i < bytes.Length - 2; i ++)
            {
                bytes[bytes.Length - 2] += bytes[i];
            }
            bytes[bytes.Length - 1] = 0xF1;
            SysParam.readmark = false;
            int tick = Environment.TickCount;
            frm.MyUdpClient.Send(bytes, bytes.Length, nodemsg.mendpoint);
            while (Environment.TickCount - tick < timeout + 3000 && !SysParam.readmark)
            {
                Thread.Sleep(1);
            }
            if (SysParam.readmark)
            {
                return true;
            }
            return false;
        }
    }
    enum operationtype
    {
        nodereadnodeinfor,
        referreadreferinfor,
        nodesetserverip,
        nodereadserverip,
        nodesetserverport,
        nodereadserverport,
        nodesetselfipmode,
        nodereadselfipmode,
        nodesetselfip,
        nodereadselfip,
        nodesetselfmask,
        nodereadselfmask,
        nodesetselfgateway,
        nodereadselfgateway,
        nodesetwifiname,
        nodereadwifiname,
        nodesetwifipsw,
        nodereadwifipsw,
        nodereadwifiipmode,
        nodereadwifistaticip,
        nodesetwifiipmode,
        nodesetwifistaticip,
        nodeselectcnnstatus,
        refersetThreshold,
        referreadThreshold,
        refersetk1,
        referreadk1,
        referreadsig,
        nodereset,
        referrest
    }
}
