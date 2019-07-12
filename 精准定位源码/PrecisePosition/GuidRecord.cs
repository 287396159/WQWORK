using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
namespace PrecisePosition
{
    public partial class GuidRecord : Form
    {
        Form1 frm = null;
        Timer MyTimer = null;
       
        public GuidRecord()
        {
            InitializeComponent();
        }
        public GuidRecord(Form1 frm)
        {
            InitializeComponent();
            this.frm = frm;
        }
        private void GuidRecord_Load(object sender, EventArgs e)
        {
            if (null == frm) return;
            int x = frm.Location.X + frm.Size.Width - this.Size.Width;
            this.Location = new System.Drawing.Point(x - 18, frm.Map_panel.Location.Y + frm.TagPageBox.Location.Y);
            if (MyTimer == null)
            {
                MyTimer = new Timer();
            }
            MyTimer.Interval =(Parameter.isDefineInterval ? Parameter.DefineInterval : 1000);
            MyTimer.Tick += MyTimer_Tick;
            MyTimer.Start();
        }
        private void MyTimer_Tick(Object obj,EventArgs args)
        {
            CardImg tagimg = null;
            string strname = "",strtag = "";
            if (null == frm) return;
            recordtb.Clear();
            try
            {
                foreach (KeyValuePair<string, PrecisePositionLibrary.TagPack> tag in PrecisePositionLibrary.PrecisePosition.TagPacks)
                {
                    //判断是否是跟踪模式
                    if (frm.isTrace)
                    {
                        if (!tag.Key.Equals(frm.TraceTagId))
                        {
                            continue;
                        }
                    }
                    frm.CardImgs.TryGetValue(tag.Key, out tagimg);
                    if (null == tagimg || !tagimg.isShowTag)
                    {
                        continue;
                    }
                    if (null == recordtb || recordtb.IsDisposed)
                    {
                        return;
                    }
                    strname = Ini.GetValue(Ini.CardPath, tag.Key, Ini.Name);
                    strtag = ((null == strname) || "".Equals(strname)) ? (tag.Key) : (strname + "(" + tag.Key + ")");
                    recordtb.AppendText("####TagID:" + strtag + " index:" + tag.Value.index + " BasicStationTotalNum:" + tag.Value.TotalbasicNum + " ReceiveBasicStationNum:" + tag.Value.CurBasicNum + "\r\n");
                    foreach (KeyValuePair<string, PrecisePositionLibrary.BasicReportTag> br in tag.Value.CurBasicReport)
                    {
                        strname = Ini.GetValue(Ini.PortPath, br.Key,Ini.Name);
                        strtag = ((null == strname) || "".Equals(strname)) ? (br.Key) : (strname + "(" + br.Key + ")");
                        recordtb.AppendText("        BasicStationID:" + strtag + " Priority:" + br.Value.Priority + " Distanse:" + string.Format("{0:F2}", br.Value.distanse) + "\r\n");
                    }
                }
            }
            catch (Exception) { }
        }

        

    }
}
