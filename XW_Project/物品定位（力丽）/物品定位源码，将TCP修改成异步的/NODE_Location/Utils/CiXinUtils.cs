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
    class CiXinUtils
    {

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

        /// <summary>
        /// 文件信息，年月日版本和设备类型
        /// </summary>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string VersionMessage(byte[] version, byte[] type)
        {
            string versionMessage = "";
            if (version == null || type == null || version.Length != 4 || type.Length < 2) return versionMessage;
            int year = version[0] + 2000;
            byte month = version[1];
            byte day = version[2];
            byte ver = version[3];
            return year.ToString() + StringUtils.year + month.ToString() + StringUtils.month + day.ToString()
                + StringUtils.day + "V" + (ver / 16).ToString("X") + "." + (ver % 16).ToString("X") + "，"
                + StringUtils.driType + "：" + getTypeName(type[1], type[0]);
        }


        public static string getTypeName(byte mainType, byte type)
        {
            string msg = "";
            if (0x04 == mainType)
            {
                msg = StringUtils.USB_Dangle;
            }
            else if (0x03 == mainType)
            {
                msg = StringUtils.card;
                msg += getSunCardTypeName(mainType, type);
            }
            else if (0x02 == mainType)
            {
                msg = StringUtils.cankaodian;
                msg += getSunCankaodianTypeName(mainType, type);
            }
            else if (0x01 == mainType)
            {
                msg = StringUtils.node;
                msg += getSunNODETypeName(mainType, type);
            }
            else if (StringUtils.ceTable != null)
            {
                msg = StringUtils.ceTable.ContainsKey(msg) ? (string)StringUtils.ceTable[msg] : msg; //三目运算。若指定转英文，则英文显示，否则该干嘛干嘛
            }
            return msg;
        }

        /// <summary>
        /// 获取子设备类型的字符串
        /// </summary>
        /// <param name="mainType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string getSunTypeName(byte mainType, byte type)
        {
            if (mainType == 0x01) return getSunNODETypeName(0x01, type);
            else if (mainType == 0x02) return getSunCankaodianTypeName(0x02, type);
            else if (mainType == 0x03) return getSunCardTypeName(0x03, type);
            else return "";
        }


        public static string getSunTypeName(DrivaceType dtType, byte type)
        {
            byte mainType = 0;
            if (dtType == DrivaceType.NODE) mainType = 0x01;
            else if (dtType == DrivaceType.CANKAODIAN) mainType = 0x02;
            else if (dtType == DrivaceType.CARD) mainType = 0x03;
            else if (dtType == DrivaceType.USB_DANGLE) mainType = 0x04;
            return getSunTypeName(mainType, type);
        }

        public static string getSunCardTypeName(byte mainType, byte type)
        {
            string msg = "";
            if (0x03 == mainType)
            {
                if (type == 0x01) msg += "-SL03";
                if (type == 0x02) msg += "-PTAG_9056";
                if (type == 0x03) msg += "-PTAG_H";
                if (type == 0x04) msg += "-PTAG_7045";
                if (type == 0x05) msg += "-PTAG_8141";
                if (type == 0x06) msg += "-ETAG_E290";
            }
            return msg;
        }

        public static string getSunCankaodianTypeName(byte mainType, byte type)
        {
            string msg = "";
            if (0x02 == mainType)
            {
                if (type == 0x01) msg += "-ZB2530_01PA";
                if (type == 0x02) msg += "-ZB2530_03";
            }
            return msg;
        }

        public static string getSunNODETypeName(byte mainType, byte type)
        {
            string msg = "";
            if (0x01 == mainType)
            {
                if (type == 0x01) msg += "-ZB2530_01PA";
                else if (type == 0x02) msg += "-ZB2530_LAN";
                else if (type == 0x03) msg += "-ZB2530_03SV1.0";
            }
            return msg;
        }

        public static byte getSunType(string sunTypeString)
        {
            if (sunTypeString.Equals("-SL03") || sunTypeString.Equals("-ZB2530_01PA")) return 0x01;
            else if (sunTypeString.Equals("-PTAG_9056") || sunTypeString.Equals("-ZB2530_03") || sunTypeString.Equals("-ZB2530_LAN")) return 0x02;
            else if (sunTypeString.Equals("-PTAG_H") || sunTypeString.Equals("-ZB2530_03SV1.0")) return 0x03;
            else if (sunTypeString.Equals("-PTAG_7045")) return 0x04;
            else if (sunTypeString.Equals("-PTAG_8141")) return 0x05;
            else if (sunTypeString.Equals("-ETAG_E290")) return 0x06;
            else return 0x00;
        }

    }

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

        public static string getTypeName(byte mainType, byte type)
        {
            string msg = "";
            if (0x04 == mainType)
            {
                msg = StringUtils.USB_Dangle;
            }
            else if (0x03 == mainType)
            {
                msg = StringUtils.card;
                msg += getSunCardTypeName(mainType, type);
            }
            else if (0x02 == mainType)
            {
                msg = StringUtils.cankaodian;
                msg += getSunCankaodianTypeName(mainType, type);
            }
            else if (0x01 == mainType)
            {
                msg = StringUtils.node;
                msg += getSunNODETypeName(mainType, type);
            }
            else if (StringUtils.ceTable != null)
            {
                msg = StringUtils.ceTable.ContainsKey(msg) ? (string)StringUtils.ceTable[msg] : msg; //三目运算。若指定转英文，则英文显示，否则该干嘛干嘛
            }
            return msg;
        }

        /// <summary>
        /// 获取子设备类型的字符串
        /// </summary>
        /// <param name="mainType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string getSunTypeName(byte mainType, byte type)
        {
            if (mainType == 0x01) return getSunNODETypeName(0x01, type);
            else if (mainType == 0x02) return getSunCankaodianTypeName(0x02, type);
            else if (mainType == 0x03) return getSunCardTypeName(0x03, type);
            else return "";
        }


        public static string getSunTypeName(DrivaceType dtType, byte type)
        {
            byte mainType = 0;
            if (dtType == DrivaceType.NODE) mainType = 0x01;
            else if (dtType == DrivaceType.CANKAODIAN) mainType = 0x02;
            else if (dtType == DrivaceType.CARD) mainType = 0x03;
            else if (dtType == DrivaceType.USB_DANGLE) mainType = 0x04;
            return getSunTypeName(mainType, type);
        }

        public static string getSunCardTypeName(byte mainType, byte type)
        {
            string msg = "";
            if (0x03 == mainType)
            {
                if (type == 0x01) msg += "-SL03";
                if (type == 0x02) msg += "-PTAG_9056";
                if (type == 0x03) msg += "-PTAG_H";
                if (type == 0x04) msg += "-PTAG_7045";
                if (type == 0x05) msg += "-PTAG_8141";
                if (type == 0x06) msg += "-ETAG_E290";
            }
            return msg;
        }

        public static string getSunCankaodianTypeName(byte mainType, byte type)
        {
            string msg = "";
            if (0x02 == mainType)
            {
                if (type == 0x01) msg += "-ZB2530_01PA";
                if (type == 0x02) msg += "-ZB2530_03";
            }
            return msg;
        }

        public static string getSunNODETypeName(byte mainType, byte type)
        {
            string msg = "";
            if (0x01 == mainType)
            {
                if (type == 0x01) msg += "-ZB2530_01PA";
                if (type == 0x02) msg += "-ZB2530_LAN";
            }
            return msg;
        }

        public static byte getSunType(string sunTypeString)
        {
            if (sunTypeString.Equals("-SL03") || sunTypeString.Equals("-ZB2530_01PA")) return 0x01;
            else if (sunTypeString.Equals("-PTAG_9056") || sunTypeString.Equals("-ZB2530_03") || sunTypeString.Equals("-ZB2530_LAN")) return 0x02;
            else if (sunTypeString.Equals("-PTAG_H")) return 0x03;
            else if (sunTypeString.Equals("-PTAG_7045")) return 0x04;
            else if (sunTypeString.Equals("-PTAG_8141")) return 0x05;
            else if (sunTypeString.Equals("-ETAG_E290")) return 0x06;
            else return 0x00;
        }

    }
}
