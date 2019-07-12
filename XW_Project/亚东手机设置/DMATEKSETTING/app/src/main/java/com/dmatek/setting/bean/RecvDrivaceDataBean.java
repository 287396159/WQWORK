package com.dmatek.setting.bean;

import com.dmatek.setting.enums.GetSetType;
import com.dmatek.utils.DrivaceInfomation;
import com.dmatek.utils.XWUtils;

import java.io.UnsupportedEncodingException;
import java.util.Arrays;
import java.util.Base64;

public class RecvDrivaceDataBean {

    private byte[] drivaceID;
    private byte[] backDatas; //回复多个数据的时候
    private byte backDt; //回复单个数据的时候
    private byte packType; // 数据包类型


    public byte[] getDrivaceID() {
        return drivaceID;
    }

    public RecvDrivaceDataBean setDrivaceID(byte[] drivaceID) {
        this.drivaceID = drivaceID;
        return  this;
    }

    public byte[] getBackDatas() {
        return backDatas;
    }

    public RecvDrivaceDataBean setBackDatas(byte[] backDatas) {
        this.backDatas = backDatas;
        return  this;
    }

    public byte getBackDt() {
        return backDt;
    }

    public RecvDrivaceDataBean setBackDt(byte backDt) {
        this.backDt = backDt;
        return  this;
    }

    public byte getPackType() {
        return packType;
    }

    public void setPackType(byte packType) {
        this.packType = packType;
    }

    public String getIpData(){
        StringBuilder suder = new StringBuilder();
        for (int i = 0;i<backDatas.length;i++){
            suder.append(XWUtils.uns(backDatas[i]));
            if (i != backDatas.length - 1)suder.append(".");
        }
        return suder.toString();
    }

    public String getPortData(){
        int bData = XWUtils.uns(backDatas[0],backDatas[1]);
        String msg = bData+"";
        return msg;
    }

    /**
     * 真是操碎了心，各种判断
     * @return
     */
    public String getBackString(){
        if (backDatas == null) return "";
        if (packType == 0x09 || packType == 0x0a || packType == 0x0b || packType == 0x0c){
            String wifi = "";
            try {
                wifi = new String(backDatas,"utf-8").trim();
            } catch (UnsupportedEncodingException e) {
                e.printStackTrace();
            }
            return wifi;
        }
        if (backDatas.length == 4){
            return getIpData();
        }else if (backDatas.length == 2){
            String poetMsg = getPortData();
            return poetMsg;
        }else if (backDatas.length == 1){
            return  XWUtils.uns(backDt)+"";
        }
        return  "error:"+new String(backDatas);
    }

    /**
     * 节点 获取不同的返回数据
     * @param DrivaceType
     * @return
     */
    public String getDiffBackString(int DrivaceType){
        String msg = "";
        if(packType == 0x17){
            msg = DrivaceInfomation.getType(DrivaceType,backDt);
        }else if (packType == 0x19 || packType == 0x0f){
            msg = backDt == 1?"static model":"Dynamic mode";
        } else if (packType == 0x48){
            Double d = new Double(XWUtils.uns(backDt));
            msg = d/100+"";
        }else{
            msg = getBackString();
        }
        return msg;
    }

    @Override
    public String toString() {
        return "RecvDrivaceDataBean{" +
                "drivaceID=" + XWUtils.toHexString(drivaceID) +
                ", backDatas=" + XWUtils.toHexString(backDatas) +
                ", backDt=" + backDt +
                ", packType=" + packType +
                '}';
    }
}
