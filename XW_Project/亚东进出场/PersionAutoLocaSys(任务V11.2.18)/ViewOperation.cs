using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PersionAutoLocaSys
{
    public partial class ViewOperation : Form
    {
        public ViewOperation()
        {
            InitializeComponent();
        }
        private void ViewOperation_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new Size(749, 614);
            this.MinimumSize = new Size(749, 614);
            //刷新清理时间显示
            switch (SysParam.cleardaysnum)
            {
                case 7: clearopertimecb.SelectedIndex = 0;break;
                case 30:clearopertimecb.SelectedIndex = 1;break;
                case 90: clearopertimecb.SelectedIndex = 2; break;
                case 180: clearopertimecb.SelectedIndex = 3;break;
                case 365: clearopertimecb.SelectedIndex = 4;break;
                case int.MaxValue: clearopertimecb.SelectedIndex = 5;break;
                default: clearopertimecb.SelectedIndex = 1;break;
            }
        }
        private void selectbtn_Click(object sender, EventArgs e)
        {
            string stryear, strmonth, strday , str, strperson;
            int year, month, day, start, end;
            Object obj = null;
            List<PersonOperation> curperopers = null, allperopers = new List<PersonOperation>();
            DateTime startdt, enddt, mdt;
            startdt = new DateTime(startdtpk.Value.Year, startdtpk.Value.Month, startdtpk.Value.Day,0,0,0);
            enddt = new DateTime(enddtpk.Value.Year, enddtpk.Value.Month, enddtpk.Value.Day,23,59,59);
            strperson = persontxt.Text.ToUpper();
            if(DateTime.Compare(startdt,enddt) > 0)
            {
                MessageBox.Show("對不起，開始時間不能晚於結束時間!");
                return;
            }
            string[] strfiles = null;
            try
            {
                if (Directory.Exists(FileOperation.OperRecordPath))
                {
                    strfiles = Directory.GetFiles(FileOperation.OperRecordPath);
                }
            }catch(Exception ex)
            {
                FileOperation.WriteLog("查找操作记录出错，错误原因:"+ex.ToString());
            }
            if (null != strfiles)
            {
                for (int i = 0; i < strfiles.Length; i++)
                {
                    start = strfiles[i].LastIndexOf("\\");
                    end = strfiles[i].LastIndexOf(".dat");
                    str = strfiles[i].Substring(start + 1, end - start - 1);
                    if (str.Length != 8)
                        continue;
                    stryear = str.Substring(0, 4);
                    strmonth = str.Substring(4, 2);
                    strday = str.Substring(6, 2);
                    try
                    {
                        year = Convert.ToInt32(stryear);
                        month = Convert.ToInt32(strmonth);
                        day = Convert.ToInt32(strday);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    mdt = new DateTime(year, month, day);
                    if (DateTime.Compare(startdt, mdt) > 0 || DateTime.Compare(enddt, mdt) < 0)
                    {
                        continue;
                    }
                    //将当前的文件序列化出来以后显示在文档中
                    FileOperation.DeserializeObject(out obj, strfiles[i]);
                    curperopers = obj as List<PersonOperation>;
                    if (null == curperopers)
                        continue;
                    allperopers.AddRange(curperopers.ToArray());
                }
            }
            //查看当前集合中的元素是否在时间范围以内
            foreach(PersonOperation curoper in CommonCollection.personOpers)
            {
                if (null == curoper)
                    continue;
                if (DateTime.Compare(startdt, curoper.operdt) > 0 || DateTime.Compare(enddt, curoper.operdt) < 0)
                    continue;
                allperopers.Add(curoper);
            }
            peroperlv.Items.Clear();
            foreach (PersonOperation peroper in allperopers)
            {
                str = peroper.ID[0].ToString("X2") + peroper.ID[1].ToString("X2");
                ListViewItem item = new ListViewItem();
                if ("0000".Equals(str) || "FFFF".Equals(str))
                {
                    item.Text = "****";
                }
                else
                {
                    item.Text = str;
                }
				Person ps = null;
                CommonCollection.Persons.TryGetValue(str,out ps);
                if (null != ps)
                {
                    if (!strperson.Equals(str) && !strperson.Equals(ps.Name.ToUpper()) && !"".Equals(strperson))
                    {
                        continue;
                    }
                    item.SubItems.Add(ps.Name);
                }
                else
                {
                    if (peroper.ID[0] != 0xFF || peroper.ID[1] != 0xFF)
                    {
                        if (!strperson.Equals(str))
                        {
                            continue;
                        }
                        item.SubItems.Add("");
                    }
                    else
                    {
                        item.SubItems.Add(ConstInfor.dmatekname);
                    }
                }
                item.SubItems.Add(peroper.operdt.ToString());
                item.SubItems.Add(GetPersonOperation(peroper.mopertype));
                peroperlv.Items.Add(item);
            }
        }
        private string GetPersonOperation(OperType curopertype)
        {
            string str = "";
            switch (curopertype)
            {
                case OperType.OpenForm:
                    str = "打開窗體";
                    break;
                case OperType.CloseForm:
                    str = "關閉窗體";
                    break;
                case OperType.OpenListen:
                    str = "打開監聽";
                    break;
                case OperType.CloseListen:
                    str = "關閉監聽";
                    break;
                case OperType.EnterSetting:
                    str = "進入設置界面";
                    break;
                case OperType.LeaveSetting:
                    str = "離開設置界面";
                    break;
                case OperType.SearchTag:
                    str = "搜索卡片";
                    break;
                case OperType.SearchRefer:
                    str = "搜索參考點";
                    break;
                case OperType.SearchNode:
                    str = "搜索數據節點";
                    break;
                case OperType.HandlePersonHelpAlarm:
                    str = "處理人員求救警報";
                    break;
                case OperType.DelePersonHelpAlarm:
                    str = "删除人員求救警報";
                    break;
                case OperType.HandleAreaAdminAlarm:
                    str = "處理區域管制警報";
                    break;
                case OperType.DeleAreaAdminAlarm:
                    str = "刪除區域管制警報";
                    break;
                case OperType.HandlePersonStopAlarm:
                    str = "處理人員未移動警報";
                    break;
                case OperType.DelePersonStopAlarm:
                    str = "删除人員未移動警報";
                    break;
                case OperType.HandleBattLackAlarm:
                    str = "處理電量不足警報";
                    break;
                case OperType.DeleBattLackAlarm:
                    str = "删除電量不足警報";
                    break;
                case OperType.HandleTagExcepAlarm:
                    str = "處理卡片異常斷開警報";
                    break;
                case OperType.DeleTagExcepAlarm:
                    str = "刪除卡片異常斷開警報";
                    break;
                case OperType.HandleReferExcepAlarm:
                    str = "處理參考點異常斷開警報";
                    break;
                case OperType.DeleReferExcepAlarm:
                    str = "刪除參考點異常斷開警報";
                    break;
                case OperType.HandleNodeExcepAlarm:
                    str = "處理數據節點異常斷開警報";
                    break;
                case OperType.DeleNodeExcepAlarm:
                    str = "刪除數據節點異常斷開警報";
                    break;
                case OperType.ViewTraceAnalysis:
                    str = "進入瀏覽人員軌跡界面";
                    break;
                case OperType.ViewAlarmRecord:
                    str = "進入報警記錄查詢界面";
                    break;
                case OperType.PersonOperation:
                    str = "進入查看人員操作界面";
                    break;
                case OperType.LoginIn:
                    str = "登錄系統";
                    break;
                case OperType.LoginOut:
                    str = "退出登錄";
                    break;
                case OperType.EnterNodeParam:
                    str = "進入數據節點設備網絡參數設置界面";
                    break;
                case OperType.EnterReferParam:
                    str = "進入參考點參數設置界面";
                    break;
                case OperType.LeaveNodeParam:
                    str = "離開數據節點設備網絡參數設置界面";
                    break;
                case OperType.LeaveReferParam:
                    str = "離開參考點參數設置界面";
                    break;
                case OperType.NodeSettingDevice:
                    str = "通過數據節點設置周圍設備參數";
                    break;
                case OperType.DebugSetAroundDeviceParam:
                    str = "調試模式下通過數據節點搜索或設置周圍設備參數";
                    break;
            }
            return str;
        }
        private void clearopertimecb_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (clearopertimecb.SelectedIndex)
            {
                case 0:
                    SysParam.cleardaysnum = 7;
                    break;
                case 1:
                    SysParam.cleardaysnum = 30;
                    break;
                case 2:
                    SysParam.cleardaysnum = 90;
                    break;
                case 3:
                    SysParam.cleardaysnum = 180;
                    break;
                case 4:
                    SysParam.cleardaysnum = 365;
                    break;
                case 5:
                    SysParam.cleardaysnum = Int32.MaxValue;
                    break;
            }
        }
        private void ViewOperation_FormClosing(object sender, FormClosingEventArgs e)
        {
            //保存参数
            FileOperation.SetValue(FileOperation.OtherPath, FileOperation.StrPersonOperSeg, FileOperation.StrOpertimeKey, SysParam.cleardaysnum + "");
        }
    }
}
