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
    public partial class AccessStatistics : Form
    {
        public AccessStatistics()
        {
            InitializeComponent();
        }
        private void AccessStatistics_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new Size(832, 668);
            this.MinimumSize = new Size(832, 668);

            if (SysParam.ClearAccessTime == 7)
            {
                ClearTimeCB.SelectedIndex = 0;
            }
            else if (SysParam.ClearAccessTime == 30)
            {
                ClearTimeCB.SelectedIndex = 1;
            }
            else if (SysParam.ClearAccessTime == 90)
            {
                ClearTimeCB.SelectedIndex = 2;
            }
            else if (SysParam.ClearAccessTime == 180)
            {
                ClearTimeCB.SelectedIndex = 3;
            }
            else if (SysParam.ClearAccessTime == 365)
            {
                ClearTimeCB.SelectedIndex = 4;
            }
            else
            {
                ClearTimeCB.SelectedIndex = 1;
            
            }
        }
        /// <summary>
        /// 获取出入记录
        /// </summary>
        /// <param name="accesslist"></param>
        /// <param name="TagID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private AccessRecord SelectAccessRecord(List<AccessRecord> accesslist,byte[] TagID, ReferType type)
        { //当前所要找的是第一次外出的记录
            foreach (AccessRecord access in accesslist)
            {
                //判断ID是否相同
                if (access.TagID[0] != TagID[0] || access.TagID[1] != TagID[1])
                {
                    continue;
                }
                if (access.rttype != type)
                {//说明当前的记录也是进入基站，需要我们舍弃
                    access.isMark = true;
                    continue;
                }
                return access;
            }
            return null;
        }

        /// <summary>
        /// 在集合中取出指定的记录
        /// </summary>
        /// <param name="accesslists"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="type"></param>
        private AccessRecord SelectAccessRecord(List<AccessRecord> accesslists,byte[] TagID, DateTime dt, ReferType type)
        {
            AccessRecord curaccess = null;
            foreach (AccessRecord access in accesslists)
            {
                //ID是否相同
                if (access.TagID[0] != TagID[0] || access.TagID[1] != TagID[1])
                {
                    continue;
                }
                if (access.rttype != type)
                {
                    //判断时间是否相同
                    if (dt.Year == access.Occtime.Year && dt.Month == access.Occtime.Month && dt.Day == access.Occtime.Day)
                    {
                        access.isMark = true;
                    }
                    continue;
                }
                if (access.rttype == ReferType.Entrance)
                {//找最早的一个
                    if (dt.Year == access.Occtime.Year && dt.Month == access.Occtime.Month && dt.Day == access.Occtime.Day)
                    {
                        if (null == curaccess)
                        {
                            curaccess = access;
                        }
                        access.isMark = true;
                    }
                }
                else if (access.rttype == ReferType.Export)
                {//找最晚的一个 
                    if (dt.Year == access.Occtime.Year && dt.Month == access.Occtime.Month && dt.Day == access.Occtime.Day)
                    {
                        curaccess = access;
                        access.isMark = true;
                    }
                }
            }
            return curaccess;
        }
        private void selectbtn_Click(object sender, EventArgs e)
        {
            DateTime dt = startdtpicker.Value;
            DateTime Startdt = new DateTime(dt.Year, dt.Month, dt.Day);
            dt = enddtpicker.Value;
            DateTime Enddt = new DateTime(dt.Year, dt.Month, dt.Day);
            List<AccessRecord> lists = new List<AccessRecord>();
            string strid = "", strname = "", strinname = "",stroutname = "";
            strid = tagtb.Text;
            if (!"".Equals(strid))
            {
                strname = CommonBoxOperation.GetTagName(strid);
                if (null == strname)
                {//说明strid可能是名称
                    Tag tg = CommonBoxOperation.GetTagFromName(strid);
                    if (null != tg)
                    {
                        strid = tg.ID[0].ToString("X2") + tg.ID[1].ToString("X2");
                    }
                }
            }
            TagAccessRecordOperation.tagAccessOper.GetAccessRecord(strid, Startdt, Enddt, ref lists);
            recordlistview.Items.Clear();
            int index = -1;
            ListViewItem item = null;
            foreach (AccessRecord record in lists)
            {
                index++;
                if (record.isMark)
                {
                    continue;
                }
                item = new ListViewItem();
                item.Text = record.TagID[0].ToString("X2") + record.TagID[1].ToString("X2");
                strname = CommonBoxOperation.GetTagName(item.Text);
                if (null == strname || "".Equals(strname))
                {
                    item.SubItems.Add("****");
                }
                else
                {
                    item.SubItems.Add(strname);
                }
                AccessRecord access = null;
                if (record.rttype == ReferType.Entrance)
                {//说明当前是一个入口参考点，我们接下来需要找出口
                    strinname = CommonBoxOperation.GetRouterName(record.RouterID[0].ToString("X2") + record.RouterID[1].ToString("X2"));
                    access = SelectAccessRecord(lists.GetRange(index + 1, lists.Count - index - 1), record.TagID, ReferType.Export);
                    if (null == access)
                    {
                        continue;
                    }
                    access.isMark = true;
                    if (null == strinname || "".Equals(strinname))
                    {
                        item.SubItems.Add(record.RouterID[0].ToString("X2") + record.RouterID[1].ToString("X2"));
                    }
                    else
                    {
                        item.SubItems.Add(strinname + "(" + record.RouterID[0].ToString("X2") + record.RouterID[1].ToString("X2") +")");
                    }
                    stroutname = CommonBoxOperation.GetRouterName(access.RouterID[0].ToString("X2") + access.RouterID[1].ToString("X2"));

                    if (null == stroutname || "".Equals(stroutname))
                    {
                        item.SubItems.Add(access.RouterID[0].ToString("X2") + access.RouterID[1].ToString("X2"));
                    }
                    else
                    {
                        item.SubItems.Add(stroutname + "(" + access.RouterID[0].ToString("X2") + access.RouterID[1].ToString("X2") + ")");
                    }
                    item.SubItems.Add(record.Occtime.ToString());
                    item.SubItems.Add(access.Occtime.ToString());
                    if (DateTime.Compare(record.Occtime, access.Occtime) > 0)
                    {
                        item.SubItems.Add("****");
                    }
                    else
                    {
                        item.SubItems.Add(String.Format("{0:N2} h", (access.Occtime - record.Occtime).TotalHours));
                    }
                }
                else
                {
                    continue;
                }
                recordlistview.Items.Add(item);
            }
            label4.Text = "Current record number: " + recordlistview.Items.Count;
        }
        private void ClearTimeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ClearTimeCB.SelectedIndex == 0)
            {
                SysParam.ClearAccessTime = 7;
            }
            else if (ClearTimeCB.SelectedIndex == 1)
            {
                SysParam.ClearAccessTime = 30;
            }
            else if (ClearTimeCB.SelectedIndex == 2)
            {
                SysParam.ClearAccessTime = 90;
            }
            else if (ClearTimeCB.SelectedIndex == 3)
            {
                SysParam.ClearAccessTime = 180;
            }
            else if (ClearTimeCB.SelectedIndex == 4)
            {
                SysParam.ClearAccessTime = 365;
            }
        }
        private void AccessStatistics_FormClosing(object sender, FormClosingEventArgs e)
        {
            FileOperation.SetValue(FileOperation.OtherPath, FileOperation.StrAccessTime, FileOperation.StrAccessKey, SysParam.ClearAccessTime + "");
        }
        /// <summary>
        /// Excel导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void excelbtn_Click(object sender, EventArgs e)
        {
            //开始选择出入讯息
            DateTime dt = startdtpicker.Value;
            DateTime Startdt = new DateTime(dt.Year, dt.Month, dt.Day);
            dt = enddtpicker.Value;
            DateTime Enddt = new DateTime(dt.Year, dt.Month, dt.Day);
            List<AccessRecord> lists = new List<AccessRecord>();
            string strid = "", strname = "",strinname = "",stroutname = "";
            strid = tagtb.Text;
            if (!"".Equals(strid))
            {
                strname = CommonBoxOperation.GetTagName(strid);
                if (null == strname)
                {//说明strid可能是名称
                    Tag tg = CommonBoxOperation.GetTagFromName(strid);
                    if (null != tg)
                    {
                        strid = tg.ID[0].ToString("X2") + tg.ID[1].ToString("X2");
                    }
                }
            }
            TagAccessRecordOperation.tagAccessOper.GetAccessRecord(strid, Startdt, Enddt, ref lists);
            if (lists.Count <= 0)
            {
                MessageBox.Show("對不起，記錄不存在！");
                return;
            }
            
            SaveFileDialog MyDialog = new SaveFileDialog();
            MyDialog.Title = "選擇出入統計文件保存位置";
            MyDialog.Filter = "所有文本文件|*.xls";
            if (MyDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string StrFilePath = MyDialog.FileName;
            NpoiLib MyNpoiLib = new NpoiLib("Access statistics");
            MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 0, "Tag ID");
            MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1, 0, 6000);
            MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 1, "卡片名稱");
            MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1, 1, 6000);
            MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 2, "入口基站");
            MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1, 2, 6000);
            MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 3, "出口基站");
            MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1, 3, 6000);

            MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 4, "進入時間");
            MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1, 4, 8000);
            MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 5, "外出時間");
            MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1, 5, 8000);
            MyNpoiLib.writeToCell(MyNpoiLib.sheet1, 0, 6, "間隔時間");
            MyNpoiLib.SetColumnWidth(MyNpoiLib.sheet1, 6, 2000);

            int index = -1,row = 0;
            foreach (AccessRecord record in lists)
            {
                index++;
                if (record.isMark)
                {
                    continue;
                }
                strid = record.TagID[0].ToString("X2") + record.TagID[1].ToString("X2");
                strname = CommonBoxOperation.GetTagName(strid);
                AccessRecord access = null;
                if (record.rttype == ReferType.Entrance)
                {//说明当前是一个入口参考点，我们接下来需要找出口
                    strinname = CommonBoxOperation.GetRouterName(record.RouterID[0].ToString("X2") + record.RouterID[1].ToString("X2"));
                    access = SelectAccessRecord(lists.GetRange(index + 1, lists.Count - index - 1), record.TagID, ReferType.Export);
                    if (null == access)
                    {
                        continue;
                    }
                    stroutname = CommonBoxOperation.GetRouterName(access.RouterID[0].ToString("X2") + access.RouterID[1].ToString("X2"));
                    access.isMark = true;
                }
                else
                {
                    continue;
                }
                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, row + 1, 0, strid);
                if (null == strname || "".Equals(strname))
                {
                    MyNpoiLib.writeToCell(MyNpoiLib.sheet1, row + 1, 1, "****");
                }
                else
                {
                    MyNpoiLib.writeToCell(MyNpoiLib.sheet1, row + 1, 1, strname);
                }
                if (null == strinname || "".Equals(strinname))
                {
                    MyNpoiLib.writeToCell(MyNpoiLib.sheet1, row + 1, 2, record.RouterID[0].ToString("X2") + record.RouterID[1].ToString("X2"));
                }
                else
                {
                    MyNpoiLib.writeToCell(MyNpoiLib.sheet1, row + 1, 2,strinname + "(" + record.RouterID[0].ToString("X2") + record.RouterID[1].ToString("X2")+")");
                }
                if (null == stroutname || "".Equals(stroutname))
                {
                    MyNpoiLib.writeToCell(MyNpoiLib.sheet1, row + 1, 3, access.RouterID[0].ToString("X2") + access.RouterID[1].ToString("X2"));
                }
                else
                {
                    MyNpoiLib.writeToCell(MyNpoiLib.sheet1, row + 1, 3, stroutname + "(" + access.RouterID[0].ToString("X2") + access.RouterID[1].ToString("X2")+")");
                }
                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, row + 1, 4, record.Occtime.ToString());
                MyNpoiLib.writeToCell(MyNpoiLib.sheet1, row + 1, 5, access.Occtime.ToString());
                if (DateTime.Compare(record.Occtime, access.Occtime) > 0)
                {
                    MyNpoiLib.writeToCell(MyNpoiLib.sheet1, row + 1, 6, "****");
                }
                else
                {
                    MyNpoiLib.writeToCell(MyNpoiLib.sheet1, row + 1, 6, String.Format("{0:N2} h", (access.Occtime - record.Occtime).TotalHours));
                }
                row ++;
            }
            if (row <= 0)
            {
                MessageBox.Show("對不起，當前沒有記錄!");
                return;
            }
            MyNpoiLib.writeToCell(MyNpoiLib.sheet1, row + 1, 0, "###當前記錄總數為:");
            MyNpoiLib.writeToCell(MyNpoiLib.sheet1, row + 1, 1, row.ToString()+"     條");
            MyNpoiLib.WriteToFile(StrFilePath);
            MessageBox.Show("導出文件成功!");
        }
    }
}
