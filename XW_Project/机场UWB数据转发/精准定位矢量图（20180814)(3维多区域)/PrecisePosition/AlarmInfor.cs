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
    public partial class AlarmInfor : Form
    {
        Form1 frm = null;
        Timer ShowCurAlarmTimer = null;
        private static int CurCount = 0;
        public AlarmInfor()
        {
            InitializeComponent();
        }
        public AlarmInfor(Form1 frm)
        {
            InitializeComponent();
            this.frm = frm;
        }
        private void AlarmInfor_Load(object sender, EventArgs e)
        {
            ShowAlarmInfor_Task(null,null);
            if (ShowCurAlarmTimer == null)
                ShowCurAlarmTimer = new Timer();
            ShowCurAlarmTimer.Interval = 1000;
            ShowCurAlarmTimer.Tick += ShowAlarmInfor_Task;
            ShowCurAlarmTimer.Start();
        }
        //记录当前鼠标的位置
        private void ShowAlarmInfor_Task(Object obj,EventArgs args) 
        {
            int num = frm.AlarmInfors.Count;
            if (CurCount < frm.AlarmInfors.Count)
            {
                for (int i = CurCount; i < frm.AlarmInfors.Count; i++)
                    AlarmInfor_textBox.AppendText(frm.AlarmInfors[i] + "\r\n");
                CurCount = frm.AlarmInfors.Count;
            }
        }
        private void AlarmInfor_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ShowCurAlarmTimer.Enabled)
            {
                ShowCurAlarmTimer.Stop();
            }
            ShowCurAlarmTimer = null;
        }
        private void ClearBtn_Click(object sender, EventArgs e)
        {
            AlarmInfor_textBox.Text = "";
            frm.AlarmInfors.Clear();
            frm.Alarminfor_textBox.Text = "";
            CurCount = 0;
        }
    }
}
