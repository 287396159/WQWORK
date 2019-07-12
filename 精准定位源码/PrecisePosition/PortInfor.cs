using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace PrecisePosition
{
    public partial class PortInfor : Form
    {
        private Form1 frm;
        private Timer UpdateListInfor_Timer = null;
        public PortInfor()
        {
            InitializeComponent();
        }
        public PortInfor(Form1 frm) {
            InitializeComponent();
            this.frm = frm;
        }
        private void UpdateListTask(Object obj,EventArgs args) {
            lock (frm.Ports_Lock)
            {
                foreach (KeyValuePair<string, Form1.PortInfor> port in frm.Ports)
                {
                    ListViewItem item = new ListViewItem();
                    if (Port_listView.Items.ContainsKey(port.Key))
                    {
                        item = Port_listView.FindItemWithText(port.Key, false, 0);
                        if (item != null)
                        {
                            item.SubItems[1].Text = port.Value.ReportTime.ToString();
                            item.SubItems[2].Text = port.Value.ver.ToString("X2");
                        }
                    }
                    else
                    {
                        item.Text = port.Key;
                        item.Name = port.Key;
                        item.SubItems.Add(port.Value.ReportTime.ToString());
                        item.SubItems.Add(port.Value.ver.ToString("X2"));
                        Port_listView.Items.Add(item);
                    }
                }
            }
        }
        private void PortInfor_Load(object sender, EventArgs e)
        {
            UpdateListTask(null,null);
            if (UpdateListInfor_Timer == null)
                UpdateListInfor_Timer = new Timer();
            UpdateListInfor_Timer.Interval = 1000;
            UpdateListInfor_Timer.Tick += UpdateListTask;
            UpdateListInfor_Timer.Start();
        }
        private void PortInfor_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (UpdateListInfor_Timer.Enabled)
                UpdateListInfor_Timer.Stop();
            UpdateListInfor_Timer = null;
        }

        private void Port_listView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
