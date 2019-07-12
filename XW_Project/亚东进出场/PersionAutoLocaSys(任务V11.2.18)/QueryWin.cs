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
    public partial class QueryWin : Form
    {
        private AllRegInfoWin MyAllRegInfoWin = null;
        private const string Label = "請輸入卡片的ID或名稱:";
        private BtnStatus CurX_Status = BtnStatus.Bt_start_No_Press;
        private Form1 frm = null;
        private Bitmap BgBitmap = null;
        private Graphics BgG = null;
        public QueryWin()
        {
            InitializeComponent();
        }
        public QueryWin(Form1 frm)
        {
            InitializeComponent();
            this.frm = frm;
        }
        private void QueryWin_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new Size(304, 139);
            this.MinimumSize = new Size(304, 139);
            BgBitmap = new Bitmap(Properties.Resources.srh_bj, 304, 139);
            BgG = Graphics.FromImage(BgBitmap);
            if (null != BgG)
            {
                BgG.DrawString(Label, new Font("宋体", 12), Brushes.Black, ConstInfor.QueryLabel_Left, ConstInfor.QueryLabel_Top);
            }
            QueryWin_Paint(null,null);
        }
        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QueryWin_MouseDown(object sender, MouseEventArgs e)
        {
            SysParam.isQueryDrag = true;
            if (SysParam.oldX <= 0 || SysParam.oldY <=0)
            {
                SysParam.oldX = e.X;
                SysParam.oldY = e.Y;
            }
            if (e.X > ConstInfor.QueryX_Place && e.X < ConstInfor.QueryX_Place + ConstInfor.QueryX_Width && e.Y < ConstInfor.QueryX_Height && e.Y > 0)
            {
                if (CurX_Status == BtnStatus.Bt_start_No_Press)
                {
                    CurX_Status = BtnStatus.Bt_start_Press;
                }
            }
            QueryWin_Paint(null,null);
        }
        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QueryWin_MouseMove(object sender, MouseEventArgs e)
        {
            if (SysParam.isQueryDrag)
                this.Location = new Point(this.Location.X + e.X - SysParam.oldX, this.Location.Y + e.Y - SysParam.oldY);
        }
        /// <summary>
        /// 鼠标释放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QueryWin_MouseUp(object sender, MouseEventArgs e)
        {
            SysParam.isQueryDrag = false;
            SysParam.oldX = -1;
            SysParam.oldY = -1;
            if (CurX_Status == BtnStatus.Bt_start_Press)
            {
                CurX_Status = BtnStatus.Bt_start_No_Press;
                this.Close();
            }
            QueryWin_Paint(null, null);
        }
        private void QueryWin_Paint(object sender, PaintEventArgs e)
        {
            DrawImageX();
            if (null == BgBitmap)
            return;
            this.CreateGraphics().DrawImageUnscaled(BgBitmap, 0, 0);
        }
        private void DrawImageX()
        {
            if (CurX_Status == BtnStatus.Bt_start_Press)
            {
                BgG.DrawImageUnscaled(new Bitmap(Properties.Resources.x_2), ConstInfor.QueryX_Place, 0);
            }
            else if (CurX_Status == BtnStatus.Bt_start_No_Press)
            {
                BgG.DrawImageUnscaled(new Bitmap(Properties.Resources.x_1), ConstInfor.QueryX_Place,0);
            }
        }
        /// <summary>
        /// 查找按钮,用于查找Tag和设备讯息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBtn_Click(object sender, EventArgs e)
        {
            String StrIDorName = TagIDorNameTB.Text.ToUpper();
            if ("".Equals(StrIDorName))
            {
                MessageBox.Show("設備的ID或名稱不能為空!");
                return;
            }
            if (tagrb.Checked)
            {
                #region 查找Tag设备
                PersonOperation curpersonoper = new PersonOperation(frm.CurPerson.ID, OperType.SearchTag);
                CommonCollection.personOpers.Add(curpersonoper);
                TagPack MyTag = null;
                MyTag = SysParam.GetPackTag(StrIDorName);
                if (null == MyTag)
                {
                    MyTag = SysParam.GetPackTag_Name(StrIDorName);
                }
                if (null == MyTag)
                {
                    MessageBox.Show("查詢" + StrIDorName + "的Tag設備不存在!");
                    return;
                }
                Area curarea = CommonBoxOperation.GetAreaFromRouterID(MyTag.RD_New[0].ToString("X2") + MyTag.RD_New[1].ToString("X2"));
                if (null == curarea)
                {
                    MessageBox.Show("對不起，卡片所在的區域不存在!");
                    return;
                }
                try
                {
                    SysParam.isTracking = true;
                    MyAllRegInfoWin = new AllRegInfoWin(frm, SpeceilAlarm.PersonHelp, MyTag.TD[0].ToString("X2") + MyTag.TD[1].ToString("X2"));
                    MyAllRegInfoWin.ShowDialog();
                }
                catch (Exception ex)
                {
                    FileOperation.WriteLog("搜索Tag設備出現異常！異常原因:" + ex.ToString());
                }
                finally
                {
                    this.Close();
                }
                #endregion
            }else if(rfrb.Checked)
            {
                #region 查找Refer设备
                PersonOperation curpersonoper = new PersonOperation(frm.CurPerson.ID, OperType.SearchRefer);
                CommonCollection.personOpers.Add(curpersonoper);
                string strareaid = "";
                BasicRouter mrt = null;
                foreach (KeyValuePair<string, Area> marea in CommonCollection.Areas)
                {
                    if (null == marea.Value)
                    {
                            continue;
                    }
                    marea.Value.AreaRouter.TryGetValue(StrIDorName, out mrt);
                    if (mrt != null)
                    {
                        strareaid = marea.Key;
                        break;
                    }
                    foreach (KeyValuePair<string, BasicRouter> router in marea.Value.AreaRouter)
                    {
                         if (null == router.Value)
                         {
                            continue;
                         }
                         if (StrIDorName.Equals(router.Value.Name))
                         {
                            mrt = router.Value;
                            break;
                         }
                    }
                    if (null != mrt)
                    {
                       strareaid = marea.Key;
                       break;
                    }
                 }
                if (null == strareaid || "".Equals(strareaid))
                 {
                    MessageBox.Show("查詢" + StrIDorName + @"的參考點設備不存在!");
                    return;
                 }
 
                RegInfoWin myregwin = new RegInfoWin(frm, strareaid);
                myregwin.ShowDialog();
                this.Close();
                #endregion
            }else if(ndrb.Checked)
            {
                #region 查找Node设备
                PersonOperation curpersonoper = new PersonOperation(frm.CurPerson.ID, OperType.SearchNode);
                CommonCollection.personOpers.Add(curpersonoper);
                string strareaid = "";
                BasicNode mnd = null;
                foreach (KeyValuePair<string, Area> marea in CommonCollection.Areas)
                {
                    if (null == marea.Value)
                    {
                            continue;
                    }
                    marea.Value.AreaNode.TryGetValue(StrIDorName, out mnd);
                    if (mnd!=null)
                    {
                        strareaid = marea.Key;
                        break;
                    }
                    foreach(KeyValuePair<string,BasicNode> node in marea.Value.AreaNode)
                    {
                        if (null == node.Value)
                        {
                             continue;
                        }
                        if (StrIDorName.Equals(node.Value.Name))
                        {
                             mnd = node.Value;
                             break;
                        }
                    }
                    if (null != mnd)
                    {
                        strareaid = marea.Key;
                        break;
                    }
                 }
                if (null == strareaid || "".Equals(strareaid))
                 {
                     MessageBox.Show("查詢" + StrIDorName + @"的數據節點設備不存在!");
                    return;
                 }
 
                RegInfoWin myregwin = new RegInfoWin(frm, strareaid);
                myregwin.ShowDialog();
                this.Close();
                #endregion
            }
        }
    }
}
