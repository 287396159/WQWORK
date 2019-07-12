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

        public long CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        public bool IsReturn {
            get { return isReturn; }
            set { isReturn = value; }
        }


    }
}
