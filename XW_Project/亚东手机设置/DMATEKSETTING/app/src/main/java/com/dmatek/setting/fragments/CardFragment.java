package com.dmatek.setting.fragments;

import android.graphics.Color;
import android.os.Bundle;
import android.support.v4.widget.SwipeRefreshLayout;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.dmatek.seting.dmatekseting.R;
import com.dmatek.setting.utils.SendDataUtils;

public class CardFragment extends BaseFragment {

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.activity_drivaceid_layout, container, false);
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

    @Override
    public void initData() {
        searchDrivaceID = (byte)0x80;
        searchDVersion = (byte)0x81;
        refreshTime = 1000;
    }

    @Override
    public byte[] searchDriInfor(byte[] id, byte channel) {
        return SendDataUtils.sendSearchCardInfor(id);
    }

    @Override
    public byte[] sendSearchID() {
        return SendDataUtils.sendSearchCardId();
    }
}
