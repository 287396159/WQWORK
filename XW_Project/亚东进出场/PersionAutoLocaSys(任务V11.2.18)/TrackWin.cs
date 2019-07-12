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
    public partial class TrackWin : Form
    {
        public Brush SimpleBrush = Brushes.Green;
        public Brush AlarmBrush = Brushes.Red;
        public Brush FontBrush = Brushes.Black;
        public List<TagPack> PickerTagsList=new List<TagPack>();
        public TagPack[] PickTags = null;
        public  int speed = 1000;
        public  Timer DrawTimer;
        public DateTime StartDTe,EndDTe;
        public Bitmap TrackBtmp = null;
        public int CurIndex = 0;
        public Bitmap TrackBtm = null;
        private string StrStart = "Start";
        public void Start()
        {
            if (null == DrawTimer) DrawTimer = new Timer();
            DrawTimer.Tick += DrawTimer_Tick;
            DrawTimer.Interval = speed;
            PickTags = PickerTagsList.ToArray();
            CurIndex = 0;
            TrackBtm = null;
            TrackPanel_Paint(null, null);
            label7.Text = "當前時間：" + StartDTe.ToString();
            CurAreaLabel.Text = "當前區域：空";
            StartBtn.Text = "Stop";
            PauseBtn.Text = "暫停";
            DrawTimer.Start();
        }
        public void Stop()
        {
            if (null == DrawTimer) return;
            DrawTimer.Stop();
            DrawTimer = null;
			StartBtn.Text = "Start";
			Application.DoEvents(); 
			TrackPanel_Paint(null, null);
        }
        public void DrawTimer_Tick(Object obj,EventArgs args)
        {
            StartDTe = StartDTe.AddMilliseconds(1000);//每次增加1s
            label7.Text = "當前時間：" + StartDTe.ToString();
            if (CurIndex >= PickTags.Length)
            { 
                Stop();
                return; 
            }
            float scalew, scaleh;
            if (DateTime.Compare(StartDTe, PickTags[CurIndex].ReportTime) >= 0)
            {//画出当前的地图
                string StrTagID = PickTags[CurIndex].TD[0].ToString("X2") + PickTags[CurIndex].TD[1].ToString("X2");
                string StrRouterID = PickTags[CurIndex].RD_New[0].ToString("X2") + PickTags[CurIndex].RD_New[1].ToString("X2");
                Area CurArea = CommonBoxOperation.GetAreaFromRouterID(StrRouterID);
                if (CurArea != null)
                {
                    if (null == CurArea.Name || "".Equals(CurArea.Name))
                    {
                        CurAreaLabel.Text = "當前區域：" + CurArea.ID[0].ToString("X2") + CurArea.ID[1].ToString("X2");
                    }
                    else
                    {
                        CurAreaLabel.Text = "當前區域：" + CurArea.Name + "(" + CurArea.ID[0].ToString("X2") + CurArea.ID[1].ToString("X2") + ")";
                    }
                    BasicRouter CurBasic = CommonBoxOperation.GetBasicRouter(StrRouterID);
                    CurBasic.isReport = true;
                    string StrTagInfor = "";
                    string StrTagName = CommonBoxOperation.GetTagName(StrTagID);
                    if (null == StrTagName || "".Equals(StrTagName)) StrTagInfor = StrTagID;
                    else StrTagInfor = StrTagName + "(" + StrTagID + ")";

                    scalew = (float)TrackPanel.Width / CurArea.AreaBitMap.MyBitmap.Width;
                    scaleh = (float)TrackPanel.Height / CurArea.AreaBitMap.MyBitmap.Height;
                    TrackBtm = new Bitmap(655, 358);
                    TrackBtm = new Bitmap(CurArea.AreaBitMap.MyBitmap, TrackPanel.Width, TrackPanel.Height);
                    //画Router的信息
                    DrawAreaMap.DrawBasicRouter(TrackBtm, CurArea.ID[0].ToString("X2") + CurArea.ID[1].ToString("X2"), 1, scalew, scaleh);
                    //画Tag的信息
                    if (null != CurBasic)
                    {
                        if (PickTags[CurIndex].isAlarm == 0x03)
                        {
                            Graphics.FromImage(TrackBtm).FillEllipse(SimpleBrush, CurBasic.x * scalew - 8, CurBasic.y * scaleh - 32, ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                            Graphics.FromImage(TrackBtm).DrawString(StrTagInfor, new Font("宋体", 10), SimpleBrush, CurBasic.x * scalew - 5, CurBasic.y * scaleh - 42);
                        }
                        else if (PickTags[CurIndex].isAlarm == 0x04)
                        {
                            Graphics.FromImage(TrackBtm).FillEllipse(AlarmBrush, CurBasic.x * scalew - 8, CurBasic.y * scaleh - 32, ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                            Graphics.FromImage(TrackBtm).DrawString(StrTagInfor, new Font("宋体", 10), AlarmBrush, CurBasic.x * scalew - 5, CurBasic.y * scaleh - 42);
                        }
                    }
                    TrackPanel_Paint(null, null);
                }
                CurIndex++;
            }
        }
        public TrackWin()
        {
            InitializeComponent();
        }
        private void TrackWin_Load(object sender, EventArgs e)
        {
            for(int i=0;i<60;i++)sMinitueCb.Items.Add(i.ToString());
            sMinitueCb.SelectedIndex = 0;
            for (int i = 0; i < 24;i++)sHourCb.Items.Add(i.ToString());
            sHourCb.SelectedIndex = 0;
            for (int i = 0; i < 60;i++)eMinitueCB.Items.Add(i.ToString());
            eMinitueCB.SelectedIndex = eMinitueCB.Items.Count - 1;
            for (int i = 0; i < 24;i++)eHourCB.Items.Add(i.ToString());
            eHourCB.SelectedIndex = eHourCB.Items.Count - 1;
            this.MaximumSize = new Size(721, 638);
            this.MinimumSize = new Size(721, 638);
        }
        /// <summary>
        /// 开始按钮，从原始资料中取出时间段的数据包数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartBtn_Click(object sender, EventArgs e)
        {
            if (StrStart.Equals(StartBtn.Text))
            {
                //获取设置的年月日，并判断其是否符合条件
                int syear, smonth, sday, eyear, emonth, eday, shour, sminute, ehour, eminute;
                //tag的ID或名称
                string strtaginfor = TrackTagText.Text.ToUpper().Trim();
                if ("".Equals(strtaginfor))
                {
                    MessageBox.Show("軌跡分析的Tag信息不能為空!");
                    return;
                }
                Tag tag_name = CommonBoxOperation.GetTagFromName(strtaginfor);
                if (null != tag_name)
                    strtaginfor = tag_name.ID[0].ToString("X2") + tag_name.ID[1].ToString("X2");
                
                DateTime StartDT, EndDT;
                StartDT = StartDTPicker.Value;
                EndDT = EndDTPicker.Value;
                syear = StartDT.Year;
                smonth = StartDT.Month;
                sday = StartDT.Day;
                eyear = EndDT.Year;
                emonth = EndDT.Month;
                eday = EndDT.Day;
                try
                {
                    shour = Convert.ToInt32(sHourCb.Text);
                    sminute = Convert.ToInt32(sMinitueCb.Text);
                    ehour = Convert.ToInt32(eHourCB.Text);
                    eminute = Convert.ToInt32(eMinitueCB.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("對不起,設置的開始時間或結束時間有誤!");
                    return;
                }
                //获取开始时间和结束时间
                StartDT = new DateTime(syear, smonth, sday, shour, sminute, 0);
                EndDT = new DateTime(eyear, emonth, eday, ehour, eminute, 59);
                //小于零t1早于t2
                if (DateTime.Compare(StartDT, EndDT) > 0)
                {
                    MessageBox.Show("結束日期時間應該在開始日期時間之後!");
                    return;
                }
                //取出原始记录信息
                string strrecorddir = Environment.CurrentDirectory + @"\Record";
                if (!Directory.Exists(strrecorddir))
                {
                    MessageBox.Show("對不起，沒有任何記錄!");
                    return;
                }
                string[] StrDirs = FileOperation.GetAllDirName(strrecorddir);
                if (null == StrDirs || StrDirs.Length <= 0)
                {
                    MessageBox.Show("對不起，沒有任何記錄!");
                    return;
                }
                if (null != DrawIMG.Frm && DrawIMG.Frm.MyUdpClient != null)
                {
                    MessageBox.Show("對不起，查詢歷史軌跡時需要關閉網絡連接!");
                    return;
                }
                string strdirname = "", strcuryear = "", strcurmonth = "", strcurday = "", strcurhour = "";
                int index = -1, curyear = -1, curmonth = -1, curday = -1, curhour = -1;
                DateTime curdt;
                PickerTagsList.Clear();
                //判断年月日是否符合条件
                foreach (string dir in StrDirs)
                {
                    index = dir.LastIndexOf("\\");
                    strdirname = dir.Substring(index + 1, 8);
                    strcuryear = strdirname.Substring(0, 4);
                    strcurmonth = strdirname.Substring(4, 2);
                    strcurday = strdirname.Substring(6, 2);
                    try
                    {
                        curyear = Convert.ToInt32(strcuryear);
                        curmonth = Convert.ToInt32(strcurmonth);
                        curday = Convert.ToInt32(strcurday);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    //文件创建的时间
                    curdt = new DateTime(curyear, curmonth, curday);
                    DateTime startdt = new DateTime(StartDT.Year, StartDT.Month, StartDT.Day);
                    DateTime enddt = new DateTime(EndDT.Year, EndDT.Month, EndDT.Day);
                    //判断文件创建时间是否在指定的范围内
                    if (DateTime.Compare(curdt, startdt) < 0 || DateTime.Compare(curdt, enddt) > 0)
                    {
                        continue;
                    }
                    //获取到当前时间的小时部分
                    try
                    {
                        string[] StrFiles = Directory.GetFiles(dir);
                        foreach (string str in StrFiles)
                        {
                            strcurhour = str.Substring(str.LastIndexOf("\\") + 1, 2);
                            try
                            {
                                curhour = Convert.ToInt32(strcurhour);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                            //此时判断小时是否满足要求
                            curdt = new DateTime(curdt.Year, curdt.Month, curdt.Day, curhour, 0, 0);
                            startdt = new DateTime(startdt.Year, startdt.Month, startdt.Day, shour, 0, 0);
                            enddt = new DateTime(enddt.Year, enddt.Month, enddt.Day, ehour, 0, 0);
                            if (DateTime.Compare(curdt, startdt) < 0 || DateTime.Compare(curdt, enddt) > 0)
                                continue;
                            //记录年月日时都满足要求
                            startdt = new DateTime(startdt.Year, startdt.Month, startdt.Day, startdt.Hour, sminute, 0);
                            enddt = new DateTime(enddt.Year, enddt.Month, enddt.Day, enddt.Hour, eminute, 0);
                            RecordOperation.GetRecord(startdt, enddt,strtaginfor, str, ref PickerTagsList);
                        }
                    }
                    catch (System.IO.IOException)
                    {
                        MessageBox.Show("對不起,文件流已经被占用。請先關閉連接監控...");
                    }
                    catch (Exception ex)
                    {
                        FileOperation.WriteLog("获取原始记录时出现异常，异常异常原因:" + ex.ToString());
                    }
                }
                List<TagPack> deletagpacks = new List<TagPack>();
                TagPack temptpk = null;
                //清除掉两个序列号相同的Tag数据包
                for (int i = 0; i < PickerTagsList.Count; i++)
                {
                    if (i + 1 < PickerTagsList.Count)
                    {
                        if (PickerTagsList[i].index == PickerTagsList[i + 1].index)
                        {
                            if (PickerTagsList[i].SigStren < PickerTagsList[i + 1].SigStren)
                            {//交换位置
                                temptpk = CommonBoxOperation.CloneTagPack(PickerTagsList[i]);
                                PickerTagsList[i] = PickerTagsList[i + 1];
                                PickerTagsList[i + 1] = temptpk;
                            }
                            deletagpacks.Add(PickerTagsList[i]);
                        }
                    }
                }
                foreach (TagPack deletp in deletagpacks)
                {
                    PickerTagsList.Remove(deletp);
                }
                //此时所有符合添加的数据包都添加到PickerTags集合中处了
                TrackListBX.Items.Clear();
                foreach (TagPack tpk in PickerTagsList)
                {
                    TrackListBX.Items.Add(
                       tpk.ReportTime + " TagID:" +
                       tpk.TD[0].ToString("X2") + tpk.TD[1].ToString("X2") + " RouterID:" +
                       tpk.RD_New[0].ToString("X2") + tpk.RD_New[1].ToString("X2") +
                       " 是否警報:" + (tpk.isAlarm == 0x04 ? "yes" : "no") + " 信號強度:" + tpk.SigStren.ToString() +
                       " 電量:" + tpk.Bat.ToString() + " 休眠時間:" + (tpk.Sleep/10).ToString() + " 未移動時間:" + tpk.ResTime.ToString() +
                       " 序列號:" + tpk.index.ToString());
                }
                recordnumlabel.Text = "總記錄數為：" + PickerTagsList.Count;
                if (PickerTagsList.Count <= 0)
                {
                    MessageBox.Show("該Tag的原始資料不存在!");
                    return;
                }
                //图形模式画图地图
                StartDTe = new DateTime(StartDT.Year, StartDT.Month, StartDT.Day, StartDT.Hour, StartDT.Minute, StartDT.Second);
                EndDTe = new DateTime(EndDT.Year, EndDT.Month, EndDT.Day, EndDT.Hour, EndDT.Minute, EndDT.Second);
                Start();
            }
            else
            {
                Stop();
                recordnumlabel.Text = "總記錄數為：0";
                return;
            }
        }
        /// <summary>
        /// 找出前后阈值时间
        /// </summary>
        /// <param name="alltpks"></param>
        /// <param name="startdt"></param>
        /// <returns></returns>
        public int FindStartThreSholdTime(List<TagPack> alltpks,DateTime startdt)
        {
            for (int i = 0; i < alltpks.Count;i++)
            {
                if (DateTime.Compare(alltpks[i].ReportTime, startdt) <= 0)
                {//当前时间在前面
                    if (i + 1 < alltpks.Count)
                    {
                        if (DateTime.Compare(alltpks[i + 1].ReportTime, startdt) > 0)
                        {//此时i+1即为最开始一包数据
                            return i + 1;
                        }
                    }
                    else return -1;
                }
                else return i;
            }
            return -1;
        }

        public int FindEndThreSholdTime(List<TagPack> alltpks, DateTime enddt)
        {
            for (int i = 0; i < alltpks.Count; i++)
            {
                if (DateTime.Compare(alltpks[i].ReportTime,enddt) <= 0)
                {//当前时间在前面
                    if (i + 1 < alltpks.Count)
                    {
                        if (DateTime.Compare(alltpks[i + 1].ReportTime, enddt) > 0)
                        {//此时i+1即为最开始一包数据
                            return i;
                        }
                    }
                    else return i;
                }
                else return -1;
            }
            return -1;
        }
        /// <summary>
        /// 轨迹面板，用于画出Tag的运动轨迹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrackPanel_Paint(object sender, PaintEventArgs e)
        {
            if (null == TrackPanel || TrackPanel.IsDisposed)
            {
                return;
            }
            try
            {
                if (null != TrackBtm)
                {
                    TrackPanel.CreateGraphics().DrawImageUnscaled(TrackBtm, 0, 0);
                }
                else
                {
                    TrackPanel.CreateGraphics().Clear(Color.White);
                }
            }catch(Exception)
            {
            
            }
        }
        /// <summary>
        /// 提高速度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpeedUpBtn_Click(object sender, EventArgs e)
        {
            if (speed <= 50)
            {
                MessageBox.Show("已達到最大速度!");
                return;
            }
            speed -= 475;
            if (null != DrawTimer)
                DrawTimer.Interval = speed;
        }
        /// <summary>
        /// 减低速度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpeedDownBtn_Click(object sender, EventArgs e)
        {
            if (speed >= 1950)
            {
                MessageBox.Show("以達到最低速度!");
                return;
            }
            speed += 475;
            if (null != DrawTimer)
            DrawTimer.Interval = speed;
        }
        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PauseBtn_Click(object sender, EventArgs e)
        {
            if (null == DrawTimer) return;
            if ("暫停".Equals(PauseBtn.Text))
            {
                DrawTimer.Stop();
                PauseBtn.Text = "繼續";
            }
            else if ("繼續".Equals(PauseBtn.Text))
            {
                DrawTimer.Start();
                PauseBtn.Text = "暫停";
            }
        }
        private void TrackWin_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Stop();
            }catch(Exception ex)
            {
                FileOperation.WriteLog("关闭轨迹窗体时出现异常，异常原因："+ex.ToString());
            }
        }
    }
}
