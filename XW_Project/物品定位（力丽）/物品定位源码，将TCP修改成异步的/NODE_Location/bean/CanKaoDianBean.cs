using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CiXinLocation.bean
{
    [Serializable]
    public class CanKaoDianBean
    {
        private string name;
        private string id;
        private string quYuname;
        private string cenJiname;
        private string quYuID;
        private string cenJiID;
        private Point point;
        private byte[] canDianID;
        private int peopleCount = 0;
        private int[] colWeiHei;
        private long timeReceive;      

        public CanKaoDianBean()
        {
            colWeiHei = new int[2]{1,1};
        }

        public CanKaoDianBean(CanKaoDianBean cbean)
        {
            if (cbean == null) return;
            Name = cbean.Name;
            Id = cbean.Id;
            POint = cbean.POint;
            CanDianID = cbean.CanDianID;
            QuYuname = cbean.QuYuname;
            CenJiname = cbean.CenJiname;
            QuYuID = cbean.QuYuID;
            CenJiID = cbean.CenJiID;
            ColWeiHei = cbean.ColWeiHei;
        }
        //
        public long TimeReceive
        {
            get { return timeReceive; }
            set { timeReceive = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int PeopleCount
        {
            get { return peopleCount; }
            set { peopleCount = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public Point POint
        {
            get { return point; }
            set { point = value; }
        }

        public int[] ColWeiHei
        {
            get { return colWeiHei; }
            set { colWeiHei = value; }
        }

        public byte[] CanDianID
        {
            get { return canDianID; }
            set { canDianID = value; }
        }


        public string QuYuname
        {
            get { return quYuname; }
            set { quYuname = value; }
        }

        public string CenJiname
        {
            get { return cenJiname; }
            set { cenJiname = value; }
        }

        public string QuYuID
        {
            get { return quYuID; }
            set { quYuID = value; }
        }

        public string CenJiID
        {
            get { return cenJiID; }
            set { cenJiID = value; }
        }

    }
}
