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
    public partial class LimitAreaSet : Form
    {
        Form1 frm = null;
        LimitArea curlimitarea = null;
        String strgroupid = "";
        public LimitAreaSet()
        {
            InitializeComponent();
        }
        public LimitAreaSet(LimitArea mLimitArea, Form1 frm)
        {
            InitializeComponent();
            this.frm = frm;
            curlimitarea = mLimitArea;
        }
        public LimitAreaSet(LimitArea mLimitArea, Form1 frm,String strgroupid)
        {
            InitializeComponent();
            this.frm = frm;
            curlimitarea = mLimitArea;
            this.strgroupid = strgroupid;
        }
        private void LimitAreaSet_Load(object sender, EventArgs e)
        {
            id0tx.Text = curlimitarea.ID[0].ToString("X2");
            id1tx.Text = curlimitarea.ID[1].ToString("X2");
            nametb.Text = curlimitarea.Name;
        }
        private void setbtn_Click(object sender, EventArgs e)
        {
            string strid0 = "", strid1 = "";
            strid0 = id0tx.Text;
            strid1 = id1tx.Text;
            if ("".Equals(strid0) || "".Equals(strid1))
            {
                MessageBox.Show("Sorry, the zone ID cannot be empty!");
                return;
            }
            if (strid0.Length != 2 || strid1.Length != 2)
            {
                MessageBox.Show("The area ID format is wrong!");
                return;
            }
            byte id0 = 0, id1 = 0;
            try 
            {
                id0 = Convert.ToByte(strid0,16);
                id1 = Convert.ToByte(strid1,16);
            }catch(Exception)
            {
                MessageBox.Show("Sorry, the regional ID format is wrong!");
                return;
            }
            if (id0 == 0 && id1 == 0)
            {
                MessageBox.Show("Sorry, the zone ID cannot be 0000！");
                return;
            }
            string strid = id0.ToString("X2") + id1.ToString("X2");
            if (curlimitarea.ID[0] == 0 && curlimitarea.ID[1] == 0)
            {//说明是开始时新增的
                LimitArea myarea = new LimitArea();
                myarea.ID[0] = id0; myarea.ID[1] = id1;
                myarea.Name = nametb.Text;
                myarea.startpoint = new Point(curlimitarea.startpoint.x, curlimitarea.startpoint.y, curlimitarea.startpoint.z);
                myarea.endpoint = new Point(curlimitarea.endpoint.x, curlimitarea.endpoint.y, curlimitarea.endpoint.z);
                if (!strgroupid.Equals("") && null != strgroupid)
                {//说明此时是多区域
                    Group curgroup = null;
                    LimitArea selearea = null;
                    if (frm.IsExistArea(strid, out selearea))
                    {
                        MessageBox.Show("Sorry, the added area already exists!");
                        return;
                    }
                    if (frm.Groups.TryGetValue(strgroupid, out curgroup))
                    {
                        try
                        {
                            curgroup.grouplimiares.Add(strid, myarea);
                        }
                        catch (System.ArgumentException)
                        {
                            MessageBox.Show("Sorry, the added area already exists");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry, the area doesn't exist!");
                    }
                }
                else
                {//说明是单区域 
                    try
                    {
                        frm.Areas.TryAdd(strid, myarea);
                    }
                    catch (System.ArgumentException)
                    {
                        MessageBox.Show("Sorry, the added area already exists");
                    }
                }
            }
            else
            { 
                if (curlimitarea.ID[0] != id0 || curlimitarea.ID[1] != id1)
                {
                    MessageBox.Show("Sorry, the zone ID cannot be modified!");
                    return;
                }
                else
                {
                    LimitArea area = null;
                    if (null != strgroupid && !"".Equals(strgroupid))
                    {//说明是多区域
                        Group group = null;
                        if (frm.Groups.TryGetValue(strgroupid, out group))
                        {
                            group.grouplimiares.TryGetValue(strid, out area);
                            if (null == area)
                            {
                                MessageBox.Show("The currently modified area does not exist!");
                                return;
                            }
                            area.Name = nametb.Text;
                        }
                        else
                        {
                            MessageBox.Show("Sorry, the area doesn't exist!");
                        }
                    }
                    else
                    {//说明是单区域
                        frm.Areas.TryGetValue(strid, out area);
                        if (null == area)
                        {
                            MessageBox.Show("The currently modified area does not exist!");
                            return;
                        }
                        area.Name = nametb.Text;
                    }
                }
            }
            this.Close();
        }
        private void deleteBtn_Click(object sender, EventArgs e)
        {
            string strid = curlimitarea.ID[0].ToString("X2") + curlimitarea.ID[1].ToString("X2");
            LimitArea removearea = null;
            if (null != strgroupid && !"".Equals(strgroupid))
            {//多区域
                Group group = null;
                if (frm.Groups.TryGetValue(strgroupid, out group))
                {
                    group.grouplimiares.Remove(strid);
                }
                else
                {
                    MessageBox.Show("Sorry, the area doesn't exist!");
                }
            }
            else
            {//单区域
                if (frm.Areas.ContainsKey(strid))
                {
                    frm.Areas.TryRemove(strid, out removearea);
                }
                else
                {
                    MessageBox.Show("Delete area does not exist!");
                }
            }
            this.Close();
        }
    }
}
