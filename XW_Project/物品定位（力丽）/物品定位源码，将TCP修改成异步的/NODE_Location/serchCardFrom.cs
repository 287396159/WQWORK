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
    public partial class serchCardFrom : Form
    {

        public delegate void serchCardHandle(string cardID);
        public serchCardHandle serchCard;

        public serchCardFrom()
        {
            InitializeComponent();
        }

        private void serchCardFrom_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            label2.Text = "搜索結果:";
            if (serchCard != null) serchCard(textBox1.Text);
        }

        public void tagSerchHandle(bool result)       
        {
            if (result) this.Close();
            else label2.Text = "搜索結果:失敗";
        }

    }
}
