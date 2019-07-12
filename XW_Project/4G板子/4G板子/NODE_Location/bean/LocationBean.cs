using SerialportSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiXinLocation.bean
{
    class LocationBean
    {
        private byte[] tagId = new byte[2];
        private byte[] locationID = new byte[2];
        private byte sinness;//灵敏度
        private byte mIndex; //下标
        private bool isDeal = false;
        private long time;

        /// <summary>
        /// 定位的封包
        /// </summary>
        /// <param name="buf"></param>
        public LocationBean(byte[] buf) {
            if (buf.Length != 15) return;
            Array.Copy(buf,2,tagId,0,2);
            Array.Copy(buf,4,locationID,0,2);
            sinness = buf[6];
            mIndex = buf[12];
            time = XwDataUtils.GetTimeStamp();
        }

        public byte[] TagId {
            get { return tagId; }
        }

        public byte[] LocationID
        {
            get { return locationID; }
        }

        public byte Sinness
        {
            get { return sinness; }
        }

        public byte MIndex
        {
            get { return mIndex; }
        }

        public bool IsDeal
        {
            get { return isDeal; }
            set { isDeal = value; }
        }

        public long Time
        {
            get { return time; }
        }

    }
}
