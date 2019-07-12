using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ArrayList = System.Collections.ArrayList;

namespace PersionAutoLocaSys
{
    public partial class AlarmInfoWin : Form
    {
        private Timer MyTimer = null;
        private SpeceilAlarm CurSpeceilAlarm = SpeceilAlarm.UnKnown;
        private Form1 frm = null;
        public AlarmInfoWin()
        {
            InitializeComponent();
        }
        public AlarmInfoWin(Form1 frm,SpeceilAlarm CurSpeceilAlarm)
        {
            InitializeComponent();
            this.CurSpeceilAlarm = CurSpeceilAlarm;
            this.frm = frm;
        }
        private void AlarmInfoWin_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new Size(952, 592);
            this.MinimumSize = new Size(952, 592);
            PersonAlarmListView.Items.Clear();
            switch (CurSpeceilAlarm)
            {
                case SpeceilAlarm.PersonHelp:
                    this.Text = "人員求救資訊";
                    break;
                case SpeceilAlarm.AreaControl:
                    this.Text = "區域管制資訊";
                    break;
                case SpeceilAlarm.Resid:
                    this.Text = "人員未移動";
                    break;
            }
            MyTimer = new Timer();
            MyTimer.Interval = 1000;
            MyTimer.Tick += MyTimer_Tick;
            MyTimer.Start();
        }
        //定时器触发的事件
        private void MyTimer_Tick(Object obj, EventArgs arg)
        {
            // 列表中的每一项都将对应的警告集合中的项，这样便于查找
            string strtagid, strtagname,strreferid,strrefername,strareaid;
            StringBuilder strtaginfor = null,strreferinfor = null,strareainfo = null;
            Area curarea = null;

            ListViewItem item = null;
            List<WarmInfo> tempwarms = null;
            lock (CommonCollection.warm_lock)
            {
                tempwarms = CommonCollection.WarmInfors.ToList();
            }
            for (int i = 0; i < tempwarms.Count; i++)
            {
                if (null == tempwarms[i])
                    continue;
               string strclassname = tempwarms[i].GetType().Name;
               switch (CurSpeceilAlarm)
               {
                   case SpeceilAlarm.PersonHelp:
                       {
                           if (!"PersonHelp".Equals(strclassname))
                               continue;
                           strtagid = ((PersonHelp)tempwarms[i]).TD[0].ToString("X2") + ((PersonHelp)tempwarms[i]).TD[1].ToString("X2");
                           strtagname = CommonBoxOperation.GetTagName(strtagid);
                           if (null != strtagname && !"".Equals(strtagname))
                           {
                               strtaginfor = new StringBuilder(strtagname);
                               strtaginfor.Append("(");
                               strtaginfor.Append(strtagid);
                               strtaginfor.Append(")");
                               //TagInfo = TagName + "(" + TagInfo + ")";
                           }
                           else
                           {
                               strtaginfor = new StringBuilder(strtagid);
                           }
                           break;
                       }
                   case SpeceilAlarm.AreaControl:
                       {
                           if (!"AreaAdmin".Equals(strclassname))
                               continue;
                           strtagid = ((AreaAdmin)tempwarms[i]).TD[0].ToString("X2") + ((AreaAdmin)tempwarms[i]).TD[1].ToString("X2");
                           strtagname = CommonBoxOperation.GetTagName(strtagid);
                           if (null != strtagname && !"".Equals(strtagname))
                           {
                               strtaginfor = new StringBuilder(strtagname);
                               strtaginfor.Append("(");
                               strtaginfor.Append(strtagid);
                               strtaginfor.Append(")");
                           }
                           else
                           {
                               strtaginfor = new StringBuilder(strtagid);
                           }
                           break;
                       }
                   case SpeceilAlarm.Resid:
                       {
                           if (!"PersonRes".Equals(strclassname))
                               continue;
                           strtagid = ((PersonRes)tempwarms[i]).TD[0].ToString("X2") + ((PersonRes)tempwarms[i]).TD[1].ToString("X2");
                           strtagname = CommonBoxOperation.GetTagName(strtagid);
                           if (null != strtagname && !"".Equals(strtagname))
                           {
                               strtaginfor = new StringBuilder(strtagname);
                               strtaginfor.Append("(");
                               strtaginfor.Append(strtagid);
                               strtaginfor.Append(")");
                           }
                           else
                           {
                               strtaginfor = new StringBuilder(strtagid);
                           }
                           break;
                       }
                   default:
                       break;
               }
               strreferid = tempwarms[i].RD[0].ToString("X2") + tempwarms[i].RD[1].ToString("X2");
               curarea = CommonBoxOperation.GetAreaFromRouterID(strreferid);
               strrefername = CommonBoxOperation.GetRouterName(strreferid);
               if (null != strrefername && !"".Equals(strrefername))
               {
                   strreferinfor = new StringBuilder(strrefername);
                   strreferinfor.Append("(");
                   strreferinfor.Append(strreferid);
                   strreferinfor.Append(")");
               }
               else
               {
                   strreferinfor = new StringBuilder(strreferid);
               }
               if (null != curarea)
               {
                   strareaid = curarea.ID[0].ToString("X2") + curarea.ID[1].ToString("X2");
                   if (null != curarea.Name && !"".Equals(curarea.Name))
                   {
                       strareainfo = new StringBuilder(curarea.Name);
                       strareainfo.Append("(");
                       strareainfo.Append(strareaid);
                       strareainfo.Append(")");
                   }
                   else
                   {
                       strareainfo = new StringBuilder(strareaid);
                   }
               }
               else
               {
                   strareainfo = new StringBuilder("****");
               }
               if (PersonAlarmListView.Items.ContainsKey(i + ""))
               {//没有包含这一项
                   ListViewItem[] Items = PersonAlarmListView.Items.Find(i.ToString(), false);
                   if (null != Items && Items.Length > 0)
                   {
                       Items[0].SubItems[3].Text = tempwarms[i].AlarmTime.ToString();
                       if (!tempwarms[i].isHandler)
                       {//说明没有被处理
                           Items[0].SubItems[4].Text = "****";
                           Items[0].SubItems[5].Text = "未處理";
                       }
                       else
                       {
                           Items[0].SubItems[4].Text = tempwarms[i].ClearAlarmTime.ToString();
                           Items[0].SubItems[5].Text = "處理";
                       }
                   }
               }
               else
               {
                   if (tempwarms[i].isClear)
                       continue;
                   //包含这一项
                   item = new ListViewItem();
                   item.Name = i.ToString();
                   item.Text = strtaginfor.ToString();//Tag信息
                   item.SubItems.Add(strareainfo.ToString());
                   item.SubItems.Add(strreferinfor.ToString());
                   item.SubItems.Add(tempwarms[i].AlarmTime.ToString());
                   if (!tempwarms[i].isHandler)
                   {
                       item.SubItems.Add("****");
                       item.SubItems.Add("未處理");
                   }
                   else
                   {
                       item.SubItems.Add(tempwarms[i].ClearAlarmTime.ToString());
                       item.SubItems.Add("處理");
                   }
                   PersonAlarmListView.Items.Add(item);
               }
            }
            //显示当前的总人数
            label1.Text = "報警資訊數量：" + PersonAlarmListView.Items.Count;
        }

        private void AlarmInfoWin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != MyTimer)
                MyTimer.Stop();
            if(SysParam.isTracking)
                SysParam.isTracking = false;
            this.Dispose();
        }
        private void PsAlarmAllSelecCB_CheckedChanged(object sender, EventArgs e)
        {
            if (PsAlarmAllSelecCB.Checked)
            {
                foreach (ListViewItem item in PersonAlarmListView.Items)
                    item.Checked = true;
            }
            else
            {
                foreach (ListViewItem item in PersonAlarmListView.Items)
                    item.Checked = false;
            }
        }
        /// <summary>
        /// 处理警告资讯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandlerAlarmBtn_Click(object sender, EventArgs e)
        {
            OperType curOperType = OperType.UnKnown;
            switch (CurSpeceilAlarm)
            {
                case SpeceilAlarm.PersonHelp:
                    curOperType = OperType.HandlePersonHelpAlarm;
                    break;
                case SpeceilAlarm.AreaControl:
                    curOperType = OperType.HandleAreaAdminAlarm;
                    break;
                case SpeceilAlarm.Resid:
                    curOperType = OperType.HandlePersonStopAlarm;
                    break;
            }

            if (frm.CurPerson == null)
            {
                if (MessageBox.Show("對不起,你還沒有登錄,不能處理警報訊息,請問是否需要登陸?","提醒",MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    EnterPassWin MyEnterPassWin = new EnterPassWin(frm, curOperType);
                    if (DialogResult.OK != MyEnterPassWin.ShowDialog())
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            if (PersonAlarmListView.CheckedItems.Count <= 0)
            {
                MessageBox.Show("請先選擇處理的项!");
                return;
            }

            PersonOperation curpersonoper = new PersonOperation(frm.CurPerson.ID, curOperType);
            CommonCollection.personOpers.Add(curpersonoper);


            int index = -1;
            foreach (ListViewItem item in PersonAlarmListView.CheckedItems)
            {
               string StrIndex = item.Name;
               try 
               {
                   index = Convert.ToInt32(StrIndex);
               }catch(Exception)
               {
                   index = -1;
                   continue;
               }
               if (index >= 0)
               {
                   try
                   {
                       if (!CommonCollection.WarmInfors[index].isHandler)
                       {
                           CommonCollection.WarmInfors[index].isHandler = true;
                           CommonCollection.WarmInfors[index].ClearAlarmTime = DateTime.Now;
                       }
                   }catch(Exception)
                   {

                   }
               }
            }
        }
        /// <summary>
        /// 清除警告资讯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearAlarmTagsBtn_Click(object sender, EventArgs e)
        {
            OperType curOperType = OperType.UnKnown;
            switch (CurSpeceilAlarm)
            {
                case SpeceilAlarm.PersonHelp:
                    curOperType = OperType.DelePersonHelpAlarm;
                    break;
                case SpeceilAlarm.AreaControl:
                    curOperType = OperType.DeleAreaAdminAlarm;
                    break;
                case SpeceilAlarm.Resid:
                    curOperType = OperType.DelePersonStopAlarm;
                    break;
            }
            if(frm.CurPerson == null)
            {
                if (MessageBox.Show("對不起,你還沒有登錄,不能清除已經處理的警告訊息,請問是否需要登陸?") == System.Windows.Forms.DialogResult.OK)
                {
                    EnterPassWin MyEnterPassWin = new EnterPassWin(frm,curOperType);
                    if (DialogResult.OK != MyEnterPassWin.ShowDialog())
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            if (PersonAlarmListView.CheckedItems.Count <= 0)
            {
                MessageBox.Show("請先選擇處理的项!");
                return;
            }
            PersonOperation curpersonoper = new PersonOperation(frm.CurPerson.ID, curOperType);
            CommonCollection.personOpers.Add(curpersonoper);
            string StrIndex = "";
            int index = -1;
            foreach (ListViewItem item in PersonAlarmListView.CheckedItems)
            {
                StrIndex = item.Name;
                try 
                {
                    index = Convert.ToInt32(StrIndex);
                }catch(Exception)
                {
                    index = -1;
                    continue;
                }
                if (index >= 0)
                {
                    try
                    {
                        if (CommonCollection.WarmInfors[index].isHandler)
                        {
                            CommonCollection.WarmInfors[index].isClear = true;
                        }
                    }catch(Exception)
                    {

                    }
                }
            }
            string StrDir = FileOperation.WarmMsg;
            string StrFileName = StrDir + "\\" + FileOperation.WarmName;
            List<WarmInfo> listbox = null;
            if (File.Exists(StrFileName))
            {
               listbox = FileOperation.GetWarmData(StrFileName);
            }
            //清除掉警报讯息集合中需要清理的项
            for (int i = 0; i < CommonCollection.WarmInfors.Count;i++)
            {
                if (CommonCollection.WarmInfors[i].isClear)
                {
                    //需要先存起来这样方便警告讯息的查找
                    if (null != listbox)
                    {
                        listbox.Add(CommonCollection.WarmInfors[i]);
                    }
                    CommonCollection.WarmInfors.Remove(CommonCollection.WarmInfors[i]);
                    i--;
                }
            }
            //完成清理警告讯息后，再将警报讯息保存起来，方便查看
            if (null != listbox)
            {
                FileOperation.SaveWarmData(listbox, StrFileName);
            }
            //清除掉列表项，这样就能保证每次序列号不会出错
            PersonAlarmListView.Items.Clear();
            DrawIMG.DrawMainCenter(frm.MainCenter_G);
            frm.MainCenter_Panel_Paint(null, null);
        }

        private void PersonAlarmListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //点击的过程中发现列表中的项全部被勾选,
            if (PersonAlarmListView.CheckedItems.Count >= CommonBoxOperation.GetAlarmTagsNum(CurSpeceilAlarm))
            {
                PsAlarmAllSelecCB.Checked = true;
            }
            if (PersonAlarmListView.CheckedItems.Count <= 0)
            {
                PsAlarmAllSelecCB.Checked = false;
            }
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            string StrSearchTB = SearchTB.Text;
            if ("".Equals(StrSearchTB) || null == StrSearchTB)
            {
                MessageBox.Show("搜索时搜索框中的内容不能为空!");
                return;
            }
            SysParam.SearchListViewStr(PersonAlarmListView, StrSearchTB);
        }
        //文本改变时
        private void SearchTB_TextChanged(object sender, EventArgs e)
        {
            SysParam.ClearListViewWhiteItem(PersonAlarmListView);
        }
    }
}
