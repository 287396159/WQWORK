package com.dmatek.setting.fragments;

import android.graphics.Color;
import android.net.Uri;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v4.widget.SwipeRefreshLayout;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.dmatek.seting.dmatekseting.R;
import com.dmatek.setting.DesignatedNumberToSendThread;
import com.dmatek.setting.adapter.DrivaceIdAdapter;
import com.dmatek.setting.bean.DrivaceInfoBean;
import com.dmatek.setting.bean.RecvDrivaceDataBean;
import com.dmatek.setting.bean.SendDataBean;
import com.dmatek.setting.linster.EndlessRecyclerOnScrollListener;
import com.dmatek.setting.utils.FileReadSetUtils;
import com.dmatek.setting.utils.SendDataUtils;
import com.dmatek.utils.XWUtils;

import org.greenrobot.eventbus.EventBus;
import org.greenrobot.eventbus.Subscribe;
import org.greenrobot.eventbus.ThreadMode;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Timer;
import java.util.TimerTask;

public abstract class BaseFragment extends Fragment {

    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";
    private String context="xxxxxxxxxxxxx";
    private TextView mTextView;
    public Toolbar toolbar;
    public SwipeRefreshLayout swipeRefreshLayout;
    public RecyclerView refashRecyclerView;
    public DrivaceIdAdapter driAdapter;
    public boolean isChannel = false;
    public byte searchDrivaceID = 0x41;
    public byte searchDVersion = 0x42;
    public int refreshTime = 3000;

    //要显示的页面
    private int FragmentPage;
    /*public  static  BaseFragment newInstance(String context,int iFragmentPage){
        BaseFragment myFragment = new BaseFragment();
        myFragment.context = context;
        myFragment.FragmentPage = iFragmentPage;
        return  myFragment;
    }*/
    private String mParam1;
    private String mParam2;

    private OnFragmentInteractionListener mListener;
    //初始化数据
    List<DrivaceInfoBean> datas = new ArrayList<>();
    DesignatedNumberToSendThread searchDriIDThread;

    public BaseFragment() {
        // Required empty public constructor
    }

    /*public static BaseFragment newInstance(String param1, String param2) {
        BaseFragment fragment = new BaseFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }*/

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
        initData();

    }

    public abstract  void initData();


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.activity_drivaceid_layout, container, false);
        //startData(view,container);
        findView(view);
        initView();
        return view;
    }

    public void findView(View view){
        toolbar = (Toolbar) view.findViewById(R.id.toolbar);

        swipeRefreshLayout = (SwipeRefreshLayout)  view.findViewById(R.id.swipe_refresh_layout);
        refashRecyclerView = (RecyclerView)  view.findViewById(R.id.drivceRecView);

        // 使用Toolbar替换ActionBar
        ((AppCompatActivity) getActivity()).setSupportActionBar(toolbar);
        //setSupportActionBar(toolbar);

        // 设置刷新控件颜色
        swipeRefreshLayout.setColorSchemeColors(Color.parseColor("#4DB6AC"));
    }

    private void initData(SendDataBean designateSendDataBean){
        if (searchDriIDThread != null) searchDriIDThread.interrupt();
        searchDriIDThread = new DesignatedNumberToSendThread();
        searchDriIDThread.setDesignateSendDataBean(designateSendDataBean);
    }

    public void initView() {
        // 模拟获取数据
        //getData();
        driAdapter = new DrivaceIdAdapter(getActivity().getApplicationContext(),datas);

        refashRecyclerView.setLayoutManager(new LinearLayoutManager(getActivity().getApplicationContext()));
        refashRecyclerView.setAdapter(driAdapter);

        // 设置下拉刷新
        swipeRefreshLayout.setOnRefreshListener(onRefreshListener);

        // 设置加载更多监听
        refashRecyclerView.addOnScrollListener(onScrollListener);
    }

    /**
     * 下拉刷新事件处理
     */
    SwipeRefreshLayout.OnRefreshListener onRefreshListener = new SwipeRefreshLayout.OnRefreshListener() {
        @Override
        public void onRefresh() {
            // 刷新数据
            FileReadSetUtils.writeFile("下拉刷新了一次："+searchDrivaceID);
            datas.clear();
            driAdapter.notifyDataSetChanged();
            initData(searchDrivaceID());
            try{
                if (searchDriIDThread != null) searchDriIDThread.start();
            }catch (IllegalThreadStateException Illse){
            }
            // 延时1s关闭下拉刷新
            swipeRefreshLayout.postDelayed(refreshRunnab, refreshTime);
        }
    };

    /**
     * 下拉刷新，限时关闭线程
     */
    Runnable refreshRunnab = new Runnable() {
        @Override
        public void run() {
            if (swipeRefreshLayout != null && swipeRefreshLayout.isRefreshing()) {
                swipeRefreshLayout.setRefreshing(false);
            }
        }
    };

    /**
     * 上拉加载
     */
    EndlessRecyclerOnScrollListener onScrollListener = new EndlessRecyclerOnScrollListener() {
        @Override
        public void onLoadMore() {
            driAdapter.setLoadState(driAdapter.LOADING);
            initData(searchDrivaceID());
            try{
                if (searchDriIDThread != null) searchDriIDThread.start();
            }catch (IllegalThreadStateException Illse){
            }
            if (datas.size() < 30) {
                // 模拟获取网络数据，延时1s
                new Timer().schedule(timerTask, 1000);
            } else {
                // 显示加载到底的提示
                driAdapter.setLoadState(driAdapter.LOADING_END);
            }
        }
    };

    /**
     * 定时任务
     */
    TimerTask timerTask = new TimerTask() {
        @Override
        public void run() {
            getActivity().runOnUiThread(loadStateRunn); //线程转主线程
        }
    };

    /**
     * 底部页脚加载提示
     */
    Runnable loadStateRunn = new Runnable() {
        @Override
        public void run() {
            //getData();
            driAdapter.setLoadState(driAdapter.LOADING_COMPLETE);
        }
    };

    public void onButtonPressed(Uri uri) {
        if (mListener != null) {
            mListener.onFragmentInteraction(uri);
        }
    }

    @Override
    public void onActivityCreated(@Nullable Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);
    }

    @Override
    public void onDetach() {
        super.onDetach();
        mListener = null;
    }

    /**
     * This interface must be implemented by activities that contain this
     * fragment to allow an interaction in this fragment to be communicated
     * to the activity and potentially other fragments contained in that
     * activity.
     * <p>
     * See the Android Training lesson <a href=
     * "http://developer.android.com/training/basics/fragments/communicating.html"
     * >Communicating with Other Fragments</a> for more information.
     */
    public interface OnFragmentInteractionListener {
        void onFragmentInteraction(Uri uri);
    }

    private void getData(){
        for(int i=0;i<25;i++){
            DrivaceInfoBean DrivaceInfoBean = new DrivaceInfoBean();
            byte[] tagID = new byte[]{(byte)(i/0x100),(byte)(i%0x100)};
            byte[] version = new byte[]{1,2,(byte)(i/0x100),(byte)(i%0x100)};
            DrivaceInfoBean.setDrivaceType((byte)0x01);
            DrivaceInfoBean.setDrivaceVersion(version);
            DrivaceInfoBean.setTagID(tagID);
            datas.add(DrivaceInfoBean);
        }
    }

    private void startData(View view, ViewGroup container){
        getData();
        RecyclerView recyclerView = view.findViewById(R.id.drivceRecView);
        //设置LayoutManager为LinearLayoutManager
        recyclerView.setLayoutManager(new LinearLayoutManager(getActivity().getApplicationContext()));
        //设置Adapter
        recyclerView.setAdapter(new DrivaceIdAdapter(getActivity().getApplicationContext(),datas));
    }

    //定义处理接收的方法
    @Subscribe(threadMode = ThreadMode.MAIN)
    public void userEventBus(RecvDrivaceDataBean recvDrivaceDataBean){
        if (recvDrivaceDataBean == null) return;
        Log.i("UDP", "userEventBus: " +recvDrivaceDataBean.toString());
        FileReadSetUtils.writeFile("主界面接收到的數據："+recvDrivaceDataBean.toString());
        byte packType = recvDrivaceDataBean.getPackType();
        int index = 0;
        if (packType == searchDrivaceID){
            if (searchDriIDThread != null)searchDriIDThread.interrupt();
            byte[] bkDts = getRrecvData(recvDrivaceDataBean); // 组装一下ID
            boolean isAddNew = addDriID(recvDrivaceDataBean,bkDts); //判断是否有新的ID加入
            findDrivaceInforThread(isAddNew);  //查一下新ID 的信息
        }else if (packType == searchDVersion){
            byte[] drivaceID = recvDrivaceDataBean.getDrivaceID();
            addDrivaceInfor(drivaceID,recvDrivaceDataBean.getBackDatas());
            driAdapter.notifyDataSetChanged();
        }
    }

    private boolean addDriID(RecvDrivaceDataBean recvDrivaceDataBean, byte[] bkDts){
        int drivaceCount = recvDrivaceDataBean.getDrivaceID()[0];
        boolean isAddNew = false;
        int length = 2;
        if (isChannel) length = 3; // 是节点，就加上channel
        for (int i = 0; i < drivaceCount;i ++){
            int i_index = i*length;
            byte[] driBt = new byte[]{bkDts[i_index],bkDts[i_index+1],0};
            if (length == 3)driBt[2] = bkDts[i_index+2];
            if(addDrivaceID(driBt)) isAddNew = true; //添加设备操作，并且返回是否添加新元素
        }
        return isAddNew;
    }

    /**
     * 将RecvDrivaceDataBean中收到的ID数据，打包成需要的数组
     * @param recvDrivaceDataBean 源数据
     * @return
     */
    private byte[] getRrecvData(RecvDrivaceDataBean recvDrivaceDataBean){
        int drivaceCount = recvDrivaceDataBean.getDrivaceID()[0];
        byte[] bkDts = null;
        if (!isChannel && drivaceCount == 1){
            bkDts = new byte[2];
            bkDts[1] = recvDrivaceDataBean.getBackDt();
        }else {
            bkDts = new byte[recvDrivaceDataBean.getBackDatas().length + 1];
            System.arraycopy(recvDrivaceDataBean.getBackDatas(),0,bkDts,1,bkDts.length - 1);
        }
        bkDts[0] =recvDrivaceDataBean.getDrivaceID()[1];
        return bkDts;
    }

    /**
     * 开一个线程查询新收到的节点ID
     * @param isFind
     */
    private void findDrivaceInforThread(boolean isFind){
        if (isFind){ //添加了新元素，才继续查询新元素的版本信息
            driAdapter.notifyDataSetChanged();
            new Thread(new Runnable() {
                @Override
                public void run() {
                    findNoInforDrivaceAndSearchDrivaceInfor();
                }
            }).start();
        }
    }

    private void addDrivaceInfor(byte[] ID,byte[] infor){
        int index = getDrivaceInfoBeanIndex(ID);
        DrivaceInfoBean drivaceInfoBean = null;
        byte type = infor[0];
        byte[] version = new byte[]{infor[1],infor[2],infor[3],infor[4]};
        byte channel = 0x00;
        if (getCurrentDrivaceType() == 0x01) channel = infor[5];
        if (index < 0){
            drivaceInfoBean = new DrivaceInfoBean();
            drivaceInfoBean.setTagID(ID);
        }else{
            drivaceInfoBean = datas.get(index);
        }
        synchronized (datas){
            drivaceInfoBean.setChannel(channel);
            drivaceInfoBean.setMainDrivaceType(getCurrentDrivaceType());
            drivaceInfoBean.setDrivaceType(type);
            drivaceInfoBean.setDrivaceVersion(version);
            if(index < 0)datas.add(drivaceInfoBean);
        }
        Log.i("UDP", "baseFragment==>>infor: "+XWUtils.toHexString(infor));
    }

    /**
     * 添加上报的的ID
     * @param IDChannel 上报的ID
     * @return 是否添加了新的元素
     */
    private Boolean addDrivaceID(byte[] IDChannel){
        byte[] ID = new byte[]{IDChannel[0],IDChannel[1]};
        byte channel = IDChannel[2];
        int index = getDrivaceInfoBeanIndex(ID);
        boolean isAddNew = false;
        if (index < 0){
            isAddNew = true;
            DrivaceInfoBean drivaceInfoBean = new DrivaceInfoBean();
            drivaceInfoBean.setTagID(ID);
            drivaceInfoBean.setChannel(channel);
            synchronized (datas){
                datas.add(drivaceInfoBean);
            }
        }
        return isAddNew;
    }

    /**
     * 遍历集合，通过ID找到ID在集合的位置的下标
     * @param ID 设备ID
     * @return 集合的下标位置
     */
    private int getDrivaceInfoBeanIndex(byte[] ID){
        if (ID == null || ID.length != 2) return -1;
        for (int i = 0; i < datas.size();i++){
            if(Arrays.equals(datas.get(i).getTagID(),ID)){
                return  i;
            }
        }
        return -2;
    }

    /**
     * 找到存储的ID，但是没有设备信息的集合，并且搜寻设备信息
     */
    private void findNoInforDrivaceAndSearchDrivaceInfor(){
        final  List<DrivaceInfoBean> drivaceIDs = new ArrayList<>();
        synchronized (datas){
            for (int i = 0;i < datas.size();i++){
                if (datas.get(i).getDrivaceVersion() == null){
                    DrivaceInfoBean drivaceInfoBean = new DrivaceInfoBean();
                    drivaceInfoBean.setTagID(datas.get(i).getTagID());
                    drivaceInfoBean.setChannel(datas.get(i).getChannel());
                    drivaceIDs.add(drivaceInfoBean);
                }
            }
        }
        serchDrivaceInfor(drivaceIDs);
    }

    /**
     * 通过EventBus，将搜索设备ID命令，发送出去
     * @param drivaceIDs  设备ID集合
     */
    private void serchDrivaceInfor(List<DrivaceInfoBean> drivaceIDs){
        if (drivaceIDs == null) return;;
        for (int i = 0;i < drivaceIDs.size();i++){
            SendDataBean sendDataBean = new SendDataBean();
            sendDataBean.setPackData(searchDVersion);
            sendDataBean.setSendDt(searchDriInfor(drivaceIDs.get(i).getTagID(),drivaceIDs.get(i).getChannel()));
            EventBus.getDefault().post(sendDataBean);

            try {
                Thread.sleep(50);  //睡一下，留点时间，下位机好工作
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }

    public abstract  byte[] searchDriInfor(byte[] id,byte channel);

    private SendDataBean searchDrivaceID(){
        SendDataBean sendDataBean = new SendDataBean();
        sendDataBean.setPackData(searchDrivaceID);
        sendDataBean.setSendDt(sendSearchID());
        return sendDataBean;
    }

    public abstract byte[] sendSearchID();

    public byte getCurrentDrivaceType(){
        if (searchDrivaceID == 0x01) return 0x01;
        else if (searchDrivaceID == (byte)0x41) return 0x02;
        else if(searchDrivaceID == (byte)0x80) return 0x03;
        else return 0x04;
    }
//
    @Override
    public void onDestroy() {
        super.onDestroy();
        Log.i("UDP", "baseFragment==>>onDestroy: ");

    }

    @Override
    public void onResume(){
        super.onResume();
        EventBus.getDefault().register(this);
    }

    @Override
    public void onPause(){
        super.onPause();
        EventBus.getDefault().unregister(this);
    }
}
