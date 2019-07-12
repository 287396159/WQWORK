package com.dmatek.setting.dialog;

import android.app.Activity;
import android.app.Dialog;
import android.content.Context;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.util.Log;
import android.view.View;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.dmatek.seting.dmatekseting.R;

public class MessageDialog extends Dialog {
    private TextView contentTxt;
    private TextView titleTxt;
    private TextView submitTxt;
    private TextView cancelTxt;

    private Context mContext;
    private String content;
    private OnCloseListener listener;
    private String positiveName;
    private String negativeName;
    private String title;
    ProgressBar progressBar;
    TextView msgTextView;
    private boolean isCallBack = true;
    private boolean isSend = false;

    public MessageDialog(Context context) {
        super(context);
        this.mContext = context;
    }

    public MessageDialog(Context context, int themeResId, String content) {
        super(context, themeResId);
        this.mContext = context;
        this.content = content;
    }

    public MessageDialog(Context context, int themeResId, String content, OnCloseListener listener) {
        super(context, themeResId);
        this.mContext = context;
        this.content = content;
        this.listener = listener;
    }

    protected MessageDialog(Context context, boolean cancelable, OnCancelListener cancelListener) {
        super(context, cancelable, cancelListener);
        this.mContext = context;
    }

    public MessageDialog setTitle(String title){
        this.title = title;
        return this;
    }

    public MessageDialog setPositiveButton(String name){
        this.positiveName = name;
        return this;
    }

    public MessageDialog setNegativeButton(String name){
        this.negativeName = name;
        return this;
    }

    public void errorViewVisibist(String msg){
        progressBar.setVisibility(View.GONE);
        msgTextView.setText("未知錯誤"+msg);
        disPlayDissmis(2000);
    }

    /**
     * Dialog.show,一定要show，否則會報空指針異常，因為show后，
     * onCreate才開始跑
     * @param dis
     */
    public void viewStartVisi(final int dis){
        progressBar.setVisibility(View.VISIBLE);
        msgTextView.setText("发送中，请稍等...");
        isCallBack = false;
        new Thread(new Runnable() {
            @Override
            public void run() {
                isSend = true;
                for (int i = 0;i < 10;i++){
                    try {
                        Thread.sleep(dis/10);
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                    if (isCallBack) return;
                }
                Message message = new Message();
                message.what = 5;
                handler.sendMessage(message);
            }
        }).start();

    }

    public void okViewVisi(){
        if (!this.isShowing() ){
            okViewSHowThread();
        }
        isCallBack = true;
        isSend  = false;
        progressBar.setVisibility(View.GONE);
        msgTextView.setText("ok");
        disPlayDissmis(1000);
    }

    public void okViewSHowThread(){
        new Thread(new Runnable() {
            @Override
            public void run() {
                for (int i = 0;i < 10;i++){
                    try {
                        Thread.sleep(100);
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                    if (isShowing() && isSend) break;
                }
                isSend  = false;
                if (isShowing()){
                    Message message = new Message();
                    message.what = 6;
                    handler.sendMessage(message);
                }
            }
        }).start();
    }

    public void failViewVisi(){
        progressBar.setVisibility(View.GONE);
        msgTextView.setText("發送失敗，請稍後再試");
        disPlayDissmis(2000);
    }

    /**
     * 延時dis毫秒,然後自動關閉Dialog
     * @param dis
     */
    private void disPlayDissmis(final int dis){
        new Thread(new Runnable() {
            @Override
            public void run() {
                for (int i = 0;i < 10;i++){
                    try {
                        Thread.sleep(dis/10);
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                }
                Message message = new Message();
                message.what = 4;
                handler.sendMessage(message);
            }
        }).start();
    }

    Handler handler = new Handler(){
        @Override
        public void handleMessage(Message msg) {
            super.handleMessage(msg);
            switch (msg.what){
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    isCallBack = true;
                    dismiss();
                    break;
                case 5:
                    failViewVisi();
                    break;
                case 6:
                    okViewVisi();
                    break;
            }
        }
    };

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.dialog_msg_layout);
        progressBar = findViewById(R.id.dialog_progressBar);
        msgTextView = findViewById(R.id.dialog_msg);
        findViewById(R.id.dialog_textView).setOnClickListener(onClickListener);
        findViewById(R.id.dialog_ok).setOnClickListener(onClickListener);
        setCanceledOnTouchOutside(false);
        viewStartVisi(2000);
    }


    public interface OnCloseListener{
        void onClick(Dialog dialog, boolean confirm);
    }

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {
            dismiss();
        }
    };


}
