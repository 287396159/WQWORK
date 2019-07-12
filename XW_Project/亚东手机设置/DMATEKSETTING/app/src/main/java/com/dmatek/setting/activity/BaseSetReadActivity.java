package com.dmatek.setting.activity;

import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.View;
import android.widget.Toast;

import com.dmatek.seting.dmatekseting.R;
import com.dmatek.setting.adapter.ReadSetViewAdapter;
import com.dmatek.setting.bean.DrivaceInfoBean;
import com.dmatek.setting.bean.ReadSetLabel;
import com.dmatek.setting.bean.ReadSetViewBean;
import com.dmatek.setting.bean.RecvDrivaceDataBean;
import com.dmatek.setting.bean.SendDataBean;
import com.dmatek.setting.dialog.MessageDialog;
import com.dmatek.setting.enums.GetSetType;
import com.dmatek.setting.utils.DrivaceReadSetUtils;
import com.dmatek.setting.utils.FileReadSetUtils;
import com.dmatek.utils.XWUtils;

import org.greenrobot.eventbus.EventBus;
import org.greenrobot.eventbus.Subscribe;
import org.greenrobot.eventbus.ThreadMode;

import java.util.ArrayList;
import java.util.List;

public abstract class BaseSetReadActivity extends AppCompatActivity {

    List<ReadSetViewBean> mReadSetMaps;
    RecyclerView recyclerView;
    LinearLayoutManager linearLayoutManager;
    ReadSetViewAdapter rsViewAdapter;
    DrivaceInfoBean drivaceInfoBean;
    public ReadSetLabel[] readSetLabels;
    public byte resertByte = 0x0d;
    MessageDialog mMyDialog;

    {
        readSetLabels = DrivaceReadSetUtils.getAllGetSetCKDType();
        mReadSetMaps = new ArrayList<ReadSetViewBean>();
        DrivaceReadSetUtils.readSetCanKaoDianMap(mReadSetMaps);
        initData();
    }

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_node_setting_layout);
        EventBus.getDefault().register(this);
        initView();
        mMyDialog = new MessageDialog(BaseSetReadActivity.this,R.style.DialogTheme,"提示");
    }

    public abstract void initData();

    private void initView(){
        drivaceInfoBean = (DrivaceInfoBean) getIntent().getSerializableExtra("DrivaceInfoBean");
        onCreaterAafter();
        recyclerView = findViewById(R.id.node_get_set_recyclerView);
        //设置LayoutManager为LinearLayoutManager
        linearLayoutManager = new LinearLayoutManager(this);
        recyclerView.setLayoutManager(linearLayoutManager);
        //设置Adapter
        rsViewAdapter = new ReadSetViewAdapter(this,mReadSetMaps,drivaceInfoBean);
        rsViewAdapter.setReadValueClickLinster(readValueClickLinster);
        rsViewAdapter.setSetValueClickLinster(setValueClickLinster);
        rsViewAdapter.setResetClickLinster(resetClickLinster);
        recyclerView.setAdapter(rsViewAdapter);
    }

    View.OnClickListener resetClickLinster = new View.OnClickListener(){
        @Override
        public void onClick(View v) {
            byte[] sendData = getReadBaseBt(resertByte,drivaceInfoBean.getTagID(),drivaceInfoBean.getChannel());
            SendDataBean sendDataBean = new SendDataBean();
            sendDataBean.setPackData(resertByte);
            sendDataBean.setSendDt(sendData);
            startWaitDialogShow(3);
            EventBus.getDefault().post(sendDataBean);
        }
    };

    View.OnClickListener setValueClickLinster = new View.OnClickListener(){
        @Override
        public void onClick(View v) {
            int position = (int)v.getTag();
            RecyclerView.ViewHolder viewHolder = getViewItemHodel(position);

            if (null == viewHolder) {
                Log.e("setValueClickLinster", "--- onClick:  viewHolder为null--- " );
                startWaitDialogShow(2);
                return; // 既然不存在，应该是哪里出现问题
            }
            startWaitDialogShow(3);
            String setTest = "";
            ReadSetViewBean rsData = mReadSetMaps.get(position);
            if(viewHolder instanceof ReadSetViewAdapter.NomalRSViewHolder){
                ReadSetViewAdapter.NomalRSViewHolder nomalViewHolder =
                        (ReadSetViewAdapter.NomalRSViewHolder)viewHolder;
                setTest = nomalViewHolder.setEditText.getText().toString();
            }else if (viewHolder instanceof ReadSetViewAdapter.ModelRSViewHolder){
                ReadSetViewAdapter.ModelRSViewHolder modelViewHolder = (ReadSetViewAdapter.ModelRSViewHolder)viewHolder;
                setTest = (String) modelViewHolder.modelSpinner.getSelectedItem();
            }
            byte[] sendDt = DrivaceReadSetUtils.getDrivcePackTypeSetData(rsData.getGetsetType(),setTest);
            if (sendDt != null)
            {
                setSend(rsData,sendDt);
                mReadSetMaps.get(position).setEditSetText(setTest);
            }
            else {
                Toast.makeText(getApplicationContext(),"錯誤：輸入數值有誤",Toast.LENGTH_LONG).show();
                Log.i("UDP", "onClick: "+rsData.toString());
            }
        }
    };

    private void startWaitDialogShow(final int whit){
        mMyDialog.show();
        new Thread(new Runnable() {
            @Override
            public void run() {
                for (int i = 0;i < 10;i++){
                    try {
                        if (mMyDialog.isShowing())break;
                        Thread.sleep(100);
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                }
                Message message = new Message();
                message.what = whit;
                handler.sendMessage(message);
            }
        }).start();
    }

    private void setSend(ReadSetViewBean rsData,byte[] sendSetDt){
        byte packType = DrivaceReadSetUtils.getDrivaceTypeInGetSetType(rsData.getGetsetType(),1);
        byte[] sendDt = getSetBaseBt(packType,drivaceInfoBean.getTagID(),drivaceInfoBean.getChannel(),sendSetDt);

        SendDataBean sendDataBean = new SendDataBean();
        sendDataBean.setPackData(packType);
        sendDataBean.setSendDt(sendDt);

        EventBus.getDefault().post(sendDataBean);

    }

    /**
     * 界面点击读取按钮时，事件处理
     */
    View.OnClickListener readValueClickLinster = new View.OnClickListener(){
        @Override
        public void onClick(View v) {
            int position = (int)v.getTag();
            RecyclerView.ViewHolder viewHolder = getViewItemHodel(position);
            if (null == viewHolder) {
                Log.e("setValueClickLinster", "--- onClick:  viewHolder为null--- " );
                return; // 既然不存在，应该是哪里出现问题
            }
            startWaitDialogShow(3);
            ReadSetViewBean rsData = mReadSetMaps.get(position);
            readSend(rsData);
        }
    };

    /**
     * 獲取指定標誌位position的RecyclerView.ViewHolder
     * @param position
     * @return
     */
    private RecyclerView.ViewHolder getViewItemHodel(int position){
        int firstItemPosition = linearLayoutManager.findFirstVisibleItemPosition();
        if (position - firstItemPosition < 0) {
            return null;  ///下标出现异常，退出
        }
        // 得到要更新的item的view
        View view = recyclerView.getChildAt(position - firstItemPosition+1);
        RecyclerView.ViewHolder viewHolder = recyclerView.getChildViewHolder(view);
        return viewHolder;
    }

    /**
     * 发送，读取命令
     * @param rsData
     */
    private void readSend(ReadSetViewBean rsData){
        byte packType = DrivaceReadSetUtils.getDrivaceTypeInGetSetType(rsData.getGetsetType(),2);
        byte[] sendData = getReadBaseBt(packType,drivaceInfoBean.getTagID(),drivaceInfoBean.getChannel());

        SendDataBean sendDataBean = new SendDataBean();
        sendDataBean.setPackData(packType);
        sendDataBean.setSendDt(sendData);

        EventBus.getDefault().post(sendDataBean);
    }

    public abstract byte[] getSetBaseBt(byte pack,byte[] id,byte channel,byte[] date);

    public abstract byte[] getReadBaseBt(byte pack,byte[] id,byte channel);
    public abstract void onCreaterAafter();

    //定义处理接收的方法
    @Subscribe(threadMode = ThreadMode.POSTING)
    public void userEventBus(RecvDrivaceDataBean recvDrivaceDataBean) {
        if (mReadSetMaps == null || mReadSetMaps.size() == 0) return;
        FileReadSetUtils.writeFile("界面接收到的數據："+recvDrivaceDataBean.toString());
        int index = 0;
        byte packType = recvDrivaceDataBean.getPackType();
        int pType = XWUtils.uns(packType);
        //if (pType > 0x50 || pType < 0x40) return; // 非节点，不考虑
        Message message = new Message();
        message.what = 1;
        if (pType == resertByte){
            handler.sendMessage(message); // 将Message对象发送出去
            return;
        }
        if (pType == 0x02){
            byte[] infor = recvDrivaceDataBean.getBackDatas();
            if (infor.length > 5) {
                byte channel = infor[5];
                drivaceInfoBean.setChannel(channel);
            }
            return;
        }
        if (pType >= readSetLabels.length) {
            message.what = 4;
            handler.sendMessage(message); // 将Message对象发送出去
            return;
        }

        ReadSetLabel readSetLabel = readSetLabels[XWUtils.uns(packType)];
        index = queryReadSetIndex(readSetLabel.getGetSetType());
        if (index == -1) return;

        if (readSetLabel.getRsType() == ReadSetLabel.SET) {
            //mReadSetMaps.get(index).setToastVisi(View.VISIBLE);
        } else if (readSetLabel.getRsType() == ReadSetLabel.READ) {
            String msg = recvDrivaceDataBean.getDiffBackString(getDrivaceTypeInt());
            mReadSetMaps.get(index).setEditReadText(msg);
        }
        handler.sendMessage(message); // 将Message对象发送出去
    }

    /**
     * 获取节点设备类型代号，1=有线，0=无线
     * @return
     */
    public int getDrivaceTypeInt(){
        int drivaceType = 0;
        byte driType = drivaceInfoBean.getDrivaceType();
        drivaceType = (driType == 1|| driType == 4)?0:1;
        return drivaceType;
    }

    Handler handler = new Handler(){
        @Override
        public void handleMessage(Message msg) {
            switch (msg.what){
                case 1:
                    mMyDialog.okViewVisi();
                    rsViewAdapter.notifyDataSetChanged();
                    break;
                case 2:
                    mMyDialog.errorViewVisibist("viewHolder=null");
                    break;
                case 3:
                    mMyDialog.viewStartVisi(1000);
                    break;
                case 4:
                    mMyDialog.errorViewVisibist("接受類型錯誤");
                    break;
                default:
                    break;
            }
        }
    };

    /**
     * 查找mReadSetMaps中元素的下标
     * @param getSetType
     * @return
     */
    private int queryReadSetIndex(GetSetType getSetType){
        if (mReadSetMaps == null) return -1;
        for (int i = 0;i < mReadSetMaps.size();i++){
            if (getSetType == mReadSetMaps.get(i).getGetsetType()){
                return i;
            }
        }
        return -1;
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        EventBus.getDefault().unregister(this);
    }
}
