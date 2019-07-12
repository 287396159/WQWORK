package cj.tzw.uwb_m;

import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.graphics.Color;
import android.hardware.usb.UsbDevice;
import android.hardware.usb.UsbManager;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.alibaba.android.arouter.facade.annotation.Route;
import com.alibaba.android.arouter.launcher.ARouter;

import java.util.ArrayList;
import java.util.Collection;

import cj.tzw.base_uwb_m.adapter.DeviceAdapter;
import cj.tzw.base_uwb_m.callback.FtOcCallback;
import cj.tzw.base_uwb_m.model.Device;
import cj.tzw.base_uwb_m.router.RouterPathMethod;
import cj.tzw.base_uwb_m.utils.ByteUtil;
import cj.tzw.base_uwb_m.utils.DialogUtil;
import cj.tzw.base_uwb_m.utils.FtAnalysisUtil;
import cj.tzw.base_uwb_m.utils.FtBaseUtil;
import cj.tzw.base_uwb_m.utils.FtComUtil;
import cj.tzw.base_uwb_m.utils.FtHandleUtil;
import cj.tzw.base_uwb_m.utils.ToastUtil;
import io.reactivex.Observable;
import io.reactivex.ObservableSource;
import io.reactivex.functions.Consumer;
import io.reactivex.functions.Function;
import io.reactivex.functions.Predicate;

@Route(path = RouterPathMethod.LAUNCH_PATH)
public class MainActivity extends AppCompatActivity implements FtOcCallback{// implements DataCallBack
    private final String TAG = "MainActivity";
    private static final String ACTION_USB_PERMISSION = "com.android.example.USB_PERMISSION";
    private MainActivity main;
    private android.support.v7.widget.Toolbar toolbar;
    private TextView tvConnectStatus;
    private Button btnConnect,btnSreach;
    private ListView lvDevice;
    private UsbManager usbManager;
    private FtBaseUtil ftBaseUtil;
    private ArrayList<Device> deviceList;
    private boolean usbAttached=false;
    private boolean ftIsOpen = false;
    private boolean cancelOperate = false;
    private boolean hadDetached = false;
    //USB动作监控广播
    private BroadcastReceiver usbReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            String action = intent.getAction();
            if(action.equals(UsbManager.ACTION_USB_DEVICE_ATTACHED)){
                Log.i(TAG,"ACTION_USB_ACCESSORY_ATTACHED...");
                usbAttached();
            }else if(action.equals(UsbManager.ACTION_USB_DEVICE_DETACHED)){
                Log.i(TAG,"ACTION_USB_DEVICE_DETACHED...");
                usbDetached();
            }else if(action.equals(ACTION_USB_PERMISSION)){
                Log.i(TAG,"ACTION_USB_PERMISSION...");
                UsbDevice usbDevice = intent.getParcelableExtra(UsbManager.EXTRA_DEVICE);
                if (intent.getBooleanExtra(UsbManager.EXTRA_PERMISSION_GRANTED, false)) {  //允许权限申请
                    if (usbDevice != null) {
                        Log.i(TAG,"已经有权限了！");
                        checkUsbDevice();
                    } else {
                        Log.i(TAG,"未获取到设备信息！");
                    }
                } else {
                    Log.i(TAG,"用户未授权，读取失败！");
                }
            }
        }
    };

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        registerUsbBoardcast();
        initViewAndParam();
        initListener();
    }

    @Override
    public void onBackPressed() {
        cancelOperate = true;
        super.onBackPressed();
    }

    /**
     * 注册USB监控广播
     */
    public void registerUsbBoardcast(){
        IntentFilter iFilter = new IntentFilter();
        iFilter.addAction(UsbManager.ACTION_USB_DEVICE_ATTACHED);
        iFilter.addAction(UsbManager.ACTION_USB_DEVICE_DETACHED);
        registerReceiver(usbReceiver,iFilter);
        //注册监听自定义广播
        IntentFilter filter = new IntentFilter(ACTION_USB_PERMISSION);
        registerReceiver(usbReceiver, filter);
    }


    /**
     * 取消注册USB监控广播
     */
    public void unregisterUsbBoardcase(){
        unregisterReceiver(usbReceiver);
    }

    public void initViewAndParam(){
        main = this;
        tvConnectStatus = findViewById(R.id.tvConnectStatus);
        btnConnect = findViewById(R.id.btnConnect);
        btnSreach = findViewById(R.id.btnSreach);
        lvDevice = findViewById(R.id.lvDevice);
        usbManager = (UsbManager) getSystemService(USB_SERVICE);
        toolbar = findViewById(R.id.toolBar);
        setSupportActionBar(toolbar);
        checkUsbDevice();
    }

    /**
     * 控件监控事件
     */
    public void initListener(){
        btnConnect.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                //ARouter.getInstance().build(RouterPathMethod.ALERTER_SET_PATH).navigation();
                if(usbAttached){
                    checkUsbDevice();
                }else{
                    Toast.makeText(main,"请将USB插入设备！",Toast.LENGTH_SHORT).show();
                }
            }
        });
        btnSreach.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if(lvDevice.getChildCount()>0){
                    deviceList = new ArrayList<>();
                    lvDevice.setAdapter(null);
                }
                cancelOperate = false;
                DialogUtil.showWait(main,DialogUtil.WAIT_DIALOG,"正在搜索，请稍候...");
                FtHandleUtil ftHandleUtil = new FtHandleUtil(ftBaseUtil,null);
                ftHandleUtil.searchAllDevice();
            }
        });
        lvDevice.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                if(!usbAttached){
                    ToastUtil.toastShortMsg(main,"USB已拔掉，请重新插上后操作！");
                    return;
                }
                Device device = deviceList.get(position);
                if(device.getType()==1){
                    ARouter.getInstance().build(RouterPathMethod.ALERTER_SET_PATH).withParcelable("device",device).navigation();
                }else if(device.getType()==2){
                    ARouter.getInstance().build(RouterPathMethod.ALERTER_SET_PATH).withParcelable("device",device).navigation();
                }
            }
        });
    }

    /**
     * 检查USB是否有权限
     */
    public void checkUsbDevice(){
        Observable.just(usbManager.getDeviceList().values())
        .doOnError(new Consumer<Throwable>() {
            @Override
            public void accept(Throwable throwable) throws Exception {
                checkUsbDevice();
            }
        })
        .flatMap(new Function<Collection<UsbDevice>, ObservableSource<UsbDevice>>() {
            @Override
            public ObservableSource<UsbDevice> apply(Collection<UsbDevice> usbDevices) throws Exception {
                return Observable.fromArray(usbDevices.toArray(new UsbDevice[0]));
            }
        }).filter(new Predicate<UsbDevice>() {
            @Override
            public boolean test(UsbDevice usbDevice) throws Exception {
                return usbDevice.getProductName().contains("USB UART");
            }
        }).subscribe(new Consumer<UsbDevice>() {
            @Override
            public void accept(UsbDevice usbDevice) throws Exception {
                usbAttached = true;
                //检测该USB是否有权限
                if(usbManager.hasPermission(usbDevice)){
                    //打开串口
                    ftBaseUtil = FtBaseUtil.getInstance(getApplicationContext()).setFtOcCallback(main);
                    ftBaseUtil.openFt();
                    Log.i(TAG,"FT已经打开！");
                }else{
                    Log.i(TAG,"申请权限！");
                    //申请权限
                    usbManager.requestPermission(usbDevice,PendingIntent.getBroadcast(main, 0, new Intent(ACTION_USB_PERMISSION), 0));
                }
            }
        });
    }

    /**
     * USB插入
     */
    public void usbAttached(){
        usbAttached = true;
        if(hadDetached){
            Intent intent = new Intent(this,MainActivity.class);
            intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
            startActivity(intent);
            System.exit(0);
        }else
            checkUsbDevice();
    }

    /**
     * USB拔出
     */
    public void usbDetached(){
        hadDetached = true;
        usbAttached = false;
        changeViewStatus();
        if(ftBaseUtil !=null){
            ftBaseUtil.closeFt();
        }


    }

    /**
     * 改变控件的状态
     */
    public void changeViewStatus(){
        Log.i(TAG,"参数："+usbAttached+"-"+ftIsOpen);
        if(usbAttached){
            if(ftIsOpen){
                tvConnectStatus.setTextColor(Color.GREEN);
                tvConnectStatus.setText("当前状态：已连接");
                btnConnect.setText("disConnect");
                btnConnect.setEnabled(false);
                btnSreach.setEnabled(true);
                return;
            }
        }
        tvConnectStatus.setTextColor(Color.RED);
        tvConnectStatus.setText("当前状态：未连接");
        btnConnect.setText("connect");
        btnConnect.setEnabled(true);
        btnSreach.setEnabled(false);
    }


    @Override
    public void ftOpened() {
        Log.i(TAG,"ftOpened!"+usbAttached+"-"+ftIsOpen);
        ftIsOpen = true;
        changeViewStatus();
    }

    @Override
    public void ftOpenFail(String failMsg) {
        ftIsOpen = false;
        changeViewStatus();
    }

    @Override
    public void ftClosed() {
        Log.i(TAG,"ftClosed！");
        ftIsOpen = false;
    }

    @Override
    public void ftSended() {
        Log.i(TAG,"ftSended！");
    }

    @Override
    public void ftSendFail(String failMsg) {
        Log.i(TAG,"ftSendFail！");
        DialogUtil.showWait(main,DialogUtil.ERROR_DIALOG,"搜索失败！");
    }

    @Override
    public void ftRecevied(byte[] receviedBytes) {
        Log.i(TAG,"接受的内容："+ ByteUtil.bytesToHexFun3(receviedBytes));
        deviceList = FtAnalysisUtil.analysisDeviceListData(receviedBytes);
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                DialogUtil.showWait(main,DialogUtil.OK_DIALOG,"搜索完毕！");
                if(deviceList!=null){
                    lvDevice.setAdapter(new DeviceAdapter(main,deviceList));
                }
            }
        });
    }

    @Override
    public void ftRecevieOutTime() {
        if(cancelOperate){
            return;
        }
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                DialogUtil.showWait(main,DialogUtil.ERROR_DIALOG,"操作超时！");
            }
        });
    }

    @Override
    protected void onResume() {
        super.onResume();
        if(ftBaseUtil!=null){
            ftBaseUtil.setFtOcCallback(main);
        }
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        DialogUtil.releaseWait();
        unregisterUsbBoardcase();
    }
}
