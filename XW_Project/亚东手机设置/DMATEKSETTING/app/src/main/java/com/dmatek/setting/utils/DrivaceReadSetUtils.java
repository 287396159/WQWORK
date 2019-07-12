package com.dmatek.setting.utils;

import com.dmatek.setting.adapter.ReadSetViewAdapter;
import com.dmatek.setting.bean.CardReadSetLabel;
import com.dmatek.setting.bean.ReadSetLabel;
import com.dmatek.setting.bean.ReadSetViewBean;
import com.dmatek.setting.enums.GetSetType;
import com.dmatek.utils.XWUtils;

import java.io.UnsupportedEncodingException;
import java.util.List;

public class DrivaceReadSetUtils {

    public  static ReadSetLabel[] getAllGetSetNODEType(){
        ReadSetLabel[] getSetTypes = new ReadSetLabel[0xe5];
        for (int i = 0; i < getSetTypes.length;i++){
            getSetTypes[i] = new ReadSetLabel(GetSetType.Nothing,ReadSetLabel.READ);
        }
        getSetTypes[0x05].setGetSetType(GetSetType.SERVER_IP);
        getSetTypes[0x05].setRsType(ReadSetLabel.SET);
        getSetTypes[0x06].setGetSetType(GetSetType.SERVER_IP);
        getSetTypes[0x06].setRsType(ReadSetLabel.READ);

        getSetTypes[0x07].setGetSetType(GetSetType.SERVER_PORT);
        getSetTypes[0x07].setRsType(ReadSetLabel.SET);
        getSetTypes[0x08].setGetSetType(GetSetType.SERVER_PORT);
        getSetTypes[0x08].setRsType(ReadSetLabel.READ);

        getSetTypes[0x09].setGetSetType(GetSetType.WIFI_NAME);
        getSetTypes[0x09].setRsType(ReadSetLabel.SET);
        getSetTypes[0x0A].setGetSetType(GetSetType.WIFI_NAME);
        getSetTypes[0x0A].setRsType(ReadSetLabel.READ);

        getSetTypes[0x0B].setGetSetType(GetSetType.WIFI_PASSWORD);
        getSetTypes[0x0B].setRsType(ReadSetLabel.SET);
        getSetTypes[0x0C].setGetSetType(GetSetType.WIFI_PASSWORD);
        getSetTypes[0x0C].setRsType(ReadSetLabel.READ);

        //getSetTypes[0x0D].setGetSetType(GetSetType.RESERT);

        getSetTypes[0x0E].setGetSetType(GetSetType.NODE_MODEL);
        getSetTypes[0x0E].setRsType(ReadSetLabel.SET);
        getSetTypes[0x0F].setGetSetType(GetSetType.NODE_MODEL);
        getSetTypes[0x0F].setRsType(ReadSetLabel.READ);

        getSetTypes[0x10].setGetSetType(GetSetType.NODE_IP);
        getSetTypes[0x10].setRsType(ReadSetLabel.SET);
        getSetTypes[0x11].setGetSetType(GetSetType.NODE_IP);
        getSetTypes[0x11].setRsType(ReadSetLabel.READ);

        getSetTypes[0x12].setGetSetType(GetSetType.SUBMASK);
        getSetTypes[0x12].setRsType(ReadSetLabel.SET);
        getSetTypes[0x13].setGetSetType(GetSetType.SUBMASK);
        getSetTypes[0x13].setRsType(ReadSetLabel.READ);

        getSetTypes[0x14].setGetSetType(GetSetType.GATEWAY);
        getSetTypes[0x14].setRsType(ReadSetLabel.SET);
        getSetTypes[0x15].setGetSetType(GetSetType.GATEWAY);
        getSetTypes[0x15].setRsType(ReadSetLabel.READ);

        getSetTypes[0x16].setGetSetType(GetSetType.WIFI_RESSI);
        getSetTypes[0x17].setGetSetType(GetSetType.WIFI_LAST_CONNECTION_TYPE);

        getSetTypes[0x18].setGetSetType(GetSetType.WIFI_MODEL);
        getSetTypes[0x18].setRsType(ReadSetLabel.SET);
        getSetTypes[0x19].setGetSetType(GetSetType.WIFI_MODEL);
        getSetTypes[0x19].setRsType(ReadSetLabel.READ);

        getSetTypes[0x1A].setGetSetType(GetSetType.WIFI_IP);
        getSetTypes[0x1A].setRsType(ReadSetLabel.SET);
        getSetTypes[0x1B].setGetSetType(GetSetType.WIFI_IP);
        getSetTypes[0x1B].setRsType(ReadSetLabel.READ);

        return getSetTypes;
    }

    public  static ReadSetLabel[] getAllGetSetCKDType(){
        ReadSetLabel[] getSetTypes = new ReadSetLabel[0xd0];
        for (int i = 0; i < getSetTypes.length;i++){
            getSetTypes[i] = new ReadSetLabel(GetSetType.Nothing,CardReadSetLabel.READ);
        }

        getSetTypes[0x45].setGetSetType(GetSetType.CKD_THRESHOLD);
        getSetTypes[0x45].setRsType(ReadSetLabel.SET);
        getSetTypes[0x46].setGetSetType(GetSetType.CKD_THRESHOLD);
        getSetTypes[0x46].setRsType(ReadSetLabel.READ);

        getSetTypes[0x47].setGetSetType(GetSetType.CKD_K1);
        getSetTypes[0x47].setRsType(ReadSetLabel.SET);
        getSetTypes[0x48].setGetSetType(GetSetType.CKD_K1);
        getSetTypes[0xc8].setRsType(ReadSetLabel.READ);

        return getSetTypes;
    }

    public  static ReadSetLabel[] getAllGetSetTagType(){
        ReadSetLabel[] getSetTypes = new ReadSetLabel[0xd0];
        for (int i = 0; i < getSetTypes.length;i++){
            getSetTypes[i] = new ReadSetLabel(GetSetType.Nothing,CardReadSetLabel.READ);
        }

        getSetTypes[0xc4].setGetSetType(GetSetType.CARD_UPTIME);
        getSetTypes[0xc4].setRsType(ReadSetLabel.SET);
        getSetTypes[0xc5].setGetSetType(GetSetType.CARD_UPTIME);
        getSetTypes[0xc5].setRsType(ReadSetLabel.READ);

        getSetTypes[0xc6].setGetSetType(GetSetType.CARD_POWER);
        getSetTypes[0xc6].setRsType(ReadSetLabel.SET);
        getSetTypes[0xc7].setGetSetType(GetSetType.CARD_POWER);
        getSetTypes[0xc7].setRsType(ReadSetLabel.READ);

        getSetTypes[0xc8].setGetSetType(GetSetType.CARD_STATICSLEEPTIME);
        getSetTypes[0xc8].setRsType(ReadSetLabel.SET);
        getSetTypes[0xc9].setGetSetType(GetSetType.CARD_STATICSLEEPTIME);
        getSetTypes[0xc9].setRsType(ReadSetLabel.READ);

        getSetTypes[0xca].setGetSetType(GetSetType.CARD_SENSITIVITY);
        getSetTypes[0xca].setRsType(ReadSetLabel.SET);
        getSetTypes[0xcb].setGetSetType(GetSetType.CARD_SENSITIVITY);
        getSetTypes[0xcb].setRsType(ReadSetLabel.READ);
        return getSetTypes;
    }

    public  static void readSetCardMap(List<ReadSetViewBean> mReadSetMaps){
        ReadSetViewBean cardUptime = new ReadSetViewBean();
        cardUptime.setReadSetData(GetSetType.CARD_UPTIME,"Up Time")
                .setTitleMsg("讀取和設置上報時間，取值範圍0~65535");
        ReadSetViewBean cardPower = new ReadSetViewBean();
        cardPower.setReadSetData(GetSetType.CARD_POWER,"Power")
                .setTitleMsg("卡片功率");
        cardPower.setViewType(ReadSetViewAdapter.MODEL_SET_NOMAL_READ);
        ReadSetViewBean cardStaticTime = new ReadSetViewBean();
        cardStaticTime.setReadSetData(GetSetType.CARD_STATICSLEEPTIME,"Static Time")
                .setTitleMsg("卡片靜止時間，取值範圍0~65535");
        ReadSetViewBean cardSensitivity = new ReadSetViewBean();
        cardSensitivity.setReadSetData(GetSetType.CARD_SENSITIVITY,"Sensitivity")
                .setTitleMsg("卡片靈敏度，取值範圍1~100");
        if (mReadSetMaps != null)
        {
            mReadSetMaps.add(cardUptime);
            mReadSetMaps.add(cardPower);
            mReadSetMaps.add(cardStaticTime);
            mReadSetMaps.add(cardSensitivity);
        }
    }

    public  static void readSetCanKaoDianMap(List<ReadSetViewBean> mReadSetMaps){
        ReadSetViewBean cardUptime = new ReadSetViewBean();
        cardUptime.setReadSetData(GetSetType.CKD_THRESHOLD,"Threshold")
                .setTitleMsg("信號強度閾值,取值範圍0~255");
        ReadSetViewBean cardPower = new ReadSetViewBean();
        cardPower.setReadSetData(GetSetType.CKD_K1,"K1")
                .setTitleMsg("信號強度係數k,取值範圍0.01~2.55");

        if (mReadSetMaps != null)
        {
            mReadSetMaps.add(cardUptime);
            mReadSetMaps.add(cardPower);
        }
    }

    public  static void readSetNodeWifiMap(List<ReadSetViewBean> mReadSetMaps){
        ReadSetViewBean wifiRssi = new ReadSetViewBean();
        wifiRssi.setReadSetData(GetSetType.WIFI_RESSI,"WiFi Ress")
                .setTitleMsg("輸入wifi名稱。可以查詢wifi的信號強度");
        ReadSetViewBean queryNetWork = new ReadSetViewBean();
        queryNetWork.setReadSetData(GetSetType.WIFI_LAST_CONNECTION_TYPE,"Query network")
                .setTitleMsg("查詢網絡連接狀態");
        ReadSetViewBean serverIp = new ReadSetViewBean();
        serverIp.setReadSetData(GetSetType.SERVER_IP,"ServerIp")
                .setTitleMsg("Server Ip,參考值：192.168.1.63");
        ReadSetViewBean serverPort = new ReadSetViewBean();
        serverPort.setReadSetData(GetSetType.SERVER_PORT,"ServerPort")
                .setTitleMsg("Server端口，取值範圍0~65535");
        ReadSetViewBean wifiName = new ReadSetViewBean();
        wifiName.setReadSetData(GetSetType.WIFI_NAME,"wifi name")
                .setTitleMsg("WIFI名稱，建議少於32個英文數字組合");
        ReadSetViewBean wifiPassword = new ReadSetViewBean();
        wifiPassword.setReadSetData(GetSetType.WIFI_PASSWORD,"wifi password")
                .setTitleMsg("WIFI密碼，建議少於32個英文數字組合");
        ReadSetViewBean wifiModel = new ReadSetViewBean();
        wifiModel.setReadSetData(GetSetType.WIFI_MODEL,"wifi model")
                .setTitleMsg("WIFI模式，建議少於32個英文數字組合");
        wifiModel.setViewType(ReadSetViewAdapter.MODEL_SET_NOMAL_READ);
        ReadSetViewBean wifiIp = new ReadSetViewBean();
        wifiIp.setReadSetData(GetSetType.WIFI_IP,"wifi ip")
                .setTitleMsg("WIFI的ip，參考值：192.168.1.63");
        ReadSetViewBean nodeIp = new ReadSetViewBean();
        nodeIp.setReadSetData(GetSetType.NODE_IP,"node ip")
                .setTitleMsg("節點的ip，參考值：192.168.1.63");
        ReadSetViewBean subMask = new ReadSetViewBean();
        subMask.setReadSetData(GetSetType.SUBMASK,"subMask")
                .setTitleMsg("節點的子網掩碼，參考值：192.168.1.63");
        ReadSetViewBean gateWay = new ReadSetViewBean();
        gateWay.setReadSetData(GetSetType.GATEWAY,"gateWay")
                .setTitleMsg("節點的GateWay，參考值：192.168.1.63");
        if (mReadSetMaps != null)
        {
            mReadSetMaps.add(wifiRssi);
            mReadSetMaps.add(queryNetWork);
            mReadSetMaps.add(serverIp);
            mReadSetMaps.add(serverPort);
            mReadSetMaps.add(wifiName);
            mReadSetMaps.add(wifiPassword);
            mReadSetMaps.add(wifiModel);
            mReadSetMaps.add(wifiIp);
            mReadSetMaps.add(nodeIp);
        }
    }

    public  static void readSetNodeLanMap(List<ReadSetViewBean> mReadSetMaps){
        ReadSetViewBean queryNetWork = new ReadSetViewBean();
        queryNetWork.setReadSetData(GetSetType.WIFI_LAST_CONNECTION_TYPE,"Query network")
                .setTitleMsg("查詢網絡連接狀態");
        ReadSetViewBean serverIp = new ReadSetViewBean();
        serverIp.setReadSetData(GetSetType.SERVER_IP,"ServerIp")
                .setTitleMsg("Server Ip,參考值：192.168.1.63");
        ReadSetViewBean serverPort = new ReadSetViewBean();
        serverPort.setReadSetData(GetSetType.SERVER_PORT,"ServerPort")
                .setTitleMsg("Server端口，取值範圍0~65535");

        ReadSetViewBean nodeModel = new ReadSetViewBean();
        nodeModel.setReadSetData(GetSetType.NODE_MODEL,"node model")
                .setTitleMsg("節點模式，建議少於32個英文數字組合");
        nodeModel.setViewType(ReadSetViewAdapter.MODEL_SET_NOMAL_READ);
        ReadSetViewBean nodeIp = new ReadSetViewBean();
        nodeIp.setReadSetData(GetSetType.NODE_IP,"node ip")
                .setTitleMsg("節點的ip，參考值：192.168.1.63");
        ReadSetViewBean subMask = new ReadSetViewBean();
        subMask.setReadSetData(GetSetType.SUBMASK,"subMask")
                .setTitleMsg("節點的子網掩碼，參考值：192.168.1.63");
        ReadSetViewBean gateWay = new ReadSetViewBean();
        gateWay.setReadSetData(GetSetType.GATEWAY,"gateWay")
                .setTitleMsg("節點的GateWay，參考值：192.168.1.63");
        if (mReadSetMaps != null)
        {
            mReadSetMaps.add(queryNetWork);
            mReadSetMaps.add(serverIp);
            mReadSetMaps.add(serverPort);
            mReadSetMaps.add(nodeModel);
            mReadSetMaps.add(nodeIp);
            mReadSetMaps.add(subMask);
            mReadSetMaps.add(gateWay);
        }
    }

    public  static void readSetMap(List<ReadSetViewBean> mReadSetMaps){}

    /**
     *
     * @param getSetType
     * @param readSet 2代表读取，1代表设置
     * @return
     */
    public  static byte getDrivaceTypeInGetSetType(GetSetType getSetType, int readSet){
        int pack = 0;
        switch (getSetType){
            case SERVER_IP:
                pack = 0x05;
                break;
            case SERVER_PORT:
                pack = 0x07;
                break;
            case WIFI_NAME:
                pack = 0x09;
                break;
            case WIFI_PASSWORD:
                pack = 0x0B;
                break;
            case RESERT:
                pack = 0x0D;
                if (readSet == 1)pack -- ;
                break;
            case NODE_MODEL:
                pack = 0x0E;
                break;
            case NODE_IP:
                pack = 0x10;
                break;
            case SUBMASK:
                pack = 0x12;
                break;
            case GATEWAY:
                pack = 0x14;
                break;
            case WIFI_RESSI:
                pack = 0x16;
                if (readSet == 2)pack -- ;
                break;
            case WIFI_LAST_CONNECTION_TYPE:
                pack = 0x17;
                if (readSet == 2)pack -- ;
                break;
            case WIFI_MODEL:
                pack = 0x18;
                break;
            case WIFI_IP:
                pack = 0x1A;
                break;
            case CKD_THRESHOLD:
                pack = 0x45;
                break;
            case CKD_K1:
                pack = 0x47;
                break;

            case CARD_UPTIME:
                pack = 0xc4;
                break;
            case CARD_POWER:
                pack = 0xc6;
                break;
            case CARD_STATICSLEEPTIME:
                pack = 0xc8;
                break;
            case CARD_SENSITIVITY:
                pack = 0xca;
                break;
            default:
                break;
        }
        if (readSet != 1) pack += 1;
        return  (byte)pack;
    }

    /**
     * 发送数据之 设置参数的专属String转byte[]
     * @param getSetType
     * @param setText
     * @return
     */
    public static byte[] getDrivcePackTypeSetData(GetSetType getSetType, String setText){
        byte[] sDt = null;
        switch (getSetType){
            case SERVER_IP:
                sDt = XWUtils.getIp(setText);
                break;
            case SERVER_PORT:
                try{
                    int serverPort = Integer.parseInt(setText);
                    sDt = new byte[]{(byte)(serverPort/0x100),(byte)(serverPort%0x100)};
                }catch (NumberFormatException nex){}
                break;
            case WIFI_NAME:
                sDt = wifiNamePass(setText);
                break;
            case WIFI_PASSWORD:
                sDt = wifiNamePass(setText);
                break;
            case RESERT:
                break;
            case NODE_MODEL:
                if ("static model".equals(setText)){
                    sDt = new byte[]{(byte)(1)};
                }else if ("Dynamic mode".equals(setText)){
                    sDt = new byte[]{(byte)(2)};
                }
                break;
            case NODE_IP:
                sDt = XWUtils.getIp(setText);
                break;
            case SUBMASK:
                sDt = XWUtils.getIp(setText);
                break;
            case GATEWAY:
                sDt = XWUtils.getIp(setText);
                break;
            case WIFI_RESSI:
                sDt = wifiNamePass(setText);
                break;
            case WIFI_LAST_CONNECTION_TYPE:
                break;
            case WIFI_MODEL:
                if ("static model".equals(setText)){
                    sDt = new byte[]{(byte)(1)};
                }else if ("Dynamic mode".equals(setText)){
                    sDt = new byte[]{(byte)(2)};
                }
                break;
            case WIFI_IP:
                sDt = XWUtils.getIp(setText);
                break;
            case CKD_THRESHOLD:
                try{
                    int threshold = Integer.parseInt(setText);
                    sDt = new byte[]{(byte)(threshold)};
                }catch (NumberFormatException nex){}
                break;
            case CKD_K1:
                try{
                    double sdp = Double.parseDouble(setText);
                    double ppDouble = (sdp*100+0.01);
                    int ss = (int)ppDouble;
                    ///int k1 = Integer.parseInt(setText);
                    sDt = new byte[]{(byte)(ss)};
                }catch (NumberFormatException nex){}
                break;
            case CARD_UPTIME:
                try{
                    int upTime = Integer.parseInt(setText);
                    sDt = new byte[]{(byte)(upTime/0x100),(byte)(upTime%0x100)};
                }catch (NumberFormatException nex){}
                break;
            case CARD_POWER:
                try{
                    int power = Integer.parseInt(setText);
                    sDt = new byte[]{(byte)(power)};
                }catch (NumberFormatException nex){}
                break;
            case CARD_STATICSLEEPTIME:
                try{
                    int staticTime = Integer.parseInt(setText);
                    sDt = new byte[]{(byte)(staticTime/0x100),(byte)(staticTime%0x100)};
                }catch (NumberFormatException nex){}
                break;
            case CARD_SENSITIVITY:
                try{
                    int sensitivity = Integer.parseInt(setText);
                    sDt = new byte[]{(byte)(sensitivity)};
                }catch (NumberFormatException nex){}
                break;
            default:
                break;
        }
        return sDt;
    }

    /**
     * 固定的32位字节，比如wifi账号，wifi密码，不够的位数，填0
     * @param setText
     * @return
     */
    public static byte[] wifiNamePass(String setText)
    {
        byte[] sDt = new byte[32];
        try {
            byte[] stext = setText.getBytes("utf-8");
            System.arraycopy(stext,0,sDt,0,stext.length);
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
        }
        return sDt;
    }

}
