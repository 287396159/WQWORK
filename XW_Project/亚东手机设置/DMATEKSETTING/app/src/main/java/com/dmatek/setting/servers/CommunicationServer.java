package com.dmatek.setting.servers;

import android.app.Service;
import android.content.Intent;
import android.os.Binder;
import android.os.IBinder;
import android.support.annotation.Nullable;
import android.util.Log;

import com.dmatek.setting.bean.RecvDrivaceDataBean;
import com.dmatek.setting.bean.SendDataBean;
import com.dmatek.utils.USB_Utils;
import com.dmatek.utils.UdpUtils;

import org.greenrobot.eventbus.EventBus;
import org.greenrobot.eventbus.Subscribe;
import org.greenrobot.eventbus.ThreadMode;

/**
 * Created by Administrator on 2019/1/4.
 */

public class CommunicationServer extends Service {

    /** 创建参数 */
    boolean threadDisable;
    int count;
    //UdpUtils udpUtils;
    USB_Utils udpUtils;

    public IBinder onBind(Intent intent) {
        System.out.println("bind");
        return null;
    }

    @Override
    public void onCreate() {
        super.onCreate();
        Log.i("UDP", "onCreate: ");
        EventBus.getDefault().register(CommunicationServer.this);
        startOpenUDP();
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        Log.i("server", "onStartCommand: ");
        return super.onStartCommand(intent, flags, startId);
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
        EventBus.getDefault().unregister(this);
        Log.i("UDP", "onDestroy: over");
    }

    public void startOpenUDP(){
        //udpUtils = new UdpUtils();
        udpUtils = new USB_Utils(this);
        udpUtils.isCheckUsbDevice();
    }

    //定义处理接收的方法
    @Subscribe(threadMode = ThreadMode.POSTING)
    public void userEventBus(Object obj){
        if (obj instanceof SendDataBean){
            SendDataBean sendDataBean = (SendDataBean)obj;
            if (udpUtils != null) udpUtils.sendData(sendDataBean.getSendDt());
        }else if (obj instanceof String){
            String udpMessage = (String)obj;
            Log.i("UDP", "userEventBus: "+udpMessage);
            if ("openUdp".equals(udpMessage)){
                if (udpUtils != null) udpUtils.setSocket();
            }else if ("closeUdp".equals(udpMessage)){
                if (udpUtils != null) udpUtils.closeUdp();
            }
        }
    }


}
