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
    public partial class Current_OnLine : Form
    {
        Form1 frm = null;
        private Timer UpdateCurListTask = null;
        public Current_OnLine()
        {
            InitializeComponent();
        }
        public Current_OnLine(Form1 frm)
        {
            InitializeComponent();
            this.frm = frm;
        }
        private void Current_OnLine_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new System.Drawing.Size(1171, 712);
            this.MinimumSize = new System.Drawing.Size(1171, 712);
            UpdateCurListView(null,null);
            if (UpdateCurListTask == null)
                UpdateCurListTask = new Timer();
            UpdateCurListTask.Interval = 1000;
            UpdateCurListTask.Tick += UpdateCurListView;
            UpdateCurListTask.Start();
        }
        private void UpdateCurListView(Object obj, EventArgs args)
        {
            TimeSpan TabSpan;
            string StrName = "", StrGroup = "";
            Group selegp = null;
            //lock(frm.CardImgs_Lock)
            {
                foreach (KeyValuePair<string,CardImg> card in frm.CardImgs)
                {
                    ListViewItem item = null;
                    string StrID = card.Value.ID[0].ToString("X2") + card.Value.ID[1].ToString("X2");
                    if (Current_OnLineListView.Items.ContainsKey(StrID))
                    {
                        item = Current_OnLineListView.FindItemWithText(StrID, false, 0);
                        if (item!=null)
                        {
                            StrName = Ini.GetValue(Ini.CardPath,StrID,Ini.Name);
                            if (StrName != null && !"".Equals(StrName))
                                item.SubItems[1].Text = StrName;
                            if (card.Value.LocaType == PrecisePositionLibrary.TPPID.TagCommonLocal)
                            {item.SubItems[2].Text = "General";
                            }else item.SubItems[2].Text = "Emergency";

                            StrGroup = card.Value.GroupID[0].ToString("X2") + card.Value.GroupID[1].ToString("X2");
                            if (frm.Groups.TryGetValue(StrGroup, out selegp))
                            {
                                StrGroup = (null != selegp.name && !"".Equals(selegp.name)) ? selegp.name : StrGroup;
                            }
                            item.SubItems[3].Text = StrGroup;

                            item.SubItems[4].Text = card.Value.Battery.ToString();
                            item.SubItems[5].Text = card.Value.No_Exe_Time.ToString();
                            item.SubItems[6].Text = card.Value.ReceiTime.ToString();
                            TabSpan = (DateTime.Now - card.Value.ReceiTime);
                            item.SubItems[7].Text = Math.Round(TabSpan.TotalSeconds,0)+ " s";

                            item.SubItems[8].Text = "X: " + ((float)card.Value.GsensorX / 100).ToString("0.00") + "g Y: " + ((float)card.Value.GsensorY / 100).ToString("0.00") + "g Z: " + ((float)card.Value.GsensorZ / 100).ToString("0.00")+"g";
                            continue;
                        }
                    }
                    item = new ListViewItem();
                    String strID = card.Value.ID[0].ToString("X2") + card.Value.ID[1].ToString("X2");
                    item.Text = strID;
                    item.Name = strID;
                    //得到卡片的名称
                    StrName = Ini.GetValue(Ini.CardPath,strID, Ini.Name);
                    if (StrName != null && !"".Equals(StrName))item.SubItems.Add(StrName);
                    else item.SubItems.Add("****");
                    if (card.Value.LocaType == PrecisePositionLibrary.TPPID.TagCommonLocal)item.SubItems.Add("General");
                    else item.SubItems.Add("Emergency");
                    //显示组别讯息
                    StrGroup = card.Value.GroupID[0].ToString("X2") + card.Value.GroupID[1].ToString("X2");
                    if (frm.Groups.TryGetValue(StrGroup, out selegp))
                    {
                        StrGroup = (null != selegp.name && !"".Equals(selegp.name)) ? selegp.name : StrGroup;
                    }
                    item.SubItems.Add(StrGroup);
                    item.SubItems.Add(card.Value.Battery.ToString());
                    item.SubItems.Add(card.Value.No_Exe_Time.ToString());
                    item.SubItems.Add(card.Value.ReceiTime.ToString());
                    TabSpan = (DateTime.Now - card.Value.ReceiTime);
                    item.SubItems.Add(Math.Round(TabSpan.TotalSeconds,0)+" s");
                    item.SubItems.Add("X: " + ((float)card.Value.GsensorX / 100).ToString("0.00") + "g Y: " + ((float)card.Value.GsensorY / 100).ToString("0.00") + "g Z: " + ((float)card.Value.GsensorZ / 100).ToString("0.00") + "g");

                    Current_OnLineListView.Items.Add(item);
                }
            }
        }
        private void Current_OnLine_FormClosed(object sender, FormClosedEventArgs e){
            if (UpdateCurListTask.Enabled)
                UpdateCurListTask.Stop();
            UpdateCurListTask = null;
        }

        private void Current_OnLineListView_SelectedIndexChanged(object sender, EventArgs e){}
    }
}
