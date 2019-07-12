using CiXinLocation.bean;
using CiXinLocation.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CiXinLocation
{
    public partial class AllViewShow : Form
    {

        private CenJiBean selectCenJiBean;//當前選中的層級
        public LocationViewFrom locaViewFrom;
        private CanKaoDianBean canKaoDian;

        public CanKaoDianBean CanKaoDian
        {
            get { return canKaoDian; }
            set { canKaoDian = value; }
        }
        private string tagID;

        public string TagID
        {
            get { return tagID; }
            set { tagID = value; }
        }

        public AllViewShow()
        {
            locaViewFrom = new LocationViewFrom();
            locaViewFrom.serchViewVisi(false);            
            InitializeComponent();
            loadLocationViewFrom();
        }

        private void AllViewShow_Load(object sender, EventArgs e)
        {
            loadDataCom();
            
            serchData();
            serchCard();
        }

        //将搜寻结果，赋值到相应对框中
        public void serchData() 
        {
            serchData(canKaoDian, tagID);
        }

        public  void serchData(CanKaoDianBean canKaoDian, string tagID)
        {
            if (CanKaoDian == null || tagID == null) return;
            textBox1.Text = tagID;

            StringBuilder buder = new StringBuilder();
            buder.Append(CanKaoDian.CenJiname);
            buder.Append("(");
            buder.Append(CanKaoDian.CenJiID);
            buder.Append(")");

            comboBox1.Text = buder.ToString();

            StringBuilder quyu = new StringBuilder();
            quyu.Append(CanKaoDian.QuYuname);
            quyu.Append("(");
            quyu.Append(CanKaoDian.QuYuID);
            quyu.Append(")");

            comboBox2.Text = quyu.ToString();
        }

        public CenJiBean SelectCenJiBean
        {
            get { return selectCenJiBean; }
            set { selectCenJiBean = value; }
        }


        //加載界面
        private void loadLocationViewFrom() 
        {
            locaViewFrom.TopLevel = false;
            locaViewFrom.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            locaViewFrom.WindowState = FormWindowState.Normal;
            setDataForFrom(selectCenJiBean);
            panel1.Controls.Add(locaViewFrom);
            locaViewFrom.showDataType(3); // 3代表的是这个界面的内容，locaViewFrom中，意思是显示全部信息
            

            locaViewFrom.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top;
            locaViewFrom.SelectBool = checkBox1.Checked;
            locaViewFrom.addAllData();
            locaViewFrom.setCardID(textBox1.Text);
            locaViewFrom.setDataLoad();
            locaViewFrom.Show();
            locaViewFrom.loadDataInAllViewFrom();
        }

        /// <summary>
        /// 為From添加屬性
        /// </summary>
        /// <param name="selectCenJiBean"></param>
        private void setDataForFrom(CenJiBean selectCenJiBean) 
        {
            if (SelectCenJiBean == null) return;
            setDataForFrom(selectCenJiBean.ID, selectCenJiBean.QuYuBeans[0]);
        }

        private void setDataForFrom(string cengjiID, QuYuBean quyu)
        {
           // if (SelectCenJiBean == null) return;
            if (quyu != null) locaViewFrom.QUYUBean = quyu;
            locaViewFrom.CkdBeans = FileModel.getFlModel().ChFlBean.CanKaoDians;
            locaViewFrom.CengjiID = cengjiID;
        }

        public void loadDataCom()
        {
            loadDataCom(comboBox1, comboBox2);
         }
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void loadDataCom(ComboBox cbCenJi2, ComboBox cbQuYu)
        {
            List<CenJiBean> cLit = FileModel.getFlModel().CenJiData;
            if (cLit == null || cLit.Count < 1) return;

            cbCenJi2.Items.Clear();
            cbQuYu.Items.Clear();

            foreach (CenJiBean cjItem in cLit)
            {
                cbCenJi2.Items.Add(getCenJibuder(cjItem).ToString());
                if (cjItem.QuYuBeans == null) continue;
                foreach (QuYuBean quYuItem in cjItem.QuYuBeans)
                {
                    cbQuYu.Items.Add(getquyuNameId(quYuItem));
                }
            }
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

        private StringBuilder getquyuNameId(QuYuBean quYuItem)
        {
            StringBuilder buder = new StringBuilder();
            buder.Append(quYuItem.QuyuName);
            buder.Append("(");
            buder.Append(quYuItem.QuyuID);
            buder.Append(")");
            return buder;
        }

        public void comItemSelect(ComboBox comScour, ComboBox com)
        {
            if (comScour.Text == null || comScour.Text.Length < 1) return;

            List<CenJiBean> cLit = FileModel.getFlModel().CenJiData;
            if (cLit == null || cLit.Count < 1) return;
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

        /// <summary>
        /// 从括号中提取层级ID号
        /// </summary>
        /// <returns></returns>
        public string getIDFromKuohao(string cejiText)
        {
            if (cejiText.Equals("") || cejiText == null) return "";
            int start = cejiText.IndexOf('(');
            int end = cejiText.IndexOf(')');
            return cejiText.Substring(start + 1, end - start - 1);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comItemSelect(comboBox1, comboBox2);
        }

        public void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {            
            string cenjiID = getIDFromKuohao(comboBox1.Text);
            string quyuID = getIDFromKuohao(comboBox2.Text);
            loadNewImage(cenjiID, quyuID);
            locaViewFrom.SelectBool = checkBox1.Checked;
            locaViewFrom.addAllData();
        }

        private void loadNewImage(string cenjiID, string quyuID) 
        {
            List<CenJiBean> cLit = FileModel.getFlModel().CenJiData;
            var cenji = cLit.Where(cItem => { return cItem.ID.Equals(cenjiID); });
            if (cenji.Count() == 0) return;

            var cenJiItem = cenji.First();
            var quItem = cenJiItem.QuYuBeans.Where(item => item.QuyuID.Equals(quyuID));
            if (quItem.Count() > 0)
            {
                setDataForFrom(cenJiItem.ID, quItem.First());
                locaViewFrom.addLoadData();
            }    
        }

        /// <summary>
        /// 搜寻卡片
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public void serchCard() 
        {
            if (canKaoDian != null) 
            {
                loadNewImage(canKaoDian.CenJiID, canKaoDian.QuYuID);
            } 
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            locaViewFrom.SelectBool = checkBox1.Checked;
            locaViewFrom.addAllData();
        }

        public delegate void serchCardHandle(string cardID);
        public serchCardHandle serchCardAllView;


        public void tagSerchHandle(bool result)
        {
            if (!result && serch) 
            {
                MessageBox.Show("搜索失敗！");
                serch = false;
            } 
        }

        private bool serch = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if (!PeoplePowerModel.getPeoplePowerModel().isSerchTag())
            {
                MessageBox.Show("您無此權限");
                return;
            }

            if (textBox1.Text.Length < 1) return;
            serch = true;
            if (serchCardAllView != null) serchCardAllView(textBox1.Text);        
            locaViewFrom.setCardID(textBox1.Text);
        }

        public void close() 
        {
            serch = false;
            locaViewFrom.Close();
            locaViewFrom = null;
            canKaoDian = null;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
