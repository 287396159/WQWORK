using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PrecisePositionLibrary;
using System.IO;
using System.Threading;
using Point = PrecisePositionLibrary.Point;
using System.Collections;
using System.Runtime.InteropServices;
namespace PrecisePosition
{

    public partial class HistoryrecordWin : Form
    {
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        private const string StrStart = "Start";
        private const string StrStop = "Stop";
        private const string StrPause = "Pause";
        private const string StrContinue = "Continue";
        private Bitmap CurBitMap = null;
        private double mapwid, mapheight;
        private double mrscale;
        private Form1 frm = null;
        private string StrMapPath = "";
        private object TraceLock = new object();
        private int SpeedRate = 8;

        private class Dxf
        {
            public static double scale = 1;
            public static double CenterX = 0;
            public static double CenterY = 0;

            public static double PanelCenterX = 0;
            public static double PanelCenterY = 0;
        }
        public HistoryrecordWin()
        {
            InitializeComponent();
        }
        public HistoryrecordWin(Form1 frm)
        {
            InitializeComponent();
            this.frm = frm;
        }
        private void HistoryrecordWin_Load(object sender, EventArgs e)
        {
            for(int i=0;i<24;i++)
            {shourTB.Items.Add(i); ehourTB.Items.Add(i);}
            for (int i = 0; i < 60; i++)
            {sminuteTB.Items.Add(i); eminuteTB.Items.Add(i);}
            shourTB.SelectedIndex = 0;ehourTB.SelectedIndex = 0;
            sminuteTB.SelectedIndex = 0;
            eminuteTB.SelectedIndex = eminuteTB.Items.Count - 1;
            ehourTB.SelectedIndex = ehourTB.Items.Count - 1;
            Dxf.scale = 1;
            pauseBtn.Text = StrPause;
            TraceCb.SelectedIndex = 0;
            //DrawPanel.MouseWheel += DrawPanel_MouseWheel;
            //设置窗体大小，高度和屏幕相同，宽度为屏幕的2/3;
            SetWinParam();
            //加载当前的地图
            StrMapPath = Ini.GetValue(Ini.ConfigPath,Ini.MapSeg,Ini.MapKey_Path);
            Dxf.PanelCenterX = Dxf.CenterX = DrawPanel.Width / 2;
            Dxf.PanelCenterY = Dxf.CenterY = DrawPanel.Height / 2;
            CurBitMap = DxfMapParam.GetDxfMap(StrMapPath, 1, Dxf.CenterX, Dxf.CenterY, DrawPanel.Width, DrawPanel.Height);
            DrawPanel_Paint(null,null);
            //计算出面板中地图的宽度与高度
            if (DrawPanel.Width / Parameter.RealWidth > DrawPanel.Height / Parameter.RealHeight)
            {
                mapheight = DrawPanel.Height;
                mapwid = mapheight * (Parameter.RealWidth / Parameter.RealHeight);
            }
            else
            {
                mapwid = DrawPanel.Width;
                mapheight = mapwid * (Parameter.RealHeight / Parameter.RealWidth);
            }
            //计算当前图片与面板的比例
            mrscale = (double)mapwid / Parameter.RealWidth;

            //判断当前是否是多区域模式
            if(Parameter.isSupportMulArea)
            {//在多区域模式下，无法查看轨迹，
                TraceCb.SelectedIndex = 0;
                TraceCb.Enabled = false;
            }
        }
        private void SetWinParam()
        {
            Rectangle sc = new Rectangle();
            sc = Screen.GetWorkingArea(sc);
            if (null == sc) return;
            if (sc.Width <= 0 || sc.Height <= 0) return;
            int w = sc.Width * 3 / 4;
            int h = sc.Height;
            choosetimebox.Width = (int)(w * 0.96);
            selectcardbox.Width = (int)(w * 0.96);
            operationbox.Width = (int)(w * 0.96);
            DrawPanel.Width = (int)(w * 0.96);
            this.Bounds = new Rectangle((sc.Width - w) / 2, 0, w, h);
            choosetimebox.Bounds = new Rectangle((w - choosetimebox.Width - 20) / 2, 5, choosetimebox.Width, choosetimebox.Height);
            selectcardbox.Bounds = new Rectangle((w - selectcardbox.Width - 20) / 2, choosetimebox.Location.Y + choosetimebox.Height + 5, selectcardbox.Width, selectcardbox.Height);
            operationbox.Bounds = new Rectangle((w - operationbox.Width - 20) / 2, selectcardbox.Location.Y + selectcardbox.Height + 5, operationbox.Width, operationbox.Height);
            
            DrawPanel.Bounds = new Rectangle((w - DrawPanel.Width - 20) / 2, operationbox.Location.Y + operationbox.Height + 5, operationbox.Width, operationbox.Height);
            DrawPanel.Height = h - DrawPanel.Location.Y - 40;
        }

        private void DrawPanel_Paint(object sender, PaintEventArgs e)
        {
            if (null != CurBitMap)
            {

                lock (TraceLock)
                {
                    DrawPanel.CreateGraphics().DrawImageUnscaled(CurBitMap, 0, 0);
                }
            }
            else
            {
                DrawPanel.CreateGraphics().Clear(Color.White);
            }
        }
        private int TraceMode = -1;//0:圆显示，1：线显示
        DateTime startdt, enddt;
        private void StartBtn_Click(object sender, EventArgs e)
        {
            string StrText = StartBtn.Text.Trim();
            if (StrText.Equals(StrStart))
            {
                SpeedRate = 8;
                //在调用程序开始时，先将地图上原来的轨迹删除掉
                if(!Parameter.isSupportMulArea)
                {
                    string StrMapPath = Ini.GetValue(Ini.ConfigPath, Ini.MapSeg, Ini.MapKey_Path);
                    CurBitMap = DxfMapParam.GetDxfMap(StrMapPath, 1, DrawPanel.Width / 2, DrawPanel.Height / 2, DrawPanel.Width, DrawPanel.Height);
                    DrawPanel_Paint(null, null);
                }
                StartTrace();
            }
            else 
            {
                StopTrace();
                pauseBtn.Text = StrPause;
            }
            Thread.Sleep(10);
        }
        /// <summary>
        /// 开始轨迹
        /// </summary>
        private void StartTrace()
        {
            //判断日期时间是否选择正确
            int start, end;
            int syear, smonth, sday, shour, sminute, eyear, emonth, eday, ehour, eminute;
            syear = StartTimePicker.Value.Year; smonth = StartTimePicker.Value.Month; sday = StartTimePicker.Value.Day;
            eyear = EndTimePicker.Value.Year; emonth = EndTimePicker.Value.Month; eday = EndTimePicker.Value.Day;
            string StrSHour, StrSMinute, StrEHour, StrEMinute;
            StrSHour = shourTB.Text.Trim(); StrSMinute = sminuteTB.Text.Trim();
            StrEHour = ehourTB.Text.Trim(); StrEMinute = eminuteTB.Text.Trim();
            string strtagid = TagTB.Text;
            if(Parameter.isSupportMulArea)
            {
                if (null == strtagid || "".Equals(strtagid))
                {
                    MessageBox.Show("Sorry, in multi-zone mode, the card ID cannot be empty!");
                    return;
                }
            }

            if ("".Equals(StrSHour))
            { 
                MessageBox.Show("The beginning hour cannot be empty！"); 
                return;
            }
            if ("".Equals(StrSMinute))
            { 
                MessageBox.Show("The beginning minute cannot be empty！");
                return;
            }
            if ("".Equals(StrEHour))
            { 
                MessageBox.Show("The end hour cannot be empty！");
                return;
            }
            if ("".Equals(StrEMinute))
            { 
                MessageBox.Show("The end minute cannot be empty！");
                return;
            }
            try
            {
                shour = Convert.ToInt32(StrSHour); ehour = Convert.ToInt32(StrEHour);
                sminute = Convert.ToInt32(StrSMinute); eminute = Convert.ToInt32(StrEMinute);
            }
            catch (Exception)
            { 
                MessageBox.Show("The hour or minute format is wrong!"); 
                return; 
            }
            if (shour < 0 || shour > 23 || sminute < 0 || sminute > 59 ||
                ehour < 0 || ehour > 23 || eminute < 0 || eminute > 59)
            { 
                MessageBox.Show("The range of the hour is 0-24, the range of the minute needle is 0-60!"); 
                return;
            }
            startdt = new DateTime(syear, smonth, sday, shour, sminute, 0);
            enddt = new DateTime(eyear, emonth, eday, ehour, eminute, 59);
            if (DateTime.Compare(startdt, enddt) > 0)
            { 
                MessageBox.Show("I'm sorry, the start time is earlier than the end！");
                return;
            }
            //读取记录文件
            if (!Directory.Exists(Parameter.RecordDir))
            {
                MessageBox.Show("Sorry, the record file doesn't exist！");
                return;
            }
            string[] dirs = Directory.GetDirectories(Parameter.RecordDir);
            string strdirname = "", strcuryear = "", strcurmonth = "", strcurday = "", strcurhour = "";
            int index = -1, curyear = -1, curmonth = -1, curday = -1, curhour = -1;
            DateTime curdt;
            Allpack.Clear();
            foreach (string dir in dirs)
            {
                index = dir.LastIndexOf("\\");
                strdirname = dir.Substring(index + 1, dir.Length - index - 1);
                strcuryear = strdirname.Substring(0, 4);
                strcurmonth = strdirname.Substring(4, 2);
                strcurday = strdirname.Substring(6, 2);
                try
                {
                    curyear = Convert.ToInt32(strcuryear);
                    curmonth = Convert.ToInt32(strcurmonth);
                    curday = Convert.ToInt32(strcurday);
                }catch (Exception)
                {
                    continue;
                }
                curdt = new DateTime(curyear, curmonth, curday, 23, 59, 59);
                //说明当前的年+月+日符合选择的时间
                string[] StrFiles = Directory.GetFiles(dir);
                foreach (string str in StrFiles)
                {
                    index=str.LastIndexOf("\\");
                    strcurhour=str.Substring(index + 1, str.Length - index - 5);
                    try
                    {
                        curhour=Convert.ToInt32(strcurhour);
                    }
                    catch(Exception)
                    {
                        continue;
                    }
                    curdt = new DateTime(curyear, curmonth, curday, curhour, startdt.Minute, 0);
                    if (DateTime.Compare(curdt, startdt) < 0)
                    {
                        continue;
                    }
                    curdt = new DateTime(curyear, curmonth, curday, curhour, enddt.Minute, 0);
                    if (DateTime.Compare(curdt, enddt) > 0)
                    {
                        continue;
                    }
                    //说明符合要求，将其序列化回来
                    Object obj = null;
                    frm.DeserializeObject(out obj, str);
                    if (null == obj)
                    {
                        continue;
                    }
                    List<CardImg> tpks = obj as List<CardImg>;
                    if (null == tpks)
                    {
                        continue;
                    }
                    //从当前的集合中找出时间头和时间尾，排除掉"分"不符合要求
                    start = GetDTHead(tpks, startdt);
                    end = GetDTEnd(tpks,enddt);
                    if (start < 0)
                    {
                        continue;
                    }
                    for (int i = start; i <= end; i++)
                    {
                        Allpack.Add(tpks[i]);
                    }
                }
            }
            if(Allpack.Count <= 0)
            {
                MessageBox.Show("Sorry, there is no record！");
                return;
            }
            if (TraceCb.SelectedIndex == 0)
            {
                TraceMode = 0;
            }
            else
            {
                TraceMode = 1;
            }
            //启动一个定时器在面板上显示历史Tag的位置
            StrTagInfor = TagTB.Text.Trim();
            if (!"".Equals(StrTagInfor))
            {
                bool isShow = false;
                foreach (CardImg tg in Allpack)
                {
                    if (StrTagInfor.Equals(tg.Name)) 
                    { 
                        isShow = true;
                        break;
                    }
                    if (StrTagInfor.Equals(tg.ID[0].ToString("X2") + tg.ID[1].ToString("X2"))) 
                    { 
                        isShow = true;
                        break;
                    }
                }
                if (!isShow) 
                { 
                    MessageBox.Show("Sorry,there are no tags of " + StrTagInfor); 
                    return;
                }
            }
            index_color = -1;
            isStop = true;
            TabSec = 1;
            if (null == ShowThread)
            {
                ShowThread = new Thread(ShowTag);
            }
            try
            {
                ShowThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            Speedratelabel.Text = "Speed rating： " + SpeedRate + " (level)";
            StartBtn.Text = StrStop;
            speedupBtn.Enabled = true;
            speeddownBtn.Enabled = true;
            pauseBtn.Enabled = true;
            TraceCb.Enabled = false;
            TagTB.Enabled = false;
        }

        //找出集合中时间头和时间尾,在当前集合中的年月日时都应该和开始时间相同
        public int GetDTHead(List<CardImg> tagimgs,DateTime dtstart)
        {
            for (int i = 0; i < tagimgs.Count;i++)
            {
                if (DateTime.Compare(tagimgs[i].ReceiTime, dtstart) < 0)
                {
                    if ((i + 1) >= tagimgs.Count)return -1;
                    else
                    {
                        if(DateTime.Compare(tagimgs[i+1].ReceiTime,dtstart) > 0)
                            return (i+1);
                    }
                }else return i;
            }
            return -1;
        }
        //找出集合中时间结尾的项
        public int GetDTEnd(List<CardImg> tagimgs,DateTime dtend)
        {
            for (int i = 0; i < tagimgs.Count;i++)
            {
                if (DateTime.Compare(tagimgs[i].ReceiTime, dtend) < 0)
                {
                    if ((i + 1) >= tagimgs.Count) return i;
                    else
                    { 
                        if(DateTime.Compare(tagimgs[i+1].ReceiTime,dtend) > 0)return i;
                    }
                }
                else return -1;
            }
            return -1;
        }
        string StrTagInfor = "";
        private void StopTrace()
        {
            StartBtn.Text = StrStart;
            TraceCb.Enabled = true; 
            TagTB.Enabled = true;
            speedupBtn.Enabled = false; 
            speeddownBtn.Enabled = false;
            pauseBtn.Enabled = false;
            isStop = false;
            if (null != ShowThread)
            {
                if (ShowThread.IsAlive)
                {
                    ShowThread.Abort();
                }
                ShowThread = null;
            }
            Application.DoEvents();
            Thread.Sleep(10);
            ClearTagPen();
            Ini.Close(Ini.CardPath);
        }
        //清除掉时间点相同，Tag的ID也相同的点(原因：每次增加1s，但是下位上报数据太快<1s时，会出现这个问题)
        private void DealAllpack(List<CardImg> curtags)
        {
            List<CardImg> deletes = new List<CardImg>();
            DateTime startDT, endDT;
            for (int i = 0; i < curtags.Count; i++)
            {
                startDT = new DateTime(curtags[i].ReceiTime.Year, curtags[i].ReceiTime.Month, curtags[i].ReceiTime.Day, curtags[i].ReceiTime.Hour, curtags[i].ReceiTime.Minute, curtags[i].ReceiTime.Second);
                for (int j = i + 1; j < curtags.Count; j++)
                {
                    //判断当前的两个时间是否是在同一个时间点上发生的
                    endDT = new DateTime(curtags[j].ReceiTime.Year, curtags[j].ReceiTime.Month, curtags[j].ReceiTime.Day, curtags[j].ReceiTime.Hour, curtags[j].ReceiTime.Minute, curtags[j].ReceiTime.Second);
                    if(DateTime.Compare(startDT,endDT)==0)
                    {
                        if (curtags[i].ID[0] == curtags[j].ID[0] && curtags[i].ID[1] == curtags[j].ID[1])
                        {
                            deletes.Add(curtags[j]);
                        }
                    }
                }
            }
            for(int k =0 ;k < deletes.Count;k++)curtags.Remove(deletes[k]);
        }
        List<CardImg> Allpack = new List<CardImg>();
        private Thread ShowThread = null;
        private bool isStop = false;
        private double TabSec = 1;
        private void ShowTag()
        {
            try
            {
                Dictionary<string, CardImg> dictags = new Dictionary<string,CardImg>();
                string StrID = "";
                int index = 0;
                //String strgroupid = "";
                while(isStop)
                {
                    if (DateTime.Compare(startdt,enddt) >= 0)
                    {//达到设置的最终时间后，停止显示
                        isStop = false;
                        this.Invoke(new Action(() =>
                        { 
                            StopTrace(); 
                        }));
                        break;
                    }
                    Thread.Sleep((int)(TabSec * 1000));
                    this.Invoke(new Action(() => 
                    {
                        label8.Text = "The current time is " + startdt.ToString();
                    }));
                    //Allpack是所有记录讯息
                    for (int i = index; i < Allpack.Count;i++)
                    {
                        if (DateTime.Compare(startdt, Allpack[i].ReceiTime) < 0)
                        {//此时时间早于数据包上报的时间
                            index = i;
                            break;
                        }
                        StrID = Allpack[i].ID[0].ToString("X2") + Allpack[i].ID[1];
                        CardImg tagimg = null;
                        if (dictags.TryGetValue(StrID, out tagimg))
                        {
                            tagimg.CardPoint = Allpack[i].CardPoint;
                            tagimg.isShowImg = Allpack[i].isShowImg;
                            tagimg.LocaType = Allpack[i].LocaType;
                            tagimg.isOverNoMove = Allpack[i].isOverNoMove;
                            tagimg.isLowBattery = Allpack[i].isLowBattery;
                            System.Buffer.BlockCopy(Allpack[i].GroupID, 0, tagimg.GroupID,0,2);
                            
                        }
                        else
                        {
                            dictags.Add(StrID, (CardImg)Form1.CopyObject(Allpack[i]));
                        }
                    }
                    if(!Parameter.isSupportMulArea)
                    {
                        if (TraceMode == 0)
                        {//以坐标进行显示时每次清楚地图
                            if (!Parameter.isSupportMulArea)
                            {
                                CurBitMap = DxfMapParam.GetDxfMap(StrMapPath, Dxf.scale, DrawPanel.Width / 2, DrawPanel.Height / 2, DrawPanel.Width, DrawPanel.Height);
                                if (null == CurBitMap) continue;
                                else DrawPanel_Paint(null, null);
                            }
                        }
                    }
                    DrawTagNode(dictags);
                    try
                    {
                        startdt = startdt.AddSeconds(1);
                    }
                    catch (Exception)
                    {
                        
                    }
                }
                
            }
            catch (System.Threading.ThreadAbortException)
            {
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }
        private void DrawTagNode(Dictionary<string,CardImg> tpks)
        {
            if (!Parameter.isSupportMulArea)
            {//不是多区域时，只有一张地图
                if (null == CurBitMap)
                {
                    return;
                }
            }
            Graphics g = null;
            try
            {
                if (!Parameter.isSupportMulArea)
                {
                    g = Graphics.FromImage(CurBitMap);
                }
            }
            catch (System.InvalidOperationException)
            {
                return;
            }
            float wscale = 0.0f, hscale = 0.0f;
            if (!Parameter.isSupportMulArea)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            }
            Brush tagbrush = Brushes.Green;
            TagPen linetagpen = null; Pen linepen;
            System.Drawing.PointF[] points = null;
            double mapx = 0.0d, mapy = 0.0d, panelx = 0.0d, panely = 0.0d;
            string strtagid = "",strgroupid = "";
            foreach (CardImg tpk in tpks.Values)
            {
               strtagid = tpk.ID[0].ToString("X2") + tpk.ID[1].ToString("X2");
               if (tpk.Name == null || "".Equals(tpk.Name))
               {
                   tpk.Name = Ini.GetValue(Ini.CardPath, strtagid, Ini.Name);
               }
               if (!"".Equals(StrTagInfor) && !StrTagInfor.Equals(tpk.Name) && !StrTagInfor.Equals(tpk.ID[0].ToString("X2") + tpk.ID[1].ToString("X2")))
               {
                   continue;
               }
               if (Parameter.isSupportMulArea)
               {//支持多区域
                   strgroupid = tpk.GroupID[0].ToString("X2") + tpk.GroupID[1].ToString("X2");
                   Group group = null;
                   if (!frm.Groups.TryGetValue(strgroupid, out group))
                   {
                       continue;
                   }
                   DxfMapParam.ClearDxf();
                   CurBitMap = DxfMapParam.GetDxfMap(group.grouppath, Dxf.scale, DrawPanel.Width / 2, DrawPanel.Height / 2, DrawPanel.Width, DrawPanel.Height);
                   if (null == CurBitMap)
                   {
                       continue;
                   }
                   DrawPanel.CreateGraphics().DrawImageUnscaled(CurBitMap, 0, 0);
                   g = Graphics.FromImage(CurBitMap);
                   wscale = (float)DrawPanel.Width / group.actualwidth;
                   hscale = (float)DrawPanel.Height / group.actualheight;
                   mrscale = wscale > hscale ? hscale : wscale;
                   mapwid = group.actualwidth * mrscale;
                   mapheight = group.actualheight * mrscale;
               }
               // 计算Tag在地图上的坐标
               mapx = mrscale * tpk.CardPoint.x;
               mapy = mrscale * tpk.CardPoint.y;
               // Tag在面板中的坐标
               panelx = (DrawPanel.Width - mapwid) / 2 + mapx;
               panely = (DrawPanel.Height - mapheight) / 2 + mapy;
               if (tpk.LocaType == TPPID.TagCommonLocal)
               {
                   tagbrush = Brushes.Green;
               }
               if (tpk.isOverNoMove)
               {
                   tagbrush = Brushes.Black;
               }
               if (tpk.isLowBattery)
               {
                   tagbrush = Brushes.Indigo;
               }
               if (tpk.LocaType == TPPID.TagWarmLocal)
               {
                   tagbrush = Brushes.Red;
               }
               if (TraceMode == 0)
               {
                   if (panelx <= 0 || panely <= 0)
                   {
                       continue;
                   }
                   g.FillEllipse(tagbrush, (float)(panelx - 4), (float)panely - 4, 8, 8);
                   if (null == tpk.Name || "".Equals(tpk.Name))
                   {
                       g.DrawString(tpk.ID[0].ToString("X2") + tpk.ID[1].ToString("X2"), new Font("宋体", 12), Brushes.Red, (float)(panelx + 4), (float)(panely + 6));
                   }
                   else
                   {
                       g.DrawString(tpk.Name + "(" + tpk.ID[0].ToString("X2") + tpk.ID[1].ToString("X2") + ")", new Font("宋体", 12), Brushes.Red, (float)(panelx + 4), (float)(panely + 6));
                   }
               }
               else
               {
                   linetagpen = GetTagPen(strtagid);
                   if (null == linetagpen)
                   {
                       if (AllTagPens.Count >= 6)
                       {
                           continue;
                       }
                       linetagpen = new TagPen();
                       System.Buffer.BlockCopy(tpk.ID, 0, linetagpen.ID, 0, 2);
                       linepen = Get6Color();
                       linetagpen.tagpen = linepen;
                       linetagpen.Name = tpk.Name;
                       AllTagPens.Add(linetagpen);
                       if (panelx >= 0 && panely >= 0)
                       {
                           linetagpen.oldx = panelx;
                           linetagpen.oldy = panely;
                       }
                   }
                   else
                   {
                       if (linetagpen.oldx <= 0 || linetagpen.oldy <= 0)
                       {
                           linetagpen.oldx = panelx;
                           linetagpen.oldy = panely;
                           return;
                       }
                       if (panelx > 0 && panely > 0)
                       {
                           if (null == points)
                           {
                               points = new PointF[2];
                           }
                           points[0] = new System.Drawing.PointF((float)linetagpen.oldx, (float)linetagpen.oldy);
                           points[1] = new System.Drawing.PointF((float)panelx, (float)panely);
                           try
                           {
                               g.DrawCurve(linetagpen.tagpen, points);
                           }
                           catch (System.OutOfMemoryException ex)
                           {
                               FreeMemory();
                               Console.WriteLine("内存不足!" + ex.ToString());
                           }
                       }
                       linetagpen.oldx = panelx; linetagpen.oldy = panely;
                   }
               }
               if (Parameter.isSupportMulArea)
               {
                   if (null == CurBitMap) return;
                   DrawPanel_Paint(null, null);
               }
            }
            if (!Parameter.isSupportMulArea)
            {
                if (null == CurBitMap) return;
                DrawPanel_Paint(null, null);
            }
            if (TraceMode == 1)
            {
              if (null == AllTagPens)return;
              if (AllTagPens.Count <= 0)return;
              int logox1 = 15,logoy1 = 0,logox2 = 45,logoy2 = 0;
              for (int i = 0; i < AllTagPens.Count;i++)
              {
                  if (null == AllTagPens[i])continue;
                  if (AllTagPens[i].isLogo)continue;
                  logoy1 = logoy2 = (i + 1) * 25;
                  if (null == AllTagPens[i].tagpen)continue;
                  g.DrawLine(AllTagPens[i].tagpen,new System.Drawing.Point(logox1, logoy1),new System.Drawing.Point(logox2,logoy2));
                  if (null == AllTagPens[i].Name || "".Equals(AllTagPens[i].Name))g.DrawString(AllTagPens[i].ID[0].ToString("X2") + AllTagPens[i].ID[1].ToString("X2"), new Font("宋体", 12), AllTagPens[i].tagpen.Brush, logox2 + 5, logoy2 - 8);
                  else g.DrawString(AllTagPens[i].Name + "(" + AllTagPens[i].ID[0].ToString("X2") + AllTagPens[i].ID[1].ToString("X2") + ")", new Font("宋体", 12), AllTagPens[i].tagpen.Brush, logox2 + 5, logoy2 - 8);
                  AllTagPens[i].isLogo = true;
              }
            }
            if (null != g)
            {
                g.Dispose();
            }
        }
        List<TagPen> AllTagPens = new List<TagPen>();
        private TagPen GetTagPen(string StrTagID)
        {
            foreach (TagPen tp in AllTagPens)
            {
                if (null == tp)
                {
                    continue;
                }
                string strid = tp.ID[0].ToString("X2") + tp.ID[1].ToString("X2");
                if (strid.Equals(StrTagID))
                {
                    return tp;
                }
            }
            return null;
        }
        private void FreeMemory()
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            if(Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle,-1,-1);
            }
        }
        private void ClearTagPen()
        {
            AllTagPens.Clear();
        }
        private static int index_color = -1;
        private Pen Get6Color()
        {
            index_color++;
            switch (index_color%6)
            {
                case 0: return Pens.Red;
                case 1: return Pens.Green;
                case 2: return Pens.Blue;
                case 3: return Pens.Orange;
                case 4: return Pens.Gray;
                case 5: return Pens.Yellow;
                default:return Pens.DarkRed;
            }
        }
       
        /// <summary>
        /// 获取画笔
        /// </summary>
        /// <returns></returns>
        private Pen GetOther()
        {
            Random rdR = new Random(Guid.NewGuid().GetHashCode());
            Random rdG = new Random(Guid.NewGuid().GetHashCode());
            Random rdB = new Random(Guid.NewGuid().GetHashCode());
            int rr = rdR.Next(255);
            int rg = rdG.Next(255);
            int rb = rdB.Next(255);
            return GetDarkPen(Color.FromArgb(rr, rg, rb),3);
        }
        /// <summary>
        /// 获得加深的颜色画笔
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public Pen GetDarkPen(Color color,int width)
        {
            const int max = 255;
            int increase = new Random(Guid.NewGuid().GetHashCode()).Next(30,255);
            int r = Math.Abs(Math.Min(color.R - increase,max));
            int g = Math.Abs(Math.Min(color.G - increase, max));
            int b = Math.Abs(Math.Min(color.B - increase, max));
            return new Pen(Color.FromArgb(r, g, b), width);
        }
        /// <summary>
        /// 获取指定时间的Tag
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="curtags"></param>
        private void GetDateTimeTag(DateTime dt, out Dictionary<string,CardImg> curtags)
        {
            int index = 0;
            DateTime curtagdt;
            curtags = new Dictionary<string,CardImg>();
            while (Allpack.Count > 0 && index < Allpack.Count)
            {
                curtagdt = new DateTime(Allpack[index].ReceiTime.Year,Allpack[index].ReceiTime.Month,Allpack[index].ReceiTime.Day,Allpack[index].ReceiTime.Hour,Allpack[index].ReceiTime.Minute,Allpack[index].ReceiTime.Second);
                if (DateTime.Compare(dt, curtagdt) == 0)
                {
                    CardImg tag = null;
                    string strid = Allpack[index].ID[0].ToString("X2") + Allpack[index].ID[1].ToString("X2");
                    if (curtags.TryGetValue(strid, out tag))
                    {
                        tag = (CardImg)Form1.CopyObject(Allpack[index]);
                    }
                    else
                    {
                        curtags.Add(strid, Allpack[index]);
                    }
                }
                else if (DateTime.Compare(curtagdt, dt) < 0) Allpack.RemoveAt(index);
                else return;
                index++;
            }
            return;
        }
        /// <summary>
        /// 加速播放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void speedupBtn_Click(object sender, EventArgs e)
        {
            if (TabSec > 0.2) { TabSec -= 0.2; SpeedRate++;}
            else MessageBox.Show("It has speeded up to the maximum speed！");
            Speedratelabel.Text = "Speed rating： " + SpeedRate + " (level)";
        }
        /// <summary>
        /// 减速播放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void speeddownBtn_Click(object sender, EventArgs e)
        {

            if (TabSec > 2.2) { MessageBox.Show("It has speeded up to the minimum speed！"); return; }
            TabSec += 0.2;
            SpeedRate--;
            Speedratelabel.Text = "Speed rating： " + SpeedRate + " (level)";
        }
        /// <summary>
        /// 暂停播放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pauseBtn_Click(object sender, EventArgs e)
        {
            if (pauseBtn.Text.Trim().Equals(StrPause))
            {
                //暂停
                isStop = false;
                //Thread.Sleep((int)(TabSec * 100) + 100);
                try
                {
                    if (null != ShowThread)
                    {
                        ShowThread.Abort();
                    }
                }catch(Exception){ }
                ShowThread = null;
                pauseBtn.Text = StrContinue;
            }
            else if(pauseBtn.Text.Trim().Equals(StrContinue))
            {
                isStop = true;
                try
                {
                    if (null == ShowThread) ShowThread = new Thread(ShowTag);
                    ShowThread.Start();
                }catch (Exception) { }
                //继续执行
                pauseBtn.Text = StrPause;
            }
        }
        private void HistoryrecordWin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (StrStop.Equals(StartBtn.Text.Trim()))
            {
                StopTrace();
            }
        }
    }
    class TagPen
    {
      public byte[] ID = new byte[2];
      public string Name = "";
      public Pen tagpen = null;
      public double oldx, oldy;
      public bool isLogo = false;
    }
}
