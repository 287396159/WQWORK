package com.dmatek.utils;

import com.dmatek.setting.enums.DrivaceType;

public class DrivaceInfomation {
    /// <summary>
    /// 版本信息，主要有年月日和版本
    /// </summary>
    /// <param name="version"></param>
    /// <returns></returns>
    public static String getVersion(byte[] version)
    {
        if (version == null || version.length != 4) return "";
        int year = version[0] + 2000;
        byte month = version[1];
        byte day = version[2];
        byte ver = version[3];

        return Integer.toString(year) + "年" + Integer.toString(month) + "月" + Integer.toString(day)
                + "日 V" +  XWUtils.getHexString(ver / 16,1) + "." +
                XWUtils.getHexString(ver % 16,1);
    }

    /// <summary>
    /// 文件信息，年月日版本和设备类型
    /// </summary>
    /// <param name="version"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static String VersionMessage(byte[] version, byte[] type)
    {
        String versionMessage = "";
        if (version == null || type == null || version.length != 4 || type.length < 2) return versionMessage;
        int year = version[0] + 2000;
        byte month = version[1];
        byte day = version[2];
        byte ver = version[3];
        return Integer.toString(year) + "年" + Integer.toString(month) + "月" + Integer.toString(day)
                + "日 V" +  XWUtils.getHexString(ver / 16,1) + "." +
            XWUtils.getHexString(ver % 16,1) + "，"
                 + "設備類型：" + getTypeName(type[2], type[3]);
    }

    public static String getTypeName(byte[] type)
    {
        if (type == null) return "";
        return getTypeName(type[2], type[3]);
    }

    public static String getTypeName(byte mainType, byte type)
    {
        String msg = "";
        if (0x04 == mainType)
        {
            msg = "USB_Dangle";
        }
        else if (0x03 == mainType)
        {
            msg = "卡片";
            msg += getSunCardTypeName(mainType, type);
        }
        else if (0x02 == mainType)
        {
            msg = "參考點";
            msg += getSunCankaodianTypeName(mainType, type);
        }
        else if (0x01 == mainType)
        {
            msg = "節點";
            msg += getSunNODETypeName(mainType, type);
        }
        return msg;
    }

    /// <summary>
    /// 获取子设备类型的字符串
    /// </summary>
    /// <param name="mainType"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static String getSunTypeName(byte mainType, byte type)
    {
        if (mainType == 0x01) return getSunNODETypeName(mainType, type);
        else if (mainType == 0x02) return getSunCankaodianTypeName(mainType, type);
        else if (mainType == 0x03) return getSunCardTypeName(mainType, type);
        else return "";
    }

    public static String getSunTypeName(DrivaceType dtType, byte type)
    {
        byte mainType = 0;
        if (dtType == DrivaceType.NODE) mainType = 0x01;
        else if (dtType == DrivaceType.LOCATION) mainType = 0x02;
        else if (dtType == DrivaceType.TAG) mainType = 0x03;
        else if (dtType == DrivaceType.USB_DANGLE) mainType = 0x04;
        return getSunTypeName(mainType, type);
    }

    public static String getSunCardTypeName(byte mainType, byte type)
    {
        String msg = "";
        if (0x03 == mainType)
        {
            if (type == 0x01) msg += "-SL03";
            if (type == 0x02) msg += "-PTAG_9056";
            if (type == 0x03) msg += "-PTAG_H";
        }
        return msg;
    }

    public static String getSunCankaodianTypeName(byte mainType, byte type)
    {
        String msg = "";
        if (0x02 == mainType)
        {
            if (type == 0x01) msg += "-ZB2530_01PA";
            if (type == 0x02) msg += "-ZB2530_03";
            if (type == 0x03) msg += "-ZB2530_LAN/WIFI_04";
        }
        return msg;
    }

    public static String getSunNODETypeName(byte mainType, byte type)
    {
        String msg = "";
        if (0x01 == mainType)
        {
            if (type == 0x01) msg += "-ZB2530_01PA";
            if (type == 0x02) msg += "-ZB2530_LAN";
            if (type == 0x03) msg += "-ZB2530_LAN/WIFI_04_lan";
            if (type == 0x04) msg += "-ZB2530_LAN/WIFI_04_wifi";
        }
        return msg;
    }

    public static byte getSunType(String sunTypeString)
    {
        if (sunTypeString.equals("-SL03") || sunTypeString.equals("-ZB2530_01PA")) return 0x01;
        else if (sunTypeString.equals("-PTAG_9056") || sunTypeString.equals("-ZB2530_03") || sunTypeString.equals("-ZB2530_LAN")) return 0x02;
        else if (sunTypeString.equals("-PTAG_H") || sunTypeString.equals("-ZB2530_LAN/WIFI_04_lan")) return 0x03;
        else if (sunTypeString.equals("-ZB2530_LAN/WIFI_04_wifi")) return 0x04;
        else return 0x00;
    }

    /**
     *
     * @param DrivateInt 0 代表wifi连接，1代表有线连接
     * @param data 状态码
     * @return
     */
    public static String getType(int DrivateInt, byte data)
    {
        if (DrivateInt == 0)
        {
            if (data == 0)      return "wifi連接成功";
            else if (data == 1) return "AT+RST失敗";
            else if (data == 2) return "AT+CWMODE_CUR失敗";
            else if (data == 3) return "AT+CWDHCP_CUR失敗";
            else if (data == 4) return "AT+CIPSTA_CUR失敗";
            else if (data == 5) return "AT+CWJAP_CUR失敗_未響應";
            else if (data == 6) return "AT+PING失敗";
            else if (data == 7) return "AT+CIPSEND失敗";
            else if (data == 8) return "AT+CWJAP_CUR失敗_連接超時";
            else if (data == 9) return "AT+CWJAP_CUR失敗_密碼錯誤";
            else if (data == 10) return "AT+CWJAP_CUR失敗_找不到目標AP";
            else if (data == 11) return "AT+CWJAP_CUR失敗_連接失敗";
            else if (data == 12) return "AT+CWJAP_CUR失敗_其他";
        }
        else if (DrivateInt == 1)
        {
            if (data == 0) return "網絡連接成功";
            else if (data == 1) return "網絡IC初始化失敗";
            else if (data == 2) return "物理網線連接失敗";
            else if (data == 3) return "獲取ARP失敗";
            else if (data == 4) return "DHCP失敗";
            else if (data == 5) return "IP衝突";
        }
        return "";
    }

}
