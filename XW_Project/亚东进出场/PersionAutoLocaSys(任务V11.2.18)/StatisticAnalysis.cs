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
    public partial class StatisticAnalysis : Form
    {
        public List<TagPack> PickerTagsList = new List<TagPack>();

        public Bitmap PanelBitmap = null;
        public StatisticAnalysis()
        {
            InitializeComponent();
        }
        private void StatisticAnalysis_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 24; i++){sHourCb.Items.Add(i.ToString());eHourCb.Items.Add(i.ToString());}
            for (int i = 0; i < 60; i++){sMinuteCb.Items.Add(i.ToString());eMinuteCb.Items.Add(i.ToString());}
            sHourCb.SelectedIndex=0;eHourCb.SelectedIndex=0;sMinuteCb.SelectedIndex=0;eMinuteCb.SelectedIndex=0;
            this.MaximumSize = new Size(721, 588);this.MinimumSize = new Size(721, 588);
        }
        /// <summary>
        /// 开始统计分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SttAlBtn_Click(object sender, EventArgs e)
        {
           label10.Visible = false;
           label8.Text = "";
           PanelBitmap = null;
           PieCharPanel_Paint(null, null);
           PickerTagsList.Clear();
           string StrTagInfor = TagInforTb.Text;
           if ("".Equals(StrTagInfor)){MessageBox.Show("Tag的ID或名稱不能為空!");return;}
            //查看时间是否有效
           DateTime StartDT, EndDT;
           StartDT = StartDTPicker.Value;EndDT = EndDTPicker.Value;
           int syear, smonth, sday, shour, sminute, eyear, emonth, eday, ehour, eminute;
           syear = StartDT.Year; smonth = StartDT.Month; sday = StartDT.Day;
           eyear = EndDT.Year; emonth = EndDT.Month; eday = EndDT.Day;
           if (eyear < syear || (eyear == syear && emonth < smonth) || (eyear == syear && emonth == smonth && eday < sday))
           { MessageBox.Show("結束日期時間應該在開始日期時間之後!"); return; }
           string StrSHour, StrSMinute, StrEHour, StrEMinute;
           StrSHour = sHourCb.Text;StrSMinute = sMinuteCb.Text;StrEHour = eHourCb.Text;StrEMinute = eMinuteCb.Text;
           try{shour = Convert.ToInt32(StrSHour);sminute = Convert.ToInt32(StrSMinute);ehour = Convert.ToInt32(StrEHour);eminute = Convert.ToInt32(StrEMinute);
           }catch(Exception)
           {MessageBox.Show("對不起,開始時間或結束時間格式有誤!");return;}

           string[] StrDirs = FileOperation.GetAllDirName(FileOperation.Original);
           if (null == StrDirs)
           {MessageBox.Show("對不起，没有任何原始數據包資料,不能進行軌跡分析!");return;}
           int IntStartDT, IntEndDT;
           try
           {IntStartDT = Convert.ToInt32(syear.ToString() + smonth.ToString() + sday.ToString());
            IntEndDT = Convert.ToInt32(eyear.ToString() + emonth.ToString() + eday.ToString());}
           catch (Exception)
           {MessageBox.Show("對不起,選擇的日期時間格式有誤!");return;}
           int CurFileDT;
           foreach (string DirName in StrDirs)
           {
               if (null == DirName || "".Equals(DirName)) continue;
               try
               {CurFileDT = Convert.ToInt32(DirName.Substring(DirName.LastIndexOf('\\') + 1, DirName.Length - DirName.LastIndexOf('\\') - 1));}
               catch (Exception)
               {continue;}
               if (CurFileDT < IntStartDT || CurFileDT > IntEndDT) continue;
               //当前的文件夹中的数据满足要求
               string[] AllFileNames = FileOperation.GetAllFileName(DirName);
               if (null == AllFileNames) continue;
               //符合时间条件的文件才有效:SHour<CurHour<Ehour
               foreach (string StrHour in AllFileNames)
               {
                   if (null == StrHour||"".Equals(StrHour)) continue;
                   int CurHour;
                   try
                   {string StrCurHour = StrHour.Substring(StrHour.LastIndexOf('\\') + 1, StrHour.IndexOf(".") - StrHour.LastIndexOf('\\') - 1);CurHour = Convert.ToInt32(StrCurHour);}
                   catch (Exception){continue;}
                   if (IntStartDT == CurFileDT && CurFileDT == IntEndDT)
                   { if (CurHour < shour || CurHour > ehour) continue;}
                   else if (IntStartDT < CurFileDT && CurFileDT == IntEndDT) 
                   { if (CurHour > ehour) continue; }
                   else if (IntStartDT == CurFileDT && CurFileDT < IntEndDT){ if (CurHour < shour) continue; }
                   List<TagPack> CurList = FileOperation.GetOriginalData(StrHour);
                   if (null == CurList)continue;
                   if (CurList.Count <= 0)continue;
                   foreach (TagPack tp in CurList)
                   {
                       if (null == tp) continue;
                       int CurMinite = tp.ReportTime.Minute;
                       if (IntStartDT == CurFileDT && CurFileDT == IntEndDT)
                       {
                           if (CurHour == shour && CurHour == ehour)
                           { if (CurMinite >= eminute || CurMinite < sminute) continue; }
                           else if (CurHour == shour && CurHour < ehour)
                           { if (CurMinite < sminute) continue; }
                           else if (CurHour > shour && CurHour == ehour)
                           { if (CurMinite >= eminute) continue; }
                       }
                       else if (IntStartDT < CurFileDT && CurFileDT == IntEndDT)
                       {
                           if (CurHour == ehour)
                           { if (CurMinite > eminute) continue; }
                           else if (CurHour > ehour) continue;
                       }
                       else if (IntStartDT == CurFileDT && CurFileDT < IntEndDT)
                       {
                           if (CurHour == shour) {if(CurMinite < sminute)continue; }
                           else if (CurHour < shour)continue;
                       }
                       string StrTagID = tp.TD[0].ToString("X2") + tp.TD[1].ToString("X2");
                       if (StrTagInfor.Equals(StrTagID))PickerTagsList.Add(tp);
                       Tag tag = CommonBoxOperation.GetTag(StrTagID);
                       if (null == tag) continue;
                       if (StrTagInfor.Equals(tag.Name))PickerTagsList.Add(tp);
                   }
               }
           }
           //资料全部取出到集合PickerTagsList中，统计时根据集合中的类型进行统计
           if (null == PickerTagsList || PickerTagsList.Count <= 0){label8.Text = "當前的時間段不存在任何Tag！";return;}
           List<Area> Areas = GetAreas(PickerTagsList);
           if (null == Areas || Areas.Count <= 0){label8.Text = "當前時間段上的Tag区域不存在!";return;}
           label10.Visible=true;
           PanelBitmap=null;
           PieCharPanel_Paint(null,null);
           // 计算时间段上的总秒数
           int TotalSec = 0;
           TotalSec=(eyear-syear)*365*24*60*60+(emonth-smonth)*30*24*60*60+(eday-sday)*24*60*60+(ehour-shour)*60*60+(eminute-sminute)*60;
           label8.Text = "統計的總時間： "+TotalSec+" 秒";
           PanelBitmap = new Bitmap(654,317);
           PanelBitmap = new Bitmap(PanelBitmap);
           Graphics.FromImage(PanelBitmap).DrawString("統計餅狀圖",new Font("黑体", 12),Brushes.Black,PanelBitmap.Width/2+75,10);
           Graphics.FromImage(PanelBitmap).DrawString(StrTagInfor+"卡片在各個區域的停留總時間：",new Font("黑体", 10),Brushes.Black,5,20);
           Graphics.FromImage(PanelBitmap).FillEllipse(Brushes.Gray,PanelBitmap.Width/2+20,50,200,200);
           Brush CurAreaColor = null;
           float startAngle = -90;
           int index=0;
           foreach(Area CurArea in Areas)
           {   if(null == CurArea)continue;
           string StrAreaID = CurArea.ID[0].ToString("X2") + CurArea.ID[1].ToString("X2");
               int num = GetTagNum(PickerTagsList,StrAreaID);
               //----表示当前Tag在区域CurArea中的数据包数----//
               switch (index)
               {   case 0:CurAreaColor = Brushes.Red;break;
                   case 1:CurAreaColor = Brushes.Blue;break;
                   case 2:CurAreaColor = Brushes.Green;break;
                   case 3:CurAreaColor = Brushes.Yellow;break;
                   case 4:CurAreaColor = Brushes.Azure;break;
                   case 5:CurAreaColor = Brushes.Beige;break;
                   case 6:CurAreaColor = Brushes.Brown;break;
                   case 7:CurAreaColor = Brushes.Coral; break;
                   case 8:CurAreaColor = Brushes.Cyan; break;
                   case 9:CurAreaColor = Brushes.Fuchsia;break;
                   default:CurAreaColor= Brushes.Snow;break;
               }
               string StrTagID = PickerTagsList[0].TD[0].ToString("X2") + PickerTagsList[0].TD[1].ToString("X2");
               int TagInAreaTime = GetTagInAreaTimeReport(PickerTagsList, StrAreaID, StrTagID);
               float rec = ((float)TagInAreaTime / TotalSec);
               Graphics.FromImage(PanelBitmap).FillPie(CurAreaColor, PanelBitmap.Width / 2 + 20,50, 200, 200, startAngle,rec * 360);
               if ("".Equals(CurArea.Name))Graphics.FromImage(PanelBitmap).DrawString("  #" + StrAreaID + "區域：" + TagInAreaTime.ToString() + "秒" + " 百分比：" + (rec * 100).ToString("#0.00") + "%", new Font("宋体", 10), CurAreaColor, 5, 20 + (index + 1) * 20);
               else Graphics.FromImage(PanelBitmap).DrawString("  #" + CurArea.Name + "(" + StrAreaID + ")區域：" + TagInAreaTime.ToString() + "秒" + " 百分比：" + (rec * 100).ToString("#0.00") + "%", new Font("宋体", 10), CurAreaColor, 5, 20 + (index + 1) * 20);
               startAngle += rec * 360;
               index++;
           }
           PieCharPanel_Paint(null, null);
        }
        /// <summary>
        /// 获取在集合Tags中在区域StrAreaID上的数量
        /// </summary>
        /// <param name="Tags"></param>
        /// <param name="StrAreaID"></param>
        /// <returns></returns>
        public int GetTagNum(List<TagPack> Tags,string StrAreaID)
        {
            if (null == Tags || Tags.Count <= 0) return 0;
            if (null == StrAreaID || "".Equals(StrAreaID)) return 0;
            int num = 0;
            foreach(TagPack tpk in Tags)
            {   if(null==tpk)continue;
            string StrRouterID = tpk.RD_New[0].ToString("X2") + tpk.RD_New[1].ToString("X2");
                Area CurArea=CommonBoxOperation.GetAreaFromRouterID(StrRouterID);
                if(null==CurArea)continue;
                string StrCurAreaID = CurArea.ID[0].ToString("X2") + CurArea.ID[1].ToString("X2");
                if(StrAreaID.Equals(StrCurAreaID))num++;
            }
            return num;
        }

        public List<Area> GetAreas(List<TagPack> Tags)
        {
            if (null == Tags) return null;
            if (Tags.Count <= 0) return null;
            List<Area> Areas = new List<Area>();
            foreach(TagPack tp in Tags)
            {   if (null == tp) continue;
            string StrRouterID = tp.RD_New[0].ToString("X2") + tp.RD_New[1].ToString("X2");
                Area area = CommonBoxOperation.GetAreaFromRouterID(StrRouterID);
                if (null == area) continue;
                string StrAreaID = area.ID[0].ToString("X2") + area.ID[1].ToString("X2");
                if (!IsAreas(Areas, StrAreaID)) Areas.Add(area);
            }
            return Areas;
        }

        public bool IsAreas(List<Area> Areas,string StrAreaId)
        {
            if (null==Areas||Areas.Count<=0)return false;
            if (null==StrAreaId||"".Equals(StrAreaId))return false;
            foreach(Area area in Areas)
            {   if (null==area)continue;
            string StrAreaID = area.ID[0].ToString("X2") + area.ID[1].ToString("X2");
                if (StrAreaId.Equals(StrAreaID))
                {return true;}
            }
            return false;
        }
        /// <summary>
        /// 根据接收到的数据包统计总的时间
        /// </summary>
        /// <returns></returns>
        public int GetTagInAreaTimeReport(List<TagPack> Tags, string StrAreaID, string StrTagID)
        {
            if (null == Tags || null == StrAreaID || "".Equals(StrAreaID) || null == StrTagID || "".Equals(StrTagID)) return 0;
            if (Tags.Count <= 0) return 0;
            int TotalTime = 0;
            TagPack[] tpks = Tags.ToArray();
            for (int i = 0; i < tpks.Length;i++)
            {
                if (null == Tags[i]) continue;
                string StrTID = Tags[i].TD[0].ToString("X2") + Tags[i].TD[1].ToString("X2");
                if (StrTID.Equals(StrTagID))
                {
                    string StrRID = Tags[i].RD_New[0].ToString("X2") + Tags[i].RD_New[1].ToString("X2");
                    Area CurArea = CommonBoxOperation.GetAreaFromRouterID(StrRID);
                    if (null == CurArea) continue;
                    string StrAID = CurArea.ID[0].ToString("X2") + CurArea.ID[1].ToString("X2");
                    if (StrAID.Equals(StrAreaID))
                    { //判断确定在区域中了
                        if (i + 1 < tpks.Length)
                        {TotalTime += (tpks[i + 1].ReportTime - tpks[i].ReportTime).Seconds;}
                        else continue;
                    }
                }
            }
            return TotalTime;
        }
        /// <summary>
        /// 获取Tag在指定的区域呆的总时间；
        /// </summary>
        /// <param name="Tags"></param>
        /// <param name="StrAreaID"></param>
        /// <param name="StrTagID"></param>
        /// <returns></returns>
        public int GetTagInAreaTime(List<TagPack> Tags,string StrAreaID,string StrTagID)
        {
            if (null == Tags || null == StrAreaID || "".Equals(StrAreaID) || null == StrTagID || "".Equals(StrTagID))return 0;
            int TotalTime = 0;
            foreach(TagPack tpk in Tags)
            {
                if (null == tpk) continue;
                string StrTID = tpk.TD[0].ToString("X2") + tpk.TD[1].ToString("X2");
                if (StrTID.Equals(StrTagID))
                {
                    string StrRID = tpk.RD_New[0].ToString("X2") + tpk.RD_New[1].ToString("X2");
                    Area CurArea =  CommonBoxOperation.GetAreaFromRouterID(StrRID);
                    if (null == CurArea) continue;
                    string StrAid = CurArea.ID[0].ToString("X2") + CurArea.ID[1].ToString("X2");
                    if (StrAid.Equals(StrAreaID))TotalTime += tpk.Sleep;
                }
            }
            return TotalTime;
        }

        private void PieCharPanel_Paint(object sender, PaintEventArgs e)
        {
            if (null != PanelBitmap) PieCharPanel.CreateGraphics().DrawImageUnscaled(PanelBitmap, 0, 0);
            else PieCharPanel.CreateGraphics().Clear(Color.White);
        }
    }
}
