package com.dmatek.setting.bean;

public class SendDataBean {

    private int packData;
    private byte[] sendDt;
    private String sendIp;

    public SendDataBean(){
    }

    public int getPackData() {
        return packData;
    }

    public void setPackData(int packData) {
        this.packData = packData;
    }

    public byte[] getSendDt() {
        return sendDt;
    }

    public void setSendDt(byte[] sendDt) {
        this.sendDt = sendDt;
    }
}
