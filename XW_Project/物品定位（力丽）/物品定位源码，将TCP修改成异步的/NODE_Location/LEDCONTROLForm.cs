using CiXinLocation.Model;
using SerialportSample;
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
    public partial class LEDCONTROLForm : Form
    {
        public LEDCONTROL_model LEDmodel;
        private string ledNmae = "LED";
        private byte[] cardID;
        private byte type;
        private int sleepTime;
        private byte gongLv = 0xff;//功率

        public byte GongLv
        {
            get { return gongLv; }
            set { gongLv = value; }
        }

        public int SleepTime
        {
            get { return sleepTime; }
            set { sleepTime = value; }
        }

        public byte MType
        {
            get { return type; }
            set { type = value; }
        }      

        public byte[] CardID
        {
            get { return cardID; }
            set { cardID = value; }
        }
         
        public string LedNmae
        {
            get { return ledNmae; }
            set { ledNmae = value; }
        }

        public LEDCONTROLForm()
        {
            LEDmodel = new LEDCONTROL_model();
            LEDmodel.labVisi += labVisi;
            LEDmodel.labSet += labSetComText;
            if (MType == 1) groupBox1.BackColor = Color.Black;
            else if (MType == 2) groupBox5.BackColor = Color.Black;
                InitializeComponent();
        }      

        private void LEDCONTROLForm_Load(object sender, EventArgs e)
        {
            this.Text = ledNmae;
            if (MType == 1)
            {
                this.Width = 913;
                this.Height = 386;
                groupBox1.Visible = false;
                Point point = new Point(12, 12);
                groupBox5.Location = point;
                Point point2 = new Point(235, 12);
                groupBox6.Location = point2;
                Point point3 = new Point(458, 12);
                groupBox7.Location = point3;
                Point point4 = new Point(681, 12);
                groupBox8.Location = point4;

                this.Text += " --- LED-7045";
            }
            else if (MType == 2) {
                groupBox5.Visible = false;
                this.Width = 813;
                this.Height = 697;
                Point point = new Point(12, 12);
                groupBox1.Location = point;
                Point point2 = new Point(573, 12);
                groupBox6.Location = point2;
                Point point3 = new Point(12, 336);
                groupBox7.Location = point3;
                Point point4 = new Point(253, 336);
                groupBox8.Location = point4;

                this.Text += " --- LED-E290";         
            }    
            LEDmodel.LedID = cardID;

            if (sleepTime > 0) 
            {
                int time = sleepTime * 5;
                if (time > 255) time = 255;
                textBox14.Text = time.ToString();
                textBox13.Text = time.ToString();
                textBox16.Text = time.ToString();
                textBox15.Text = sleepTime.ToString();
            }            

            if (gongLv >= 0 && gongLv < 16) comboBox1.Text = gongLv.ToString();
            else comboBox1.Text = comboBox1.Items.Count > 0 ? comboBox1.Items[0].ToString() : "0";
        }

        private void number_keyPress(object sender, KeyPressEventArgs e)
        {
            XWUtils.justNumberInput(e);
        }

        private int LED7045Type() 
        {
            int ledTy = 0;
            if (radioButton11.Checked) ledTy = 1;
            else if (radioButton10.Checked) ledTy = 2;
            return ledTy;
        }

        private int LEDE290Type()
        {
            int ledTy = 0;
            if (radioButton1.Checked) ledTy |= 1;         //1 = 红亮  0000 0001
            //else if (radioButton2.Checked) ledTy = 2;   //2 = 红灭  0000 0000
            else if (radioButton3.Checked) ledTy |= 2;    //2 = 红闪烁 0000 0010

            if (radioButton5.Checked) ledTy |= 4;         //1 = 绿亮  0000 0100
            //else if (radioButton2.Checked) ledTy = 2;   //2 = 绿灭  0000 0000
            else if (radioButton4.Checked) ledTy |= 8;    //2 = 绿闪烁 0000 1000

            if (radioButton8.Checked) ledTy |= 16;        //1 = 红亮  0001 0000
            //else if (radioButton2.Checked) ledTy = 2;   //2 = 红灭  0000 0000
            else if (radioButton7.Checked) ledTy |= 32;   //2 = 红闪烁 0010 0000
             
            return ledTy;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MType == 1) LEDmodel.sendLED7045com(LED7045Type(), textBox12.Text, textBox11.Text,
                textBox10.Text, textBox13.Text);
            else if (MType == 2) LEDmodel.sendLEDE290com(LEDE290Type(), textBox1.Text, textBox2.Text, textBox3.Text,textBox6.Text, textBox5.Text, textBox4.Text, textBox9.Text, textBox8.Text, textBox7.Text, textBox13.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LEDmodel.setLEDsleepTime(textBox15.Text,textBox14.Text);
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e) //亮
        {
            textBox11.Enabled = false;
            textBox10.Enabled = true;
            textBox12.Enabled = false;
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)//灭
        {
            textBox11.Enabled = false;
            textBox10.Enabled = false;
            textBox12.Enabled = false;
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)//闪烁
        {
            textBox11.Enabled = true;
            textBox10.Enabled = true;
            textBox12.Enabled = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = true;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)//闪
        {
            textBox6.Enabled = true;
            textBox5.Enabled = true;
            textBox4.Enabled = true;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            textBox6.Enabled = false;
            textBox5.Enabled = false;
            textBox4.Enabled = false;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e) //绿 亮
        {
            textBox6.Enabled = false;
            textBox5.Enabled = false;
            textBox4.Enabled = true;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e) //蓝 闪
        {
            textBox9.Enabled = true;
            textBox8.Enabled = true;
            textBox7.Enabled = true;
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e) //蓝 灭
        {
            textBox9.Enabled = false;
            textBox8.Enabled = false;
            textBox7.Enabled = false;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            textBox9.Enabled = false;
            textBox8.Enabled = false;
            textBox7.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="type">1 = 设置命令的Label，2 = 设置睡眠时间Label,3 = 設置發射功率</param>
        public void labSetComText(string text,int type) 
        {
            this.Invoke((EventHandler)(delegate
            { //放入主線程    
                if (1 == type) label46.Text = text;
                else if (2 == type) label47.Text = text;
                else if (3 == type) label48.Text = text;
            }));            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">1 = 设置命令的Label，2 = 设置睡眠时间Label</param>
        /// <param name="visi"></param>
        public void labVisi(int type, bool visi) 
        {
            try {
                this.Invoke((EventHandler)(delegate
                { //放入主線程    
                    if (1 == type && label46.Visible != visi) label46.Visible = visi;
                    else if (2 == type && label47.Visible != visi) label47.Visible = visi;
                    else if (3 == type && label48.Visible != visi) label48.Visible = visi;
                }));   
            }
            catch { }             
        }

 

        private void button3_Click(object sender, EventArgs e)
        {
            LEDmodel.setLED(comboBox1.Text, textBox16.Text,0x16);
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
