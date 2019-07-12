using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiXinLocation.bean
{
    class IPIDRessBean
    {
        private string ipKey;
        private byte[] ipKeyBt;
        private byte[] tagID;
        private byte[] portIDRes;
        private List<string> ipKeys;
        private int index = 0;
       

        public IPIDRessBean() 
        {
            ipKeys = new List<string>();
        }

        public IPIDRessBean(IPIDRessBean item)
        {
            ipKeys = new List<string>();
            ipKeyBt = item.IpKeyBt;
            ipKey = item.IpKey;
            tagID = item.TagID;
        }

        public List<string> IpKeys
        {
            get { return ipKeys; }
            set { ipKeys = value; }
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        
        public byte[] getIDbyte() 
        {
            byte[] by = new byte[5];
            Array.Copy(tagID, 0, by,0,2);
            Array.Copy(portIDRes, 0, by, 2, 2);
            return by;
        }

        public byte[] PortIDRes
        {
            get { return portIDRes; }
            set { portIDRes = value; }
        }

        public byte[] TagID
        {
            get { return tagID; }
            set { tagID = value; }
        }

        public string TagIDStr
        {
            get {
                if (tagID == null) return "";
                return tagID[0].ToString("X2") + tagID[1].ToString("X2"); }
        }

        public byte[] IpKeyBt
        {
            get { return ipKeyBt; }
            set { ipKeyBt = value; }
        }

        public string IpKey
        {
            get { return ipKey; }
            set { ipKey = value; }
        }

    }
}
