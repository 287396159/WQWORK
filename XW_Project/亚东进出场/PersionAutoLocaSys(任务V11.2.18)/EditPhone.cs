using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PersionAutoLocaSys
{
    public partial class EditPhone : Form
    {
        public EditPhone()
        {
            InitializeComponent();
        }
        private void EditPhone_Load(object sender, EventArgs e)
        {
            UpdatePersonLv();
        }
        //刷新列表中的内容
        private void UpdatePersonLv()
        {
            PersonNumberLV.Items.Clear();
            ListViewItem item = null;
            foreach (PhonePerson PPson in CommonCollection.PhonePersons)
            {
                if (null == PPson) continue;
                item = new ListViewItem();
                item.Text = PPson.ID.ToString().PadLeft(2, '0');
                item.SubItems.Add(PPson.Name);
                item.SubItems.Add(PPson.PhoneNumber);
                PersonNumberLV.Items.Add(item);
            }
        }
        private void AddBtn_Click(object sender, EventArgs e)
        {
            int MaxValue=GetMax();
            IDTB.Text = (MaxValue + 1).ToString().PadLeft(2, '0');
            NameTB.Text = "";
            PhoneNumberTB.Text = "";
        }
        /// <summary>
        /// 取得最大的ID值
        /// </summary>
        /// <returns></returns>
        private int GetMax()
        {
            int Max = 0;
            foreach (PhonePerson PPson in CommonCollection.PhonePersons)
            {
                if (PPson.ID > Max) Max = PPson.ID;
            }
            return Max;
        }
        /// <summary>
        /// 将当前项添加到集合中，再刷新列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateLeftBtn_Click(object sender, EventArgs e)
        {
            //将项添加到集合中再重新刷新列表
            string StrID = IDTB.Text;
            string StrName = NameTB.Text;
            string StrPhoneNumber = PhoneNumberTB.Text;
            int id;
            try {
                id = Convert.ToInt32(StrID);
            }catch(Exception)
            {MessageBox.Show("對不起,人員ID格式错误!");return;}
            //查看列表中是否存在该项
            int Index = GetPersonFromID(StrID);
            if (Index >= 0)
            {//说明已经存在
                CommonCollection.PhonePersons[Index].Name = StrName;
                CommonCollection.PhonePersons[Index].PhoneNumber = StrPhoneNumber;
                UpdatePersonLv();
                return;
            }
            PhonePerson MyPhonePerson = new PhonePerson();
            MyPhonePerson.ID = id;
            MyPhonePerson.Name = StrName;
            MyPhonePerson.PhoneNumber = StrPhoneNumber;
            CommonCollection.PhonePersons.Add(MyPhonePerson);
            UpdatePersonLv();
        }
        /// <summary>
        /// 点击所选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PersonNumberLV_Click(object sender, EventArgs e)
        {
            if (PersonNumberLV.SelectedItems.Count <= 0) return;
            string StrID = PersonNumberLV.SelectedItems[0].Text;
            string StrName = PersonNumberLV.SelectedItems[0].SubItems[1].Text;
            string StrPhoneNum = PersonNumberLV.SelectedItems[0].SubItems[2].Text;
            IDTB.Text = StrID;
            NameTB.Text = StrName;
            PhoneNumberTB.Text = StrPhoneNum;
        }
        private void DeleBtn_Click(object sender, EventArgs e)
        {
            string StrID = IDTB.Text;
            int Index = GetPersonFromID(StrID);
            if (Index < 0) return;
            CommonCollection.PhonePersons.RemoveAt(Index);
            UpdatePersonLv();
            IDTB.Text = "";
            NameTB.Text = "";
            PhoneNumberTB.Text = "";
        }
        private int GetPersonFromID(string StrID)
        {
            for (int i = 0; i < CommonCollection.PhonePersons.Count; i++)
            {
                if (CommonCollection.PhonePersons[i].ID.ToString().PadLeft(2, '0').Equals(StrID))
                {
                    return i;
                }
            }
            return -1;
        }

    }
}
