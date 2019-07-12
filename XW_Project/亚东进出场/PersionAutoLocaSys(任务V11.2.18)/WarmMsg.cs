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
    public partial class WarmMsg : Form
    {
        private SpeceilAlarm CurWarmType = SpeceilAlarm.UnKnown;
        public WarmMsg()
        {
            InitializeComponent();
        }
        private void WarmMsg_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new Size(721, 621);
            this.MinimumSize = new Size(721, 621);
            AlarmTypeCB.SelectedIndex = 0;
            int i=0;
            for (i = 0; i < 24; i++) { sHourCb.Items.Add(i.ToString()); eHourCB.Items.Add(i.ToString());};
            for (i = 0; i < 60; i++) { sMinitueCb.Items.Add(i.ToString()); eMinitueCB.Items.Add(i.ToString());};
            sHourCb.SelectedIndex = 0;
            eHourCB.SelectedIndex = eHourCB.Items.Count - 1;
            sMinitueCb.SelectedIndex = 0;
            eMinitueCB.SelectedIndex = eMinitueCB.Items.Count - 1;
            AlarmMsgOutBtn.Enabled = false;
        }
        /// <summary>
        /// 选择不同警报时会显示不同的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlarmTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (AlarmTypeCB.SelectedIndex)
            {   case 0: CurWarmType = SpeceilAlarm.UnKnown;break;
                case 1: CurWarmType = SpeceilAlarm.BatteryLow;break;
                case 2: CurWarmType = SpeceilAlarm.PersonHelp;break;
                case 3: CurWarmType = SpeceilAlarm.AreaControl;break;
                case 4: CurWarmType = SpeceilAlarm.Resid;break;
                case 5: CurWarmType = SpeceilAlarm.TagDis;break;
                case 6: CurWarmType = SpeceilAlarm.ReferDis;break;
                case 7: CurWarmType = SpeceilAlarm.NodeDis;break;
                default: CurWarmType = SpeceilAlarm.UnKnown;break;
            }
        }
        private void StartBtn_Click(object sender, EventArgs e)
        {
            //清除列表项
            WarmListView.Items.Clear();
            DateTime SDTP = StartDTPicker.Value; 
            DateTime EDTP = EndDTPicker.Value;
            int syear, smonth, sday, shour, sminute, eyear, emonth, eday, ehour, eminute;
            syear = SDTP.Year; smonth = SDTP.Month; sday = SDTP.Day;
            eyear = EDTP.Year; emonth = EDTP.Month; eday = EDTP.Day;
            string StrSHour = sHourCb.Text;
            string StrSMinute = sMinitueCb.Text;
            string StrEHour = eHourCB.Text;
            string StrEMinute = eMinitueCB.Text;
            if ("".Equals(StrSHour) || "".Equals(StrSMinute) || "".Equals(StrEHour) || "".Equals(StrEMinute))
            {
                MessageBox.Show("開始時間與結束時間的小時與分針不能為空!");
                return;
            }
            try
            {
                shour = Convert.ToInt32(StrSHour);
                sminute = Convert.ToInt32(StrSMinute);
                ehour = Convert.ToInt32(StrEHour);
                eminute = Convert.ToInt32(StrEMinute);
            }
            catch (Exception)
            {
                MessageBox.Show("開始時間與結束時間的小時與分針格式有誤!");
                return;
            }
            DateTime SDT, EDT;
            try
            {
                SDT = new DateTime(syear, smonth, sday, shour, sminute, 0);
                EDT = new DateTime(eyear, emonth, eday, ehour, eminute, 59);
            }catch(Exception)
            {
                MessageBox.Show("開始時間或結束時間格式有誤!");
                return;
            }
            if (DateTime.Compare(EDT, SDT) <= 0)
            {
                MessageBox.Show("結束時間要大於開始時間!");
                return;
            }
            //时间参数没有问题后，开始从原始的警报文件中导出数据
            string StrDir = FileOperation.WarmMsg;
            string StrFileName = StrDir + "\\" + FileOperation.WarmName;
            //得到源文件中的数据信息

            if (null == StrFileName || "".Equals(StrFileName))
            {
                MessageBox.Show("源文件路徑出錯!");
                return;
            }
            CommonCollection.LogWarms = FileOperation.GetWarmData(StrFileName);
            //将当前的数据信息加到集合中去
            try
            {
                foreach (WarmInfo wm in CommonCollection.WarmInfors)
                {
                    CommonCollection.LogWarms.Add(wm);
                }
            }catch(Exception)
            {

            }
            if (null == CommonCollection.LogWarms)
            {
                MessageBox.Show("獲取警報資訊失敗!");
                return;
            }
            if (CommonCollection.LogWarms.Count<=0)
            {
                MessageBox.Show("獲取警報訊息為空!");
                return;
            }
            //从全部的警报资讯信息中取出时间段中的警报资讯
            WarmListView.Items.Clear();
            foreach(WarmInfo wm in CommonCollection.LogWarms)
            {
                if (null == wm)
                {
                    continue;
                }
                // 判断警报产生时间是否在范围中
                if (DateTime.Compare(SDT, wm.AlarmTime) > 0)
                {
                    continue;
                }
                if (DateTime.Compare(EDT, wm.AlarmTime) <= 0)
                {
                    continue;
                }
                //当前时间满足条件
                //参考点信息
                string StrRouterInfo = wm.RD[0].ToString("X2") + wm.RD[1].ToString("X2");
                string StrRouterName = wm.RDName;
                if (null == StrRouterName || "".Equals(StrRouterName))
                {
                    StrRouterInfo = "參考點:" + StrRouterInfo;
                }
                else
                {
                    StrRouterInfo = "參考點:" + StrRouterName + "(" + StrRouterInfo + ")";
                }
                //區域信息
                string StrAreaInfo = wm.AD[0].ToString("X2") + wm.AD[1].ToString("X2");
                string StrAreaName = wm.AreaName;
                if (null == StrAreaName || "".Equals(StrAreaName))
                {
                    StrAreaInfo = "所在區域:" + StrAreaInfo;
                }
                else
                {
                    StrAreaInfo = "所在區域:" + StrAreaName + "(" + StrAreaInfo + ")";
                }
                //警报产生时间
                string StrAlarmTime = "警報產生時間:" + wm.AlarmTime.ToString();
                if (DateTime.Compare(wm.AlarmTime, wm.ClearAlarmTime) != 0)
                {
                    StrAlarmTime = StrAlarmTime + " 警報處理時間:" + wm.ClearAlarmTime.ToString();
                }
                else
                {
                    StrAlarmTime = StrAlarmTime + " 警報處理時間:    未處理   ";
                }
                string StrHandler = "是否處理:" + wm.isHandler.ToString();
                string StrClear = "是否清除:" + wm.isClear.ToString();
                string StrLog = "";
                string ClassName = wm.GetType().Name;
                switch(CurWarmType)
                {
                    case SpeceilAlarm.BatteryLow:
                        if ("BattLow".Equals(ClassName))
                        {
                            string StrTagInfo = ((BattLow)wm).TD[0].ToString("X2") + ((BattLow)wm).TD[1].ToString("X2");
                            string StrTagName = ((BattLow)wm).TagName;
                            //卡片信息
                            if (null == StrTagName || "".Equals(StrTagName))
                                StrTagInfo = "卡片:" + StrTagInfo;
                            else
                                StrTagInfo = "卡片:" + StrTagName + "(" + StrTagInfo + ")";
                            //當前的電量
                            string StrBatt = "當前的電量:" + ((BattLow)wm).Batt.ToString() + "(" + ((BattLow)wm).BasicBatt.ToString() + ")" + "%";
                            StrLog ="電量不足###"+StrTagInfo + " " + StrRouterInfo + " " + StrAreaInfo + " " + StrBatt + " " + StrAlarmTime + " " + StrHandler + " " + StrClear;
                        }
                        break;
                    case SpeceilAlarm.PersonHelp:
                        if ("PersonHelp".Equals(ClassName))
                        {
                            string StrTagInfo = ((PersonHelp)wm).TD[0].ToString("X2") + ((PersonHelp)wm).TD[1].ToString("X2");
                            string StrTagName = ((PersonHelp)wm).TagName;
                            //卡片信息
                            if (null == StrTagName || "".Equals(StrTagName))
                                StrTagInfo = "卡片:" + StrTagInfo;
                            else
                                StrTagInfo = "卡片:" + StrTagName + "(" + StrTagInfo + ")";
                            StrLog ="人員求助###" + StrTagInfo + " " + StrRouterInfo + " " + StrAreaInfo + " " + StrAlarmTime + " " + StrHandler + " " + StrClear;
                        }
                        break;
                    case SpeceilAlarm.AreaControl:
                        if ("AreaAdmin".Equals(ClassName))
                        {
                            string StrTagInfo = ((AreaAdmin)wm).TD[0].ToString("X2") + ((AreaAdmin)wm).TD[1].ToString("X2");
                            string StrTagName = ((AreaAdmin)wm).TagName;
                            if (null == StrTagName || "".Equals(StrTagName))
                                StrTagInfo = "卡片:" + StrTagInfo;
                            else
                                StrTagInfo = "卡片:" + StrTagName +"("+StrTagInfo+")";
                            //卡片当前的区域状况
                            string StrTagType = "卡片權限:";
                            if (null != ((AreaAdmin)wm).TagAreaSt)
                                StrTagType = StrTagType + ((AreaAdmin)wm).TagAreaSt.GetAreasStr();
                            StrLog ="區域管制警報###"+StrTagInfo + " " + StrRouterInfo + " " + StrAreaInfo + " " + StrAlarmTime + " " + StrHandler + " " + StrClear;
                        }
                        break;
                    case SpeceilAlarm.Resid:
                        if ("PersonRes".Equals(ClassName))
                        {
                            string StrTagInfo = ((PersonRes)wm).TD[0].ToString("X2") + ((PersonRes)wm).TD[1].ToString("X2");
                            string StrTagName = ((PersonRes)wm).TagName;
                            if (null == StrTagName || "".Equals(StrTagName))StrTagInfo = "卡片:" + StrTagInfo;
                            else StrTagInfo = "卡片:" + StrTagName + "(" + StrTagInfo + ")";
                            //人员滞留时间
                            string StrResTime = "人員未移動時間:" + ((PersonRes)wm).ResTime.ToString() + "(" + ((PersonRes)wm).BasicResTime+") s";
                            StrLog = "人員未移動警報###"+StrTagInfo + " " + StrRouterInfo + " " + StrAreaInfo + " " + StrResTime + " " + StrAlarmTime + " " + StrHandler + " " + StrClear;
                        }
                        break;
                    case SpeceilAlarm.TagDis:
                        if ("TagDis".Equals(ClassName))
                        {
                            string StrTagInfo = ((TagDis)wm).TD[0].ToString("X2") + ((TagDis)wm).TD[1].ToString("X2");
                            string StrTagName = ((TagDis)wm).TagName;
                            if (null == StrTagName || "".Equals(StrTagName))StrTagInfo = "卡片:" + StrTagInfo;
                            else StrTagInfo = "卡片:" + StrTagName + "(" + StrTagInfo + ")";
                            //卡片的休眠时间
                            string StrSleepTime = "卡片的休眠時間:" + (((TagDis)wm).SleepTime/10).ToString() + " s";
                            StrLog ="卡片異常警報###"+StrTagInfo + " " + StrRouterInfo + " " + StrAreaInfo + " " + StrSleepTime + " " + StrAlarmTime + " " + StrHandler + " " + StrClear;
                        }
                        break;
                    case SpeceilAlarm.ReferDis:
                        if ("ReferDis".Equals(ClassName))
                        {
                            //参考点的休眠时间

                            string StrRouterSp = "參考點上報间隔时间:" + ((((ReferDis)wm).SleepTime <= 0)?"***" : ((ReferDis)wm).SleepTime.ToString() + " s");
                            StrLog ="參考點異常警報###"+StrRouterInfo + " " + StrAreaInfo + " " + StrRouterSp + " " + StrAlarmTime + " " + StrHandler + " " + StrClear;
                        }
                        break;
                    case SpeceilAlarm.NodeDis:
                        if ("NodeDis".Equals(ClassName))
                        {
                            string StrNodeSp = "數據節點上報間隔時間:" + ((((NodeDis)wm).SleepTime <= 0) ? "***" : ((NodeDis)wm).SleepTime.ToString() + " s");
                            StrLog = "數據節點異常警報###"+StrRouterInfo + " " + StrAreaInfo + " " + StrNodeSp + " " + StrAlarmTime + " " + StrHandler + " " + StrClear;
                        }
                        break;
                    case SpeceilAlarm.UnKnown:
                    default:
                        if ("BattLow".Equals(ClassName))
                        {
                            string StrTagInfo = ((BattLow)wm).TD[0].ToString("X2") + ((BattLow)wm).TD[1].ToString("X2");
                            string StrTagName = ((BattLow)wm).TagName;
                            //卡片信息
                            if (null == StrTagName || "".Equals(StrTagName))StrTagInfo = "卡片:" + StrTagInfo;
                            else StrTagInfo = "卡片:" + StrTagName + "(" + StrTagInfo + ")";
                            //當前的電量
                            string StrBatt = "當前的電量:" + ((BattLow)wm).Batt.ToString() + "(" + ((BattLow)wm).BasicBatt.ToString() + ")" + "%";
                            StrLog ="電量不足###" + StrTagInfo + " " + StrRouterInfo + " " + StrAreaInfo + " " + StrBatt + " " + StrAlarmTime + " " + StrHandler + " " + StrClear;

                        }else if ("PersonHelp".Equals(ClassName))
                        {
                            string StrTagInfo = ((PersonHelp)wm).TD[0].ToString("X2") + ((PersonHelp)wm).TD[1].ToString("X2");
                                string StrTagName = ((PersonHelp)wm).TagName;
                                //卡片信息
                                if (null == StrTagName || "".Equals(StrTagName))StrTagInfo = "卡片:" + StrTagInfo;
                                else StrTagInfo = "卡片:" + StrTagName + "(" + StrTagInfo + ")";
                                StrLog = "人員求助###" + StrTagInfo + " " + StrRouterInfo + " " + StrAreaInfo + " " + StrAlarmTime + " " + StrHandler + " " + StrClear;
                         }else if ("AreaAdmin".Equals(ClassName))
                         {
                             string StrTagInfo = ((AreaAdmin)wm).TD[0].ToString("X2") + ((AreaAdmin)wm).TD[1].ToString("X2");
                                string StrTagName = ((AreaAdmin)wm).TagName;
                                if (null == StrTagName || "".Equals(StrTagName)) StrTagInfo = "卡片:" + StrTagInfo;
                                else StrTagInfo = "卡片:" + StrTagName + "(" + StrTagInfo + ")";
                                //卡片当前的区域状况
                                string StrTagType = "卡片權限:";
                                if (null != ((AreaAdmin)wm).TagAreaSt)StrTagType = StrTagType + ((AreaAdmin)wm).TagAreaSt.GetAreasStr();
                                StrLog = "區域管制警報###" + StrTagInfo + " " + StrRouterInfo + " " + StrAreaInfo + " " + StrAlarmTime + " " + StrHandler + " " + StrClear;
                         }else if ("PersonRes".Equals(ClassName))
                         {
                                 string StrTagInfo = ((PersonRes)wm).TD[0].ToString("X2") + ((PersonRes)wm).TD[1].ToString("X2");
                                 string StrTagName = ((PersonRes)wm).TagName;
                                 if (null == StrTagName || "".Equals(StrTagName))StrTagInfo = "卡片:" + StrTagInfo;
                                 else StrTagInfo = "卡片:" + StrTagName + "(" + StrTagInfo + ")";
                                 //人员滞留时间
                                 string StrResTime = "人員未移動時間:" + ((PersonRes)wm).ResTime.ToString() + "(" + ((PersonRes)wm).BasicResTime + ") s";
                                 StrLog = "人員未移動警報###" + StrTagInfo + " " + StrRouterInfo + " " + StrAreaInfo + " " + StrResTime + " " + StrAlarmTime + " " + StrHandler + " " + StrClear;
                         }else if ("TagDis".Equals(ClassName))
                         {
                                 string StrTagInfo = ((TagDis)wm).TD[0].ToString("X2") + ((TagDis)wm).TD[1].ToString("X2");
                                 string StrTagName = ((TagDis)wm).TagName;
                                 if (null == StrTagName || "".Equals(StrTagName)) StrTagInfo = "卡片:" + StrTagInfo;
                                 else StrTagInfo = "卡片:" + StrTagName + "(" + StrTagInfo + ")";
                                 //卡片的休眠时间
                                 string StrSleepTime = "卡片的休眠時間:" + ((TagDis)wm).SleepTime.ToString();
                                 StrLog = "卡片異常警報###" + StrTagInfo + " " + StrRouterInfo + " " + StrAreaInfo + " " + StrSleepTime + " " + StrAlarmTime + " " + StrHandler + " " + StrClear;
                          }else if ("ReferDis".Equals(ClassName))
                          {
                                 //参考点的休眠时间
                              string StrRouterSp = "參考點上報间隔时间:" + ((((ReferDis)wm).SleepTime <= 0) ? "***" : ((ReferDis)wm).SleepTime.ToString() + " s");
                              StrLog ="參考點異常警報###" + StrRouterInfo + " " + StrAreaInfo + " " + StrRouterSp + " " + StrAlarmTime + " " + StrHandler + " " + StrClear;
                          }else if ("NodeDis".Equals(ClassName))
                          {
                              string StrNodeSp = "數據節點上報間隔時間:" + ((((NodeDis)wm).SleepTime <= 0) ? "***" : ((NodeDis)wm).SleepTime.ToString() + " s");
                              StrLog = "數據節點異常警報###" + StrRouterInfo + " " + StrAreaInfo + " " + StrNodeSp + " " + StrAlarmTime + " " + StrHandler + " " + StrClear;
                          }
                    break;
                }
                if (null != StrLog && !"".Equals(StrLog))WarmListView.Items.Add(StrLog);
            }
            label8.Text = "當前記錄數："+WarmListView.Items.Count + "條";
            if (WarmListView.Items.Count > 0)AlarmMsgOutBtn.Enabled = true;

        }
        /// <summary>
        /// 记录导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlarmMsgOutBtn_Click(object sender, EventArgs e)
        {
            if (WarmListView.Items.Count <= 0)
            {
                MessageBox.Show("請先搜索出需要的警報信息!");
                return;
            }
            SaveFileDialog MyDialog = new SaveFileDialog();
            MyDialog.Title = "選擇警報文件保存位置";
            if(txtRB.Checked)
			{
                MyDialog.Filter = "所有文本文件|*.txt";
            }
            else if (xlsRB.Checked)
            {
                MyDialog.Filter = "所有文本文件|*.xls";
            }
            if (MyDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string StrFilePath = MyDialog.FileName;
            if (txtRB.Checked)
            {
                if (!FileOperation.CreateDirFile(StrFilePath)) return;
                FileOperation.ClearFileContent(StrFilePath);
                foreach (string str in WarmListView.Items)FileOperation.WriteDataFile(StrFilePath, str+"\r\n");
            }else if(xlsRB.Checked)
            {
                //导入为xls格式
                NpoiLib MyNpoiLib = new NpoiLib("AlarmMessage");
                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 0, "警報類型");
                MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1,0,3000);
                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 1, "卡片信息");
                MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1, 1, 6000);
                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 2, "參考點信息");
                MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1, 2, 8000);
                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 3, "區域信息");
                MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1, 3, 8000);

                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 4, "當前電量");
                MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1, 4, 3000);
                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 5, "休眠時間");
                MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1, 5, 3000);
                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 6, "人員未移動時間");
                MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1, 6, 3000);
                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 7, "參考點上報間隔時間");
                MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1, 7, 3000);
                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 8, "數據節點上報間隔時間");
                MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1, 8, 3000);
                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 9, "警報產生時間");
                MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1, 9, 5000);
                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 10, "警報消除時間");
                MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1, 10, 5000);
                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 11, "是否處理");
                MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1, 11, 2000);
                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 12, "是否清除");
                MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1, 12, 2000);

                if(null == CommonCollection.LogWarms){MessageBox.Show("對不起,導出文件失敗!");return;}
                DateTime SDTP = StartDTPicker.Value;DateTime EDTP = EndDTPicker.Value;
                int syear, smonth, sday, shour, sminute, eyear, emonth, eday, ehour, eminute;
                syear = SDTP.Year; smonth = SDTP.Month; sday = SDTP.Day;eyear = EDTP.Year; emonth = EDTP.Month; eday = EDTP.Day;
                string StrSHour = sHourCb.Text;string StrSMinute = sMinitueCb.Text;
                string StrEHour = eHourCB.Text;string StrEMinute = eMinitueCB.Text;
                try
                { shour = Convert.ToInt32(StrSHour);sminute = Convert.ToInt32(StrSMinute); ehour = Convert.ToInt32(StrEHour); eminute = Convert.ToInt32(StrEMinute); }
                catch (Exception) { MessageBox.Show("開始時間與結束時間的小時與分針格式有誤!"); return; }
                DateTime SDT, EDT;
                try
                {SDT = new DateTime(syear, smonth, sday, shour, sminute, 0);EDT = new DateTime(eyear, emonth, eday, ehour, eminute, 59);}
                catch (Exception)
                {MessageBox.Show("開始時間或結束時間格式有誤!"); return;}
                int index = 1;
                foreach(WarmInfo wm in CommonCollection.LogWarms)
                {
                    if (null == wm) continue;
                    if (DateTime.Compare(SDT, wm.AlarmTime) > 0) continue;
                    if (DateTime.Compare(EDT, wm.AlarmTime) < 0) continue;
                    //参考点信息
                    string StrRouterInfo = wm.RD[0].ToString("X2") + wm.RD[1].ToString("X2");
                    string StrRouterName = wm.RDName;
                    if (null != StrRouterName && !"".Equals(StrRouterName))
                        StrRouterInfo = StrRouterName + "(" + StrRouterInfo + ")";
                    //區域信息
                    string StrAreaInfo = wm.AD[0].ToString("X2") + wm.AD[1].ToString("X2");
                    string StrAreaName = wm.AreaName;
                    if (null != StrAreaName && !"".Equals(StrAreaName))
                        StrAreaInfo =StrAreaName + "(" + StrAreaInfo + ")";
                    string StrClassName = wm.GetType().Name;
                    string StrWarmType = "";
                    switch (CurWarmType)
                    { 
                        case SpeceilAlarm.BatteryLow:
                            if ("BattLow".Equals(StrClassName))
                            {
                                StrWarmType = "低電量警報";
                                string StrTagInfo = ((BattLow)wm).TD[0].ToString("X2") + ((BattLow)wm).TD[1].ToString("X2");
                                //卡片信息
                                if (null != ((BattLow)wm).TagName && !"".Equals(((BattLow)wm).TagName))
                                    StrTagInfo = ((BattLow)wm).TagName + "(" + StrTagInfo + ")";
                                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 1, StrTagInfo);
                                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 4, ((BattLow)wm).Batt + "(" + ((BattLow)wm).BasicBatt + ") %");
                                break;
                            }else continue;
                        case SpeceilAlarm.PersonHelp:
                            if ("PersonHelp".Equals(StrClassName))
                            {
                                 StrWarmType = "人員求救警報";
                                 string StrTagInfo = ((PersonHelp)wm).TD[0].ToString("X2") + ((PersonHelp)wm).TD[1].ToString("X2");
                                 //卡片信息
                                 if (null != ((PersonHelp)wm).TagName && !"".Equals(((PersonHelp)wm).TagName))
                                     StrTagInfo = ((PersonHelp)wm).TagName + "(" + StrTagInfo + ")";
                                 MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 1, StrTagInfo);
                                break;
                            }else continue;
                        case SpeceilAlarm.AreaControl:
                            if ("AreaAdmin".Equals(StrClassName))
                            {
                                StrWarmType = "區域管制警報";
                                string StrTagInfo = ((AreaAdmin)wm).TD[0].ToString("X2") + ((AreaAdmin)wm).TD[1].ToString("X2");
                                //卡片信息
                                if (null != ((AreaAdmin)wm).TagName && !"".Equals(((AreaAdmin)wm).TagName))
                                    StrTagInfo = ((AreaAdmin)wm).TagName + "(" + StrTagInfo + ")";
                                //ExcelOperation.SaveData(index, 2, StrTagInfo);
                                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 1, StrTagInfo);
                                //string StrAreaType = "";
                               /* switch (((AreaAdmin)wm).AreaType)
                                {
                                    case AreaType.SimpleArea:
                                        StrAreaType = "一般區域";
                                        break;
                                    case AreaType.ControlArea:
                                        StrAreaType = "管制區域";
                                        break;
                                    case AreaType.DangerArea:
                                        StrAreaType = "危險區域";
                                        break;
                                }*/
                                string StrTagType = "";
                                if (null != ((AreaAdmin)wm).TagAreaSt)
                                {
                                    StrTagType = StrTagType + ((AreaAdmin)wm).TagAreaSt.GetAreasStr();
                                }
                             break;
                            }else continue;
                           
                        case SpeceilAlarm.Resid:
                            if ("PersonRes".Equals(StrClassName))
                            {
                                StrWarmType = "人員未移動警報";
                                string StrTagInfo = ((PersonRes)wm).TD[0].ToString("X2") + ((PersonRes)wm).TD[1].ToString("X2");
                                //卡片信息
                                if (null != ((PersonRes)wm).TagName && !"".Equals(((PersonRes)wm).TagName))
                                    StrTagInfo = ((PersonRes)wm).TagName + "(" + StrTagInfo + ")";
                                MyNpoiLib.writeToCell(MyNpoiLib.sheet1,index, 1,StrTagInfo);
                                MyNpoiLib.writeToCell(MyNpoiLib.sheet1,index, 6, ((PersonRes)wm).ResTime + "(" + ((PersonRes)wm).BasicResTime + ") s");
                            break;
                            }else continue;
                        case SpeceilAlarm.TagDis:
                            if ("TagDis".Equals(StrClassName))
                            {
                                StrWarmType = "卡片異常警報";
                                string StrTagInfo = ((TagDis)wm).TD[0].ToString("X2") + ((TagDis)wm).TD[1].ToString("X2");
                                //卡片信息
                                if (null != ((TagDis)wm).TagName && !"".Equals(((TagDis)wm).TagName))
                                    StrTagInfo = ((TagDis)wm).TagName + "(" + StrTagInfo + ")";
                                MyNpoiLib.writeToCell(MyNpoiLib.sheet1,index,5,StrTagInfo);
                                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 5, ((TagDis)wm).SleepTime + " s");
                                break;
                            }
                            else continue;
                        case SpeceilAlarm.ReferDis:
                            if ("ReferDis".Equals(StrClassName))
                            {
                                StrWarmType = "參考點異常警報";
                                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 7, ((ReferDis)wm).SleepTime + " s");
                                break;
                            }
                            else continue;
                        case SpeceilAlarm.NodeDis:
                            if ("NodeDis".Equals(StrClassName))
                            {
                                StrWarmType = "數據節點警報";
                               
                                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 8, ((NodeDis)wm).SleepTime + " s");
                                break;
                            }
                            else continue;
                        default:
                            StrWarmType = "未知警報類型";
                            if ("BattLow".Equals(StrClassName))
                            {
                                StrWarmType = "低電量警報";
                                string StrTagInfo = ((BattLow)wm).TD[0].ToString("X2") + ((BattLow)wm).TD[1].ToString("X2");
                                //卡片信息
                                if (null != ((BattLow)wm).TagName && !"".Equals(((BattLow)wm).TagName))
                                    StrTagInfo = ((BattLow)wm).TagName + "(" + StrTagInfo + ")";
                                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 1, StrTagInfo);
                                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 4, ((BattLow)wm).Batt + "(" + ((BattLow)wm).BasicBatt + ") %");
                            }
                            else if ("PersonHelp".Equals(StrClassName))
                            {
                                StrWarmType = "人員求救警報";
                                string StrTagInfo = ((PersonHelp)wm).TD[0].ToString("X2") + ((PersonHelp)wm).TD[1].ToString("X2");
                                //卡片信息
                                if (null != ((PersonHelp)wm).TagName && !"".Equals(((PersonHelp)wm).TagName))
                                    StrTagInfo = ((PersonHelp)wm).TagName + "(" + StrTagInfo + ")";
                         
                                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 1, StrTagInfo);
                            }
                            else if ("AreaAdmin".Equals(StrClassName))
                            {
                                StrWarmType = "區域管制警報";
                                //string StrAreaType = "";
                                string StrTagInfo = ((AreaAdmin)wm).TD[0].ToString("X2") + ((AreaAdmin)wm).TD[1].ToString("X2");
                                //卡片信息
                                if (null != ((AreaAdmin)wm).TagName && !"".Equals(((AreaAdmin)wm).TagName))
                                    StrTagInfo = ((AreaAdmin)wm).TagName + "(" + StrTagInfo + ")";
                          
                                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 1, StrTagInfo);
                               /* switch (((AreaAdmin)wm).AreaType)
                                {
                                    case AreaType.SimpleArea:
                                        StrAreaType = "一般區域";
                                        break;
                                    case AreaType.ControlArea:
                                        StrAreaType = "管制區域";
                                        break;
                                    case AreaType.DangerArea:
                                        StrAreaType = "危險區域";
                                        break;
                                }*/
                                string StrTagType = "";
                                if (null != ((AreaAdmin)wm).TagAreaSt)
                                {
                                    StrTagType = StrTagType + ((AreaAdmin)wm).TagAreaSt.GetAreasStr();
                                }
                            }
                            else if ("PersonRes".Equals(StrClassName))
                            {
                                StrWarmType = "人員未移動警報";
                                string StrTagInfo = ((PersonRes)wm).TD[0].ToString("X2") + ((PersonRes)wm).TD[1].ToString("X2");
                                //卡片信息
                                if (null != ((PersonRes)wm).TagName && !"".Equals(((PersonRes)wm).TagName))
                                    StrTagInfo = ((PersonRes)wm).TagName + "(" + StrTagInfo + ")";
                                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 1, StrTagInfo);
                                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 6, ((PersonRes)wm).ResTime + "(" + ((PersonRes)wm).BasicResTime + ") s");
                            }
                            else if ("TagDis".Equals(StrClassName))
                            {
                                StrWarmType = "卡片異常警報";
                                string StrTagInfo = ((TagDis)wm).TD[0].ToString("X2") + ((TagDis)wm).TD[1].ToString("X2");
                                //卡片信息
                                if (null != ((TagDis)wm).TagName && !"".Equals(((TagDis)wm).TagName))
                                    StrTagInfo = ((TagDis)wm).TagName + "(" + StrTagInfo + ")";
                                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 1, StrTagInfo);
                                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 5, ((TagDis)wm).SleepTime + " s");
                            }
                            else if ("ReferDis".Equals(StrClassName))
                            {
                                StrWarmType = "參考點異常警報";
                                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 7, ((ReferDis)wm).SleepTime + " s");
                            }else
                            if ("NodeDis".Equals(StrClassName))
                            { StrWarmType = "數據節點警報";
                              MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 8, ((NodeDis)wm).SleepTime + " s");
                            }
                            break;
                    }
                    MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 2, StrRouterInfo);
                    MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 3, StrAreaInfo);
                    MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 9, wm.AlarmTime.ToString());

                    if (DateTime.Compare(wm.AlarmTime, wm.ClearAlarmTime) != 0)
                        MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 10, wm.ClearAlarmTime.ToString());
                    MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 11, wm.isHandler.ToString());
                    MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 12, wm.isClear.ToString());
                    MyNpoiLib.writeToCell(MyNpoiLib.sheet1, index, 0, StrWarmType);
                    index++;
                }
                MyNpoiLib.WriteToFile(StrFilePath);
                MessageBox.Show("導出文件成功!");
            }
        }

        private void StartDTPicker_ValueChanged(object sender, EventArgs e)
        {AlarmMsgOutBtn.Enabled = false;}

        private void sHourCb_SelectedIndexChanged(object sender, EventArgs e)
        {AlarmMsgOutBtn.Enabled = false;}

        private void sMinitueCb_SelectedIndexChanged(object sender, EventArgs e)
        {AlarmMsgOutBtn.Enabled = false;}

        private void EndDTPicker_ValueChanged(object sender, EventArgs e)
        {AlarmMsgOutBtn.Enabled = false;}

        private void eHourCB_SelectedIndexChanged(object sender, EventArgs e)
        {AlarmMsgOutBtn.Enabled = false;}

        private void eMinitueCB_SelectedIndexChanged(object sender, EventArgs e)
        {AlarmMsgOutBtn.Enabled = false;}
    }
}
