using CiXinLocation.Utils;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CiXinLocation.bean
{
    class Loca_NodeUtils
    {

        public static string getDrivaceNODEType(byte[] TYPE) 
        {
            if (TYPE == null || TYPE.Length != 4) return "";
            if (TYPE[2] != 1) return "";
            StringBuilder suder = new StringBuilder();
            suder.Append(TYPE[0] / 0x10 + 2016);
            suder.Append("年");
            suder.Append(TYPE[0] % 0x10);
            suder.Append("月");
            suder.Append(TYPE[1]);
            suder.Append("日");
            if(TYPE[3] == 1)
                suder.Append("ZB2530-WIFI_V1.0");
            else if(TYPE[3] == 2)
                suder.Append("ZB2530-LAN_V02.02");
            return suder.ToString();
        }

        /// <summary>
        /// 版本信息，主要有年月日和版本
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string getVersion(byte[] version)
        {
            if (version == null || version.Length != 4) return "";
            int year = version[0] + 6;
            byte month = version[1];
            byte day = version[2];
            byte ver = version[3];
            return year.ToString() + StringUtils.year + month.ToString() + StringUtils.month + day.ToString()
                + StringUtils.day + "V" + (ver / 16).ToString("X") + "." + (ver % 16).ToString("X");
        }

        /// <summary>
        /// 版本信息，主要有年月日和版本
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string getWuPinLocaVersion(byte[] version)
        {
            if (version == null || version.Length != 4) return "";
            int year = version[0] + 2000;
            byte month = version[1];
            byte day = version[2];
            byte ver = version[3];
            return year.ToString() + StringUtils.year + month.ToString() + StringUtils.month + day.ToString()
                + StringUtils.day + " V" + (ver / 16).ToString("") + "." + (ver % 16).ToString("");
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

        public static List<Color[]> getColors() 
        {
            List<Color[]> colors = new List<Color[]>();
            Color[] colo = new Color[2];
            colo[0] = Color.FromArgb(104, 75, 59);
            colo[1] = Color.FromArgb(192, 137, 107);
            colors.Add(colo);
            colo = new Color[2];
            colo[0] = Color.FromArgb(52, 81, 97);
            colo[1] = Color.FromArgb(92, 148, 175);
            colors.Add(colo);
            colo = new Color[2];
            colo[0] = Color.FromArgb(54, 92, 71);
            colo[1] = Color.FromArgb(99, 167, 130);
            colors.Add(colo);
            colo = new Color[2];
            colo[0] = Color.FromArgb(86, 105, 59);
            colo[1] = Color.FromArgb(157, 192, 108);
            colors.Add(colo);
            colo = new Color[2];
            colo[0] = Color.FromArgb(51, 54, 95);
            colo[1] = Color.FromArgb(92, 100, 175);
            colors.Add(colo);
            colo = new Color[2];
            colo[0] = Color.FromArgb(78, 53, 92);
            colo[1] = Color.FromArgb(142, 100, 166);
            colors.Add(colo);
            colo = new Color[2];
            colo[0] = Color.FromArgb(60, 105, 100);
            colo[1] = Color.FromArgb(109, 192, 182);
            colors.Add(colo);
            colo = new Color[2];
            colo[0] = Color.FromArgb(95, 92, 52);
            colo[1] = Color.FromArgb(175, 167, 92);
            colors.Add(colo);
            colo = new Color[2];
            colo[0] = Color.FromArgb(91, 53, 64);
            colo[1] = Color.FromArgb(166, 99, 117);
            colors.Add(colo);
            return colors;
        }

    }
}
