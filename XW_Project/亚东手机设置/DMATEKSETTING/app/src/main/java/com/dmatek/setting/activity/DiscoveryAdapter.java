package com.dmatek.setting.activity;

import android.content.Context;
import android.content.Intent;
import android.support.annotation.NonNull;
import android.support.v7.widget.CardView;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.dmatek.seting.dmatekseting.R;
import com.dmatek.setting.adapter.DrivaceIdAdapter;
import com.dmatek.setting.bean.DrivaceInfoBean;

import java.util.List;

/**
 * 卡片ID列表adapter
 */
public class DiscoveryAdapter extends  RecyclerView.Adapter<RecyclerView.ViewHolder> {
    //当前上下文对象
    Context context;
    //RecyclerView填充Item数据的List对象
    List<DrivaceInfoBean> datas;

    // 普通布局
    private final int TYPE_ITEM = 1;
    // 脚布局
    private final int TYPE_FOOTER = 2;
    // 当前加载状态，默认为加载完成
    private int loadState = 2;
    // 正在加载
    public final int LOADING = 1;
    // 加载完成
    public final int LOADING_COMPLETE = 2;
    // 加载到底
    public final int LOADING_END = 3;

    public DiscoveryAdapter(Context context,List<DrivaceInfoBean> datas){
        this.context = context;
        this.datas = datas;
    }

    //创建ViewHolder
    @NonNull
    @Override
    public RecyclerView.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        // 通过判断显示类型，来创建不同的View
        if (viewType == TYPE_ITEM) {
            View view = LayoutInflater.from(parent.getContext())
                    .inflate(R.layout.item_discovery_adapter, parent, false);
            return new RecyclerViewHolder(view);
        } else if (viewType == TYPE_FOOTER) {
            View view = LayoutInflater.from(parent.getContext())
                    .inflate(R.layout.layout_refresh_footer, parent, false);
            return new FootViewHolder(view);
        }
        return null;

        //实例化得到Item布局文件的View对象
        //View v = View.inflate(context, R.layout.item_recy_drivaceid,null);
        //返回MyViewHolder的对象
        //return new MyViewHolder(v);
    }

    @Override
    public void onBindViewHolder(RecyclerView.ViewHolder holder, int position) {
        if (holder instanceof RecyclerViewHolder) {
            RecyclerViewHolder recyclerViewHolder = (RecyclerViewHolder) holder;
            DrivaceInfoBean drivaceInfoBean = datas.get(position);
            recyclerViewHolder.cardName.setText("設備名稱："+drivaceInfoBean.getStringTagID());
            recyclerViewHolder.cardVersioin.setText("設備版本："+drivaceInfoBean.getDrivaceVersionString());
            recyclerViewHolder.cardType.setText("設備類型："+drivaceInfoBean.getDrivaceTypeString());
            recyclerViewHolder.cardView.setTag(drivaceInfoBean);
            recyclerViewHolder.cardView.setOnClickListener(cardViewClick);
        } else if (holder instanceof FootViewHolder) {
            FootViewHolder footViewHolder = (FootViewHolder) holder;
            switch (loadState) {
                case LOADING: // 正在加载
                    footViewHolder.pbLoading.setVisibility(View.VISIBLE);
                    footViewHolder.tvLoading.setVisibility(View.VISIBLE);
                    footViewHolder.llEnd.setVisibility(View.GONE);
                    break;

                case LOADING_COMPLETE: // 加载完成
                    footViewHolder.pbLoading.setVisibility(View.INVISIBLE);
                    footViewHolder.tvLoading.setVisibility(View.INVISIBLE);
                    footViewHolder.llEnd.setVisibility(View.GONE);
                    break;

                case LOADING_END: // 加载到底
                    footViewHolder.pbLoading.setVisibility(View.GONE);
                    footViewHolder.tvLoading.setVisibility(View.GONE);
                    footViewHolder.llEnd.setVisibility(View.VISIBLE);
                    break;

                default:
                    break;
            }
        }
    }

    //绑定数据
    public void onBindViewHolder1(@NonNull RecyclerViewHolder holder, int position) {
        DrivaceInfoBean drivaceInfoBean = datas.get(position);
        holder.cardName.setText("設備名稱："+drivaceInfoBean.getStringTagID());
        holder.cardVersioin.setText("設備版本："+drivaceInfoBean.getDrivaceVersionString());
        holder.cardType.setText("設備類型："+drivaceInfoBean.getDrivaceTypeString());
        holder.cardView.setTag(drivaceInfoBean);
        holder.cardView.setOnClickListener(cardViewClick);
    }

    @Override
    public int getItemViewType(int position) {
        // 最后一个item设置为FooterView
        if (position + 1 == getItemCount()) {
            return TYPE_FOOTER;
        } else {
            return TYPE_ITEM;
        }
    }

    //返回Item的数量
    @Override
    public int getItemCount() {
        return datas.size()+1;
    }

    private class FootViewHolder extends RecyclerView.ViewHolder {

        ProgressBar pbLoading;
        TextView tvLoading;
        LinearLayout llEnd;

        FootViewHolder(View itemView) {
            super(itemView);
            pbLoading = (ProgressBar) itemView.findViewById(R.id.pb_loading);
            tvLoading = (TextView) itemView.findViewById(R.id.tv_loading);
            llEnd = (LinearLayout) itemView.findViewById(R.id.ll_end);
        }
    }

    // 继承RecyclerView.ViewHolder抽象类的自定义ViewHolder
    class RecyclerViewHolder extends RecyclerView.ViewHolder{
        TextView cardVersioin;
        TextView cardName;
        TextView cardType;
        CardView cardView;

        public RecyclerViewHolder(View itemView) {
            super(itemView);
            cardVersioin = itemView.findViewById(R.id.item_discovery_text);
            cardName = itemView.findViewById(R.id.item_discovery_drivaceIdText);
            cardType = itemView.findViewById(R.id.item_discovery_drivceType);
            cardView = itemView.findViewById(R.id.item_discovery_cardView);
        }
    }

    View.OnClickListener cardViewClick = new View.OnClickListener(){
        @Override
        public void onClick(View v) {
            Log.i("cardView", "onClick: cardViewClick");
            Log.i("cardView", (v instanceof CardView)+"");
            Object object = v.getTag();
            if (object instanceof DrivaceInfoBean ){
                DrivaceInfoBean drivaceInfoBean = (DrivaceInfoBean)object;
                Intent intent = new Intent(context,NODEReadSetActivity.class);
                intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK );
                intent.putExtra("DrivaceInfoBean",drivaceInfoBean);
                context.startActivity(intent);
            }
        }
    };


    @Override
    public void onAttachedToRecyclerView(RecyclerView recyclerView) {
        super.onAttachedToRecyclerView(recyclerView);
        RecyclerView.LayoutManager manager = recyclerView.getLayoutManager();
        if (manager instanceof GridLayoutManager) {
            final GridLayoutManager gridManager = ((GridLayoutManager) manager);
            gridManager.setSpanSizeLookup(new GridLayoutManager.SpanSizeLookup() {
                @Override
                public int getSpanSize(int position) {
                    // 如果当前是footer的位置，那么该item占据2个单元格，正常情况下占据1个单元格
                    return getItemViewType(position) == TYPE_FOOTER ? gridManager.getSpanCount() : 1;
                }
            });
        }
    }

    /**
     * 设置上拉加载状态
     *
     * @param loadState 0.正在加载 1.加载完成 2.加载到底
     */
    public void setLoadState(int loadState) {
        this.loadState = loadState;
        notifyDataSetChanged();
    }

}
