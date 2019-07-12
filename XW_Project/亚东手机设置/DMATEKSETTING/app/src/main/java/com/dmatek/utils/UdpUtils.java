package com.dmatek.utils;

import android.util.Log;

import com.dmatek.setting.bean.ReadSetViewBean;
import com.dmatek.setting.bean.RecvDrivaceDataBean;
import com.dmatek.setting.enums.GetSetType;
import com.dmatek.setting.utils.FileReadSetUtils;
import com.dmatek.setting.utils.RecvDrivaceDataInfor;

import org.greenrobot.eventbus.EventBus;

import java.io.IOException;
import java.lang.reflect.Array;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.SocketAddress;
import java.net.SocketException;
import java.net.UnknownHostException;
import java.util.Arrays;

public class UdpUtils {

    DatagramPacket recvDatagramPacket = null;
    DatagramSocket socket = null;
    InetAddress serverAddress = null;
    private int sendPort = 51234;
    private boolean isOpen = false;
    private int port = 51234;
    private String udpIp;
    private Thread udpThread;


    public UdpUtils(){
        try {
            serverAddress = InetAddress.getByName("192.168.1.63");
        } catch (UnknownHostException e) {
            e.printStackTrace();
        }
    }

    public UdpUtils(int port,String ip){
        this();
        setPort(port);
        setUdpIp(ip);
    }

    public int getPort() {
        return port;
    }

    public void setPort(int port) {
        this.port = port;
    }

    public String getUdpIp() {
        return udpIp;
    }

    public boolean isOpen() {
        return isOpen;
    }

    public void setUdpIp(String udpIp) {
        this.udpIp = udpIp;
    }

    public  void closeUdp(){
        Log.i("UDP", "UDP关闭了 啊！！！！");
        udpThread.interrupt();
        socket.close();
        isOpen = false;
    }

    public void setSocket(){
        if(isOpen()) return;;
        try{
            socket = new DatagramSocket(getPort());
            isOpen = true;
            udpThread = new Thread(new Runnable() {
                @Override
                public void run() {
                    recvThread();
                }
            });
            udpThread.start();
        }catch (SocketException sex){
            Log.e("udpSocket", "setSocket: new DatagramSocket出现问题"+sex.getMessage());
        }
    }

    public void recvThread(){
        byte[] message = new byte[500];
        try{
            socket.setBroadcast(true);
        }catch (SocketException socket){
            Log.e("udpSocket", "socket.setBroadcast:"+socket.getMessage());
        }
        recvDatagramPacket = new DatagramPacket(message,message.length);
        try {
            sendFirstData();
            Log.i("UDP", "UDP开启了 啊！！！！");
            while (!Thread.currentThread().isInterrupted()) {
                // 准备接收数据
                try {
                    socket.receive(recvDatagramPacket);
                } catch (IOException e) {
                    e.printStackTrace();
                }
                Thread.sleep(0);
                byte[] recvData = Arrays.copyOf(recvDatagramPacket.getData(),recvDatagramPacket.getLength());
                if (recvDatagramPacket.getAddress()!= null)serverAddress = recvDatagramPacket.getAddress();
                sendPort = recvDatagramPacket.getPort();
                FileReadSetUtils.writeFile("收到數據："+XWUtils.toHexString(recvData));
                Log.i("UDP", "recvThread: ===>>>> "+Arrays.toString(recvData));
                revePortsData(recvData,"还不知道怎么调用IP");
            }
        } catch (InterruptedException e) {
            Log.i("InterruptedException", "run: searchDrivaceIDRunnable 中断了一次");
        }finally {
            Log.i("UDP", "UDP结束了 啊！！！！");
        }
    }

    private void sendFirstData(){
        try {
            byte[] data = new byte[]{(byte)0x05,(byte)0xfb,(byte)0xfb,(byte)0xfb,(byte)0xfb};
            DatagramPacket packet = new DatagramPacket(data, data.length, InetAddress.getByName("192.168.1.63"), 51234);
            socket.send(packet);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }


    public byte sendByteHandle = (byte)0xf2;  //发送包头
    public byte sendByteend = (byte)0xf1;     //发送包尾
    public byte receVeByteHandle = (byte)0xfc;//接收包头
    public byte receVeByteend = (byte)0xfb;   //接收包尾
    private int index_buf = 0;
    private byte[] bytesBuf = new byte[288];

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
                if (j - i > 72) break;;
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
        if (index+length > buffer.length) return;;
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
        RecvDrivaceDataInfor recvInfor = new RecvDrivaceDataInfor();
        byte[] id = new byte[]{buf[2],buf[3]};
        int bufLength = buf.length;
        byte[] DrivaceData = new byte[bufLength - 6];
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

    public void sendData(byte[] data){
        if (data == null ||  socket == null) return;
        Log.i("UDP", "sendData: data = "+Arrays.toString(data));
        FileReadSetUtils.writeFile("發送出去的數據："+XWUtils.toHexString(data));
        DatagramPacket packet = new DatagramPacket(data, data.length, serverAddress, sendPort);
        sendDataThread(packet);
    }

    private void sendDataThread(final DatagramPacket packet){
        new Thread(new Runnable() {
            @Override
            public void run() {
                try {
                    Log.i("UDP", "sendData: ip = "+packet.getAddress().toString()+",port="+packet.getPort()
                    +",data = "+Arrays.toString(packet.getData()));
                    if (socket!= null)socket.send(packet);
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        }).start();
    }

}
