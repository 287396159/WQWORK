using CiXinLocation.CiXinInterface;
using CiXinLocation.Utils;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CiXinLocation.Model
{
    class MainFromModel : FromBaseModel, DrivaceSetOtherInterface
    {
        private DrivaceSetInterface objDrivaceSet;
        private byte[] id;
        private byte[] NODEid;
        private byte[] TAGid;
        private byte[] LOCATIONid;
        private byte channel = 0xff;
        
        public MainFromModel(DrivaceSetInterface drivaceSet)
        {
            if (drivaceSet == null) return;
            objDrivaceSet = drivaceSet;
        }

        public byte[] Id
        {
            get { return id; }
            set { id = value; }
        }

        public byte Channel
        {
            get { return channel; }
            set { channel = value; }
        }

        public void setChannel(byte channel) 
        {
            Channel = channel;
        }

        public byte getChannel()
        {
            return Channel;
        }

        /// <summary>
        /// 组装主体数据
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="data"></param>
        private void sendPortData(byte Type,byte[] data) {
            byte[] packSend = new byte[4 + data.Length]; //4代表包头包尾包类型包校验
            Array.Copy(data, 0, packSend, 2, data.Length);
            packDeal(Type,packSend);
        }

        /// <summary>
        /// 拼接包头包尾
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="packSend"></param>
        private void packDeal(byte Type,byte[] packSend){
            packSend[0] = 0xf2;
            packSend[1] = Type;
            packSend[packSend.Length - 1] = 0xf1;
            addCSAndsendData(packSend);
        }

        private void dataCopy(byte type, byte[] ID, byte[] typtData) {
            dataCopy(type, ID, ID.Length, typtData, typtData.Length);
        }

        private void dataCopy(byte type,byte[] ID,int id_length,byte[] typtData,int data_length) {
            if (ID == null || typtData == null || ID.Length != id_length || typtData.Length != data_length) return;
            byte[] data = new byte[id_length + data_length];
            Array.Copy(ID, 0, data, 0, id_length);
            Array.Copy(typtData, 0, data, id_length, data_length);
            sendPortData(type, data);
        }

        public void onCheckUSB_DANGLE(byte[] version)
        {
            byte[] packSend = new byte[4]; //4代表包头包尾包类型包校验
            packDeal(0xe0, packSend);
        }

        public void onCheckJieDianID(byte[] ID,byte Channel){
            byte[] packSend = new byte[4]; //4代表包头包尾包类型包校验
            packDeal(0x01, packSend);
        }

        public void clearTag()
        {
            byte[] packSend = new byte[4]; //4代表包头包尾包类型包校验
            packDeal(0x87, packSend);
        }

        public void onCheckJieDianVersion(byte[] ID, byte type, byte[] version,byte Channel)
        {
            byte[] data = new byte[ID.Length + 1];
            Array.Copy(ID, 0, data, 0, ID.Length);
            data[2] = Channel;

            sendPortData(0x02, data);
        }

        private byte[] getCannelData(byte[] ID,byte channel) 
        {
            byte[] data = new byte[ID.Length + 1];
            Array.Copy(ID, 0, data, 0, ID.Length);
            data[2] = Channel;
            return data;
        }

        public void onSetJieDianServiseIP(byte[] ID, byte[] IP, byte Channel)
        {
            dataCopy(0x05, getCannelData(ID, Channel), IP);
        }

        public void onReadJieDianServiseIP(byte[] ID, byte[] IP, byte Channel)
        {
            sendPortData(0x06, getCannelData(ID, Channel));
        }

        public void onSetJieDianServisePort(byte[] ID, byte[] port, byte Channel)
        {
            dataCopy(0x07, getCannelData(ID, Channel), port);
        }

        public void onReadJieDianServisePort(byte[] ID, byte[] port, byte Channel)
        {
            sendPortData(0x08, getCannelData(ID, Channel));
        }

        public void onSetJieDianWifiName(byte[] ID, byte[] name, byte Channel)
        {
            dataCopy(0x09, getCannelData(ID, Channel), name);
        }

        public void onReadJieDianWifiName(byte[] ID, byte[] name, byte Channel)
        {
            sendPortData(0x0a, getCannelData(ID, Channel));
        }

        public void onSetJieDianWifiPassWord(byte[] ID, byte[] password, byte Channel)
        {
            dataCopy(0x0b, getCannelData(ID, Channel), password);
        }

        public void onReadJieDianWifiPassWord(byte[] ID, byte[] password, byte Channel)
        {
            sendPortData(0x0c, getCannelData(ID, Channel));
        }

        public void onCheckCanKaoDianID(byte[] ID)
        {
            byte[] packSend = new byte[4]; //4代表包头包尾包类型包校验
            packDeal(0x41, packSend);
        }

        public void onCheckCanKaoDianVersion(byte[] ID, byte type, byte[] version)
        {
            sendPortData(0x42, ID);
        }

        public void onCheckTagID(byte[] ID)
        {
            byte[] packSend = new byte[4]; //4代表包头包尾包类型包校验
            packDeal(0x80, packSend);
        }

        public void onCheckTagVersion(byte[] ID, byte type, byte[] version)
        {
            sendPortData(0x81, ID);
        }

        public void onSetTagUpTime(byte[] ID, byte[] upTime)
        {
            dataCopy(0xc4, ID, upTime);
        }

        public void onReadTagUpTime(byte[] ID, byte[] upTime)
        {
            sendPortData(0xc5, ID);
        }

        public void onSetTagRF(byte[] ID, byte RF)
        {
            byte[] RFbt = new byte[] { RF };
            dataCopy(0xc6, ID, RFbt);
        }

        public void onReadTagRF(byte[] ID, byte RF) {
            sendPortData(0xc7, ID);
        }

        private byte[] getPactData(byte[] buf, int length) {
            return getPactData(buf,4,length);
        }

        private byte[] getPactData(byte[] buf,int index,int length)
        {
            if (index + length + 2 > buf.Length) return null;
            byte[] packData = new byte[length];
            Array.Copy(buf, index, packData, 0, length);
            return packData;
        }

        public void onNODEfuwei(byte[] ID, byte Channel)
        {
            sendPortData(0x0d, getCannelData(ID, Channel));
        }

        public void onCanKaoDianfuwei(byte[] ID)
        {
            sendPortData(0x49, ID);
        }

        public void onSetNODEmodel(byte[] ID, byte model, byte Channel)
        {
            byte[] Model = new byte[] { model };
            dataCopy(0x0e, getCannelData(ID, Channel), Model);
        }

        public void onReadNODEmodel(byte[] ID, byte model, byte Channel)
        {
            sendPortData(0x0f, getCannelData(ID, Channel));
        }

        public void onSetNODE_IP(byte[] ID, byte[] IP, byte Channel)
        {
            dataCopy(0x10, getCannelData(ID, Channel), IP);
        }

        public void onReadNODE_IP(byte[] ID, byte[] IP, byte Channel)
        {
            sendPortData(0x11, getCannelData(ID, Channel));
        }

        public void onSetNODESubMask(byte[] ID, byte[] IP, byte Channel)
        {
            dataCopy(0x12, getCannelData(ID, Channel), IP);
        }

        public void onReadNODESubMask(byte[] ID, byte[] IP, byte Channel)
        {
            sendPortData(0x13, getCannelData(ID, Channel));
        }

        public void onSetNODEGateWay(byte[] ID, byte[] IP, byte Channel)
        {
            dataCopy(0x14, getCannelData(ID, Channel), IP);
        }

        public void onReadNODEGateWay(byte[] ID, byte[] IP, byte Channel)
        {
            sendPortData(0x15, getCannelData(ID, Channel));
        }

        public void onSet_XinHaoQiangdu_(byte[] ID, byte Threshold)
        {
            byte[] ThresholdBT = new byte[1] { Threshold };
            dataCopy(0x45, ID, ThresholdBT);
        }

        public void onRead_XinHaoQiangdu_(byte[] ID, byte Threshold)
        {
            sendPortData(0x46, ID);
        }

        public void onSet_XinHaoQiangduXiShu_(byte[] ID, byte k1)
        {
            byte[] k1BT = new byte[1] { k1 };
            dataCopy(0x47, ID, k1BT);
        }

        public void onRead_XinHaoQiangduXiShu(byte[] ID, byte k1)
        {
            sendPortData(0x48, ID);
        }

        /// <summary>
        /// 设置卡片的静止定位封包上报间隔时间
        /// </summary>
        /// <param name="ID">卡片ID 2Byte</param>
        /// <param name="upTime">卡片上报时间 2Byte</param>
        public void onSetTagStaticTime(byte[] ID, byte[] staticTime) 
        {
            dataCopy(0xc8, ID, staticTime);
        }

        /// <summary>
        /// 读取卡片的静止定位封包上报间隔时间
        /// </summary>
        /// <param name="ID">卡片ID 2Byte</param>
        /// <param name="upTime">卡片上报时间 2Byte</param>
        public void onReadTagStaticTime(byte[] ID, byte[] staticTime)
        {
            sendPortData(0xc9, ID);
        }

        /// <summary>
        /// 设置卡片的Gsensor
        /// </summary>
        /// <param name="ID">卡片ID 2Byte</param>
        /// <param name="RF">发射功率，范围1~16</param>
        public void onSetTagGsensor(byte[] ID, byte Gsensor)
        {
            byte[] gen = new byte[1] { Gsensor };
            dataCopy(0xca, ID, gen);
        }

        /// <summary>
        /// 读取卡片的Gsensor
        /// </summary>
        /// <param name="ID">卡片ID 2Byte</param>
        /// <param name="RF">发射功率，范围1~16</param>
        public void onReadTagGsensor(byte[] ID, byte Gsensor) 
        {
            sendPortData(0xcb, ID);
        }


        public override void reveData(byte[] buf){
            if (objDrivaceSet == null) return;
            byte[] ID = new byte[2];
            Array.Copy(buf, 2, ID, 0, 2);
            switch (buf[1]) { 
                case 0x01://查找周围节点ID   
                    new Thread(jieXiData).Start(buf);     
                    break;
                case 0x02://查找节点版本号
                    if(buf.Length != 12) return;
                    objDrivaceSet.onCheckJieDianVersion(ID, buf[4],  getPactData(buf, 5,4),buf[9]);
                    break;
                case 0x05://设置节点的serviseIP
                    if (buf.Length != 10) return;
                    objDrivaceSet.onSetJieDianServiseIP(ID, getPactData(buf, 4),0);
                    onReadJieDianServiseIP(ID, getPactData(buf, 4),0);
                    break;
                case 0x06://读取节点的serviseIP
                    if (buf.Length != 10) return;
                    objDrivaceSet.onReadJieDianServiseIP(ID, getPactData(buf, 4),0);
                    break;
                case 0x07://设置节点的servise Port
                    if (buf.Length != 8) return;
                    objDrivaceSet.onSetJieDianServisePort(ID, getPactData(buf, 2),0);
                    onReadJieDianServisePort(ID, getPactData(buf, 2),0);
                    break;
                case 0x08://读取节点的servise Port
                    if (buf.Length != 8) return;
                    objDrivaceSet.onReadJieDianServisePort(ID, getPactData(buf, 2),0);
                    break;
                case 0x09://设置节点的wifi name
                    if (buf.Length != 38) return;
                    objDrivaceSet.onSetJieDianWifiName(ID, getPactData(buf, 32),0);
                    onReadJieDianWifiName(ID, getPactData(buf, 32),0);
                    break;
                case 0x0a://读取节点的wifi name
                    if (buf.Length != 38) return;
                    objDrivaceSet.onReadJieDianWifiName(ID,getPactData(buf,32),0);
                    break;
                case 0x0b://设置节点的wifi 密码
                    if (buf.Length != 38) return;
                    objDrivaceSet.onSetJieDianWifiPassWord(ID, getPactData(buf, 32),0);
                    onReadJieDianWifiPassWord(ID, getPactData(buf, 32),0);
                    break;
                case 0x0c://读取节点的wifi 密码
                    if (buf.Length != 38) return;
                    objDrivaceSet.onReadJieDianWifiPassWord(ID, getPactData(buf, 32),0);
                    break;
                case 0x0d://复位
                    if (buf.Length != 6) return;
                    objDrivaceSet.onNODEfuwei(ID,0);
                    break;
                case 0x0e://设置节点模式
                    if (buf.Length != 7) return;
                    objDrivaceSet.onSetNODEmodel(ID,buf[4],0);
                    onReadNODEmodel(ID, buf[4],0);
                    break;
                case 0x0f://读取节点模式
                    if (buf.Length != 7) return;
                    objDrivaceSet.onReadNODEmodel(ID, buf[4],0);
                    break;
                case 0x10://设置节点IP
                    if (buf.Length != 10) return;
                    byte[] set_node_ip = new byte[4];
                    Array.Copy(buf, 4, set_node_ip, 0, 4);
                    objDrivaceSet.onSetNODE_IP(ID, set_node_ip,0);
                    onReadNODE_IP(ID, null,0);
                    break;
                case 0x11://读取节点IP
                    if (buf.Length != 10) return;
                    byte[] read_node_ip = new byte[4];
                    Array.Copy(buf, 4, read_node_ip, 0, 4);
                    objDrivaceSet.onReadNODE_IP(ID, read_node_ip,0);
                    break;
                case 0x12://设置节点submak
                    if (buf.Length != 10) return;
                    byte[] set_submask_ip = new byte[4];
                    Array.Copy(buf, 4, set_submask_ip, 0, 4);
                    objDrivaceSet.onSetNODESubMask(ID, set_submask_ip,0);
                    onReadNODESubMask(ID, null,0);
                    break;
                case 0x13://读取节点submak
                    if (buf.Length != 10) return;
                    byte[] read_submask_ip = new byte[4];
                    Array.Copy(buf, 4, read_submask_ip, 0, 4);
                    objDrivaceSet.onReadNODESubMask(ID, read_submask_ip,0);
                    break;
                case 0x14://设置节点GateWay
                    if (buf.Length != 10) return;
                    byte[] set_gateway_ip = new byte[4];
                    Array.Copy(buf, 4, set_gateway_ip, 0, 4);
                    objDrivaceSet.onSetNODEGateWay(ID, set_gateway_ip,0);
                    onReadNODEGateWay(ID, null,0);
                    break;
                case 0x15://读取节点GateWay
                    if (buf.Length != 10) return;
                    byte[] read_gateway_ip = new byte[4];
                    Array.Copy(buf, 4, read_gateway_ip, 0, 4);
                    objDrivaceSet.onReadNODEGateWay(ID, read_gateway_ip,0);
                    break;
                case 0x16://查询指定wifi强度
                    if (buf.Length != 7) return;
                    objDrivaceSet.onWIFIRess(ID, new byte[1]{buf[4]}, 0);
                    break;
                case 0x17://读取节点GateWay
                    if (buf.Length != 7) return;
                    objDrivaceSet.onLastConnWIFIType(ID, new byte[1] { buf[4] }, 0);
                    break;
                case 0x18://设置节点WIFI mode;
                    if (buf.Length != 7) return;
                    objDrivaceSet.onSetJieDianWifiModel(ID, buf[4], 0);
                    onReadJieDianWifiModel(ID, buf[4], 0);
                    break;
                case 0x19://读取节点WIFI mode;
                    if (buf.Length != 7) return;
                    objDrivaceSet.onReadJieDianWifiModel(ID, buf[4], 0);
                    break;
                case 0x1A://读取节点GateWay
                    if (buf.Length != 10) return;
                    byte[] set_nodewifi_ip = new byte[4];
                    Array.Copy(buf, 4, set_nodewifi_ip, 0, 4);
                    objDrivaceSet.onSetJieDianWifiStaticIp(ID, set_nodewifi_ip, 0);
                    onReadJieDianWifiStaticIp(ID, set_nodewifi_ip, 0);
                    break;
                case 0x1B://读取节点GateWay
                    if (buf.Length != 10) return;
                    byte[] read_wifinode_ip = new byte[4];
                    Array.Copy(buf, 4, read_wifinode_ip, 0, 4);
                    objDrivaceSet.onReadJieDianWifiStaticIp(ID, read_wifinode_ip, 0);
                    break;

                case 0x41://查找周围参考点ID
                    new Thread(jieXiData).Start(buf); 
                    break;
                case 0x42://查找周围参考点版本
                    if(buf.Length != 11) return;
                    objDrivaceSet.onCheckCanKaoDianVersion(ID, buf[4], getPactData(buf, 5, 4));
                    break;
                case 0x45://设置参考点接收Tag定位信号强度阀值
                    if (buf.Length != 7) return;
                    objDrivaceSet.onSet_XinHaoQiangdu_(ID, buf[4]);
                    onRead_XinHaoQiangdu_(ID, buf[4]);
                    break;
                case 0x46://读取参考点接收Tag定位信号强度阀值
                    if (buf.Length != 7) return;
                    objDrivaceSet.onRead_XinHaoQiangdu_(ID,buf[4]);
                    break;
                case 0x47://设置参考点发送Tag定位包的信号强度系数k
                    if (buf.Length != 7) return;
                    objDrivaceSet.onSet_XinHaoQiangduXiShu_(ID,buf[4]);
                    onRead_XinHaoQiangduXiShu(ID, buf[4]);
                    break;
                case 0x48://读取参考点发送Tag定位包的信号强度系数k
                    if (buf.Length != 7) return;
                    objDrivaceSet.onRead_XinHaoQiangduXiShu(ID, buf[4]);
                    break;
                case 0x49://复位
                    if (buf.Length != 6) return;
                    objDrivaceSet.onCanKaoDianfuwei(ID);
                    break;
                case 0x80://查找周围卡片ID
                    new Thread(jieXiData).Start(buf); 
                    break;
                case 0x81://查找周围卡片版本
                case 0x82:
                    if(buf.Length != 11) return;
                    objDrivaceSet.onCheckTagVersion(ID, buf[4], getPactData(buf, 5, 4));
                    break;
                case 0xc4://设置卡片上报时间
                    if (buf.Length != 8) return;
                    objDrivaceSet.onSetTagUpTime(ID, getPactData(buf, 2));
                    onReadTagUpTime(ID, getPactData(buf, 2));
                    break;
                case 0xc5://读取卡片上报时间
                    if (buf.Length != 8) return;
                    objDrivaceSet.onReadTagUpTime(ID, getPactData(buf, 2));
                    break;
                case 0xc6://设置卡片发射功率
                    if (buf.Length != 7) return;
                    objDrivaceSet.onSetTagRF(ID, buf[4]);
                    onReadTagRF(ID, buf[4]);
                    break;
                case 0xc7://读取卡片发射功率
                    if (buf.Length != 7) return;
                    objDrivaceSet.onReadTagRF(ID, buf[4]);
                    break;
                case 0xc8://设置卡片静止睡眠时间
                    if (buf.Length != 8) return;
                    objDrivaceSet.onSetTagStaticTime(ID, getPactData(buf, 2));
                    onReadTagStaticTime(ID, getPactData(buf, 2));
                    break;
                case 0xc9://读取卡片静止睡眠时间
                    if (buf.Length != 8) return;
                    objDrivaceSet.onReadTagStaticTime(ID, getPactData(buf, 2));
                    break;
                case 0xca://设置卡片Gsensor
                    if (buf.Length != 7) return;
                    objDrivaceSet.onSetTagGsensor(ID, buf[4]);
                    onReadTagGsensor(ID, buf[4]);
                    break;
                case 0xcb://读取卡片Gsensor
                    if (buf.Length != 7) return;
                    objDrivaceSet.onReadTagGsensor(ID, buf[4]);
                    break;
                case 0xe0://读取USB_Dangle版本
                    if (buf.Length != 8) return;
                    objDrivaceSet.onCheckUSB_DANGLE(getPactData(buf, 2, 4));
                    break;
                case 0xe4://读取USB_Dangle版本
                    if (buf.Length != 8) return;
                    objDrivaceSet.onCheckUSB_DANGLE(getPactData(buf, 2, 4));
                    break;
                default:
                    break;
            }
        }

        private void jieXiData(object obj)//解析搜索返回的ID包
        {
            if (!(obj is byte[])) return;
            byte[] buf = (byte[])obj;
            int idCount = buf[2];
            if (idCount == 0) return;
            for (int i = 0; i < idCount; i++)
            {
                byte[] ID = new byte[2];
                byte Channel = 0;
                int id_index = 3 + (i * 2);
                if (buf[1] == 0x01) 
                {
                    id_index = 3 + (i * 3);
                    Channel = buf[id_index + 2];
                } 
                Array.Copy(buf, id_index, ID, 0, 2);
                if (buf[1] == 0x01) objDrivaceSet.onCheckJieDianID(ID,Channel);
                if (buf[1] == 0x41) objDrivaceSet.onCheckCanKaoDianID(ID);
                if (buf[1] == 0x80) objDrivaceSet.onCheckTagID(ID);
                try {
                    Thread.Sleep(50);
                }
                catch { }               
            }      
        }

        public override string TAG() { 
            return "MainFromModel";
        }

        public void readAllParameter(DrivaceType driType,byte[] ID) {
            if (driType == DrivaceType.NODE) new Thread(readNODEParameter).Start(ID);
            else if (driType == DrivaceType.TAG) new Thread(readTAGParameter).Start(ID);
            else if (driType == DrivaceType.LOCATION) new Thread(readLOCATIONParameter).Start(ID);
        }

        private int sleepTime = 100;
        private void readLOCATIONParameter(object obj) {
            if (!(obj is byte[])) return;
            byte[] locaByteID = (byte[])obj;
            if (locaByteID == null) return;
            LOCATIONid = locaByteID;

            onRead_XinHaoQiangdu_(LOCATIONid, 0);
            Thread.Sleep(sleepTime);
            if (!XWUtils.byteBTBettow(LOCATIONid, locaByteID, LOCATIONid.Length)) return;

            onRead_XinHaoQiangduXiShu(LOCATIONid,0);
        }

        //在线程睡眠之后做一个判断的原因是，线程运行期间，只能有一个ID在查询参数，因为存在在查询期间
        //会有第二次的查询发起，这会导致混乱，故，有新ID查询，就当前线程关闭
        private void readNODEParameter(object obj) {
            if (obj is byte[]) {
                byte[] locaByteID = (byte[])obj;
                if (locaByteID == null) return;
                NODEid = locaByteID;

                onLastConnWIFIType(NODEid, null, 0);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;
                onReadJieDianServiseIP(NODEid, null, 0);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;
                onReadJieDianServisePort(NODEid, null, 0);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;
                onReadJieDianWifiName(NODEid, null, 0);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;
                onReadJieDianWifiPassWord(NODEid, null, 0);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;
                onReadNODEmodel(NODEid, 0x00, 0);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;
                onReadJieDianWifiModel(NODEid, 0x00, 0);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;
                onReadJieDianWifiStaticIp(NODEid, null, 0);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;
                onReadNODE_IP(NODEid, null, 0);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;
                onReadNODESubMask(NODEid, null, 0);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;
                onReadNODEGateWay(NODEid, null, 0);
            }
        }

        private void readTAGParameter(object obj)
        {
            if (obj is byte[])
            {
                Thread.Sleep(sleepTime * 2);
                byte[] locaByteID = (byte[])obj;
                if (locaByteID == null) return;
                TAGid = locaByteID;
                onReadTagUpTime(TAGid, null);                
                Thread.Sleep(sleepTime*2);
                if (!XWUtils.byteBTBettow(TAGid, locaByteID, TAGid.Length)) return;
                onReadTagRF(TAGid, 0);
                Thread.Sleep(sleepTime * 2);
                if (!XWUtils.byteBTBettow(TAGid, locaByteID, TAGid.Length)) return;
                onReadTagStaticTime(TAGid, null);
                Thread.Sleep(sleepTime * 2);
                if (!XWUtils.byteBTBettow(TAGid, locaByteID, TAGid.Length)) return;
                onReadTagGsensor(TAGid, 0);
            }
        }

        /// <summary>
        /// 设置节点使用WiFi模式
        /// </summary>
        /// <param name="ID">节点ID 2byte</param>
        /// <param name="model">model = 2 ->DHCP  ||  model = 1 ->静态模式</param>
        /// <param name="Channel"></param>
        public void onSetJieDianWifiModel(byte[] ID, byte model, byte Channel)
        {
            byte[] Model = new byte[] { model };
            dataCopy(0x18, getCannelData(ID, Channel), Model);
        }

        /// <summary>
        /// 读取节点使用WiFi时的IP模式
        /// </summary>
        /// <param name="ID">节点ID 2byte</param>
        /// <param name="model">model = 2 ->DHCP  ||  model = 1 ->静态模式</param>
        /// <param name="Channel"></param>
        public void onReadJieDianWifiModel(byte[] ID, byte model, byte Channel)
        {
            sendPortData(0x19, getCannelData(ID, Channel));
        }

        /// <summary>
        /// 设置节点使用Wifi时的静态IP
        /// </summary>
        /// <param name="ID">节点ID 2byte</param>
        /// <param name="IP">设置成功的IP 4个Byte</param>
        /// <param name="Channel"></param>
        public void onSetJieDianWifiStaticIp(byte[] ID, byte[] IP, byte Channel)
        {
            dataCopy(0x1A, getCannelData(ID, Channel), IP);
        }

        /// <summary>
        /// 读取节点使用Wifi时的静态IP
        /// </summary>
        /// <param name="ID">节点ID 2byte</param>
        /// <param name="IP">读取成功的IP 4个Byte</param>
        /// <param name="Channel"></param>
        public void onReadJieDianWifiStaticIp(byte[] ID, byte[] IP, byte Channel)
        {
            sendPortData(0x1B, getCannelData(ID, Channel));
        }

        /// <summary>
        /// 查询指定wifi信号强度
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="wifiSSID"></param>
        /// <param name="Channel"></param>
        public void onWIFIRess(byte[] ID, byte[] wifiSSID, byte Channel)
        {
            dataCopy(0x16, getCannelData(ID, Channel), wifiSSID);
        }

        /// <summary>
        /// 最后一次连接wifi状态
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="wifiSSID"></param>
        /// <param name="Channel"></param>
        public void onLastConnWIFIType(byte[] ID, byte[] connecSSID, byte Channel)
        {
            sendPortData(0x17, getCannelData(ID, Channel));
        }

    }
}
