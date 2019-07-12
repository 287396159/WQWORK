package com.dmatek.setting.bean;

import com.dmatek.setting.enums.GetSetType;

public class ReadSetLabel {

    public static final int READ = 1;
    public static final int SET = 2;

    public ReadSetLabel(GetSetType getSetType, int rsType) {
        this.getSetType = getSetType;
        this.rsType = rsType;
    }

    private GetSetType getSetType;
    private int rsType;

    public GetSetType getGetSetType() {
        return getSetType;
    }

    public void setGetSetType(GetSetType getSetType) {
        this.getSetType = getSetType;
    }

    public int getRsType() {
        return rsType;
    }

    public void setRsType(int rsType) {
        this.rsType = rsType;
    }
}
