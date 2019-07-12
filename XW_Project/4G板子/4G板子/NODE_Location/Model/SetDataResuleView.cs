using CiXinLocation.bean;
using CiXinLocation.CiXinInterface;
using CiXinLocation.Utils;
using SerialportSample;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CiXinLocation.Model
{ 
    /// 设置结果显示
    /// </summary>
    class SetDataResuleView
    {
        private static SetDataResuleView sdrView;
        private static object obj = new object();
        private static object objMain = new object();
        private bool isThreadOver = true;
        private static Hashtable listSetType;
        private LabelChangeInterface labInterface;
        private bool isClose = false;


        public static SetDataResuleView getInstance(){ //单例模式的对象，锁加双重判断。
            if (sdrView != null) return sdrView;
            lock (objMain)
            {
                if (sdrView == null) {
                    listSetType = new Hashtable();
                    sdrView = new SetDataResuleView();
                } 
                return sdrView;
            }                      
        }

        public void setLanINterface(LabelChangeInterface labInterface)
        {
            this.labInterface = labInterface;
        }

        public void insertSet(DP_TYPE_LABEL dptLabel)
        {
            if (dptLabel == null) return;
            dptLabel.CreateTime = XwDataUtils.GetTimeStamp(false);
            lock (obj) {
                if (listSetType.ContainsKey(dptLabel.DpType)) listSetType[dptLabel.DpType] = dptLabel;
                else listSetType.Add(dptLabel.DpType, dptLabel);    
            }            
            if (isThreadOver) new Thread(runThread).Start();
        }


        /// <summary>
        /// 成功返回数据时
        /// </summary>
        /// <param name="dptLabel"></param>
        public void returnSuccess(DP_TYPE_LABEL dptLabel)
        {
            lock (obj)
            {
                foreach (DP_TYPE_LABEL dptLabelItem in listSetType.Values)
                {
                    if (dptLabelItem == null) continue;
                    if (dptLabelItem.DpType != dptLabel.DpType) continue;

                    if (this.labInterface != null) labInterface.setUpdataResultLabelMain(dptLabel.Lab, "成功");
                    dptLabelItem.CreateTime = XwDataUtils.GetTimeStamp(false);
                    dptLabelItem.IsReturn = true;
                }
                
            }
        }


        private long getLongTime() { 
            long time = 0;
            foreach (DP_TYPE_LABEL dptLabelItem in listSetType.Values)
            {
                if (dptLabelItem == null) continue;
                if (dptLabelItem.CreateTime > time) time = dptLabelItem.CreateTime;
            }
            return time;
        }


        /// <summary>
        /// 运行的线程
        /// </summary>
        private void runThread() {
            isThreadOver = false;
            long currentTime = getLongTime();
            while (XwDataUtils.GetTimeStamp(false) < currentTime+3000 && !isClose)
            {
                try {
                    Thread.Sleep(300);
                }
                catch { }                
                lock (obj) {
                    foreach (DP_TYPE_LABEL dptLabelItem in listSetType.Values)
                    {
                        if (dptLabelItem == null) continue;
                        if (dptLabelItem.CreateTime + 1000 < XwDataUtils.GetTimeStamp(false)) longTimeChangeLab(dptLabelItem);
                    }
                    deleteValue();
                    currentTime = getLongTime();
                }               
            }
            isThreadOver = true;
        }


        /// <summary>
        /// 超时未收到返回数据
        /// </summary>
        /// <param name="dptLabel"></param>
        private void longTimeChangeLab(DP_TYPE_LABEL dptLabel)
        {
            if (dptLabel.IsReturn) return;
            if (this.labInterface != null) labInterface.setUpdataResultLabelMain(dptLabel.Lab, "失败");
            dptLabel.CreateTime = XwDataUtils.GetTimeStamp(false);
            dptLabel.IsReturn = true;
        }


        private void deleteValue() {
            List<DataPacketType> hashKeys = new List<DataPacketType>();
            foreach (DataPacketType dpTItem in listSetType.Keys)
            {
                if (!(listSetType[dpTItem] is DP_TYPE_LABEL)) continue;
                DP_TYPE_LABEL dptLabelItem = (DP_TYPE_LABEL)listSetType[dpTItem];
                if (dptLabelItem.IsReturn && dptLabelItem.CreateTime + 1000 < XwDataUtils.GetTimeStamp(false)) {
                    hashKeys.Add(dpTItem);
                    //listSetType.Remove(dpTItem);
                    if (this.labInterface != null) labInterface.enableFalse(dptLabelItem.Lab);
                } 
            }
            foreach (DataPacketType dpTItem in hashKeys)
            {
                listSetType.Remove(dpTItem);
            }
            hashKeys.Clear();
            hashKeys = null;
        }


        public void clear() {
            isClose = true;
            listSetType.Clear();
            labInterface = null;
            sdrView = null;
            
        }
    }

    public interface LabelChangeInterface
    {
        /// <summary>
        /// 改变lab的值
        /// </summary>
        /// <param name="lab"></param>
        /// <param name="text"></param>
        void setUpdataResultLabelMain(Label lab, string text);
        void enableFalse(Label lab);
    }
}
