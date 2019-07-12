using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiXinLocation.bean
{
    //参考点上报信息
    public class CanKaoDianUpInfo
    {
        public CanKaoDianUpInfo()
        {
            CID = new byte[2];
            Version = new byte[4];
            nodeType = new byte[4];
        }

        public CanKaoDianUpInfo(CanKaoDianUpInfo canInfo):this()
        {
            setValue(canInfo);      
        }

        private string ipInfo;

        public string IpInfo
        {
            get { return ipInfo; }
            set { ipInfo = value; }
        }


        private string ckdId;

        public string CkdId
        {
            get { return ckdId; }
            set { ckdId = value; }
        }
        private byte[] cID;

        public byte[] CID
        {
            get { return cID; }
            set { cID = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private byte[] nodeType;

        public byte[] NodeType
        {
            get { return nodeType; }
            set { nodeType = value; }
        }
        private byte[] version;

        public byte[] Version
        {
            get { return version; }
            set { version = value; }
        }
        private long upTime;

        public long UpTime
        {
            get { return upTime; }
            set { upTime = value; }
        }

        public void setValue(CanKaoDianUpInfo canInfo)
        {
            ckdId  = canInfo.CkdId;
            upTime = canInfo.UpTime;
            name = canInfo.Name;
            Array.Copy(canInfo.Version, 0, version, 0, version.Length);
            Array.Copy(canInfo.CID, 0, cID, 0, cID.Length);
            Array.Copy(canInfo.NodeType, 0, nodeType, 0, nodeType.Length);
        }


        public void removeVersionType()
        {
            upTime = 0;
            version = null;
            nodeType = null;
        }



    }

}
