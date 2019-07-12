using CiXinLocation.bean;
using CiXinLocation.CiXinInterface;
using CiXinLocation.control;
using CiXinLocation.Model;
using CiXinLocation.Utils;
using Common;
using MoveableListLib;
using SerialportSample;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CiXinLocation
{
    public partial class CanShuSetFrom : Form, CanShuViewInterface
    {
        /// <summary>
        /// 改From的控制器
        /// </summary>
        private CanShuSetControl cssCon;

        public CanShuSetFrom()
        {
            cssCon = new CanShuSetControl(this);
            InitializeComponent();
        }

        private void CanShuSet_Load(object sender, EventArgs e)
        {
            this.FormClosed += formClosedEventHandler;
            cssCon.removeTabControlTitle(tabControl1, tabPage1);
            cssCon.loadData5(comboBox6, textBox19, comboBox8, textBox26);
            if (PeoplePowerModel.getPeoplePowerModel().Jurisdiction == PeoplePowerModel.getPeoplePowerModel().CongjiValue)
            {
                listView1.Items.Clear();
                listView1.Items.Add("網絡設定");
                try 
                {
                    tabControl1.SelectedIndex = 4;
                }
                catch { }               
                return;
            }
            
            cssCon.loadData(listView3, listView4, comboBox2, comboBox4, comboBox5);
            cssCon.loadData2(listView2, comboBox3, textBox8, comboBox1,PeoplePowerModel.getPeoplePowerModel().Jurisdiction);
            cssCon.loadData3(listView5, listView6);
            cssCon.loadData4(checkBox1, checkBox2, checkBox3, checkBox4, textBox17, textBox18, textBox20);
            cssCon.loadData6(checkBox15, textBox25);
            peopleCanShuSetPower();

            this.listView2.ColumnClick += new ColumnClickEventHandler(listView_ColumnClick);
            this.listView3.ColumnClick += new ColumnClickEventHandler(listView_ColumnClick);
            this.listView4.ColumnClick += new ColumnClickEventHandler(listView_ColumnClick);
            this.listView5.ColumnClick += new ColumnClickEventHandler(listView_ColumnClick);
            this.listView6.ColumnClick += new ColumnClickEventHandler(listView_ColumnClick);
        }

        private void peopleCanShuSetPower()
        {
            if (!PeoplePowerModel.getPeoplePowerModel().isCanshuChange())
            {
                ButtonVisibistFalse(button4);
                ButtonVisibistFalse(button5);
                ButtonVisibistFalse(button6);
                ButtonVisibistFalse(button7);
                ButtonVisibistFalse(button8);
                ButtonVisibistFalse(button10);
                ButtonVisibistFalse(button11);
                ButtonVisibistFalse(button12);
                ButtonVisibistFalse(button17);
                ButtonVisibistFalse(button18);
                ButtonVisibistFalse(button19);
                ButtonVisibistFalse(button13);
                ButtonVisibistFalse(button20);
                ButtonVisibistFalse(button23);
                ButtonVisibistFalse(button14);
                ButtonVisibistFalse(button15);
                ButtonVisibistFalse(button16);
                ButtonVisibistFalse(button21);
                ButtonVisibistFalse(button22);
                ButtonVisibistFalse(button1);
                ButtonVisibistFalse(button2);
                ButtonVisibistFalse(button3);
            }
            if (!PeoplePowerModel.getPeoplePowerModel().isAllPower()) 
            {
                if (checkBox14.Enabled) checkBox14.Enabled = false;
            }
        }

        private void ButtonVisibistFalse(Button button) 
        {
            if (button.Enabled) button.Enabled = false;
        }

        //关闭界面时，触发一些设置参数的保存操作
        private void formClosedEventHandler(object sender, FormClosedEventArgs e) 
        {
            FileTcpClienModel.getFileTcpClienMidel().tcpHandle -= tongbuMessage;
            if (!PeoplePowerModel.getPeoplePowerModel().isCanshuChange())
            {
                return;
            }

            int blTime = XWUtils.stringToInt1(textBox17.Text);
            if (blTime > -1) FileModel.getFlModel().ChFlBean.BlackTimeText = blTime;
            if (!checkBox3.Checked) blTime = int.MaxValue;
            if (blTime > -1) FileModel.getFlModel().ChFlBean.BlackTime = blTime;

            int noReTime = XWUtils.stringToInt1(textBox18.Text);
            if (noReTime > -1) FileModel.getFlModel().ChFlBean.NoReveTimeText = noReTime;
            if (!checkBox4.Checked) noReTime = int.MaxValue;
            if (noReTime > -1) FileModel.getFlModel().ChFlBean.NoReveTime = noReTime;

            int checkC = XWUtils.stringToInt1(textBox20.Text);
            if (checkC > -1) FileModel.getFlModel().ChFlBean.CheckCText = checkC;
            if (!checkBox5.Checked) checkC = 0;
            if (checkC > -1) FileModel.getFlModel().ChFlBean.CheckC = checkC;

            int tagLowInt = XWUtils.stringToInt1(textBox25.Text);
            if (tagLowInt > -1) FileModel.getFlModel().ChFlBean.TagLowText = tagLowInt;
            if (!checkBox15.Checked) tagLowInt = -1;
            if (tagLowInt > -2) FileModel.getFlModel().ChFlBean.TagLow = tagLowInt;

            FileModel.getFlModel().setCFData();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PeoplePowerModel.getPeoplePowerModel().Jurisdiction == PeoplePowerModel.getPeoplePowerModel().CongjiValue)
            {
                return;
            }
            int index = cssCon.setTabPageFromList(sender, tabControl1);
            if (index == 5) cssCon.loadData3(listView5, null);
            else if (index == 2) cssCon.loadData(listView3, listView4, comboBox2, comboBox4, comboBox5);
        }       

        private void button5_Click(object sender, EventArgs e) //添加組別
        {
            textBox5.Text = "0000";
        }

        private void button4_Click(object sender, EventArgs e) //刪除組別
        {
            if (listView3.SelectedItems.Count < 1) return;
            cssCon.removeItem(listView3, listView4, listView3.SelectedItems[0], CenQuType.CENGJI);
            cssCon.loadData(null, listView4, comboBox2, comboBox4, comboBox5);
        }

        /// <summary>
        /// 删除掉ListView中某个Item
        /// </summary>
        /// <param name="sourLView">要操作的ListView对象</param>
        /// <param name="deleItem">删除的Item</param>
        public void deleteListViewItem(ListView sourLView, ListViewItem deleItem) 
        {
            try 
            {
                sourLView.Items.Remove(deleItem);
            }
            catch(Exception e) { }
        }

        private void button6_Click(object sender, EventArgs e) //组别更新
        {
            if (!dealNameChongFu(textBox5, textBox4, 1)) return;
            cssCon.CenJiUpdate(textBox4, textBox5, listView3);
            cssCon.loadData(null, listView4, comboBox2, comboBox4, comboBox5);
            cssCon.changeNodeCenjiID(textBox5.Text);
            cssCon.loadData3(listView5, null);
        }

        private void keyNumberPress(object sender, KeyPressEventArgs e) //KeyPass按鍵設置
        {
            cssCon.justNumberInput(e);
        }

        private void keyTenSexNumberPress(object sender, KeyPressEventArgs e) //KeyPass按鍵設置
        {
            XWUtils.justTenSixNumberInput(e);
        }

        private void button8_Click(object sender, EventArgs e) //添加区域
        {
            textBox6.Text = "0000";
        }

        private void button7_Click(object sender, EventArgs e) //删除区域
        {
            if (listView4.SelectedItems.Count < 1) return;
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            //"确定要退出吗？"是对话框的显示信息，"退出系统"是对话框的标题
            //默认情况下，如MessageBox.Show("确定要退出吗？")只显示一个“确定”按钮。
            DialogResult dr = MessageBox.Show("該區域下所有節點都將刪除，確定要刪除嗎?", "刪除區域", messButton);
            if (dr == DialogResult.OK)//如果点击“确定”按钮
            {
                cssCon.removeItem(listView4, null, listView4.SelectedItems[0], CenQuType.QUYU);
            }            
        }

        private void button10_Click(object sender, EventArgs e) //区域更新
        {
            if (!dealNameChongFu(textBox6, textBox7, 2)) return;
            cssCon.quYuUpdate(textBox6, textBox7, textBox9, comboBox2, listView4);
            cssCon.changeNodeQuYuID(textBox6.Text);
            cssCon.loadData3(listView5, null);
        }

        private void listView4_SelectedIndexChanged(object sender, EventArgs e) 
        {
            if (listView4.SelectedItems.Count < 1) return;
            cssCon.quYuListSelect(listView4.SelectedItems[0], textBox6, textBox7, textBox9, comboBox2);
        }

        public void textBoxChange(Control con, string text)
        {
            if (con == null) return;
            con.Text = text;
        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count < 1) return;
            cssCon.cenJiListSelect(listView3.SelectedItems[0], textBox5, textBox4);
        }

        private void button11_Click(object sender, EventArgs e) //存储本机IP
        {
            cssCon.cacheLocationIP(comboBox3.Text,1);
        }

        private void button12_Click(object sender, EventArgs e) //存储本机端口
        {
            cssCon.cacheLocationPort(textBox8.Text,1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!dealNameChongFu(textBox1, textBox2, 0)) return;
            cssCon.peopleUpdate(textBox1, textBox2, textBox3, comboBox1, listView2, getPeopleValue());
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
        private bool dealNameChongFu(TextBox tb1, TextBox tb2, int type) 
        {
            StringBuilder suder = new StringBuilder();
            switch (type)
            {
                case 0:
                    suder.Append("帳號");
                    break;
                case 1:
                    suder.Append("組名");
                    break;
                case 2:
                    suder.Append("區域名稱");
                    break;
                case 3:
                    suder.Append("名稱");
                    break;
                case 4:
                    suder.Append("卡片名稱");
                    break;
            }
            suder.Append("重複，操作失敗");
            try 
            {
                bool isHace = cssCon.nameUpdate(tb1, tb2, type);
                if (!isHace)
                    MessageBox.Show(suder.ToString());
                return isHace;
            }
            catch { }
            return true;
        }

        private int getPeopleValue() 
        {
            int value = 0;
            if (checkBox6.Checked) value |= PeoplePowerModel.getPeoplePowerModel().ExeOC;
            if (checkBox7.Checked) value |= PeoplePowerModel.getPeoplePowerModel().Monitor;
            if (checkBox8.Checked) value |= PeoplePowerModel.getPeoplePowerModel().CanshuOC;
            if (checkBox9.Checked) value |= PeoplePowerModel.getPeoplePowerModel().WarnDeal;
            if (checkBox10.Checked) value |= PeoplePowerModel.getPeoplePowerModel().SerchTag;
            if (checkBox11.Checked) value |= PeoplePowerModel.getPeoplePowerModel().LocationShow;
            if (checkBox12.Checked) value |= PeoplePowerModel.getPeoplePowerModel().CanshuChange;
            if (checkBox13.Checked) value |= PeoplePowerModel.getPeoplePowerModel().HisData;
            if (checkBox14.Checked) value |= PeoplePowerModel.getPeoplePowerModel().PeopleOperation;
            return value;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "0000";
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count < 1) return;
            cssCon.peopleListSelect(listView2.SelectedItems[0], textBox1, textBox2, textBox3, comboBox1);
            if (listView2.SelectedItems[0].Tag is int) 
            {
                int peopleValue = (int)listView2.SelectedItems[0].Tag;
                setPeopleValueInCheckBox(peopleValue);
            }
        }

        private void setPeopleValueInCheckBox(int peopleValue)
        {
            checkBox6.Checked = (peopleValue & PeoplePowerModel.getPeoplePowerModel().ExeOC) > 0 ;
            checkBox7.Checked = (peopleValue & PeoplePowerModel.getPeoplePowerModel().Monitor) > 0;
            checkBox8.Checked = (peopleValue & PeoplePowerModel.getPeoplePowerModel().CanshuOC) > 0 ;
            checkBox9.Checked = (peopleValue & PeoplePowerModel.getPeoplePowerModel().WarnDeal) > 0 ;
            checkBox10.Checked = (peopleValue & PeoplePowerModel.getPeoplePowerModel().SerchTag) > 0;
            checkBox11.Checked = (peopleValue & PeoplePowerModel.getPeoplePowerModel().LocationShow) > 0;
            checkBox12.Checked = (peopleValue & PeoplePowerModel.getPeoplePowerModel().CanshuChange) > 0;
            checkBox13.Checked = (peopleValue & PeoplePowerModel.getPeoplePowerModel().HisData) > 0;
            checkBox14.Checked = (peopleValue & PeoplePowerModel.getPeoplePowerModel().PeopleOperation) > 0;
        }

        private void button2_Click(object sender, EventArgs e) //删除人员
        {
            if (listView2.SelectedItems.Count < 1) return;
            cssCon.removePeopleItem(listView2, listView2.SelectedItems[0]);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            cssCon.comItemSelect(comboBox4, comboBox5);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (!dealNameChongFu(textBox11, textBox12, 3)) return;
            cssCon.upDataNode(textBox11,textBox12,textBox13,textBox14,listView5);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            textBox15.Text = "0000";
        }

        private void button16_Click(object sender, EventArgs e) //更新卡片列表
        {
            if (!dealNameChongFu(textBox15, textBox16, 4)) return;
            cssCon.upDataCard(textBox15, textBox16,listView6);
        }

        private void button14_Click(object sender, EventArgs e)//刪除卡片
        {
            if (listView6.SelectedItems.Count < 1) return;
            cssCon.removeCardItem(listView6, listView6.SelectedItems[0]);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog MyFileDialog = new OpenFileDialog();
            MyFileDialog.Filter = "所有圖片文件|*.bmp;*.jpg;*.png";
            if (MyFileDialog.ShowDialog() == DialogResult.OK)
            {              
                textBox9.Text = MyFileDialog.FileName;
                Bitmap bitmap = new Bitmap(MyFileDialog.FileName, true);
                bitmap = new Bitmap(bitmap, panel2.Width, panel2.Height);
                panel2.BackgroundImage = bitmap;
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (panel1.Controls.Count > 0)
                panel1.Controls.Clear();
            cssCon.comItemSelect2(comboBox4, comboBox5,textBox10,panel1);
            if (panel1.Controls.Count < 1) return;
            foreach (Control col in panel1.Controls) 
            {
                if (!(col is CanKaoDianView)) continue;
                CanKaoDianView cView =(CanKaoDianView)col;
                //cView.Click += new System.EventHandler(this.CanKaoDianView_Click);
                cView.ViewEnable();
                cView.MouseDown += new MouseEventHandler(CanKaoDianViewMouseDown);
                cView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(CKDView_mouDouble);
            }            
        }

        private void CanKaoDianViewMouseDown(object sender, MouseEventArgs e)
        {
            if (!(sender is CanKaoDianView)) return;
            CanKaoDianView cView = (CanKaoDianView)sender;
            ControlMoveResize(cView, panel1);
        }
        

        private void CanKaoDianView_Click(object sender, EventArgs e) //存储本机端口
        {
            
        }


        private void mapPel_DoubleClicl(object sender, MouseEventArgs e)
        {
            if (panel1.BackgroundImage == null) return;
            if (comboBox4.Text.Equals("") || comboBox5.Text.Equals("")) return;
            //Graphics gs = panel1.CreateGraphics();  
           
            Point point = new Point(e.X, e.Y);
            SetCanKaoDianFrom sckFrom = new SetCanKaoDianFrom(point,null);
            sckFrom.onSetCanKaoHandle += setCanKAOdian;
            sckFrom.Show();
        }


        /// <summary>
        /// 双击控件发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CKDView_mouDouble(object sender, MouseEventArgs e)
        {
            if (!(sender is CanKaoDianView)) return;
            CanKaoDianView cView = (CanKaoDianView)sender;
            SetCanKaoDianFrom sckFrom = new SetCanKaoDianFrom(cView.Location, cView.CkdID);
            sckFrom.onSetCanKaoHandle += setCanKAOdian;
            sckFrom.Show();
        }


        public void setCanKAOdian(SetCanKaoDianType type, CanKaoDianBean cdBean)
        {
            if (type == SetCanKaoDianType.ADD)
            {
                string mag = "";
                var ckdItem = cssCon.haveCanKaoDian(cdBean, comboBox4.Text, comboBox5.Text);
                
                if (ckdItem == null) 
                {
                    addCkdView(cdBean);
                    return;
                }
                else if (ckdItem.QuYuID.Equals(cssCon.getIDFromKuohao(comboBox5.Text))) mag = "本區域，已經添加該參考點，確定要將參考點移到該位置嗎?";
                else mag = "在區域" + ckdItem.QuYuname + "，已經添加該參考點，確定要將參考點移到該區域嗎?";
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                //"确定要退出吗？"是对话框的显示信息，"退出系统"是对话框的标题
                //默认情况下，如MessageBox.Show("确定要退出吗？")只显示一个“确定”按钮。
                DialogResult dr = MessageBox.Show(mag, "再次添加", messButton);
                if (dr == DialogResult.OK)//如果点击“确定”按钮
                {
                    cssCon.canKaoDianDelete(panel1, cdBean);
                    addCkdView(cdBean);
                }                                           
            }              
            else if (type == SetCanKaoDianType.DELETE)
                cssCon.canKaoDianDelete(panel1, cdBean);
            else return;
        }

        private void addCkdView(CanKaoDianBean cdBean) 
        {
            CanKaoDianView ckdView = cssCon.canKaoDianAdd(panel1, cdBean, comboBox4.Text, comboBox5.Text);
            if (ckdView != null)
            {
                ckdView.ViewEnable();
                ckdView.MouseDown += new MouseEventHandler(CanKaoDianViewMouseDown);
                ckdView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(CKDView_mouDouble);
            }   
        }
        
        private void panel1_Paint(object sender, PaintEventArgs e){}

        private void listView5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView5.SelectedItems.Count < 1) return;
            cssCon.qcanKaodiNListSelect(listView5.SelectedItems[0], textBox11, textBox12, textBox13, textBox14);
        }

        private void listView6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView6.SelectedItems.Count < 1) return;
            cssCon.peopleCardListSelect(listView6.SelectedItems[0], textBox15, textBox16);
        }


        //#region 私有成员
        bool IsMoving = false;
        Point pCtrlLastCoordinate = new Point(0, 0);
        Point pCursorOffset = new Point(0, 0);
        Point pCursorLastCoordinate = new Point(0, 0);
        private Control ctrl = null;
        private ScrollableControl Containe = null;
        //#endregion
        //#region 私有方法
        /// <summary>
        /// 在鼠标左键按下的状态记录鼠标当前的位置,以及被移动组件的当前位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canMouseDown(object sender, MouseEventArgs e)
        {
            if (Containe == null)
            {
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                IsMoving = true;
                pCtrlLastCoordinate.X = ctrl.Left;
                pCtrlLastCoordinate.Y = ctrl.Top;
                pCursorLastCoordinate.X = Cursor.Position.X;
                pCursorLastCoordinate.Y = Cursor.Position.Y;
            }
        }
        private void canMouseMove(object sender, MouseEventArgs e)
        {
            if (Containe == null)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                if (this.IsMoving)
                {
                    Point pCursor = new Point(Cursor.Position.X, Cursor.Position.Y);

                    pCursorOffset.X = pCursor.X - pCursorLastCoordinate.X;

                    pCursorOffset.Y = pCursor.Y - pCursorLastCoordinate.Y;
                    ctrl.Left = pCtrlLastCoordinate.X + pCursorOffset.X;
                    ctrl.Top = pCtrlLastCoordinate.Y + pCursorOffset.Y;
                }
            }
        }

        private void canMouseUp(object sender, MouseEventArgs e)
        {
            if (Containe == null)
            {
                return;
            }
            if (this.IsMoving)
            {
                if (pCursorOffset.X == 0 && pCursorOffset.Y == 0)
                {
                    return;
                }
                if ((pCtrlLastCoordinate.X + pCursorOffset.X + ctrl.Width) > 0)
                {
                    ctrl.Left = pCtrlLastCoordinate.X + pCursorOffset.X;
                }
                else
                {
                    ctrl.Left = 0;
                }
                if ((pCtrlLastCoordinate.Y + pCursorOffset.Y + ctrl.Height) > 0)
                {
                    ctrl.Top = pCtrlLastCoordinate.Y + pCursorOffset.Y;
                }
                else
                {
                    ctrl.Top = 0;
                }
                pCursorOffset.X = 0;
                pCursorOffset.Y = 0;

                ctrl.MouseDown -= new MouseEventHandler(canMouseDown);
                ctrl.MouseMove -= new MouseEventHandler(canMouseMove);
                ctrl.MouseUp -= new MouseEventHandler(canMouseUp);

                if (!(ctrl is CanKaoDianView)) return;
                CanKaoDianView ct = (CanKaoDianView)ctrl;
                cssCon.canKaoDianMove(ctrl.Location, ct.LabText, comboBox4.Text, comboBox5.Text,panel1.Width,panel1.Height);
            }
        }

        //#endregion
        //#region 构造函数
        /// <summary>
        /// 获取被移动控件对象和容器对象
        /// </summary>
        /// <param name="c">被设置为可运行时移动的控件</param>
        /// <param name="parentContain">可移动控件的容器</param>
        public void ControlMoveResize(Control c, ScrollableControl parentContain)
        {
            ctrl = c;
            this.Containe = parentContain;
            ctrl.MouseDown += new MouseEventHandler(canMouseDown);
            ctrl.MouseMove += new MouseEventHandler(canMouseMove);
            ctrl.MouseUp += new MouseEventHandler(canMouseUp);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            FileModel.getFlModel().ChFlBean.ShowCanKaoDian = checkBox1.Checked ;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            FileModel.getFlModel().ChFlBean.ShowJingJiTag = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            FileModel.getFlModel().ChFlBean.ShowBlackTag = checkBox3.Checked;
            //if (!checkBox4.Checked) FileModel.getFlModel().ChFlBean.BlackTime = int.MaxValue;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            FileModel.getFlModel().ChFlBean.NoShowTag = checkBox4.Checked;
            //if (!checkBox4.Checked) FileModel.getFlModel().ChFlBean.NoReveTime = int.MaxValue;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            cssCon.cacheLocationIP(comboBox6.Text, 2);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            cssCon.cacheLocationPort(textBox19.Text, 2);
        }

        public void tcpServerHandle(bool isOpen, CommunicationMode comMode)
        {
            if (comMode == CommunicationMode.TCPServer) 
            {
                if (isOpen) button19.Text = "關閉";
                else button19.Text = "打開TCPServer";
            }
            else if (comMode == CommunicationMode.TCPClien)
            {
                if (isOpen) button24.Text = "關閉";
                else button24.Text = "連接TCPServer";
            }
            else if (comMode == CommunicationMode.TCPClien_File) 
            {
                if (PeoplePowerModel.getPeoplePowerModel().Jurisdiction == PeoplePowerModel.getPeoplePowerModel().CongjiValue)
                {
                    button25.Visible = isOpen;
                }                
            }            
        }

        private void label25_Click(object sender, EventArgs e){}

        private void textBox18_TextChanged(object sender, EventArgs e){}

        private void textBox17_TextChanged(object sender, EventArgs e){}

        private void label22_Click(object sender, EventArgs e){}

        private void label19_Click(object sender, EventArgs e){}

        private void label26_Click(object sender, EventArgs e){}

        private void button20_Click(object sender, EventArgs e)//导出节点
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files   (*.xls)|*.xls";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox21.Text = saveFileDialog1.FileName;
                cssCon.saveNODEXlsFile(saveFileDialog1.FileName);
                new Thread(saveNODEXlsFileThread).Start(saveFileDialog1.FileName);
            } 
        }

        private void saveNODEXlsFileThread(object obj) 
        {
            string filePath = (string)obj;
            cssCon.saveNODEXlsFile(filePath);
        }

        private void button21_Click(object sender, EventArgs e)//导出卡片
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "xls files   (*.xls)|*.xls";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox22.Text = saveFileDialog1.FileName;
                new Thread(saveCardXlsFileThread).Start(saveFileDialog1.FileName);
            } 
        }

        private void saveCardXlsFileThread(object obj)
        {
            string filePath = (string)obj;
            cssCon.saveCardXlsFile(filePath);
        }

        private void button22_Click(object sender, EventArgs e) //导入卡片
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "xls|*.xls|all|*.*";//"xls files   (*.xls)|*.xls |所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName; // 保存文件并获取文件路径
                textBox23.Text = filePath;
                new Thread(importCardXlsFileThread).Start(filePath);
            }         
        }
         
        private void importCardXlsFileThread(object obj)
        {
            string filePath = (string)obj;
            cssCon.importCardXlsFile(filePath);
        }

        private void button23_Click(object sender, EventArgs e) //导入节点
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "xls|*.xls|all|*.*"; //"xls files   (*.xls)|*.xls |所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName; // 保存文件并获取文件路径
                textBox24.Text = filePath;
                new Thread(importNODEXlsFileThread).Start(filePath);
            }
        }
         
        private void importNODEXlsFileThread(object obj)
        {
            string filePath = (string)obj;
            cssCon.importNODEXlsFile(filePath);
        }

        public void refash(RefashList reList, bool isRefesh) 
        {
            this.Invoke((EventHandler)(delegate
            { //放入主線程    
                if (!isRefesh)
                {
                    MessageBox.Show("導入失敗！");
                    return;
                }
                if (reList == RefashList.CANKAODIANLIST)
                    cssCon.loadData3(listView5, null);
                else if (reList == RefashList.CARDLIST)
                    cssCon.loadData3(null, listView6);
                MessageBox.Show("導入成功！");
            }));
            
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox16.Checked) 
            {
                checkBox6.Checked = true;
                checkBox7.Checked = true;
                checkBox8.Checked = true;
                checkBox9.Checked = true;
                checkBox10.Checked = true;
                checkBox11.Checked = true;
                checkBox12.Checked = true;
                checkBox13.Checked = true;
            }
        }

        private void button28_Click(object sender, EventArgs e) //存储主机Ip
        {
            cssCon.cacheLocationIP(comboBox8.Text, 3);
        }

        private void button27_Click(object sender, EventArgs e)//存储主机端口
        {
            cssCon.cacheLocationPort(textBox26.Text, 3);
        }


        private void listView_ColumnClick(object o, ColumnClickEventArgs e)
        {
            if (!(o is ListView)) return;
            ListView listViewPaiXu = (ListView)o;
            if (listViewPaiXu.Columns[e.Column].Tag == null)
            {
                listViewPaiXu.Columns[e.Column].Tag = true;
            }
            bool tabK = (bool)listViewPaiXu.Columns[e.Column].Tag;
            if (tabK)
            {
                listViewPaiXu.Columns[e.Column].Tag = false;
            }
            else
            {
                listViewPaiXu.Columns[e.Column].Tag = true;
            }
            listViewPaiXu.ListViewItemSorter = new ListViewSort(e.Column, listViewPaiXu.Columns[e.Column].Tag);
            //指定排序器并传送列索引与升序降序关键字  
            listViewPaiXu.Sort();//对列表进行自定义排序 
        }

        //排序类的定义:  
        ///  
        ///自定义ListView控件排序函数  
        ///  
        public class ListViewSort : IComparer
        {
            private int col;
            private bool descK;

            public ListViewSort()
            {
                col = 0;
            }
            public ListViewSort(int column, object Desc)
            {
                descK = (bool)Desc;
                col = column; //当前列,0,1,2...,参数由ListView控件的ColumnClick事件传递  
            }
            public int Compare(object x, object y)
            {
                int tempInt = String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
                if (descK)
                {
                    return -tempInt;
                }
                else
                {
                    return tempInt;
                }
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            FileTcpClienModel.getFileTcpClienMidel().tcpHandle -= tongbuMessage; //此處是為了防止點擊一次，則創建一次tongbuMessage
            FileTcpClienModel.getFileTcpClienMidel().tcpHandle += tongbuMessage;
            FileTcpClienModel.getFileTcpClienMidel().startFile();
        }

        private void tongbuMessage(string msg)
        {
            this.Invoke((EventHandler)(delegate
            { //放入主線程    
                if (!label29.Visible) label29.Visible = true;
                label29.Text = msg;
            }));           
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            FileModel.getFlModel().ChFlBean.ShowTagLow = checkBox15.Checked;
        }

    }
}
