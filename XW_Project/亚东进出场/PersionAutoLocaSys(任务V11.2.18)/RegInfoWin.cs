using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace PersionAutoLocaSys
{
    public partial class RegInfoWin : Form
    {
        private Form1 frm = null;
        private string StrAreaID = "";
        private Timer ListTimer = null;
        private Timer ImageTimer = null;
        Area MyArea = null;
        private Bitmap ImageBitmap = null;
        float scalew, scaleh;
        public RegInfoWin()
        {
            InitializeComponent();
        }
        public RegInfoWin(Form1 frm,string StrAreaID)
        {
            InitializeComponent();
            this.frm = frm;
            this.StrAreaID = StrAreaID;
        }
        public void RegInfoWin_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new Size(967, 603);
            this.MinimumSize = new Size(967, 603);
            scalew = (float)ImagePanel.Width / ConstInfor.MapWidth;
            scaleh = (float)ImagePanel.Height / ConstInfor.MapHeight;

           // scale = scalew < scaleh?scalew : scaleh;
            if (null == StrAreaID || "".Equals(StrAreaID))
            {
                MessageBox.Show("區域不能為空!");
                this.Close();
                return;
            }
            CommonCollection.Areas.TryGetValue(StrAreaID, out MyArea);
            
            if (null == MyArea)
            {
                MessageBox.Show("區域不能為空!");
                this.Close();
                return;
            }
            if (null == MyArea.Name || "".Equals(MyArea.Name))
            {
                this.Text = "當前區域為：" + StrAreaID;
            }
            else {
                this.Text = "當前區域為：" + MyArea.Name + "(" + StrAreaID+")";
            }
            if (MyArea.AreaBitMap == null)
            {
                MessageBox.Show("當前區域沒有設置地圖!");
            }
            else
            {
                if (null != MyArea.AreaBitMap.MyBitmap)
                {
                    //if (scalew < scaleh) ImageBitmap = new Bitmap(MyArea.AreaBitMap.MyBitmap, ImagePanel.Width, (int)(ConstInfor.MapHeight * scalew));
                    //else ImageBitmap = new Bitmap(MyArea.AreaBitMap.MyBitmap, (int)(scaleh * ImagePanel.Width), ImagePanel.Height);

                    ImageBitmap = new Bitmap(MyArea.AreaBitMap.MyBitmap, ImagePanel.Width, ImagePanel.Height);
                }
                else
                {
                    MessageBox.Show("當前區域沒有設置地圖!");
                }
            }
            ListTimer = new Timer();
            ListTimer.Interval = 1000;
            ListTimer.Tick += ShowTimer_Tick;
            ListTimer.Start();
            ImageTimer = new Timer();
            ImageTimer.Interval = 1000;
            ImageTimer.Tick += ImageTimer_Tick;
            ShowModeTabControl.SelectedIndex = 1;
        }
        /// <summary>
        /// 切换显示模式时，初始化各种模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShowModeTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ShowModeTabControl.SelectedIndex)
            {
                case 0:
                    // 列表模式
                    if(ImageTimer!=null)
                        ImageTimer.Stop();
                    ListTimer.Start();
                    break;
                case 1:
                    // 图形模式
                    if (ListTimer != null)
                        ListTimer.Stop();
                    ImageTimer.Start();
                    //初始化所有参考点的周围位置占用信息
                    CommonBoxOperation.InitAllReferLocaInfo();
                    break;
            }
        }
        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        private void ShowTimer_Tick(Object obj,EventArgs args)
        {
            String StrRouterName, StrRouterID, StrTagName, StrID;
            ListViewItem item = null;
            int num = CommonBoxOperation.GetCurAreaTagNum(StrAreaID);
            label4.Text = "人員當前總數：" + num;
            ArrayList MyArrayList = new ArrayList();
            Area ar = null;
            try{
                foreach (ListViewItem im in AreaTaglistView.Items)
                {
                    StrID = im.Name;
                    if (!CommonCollection.TagPacks.ContainsKey(StrID))
                        MyArrayList.Add(StrID);
                }
                //清除TagPacks中已经不存在的项
                for (int i = 0; i < MyArrayList.Count;i++)
                    AreaTaglistView.Items.RemoveByKey(MyArrayList[i].ToString());

                foreach (KeyValuePair<string,TagPack> tbk in CommonCollection.TagPacks)
                {
                    StrRouterID = tbk.Value.RD_New[0].ToString("X2") + tbk.Value.RD_New[1].ToString("X2");
                    //判断当前的Router是否在本区域中
                    Area CurArea = CommonBoxOperation.GetAreaFromRouterID(StrRouterID);
                    if (null == CurArea)
                        continue;
                    if (CurArea.ID[0] != MyArea.ID[0] || CurArea.ID[1] != MyArea.ID[1])
                        continue;
                    StrRouterName = CommonBoxOperation.GetRouterName(StrRouterID);
                    //判断当前Tag是否在当前的区域中
                    if (AreaTaglistView.Items.ContainsKey(tbk.Key))
                    {
                        item = null;
                        if (AreaTaglistView.Items.Count > 0)
                        {
                            item = AreaTaglistView.FindItemWithText(tbk.Key, false, 0);
                        }
                        else
                            continue;
                        if (null == item)
                        {
                            StrTagName = CommonBoxOperation.GetTagName(tbk.Key);
                            if (null == StrTagName)
                                continue;
                            item = AreaTaglistView.FindItemWithText(StrTagName, false, 0);
                        }
                        ar = CommonBoxOperation.GetRouterArea(StrRouterID);
                        if (ar == null)
                        {
                            continue;
                        }
                        if (!(ar.ID[0].ToString("X2") + ar.ID[1].ToString("X2")).Equals(StrAreaID))
                        {
                            continue;
                        }
                        if (null != item)
                        {
                            if (null == StrRouterName || "".Equals(StrRouterName))
                            {
                                item.SubItems[2].Text = StrRouterID;
                            }
                            else
                                item.SubItems[2].Text = StrRouterName;
                            item.SubItems[3].Text = tbk.Value.SigStren.ToString();
                            item.SubItems[4].Text = tbk.Value.Bat.ToString();
                            item.SubItems[5].Text = tbk.Value.ResTime.ToString();
                            item.SubItems[6].Text = tbk.Value.ReportTime.ToString();
                        }
                    }
                    else
                    {
                        ListViewItem Item = new ListViewItem();
                        Item.Name = tbk.Key;//其中Name是指Tag的ID
                        Item.Text = tbk.Key;
                        StrTagName = CommonBoxOperation.GetTagName(tbk.Key);
                        if (null == StrTagName)Item.SubItems.Add("****");
                        else Item.SubItems.Add(StrTagName);
                        if (null == StrRouterName || "".Equals(StrRouterName))Item.SubItems.Add(StrRouterID);
                        else Item.SubItems.Add(StrRouterName);
                        Item.SubItems.Add(tbk.Value.SigStren.ToString());
                        Item.SubItems.Add(tbk.Value.Bat.ToString());
                        Item.SubItems.Add(tbk.Value.ResTime.ToString());
                        Item.SubItems.Add(tbk.Value.ReportTime.ToString());
                        AreaTaglistView.Items.Add(Item);
                    }
                }
            }catch(Exception)
            {
            }
            
        }
        /// <summary>
        /// 刷新地图
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        private void ImageTimer_Tick(Object obj, EventArgs args)
        {
            if (null == MyArea.AreaBitMap || null == MyArea.AreaBitMap.MyBitmap) 
                return;

            ImageBitmap = new Bitmap(MyArea.AreaBitMap.MyBitmap, ImagePanel.Width, ImagePanel.Height);

            //if (scalew < scaleh)
            //{
            //    ImageBitmap = new Bitmap(MyArea.AreaBitMap.MyBitmap, ImagePanel.Width, (int)(ConstInfor.MapHeight * scalew));
            //}
            //else
            //{
            //    ImageBitmap = new Bitmap(MyArea.AreaBitMap.MyBitmap, (int)(scaleh * ImagePanel.Width), ImagePanel.Height);
            //}
            DrawAreaMap.DrawBasicRouter(ImageBitmap, StrAreaID, 1, scalew, scaleh);
            RtAroundTagPlace.ClearAreaAllRouterStand(MyArea);
            IEnumerable<KeyValuePair<string, TagPack>> TEMP =
            CommonCollection.TagPacks.Reverse<KeyValuePair<string, TagPack>>();
            try{
                foreach (KeyValuePair<string, TagPack> tp in TEMP)
                {
                    if (null == tp.Value)
                        continue;
                    //判断Tag是否在当前区域中
                    if (CommonBoxOperation.JudgeTagArea(tp.Key, StrAreaID))
                    {
                        RtAroundTagPlace.Num = CommonBoxOperation.GetRouterAroundNum(tp.Value.RD_New[0].ToString("X2") + tp.Value.RD_New[1].ToString("X2"));
                        RtAroundTagPlace.CurImageTag = tp.Value;
                        if (ImageBitmap == null) continue;
                        RtAroundTagPlace.DrawTag3_Place(ImageBitmap,scalew, scaleh);
                    }
                }
            }catch(Exception)
            {
            }
            ImagePanel_Paint(null,null);
        }
        private void ImagePanel_Paint(object sender, PaintEventArgs e)
        {
            if (null != ImageBitmap)ImagePanel.CreateGraphics().DrawImageUnscaled(ImageBitmap, 0, 0);
            else ImagePanel.CreateGraphics().Clear(Color.White);
        }
        private void RegInfoWin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(ImageTimer!=null)ImageTimer.Stop();
            // 图形模式
            if (ListTimer != null)ListTimer.Stop();
            //清除各Tag上次的位置
            if (null == MyArea)return;
            RtAroundTagPlace.ClearAreaAllRouterStand(MyArea);
            RtAroundTagPlace.ClearAreaAllTagOldRouter(MyArea);
        }
        private void ImagePanel_Click(object sender, EventArgs e)
        {
            int Cur_X = ((MouseEventArgs)e).X;
            int Cur_Y = ((MouseEventArgs)e).Y;
            if (null == MyArea)
                return;
            foreach (KeyValuePair<string, BasicRouter> br in MyArea.AreaRouter)
            {
                if (null == br.Value)
                    continue;
                if (Cur_X > br.Value.x * scalew - ConstInfor.RouterWidth / 2 && Cur_X < br.Value.x * scalew + ConstInfor.RouterWidth / 2 && Cur_Y > br.Value.y * scaleh - ConstInfor.RouterHeight / 2 && Cur_Y < br.Value.y * scaleh + ConstInfor.RouterHeight / 2)
                {
                    ShowTag MyShowTag = new ShowTag(br.Value);
                    MyShowTag.ShowDialog();
                    return;
                }
            }
        }
        private int CurSortingCol = 0;
        private void AreaTaglistView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != CurSortingCol)
            {//需要给当前的列表重新排序
                AreaTaglistView.Sorting = SortOrder.Descending;
            }
            else
            {
                if (AreaTaglistView.Sorting == SortOrder.Descending)AreaTaglistView.Sorting = SortOrder.Ascending;
                else AreaTaglistView.Sorting = SortOrder.Descending;
            }
            AreaTaglistView.Sort();
            AreaTaglistView.ListViewItemSorter = new ListViewItemComparer(e.Column, AreaTaglistView.Sorting);
            CancelSortingCol(CurSortingCol);
            SetSortingCol(e.Column);
            CurSortingCol = e.Column;
        }

        private void SetSortingCol(int sortindex)
        {
            switch (sortindex)
            {
                case 0:
                    if (AreaTaglistView.Sorting == SortOrder.Descending)AreaTaglistView.Columns[sortindex].Text = "卡片ID(↑)";
                    else AreaTaglistView.Columns[sortindex].Text = "卡片ID(↓)";
                    break;
                case 1:
                    if (AreaTaglistView.Sorting == SortOrder.Descending)AreaTaglistView.Columns[sortindex].Text = "卡片名稱(↑)";
                    else AreaTaglistView.Columns[sortindex].Text = "卡片名稱(↓)";
                    break;
                case 2:
                    if (AreaTaglistView.Sorting == SortOrder.Descending) AreaTaglistView.Columns[sortindex].Text = "參考點(↑)";
                    else AreaTaglistView.Columns[sortindex].Text = "參考點(↓)";
                    break;
                case 3:
                    if (AreaTaglistView.Sorting == SortOrder.Descending)AreaTaglistView.Columns[sortindex].Text = "信號強度(↑)";
                    else AreaTaglistView.Columns[sortindex].Text = "信號強度(↓)";
                    break;
                case 4:
                    if (AreaTaglistView.Sorting == SortOrder.Descending)
                        AreaTaglistView.Columns[sortindex].Text = "卡片電量%(↑)";
                    else AreaTaglistView.Columns[sortindex].Text = "卡片電量%(↓)";
                    break;
                case 5:
                    if (AreaTaglistView.Sorting == SortOrder.Descending)AreaTaglistView.Columns[sortindex].Text = "未移動時間sec(↑)";
                    else AreaTaglistView.Columns[sortindex].Text = "未移動時間sec(↓)";
                    break;
                case 6:
                    if (AreaTaglistView.Sorting == SortOrder.Descending)AreaTaglistView.Columns[sortindex].Text = "數據上報時間(↑)";
                    else AreaTaglistView.Columns[sortindex].Text = "數據上報時間(↓)";
                    break;
            }
        }

        private void CancelSortingCol(int sortindex)
        {
            switch (sortindex)
            {
                case 0:AreaTaglistView.Columns[sortindex].Text = "卡片ID";break;
                case 1:AreaTaglistView.Columns[sortindex].Text = "卡片名稱";break;
                case 2:AreaTaglistView.Columns[sortindex].Text = "參考點";break;
                case 3:AreaTaglistView.Columns[sortindex].Text = "信號強度";break;
                case 4:AreaTaglistView.Columns[sortindex].Text = "卡片電量%";break;
                case 5:AreaTaglistView.Columns[sortindex].Text = "未移動時間sec";break;
                case 6:AreaTaglistView.Columns[sortindex].Text = "數據上報時間";break;
            }
        }

    }
}
