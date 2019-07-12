package com.dmatek.setting;

import android.util.Log;

import com.dmatek.setting.bean.SendDataBean;
import com.dmatek.setting.utils.SendDataUtils;

import org.greenrobot.eventbus.EventBus;

/**
 * 指定重复发送次数线程
 */
public class DesignatedNumberToSendThread extends Thread {

    private int designatedNumber = 5;
    private SendDataBean designateSendDataBean;

    @Override
    public void run() {
        super.run();
        try {
            while (!Thread.currentThread().isInterrupted() && designatedNumber > 0){
                sendDataToExenBus(designateSendDataBean);
                Thread.sleep(200);
                designatedNumber--;
            }
        } catch (InterruptedException e) {
            Log.i("InterruptedException", "run: searchDrivaceIDRunnable 中断了一次");
        }finally {
        }
    }

    private void sendDataToExenBus(SendDataBean sendDataBean){
        if (sendDataBean != null) EventBus.getDefault().post(sendDataBean);
    }

    public void setDesignateSendDataBean(SendDataBean designateSendDataBean) {
        this.designateSendDataBean = designateSendDataBean;
    }

    public void setDesignatedNumber(int designatedNumber) {
        this.designatedNumber = designatedNumber;
    }
}
