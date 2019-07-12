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
    public partial class UpdateVerifyWin : Form
    {
        public UpdateVerifyWin()
        {
            InitializeComponent();
        }

        private void okbtn_Click(object sender, EventArgs e)
        {
            string strpsw = pswtxt.Text;
            if (strpsw.ToUpper().Equals(ConstInfor.dmatekpsw.ToUpper()))
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }

        }
    }
}
