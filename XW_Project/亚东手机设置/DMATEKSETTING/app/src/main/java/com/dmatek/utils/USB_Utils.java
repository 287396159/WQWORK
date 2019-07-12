package com.dmatek.utils;

import android.annotation.SuppressLint;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.hardware.usb.UsbManager;
import android.util.Log;
import android.view.View;
import android.widget.Toast;

import com.dmatek.seting.dmatekseting.R;
import com.dmatek.setting.bean.RecvDrivaceDataBean;
import com.dmatek.setting.dialog.MessageDialog;
import com.dmatek.setting.utils.FileReadSetUtils;
import com.dmatek.setting.utils.RecvDrivaceDataInfor;

import org.greenrobot.eventbus.EventBus;

import java.net.DatagramPacket;
import java.util.Arrays;

import ftapi.DataCallBack;
import ftapi.FtLib;
import ftapi.UntiTools;

/**
 * USB串口操作，DataCallBack是接受数据的接口
 */
public class USB_Utils implements DataCallBack{

    private FtLib mFtClient = null;
    public byte sendByteHandle = (byte)0xf2;  //发送包头
    public byte sendByteend = (byte)0xf1;     //发送包尾
    public byte receVeByteHandle = (byte)0xfc;//接收包头
    public byte receVeByteend = (byte)0xfb;   //接收包尾
    private int index_buf = 0;
    private byte[] bytesBuf = new byte[288];
    private  Context context;
    MessageDialog messageDialog = null;

    public USB_Utils(Context deviceContext){
        this.context = deviceContext;
        getFTLIB();
        messageDialog = new MessageDialog(this.context,R.style.DialogTheme,"提示");
    }

    private void getFTLIB(){
        mFtClient = FtLib.getInstance(this.context,this);
        filterUsbListener(this.context);
    }

    @SuppressLint("InlinedApi")
    private void filterUsbListener(Context deviceContext) {
        // TODO Auto-generated method stub
        IntentFilter filter = new IntentFilter();
        filter.addAction(UsbManager.ACTION_USB_DEVICE_ATTACHED);
        filter.addAction(UsbManager.ACTION_USB_DEVICE_DETACHED);
        filter.setPriority(500);
        deviceContext.getApplicationContext().registerReceiver(mUsbReceiver, filter);
    }

    private final BroadcastReceiver mUsbReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            String TAG = "FragL";
            String action = intent.getAction();
            if (UsbManager.ACTION_USB_DEVICE_DETACHED.equals(action)) {
                Log.e(TAG, "DETACHED...");
                notifyUSBDeviceDetach();
            } else if (UsbManager.ACTION_USB_DEVICE_ATTACHED.equals(action)) {
                Log.e(TAG, "ATTACHED...");
                notifyUSBDeviceAttach();
            }
        }
    };

    public void notifyUSBDeviceDetach() {
        if (mFtClient != null)
            mFtClient.disConnectFt();
    }

    public void notifyUSBDeviceAttach() {
        UntiTools.delay_ms(1000);
        isCheckUsbDevice();
    }

    public void isCheckUsbDevice() {
        int ret = 0;
        if (mFtClient != null)
            ret = mFtClient.createDeviceList();
        if (ret > 0) {
            OnPenFt();
            Toast.makeText(this.context, "ok", Toast.LENGTH_LONG).show();
        } else {
            Toast.makeText(this.context, "device  DETACHED", Toast.LENGTH_LONG).show();
        }
    }

    @Override
    public void method(char[] chars) {
        byte[] recvBuff = new byte[chars.length];
        for (int i = 0;i < chars.length;i++){
            recvBuff[i] = (byte) chars[i];
        }
        Log.i("UDP", "method: "+XWUtils.toHexString(recvBuff));
        FileReadSetUtils.writeFile("收到數據："+XWUtils.toHexString(recvBuff));
        revePortsData(recvBuff,"串口接收的数据");
    }

    /// <summary>
    /// 接收数据开始处,处理原始数据过程，结果就在reveData处有，
    /// 没收到结果，那就是数据包不对，胎死腹中
    /// </summary>
    /// <param name="buf"></param>
    public void revePortsData(byte[] buf,String ip) {
        if (buf == null || buf.length < 1) return;
        //if (index_buf == 0 && buf[0] != receVeByteHandle) return;
        if (buf[0] == receVeByteHandle && buf[buf.length - 1] == receVeByteend) //是一组包头包尾
        {
            if (buf[buf.length - 2] == XWUtils.getCheckBit(buf, 0, buf.length - 2))
            {//校验也正确
                index_buf = 0;
                reveData(buf,ip);
                return;
            }
        }
        if(index_buf == 0) bytesBuf = new byte[288];//先清理，后面才好添加数据
        if (bytesBuf.length <= buf.length + 38) {
            index_buf = 0;
            bytesBuf = new byte[288];
            if (bytesBuf.length < buf.length) return;
        }
        else if (index_buf + buf.length >= bytesBuf.length) {
            byte[] chacheData = new byte[38];
            System.arraycopy(bytesBuf,index_buf - 38,chacheData,0,38);
            bytesBuf = new byte[288];
            index_buf = 38;
            System.arraycopy(chacheData,0,bytesBuf,0,38);
        }
        System.arraycopy(buf,0,bytesBuf,index_buf,buf.length);
        findPack(bytesBuf,ip);

        index_buf += buf.length;
        if (index_buf >= bytesBuf.length) index_buf = 0;
    }

    /// <summary>
    /// 将缓存的数据中符合要求的数据包解析出来
    /// </summary>
    /// <param name="buf"></param>
    /// <param name="index"></param>
    private void findPack(byte[] buf,String ip){
        for (int i = 0; i < buf.length;i++ ) {
            if (buf[i] != receVeByteHandle) continue; //只要包头
            for (int j = i; j < buf.length; j++) {
                if (j - i > 72) break;
                if (buf[j] != receVeByteend) continue;//只要包尾
                byte[] packData = new byte[j - i + 1];
                System.arraycopy(buf,i,packData,0,packData.length);
                if (checkPacket(packData,ip)){
                    clearArray(buf,i,j-i); //校验，并且清理掉
                }
            }
        }
    }

    /**
     * 将buffer 中的一系列元素设置为零
     * @param buffer 需要清除其元素
     * @param index 要清除的一系列元素的起始索引
     * @param length 要清除的元素数。
     */
    private void clearArray(byte[] buffer,int index,int length){
        if (index+length > buffer.length) return;
        int bufferIndex = index;
        int clearLength = length;
        while(clearLength > 0){
            buffer[bufferIndex] = 0;
            bufferIndex++;
            clearLength--;
        }
    }

    private Boolean checkPacket(byte[] checkBuf,String udpIp) {
        byte check = XWUtils.getCheckBit(checkBuf, 0, checkBuf.length-2);
        if (checkBuf[checkBuf.length - 2] == check) {
            reveData(checkBuf,udpIp);
            return true;
        }
        return false;
    }

    /**
     * 讲过一系列的操作，这个方法每次buf都是一个标准的长聚微嵌定义的协议包
     * 函数switch的buf[1]在相关协议文档中，都是可以找到的，请查阅文档
     * @param buf  发送的标准数据包
     * @param udpIp  发送过来的节点IP
     */
    public void reveData(byte[] buf,String udpIp){
        Log.i("UDP", "reveData: buf = "+XWUtils.toHexString(buf));
        RecvDrivaceDataInfor recvInfor = new RecvDrivaceDataInfor();
        byte[] id = new byte[]{buf[2],buf[3]};
        int bufLength = buf.length;
        byte[] DrivaceData = null;
        if (bufLength > 6)DrivaceData = new byte[bufLength - 6];
        else DrivaceData = new byte[0];
        byte drivaceD = 0;
        if (DrivaceData.length == 1){
            drivaceD = buf[4];
        } else{
            System.arraycopy(buf,4,DrivaceData,0,DrivaceData.length);
        }
        RecvDrivaceDataBean recvDrivaceDataBean = new RecvDrivaceDataBean();
        recvDrivaceDataBean.setBackDt(drivaceD)
                .setDrivaceID(id)
                .setBackDatas(DrivaceData)
                .setPackType(buf[1]);
        EventBus.getDefault().post(recvDrivaceDataBean);
    }

    public void sendData(final byte[] data){
//        Toast.makeText(context, "send:"+XWUtils.toHexString(data), Toast.LENGTH_LONG).show();
        new Thread(new Runnable() {
            @Override
            public void run() {
                Log.i("UDP", "sendData: data = "+XWUtils.toHexString(data));
                FileReadSetUtils.writeFile("發送出去的數據："+XWUtils.toHexString(data));
                int ret = mFtClient.SendMessage(data, data.length);
                if (ret > 0) {
                    Log.i("UDP", "sendData:Ok data = "+XWUtils.toHexString(data));
                } else {
                    Log.i("UDP", "sendData: 失败 = "+XWUtils.toHexString(data));
                }
            }
        }).start();
    }

    public void OnSendFt(View view) {
        byte[] sendbyte = new byte[]{(byte)0xf2,0x01,(byte)0xf3,(byte)0xf1};
    }

    public void setSocket(){
        getFTLIB();
        isCheckUsbDevice();
        OnPenFt();
    }

    public void closeUdp(){
        notifyUSBDeviceDetach();
    }

    public void OnPenFt() {
        int ret = -1;
        try{
            if (mFtClient != null) ret = mFtClient.connectFt();
        }catch (IllegalStateException Illse){
            Log.i("sp", "OnPenFt: "+Illse.getMessage());
        }
        if (ret == 0 || ret == -3) {
            ret = mFtClient.setConfig(115200, (byte) 8, (byte) 0, (byte) 0, (short) 0);
            if (ret == 0) { }
        }

    }

}
