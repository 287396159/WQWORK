package com.dmatek.setting.fragments;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.hardware.usb.UsbManager;
import android.net.Uri;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.text.style.TtsSpan;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;

import com.dmatek.seting.dmatekseting.R;
import com.dmatek.setting.DesignatedNumberToSendThread;
import com.dmatek.setting.bean.DrivaceInfoBean;
import com.dmatek.setting.utils.FileReadSetUtils;

import org.greenrobot.eventbus.EventBus;

import java.util.ArrayList;
import java.util.List;

import ftapi.FtLib;
import ftapi.UntiTools;

public class MyFragment extends Fragment {

    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";
    private String context="xxxxxxxxxxxxx";
    private TextView mTextView;
    private Toolbar toolbar;
    TextView tv;

    //要显示的页面
    private int FragmentPage;
    public  static  MyFragment newInstance(String context,int iFragmentPage){
        MyFragment myFragment = new MyFragment();
        myFragment.context = context;
        myFragment.FragmentPage = iFragmentPage;
        return  myFragment;
    }

    private String mParam1;
    private String mParam2;

    //private HomeFragment.OnFragmentInteractionListener mListener;
    //初始化数据

    public MyFragment() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment HomeFragment.
     */
    public static MyFragment newInstance(String param1, String param2) {
        MyFragment fragment = new MyFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);

        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.activity_set_read_layout, container, false);
        toolbar = (Toolbar) view.findViewById(R.id.activity_r_s_toolbar);
        ((AppCompatActivity) getActivity()).setSupportActionBar(toolbar);
        return view;
    }

    @Override
    public void onActivityCreated(@Nullable Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);
        Button button = getActivity().findViewById(R.id.openBtn);
        Button openFileBtn = getActivity().findViewById(R.id.getFileBtn);
        button.setOnClickListener(onClickListener);
        openFileBtn.setOnClickListener(onOpenFileClickListener);
        tv = getActivity().findViewById(R.id.text_cotent);
        getActivity().findViewById(R.id.clearBtn).setOnClickListener(onClearClickListener);
    }


    View.OnClickListener onClearClickListener = new View.OnClickListener(){
        @Override
        public void onClick(View v) {
            FileReadSetUtils.clearFile(FileReadSetUtils.WritePath);
        }
    };


    View.OnClickListener onOpenFileClickListener = new View.OnClickListener(){
        @Override
        public void onClick(View v) {
            String fileMsg = FileReadSetUtils.getFileContent(FileReadSetUtils.ReadPath);
             tv.setText(fileMsg);
        }
    };


    View.OnClickListener onClickListener = new View.OnClickListener(){
        @Override
        public void onClick(View v) {
            FileReadSetUtils.writeFile("按了一次开关：");
            Button button = (Button)v;
            if ("打開".equals(button.getText())){
                //OnPenFt();
                button.setText("關閉");
                EventBus.getDefault().post("openUdp");
            }else if ("關閉".equals(button.getText())){
                button.setText("打開");
                EventBus.getDefault().post("closeUdp");
            }
        }
    };

    // TODO: Rename method, update argument and hook method into UI event
    /*public void onButtonPressed(Uri uri) {
        if (mListener != null) {
            mListener.onFragmentInteraction(uri);
        }
    }*/


    /*@Override
    public void onAttach(Context context) {
        super.onAttach(context);
        if (context instanceof HomeFragment.OnFragmentInteractionListener) {
            mListener = (HomeFragment.OnFragmentInteractionListener) context;
        } else {
            throw new RuntimeException(context.toString()
                    + " must implement OnFragmentInteractionListener");
        }
    }*/

    @Override
    public void onDetach() {
        super.onDetach();
        //mListener = null;
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


    private FtLib mFtClient = null;



    @Override
    public void onDestroy() {
        super.onDestroy();
    }

}
