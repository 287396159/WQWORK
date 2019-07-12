using CiXinLocation.bean;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiXinLocation.Model
{
    class LostPackFormModel:FromBaseModel
    {
        private string tag = "";

        private int reveDataCount = 0;
        private List<LocationBean> LocationBeans;
        private int locaCount = 3;
        private LostIntBean loIntBean;

        public LostPackFormModel()
        {
            receVeByteHandle = 0xfe;
            receVeByteend = 0xfd;
            //this.scourDataInter = scourDataInter;
            tag = XwDataUtils.GetTimeStamp(false).ToString() + new Random().Next(1024).ToString() + "LostPackFormModel";
            LocationBeans = new List<LocationBean>();
            loIntBean = new LostIntBean();
        }


        public override void reveData(SendDataType sType)
        {

        }

        public override void reveData(byte[] buf, string ip)
        {
            if (buf.Length != 15 || buf[1] != 0x03) return;
            //reveDataCount++;
            loIntBean.LocationReve++;
            LocationBean loBean = new LocationBean(buf);
            LocationBeans.Add(loBean);
        }


        /// <summary>
        /// 是否是丢包处理
        /// </summary>
        private void dealLost(LocationBean newLoBean)
        {

            /*List<LocationBean> dealLs = getNoDealLocationBean(newLoBean.TagId);
            byte minIndex = getminIndex(newLoBean.TagId);
            if (dealLs == null || dealLs.Count == 0) return;
            // loIntBean
            if (minIndex > newLoBean.MIndex && minIndex > 0xfe && newLoBean.MIndex < 0x05) {
                lostCountRe(dealLs, newLoBean.MIndex, 0xff);
                lostCountRe(dealLs, 0, newLoBean.MIndex);
            }
            else if (minIndex < newLoBean.MIndex)
                lostCountRe(dealLs,minIndex,newLoBean.MIndex);*/
                //loIntBean.
        }

        private void lostCountRe(List<LocationBean> dealLs,int minIndex,int maxIndex)
        {
            if (minIndex > maxIndex) return;
            //foreach()
        }

        /// <summary>
        /// 获取保存的没有处理过的LocationBean
        /// </summary>
        /// <returns></returns>
        private List<LocationBean> getNoDealLocationBean(byte[] tagID) {
            List<LocationBean> noDeal = new List<LocationBean>();
            foreach (LocationBean item in LocationBeans)
            {
                if (item.IsDeal) continue;
                if (!XWUtils.byteBTBettow(item.TagId, tagID, 2)) continue;
                noDeal.Add(item);
            }
            return noDeal;
        }

        private byte getminIndex(byte[] tagID)
        {
            byte minIndex = 0;
            foreach (LocationBean item in LocationBeans)
            {
                if (!item.IsDeal) continue;
                if (!XWUtils.byteBTBettow(item.TagId, tagID, 2)) continue;
                if (item.MIndex > minIndex) minIndex = item.MIndex;
            }
            return minIndex;
        }

        public override void close() {
            //if (scourDataInter != null) scourDataInter = null;
        }

        public override string TAG()
        {
            return tag;
        }

    }
}
