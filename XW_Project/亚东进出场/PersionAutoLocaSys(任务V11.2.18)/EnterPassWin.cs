using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace PersionAutoLocaSys
{
    public partial class EnterPassWin : Form
    {
        private Form1 mainfrm = null;
        private static SByte count = 3;
        private OperType curopertype;
        public EnterPassWin()
        {
            InitializeComponent();
        }
        public EnterPassWin(Form1 mainfrm)
        {
            InitializeComponent();
            this.mainfrm = mainfrm;
        }
        public EnterPassWin(Form1 mainfrm,OperType curopertype)
        {
            InitializeComponent();
            this.mainfrm = mainfrm;
            this.curopertype = curopertype;
        }
        private void EnterPassWin_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new Size(232, 174);
            this.MinimumSize = new Size(232, 174);
            this.Location = new Point(mainfrm.Location.X + (mainfrm.Width - this.Width) / 2, mainfrm.Location.Y + (mainfrm.Height - this.Height) / 2);
            count = 3;
            this.TopMost = true;
            
            if (curopertype == OperType.CloseForm)
            {
                this.MaximumSize = new Size(229, 229);
                this.MinimumSize = new Size(229, 229);
                exittypebox.Location = new Point(21, 20);
                PWAutherBox.Location = new Point(20, 57);
            }
            else
            {
                this.MaximumSize = new Size(232, 174);
                this.MinimumSize = new Size(232, 174);
            }
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == ConstInfor.FORMMESSAGE && (int)m.WParam == ConstInfor.CLOSEMSGPARAM)
            {
                //捕捉关闭窗体消息
                if (curopertype == OperType.OpenForm)
                {
                    mainfrm.Close();
                }
            }
            else if (m.Msg == ConstInfor.FORMMESSAGE && m.WParam ==ConstInfor.FORMMSGMOVE)
            {//移动窗体的事件不处理
                return;
            }
            base.WndProc(ref m);
        }
        /// <summary>
        /// 修改用户名及密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModifyUser_LB_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PWAutherBox.Visible = false;
            ModifyPWBox.Location = new Point(12, 18);
            this.MaximumSize = new System.Drawing.Size(232, 300);
            this.MinimumSize = new System.Drawing.Size(232, 220);
            ModifyPWBox.Visible = true;
        }

        private Boolean VerifyUserMsg(Person curperson,string name,string psw)
        {
            if (null == curperson && mainfrm.CurPerson != null)
            {
                if (mainfrm.CurPerson.ID[0] == 0xFF && mainfrm.CurPerson.ID[1] == 0xFF)
                {//公司内部使用
                    if (ConstInfor.dmatekname.ToUpper().Equals(name.ToUpper()) && 
                         ConstInfor.dmatekpsw.ToUpper().Equals(psw.ToUpper()))
                    {
                        return true;
                    }
                }
            }

            if (curperson == null || mainfrm.CurPerson == null)
            {
                return false;
            }
            if(!mainfrm.CurPerson.Name.ToUpper().Equals(curperson.Name.ToUpper()))
            {//用户名不相同
                return false;
            }
            if(!curperson.Ps.ToUpper().Equals(mainfrm.CurPerson.Ps.ToUpper()))
            {//用户密码不同
                return false;
            }
            return true;
        }

        /// <summary>
        // 开始验证密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Btn_Click(object sender, EventArgs e)
        {
            string UserName = UserName_TB.Text.Trim();
            string Pass = Pass_TB.Text.Trim();
            if ("".Equals(UserName))
            {
                MessageBox.Show("對不起，用戶名不能為空!");
                return;
            }
            if (logoutcb.Checked)
            {
                mainfrm.mexittype = ExitType.LogOut;
            }
            else
            {
                mainfrm.mexittype = ExitType.AppExit;
            }
            Person mperson = GetUser(UserName, Pass);
            if (mainfrm.CurPerson == null)
            {//说明此时是用户登录操作
                if (ConstInfor.dmatekname.ToUpper().Equals(UserName.ToUpper()) && ConstInfor.dmatekpsw.ToUpper().Equals(Pass.ToUpper()))
                {   //登陆时发现输入的账号及密码时dmatek和dmatek1234时，说明是本公司的人员
                    byte[] dmatekid = new byte[2];
                    dmatekid[0] = 0xFF; dmatekid[1] = 0xFF;
                    mperson = new Person(dmatekid, ConstInfor.dmatekname, ConstInfor.dmatekpsw,1);
                }
                mainfrm.CurPerson = mperson;
                if (null != mainfrm.CurPerson)
                {
                    mainfrm.Text = "人員定位系統V11.2.18 (當前用戶: " + mainfrm.CurPerson.Name + ")";
                    PersonOperation curpersonoper = new PersonOperation(mainfrm.CurPerson.ID, OperType.LoginIn);
                    CommonCollection.personOpers.Add(curpersonoper);
                }
            }
            if (!VerifyUserMsg(mperson, UserName, Pass))
            {
                if (--count > 0)
                {
                    MessageBox.Show("用戶名或密碼不正確!");
                    return;
                }
                else
                {//超过3次密码都输入错误时，退出应用
                    MessageBox.Show("對不起,用戶名或密碼錯誤,退出登錄/驗證窗體...");
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    if (curopertype == OperType.OpenForm)
                    {
                        mainfrm.Close();
                    }
                    return;
                }
            }
            this.DialogResult = DialogResult.OK;
            //此时记录下操作
            if (curopertype == OperType.OpenForm || curopertype == OperType.CloseForm)
            {
                PersonOperation curpersonoper = null;
                if (curopertype == OperType.CloseForm)
                {
                    if (mainfrm.mexittype == ExitType.AppExit)
                    {
                        curpersonoper = new PersonOperation(mainfrm.CurPerson.ID,OperType.CloseForm);
                    }
                    else if (mainfrm.mexittype == ExitType.LogOut)
                    {
                        curpersonoper = new PersonOperation(mainfrm.CurPerson.ID, OperType.LoginOut);
                    }
                }
                else if (curopertype == OperType.OpenForm)
                {
                    curpersonoper = new PersonOperation(mainfrm.CurPerson.ID, OperType.OpenForm);
                }
                if (null != curpersonoper)
                {
                    CommonCollection.personOpers.Add(curpersonoper);
                }
            }
            this.Close();
        }
        private Person GetUser(string UserName, string UserPsw)
        {
            ArrayList Users = new ArrayList();
            //得到文件中的所有的键值
            foreach (KeyValuePair<string, Person> person in CommonCollection.Persons)
            {
                if (null == person.Value) 
                    continue;

                if (UserName.ToUpper().Trim().Equals(person.Value.Name.ToUpper().Trim()) && UserPsw.ToUpper().Trim().Equals(person.Value.Ps.ToUpper().Trim()))
                {
                    return person.Value;
                }
            }
            return null;
        }
        private void Cancel_Btn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            if (curopertype == OperType.OpenForm)
            {
                mainfrm.Close();
            }
        }
        private void ModifyEnterBtn_Click(object sender, EventArgs e)
        {
            string StrOldUserName = OldUserNameTB.Text.Trim();
            if ("".Equals(StrOldUserName))
            { 
                MessageBox.Show("對不起，原用戶名不為空!"); 
                return;
            }
            string UserName_Ok = FileOperation.GetValue(FileOperation.SafePath, FileOperation.SafeSeg, FileOperation.UserName);
            if (null == UserName_Ok)
            {
                UserName_Ok = ConstInfor.DefaultUserName;
            }
            if (!StrOldUserName.ToLower().Equals(UserName_Ok.ToLower()))
            { 
                MessageBox.Show("對不起，原用戶名錯誤!");
                return; 
            }
            string StrOldPW = OldPassTB.Text.Trim();
            string OldPW_Ok = FileOperation.GetValue(FileOperation.SafePath, FileOperation.SafeSeg, FileOperation.UserPass);
            if (null != OldPW_Ok)
            {
                OldPW_Ok = Encryption.Decrypt(OldPW_Ok);
            }
            else
            {
                OldPW_Ok = ConstInfor.DefaultPW;
            }
            if (!StrOldPW.ToLower().Equals(OldPW_Ok))
            { 
                MessageBox.Show("對不起，原密碼錯誤!"); 
                return;
            }
            string StrNewUser = NewUserNameTB.Text.Trim();
            if ("".Equals(StrNewUser))
            { 
                MessageBox.Show("對不起，新用戶名不能為空!");
                return;
            }
            string StrNewPW = NewPassTB.Text.Trim();
            FileOperation.SetValue(FileOperation.SafePath, FileOperation.SafeSeg, FileOperation.UserName, StrNewUser.ToLower());
            FileOperation.SetValue(FileOperation.SafePath, FileOperation.SafeSeg, FileOperation.UserPass, Encryption.Encrypti(StrNewPW));
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void logoutcb_CheckedChanged(object sender, EventArgs e)
        {
            if (logoutcb.Checked)
            {
                mainfrm.mexittype = ExitType.LogOut;
                quitappcb.Checked = false;
            }
            else
            {
                mainfrm.mexittype = ExitType.AppExit;
                quitappcb.Checked = true;
            }
        }

        private void quitappcb_CheckedChanged(object sender, EventArgs e)
        {
            if (quitappcb.Checked)
            {
                mainfrm.mexittype = ExitType.AppExit;
                logoutcb.Checked = false;
            }
            else
            {
                mainfrm.mexittype = ExitType.LogOut;
                logoutcb.Checked = true;
            }
        }
        private void EnterPassWin_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (null != mainfrm.MyEnterPassWin && !mainfrm.MyEnterPassWin.IsDisposed)
                {
                    mainfrm.MyEnterPassWin.Dispose(true);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                mainfrm.MyEnterPassWin = null;
            }
        }
    }
    public enum ExitType
    { 
        AppExit,
        LogOut,
        UnKunown
    }
}
