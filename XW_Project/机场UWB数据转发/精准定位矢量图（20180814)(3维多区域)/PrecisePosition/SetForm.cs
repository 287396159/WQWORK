using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using WW.Cad.Model;
using WW.Cad.Drawing.GDI;
using WW.Math;
using WW.Cad.IO;
using WW.Cad.Base;
using WW.Cad.Drawing;
using Color = System.Drawing.Color;
using PosititionMode = PrecisePositionLibrary.PrecisePosition.PosititionMode;
namespace PrecisePosition
{
    public partial class SetForm : Form
    {
        Form1 frm = null;
        bool isListBoxTab1,isListBoxTab2;
        private Bitmap DxfMap = null;
        private Bitmap smallmap = null;

        public SetForm()
        {
            InitializeComponent();
        }
        public SetForm(Form1 frm) {
            InitializeComponent();
            this.frm = frm;
        }
        private void Setindex_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Setindex_listBox.SelectedIndex)
            {
                case 0:
                    Net_panel.Location = new Point(148, 13);
                    Net_panel.Visible = true;
                    Map_panel.Visible = false;
                    Show_panel.Visible = false;
                    Alarm_panel.Visible = false;
                    mulareapanel.Visible = false;
                    string HostNameStr = Dns.GetHostName();
                    IPAddress[] ips = Dns.GetHostAddresses(HostNameStr);
                    IP_ComboBox.Items.Clear();
                    jsonComboBox.Items.Clear();
                    comboBox1.Items.Clear();
                    
                    foreach (IPAddress ip in ips)
                    {
                        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            IP_ComboBox.Items.Add(ip.ToString());
                            jsonComboBox.Items.Add(ip.ToString());
                            comboBox1.Items.Add(ip.ToString());
                        }
                    }
                    IP_ComboBox.SelectedIndex = 0;
                    jsonComboBox.SelectedIndex = 0;
                    comboBox1.SelectedIndex = 0;
                    break;
                case 1:
                    if (Parameter.isSupportMulArea)
                    {//支持多区域
                        mulareapanel.Visible = true;
                        Net_panel.Visible = false;
                        Map_panel.Visible = false;
                        Show_panel.Visible = false;
                        Alarm_panel.Visible = false;
                        mulareapanel.Location = new Point(148, 13);
                        //重新把集合中的组别加载到列表中
                        UpdateCollectionToList();
                    }
                    else
                    {
                        Map_panel.Location = new Point(148, 13);
                        Net_panel.Visible = false;
                        Map_panel.Visible = true;
                        Show_panel.Visible = false;
                        Alarm_panel.Visible = false;
                        String StrPath = Ini.GetValue(Ini.ConfigPath, Ini.MapSeg, Ini.MapKey_Path);
                        if (null != StrPath && !"".Equals(StrPath))
                        {
                            MapPath_TextBox.Text = StrPath;
                            DxfMap = DxfMapParam.GetDxfMap(StrPath, 1, DrawMap_Panel.Width / 2, DrawMap_Panel.Height / 2, DrawMap_Panel.Width, DrawMap_Panel.Height);
                            DrawMap_Panel_Paint(null, null);
                        }
                    }
                    break;
                case 2:
                    isListBoxTab1 = true;
                    Show_panel.Location = new Point(148, 13);
                    Net_panel.Visible = false;
                    Map_panel.Visible = false;
                    Show_panel.Visible = true;
                    Alarm_panel.Visible = false;
                    mulareapanel.Visible = false;
                    if (Parameter.Px_Dis > 0)
                    {
                        RealWidth_textBox.Text = Parameter.Px_Dis.ToString();
                    }
                    if (Parameter.NoShow_OverTime_NoRecei)
                    {
                        NoShow_OverTime_NoRecei_checkBox.Checked = Parameter.NoShow_OverTime_NoRecei;
                        OverTime1_textBox.Text = Parameter.OverTime1.ToString();
                        OverTime1_textBox.Enabled = false;
                    }
                    if (Parameter.OverTime1 > 0)
                    {
                        OverTime1_textBox.Text = Parameter.OverTime1.ToString();
                    }
                    ShowPlacePort_checkBox.Checked = Parameter.ShowPlacePort;
                    LongTime_NoExe_ToBlackShow_checkBox.Checked = Parameter.LongTime_NoExe_ToBlackShow;
                    if (Parameter.OverTime2 > 0)
                    {
                        OverTime2_textBox.Text = Parameter.OverTime2.ToString();
                        OverTime2_textBox.Enabled = false;
                    }
                    isListBoxTab1 = false;
                    if (Parameter.isShowTrace)
                    {
                        ShowTraceCB.Checked = true;
                    }
                    else
                    {
                        ShowTraceCB.Checked = false;
                    }
                    if (Parameter.TagShowOver)
                    {
                        TagShowCB.Checked = true;
                    }
                    else
                    {
                        TagShowCB.Checked = false;
                    }
                    if (Parameter.isEnableReferType)
                    {
                        refretypecb.Checked = true;
                    }
                    else
                    {
                        refretypecb.Checked = false;
                    }
                    if (Parameter.isKalman)
                    {
                        MNosieCovarTB.Text   = Parameter.KalmanMNosieCovar + "";
                        ProNosieCovarTB.Text = Parameter.KalmanProNosieCovar + "";
                        LastStatePreTB.Text  = Parameter.KalmanLastStatePre + "";
                        kalmanCB.Checked     = true;
                    }
                    else
                    {
                        kalmanCB.Checked     = false;
                    }
                    if (kalmanCB.Checked)
                    {
                        LastStatePreTB.Enabled = false;
                    }
                    else
                    {
                        LastStatePreTB.Enabled = true;
                    }
                    SupportMulAreaCB.CheckedChanged -= SupportMulAreaCB_CheckedChanged;
                    if (Parameter.isSupportMulArea)
                    {
                        SupportMulAreaCB.Checked = true;
                        posimodecb.SelectedIndex = 1;
                        posimodecb.Enabled = false;
                    }
                    else
                    {
                        SupportMulAreaCB.Checked = false;
                    }
                    SupportMulAreaCB.CheckedChanged += SupportMulAreaCB_CheckedChanged;
                    if (Parameter.isClearHistory)
                    {
                        ClearTimeTab.Text    = Parameter.ClearHistoryTime + "";
                        ClearCb.Checked      = true;
                        ClearTimeTab.Enabled = false;
                    }
                    else
                    {
                        ClearCb.Checked = false;
                    }
                    refreshmscb.Text = Parameter.DefineInterval + "";
                    if (Parameter.isDefineInterval)
                    {
                        showrefrecb.Checked = true;
                    }
                    else
                    {
                        showrefrecb.Checked = false;
                    }
                    if (Parameter.positionmode == PosititionMode.SigQuality)
                    {
                        posimodecb.SelectedIndex = 0;
                    }
                    else
                    {
                        posimodecb.SelectedIndex = 1;
                    }
                    //采用3点定位
                    if (Parameter.isUse3Station)
                    {
                        IsUse3SCb.Checked = true;
                    }
                    else
                    {
                        IsUse3SCb.Checked = false;
                    }
                    if (Directory.Exists(Ini.AreaMapPath))
                    {
                        //获取所有地图的名称
                        string[] StrMaps = Directory.GetFiles(Ini.AreaMapPath);
                        //得到当前单区域地图
                        string CurMap = Ini.GetValue(Ini.ConfigPath, Ini.MapSeg, Ini.MapKey_Path);
                        string strname = "";
                        int start = -1;
                        bool mark = false;
                        foreach (string strmap in StrMaps)
                        {
                            if (null == strmap)
                            {
                                continue;
                            }
                            start = strmap.LastIndexOf("\\");
                            strname = strmap.Substring(start + 1, strmap.Length - start - 1);
                            if (strname.Equals(CurMap))
                                continue;
                            //查看多区域的集合中是否有包含当前地图的
                            mark = false;
                            foreach (KeyValuePair<string, Group> group in frm.Groups)
                            {
                                if (null == group.Value)
                                    continue;
                                if (group.Value.grouppath.Equals(strname))
                                {//说明多区域集合中也需要当前的地图，不能删除
                                    mark = true;
                                    break;
                                }
                            }
                            if (!mark)
                            {
                                break;
                            }
                        }
                        if (!mark)
                        {
                            ClearcacheMapBtn.Enabled = true;
                        }
                        else
                        {
                            ClearcacheMapBtn.Enabled = false;
                        }
                    }
                    break;
                case 3:
                    isListBoxTab2 = true;
                    Alarm_panel.Location = new Point(148, 13);
                    Net_panel.Visible = false;
                    Map_panel.Visible = false;
                    Show_panel.Visible = false;
                    Alarm_panel.Visible = true;
                    mulareapanel.Visible = false;
                    overtimetb.Text = Parameter.OverNoReceiveWarmTime + "";
                    RecordOverTimeNoReceiInfo_checkBox.Checked = Parameter.RecordOverTimeNoReceiInfo;
                    if (Parameter.RecordBatteryLessCard)
                    {
                        LowBattry_textBox.Enabled = false;
                    }
                    RecordBatteryLessCardcheckBox.Checked = Parameter.RecordBatteryLessCard;
                    enzonealarmcb.Checked = Parameter.isEnableLimitArea;
                    if (Parameter.LowBattry > 0)
                    {
                        LowBattry_textBox.Text = Parameter.LowBattry.ToString();
                    }
                    isListBoxTab2 = false;
                    break;
            }
        }
        private void SaveIP_Btn_Click(object sender, EventArgs e)
        {
            String Value = IP_ComboBox.Text;
            saveFileValue(Value, Ini.NetSeg, Ini.NetKey_IP,"IP Adress");
        }

        private bool saveFileValue(String Value, String segment, String key,String mes) 
        {
            //文件路径
            if (Value.Equals("") || Value == null)
            {
                MessageBox.Show(mes +" cannot be empty！");
                return false;
            }
            if (!Ini.SetValue(Ini.ConfigPath, segment, key, Value))
            {
                MessageBox.Show("save "+mes + " to fail！");
                return false;
            }
            return true;
        }

        private void SeleMap_button_Click(object sender, EventArgs e)
        {
            //选择地图路径
            OpenFileDialog MyFileDialog = new OpenFileDialog();
            MyFileDialog.Filter = "All(.dxf)|*.dxf;*.dwg;";
            if (MyFileDialog.ShowDialog() == DialogResult.OK)
            {
                //将Map路径保存到配置文件中
                String MapPath = MyFileDialog.FileName;
                //将当前的图片文件保存到复制一份保存到指定的文件夹下
                if (!Ini.CopyFileToDir(MapPath, Ini.AreaMapPath))
                {
                    MessageBox.Show("File save failure！");
                    return;
                }
                string StrFileName = Ini.GetFileName(MapPath);
                MapPath_TextBox.Text = Ini.AreaMapPath + StrFileName;
                if (StrFileName.Equals("") || StrFileName == null)
                {
                    MessageBox.Show("Map the path cannot be empty！");
                    return;
                }
                if (Ini.SetValue(Ini.ConfigPath, Ini.MapSeg, Ini.MapKey_Path, StrFileName))
                {//开始在面板中画出Dex的图形
                    DxfMapParam.ClearDxf();
                    DxfMap = DxfMapParam.GetDxfMap(StrFileName, 1, DrawMap_Panel.Width / 2, DrawMap_Panel.Height / 2, DrawMap_Panel.Width, DrawMap_Panel.Height);
                    DrawMap_Panel_Paint(null, null);
                    DxfMapParam.scale = 1;
                    DxfMapParam.CenterX = frm.Map_panel.Width / 2;
                    DxfMapParam.CenterY = frm.Map_panel.Height / 2;
                    frm.StrMapPath = StrFileName;
                    frm.LoadDxfMap();
                    frm.Map_panel_Paint(null, null);
                }
                else
                {
                    MessageBox.Show("Save failed map path！");
                }
            }
        }
        private void DrawMap_Panel_Paint(object sender, PaintEventArgs e)
        {
            if (null != DxfMap)
            {
                DrawMap_Panel.CreateGraphics().DrawImageUnscaled(DxfMap, 0, 0);
            }
            else
            {
                DrawMap_Panel.CreateGraphics().Clear(Color.White);
            }
        }
  
        private void Save_P_Btn_Click(object sender, EventArgs e)
        {
            String Value = Port_TextBox.Text;
            int Port = 0;
            //文件路径
            if (Value.Equals("") || Value == null)
            {
                MessageBox.Show("Port can't be empty！");
                return;
            }
            try
            {
                Port = Convert.ToInt32(Value);
            }
            catch (Exception)
            {
                MessageBox.Show("Port format is wrong！");
                return;
            }
            if (Port > 65535 || Port < 1024)
            {
                MessageBox.Show("Port is greater than 1024 and less than or equal to 65535！");
            }
            if (!Ini.SetValue(Ini.ConfigPath, Ini.NetSeg, Ini.NetKey_Port, Value))
            {
               MessageBox.Show("Set port failure！");
            }
        }
        private void NoShow_OverTime_NoRecei_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (isListBoxTab1)
            {
                return;
            }
            if (NoShow_OverTime_NoRecei_checkBox.Checked)
            {
                String StrOverTime = OverTime1_textBox.Text;
                if (StrOverTime.Equals("") || StrOverTime == null)
                {
                    MessageBox.Show("Timeout time cannot be empty！");
                    NoShow_OverTime_NoRecei_checkBox.Checked = false;
                    return;
                }
                int OverTime = 0;
                try
                {
                    OverTime = Convert.ToInt32(StrOverTime);
                }
                catch (Exception)
                {
                    MessageBox.Show("Timeout time format is wrong！");
                    NoShow_OverTime_NoRecei_checkBox.Checked = false;
                    return;
                }
                if (OverTime < 0 || OverTime > 255)
                {
                    MessageBox.Show("Timeout time should not less than zero or greater than 255！");
                    NoShow_OverTime_NoRecei_checkBox.Checked = false;
                    return;
                }

                Parameter.OverTime1 = OverTime;
                Parameter.NoShow_OverTime_NoRecei = true;
                OverTime1_textBox.Enabled = false;
            }
            else
            {
                Parameter.NoShow_OverTime_NoRecei = false;
                OverTime1_textBox.Enabled = true;
            }
        }
        private void SetForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            if(!Parameter.isSupportMulArea)
            {
                String StrPath = Ini.GetValue(Ini.ConfigPath, Ini.MapSeg, Ini.MapKey_Path);
                if (StrPath == null || "".Equals(StrPath))
                {
                    return;
                }
                string StrWidth = RealWidth_textBox.Text;
                string StrHeight = RealHeight_textBox.Text;
                if (StrWidth.Equals("") || StrWidth == null)
                {
                    return;
                }
                if (StrHeight.Equals("") || StrHeight == null)
                {
                    return;
                }
                try
                {
                    Parameter.RealWidth = Convert.ToDouble(StrWidth);
                    Parameter.RealHeight = Convert.ToDouble(StrHeight);
                }
                catch (Exception)
                {
                    MessageBox.Show("Images of the actual width and height format is wrong！");
                    return;
                }
                //将图片的实际宽度和高度保存起来
                if (!Ini.SetValue(Ini.ConfigPath, Ini.MapSeg, Ini.RealWidth, StrWidth) || !Ini.SetValue(Ini.ConfigPath, Ini.MapSeg, Ini.RealHeight, StrHeight))
                {
                    MessageBox.Show("Save the pictures I actual width and height of the failure！");
                    return;
                }
            }


            if (Parameter.ShowPlacePort)
            {
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.ShowReferKey, "True");
            }
            else
            {
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.ShowReferKey, "False");
            }

            if (Parameter.LongTime_NoExe_ToBlackShow)
            {
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.LongTime_NoExeKey, "True");
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.NoExeTime, Parameter.OverTime2.ToString());
            }
            else
            {
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.LongTime_NoExeKey, "False");
            }
            if (Parameter.NoShow_OverTime_NoRecei)
            {
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.TimeOutNoShowKey, "True");
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.OutTime, Parameter.OverTime1.ToString());
            }
            else
            {
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.TimeOutNoShowKey, "False");
            }

            if (Parameter.TagShowOver)
            {
                Ini.SetValue(Ini.ConfigPath, Ini.TagShowOver, Ini.TagShowOverKey, "True");
            }
            else
            {
                Ini.SetValue(Ini.ConfigPath, Ini.TagShowOver, Ini.TagShowOverKey, "False");
            }
            if (Parameter.isEnableReferType)
            {
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.isEnableReferType, "True");
            }
            else
            {
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.isEnableReferType, "False");
            }
            //保存刷新时间
            if (Parameter.isDefineInterval)
            {
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.ShowRefreshKey, "True");
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.ShowRefreshTime, Parameter.DefineInterval.ToString());
            }
            else
            {
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.ShowRefreshKey, "False");
            }
            if (kalmanCB.Checked)
            {
                string StrMnosiecovar = MNosieCovarTB.Text;
                try
                {
                    Parameter.KalmanMNosieCovar = Convert.ToDouble(StrMnosiecovar);
                }
                catch (Exception)
                { 
                    Parameter.KalmanMNosieCovar = 0.1;
                }
                string StrProNosieCovar = ProNosieCovarTB.Text;
                try
                {
                    Parameter.KalmanProNosieCovar = Convert.ToDouble(StrProNosieCovar);
                }
                catch (Exception)
                { 
                    Parameter.KalmanProNosieCovar = 0.2;
                }
                string StrLastStatePre = LastStatePreTB.Text;
                try
                {
                    Parameter.KalmanLastStatePre = Convert.ToDouble(StrLastStatePre);
                }
                catch (Exception)
                { 
                    Parameter.KalmanLastStatePre = 0.5;
                }
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.isKalmanKey, "True");
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.MNosieCovar, Parameter.KalmanMNosieCovar + "");
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.ProNosieCovar, Parameter.KalmanProNosieCovar + "");
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.LastStatePre, Parameter.KalmanLastStatePre + "");
            }
            else
            {
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.isKalmanKey, "False");
            }
            //保存是否采用多地图模式
            if (Parameter.isSupportMulArea)
            {
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.EnableMulAreaMode, "True");
            }
            else
            {
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.EnableMulAreaMode, "False");
            }
            //每次从设置界面退出时重新加载图形信息
            //保存区域讯息
            if (Parameter.isSupportMulArea)
            {
                frm.SaveMulAreas();
            }
            else
            {
                frm.SavePorts();
            }
            //使用3个基站定位
            if (Parameter.isUse3Station)
            {
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.Use3Place, "True");
            }
            else
            {
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.Use3Place, "False");
            }
            //重新计算Img_RealDisRelation
            if (Parameter.RealWidth != 0 && Parameter.RealWidth != 0)
            {
                float wscale = (float)Map_panel.Width / (float)Parameter.RealWidth;
                float hscale = (float)Map_panel.Height / (float)Parameter.RealHeight;
                frm.Img_RealDisRelation = wscale > hscale ? hscale : wscale;
            }

            frm.RefreshAreaUI();
            frm.LoadDxfMap();
        }
        private void ShowPlacePort_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            // 是否显示位置参考点
            if (ShowPlacePort_checkBox.Checked)Parameter.ShowPlacePort = true;
            else Parameter.ShowPlacePort = false;
        }
        private void LongTime_NoExe_ToBlackShow_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (isListBoxTab1)
            {
                return;
            }
            //长时间不移动的卡片切换到黑色显示
            if (LongTime_NoExe_ToBlackShow_checkBox.Checked)
            {
                string StrLongTime_NoExeOverTime = OverTime2_textBox.Text;
                if (StrLongTime_NoExeOverTime.Equals(""))
                {
                    MessageBox.Show("Please set the timeout！");
                    LongTime_NoExe_ToBlackShow_checkBox.Checked = false;
                    return;
                }
                try
                {
                    Parameter.OverTime2 = Convert.ToInt32(StrLongTime_NoExeOverTime);
                }
                catch (Exception)
                {
                    MessageBox.Show("Timeout time format is wrong！");
                    LongTime_NoExe_ToBlackShow_checkBox.Checked = false;
                    return;
                }
                if (Parameter.OverTime2 < 0 || Parameter.OverTime2 > 255)
                {
                    MessageBox.Show("Timeout time is not less than zero and cannot be less than 255！");
                    LongTime_NoExe_ToBlackShow_checkBox.Checked = false;
                    return;
                }
                Parameter.LongTime_NoExe_ToBlackShow = true;
                OverTime2_textBox.Enabled = false;
            }
            else
            {
                Parameter.LongTime_NoExe_ToBlackShow = false;
                OverTime2_textBox.Enabled = true;
            }
        }
        private void RecordBatteryLessCardcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (isListBoxTab2)
            {
                return;
            }
            string StrLowBattry = "";
            if (RecordBatteryLessCardcheckBox.Checked)
            {
                StrLowBattry = LowBattry_textBox.Text;
                if ("".Equals(StrLowBattry))
                {
                    MessageBox.Show("Battery minimum value cannot be empty！");
                    RecordBatteryLessCardcheckBox.Checked = false;
                    return;
                }
                try
                {
                    Parameter.LowBattry = Convert.ToInt32(StrLowBattry);
                }
                catch (Exception)
                {
                    MessageBox.Show("Battery minimum format is wrong！");
                    RecordBatteryLessCardcheckBox.Checked = false;
                    return;
                }
                if (Parameter.LowBattry < 0 || Parameter.LowBattry > 80)
                {
                    MessageBox.Show("Battery minimum range between 0 and 80！");
                    RecordBatteryLessCardcheckBox.Checked = false;
                    return;
                }
                Parameter.RecordBatteryLessCard = true;
                LowBattry_textBox.Enabled = false;
            }
            else 
            {
                Parameter.RecordBatteryLessCard = false;
                LowBattry_textBox.Enabled = true;
            }
            if (Parameter.RecordBatteryLessCard)
            {
                Ini.SetValue(Ini.ConfigPath, Ini.BatterySeg, Ini.IsLowBattery, "True");
            }
            else
            {
                Ini.SetValue(Ini.ConfigPath, Ini.BatterySeg, Ini.IsLowBattery, "False");
            }
            Ini.SetValue(Ini.ConfigPath, Ini.BatterySeg, Ini.LowBattery, StrLowBattry);
        }
        //警告超时没有收到记录的定位讯息
        private void RecordOverTimeNoReceiInfo_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RecordOverTimeNoReceiInfo_checkBox.Checked)
            {
                string strovertime = overtimetb.Text;
                if ("".Equals(strovertime.Trim()))
                { 
                    MessageBox.Show("The unreceived timeout alarm time cannot be empty！");
                    RecordOverTimeNoReceiInfo_checkBox.Checked = false; 
                    return;
                }
                try
                {
                    Parameter.OverNoReceiveWarmTime = Convert.ToUInt16(strovertime);
                }
                catch (Exception)
                { 
                    MessageBox.Show("Unreceived timeout alarm time format error！"); 
                    RecordOverTimeNoReceiInfo_checkBox.Checked = false; 
                    return;
                }
                if (Parameter.OverNoReceiveWarmTime < 0 || Parameter.OverNoReceiveWarmTime > 65535)
                {
                    MessageBox.Show("The unreceived timeout alarm time is greater than 0 and less than 65536!"); 
                    RecordOverTimeNoReceiInfo_checkBox.Checked = false; 
                    return;
                }
                overtimetb.Enabled = false;
                Parameter.RecordOverTimeNoReceiInfo = true;
                Ini.SetValue(Ini.ConfigPath, Ini.OTNoReceiveSeg, Ini.OTNoReceiveWarmTime,Parameter.OverNoReceiveWarmTime+"");
                Ini.SetValue(Ini.ConfigPath,Ini.OTNoReceiveSeg,Ini.OTNoReveiveKey,"True");
            }
            else 
            {
                overtimetb.Enabled = true;
                Parameter.RecordOverTimeNoReceiInfo = false;
                Ini.SetValue(Ini.ConfigPath, Ini.OTNoReceiveSeg, Ini.OTNoReveiveKey, "False");
            }
        }

        private void ClearCb_CheckedChanged(object sender, EventArgs e)
        {
            if (ClearCb.Checked)
            {
                string StrClearTime = ClearTimeTab.Text;
                if ("".Equals(StrClearTime))
                {
                    MessageBox.Show("The time for cleaning up the historical record is not empty！");
                    ClearCb.Checked = false;
                    return;
                }
                try
                {
                    Parameter.ClearHistoryTime = Convert.ToInt32(StrClearTime);
                }
                catch (Exception)
                {
                    MessageBox.Show("The time format for cleaning history is wrong！");
                    ClearCb.Checked = false;
                    return;
                }
                Parameter.isClearHistory = true;
                ClearTimeTab.Enabled = false;
            }
            else
            {
                Parameter.isClearHistory = false;
                Parameter.ClearHistoryTime = 30;
                ClearTimeTab.Enabled = true;
            }
            if (!Parameter.isClearHistory)
            {
                Ini.SetValue(Ini.ConfigPath, Ini.ClearSeg, Ini.IsClearKey, "False");
            }
            else
            {
                Ini.SetValue(Ini.ConfigPath, Ini.ClearSeg, Ini.IsClearKey, "True");
                Ini.SetValue(Ini.ConfigPath, Ini.ClearSeg, Ini.ClearTimeKey, Parameter.ClearHistoryTime + "");
            }
        }

        private void ShowTraceCB_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowTraceCB.Checked)
            {
                Parameter.isShowTrace = true;
                Ini.SetValue(Ini.ConfigPath,Ini.ShowSeg,Ini.ShowTraceKey,"True");
            }
            else
            {
                Parameter.isShowTrace = false;
                Ini.SetValue(Ini.ConfigPath, Ini.ShowSeg, Ini.ShowTraceKey, "False");
            }
        }
        private void kalmanCB_CheckedChanged(object sender, EventArgs e)
        {
            if (kalmanCB.Checked)
            {
                Parameter.isKalman = true;
                KalmanBox.Visible = true;
                string StrMnosiecovar = MNosieCovarTB.Text;
                try
                {
                    Parameter.KalmanMNosieCovar = Convert.ToDouble(StrMnosiecovar);
                }
                catch (Exception)
                {
                    Parameter.KalmanMNosieCovar = 0.1;
                }
                string StrProNosieCovar = ProNosieCovarTB.Text;
                try
                {
                    Parameter.KalmanProNosieCovar = Convert.ToDouble(StrProNosieCovar);
                }
                catch (Exception)
                {
                    Parameter.KalmanProNosieCovar = 0.2;
                }
                string StrLastStatePre = LastStatePreTB.Text;
                try
                {
                    Parameter.KalmanLastStatePre = Convert.ToDouble(StrLastStatePre);
                }
                catch (Exception)
                {
                    Parameter.KalmanLastStatePre = 0.5;
                }
                LastStatePreTB.Enabled = false;
            }
            else
            {
                Parameter.isKalman = false;
                KalmanBox.Visible = false;
                LastStatePreTB.Enabled = true;
            }
        }

        private void SetForm_Load(object sender, EventArgs e)
        {
    
            Setindex_listBox.SelectedIndex = 0;
            this.Size = new Size(840, 650);//740, 550
            /*String StrPort = Ini.GetValue(Ini.ConfigPath, Ini.NetSeg, Ini.NetKey_Port);
            if (StrPort != null && !StrPort.Equals(""))
            {
                int Port = 0;
                try
                {
                    Port = Convert.ToInt32(StrPort);
                }
                catch (Exception){}
                if ((Port < 65535 && Port > 1024) || Port == 65535)
                {
                    Port_TextBox.Text = StrPort;
                }
            }*/
            loadJsonPort(Ini.NetKey_Port, Port_TextBox);

            loadJsonPort(Ini.NetKey_UWB1Port,textBox_json);
            loadFileValue(Ini.NetKey_UWB1IP, jsonComboBox);
            loadJsonPort(Ini.NetKey_UWB3Port, textBox1);
            loadFileValue(Ini.NetKey_UWB3IP, comboBox1);

            string StrWidth = Ini.GetValue(Ini.ConfigPath, Ini.MapSeg, Ini.RealWidth);
            string StrHeight = Ini.GetValue(Ini.ConfigPath, Ini.MapSeg, Ini.RealHeight);
            if (null != StrWidth && !"".Equals(StrWidth))
            {
                RealWidth_textBox.Text = StrWidth;
            }
            if (null != StrHeight && !"".Equals(StrHeight))
            {
                RealHeight_textBox.Text = StrHeight;
            }
            //重新定位窗体的位置
            this.Location = new Point((frm.Width - this.Width) / 2, (frm.Height - this.Height) / 2);
        }

        private void loadJsonPort(String kry,TextBox te) 
        {
            String StrPort = Ini.GetValue(Ini.ConfigPath, Ini.NetSeg, kry);
            te.Text = StrPort;
            /*if (StrPort != null && !StrPort.Equals(""))
            {
                int Port = 0;
                try
                {
                    Port = Convert.ToInt32(StrPort);
                }
                catch (Exception)
                {
                }
                if ((Port < 65535 && Port > 1024) || Port == 65535)
                {
                    te.Text = StrPort;
                }
            }*/
        }


        private void loadFileValue(String kry, ComboBox com)
        {
            String value = Ini.GetValue(Ini.ConfigPath, Ini.NetSeg, kry);
            if(value != null){
                 com.Items.Clear();
                 com.Items.Add(value);
                 com.SelectedIndex = 0;
            }         
        }

        
        private void TagShowCB_CheckedChanged(object sender, EventArgs e)
        {
            if (TagShowCB.Checked) Parameter.TagShowOver = true;
            else Parameter.TagShowOver = false;
        }
        private void showrefrecb_CheckedChanged(object sender, EventArgs e)
        {
            if (showrefrecb.Checked)
            {
                if (refreshmscb.SelectedIndex < 0)
                {
                    MessageBox.Show("Refresh time cannot be empty!");
                    showrefrecb.Checked = false;
                    return;
                }
                try
                {
                    Parameter.DefineInterval = Convert.ToInt32(refreshmscb.Text);
                }catch(Exception)
                {
                    MessageBox.Show("The refresh time format is wrong！");
                    showrefrecb.Checked = false;
                    return;
                }
                refreshmscb.Enabled = false;
                Parameter.isDefineInterval = true;
            }
            else
            {
                Parameter.isDefineInterval = false;
                refreshmscb.Enabled = true;
            }
        }

        private void refretypecb_CheckedChanged(object sender, EventArgs e)
        {
            if (refretypecb.Checked)
            {
                Parameter.isEnableReferType = true;
            }
            else
            {
                Parameter.isEnableReferType = false;
            }
        }

        private void enzonealarmcb_CheckedChanged(object sender, EventArgs e)
        {
            if (enzonealarmcb.Checked)
            {
                Parameter.isEnableLimitArea = true;
            }
            else
            {
                Parameter.isEnableLimitArea = false;
            }
            if (Parameter.isEnableLimitArea)
            {
                Ini.SetValue(Ini.ConfigPath, Ini.AreaAramSeg, Ini.EnableAreaAlarm, "True");
            }
            else
            {
                Ini.SetValue(Ini.ConfigPath, Ini.AreaAramSeg, Ini.EnableAreaAlarm, "False");
            }
        }
        private void posimodecb_SelectedIndexChanged(object sender, EventArgs e)
        {
            //选择定位
            if (posimodecb.SelectedIndex == 0)
            {
                Parameter.positionmode = PosititionMode.SigQuality;
                Ini.SetValue(Ini.ConfigPath, Ini.StrPositionMode, Ini.StrMode,"0");
            }
            else
            {
                Parameter.positionmode = PosititionMode.Closestdistance;
                Ini.SetValue(Ini.ConfigPath, Ini.StrPositionMode, Ini.StrMode, "1");
            }
        }
        /// <summary>
        /// 是否支持多区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SupportMulAreaCB_CheckedChanged(object sender, EventArgs e)
        {
            //如果监听已经打开此时是不能切换区域模式
            if (frm.isStart)
            {
                SupportMulAreaCB.CheckedChanged -= SupportMulAreaCB_CheckedChanged;
                SupportMulAreaCB.Checked = Parameter.isSupportMulArea;
                SupportMulAreaCB.CheckedChanged += SupportMulAreaCB_CheckedChanged;
                MessageBox.Show("Sorry, the network monitor has opened the area mode, please turn off the network monitor first!");
                return;
            }
            if (!Parameter.isSupportMulArea && SupportMulAreaCB.Checked)
            {
                /* 当前是单区域，切换到多区域
                 * 1、保存单区域的基本讯息
                 * 2、加载多区域的基本讯息
                 */
                posimodecb.SelectedIndex = 1;
                posimodecb.Enabled = false;
                Parameter.isSupportMulArea = SupportMulAreaCB.Checked;
                //保存基站
                frm.SavePorts();
                //保存限制区域
                frm.SaveLimitArea();
                frm.LoadMulAreas();
            }
            else if (Parameter.isSupportMulArea && !SupportMulAreaCB.Checked)
            {
                /* 当前是多区域，切换到单区域
                 * 1、保存多区域的基本讯息
                 * 2、加载单区域的基本讯息
                 */
                frm.group = null;
                if (Parameter.RealWidth != 0 && Parameter.RealWidth != 0)
                {
                    //计算当前图片与面板的比例
                    double wscale = (double)Map_panel.Width / Parameter.RealWidth;
                    double hscale = (double)Map_panel.Height / Parameter.RealHeight;
                    //面板距离与实际距离的比值知道
                    frm.Img_RealDisRelation = wscale > hscale ? hscale : wscale;
                }
                posimodecb.Enabled = true;
                Parameter.isSupportMulArea = SupportMulAreaCB.Checked;
                frm.SaveMulAreas();
                frm.StrMapPath = Ini.GetValue(Ini.ConfigPath, Ini.MapSeg, Ini.MapKey_Path);
                frm.InnerPorts.Clear();
                frm.LoadPorts();
                frm.LoadLimitAreas();
                DxfMapParam.ClearDxf();
                frm.LoadDxfMap();
            }
            frm.RefreshAreaUI();
        }

        private void updatemularealistbtn_Click(object sender, EventArgs e)
        {
            if(frm.isStart)
            {
                MessageBox.Show("Sorry, the network monitor has been opened, the map message cannot be updated at this time. Please turn off the network monitor first!");
                return;
            }
            String strid = areaidedt.Text;
            String name = areanameedt.Text;
            String path = areamappathedt.Text;
            String stractualX = actualXTb.Text;
            String stractualY = actualYTb.Text;
            areaidedt.Enabled = true;
            if ("".Equals(strid))
            {
                MessageBox.Show("Sorry, the ID cannot be empty！");
                return;
            }
            if (strid.Length != 4)
            {
                MessageBox.Show("Sorry, the length of ID has to be 4!");
                return;
            }
            byte[] id = new byte[2];
            try
            {
                id[0] = Convert.ToByte(strid.Substring(0, 2), 16);
                id[1] = Convert.ToByte(strid.Substring(2, 2), 16);
            }
            catch (Exception)
            {
                MessageBox.Show("Sorry, the ID format is wrong!");
                return;
            }
            if ("0000".Equals(strid))
            {
                MessageBox.Show("Sorry, the area ID cannot be 0000!");
                return;
            }
            if ("".Equals(stractualX) || "".Equals(stractualY))
            {
                MessageBox.Show("Sorry, The actual width and height cannot be empty!");
                return;
            }
            float actualx = 0f, actualy = 0f;
            try
            {
                actualx = Convert.ToSingle(stractualX);
                actualy = Convert.ToSingle(stractualY);
            }catch(Exception)
            {
                MessageBox.Show("Sorry, Actual width and height format error!");
                return;
            }
            if (actualx <= 0 || actualy <= 0)
            {
                MessageBox.Show("Sorry, The actual width and height are greater than 0!");
                return;
            }
            ListViewItem[] items = null;
            //判断ID = 0000 是否存在
            if (mularealist.Items.ContainsKey("0000"))
            {//存在ID = 0000说明是向里面添加区域
                items = mularealist.Items.Find("0000", false);
                if (items.Length <= 0)
                {
                    MessageBox.Show("Sorry, get list add item failed!");
                    return;
                }
                items[0].Name = strid;
                items[0].Text = strid;
                items[0].SubItems[1].Text = name;
                items[0].SubItems[2].Text = stractualX;
                items[0].SubItems[3].Text = stractualY;
                items[0].SubItems[4].Text = path;
            }
            else
            {//说明是修改区域讯息
                //查看当前是否有现在项
                items = mularealist.Items.Find(strid, false);
                if (null == items || items.Length <= 0)
                {
                    MessageBox.Show("Sorry, please click add to add!");
                    return;
                }
                items[0].SubItems[1].Text = name;
                items[0].SubItems[2].Text = stractualX;
                items[0].SubItems[3].Text = stractualY;
                items[0].SubItems[4].Text = path;
            }
            //每次点击更新时把列表中的项重新保存到集合中
            UpdateListToCollection();
        }
        //更新集合中的项到列表中
        private void UpdateCollectionToList()
        {
            String strid = "", strname = "", strpath = "";
            ListViewItem item = null;
            mularealist.Items.Clear();
            List<KeyValuePair<string, Group>> grouplist = frm.Groups.OrderBy(group => group.Key).ToList();
            foreach (KeyValuePair<string, Group> group in grouplist)
            {
                strid = group.Value.id[0].ToString("X2") + group.Value.id[1].ToString("X2");
                strname = group.Value.name;
                strpath = group.Value.grouppath;
                item = new ListViewItem();
                item.Text = strid;
                item.Name = strid;
                item.SubItems.Add(strname);
                item.SubItems.Add(group.Value.actualwidth + "");
                item.SubItems.Add(group.Value.actualheight + "");
                item.SubItems.Add(strpath);
                mularealist.Items.Add(item);
            }
        }
        /// <summary>
        /// 更新列表中的项到集合中去
        /// </summary>
        private void UpdateListToCollection()
        {
            String strid = "", strname = "", strpath = "", stractualwidth = "", stractualheight = "";
            float actualwidth = 0.0f, actualheight = 0.0f,wscale = 0.0f,hscale = 0.0f;
            byte[] id = null;
            Group curgp = null;
            #region 组别视图遍历列表
            /* 遍历列表
             * 若组别集合中存在指定的组别则只修改组别讯息
             * 若不存在则向集合中添加组别
             */
            for (int i = 0; i < mularealist.Items.Count;i ++)
            {
                strid = mularealist.Items[i].Text;
                strname = mularealist.Items[i].SubItems[1].Text;
                stractualwidth = mularealist.Items[i].SubItems[2].Text;
                stractualheight = mularealist.Items[i].SubItems[3].Text;
                strpath = mularealist.Items[i].SubItems[4].Text;
                if ("".Equals(strid))
                {
                    continue;
                }
                if(strid.Length != 4)
                {
                    continue;
                }
                if ("0000".Equals(strid))
                {
                    continue;
                }
                id = new Byte[2];
                try 
                {
                    id[0] = Convert.ToByte(strid.Substring(0, 2), 16);
                    id[1] = Convert.ToByte(strid.Substring(2, 2), 16);
                }catch(Exception)
                {
                    continue;
                }
                if ("".Equals(stractualwidth) || "".Equals(stractualheight))
                {
                    continue;
                }
                try 
                {
                    actualwidth  = Convert.ToSingle(stractualwidth);
                    actualheight = Convert.ToSingle(stractualheight);
                }catch(Exception)
                {
                    continue;
                }
                if (actualwidth <= 0 || actualheight <= 0)
                {
                    continue;
                }
                //查看当前的ID是否存在，如果已经存在则只需要我们对它的基本属性进行改变
                if (!frm.Groups.TryGetValue(strid, out curgp))
                {
                    curgp = new Group();
                }
                curgp.id = id;
                curgp.name = strname;
                curgp.grouppath = strpath;
                curgp.actualwidth = actualwidth;
                curgp.actualheight = actualheight;
                //计算画图面板与实际距离的比值关系
                wscale = (float)frm.Map_panel.Width / actualwidth;
                hscale = (float)frm.Map_panel.Height / actualheight;
                curgp.scale = wscale > hscale ? hscale : wscale;
                if (!frm.Groups.ContainsKey(strid))
                {
                    frm.Groups.TryAdd(strid, curgp);
                }
            }
            #endregion
            #region 删除组别集合中存在，视图列表中不存在的组别讯息
            List<String> delegps = new List<String>();
            //再把Groups中存在，列表中不存在的删除掉
            foreach(KeyValuePair<string,Group> gp in frm.Groups)
            {
                if (!mularealist.Items.ContainsKey(gp.Key))
                {
                    delegps.Add(gp.Key);
                }
            }
            Group gpit = null;
            for (int i = 0; i < delegps.Count;i++)
            {
                frm.Groups.TryRemove(delegps[i], out gpit);
            }
            #endregion
        }
        //在列表中添加一个空行，ID为0000
        private void addbtn_Click(object sender, EventArgs e)
        {
            //判断里面是否已经存在“0000”项
            areaidedt.Enabled = true;
            if (mularealist.Items.ContainsKey("0000"))
            {
                MessageBox.Show("Sorry, we can already add group items!");
                return;
            }
            ListViewItem item = new ListViewItem();
            item.Name = "0000";
            item.Text = "0000";
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("");
            mularealist.Items.Add(item);
            areaidedt.Text = "0000";
            scaletb.Text = "****";
        }
        /// <summary>
        /// 选择当前区域的地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mulareaselectmapbtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.Filter = "All(.dxf)|*.dxf;*.dwg;";
            filedialog.Title = "Select area map";
            if (filedialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //将Map路径保存到配置文件中
                String strpath = filedialog.FileName;
                //获取字符串名称
                int index = strpath.LastIndexOf(@"\");
                String strname = strpath.Substring(index + 1, strpath.Length - index - 1);
                if (strname.Equals("") || strname == null)
                {
                    MessageBox.Show("Map the path cannot be empty！");
                    return;
                }
                //查看是否存在地图名称冲突
                bool mark = false;
                if (Ini.isFileNameConflict(strname, Ini.AreaMapPath))
                {
                    if (MessageBox.Show("Sorry, the file already exists in the specified directory. Whether to use the same map file?", "Pay attention to", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                    else
                    {
                        mark = true;
                    }
                }
                //将当前的图片文件保存到复制一份保存到指定的文件夹下
                if (!Ini.CopyFileToDir(strpath, Ini.AreaMapPath) && !mark)
                {
                    MessageBox.Show("File save failure！");
                    return;
                }
                //画图到面板上
                areamappathedt.Text = strname;
                //清除掉GDIGraphics3D
                DxfMapParam.ClearDxf();
                smallmap = DxfMapParam.GetDxfMap(strname, 1, mulareamappanel.Width / 2, mulareamappanel.Height / 2, mulareamappanel.Width, mulareamappanel.Height);
                mulareamappanel_Paint(null,null);
                DxfMapParam.ClearDxf();
            }
        }
        /// <summary>
        /// 点击每一行以后即可更新到编辑框中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mularealist_Click(object sender, EventArgs e)
        {
            if (mularealist.SelectedItems.Count <= 0)
            {
                return;
            }

            if(frm.isStart)
            {
                MessageBox.Show("Sorry, the network monitor has been opened, you can't view the map information at this time, please turn off the network monitor first!");
                return;
            }
            Group gp = null;
            areaidedt.Text = mularealist.SelectedItems[0].Text;
            if ("0000".Equals(areaidedt.Text))
            {
                areaidedt.Enabled = true;
            }
            else
            {
                areaidedt.Enabled = false;
            }
            areanameedt.Text = mularealist.SelectedItems[0].SubItems[1].Text;
            actualXTb.Text = mularealist.SelectedItems[0].SubItems[2].Text;
            actualYTb.Text = mularealist.SelectedItems[0].SubItems[3].Text;
            //获取Groups中的讯息
            if (frm.Groups.TryGetValue(areaidedt.Text, out gp))
            {
                scaletb.Text = gp.scale.ToString();
            }
            else
            {
                scaletb.Text = "****";
            }
            areamappathedt.Text = mularealist.SelectedItems[0].SubItems[4].Text;
            DxfMapParam.ClearDxf();
            smallmap = DxfMapParam.GetDxfMap(areamappathedt.Text, 1, mulareamappanel.Width / 2, mulareamappanel.Height / 2, mulareamappanel.Width, mulareamappanel.Height);
            mulareamappanel_Paint(null, null);
            DxfMapParam.ClearDxf();
        }

        private void removebtn_Click(object sender, EventArgs e)
        {
            areaidedt.Enabled = true;
            if (mularealist.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Sorry, select the specified item to delete!");
                return;
            }
            mularealist.Items.Remove(mularealist.SelectedItems[0]);
            UpdateListToCollection();
        }
        private void mulareamappanel_Paint(object sender, PaintEventArgs e)
        {
            if (null != smallmap)
            {
                mulareamappanel.CreateGraphics().DrawImageUnscaled(smallmap, 0, 0);
            }
        }

        private void IsUse3SCb_CheckedChanged(object sender, EventArgs e)
        {
            //采用定位方式
            if (IsUse3SCb.Checked)
            {
                Parameter.isUse3Station = true;
            }
            else
            {
                Parameter.isUse3Station = false;
            }
        }

        private void ClearcacheMapBtn_Click(object sender, EventArgs e)
        {
            frm.ClearMap();
            ClearcacheMapBtn.Enabled = false;
        }

        private void LastStatePreTB_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            String Value = jsonComboBox.Text;
            //文件路径
            /*if (Value.Equals("") || Value == null)
            {
                MessageBox.Show("IP address cannot be empty！");
                return;
            }
            if (!Ini.SetValue(Ini.ConfigPath, Ini.NetSeg, Ini.NetKey_jsonIP, Value))
            {
                MessageBox.Show("Save IP addresses to fail！");
            }*/
            saveFileValue(Value, Ini.NetSeg, Ini.NetKey_UWB1IP, "IP Adress");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String Value = textBox_json.Text;
            int Port = 0;
            //文件路径
            if (Value.Equals("") || Value == null)
            {
                MessageBox.Show("Port can't be empty！");
                return;
            }
            try
            {
                Port = Convert.ToInt32(Value);
            }
            catch (Exception)
            {
                MessageBox.Show("Port format is wrong！");
                return;
            }
            if (Port > 65535 || Port < 1024)
            {
                MessageBox.Show("Port is greater than 1024 and less than or equal to 65535！");
            }
            if (!Ini.SetValue(Ini.ConfigPath, Ini.NetSeg, Ini.NetKey_UWB1Port, Value))
            {
                MessageBox.Show("Set port failure！");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            String Value = comboBox1.Text;
            saveFileValue(Value, Ini.NetSeg, Ini.NetKey_UWB3IP, "IP Adress");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String Value = textBox1.Text;
            saveFileValue(Value, Ini.NetSeg, Ini.NetKey_UWB3Port, "Port");
        }


    }
    //dex地图的相关参数
    class DxfMapParam
    {
        private DxfMapParam() { }
        public static double scale = 1;
        public static double CenterX = 0;
        public static double CenterY = 0;
        public static double PanelCenterX = 0;
        public static double PanelCenterY = 0;
        private static WW.Cad.Model.DxfModel model = null;
        private static GDIGraphics3D gdiGraphics3D = null;
        public static Bitmap GetDxfMap(string FilePath, double CurScale, double CenterX, double CenterY, double Width, double Height)
        {
            WW.Cad.Drawing.GDI.GDIGraphics3D GDI = DrawDxf(FilePath, CurScale, CenterX, CenterY, Width, Height);
            if (null == GDI)
            {
                return null;
            }
            Bitmap CurBitmap = null;
            try
            {
                CurBitmap = new Bitmap((int)Width,(int)Height);
                Graphics DxfG = Graphics.FromImage(CurBitmap);
                DxfG.Clear(Color.White);
            
                GDI.Draw(DxfG,new Rectangle(0,0,(int)Width,(int)Height));
            }
            catch (Exception)
            {
                return null;
            }
            return CurBitmap;
        }

        public static void ClearDxf()
        {
            model = null; 
            gdiGraphics3D = null;
        }
        /// <summary>
        /// 得到GDI画笔GDIGraphics3D
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="CurPanel"></param>
        /// <param name="CurScale"></param>
        public static GDIGraphics3D DrawDxf(string FilePath, double CurScale, double CenterX, double CenterY, double Width, double Height)
        {
            if (null == gdiGraphics3D)
            {
                gdiGraphics3D = new GDIGraphics3D(GraphicsConfig.WhiteBackgroundCorrectForBackColor);
            }
            Matrix4D modelTransform = Matrix4D.Identity;
            Bounds3D bounds = new Bounds3D();
            string absPath = Ini.AreaMapPath + "\\" + FilePath;
            if (!File.Exists(absPath))
            {
                return null;
            }
            if (null == model)
            {
                if (FilePath.EndsWith(".dxf"))
                {
                    model = DxfReader.Read(absPath);
                }
                else if (FilePath.EndsWith(".dwg"))
                {
                    model = DwgReader.Read(absPath);
                }
                else
                {
                    return null;
                }
                try
                {
                    gdiGraphics3D.CreateDrawables(model);
                }catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            try
            {
                gdiGraphics3D.BoundingBox(bounds, modelTransform);
                CalculateTo2DTransform(CurScale, CenterX, CenterY, Width, Height, bounds, gdiGraphics3D, modelTransform);
            }
            catch (Exception)
            {
                return null;
            }
            return gdiGraphics3D;
        }
        //计算画图的相关信息
        private static void CalculateTo2DTransform(double scale, double Center_X, double Center_Y, double Width, double Height, Bounds3D bounds, GDIGraphics3D gdiGraphics3D, Matrix4D modelTransform)
        {
            if (null != bounds)
            {
                Matrix4D to2DTransform = DxfUtil.GetScaleTransform(bounds.Corner1,bounds.Corner2,bounds.Center,
                        new Point3D(0, Height / scale, 0),new Point3D(Width / scale, 0, 0),new Point3D(Center_X, Center_Y, 0));
                if (null != gdiGraphics3D)
                {
                    gdiGraphics3D.To2DTransform = to2DTransform * modelTransform;
                }
            }
        }
    }

}
