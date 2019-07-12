package com.dmatek.setting.activity;

import android.Manifest;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.design.widget.BottomNavigationView;
import android.support.v4.app.ActivityCompat;
import android.support.v4.app.Fragment;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.MenuItem;
import android.widget.Toast;

import com.dmatek.setting.fragments.CanKaoDianFragment;
import com.dmatek.setting.fragments.CardFragment;
import com.dmatek.setting.fragments.MyFragment;
import com.dmatek.setting.fragments.NODEFragment;
import com.dmatek.setting.fragments.NODENoChannlFragment;
import com.dmatek.seting.dmatekseting.R;
import com.dmatek.setting.servers.CommunicationServer;
import com.dmatek.setting.utils.BottomNavigationViewHelper;

public class MainActivity extends AppCompatActivity {


    private  String className= "MenuActivity";
    //  private ViewPager viewPager;
    String msg = "Android : ";
    //继承Activity 不会显示APP头上的标题
    private Fragment f1,f2,f3,f4;
    //USB_Utils usbUtils;

    private BottomNavigationView bottomNavigationView;
    private NODEFragment fragment1;
    private CanKaoDianFragment fragment2;
    private CardFragment fragment3;
    private MyFragment fragment4;
    private NODENoChannlFragment fragment5;
    private Fragment[] fragments;
    private int lastfragment;//用于记录上个选择的Fragment
    private int REQUEST_WRITE_EXTERNAL_STORAGE = 1;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_ok_main_layout);
        initFragment();
        checkPermission();
    }

    private void checkPermission() {
        //检查权限（NEED_PERMISSION）是否被授权 PackageManager.PERMISSION_GRANTED表示同意授权
        if (ActivityCompat.checkSelfPermission(this, Manifest.permission.WRITE_EXTERNAL_STORAGE)
                != PackageManager.PERMISSION_GRANTED) {
            //用户已经拒绝过一次，再次弹出权限申请对话框需要给用户一个解释
            if (ActivityCompat.shouldShowRequestPermissionRationale(this, Manifest.permission
                    .WRITE_EXTERNAL_STORAGE)) {
                Toast.makeText(this, "请开通相关权限，否则无法正常使用本应用！", Toast.LENGTH_SHORT).show();
            }
            //申请权限
            ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.WRITE_EXTERNAL_STORAGE}, REQUEST_WRITE_EXTERNAL_STORAGE);
        } else {
            Toast.makeText(this, "授权成功！", Toast.LENGTH_SHORT).show();
            Log.e("file", "checkPermission: 已经授权！");
        }
    }

    @Override
    protected void onResume() {
        super.onResume();
        //usbUtils.isCheckUsbDevice();
        //Log.i("server", "启动服务,打开串口");
        Intent intent = new Intent(this,CommunicationServer.class);
        this.startService(intent);
    }

    @Override
    protected void onRestart() {
        super.onRestart();
    }

    //初始化fragment和fragment数组
    private void initFragment()
    {
        fragment1 = new NODEFragment();
        //fragment5 = new NODENoChannlFragment();
        fragment2 = new CanKaoDianFragment();
        fragment3 = new CardFragment();
        fragment4 = new MyFragment();

        fragments = new Fragment[]{fragment1,fragment2,fragment3,fragment4};
        lastfragment=0;

        getSupportFragmentManager().beginTransaction().replace(R.id.mainview_ok,fragment1).show(fragment1).commit();
        bottomNavigationView=(BottomNavigationView)findViewById(R.id.navigation_te);
        BottomNavigationViewHelper.disableShiftMode(bottomNavigationView);
        bottomNavigationView.setOnNavigationItemSelectedListener(changeFragment);
    }
    //判断选择的菜单
    private BottomNavigationView.OnNavigationItemSelectedListener changeFragment= new BottomNavigationView.OnNavigationItemSelectedListener() {
        @Override
        public boolean onNavigationItemSelected(@NonNull MenuItem item) {
            switch (item.getItemId()){
                case R.id.navigation_home:
                    {
                    if(lastfragment!=0)
                    {
                        switchFragment(lastfragment,0);
                        lastfragment=0;
                    }
                    return true;
                }                case R.id.navigation_dashboard:
                {
                    if(lastfragment!=1)
                    {
                        switchFragment(lastfragment,1);
                        lastfragment=1;
                    }
                    return true;
                }
                case R.id.navigation_notifications:
                {
                    if(lastfragment!=2)
                    {
                        switchFragment(lastfragment,2);
                        lastfragment=2;
                    }
                    return true;
                }
                case R.id.navigation_dashboard2:
                {
                    if(lastfragment!=3)
                    {
                        switchFragment(lastfragment,3);
                        lastfragment=3;
                    }
                    return true;
                }
            }
            return false;
        }
    };

    //切换Fragment
    private void switchFragment(int lastfragment,int index) {
        android.support.v4.app.FragmentTransaction transaction = getSupportFragmentManager().beginTransaction();
        transaction.hide(fragments[lastfragment]);//隐藏上个Fragment
        if(fragments[index].isAdded()==false){
            transaction.add(R.id.mainview_ok,fragments[index]);
        }
        transaction.show(fragments[index]).commitAllowingStateLoss();
    }

}
