package com.dmatek.setting.utils;

import com.dmatek.utils.XWUtils;

public class SendDataUtils {

    public static byte[] getBaseSendBt(byte packType,int length){
        byte[] sendDt = new byte[length];
        sendDt[0] = (byte)0xf2;
        sendDt[1] = packType;
        sendDt[length - 1] = (byte)0xf1;
        return sendDt;
    }

    public static byte[] sendSearchNODEId(){
        return new byte[]{(byte)0xf2,0x01,(byte)0xf3,(byte)0xf1};
    }

    /**
     * 节点信息的发送数据处理
     * @param id ID号
     * @param channel
     * @return
     */
    public static byte[] sendSearchNODEInfor(byte[] id,byte channel){
        byte[] sendDt = getBaseSendBt((byte)0x02,7);
        System.arraycopy(id,0,sendDt,2,id.length);
        sendDt[4] = channel;
        sendDt[sendDt.length - 2] = XWUtils.getCheckBit(sendDt,0,sendDt.length - 2);
        return  sendDt;
    }

    /**
     * 节点信息的发送数据处理
     * @param id ID号
     * @param channel
     * @return
     */
    public static byte[] sendSearchNODENoChannelInfor(byte[] id,byte channel){
        byte[] sendDt = getBaseSendBt((byte)0x02,6);
        System.arraycopy(id,0,sendDt,2,id.length);
        sendDt[sendDt.length - 2] = XWUtils.getCheckBit(sendDt,0,sendDt.length - 2);
        return  sendDt;
    }

    public static byte[] sendSearchCanKaoDianId(){
        return new byte[]{(byte)0xf2,(byte)0x41,(byte)0x33,(byte)0xf1};
    }

    /**
     * 参考点信息的发送数据处理
     * @param id ID号
     * @return
     */
    public static byte[] sendSearchCanKaoDianInfor(byte[] id){
        byte[] sendDt = getBaseSendBt((byte)0x42,6);
        System.arraycopy(id,0,sendDt,2,id.length);
        sendDt[sendDt.length - 2] = XWUtils.getCheckBit(sendDt,0,sendDt.length - 2);
        return  sendDt;
    }


    public static byte[] sendSearchCardId(){
        return new byte[]{(byte)0xf2,(byte)0x80,(byte)0x72,(byte)0xf1};
    }

    /**
     * 卡片信息的发送数据处理，卡片没有channel
     * @param id
     * @return
     */
    public static byte[] sendSearchCardInfor(byte[] id){
        byte[] sendDt = getBaseSendBt((byte)0x81,6);
        System.arraycopy(id,0,sendDt,2,id.length);
        sendDt[sendDt.length - 2] = XWUtils.getCheckBit(sendDt,0,sendDt.length - 2);
        return  sendDt;
    }

}
