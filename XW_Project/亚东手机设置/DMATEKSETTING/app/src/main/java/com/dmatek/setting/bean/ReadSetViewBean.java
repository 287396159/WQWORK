package com.dmatek.setting.bean;

import android.view.View;

import com.dmatek.setting.adapter.ReadSetViewAdapter;
import com.dmatek.setting.enums.GetSetType;

public class ReadSetViewBean {

    private GetSetType getsetType;
    private int viewType;
    private String btnSetName;
    private String btnReadName;
    private String editSetHint;
    private String editReadHint;
    private String toast;
    private int toastVisi;
    private String editSetText;
    private String editReadText;
    private String titleMsg;

    public ReadSetViewBean()
    {
        toastVisi = View.GONE;
        toast = "成功";
        viewType = ReadSetViewAdapter.NOMAL_READ_SET;
    }

    public int getViewType() {
        return viewType;
    }

    public void setViewType(int viewType) {
        this.viewType = viewType;
    }

    public String getBtnSetName() {
        return btnSetName;
    }

    public void setBtnSetName(String btnSetName) {
        this.btnSetName = btnSetName;
    }

    public String getBtnReadName() {
        return btnReadName;
    }

    public void setBtnReadName(String btnReadName) {
        this.btnReadName = btnReadName;
    }

    public String getEditSetHint() {
        return editSetHint;
    }

    public void setEditSetHint(String editSetHint) {
        this.editSetHint = editSetHint;
    }

    public String getEditReadHint() {
        return editReadHint;
    }

    public void setEditReadHint(String editReadHint) {
        this.editReadHint = editReadHint;
    }

    public String getToast() {
        return toast;
    }

    public void setToast(String toast) {
        this.toast = toast;
    }

    public GetSetType getGetsetType() {
        return getsetType;
    }

    public void setGetsetType(GetSetType getsetType) {
        this.getsetType = getsetType;
    }

    public int getToastVisi() {
        return toastVisi;
    }

    public void setToastVisi(int toastVisi) {
        this.toastVisi = toastVisi;
    }

    public String getEditSetText() {
        return editSetText;
    }

    public void setEditSetText(String editSetText) {
        this.editSetText = editSetText;
    }

    public String getEditReadText() {
        return editReadText;
    }

    public void setEditReadText(String editReadText) {
        this.editReadText = editReadText;
    }

    public String getTitleMsg() {
        return titleMsg;
    }

    public ReadSetViewBean setTitleMsg(String titleMsg) {
        this.titleMsg = titleMsg;
        return this;
    }

    public ReadSetViewBean setReadSetData(GetSetType getsetType, String editSetHint){
        setGetsetType(getsetType);
        setEditSetHint(editSetHint);
        return this;
    }



    @Override
    public String toString() {
        return "ReadSetViewBean{" +
                "getsetType=" + getsetType +
                ", viewType=" + viewType +
                ", btnSetName='" + btnSetName + '\'' +
                ", btnReadName='" + btnReadName + '\'' +
                ", editSetHint='" + editSetHint + '\'' +
                ", editReadHint='" + editReadHint + '\'' +
                ", toast='" + toast + '\'' +
                ", toastVisi=" + toastVisi +
                ", editSetText='" + editSetText + '\'' +
                ", editReadText='" + editReadText + '\'' +
                '}';
    }


}
