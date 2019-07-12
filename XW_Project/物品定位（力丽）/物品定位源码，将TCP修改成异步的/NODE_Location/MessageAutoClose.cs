using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CiXinLocation
{
    public partial class MessageAutoClose : Form
    {
        private int time; //時間，單位秒

        public int Time
        {
            get { return time; }
            set { time = value; }
        }

        public MessageAutoClose()
        {
            InitializeComponent();
        }

        public MessageAutoClose(int time)
        {
            InitializeComponent();
            this.time = time;
        }

        private void MessageAutoClose_Load(object sender, EventArgs e)
        {
            if (!timer1.Enabled) timer1.Start();
            this.FormClosing += messageFormClosingEventHandler;
        }

        public void show(string msg) 
        {
            label1.Text = msg;
            timer1.Interval = 1000 * Time;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void messageFormClosingEventHandler(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
            timer1.Dispose();
        }


    }
}
