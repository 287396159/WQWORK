using CiXinLocation.bean;
using CiXinLocation.Model;
using SerialportSample;
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
    public partial class SetCanKaoDianFrom : Form
    {

        public delegate void SetCanKaoHandle(SetCanKaoDianType type, CanKaoDianBean cdBean);
        public SetCanKaoHandle onSetCanKaoHandle;
        private Point point;
        private byte[] ID;

        public SetCanKaoDianFrom(Point point,byte[] ID)
        {
            InitializeComponent();
            this.point = point;
            this.ID = ID;
        }

        private void setText() 
        {
            if(ID == null || ID.Length != 2) return;
            textBox1.Text = ID[0].ToString("X2");
            textBox2.Text = ID[1].ToString("X2");
        }

        private void SetCanKaoDian_Load(object sender, EventArgs e)
        {
            setText();
        }

        private void setCanKAOdian(SetCanKaoDianType type, CanKaoDianBean cdBean) 
        {
            if (onSetCanKaoHandle != null) 
            {
                onSetCanKaoHandle(type, cdBean);
            }
        }

        private void button1_Click(object sender, EventArgs e) //添加
        {
            getCanKaoDianBean(SetCanKaoDianType.ADD);
        }

        private void numberKeyPress(object sender, KeyPressEventArgs e)
        {
            XWUtils.justTenSixNumberInput(e);
            if (e.KeyChar == 13) 
            {
                getCanKaoDianBean(SetCanKaoDianType.ADD);
            }
        }


        private void button2_Click(object sender, EventArgs e) //删除
        {
            getCanKaoDianBean(SetCanKaoDianType.DELETE);
        }

        private void  getCanKaoDianBean(SetCanKaoDianType type) 
        {
            if (textBox1.Text.Equals("") || textBox2.Text.Equals("")) return;
            int cID1 = XWUtils.hexStrToInt1(textBox1.Text);
            int cID2 = XWUtils.hexStrToInt1(textBox2.Text);
            if (cID1 == -1 || cID2 == -1 || cID1 > 255 || cID2 > 255)
            {
                MessageBox.Show("輸入的ID有誤，ID取值範圍為01到FF");
                return;
            }

            CanKaoDianBean cdBean = new CanKaoDianBean();
            cdBean.POint = point;
            byte[] idByte = new byte[2];
            idByte[0] = (byte)cID1;
            idByte[1] = (byte)cID2;
            cdBean.CanDianID = idByte;
            cdBean.Id = idByte[0].ToString("X2") + idByte[1].ToString("X2");
            cdBean.Name = cdBean.Id;
            List<CanKaoDianBean> canKaoBns = FileModel.getFlModel().ChFlBean.CanKaoDians.ToList();
            foreach (CanKaoDianBean ckdBean in canKaoBns)
            {
                if (ckdBean.Id.Equals(cdBean.Id))
                {
                    cdBean.Name = ckdBean.Name;
                    break;
                }               
            }
            
            setCanKAOdian(type, cdBean);
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            setCanKAOdian(SetCanKaoDianType.RETURN, null);
        }

        private void IDLength(object sender, EventArgs e)
        {
            if (textBox1.Text.Length >= 2) textBox2.Select();
        }


    }
}
