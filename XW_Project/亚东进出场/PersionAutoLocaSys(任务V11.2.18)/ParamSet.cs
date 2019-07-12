using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Collections;
using System.Globalization;
using System.Threading;

using MG3732_MSG;
using System.Net.Sockets;
using PersionAutoLocaSys.Model;
namespace PersionAutoLocaSys
{
    public partial class ParamSet : Form
    {
        public Form1 frm = null;

        public EditRouter MyEditRouter = null;
        public Bitmap AreaBitmap = null;
        private bool isplaymedia = false;
        public ParamSet()
        {
            InitializeComponent();
        }

        public ParamSet(Form1 frm)
        {
            InitializeComponent();
            this.frm = frm;
            SysParam.Pst = this;
        }
        private void Param_ListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(Param_ListBox.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
        }

        private void Param_ListBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 20;
        }

        private void ParamSet_Load(object sender, EventArgs e)
        {
            Param_ListBox.SelectedIndex = 0;
            this.MaximumSize = new Size(975, 618);
            this.MinimumSize = new Size(975, 618);
            //设置默认的优化值
            if (SysParam.curOptimalMedol == OptimizationModel.HopTimes)
            {
                OptModelCB.SelectedIndex = 1;
            }
            else
            {
                OptModelCB.SelectedIndex = 0;
            }
            startTP.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            EndTP.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            gsstartdtp.CustomFormat = "HH:mm";
            gsenddtp.CustomFormat = "HH:mm";

            AreaBitmap = new Bitmap(ShowAreaMapPanel.Width,ShowAreaMapPanel.Height);
            //刷新组别列表
            GroupPanel.Location = new Point(132, 12);
            UpdateGroupListView();
            SysParam.InitNetParam();
            SysParam.InitAreaPara();
            SysParam.InitRouterPara();
            TagPanel.Location = new Point(132, 12);
            AlarmPanel.Location = new Point(132, 12);
            PersonAdminPanel.Location = new Point(132,12);
            this.Location = new Point(frm.Location.X + (frm.Width - this.Width) / 2, frm.Location.Y + (frm.Height - this.Height) / 2);
            SigThreCB.SelectedIndex = SysParam.RssiThreshold;
            SysParam.InitAllParams();
            this.Text = "當前管理人員為:" + frm.CurPerson.Name;

            if (frm.CurPerson.ID[0] == 0xFF && frm.CurPerson.ID[1] == 0xFF)
            {
                debuggroupbox.Visible = true;
            }
            else
            {
                debuggroupbox.Visible = false;
            }
            debugcb.Checked = SysParam.isDebug;
            if (SysParam.isDebug)
            {
                setarroundcb.Visible = true;
                setarroundtx.Visible = true;
                setarroundcb.Checked = SysParam.isSettingArroundDevices;
            }
            else
            {
                setarroundcb.Visible = false;
                setarroundtx.Visible = false;
            }
            if (SysParam.isDevCnnLoss)
            {
                devcnnalarmmisscb.Checked = true;
            }
            else
            {
                devcnnalarmmisscb.Checked = false;
            }
        }

        private void Param_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SysParam.SetPanelStatus(Param_ListBox.SelectedIndex);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == ConstInfor.FORMMESSAGE && (int)m.WParam == ConstInfor.CLOSEMSGPARAM)
            {                
                //SettingWin窗体退出时，记录退出讯息
                PersonOperation curpersonoper = new PersonOperation(frm.CurPerson.ID, OperType.LeaveSetting);
                CommonCollection.personOpers.Add(curpersonoper);
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// 向集合中添加区域
        /// </summary>
        /// <param name="sender"></param> 
        /// <param name="e"></param>
        private void AddAreaBtn_Click(object sender, EventArgs e)
        {
            AreaIDTX.Enabled = true;
            AreaIDTX.Text = "0000";
            if (AreaGroupCB.Items.Count > 0)
            {
                AreaGroupCB.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 删除列表中的项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleAreaBtn_Click(object sender, EventArgs e)
        {
            string StrAreaID = AreaIDTX.Text;
            if ("".Equals(StrAreaID))
            {
                return;
            }
                Area marea = null;
                if (CommonCollection.Areas.ContainsKey(StrAreaID))
                {
                    CommonCollection.Areas.TryRemove(StrAreaID, out marea);
                }
                else
                {
                    MessageBox.Show("對不起，刪除的區域" + StrAreaID + "不存在!");
                }
            
            UpdateAraeListView();
        }

        /// <summary>
        /// 将编辑框中的内容更新到列表框中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AreaUpdateBtn_Click(object sender, EventArgs e)
        {
           string StrAreaID = AreaIDTX.Text;
           if ("".Equals(StrAreaID))
           {
               MessageBox.Show("對不起,區域ID不能為空!");
               return;
           }
           if (StrAreaID.Length != 4)
           {
               MessageBox.Show("對不起,區域的ID長度為4，例如:ID=0001");
               return;
           }
           if ("0000".Equals(StrAreaID))
           {
               MessageBox.Show("對不起,區域的ID不能為0000!");
               return;
           }
           byte[] AreaID = new byte[2];
           try {
               AreaID[0] = Convert.ToByte(StrAreaID.Substring(0,2),16);
               AreaID[1] = Convert.ToByte(StrAreaID.Substring(2,2),16);
           }catch(Exception)
           {
               MessageBox.Show("對不起,區域的ID格式有誤!");
               return;
           }
           string StrAreaName = AreaNameTX.Text;
           if (DrawIMG.GetLength(StrAreaName) > 14)
           {
               MessageBox.Show("對不起,組別名稱絕對長度不能大于14，其中一個漢字長度為2，默認取前14个字符，若第14个为汉字则取前13个字符!");
               StrAreaName = DrawIMG.Get14Char(StrAreaName);
           }
           string StrAreaMapPath = AreaMapPathTX.Text;
           string StrAreaGroup = AreaGroupCB.Text;

           if (ConstInfor.NoGroup.Equals(StrAreaGroup))
           {
               MessageBox.Show("對不起,區域中的組別不能為空!");
               return;
           }

           if (SysParam.GetAreaNumInGroup(StrAreaGroup.Trim()) > 16)
           {
               MessageBox.Show("對不起，組別" + StrAreaGroup + "中的區域超過16個!");
               return;
           }
           Area area = new Area();
           area.ID[0] = AreaID[0];
           area.ID[1] = AreaID[1];
           area.Name = StrAreaName;
           Group grp = SysParam.GetGroup_IDName(StrAreaGroup);
           if (null != grp)
           {
               System.Buffer.BlockCopy(grp.ID, 0, area.GroupID,0,2);
           }
           else 
           { 
               area.GroupID[0] = 0; 
               area.GroupID[1] = 0;
           }
           area.AreaBitMap.MapPath = StrAreaMapPath;
           //判断地图路径
           area.AreaBitMap.MyBitmap = DrawAreaMap.GetBitmap(FileOperation.MapPath + "\\" + area.AreaBitMap.MapPath);

           if (CommonCollection.Areas.ContainsKey(StrAreaID))
           {
              if (!AreaIDTX.Enabled)
              {
                  Area TempArea = null;
                  //修改
                  CommonCollection.Areas.TryRemove(StrAreaID, out TempArea);
                  if (null != TempArea)
                  {
                     if (null != TempArea.AreaRouter)
                     {
                        area.AreaRouter = TempArea.AreaRouter;
                     }
                  }
                  CommonCollection.Areas.TryAdd(StrAreaID, area);
              }
              else 
              {
                  MessageBox.Show("對不起,添加的項"+StrAreaID+"在列裱中已經存在!");
              }
              UpdateAraeListView();
              return;    
           }
           CommonCollection.Areas.TryAdd(StrAreaID, area);
           AreaIDTX.Enabled = false;
           UpdateAraeListView();
        }

        public void UpdateAraeListView()
        {            
            ListViewItem item = null;
            AreaListView.Items.Clear();
            List<KeyValuePair<string, Area>> list = CommonCollection.Areas.OrderByDescending(c => c.Key).ToList();
            foreach (KeyValuePair<string, Area> area in list)
            {
                if (null == area.Value)
                {
                    continue;
                }
                item = new ListViewItem();
                item.Text = area.Key;
                item.Name = area.Key;
                item.SubItems.Add(area.Value.Name);
                if (area.Value.AreaType == AreaType.SimpleArea) item.SubItems.Add(ConstInfor.StrSimpleArea);
                else if (area.Value.AreaType == AreaType.ControlArea) item.SubItems.Add(ConstInfor.StrControlArea);
                else if (area.Value.AreaType == AreaType.DangerArea) item.SubItems.Add(ConstInfor.StrDangerArea);
                Group grp = SysParam.GetGroup_ID(area.Value.GroupID[0].ToString("X2") + area.Value.GroupID[1].ToString("X2"));
                if (null == grp || (grp.ID[0] == 0 && grp.ID[1] == 0))
                {
                    item.SubItems.Add(ConstInfor.NoGroup);
                }
                else
                {
                    if ("".Equals(grp.Name) || null == grp.Name)
                    {
                        item.SubItems.Add(grp.ID[0].ToString("X2") + grp.ID[1].ToString("X2"));
                    }
                    else
                    {
                        item.SubItems.Add(grp.Name + "(" + grp.ID[0].ToString("X2") + grp.ID[1].ToString("X2") + ")");
                    }
                }
                if (null == area.Value.AreaBitMap || "".Equals(area.Value.AreaBitMap.MapPath))
                {
                    item.SubItems.Add(ConstInfor.NoArea);
                }
                else
                {
                    item.SubItems.Add(area.Value.AreaBitMap.MapPath);
                }
                	AreaListView.Items.Add(item);
                }
            //对区域列表进行排序
            AreaListView.Sort();
            AreaListView.ListViewItemSorter = new ListViewItemComparer(0,AreaListView.Sorting);
            CancelAreaSort(curareasortcolumn);
            UpdateAreaSort(0,AreaListView.Sorting);
            curareasortcolumn = 0;
            if (null != frm)
            {
                DrawIMG.DrawMainCenter(frm.MainCenter_G);
            }
            frm.MainCenter_Panel_Paint(null, null);

            //验证将集合中没有的图片名称全部删掉
            CommonBoxOperation.ClearIMGFile();
        
        }

        /// <summary>
        /// 将集合中的项刷新到人员列表中
        /// </summary>
        public void UpdatePersonListView()
        {
            PersonLv.Items.Clear();
            ListViewItem item = null;
            foreach (KeyValuePair<string,Person> prs in CommonCollection.Persons)
            {
                if (null == prs.Value)
                {
                    continue;
                }
               item = new ListViewItem(); 
               item.Text = prs.Key;
               item.Name = prs.Key;
               item.SubItems.Add(prs.Value.Name);
               item.SubItems.Add(prs.Value.Ps);
               if (prs.Value.PersonAccess == PersonAccess.SimplePerson)
               {
                   item.SubItems.Add(ConstInfor.StrSimplePerson);
               }
               else
               {
                   item.SubItems.Add(ConstInfor.StrAdminPerson);
               }
               PersonLv.Items.Add(item);
            }
        }

        /// <summary>
        /// 点击列表中项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AreaListView_Click(object sender, EventArgs e)
        {
            if (AreaListView.SelectedItems.Count <= 0) return;
            string StrAreaID = AreaListView.SelectedItems[0].Text;
            string StrAreaName = AreaListView.SelectedItems[0].SubItems[1].Text;
            string StrAreaType = AreaListView.SelectedItems[0].SubItems[2].Text;
            string StrAreaGroupIDName = AreaListView.SelectedItems[0].SubItems[3].Text;
            string StrAreaMapPath = AreaListView.SelectedItems[0].SubItems[4].Text;
            AreaIDTX.Text = StrAreaID; AreaIDTX.Enabled = false;
            AreaNameTX.Text = StrAreaName;
            //区域类型
            AreaGroupCB.SelectedItem = StrAreaGroupIDName;
            //清除掉当前面板中的内容
            AreaMapPanel.CreateGraphics().Clear(Color.White);
            if (ConstInfor.NoArea.Equals(StrAreaMapPath)) AreaMapPathTX.Text = "";
            else AreaMapPathTX.Text = StrAreaMapPath;
            AreaMapPanel_Paint(null, null);
        }

        /// <summary>
        /// 保存IP地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveIP_Btn_Click(object sender, EventArgs e)
        {
            string StrIp = IPComBox.Text;
            if ("".Equals(StrIp))
            {
                MessageBox.Show("IP地址不能為空!");
                return;
            }
            IPAddress Ip = null;
            if (!IPAddress.TryParse(StrIp, out Ip))
            {
                MessageBox.Show("IP地址格式有誤!");
                return;
            }
            if (null == Ip)
            {
                MessageBox.Show("Ip地址獲取失敗!");
                return;
            }
            NetParam.Ip = StrIp;
        }

        /// <summary>
        /// 保存端口信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SavePort_Btn_Click(object sender, EventArgs e)
        {
            string StrPort = PortTB.Text;
            if ("".Equals(StrPort))
            {
                MessageBox.Show("端口不能為空!");
                return;
            }
            int Port;
            try 
			{
                Port = Convert.ToInt32(StrPort);
            }catch(Exception)
            {
                MessageBox.Show("端口格式有誤!");
                return;
            }
            if (Port <= 1024 || Port > 65535)
            {
                MessageBox.Show("端口超出範圍，端口範圍在1024-65535之间!");
                return;
            }
            NetParam.Port = Port;
        }

        //int tick = 0;
        /// <summary>
        /// 设置窗口关闭时重绘表格面板当前的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParamSet_FormClosing(object sender, FormClosingEventArgs e)
        {
            //保存参数
            try
            {
                if (client != null) client.Close();
                SaveParam();
                DrawIMG.DrawMainCenter(frm.MainCenter_G);
                frm.MainCenter_Panel_Paint(null, null);
            }
            catch{
            }
            finally
            {
                //每次窗口关闭时，重新保存集合中的各项
                this.Cursor = Cursors.WaitCursor;
                SysParam.SaveAllInfor();
                this.Cursor = Cursors.Default;
                try
                {
                    DefinePlay.RemoveMediaPlayer(this);
                }
                catch (Exception)
                {
                }
                FileModel.fileInit().getCommCollTags();
                //SysParam.LoadPersonTagInfo(1);  
            }
        }

        private void SaveParam()
        {
          String StrScanTime = SysScanTime_TB.Text;
          String StrTagDisParam1 = TagDisTimeParam1TB.Text;
          String StrTagDisParam2 = TagDisTimeParam2TB.Text;
          String StrRouterDisParam1 = RouterDisTimeParam1TB.Text;
          String StrRouterDisParam2 = RouterDisTimeParam2TB.Text;
          if ("".Equals(StrScanTime))
          {
              SysParam.Measure_Interval = ConstInfor.DefaultSysScanTime;
          }
          try
		  {
              SysParam.Measure_Interval = Convert.ToInt32(StrScanTime);
          }catch(Exception)
          {
              SysParam.Measure_Interval = ConstInfor.DefaultSysScanTime;
          }
          if (SysParam.Measure_Interval <= 0)
          {
              SysParam.Measure_Interval = ConstInfor.DefaultSysScanTime;
          }
   
          if ("".Equals(StrTagDisParam1) || "".Equals(StrTagDisParam2))
          {
              SysParam.TagDisParam1 = ConstInfor.DefaultTagDisParam1Time;
              SysParam.TagDisParam2 = ConstInfor.DefaultTagDisParam2Time;
          }
          try 
          {
              SysParam.TagDisParam1 = Convert.ToInt32(StrTagDisParam1);
              SysParam.TagDisParam2 = Convert.ToInt32(StrTagDisParam2);
          }catch(Exception)
          {
              SysParam.TagDisParam1 = ConstInfor.DefaultTagDisParam1Time;
              SysParam.TagDisParam2 = ConstInfor.DefaultTagDisParam2Time;
          }
          if (SysParam.TagDisParam1 <= 0 || SysParam.TagDisParam2 <= 0)
          {
              SysParam.TagDisParam1 = ConstInfor.DefaultTagDisParam1Time;
              SysParam.TagDisParam2 = ConstInfor.DefaultTagDisParam2Time;
          }
          if ("".Equals(StrRouterDisParam1) || "".Equals(StrRouterDisParam2))
          {
              SysParam.RouterParam1 = ConstInfor.DefaultRouterParam1Time;
              SysParam.RouterParam2 = ConstInfor.DefaultRouterParam2Time;
          }
          try
          {
              SysParam.RouterParam1 = Convert.ToInt32(StrRouterDisParam1);
              SysParam.RouterParam2 = Convert.ToInt32(StrRouterDisParam2);
          }
          catch (Exception)
          {
              SysParam.RouterParam1 = ConstInfor.DefaultRouterParam1Time;
              SysParam.RouterParam2 = ConstInfor.DefaultRouterParam2Time;
          }
          if (SysParam.RouterParam1 <= 0 || SysParam.RouterParam2 <= 0)
          {
              SysParam.RouterParam1 = ConstInfor.DefaultRouterParam1Time;
              SysParam.RouterParam2 = ConstInfor.DefaultRouterParam2Time;
          }
          string StrSoundTime = SoundTimeTB.Text.Trim();
          if (SoundTimeTB.SelectedIndex == 1)
          {
              SysParam.SoundTime = int.MaxValue;
          }
          else
          {
              SysParam.SoundTime = 0;
          }      
          if (OptModelCB.SelectedIndex == 1)
          {//连续跳点次数
              SysParam.curOptimalMedol = OptimizationModel.HopTimes;
              try
              {
                  SysParam.PopTimes = Convert.ToInt32(PopTimesCb.Text);
              }catch(Exception)
              {
                  SysParam.PopTimes = 2;
              }
          } else
          { //信号强度差阀值
              SysParam.curOptimalMedol = OptimizationModel.SigStrengthValue;
              try
              {
                  SysParam.RssiThreshold = Convert.ToInt32(SigThreCB.Text);
              }catch(Exception)
              {
                  SysParam.RssiThreshold = 3;
              }
           }
       }

        /// <summary>
        /// 将参考点编辑栏中的内容更新到列表中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RouterUpdateBtn_Click(object sender, EventArgs e)
        {
             //判断参考点的ID是否正常
            string StrID = RouterIDTX.Text;
            string StrName = RouterNameTX.Text;
            //判断列表中是否已经添加了编辑项
            if (RouterListView.Items.ContainsKey(StrID))
            {
                ListViewItem[] items = RouterListView.Items.Find(StrID,false);
                if (null == items || items.Length <= 0)
                {
                    MessageBox.Show("對不起，節點不存在!");
                    return;
                }
                //判断该项是否被选中
                if (items[0].Selected)
                {
                    items[0].SubItems[1].Text = StrName;
                    if (RouterVisibleCb.Checked)
                    {
                        items[0].SubItems[3].Text = "Yes";
                    }
                    else
                    {
                        items[0].SubItems[3].Text = "No";
                    }
                    if (NodeTypeCB.SelectedIndex == 0)
                    {
                        SysParam.UpdateAreaRouterName(StrID, StrName, RouterVisibleCb.Checked);
                    }
                    else
                    {
                        SysParam.UpdateAreaDataNodeName(StrID, StrName, RouterVisibleCb.Checked);
                    }
                }
                else
                {
                    MessageBox.Show("請在列裱中選擇要更改的項!");
                }
                return;
            }
        }

        /// <summary>
        /// 点击了参考点列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RouterListView_Click(object sender, EventArgs e)
        {
            if (RouterListView.SelectedItems.Count <= 0)
            {
                return;
            }

            RouterIDTX.Text = RouterListView.SelectedItems[0].Text;
            RouterNameTX.Text = RouterListView.SelectedItems[0].SubItems[1].Text;
            if (RouterListView.SelectedItems[0].SubItems[2].Text.Trim().Equals("參考點"))NodeTypeCB.SelectedIndex = 0;
            else NodeTypeCB.SelectedIndex = 1;
            string StrVisible = RouterListView.SelectedItems[0].SubItems[3].Text;
            if ("Yes".Equals(StrVisible))
            {
                RouterVisibleCb.Checked = true;
            }
            else
            {
                RouterVisibleCb.Checked = false;
            }
            RouterGroupTX.Text = RouterListView.SelectedItems[0].SubItems[4].Text;
            //选择当前的区域
            foreach (string str in RouterAreaCB.Items)
            {
                if (str.Equals(RouterListView.SelectedItems[0].SubItems[5].Text))
                {
                    RouterAreaCB.SelectedItem = str;
                    return;
                }
            }
            //说明没有选择的区域
            RouterAreaCB.SelectedItem = ConstInfor.NoArea;
        }

        /// <summary>
        /// 向列表中添加Tag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTagBtn_Click(object sender, EventArgs e)
        {
            TagIDTB.Enabled = true;
            TagIDTB.Text = "0000";
        }

        /// <summary>
        /// 删除Tag列表中的项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleTagBtn_Click(object sender, EventArgs e)
        {
            string StrTagID = TagIDTB.Text;
            lock (CommonCollection.Tags_Lock)
            {
                if (!CommonCollection.Tags.ContainsKey(StrTagID))
                {
                    MessageBox.Show("對不起,需要刪除的Tag不存在!");
                    return;
                }
                else
                    CommonCollection.Tags.Remove(StrTagID);
            }
            UpdateTagListView();
        }

        /// <summary>
        /// 更新Tag编辑框中的项到列表中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTagBtn_Click(object sender, EventArgs e)
        {
            string strstoptime = string.Empty;
            int stoptime = int.MinValue;
            string StrTagID = TagIDTB.Text;
            if ("".Equals(StrTagID))
            {
                MessageBox.Show("對不起,Tag的ID不能為空!");
                return;
            }
            if (StrTagID.Length != 4)
            {
                MessageBox.Show("對不起,Tag的ID長度為4，例如：ID=0001");
                return;
            }
            if("0000".Equals(StrTagID))
            {
                MessageBox.Show("對不起,Tag的ID不能為0000");
                return;
            }
            StrTagID = StrTagID.ToUpper();
            TagIDTB.Text = StrTagID;
            byte[] TagID = new byte[2];
            try 
            {
                TagID[0] = Convert.ToByte(StrTagID.Substring(0,2),16);
                TagID[1] = Convert.ToByte(StrTagID.Substring(2,2),16);
            }catch(Exception)
            {
                MessageBox.Show("對不起,Tag的ID格式有誤,ID范围为0000-FFFF!");
                return;
            }
          
            Tag tag = new Tag();
           
            //卡片ID
            tag.ID[0] = TagID[0];
            tag.ID[1] = TagID[1];
            //卡片名称
            tag.Name = TagNameTB.Text;

            byte[] wTagID = DrawIMG.getworkIDFormTextBox(textBox1.Text);

            List<Tag> mTags = CommonCollection.Tags.Values.ToList();
            for (int i = 0; i < mTags.Count; i++)
            {
                Tag wTag = mTags[i];
                if (DrawIMG.isBettonBt(wTagID, wTag.workID))
                {
                    if (DrawIMG.isBettonBt(tag.ID, wTag.ID)) continue;
                    MessageBox.Show("對不起,該打卡ID和卡片" + wTag.Name + "綁定完成，請勿重複綁定");
                    return;
                }
            }
            tag.workID = wTagID;
            if (isStopAlarmCB.Checked)
            {
                tag.IsStopAlarm = true;
                strstoptime = tagstoptimetb.Text;
                if ("".Equals(strstoptime))
                {
                    MessageBox.Show(StrTagID+"卡片的未移動時間不能為空!");
                    return;
                }
                try 
                {
                    stoptime = Convert.ToInt32(strstoptime);
                }catch(Exception)
                {
                    MessageBox.Show(StrTagID+"卡片的未移動時間格式有誤!");
                    return;
                }
                if (stoptime < 30 || stoptime > 100)
                {
                    MessageBox.Show(StrTagID+"卡片的未移動時間不能小於30且不能大於100!");
                    return;
                }
                tag.StopTime = stoptime;
            }
            else
            {
                tag.IsStopAlarm = false;            
                tag.StopTime = 0;
            }

            /*switch (WorkTimeTypeCB.SelectedIndex)
            {
                case 0:tag.CurTagWorkTime = WorkTime.NoWork;break;
                case 1:tag.CurTagWorkTime = WorkTime.LimitTime;
                    //开始时间
                    tag.StartWorkDT = startTP.Value;
                    tag.EndWorkDT = EndTP.Value;
                    break;
                case 2:
                    tag.CurTagWorkTime = WorkTime.AlwaysWork;
                    break;
            }*/
            switch (GSensorCB.SelectedIndex)
            {
                case 0:
                    tag.CurGSWorkTime = WorkTime.NoWork;break;
                case 1:
                    tag.CurGSWorkTime = WorkTime.LimitTime;
                    tag.StartGSDT = gsstartdtp.Value;
                    tag.EndGSDT = gsenddtp.Value;
                    break;
                case 2:tag.CurGSWorkTime = WorkTime.AlwaysWork;break;
            }
            //可进的区域
            foreach (TreeNode node in ReferTreeView.Nodes)
            {  // 遍历所有的区域
                foreach(TreeNode childnode in node.Nodes)
                { // 遍历所有区域中的参考点
                    if (childnode.Checked)
                    {
                        tag.TagReliableList.Add(childnode.Name);
                    }
                }
            }

            tag.CurTagWorkTime = WorkTime.LimitTime;
            tag.StartWorkDT = startTP.Value;
            tag.EndWorkDT = EndTP.Value;

            if (CommonCollection.Tags.ContainsKey(StrTagID))
            {
                if (!TagIDTB.Enabled)
                {
                    tag.StartWorkDT = CommonCollection.Tags[StrTagID].StartWorkDT;
                    tag.EndWorkDT = CommonCollection.Tags[StrTagID].EndWorkDT;
                    CommonCollection.Tags.Remove(StrTagID);
                    CommonCollection.Tags.Add(StrTagID, tag);
                    UpdateTagListView();
                }
                else
                {
                    MessageBox.Show("對不起,列裱中已經存在ID为" + StrTagID + "的项!");
                }
                return;
            }
            CommonCollection.Tags.Add(StrTagID, tag);
            TagIDTB.Enabled = false;
            UpdateTagListView();
        }

        public void UpdateTagListView()
        {
            String StrAreaMsg = "",StrReferName = "",StrMsg = "",workTime;
            TagListView.Items.Clear();
            ListViewItem item = null;
            foreach(KeyValuePair<string,Tag> tag in CommonCollection.Tags)
            {
               if(null == tag.Value)
                   continue;
               item = new ListViewItem();
               item.Text = tag.Key;
               item.Name = tag.Key;
               item.SubItems.Add(tag.Value.Name);

               if (tag.Value.IsStopAlarm)
               {
                   item.SubItems.Add("Yes" + "(" + tag.Value.StopTime + ")");
               }
               else
               {
                   item.SubItems.Add("No");
               }

               /*switch (tag.Value.CurGSWorkTime)
               {
                   case WorkTime.AlwaysWork:
                       item.SubItems.Add("一直工作"); break;
                   case WorkTime.LimitTime:
                       item.SubItems.Add(tag.Value.StartGSDT.ToShortTimeString() + "--" + tag.Value.EndGSDT.ToShortTimeString()); break;
                   case WorkTime.NoWork:
                       item.SubItems.Add("不工作"); break;
                   default:
                       item.SubItems.Add("未知");
                       break;
               }*/

               switch (tag.Value.CurTagWorkTime)
               {
                   case WorkTime.AlwaysWork:
                       workTime = "一直工作";
                       //item.SubItems.Add("一直工作");
                       break;
                   case WorkTime.LimitTime:
                       workTime = tag.Value.StartWorkDT.ToShortTimeString() + "--" + getEndWorkStr(tag.Value.EndWorkDT);
                       //item.SubItems.Add(tag.Value.StartWorkDT.ToShortTimeString() + "--" + getEndWorkStr(tag.Value.EndWorkDT));
                       break;
                   case WorkTime.NoWork:
                       workTime = "不工作";
                       //item.SubItems.Add("不工作");
                       break;
                   default:
                       workTime = "未知";
                       //item.SubItems.Add("未知");
                       break;
               }

               item.SubItems.Add(workTime);
               item.SubItems.Add(workTime);

               StrAreaMsg = string.Empty;
               foreach(string strid in tag.Value.TagReliableList)
               {
                   StrMsg = "";
                   StrReferName = CommonBoxOperation.GetRouterName(strid);
                   if (null == StrReferName || "".Equals(StrReferName))StrMsg = strid;
                   else StrMsg = StrReferName + "(" + strid + ")";
                   StrAreaMsg += StrMsg + "、";
               }
               if (StrAreaMsg.EndsWith("、"))
               {
                   StrAreaMsg = StrAreaMsg.Substring(0, StrAreaMsg.Length - 1);
               }
               item.SubItems.Add(StrAreaMsg);
               String workIDs = "";
               for (int i = 0; i < 16; i++)
               {
                   workIDs += tag.Value.workID[i].ToString("X2");
               }
               item.SubItems.Add(workIDs);
               TagListView.Items.Add(item);
            }
        }

        private String getEndWorkStr(DateTime dt) 
        {
            if (dt.CompareTo(DrawIMG.maxDt) == 0) return "";
            return dt.ToShortTimeString();
        }

        //往一个容器中添加勾选框  
        public void AddCheckControl(TreeView refertree)
        {
            TreeNode node = null,chilenode = null;
            //每次添加前清除掉节点，防止重复添加
            refertree.Nodes.Clear();
            //以区域为父节点向节点上添加参考点
            foreach(KeyValuePair<string,Area> cuar in CommonCollection.Areas)
            {
                if (null == cuar.Value) continue;
                node = new TreeNode();
                node.Name = cuar.Key;
                node.Text = (null == cuar.Value.Name || "".Equals(cuar.Value.Name)) ? cuar.Key : (cuar.Value.Name + "(" + cuar.Key + ")");
                foreach(KeyValuePair<string,BasicRouter> br in cuar.Value.AreaRouter)
                {
                    if (null == br.Value) 
                        continue;
                    chilenode = new TreeNode();
                    chilenode.Name = br.Key;
                    chilenode.Text = (null == br.Value.Name || "".Equals(br.Value.Name)) ? br.Key : (br.Value.Name + "(" + br.Key + ")");
                    node.Nodes.Add(chilenode);
                }
                refertree.Nodes.Add(node);
            }
            refertree.CollapseAll();
            refertree.AfterCheck += refertree_AfterCheck;
        }
        private void refertree_AfterCheck(Object obj, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Nodes.Count > 0)
                {
                    bool NoFalse = true;
                    foreach (TreeNode tn in e.Node.Nodes)
                    {
                        if (tn.Checked == false)
                        {
                            NoFalse = false;
                        }
                    }
                    if (e.Node.Checked == true || NoFalse)
                    {
                        foreach (TreeNode tn in e.Node.Nodes)
                        {
                            if (tn.Checked != e.Node.Checked)
                            {
                                tn.Checked = e.Node.Checked;
                            }
                        }
                    }
                }
                if (e.Node.Parent != null && e.Node.Parent is TreeNode)
                {
                    bool ParentNode = true;
                    foreach (TreeNode tn in e.Node.Parent.Nodes)
                    {
                        if (tn.Checked == false)
                        {
                            ParentNode = false;
                        }
                    }
                    if (e.Node.Parent.Checked != ParentNode && (e.Node.Checked == false || e.Node.Checked == true && e.Node.Parent.Checked == false))
                    {
                        e.Node.Parent.Checked = ParentNode;
                    }
                }
            }
            catch (Exception ex)
            {
                FileOperation.WriteLog("勾选参考点是否可进入时出现异常,异常原因:"+ex.ToString());
            }
        }

        /// <summary>
        /// 点击列表Tag列表中的项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TagListView_Click(object sender, EventArgs e)
        {
            Tag tag = null;
            if (TagListView.SelectedItems.Count <= 0)
                return;
            AllAreaCB.Checked = false;
            AllAreaCB_CheckedChanged(null,null);
            TagIDTB.Enabled = false;
            TagIDTB.Text   = TagListView.SelectedItems[0].Name;
            TagNameTB.Text = TagListView.SelectedItems[0].SubItems[1].Text;
            CommonCollection.Tags.TryGetValue(TagIDTB.Text,out tag);
            if (null == tag) return;

            WorkTimeTypeCB.SelectedIndex = 1;
            startTP.Value = tag.StartWorkDT;
            if (tag.EndWorkDT.Year != 1) 
                EndTP.Value = tag.EndWorkDT;

            /*switch (tag.CurTagWorkTime)
            {
                case WorkTime.AlwaysWork:
                    WorkTimeTypeCB.SelectedIndex = 2;
                    break;
                case WorkTime.LimitTime:
                    WorkTimeTypeCB.SelectedIndex = 1;
                    startTP.Value = tag.StartWorkDT;
                    EndTP.Value = tag.EndWorkDT;
                    break;
                case WorkTime.NoWork:
                    WorkTimeTypeCB.SelectedIndex = 0;
                    break;
                default:
                    //默认显示一直工作，这样就可以与接收数据包相对应
                    WorkTimeTypeCB.SelectedIndex = 2;
                    break;
            }*/
            switch (tag.CurGSWorkTime)
            {
                case WorkTime.AlwaysWork:
                    GSensorCB.SelectedIndex = 2; break;
                case WorkTime.LimitTime:
                    GSensorCB.SelectedIndex = 1;
                    gsstartdtp.Value = tag.StartGSDT;
                    gsenddtp.Value = tag.EndGSDT;
                    break;
                case WorkTime.NoWork:
                    GSensorCB.SelectedIndex = 0;
                    break;
                default:
                    GSensorCB.SelectedIndex = 2;
                    break;
            }
            if (tag.IsStopAlarm)
            {
                tagstoptimetb.Text = tag.StopTime + "";
                isStopAlarmCB.Checked = true;
            }else
            {
                tagstoptimetb.Text = "";
                isStopAlarmCB.Checked = false;
            }
            foreach(string str in tag.TagReliableList)
            {
                TreeNode[] treenodes = ReferTreeView.Nodes.Find(str,true);
                if(treenodes.Length > 0)
                {
                   foreach(TreeNode node in treenodes)
                   {
                      if(null != node.Parent)
                         node.Checked = true;
                    }
                 }
            }

            textBox1.Text = DrawIMG.getBUffIdStr(tag.workID);
        }
        /// <summary>
        /// 编辑地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditAreaMapBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog MyFileDialog = new OpenFileDialog();
            MyFileDialog.Filter = "所有圖片文件|*.bmp;*.jpg;*.png";
            if (MyFileDialog.ShowDialog() == DialogResult.OK)
            {
                string StrAbs = MyFileDialog.FileName;
                int start = StrAbs.LastIndexOf("\\");
                string StrRelate = StrAbs.Substring(start + 1, StrAbs.Length - start - 1);
                if (FileOperation.isFileExist(StrRelate, 0))
                {
                    int end = StrRelate.LastIndexOf(".");
                    string StrCur = StrRelate.Substring(0, end);
                    StrCur += (new Random()).Next(1000).ToString();
                    StrRelate = StrCur + StrRelate.Substring(end, StrRelate.Length - end);
                }
                if (FileOperation.SetMap(MyFileDialog.FileName, StrRelate))
                {
                    if (null == StrRelate || "".Equals(StrRelate)) return;
                    AreaMapPathTX.Text = StrRelate;
                    AreaMapPanel.CreateGraphics().Clear(Color.White);
                    DrawAreaMap.DrawMap(AreaMapPanel,AreaMapPathTX.Text);
                }
            }
        }
        /// <summary>
        /// 画出区域的小地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AreaMapPanel_Paint(object sender, PaintEventArgs e)
        {
            if (!"".Equals(AreaMapPathTX.Text))
                DrawAreaMap.DrawMap(AreaMapPanel,AreaMapPathTX.Text);
            else
                DrawAreaMap.DrawNoMap(AreaMapPanel);
        }
        /// <summary>
        /// 显示区域的选项框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAreaCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            Area ar = SysParam.GetArea_IDName(ShowAreaCB.Text);
            if (ar != null && null != ar.AreaBitMap)
            {
                MapPathTB.Text = ar.AreaBitMap.MapPath;
                DrawAreaMap.DrawMap(ShowAreaMapPanel, MapPathTB.Text);
            }
            else
            {
                MapPathTB.Text = "";
                DrawAreaMap.DrawMap(ShowAreaMapPanel, null);
            }
            SysParam.RestoreShow();
        }
        /// <summary>
        /// 双击图片，弹出参考点设置窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAreaMapPanel_DoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs ex = (MouseEventArgs)e;
            string StrAreaIDName = ShowAreaCB.Text;
            Area area = SysParam.GetArea_IDName(StrAreaIDName);
            //获取到区域信息
            MyEditRouter = new EditRouter(this, ex, area);
            MyEditRouter.ShowDialog();
        }
        /// <summary>
        /// 单击已有的参考点，弹出编辑框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAreaMapPanel_Click(object sender, EventArgs e)
        {
            MouseEventArgs ex = (MouseEventArgs)e;
            //获取区域信息
            String StrID_Name =  ShowAreaCB.Text;
            Area ar = SysParam.GetArea_IDName(StrID_Name);
            if (null == ar)return;
            foreach(KeyValuePair<string,BasicRouter> Br in ar.AreaRouter)
            {
                if (ex.X > Br.Value.x - ConstInfor.RouterWidth / 2 && ex.X < Br.Value.x + ConstInfor.RouterWidth / 2 && ex.Y > Br.Value.y - ConstInfor.RouterHeight / 2 && ex.Y < Br.Value.y + ConstInfor.RouterHeight / 2)
                {
                    if (!SysParam.isMove)
                    {
                        MyEditRouter = new EditRouter(this, ex, ar, Br.Key,0);
                        MyEditRouter.ShowDialog();
                    }
                    return;
                }
            }
            foreach (KeyValuePair<string,BasicNode> Bn in ar.AreaNode)
            {
                if (ex.X > Bn.Value.x - ConstInfor.DataNodeWidth / 2 && ex.X < Bn.Value.x + ConstInfor.DataNodeWidth / 2 && ex.Y > Bn.Value.y - ConstInfor.DataNodeHeight / 2 && ex.Y < Bn.Value.y + ConstInfor.DataNodeHeight / 2)
                {
                    if (!SysParam.isMove)
                    {
                        MyEditRouter = new EditRouter(this, ex, ar, Bn.Key,1);
                        MyEditRouter.ShowDialog();
                    }
                    return;
                }
            }

        }
        /// <summary>
        /// 重绘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParamSet_Paint(object sender, PaintEventArgs e)
        {
            //列表栏中选择第二行
            if (Param_ListBox.SelectedIndex == 2)
            {
                SysParam.RestoreShow();
            }
        }
        /// <summary>
        /// 鼠标在显示面板上按下后，表示可以移动参考点了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAreaMapPanel_MouseDown(object sender, MouseEventArgs e)
        {
            SysParam.isMove = false;
            //获取区域信息
            String StrID_Name = ShowAreaCB.Text;
            Area ar = SysParam.GetArea_IDName(StrID_Name);
            if (null==ar)return;


            foreach (KeyValuePair<string, BasicRouter> Br in ar.AreaRouter)
            {
                 if(e.X > Br.Value.x - ConstInfor.RouterWidth / 2 && e.X < Br.Value.x + ConstInfor.RouterWidth / 2 && e.Y > Br.Value.y - ConstInfor.RouterHeight / 2 && e.Y < Br.Value.y + ConstInfor.RouterHeight/2)
                 { SysParam.isDrag = true; SysParam.DragBasicRouter = Br.Value; SysParam.isNode = false; return; }
            }

            foreach (KeyValuePair<string, BasicNode> Bn in ar.AreaNode)
            {
                if (e.X > Bn.Value.x - ConstInfor.DataNodeWidth / 2 && e.X < Bn.Value.x + ConstInfor.DataNodeWidth / 2 && e.Y > Bn.Value.y - ConstInfor.DataNodeHeight / 2 && e.Y < Bn.Value.y + ConstInfor.DataNodeHeight / 2)
                  { SysParam.isDrag = true; SysParam.DragBasicNode = Bn.Value; SysParam.isNode = true;return; }
            }
        } 
        /// <summary>
        /// 鼠标移动过程中拖动参考点的位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAreaMapPanel_MouseMove(object sender, MouseEventArgs e)
        {
            SysParam.isMove = true;
            if (SysParam.isDrag)
            {//可以拖动参考点
                if (e.X > ShowAreaMapPanel.Width || e.X < 0 || e.Y > ShowAreaMapPanel.Height || e.Y < 0)
                {
                  SysParam.isDrag = false;return;
                }
                if (!SysParam.isNode)
                {
                    SysParam.DragBasicRouter.x = e.X;
                    SysParam.DragBasicRouter.y = e.Y;
                }
                else
                {
                    SysParam.DragBasicNode.x = e.X;
                    SysParam.DragBasicNode.y = e.Y;
                }
                SysParam.RestoreShow();
            }
        }
        /// <summary>
        /// 鼠标释放后，表示拖动完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowAreaMapPanel_MouseUp(object sender, MouseEventArgs e)
        {
            SysParam.isMove = false;
            if (SysParam.isDrag)
            {
                SysParam.isDrag = false;SysParam.DragBasicRouter = null;
                SysParam.isNode = false; SysParam.DragBasicNode = null;
            }
        }

        /// <summary>
        /// 在列表中搜索人员卡片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBtn_Click(object sender, EventArgs e)
        {
            string StrSearchTB = PsTagSearchTB.Text;
            if ("".Equals(StrSearchTB) || null == StrSearchTB)
            {MessageBox.Show("搜索时搜索框中的内容不能为空!");return;
            }
            SysParam.SearchListViewStr(TagListView, StrSearchTB);
        }

        /// <summary>
        /// 搜索文本框中的内容改变时，重新设置搜索内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PsTagSearchTB_TextChanged(object sender, EventArgs e)
        {
            SysParam.ClearListViewWhiteItem(TagListView);
        }
        /// <summary>
        /// 在列表中搜索参考点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RouterSearchBtn_Click(object sender, EventArgs e)
        {
            string StrSearchTB = RouterSearchTB.Text;
            if ("".Equals(StrSearchTB) || null == StrSearchTB)
            {
                MessageBox.Show("搜索时搜索框中的内容不能为空!");
                return;
            }
            SysParam.SearchListViewStr(RouterListView, StrSearchTB);
        }
        /// <summary>
        /// 搜索文本框中的内容改变时，重新设置搜索内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RouterSearchTB_TextChanged(object sender, EventArgs e)
        {
            SysParam.ClearListViewWhiteItem(RouterListView);
        }
        /// <summary>
        /// 添加组项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddGroupBtn_Click(object sender, EventArgs e)
        {
            GroupID_TX.Enabled = true;
            GroupID_TX.Text = "0000";
        }
        /// <summary>
        /// 删除组项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleGroupBtn_Click(object sender, EventArgs e)
        {
            string StrGroupID = GroupID_TX.Text;
            lock(CommonCollection.Groups_Lock)
            {
                if (!CommonCollection.Groups.ContainsKey(StrGroupID))
                {
                    MessageBox.Show("對不起，需要刪除的項不存在!");
                }
                else
                {
                    CommonCollection.Groups.Remove(StrGroupID);
                }
            }
            UpdateGroupListView();

        }
        /// <summary>
        /// 将右侧编辑栏中的项添加到列表中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateGroupBtn_Click(object sender, EventArgs e)
        {
            string StrGroupID = GroupID_TX.Text;
            string StrGroupName = GroupName_TX.Text;
            if ("".Equals(StrGroupID))
            {MessageBox.Show("對不起,組別的ID不能為空!");return;
            }
            if (StrGroupID.Length != 4)
            {MessageBox.Show("對不起,組別的ID长度为4，例如：ID为0001");return;
            }
            if ("0000".Equals(StrGroupID))
            {MessageBox.Show("對不起,組別的ID不能為0000");return;
            }
            byte[] GroupID = new byte[2];
            try {
                GroupID[0] = Convert.ToByte(StrGroupID.Substring(0,2),16);GroupID[1] = Convert.ToByte(StrGroupID.Substring(2,2),16);
            }catch(Exception)
            {MessageBox.Show("對不起,組別的ID格式有误!");return;
            }
            if (DrawIMG.GetLength(StrGroupName) > 14)
            {   MessageBox.Show("對不起,組別名稱絕對長度不能大于14，其中一個漢字長度為2，默認取前14个字符，若第14个为汉字则取前13个字符!");
                StrGroupName = DrawIMG.Get14Char(StrGroupName);
            }
            Group grp = new Group();
            grp.ID[0] = GroupID[0];grp.ID[1] = GroupID[1];
            grp.Name = StrGroupName;
            //查看集合中是否存在该项
            lock (CommonCollection.Groups_Lock)
            {
                if (CommonCollection.Groups.ContainsKey(StrGroupID))
                {
                    if (!GroupID_TX.Enabled)
                    {
                        CommonCollection.Groups.Remove(StrGroupID);
                        CommonCollection.Groups.Add(StrGroupID, grp);
                        UpdateGroupListView();
                    }
                    else MessageBox.Show("對不起,該組別已經存在!");
                    return;
                }
                CommonCollection.Groups.Add(StrGroupID, grp);
            }
            GroupID_TX.Enabled = false;
            //重新刷新列表
            UpdateGroupListView();
        }
        /// <summary>
        /// 将集合中的项刷新到列表中
        /// </summary>
        public void UpdateGroupListView()
        {
            GroupListView.Items.Clear();
            ListViewItem item = null;
            //每次更新到列表中时，重新对它进行排序
            List<KeyValuePair<string, Group>> gps = CommonCollection.Groups.OrderBy(c => c.Key).ToList();
            CommonCollection.Groups.Clear();
            foreach (KeyValuePair<string, Group> grp in gps)
            {
                CommonCollection.Groups.Add(grp.Key, grp.Value);
            }
            foreach(KeyValuePair<string,Group> grp in CommonCollection.Groups)
            {
                if(null == grp.Value)
                   continue;
                item = new ListViewItem();
                item.Text = grp.Key;
                item.SubItems.Add(grp.Value.Name);
                GroupListView.Items.Add(item);
            }
            //将组别中的显示项进行更新
            SysParam.GroupIndex = 0;
            CommonBoxOperation.UpdateShowGroups();
            SysParam.isLeftArrowDown = false;
            SysParam.isRightArrowDown = false;
            if (null != frm)
            {
                DrawIMG.DrawMainCenter(frm.MainCenter_G);
            }
            frm.MainCenter_Panel_Paint(null,null);
        }
        /// <summary>
        /// 组别发生变化后，对应的区域选择框中的区域也将发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowGroupCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowAreaCB.Items.Clear();
            if (ShowGroupCB.Items.Count <= 0)
            {
                ShowAreaCB.Items.Add(ConstInfor.NoArea);
                ShowAreaCB.SelectedIndex = 0;
                return;
            }
            Group MyGroup = SysParam.GetGroup_IDName(ShowGroupCB.Text);
            List<KeyValuePair<string, Area>> areas = CommonCollection.Areas.OrderBy(k => k.Key).ToList();
            if (null != MyGroup)
            {
                foreach (KeyValuePair<string, Area> area in areas)
                {
                    if (null != area.Value.GroupID)
                    {
                        if (area.Value.GroupID[0] == MyGroup.ID[0] && area.Value.GroupID[1] == MyGroup.ID[1])
                        {
                            SysParam.AddNameID(area.Key, area.Value.Name, ShowAreaCB);
                        }
                    }
                }
            }
            ShowAreaCB.Items.Add(ConstInfor.NoArea);
            ShowAreaCB.SelectedIndex = 0;
        }

        private void GroupListView_Click(object sender, EventArgs e)
        {
            if (GroupListView.SelectedItems.Count <= 0)return;
            GroupID_TX.Text = GroupListView.SelectedItems[0].Text;
            GroupName_TX.Text = GroupListView.SelectedItems[0].SubItems[1].Text;
            GroupID_TX.Enabled = false;
        }
        /// <summary>
        /// 选择低电量监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LowBatteryCB_CheckedChanged(object sender, EventArgs e)
        {
            if (LowBatteryCB.Checked)
            {
                String StrLowBattery = LowBattryTX.Text;
                if ("".Equals(StrLowBattery))
                {
                    MessageBox.Show("選擇低電量監控時，設置的最低電量不能為空!");
                    LowBatteryCB.Checked = false;
                    LowBattryTX.Enabled = true;
                    return;
                }
                byte LowBattery = 0;
                try
                {LowBattery = Convert.ToByte(StrLowBattery);
                }
                catch (Exception)
                {
                    MessageBox.Show("最低電量的格式有誤!");
                    LowBatteryCB.Checked = false;LowBattryTX.Enabled = true;return;
                }

                if (LowBattery <= 0 || LowBattery > 20)
                {
                    MessageBox.Show("最低電量不能大於20，也不能小於等於0!");
                    LowBatteryCB.Checked = false;LowBattryTX.Enabled = true;
                    return;
                }
                SysParam.isLowBattery = true; SysParam.LowBattery = LowBattery;
                LowBattryTX.Enabled = false;
            }
            else
            {
                SysParam.isLowBattery = false;SysParam.LowBattery = 0;
                LowBattryTX.Enabled = true;
                LowBattryTX.Text = "";
            }
        }
        /// <summary>
        /// 选择声音的类型:自定义或默认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SoundSelectedCbB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsSoundAlarmCB.Checked)
                return;
            
            if (SoundSelectedCbB.SelectedIndex == 0)
            {
                LoadSoungBtn.Visible = false;
                label36.Visible = false;

                if (isplaymedia)
                {
                    try
                    {
                        DefinePlay.RemoveMediaPlayer(this);
                        DefinePlay.Close();
                    }catch(Exception e1)
                    {
                        FileOperation.WriteLog("关闭声音警报时出现异常!异常原因:"+e1.ToString());
                    }
                    SysParam.SoundName = ConstInfor.DefaultSoundAlarm;
                    SoundOperation.DefaultSoundPlay(0);
                    SoundOperation.PlayCount = 1;
                }
                SoundTimeTB.Enabled = true;
                LoadSoungBtn.Visible = false;
                label36.Visible = false;

            }
            else if (SoundSelectedCbB.SelectedIndex == 1)
            {
                LoadSoungBtn.Visible = true;
                LoadSoungBtn.Visible = true;
                label36.Visible = true;
                //获取Sound文件夹中的音频文件
                SysParam.SoundName = FileOperation.GetSound();
                if (!ConstInfor.DefaultSoundAlarm.Equals(SysParam.SoundName))
                {
                    label36.Text = "导入的音频为：" + SysParam.SoundName;

                    try
                    {
                        if (isplaymedia)
                        {
                            AxWMPLib.AxWindowsMediaPlayer axplayer = new AxWMPLib.AxWindowsMediaPlayer();
                            axplayer.Visible = false;
                            axplayer.BeginInit();
                            this.Controls.Add(axplayer);
                            axplayer.EndInit();
                            DefinePlay.SinglePlay(axplayer, FileOperation.SoundPath + "\\" + SysParam.SoundName);
                        }
                    }
                    catch (Exception ex)
                    { 
                        FileOperation.WriteLog(DateTime.Now + " " + ex.ToString());
                    }
                }
                else
                {
                    label36.Text = "";
                }
                label36.Visible = true;
                
            }
            isplaymedia = true;
        }
        /// <summary>
        /// 导入自定义的声音
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadSoungBtn_Click(object sender, EventArgs e)
        {
            //如wma，mmf，mp3，mid等
            OpenFileDialog MyFileDialog = new OpenFileDialog();
            MyFileDialog.Filter = "所有音频文件|*.mp3;*.wav" ;
            if (MyFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (FileOperation.SetSound(MyFileDialog.FileName))
                {
                    String StrImageName = FileOperation.GetFileName(MyFileDialog.FileName, 1);
                    if (null == StrImageName || "".Equals(StrImageName))
                        return;
                    label36.Text = "导入的音频为：" + StrImageName;
                    SysParam.SoundName = StrImageName;
                    //创建一个MediaPlay
                    try
                    {
                        AxWMPLib.AxWindowsMediaPlayer axplayer = new AxWMPLib.AxWindowsMediaPlayer();
                        axplayer.Visible = false;
                        axplayer.BeginInit();
                        this.Controls.Add(axplayer);
                        axplayer.EndInit();
                        DefinePlay.SinglePlay(axplayer, FileOperation.SoundPath + "\\" + StrImageName);
                    }catch(Exception ex)
                    {FileOperation.WriteLog(DateTime.Now+" "+ex.ToString());}
                }
            }

        }
        /// <summary>
        /// 切换是否提示音
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsSoundAlarmCB_CheckedChanged(object sender, EventArgs e)
        {
            if (IsSoundAlarmCB.Checked)
            {
                SysParam.IsSoundAlarm = true;
                SoundSelectedCbB.Visible = true;

                soundPersonHelpCB.Visible = true;
                SoundAreaControlCB.Visible = true;
                SoundPersonResCB.Visible = true;
                SoundBatteryLowCB.Visible = true;
                SoundDeviceThroubleCB.Visible = true;

                if (ConstInfor.DefaultSoundAlarm.Equals(SysParam.SoundName))
                {
                    SoundSelectedCbB.SelectedIndex = 0;
                    LoadSoungBtn.Visible = false;
                    label36.Visible = false;
                }
                else
                {
                    SoundSelectedCbB.SelectedIndex = 1;
                    LoadSoungBtn.Visible = true;
                    label36.Text = "导入的音频为：" + SysParam.SoundName;
                    label36.Visible = true;
                }

                IsSoundAlarmCB.Checked = true;
                SoundSelectedCbB.Visible = true;
                LoadSoungBtn.Visible = true;
                label36.Visible = true;
                label45.Visible = true;
                SoundTimeTB.Visible = true;
          
                label46.Visible = true;
                SoundSelectedCbB_SelectedIndexChanged(null,null);
            }
            else
            {
                try
                {
                    DefinePlay.RemoveMediaPlayer(this);
                    DefinePlay.Close();
                }
                catch (Exception e1)
                {
                    FileOperation.WriteLog("关闭声音警报时出现异常!异常原因:"+e1.ToString());
                }
                SysParam.IsSoundAlarm = false;
                IsSoundAlarmCB.Checked = false;
                SoundSelectedCbB.Visible = false;
                LoadSoungBtn.Visible = false;
                label36.Visible = false;
                IsSoundAlarmCB.Checked = false;
                SoundSelectedCbB.Visible = false;
                LoadSoungBtn.Visible = false;
                label36.Visible = false;
                label45.Visible = false;
                SoundTimeTB.Visible = false;
               
                label46.Visible = false;

                soundPersonHelpCB.Visible = false;
                SoundAreaControlCB.Visible = false;
                SoundPersonResCB.Visible = false;
                SoundBatteryLowCB.Visible = false;
                SoundDeviceThroubleCB.Visible = false;
            }
        }

        public void ShowAreaMapPanel_Paint(object sender, PaintEventArgs e)
        {
            if (null != AreaBitmap)
            {
                ShowAreaMapPanel.CreateGraphics().DrawImage(AreaBitmap, 0, 0);
            }
            else
            {
                ShowAreaMapPanel.CreateGraphics().Clear(Color.White);
            }
        }

        private void IsClearTimerTabCb_CheckedChanged(object sender, EventArgs e)
        {
            //清理数据资料
            if (IsClearTimerTabCb.Checked)
            {
                string StrClearTime = ClearTimeTB.Text;
                if ("".Equals(StrClearTime))
                { 
                    MessageBox.Show("清理資料的時間不能為空!"); 
                    IsClearTimerTabCb.Checked = false; return;
                }
                int ClearTime = 0;
                try
                {
                    ClearTime = Convert.ToInt32(StrClearTime);
                }
                catch (Exception)
                { 
                    MessageBox.Show("清理資料的時間格式有誤!"); 
                    IsClearTimerTabCb.Checked = false;
                    return;
                }
                if (ClearTime <= 0)
                { 
                    MessageBox.Show("清理資料的時間不能小於0");
                    IsClearTimerTabCb.Checked = false; 
                    return;
                }
                ClearTimeTB.Enabled = false;    
                SysParam.isClearData = true;
                SysParam.ClearTime = ClearTime;
            }
            else 
            {
                SysParam.isClearData = false;
                SysParam.ClearTime = 180;
                ClearTimeTB.Enabled = true;
            }
        }
        private void ComNamesCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //当前选择串口时确定当前选择的串口是否可用
            SysParam.isMsgSend = false;
            string StrComName = ComNamesCB.SelectedItem.ToString().Trim();
            if ("".Equals(StrComName)) { SysParam.InitNoCom();return; }
            if (!MG3732_SendMSG_PUD.OpenSerial(StrComName))
            {MessageBox.Show("對不起,當前端口無法打開,请查看当前端口是否被占用...");
             ComNamesCB.SelectedIndex = 0;return;}
            //查看当前的串口是否有效
            if (!MG3732_SendMSG_PUD.IsComCanUse())
            { MessageBox.Show("對不起，當前串口無效，请尝试其他端口..."); MG3732_SendMSG_PUD.CloseSerial(); return; }
            //查看当前的3G模块是否有SIM卡
            if (!MG3732_SendMSG_PUD.isExistSIM_MG3732())
            { MessageBox.Show("對不起，當前的3G设备没有SIM卡,请先装上SIM卡..."); MG3732_SendMSG_PUD.CloseSerial(); return; }
            //查看是否添加了人员信息
            MG3732_SendMSG_PUD.CloseSerial();
            SysParam.isMsgSend = true;
            SysParam.ComName = StrComName;
            MsgPersonHelpCB.Enabled = true; MsgBatteryLowCB.Enabled = true; MsgAreaControlCB.Enabled = true; MsgDeviceThroubleCB.Enabled = true; MsgPersonResCB.Enabled = true;
        }
        private void MsgPersonHelpCB_CheckedChanged(object sender, EventArgs e)
        {
            SysParam.isSendPersonHelpMsg = MsgPersonHelpCB.Checked;
        }

        private void MsgPersonResCB_CheckedChanged(object sender, EventArgs e)
        {
            SysParam.isSendPersonResMsg = MsgPersonResCB.Checked;
        }

        private void MsgAreaControlCB_CheckedChanged(object sender, EventArgs e)
        {
            SysParam.isSendAreaControlMsg = MsgAreaControlCB.Checked;
        }

        private void MsgBatteryLowCB_CheckedChanged(object sender, EventArgs e)
        {
            if (MsgBatteryLowCB.Checked)
            {
                if (SysParam.isLowBattery)
                    SysParam.isSendBatteryLowMsg = true;
                else
                {
                    SysParam.isSendBatteryLowMsg = false;
                    MsgBatteryLowCB.Checked = false;
                    MessageBox.Show("請先設置并選擇低電量報警条件!");
                }
            }
            else SysParam.isSendBatteryLowMsg = false;
        }

        private void MsgDeviceThroubleCB_CheckedChanged(object sender, EventArgs e)
        {
            SysParam.isSendDeviceTrouble = MsgDeviceThroubleCB.Checked;
        }

        private void EditPhoneBtn_Click(object sender, EventArgs e)
        {
            EditPhone MyEditPhone = new EditPhone();
            MyEditPhone.ShowDialog();
        }

        private void isOnTimeClearHandlerWarm_CheckedChanged(object sender, EventArgs e)
        {
            if (isOnTimeClearHandlerWarmCB.Checked)
            {
               string StrOnTimeWarms = OnTimeHandlerWarmTimesTB.Text;
               if ("".Equals(StrOnTimeWarms))
               {
                   isOnTimeClearHandlerWarmCB.Checked = false;
                   MessageBox.Show("對不起，定時清理已處理警告的時間不能為空!");
                   return;
               }
               int OnTime = 0;
               try {
                   OnTime = Convert.ToInt32(StrOnTimeWarms);
               }catch(Exception)
               {
                   isOnTimeClearHandlerWarmCB.Checked = false;
                   MessageBox.Show("對不起，定時清理已處理警告的時間格式有誤!");
                   return;
               }
               if (OnTime <= 0)
               {
                   isOnTimeClearHandlerWarmCB.Checked = false;
                   MessageBox.Show("對不起，定時清理已處理警告的時間要大于0!");
                   return;
               }
               OnTimeHandlerWarmTimesTB.Enabled = false;
               SysParam.isOnTimeClearHandlerWarm = true;
               SysParam.OnTimes = OnTime;
            }
            else
            {
               OnTimeHandlerWarmTimesTB.Enabled = true;
               SysParam.isOnTimeClearHandlerWarm = false;
               SysParam.OnTimes = 0;
            }
        }
        /// <summary>
        /// 添加人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBtn_Click(object sender, EventArgs e)
        {
            PersonIDTB.Text = "0000";
            PersonIDTB.Enabled = true;
            PersonNameTB.Enabled = true;
            PersonAccessCB.Enabled = true;
            PersonNameTB.Text="";
            PersonPswTB.Text="";
        }
        private void UpdateToLeftBtn_Click(object sender, EventArgs e)
        {
            string StrPersonID = PersonIDTB.Text.ToUpper();
            string StrPersonName = PersonNameTB.Text.ToUpper().Trim();
            string StrPersonPsw = PersonPswTB.Text.ToUpper().Trim();
            if ("".Equals(StrPersonID))
            {
                MessageBox.Show("對不起，人員ID不能為空!");
                return;
            }
            if (StrPersonID.Length != 4)
            {
                MessageBox.Show("對不起，人員ID長度為4!");
                return;
            }
            if ("0000".Equals(StrPersonID))
            {
                MessageBox.Show("對不起，人員ID不能為\"0000\"!");
                return;
            }
            //不要區分大小寫
            if ("FFFF".Equals(StrPersonID,StringComparison.CurrentCultureIgnoreCase))
            {
                MessageBox.Show("對不起，人員ID不能為\"FFFF\"!");
                return;
            }
            if (ConstInfor.dmatekname.Equals(StrPersonName, StringComparison.CurrentCultureIgnoreCase)
                && ConstInfor.dmatekpsw.Equals(StrPersonPsw, StringComparison.CurrentCultureIgnoreCase))
            {
                MessageBox.Show("對不起，不能將本公司內部賬號添加到列表中!");
                return;
            }
            byte[] PersonID = new byte[2];
            try
            {
                PersonID[0] = Convert.ToByte(StrPersonID.Substring(0, 2),16);
                PersonID[1] = Convert.ToByte(StrPersonID.Substring(2, 2),16);   
            }
            catch(Exception)
            {
                MessageBox.Show(ConstInfor.StrPersonIDFormatErr);
                return;
            }
            if ("".Equals(StrPersonName))
            {
                MessageBox.Show("對不起，操作人員的名稱不能為空!");
                return;
            }
            Person psn = null;
            //判断当前的人员是否在列表中
            if (CommonCollection.Persons.ContainsKey(StrPersonID))
            {//列表中已经包含了该项,查看用户是否选择了项
                int count = PersonLv.SelectedItems.Count;
                if(count <= 0)
                {
                   MessageBox.Show("對不起，你選擇的人員項不存在!");
                   return;
                }
                string StrNewPersonID = PersonLv.SelectedItems[0].Text;
                if(!StrNewPersonID.Equals(StrPersonID))
                {
                    MessageBox.Show(ConstInfor.StrSorryUpdateUserExist);
                    return;
                }
                string StrPsAdmin = PersonAccessCB.Text;
                Person person = null;
                if (CommonCollection.Persons.TryGetValue(StrPersonID, out person))
                {
                    psn = GetExistPerson(StrPersonName,StrPersonPsw);
                    if (null != psn && !(psn.ID[0].ToString("X2").PadLeft(2, '0') + psn.ID[1].ToString("X2").PadLeft(2, '0')).Equals(StrPersonID))
                    {
                        MessageBox.Show("對不起，不能修改為用户名和密码都相同的兩個操作員！");
                        return;
                    }
                    person.Name = StrPersonName;
                    person.Ps = StrPersonPsw;
                    if (ConstInfor.StrAdminPerson.Equals(StrPsAdmin))
                    {
                        person.PersonAccess = PersonAccess.AdminPerson;
                    }
                    else
                    {
                        person.PersonAccess = PersonAccess.SimplePerson;
                    }
                }
                PersonIDTB.Enabled = false;
                UpdatePersonListView();
                return;
            }
            psn = GetExistPerson(StrPersonName, StrPersonPsw);
            if (null != psn)
            {
                MessageBox.Show("對不起，不能添加用户名和密码都相同的兩個操作員！");
                return;
            }
            Person NewPerson = null;
            if (PersonAccessCB.SelectedIndex == 0)
            {
                NewPerson = new Person(PersonID, StrPersonName, StrPersonPsw, 0);
            }
            else
            {
                NewPerson = new Person(PersonID, StrPersonName, StrPersonPsw, 1);
            }
            CommonCollection.Persons.Add(PersonID[0].ToString("X2").PadLeft(2, '0') + PersonID[1].ToString("X2").PadLeft(2, '0'), NewPerson);
            PersonIDTB.Enabled = false;
            UpdatePersonListView();
        }

        private Person GetExistPerson(string UserName, string UserPsw)
        { 
            foreach(KeyValuePair<string,Person> person in CommonCollection.Persons)
            {
                if(null == person.Value)
                    continue;
                if (UserName.Equals(person.Value.Name.Trim().ToUpper()) && UserPsw.Equals(person.Value.Ps.Trim().ToUpper()))
                    return person.Value;
            }
            return null;
        }
        private void DeleBtn_Click(object sender, EventArgs e)
        {
            //删除项
            int count = PersonLv.SelectedItems.Count;
            if (count <= 0)
            {
                MessageBox.Show("對不起,你還沒有選擇需要删除的项!");
                return;
            }
            string StrPersonID = PersonLv.SelectedItems[0].Text;
            if ("0000".Equals(StrPersonID) && "ADMIN".Equals(PersonLv.SelectedItems[0].SubItems[1].Text.Trim().ToUpper()))
            {
                MessageBox.Show("對不起，Admin的操作員不能删除!");
                return;
            }
            CommonCollection.Persons.Remove(StrPersonID);
            string StrPerSonName = PersonLv.SelectedItems[0].SubItems[1].Text;
            UpdatePersonListView();
        }
        private void PersonLv_Click(object sender, EventArgs e)
        {
            int count=PersonLv.SelectedItems.Count;
            if (count<=0)return;
            string StrPersonID = PersonLv.SelectedItems[0].Text.Trim();
            string StrPersonName = PersonLv.SelectedItems[0].SubItems[1].Text.Trim();
            string StrPersonPsw = PersonLv.SelectedItems[0].SubItems[2].Text;
            string StrPersonAccess = PersonLv.SelectedItems[0].SubItems[3].Text;

            if (StrPersonID.Equals("0000") && StrPersonName.ToUpper().Equals("ADMIN"))
            { PersonNameTB.Enabled = false; PersonAccessCB.Enabled = false; }
            else { PersonNameTB.Enabled = true; PersonAccessCB.Enabled = true; }

            PersonIDTB.Text = StrPersonID;
            PersonIDTB.Enabled = false;
            PersonNameTB.Text = StrPersonName;
            PersonPswTB.Text = StrPersonPsw;
            if (ConstInfor.StrAdminPerson.Equals(StrPersonAccess.Trim())) PersonAccessCB.SelectedIndex=1;
            else if (ConstInfor.StrSimplePerson.Equals(StrPersonAccess.Trim())) PersonAccessCB.SelectedIndex=0;
        }

        private void ParamSet_FormClosed(object sender, FormClosedEventArgs e)
        {            
            try
            {
                if (frm != null)
                {
                    if (null != frm.MyParamSet)
                    {
                        if (!frm.MyParamSet.IsDisposed)
                        {
                            frm.MyParamSet.Dispose();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                frm.MyParamSet = null;
                if (null != DefinePlay.axMediaplayer)
                    DefinePlay.Close();
            }
        }

        private void PersonAccessCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            string StrCurPersonId = frm.CurPerson.ID.ToString().PadLeft(4, '0');
            string StrSetPersonId = PersonIDTB.Text.Trim();
            if (PersonAccessCB.SelectedIndex == 0)
            {
                if (StrCurPersonId.Equals(StrSetPersonId))
                {
                    if (DialogResult.Yes == MessageBox.Show("你修改自己權限為“一般人員”后，将不能進入設置模式，確定要修改自己的權限嗎?", "提醒", MessageBoxButtons.YesNo))PersonAccessCB.SelectedIndex = 0;
                    else PersonAccessCB.SelectedIndex = 1;
                    return;
                }
            }
        }
        private void AllAreaCB_CheckedChanged(object sender, EventArgs e)
        {
            foreach (TreeNode treenode in ReferTreeView.Nodes)
            {
                treenode.Checked = AllAreaCB.Checked;
                foreach (TreeNode node in treenode.Nodes)
                {
                    node.Checked = AllAreaCB.Checked;
                }
            }
        }

        private void isStopAlarmCB_CheckedChanged(object sender, EventArgs e)
        {
            string strstoptime = string.Empty;
            int stoptime = 0;
            strstoptime = tagstoptimetb.Text;
            if (isStopAlarmCB.Checked)
            {
                if ("".Equals(strstoptime))
                {
                    MessageBox.Show("卡片的未移動時間不能為空!");
                    isStopAlarmCB.Checked = false;
                    return;
                }
                try
                {
                    stoptime = Convert.ToInt32(strstoptime);
                }
                catch (Exception)
                {
                    MessageBox.Show("卡片的未移動時間格式有誤!");
                    tagstoptimetb.Text = "";
                    isStopAlarmCB.Checked = false;
                    return;
                }
                if (stoptime < 30 || stoptime > 100)
                {
                    MessageBox.Show("卡片的未移動時間不能小於30且不能大於100!");
                    tagstoptimetb.Text = "";
                    isStopAlarmCB.Checked = false;
                    return;
                }
                tagstoptimetb.Enabled = false;
            }
            else
            {
                tagstoptimetb.Enabled = true;
            }
        }

        private void GSensorCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (GSensorCB.SelectedIndex)
            {
                case 0:
                    gsstartdtp.Enabled = false; 
                    gsenddtp.Enabled = false;
                    break;
                case 1:
                    gsstartdtp.Enabled = true; 
                    gsenddtp.Enabled = true;
                    break;
                case 2:
                    gsstartdtp.Enabled = false; 
                    gsenddtp.Enabled = false;
                    break;
            }
        }

        private void WorkTimeTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TimeZoneBox
            if (WorkTimeTypeCB.SelectedIndex == 1)
            { 
                startTP.Enabled = true; 
                EndTP.Enabled = true;
            }
            else
            { 
                startTP.Enabled = false;
                EndTP.Enabled = false;
            }
        }

        private void JumpLimitCB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SoundTimeTB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void OptModelCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OptModelCB.SelectedIndex == 0)
            {//选择信号强度差值
                SysParam.curOptimalMedol = OptimizationModel.SigStrengthValue;
                SelectSigValueBox.Visible = true;
                SelectSigValueBox.Location = new Point(276, 19);
                SigThreCB.SelectedIndex = SysParam.RssiThreshold;
                PopTimesBox.Visible = false;
            }
            else if (OptModelCB.SelectedIndex == 1)
            {//选择连续跳点次数
                SysParam.curOptimalMedol = OptimizationModel.HopTimes;
                PopTimesBox.Visible = true;
                PopTimesBox.Location = new Point(276, 19);
                PopTimesCb.SelectedIndex = SysParam.PopTimes;
                SelectSigValueBox.Visible = false;
            }
        }
        //点击后对列进行排序
        public int curSortColumn = 0;
        private void RouterListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != curSortColumn)
            {
                RouterListView.Sorting = SortOrder.Ascending;
            }
            else
            {
                if (RouterListView.Sorting == SortOrder.Ascending)
                {
                    RouterListView.Sorting = SortOrder.Descending;
                }
                else
                {
                    RouterListView.Sorting = SortOrder.Ascending;
                }
            }
            RouterListView.Sort();
            RouterListView.ListViewItemSorter = new ListViewItemComparer(e.Column, RouterListView.Sorting);
            
            //更新显示
            CancalSortingView(curSortColumn);
            UpdateSortingView(e.Column, RouterListView.Sorting);
            curSortColumn = e.Column;
        }
        public void UpdateSortingView(int col,SortOrder order)
        {
            switch (col)
            {
                case 0:
                    if (order == SortOrder.Ascending)
                    {
                        this.RouterListView.Columns[col].Text = "ID(↓)";
                    }
                    else
                    {
                        this.RouterListView.Columns[col].Text = "ID(↑)";
                    }
                    break;
                case 1:
                    if (order == SortOrder.Ascending)
                    {
                        this.RouterListView.Columns[col].Text = "名稱(↓)";
                    }
                    else
                    {
                        this.RouterListView.Columns[col].Text = "名稱(↑)";
                    }
                    break;
                case 2:
                    if (order == SortOrder.Ascending)
                    {
                        this.RouterListView.Columns[col].Text = "類型(↓)";
                    }
                    else
                    {
                        this.RouterListView.Columns[col].Text = "類型(↑)";
                    }
                    break;
                case 3:
                    if (order == SortOrder.Ascending)
                    {
                        this.RouterListView.Columns[col].Text = "是否可見(↓)";
                    }
                    else
                    {
                        this.RouterListView.Columns[col].Text = "是否可見(↑)";
                    }
                    break;
                case 4:
                    if (order == SortOrder.Ascending)
                    {
                        this.RouterListView.Columns[col].Text = "所有組(↓)";
                    }
                    else
                    {
                        this.RouterListView.Columns[col].Text = "所有組(↑)";
                    }
                    break;
                case 5:
                    if (order == SortOrder.Ascending)
                    {
                        this.RouterListView.Columns[col].Text = "所在區域(↓)";
                    }
                    else
                    {
                        this.RouterListView.Columns[col].Text = "所在區域(↑)";
                    }
                    break;
            }
        }

        public void CancalSortingView(int col)
        {
            switch (col)
            {
                case 0: this.RouterListView.Columns[col].Text = "ID";
                    break;
                case 1: this.RouterListView.Columns[col].Text = "名稱";
                    break;
                case 2: this.RouterListView.Columns[col].Text = "類型";
                    break;
                case 3: this.RouterListView.Columns[col].Text = "是否可見";
                    break;
                case 4: this.RouterListView.Columns[col].Text = "所有組";
                    break;
                case 5: this.RouterListView.Columns[col].Text = "所在區域";
                    break;
            }
        }
        //对区域列表进行排序
        public int curareasortcolumn = 0;
        private void AreaListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != curareasortcolumn)
            {
                AreaListView.Sorting = SortOrder.Ascending;
            }
            else
            {
                if (AreaListView.Sorting == SortOrder.Ascending)
                {
                    AreaListView.Sorting = SortOrder.Descending;
                }
                else
                {
                    AreaListView.Sorting = SortOrder.Ascending;
                }
            }
            AreaListView.Sort();
            AreaListView.ListViewItemSorter = new ListViewItemComparer(e.Column, AreaListView.Sorting);

            CancelAreaSort(curareasortcolumn);
            UpdateAreaSort(e.Column, AreaListView.Sorting);
            curareasortcolumn = e.Column;
        }
        private void UpdateAreaSort(int col,SortOrder sortorder)
        {
            switch (col)
            {
                case 0:
                    if (sortorder == SortOrder.Ascending)
                    {
                        this.AreaListView.Columns[col].Text = "ID(↓)";
                    }
                    else
                    {
                        this.AreaListView.Columns[col].Text = "ID(↑)";
                    }
                    break;
                case 1:
                    if (sortorder == SortOrder.Ascending)
                    {
                        this.AreaListView.Columns[col].Text = "名稱(↓)";
                    }
                    else
                    {
                        this.AreaListView.Columns[col].Text = "名稱(↑)";
                    }
                    break;
                case 2:
                    if (sortorder == SortOrder.Ascending)
                    {
                        this.AreaListView.Columns[col].Text = "類型(↓)";
                    }
                    else
                    {
                        this.AreaListView.Columns[col].Text = "類型(↑)";
                    }
                    
                    break;
                case 3:
                    if (sortorder == SortOrder.Ascending)
                    {
                        this.AreaListView.Columns[col].Text = "所屬組(↓)";
                    }
                    else
                    {
                        this.AreaListView.Columns[col].Text = "所屬組(↑)";
                    }
                   
                    break;
                case 4:
                    if (sortorder == SortOrder.Ascending)
                    {
                        this.AreaListView.Columns[col].Text = "地圖名稱(↓)";
                    }
                    else
                    {
                        this.AreaListView.Columns[col].Text = "地圖名稱(↑)";
                    }
                    break;
            }
        }
        private void CancelAreaSort(int col)
        {
            switch (col)
            {
                case 0: this.AreaListView.Columns[col].Text = "ID";
                    break;
                case 1: this.AreaListView.Columns[col].Text = "名稱";
                    break;
                case 2: this.AreaListView.Columns[col].Text = "類型";
                    break;
                case 3: this.AreaListView.Columns[col].Text = "所屬組";
                    break;
                case 4: this.AreaListView.Columns[col].Text = "地圖名稱";
                    break;
            }
        }

        private void AutoClearHandleCB_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoClearHandleCB.Checked)
            {//是否自动处理
                string StrAutoClearTime = AutoClearTimesTB.Text;
                if ("".Equals(StrAutoClearTime))
                {
                    MessageBox.Show("對不起，自動清理已處理警報時間不能為空!");
                    AutoClearHandleCB.Checked = false;
                    return;
                }
                int AutoClearTime = 0;
                try
                {
                    AutoClearTime = Convert.ToInt32(StrAutoClearTime);
                }catch(Exception)
                {
                    MessageBox.Show("對不起，自動清理已處理警報時間格式有誤!");
                    AutoClearHandleCB.Checked = false;
                    return;
                }
                if (AutoClearTime < 0)
                {
                    MessageBox.Show("對不起，自動清理已處理警報時間要大於0!");
                    AutoClearHandleCB.Checked = false;
                    return;
                }
                SysParam.isClearHandleAlarm = true;
                SysParam.AutoClearHandleAlarmTime = AutoClearTime;
                AutoClearTimesTB.Enabled = false;

            }
            else
            {
                SysParam.isClearHandleAlarm = false;
                SysParam.AutoClearHandleAlarmTime = 3600;
                AutoClearTimesTB.Enabled = true;
            }
        }
        private void debugcb_CheckedChanged(object sender, EventArgs e)
        {
            SysParam.isDebug = debugcb.Checked;
            if (SysParam.isDebug)
            {
                setarroundcb.Visible = true;
                setarroundtx.Visible = true;
            }else
            {
                setarroundcb.Visible = false;
                setarroundtx.Visible = false;
            }
        }
        private void setarroundcb_CheckedChanged(object sender, EventArgs e)
        {
            SysParam.isSettingArroundDevices = setarroundcb.Checked;
        }
        /// <summary>
        /// 节点/参考点设备失去连接后，再次连接上，异常警报消失使能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void devcnnalarmmisscb_CheckedChanged(object sender, EventArgs e)
        {
            SysParam.isDevCnnLoss = devcnnalarmmisscb.Checked;
        }

        private void soundPersonHelpCB_CheckedChanged(object sender, EventArgs e)
        {
            SysParam.isSoundPersonHelp = soundPersonHelpCB.Checked;
        }

        private void SoundAreaControlCB_CheckedChanged(object sender, EventArgs e)
        {
            SysParam.isSoundAreaControl = SoundAreaControlCB.Checked;
        }

        private void SoundPersonResCB_CheckedChanged(object sender, EventArgs e)
        {
            SysParam.isSoundPersonRes = SoundPersonResCB.Checked;
        }

        private void SoundBatteryLowCB_CheckedChanged(object sender, EventArgs e)
        {
            SysParam.isSoundBatteryLow = SoundBatteryLowCB.Checked;
        }

        private void SoundDeviceThroubleCB_CheckedChanged(object sender, EventArgs e)
        {
            SysParam.isSoundDeviceTrouble = SoundDeviceThroubleCB.Checked;
        }

        /// <summary>
        /// 手動保存文件資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Manualsavebtn_Click(object sender, EventArgs e)
        {
            String str = "";
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (SysParam.SaveTag(0))
                {
                    str = "保存卡片資料成功!";
                }
                else
                {
                    str = "保存卡片資料失敗!";
                }
            }
            catch
            {
                str = "保存卡片資料失敗!";
            }
            this.Cursor = Cursors.Default;
            MessageBox.Show(str);
        }

        private void groupmulsavebtn_Click(object sender, EventArgs e)
        {
            String str = "";
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (SysParam.SaveGroup(1))
                {
                    str = "保存組別資料成功!";
                }
                else 
                {
                    str = "保存組別資料失敗!";
                }
            }catch
            {
                str = "保存組別資料失敗!";
            }
            this.Cursor = Cursors.Default;
            MessageBox.Show(str);
        }

        private void areamulsavebtn_Click(object sender, EventArgs e)
        {
            String str = "";
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (SysParam.SaveAreaInfo(0))
                {
                    str = "保存區域訊息成功!";
                }
                else
                {
                    str = "保存區域訊息失敗!";
                }
            }
            catch {
                str = "保存區域訊息失敗!";
            }
            this.Cursor = Cursors.Default;
            MessageBox.Show(str);
        }

        private void personmulsavebtn_Click(object sender, EventArgs e)
        {
            String str = "";
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (SysParam.SavePerson(0))
                {
                    str = "保存人員訊息成功!";
                }
                else
                {
                    str = "保存人員訊息失敗!";
                }
            }catch{
                str = "保存人員訊息失敗!";
            }
            this.Cursor = Cursors.Default;
            MessageBox.Show(str);
        }

        private void backareabtn_Click(object sender, EventArgs e)
        {
            String str = "";

            if(MessageBox.Show("本次備份會刪除掉上次的備份記錄，確定要備份嗎?","提醒",MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //備份地圖
                //先备份图片讯息
                FileOperation.CopyFolderTo(FileOperation.MapPath,FileOperation.BackUpMapPath);
                if (SysParam.SaveAreaInfo(1))
                {
                    str = "備份區域訊息成功!";
                }
                else
                {
                    str = "備份區域訊息失敗!";
                }
            }
            catch
            {
                str = "備份區域訊息失敗!";
            }
            this.Cursor = Cursors.Default;
            MessageBox.Show(str);
        }

        private void backuptagbtn_Click(object sender, EventArgs e)
        {
            saveTagInfor(1);
        }

        private void backuppersonbtn_Click(object sender, EventArgs e)
        {
            String str = "";
            if (MessageBox.Show("本次備份會刪除掉上次的備份記錄，確定要備份嗎?", "提醒", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (SysParam.SavePerson(1))
                {
                    str = "備份人員訊息成功!";
                }
                else
                {
                    str = "備份人員訊息失敗!";
                }
            }
            catch
            {
                str = "備份人員訊息失敗!";
            }
            this.Cursor = Cursors.Default;
            MessageBox.Show(str);
        }
        /// <summary>
        /// 導入人員訊息資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importbackuppersonbtn_Click(object sender, EventArgs e)
        {
            if (!SysParam.ImportBackUpPerson()){
                MessageBox.Show("對不起,導入備份資料失敗!");
                return;
            }
            //備份資料成功, 重新加載列表
            CommonCollection.Persons.Clear();
            SysParam.LoadPerson();
            UpdatePersonListView();
            MessageBox.Show("導入人員的備份資料成功!");
        }

        private void backupgroupbtn_Click(object sender, EventArgs e)
        {
            String str = "";
            if (MessageBox.Show("本次備份會刪除掉上次的備份記錄，確定要備份嗎?", "提醒", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (SysParam.SaveGroup(1))
                {
                    str = "備份組別訊息成功!";
                }
                else
                {
                    str = "備份組別訊息失敗!";
                }
            }
            catch
            {
                str = "備份組別訊息失敗!";
            }
            this.Cursor = Cursors.Default;
            MessageBox.Show(str);
        }
        /// <summary>
        /// 導入備份組別資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importbackupgroupbtn_Click(object sender, EventArgs e)
        {
            if (!SysParam.ImportBackUpGroup())
            {
                MessageBox.Show("對不起,導入備份資料失敗!");
                return;
            }
            //備份資料成功, 重新加載列表
            CommonCollection.Groups.Clear();
            SysParam.LoadGroup();
            UpdateGroupListView();
            MessageBox.Show("導入組別的備份資料成功!");
        }

        private void importbackupareabtn_Click(object sender, EventArgs e)
        {
            if (!SysParam.ImportBackUpArea())
            {
                MessageBox.Show("對不起,導入備份資料失敗!");
                return;
            }
            //備份資料成功, 重新加載列表
            CommonCollection.Areas.Clear();
            SysParam.LoadAreaInfo();
            UpdateAraeListView();
            SysParam.SetPanelStatus(4);
            MessageBox.Show("導入區域的備份資料成功!");
        }

        private void importbackuptagbtn_Click(object sender, EventArgs e)
        {
            if (!FileModel.fileInit().getBackUpCommCollTags()) //改變存儲方式，代替了SysParam.LoadPersonTagInfo()方法
            //if (!SysParam.ImportBackUpTag())
            {
                MessageBox.Show("對不起,導入備份資料失敗!");
                return;
            }
            //備份資料成功, 重新加載列表
            //CommonCollection.Tags.Clear();
            //SysParam.LoadPersonTagInfo(0);
            UpdateTagListView();
            MessageBox.Show("導入Tag的備份資料成功!");
        }

        private Socket client;
        private IPEndPoint servicePoint;
        private void button3_Click(object sender, EventArgs e) //打開一個UDP
        {
            try
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                int servicePort = int.Parse(PortTB.Text);
                servicePoint = new IPEndPoint(IPAddress.Parse(IPComBox.Text), servicePort);
                client.Bind(servicePoint);
                Thread t2 = new Thread(ReciveMsg);
                t2.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("打開失敗");
                return;
            }
            button3.Text = "打開成功";
        }


        private void button4_Click(object sender, EventArgs e)
        {
            try 
            {
                client.Close();
                button3.Text = "打開";
            }
            catch(Exception ex)
            {
                MessageBox.Show("關閉UDP出現異常");
            }          
        }


        EndPoint receivePoint;
        /// <summary>
        /// 接收发送给本机ip对应端口号的数据报
        /// </summary>
        private void ReciveMsg()
        {
            receivePoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {             
                    byte[] buffer = new byte[1024 * 5];
                    int length = client.ReceiveFrom(buffer, ref receivePoint);//接收数据报 
                    if(length < 1)return;
                    if (buffer[0] != 0xfc || buffer[length - 1] != 0xfb) return; //包头，包尾
                    if (buffer[1] != 0xd3) return;

                    this.Invoke((EventHandler)(delegate
                    {
                        textBox1.Text = "";
                        for (int i = 2; i < length - 2; i++)
                        {
                            textBox1.Text += buffer[i].ToString("X2");
                        }
                    }));
                    buffer[0] = 0xfa;
                    buffer[length - 1] = 0xf9;
                    client.SendTo(buffer, 0, length, SocketFlags.None, receivePoint);
                }
                catch (Exception e)
                {
                    string msg = e.Message;
                    break ;
                }
            }
        }


        private void 全部重置ToolStripMenuItem_Click(object sender, EventArgs e) //全部重置
        {
            foreach (KeyValuePair<string, Tag> tag in CommonCollection.Tags)
            {
                tag.Value.EndWorkDT = DrawIMG.getMaxDate();//DateTime.MaxValue;
            }
            UpdateTagListView();
        }


        private void selectMenuItem_Click(object sender, EventArgs e) //選擇重置
        {
            
        }

        private void TagListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tag tag = null;
            if (TagListView.SelectedItems.Count <= 0)
                return;
            AllAreaCB.Checked = false;
            AllAreaCB_CheckedChanged(null, null);
            TagIDTB.Enabled = false;
            TagIDTB.Text = TagListView.SelectedItems[0].Name;
            TagNameTB.Text = TagListView.SelectedItems[0].SubItems[1].Text;
            CommonCollection.Tags.TryGetValue(TagIDTB.Text, out tag);
            if (null == tag)
            {
                MessageBox.Show("重置出現異常");
                return;
            }
            foreach (KeyValuePair<string, Tag> tagItem in CommonCollection.Tags)
            {
                if (!tag.Name.Equals(tagItem.Value.Name)) continue;
                tagItem.Value.EndWorkDT = DrawIMG.getMaxDate();//DateTime.MaxValue;
            }
            UpdateTagListView();
        }

        private void BackUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SysParam.ImportBackUpTag())
            {
                MessageBox.Show("對不起,導入備份資料失敗!");
                return;
            }
            //備份資料成功, 重新加載列表
            //CommonCollection.Tags.Clear();
            //SysParam.LoadPersonTagInfo(0);
            UpdateTagListView();
            MessageBox.Show("導入Tag的備份資料成功!");
        }

        private void saveBackUPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveTagInfor(3);
        }

        private void saveTagInfor(int type) 
        {
            String str = "";
            if (MessageBox.Show("本次備份會刪除掉上次的備份記錄，確定要備份嗎?", "提醒", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (SysParam.SaveTag(type))
                {
                    str = "備份卡片資料成功!";
                }
                else
                {
                    str = "備份卡片資料失敗!";
                }
            }
            catch
            {
                str = "備份卡片資料失敗!";
            }
            this.Cursor = Cursors.Default;
            MessageBox.Show(str);
        }

    }


    public enum OptimizationModel
    { 
        SigStrengthValue,
        HopTimes
    }
}
