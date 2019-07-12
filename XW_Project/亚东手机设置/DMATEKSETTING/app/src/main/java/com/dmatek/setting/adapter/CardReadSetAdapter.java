package com.dmatek.setting.adapter;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.TextView;

import com.dmatek.seting.dmatekseting.R;
import com.dmatek.setting.bean.DrivaceInfoBean;
import com.dmatek.setting.bean.ReadSetViewBean;
import com.dmatek.setting.enums.GetSetType;

import java.util.ArrayList;
import java.util.List;

public class CardReadSetAdapter extends RecyclerView.Adapter<RecyclerView.ViewHolder>  {

    public final static int NOMAL_READ_SET = 1;
    public final static int MODEL_SET_NOMAL_READ = 2;
    public final static int TOP_VIEW = 3;

    // 当前上下文对象
    Context context;
    // RecyclerView填充Item数据的List对象
    List<ReadSetViewBean> datas;
    DrivaceInfoBean titleDatas;
    View.OnClickListener setValueClickLinster;
    View.OnClickListener readValueClickLinster;
    View.OnClickListener resetClickLinster;

    public CardReadSetAdapter(Context context, List<ReadSetViewBean> mReadSetMaps, DrivaceInfoBean topInfor){
        this.context = context;
        this.datas = mReadSetMaps;
        titleDatas = topInfor;
    }

    @Override
    public RecyclerView.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view;
        RecyclerView.ViewHolder viewHolder = null;
        //实例化得到Item布局文件的View对象
        switch (viewType){
            case NOMAL_READ_SET:
                view = getView(R.layout.item_read_set_layout);
                viewHolder = new NomalRSViewHolder(view);
                break;
            case MODEL_SET_NOMAL_READ:
                view = getView(R.layout.item_model_rs_layout);
                viewHolder = new ModelRSViewHolder(view);
                break;
            case TOP_VIEW:
                view = getView(R.layout.item_top_view_layout);
                viewHolder = new TopViewHolder(view);
                if (titleDatas.getMainDrivaceType() == 3){
                    ((TopViewHolder) viewHolder).resetBtn.setVisibility(View.GONE);
                }
                break;
            default:
                //view = getView(R.layout.item_read_set_layout);
                //viewHolder = new NomalRSViewHolder(view);
                break;
        }
        //返回MyViewHolder的对象
        return viewHolder;
    }

    /**
     * 用来引入布局的方法
     */
    private View getView(int view) {
        View view1 = View.inflate(context, view, null);
        return view1;
    }

    @Override
    public int getItemViewType(int position) {
        if (position == 0) return TOP_VIEW;
        return datas.get(position-1).getViewType();
    }

    @Override
    public void onBindViewHolder(RecyclerView.ViewHolder holder, int position) {
        if(position > datas.size()+1){
            return;
        }
        if (position == 0){
            if (holder instanceof TopViewHolder){
                TopViewHolder topHodel = (TopViewHolder)holder;
                topReadSetData(topHodel,titleDatas);
            }
            return;
        }
        ReadSetViewBean readView = datas.get(position-1);
        if (holder instanceof NomalRSViewHolder){
            NomalRSViewHolder noHodel = (NomalRSViewHolder)holder;
            setNomalReadSetData(noHodel,readView,position);
        }else if (holder instanceof ModelRSViewHolder){
            ModelRSViewHolder modelHodel = (ModelRSViewHolder)holder;
            setModelReadSetData(modelHodel,readView,position);
        }
    }

    /**
     * 把對應的數據放入到nomal版的item中
     * @param noHodel View的集合類
     * @param readView  數據源
     */
    private void setNomalReadSetData(NomalRSViewHolder noHodel, ReadSetViewBean readView, int position){
        noHodel.readEditText.setText("");
        if(readView.getEditReadText() == null || readView.getEditReadText().isEmpty()){
            noHodel.readEditText.setHint(readView.getEditReadHint());
        }else {
            noHodel.readEditText.setText(readView.getEditReadText());
        }if(readView.getEditSetText() == null || readView.getEditSetText().isEmpty()){
            noHodel.setEditText.setHint(readView.getEditSetHint());
        }else {
            noHodel.setEditText.setText(readView.getEditSetText());
        }if (readView.getToastVisi() == View.VISIBLE){
            noHodel.toastText.setText(readView.getToast());
        }
        noHodel.setButton.setText(readView.getBtnSetName()+readView.getEditSetHint());
        noHodel.setButton.setTag(position - 1);
        noHodel.setButton.setClickable(true);
        //WiFi_LAST_CONNECTION_View(noHodel,readView);
        noHodel.readBtn.setText(readView.getBtnReadName()+readView.getEditSetHint());
        noHodel.readBtn.setTag(position - 1);
        noHodel.readBtn.setClickable(true);
        //WIFI_RESSI_View(noHodel,readView);
    }

    private void WiFi_LAST_CONNECTION_View(NomalRSViewHolder noHodel, ReadSetViewBean readView){
        if (readView.getGetsetType() == GetSetType.WIFI_LAST_CONNECTION_TYPE){
            noHodel.setButton.setVisibility(View.GONE);
            noHodel.setEditText.setVisibility(View.GONE);
        }else{
            noHodel.setButton.setVisibility(View.VISIBLE);
            noHodel.setEditText.setVisibility(View.VISIBLE);
        }
    }

    private void WIFI_RESSI_View(NomalRSViewHolder noHodel, ReadSetViewBean readView){
        if (readView.getGetsetType() == GetSetType.WIFI_RESSI){
            noHodel.readBtn.setVisibility(View.GONE);
            noHodel.readEditText.setVisibility(View.GONE);
            noHodel.toastText.setVisibility(View.VISIBLE);
            String toast = "result:";
            if(readView.getEditReadText() != null){
                toast+=readView.getEditReadText();
            }
            noHodel.toastText.setText(toast);
        }else{
            noHodel.readBtn.setVisibility(View.VISIBLE);
            noHodel.readEditText.setVisibility(View.VISIBLE);
            noHodel.toastText.setVisibility(readView.getToastVisi());
            noHodel.toastText.setText("ok");
        }
    }

    /**
     * 把對應的數據放入到Model版的item中
     * @param modelHodel View的集合類
     * @param readView 數據源
     */
    private void setModelReadSetData(ModelRSViewHolder modelHodel, ReadSetViewBean readView, int position){
        if(readView.getEditReadText() == null || readView.getEditReadText().isEmpty())
        {
            modelHodel.readEditText.setHint(readView.getEditReadHint());
        }else {
            modelHodel.readEditText.setText(readView.getEditReadText());
        }
        modelHodel.modelTextView.setText(readView.getEditSetHint());
        modelHodel.setButton.setText(readView.getBtnSetName()+readView.getEditSetHint());
        modelHodel.setButton.setTag(position - 1);
        modelHodel.setButton.setClickable(true);
        modelHodel.readBtn.setText(readView.getBtnReadName()+readView.getEditSetHint());
        modelHodel.readBtn.setTag(position - 1);
        modelHodel.readBtn.setClickable(true);
        if (readView.getToastVisi() == View.VISIBLE){
            modelHodel.toastText.setText(readView.getToast());
        }
    }

    private void topReadSetData(TopViewHolder topHodel, DrivaceInfoBean drivaceInfoBean){
        topHodel.cardName.setText("設備名稱："+drivaceInfoBean.getStringTagID());
        topHodel.cardVersioin.setText("設備版本："+drivaceInfoBean.getDrivaceVersionString());
        topHodel.cardType.setText("設備類型："+drivaceInfoBean.getDrivaceTypeString());
    }

    @Override
    public int getItemCount() {
        return datas.size()+1;
    }

    // 继承RecyclerView.ViewHolder抽象类的自定义ViewHolder
    public class NomalRSViewHolder extends RecyclerView.ViewHolder{
        public EditText setEditText;
        Button setButton;
        public TextView toastText;
        Button readBtn;
        public EditText readEditText;

        public NomalRSViewHolder(View itemView) {
            super(itemView);
            setEditText = itemView.findViewById(R.id.item_readSet_set_edittext);
            setButton = itemView.findViewById(R.id.item_read_set_setBtn);
            toastText = itemView.findViewById(R.id.item_read_set_toastTView);
            readBtn = itemView.findViewById(R.id.item_read_set_readBtn);
            readEditText = itemView.findViewById(R.id.item_read_set_readEtext);
            setButton.setOnClickListener(setValueClickLinster);
            readBtn.setOnClickListener(readValueClickLinster);
        }
    }

    public class ModelRSViewHolder extends RecyclerView.ViewHolder{
        public Spinner modelSpinner;
        Button setButton;
        TextView toastText;
        Button readBtn;
        EditText readEditText;
        TextView modelTextView;

        public ModelRSViewHolder(View itemView) {
            super(itemView);;
            modelSpinner = itemView.findViewById(R.id.item_model_set_spinner);
            setButton = itemView.findViewById(R.id.item_model_setBtn);
            toastText = itemView.findViewById(R.id.item_model_toastTView);
            readBtn = itemView.findViewById(R.id.item_model_readBtn);
            readEditText = itemView.findViewById(R.id.item_model_readEtext);
            modelTextView = itemView.findViewById(R.id.item_model_spinnerTextView);

            List<String> list = new ArrayList<>();
            for (int i = 0;i < 16;i++){
                list.add(i+"");
            }
            final ArrayAdapter<String> adapter = new ArrayAdapter<String>(context,
                    android.R.layout.simple_spinner_item, list);
            adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
            modelSpinner.setAdapter(adapter);

            setButton.setOnClickListener(setValueClickLinster);
            readBtn.setOnClickListener(readValueClickLinster);
        }
    }

    public class TopViewHolder extends RecyclerView.ViewHolder{

        TextView cardVersioin;
        TextView cardName;
        TextView cardType;
        Button resetBtn;

        public TopViewHolder(View itemView) {
            super(itemView);
            cardVersioin = itemView.findViewById(R.id.item_top_view_version_tv);
            cardName = itemView.findViewById(R.id.item_top_view_drivaceIdText);
            cardType = itemView.findViewById(R.id.item_top_view_drivceType_tv);
            resetBtn = itemView.findViewById(R.id.item_top_view_reset_btn);
            resetBtn.setOnClickListener(resetClickLinster);
        }
    }

    public void setSetValueClickLinster(View.OnClickListener setValueClickLinster) {
        this.setValueClickLinster = setValueClickLinster;
    }

    public void setReadValueClickLinster(View.OnClickListener readValueClickLinster) {
        this.readValueClickLinster = readValueClickLinster;
    }

    public void setResetClickLinster(View.OnClickListener resetClickLinster) {
        this.resetClickLinster = resetClickLinster;
    }
}
