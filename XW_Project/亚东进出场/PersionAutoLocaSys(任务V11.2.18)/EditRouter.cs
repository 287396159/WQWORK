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
    public partial class EditRouter : Form
    {
        public ParamSet Ps = null;
        public MouseEventArgs ex = null;
        public Area area = null;
        public string StrRouterID = null;
        public int type = -1;
        public EditRouter()
        {
            InitializeComponent();
        }
        public EditRouter(ParamSet Ps, MouseEventArgs ex, Area area, String StrRouterID)
        {
            InitializeComponent();
            this.Ps = Ps;
            this.ex = ex;
            this.area = area;
            this.StrRouterID = StrRouterID;
        }
        public EditRouter(ParamSet Ps,MouseEventArgs ex,Area area,String StrRouterID,int type)
        {
            InitializeComponent();
            this.Ps = Ps;
            this.ex = ex;
            this.area = area;
            this.StrRouterID = StrRouterID;
            this.type = type;
        }
        public EditRouter(ParamSet Ps,MouseEventArgs ex,Area area)
        {
            InitializeComponent();
            this.Ps = Ps;
            this.ex = ex;
            this.area = area;
        }
        private void EditRouter_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new Size(254, 308);
            this.MinimumSize = new Size(254, 308);
            if (null == StrRouterID || "".Equals(StrRouterID))
            {
                RID_1TB.Text = "00";
                RID_2TB.Text = "00";
                NodeTypeCB.SelectedIndex = 0;
            }
            else 
            {
                RID_1TB.Text = StrRouterID.Substring(0,2);
                RID_2TB.Text = StrRouterID.Substring(2,2);
                if (1 == type)
                {
                    NodeTypeCB.SelectedIndex = 1;
                    BasicNode MyBasicNode = null;
                    area.AreaNode.TryGetValue(StrRouterID,out MyBasicNode);
                    if (null != MyBasicNode)
                    {
                        RouterVisibleCB.Checked = MyBasicNode.isVisible;
                        ReferNameTB.Text = MyBasicNode.Name;
                    }
                }
                else
                {
                    NodeTypeCB.SelectedIndex = 0;
                    BasicRouter MyBasicRouter = null;
                    area.AreaRouter.TryGetValue(StrRouterID, out MyBasicRouter);
                    if (null != MyBasicRouter)
                    {
                        RouterVisibleCB.Checked = MyBasicRouter.isVisible;
                        ReferNameTB.Text = MyBasicRouter.Name;
                    }
                }
            }
        }
        /// <summary>
        /// 查看区域中是否包含指定的参考点或节点
        /// </summary>
        /// <param name="strid">参考点或节点的ID</param>
        /// <param name="area">参考点或节点所在的区域</param>
        /// <returns>-1：表示不包含参考点或节点;1：表示包含节点;2：表示包含参考点;</returns>
        private int IsAreaContainNodeOrRefer(string strid,out Area area)
        {
            foreach (KeyValuePair<string, Area> myarea in CommonCollection.Areas)
            {
                if (null == myarea.Value)
                {
                    continue;
                }
                foreach(KeyValuePair<string,BasicNode> bsnode in myarea.Value.AreaNode)
                {
                    if (null == bsnode.Value)
                    {
                        continue;
                    }
                    if (bsnode.Key.Equals(strid))
                    {
                        area = myarea.Value;
                        return 1;
                    }
                }
                foreach(KeyValuePair<string,BasicRouter> bsrtor in myarea.Value.AreaRouter)
                {
                    if (null == bsrtor.Value)
                    {
                        continue;
                    }
                    if (bsrtor.Key.Equals(strid))
                    {
                        area = myarea.Value;
                        return 2;
                    }
                }
            }
            area = null;
            return -1;
        }
        /// <summary>
        /// 设置参考点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetBt_Click(object sender, EventArgs e)
        {
            #region 获取设置的参考点及节点讯息
            string StrID1,StrID2,StrName,StrArea;
            StrID1 = RID_1TB.Text.ToUpper();
            StrID2 = RID_2TB.Text.ToUpper();
            //参考点名称
            StrName = ReferNameTB.Text.ToUpper();
            //判断区域是否存在
            StrArea = Ps.ShowAreaCB.Text;
            //取出区域的ID
            Area are = SysParam.GetArea_IDName(StrArea);
            int index = NodeTypeCB.SelectedIndex;
            #endregion
            #region 檢查ID是否有误
            if ("".Equals(StrID1) || "".Equals(StrID2))
            {
                MessageBox.Show("對不起,ID不能为空！");
                return;
            }
            if (StrID1.Length != 2 || StrID2.Length != 2)
            {
                MessageBox.Show("對不起,ID格式有誤,正常格式為ID1和ID2的長度都為2！");
                return;
            }
            byte[] ID = new byte[2];
            try 
            {
                ID[0] = Convert.ToByte(StrID1,16);
                ID[1] = Convert.ToByte(StrID2,16);
            }catch(Exception)
			{
                MessageBox.Show("對不起,ID格式有误！");
                return;
            }
            if (0 == ID[0] && 0 == ID[1])
            {
                MessageBox.Show("對不起，參考點和數據節點的ID不能為0000！");
                return;
            }
            if(0xFF == ID[0] && 0xFF == ID[1])
            {
                MessageBox.Show("對不起，參考點和數據節點的ID不能為FFFF！");
                return;
            }
            #endregion
            #region 检查添加或修改的参考点或数据节点是否已经存在
            Area marea = null;
            Group mygroup = null;
            string strarea = "",strgrpid = "",strgroup = "";
            int res = IsAreaContainNodeOrRefer(StrID1 + StrID2, out marea);
            if (null != marea)
            {
                if ("".Equals(marea.Name) || null == marea.Name)
                {
                    strarea = marea.ID[0].ToString("X2") + marea.ID[1].ToString("X2");
                }
                else
                {
                    strarea = marea.Name;
                }
                strgrpid = marea.GroupID[0].ToString("X2") + marea.GroupID[1].ToString("X2");
            }
            if (CommonCollection.Groups.TryGetValue(strgrpid, out mygroup))
            {//获取到组别讯息
                if ("".Equals(mygroup.Name) || null == mygroup.Name)
                {
                    strgroup = mygroup.ID[0].ToString("X2") + mygroup.ID[1].ToString("X2");
                }
                else
                {
                    strgroup = mygroup.Name;
                }
            }
            #endregion
            if (StrRouterID != null && !"".Equals(StrRouterID))
            {
                #region  修改参考点和数据节点讯息
                if (!StrRouterID.Equals(StrID1 + StrID2))
                {
                    MessageBox.Show("對不起，參考點及數據節點的ID不能修改!");
                    return;
                }
                if (type != index)
                {//说明修改了节点的类型
                    if (index == 1)
                    {//从参考点修改为数据节点
                        if (res == 1)
                        {//包含数据节点
                            MessageBox.Show("在" + strgroup + "組別的" + strarea + "區域中" + "已經存在數據節點" + (StrID1 + StrID2) + "...");
                            return;
                        }
                        else
                        {
                            if (area.AreaRouter.Remove(StrRouterID))
                            {
                                BasicNode Bn = new BasicNode();
                                Bn.ID = ID;
                                Bn.Name = StrName;
                                Bn.isVisible = true;
                                Bn.x = ex.X;
                                Bn.y = ex.Y;
                                Bn.isVisible = RouterVisibleCB.Checked;
                                are.AreaNode.Add(StrID1 + StrID2, Bn);
                                this.Close();
                                return;
                            }
                        }
                    }
                    else
                    {//从数据节点修改为参考点 
                        if (res == 2)
                        {
                            MessageBox.Show("在" + strgroup + "組別的" + strarea + "區域中" + "已經存在參考點" + (StrID1 + StrID2) + "...");
                            return;
                        }
                        else
                        {
                            if (area.AreaNode.Remove(StrRouterID))
                            {
                                BasicRouter BR = new BasicRouter();
                                BR.ID = ID;
                                BR.Name = StrName;
                                BR.isVisible = true;
                                BR.x = ex.X;
                                BR.y = ex.Y;
                                BR.isVisible = RouterVisibleCB.Checked;
                                are.AreaRouter.Add(StrID1 + StrID2, BR);
                                this.Close();
                                return;
                            }
                        }
                    }
                    return;
                }
                //没有修改数据节点类型
                if (index == 1)
                {//设置数据节点
                    BasicNode MyBasicNode = null;
                    area.AreaNode.TryGetValue(StrRouterID, out MyBasicNode);
                    if (null != MyBasicNode)
                    {
                        MyBasicNode.Name = StrName;
                        MyBasicNode.isVisible = RouterVisibleCB.Checked;
                        this.Close();
                        return;
                    }
                }
                else
                {//设置参考点
                    BasicRouter MyBasicRouter = null;
                    area.AreaRouter.TryGetValue(StrRouterID, out MyBasicRouter);
                    if (null != MyBasicRouter)
                    {
                        MyBasicRouter.Name = StrName;
                        MyBasicRouter.isVisible = RouterVisibleCB.Checked;
                        this.Close();
                        return;
                    }
                }
                #endregion
            }
            #region 添加参考点及节点
            if (res == 1)
            {//包含节点
                if (index == 1)
                {//数据节点
                    MessageBox.Show("在" + strgroup + "組別的" + strarea + "區域中" + "已經存在數據節點" + (StrID1 + StrID2)+"...");
                }
                else
                {//参考点 
                    MessageBox.Show("對不起,參考點ID與數據節點ID不能相同，在組別" + strgroup + "的" + strarea + "區域中" + "已經存在數據節點" + (StrID1 + StrID2) + "...");
                }
                return;
            }
            else if (res == 2)
            {//包含参考点
                if (index == 1)
                {//数据节点
                    MessageBox.Show("對不起,參考點ID與數據節點ID不能相同，在組別" + strgroup + "的" + strarea + "區域中" + "已經存在參考點" + (StrID1 + StrID2) + "...");
                }
                else
                {//参考点 
                    MessageBox.Show("對不起,在組別" + strgroup + "的" + strarea + "區域中" + "已經存在參考點" + (StrID1 + StrID2) + "...");
                }
                return;
            }
            else if (res < 0)
            {//添加参考点与数据节点
                if (index == 1)
                {//数据节点
                    BasicNode Bn = new BasicNode();
                    Bn.ID = ID;
                    Bn.Name = StrName;
                    Bn.isVisible = true;
                    Bn.x = ex.X;
                    Bn.y = ex.Y;
                    Bn.isVisible = RouterVisibleCB.Checked;
                    if (are.AreaNode.ContainsKey(StrID1 + StrID2))
                    {
                        MessageBox.Show("Sorry,該區域上已經包含了" + StrID1 + StrID2 + "數據節點!");
                        return;
                    }
                    are.AreaNode.Add(StrID1 + StrID2, Bn);
                }
                else
                {//参考点
                    BasicRouter BR = new BasicRouter();
                    BR.ID = ID;
                    BR.Name = StrName;
                    BR.isVisible = true;
                    BR.x = ex.X;
                    BR.y = ex.Y;
                    BR.isVisible = RouterVisibleCB.Checked;
                    if (are.AreaRouter.ContainsKey(StrID1 + StrID2))
                    {
                        MessageBox.Show("Sorry,該區域上已經包含了" + StrID1 + StrID2 + "參考點!");
                        return;
                    }
                    are.AreaRouter.Add(StrID1 + StrID2, BR);
                }
                SysParam.RestoreShow();
                this.Close();
                return;
            }
            #endregion
        }
        /// <summary>
        /// 删除参考点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleBt_Click(object sender, EventArgs e)
        {
            string StrID1, StrID2;
            StrID1 = RID_1TB.Text;
            StrID2 = RID_2TB.Text;
            int index = NodeTypeCB.SelectedIndex;
            if ("".Equals(StrID1) || "".Equals(StrID2))
            {
                MessageBox.Show("ID不能为空!");
                return;
            }
            if (StrID1.Length != 2 || StrID2.Length != 2)
            {
                MessageBox.Show("ID格式有誤!");
                return;
            }
            byte[] ID = new byte[2];
            try
            {
                ID[0] = Convert.ToByte(StrID1,16);
                ID[1] = Convert.ToByte(StrID2,16);
            }
            catch (Exception)
            {
                MessageBox.Show("ID格式有误！");
                return;
            }
            if (null == area)
            {
                MessageBox.Show("區域不能為空!");
                return;
            }
            if (null == area.AreaRouter)
            {
                MessageBox.Show("參考點不存在！");
                return;
            }
            //判断
            if (null!= StrRouterID && !"".Equals(StrRouterID))
            {
                if (type == 0)
                {//删除参考点
                    if (!area.AreaRouter.ContainsKey(StrID1 + StrID2))
                    {
                        MessageBox.Show("參考點不存在");
                        return;
                    }
                    else
                    {
                        //检查Tag中是否有这个可进的参考点
                        Tag tag = CommonBoxOperation.GetExistRefer(StrID1 + StrID2);
                        if (null != tag)
                        {
                            if (MessageBox.Show("卡片中存在" + StrID1 + StrID2 + "參考點可進，確定要清除這些卡片中的可進參考點嗎?", "警告", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                            {//清除警告讯息
                                #region 当Tag的可进区域中存在删除的参考点时
                                if (null == ID || ID.Length != 2)
                                {
                                    MessageBox.Show("對不起，參考點ID有誤!");
                                    return;
                                }
                               
                                //清除Tag中所有含有指定参考点
                                ClearAllTag_EnRefer(ID);
                                //删除当前的参考点
                                area.AreaRouter.Remove(StrID1 + StrID2);
                                //重新去更新Tag中的Area树状图
                                Ps.AddCheckControl(Ps.ReferTreeView);
                                MessageBox.Show("刪除參考點完成!");
                                this.Close();
                                #endregion
                            }
                            //string taginfor = (null == tag.Name || "".Equals(tag.Name)) ? (tag.ID[0].ToString("X2") + tag.ID[1].ToString("X2")) : tag.Name + "(" + (tag.ID[0].ToString("X2") + tag.ID[1].ToString("X2")) + ")";
                            //MessageBox.Show(taginfor + "卡片中" + StrID1 + StrID2+"參考點可進，請先取消...");
                            return;
                        }
                        area.AreaRouter.Remove(StrID1 + StrID2);
                    }
                }
                else
                {
                    if (!area.AreaNode.ContainsKey(StrID1 + StrID2))
                    {
                        MessageBox.Show("數據節點不存在");
                        return;
                    }
                    else area.AreaNode.Remove(StrID1 + StrID2);
                }
            }
            //删除参考点后重新刷新面板
            SysParam.RestoreShow();
            this.Close();
        }
        //清除所有Tag下的指定Refer 
        private void ClearAllTag_EnRefer(byte[] Rd)
        {
            string strrd = Rd[0].ToString("X2") + Rd[1].ToString("X2");
            List<KeyValuePair<string, Tag>> tags = CommonCollection.Tags.ToList();
            foreach (KeyValuePair<string, Tag> tag in tags)
            {
                if (null == tag.Value)
                {
                    continue;
                }
                for(int i = 0;i<tag.Value.TagReliableList.Count;i++)
                {
                    if (strrd.Equals(tag.Value.TagReliableList[i]))
                    {
                        tag.Value.TagReliableList.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackBt_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 窗体关闭时，重新绘制ParamSet中的面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditRouter_FormClosing(object sender, FormClosingEventArgs e)
        {
            SysParam.RestoreShow();
        }
    }
}
