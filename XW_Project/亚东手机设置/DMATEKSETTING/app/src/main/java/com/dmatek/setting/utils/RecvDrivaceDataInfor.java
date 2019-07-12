package com.dmatek.setting.utils;

import com.dmatek.setting.bean.RecvDrivaceDataBean;
import com.dmatek.setting.enums.GetSetType;

public class RecvDrivaceDataInfor {

    public RecvDrivaceDataInfor(){
    }

    /**
     * 设置RecvDrivaceDataBean对象的基本函数
     * @param getSetType
     * @param rs
     * @param drivaceID
     * @param backDatas
     */
    public RecvDrivaceDataBean setRecvDataBase( GetSetType getSetType,int rs,byte[] drivaceID,byte[] backDatas){
        RecvDrivaceDataBean recvDrivaceDataBean = new RecvDrivaceDataBean();
        recvDrivaceDataBean
                .setDrivaceID(drivaceID)
                .setBackDatas(backDatas);
        return recvDrivaceDataBean;
    }

}
