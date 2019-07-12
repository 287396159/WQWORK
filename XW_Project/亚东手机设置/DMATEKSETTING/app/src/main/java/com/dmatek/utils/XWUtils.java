package com.dmatek.utils;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import java.util.Map;

public class XWUtils {

    // <summary>
    /// 计算校验位
    /// </summary>
    /// <param name="buf">数据包</param>
    /// <returns></returns>
    public static byte getCheckBit(byte[] buf) {
        byte check = 0;
        for (int i = 0; i < buf.length; i++) {
            check += buf[i];
        }
        return check;
    }


    /// <summary>
    /// 指定部位计算校验位
    /// </summary>
    /// <param name="buf">数据包</param>
    /// <param name="index">初始下标</param>
    /// <param name="length">长度</param>
    /// <returns></returns>
    public static byte getCheckBit(byte[] buf,int index,int length){
        byte check = 0;
        if(index >= length) return check;
        for (int i = index; i < length; i++){
            check += buf[i];
        }
        return check;
    }


    /**
     * 将byte改成无符号，方便一些运算
     * @param value
     * @return
     */
    public static int uns(byte value){
        return value&0xff;
    }

    /**
     * 将两个字节，换算成INT
     * @param value1 高八位
     * @param value2 低八位
     * @return
     */
    public static int uns(byte value1,byte value2) {
        return uns(value1) << 8 | uns(value2);
    }


    /**
     * 將10進制值，轉換為固定位數的16進制值
     * @param value 10進制值
     * @param dexLength 位數
     * @return 長度為dexLength的16進制值
     */
    public static String getHexString(int value,int dexLength){
        String hexStr = Integer.toHexString(value);
        if (dexLength > hexStr.length()){
            String hexLen = getZero(dexLength-hexStr.length())+hexStr;
            hexStr = hexLen;
        }
        return hexStr;
    }

    /**
     * 獲取給定位數的0
     * @param length 長度
     * @return length個0
     */
    public static String getZero(int length){
        StringBuffer sffer = new StringBuffer();
        for (int i = 0;i < length;i++){
            sffer.append("0");
        }
        return  sffer.toString();
    }

    /**
     * map集合转list集合
     * @param myGroupMaps
     * @param <K>
     * @param <T>
     * @return
     */
    public static <K,T> List<T> getListFormMap(Map<K,T> myGroupMaps)
    {
        if (myGroupMaps == null) {
            return null;
        }
        Collection<T> valueCollection = myGroupMaps.values();
        List<T> myGroups = new ArrayList<T>(valueCollection);
        return myGroups;
    }

    public static byte[] getIp(String ip){
        String[] ipsql = ip.split("\\.");
        if (ipsql.length != 4) return  null;
        byte[] ipBt = new byte[4];
        for (int i = 0; i < 4; i++){
            try{
                ipBt[i] = (byte)Integer.parseInt(ipsql[i]);
            }catch (NumberFormatException nEx){
            }
        }
        return  ipBt;
    }

    public static String toHexString(byte[] a) {
        if (a == null)
            return "null";
        int iMax = a.length - 1;
        if (iMax == -1)
            return "[]";

        StringBuilder b = new StringBuilder();
        b.append('[');
        for (int i = 0; ; i++) {
            String ss = Integer.toHexString(uns(a[i]));
            b.append(ss);
            if (i == iMax)
                return b.append(']').toString();
            b.append(", ");
        }
    }

}
