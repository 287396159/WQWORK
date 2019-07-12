using SerialportSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiXinLocation.bean
{
    public class LocationNode
    {

        private byte[] ID_c;
        private byte[] ID_j;
        private byte[] version;
        private int version_length = 4;
        private Int32 timeStamp;

        private byte packType = 0x00;

        private byte DelayTime = 0;//转发数据的延迟时间，单位毫秒
        private byte rssi_cj = 0; //参考点发送，节点接收时获取到的信号强度值
        private byte rssi_jc = 0; //节点发送，参考点接收时获取到的信号强度值

        /// <summary>
        /// 参数data是一条协议包 FE + 02 + ID_c + Version + ID_j + CS + FD  共12byte
        /// fe 02 01 00 11 08 12 10 00 01 3d fd
        /// fe 02 01 01 11 08 12 10 02 01 40 fd
        /// </summary>
        /// <param name="data"></param>
        public LocationNode(byte[] data) {
            timeStamp = XwDataUtils.GetTimeStamp();
            packType = data[1];
            createPackData02(data);
            createPackData05(data);
        }


        private void createPackData02(byte[] data)
        {
            if (data == null || data.Length != 14 || data[1] != 0x02) return;
            byteCreate();
            Array.Copy(data, 2, ID_c, 0, ID_c.Length);
            Array.Copy(data, 4, version, 0, Version.Length);
            Array.Copy(data, 10, ID_j, 0, ID_j.Length);
        }

        private void createPackData05(byte[] data)
        {
            if (data == null || data.Length != 11 || data[1] != 0x05) return;
            byteCreate();
            version = null;
            DelayTime = data[4];
            rssi_cj = data[5];
            rssi_jc = data[6];
            Array.Copy(data, 2, ID_c, 0, ID_c.Length);
            Array.Copy(data, 7, ID_j, 0, ID_j.Length);
        }

        private void byteCreate() {
            ID_c = new byte[2];
            ID_j = new byte[2];
            version = new byte[version_length];
        }


        private string getByteHex(byte[] data) {
            if (data == null) return "";
            StringBuilder strBuder = new StringBuilder();
            foreach (byte item in data)
            {
                strBuder.Append(item.ToString("X2"));
            }
            return strBuder.ToString();
        }

        public byte[] ID_C {
            get { return ID_c; }
        }

        public string ID_C_str
        {
            get { return getByteHex(ID_c); }
        }

        public byte[] ID_J
        {
            get { return ID_j; }
        }

        public string ID_J_str
        {
            get { return getByteHex(ID_j); }
        }

        public byte[] Version
        {
            get { return version; }
        }

        public string Version_str
        {
            get { return getByteHex(version); }
        }

        public string Version_timeStr
        {
            get { return getByteHex(version); }
        }

        public Int32 TimeStamp
        {
            get { return timeStamp; }
        }

        public byte PackType {
            get { return packType; }
        }

        public byte Delay_Time
        {
            get { return DelayTime; }
        }

        public byte Rssi_cj
        {
            get { return rssi_cj; }
        }

        public byte Rssi_jc
        {
            get { return rssi_jc; }
        }

    }
}
