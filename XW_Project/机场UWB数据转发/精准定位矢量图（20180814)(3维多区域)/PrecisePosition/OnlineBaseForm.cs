using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PrecisePosition
{
    public partial class OnlineBaseForm : Form
    {
        private Form1 form = null;
        private Timer timer = null;
        public OnlineBaseForm()
        {
            InitializeComponent();
        }
        public OnlineBaseForm(Form1 form)
        {
            InitializeComponent();
            this.form = form;
        }
        private void timer_Tick(Object obj,EventArgs args)
        {
            string str = "",StrGroup = "";
            PrecisePositionLibrary.BsInfo bs = null;
            Group gp = null;
            if(null == form)
                return;
            foreach(KeyValuePair<string,Form1.PortInfor> port in form.Ports)
            {
                if (PortListView.Items.ContainsKey(port.Key))
                {
                    ListViewItem[] items = PortListView.Items.Find(port.Key,false);
                    if (items.Length <= 0)
                    {
                        continue;
                    }
                    //获取指定的基站
                    if (form.IsExistRefer(port.Key, out bs))
                    {
                        StrGroup = bs.GroupID[0].ToString("X2") + bs.GroupID[1].ToString("X2");
                        if (form.Groups.TryGetValue(StrGroup, out gp))
                        {
                            StrGroup = gp.name;
                        }
                    }
                    else
                    {
                        StrGroup = "****";
                    }
                    items[0].SubItems[1].Text = StrGroup;
                    items[0].SubItems[2].Text = port.Value.sleep + " S";
                    items[0].SubItems[3].Text = port.Value.ReportTime.ToString();
                    items[0].SubItems[4].Text = port.Value.ver.ToString("X2");
                    continue;
                }
                ListViewItem item = new ListViewItem();
                item.Name = port.Key;
                str = Ini.GetValue(Ini.PortPath, port.Key, Ini.Name);
                if (null == str || "".Equals(str))
                {
                    item.Text = port.Key;
                }
                else
                {
                    item.Text = str + "(" + port.Key + ")";
                }
                //获取指定的基站
                if (form.IsExistRefer(port.Key, out bs))
                {
                    StrGroup = bs.GroupID[0].ToString("X2") + bs.GroupID[1].ToString("X2");
                    if (form.Groups.TryGetValue(StrGroup, out gp))
                    {
                        StrGroup = gp.name;
                    }
                }
                else
                {
                    StrGroup = "****";
                }
                item.SubItems.Add(StrGroup);
                item.SubItems.Add(port.Value.sleep+" S");
                item.SubItems.Add(port.Value.ReportTime.ToString());
                item.SubItems.Add(port.Value.ver.ToString("X2"));
                PortListView.Items.Add(item);
            }
        }
        private void OnlineBaseForm_Load(object sender, EventArgs e)
        {
            if (null == timer)
                timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
            timer.Start();
        }
    }
}
