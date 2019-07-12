using CiXinLocation.Model;
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
    public partial class loginForm : Form
    {
        public loginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) //確定按鈕
        {
            string count = textBox1.Text;
            string pasword = textBox2.Text;
            if (count.Length == 0 || pasword.Length == 0) 
            {
                MessageBox.Show("帳號或者密碼不能為空！");
                return;
            }
            string msg = "";
            if (!radioButton2.Checked) msg = mainYanzhen(count, pasword);
            else msg = congjiYanZhen(count, pasword);

            if ("驗證成功！".Equals(msg))
                this.DialogResult = DialogResult.OK;
            else MessageBox.Show(msg);
        }

        //主機驗證
        private string mainYanzhen(string count, string pasword) 
        {
            if (!PeoplePowerModel.getPeoplePowerModel().textPeoplePower(count, pasword))
            {
                //MessageBox.Show();
                return "帳號密碼錯誤！";
            }
            return "驗證成功！";
        }

        //從機驗證
        private string congjiYanZhen(string count, string pasword) 
        {
            PeoplePowerModel.getPeoplePowerModel().congJiPeoplePower(count, pasword);
            return "驗證成功！";
        }

        private void button2_Click(object sender, EventArgs e) //取消 
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public string getAccount() 
        {
            return textBox1.Text;
        }

        private void loginForm_Load(object sender, EventArgs e)
        {
            this.Text = FormMain.getMainName() + "帳號驗證";
            if (FormMain.MainModel == 1)
            {
                radioButton1.Checked = true;
                radioButton2.Visible = false;
            }
            else 
            {
                radioButton2.Checked = true;
                radioButton1.Visible = false;
            }
            PeoplePowerModel.getPeoplePowerModel().startCheck();
            textBox2.Text = "";
            if (PeoplePowerModel.getPeoplePowerModel().Jurisdiction == PeoplePowerModel.getPeoplePowerModel().CongjiValue)
                radioButton2.Checked = true;
        }



    }
}
