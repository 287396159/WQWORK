using CiXinLocation.Utils;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CiXinLocation.bean
{
    class Loca_NodeUtils
    {

        public static ListViewItem getlvItem(LocationNode loca_node)
        {
            if (loca_node == null) return null;
            if (loca_node.ID_C == null || loca_node.ID_J == null) return null;
            ListViewItem lvItem = new ListViewItem();
            lvItem.SubItems[0].Text = loca_node.ID_C_str;
            lvItem.SubItems.Add(loca_node.ID_J_str);
            lvItem.SubItems.Add(loca_node.Version_str);
            lvItem.SubItems.Add(XwDataUtils.dataFromTimeStamp(loca_node.TimeStamp));
            return lvItem;
        }

        /// <summary>
        /// 版本信息，主要有年月日和版本
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string getVersion(byte[] version)
        {
            if (version == null || version.Length != 4) return "";
            int year = version[0] + 2000;
            byte month = version[1];
            byte day = version[2];
            byte ver = version[3];
            return year.ToString() + StringUtils.year + month.ToString() + StringUtils.month + day.ToString()
                + StringUtils.day + "V" + (ver / 16).ToString("X") + "." + (ver % 16).ToString("X");
        }

        public static string getByteHex(byte[] data)
        {
            if (data == null) return "";
            StringBuilder strBuder = new StringBuilder();
            foreach (byte item in data)
            {
                strBuder.Append(item.ToString("X2"));
                strBuder.Append(" ");
            }
            return strBuder.ToString();
        }

    }
}
