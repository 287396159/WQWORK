using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiXinLocation.bean
{
    class UDPDATA_IP
    {
        private int length;

        public int Length
        {
            get { return length; }
            set { length = value; }
        }
        private string ip;

        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }

    }
}
