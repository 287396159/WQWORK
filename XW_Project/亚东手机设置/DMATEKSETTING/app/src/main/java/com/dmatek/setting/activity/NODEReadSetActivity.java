package com.dmatek.setting.activity;

import com.dmatek.setting.bean.ReadSetViewBean;
import com.dmatek.setting.utils.DrivaceReadSetUtils;
import com.dmatek.utils.XWUtils;

import java.util.ArrayList;

public class NODEReadSetActivity extends BaseSetReadActivity {

    {
        readSetLabels = DrivaceReadSetUtils.getAllGetSetNODEType();
        mReadSetMaps = new ArrayList<ReadSetViewBean>();

    }

    @Override
    public void initData() {
        resertByte = (byte) 0x0d;
    }

    @Override
    public byte[] getSetBaseBt(byte pack, byte[] id,byte channel, byte[] date) {
        byte[] sendBt = new byte[7+date.length];
        sendBt[0] = (byte)0xf2;
        sendBt[1] = pack;
        sendBt[2] = id[0];
        sendBt[3] = id[1];
        sendBt[4] = channel;
        System.arraycopy(date,0,sendBt,5,date.length);
        sendBt[sendBt.length -2] = XWUtils.getCheckBit(sendBt,0,sendBt.length -2);
        sendBt[sendBt.length -1] = (byte)0xf1;
        return sendBt;
    }

    @Override
    public byte[] getReadBaseBt(byte pack,byte[] id,byte channel){
        byte[] sendBt = new byte[]{(byte)0xf2,pack,id[0],id[1],(byte)channel,(byte)0xf1,(byte)0xf1};
        sendBt[sendBt.length -2] = XWUtils.getCheckBit(sendBt,0,sendBt.length -2);
        return  sendBt;
    }

    public void onCreaterAafter(){
        int i = getDrivaceTypeInt();
        if (i == 0){ //wifi?
            DrivaceReadSetUtils.readSetNodeWifiMap(mReadSetMaps);

        }else {
            DrivaceReadSetUtils.readSetNodeLanMap(mReadSetMaps);
        }
    }

}
