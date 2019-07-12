using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Timer = System.Windows.Forms.Timer;
using System.Net;
using System.Net.Sockets;
using MG3732_MSG;
using PersionAutoLocaSys.Model;

namespace PersionAutoLocaSys
{
    public partial class Form1 : Form
    {
        public  ParamSet MyParamSet = null;
        public ExitType mexittype = ExitType.UnKunown;
        private Bitmap MainTitleBitmap, MainCenterBitmap;
        public BtnStatus cnn_btn_status = BtnStatus.Bt_start_No_Press;//开始连接按键的状态
        public BtnStatus set_btn_status = BtnStatus.Bt_start_No_Press;//设置按键的状态
        public BtnStatus select_btn_status = BtnStatus.Bt_start_No_Press;//查询按钮的状态
        public Graphics MainTitle_G, MainCenter_G;
        private System.Object AllRegInfoWin_Lock = new System.Object();
        public AllRegInfoWin MyAllRegInfoWin = null;
        public AlarmInfoWin MyAlarmInfoWin = null;
        public OterAlarmWin MyOterAlarmWin = null;
        public RegInfoWin MyRegInfoWin = null;
        // 测试定时器

        private Timer MyTimer = null;
       
        // 开启端口监听

        public  UdpClient MyUdpClient = null;
        public Thread UdpListenerThread = null;
        public bool isStart = false;
        public float MeasurTick = 0;
        public bool TempAlarmFlg = false;
        public Person CurPerson = null;
        public EnterPassWin MyEnterPassWin = null;
        private List<string> strdeletes = new List<string>();
        private List<object> deleteobjs = new List<object>();
        private WarmInfo[] TempWarmes = null;
        private int totals = 0;

        public void udpSendData(byte[] dgram, int bytes, IPEndPoint endPoint) 
        {
            try 
            {
                MyUdpClient.Send(dgram, bytes, endPoint);
            } catch(SocketException ex) { }           
        }

        //是否刷新列表
        public Form1()
        {
            InitializeComponent();
            DrawIMG.maxDt = DrawIMG.getMaxDate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MyTimer = new Timer();
            MyTimer.Interval = 1300;
            MyTimer.Tick += MyTimer_Tick;
            //  每次打开窗口后，重新设置的相关信息
            #region 加载设置的基本参数
            SysParam.LoadGroup();
            DrawIMG.LoadShowGroups();
            SysParam.LoadAreaInfo();

            FileModel.fileInit().getCommCollTags(); //改變存儲方式，代替了SysParam.LoadPersonTagInfo()方法
            //SysParam.LoadPersonTagInfo();            
            SysParam.LoadNetInfo();
            SysParam.LoadAlarmParam();            
            SysParam.LoadPerson();
            SysParam.LoadPersonMsg();
            FileModel.fileInit().getSysAdmissionExit();
            #endregion
            #region 清除原始资讯、保存的警告讯息、人员操作讯息
            if (SysParam.isClearData)
			{
				SysParam.ClearOrigionalAlarm();
			}
            if (SysParam.isOnTimeClearHandlerWarm)
			{
				SysParam.ClearWarmData();
			}
            SysParam.ClearLog(30);
            SysParam.ClearPersonOperRecords();
            #endregion
            #region 启动时画出主页面
            DrawIMG.Frm = this;
            MainTitleBitmap = new Bitmap(ConstInfor.MainTitle_Width, ConstInfor.MainTitle_Height);
            MainCenterBitmap = new Bitmap(ConstInfor.MainCenter_Width, ConstInfor.MainCenter_Height);
            MainTitle_G = Graphics.FromImage(MainTitleBitmap);
            MainCenter_G = Graphics.FromImage(MainCenterBitmap);
            MainTitle_G.DrawImageUnscaled(Properties.Resources.f075edc, ConstInfor.Log_left, ConstInfor.Log_top);
            DrawIMG.DrawPC_Tag(MainTitle_G, cnn_btn_status);
            DrawIMG.Paint_CnnBtn(MainTitle_G, cnn_btn_status);
            DrawIMG.Paint_SetBtn(MainTitle_G, set_btn_status);
            DrawIMG.Paint_SelectBtn(MainTitle_G, select_btn_status);
            //画出表格
            DrawIMG.DrawMainCenter(MainCenter_G);
            MyEnterPassWin = new EnterPassWin(this, OperType.OpenForm);
            MyEnterPassWin.Show();
            #endregion
        }


        private void cumpterDoubleClick()
        {        
        }

        /// <summary>
        /// 画出画出主页面的标题中的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainTitle_Panel_Paint(object sender, PaintEventArgs e)
        {
            //画出主页面的标题
            if (null != MainTitleBitmap)
            {
                MainTitle_Panel.CreateGraphics().DrawImageUnscaled(MainTitleBitmap, 0, 0);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == ConstInfor.FORMMESSAGE && (int)m.WParam == ConstInfor.CLOSEMSGPARAM)
            {
                if (this.CurPerson == null)
                {
                    this.Close();
                    return;
                }
                else
                {
                    MyEnterPassWin = new EnterPassWin(this, OperType.CloseForm);
                    if (MyEnterPassWin.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }
                    else
                    {
                        if (mexittype == ExitType.AppExit)
                        {
                            this.Close();
                        }
                        else if (mexittype == ExitType.LogOut)
                        {
                            MessageBox.Show("退出當前登錄成功!");
                            CurPerson = null;
                            this.Text = "人員定位系統V11.2.19 ( 當前沒有用戶登陸)";
                        }
                        return;
                    }
                }
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// 在主页面的标题栏中，鼠标按下按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainTitle_Panel_MouseDown(object sender, MouseEventArgs e)
        {
            int Cur_X, Cur_Y;
            Cur_X = e.X;
            Cur_Y = e.Y;
            if (Cur_X > ConstInfor.BgCnnBtn_left && Cur_Y > ConstInfor.BgCnnBtn_top && Cur_X < ConstInfor.BgCnnBtn_left+ConstInfor.BgCnn_Width && Cur_Y < ConstInfor.BgCnnBtn_top + ConstInfor.BgCnn_Height)
            {
                switch (cnn_btn_status)
                {
                    case BtnStatus.Bt_start_No_Press:cnn_btn_status = BtnStatus.Bt_start_Press;break;
                    case BtnStatus.Bt_stop_No_Press:cnn_btn_status = BtnStatus.Bt_stop_Press;break;
                }
            }
            else if (Cur_X > ConstInfor.BgSetBtn_left && Cur_Y > ConstInfor.BgSetBtn_top && Cur_X < ConstInfor.BgSetBtn_left + ConstInfor.BgSet_Width && Cur_Y < ConstInfor.BgSetBtn_top + ConstInfor.BgSet_Height)
            {
                switch (set_btn_status)
                {
                    case BtnStatus.Bt_start_No_Press:set_btn_status = BtnStatus.Bt_start_Press;break;
                    case BtnStatus.Bt_start_Press:set_btn_status = BtnStatus.Bt_start_No_Press;break;
                }
            }
            else if (Cur_X > ConstInfor.SelecBtn_left && Cur_Y > ConstInfor.SelectBtn_top && Cur_X < ConstInfor.SelecBtn_left + ConstInfor.Select_Width && Cur_Y < ConstInfor.SelectBtn_top + ConstInfor.Select_Height)
            {
                switch (select_btn_status)
                {
                    case BtnStatus.Bt_start_No_Press:select_btn_status = BtnStatus.Bt_start_Press;break;
                    case BtnStatus.Bt_start_Press:select_btn_status = BtnStatus.Bt_start_No_Press;break;
                }
            }
            DrawIMG.Paint_CnnBtn(MainTitle_G, cnn_btn_status);
            DrawIMG.Paint_SetBtn(MainTitle_G, set_btn_status);
            DrawIMG.Paint_SelectBtn(MainTitle_G, select_btn_status);
            MainTitle_Panel_Paint(null,null);
        }

        /// <summary>
        /// 鼠标在MainTitle_Panel上移动时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainTitle_Panel_MouseMove(object sender, MouseEventArgs e)
        {
            int Cur_X, Cur_Y;
            Cur_X = e.X;
            Cur_Y = e.Y;
            //判断是否按下了连接按键或设置按键,按下按键后，鼠标的光标移动到按键外面，认为是松开了按键
            if (BtnStatus.Bt_start_Press == cnn_btn_status)
            {
                 if (Cur_X < ConstInfor.BgCnnBtn_left || Cur_X > ConstInfor.BgCnnBtn_left+ConstInfor.BgCnn_Width || Cur_Y < ConstInfor.BgCnnBtn_top || Cur_Y > ConstInfor.BgCnnBtn_top + ConstInfor.BgCnn_Height)
                 {
                     cnn_btn_status = BtnStatus.Bt_start_No_Press;
                 }
            }else if(BtnStatus.Bt_stop_Press == cnn_btn_status)
            {
                if (Cur_X < ConstInfor.BgCnnBtn_left || Cur_X > ConstInfor.BgCnnBtn_left + ConstInfor.BgCnn_Width || Cur_Y < ConstInfor.BgCnnBtn_top || Cur_Y > ConstInfor.BgCnnBtn_top + ConstInfor.BgCnn_Height)
                {
                    cnn_btn_status = BtnStatus.Bt_stop_No_Press;
                }
            }
            else if (BtnStatus.Bt_start_Press == set_btn_status)
            {
                if (Cur_X < ConstInfor.BgSetBtn_left || Cur_X > ConstInfor.BgSetBtn_left + ConstInfor.BgSet_Width || Cur_Y < ConstInfor.BgSetBtn_top || Cur_Y > ConstInfor.BgSetBtn_top + ConstInfor.BgSet_Height)
                {
                    set_btn_status = BtnStatus.Bt_start_No_Press;
                }
            }
            else if (BtnStatus.Bt_start_Press == select_btn_status)
            {
                if (Cur_X < ConstInfor.SelecBtn_left || Cur_X > ConstInfor.SelecBtn_left + ConstInfor.Select_Width || Cur_Y < ConstInfor.SelectBtn_top || Cur_Y > ConstInfor.SelectBtn_top + ConstInfor.Select_Height)
                {
                    select_btn_status = BtnStatus.Bt_start_No_Press;
                }
            }
            DrawIMG.Paint_CnnBtn(MainTitle_G, cnn_btn_status);
            DrawIMG.DrawPC_Tag(MainTitle_G, cnn_btn_status);
            DrawIMG.Paint_SetBtn(MainTitle_G, set_btn_status);
            DrawIMG.Paint_SelectBtn(MainTitle_G,select_btn_status);
            MainTitle_Panel_Paint(null, null);
        }

		private void Start()
        {
            if (null != MyEnterPassWin && !MyEnterPassWin.IsDisposed)
            {
                return;
            }
            //記錄當前人員的操作
            PersonOperation curpersonoper = new PersonOperation(this.CurPerson.ID, OperType.OpenListen);
            CommonCollection.personOpers.Add(curpersonoper);
            isStart = false;
            switch (DrawIMG.InitNet(NetParam.Ip, NetParam.Port))
            {
                case ConstInfor.Net_Ok: isStart = true; break;
                case ConstInfor.Ip_Invalid: MessageBox.Show("Ip地址有誤!"); break;
                case ConstInfor.Port_Invalid: MessageBox.Show("端口格式有誤!"); break;
                case ConstInfor.NetNode_Fail: MessageBox.Show("獲取網絡節點失敗!"); break;
                case ConstInfor.UdpClient_ReDefine: MessageBox.Show("UdpClient已經存在!"); break;
                case ConstInfor.UdpClient_Fail: MessageBox.Show("UdpClient生成失敗!"); break;
            }
            cnn_btn_status = BtnStatus.Bt_stop_No_Press;
            if (isStart)
            {
                FileOperation.ClearBox();
				SysParam.CurrentTickCount = Environment.TickCount;
                DrawIMG.StartListenerThread();
                DrawIMG.StartSig();
                MyTimer.Start();
                //说明此时需要发送简讯信息
                if (SysParam.isMsgSend)
                {
                    SysParam.SendMsgThread = new Thread(SysParam.ScanSendMsg);
                    SysParam.SendMsgThread.Start();
                }               
            }
            else
            {
                cnn_btn_status = BtnStatus.Bt_start_No_Press;
            }
        }
		//关闭监控
        private void CloseListen()
        {
            //記錄當前人員的操作
            PersonOperation curpersonoper = new PersonOperation(this.CurPerson.ID, OperType.CloseListen);
            CommonCollection.personOpers.Add(curpersonoper);

            cnn_btn_status = BtnStatus.Bt_start_No_Press;
            MyTimer.Stop();
            DrawIMG.StopThread();
            RecordOperation.Close();
            if (SoundOperation.isSoundPlay)
            {
                SoundOperation.StopSoundPlay();
            }
        }

        //进入设置界面
        private void EnterSettingWin()
        {
            //查看当前登陆的用户是否具有相应的权限
            if (this.CurPerson.PersonAccess == PersonAccess.SimplePerson)
            {
                MessageBox.Show("對不起，當前用戶不具有設置參數權限!");
                return;
            }
            //記錄当前人员进入设置界面的操作
            PersonOperation curpersonoper = new PersonOperation(this.CurPerson.ID, OperType.EnterSetting);
            CommonCollection.personOpers.Add(curpersonoper);
            //开启设置窗体
            MyParamSet = new ParamSet(this);
            MyParamSet.ShowDialog();
        }

        //进入查询界面
        private void EnterSelectWin()
        {
            QueryWin MyQueryWin = new QueryWin(this);
            MyQueryWin.ShowDialog();
        }

        /// <summary>
        ///  在主页面的标题栏中，鼠标松开按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void MainTitle_Panel_MouseUp(object sender, MouseEventArgs e)
        {
            int Cur_X, Cur_Y;
            Cur_X = e.X;
            Cur_Y = e.Y;
			
			if (null != MyEnterPassWin && !MyEnterPassWin.IsDisposed)
            {
                return;
            }
			
            if (Cur_X > ConstInfor.BgCnnBtn_left && Cur_Y > ConstInfor.BgCnnBtn_top && Cur_X < ConstInfor.BgCnnBtn_left + ConstInfor.BgCnn_Width && Cur_Y < ConstInfor.BgCnnBtn_top + ConstInfor.BgCnn_Height)
            {
                switch (cnn_btn_status)
                {
                    case BtnStatus.Bt_start_Press:
                        if (null == CurPerson)
                        {
                            if (MessageBox.Show("對不起，你還沒有登錄，不能開啟監控,請問是否需要登陸?", "提醒", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                            {
                                MyEnterPassWin = new EnterPassWin(this, OperType.OpenListen);
                                if (DialogResult.OK == MyEnterPassWin.ShowDialog())
                                {
                                    Start();
                                    break;
                                }
                            }
                            cnn_btn_status = BtnStatus.Bt_start_No_Press;
                            break;
                        }
                        else
                        {
                            Start();
                        }
                        break;
                    case BtnStatus.Bt_stop_Press:
                        if (null == CurPerson)
                        {
                            if (MessageBox.Show("對不起，你還沒有登錄，不能關閉監控，請問是否需要登陸?", "提醒", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                            {
                                MyEnterPassWin = new EnterPassWin(this, OperType.CloseListen);
                                if (DialogResult.OK == MyEnterPassWin.ShowDialog())
                                {
                                    CloseListen();
                                    cnn_btn_status = BtnStatus.Bt_start_No_Press;
                                    break;
                                }
                            }
                            //此时开始按钮状态变为灰色监控状态
                            cnn_btn_status = BtnStatus.Bt_stop_No_Press;
                            break;
                        }
                        else
                        {
                            CloseListen();
                        }
                        break;
                }
            }else if (Cur_X > ConstInfor.BgSetBtn_left && Cur_Y > ConstInfor.BgSetBtn_top && Cur_X < ConstInfor.BgSetBtn_left + ConstInfor.BgSet_Width && Cur_Y < ConstInfor.BgSetBtn_top + ConstInfor.BgSet_Height)
            {
                switch (set_btn_status)
                {
                    case BtnStatus.Bt_start_No_Press:
                        set_btn_status = BtnStatus.Bt_start_Press;
                        break;
                    case BtnStatus.Bt_start_Press:
						set_btn_status = BtnStatus.Bt_start_No_Press;
                        if (cnn_btn_status != BtnStatus.Bt_stop_No_Press)
                        {
                            if (null == CurPerson)
                            {
                                if (MessageBox.Show("對不起，你還沒有登錄，不能進入設置界面，請問是否需要登陸?", "提醒", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                                {
                                    MyEnterPassWin = new EnterPassWin(this, OperType.EnterSetting);
                                    if (DialogResult.OK == MyEnterPassWin.ShowDialog())
                                    {
                                        EnterSettingWin();
                                        break;
                                    }
                                }
                                set_btn_status = BtnStatus.Bt_start_No_Press;
                                break;
                            }
                            else
                            {
                                EnterSettingWin();
                            }
                        }
                        else
                        {
                            MessageBox.Show("對不起，需要關閉連接監控才能進行參數設置!");
                        }
                        break;

                }
            }else if (Cur_X > ConstInfor.SelecBtn_left && Cur_Y > ConstInfor.SelectBtn_top && Cur_X < ConstInfor.SelecBtn_left + ConstInfor.Select_Width && Cur_Y < ConstInfor.SelectBtn_top + ConstInfor.Select_Height)
            {
                switch (select_btn_status)
                {
                    case BtnStatus.Bt_start_No_Press:
                        select_btn_status = BtnStatus.Bt_start_Press;
                        break;
                    case BtnStatus.Bt_start_Press:
                        select_btn_status = BtnStatus.Bt_start_No_Press;
                        if (null == this.CurPerson)
                        {
                            if (MessageBox.Show("對不起，你還沒有登錄，不能查詢設備訊息,請問是否需要登陸?", "提醒", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                            {//实现登陆
                                MyEnterPassWin = new EnterPassWin(this, OperType.SearchTag);
                                if (DialogResult.OK == MyEnterPassWin.ShowDialog())
                                {
                                    EnterSelectWin();
                                    break;
                                }
                            }
                            select_btn_status = BtnStatus.Bt_start_No_Press;
                            break;
                        }
                        else
                        {
                            EnterSelectWin();
                        }
                        break;
                }
            }
            DrawIMG.Paint_CnnBtn(MainTitle_G, cnn_btn_status);
            DrawIMG.Paint_SetBtn(MainTitle_G, set_btn_status);
            DrawIMG.DrawPC_Tag(MainTitle_G, cnn_btn_status);
            DrawIMG.Paint_SelectBtn(MainTitle_G, select_btn_status);
            MainTitle_Panel_Paint(null, null);
        }
        /// <summary>
        /// 中心部分，鼠标的点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainCenter_Panel_MouseClick(object sender, MouseEventArgs e)
        {
            if (null != MyEnterPassWin && !MyEnterPassWin.IsDisposed)
            {
                return;
            }
            int CurX,CurY;
            CurX = e.X;
            CurY = e.Y;
            int CommCellWidth = (ConstInfor.MainCenter_Width - ConstInfor.AlarmCell_First_Width - ConstInfor.Table_Left * 2) / ConstInfor.AlarmCell_RowNum;
            int CommCelleHeight = (ConstInfor.Alarm_TB_Bottom - ConstInfor.AlarmBtt - ConstInfor.Cross_Ban_H) / (ConstInfor.AlarmCell_ColNum);
            int X_left = ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width;
            int Y_top = ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H + CommCelleHeight;
            if (CurX > X_left && CurX < X_left + CommCellWidth && CurY > Y_top &&　CurY < Y_top + CommCelleHeight)
            {
                SysParam.isTracking = true;
                MyAlarmInfoWin = new AlarmInfoWin(this,SpeceilAlarm.PersonHelp);
                MyAlarmInfoWin.ShowDialog();
                MyAlarmInfoWin = null;
                return;
            }
            else if (CurX > X_left + CommCellWidth && CurX < X_left + CommCellWidth * 2 && CurY > Y_top && CurY < Y_top + CommCelleHeight)
            {
                //区域管制
                SysParam.isTracking = true;
                MyAlarmInfoWin = new AlarmInfoWin(this,SpeceilAlarm.AreaControl);
                MyAlarmInfoWin.ShowDialog();
                MyAlarmInfoWin = null;
                return;
            }
            else if (CurX > X_left + CommCellWidth*2 && CurX < X_left + CommCellWidth * 3 && CurY > Y_top && CurY < Y_top + CommCelleHeight)
            {
                SysParam.isTracking = true;
                MyAlarmInfoWin = new AlarmInfoWin(this, SpeceilAlarm.Resid);
                MyAlarmInfoWin.ShowDialog();
                MyAlarmInfoWin = null;
                return;
            }
            else if (CurX > X_left + CommCellWidth * 3 && CurX < X_left + CommCellWidth * 4 && CurY > Y_top && CurY < Y_top + CommCelleHeight)
            {
                SysParam.isTracking = true;
                MyOterAlarmWin = new OterAlarmWin(this);
                MyOterAlarmWin.ShowDialog();
                MyAlarmInfoWin = null;
                return;
            }
            int count = 0;
            count = CommonCollection.Areas.Count;
            
            CommCellWidth = (ConstInfor.MainCenter_Width - ConstInfor.PsCell_First_Width - ConstInfor.Table_Left * 2) / ConstInfor.PsCell_RowNum;
            CommCelleHeight = (ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom - ConstInfor.Cross_Ban_H - 5) / (ConstInfor.PsCell_ColNum);
            X_left = ConstInfor.Table_Left + ConstInfor.PsCell_First_Width;
            Y_top = ConstInfor.Table_Top + CommCelleHeight;
            for (int i = 0; i < SysParam.GroupShows.Length; i++)
            {
                if (null != SysParam.GroupShows)
                {
                    if (CurX > X_left + CommCellWidth * i && CurX < X_left + CommCellWidth * (i + 1) && CurY > Y_top - CommCelleHeight && CurY < Y_top)
                    {
                        if (i == DrawIMG.SetShowGroupSelected(i))
                        {
                            DrawIMG.DrawMainCenter(MainCenter_G);
                            MainCenter_Panel_Paint(null,null);
                        }
                    }
                }
            }
            //确定用户按下了哪一个区域
            int index = 0;
            foreach (Area area in SysParam.CurAreas)
            {
                if (area == null)
                {
                    break;
                }
                if (CurX > X_left + (index % 4) * CommCellWidth && CurX < X_left + ((index % 4) + 1) * CommCellWidth && CurY > Y_top + (index / 4) * CommCelleHeight * 2 + CommCelleHeight && CurY < Y_top + (index / 4) * CommCelleHeight * 2 + CommCelleHeight*2)
                {
                    String StrAreaID = area.ID[0].ToString("X2") + area.ID[1].ToString("X2");
                    MyRegInfoWin = new RegInfoWin(this, StrAreaID);
                    MyRegInfoWin.ShowDialog();
                    MyRegInfoWin = null;
                    return;
                }
                index++;
            }
            //判断是否按下总区域框
            if (CurX > X_left && CurX < X_left + CommCellWidth * ConstInfor.PsCell_RowNum && CurY > Y_top + CommCelleHeight * (ConstInfor.PsCell_ColNum - 2) && CurY < Y_top + CommCelleHeight * (ConstInfor.PsCell_ColNum-1))
            {
                MyAllRegInfoWin = new AllRegInfoWin(this, SpeceilAlarm.UnKnown, "");
                MyAllRegInfoWin.ShowDialog();
                MyAllRegInfoWin = null;
                return;
            }else if (CurX > X_left - ConstInfor.AlarmCell_First_Width && CurX < X_left && CurY > Y_top - CommCelleHeight && CurY < Y_top)
            {    //鼠标移动到两端的箭头时，可点击
                SysParam.isLeftArrowDown = false;
                CommonBoxOperation.MoveLeftShowGroupsItem();
                DrawIMG.DrawMainCenter(MainCenter_G);
                MainCenter_Panel_Paint(null, null);
            }
            else if (CurX > X_left + CommCellWidth * 3 && CurX < X_left + CommCellWidth * 4 && CurY > Y_top - CommCelleHeight && CurY < Y_top)
            {
                SysParam.isRightArrowDown = false;
                CommonBoxOperation.MoveRightShowGroupsItem();
                DrawIMG.DrawMainCenter(MainCenter_G);
                MainCenter_Panel_Paint(null, null);
            }
        }
        /// <summary>
        /// 鼠标按下触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainCenter_Panel_MouseDown(object sender, MouseEventArgs e)
        {
            int CurX, CurY;
            CurX = e.X;
            CurY = e.Y;
            int CommCellWidth = CommCellWidth = (ConstInfor.MainCenter_Width - ConstInfor.PsCell_First_Width - ConstInfor.Table_Left * 2) / ConstInfor.PsCell_RowNum;
            int CommCelleHeight = (ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom - ConstInfor.Cross_Ban_H - 5) / (ConstInfor.PsCell_ColNum);
            int X_left = ConstInfor.Table_Left + ConstInfor.PsCell_First_Width;
            int Y_top = ConstInfor.Table_Top + CommCelleHeight;
            if (CurX > X_left - ConstInfor.AlarmCell_First_Width && CurX < X_left && CurY > Y_top - CommCelleHeight && CurY < Y_top)
            {
                SysParam.isLeftArrowDown = true;
            }
            else if (CurX > X_left + CommCellWidth * 3 && CurX < X_left + CommCellWidth * 4 && CurY > Y_top - CommCelleHeight && CurY < Y_top)
            {
                SysParam.isRightArrowDown = true;
            }
            DrawIMG.DrawMainCenter(MainCenter_G);
            MainCenter_Panel_Paint(null, null);
        }
        private void MainCenter_Panel_MouseUp(object sender, MouseEventArgs e)
        {
            if (SysParam.isRightArrowDown)
            {
                SysParam.isRightArrowDown = false;
            }
            if (SysParam.isLeftArrowDown)
            {
                SysParam.isLeftArrowDown = false;
            }
            DrawIMG.DrawMainCenter(MainCenter_G);
            MainCenter_Panel_Paint(null, null);
        }
        /// <summary>
        /// 鼠标移动到可点击的部分，鼠标变为小手状
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainCenter_Panel_MouseMove(object sender, MouseEventArgs e)
        {
            bool isHand = false;
            int CurX, CurY;
            CurX = e.X;
            CurY = e.Y;
            int CommCellWidth = (ConstInfor.MainCenter_Width - ConstInfor.AlarmCell_First_Width - ConstInfor.Table_Left * 2) / ConstInfor.AlarmCell_RowNum;
            int CommCelleHeight = (ConstInfor.Alarm_TB_Bottom - ConstInfor.AlarmBtt - ConstInfor.Cross_Ban_H) / (ConstInfor.AlarmCell_ColNum);
            int X_left = ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width;
            int Y_top = ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H + CommCelleHeight;
            if (CurX > X_left && CurX < X_left + CommCellWidth && CurY > Y_top && CurY < Y_top + CommCelleHeight)
            {
                MainCenter_Panel.Cursor = Cursors.Hand;
                isHand = true;
            }
            else if (CurX > X_left + CommCellWidth && CurX < X_left + CommCellWidth * 2 && CurY > Y_top && CurY < Y_top + CommCelleHeight)
            {
                MainCenter_Panel.Cursor = Cursors.Hand;
                isHand = true;
            }
            else if (CurX > X_left + CommCellWidth * 2 && CurX < X_left + CommCellWidth * 3 && CurY > Y_top && CurY < Y_top + CommCelleHeight)
            {
                MainCenter_Panel.Cursor = Cursors.Hand;
                isHand = true;
            }
            else if (CurX > X_left + CommCellWidth * 3 && CurX < X_left + CommCellWidth * 4 && CurY > Y_top && CurY < Y_top + CommCelleHeight)
            {
                MainCenter_Panel.Cursor = Cursors.Hand;
                isHand = true;
            }

            CommCellWidth = CommCellWidth = (ConstInfor.MainCenter_Width - ConstInfor.PsCell_First_Width - ConstInfor.Table_Left * 2) / ConstInfor.PsCell_RowNum;
            CommCelleHeight = (ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom - ConstInfor.Cross_Ban_H - 5) / (ConstInfor.PsCell_ColNum);

            X_left = ConstInfor.Table_Left + ConstInfor.PsCell_First_Width;
            Y_top = ConstInfor.Table_Top;

            for (int i = 0; i < SysParam.GroupShows.Length; i++)
            {
                if (null != SysParam.GroupShows)
                {
                    if (CurX > X_left + CommCellWidth * i && CurX < X_left + CommCellWidth * (i + 1) && CurY > Y_top && CurY < Y_top + CommCelleHeight)
                    {
                        MainCenter_Panel.Cursor = Cursors.Hand;
                        isHand = true;
                    }
                }
            }
            
			int count = CommonCollection.Areas.Count;
            
            Y_top = ConstInfor.Table_Top + CommCelleHeight;
            int index = 0;
            foreach (Area area in SysParam.CurAreas)
            {
                if (area == null)
                {
                    break;
                }
                if (CurX > X_left + (index % 4) * CommCellWidth && CurX < X_left + ((index % 4) + 1) * CommCellWidth && CurY > Y_top + (index / 4) * CommCelleHeight * 2 + CommCelleHeight && CurY < Y_top + (index / 4) * CommCelleHeight * 2 + CommCelleHeight * 2)
                {
                    MainCenter_Panel.Cursor = Cursors.Hand;
                    isHand = true;
                }
                index++;
            }
            if (CurX > X_left && CurX < X_left + CommCellWidth * ConstInfor.PsCell_RowNum && CurY > Y_top + CommCelleHeight * (ConstInfor.PsCell_ColNum - 2) && CurY < Y_top + CommCelleHeight * (ConstInfor.PsCell_ColNum - 1))
            {
                MainCenter_Panel.Cursor = Cursors.Hand;
                isHand = true;
            }
            else
            {
                //鼠标移动到两端的箭头时，可点击
                if (CurX > X_left - ConstInfor.AlarmCell_First_Width && CurX < X_left && CurY > Y_top - CommCelleHeight && CurY < Y_top)
                {
                    MainCenter_Panel.Cursor = Cursors.Hand;
                    isHand = true;
                }
                else if (CurX > X_left + CommCellWidth * 3 && CurX < X_left + CommCellWidth * 4 && CurY > Y_top - CommCelleHeight && CurY < Y_top)
                {
                    MainCenter_Panel.Cursor = Cursors.Hand;
                    isHand = true;
                }
            }
            if (!isHand)
            {
                MainCenter_Panel.Cursor = Cursors.Default;
            }
        }
        /// <summary>
        /// //画出主页面的中心
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MainCenter_Panel_Paint(object sender, PaintEventArgs e)
        {
            if (null != MainCenterBitmap)
            {
                try
                {
                    MainCenter_Panel.CreateGraphics().DrawImageUnscaled(MainCenterBitmap, 0, 0);
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        private void Stop()
        {
            //关闭信号线程
           
            DrawIMG.Stop();
            if (SoundOperation.isSoundPlay)
            {
                SoundOperation.StopSoundPlay();
            }
        }
        /// <summary>
        /// 窗体关闭时，1、释放系统资源。2、保存设置的文件信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Stop();
                MyTimer.Stop();
                DrawIMG.StopThread();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                //将警报的讯息序列化到文件中
                SysParam.SaveOriginalAlarm();
                //保存数据记录
                SysParam.SaveRecord();
                //保存操作记录
                SysParam.SaveOperationRecord();
            }
        }
        private void MyTimer_Tick(Object obj,EventArgs args)
        {
            
            DrawIMG.DrawMainCenter(MainCenter_G);
            SysParam.AlarmFontChangeInt++;
            MainCenter_Panel_Paint(null,null);
            //因为定时器每隔1300秒刷新一次
            MeasurTick += 1.3f;
            #region 检测添加的设备是否有上报ID
            /**对于Refer和Node需要我们进行两次判断**
             * 1、判断已经添加的Refer和Node是否已经上报ID及版本上来。
             * 2、判断之前上报ID及版本的Refer和Node是否已经断开连接。
             * */
            //检查区域中的参考点及节点是否上报，这里设置一个固定的时间60s检测一次，避免每次都去检测，减少主线程的处理时间
            if (Environment.TickCount - SysParam.CurrentTickCount > 60000)
            {
                SysParam.CurrentTickCount = Environment.TickCount;
                Router rprouter = null;
                foreach (KeyValuePair<string, Area> marea in CommonCollection.Areas)
                {
                    if (null == marea.Value)
                    {
                        continue;
                    }
                    foreach (KeyValuePair<string, BasicRouter> mrouter in marea.Value.AreaRouter)
                    {
                        if (null == mrouter.Value)
                        {
                            continue;
                        }
                        if (CommonCollection.Routers.TryGetValue(mrouter.Key, out rprouter))
                        {
                            if (rprouter.CurType == NodeType.ReferNode)
                            {
                                mrouter.Value.isReport = true;
                                mrouter.Value.NoReportTick = 0;
                            }
                            else
                            {
                                rprouter = null;
                            }
                        }
                        if (null == rprouter)
                        {
                            mrouter.Value.NoReportTick += 60000;
                            if (mrouter.Value.NoReportTick >= 60 * SysParam.RouterParam1 * 1000 + SysParam.RouterParam2 * 1000)
                            {//开始上报警报讯息
                                WarmInfo wminfor = CommonBoxOperation.GetWarmItem(mrouter.Key, SpeceilAlarm.ReferDis);
                                if (null == wminfor || wminfor.isHandler)
                                {//产生新的Refer断开警报讯息
                                    wminfor = new ReferDis();
                                    System.Buffer.BlockCopy(mrouter.Value.ID, 0, wminfor.RD, 0, 2);
                                    System.Buffer.BlockCopy(marea.Value.ID, 0, wminfor.AD, 0, 2);
                                    wminfor.AlarmTime = DateTime.Now;
                                    wminfor.ClearAlarmTime = DateTime.Now;
                                    wminfor.isHandler = false;
                                    wminfor.isClear = false;
                                    CommonCollection.WarmInfors.Add(wminfor);
                                }
                                else
                                {
                                    mrouter.Value.NoReportTick = mrouter.Value.NoReportTick - (60 * SysParam.RouterParam1 * 1000 + SysParam.RouterParam2 * 1000);
                                }
                                if (SysParam.IsSoundAlarm && SysParam.isSoundDeviceTrouble)
                                {
                                    if (SysParam.SoundName == ConstInfor.DefaultSoundAlarm)
                                    {
                                        if (!SoundOperation.isSoundPlay)
                                        {
                                            SoundOperation.DefaultSoundPlay(SysParam.SoundTime);
                                        }
                                        else
                                        {
                                            SoundOperation.PlayCount++;
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (null == DefinePlay.axMediaplayer)
                                            {
                                                AxWMPLib.AxWindowsMediaPlayer axplayer = new AxWMPLib.AxWindowsMediaPlayer();
                                                axplayer.Visible = false;
                                                axplayer.BeginInit();
                                                this.Controls.Add(axplayer);
                                                axplayer.EndInit();
                                                if (SysParam.SoundTime == 0)
                                                {
                                                    DefinePlay.SinglePlay(axplayer, FileOperation.SoundPath + "\\" + SysParam.SoundName);
                                                }
                                                else
                                                {
                                                    DefinePlay.Play(axplayer, FileOperation.SoundPath + "\\" + SysParam.SoundName, SysParam.SoundTime);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            FileOperation.WriteLog("声音报警异常，异常原因: " + ex.ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                    foreach (KeyValuePair<string, BasicNode> node in marea.Value.AreaNode)
                    {
                        if (null == node.Value)
                        {
                            continue;
                        }
                        if (CommonCollection.Routers.TryGetValue(node.Key, out rprouter))
                        {
                            if (rprouter.CurType == NodeType.DataNode)
                            {
                                node.Value.isReport = true;
                                node.Value.NoReportTick = 0;
                            }
                            else
                            {
                                rprouter = null;
                            }
                        }
                        routerNull(rprouter, node, marea);
                    }
                }
                //定时清理已经被处理掉的警报讯息
            }
            #endregion
            if (MeasurTick >= SysParam.Measure_Interval)
            {//检测Tag是否有断开连接，若发现断开连接，则产生一次警告资讯
                #region  检测Tag是否断开连接
                strdeletes.Clear();
                try
                {
                    foreach (KeyValuePair<string,TagPack> tp in CommonCollection.TagPacks)
                    {
                        if (null == tp.Value)
                            continue;
                        int waitsleep = 0;
                        if (tp.Value.ResTime > 3 && tp.Value.StaticSleep > tp.Value.Sleep)
                        {
                            waitsleep = tp.Value.StaticSleep;
                        }  else  {
                            waitsleep = tp.Value.Sleep;
                        }
                        // 判断设备是否断开连接，需要确定设备是否在工作时间中，若不在工作时间是不会接收数据的
                        if (Environment.TickCount - tp.Value.TickCount > waitsleep * 100 * SysParam.TagDisParam1 + SysParam.TagDisParam2 * 1000)
                        {// Tag断开连接
                            strdeletes.Add(tp.Key);//将Tag添加到一个删除缓存中
                            WarmInfo winfo = CommonBoxOperation.GetWarmItem(tp.Key,SpeceilAlarm.TagDis);
                            // 产生tag断开的警报，或者发现Tag断开的警报已经被处理了
                            if (null == winfo || winfo.isHandler)
                            {
                                winfo = new TagDis();
                                ((TagDis)winfo).TD[0] = tp.Value.TD[0];
                                ((TagDis)winfo).TD[1] = tp.Value.TD[1];
                                winfo.RD[0] = tp.Value.RD_New[0];
                                winfo.RD[1] = tp.Value.RD_New[1];
                                string StrRouterID = winfo.RD[0].ToString("X2") + winfo.RD[1].ToString("X2");
                                string StrRouterName = CommonBoxOperation.GetRouterName(StrRouterID);
                                if (null != StrRouterName && !"".Equals(StrRouterName))
                                {
                                    winfo.RDName = StrRouterName;
                                }
                                Area TagDisArea = CommonBoxOperation.GetAreaFromRouterID(StrRouterID);
                                if (null != TagDisArea)
                                {
                                    winfo.AD[0] = TagDisArea.ID[0];
                                    winfo.AD[1] = TagDisArea.ID[1];
                                    winfo.AreaName = TagDisArea.Name;
                                }
                                ((TagDis)winfo).SleepTime = tp.Value.Sleep;
                                winfo.isHandler = false;
                                winfo.AlarmTime = DateTime.Now;
                                winfo.ClearAlarmTime = winfo.AlarmTime;
                                string StrTagName = CommonBoxOperation.GetTagName(tp.Key);
                                if (null != StrTagName && !"".Equals(StrTagName))
                                {
                                    ((TagDis)winfo).TagName = StrTagName;
                                }
                                CommonCollection.WarmInfors.Add(winfo);
                            }
                            else
                            {
                                tp.Value.TickCount = Environment.TickCount - (waitsleep * 1000 * SysParam.TagDisParam1 + SysParam.TagDisParam2 * 1000);
                            }
                            if (SysParam.IsSoundAlarm && SysParam.isSoundDeviceTrouble)
                            {
                                defaultSound();
                            }
                         }
                    }
                }catch(Exception e)
                {
                    FileOperation.WriteLog(DateTime.Now + " 檢測Tag是否斷開時出現異常！異常原因:" + e.ToString());
                }
                
                //从列表中删除已经断开的Tag
                TagPack tpk = null;
                if (strdeletes.Count > 0)
                {                    
                    foreach (string str in strdeletes)
                    {
                        CommonCollection.TagPacks.TryRemove(str, out tpk);
                    }
                }

                Dictionary<string, Tag> mTags = new Dictionary<string, PersionAutoLocaSys.Tag>(CommonCollection.Tags);
                foreach(var tagItem in mTags)
                {
                    if (tagItem.Value.EndWorkDT.CompareTo(DateTime.Now) > 0) continue;
                    String tagID = tagItem.Key;
                    if (CommonCollection.TagPacks.ContainsKey(tagID))
                    {
                        CommonCollection.TagPacks.TryRemove(tagID, out tpk);
                    }
                }

                #endregion
                #region 检测已经上报ID的Router\Node设备是否有断开的情况
                foreach (KeyValuePair<string,Router> rt in CommonCollection.Routers)
                {
                    if (null == rt.Value)
                        continue;
                    if (Environment.TickCount - rt.Value.TickCount > rt.Value.SleepTime * SysParam.RouterParam1 * 1000 + SysParam.RouterParam2 * 1000)
                    {
                            string StrRouterID = rt.Value.ID[0].ToString("X2") + rt.Value.ID[1].ToString("X2");
                            string StrRouterName = CommonBoxOperation.GetRouterName(StrRouterID);
                            Area RdArea = CommonBoxOperation.GetAreaFromRouterID(StrRouterID);
                            //参考点断开
                            if (rt.Value.CurType == NodeType.ReferNode)
                            {
                                WarmInfo warm = CommonBoxOperation.GetWarmItem(rt.Key,SpeceilAlarm.ReferDis);
                                if (null == warm || warm.isHandler)
                                {
                                    warm = new ReferDis();
                                    System.Buffer.BlockCopy(rt.Value.ID, 0, warm.RD, 0, 2);

                                    if (null != StrRouterName && !"".Equals(StrRouterName))
                                    {
                                        warm.RDName = StrRouterName;
                                    }

                                    if (null != RdArea)
                                    {
                                        System.Buffer.BlockCopy(RdArea.ID, 0, warm.AD, 0, 2);
                                        warm.RDName = RdArea.Name;
                                    }

                                    ((ReferDis)warm).SleepTime = rt.Value.SleepTime;
                                    warm.AlarmTime = DateTime.Now;
                                    warm.ClearAlarmTime = warm.AlarmTime;
                                    warm.isHandler = false;
                                    CommonCollection.WarmInfors.Add(warm);
                                }
                                else
                                {
                                    rt.Value.TickCount = Environment.TickCount - (rt.Value.SleepTime * SysParam.RouterParam1 * 1000 + SysParam.RouterParam2 * 1000);
                                }
                                if (SysParam.IsSoundAlarm && SysParam.isSoundDeviceTrouble)
                                {
                                    if (SysParam.SoundName == ConstInfor.DefaultSoundAlarm)
                                    {
                                        if (!SoundOperation.isSoundPlay)
                                        {
                                            SoundOperation.DefaultSoundPlay(SysParam.SoundTime);
                                        }
                                        else
                                        {
                                            SoundOperation.PlayCount++;
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (null == DefinePlay.axMediaplayer)
                                            {
                                                AxWMPLib.AxWindowsMediaPlayer axplayer = new AxWMPLib.AxWindowsMediaPlayer();
                                                axplayer.Visible = false;
                                                axplayer.BeginInit();
                                                this.Controls.Add(axplayer);
                                                axplayer.EndInit();
                                                if(SysParam.SoundTime == 0)
                                                {
                                                    DefinePlay.SinglePlay(axplayer, FileOperation.SoundPath + "\\" + SysParam.SoundName);
                                                }
                                                else
                                                {
                                                    DefinePlay.Play(axplayer, FileOperation.SoundPath + "\\" + SysParam.SoundName, SysParam.SoundTime);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            FileOperation.WriteLog("声音报警异常，异常原因: " + ex.ToString());
                                        }
                                    }
                                }
                            }
                            else if (rt.Value.CurType == NodeType.DataNode)
                            {//数据节点断开连接
                                WarmInfo warm = CommonBoxOperation.GetWarmItem(rt.Key, SpeceilAlarm.NodeDis);
                                if (null == warm || warm.isHandler)
                                {
                                    warm = new NodeDis();
                                    System.Buffer.BlockCopy(rt.Value.ID, 0, warm.RD, 0, 2);
                                    if (null != StrRouterName && !"".Equals(StrRouterName))
                                    {
                                        warm.RDName = StrRouterName;
                                    }
                                    if (null != RdArea)
                                    {
                                        System.Buffer.BlockCopy(RdArea.ID, 0, warm.AD, 0, 2);
                                        warm.AreaName = RdArea.Name;
                                    }
                                    ((NodeDis)warm).SleepTime = rt.Value.SleepTime;
                                    warm.AlarmTime = DateTime.Now;
                                    warm.ClearAlarmTime = warm.AlarmTime;
                                    warm.isHandler = false;
                                    CommonCollection.WarmInfors.Add(warm);
                                }
                                else
                                {
                                    rt.Value.TickCount = Environment.TickCount - (rt.Value.SleepTime * SysParam.RouterParam1 * 1000 + SysParam.RouterParam2 * 1000);
                                }
                                if (SysParam.IsSoundAlarm && SysParam.isSoundDeviceTrouble)
                                {
                                    if (SysParam.SoundName == ConstInfor.DefaultSoundAlarm)
                                    {
                                        SoundOperation.DefaultSoundPlay(SysParam.SoundTime);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (null == DefinePlay.axMediaplayer)
                                            {
                                                AxWMPLib.AxWindowsMediaPlayer axplayer = new AxWMPLib.AxWindowsMediaPlayer();
                                                axplayer.Visible = false;
                                                axplayer.BeginInit();
                                                this.Controls.Add(axplayer);
                                                axplayer.EndInit();
                                                if (SysParam.SoundTime == 0)
                                                {
                                                    DefinePlay.SinglePlay(axplayer, FileOperation.SoundPath + "\\" + SysParam.SoundName);
                                                }
                                                else
                                                {
                                                    DefinePlay.Play(axplayer, FileOperation.SoundPath + "\\" + SysParam.SoundName, SysParam.SoundTime);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            FileOperation.WriteLog(DateTime.Now + " " + ex.ToString());
                                        }
                                    }
                                }
                            }
                            else
                            {//其他节点断开连接

                            }
                            rt.Value.isHandler = false;
                            rt.Value.status = false;
                     }
                }
                #endregion
                MeasurTick = 0;
            }
            #region 判断是否关闭警报讯息
            lock (CommonCollection.warm_lock)
            {
                TempWarmes = CommonCollection.WarmInfors.ToArray();
            }
            if (TempWarmes.Length > 0)
            {
                if (SoundOperation.isSoundPlay || DefinePlay.axMediaplayer != null)
                {   //警报在响
                    bool mark = false;
                    //判断是否所有的警报都处理完了，都处理完后，警报停止
                    foreach (WarmInfo wm in TempWarmes)
                    {//首先我们需要判断警报是否已经处理
                        if (!wm.isHandler)
                        {//当前警告还未处理，我们需要判断警报类型和设置的警报是否进行声音报警
                            if ("PersonHelp".Equals(wm.GetType().Name) && SysParam.isSoundPersonHelp)
                            {//人员求救警报未处理并且可以声音报警,只要发现有一个可以报警，就可以直接跳出循环
                                mark = true;
                                break;
                            }
                            else if ("AreaAdmin".Equals(wm.GetType().Name) && SysParam.isSoundAreaControl)
                            {
                                mark = true;
                                break;
                            }
                            else if ("PersonRes".Equals(wm.GetType().Name) && SysParam.isSoundPersonRes)
                            {
                                mark = true;
                                break;
                            }
                            else if ("BattLow".Equals(wm.GetType().Name) && SysParam.isSoundBatteryLow)
                            {
                                mark = true;
                                break;
                            }
                            else if (("TagDis".Equals(wm.GetType().Name) || "ReferDis".Equals(wm.GetType().Name) || "NodeDis".Equals(wm.GetType().Name)) && SysParam.isSoundDeviceTrouble)
                            {
                                mark = true;
                                break;
                            }
                        }
                    }
                    //所有的警告讯息都处理了关闭声音
                    if (!mark)
                    {
                        if (SoundOperation.isSoundPlay)
                        {
                            SoundOperation.StopSoundPlay();
                        }
                        else
                        {
                            DefinePlay.RemoveMediaPlayer(this);
                            DefinePlay.Close();
                        }
                    }
                }
            }
            #endregion
            #region 进入自动清理警报讯息
            if (SysParam.isClearHandleAlarm)
            {
                //设置一个固定的间隔时间80s检测一次是否需要清理警报讯息,与前面的时间错开
                if (Environment.TickCount - SysParam.AutoClearTickCount >= 80000)
                {
                   // Console.WriteLine("检测是否清除警告资讯时间已到...");
                    SysParam.AutoClearTickCount = Environment.TickCount;
                    deleteobjs.Clear();
                    for (int i = 0; i < TempWarmes.Length; i++)
                    {
                        if (TempWarmes[i].isHandler)
                        {
                            totals = (int)(DateTime.Now - TempWarmes[i].ClearAlarmTime).TotalSeconds;
                            if (totals >= SysParam.AutoClearHandleAlarmTime)
                            {//说明此时可以清除警告资讯了
                                TempWarmes[i].isClear = true;
                                deleteobjs.Add(TempWarmes[i]);
                                lock (CommonCollection.warm_lock)
                                {
                                    CommonCollection.WarmInfors.Remove(TempWarmes[i]);
                                }
                            }
                        }
                    }
                    //所有的警告都检查完了后，先将那些要删除的资讯保存起来再清理掉这些资讯
                    if (deleteobjs.Count > 0)
                    {
                    
                        List<WarmInfo> listbox = null;
                        string warmpath = FileOperation.WarmMsg + @"\" + FileOperation.WarmName;
                        if (File.Exists(warmpath))
                        {
                            listbox = FileOperation.GetWarmData(warmpath);
                        }
                        for (int i = 0; i < deleteobjs.Count; i++)
                        {
                            listbox.Add((WarmInfo)deleteobjs[i]);
                        }
                        if (null != listbox && listbox.Count > 0)
                        {
                           // Console.WriteLine("一共保存" + listbox.Count + " 条警告讯息...");
                            FileOperation.SaveWarmData(listbox, warmpath);
                        }
                        try
                        {
                            //只要清除了资讯就重新清理视图,这个必须要，否则视图显示会出现异常
                            if (null != MyAlarmInfoWin && !MyAlarmInfoWin.IsDisposed)
                            {
                                MyAlarmInfoWin.PersonAlarmListView.Items.Clear();
                            }
                            if (null != MyOterAlarmWin && !MyOterAlarmWin.IsDisposed)
                            {
                                MyOterAlarmWin.BattLowListView.Items.Clear();
                                MyOterAlarmWin.TagDisListView.Items.Clear();
                                MyOterAlarmWin.ReferDisListView.Items.Clear();
                                MyOterAlarmWin.NodeDisListView.Items.Clear();
                            }
                        }catch(Exception e1)
                        {
                            FileOperation.WriteLog("自动清理警告讯息,清理视图时出现异常!异常原因:"+e1.ToString());
                        }
                    }
                }
            }
            #endregion
            #region 进入短信报警
            if (SysParam.isMsgSend)
            {
                for (int i = 0; i < TempWarmes.Length; i++)
                {
                    String ClassName = TempWarmes[i].GetType().Name;
                    //判断是否需要进行短信报警
                    if (TempWarmes[i].isSendMsg)
                        continue;
                    if ("PersonHelp".Equals(ClassName) && SysParam.isSendPersonHelpMsg)
                    {
                        lock (SysParam.SendMsgList_Lock)
                        {
                            SysParam.SendMsgList.Add(TempWarmes[i]);
                            TempWarmes[i].isSendMsg = true;
                        }
                    }
                    else if ("AreaAdmin".Equals(ClassName) && SysParam.isSendAreaControlMsg)
                    {
                        lock (SysParam.SendMsgList_Lock)
                        {
                            SysParam.SendMsgList.Add(TempWarmes[i]);
                            TempWarmes[i].isSendMsg = true;
                        }
                    }
                    else if ("PersonRes".Equals(ClassName) && SysParam.isSendPersonResMsg)
                    {
                        lock (SysParam.SendMsgList_Lock)
                        {
                            SysParam.SendMsgList.Add(TempWarmes[i]);
                            TempWarmes[i].isSendMsg = true;
                        }
                    }
                    else if ("BattLow".Equals(ClassName) && SysParam.isSendBatteryLowMsg)
                    {
                        lock (SysParam.SendMsgList_Lock)
                        {
                            SysParam.SendMsgList.Add(TempWarmes[i]);
                            TempWarmes[i].isSendMsg = true;
                        }
                    }
                    else if ("TagDis".Equals(ClassName) && SysParam.isSendDeviceTrouble)
                    {
                        lock (SysParam.SendMsgList_Lock)
                        {
                            SysParam.SendMsgList.Add(TempWarmes[i]);
                            TempWarmes[i].isSendMsg = true;
                        }
                    }
                    else if ("ReferDis".Equals(ClassName) && SysParam.isSendDeviceTrouble)
                    {
                        lock (SysParam.SendMsgList_Lock)
                        {
                            SysParam.SendMsgList.Add(TempWarmes[i]);
                            TempWarmes[i].isSendMsg = true;
                        }
                    }
                    else if ("NodeDis".Equals(ClassName) && SysParam.isSendDeviceTrouble)
                    {
                        lock (SysParam.SendMsgList_Lock)
                        {
                            SysParam.SendMsgList.Add(TempWarmes[i]);
                            TempWarmes[i].isSendMsg = true;
                        }
                    }
                }
            }
            #endregion
        }




        private void routerNull(Router rprouter, KeyValuePair<string, BasicNode> node, KeyValuePair<string, Area> marea)
        {
            if (null == rprouter)
            {
                node.Value.NoReportTick += 60000;//每隔60s扫描一次
                if (node.Value.NoReportTick >= 60 * SysParam.RouterParam1 * 1000 + SysParam.RouterParam2 * 1000)
                {//开始上报警报讯息
                    WarmInfo wminfor = CommonBoxOperation.GetWarmItem(node.Key, SpeceilAlarm.NodeDis);
                    if (null == wminfor || wminfor.isHandler)
                    {
                        wminfor = new NodeDis();
                        System.Buffer.BlockCopy(node.Value.ID, 0, wminfor.RD, 0, 2);
                        System.Buffer.BlockCopy(marea.Value.ID, 0, wminfor.AD, 0, 2);
                        wminfor.AlarmTime = DateTime.Now;
                        wminfor.isHandler = false;
                        wminfor.isClear = false;
                        wminfor.ClearAlarmTime = DateTime.Now;
                        CommonCollection.WarmInfors.Add(wminfor);
                    }
                    else
                    {//这样就能保证再进过指定的时间后还能检测断开状态
                        node.Value.NoReportTick = node.Value.NoReportTick - (60 * SysParam.RouterParam1 * 1000 + SysParam.RouterParam2 * 1000);
                    }
                    if (SysParam.IsSoundAlarm && SysParam.isSoundDeviceTrouble)
                    {
                        defaultSound();
                    }
                }
            }
        }

        private void defaultSound()
        {
            if (SysParam.SoundName == ConstInfor.DefaultSoundAlarm)
            {
                if (!SoundOperation.isSoundPlay)
                {
                    SoundOperation.DefaultSoundPlay(SysParam.SoundTime);
                }
                else
                {
                    SoundOperation.PlayCount++;
                }
            }
            else
            {
                try
                {
                    if (null == DefinePlay.axMediaplayer)
                    {
                        AxWMPLib.AxWindowsMediaPlayer axplayer = new AxWMPLib.AxWindowsMediaPlayer();
                        axplayer.Visible = false;
                        axplayer.BeginInit();
                        this.Controls.Add(axplayer);
                        axplayer.EndInit();
                        if (SysParam.SoundTime == 0)
                        {
                            DefinePlay.SinglePlay(axplayer, FileOperation.SoundPath + "\\" + SysParam.SoundName);
                        }
                        else
                        {
                            DefinePlay.Play(axplayer, FileOperation.SoundPath + "\\" + SysParam.SoundName, SysParam.SoundTime);
                        }
                    }
                }
                catch (Exception ex)
                {
                    FileOperation.WriteLog("聲音警報時出現異常！异常原因: " + ex.ToString());
                }
            }
        }


        private void MainTitle_Panel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DrawIMG.Paint_SelectBtn(MainTitle_G, BtnStatus.Bt_Double);
            Console.Write("---====---");
        }

    }

}
