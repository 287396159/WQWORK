using CiXinLocation.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiXinLocation.bean
{
    public class SendDataType
    {
        private byte[] buf;
        private string ip;
        private CommunicationMode comMode;
        private SendMode sendMode;

        public SendDataType(byte[] buf, string ip, CommunicationMode comMode, SendMode sendMode)
        {
            this.buf = buf;
            this.ip = ip;
            this.comMode = comMode;
            this.sendMode = sendMode;
        }

        public SendMode SendMode
        {
            get { return sendMode; }
            set { sendMode = value; }
        }

        public CommunicationMode ComMode
        {
            get { return comMode; }
            set { comMode = value; }
        }

        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }

        public byte[] Buf
        {
            get { return buf; }
            set { buf = value; }
        }

        /// <summary>
        /// 数据是否可用
        /// </summary>
        public bool isNoDt
        {
            get 
            { if (buf == null)return false; 
                return true; 
            }
        }
    }
}
