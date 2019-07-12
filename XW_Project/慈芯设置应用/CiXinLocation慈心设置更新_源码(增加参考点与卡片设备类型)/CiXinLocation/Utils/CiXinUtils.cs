using CiXinLocation.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            else if (dtType == DrivaceType.LOCATION) mainType = 0x02;
            else if (dtType == DrivaceType.TAG) mainType = 0x03;
            else if (dtType == DrivaceType.USB_DANGLE) mainType = 0x04;
            return getSunTypeName(mainType, type);
        }

        public static string getSunCardTypeName(byte mainType, byte type)
        {
            string msg = "";
            if (0x03 == mainType)
            {
                if (type == 0x01) msg += "-SL03";
                else if (type == 0x02) msg += "-PTAG_9056";
                else if (type == 0x03) msg += "-PTAG_H";
                else if (type == 0x04) msg += "-PTAG_5136";
            }
            return msg;
        }

        public static string getSunCankaodianTypeName(byte mainType, byte type)
        {
            string msg = "";
            if (0x02 == mainType)
            {
                if (type == 0x01) msg += "-ZB2530_01PA";
                else if (type == 0x02) msg += "-ZB2530_03";
                else if (type == 0x03) msg += "-ZB2530_LAN/WIFI_04";
                else if (type == 0x04) msg += "-ZB2530_04";
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
                else if (type == 0x03) msg += "-ZB2530_LAN/WIFI_04_lan";
                else if (type == 0x04) msg += "-ZB2530_LAN/WIFI_04_wifi";
            }
            return msg;
        }

        public static byte getSunType(string sunTypeString)
        {
            if (sunTypeString.Equals("-SL03") || sunTypeString.Equals("-ZB2530_01PA")) return 0x01;
            else if (sunTypeString.Equals("-PTAG_9056") || sunTypeString.Equals("-ZB2530_03") || sunTypeString.Equals("-ZB2530_LAN")) return 0x02;
            else if (sunTypeString.Equals("-PTAG_H") || sunTypeString.Equals("-ZB2530_LAN/WIFI_04_lan")) return 0x03;
            else if (sunTypeString.Equals("-ZB2530_LAN/WIFI_04_wifi") || sunTypeString.Equals("-PTAG_5136") || sunTypeString.Equals("-ZB2530_04"))                                                  return 0x04;
            else return 0x00;
        }


    }
}
