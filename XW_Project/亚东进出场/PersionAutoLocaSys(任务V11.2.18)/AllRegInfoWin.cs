using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading;
using Timer = System.Windows.Forms.Timer;
using PersionAutoLocaSys.Bean;
using SerialportSample;

namespace PersionAutoLocaSys
{
    public partial class AllRegInfoWin : Form
    {
        private Bitmap PanelBitMap = null;
       // private Form1 frm = null;  
        private Timer ListTimer = null;
        private Timer ImageTimer = null;
        private Timer NodeListTimer = null;

        private Form1 frm = null;

        private Area MyArea = null;//图形模式下选择的区域
        private int mode = -1;
        private SpeceilAlarm MySpeceilAlarm = SpeceilAlarm.UnKnown;
        private string StrTagID = "";
        float scalew, scaleh;
        public AllRegInfoWin()
        {
            InitializeComponent();
        }

        public AllRegInfoWin(Form1 frm,SpeceilAlarm CurAlarmType, string StrTagID)
        {
            InitializeComponent();
            this.MySpeceilAlarm = CurAlarmType;
            this.StrTagID = StrTagID;
            this.frm = frm;            
        }

        private void AllRegInfoWin_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new Size(996, 585); 
			this.MinimumSize = new Size(996, 585);
            //生成定时器
            ListTimer = new Timer();
			ListTimer.Interval = 1000;
            ListTimer.Tick += ShowTimer_Tick;

            //默认是列表界面
            ListTimer.Start();
            //启动总地图刷新定时器
            ImageTimer = new Timer();
            ImageTimer.Interval = 1000;
            ImageTimer.Tick += ImageTimer_Tick;
       		//启动节点刷新定时器
            NodeListTimer = new Timer(); 
            NodeListTimer.Interval = 1000;
            NodeListTimer.Tick += NodeListTimer_Tick;
			
            scalew = (float)AllPanel.Width / ConstInfor.MapWidth;
            scaleh = (float)AllPanel.Height / ConstInfor.MapHeight;
            //scale = scalew < scaleh ? scalew : scaleh;
            if (MySpeceilAlarm == SpeceilAlarm.PersonHelp || MySpeceilAlarm == SpeceilAlarm.AreaControl || MySpeceilAlarm == SpeceilAlarm.Resid)
            {
                mode = 1;
                AllRegTabControl.SelectedIndex = 1;
                AllImageSearchTX.Text = StrTagID;
                IsTailCB.Checked = true;
            }
        }

        private void NodeListTimer_Tick(Object obj,EventArgs arg)
        {
            UInt16 nodenum = 0, refernum = 0;
            string  RouterName;
            ListViewItem CurItem = null;
            foreach (KeyValuePair<string, Router> rt in CommonCollection.Routers)
            {
                if (rt.Value.CurType == NodeType.DataNode)
                {
                    nodenum++;
                    if (RoutersLV.Items.ContainsKey(rt.Key))
                    {
                        CurItem = null;
                        if (RoutersLV.Items.Count > 0)
                        {
                            CurItem = RoutersLV.FindItemWithText(rt.Key, false, 0);
                        }
                        if (null == CurItem)
                        {
                            RouterName = CommonBoxOperation.GetNodeName(rt.Key);
                            if (null == RouterName)
                            {
                                continue;
                            }
                            if (RoutersLV.Items.Count > 0 && RoutersLV.Columns.Count > 0)
                            {
                                CurItem = RoutersLV.FindItemWithText(RouterName, false, 0);
                            }
                        }
                        if (null != CurItem)
                        {
                            CurItem.SubItems[2].Text = "數據節點";
                            CurItem.SubItems[3].Text = rt.Value.SleepTime + " s";
                            CurItem.SubItems[4].Text = "V " + ((byte)(rt.Value.Version >> 24)).ToString("D2") + ((byte)(rt.Value.Version >> 16)).ToString("D2") + ((byte)(rt.Value.Version >> 8)).ToString("D2") + ((byte)(rt.Value.Version)).ToString("X2");
                            if (rt.Value.status)
                            {
                                CurItem.SubItems[5].Text = "連接";
                            }
                            else
                            {
                                CurItem.SubItems[5].Text = "斷開";
                            }
                            CurItem.SubItems[6].Text = rt.Value.ReportTime.ToString();
                        }
                    }
                    else
                    {
                        ListViewItem item = new ListViewItem();
                        item.Name = rt.Key;//数据节点键值前面加一个FF作为与参考点的区分
                        item.Text = rt.Key;
                        RouterName = CommonBoxOperation.GetNodeName(rt.Key);
                        if (null == RouterName || "".Equals(RouterName))
                        {
                            item.SubItems.Add("****");
                        }
                        else
                        {
                            item.SubItems.Add(RouterName);
                        }
                        item.SubItems.Add("數據節點");
                        item.SubItems.Add(rt.Value.SleepTime + " s");
                        item.SubItems.Add("V " + ((byte)(rt.Value.Version >> 24)).ToString("D2") + ((byte)(rt.Value.Version >> 16)).ToString("D2") + ((byte)(rt.Value.Version >> 8)).ToString("D2") + ((byte)(rt.Value.Version)).ToString("X2"));
                        if (rt.Value.status)
                        {
                            item.SubItems.Add("連接");
                        }
                        else
                        {
                            item.SubItems.Add("斷開");
                        }
                        item.SubItems.Add(rt.Value.ReportTime.ToString());
                        RoutersLV.Items.Add(item);
                    }
                }
                else if (rt.Value.CurType == NodeType.ReferNode)
                {
                    refernum++;
                    if (RoutersLV.Items.ContainsKey(rt.Key))
                    {
                        CurItem = null;
                        if (RoutersLV.Items.Count > 0)
                        {
                            CurItem = RoutersLV.FindItemWithText(rt.Key, false, 0);
                        }
                        if (null == CurItem)
                        {
                            RouterName = CommonBoxOperation.GetRouterName(rt.Key);
                            if (null == RouterName)
                            {
                                continue;
                            }
                            if (RoutersLV.Items.Count > 0 && RoutersLV.Columns.Count > 0)
                            {
                                CurItem = RoutersLV.FindItemWithText(RouterName, false, 0);
                            }
                        }
                        if (null != CurItem)
                        {
                            CurItem.SubItems[2].Text = "參考點";
                            CurItem.SubItems[3].Text = rt.Value.SleepTime + " s";
                            CurItem.SubItems[4].Text = "V " + ((byte)(rt.Value.Version >> 24)).ToString("D2") + ((byte)(rt.Value.Version >> 16)).ToString("D2") + ((byte)(rt.Value.Version >> 8)).ToString("D2") + ((byte)(rt.Value.Version)).ToString("X2");
                            if (rt.Value.status)
                            {
                                CurItem.SubItems[5].Text = "連接";
                            }
                            else
                            {
                                CurItem.SubItems[5].Text = "斷開";
                            }
                            CurItem.SubItems[6].Text = rt.Value.ReportTime.ToString();
                        }
                    }
                    else
                    {
                        ListViewItem item = new ListViewItem();
                        item.Name = rt.Key;//数据节点键值前面加一个FF作为与参考点的区分
                        item.Text = rt.Key;
                        RouterName = CommonBoxOperation.GetRouterName(rt.Key);
                        if (null == RouterName || "".Equals(RouterName))
                        {
                            item.SubItems.Add("****");
                        }
                        else
                        {
                            item.SubItems.Add(RouterName);
                        }
                        item.SubItems.Add("參考點");
                        item.SubItems.Add(rt.Value.SleepTime + " s");
                        item.SubItems.Add("V " + ((byte)(rt.Value.Version >> 24)).ToString("D2") + ((byte)(rt.Value.Version >> 16)).ToString("D2") + ((byte)(rt.Value.Version >> 8)).ToString("D2") + ((byte)(rt.Value.Version)).ToString("X2"));
                        if (rt.Value.status)
                        {
                            item.SubItems.Add("連接");
                        }
                        else
                        {
                            item.SubItems.Add("斷開");
                        }
                        item.SubItems.Add(rt.Value.ReportTime.ToString());
                        RoutersLV.Items.Add(item);
                    }
                }
                else
                {
                    if (RoutersLV.Items.ContainsKey(rt.Key))
                    {
                        CurItem = null;
                        if (RoutersLV.Items.Count > 0)
                        {
                            CurItem = RoutersLV.FindItemWithText(rt.Key, false, 0);
                        }
                        if (null != CurItem)
                        {
                            CurItem.SubItems[2].Text = "未知類型";
                            CurItem.SubItems[3].Text = rt.Value.SleepTime + " s";
                            CurItem.SubItems[4].Text = "V " + ((byte)(rt.Value.Version >> 24)).ToString("D2") + ((byte)(rt.Value.Version >> 16)).ToString("D2") + ((byte)(rt.Value.Version >> 8)).ToString("D2") + ((byte)(rt.Value.Version)).ToString("X2");
                            if (rt.Value.status)
                            {
                                CurItem.SubItems[5].Text = "連接";
                            }
                            else
                            {
                                CurItem.SubItems[5].Text = "斷開";
                            }
                            CurItem.SubItems[6].Text = rt.Value.ReportTime.ToString();
                        }
                    }
                    else
                    {
                        ListViewItem item = new ListViewItem();
                        item.Name = rt.Key;//数据节点键值前面加一个FF作为与参考点的区分
                        item.Text = rt.Key;
                        item.SubItems.Add("****");
                        item.SubItems.Add("未知類型");
                        item.SubItems.Add(rt.Value.SleepTime + " s");
                        item.SubItems.Add("V " + ((byte)(rt.Value.Version >> 24)).ToString("D2") + ((byte)(rt.Value.Version >> 16)).ToString("D2") + ((byte)(rt.Value.Version >> 8)).ToString("D2") + ((byte)(rt.Value.Version)).ToString("X2"));
                        if (rt.Value.status)
                        {
                            item.SubItems.Add("連接");
                        }
                        else
                        {
                            item.SubItems.Add("斷開");
                        }
                        item.SubItems.Add(rt.Value.ReportTime.ToString());
                        RoutersLV.Items.Add(item);
                    }
                }
            }
            nodenumtxt.Text = nodenum + " 個";
            refernumtxt.Text = refernum + " 個";
            totaltxt.Text = (nodenum + refernum) + " 個";
            TreeViewRefresh();
        }

        private void ShowTimer_Tick(Object obj,EventArgs arg)
        {            
            //将Tag的信息添加到列表中
            String StrRouterID,StrRouterName,StrID,StrTagName;
            Area area = null;
            ListViewItem item = null;
            ArrayList MyArrayList = new ArrayList();
            try
            {
                foreach (ListViewItem im in AllTagListView.Items)
                {
                    StrID = im.Name;
                    if (!CommonCollection.TagPacks.ContainsKey(StrID))
                    MyArrayList.Add(StrID);
                }
                //清除TagPacks中已经不存在的项
                for (int i = 0; i < MyArrayList.Count; i++)
                    AllTagListView.Items.RemoveByKey(MyArrayList[i].ToString());
                //将tbk中没有的项全部清除
                foreach (KeyValuePair<string, TagPack> tbk in CommonCollection.TagPacks)
                {
                    StrRouterID = tbk.Value.RD_New[0].ToString("X2") + tbk.Value.RD_New[1].ToString("X2");
                    StrRouterName = CommonBoxOperation.GetRouterName(StrRouterID);
                    //获取Tag所在区域信息
                    area = CommonBoxOperation.GetRouterArea(StrRouterID);
                    if(AllTagListView.Items.ContainsKey(tbk.Key))
                    {
                       item = null;
                       if(AllTagListView.Items.Count > 0)
                       item = AllTagListView.FindItemWithText(tbk.Key, false, 0);
                       if (null == item)
                       {
                           StrTagName = CommonBoxOperation.GetTagName(tbk.Key);
                           if (null == StrTagName)continue;
                           item = AllTagListView.FindItemWithText(StrTagName, false, 0);
                       }
                       if (null != item)
                       {
                            if(null == area)
                            item.SubItems[2].Text = "无";
                            else
                            {
                                if (null == area.Name || "".Equals(area.Name))
                                {
                                    item.SubItems[2].Text = area.ID[0].ToString("X2") + area.ID[1].ToString("X2");
                                }
                                else
                                {
                                    item.SubItems[2].Text = area.Name + "(" + area.ID[0].ToString("X2") + area.ID[1].ToString("X2") + ")";
                                }
                            }
                            if (null == StrRouterName || "".Equals(StrRouterName))
                            {
                                item.SubItems[3].Text = StrRouterID;
                            }
                            else
                            {
                                item.SubItems[3].Text = StrRouterName;
                            }
                            item.SubItems[4].Text = tbk.Value.SigStren.ToString();
                            item.SubItems[5].Text = tbk.Value.Bat.ToString();
                        
                            item.SubItems[6].Text = tbk.Value.ResTime.ToString();
                            item.SubItems[7].Text = tbk.Value.ReportTime.ToString();
                           // item.SubItems[8].Text = "V " + tbk.Value.Version.ToString("X2");
                       }
                  }
                  else
                  {
                         ListViewItem Item = new ListViewItem();
                         Item.Name = tbk.Key;
                         Item.Text = tbk.Key;
                         String StrName = CommonBoxOperation.GetTagName(tbk.Key);
                         if (null != StrName && !"".Equals(StrName))
                             Item.SubItems.Add(StrName);
                         else
                             Item.SubItems.Add("****");
                         if (null == area)
                            Item.SubItems.Add("无");
                         else
                         {
                            if (null == area.Name || "".Equals(area.Name))
                                Item.SubItems.Add(area.ID[0].ToString("X2") + area.ID[1].ToString("X2"));
                            else
                                Item.SubItems.Add(area.Name + "(" + area.ID[0].ToString("X2") + area.ID[1].ToString("X2") + ")");
                         }
                         if (null == StrRouterName || "".Equals(StrRouterName))
                            Item.SubItems.Add(StrRouterID);
                         else
                            Item.SubItems.Add(StrRouterName);
                         Item.SubItems.Add(tbk.Value.SigStren.ToString());
                         Item.SubItems.Add(tbk.Value.Bat.ToString());
                         Item.SubItems.Add(tbk.Value.ResTime.ToString());
                         //所在区域
                         Item.SubItems.Add(tbk.Value.ReportTime.ToString());
                     
                         AllTagListView.Items.Add(Item);
                    }
                }
                //显示当前的总人数               
            }catch(Exception) {}
            label4.Text = "人員當前總數：" + AllTagListView.Items.Count;
            TreeViewRefresh();
            setWorkTimeTimeOut();
        }

        int count = 0;
        /// <summary>
        /// 设置每次刷新列表的时间
        /// </summary>
        private void setWorkTimeTimeOut() 
        {
            if (!AllRegTabControl.SelectedTab.Name.Equals("tabPage4")) return;
            if (count == 0)
            {
                setAdmissExitList();  
            }
            count++;
            if (count > 30) count = 0;
        }

        /// <summary>
        /// 加载出厂入场数据
        /// </summary>
        private void setAdmissExitList() 
        {
            Dictionary<string, Tag> Tags = new Dictionary<string,PersionAutoLocaSys.Tag>(CommonCollection.Tags);
            Dictionary<string, Tag> tagLists = new Dictionary<string,Tag>();//存储需要展示tag列表

            if (checkBox1.Checked) 
            {
                foreach (var tagItem in Tags)
                {
                    var itemValue = tagItem.Value;
                    if (itemValue.StartWorkDT.CompareTo(XwDataUtils.currentZeroTime()) <= 0) continue;
                    tagLists.Add(tagItem.Key, tagItem.Value);
                }
                admissionLab.Text = tagLists.Count + "個";
                exitLab.Text = "0個";
                onLineLab.Text = "0個";
            }
            else if(checkBox2.Checked)
            {
                foreach (var tagItem in Tags)
                {
                    var itemValue = tagItem.Value;
                    if (itemValue.EndWorkDT.CompareTo(DateTime.Now) >= 0) continue;
                    tagLists.Add(tagItem.Key, tagItem.Value);
                }
                exitLab.Text = tagLists.Count + "個";
                admissionLab.Text = "0個";
                onLineLab.Text = "0個";
            }
            else if (checkBox3.Checked)
            {
                foreach (var tagItem in Tags)
                {
                    var itemValue = tagItem.Value;
                    if (itemValue.EndWorkDT.CompareTo(DateTime.Now) < 0) continue;
                    tagLists.Add(tagItem.Key, tagItem.Value);
                }
                onLineLab.Text = tagLists.Count + "個";
                exitLab.Text = "0個";
                admissionLab.Text = "0個";
            //    setCardWorkAndUnworkTime(true);
            }
            else 
            {
                admissionLab.Text = "0個";
                exitLab.Text = "0個";
                onLineLab.Text = "0個";
                tagLists = Tags;
                //setCardWorkAndUnworkTime(false);
            }
            setCardWorkAndUnworkTime(tagLists);
        }

        /// <summary>
        /// 设置历史出厂入场数据给列表
        /// </summary>
        private void setHisAdmiExitList() 
        {
            List<AdmissionExit> admissionExits = CommonCollection.admissionExits.ToList();
            daTagListView.Items.Clear();
            int length = admissionExits.Count;
            for (int i = 0; i < length;i++ )
            {
                if (!isData(admissionExits[i])) continue;
                ListViewItem item = new ListViewItem();
                item.Text = admissionExits[i].TagID;
                item.Name = admissionExits[i].TagID;
                item.SubItems.Add(admissionExits[i].Name);
                if (admissionExits[i].Model == AdmissionExit.ADMISSION)
                {
                    item.SubItems.Add(XwDataUtils.dataFromTimeStamp((Int32)admissionExits[i].Time));
                    item.SubItems.Add("");
                }
                else 
                {
                    item.SubItems.Add("");
                    item.SubItems.Add(XwDataUtils.dataFromTimeStamp((Int32)admissionExits[i].Time));                   
                }                
                String workIDs = "";
                for (int j = 0; j < 16; j++)
                {
                    workIDs += admissionExits[i].WorkIDbyte[j].ToString("X2");
                }
                item.SubItems.Add(workIDs);
                daTagListView.Items.Add(item);
            }
        }

        /// <summary>
        /// 判断给入的参数是否是界面相关选中时间
        /// 
        /// </summary>
        /// <param name="admissExit"></param>
        /// <returns></returns>
        private bool isData(AdmissionExit admissExit) 
        {
            if (checkBox1.Checked)
            {
                return admissExit.Model.Equals(AdmissionExit.ADMISSION);
            }
            else if (checkBox2.Checked)
            {
                return admissExit.Model.Equals(AdmissionExit.EXIT);
            }
            else if (checkBox3.Checked)
            {
                return false;
            }
            else
            {
                return false;
            }
        }

        private void setCardWorkAndUnworkTime(bool workNow) 
        {
            //foreach (PhonePerson psn in CommonCollection.PhonePersons)
            //{
            //    StrID = psn.ID.ToString().PadLeft(2, '0');
            //    FileOperation.SetValue(FileOperation.PersonMsgPath, StrID, FileOperation.PersonName, psn.Name);
            //    FileOperation.SetValue(FileOperation.PersonMsgPath, StrID, FileOperation.PersonPhone, psn.PhoneNumber);
            //}
            daTagListView.Items.Clear();
            ListViewItem item = null;
            foreach (KeyValuePair<string, Tag> tag in CommonCollection.Tags)
            {
                if (null == tag.Value)
                    continue;
                if (workNow && tag.Value.EndWorkDT.CompareTo(DateTime.Now) < 0)
                    continue;
                item = new ListViewItem();
                item.Text = tag.Key;
                item.Name = tag.Key;
                item.SubItems.Add(tag.Value.Name);
                item.SubItems.Add(currentTimeToSe(tag.Value.StartWorkDT));
                item.SubItems.Add(getEndWorkStr(tag.Value.EndWorkDT));

                String workIDs = "";
                for (int i = 0; i < 16; i++)
                {
                    workIDs += tag.Value.workID[i].ToString("X2");
                }
                item.SubItems.Add(workIDs);
                daTagListView.Items.Add(item);
            }
        }

        /// <summary>
        /// 设置出入场的列表
        /// </summary>
        /// <param name="Tags"></param>
        private void setCardWorkAndUnworkTime(Dictionary<string, Tag> Tags)
        {
            daTagListView.Items.Clear();
            ListViewItem item = null;
            foreach (KeyValuePair<string, Tag> tag in Tags)
            {
                if (null == tag.Value)
                    continue;
                item = new ListViewItem();
                item.Text = tag.Key;
                item.Name = tag.Key;
                item.SubItems.Add(tag.Value.Name);
                item.SubItems.Add(currentTimeToSe(tag.Value.StartWorkDT));
                item.SubItems.Add(getEndWorkStr(tag.Value.EndWorkDT));

                String workIDs = "";
                for (int i = 0; i < 16; i++)
                {
                    workIDs += tag.Value.workID[i].ToString("X2");
                }
                item.SubItems.Add(workIDs);
                daTagListView.Items.Add(item);
            }
        }


        private String getEndWorkStr(DateTime dt)
        {
            if (dt.CompareTo(DrawIMG.maxDt) == 0) return "";
            return currentTimeToSe(dt);
        }

        public  string currentTimeToSe(DateTime dt)
        {
            return dt.ToString("yyyy年MM月dd HH:mm:ss");
        }


        private void TreeViewRefresh()
        {
            List<KeyValuePair<string, Router>> nodes = CommonCollection.Routers.OrderBy(c => c.Key).ToList();
            //先添加节点
            foreach (KeyValuePair<string, Router> node in nodes)
            {
                if (null == node.Value)
                {
                    continue;
                }
                if (node.Value.CurType == NodeType.DataNode)
                {
	                TreeNode[] treenodes = NodeTreePanal.Nodes.Find(node.Key,false);
	                if (null != treenodes && treenodes.Length > 0)
	                {
	                    continue;
	                }
                    string str =  CommonBoxOperation.GetNodeName(node.Key);
                    StringBuilder strbuilder = new StringBuilder();
                    if (null == str || "".Equals(str))
                    {
                        strbuilder.Append(node.Key);
                    }
                    else
                    {
                        strbuilder.Append(str);
                        strbuilder.Append("(");
                        strbuilder.Append(node.Key);
                        strbuilder.Append(")");
                    }
                    TreeNode treenode = new TreeNode(strbuilder.ToString());
                    treenode.Name = node.Key;
                    treenode.ForeColor = Color.Red;
                    NodeTreePanal.Nodes.Add(treenode);
                }
            }

            foreach(KeyValuePair<string,Router> node in nodes)
            {
                if (null == node.Value)
                {
                    continue;
                }
                if (node.Value.CurType != NodeType.ReferNode)
                {
                    continue;
                }
                TreeNode[] treenodes = NodeTreePanal.Nodes.Find(node.Key, true);
                //真实父节点
                string strkey = node.Value.BasicID[0].ToString("X2") + node.Value.BasicID[1].ToString("X2");
                if (null != treenodes && treenodes.Length > 0)
                {//说明当前搜到了这个节点
                    TreeNode parnode = treenodes[0].Parent;
                    if (null != parnode)
                    {//父节点不为空，判断父节点是否发生变化
                        //原来父节点
                        string lastparent = parnode.Name;
                        if (strkey.Equals(lastparent))
                        {
                            continue;
                        }
                        else
                        {//说明父节点发生变化
                            TreeNode[] nds = NodeTreePanal.Nodes.Find(strkey, false);
                            parnode.Nodes.Remove(treenodes[0]);
                            if (null != nds && nds.Length > 0)
                            {
                                nds[0].Nodes.Add(treenodes[0]);
                            }
                        }
                    }
                    continue;
                }
                TreeNode[] parentnodes = NodeTreePanal.Nodes.Find(strkey, false);
                if (null == parentnodes || parentnodes.Length <= 0)
                {
                    continue;
                }
                string str = CommonBoxOperation.GetRouterName(node.Key);
                StringBuilder strbuilder = new StringBuilder();
                if (null == str || "".Equals(str))
                {
                    strbuilder.Append(node.Key);
                }
                else
                {
                    strbuilder.Append(str);
                    strbuilder.Append("(");
                    strbuilder.Append(node.Key);
                    strbuilder.Append(")");
                }
                TreeNode childnode = new TreeNode(strbuilder.ToString());
                childnode.Name = node.Key;
                childnode.ForeColor = Color.Blue;
                parentnodes[0].Nodes.Add(childnode);
                
            }
            NodeTreePanal.ExpandAll();
        }
        /// <summary>
        /// 图形模式定时器函数
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="arg"></param>
        private void ImageTimer_Tick(Object obj,EventArgs arg)
        {
            if (IsTailCB.Checked)
            {
                String StrTagID = AllImageSearchTX.Text;
                if (null == StrTagID || "".Equals(StrTagID))
                {
                    IsTailCB.Checked = false;
                    return;
                }
                String StrRouterID = "";
                TagPack MyTagPack = null;
    
                try
                {
                    SysParam.TryGetPackTag(StrTagID, out MyTagPack);
                    if (MyTagPack == null)
                    {
                        IsTailCB.Checked = false;
                        if (mode > 0)
                            this.Close();
                        return;
                    }
                    MyTagPack.RD_Old[0] = 0;
                    MyTagPack.RD_Old[1] = 0;
                    StrRouterID = MyTagPack.RD_New[0].ToString("X2") + MyTagPack.RD_New[1].ToString("X2");
                }catch(Exception)
                {
                }
                //获取Router所在的组和区域
                MyArea = CommonBoxOperation.GetRouterArea(StrRouterID);
                if (null == MyArea)
                {//防止出现没有添加参考点时，一直出现弹出框
                    IsTailCB.Checked = false;
                    if (mode > 0)
                        this.Close();
                    else
                        MessageBox.Show("對不起，Tag所在的區域你還沒有添加!");
                    return;
                }
                string StrGroupIDNAME = "";
                //有这个区域的话，先跳转到组上，再跳转到区域
                if (null == MyArea.GroupID)
                {
                    StrGroupIDNAME = ConstInfor.NoGroup;//没在分组上，跳转到NoGroup上
                }
                else
                {
                    Group MyGroup = null;
                    CommonCollection.Groups.TryGetValue(MyArea.GroupID[0].ToString("X2") + MyArea.GroupID[1].ToString("X2"), out MyGroup);
                    if (null != MyGroup)
                    {
                        if (!"".Equals(MyGroup.Name))
                        {
                            StrGroupIDNAME = MyGroup.Name + "(" + MyGroup.ID[0].ToString("X2") + MyGroup.ID[1].ToString("X2") + ")";
                        }
                        else
                        {
                            StrGroupIDNAME = MyGroup.ID[0].ToString("X2") + MyGroup.ID[1].ToString("X2");
                        }
                    }
                }
                GroupComboBox.SelectedItem = StrGroupIDNAME;
                //有这个区域的话跳转到这个区域
                string StrAreaIDName = "";
                if (null == MyArea.Name || "".Equals(MyArea.Name))
                {
                    StrAreaIDName = MyArea.ID[0].ToString("X2") + MyArea.ID[1].ToString("X2");
                }
                else
                {
                    StrAreaIDName = MyArea.Name + "(" + MyArea.ID[0].ToString("X2") + MyArea.ID[1].ToString("X2") + ")";
                }
                for (int i = 0; i < AreaSelectCB.Items.Count; i++)
                {
                    if (StrAreaIDName.Equals(AreaSelectCB.Items[i].ToString()))
                    {
                        AreaSelectCB.SelectedIndex = i;
                    }
                }
            }
            //地图或区域不存在是返回
            if (null == MyArea || null == MyArea.AreaBitMap || null == MyArea.AreaBitMap.MyBitmap)
            {
                return;
            }
            //取得地图的位图
            String StrAreaID = MyArea.ID[0].ToString("X2") + MyArea.ID[1].ToString("X2");
            PanelBitMap = new Bitmap(MyArea.AreaBitMap.MyBitmap,AllPanel.Width, AllPanel.Height);
            //在地图上画出参考点
            DrawAreaMap.DrawBasicRouter(PanelBitMap, StrAreaID, 1,scalew,scaleh);
            //清除掉区域中所有参考点周围位置的占用情况
            RtAroundTagPlace.ClearAreaAllRouterStand(MyArea);
            IEnumerable<KeyValuePair<string, TagPack>> TEMP = 
            CommonCollection.TagPacks.Reverse<KeyValuePair<string, TagPack>>();
            try
            {
                foreach (KeyValuePair<string, TagPack> tp in TEMP)
                {
                    if (CommonBoxOperation.JudgeTagArea(tp.Key, StrAreaID))
                    {
                        //Tag在指定区域上
                        if (tp.Value.isVisble)
                        {
                            RtAroundTagPlace.Num = CommonBoxOperation.GetRouterAroundNum(tp.Value.RD_New[0].ToString("X2") + tp.Value.RD_New[1].ToString("X2"));
                            RtAroundTagPlace.CurImageTag = tp.Value;
                            //画出Tag
                            RtAroundTagPlace.DrawTag3_Place(PanelBitMap,scalew, scaleh);
                        }
                    }
                }
            }catch(Exception)
            {
                
            }
            TreeViewRefresh();
            AllPanel_Paint(null,null);
        }
        /// <summary>
        /// 区域发生改变时，切换不同的地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AreaSelectCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            String StrAreaIDName = AreaSelectCB.Text;
            //获取区域信息
            MyArea = SysParam.GetArea_IDName(StrAreaIDName);
            if (null == MyArea)
            {
                PanelBitMap = null;
                AllPanel_Paint(null,null);
                return;
            }
            String StrAreaID = MyArea.ID[0].ToString("X2") + MyArea.ID[1].ToString("X2");
            PanelBitMap = null;
            if (null != MyArea.AreaBitMap)
            {
                if (null != MyArea.AreaBitMap.MyBitmap)
                {
                    PanelBitMap = new Bitmap(MyArea.AreaBitMap.MyBitmap,ConstInfor.MapWidth,ConstInfor.MapHeight);
                    //画出参考点信息
                    DrawAreaMap.DrawBasicRouter(PanelBitMap, StrAreaID,1);
                }
            }
            if (null != PanelBitMap)
            {
                PanelBitMap = new Bitmap(MyArea.AreaBitMap.MyBitmap, AllPanel.Width, AllPanel.Height);
            }
            AllPanel_Paint(null,null);
        }
        /// <summary>
        /// 人员总数统计中的画图面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllPanel_Paint(object sender, PaintEventArgs e)
        {
            if (null != PanelBitMap)
            {
                AllPanel.CreateGraphics().DrawImageUnscaled(PanelBitMap, 0, 0);
            }
            else
            {
                AllPanel.CreateGraphics().Clear(Color.White);
            }
        }
        private void AllRegTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (AllRegTabControl.SelectedIndex)
            {
                case 0:
                    // 列表模式
                    if (ImageTimer != null)
                        ImageTimer.Stop();
                    if(NodeListTimer != null)
                        NodeListTimer.Stop();
                    ListTimer.Start();
                    break;
                case 1:
                    String StrAreaIDName = AreaSelectCB.Text;
                    InitGroupComboBox();
                    //获取区域信息
                    MyArea = SysParam.GetArea_IDName(StrAreaIDName);
                    //对区域进行
                    if (ListTimer != null)
                    {
                        ListTimer.Stop();
                    }
                    if (NodeListTimer != null)
                    {
                        NodeListTimer.Stop();
                    }
                    ImageTimer.Start();
                    if (RtAroundTagPlace.mode == 1)
                    {
                        CommonBoxOperation.SetTagAllVisible();
                        RtAroundTagPlace.mode = 0;
                    }
                    AreaSelectCB_SelectedIndexChanged(null, null);
                    IsTailCB_CheckedChanged(null,null);
                    break;
                case 2:
                    if (ImageTimer != null)
                    {
                        ImageTimer.Stop();
                    }
                    if (ListTimer != null)
                    {
                        ListTimer.Stop();
                    }
                    NodeListTimer.Start();
                    break;
            }
            foreach (KeyValuePair<string, TagPack> tpk in CommonCollection.TagPacks)
            { 
                tpk.Value.RD_Old[0] = 0; tpk.Value.RD_Old[1] = 0;
                tpk.Value.CurPlace = ReferAroundPosition.UnKnown;
            }
        }
        /// <summary>
        /// 图形模式人员Tag查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapPsSearchBtn_Click(object sender, EventArgs e)
        {
            //找到人员后，只显示该人员的Tag，其他人员
            String StrTagID = AllImageSearchTX.Text;
            if (null == StrTagID || "".Equals(StrTagID))
            {
                MessageBox.Show("Tag的ID不能为空！");
                return;
            }
            String StrRouterID = "";
            TagPack MyTagPack = null;
            //lock(CommonCollection.TagPacks_Lock)
            try{
                if (!SysParam.TryGetPackTag(StrTagID, out MyTagPack))
                {
                    MyTagPack = SysParam.GetPackTag_Name(StrTagID);
                }
                if (MyTagPack == null)
                {
                    MessageBox.Show("對不起，没有搜索到ID为" + StrTagID+"的卡片!");
                    return;
                }
                StrRouterID = MyTagPack.RD_New[0].ToString("X2") + MyTagPack.RD_New[1].ToString("X2");
            }catch(Exception)
            {
            }
            Area MyArea  = CommonBoxOperation.GetRouterArea(StrRouterID);
            if (null == MyArea)
            {
                MessageBox.Show("對不起，Tag所在的區域你還沒有添加!");
                return;
            }
            string StrGroupIDNAME = "";
            //有这个区域的话，先跳转到组上，再跳转到区域
            if (null == MyArea.GroupID)
                StrGroupIDNAME = ConstInfor.NoGroup;//没在分组上，跳转到NoGroup上
            else
            {
                Group MyGroup = null;
                CommonCollection.Groups.TryGetValue(MyArea.GroupID[0].ToString("X2") + MyArea.GroupID[1].ToString("X2"), out MyGroup);
                if (null != MyGroup)
                {
                    if (!"".Equals(MyGroup.Name))
                        StrGroupIDNAME = MyGroup.Name + "(" + MyGroup.ID[0].ToString("X2") + MyGroup.ID[1].ToString("X2") + ")";
                    else
                        StrGroupIDNAME = MyGroup.ID[0].ToString("X2") + MyGroup.ID[1].ToString("X2");
                }
            }
            GroupComboBox.SelectedItem = StrGroupIDNAME;
            //有这个区域的话跳转到这个区域
            string StrAreaIDName = "";
            if (null == MyArea.Name || "".Equals(MyArea.Name))
                StrAreaIDName = MyArea.ID[0].ToString("X2") + MyArea.ID[1].ToString("X2");
            else
                StrAreaIDName = MyArea.Name + "(" + MyArea.ID[0].ToString("X2") + MyArea.ID[1].ToString("X2") + ")";
            for(int i=0;i<AreaSelectCB.Items.Count;i++)
            {
                if (StrAreaIDName.Equals(AreaSelectCB.Items[i].ToString()))
                    AreaSelectCB.SelectedIndex = i;
            }
            CommonBoxOperation.SetTagVisible(MyTagPack.TD[0].ToString("X2") + MyTagPack.TD[1].ToString("X2"));
            RtAroundTagPlace.mode = 1;
        }
        /// <summary>
        /// 关闭窗口时，关闭所有定时器
        /// 若为跟踪模式，处理所有的特殊报警
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllRegInfoWin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (RtAroundTagPlace.mode == 1)
            {
                CommonBoxOperation.SetTagAllVisible();
                RtAroundTagPlace.mode = 0;
            }
            if (mode > 0)
            {
                if (SysParam.isTracking)
                {
                    SysParam.isTracking = false;
                }
            }
            try
            {
                foreach (KeyValuePair<string, TagPack> tp in CommonCollection.TagPacks)
                {
                    if (null == tp.Value)
                        continue;
                    tp.Value.RD_Old[0] = 0;
                    tp.Value.RD_Old[0] = 0;
                }
            }
            catch (Exception)
            {

            }
            List<Area> areas = CommonCollection.Areas.Values.ToList<Area>();
            foreach(Area area in areas)
            {
                if (null == area)
                {
                    continue;
                }
                if (null == area.AreaRouter)
                {
                    continue;
                }
                foreach (KeyValuePair<string, BasicRouter> br in area.AreaRouter)
                {
                    br.Value.ClearAllPlace();
                }
            }
            //关闭定时器
            if (null != ListTimer)
            {
                ListTimer.Stop();
                ListTimer = null;
            }
            if (null != ImageTimer)
            {
                ImageTimer.Stop();
                ImageTimer = null;
            }
            if (null != NodeListTimer)
            {
                NodeListTimer.Stop();
                NodeListTimer = null;
            }
            this.Dispose();
        }
        private void AllImageSearchTX_TextChanged(object sender, EventArgs e)
        {
            CommonBoxOperation.SetTagAllVisible();
            RtAroundTagPlace.mode = 1;
        }
        private void IsTailCB_CheckedChanged(object sender, EventArgs e)
        {
            if (IsTailCB.Checked)
            {
                //找到人员后，只显示该人员的Tag，其他人员
                String StrTagIDORNAME = AllImageSearchTX.Text;
                if (null == StrTagIDORNAME || "".Equals(StrTagIDORNAME))
                {
                    MessageBox.Show("Tag的ID不能为空！");
                    IsTailCB.Checked = false;
                    return;
                }
                String StrRouterID = "";
                TagPack MyTagPack = null;
                try
				{
                    SysParam.TryGetPackTag(StrTagIDORNAME, out MyTagPack);
                    if (MyTagPack == null)
                    {
                        if (mode > 0)
                            this.Close();
                        else 
                            MessageBox.Show(this, "對不起，没有搜索到ID为" + StrTagIDORNAME + "的卡片!");
                        IsTailCB.Checked = false;
                        return;
                    }
                    StrRouterID = MyTagPack.RD_New[0].ToString("X2") + MyTagPack.RD_New[1].ToString("X2");
                }catch(Exception)
                {
                }
                //判断当前的Tag是否在所添加的区域中
                Area MyArea = CommonBoxOperation.GetRouterArea(StrRouterID);
                if (null != MyArea)
                {
                    AllImageSearchTX.Enabled = false;
                    MapPsSearchBtn.Enabled = false;
                    CommonBoxOperation.SetTagVisible(MyTagPack.TD[0].ToString("X2") + MyTagPack.TD[1].ToString("X2"));
                    RtAroundTagPlace.mode = 1;
                    return;
                }
                MessageBox.Show("對不起，你需要跟踪的Tag不在添加的區域中!");
            }
            else
            {
                AllImageSearchTX.Enabled = true;
                MapPsSearchBtn.Enabled = true;
                CommonBoxOperation.SetTagAllVisible();
                RtAroundTagPlace.mode = 0;
                //清理参考点位置
                String StrTagIDORNAME = AllImageSearchTX.Text;
                if (null == StrTagIDORNAME || "".Equals(StrTagIDORNAME))
                {
                    return;
                }
                TagPack MyTagPack = null;

                {
                    SysParam.TryGetPackTag(StrTagIDORNAME, out MyTagPack);
                }
                if (null == MyTagPack) return;
              
                try{
                    foreach (KeyValuePair<string, TagPack> tp in CommonCollection.TagPacks)
                    {
                        if (null == tp.Value) continue;
                        if (tp.Value.RD_New[0] == MyTagPack.RD_New[0] && tp.Value.RD_New[1] == MyTagPack.RD_New[1])
                        {
                            tp.Value.RD_Old[0] = 0; 
							tp.Value.RD_Old[1] = 0;
                        }
                    }
                }catch(Exception)
                {
                
                }
            }
        }
        /// <summary>
        /// 列表模式下的搜索按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListModeSearchBt_Click(object sender, EventArgs e)
        {
            string StrSearchTB = ListModeSearchTX.Text;
            if ("".Equals(StrSearchTB) || null == StrSearchTB)
            {
                MessageBox.Show("搜索时搜索框中的内容不能为空!");
                return;
            }
            SysParam.SearchListViewStr(AllTagListView, StrSearchTB);
        }
        private void ListModeSearchTX_TextChanged(object sender, EventArgs e)
        {
            SysParam.ClearListViewWhiteItem(AllTagListView);
        }
        /// <summary>
        /// 初始化组选择框中的项
        /// </summary>
        public  void InitGroupComboBox()
        {
            GroupComboBox.Items.Clear();
            List<KeyValuePair<string, Group>> groups = null;
            lock (CommonCollection.Groups_Lock)
            {
                groups = CommonCollection.Groups.OrderBy(c => c.Key).ToList();
            }
            foreach (KeyValuePair<string, Group> MyGroup in groups)
            {
                SysParam.AddNameID(MyGroup.Key, MyGroup.Value.Name, GroupComboBox);
            }
            GroupComboBox.Items.Add(ConstInfor.NoGroup);
            GroupComboBox.SelectedIndex = 0;
        }
        /// <summary>
        /// 选择对应的楼层时，向区域中加载楼层对应的区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            String StrGroupIDName = GroupComboBox.Text;
            Group CurGroup = SysParam.GetGroup_IDName(StrGroupIDName);
            AreaSelectCB.Items.Clear();
            if (null == CurGroup)
            {
                AreaSelectCB.Items.Add(ConstInfor.NoArea);
                AreaSelectCB.SelectedIndex = (AreaSelectCB.Items.Count - 1);
                return;
            }
            List<KeyValuePair<string, Area>> areas = CommonCollection.Areas.OrderBy(c => c.Key).ToList();
            foreach (KeyValuePair<string, Area> MyArea in areas)
            {
                if (null == MyArea.Value.GroupID)
                {
                    continue;
                }
                if (MyArea.Value.GroupID[0] == CurGroup.ID[0] && MyArea.Value.GroupID[1] == CurGroup.ID[1])
                {
                    SysParam.AddNameID(MyArea.Key, MyArea.Value.Name, AreaSelectCB);
                }
            }
            AreaSelectCB.Items.Add(ConstInfor.NoArea);
            AreaSelectCB.SelectedIndex = 0;
		}
        private bool ReadNodeInfor(Router br)
        {
            /*查询Node设备类型及固件版本号
             * PC --> Node：F1 + 11 + ID(2byte) + CS + F2
             * Node --> PC：FA + 11 + ID(2byte) + type + Version + CS + FB
            */
            byte[] bytes = new byte[6];
            bytes[0] = 0xF1; bytes[1] = 0x11;
            System.Buffer.BlockCopy(br.ID, 0, bytes, 2, 2);
            bytes[bytes.Length - 2] = 0;
            for (int i = 0; i < bytes.Length - 2; i++)
            {
                bytes[bytes.Length - 2] += bytes[i];
            }
            bytes[bytes.Length - 1] = 0xF2;
            SysParam.scandevtype = false;
            SysParam.mnodemsg = null;
            SysParam.sendcount = 3;
            try
            {
	            while (true)
	            {
	                SysParam.sendcount--;
	                SysParam.tickcount = Environment.TickCount;
	                try
	                {
	                    frm.MyUdpClient.Send(bytes, bytes.Length, br.mendpoint);
	                }
	                catch (Exception ex)
	                {
	                    FileOperation.WriteLog("发送查询设备类型及版本发生异常!异常原因：" + ex.ToString());
	                }
	                while (Environment.TickCount - SysParam.tickcount < ConstInfor.recnwtimeout + 300 && !SysParam.scandevtype)
	                {
	                    Thread.Sleep(1);
	                }
	                if (SysParam.sendcount <= 0 || SysParam.scandevtype)
	                {
	                    break;
	                }
	            }
            }catch(Exception ex)
            {
                FileOperation.WriteLog("读取数据节点讯息时出现异常!异常原因："+ex.ToString());
            }
            if (!SysParam.scandevtype || null == SysParam.mnodemsg)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 读取设备讯息
        /// </summary>
        /// <returns></returns>
        private bool ReadReferInfor(Router br)
        {
            //查询当前的连接是否正常
            /*
             * 查询Refer设备类型及固件版本号
             * PC--->Node：F1 + 24 + ReferID(2byte) + CS +F2
             * Node--->PC:FA + 24 + NodeID(2byte) + ID(2byte) + type(1byte) + Version(4byte) + CS + FB
             * */
            byte[] bytes = new byte[6];
            bytes[0] = 0xF1; bytes[1] = 0x24;
            System.Buffer.BlockCopy(br.ID, 0, bytes, 2, 2);
            bytes[4] = 0;
            for (int i = 0; i < 4; i++)
            {
                bytes[4] += bytes[i];
            }
            bytes[5] = 0xF2;

            SysParam.scandevtype = false;
            SysParam.mnodemsg = null;
            SysParam.sendcount = 3;
            try
            {
	            while (true)
	            {
	                SysParam.sendcount--;
	                SysParam.tickcount = Environment.TickCount;
	                frm.MyUdpClient.Send(bytes, bytes.Length, br.mendpoint);
	                while (Environment.TickCount - SysParam.tickcount < ConstInfor.recrefertimeout + 300 && !SysParam.scandevtype)
	                {
	                    Thread.Sleep(1);
	                }
	                if (SysParam.sendcount <= 0 || SysParam.scandevtype)
	                {
	                    break;
	                }
	            }
            }catch(Exception ex)
            {
                FileOperation.WriteLog("读取参考点设备讯息时出现异常!异常原因:"+ex.ToString());
            }
            if (!SysParam.scandevtype || null == SysParam.mnodemsg)
            {
                return false;
            }
            return true;
        }
        private void AllPanel_Click(object sender, EventArgs e)
        {
            int Cur_X = ((MouseEventArgs)e).X;
            int Cur_Y = ((MouseEventArgs)e).Y;
            if (null == MyArea)
                return;
            Router rter = null;
            foreach (KeyValuePair<string, BasicRouter> br in MyArea.AreaRouter)
            {
                if (null == br.Value)
                    continue;
                if (Cur_X > br.Value.x * scalew - ConstInfor.RouterWidth / 2 && Cur_X < br.Value.x * scalew + ConstInfor.RouterWidth / 2 && Cur_Y > br.Value.y * scaleh - ConstInfor.RouterHeight / 2 && Cur_Y < br.Value.y * scaleh + ConstInfor.RouterHeight / 2)
                {
                    if (SysParam.isDebug && frm.CurPerson.ID[0] == 0xFF && frm.CurPerson.ID[1] == 0xFF)
                    {
                        //查询设备
                        if(CommonCollection.Routers.TryGetValue(br.Key,out rter))
                        {
                            if (rter.CurType != NodeType.ReferNode)
                            {
                                MessageBox.Show("Sorry, the current device is the data node, please see if there is an ID conflict!");
                                return;
                            }
                            if (ReadReferInfor(rter))
                            {
                                ReferParamSetWin MyReferParamSetWin = new ReferParamSetWin(frm, SysParam.mnodemsg);
                                MyReferParamSetWin.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("The connection device failed, please retry again, if many failures may be the network disconnection!");
                            }
                        }else
                        {
                            MessageBox.Show("I'm sorry that the device has not submitted the ID!");
                            return;
                        }
                    }
                    else
                    {
                        ShowTag MyShowTag = new ShowTag(br.Value);
                        MyShowTag.ShowDialog();
                    }
                    return;
                }
            }
            //查看是否点击了数据节点
            foreach (KeyValuePair<string, BasicNode> node in MyArea.AreaNode)
            {
                if (null == node.Value)
                {
                    continue;
                }
                if (Cur_X > node.Value.x * scalew - ConstInfor.DataNodeWidth / 2 && Cur_X < node.Value.x * scalew + ConstInfor.DataNodeWidth / 2 && Cur_Y > node.Value.y * scaleh - ConstInfor.DataNodeHeight / 2 && Cur_Y < node.Value.y * scaleh + ConstInfor.DataNodeHeight / 2)
                {
                    if (SysParam.isDebug && frm.CurPerson.ID[0] == 0xFF && frm.CurPerson.ID[1] == 0xFF)
                    {
                        if (CommonCollection.Routers.TryGetValue(node.Key, out rter))
                        {
                            if (rter.CurType != NodeType.DataNode)
                            {
                                MessageBox.Show("Sorry, the current device is a reference point. Please see if there is an ID conflict!");
                                return;
                            }
                            if (ReadNodeInfor(rter))
                            {
                                if (SysParam.isSettingArroundDevices)
                                {
                                    NodeSettingDevWin MyNodeSettingDevWin = new NodeSettingDevWin(frm, SysParam.mnodemsg);
                                    MyNodeSettingDevWin.ShowDialog();
                                }
                                else
                                {
                                    NodeParamSetWin MyNodeParamSetWin = new NodeParamSetWin(frm, SysParam.mnodemsg);
                                    MyNodeParamSetWin.ShowDialog();
                                }
                            }
                            else
                            {
                                MessageBox.Show("The connection device failed, please retry again, if many failures may be the network disconnection!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("I'm sorry that the device has not submitted the ID!");
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 轨迹分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrackBtn_Click(object sender, EventArgs e)
        {
            if (frm.CurPerson == null)
            {
                if (MessageBox.Show("對不起，你還沒有登錄,不能查看軌跡訊息,請問是否需要登陸?", "提醒", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    EnterPassWin MyEnterPassWin = new EnterPassWin(frm, OperType.ViewTraceAnalysis);
                    if (MyEnterPassWin.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            PersonOperation curpersonoper = new PersonOperation(frm.CurPerson.ID, OperType.ViewTraceAnalysis);
            CommonCollection.personOpers.Add(curpersonoper);
            TrackWin MyTrackWin = new TrackWin();
            MyTrackWin.Show();
            
        }
        /// <summary>
        /// 统计分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CountBtn_Click(object sender, EventArgs e)
        {
            StatisticAnalysis MyStatisticAnalysis = new StatisticAnalysis();
            MyStatisticAnalysis.Show();
        }
        /// <summary>
        /// 报警讯息查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WarmMsgBtn_Click(object sender, EventArgs e)
        {
            if (frm.CurPerson == null)
            {
                if (MessageBox.Show("對不起,你還沒有登錄,不能查看警報訊息,請問是否需要登陸?", "提醒", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    EnterPassWin MyEnterPassWin = new EnterPassWin(frm, OperType.ViewAlarmRecord);
                    if (MyEnterPassWin.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            PersonOperation curpersonoper = new PersonOperation(frm.CurPerson.ID, OperType.ViewAlarmRecord);
            CommonCollection.personOpers.Add(curpersonoper);

            WarmMsg MyWarmMsg = new WarmMsg();
            MyWarmMsg.Show();
        }
        int sortColumn = 0;
        private void AllTagListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != sortColumn)AllTagListView.Sorting = SortOrder.Ascending;
            else
            {
                if (AllTagListView.Sorting == SortOrder.Ascending)AllTagListView.Sorting = SortOrder.Descending;
                else AllTagListView.Sorting = SortOrder.Ascending;
            }
            AllTagListView.Sort();
            this.AllTagListView.ListViewItemSorter = new ListViewItemComparer(e.Column, AllTagListView.Sorting);
            CancelSortColumn(sortColumn,0);
            SetSortingColumn(e.Column,0);
            sortColumn = e.Column;
        }
        private void SetSortingColumn(int sortColumn,int type)
        {
            if (type == 0)
            {
                switch (sortColumn)
                {
                    case 0:
                        if (AllTagListView.Sorting == SortOrder.Descending) this.AllTagListView.Columns[sortColumn].Text = "卡片ID(↑)";
                        else this.AllTagListView.Columns[sortColumn].Text = "卡片ID(↓)";
                        break;
                    case 1:
                        if (AllTagListView.Sorting == SortOrder.Descending) this.AllTagListView.Columns[sortColumn].Text = "卡片名稱(↑)";
                        else this.AllTagListView.Columns[sortColumn].Text = "卡片名稱(↓)";
                        break;
                    case 2:
                        if (AllTagListView.Sorting == SortOrder.Descending) this.AllTagListView.Columns[sortColumn].Text = "所在區域(↑)";
                        else this.AllTagListView.Columns[sortColumn].Text = "所在區域(↓)";
                        break;
                    case 3:
                        if (AllTagListView.Sorting == SortOrder.Descending) this.AllTagListView.Columns[sortColumn].Text = "參考點(↑)";
                        else this.AllTagListView.Columns[sortColumn].Text = "參考點(↓)";
                        break;
                    case 4:
                        if (AllTagListView.Sorting == SortOrder.Descending) this.AllTagListView.Columns[sortColumn].Text = "信號強度(↑)";
                        else this.AllTagListView.Columns[sortColumn].Text = "信號強度(↓)";
                        break;
                    case 5:
                        if (AllTagListView.Sorting == SortOrder.Descending) this.AllTagListView.Columns[sortColumn].Text = "卡片電量%(↑)";
                        else this.AllTagListView.Columns[sortColumn].Text = "卡片電量%(↓)";
                        break;
                    case 6:
                        if (AllTagListView.Sorting == SortOrder.Descending) this.AllTagListView.Columns[sortColumn].Text = "未移動時間sec(↑)";
                        else this.AllTagListView.Columns[sortColumn].Text = "未移動時間sec(↓)";
                        break;
                    case 7:
                        if (AllTagListView.Sorting == SortOrder.Descending) this.AllTagListView.Columns[sortColumn].Text = "數據上報時間(↑)";
                        else this.AllTagListView.Columns[sortColumn].Text = "數據上報時間(↓)";
                        break;
                    case 8:
                        if (AllTagListView.Sorting == SortOrder.Descending) this.AllTagListView.Columns[sortColumn].Text = "版本(↑)";
                        else this.AllTagListView.Columns[sortColumn].Text = "版本(↓)";
                        break;
                }
            }
            else
            {
                switch (sortColumn)
                {
                    case 0:
                        if (RoutersLV.Sorting == SortOrder.Descending) this.RoutersLV.Columns[sortColumn].Text = "節點ID(↑)";
                        else this.RoutersLV.Columns[sortColumn].Text = "節點ID(↓)";
                        break;
                    case 1:
                        if (RoutersLV.Sorting == SortOrder.Descending) this.RoutersLV.Columns[sortColumn].Text = "節點名稱(↑)";
                        else this.RoutersLV.Columns[sortColumn].Text = "節點名稱(↓)";
                        break;
                    case 2:
                        if (RoutersLV.Sorting == SortOrder.Descending) this.RoutersLV.Columns[sortColumn].Text = "節點類型(↑)";
                        else this.RoutersLV.Columns[sortColumn].Text = "節點類型(↓)";
                        break;
                    case 3:
                        if (RoutersLV.Sorting == SortOrder.Descending) this.RoutersLV.Columns[sortColumn].Text = "休眠時間(↑)";
                        else this.RoutersLV.Columns[sortColumn].Text = "休眠時間(↓)";
                        break;
                    case 4:
                        if (RoutersLV.Sorting == SortOrder.Descending) this.RoutersLV.Columns[sortColumn].Text = "版本信息(↑)";
                        else this.RoutersLV.Columns[sortColumn].Text = "版本信息(↓)";
                        break;
                    case 5:
                        if (RoutersLV.Sorting == SortOrder.Descending) this.RoutersLV.Columns[sortColumn].Text = "狀態(↑)";
                        else this.RoutersLV.Columns[sortColumn].Text = "狀態(↓)";
                        break;
                    case 6:
                        if (RoutersLV.Sorting == SortOrder.Descending) this.RoutersLV.Columns[sortColumn].Text = "上報時間(↑)";
                        else this.RoutersLV.Columns[sortColumn].Text = "上報時間(↓)";
                        break;
                }
            }
        }
        private void CancelSortColumn(int sortColumn,int type)
        {
            if (type == 0)
            {
                switch (sortColumn)
                {
                    case 0: this.AllTagListView.Columns[sortColumn].Text = "卡片ID"; break;
                    case 1: this.AllTagListView.Columns[sortColumn].Text = "卡片名稱"; break;
                    case 2: this.AllTagListView.Columns[sortColumn].Text = "所在區域"; break;
                    case 3: this.AllTagListView.Columns[sortColumn].Text = "參考點"; break;
                    case 4: this.AllTagListView.Columns[sortColumn].Text = "信號強度"; break;
                    case 5: this.AllTagListView.Columns[sortColumn].Text = "卡片電量%"; break;
                    case 6: this.AllTagListView.Columns[sortColumn].Text = "未移動時間sec"; break;
                    case 7: this.AllTagListView.Columns[sortColumn].Text = "數據上報時間"; break;
                    case 8: this.AllTagListView.Columns[sortColumn].Text = "版本"; break;
                }
            }
            else
            {
                switch (sortColumn)
                {
                    case 0:this.RoutersLV.Columns[sortColumn].Text = "節點ID";break;
                    case 1:this.RoutersLV.Columns[sortColumn].Text = "節點名稱";break;
                    case 2:this.RoutersLV.Columns[sortColumn].Text = "節點類型";break;
                    case 3:this.RoutersLV.Columns[sortColumn].Text = "休眠時間";break;
                    case 4:this.RoutersLV.Columns[sortColumn].Text = "版本信息";break;
                    case 5:this.RoutersLV.Columns[sortColumn].Text = "狀態";break;
                    case 6:this.RoutersLV.Columns[sortColumn].Text = "上報時間";break;
                }
            }
        }
        private int SortNodeIndex = 0;
        private void RoutersLV_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != SortNodeIndex)
            {
                RoutersLV.Sorting = SortOrder.Ascending;
            }
            else
            {
                if (RoutersLV.Sorting == SortOrder.Ascending)
                {
                    RoutersLV.Sorting = SortOrder.Descending;
                }
                else
                {
                    RoutersLV.Sorting = SortOrder.Ascending;
                }
            }
            RoutersLV.Sort();
            this.RoutersLV.ListViewItemSorter = new ListViewItemComparer(e.Column, RoutersLV.Sorting);
            CancelSortColumn(SortNodeIndex,1);
            SetSortingColumn(e.Column,1);
            SortNodeIndex = e.Column;
        }
        private void selectpersonbtn_Click(object sender, EventArgs e)
        {
            if (frm.CurPerson == null)
            {
                if (MessageBox.Show("對不起,你還沒有登錄,不能查看用戶操作,請問是否需要登陸?", "提醒", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    EnterPassWin myEnterPassWin = new EnterPassWin(frm, OperType.PersonOperation);
                    if (myEnterPassWin.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
                if (frm.CurPerson.PersonAccess != PersonAccess.AdminPerson)
                {
                    MessageBox.Show("對不起，只有管理人員才具有查看人員操作記錄的權限!");
                    return;
                }
                else
                {
                    PersonOperation curpersonoper = new PersonOperation(frm.CurPerson.ID, OperType.PersonOperation);
                    CommonCollection.personOpers.Add(curpersonoper);
                    ViewOperation curViewOperation = new ViewOperation();
                    curViewOperation.ShowDialog();
                }
            
        }
        private void RoutersLV_Click(object sender, EventArgs e)
        {
            if (RoutersLV.SelectedItems.Count <= 0)
            {
                return;
            }
            if (null == frm.CurPerson)
            {
                if (MessageBox.Show("對不起，你還沒有登錄，不能進入設置參數界面，請問是否需要登陸?", "提醒", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    EnterPassWin MyEnterPassWin = new EnterPassWin(frm, OperType.EnterNodeParam);
                    if (DialogResult.OK != MyEnterPassWin.ShowDialog())
                    {
                        MessageBox.Show("對不起,登陸失敗,退出登錄窗體...");
                        return;
                    }
                }
            }
            if (frm.CurPerson.PersonAccess == PersonAccess.SimplePerson)
            {
                MessageBox.Show("對不起，當前用戶不具有設置Node或Refer设备參數的權限!");
                return;
            }
            string strid = RoutersLV.SelectedItems[0].Text;
            if (null == frm.MyUdpClient)
            {
                MessageBox.Show("Sorry, network monitoring has been disconnected!");
                return;
            }
            Router mrouter = null;
            if (!CommonCollection.Routers.TryGetValue(strid, out mrouter))
            {
                return;
            }
            if (mrouter.CurType == NodeType.DataNode)
            {
                #region
                /*每次在打开设置窗体之前先发送一个查询节点类型的封包,主要有两个作用：
                 * 1、由于Node上报的数据包不包含设备类型，进入设置窗体时，并不知道Node的rj45还是wifi
                 * 2、可以用于检测网络连接状态
                */
                /*查询Node设备类型及固件版本号
                 * PC --> Node：F1 + 11 + ID(2byte) + CS + F2
                 * Node --> PC：FA + 11 + ID(2byte) + type + Version + CS + FB
                */
                byte[] bytes = new byte[6];
                bytes[0] = 0xF1; bytes[1] = 0x11;
                System.Buffer.BlockCopy(mrouter.ID,0,bytes,2,2);
                bytes[bytes.Length - 2] = 0;
                for (int i = 0; i < bytes.Length - 2;i++)
                {
                    bytes[bytes.Length - 2] += bytes[i];
                }
                bytes[bytes.Length - 1] = 0xF2;
                SysParam.scandevtype = false;
                SysParam.mnodemsg = null;
                SysParam.sendcount = 3;
                while (true)
                {
                    SysParam.sendcount--;
                    SysParam.tickcount = Environment.TickCount;
                    try
                    {
                        frm.MyUdpClient.Send(bytes, bytes.Length, mrouter.mendpoint);
                    }catch(Exception ex)
                    {
                        FileOperation.WriteLog("发送查询设备类型及版本发生异常!异常原因：" + ex.ToString());
                    }
                    while (Environment.TickCount - SysParam.tickcount < ConstInfor.recnwtimeout + 300 && !SysParam.scandevtype)
                    {
                        Thread.Sleep(1);
                    }
                    if (SysParam.sendcount <= 0 || SysParam.scandevtype)
                    {
                        break;
                    }
                }
                if (!SysParam.scandevtype || null == SysParam.mnodemsg)
                {
                    MessageBox.Show("The connection device failed, please retry again, if many failures may be the network disconnection!");
                    return;
                }
                NodeParamSetWin myndpsetwin = new NodeParamSetWin(frm,SysParam.mnodemsg);
                myndpsetwin.ShowDialog();
                #endregion
            }
            else
            {
                #region
                //此时点击的是设置Refer参数
                /*
                 * 开始进入参数设置界面时要查看当前设备的类型及版本
                 * 1、查看当前设备连接方式
                 * 2、获取设备讯息
                 * */
                /*
                 * 查询Refer设备类型及固件版本号
                 * PC--->Node：F1 + 24 + ReferID(2byte) + CS +F2
                 * Node--->PC:FA + 24 + NodeID(2byte) + ID(2byte) + type(1byte) + Version(4byte) + CS + FB
                 * */
                byte[] bytes = new byte[6];
                bytes[0] = 0xF1; bytes[1] = 0x24;
                System.Buffer.BlockCopy(mrouter.ID,0,bytes,2,2);
                bytes[4] = 0;
                for (int i = 0; i < 4; i++) 
                {
                    bytes[4] += bytes[i];
                }
                bytes[5] = 0xF2;
                SysParam.scandevtype = false;
                SysParam.mnodemsg = null;
                SysParam.sendcount = 3;
                while (true)
                {
                    SysParam.sendcount --;
                    SysParam.tickcount = Environment.TickCount;
                    frm.MyUdpClient.Send(bytes, bytes.Length, mrouter.mendpoint);
                    while (Environment.TickCount - SysParam.tickcount < ConstInfor.recrefertimeout + 300 && !SysParam.scandevtype)
                    {
                        Thread.Sleep(1);
                    }
                    if (SysParam.sendcount <= 0 || SysParam.scandevtype)
                    {
                        break;
                    }
                }
                if (!SysParam.scandevtype || null == SysParam.mnodemsg)
                {
                    MessageBox.Show("The connection device failed, please retry again, if many failures may be the network disconnection!");
                    return;
                }
                ReferParamSetWin myReferParamSetWin = new ReferParamSetWin(frm,SysParam.mnodemsg);
                myReferParamSetWin.ShowDialog();
                #endregion
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) //入场列表
        {
            changeCheckBoxCheck(checkBox1,checkBox2,checkBox3);
            setAdmissExitList();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)//出厂列表
        {
            changeCheckBoxCheck(checkBox2, checkBox1, checkBox3);
            setAdmissExitList();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)// 在场列表
        {
            changeCheckBoxCheck(checkBox3, checkBox2, checkBox1);
            setAdmissExitList();
        }

        private void changeCheckBoxCheck(CheckBox ch1,CheckBox ch2,CheckBox ch3) 
        {
            if (ch1.Checked) 
            {
                if (ch2.Checked) ch2.Checked = false;
                if (ch3.Checked) ch3.Checked = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)  //進出場歷史記錄
        {
            // 
            AdmissionExitHistoryForm aForm = new AdmissionExitHistoryForm();
            aForm.Show();
        }

    }
    public class BuffListView : ListView
    {
        public BuffListView()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
    }
    class ListViewItemComparer : IComparer
    {
        private int col;
        private SortOrder order;
        public ListViewItemComparer()
        {
            col = 0;
            order = SortOrder.Ascending;
        }
        public ListViewItemComparer(int column, SortOrder order)
        {
            col = column;
            this.order = order;
        }
        public int Compare(object x, object y)
        {
            int returnVal = -1;
            float a = 0, b = 0;
            if (float.TryParse(((ListViewItem)x).SubItems[col].Text, out a) && float.TryParse(((ListViewItem)y).SubItems[col].Text, out b))
            {
                returnVal = a >= b ? (a == b ? 0 : 1) : -1;
                if (order == SortOrder.Descending)
                {
                    returnVal *= -1;
                }
            }
            else
            {
                returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text,
                                        ((ListViewItem)y).SubItems[col].Text);
                if (order == SortOrder.Descending)
                {
                    returnVal *= -1;
                }
            }
            return returnVal;
        }
    }
}
