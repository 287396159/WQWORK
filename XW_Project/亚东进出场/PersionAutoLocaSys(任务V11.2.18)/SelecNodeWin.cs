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
namespace PersionAutoLocaSys
{
    public partial class SelecNodeWin : Form
    {
        private Timer mytimer = null;
        private Form1 frm = null;
        public SelecNodeWin()
        {
            InitializeComponent();
        }
        public SelecNodeWin(Form1 frm)
        {
            InitializeComponent();
            this.frm = frm;
        }
        private void SelecNodeWin_Load(object sender, EventArgs e)
        {
            mytimer = new Timer();
            mytimer.Interval = 1000;
            mytimer.Tick += mytimer_Tick;
            mytimer.Start();
        }
        private void mytimer_Tick(Object obj,EventArgs args)
        {
            string strname = "";
            Area area = null;
            foreach (KeyValuePair<string, Router> router in CommonCollection.Routers)
            {
                if (null == router.Value)
                {
                    continue;
                }
                if (router.Value.CurType != NodeType.DataNode)
                {//若当前的节点断开了连接，删除掉这个参考点
                    continue;
                }
                if (nodelistview.Items.ContainsKey(router.Key))
                {
                    if (!router.Value.status)
                    {//断开连接了
                        nodelistview.Items.RemoveByKey(router.Key);
                        continue;
                    }
                    ListViewItem[] items = nodelistview.Items.Find(router.Key, false);
                    if (items.Length > 0)
                    {
                        strname =  CommonBoxOperation.GetNodeName(router.Key);
                        if (null == strname || "".Equals(strname))
                        {
                            items[0].SubItems[1].Text = "****";
                        }
                        else
                        {
                            items[0].SubItems[1].Text = strname;
                        }
                        area = CommonBoxOperation.GetAreaFromNodeID(router.Key);
                        if (null == area)
                        {
                            items[0].SubItems[2].Text = "****";
                        }
                        else
                        {
                            if (null == area.Name || "".Equals(area.Name))
                            {
                                items[0].SubItems[2].Text = area.ID[0].ToString("X2") + area.ID[1].ToString();
                            }
                            else
                            {
                                items[0].SubItems[2].Text = area.Name;
                            }
                        }
                        items[0].SubItems[3].Text = router.Value.ReportTime.ToString();
                    }
                    continue;
                }
                if (!router.Value.status)
                {//断开连接了
                    nodelistview.Items.RemoveByKey(router.Key);
                    continue;
                }
                ListViewItem item = new ListViewItem();
                item.Text = router.Key;
                item.Name = router.Key;
                strname =  CommonBoxOperation.GetNodeName(router.Key);
                if (null == strname || "".Equals(strname))
                {
                    item.SubItems.Add("****");
                }
                else
                {
                    item.SubItems.Add(strname);
                }
                area = CommonBoxOperation.GetAreaFromNodeID(router.Key);
                if (null == area)
                {
                    item.SubItems.Add("****");
                }
                else
                {
                    if (null == area.Name || "".Equals(area.Name))
                    {
                        item.SubItems.Add(area.ID[0].ToString("X2") + area.ID[1].ToString());
                    }
                    else
                    {
                        item.SubItems.Add(area.Name);
                    }
                }
                item.SubItems.Add(router.Value.ReportTime.ToString());
                nodelistview.Items.Add(item);
            }
        }
        private void nodelistview_Click(object sender, EventArgs e)
        {
            if (nodelistview.SelectedItems.Count <= 0)
            {
                return;
            }
            string strid = nodelistview.SelectedItems[0].Name;
            Router mrouter = null;
            if (!CommonCollection.Routers.TryGetValue(strid, out mrouter))
            {
                return;
            }
            //发送设备查询
            #region
            /*每次在打开设置窗体之前先发送一个查询节点类型的封包,主要有两个作用：
                 * 1、由于Node上报的数据包不包含设备类型，进入设置窗体时，并不知道Node的rj45还是wifi
                 * 2、可以用于检测网络连接状态
                */
            /*查询Node设备类型及固件版本号
             * PC --> Node：F1 + 11 + ID(2byte) + CS + F2
             * Node --> PC：FA + 11 + ID(2byte) + type + Version + CS + FB
            */
            byte[] bytes = new byte[6];
            bytes[0] = 0xF1; bytes[1] = 0x11;
            System.Buffer.BlockCopy(mrouter.ID, 0, bytes, 2, 2);
            bytes[bytes.Length - 2] = 0;
            for (int i = 0; i < bytes.Length - 2; i++)
            {
                bytes[bytes.Length - 2] += bytes[i];
            }
            bytes[bytes.Length - 1] = 0xF2;
            SysParam.scandevtype = false;
            SysParam.mnodemsg = null;
            SysParam.sendcount = 3;
            while (true)
            {
                SysParam.sendcount--;
                SysParam.tickcount = Environment.TickCount;
                try
                {
                    frm.MyUdpClient.Send(bytes, bytes.Length, mrouter.mendpoint);
                }
                catch (Exception ex)
                {
                    FileOperation.WriteLog("发送查询设备类型及版本发生异常!异常原因：" + ex.ToString());
                }
                while (Environment.TickCount - SysParam.tickcount < ConstInfor.recnwtimeout + 300 && !SysParam.scandevtype)
                {
                    Thread.Sleep(1);
                }
                if (SysParam.sendcount <= 0 || SysParam.scandevtype)
                {
                    break;
                }
            }
            if (!SysParam.scandevtype || null == SysParam.mnodemsg)
            {
                MessageBox.Show("The connection device failed, please retry again, if many failures may be the network disconnection!");
                return;
            }
            #endregion
            NodeSettingDevWin MyNodeSettingDevWin = new NodeSettingDevWin(frm,SysParam.mnodemsg);
            MyNodeSettingDevWin.ShowDialog();
        }
    }
}
