using CiXinLocation.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CiXinLocation.bean
{
    class DP_TYPE_LABEL
    {
        private DataPacketType dpType;
        private Label lab;
        private long createTime = 0;
        private bool isReturn = false;
        private int witeTime = 0;//等待时长

        public DP_TYPE_LABEL(DataPacketType dpType,Label lab)
        {
            this.dpType = dpType;
            this.lab = lab;
        }

        public DataPacketType DpType {
            get { return dpType; }
            set { }
        }

        public Label Lab {
            get { return lab; }
            set { }
        }

        /// <summary>
        /// 等待时间，单位秒
        /// </summary>
        public int WiteTime 
        {
            get { return witeTime; }
            set { witeTime = value*1000; }
        }

        /// <summary>
        /// 时间 = 发送的时间，
        /// witeTime是允许等待的时间
        /// </summary>
        public long CreateTime 
        {
            get { return createTime; }
            set { createTime = value + witeTime; }
        }

        public bool IsReturn {
            get { return isReturn; }
            set { isReturn = value; }
        }


    }
}
