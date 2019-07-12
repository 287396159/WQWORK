package com.dmatek.setting.bean;

import com.dmatek.utils.DrivaceInfomation;
import com.dmatek.utils.XWUtils;

import java.io.Serializable;
import java.util.Arrays;

public class DrivaceInfoBean implements Serializable {

    /**
     * 不是故意取tagID,只是没得该了，这该死的AS。tagID现在是指设备ID
     */
    private byte[] tagID;
    private byte channel;
    private byte drivaceType;
    private byte mainDrivaceType;
    private byte[] drivaceVersion;

    public DrivaceInfoBean(){
    }

    /**
     * 獲取16進制字符串形式的ID
     * @return
     */
    public String getStringTagID(){
        if (getTagID() == null || getTagID().length <2)return "";
        int tag = XWUtils.uns(getTagID()[0],getTagID()[1]);
        return getFourHexString(tag);
    }

    /**
     * 10進制轉16進制，固定輸出4位數
     * @param value 原始值
     * @return 16進制值，固定4位數，不夠補0
     */
    public String getFourHexString(int value){
        return XWUtils.getHexString(value,4);
    }

    /**
     * 獲取版本信息
     * @return
     */
    public String getDrivaceVersionString(){
        return DrivaceInfomation.getVersion(getDrivaceVersion());
    }

    public byte getDrivaceType() {
        return drivaceType;
    }

    public void setDrivaceType(byte drivaceType) {
        this.drivaceType = drivaceType;
    }

    public byte getMainDrivaceType() {
        return mainDrivaceType;
    }

    public void setMainDrivaceType(byte mainDrivaceType) {
        this.mainDrivaceType = mainDrivaceType;
    }

    public String getDrivaceTypeString(){
        return DrivaceInfomation.getTypeName(getMainDrivaceType(),getDrivaceType());
    }

    public byte[] getTagID() {
        return tagID;
    }

    public void setTagID(byte[] tagID) {
        this.tagID = tagID;
    }

    public byte[] getDrivaceVersion() {
        return drivaceVersion;
    }

    public void setDrivaceVersion(byte[] drivaceVersion) {
        this.drivaceVersion = drivaceVersion;
    }

    public byte getChannel() {
        return channel;
    }

    public void setChannel(byte channel) {
        this.channel = channel;
    }


}
