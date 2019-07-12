using SerialportSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersionAutoLocaSys.Bean
{
    /// <summary>
    /// 出入长数据
    /// </summary>
    [Serializable]
    public class AdmissionExit
    {
        /// <summary>
        /// 入场
        /// </summary>
        public static String ADMISSION = "入場";

        /// <summary>
        /// 出厂
        /// </summary>
        public static String EXIT = "出場";

        /// <summary>
        /// 是否是Admission（入场），还是Exit出厂
        /// </summary>
        private String model;

        private long time;
        private String tagID;
        private String name;
        private byte[] workIDbyte;
        private byte[] tagIDbyte;

        /// <summary>
        /// AdmissionExit的二进制数据，前4位为时间，中间16位是打卡ID，最后2位是卡片ID
        /// </summary>
        /// <returns></returns>
        public byte[] backAdminssionExitBinary()
        {           
            byte[] timeBt = XWUtils.longToFourByte(Time);
            byte[] bt = new byte[timeBt.Length + workIDbyte.Length + tagIDbyte.Length+1];
            Array.Copy(timeBt, 0, bt, 0, timeBt.Length);
            Array.Copy(workIDbyte, 0, bt, timeBt.Length, workIDbyte.Length);
            Array.Copy(tagIDbyte, 0, bt, timeBt.Length + workIDbyte.Length, tagIDbyte.Length);
            bt[bt.Length - 1] = (byte)(Model.Equals(AdmissionExit.ADMISSION) ? 0 : 1);
            return bt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void setHisAdminssionExit(byte[] data)
        {
            if (data.Length < 23) 
            {
                Exception ex = new Exception("AdminssionExit长度错误！！");
                return;
            } 
            Time = data[0] << 24 | data[1] << 16 | data[2] << 8 | data[3];
            workIDbyte = new byte[16];
            Array.Copy(data, 4, workIDbyte, 0, 16);
            tagIDbyte = new byte[2];
            Array.Copy(data, 20, tagIDbyte, 0, 2);
            TagID = tagIDbyte[0].ToString("X2") + tagIDbyte[1].ToString("X2");
            Name = TagID;
            Model = data[data.Length - 1] == 0 ? AdmissionExit.ADMISSION : AdmissionExit.EXIT;

            Tag CurTag = null;
            try
            {
                CommonCollection.Tags.TryGetValue(TagID, out CurTag);
            }
            catch (Exception)
            {
                return;
            }
            if (CurTag != null) 
            {
                Name = CurTag.Name;
            }
        }

        public AdmissionExit(String model) 
        {
            this.model = model;
        }

        public String Model
        {
            get { return model; }
            set { model = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public long Time
        {
            get { return time; }
            set { time = value; }
        }

        public String TagID
        {
            get { return tagID; }
            set { tagID = value; }
        }

        public byte[] WorkIDbyte
        {
            get { return workIDbyte; }
            set { workIDbyte = value; }
        }

        public String WorkIDStr
        {
            get {
                StringBuilder suder = new StringBuilder();
                if (workIDbyte == null) return "";
                for (int i = 0; i < workIDbyte.Length;i++ )
                {
                    suder.Append(workIDbyte[i].ToString("X2"));
                }
                return suder.ToString();
            }
        }

        public String AeTime
        {
            get { return XwDataUtils.dataFromTimeStamp((Int32)Time); }
        }

        public byte[] TagIDbyte
        {
            get { return tagIDbyte; }
            set { tagIDbyte = value; }
        }


        //private void 
    }
}
