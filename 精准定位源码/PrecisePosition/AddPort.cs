using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Point = PrecisePositionLibrary.Point;

namespace PrecisePosition
{
    public partial class AddPort : Form
    {
        Form1 frm = null;
        Point point = null;
        string PortID = "";
        string strcurgroupid = "";
        public AddPort()
        {
            InitializeComponent();
        }
        public AddPort(Form1 frm,Point point,string strgroupid)
        {
            InitializeComponent();
            this.frm = frm;
            PortID_01_textBox.Text = "00";
            PortID_02_textBox.Text = "00";
            float x = -1, y = -1;
            double d0, d1, L0, L1, p0, p1;
            x = (float)point.x; y = (float)point.y;
            d0 = d1 = L0 = L1 = p0 = p1 = 0;
            d0 = Math.Abs(DxfMapParam.CenterY - y);
            d1 = Math.Abs(DxfMapParam.CenterX - x);
            L0 = Math.Pow(Math.Pow(d0, 2) + Math.Pow(d1, 2), 0.5);
            L1 = L0 * DxfMapParam.scale;
            p0 = (d0 / L0) * L1;
            p1 = (d1 / L0) * L1;
            if (x < DxfMapParam.CenterX && y < DxfMapParam.CenterY)
            {
                x = (float)(DxfMapParam.PanelCenterX - p1);
                y = (float)(DxfMapParam.PanelCenterY - p0);
            }
            else if (x > DxfMapParam.CenterX && y < DxfMapParam.CenterY)
            {
                x = (float)(DxfMapParam.PanelCenterX + p1);
                y = (float)(DxfMapParam.PanelCenterY - p0);
            }
            else if (x < DxfMapParam.CenterX && y > DxfMapParam.CenterY)
            {
                x = (float)(DxfMapParam.PanelCenterX - p1);
                y = (float)(DxfMapParam.PanelCenterY + p0);
            }
            else if (x > DxfMapParam.CenterX && y > DxfMapParam.CenterY)
            {
                x = (float)(DxfMapParam.PanelCenterX + p1);
                y = (float)(DxfMapParam.PanelCenterY + p0);
            }
            this.point = new Point(x, y, 0);
            this.strcurgroupid = strgroupid;
        }
        public AddPort(Form1 frm, Point point)
        {
            InitializeComponent();
            this.frm = frm;
            PortID_01_textBox.Text = "00";
            PortID_02_textBox.Text = "00";
            //计算当前的参考点绝对坐标
            float x = -1, y = -1;
            double d0, d1, L0, L1, p0, p1;
            //其中ex.X、ex.Y是参考点绝对位置
            x = (float)point.x; y = (float)point.y;
            d0 = d1 = L0 = L1 = p0 = p1 = 0;
            d0 = Math.Abs(DxfMapParam.CenterY - y);
            d1 = Math.Abs(DxfMapParam.CenterX - x);
            L0 = Math.Pow(Math.Pow(d0, 2) + Math.Pow(d1, 2), 0.5);
            L1 = L0 * DxfMapParam.scale;
            p0 = (d0 / L0) * L1;
            p1 = (d1 / L0) * L1;
            if (x < DxfMapParam.CenterX && y < DxfMapParam.CenterY)
            {
                x = (float)(DxfMapParam.PanelCenterX - p1);
                y = (float)(DxfMapParam.PanelCenterY - p0);
            }
            else if (x > DxfMapParam.CenterX && y < DxfMapParam.CenterY)
            {
                x = (float)(DxfMapParam.PanelCenterX + p1);
                y = (float)(DxfMapParam.PanelCenterY - p0);
            }
            else if (x < DxfMapParam.CenterX && y > DxfMapParam.CenterY)
            {
                x = (float)(DxfMapParam.PanelCenterX - p1);
                y = (float)(DxfMapParam.PanelCenterY + p0);
            }
            else if (x > DxfMapParam.CenterX && y > DxfMapParam.CenterY)
            {
                x = (float)(DxfMapParam.PanelCenterX + p1);
                y = (float)(DxfMapParam.PanelCenterY + p0);
            }
            this.point = new Point(x, y, 0);
        }
        public AddPort(Form1 frm,String PortID)
        {
            InitializeComponent();
            this.frm = frm;
            PortID_01_textBox.Text = PortID.Substring(0, 2);
            PortID_02_textBox.Text = PortID.Substring(2, 2);
            this.PortID = PortID;
            if (frm.InnerPorts.ContainsKey(PortID))
            {
                PrecisePositionLibrary.BsInfo prt = null;
                frm.InnerPorts.TryGetValue(PortID, out prt);
                PortHeightTB.Text = "" + Math.Round(prt.Place.z, 0);
            }
        }
        public AddPort(Form1 frm, String PortID, String strgroupid)
        {
            InitializeComponent();
            this.strcurgroupid = strgroupid;
            this.frm = frm;
            PortID_01_textBox.Text = PortID.Substring(0, 2);
            PortID_02_textBox.Text = PortID.Substring(2, 2);
            this.PortID = PortID;
            Group group = null;
            if (frm.Groups.TryGetValue(strgroupid, out group))
            {
                PrecisePositionLibrary.BsInfo prt = null;
                if (group.groupbss.TryGetValue(PortID, out prt))
                {
                    PortHeightTB.Text = "" + Math.Round(prt.Place.z, 0);
                }
            }
        }
        private void Add_Btn_Click(object sender, EventArgs e)
        {
            string ID1 = PortID_01_textBox.Text.ToUpper();
            string ID2 = PortID_02_textBox.Text.ToUpper();
            string strheight = PortHeightTB.Text;
            int height = 0;
            byte[] BYTE_ID = new byte[2];
            if (ID1.Equals("") && ID2.Equals(""))
            {
                MessageBox.Show("Sorry, Reference point ID can not be empty！");
                return;
            }
            if (ID1.Length != 2 || ID2.Length != 2)
            {
                MessageBox.Show("Sorry, Reference point ID format is wrong！");
                return;
            }
            try
            {
                BYTE_ID[0] = Convert.ToByte(ID1, 16);
                BYTE_ID[1] = Convert.ToByte(ID2, 16);
            }
            catch (Exception)
            {
                MessageBox.Show("Sorry, Reference point ID format is wrong！");
                return;
            }
            if (BYTE_ID[0] == 0x00 && BYTE_ID[1] == 0x00)
            {
                MessageBox.Show("Sorry, Reference point ID not for 0000！");
                return;
            }
            try
            {
                height = Convert.ToInt32(strheight);
            }
            catch (Exception)
            {
                MessageBox.Show("Sorry, the height of the base station is wrong！");
                return;
            }
            if (height < 0)
            {
                MessageBox.Show("Sorry, base station height cannot be less than 0！");
                return;
            }
            // 判断当前是多区域还是单区域
            if (null == strcurgroupid || "".Equals(strcurgroupid))
            {// 单区域
                if (!PortID.Equals(""))
                {
                    if (!PortID.Equals(ID1 + ID2))
                    {
                        if (frm.InnerPorts.ContainsKey(ID1 + ID2))
                        {
                            MessageBox.Show("Sorry, the newly modified base station already exists!");
                            return;
                        }
                        PrecisePositionLibrary.BsInfo prt = null;
                        if (frm.InnerPorts.ContainsKey(PortID))
                        {// 包含这个参考点
                            frm.InnerPorts.TryGetValue(PortID, out prt);
                            PrecisePositionLibrary.BsInfo prt_ = new PrecisePositionLibrary.BsInfo();
                            try
                            {
                                prt_.ID[0] = Convert.ToByte(ID1, 16);
                                prt_.ID[1] = Convert.ToByte(ID2, 16);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Sorry, Point of reference for the change ID failure！");
                                return;
                            }
                            prt_.Place = prt.Place;
                            prt_.Place.z = height;
                            PrecisePositionLibrary.BsInfo removebs = null;
                            frm.InnerPorts.TryRemove(PortID, out removebs);
                            frm.InnerPorts.TryAdd(ID1 + ID2, prt_);
                            this.Close();
                        }
                    }
                    else
                    {
                        //说明是重新修改当前基站的高度 
                        if (frm.InnerPorts.ContainsKey(PortID))
                        {
                            PrecisePositionLibrary.BsInfo prt = null;
                            frm.InnerPorts.TryGetValue(PortID, out prt);
                            prt.Place.z = height;
                            this.Close();
                        }
                    }
                    return;
                }
                PrecisePositionLibrary.BsInfo port = null;
                if (frm.InnerPorts.ContainsKey(ID1 + ID2))
                {// 包含这个参考点
                    MessageBox.Show("Sorry, Reference points already exists！");
                    return;
                }
                else
                {
                    //没有包含这个参考点，将这个参考点添加到集合中去
                    port = new PrecisePositionLibrary.BsInfo();
                    port.ID = BYTE_ID;
                    if (point != null)
                    {
                        port.Place = point;
                    }
                    port.Place.z = height;
                    frm.InnerPorts.TryAdd(ID1 + ID2, port);
                }
            }
            else
            {//多个区域
                //判断当前是修改区域基站讯息，还是添加区域基站讯息
                Group curgroup = null;
                if (!frm.Groups.TryGetValue(strcurgroupid, out curgroup))
                {
                    return;
                }
                if (!PortID.Equals(""))
                {
                    if (!PortID.Equals(ID1 + ID2))
                    {
                        PrecisePositionLibrary.BsInfo prt = null;
                        //先判断新改的ID是否已经存在
                        if (frm.IsExistRefer(ID1 + ID2, out prt))
                        {
                            MessageBox.Show("Sorry, the newly modified base station already exists in the area!");
                            return;
                        }
                        if (curgroup.groupbss.ContainsKey(PortID))
                        {//此时我们需要删除掉原来的基站,并重新设置新的基站
                            curgroup.groupbss.TryGetValue(PortID, out prt);
                            PrecisePositionLibrary.BsInfo prt_ = new PrecisePositionLibrary.BsInfo();
                            try
                            {
                                prt_.ID[0] = Convert.ToByte(ID1, 16);
                                prt_.ID[1] = Convert.ToByte(ID2, 16);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Sorry, Point of reference for the change ID failure!");
                                return;
                            }
                            prt_.Place   = prt.Place;
                            prt_.Place.z = height;
                            System.Buffer.BlockCopy(curgroup.id, 0, prt_.GroupID, 0, 2);
                            curgroup.groupbss.Add(ID1 + ID2, prt_);
                            curgroup.groupbss.Remove(PortID);
                            this.Close();
                        }
                    }
                    else
                    {
                        if (curgroup.groupbss.ContainsKey(PortID))
                        {
                            PrecisePositionLibrary.BsInfo prt = null;
                            if (curgroup.groupbss.TryGetValue(PortID, out prt))
                            {
                                prt.Place.z = height;
                                System.Buffer.BlockCopy(curgroup.id, 0, prt.GroupID, 0, 2);
                            }
                            this.Close();
                        }
                    }
                    return;
                }
                PrecisePositionLibrary.BsInfo port = null;
                if (frm.IsExistRefer(ID1 + ID2,out port))
                {
                    MessageBox.Show("Sorry, Reference points already exists！");
                    return;
                }
                else
                {
                    //没有包含这个参考点，将这个参考点添加到集合中去
                    port = new PrecisePositionLibrary.BsInfo();
                    port.ID = BYTE_ID;
                    System.Buffer.BlockCopy(curgroup.id, 0, port.GroupID, 0, 2);
                    if (point != null)
                    {
                        port.Place = point;
                    }
                    port.Place.z = height;
                    curgroup.groupbss.Add(ID1 + ID2, port);
                }
            }
            this.Close();
        }
        private void GoBack_Btn_Click(object sender, EventArgs e)
        {
            //返回按钮
            this.Close();
        }
        private void Dele_Btn_Click(object sender, EventArgs e)
        {
            //从列表中删除参考点
            if (PortID == null || PortID.Equals(""))
            {
                return;
            }
            PrecisePositionLibrary.BsInfo removebs = null;
            Group grp = null;
            if (null != strcurgroupid && !"".Equals(strcurgroupid))
            {
                if (frm.Groups.TryGetValue(strcurgroupid, out grp))
                {
                    grp.groupbss.Remove(PortID);
                }
            }
            else
            {
                if (frm.InnerPorts.ContainsKey(PortID))
                {
                    frm.InnerPorts.TryRemove(PortID, out removebs);
                }
            }
            this.Close();
        }
        private void AddPort_FormClosing(object sender, FormClosingEventArgs e)
        {
            frm.LoadDxfMap();
        }
    }
}
