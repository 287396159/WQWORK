using CiXinLocation.bean;
using CiXinLocation.CiXinInterface;
using CiXinLocation.Model;
using Common;
using MoveableListLib;
using MoveableListLib.Bean;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CiXinLocation.control
{
    class CanShuSetControl
    {
        private CanShuViewInterface mCSVI;
        //private int Jurisdiction = 1;

        public CanShuSetControl(CanShuViewInterface mCSVI) 
        {
            this.mCSVI = mCSVI;
        }

        /// <summary>
        /// 移除掉TabControl的标签
        /// </summary>
        /// <param name="tabControl"></param>
        /// <param name="tabPage1"></param>
        public void removeTabControlTitle(TabControl tabControl, TabPage tabPage1)
        {
            if (tabControl == null || tabPage1 == null) return;
            tabControl.Region = new Region(new RectangleF(
                tabPage1.Left, tabPage1.Top, tabPage1.Width, tabPage1.Height));
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void loadData(ListView lis, ListView lisQuyu, ComboBox cbCenJi1, ComboBox cbCenJi2, ComboBox cbQuYu) 
        {
            List<CenJiBean> cLit = FileModel.getFlModel().CenJiData;
            if (cLit == null || cLit.Count < 1) return;

            if(lis != null)lis.Items.Clear();
            cbCenJi1.Items.Clear();
            cbCenJi2.Items.Clear();
            cbQuYu.Items.Clear();
            if (lisQuyu != null) lisQuyu.Items.Clear();

            foreach (CenJiBean cjItem in cLit)
            {
                CenJiUpdate(cjItem.CenJiName, cjItem.ID, lis);                
                cbCenJi1.Items.Add(getCenJibuder(cjItem).ToString());               
                cbCenJi2.Items.Add(getCenJibuder(cjItem).ToString());               
                if (cjItem.QuYuBeans == null) continue;
                foreach (QuYuBean quYuItem in cjItem.QuYuBeans)
                {
                    quYuUpdate(quYuItem.QuyuID, quYuItem.QuyuName, quYuItem.QuyuType, getCenJibuder(cjItem).ToString(),//cjItem.CenJiName,
                        quYuItem.MapID, lisQuyu);
                    cbQuYu.Items.Add(getquyuNameId(quYuItem));
                }               
            }
            if (cbCenJi2.Items.Count > 0) cbCenJi2.Text = (string)cbCenJi2.Items[0];
            if (cbQuYu.Items.Count > 0) cbQuYu.Text = (string)cbQuYu.Items[0];
        }


        private StringBuilder getCenJibuder(CenJiBean cjItem) 
        {
            StringBuilder buder = new StringBuilder();
            buder.Append(cjItem.CenJiName);
            buder.Append("(");
            buder.Append(cjItem.ID);
            buder.Append(")");
            return buder;
        }


        /// <summary>
        /// 初始化数据2
        /// </summary>
        public void loadData2(ListView lis, ComboBox cbCenJi1, TextBox tbQuYu2, ComboBox cbCenJi, int jurisdiction)
        {
           // Jurisdiction = jurisdiction;

            FileModel.getFlModel().getCFData();
            CacheFileBean chFlBean = FileModel.getFlModel().ChFlBean;
            List<PeopleBean> peoples = chFlBean.Peoples.ToList();
            foreach (PeopleBean ppItem in peoples)
            {
                if (ppItem.Jurisdiction < jurisdiction) continue;
                listPeopleChange(lis, ppItem);
            }

            if (chFlBean != null && chFlBean.LocaIP != null)
            {
                cbCenJi1.Items.Add(chFlBean.LocaIP);
                if (!chFlBean.LocaIP.Equals(XWUtils.GetIpAddress()))
                    cbCenJi1.Items.Add(XWUtils.GetIpAddress());
            }               
            else cbCenJi1.Items.Add(XWUtils.GetIpAddress());
            if (cbCenJi1.Items.Count > 0) cbCenJi1.Text = cbCenJi1.Items[0].ToString();
            if (chFlBean != null )
                tbQuYu2.Text = chFlBean.LocaPort.ToString();

            if (jurisdiction > 5) return;
            PeopleBean bBean = new PeopleBean();
            for (int i = jurisdiction; i < 5; i++)
            {
                bBean.Jurisdiction = i;
                cbCenJi.Items.Add(bBean.JurisdictionStr);
            }
        }

        /// <summary>
        /// 初始化数据3
        /// </summary>
        public void loadData3(ListView nodels, ListView cards)
        {
            List<CardBean> cardBns = FileModel.getFlModel().ChFlBean.Cards.ToList();
            if (cards != null) 
            {
                cards.Items.Clear();
                foreach (CardBean crBean in cardBns)
                {
                    addCard(crBean, cards);
                }
            }
            List<CanKaoDianBean> canKaoBns = FileModel.getFlModel().ChFlBean.CanKaoDians.ToList();
            if (nodels != null) 
            {
                nodels.Items.Clear();
                foreach (CanKaoDianBean ckdBean in canKaoBns)
                {
                    addCanKaoDian(ckdBean, nodels);
                }
            }                       
        }


        /// <summary>
        /// 初始化数据3
        /// </summary>
        public void loadData4(CheckBox cBox1, CheckBox cBox2, CheckBox cBox3, CheckBox cBox4, TextBox time1, TextBox time2, TextBox check3)
        {
            CacheFileBean cbEAN = FileModel.getFlModel().ChFlBean;
            cBox1.Checked = cbEAN.ShowCanKaoDian;
            cBox2.Checked = cbEAN.ShowJingJiTag;
            cBox3.Checked = cbEAN.ShowBlackTag;
            cBox4.Checked = cbEAN.NoShowTag;
            time1.Text = cbEAN.BlackTimeText.ToString();
            time2.Text = cbEAN.NoReveTimeText.ToString();
            check3.Text = cbEAN.CheckCText.ToString();
        }

        public void loadData6(CheckBox cBox1,TextBox time1)
        {
            CacheFileBean cbEAN = FileModel.getFlModel().ChFlBean;
            cBox1.Checked = cbEAN.ShowTagLow;
            time1.Text = cbEAN.TagLowText.ToString();
        }

        /// <summary>
        /// 初始化数据2
        /// </summary>
        public void loadData5(ComboBox cbCenJi1, TextBox tbQuYu2,ComboBox serIP,TextBox serPort)
        {
            CacheFileBean chFlBean = FileModel.getFlModel().ChFlBean;

            if (chFlBean != null && chFlBean.LocaIP_TCP != null)
            {
                cbCenJi1.Items.Add(chFlBean.LocaIP_TCP);
                if (!XWUtils.GetIpAddress().Equals(chFlBean.LocaIP))
                    cbCenJi1.Items.Add(XWUtils.GetIpAddress());
            }
            else cbCenJi1.Items.Add(XWUtils.GetIpAddress());
            if (cbCenJi1.Items.Count > 0) cbCenJi1.Text = cbCenJi1.Items[0].ToString();
            if (chFlBean != null && chFlBean.LocaPort_TCP != 0)
                tbQuYu2.Text = chFlBean.LocaPort_TCP.ToString();

            if (chFlBean != null && chFlBean.ServerIP_TCP != null)
            {
                serIP.Items.Add(chFlBean.ServerIP_TCP);
                if (!XWUtils.GetIpAddress().Equals(chFlBean.LocaIP))
                    serIP.Items.Add(XWUtils.GetIpAddress());
            }
            else serIP.Items.Add(XWUtils.GetIpAddress());
            if (serIP.Items.Count > 0) serIP.Text = serIP.Items[0].ToString();
            if (chFlBean != null && chFlBean.ServerPort_TCP != 0)
                serPort.Text = chFlBean.ServerPort_TCP.ToString();    
        }


        public bool listPeopleChangeItem(ListView lis, PeopleBean ppBean)
        {
            //FileModel.getFlModel().ChFlBean.Peoples
            CacheFileBean cbEAN = FileModel.getFlModel().ChFlBean;
            foreach (PeopleBean pbeanItem in cbEAN.Peoples)
            {
                if (!ppBean.Id.Equals(pbeanItem.Id)) continue;
                for (int i = 0; i < lis.Items.Count;i++ )
                {
                    if (!ppBean.Id.Equals(lis.Items[i].SubItems[0].Text)) continue;
                    listPeopleChange(lis.Items[i], ppBean);
                }
                pbeanItem.Id = ppBean.Id;
                pbeanItem.Name = ppBean.Name;
                pbeanItem.PassWord = ppBean.PassWord;
                pbeanItem.PowerValue = ppBean.PowerValue;
                pbeanItem.Jurisdiction = PeopleBean.JurisdictionStrToInt(ppBean.JurisdictionStr);
                FileModel.getFlModel().setCFData();
                return true;
            }
            return false;
        }

        public void listPeopleChange(ListView lis,PeopleBean ppBean) 
        {
            ListViewItem lvItem = new ListViewItem();
            lvItem.SubItems[0].Text = ppBean.Id;
            lvItem.SubItems.Add(ppBean.Name);//getVersion().ToString("X2")
            lvItem.SubItems.Add(ppBean.PassWord);
            lvItem.SubItems.Add(ppBean.JurisdictionStr);
            lvItem.Tag = ppBean.PowerValue;
            lis.Items.Add(lvItem);
        }


        public void listPeopleChange(ListViewItem lvItem, PeopleBean ppBean)
        {
            lvItem.SubItems[0].Text = ppBean.Id;
            lvItem.SubItems[1].Text = ppBean.Name;
            lvItem.SubItems[2].Text = ppBean.PassWord;
            lvItem.SubItems[3].Text = ppBean.JurisdictionStr;
            lvItem.Tag = ppBean.PowerValue;
        }

        /// <summary>
        /// listView 与TabControl的联动
        /// </summary>
        /// <param name="senderListView"></param>
        /// <param name="tabControl"></param>
        public int setTabPageFromList(object senderListView, TabControl tabControl) 
        {
            if (!(senderListView is ListView)) return -1;
            ListView senderList = (ListView)senderListView;
            if (senderList.SelectedItems.Count < 1) return -1;
            int index = senderList.SelectedItems[0].Index;

            tabControl.SelectedIndex = index;
            return index;
        }

        public void CenJiUpdate(TextBox tb1, TextBox tb2, ListView lv) // 组别ListView添加
        {
            if (!showNull(tb1) || !showNull(tb2)) return;
             
            if (changeCenJi(tb2.Text, tb1.Text, lv)) return;
            CenJiUpdate(tb1.Text, tb2.Text, lv);

            CenJiBean ceBean = new CenJiBean();
            ceBean.ID = tb2.Text;
            ceBean.CenJiName = tb1.Text;
            FileModel.getFlModel().CenJiData.Add(ceBean);
            FileModel.getFlModel().setData();
        }

        public void peopleUpdate(TextBox tb1, TextBox tb2, TextBox tb3, ComboBox cb1, ListView lv,int peopleValue) // 人员ListView添加
        {
            if (!showNull(tb1) || !showNull(tb2) || !showNull(tb3) || !showNull(cb1)) return;

            PeopleBean pBean = new PeopleBean();
            pBean.Jurisdiction = PeopleBean.JurisdictionStrToInt(cb1.Text);
            pBean.Id = tb1.Text;
            pBean.Name = tb2.Text;
            pBean.PassWord = tb3.Text;
            pBean.PowerValue = peopleValue;

            if (listPeopleChangeItem(lv, pBean)) return;
            listPeopleChange(lv, pBean);
            FileModel.getFlModel().ChFlBean.Peoples.Add(pBean);
            FileModel.getFlModel().setCFData();
        }

        /// <summary>
        ///  不能有重复的名称
        ///  以下是type含义
        ///  0 = 人员
        ///  1 = 层级
        ///  2 = 区域
        ///  3 = 节点
        ///  4 = 卡片
        /// </summary>
        /// <param name="tb1"></param>
        /// <param name="tb2"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool nameUpdate(TextBox tb1, TextBox tb2,int type) 
        {
            bool isChongFu = true;
            
            string id = tb1.Text;
            string name = tb2.Text;
            switch(type)
            {
                case 0:
                    peopleChengong(ref isChongFu, id, name);
                    break;
                case 1:
                    cengjiChengong(ref isChongFu, id, name);                    
                    break;
                case 2:
                    quyuChengong(ref isChongFu, id, name);
                    
                    break;
                case 3:
                    nodeChengong(ref isChongFu, id, name);
                    break;
                case 4:
                    cardChengong(ref isChongFu, id, name);
                    break;
                default:
                    break;
            }
            return isChongFu;
        }

        private void cardChengong(ref bool isChong, string id, string name) 
        {
            Dictionary<string, CardBean> cardDic = new Dictionary<string,CardBean>(FileModel.getFlModel().ChFlBean.CardDic);
            foreach (var item in cardDic)
            {
                if (!item.Value.Id.Equals(id) && item.Value.Name.Equals(name)) 
                {
                    isChong = false;
                    break;
                }
            }            
        }

        private void nodeChengong(ref bool isChong, string id, string name)
        {
            CacheFileBean cbEAN = FileModel.getFlModel().ChFlBean;
            List<CanKaoDianBean> canKaoDians = cbEAN.CanKaoDians.ToList();
            foreach (CanKaoDianBean ckdItem in canKaoDians)
            {
                if (!id.Equals(ckdItem.Id) && name.Equals(ckdItem.Name))
                {
                    isChong = false;
                    break;
                }
            }
        }

        private void quyuChengong(ref bool isChong, string id, string name)
        {
            List<CenJiBean> cLits = FileModel.getFlModel().CenJiData;
            foreach (CenJiBean cjItem in cLits)
            {
                foreach (QuYuBean quYuItem in cjItem.QuYuBeans)
                {
                    if (!id.Equals(quYuItem.QuyuID) && name.Equals(quYuItem.QuyuName))
                    {
                        isChong = false;
                        break;
                    }
                }
            }
        }

        private void cengjiChengong(ref bool isChong, string id, string name)
        {
            List<CenJiBean> cLit = FileModel.getFlModel().CenJiData;
            if (cLit == null || cLit.Count < 1) return ;
            foreach (CenJiBean cjItem in cLit)
            {
                if (!id.Equals(cjItem.ID) && name.Equals(cjItem.CenJiName))
                {
                    isChong = false;
                    break;
                }
            }
        }

        private void peopleChengong(ref bool isChong,string id ,string name)
        {
            List<PeopleBean> peoples = FileModel.getFlModel().ChFlBean.Peoples;
            foreach (PeopleBean pbeanItem in peoples)
            {
                if (name.Equals(pbeanItem.Name) && !id.Equals(pbeanItem.Id))
                {
                    isChong = false;
                    break;
                }
            }
        }

        /// <summary>
        /// View为空时，显示提示
        /// </summary>
        /// <param name="objView"></param>
        /// <returns></returns>
        private bool showNull(Object objView) 
        {
            if (!isNullText(objView)) 
            {
                MessageBox.Show("請輸入值");
                return false;
            }
            return true;
        }


        /// <summary>
        /// View是否是空的文本
        /// </summary>
        /// <param name="objView"></param>
        /// <returns></returns>
        private bool isNullText(Object objView)
        {
            if (!(objView is TextBox) && !(objView is ComboBox)) return false;
            Control ctrView = (Control)objView;
            if (ctrView.Text.Equals(""))
            {
                return false;
            }
            return true;
        }

        public bool changeCenJi(string subID, string subName, ListView lv) 
        {
            List<CenJiBean> cLit = FileModel.getFlModel().CenJiData;
            if (cLit == null || cLit.Count < 1) return false;
            foreach (CenJiBean cjItem in cLit)
            {
                if (!cjItem.ID.Equals(subID)) continue;
                cjItem.CenJiName = subName;
                changeCenJiList(subID, subName, lv);
                FileModel.getFlModel().setData();
                return true;
            }
            return false;
        }


        public void changeCenJiList(string subID, string subName, ListView lView)
        {
            for (int i = 0; i < lView.Items.Count; i++)
            {
                ListViewItem lvItem = lView.Items[i];
                if (subID.Equals(lvItem.SubItems[0].Text))
                {
                    lView.Items[i].SubItems[1].Text = subName;
                    break;
                }
            }//for
        }


        public void CenJiUpdate(string subT1, string subT2, ListView lv) // 组别ListView添加
        {
            if (lv == null) return;
            ListViewItem lvItem = new ListViewItem();
            lvItem.SubItems[0].Text = subT2;
            lvItem.SubItems.Add(subT1);//getVersion().ToString("X2")
            lv.Items.Add(lvItem);          
        }

        /// <summary>
        /// 区域List表添加
        /// </summary>
        public void quYuUpdate(string quYuID, string quYuName, string quYuType, string zu, string mapID, ListView lv) 
        {
            ListViewItem lvItem = new ListViewItem();
            lvItem.SubItems[0].Text = quYuID;
            lvItem.SubItems.Add(quYuName);//getVersion().ToString("X2")
            lvItem.SubItems.Add(quYuType);
            lvItem.SubItems.Add(zu);
            lvItem.SubItems.Add(mapID);
            lv.Items.Add(lvItem);
        }

        /// <summary>
        /// 只允许数字的输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void justNumberInput(KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (Char)8)
            {//
                e.Handled = true;
            }
        }

        public void removePeopleItem(ListView lView, ListViewItem lvItem)
        {
            if (lView == null || lvItem == null ) return;
            if (mCSVI != null) mCSVI.deleteListViewItem(lView, lvItem);
            CacheFileBean cbEAN = FileModel.getFlModel().ChFlBean;
            List<PeopleBean> peoples = cbEAN.Peoples;
            string lvItemId = lvItem.SubItems[0].Text;
            foreach (PeopleBean pbeanItem in peoples)
            {
                if (!lvItemId.Equals(pbeanItem.Id)) continue;
                FileModel.getFlModel().ChFlBean.Peoples.Remove(pbeanItem);
                break;
            }
            FileModel.getFlModel().setCFData();
        }


        /// <summary>
        /// 组别或者区域删除
        /// </summary>
        /// <param name="lView"></param>
        /// <param name="lView2"></param>
        /// <param name="lvItem"></param>
        /// <param name="cType"></param>
        public void removeItem(ListView lView, ListView lView2, ListViewItem lvItem, CenQuType cType) 
        {
            if (lView == null || lvItem == null) return;
            removeItem(lView, lvItem,cType);
            if (cType != CenQuType.CENGJI) return;
            string cenjiID = lvItem.SubItems[0].Text;
            removeItem(lView2, cenjiID);       
        }


        /// <summary>
        /// 删除Item，若删除的是区域，则相应的存储也要删除
        /// </summary>
        /// <param name="lView"></param>
        /// <param name="lvItem"></param>
        /// <param name="cType"></param>
        public void removeItem(ListView lView, ListViewItem lvItem, CenQuType cType)
        {
            if (mCSVI != null) mCSVI.deleteListViewItem(lView, lvItem);
            if (cType != CenQuType.QUYU) return;

            string cenJiID = getIDFromKuohao(lvItem.SubItems[3].Text);
            List<CenJiBean> cLit = FileModel.getFlModel().CenJiData;
            if (cLit == null || cLit.Count < 1) return;
            foreach (CenJiBean cjItem in cLit)
            {
                if (!cenJiID.Equals(cjItem.ID)) continue;
                foreach (QuYuBean quYuItem in cjItem.QuYuBeans)
                {
                    if (quYuItem.QuyuID.Equals(lvItem.SubItems[0].Text))
                    {
                        cjItem.QuYuBeans.Remove(quYuItem);
                        FileModel.getFlModel().setData();
                        break;
                    }
                }
                break;
            }
            List<CanKaoDianBean> canKaoBns = FileModel.getFlModel().ChFlBean.CanKaoDians.ToList();
            var canKaoBn = canKaoBns.Where(a => a.QuYuID.Equals(lvItem.SubItems[0].Text)).ToList();
            foreach (var item in canKaoBn) 
            {
                canKaoBns.Remove(item);
            }
        }


        /// <summary>
        /// 删掉组别之后，对应的区域也被删掉
        /// </summary>
        /// <param name="lView"></param>
        /// <param name="cenjiID"></param>
        public void removeItem(ListView lView, string cenjiID)
        {
            if (lView == null) return;
            List<CenJiBean> cLit = FileModel.getFlModel().CenJiData;
            if (cLit == null || cLit.Count < 1) return;
            List<ListViewItem> mListItem = new List<ListViewItem>();

            foreach (CenJiBean cjItem in cLit)
            {
                if (!cenjiID.Equals(cjItem.ID)) continue;
                foreach (QuYuBean quYuItem in cjItem.QuYuBeans)
                {
                    for (int i = 0; i < lView.Items.Count; i++) 
                    {
                        ListViewItem lvItem = lView.Items[i];
                        if (quYuItem.QuyuID.Equals(lvItem.SubItems[0].Text)) 
                        {
                            mListItem.Add(lView.Items[i]);
                            break;
                        }
                    }   //for
                }
                cLit.Remove(cjItem);
                FileModel.getFlModel().setData();
                break;
            }   //foreach  cLit

            foreach (ListViewItem item in mListItem)
            {
                mCSVI.deleteListViewItem(lView, item);
            }
        }


        public void quYuUpdate(TextBox tb1, TextBox tb2, TextBox tb3,ComboBox comboBox2, ListView lv) // 区域添加
        {
            if (!showNull(tb1) || !showNull(tb2) || !showNull(tb3) || !showNull(comboBox2)) return;
            string cenJiID = getIDFromKuohao(comboBox2.Text);
            if (changeQuYu(tb1.Text, tb2.Text, "一般區域", comboBox2.Text, tb3.Text, lv)) return;
            quYuUpdate(tb1.Text, tb2.Text, "一般區域", comboBox2.Text, tb3.Text, lv);
            List<CenJiBean> cLit = FileModel.getFlModel().CenJiData;
            if (cLit == null || cLit.Count < 1) return ;
            foreach (CenJiBean cjItem in cLit)
            {
                if (!cjItem.ID.Equals(cenJiID)) continue;
                QuYuBean quYuBean = new QuYuBean();
                quYuBean.QuyuID = tb1.Text;
                quYuBean.QuyuName = tb2.Text;
                quYuBean.QuyuType = "一般區域";
                quYuBean.PepleCount = 0;
                quYuBean.MapPath = tb3.Text;
                quYuBean.MapID = getMapNameFromPath(tb3.Text);

                if (cjItem.QuYuBeans == null) cjItem.QuYuBeans = new List<QuYuBean>();
                cjItem.QuYuBeans.Add(quYuBean);
                FileModel.getFlModel().setData();
                return;            
            }
        }

        /// <summary>
        /// 从括号中提取层级ID号
        /// </summary>
        /// <returns></returns>
        public string getIDFromKuohao(string cejiText) 
        {
            int start = cejiText.IndexOf('(');
            int end = cejiText.IndexOf(')');
            return cejiText.Substring(start + 1, end - start-1);
        }


        public string getMapNameFromPath(string mapPath) 
        {
            return Path.GetFileName(mapPath);
        }

        public bool changeQuYu(string quYuID, string quYuName,string quYuType,string group,string mapPath, ListView lv)
        {
            List<CenJiBean> cLit = FileModel.getFlModel().CenJiData;
            if (cLit == null || cLit.Count < 1) return false;
            bool isHaveItem = false;
            foreach (CenJiBean cjItem in cLit)
            {                
                //if (cjItem.QuYuBeans == null) continue;
                foreach (QuYuBean quYuItem in cjItem.QuYuBeans)
                {
                    if (!quYuID.Equals(quYuItem.QuyuID)) continue; //区域ID和存储对象中区域ID不一样，不往后面走
                    isHaveItem = true;
                    quYuItem.QuyuName = quYuName;
                    if (mapPath.Length > 2 && ":".Equals(mapPath.Substring(1,1)))
                    {
                        quYuItem.MapID = getMapNameFromPath(mapPath);
                        quYuItem.MapPath = mapPath;
                    }                    
                    quYuJiList(quYuID, quYuName, quYuType, group, mapPath, lv);

                    if (getIDFromKuohao(group).Equals(cjItem.ID)) break;

                    foreach (CenJiBean cjSunItem in cLit)
                    {
                        if (getIDFromKuohao(group).Equals(cjSunItem.ID))                        
                            cjSunItem.QuYuBeans.Add(quYuItem);                      
                    }
                    cjItem.QuYuBeans.Remove(quYuItem);
                    break;
                }                          
            }
            if (isHaveItem)
            {
                FileModel.getFlModel().setData();
                return true;
            }      
            return false;
        }


        //区域列表List的更新
        public void quYuJiList(string quYuID, string quYuName, string quYuType, string group, string mapName, ListView lv)
        {
            for (int i = 0; i < lv.Items.Count; i++)
            {
                ListViewItem lvItem = lv.Items[i];
                if (quYuID.Equals(lvItem.SubItems[0].Text))
                {
                    lv.Items[i].SubItems[1].Text = quYuName;
                    lv.Items[i].SubItems[2].Text = quYuType;
                    lv.Items[i].SubItems[3].Text = group;
                    if (mapName.Length > 2 && ":".Equals(mapName.Substring(1, 1)))
                    {
                        lv.Items[i].SubItems[4].Text = getMapNameFromPath(mapName);
                    }                  
                    break;
                }
            }//for
        }

        //区域列表点击后，在旁边textBox也显示点击
        public void quYuListSelect(ListViewItem lvItem, TextBox tb1, TextBox tb2, TextBox tb3, ComboBox comboBox2) 
        {
            if (lvItem == null) return;
            mCSVI.textBoxChange(tb1, lvItem.SubItems[0].Text);
            mCSVI.textBoxChange(tb2, lvItem.SubItems[1].Text);
            mCSVI.textBoxChange(comboBox2, lvItem.SubItems[3].Text);

            List<CenJiBean> cLit = FileModel.getFlModel().CenJiData;
            if (cLit == null || cLit.Count < 1) return ;
            foreach (CenJiBean cjItem in cLit)
            {
                foreach (QuYuBean quYuItem in cjItem.QuYuBeans)
                {
                    if (!lvItem.SubItems[0].Text.Equals(quYuItem.QuyuID)) continue;
                    //mCSVI.textBoxChange(tb3, lvItem.SubItems[4].Text);
                    mCSVI.textBoxChange(tb3, quYuItem.MapPath);
                    return;
                }
            }                       
        }

        //节点列表点击后，在旁边textBox也显示点击
        public void qcanKaodiNListSelect(ListViewItem lvItem, TextBox tb1, TextBox tb2, TextBox tb3, TextBox tb4)
        {
            if (lvItem == null) return;
            mCSVI.textBoxChange(tb1, lvItem.SubItems[0].Text);
            mCSVI.textBoxChange(tb2, lvItem.SubItems[1].Text);
            mCSVI.textBoxChange(tb3, lvItem.SubItems[2].Text);
            mCSVI.textBoxChange(tb4, lvItem.SubItems[3].Text);
        }

         //节点列表点击后，在旁边textBox也显示点击
        public void peopleCardListSelect(ListViewItem lvItem, TextBox tb1, TextBox tb2)
        {
            if (lvItem == null) return;
            mCSVI.textBoxChange(tb1, lvItem.SubItems[0].Text);
            mCSVI.textBoxChange(tb2, lvItem.SubItems[1].Text);
        }

        //区域列表点击后，在旁边textBox也显示点击
        public void peopleListSelect(ListViewItem lvItem, TextBox tb1, TextBox tb2, TextBox tb3, ComboBox comboBox1)
        {
            if (lvItem == null) return;
            mCSVI.textBoxChange(tb1, lvItem.SubItems[0].Text);
            mCSVI.textBoxChange(tb2, lvItem.SubItems[1].Text);
            mCSVI.textBoxChange(tb3, lvItem.SubItems[2].Text);
            mCSVI.textBoxChange(comboBox1, lvItem.SubItems[3].Text);
        }

        public void cenJiListSelect(ListViewItem lvItem, TextBox tb1, TextBox tb2)
        {
            if (lvItem == null) return;
            mCSVI.textBoxChange(tb1, lvItem.SubItems[0].Text);
            mCSVI.textBoxChange(tb2, lvItem.SubItems[1].Text);
        }

        /// <summary>
        /// 缓存本地IP
        /// </summary>
        /// <param name="locationIP"></param>
        /// <param name="type">1 = locationIP , 2 = locationIP_TCP,3 = </param>
        public void cacheLocationIP(string locationIP,int type)
        {
            if (locationIP == null || locationIP.Length < 1) return;
            if (type == 1) FileModel.getFlModel().ChFlBean.LocaIP = locationIP;
            else if (type == 2) FileModel.getFlModel().ChFlBean.LocaIP_TCP = locationIP;
            else if (type == 3) FileModel.getFlModel().ChFlBean.ServerIP_TCP = locationIP;
            FileModel.getFlModel().setCFData();
        }


        /// <summary>
        /// 缓存本地端口
        /// </summary>
        /// <param name="locationPort"></param>
        /// <param name="type">1 = locationPort , 2 = locationPort_TCP</param>
        public void cacheLocationPort(string locationPort, int type)
        {
            if (locationPort == null || locationPort.Length < 1) return;
            int port = XWUtils.stringToInt1(locationPort);
            if (port == -1) return;
            if (port < 0 || port > 65535) 
            {
                MessageBox.Show("失敗");
                return;
            }
            if (type == 1) FileModel.getFlModel().ChFlBean.LocaPort = port;
            else if (type == 2) FileModel.getFlModel().ChFlBean.LocaPort_TCP = port;
            else if (type == 3) FileModel.getFlModel().ChFlBean.ServerPort_TCP = port;
            FileModel.getFlModel().setCFData();
        }


        public void comItemSelect(ComboBox comScour, ComboBox com)
        {
            if (comScour.Text == null || comScour.Text.Length < 1) return;

            List<CenJiBean> cLit = FileModel.getFlModel().CenJiData;
            if (cLit == null || cLit.Count < 1) return ;
            foreach (CenJiBean cjItem in cLit)
            {
                string comID = getIDFromKuohao(comScour.Text);
                if (!cjItem.ID.Equals(comID)) continue;
                if (cjItem.QuYuBeans == null) break;
                com.Items.Clear();
                foreach (QuYuBean quYuItem in cjItem.QuYuBeans)
                {
                    com.Items.Add(getquyuNameId(quYuItem).ToString());
                }
            }
        }

        private StringBuilder getquyuNameId(QuYuBean quYuItem) 
        {
            StringBuilder buder = new StringBuilder();
            buder.Append(quYuItem.QuyuName);
            buder.Append("(");
            buder.Append(quYuItem.QuyuID);
            buder.Append(")");
            return buder;
        } 

        public void upDataNode(TextBox tb1, TextBox tb2, TextBox tb3, TextBox tb4,ListView lv)
        {
            if (!showNull(tb1) || !showNull(tb2) || !showNull(tb3) || !showNull(tb4)) return;
            upDataNode(tb1.Text, tb2.Text, tb3.Text, tb4.Text);

            for (int i = 0; i < lv.Items.Count;i++ )
            {
                if (!lv.Items[i].SubItems[0].Text.Equals(tb1.Text)) continue;
                lv.Items[i].SubItems[1].Text = tb2.Text;
                return;
            }
        }

        public bool upDataNode(string nodeID,string nodeName,string cenjiName,string quyuName)
        {
            List<CanKaoDianBean> canKaoDians = FileModel.getFlModel().ChFlBean.CanKaoDians;
            foreach (CanKaoDianBean ckdItem in canKaoDians)
            {
                if (!nodeID.Equals(ckdItem.Id)) continue;
                ckdItem.Name = nodeName;
                FileModel.getFlModel().setCFData();
                return true;
            }
            return false;
        }


        public void upDataCard(TextBox tb1, TextBox tb2, ListView lv)
        {
            if (!showNull(tb1) || !showNull(tb2) ) return;
            if (upDataCard(tb1.Text, tb2.Text, lv)) return;
            addCard(tb1.Text, tb2.Text, lv);

            CardBean cardBean = new CardBean();
            cardBean.Id = tb1.Text;
            cardBean.Name = tb2.Text;
            FileModel.getFlModel().ChFlBean.addDrivace(cardBean);
            FileModel.getFlModel().setCFData();
        }

        public bool upDataCard(string nodeID, string nodeName, ListView lv)
        {
            //List<CardBean> cards = FileModel.getFlModel().ChFlBean.Cards;

            Dictionary<string, CardBean> cardDic = new Dictionary<string,CardBean>(FileModel.getFlModel().ChFlBean.CardDic);
            if (cardDic != null && cardDic.ContainsKey(nodeID)) 
            {
                FileModel.getFlModel().ChFlBean.setCardValues(nodeID, nodeName);
                //cardItem.Name = nodeName;
                FileModel.getFlModel().setCFData();
                for (int i = 0; i < lv.Items.Count; i++)
                {
                    if (!lv.Items[i].SubItems[0].Text.Equals(nodeID)) continue;
                    lv.Items[i].SubItems[1].Text = nodeName;
                    break;
                }
                return true;
            }
             
            /*foreach (CardBean cardItem in cards)
            {
                if (!cardItem.Id.Equals(nodeID)) continue;
                cardItem.Name = nodeName;
                FileModel.getFlModel().setCFData();
                for (int i = 0; i < lv.Items.Count;i++ )
                {
                    if (!lv.Items[i].SubItems[0].Text.Equals(nodeID)) continue;
                    lv.Items[i].SubItems[1].Text = nodeName;
                    break;
                }
                return true;
            }*/
            return false;
        }

        public void addCard(string nodeID, string nodeName, ListView lv)
        {
            ListViewItem lItem = new ListViewItem();
            lItem.SubItems[0].Text = nodeID;
            lItem.SubItems.Add(nodeName);
            lv.Items.Add(lItem);
        }

        public void addCard(CardBean cardBean, ListView lv)
        {
            addCard(cardBean.Id, cardBean.Name, lv);
        }

        public void removeCardItem(ListView lView, ListViewItem lvItem)
        {
            if (mCSVI != null) mCSVI.deleteListViewItem(lView, lvItem);

            List<CardBean> cards = FileModel.getFlModel().ChFlBean.Cards;
            foreach (CardBean cardItem in cards)
            {
                if (!cardItem.Id.Equals(lvItem.SubItems[0].Text)) continue;
                cards.Remove(cardItem);
                FileModel.getFlModel().ChFlBean.deleteCardValues(cardItem.Id);
                break;
            }
            FileModel.getFlModel().setCFData();
        }

        public void addCanKaoDian(CanKaoDianBean ckdBean, ListView lv)
        {
            ListViewItem lItem = new ListViewItem();
            lItem.SubItems[0].Text = ckdBean.Id;
            lItem.SubItems.Add(ckdBean.Name);
            lItem.SubItems.Add(ckdBean.CenJiname+"("+ckdBean.CenJiID+")");
            lItem.SubItems.Add(ckdBean.QuYuname + "(" + ckdBean.QuYuID+")");
            lv.Items.Add(lItem);
        }

        public void comItemSelect2(ComboBox comScour, ComboBox com,TextBox tb,Panel pl)
        {
            if (comScour.Text == null || comScour.Text.Length < 1) return;

            List<CenJiBean> cLit = FileModel.getFlModel().CenJiData;
            if (cLit == null || cLit.Count < 1) return;
            foreach (CenJiBean cjItem in cLit)
            {
                string comID = getIDFromKuohao(comScour.Text);
                if (!cjItem.ID.Equals(comID)) continue;
                if (cjItem.QuYuBeans == null) break;

                foreach (QuYuBean quyuItem in cjItem.QuYuBeans)
                {
                    if (!getIDFromKuohao(com.Text).Equals(quyuItem.QuyuID)) continue;
                    if (quyuItem.MapPath == null || !":".Equals(quyuItem.MapPath.Substring(1, 1))) 
                    {
                        tb.Text = "";
                        pl.BackgroundImage = null;
                        return;
                    }
                    try {
                        Bitmap bitmap = new Bitmap(quyuItem.MapPath, true);
                        bitmap = new Bitmap(bitmap, pl.Width, pl.Height);
                        pl.BackgroundImage = bitmap;
                    }
                    catch { }                    
                    tb.Text = quyuItem.MapPath;
                    addViewInPanel(pl, quyuItem.QuyuID, cjItem.ID);
                    break;
                }

            }// foreach (CenJiBean cjItem in cLit)
        }

        private void addViewInPanel(Panel pl,string quyuID,string cengjiID ) 
        {
            List<CanKaoDianBean> canKaoBns = FileModel.getFlModel().ChFlBean.CanKaoDians.ToList();

            foreach (CanKaoDianBean ckdBean in canKaoBns)
            {
                if (!quyuID.Equals(ckdBean.QuYuID) || !cengjiID.Equals(ckdBean.CenJiID)) continue;

                CanKaoDianView cView = new CanKaoDianView();
                cView.CkdID = ckdBean.CanDianID;
                cView.BackColor = Color.Transparent;
                Point p = ckdBean.POint;
                cView.Location = p;
                cView.LabText = ckdBean.Name;
                //cView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(CKDView_mouDouble);
                pl.Controls.Add(cView);
                //break;
            }
        }


        /// <summary>
        /// 添加参考点
        /// </summary>
        /// <param name="pal"></param>
        /// <param name="mouseP"></param>
        public CanKaoDianView canKaoDianAdd(Panel pal, CanKaoDianBean cdBean, string cenJiName, string quYuName) 
        {
            if (isHaveCanKaoDian(cdBean, cenJiName, quYuName)) 
            {
                MessageBox.Show("該ID已添加");
                return null;
            }
                
            CanKaoDianView cView = new CanKaoDianView();
            cView.BackColor = Color.Transparent;
            cView.CkdID = cdBean.CanDianID;
            Point p = cdBean.POint;
            p.X -= cView.centerPoint().X;
            p.Y -= cView.centerPoint().Y;
            cView.Location = p;
            cView.LabText = cdBean.Name;
            cdBean.ColWeiHei = new int[2] { pal.Width, pal.Height };
            pal.Controls.Add(cView);

            //List<CanKaoDianBean> canKaoLits = FileModel.getFlModel().ChFlBean.CanKaoDians.ToList();
            CanKaoDianBean cBean = new CanKaoDianBean(cdBean);

            List<CenJiBean> cLits = FileModel.getFlModel().CenJiData;
            if (cLits == null || cLits.Count < 1) return cView;

            foreach (CenJiBean cjItem in cLits)
            {
                string comID = getIDFromKuohao(cenJiName);

                if (!cjItem.ID.Equals(comID)) continue;
                if (cjItem.QuYuBeans == null) break;
                cBean.CenJiID = comID;
                cBean.CenJiname = cjItem.CenJiName;
                foreach (QuYuBean quyuItem in cjItem.QuYuBeans)
                {
                    string quYuID = getIDFromKuohao(quYuName);
                    if (!quYuID.Equals(quyuItem.QuyuID)) continue;
                    cBean.QuYuID = quYuID;
                    cBean.QuYuname = quyuItem.QuyuName;
                    break;
                }
                break;
            }
            cBean.POint = p;
            cBean.Name = cdBean.Name;
            cBean.Id = cdBean.Id;
            cBean.ColWeiHei = new int[2] { pal.Width, pal.Height };

            FileModel.getFlModel().ChFlBean.CanKaoDians.Add(cBean);           
            FileModel.getFlModel().setCFData();
            return cView;
        }


        public void changeNodeCenjiID(string cengjiID)
        {
            List<CenJiBean> cLits = FileModel.getFlModel().CenJiData.ToList();
            if (cLits == null || cLits.Count < 1) return ;
            List<CanKaoDianBean> canKaoDians = FileModel.getFlModel().ChFlBean.CanKaoDians;
            for (int i = 0; i < canKaoDians.Count; i++)
            {
                CanKaoDianBean canItem = canKaoDians[i];
                if (!canItem.CenJiID.Equals(cengjiID)) continue;
                changeNodeCenjiID(cengjiID, canItem, cLits);
            }            
        }


        public void changeNodeCenjiID(string cengjiID, CanKaoDianBean canItem, List<CenJiBean> cLits)
        {            
            foreach (CenJiBean cjItem in cLits)
            {
                if (!cjItem.ID.Equals(cengjiID)) continue;
                canItem.CenJiname = cjItem.CenJiName;               
                break;
            }
        }

        public void changeNodeQuYuID(string quYuID)
        {
            List<CenJiBean> cLits = FileModel.getFlModel().CenJiData.ToList();
            if (cLits == null || cLits.Count < 1) return;
            List<CanKaoDianBean> canKaoDians = FileModel.getFlModel().ChFlBean.CanKaoDians;
            for (int i = 0; i < canKaoDians.Count; i++)
            {
                CanKaoDianBean canItem = canKaoDians[i];
                if (!quYuID.Equals(canItem.QuYuID)) continue;
                changeNodeQuYuID(quYuID, canItem, cLits);
            }                    
        }

        public void changeNodeQuYuID(string quYuID, CanKaoDianBean canItem, List<CenJiBean> cLits)
        {
            foreach (CenJiBean cjItem in cLits)
            {
                if (cjItem.QuYuBeans == null) continue;
                foreach (QuYuBean quyuItem in cjItem.QuYuBeans)
                {                    
                    if (!quYuID.Equals(quyuItem.QuyuID)) continue;
                    if (!cjItem.ID.Equals(canItem.CenJiID)) canItem.CenJiID = cjItem.ID;
                    if (!canItem.CenJiname.Equals(cjItem.CenJiName)) canItem.CenJiname = cjItem.CenJiName;
                    canItem.QuYuname = quyuItem.QuyuName;
                    return;
                }
            }      
        }


        private bool isHaveCanKaoDian(CanKaoDianBean cdBean, string cenJiName, string quYuName) 
        {
            List<CanKaoDianBean> canKaoLits = FileModel.getFlModel().ChFlBean.CanKaoDians.ToList();
            foreach (CanKaoDianBean cdItem in canKaoLits)
            {
                if (!cdBean.Id.Equals(cdItem.Id)) continue;
                /*string comID = getIDFromKuohao(cenJiName);
                string quYuID = getIDFromKuohao(quYuName);
                if (cdItem.CenJiID.Equals(comID) && cdItem.QuYuID.Equals(quYuID))*/
                return true;
            }
            return false;
        }

        public CanKaoDianBean haveCanKaoDian(CanKaoDianBean cdBean, string cenJiName, string quYuName)
        {
            List<CanKaoDianBean> canKaoLits = FileModel.getFlModel().ChFlBean.CanKaoDians.ToList();
            foreach (CanKaoDianBean cdItem in canKaoLits)
            {
                if (!cdBean.Id.Equals(cdItem.Id)) continue;
                /*string comID = getIDFromKuohao(cenJiName);
                string quYuID = getIDFromKuohao(quYuName);
                if (cdItem.CenJiID.Equals(comID) && cdItem.QuYuID.Equals(quYuID))*/
                return cdItem;
            }
            return null;
        }

        /// <summary>
        /// 删除参考点
        /// </summary>
        /// <param name="pal"></param>
        /// <param name="mouseP"></param>
        public void canKaoDianDelete(Panel pal, CanKaoDianBean cdBean)
        {
            foreach (Control ctrl in pal.Controls)
            {
                if (!(ctrl is CanKaoDianView)) return;
                CanKaoDianView canView = (CanKaoDianView)ctrl;
                if (canView.LabText.Equals(cdBean.Name) || canView.LabText.Equals(cdBean.Id)) 
                {
                    pal.Controls.Remove(ctrl);
                    break;
                }
            }
            List<CanKaoDianBean> canKaoLits = FileModel.getFlModel().ChFlBean.CanKaoDians;
            foreach (CanKaoDianBean ckBean in canKaoLits) 
            {
                if (ckBean.Id.Equals(cdBean.Id)) 
                {
                    canKaoLits.Remove(ckBean);
                    FileModel.getFlModel().setCFData();
                    break;
                }
            }            
        }

        /// <summary>
        /// 添加参考点
        /// </summary>
        /// <param name="pal"></param>
        /// <param name="mouseP"></param>
        public void canKaoDianMove(Point locaP,string name,string cenJiName, string quYuName,int width,int height)
        {
            string comID = getIDFromKuohao(cenJiName);
            string quYuID = getIDFromKuohao(quYuName);

            List<CanKaoDianBean> canKaoLits = FileModel.getFlModel().ChFlBean.CanKaoDians.ToList();
            foreach (CanKaoDianBean cBean in canKaoLits)
            {
                if (!comID.Equals(cBean.CenJiID) || !quYuID.Equals(cBean.QuYuID)) continue;
                if (!name.Equals(cBean.Name)) continue;
                cBean.POint = locaP;

                if (cBean.ColWeiHei == null) cBean.ColWeiHei = new int[2];
                cBean.ColWeiHei[0] = width;
                cBean.ColWeiHei[1] = height;
            }
            FileModel.getFlModel().setCFData();
        }

        public void saveNODEXlsFile(string filePath)
        {
            try
            {
                // 1.获取数据集合
                List<CanKaoDianBean> enlist = FileModel.getFlModel().ChFlBean.CanKaoDians.ToList();
                // 2.设置单元格抬头
                Dictionary<string, string> cellheader = new Dictionary<string, string> { 
                    { "Name", "名稱" },
                    { "Id", "節點ID" },
                    { "CenJiname", "層級名稱" },
                    { "CenJiID", "層級ID" },
                    { "QuYuname", "區域名稱" },
                    { "QuYuID", "區域ID" },     
                    //{ "PeopleCount", "人數" },     
                    //{ "POint.X", "X坐标" },      
                    //{ "POint.Y", "Y坐标" },   
                };
                // 3.进行Excel转换操作，并返回转换的文件下载链接
                string urlPath = ExcelHelper.EntityListToExcel2003(cellheader, enlist, filePath, "NODEInfor");
            }
            catch (Exception ex)
            {
                Debug.Write("CanShuSetControl.saveNODEXlsFile"+ex.Message);
            }
        }

        public void importNODEXlsFile(string filePath)
        {
            StringBuilder errorMsg = new StringBuilder(); // 错误信息
            // 单元格抬头
            // key：实体对象属性名称，可通过反射获取值
            // value：属性对应的中文注解
            Dictionary<string, string> cellheader = new Dictionary<string, string> { 
                    { "Name", "名稱" },
                    { "Id", "節點ID" },
                };
            // 1.2解析文件，存放到一个List集合里
            try {
                List<CanKaoDianBean> enlist = ExcelHelper.ExcelToEntityList<CanKaoDianBean>(cellheader, filePath, "NODEInfor", out errorMsg);
                FormDialog formDialog = new FormDialog(DrivaceTypeAll.CANKAODIAN);
                formDialog.saveNODEID(enlist);
                DialogResult result = formDialog.ShowDialog();
                refashIss(result, RefashList.CANKAODIANLIST);
            }
            catch(Exception e) {
                MessageBox.Show(e.ToString());
            }            
        //    if (errorMsg.Length > 0) MessageBox.Show(errorMsg.ToString());
        }

        /// <summary>
        /// 接口回調，表示導入結果
        /// </summary>
        /// <param name="result"></param>
        /// <param name="refa"></param>
        public void refashIss(DialogResult result,RefashList refa) 
        {
            if (result == DialogResult.OK) 
            {
                if (mCSVI != null) mCSVI.refash(refa, true);
            }
            else if (result == DialogResult.None) 
            {
                if (mCSVI != null) this.mCSVI.refash(refa, false);
            }                
        }

        public void saveCardXlsFile(string filePath)
        {
            try
            {
                List<CardBean> enlist = FileModel.getFlModel().ChFlBean.Cards.ToList();
                Dictionary<string, string> cellheader = new Dictionary<string, string> { 
                    { "Name", "名稱" },
                    { "Id", "卡片ID" },     
                };
                string urlPath = ExcelHelper.EntityListToExcel2003(cellheader, enlist, filePath, "CardInfo");
            }
            catch (Exception ex) 
            {
                Debug.Write("CanShuSetControl.saveCardXlsFile" + ex.Message);
            }
        }

        public void importCardXlsFile(string filePath)//导入卡片XLS资料
        {
            StringBuilder errorMsg = new StringBuilder(); // 错误信息
            Dictionary<string, string> cellheader = new Dictionary<string, string> { 
                    { "Name", "名稱" },
                    { "Id", "卡片ID" },
            };
            try
            {
                List<CardBean> enlist = ExcelHelper.ExcelToEntityList<CardBean>(cellheader, filePath, "CardInfo", out errorMsg);
                FormDialog formDialog = new FormDialog(DrivaceTypeAll.CARD);
                formDialog.saveCardID(enlist);
                DialogResult result = formDialog.ShowDialog();
                refashIss(result, RefashList.CARDLIST);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            //if (errorMsg.Length > 0) MessageBox.Show(errorMsg.ToString());
        }

    }
}
