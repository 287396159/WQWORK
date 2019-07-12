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
    public partial class OterAlarmWin : Form
    {
        private Timer MyTimer = null;
        private Form1 frm = null;
        public OterAlarmWin()
        {
            InitializeComponent();
        }
        public OterAlarmWin(Form1 frm)
        {
            InitializeComponent();
            this.frm = frm;
        }
        private void OterAlarmWin_Load(object sender, EventArgs e)
        {
            ReferDisListView.Items.Clear();
            BattLowListView.Items.Clear();
            TagDisListView.Items.Clear();
            NodeDisListView.Items.Clear();
            if (CommonBoxOperation.GetAlarmTagsNum(SpeceilAlarm.BatteryLow) > 0)
            {
                AlarmControlTab.SelectedIndex = 0;
            }
            else if (CommonBoxOperation.GetAlarmTagsNum(SpeceilAlarm.TagDis) > 0)
            {
                AlarmControlTab.SelectedIndex = 1;
            }
            else if (CommonBoxOperation.GetAlarmTagsNum(SpeceilAlarm.ReferDis) > 0)
            {
                AlarmControlTab.SelectedIndex = 2;
            }
            else if (CommonBoxOperation.GetAlarmTagsNum(SpeceilAlarm.NodeDis) > 0)
            {
                AlarmControlTab.SelectedIndex = 3;
            }
            MyTimer = new Timer();
            MyTimer.Interval = 1000;
            MyTimer.Tick += MyTimer_Tick;
            MyTimer.Start();
        }
        /// <summary>
        /// 定时器刷新函数，每隔1s刷新一次列表
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        private void MyTimer_Tick(Object obj,EventArgs args)
        {
            #region 获取显示视图ListView
            ListView SelectView = null;
            switch (AlarmControlTab.SelectedIndex)
            {
                case 0:SelectView = BattLowListView;break;
                case 1:SelectView = TagDisListView;break;
                case 2:SelectView = ReferDisListView;break;
                case 3:SelectView = NodeDisListView;break;
            }
            #endregion
            //低电量异常
            Area CurArea = null;
            string strtagid, strtagname, strreferid, strrefername,strnodeid,strnodename,strareaid;
            StringBuilder strtaginfor = null, strreferinfor = null, strnodeinfor = null, strareainfor = null ;
            List<WarmInfo> tempwarms = CommonCollection.WarmInfors.ToList();
            for (int i = 0; i < tempwarms.Count;i++)
            {
                if (null == tempwarms[i])
                {
                    continue;
                }
                string strclassname = tempwarms[i].GetType().Name;
                #region 获取一些显示讯息
                switch (AlarmControlTab.SelectedIndex)
                {
                    case 0:
                        {
                            if (!"BattLow".Equals(strclassname))
                            {
                                continue;
                            }
                            strtagid = ((BattLow)tempwarms[i]).TD[0].ToString("X2") + ((BattLow)tempwarms[i]).TD[1].ToString("X2");
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
                            strreferid = tempwarms[i].RD[0].ToString("X2") + tempwarms[i].RD[1].ToString("X2");
                            CurArea = CommonBoxOperation.GetAreaFromRouterID(strreferid);
                            if (null != CurArea)
                            {
                                strareaid = CurArea.ID[0].ToString("X2") + CurArea.ID[1].ToString("X2");
                                if (null == CurArea.Name || "".Equals(CurArea.Name))
                                {
                                    strareainfor = new StringBuilder(strareaid);
                                }
                                else
                                {
                                    strareainfor = new StringBuilder(CurArea.Name);
                                    strareainfor.Append("(");
                                    strareainfor.Append(strareaid);
                                    strareainfor.Append(")");
                                }
                            }
                            else
                            {
                                strareainfor = new StringBuilder("****");
                            }
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
                            break;
                        }
                    case 1:
                        {
                            if (!"TagDis".Equals(strclassname))
                            {
                                continue;
                            }
                            strtagid = ((TagDis)tempwarms[i]).TD[0].ToString("X2") + ((TagDis)tempwarms[i]).TD[1].ToString("X2");
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
                            strreferid = tempwarms[i].RD[0].ToString("X2") + tempwarms[i].RD[1].ToString("X2");
                            CurArea = CommonBoxOperation.GetAreaFromRouterID(strreferid);
                            if (null != CurArea)
                            {
                                strareaid = CurArea.ID[0].ToString("X2") + CurArea.ID[1].ToString("X2");
                                if (null == CurArea.Name || "".Equals(CurArea.Name))
                                {
                                    strareainfor = new StringBuilder(strareaid);
                                }
                                else
                                {
                                    strareainfor = new StringBuilder(CurArea.Name);
                                    strareainfor.Append("(");
                                    strareainfor.Append(strareaid);
                                    strareainfor.Append(")");
                                }
                            }
                            else
                            {
                                strareainfor = new StringBuilder("****");
                            }
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
                            break;
                        }
                    case 2:
                        {
                            if (!"ReferDis".Equals(strclassname))
                            {
                                continue;
                            }
                            strreferid = tempwarms[i].RD[0].ToString("X2") + tempwarms[i].RD[1].ToString("X2");
                            CurArea = CommonBoxOperation.GetAreaFromRouterID(strreferid);
                            if (null != CurArea)
                            {
                                strareaid = CurArea.ID[0].ToString("X2") + CurArea.ID[1].ToString("X2");
                                if (null == CurArea.Name || "".Equals(CurArea.Name))
                                {
                                    strareainfor = new StringBuilder(strareaid);
                                }
                                else
                                {
                                    strareainfor = new StringBuilder(CurArea.Name);
                                    strareainfor.Append("(");
                                    strareainfor.Append(strareaid);
                                    strareainfor.Append(")");
                                }
                            }
                            else
                            {
                                strareainfor = new StringBuilder("****");
                            }

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
                            break;
                        }
                    case 3:
                        {
                            if (!"NodeDis".Equals(strclassname))
                            {
                                continue;
                            }
                            strnodeid = tempwarms[i].RD[0].ToString("X2") + tempwarms[i].RD[1].ToString("X2");
                            CurArea = CommonBoxOperation.GetAreaFromNodeID(strnodeid);
                            if (null != CurArea)
                            {
                                strareaid = CurArea.ID[0].ToString("X2") + CurArea.ID[1].ToString("X2");
                                if (null == CurArea.Name || "".Equals(CurArea.Name))
                                {
                                    strareainfor = new StringBuilder(strareaid);
                                }
                                else
                                {
                                    strareainfor = new StringBuilder(CurArea.Name);
                                    strareainfor.Append("(");
                                    strareainfor.Append(strareaid);
                                    strareainfor.Append(")");
                                }
                            }
                            else
                            {
                                strareainfor = new StringBuilder("****");
                            }
                            strnodename = CommonBoxOperation.GetNodeName(strnodeid);
                            if (null != strnodename && !"".Equals(strnodename))
                            {
                                strnodeinfor = new StringBuilder(strnodename);
                                strnodeinfor.Append("(");
                                strnodeinfor.Append(strnodeid);
                                strnodeinfor.Append(")");
                            }
                            else
                            {
                                strnodeinfor = new StringBuilder(strnodeid);
                            }
                            break;
                        }
                    default:
                        continue;
                }
                #endregion
                ListViewItem[] items = null;
                switch (AlarmControlTab.SelectedIndex)
                {
                    case 0:
                        {
                            #region 显示低电量警报
                            if (BattLowListView.Items.ContainsKey(i.ToString()))
                            {
                                items = BattLowListView.Items.Find(i.ToString(), false);
                                if (null != items && items.Length > 0)
                                {

                                    if (tempwarms[i].isClear)
                                    {
                                        BattLowListView.Items.Remove(items[0]);
                                    }
                                    items[0].SubItems[4].Text = tempwarms[i].AlarmTime.ToString();

                                    if (!tempwarms[i].isHandler)
                                    {
                                        items[0].SubItems[5].Text = "****";
                                    }
                                    else
                                    {
                                        items[0].SubItems[5].Text = tempwarms[i].ClearAlarmTime.ToString();
                                    }
                                    if (tempwarms[i].isHandler)
                                    {
                                        items[0].SubItems[6].Text = "處理";
                                    }
                                    else
                                    {
                                        items[0].SubItems[6].Text = "未處理";
                                    }

                                }
                            }
                            else
                            {
                                if (tempwarms[i].isClear)
                                    continue;
                                ListViewItem item = new ListViewItem();
                                item.Name = i.ToString();
                                item.Text = strtaginfor.ToString();
                                item.SubItems.Add(strareainfor.ToString());
                                item.SubItems.Add(strreferinfor.ToString());
                                item.SubItems.Add(((BattLow)tempwarms[i]).Batt.ToString() + "%");
                                item.SubItems.Add(tempwarms[i].AlarmTime.ToString());
                                if (!tempwarms[i].isHandler)
                                {
                                    item.SubItems.Add("****");
                                }
                                else
                                {
                                    item.SubItems.Add(tempwarms[i].ClearAlarmTime.ToString());
                                }
                                if (tempwarms[i].isHandler)
                                {
                                    item.SubItems.Add("處理");
                                }
                                else
                                {
                                    item.SubItems.Add("未處理");
                                }
                                BattLowListView.Items.Add(item);
                            }
                            
                            #endregion
                            break;
                        }
                    case 1:
                        {
                            #region 显示Tag断开警报
                            if (TagDisListView.Items.ContainsKey(i.ToString()))
                            {
                                items = TagDisListView.Items.Find(i.ToString(), false);
                                if (null != items && items.Length > 0)
                                {

                                    if (tempwarms[i].isClear)
                                    {
                                        TagDisListView.Items.Remove(items[0]);
                                    }
                                    items[0].SubItems[4].Text = tempwarms[i].AlarmTime.ToString();

                                    if (!tempwarms[i].isHandler)
                                    {
                                        items[0].SubItems[5].Text = "****";
                                    }
                                    else
                                    {
                                        items[0].SubItems[5].Text = tempwarms[i].ClearAlarmTime.ToString();
                                    }
                                    if (tempwarms[i].isHandler)
                                    {
                                        items[0].SubItems[6].Text = "處理";
                                    }
                                    else
                                    {
                                        items[0].SubItems[6].Text = "未處理";
                                    }
                                }
                            }
                            else
                            {
                                if (tempwarms[i].isClear) continue;
                                ListViewItem item = new ListViewItem();
                                item.Name = i.ToString();
                                item.Text = strtaginfor.ToString();
                                item.SubItems.Add(strareainfor.ToString());
                                item.SubItems.Add(strreferinfor.ToString());
                                item.SubItems.Add((((TagDis)tempwarms[i]).SleepTime / 10).ToString() + " s");
                                item.SubItems.Add(tempwarms[i].AlarmTime.ToString());
                                if (!tempwarms[i].isHandler)
                                {
                                    item.SubItems.Add("****");
                                }
                                else
                                {
                                    item.SubItems.Add(tempwarms[i].ClearAlarmTime.ToString());
                                }
                                if (tempwarms[i].isHandler)
                                {
                                    item.SubItems.Add("處理");
                                }
                                else
                                {
                                    item.SubItems.Add("未處理");
                                }
                                TagDisListView.Items.Add(item);
                            }
                            
                            #endregion
                            break;
                        }
                    case 2:
                        {
                            #region 显示Refer设备断开警报
                            if (ReferDisListView.Items.ContainsKey(i.ToString()))
                            {
                                items = ReferDisListView.Items.Find(i.ToString(), false);
                                if (null != items && items.Length > 0)
                                {
                                    if (tempwarms[i].isClear)
                                        ReferDisListView.Items.Remove(items[0]);
                                    items[0].SubItems[3].Text = (((ReferDis)tempwarms[i]).SleepTime <= 0) ? "***" : ((ReferDis)tempwarms[i]).SleepTime.ToString() + " s";
                                    items[0].SubItems[3].Text = tempwarms[i].AlarmTime.ToString();

                                    if (!tempwarms[i].isHandler)
                                    {
                                        items[0].SubItems[4].Text = "****";
                                    }
                                    else
                                    {
                                        items[0].SubItems[4].Text = tempwarms[i].ClearAlarmTime.ToString();
                                    }

                                    if (tempwarms[i].isHandler)
                                    {
                                        items[0].SubItems[5].Text = "處理";
                                    }
                                    else
                                    {
                                        items[0].SubItems[5].Text = "未處理";
                                    }
                                }
                            }
                            else
                            {
                                if (tempwarms[i].isClear) continue;
                                ListViewItem item = new ListViewItem();
                                item.Name = i.ToString();
                                item.Text = strreferinfor.ToString();
                                item.SubItems.Add(strareainfor.ToString());
                                item.SubItems.Add((((ReferDis)tempwarms[i]).SleepTime <= 0) ? "***" : ((ReferDis)tempwarms[i]).SleepTime.ToString() + " s");
                                item.SubItems.Add(tempwarms[i].AlarmTime.ToString());
                                if (!tempwarms[i].isHandler)
                                {
                                    item.SubItems.Add("****");
                                }
                                else
                                {
                                    item.SubItems.Add(tempwarms[i].ClearAlarmTime.ToString());
                                }
                                if (tempwarms[i].isHandler)
                                {
                                    item.SubItems.Add("處理");
                                }
                                else
                                {
                                    item.SubItems.Add("未處理");
                                }
                                ReferDisListView.Items.Add(item);
                            }
                            
                            #endregion
                            break;
                        }
                    case 3:
                        {
                            #region 显示Node设置断开警报
                            if (NodeDisListView.Items.ContainsKey(i.ToString()))
                            {
                                items = NodeDisListView.Items.Find(i.ToString(), false);
                                if (null != items && items.Length > 0)
                                {
                                    if (tempwarms[i].isClear)
                                    {
                                        NodeDisListView.Items.Remove(items[0]);
                                    }
                                    items[0].SubItems[2].Text = (((NodeDis)tempwarms[i]).SleepTime <= 0) ? "***" : ((NodeDis)tempwarms[i]).SleepTime.ToString() + "s";
                                    items[0].SubItems[3].Text = tempwarms[i].AlarmTime.ToString();

                                    if (!tempwarms[i].isHandler)
                                    {
                                        items[0].SubItems[4].Text = "****";
                                    }
                                    else
                                    {
                                        items[0].SubItems[4].Text = tempwarms[i].ClearAlarmTime.ToString();
                                    }
                                    if (tempwarms[i].isHandler)
                                    {
                                        items[0].SubItems[5].Text = "處理";
                                    }
                                    else
                                    {
                                        items[0].SubItems[5].Text = "未處理";
                                    }
                                }
                            }
                            else
                            {
                                if (tempwarms[i].isClear) 
                                    continue;
                                ListViewItem item = new ListViewItem();
                                item.Name = i.ToString();
                                item.Text = strnodeinfor.ToString();
                                item.SubItems.Add(strareainfor.ToString());
                                item.SubItems.Add((((NodeDis)tempwarms[i]).SleepTime <= 0) ? "***" : ((NodeDis)tempwarms[i]).SleepTime.ToString() + "s");
                                item.SubItems.Add(tempwarms[i].AlarmTime.ToString());
                                if (!tempwarms[i].isHandler)
                                {
                                    item.SubItems.Add("****");
                                }
                                else
                                {
                                    item.SubItems.Add(tempwarms[i].ClearAlarmTime.ToString());
                                }
                                if (tempwarms[i].isHandler)
                                {
                                    item.SubItems.Add("處理");
                                }
                                else
                                {
                                    item.SubItems.Add("未處理");
                                }
                                NodeDisListView.Items.Add(item);
                            }
                            #endregion
                            break;
                        }
                }
            }

            switch (AlarmControlTab.SelectedIndex)
            {
                case 0:
                    label1.Text = "報警資訊數量：" + BattLowListView.Items.Count;
                    break;
                case 1:
                    label1.Text = "報警資訊數量：" + TagDisListView.Items.Count;
                    break;
                case 2:
                    label1.Text = "報警資訊數量：" + ReferDisListView.Items.Count;
                    break;
                case 3:
                    label1.Text = "報警資訊數量：" + NodeDisListView.Items.Count;
                    break;
            }

        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PsAlarmAllSelecCB_CheckedChanged(object sender, EventArgs e)
        {
            if (PsAlarmAllSelecCB.Checked)
            {
                if (AlarmControlTab.SelectedIndex == 0)
                {
                    foreach (ListViewItem item in BattLowListView.Items)
                    {
                        item.Checked = true;
                    }
                }
                else if (AlarmControlTab.SelectedIndex == 1)
                {
                    foreach (ListViewItem item in TagDisListView.Items)
                    {
                        item.Checked = true;
                    }
                }
                else if (AlarmControlTab.SelectedIndex == 2)
                {
                    foreach (ListViewItem item in ReferDisListView.Items)
                    {
                        item.Checked = true;
                    }
                }
                else if (AlarmControlTab.SelectedIndex == 3)
                {
                    foreach (ListViewItem item in NodeDisListView.Items)
                    {
                        item.Checked = true;
                    }
                }
            }
            else
            {
                if (AlarmControlTab.SelectedIndex == 0)
                {
                    foreach (ListViewItem item in BattLowListView.Items)
                    {
                        item.Checked = false;
                    }
                }
                else if (AlarmControlTab.SelectedIndex == 1)
                {
                    foreach (ListViewItem item in TagDisListView.Items)
                    {
                        item.Checked = false;
                    }
                }
                else if (AlarmControlTab.SelectedIndex == 2)
                {
                    foreach (ListViewItem item in ReferDisListView.Items)
                    {
                        item.Checked = false;
                    }
                }
                else if (AlarmControlTab.SelectedIndex == 3)
                {
                    foreach (ListViewItem item in NodeDisListView.Items)
                    {
                        item.Checked = false;
                    }
                }
            }

        }
        /// <summary>
        /// 切换TagPage时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlarmControlTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            PsAlarmAllSelecCB.Checked = false;
            SearchTB.Text = "";
            int count = -1,SumCount = -1;
            switch (AlarmControlTab.SelectedIndex)
            {
                case 0:
                    count = BattLowListView.CheckedItems.Count;
                    SumCount = BattLowListView.Items.Count;
                    if (count >= SumCount)
                    { 
                        //说明全选了
                        PsAlarmAllSelecCB.Checked = true;
                    }
                    if (count <= 0)
                    {
                        PsAlarmAllSelecCB.Checked = false;
                    }
                    break;
                case 1:
                    count = TagDisListView.CheckedItems.Count;
                    SumCount = TagDisListView.Items.Count;
                    if (count >= SumCount)
                    { 
                        //说明全选了
                        PsAlarmAllSelecCB.Checked = true;
                    }
                    if (count <= 0)
                    {
                        PsAlarmAllSelecCB.Checked = false;
                    }
                    break;
                case 2:
                    count = ReferDisListView.CheckedItems.Count;
                    SumCount = ReferDisListView.Items.Count;
                    if (count >= SumCount)
                    { 
                        //说明全选了
                        PsAlarmAllSelecCB.Checked = true;
                    }
                    if (count <= 0)
                    {
                        PsAlarmAllSelecCB.Checked = false;
                    }
                    break;
                case 3:
                    count = NodeDisListView.CheckedItems.Count;
                    SumCount = NodeDisListView.Items.Count;
                    if (count >= SumCount)
                    { 
                        //说明全选了
                        PsAlarmAllSelecCB.Checked = true;
                    }
                    if (count <= 0)
                    {
                        PsAlarmAllSelecCB.Checked = false;
                    }
                    break;
            }
        }


        private void PersonAlarmListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            int count = BattLowListView.CheckedItems.Count;
            int SumCount = BattLowListView.Items.Count;
            if (count >= SumCount)
            {
                //说明全选了
                PsAlarmAllSelecCB.Checked = true;
            }
            if (count <= 0)
            {
                PsAlarmAllSelecCB.Checked = false;
            }
        }

        private void HandlerAlarmBtn_Click(object sender, EventArgs e)
        {
             if (frm.CurPerson == null)
             {
                if (MessageBox.Show("對不起,你還沒有登錄,不能處理警告訊息,請問是否需要登陸?", "提醒", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    EnterPassWin MyEnterPassWin = new EnterPassWin(frm, OperType.HandleBattLackAlarm);
                    if (DialogResult.OK != MyEnterPassWin.ShowDialog())
                    {
                        return;
                    }
                }else
                {
                   return;
                }
             }
             int index = -1;
             switch (AlarmControlTab.SelectedIndex)
             {
               case 0:
                  if (BattLowListView.CheckedItems.Count <= 0)
                  {
                      MessageBox.Show("請先選擇處理的项!");
                      return;
                  }
                 
                  PersonOperation curpersonoper = new PersonOperation(frm.CurPerson.ID,OperType.HandleBattLackAlarm);
                  CommonCollection.personOpers.Add(curpersonoper);
                  foreach (ListViewItem item in BattLowListView.CheckedItems)
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

                              if (!"BattLow".Equals(CommonCollection.WarmInfors[index].GetType().Name))
                              {
                                  continue;
                              }
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
                  break;
               case 1:
                  if (TagDisListView.CheckedItems.Count <= 0) 
                  {
                      MessageBox.Show("請先選擇處理的项!");
                      return;
                  }
                  
                  PersonOperation curpersonoper1 = new PersonOperation(frm.CurPerson.ID,OperType.HandleTagExcepAlarm);
                  CommonCollection.personOpers.Add(curpersonoper1);
                  
                  foreach (ListViewItem item in TagDisListView.CheckedItems)
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
                              if (!"TagDis".Equals(CommonCollection.WarmInfors[index].GetType().Name))
                              {
                                  continue;
                              }
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
                  break;
               case 2:
                  if (ReferDisListView.CheckedItems.Count <= 0)
                  { 
                      MessageBox.Show("請先選擇處理的项!");
                      return;
                  }
                  
                  PersonOperation curpersonoper2 = new PersonOperation(frm.CurPerson.ID,OperType.HandleReferExcepAlarm);
                  CommonCollection.personOpers.Add(curpersonoper2);
                  
                  foreach (ListViewItem item in ReferDisListView.CheckedItems)
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
                              if (!"ReferDis".Equals(CommonCollection.WarmInfors[index].GetType().Name))
                              {
                                  continue;
                              }
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
                  break;
               case 3:
                  if (NodeDisListView.CheckedItems.Count <= 0)
                  { 
                      MessageBox.Show("請先選擇處理的项!");
                      return;
                  }
                  
                  PersonOperation curpersonoper3 = new PersonOperation(frm.CurPerson.ID,OperType.HandleNodeExcepAlarm);
                  CommonCollection.personOpers.Add(curpersonoper3);
                  
                  foreach (ListViewItem item in NodeDisListView.CheckedItems)
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
                              if (!"NodeDis".Equals(CommonCollection.WarmInfors[index].GetType().Name))
                              {
                                 continue;
                              }
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
                  break;
             }
        }
        private void ClearAlarmTagsBtn_Click(object sender, EventArgs e)
        {
            if(frm.CurPerson == null)
            {
                if (MessageBox.Show("對不起,你還沒有登錄,不能清除警報訊息,請問是否需要登陸?", "提醒", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    EnterPassWin MyEnterPassWin = new EnterPassWin(frm, OperType.DeleBattLackAlarm);
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
            int index = -1;
            switch (AlarmControlTab.SelectedIndex)
            {
                case 0:
                    {
                        if (BattLowListView.CheckedItems.Count <= 0)
                        {
                            MessageBox.Show("請先選擇清除的项!");
                            return;
                        }

                        PersonOperation curpersonoper = new PersonOperation(frm.CurPerson.ID, OperType.DeleBattLackAlarm);
                        CommonCollection.personOpers.Add(curpersonoper);

                        foreach (ListViewItem item in BattLowListView.CheckedItems)
                        {
                            string StrIndex = item.Name;
                            try
                            {
                                index = Convert.ToInt32(StrIndex);
                            }
                            catch (Exception)
                            {
                                index = -1; continue;
                            }
                            if (index >= 0 && index < CommonCollection.WarmInfors.Count)
                            {
                                try
                                {
                                    if (CommonCollection.WarmInfors[index].isHandler)
                                    {
                                        CommonCollection.WarmInfors[index].isClear = true;
                                    }
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        if (TagDisListView.CheckedItems.Count <= 0)
                        {
                            MessageBox.Show("請先選擇清除的项!");
                            return;
                        }
                        PersonOperation curpersonoper1 = new PersonOperation(frm.CurPerson.ID, OperType.DeleTagExcepAlarm);
                        CommonCollection.personOpers.Add(curpersonoper1);

                        foreach (ListViewItem item in TagDisListView.CheckedItems)
                        {
                            string StrIndex = item.Name;
                            try
                            {
                                index = Convert.ToInt32(StrIndex);
                            }
                            catch (Exception)
                            {
                                index = -1; continue;
                            }
                            if (index >= 0 && index < CommonCollection.WarmInfors.Count)
                            {
                                try
                                {
                                    if (CommonCollection.WarmInfors[index].isHandler)
                                    {
                                        CommonCollection.WarmInfors[index].isClear = true;
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        if (ReferDisListView.CheckedItems.Count <= 0)
                        {
                            MessageBox.Show("請先選擇清除的项!");
                            return;
                        }
                        PersonOperation curpersonoper2 = new PersonOperation(frm.CurPerson.ID, OperType.DeleReferExcepAlarm);
                        CommonCollection.personOpers.Add(curpersonoper2);

                        foreach (ListViewItem item in ReferDisListView.CheckedItems)
                        {
                            string StrIndex = item.Name;
                            try
                            {
                                index = Convert.ToInt32(StrIndex);
                            }
                            catch (Exception)
                            { index = -1; continue; }
                            if (index >= 0 && index < CommonCollection.WarmInfors.Count)
                            {
                                try
                                {
                                    if (CommonCollection.WarmInfors[index].isHandler)
                                    {
                                        CommonCollection.WarmInfors[index].isClear = true;
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        if (NodeDisListView.CheckedItems.Count <= 0)
                        {
                            MessageBox.Show("請先選擇清除的项!");
                            return;
                        }

                        PersonOperation curpersonoper3 = new PersonOperation(frm.CurPerson.ID, OperType.DeleNodeExcepAlarm);
                        CommonCollection.personOpers.Add(curpersonoper3);
                        foreach (ListViewItem item in NodeDisListView.CheckedItems)
                        {
                            string StrIndex = item.Name;
                            try
                            {
                                index = Convert.ToInt32(StrIndex);
                            }
                            catch (Exception)
                            { index = -1; continue; }
                            if (index >= 0 && index < CommonCollection.WarmInfors.Count)
                            {
                                try
                                {
                                    if (CommonCollection.WarmInfors[index].isHandler)
                                    {
                                        CommonCollection.WarmInfors[index].isClear = true;
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                        break;
                    }
            }

            string StrDir = FileOperation.WarmMsg;
            string StrFileName = StrDir + "\\" + FileOperation.WarmName;
            List<WarmInfo> listbox = null;
            if (File.Exists(StrFileName))
            {
                listbox = FileOperation.GetWarmData(StrFileName);
            }
            //删除掉已经清除的项
            for (int i = 0; i < CommonCollection.WarmInfors.Count; i++)
            {
                if (CommonCollection.WarmInfors[i].isClear)
                {
                    //需要先存起来这样方便警告讯息的查找
                    if (null != listbox)
                    {
                        listbox.Add(CommonCollection.WarmInfors[i]);
                    }
                    CommonCollection.WarmInfors.Remove(CommonCollection.WarmInfors[i]);
                    //清除掉报警间隔时间
                    i--;
                }
            }
            //完成清理警告讯息后，再将警报讯息保存起来，方便查看
            if (null != listbox)
            {
                FileOperation.SaveWarmData(listbox, StrFileName);
            }
            //同时清理列表项
            BattLowListView.Items.Clear();
            TagDisListView.Items.Clear();
            ReferDisListView.Items.Clear();
            NodeDisListView.Items.Clear();
            DrawIMG.DrawMainCenter(frm.MainCenter_G);
            frm.MainCenter_Panel_Paint(null, null);
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            string StrSearchTxt = SearchTB.Text;
            if ("".Equals(StrSearchTxt))
            {
                MessageBox.Show("搜索时搜索框中的内容不能为空!");
                return;
            }
            if (AlarmControlTab.SelectedIndex == 0)
            {
                SysParam.SearchListViewStr(BattLowListView, StrSearchTxt);
            }
            else if (AlarmControlTab.SelectedIndex == 1)
            {
                SysParam.SearchListViewStr(TagDisListView, StrSearchTxt);
            }
            else if (AlarmControlTab.SelectedIndex == 2)
            {
                SysParam.SearchListViewStr(ReferDisListView, StrSearchTxt);
            }
            else if (AlarmControlTab.SelectedIndex == 3)
            {
                SysParam.SearchListViewStr(NodeDisListView, StrSearchTxt);
            }
        }

        private void SearchTB_TextChanged(object sender, EventArgs e)
        {
            SysParam.ClearListViewWhiteItem(BattLowListView);
            SysParam.ClearListViewWhiteItem(TagDisListView);
            SysParam.ClearListViewWhiteItem(ReferDisListView);
            SysParam.ClearListViewWhiteItem(NodeDisListView);
            /*
            if (AlarmControlTab.SelectedIndex == 0)
            {
                SysParam.ClearListViewWhiteItem(BattLowListView);
            }
            else if (AlarmControlTab.SelectedIndex == 1)
            {
                SysParam.ClearListViewWhiteItem(TagDisListView);
            }
            else if (AlarmControlTab.SelectedIndex == 2)
            {
                SysParam.ClearListViewWhiteItem(ReferDisListView);
            }
            else if (AlarmControlTab.SelectedIndex == 3)
            {
                SysParam.ClearListViewWhiteItem(NodeDisListView);
            }*/
        }

        private void RouterExcepionListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            int count = ReferDisListView.CheckedItems.Count;
            int SumCount = ReferDisListView.Items.Count;
            if (count >= SumCount)
            {
                //说明全选了
                PsAlarmAllSelecCB.Checked = true;
            }
            if (count <= 0)
            {
                PsAlarmAllSelecCB.Checked = false;
            }
        }

        private void OterAlarmWin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != MyTimer)
                MyTimer.Stop();
            this.Dispose();
        }
    }
}
