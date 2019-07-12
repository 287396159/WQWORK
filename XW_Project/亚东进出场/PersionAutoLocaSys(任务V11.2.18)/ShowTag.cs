using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ArrayList = System.Collections.ArrayList;

namespace PersionAutoLocaSys
{
    public partial class ShowTag : Form
    {
        private Timer MyTimer = null;
        private BasicRouter br = null;
        public ShowTag()
        {
            InitializeComponent();
        }
        public ShowTag(BasicRouter br)
        {
            InitializeComponent();
            this.br = br;
            if (null != br)
            {
                if (null != br.Name && !"".Equals(br.Name))
                {
                    this.Text = "當前的參考點：" + br.Name + "(" + br.ID[0].ToString("X2") + br.ID[1].ToString("X2") + ")";
                }
                else
                {
                    this.Text = "當前的參考點：" + br.ID[0].ToString("X2") + br.ID[1].ToString("X2");
                }
            }
        }

        private void ShowTag_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new Size(765, 457);
            this.MinimumSize = new Size(765, 457);
            if (null == MyTimer)
            {
                MyTimer = new Timer();
            }
            MyTimer.Interval = 1000;
            MyTimer.Tick += MyTimer_Tick;
            MyTimer.Start();
        }
        private ArrayList DeleList = null;
        private void MyTimer_Tick(object obj,EventArgs e)
        {
            if (null == br)
            {
                return;
            }
            string StrTagID = "";
            if (null == DeleList)
            {
                DeleList = new ArrayList();
            }
            DeleList.Clear();
            //将当前的Router 周围的参考点刷新到列表中
            foreach (ListViewItem item in RouterTaglistView.Items)
            {
                StrTagID = item.Name;
                TagPack MyTagPack = null;
                if (!CommonCollection.TagPacks.TryGetValue(StrTagID, out MyTagPack))
                {
                    DeleList.Add(StrTagID);
                    continue;
                }
                if (MyTagPack.RD_New[0] != br.ID[0] || MyTagPack.RD_New[1] != br.ID[1])
                {
                    DeleList.Add(StrTagID);
                }
            }
            //清除不应该在列表中的项
            foreach (string str in DeleList)
            {
                RouterTaglistView.Items.RemoveByKey(str);
            }
            ListViewItem listitem = null;
            String StrTagName = null,StrTagIDName = null;
            //lock(CommonCollection.TagPacks_Lock)
            try
            {
                foreach (KeyValuePair<string, TagPack> tp in CommonCollection.TagPacks)
                {
                    if (null == tp.Value)
                        continue;

                    if (tp.Value.RD_New[0] == br.ID[0] && tp.Value.RD_New[1] == br.ID[1])
                    {
                        listitem = null;
                        if (RouterTaglistView.Items.ContainsKey(tp.Key))
                        {
                            StrTagName = null;
                            listitem = null;
                            if (RouterTaglistView.Items.Count > 0)
                            {
                                listitem = RouterTaglistView.FindItemWithText(tp.Key, false, 0);
                            }
                            if (null == listitem)
                            {
                                StrTagName = CommonBoxOperation.GetTagName(tp.Key);
                                if (null != StrTagName)
                                {
                                    listitem = RouterTaglistView.FindItemWithText(tp.Key, false, 0);
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            if (null != listitem)
                            {
                                if (null == StrTagName)
                                {
                                    StrTagName = CommonBoxOperation.GetTagName(tp.Key);
                                }
                                if (null != StrTagName)
                                {
                                    if ("".Equals(StrTagName))
                                    {
                                        StrTagIDName = tp.Key;
                                    }
                                    else
                                    {
                                        StrTagIDName = StrTagName;
                                    }
                                }
                                else
                                {
                                    StrTagIDName = tp.Key;
                                }
                                listitem.Text = StrTagIDName;
                                listitem.SubItems[1].Text = tp.Value.SigStren.ToString();
                                listitem.SubItems[2].Text = tp.Value.Bat.ToString();
                                listitem.SubItems[3].Text = tp.Value.ResTime.ToString() + "秒";
                                listitem.SubItems[4].Text = tp.Value.ReportTime.ToString();
                            }
                            continue;
                        }
                        listitem = new ListViewItem();
                        listitem.Name = tp.Key;
                        //判断卡片是否有名字
                        StrTagName = CommonBoxOperation.GetTagName(tp.Key);
                        if (null != StrTagName)
                        {
                            if ("".Equals(StrTagName))
                            {
                                StrTagIDName = tp.Key;
                            }
                            else
                            {
                                StrTagIDName = StrTagName;
                            }
                        }
                        else
                        {
                            StrTagIDName = tp.Key;
                        }
                        listitem.Text = StrTagIDName;
                        listitem.SubItems.Add(tp.Value.SigStren.ToString());
                        listitem.SubItems.Add(tp.Value.Bat.ToString());
                        listitem.SubItems.Add(tp.Value.ResTime.ToString() + "秒");
                        listitem.SubItems.Add(tp.Value.ReportTime.ToString());
                        RouterTaglistView.Items.Add(listitem);
                    }
                }
            }catch(Exception)
            {
            }
            label1.Text = "Tag的總數量為：" + RouterTaglistView.Items.Count;
        }

        private void ShowTag_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != MyTimer)
            {
                MyTimer.Stop();
                MyTimer = null;
            }
        }
    }
}
