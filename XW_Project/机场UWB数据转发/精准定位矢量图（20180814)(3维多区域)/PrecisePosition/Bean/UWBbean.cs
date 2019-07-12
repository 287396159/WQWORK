using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PositionReceiveWin.Bean
{
    [DataContract]
    public class UWBbean
    {
        [DataMember]
        private Param mParam;
        [DataMember]
        private String dataTime;
        [DataMember]
        private String method;

        public UWBbean() { }

        public Param MParam
        {
            get{ return mParam;}
            set{ mParam = value;}
        }

        public String DataTime
        {
            get { return dataTime; }
            set { dataTime = value; }
        }

        public String Method
        {
            get { return method; }
            set { method = value; }
        }
    }

    [DataContract]
    public class Param 
    {
        [DataMember]
        String id;
        [DataMember]
        int x;
        [DataMember]
        int y;
        [DataMember]
        String vid;
        [DataMember]
        int dis;
        
        public Param() { }

        public Param(String id, int x, int y,int dis,String vid) 
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.dis = dis;
            this.vid = vid;
        }

        public String Id
        {
            get { return id; }
            set { id = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }
       
        public int Dis
        {
          get { return dis; }
          set { dis = value; }
        }

        public String Vid
        {
            get { return vid; }
            set { vid = value; }
        }
    }

}
