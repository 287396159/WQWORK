using CiXinLocation.bean;
using CiXinLocation.CiXinInterface;
using CiXinLocation.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiXinLocation.Model
{
    class MainFromModel : FromBaseModel, DrivaceSetOtherInterface
    {

        private MainFromInterface mainInterface;

        public MainFromModel()
        {
            receVeByteHandle = 0xfe;
            receVeByteend = 0xfd;
        }

        public MainFromModel(MainFromInterface mainInterface):this()
        {
            this.mainInterface = mainInterface;           
        }

        public void locationOutPutID(LocationNode loca_node) { }

        public override void reveData(SendDataType sType)
        {
            if (!sType.isNoDt) return;
            receDt(sType.Buf);
        }

        private void receDt(byte[] bt)
        {
            if(bt.Length < 4) return;
            switch (bt[1])
            {
                case 0x01:
                    if (bt.Length != 5) break;
                    byte[] port = new byte[2] {0,bt[2] };
                    mainInterface.onSetJieDianServiseIP(null,port);
                    break;
                case 0x02:
                    if (bt.Length != 10) break;
                    byte[] report = new byte[2];
                    byte[] ip = new byte[4];
                    Array.Copy(bt, 2, ip, 0, 4);
                    Array.Copy(bt, 6, report, 0, 2);
                    mainInterface.onReadJieDianServiseIP(ip, report);
                    break;
                case 0x03:
                    if (bt.Length != 5) break;
                    mainInterface.onSetBoTeLvPort(bt[2]);
                    break;
                case 0x04:
                    if (bt.Length != 5) break;
                    mainInterface.onReadBoTeLvPort(bt[2]);
                    break;
                default:
                    break;
            }
        }

        public override void reveData(byte[] buf, string ip)
        {
            receDt(buf);
        }

        public override void close()
        {
        }

        public override string TAG()
        {
            return "MainFromInterface";
        }

        /// <summary>
        /// 组装主体数据
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="data"></param>
        private void sendPortData(byte Type, byte[] data)
        {
            byte[] packSend = new byte[4 + data.Length]; //4代表包头包尾包类型包校验
            Array.Copy(data, 0, packSend, 2, data.Length);
            packDeal(Type, packSend);
        }

        /// <summary>
        /// 拼接包头包尾
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="packSend"></param>
        private void packDeal(byte Type, byte[] packSend)
        {
            packSend[0] = 0xf8;
            packSend[1] = Type;
            packSend[packSend.Length - 1] = 0xf7;
            SendDataType sType = new SendDataType(packSend,"",Utils.CommunicationMode.SERIALPORT,SendMode.ALL);
            addCSAndsendData(sType);
        }

        private void dataCopy(byte type,  byte[] typtData)
        {
            dataCopy(type, typtData, typtData.Length);
        }

        private void dataCopy(byte type, byte[] typtData, int data_length)
        {
            if (typtData == null || typtData.Length != data_length) return;
            byte[] data = new byte[data_length];
            Array.Copy(typtData, 0, data, 0, data_length);
            sendPortData(type, data);
        }

        /// <summary>
        /// 读取所有的参数
        /// </summary>
        public void readAllParameter()
        {
            
        }

        /// <summary>
        /// 设置节点的ServiseIP
        /// </summary>
        /// <param name="IP">设置成功的IP 4个Byte</param>
        public void onSetJieDianServiseIP(byte[] IP, byte[] port) 
        {
            byte[] sendDT = new byte[IP.Length + port.Length];
            Array.Copy(IP, 0, sendDT, 0, IP.Length);
            Array.Copy(port, 0, sendDT, IP.Length, port.Length);
            dataCopy(0x01, sendDT);
        }

        /// <summary>
        /// 读取节点的ServiseIP
        /// </summary>
        /// <param name="IP">读取到的IP 4个Byte</param>
        public void onReadJieDianServiseIP(byte[] IP, byte[] port) 
        {
            byte[] sendBt = new byte[4];
            packDeal(0x02, sendBt);
        }

        /// <summary>
        /// 设置节点的Servise Port
        /// </summary>
        /// <param name="port">设置节点的端口 2个Byte</param>
        public void onSetBoTeLvPort(byte port)
        {
            byte[] portBt = new byte[1] { port };
            dataCopy(0x03, portBt);
        }

        /// <summary>
        /// 读取节点的Servise Port
        /// </summary>
        /// <param name="port">读取节点的端口 2个Byte</param>
        public void onReadBoTeLvPort(byte port)
        {
            byte[] sendBt = new byte[4];
            packDeal(0x04, sendBt);
        }

    }
}
