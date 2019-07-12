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

        public MainFromModel(DrivaceSetInterface drivaceSet)
        {
            if (drivaceSet == null) return;
            objDrivaceSet = drivaceSet;
        }

        public byte[] Id {
            get { return id; }
            set { id = value; }
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

        public void onCheckJieDianID(byte[] ID){
            byte[] packSend = new byte[4]; //4代表包头包尾包类型包校验
            packDeal(0x01, packSend);
        }

        public void clearTag()
        {
            byte[] packSend = new byte[4]; //4代表包头包尾包类型包校验
            packDeal(0x87, packSend);
        }

        public void onCheckJieDianVersion(byte[] ID, byte type, byte[] version)
        {
            sendPortData(0x02, ID);
        }

        public void onSetJieDianServiseIP(byte[] ID, byte[] IP) {
            dataCopy(0x05, ID, IP);
        }

        public void onReadJieDianServiseIP(byte[] ID, byte[] IP)
        {
            sendPortData(0x06, ID);
        }

        public void onSetJieDianServisePort(byte[] ID, byte[] port)
        {
            dataCopy(0x07, ID, port);
        }

        public void onReadJieDianServisePort(byte[] ID, byte[] port)
        {
            sendPortData(0x08, ID);
        }

        public void onSetJieDianWifiName(byte[] ID, byte[] name)
        {
            dataCopy(0x09, ID, name);
        }

        public void onReadJieDianWifiName(byte[] ID, byte[] name)
        {
            sendPortData(0x0a, ID);
        }

        public void onSetJieDianWifiPassWord(byte[] ID, byte[] password)
        {
            dataCopy(0x0b, ID, password);
        }

        public void onReadJieDianWifiPassWord(byte[] ID, byte[] password)
        {
            sendPortData(0x0c, ID);
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

        public void onSetTagUpTime(byte[] ID, byte upTime)
        {
            byte[] UpTime = new byte[] { upTime };
            dataCopy(0xc4, ID, UpTime);
        }

        public void onReadTagUpTime(byte[] ID, byte[] upTime)
        {
            sendPortData(0xc5, ID);
        }

        public void onReadTagUpTime(byte[] ID, byte upTime)
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
            //if (index + length + 2 != buf.Length) return null;
            if (index + length > buf.Length) return null;
            byte[] packData = new byte[length];
            Array.Copy(buf, index, packData, 0, length);
            return packData;
        }


        public void onNODEfuwei(byte[] ID) {
            sendPortData(0x0d, ID);
        }

        public void onSetNODEmodel(byte[] ID, byte model) {
            byte[] Model = new byte[] { model };
            dataCopy(0x0e, ID, Model);
        }

        public void onReadNODEmodel(byte[] ID, byte model) {
            sendPortData(0x0f, ID);
        }

        public void onSetNODE_IP(byte[] ID, byte[] IP) {
            dataCopy(0x10, ID, IP);
        }

        public void onReadNODE_IP(byte[] ID, byte[] IP) {
            sendPortData(0x11, ID);
        }

        public void onSetNODESubMask(byte[] ID, byte[] IP) {
            dataCopy(0x12, ID, IP);
        }

        public void onReadNODESubMask(byte[] ID, byte[] IP) {
            sendPortData(0x13, ID);
        }

        public void onSetNODEGateWay(byte[] ID, byte[] IP) {
            dataCopy(0x14, ID, IP);
        }

        public void onReadNODEGateWay(byte[] ID, byte[] IP) {
            sendPortData(0x15, ID);
        }

        public void onCheckVersionBack()
        {
            for (int i = 0; i < 5;i++ )
            {
                if (objDrivaceSet != null) objDrivaceSet.onCheckVersionBack();
            }
        }

        public void onSet_XinHaoQiangdu_(byte[] ID, byte Threshold)
        {
            byte[] ThresholdBT = new byte[1] { Threshold };
            dataCopy(0x16, ID, ThresholdBT);
        }

        public void onRead_XinHaoQiangdu_(byte[] ID, byte Threshold)
        {
            sendPortData(0x17, ID);
        }

        public void onSet_XinHaoQiangduXiShu_(byte[] ID, byte k1)
        {
            byte[] k1BT = new byte[1] { k1 };
            dataCopy(0x18, ID, k1BT);
        }

        public void onRead_XinHaoQiangduXiShu(byte[] ID, byte k1)
        {
            sendPortData(0x19, ID);
        }

        public override void reveData(byte[] buf){
            if (objDrivaceSet == null) return;
            byte[] ID = new byte[2];
            Array.Copy(buf, 2, ID, 0, 2);
            switch (buf[1]) { 
                case 0x01://查找周围节点ID   
                    jieXiData(buf);     
                    break;
                case 0x02://查找节点版本号
                    //if(buf.Length != 11) return;
                    if(buf.Length != 14) return;
                    byte[] ver_node = getPactData(buf, 8, 4);
                    objDrivaceSet.onCheckJieDianVersion(ID, buf[7], ver_node);
                    break;
                case 0x05://设置节点的serviseIP
                    if (buf.Length != 10) return;
                    objDrivaceSet.onSetJieDianServiseIP(ID, getPactData(buf, 4));
                    onReadJieDianServiseIP(ID, getPactData(buf, 4));
                    break;
                case 0x06://读取节点的serviseIP
                    if (buf.Length != 10) return;
                    objDrivaceSet.onReadJieDianServiseIP(ID, getPactData(buf, 4));
                    break;
                case 0x07://设置节点的servise Port
                    if (buf.Length != 8) return;
                    objDrivaceSet.onSetJieDianServisePort(ID, getPactData(buf, 2));
                    onReadJieDianServisePort(ID, getPactData(buf, 2));
                    break;
                case 0x08://读取节点的servise Port
                    if (buf.Length != 8) return;
                    objDrivaceSet.onReadJieDianServisePort(ID, getPactData(buf, 2));
                    break;
                case 0x09://设置节点的wifi name
                    if (buf.Length != 38) return;
                    objDrivaceSet.onSetJieDianWifiName(ID, getPactData(buf, 32));
                    onReadJieDianWifiName(ID, getPactData(buf, 32));
                    break;
                case 0x0a://读取节点的wifi name
                    if (buf.Length != 38) return;
                    objDrivaceSet.onReadJieDianWifiName(ID,getPactData(buf,32));
                    break;
                case 0x0b://设置节点的wifi 密码
                    if (buf.Length != 38) return;
                    objDrivaceSet.onSetJieDianWifiPassWord(ID, getPactData(buf, 32));
                    onReadJieDianWifiPassWord(ID, getPactData(buf, 32));
                    break;
                case 0x0c://读取节点的wifi 密码
                    if (buf.Length != 38) return;
                    objDrivaceSet.onReadJieDianWifiPassWord(ID, getPactData(buf, 32));
                    break;
                case 0x0d://复位
                    if (buf.Length != 6) return;
                    objDrivaceSet.onNODEfuwei(ID);
                    break;
                case 0x0e://设置节点模式
                    if (buf.Length != 7) return;
                    objDrivaceSet.onSetNODEmodel(ID,buf[4]);
                    onReadNODEmodel(ID, buf[4]);
                    break;
                case 0x0f://读取节点模式
                    if (buf.Length != 7) return;
                    objDrivaceSet.onReadNODEmodel(ID, buf[4]);
                    break;
                case 0x10://设置节点IP
                    if (buf.Length != 10) return;
                    byte[] set_node_ip = new byte[4];
                    Array.Copy(buf, 4, set_node_ip, 0, 4);
                    objDrivaceSet.onSetNODE_IP(ID, set_node_ip);
                    onReadNODE_IP(ID, null);
                    break;
                case 0x11://读取节点IP
                    if (buf.Length != 10) return;
                    byte[] read_node_ip = new byte[4];
                    Array.Copy(buf, 4, read_node_ip, 0, 4);
                    objDrivaceSet.onReadNODE_IP(ID, read_node_ip);
                    break;
                case 0x12://设置节点submak
                    if (buf.Length != 10) return;
                    byte[] set_submask_ip = new byte[4];
                    Array.Copy(buf, 4, set_submask_ip, 0, 4);
                    objDrivaceSet.onSetNODESubMask(ID, set_submask_ip);
                    onReadNODESubMask(ID, null);
                    break;
                case 0x13://读取节点submak
                    if (buf.Length != 10) return;
                    byte[] read_submask_ip = new byte[4];
                    Array.Copy(buf, 4, read_submask_ip, 0, 4);
                    objDrivaceSet.onReadNODESubMask(ID, read_submask_ip);
                    break;
                case 0x14://设置节点GateWay
                    if (buf.Length != 10) return;
                    byte[] set_gateway_ip = new byte[4];
                    Array.Copy(buf, 4, set_gateway_ip, 0, 4);
                    objDrivaceSet.onSetNODEGateWay(ID, set_gateway_ip);
                    onReadNODEGateWay(ID, null);
                    break;
                case 0x15://读取节点GateWay
                    if (buf.Length != 10) return;
                    byte[] read_gateway_ip = new byte[4];
                    Array.Copy(buf, 4, read_gateway_ip, 0, 4);
                    objDrivaceSet.onReadNODEGateWay(ID, read_gateway_ip);
                    break;
                case 0x16://设置参考点接收Tag定位信号强度阀值
                    if (buf.Length != 7) return;
                    objDrivaceSet.onSet_XinHaoQiangdu_(ID, buf[4]);
                    onRead_XinHaoQiangdu_(ID, buf[4]);
                    break;
                case 0x17://读取参考点接收Tag定位信号强度阀值
                    if (buf.Length != 7) return;
                    objDrivaceSet.onRead_XinHaoQiangdu_(ID, buf[4]);
                    break;
                case 0x18://设置参考点发送Tag定位包的信号强度系数k
                    if (buf.Length != 7) return;
                    objDrivaceSet.onSet_XinHaoQiangduXiShu_(ID, buf[4]);
                    onRead_XinHaoQiangduXiShu(ID, buf[4]);
                    break;
                case 0x19://读取参考点发送Tag定位包的信号强度系数k
                    if (buf.Length != 7) return;
                    objDrivaceSet.onRead_XinHaoQiangduXiShu(ID, buf[4]);
                    break;

                case 0x41://查找周围参考点ID
                    jieXiData(buf);  
                    break;
                case 0x42://查找周围参考点版本
                    if(buf.Length != 11) return;
                    objDrivaceSet.onCheckCanKaoDianVersion(ID, buf[4], getPactData(buf, 5, 4));
                    break;
                case 0x80://查找周围卡片ID
                    jieXiData(buf);  
                    break;
                case 0x81://查找周围卡片版本
                case 0x82:
                    if(buf.Length != 14) return;
                    byte[] ver = getPactData(buf, 8, 4);
                    //Array.Reverse(ver);
                    objDrivaceSet.onCheckTagVersion(ID, buf[7], ver);
                    break;
                case 0xc4://设置卡片上报时间
                    if (buf.Length != 7) return;
                    //objDrivaceSet.onSetTagUpTime(ID, getPactData(buf, 1));
                    onReadTagUpTime(ID, buf[4]);
                    objDrivaceSet.onSetTagUpTime(ID, buf[4]);
                    break;
                case 0xc5://读取卡片上报时间
                    if (buf.Length != 7) return;
                    //objDrivaceSet.onReadTagUpTime(ID, getPactData(buf, 1));
                    objDrivaceSet.onReadTagUpTime(ID, buf[4]);
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

        private void jieXiData(byte[] buf)//解析搜索返回的ID包
        {            
            int idCount = buf[2];
            if (idCount == 0) return;
            for (int i = 0; i < idCount; i++)
            {
                byte[] ID = new byte[2];
                int id_index = 3 + (i * 2);
                Array.Copy(buf, id_index, ID, 0, 2);
                if (buf[1] == 0x01) objDrivaceSet.onCheckJieDianID(ID);
                if (buf[1] == 0x41) objDrivaceSet.onCheckCanKaoDianID(ID);
                if (buf[1] == 0x80) objDrivaceSet.onCheckTagID(ID);          
            }
            new Thread(onCheckVersionBack).Start();
        }

        public override string TAG() { 
            return "MainFromModel";
        }

        public void readAllParameter(DrivaceType driType,byte[] ID) {
            if (driType == DrivaceType.NODE) new Thread(readNODEParameter).Start(ID);
            else if (driType == DrivaceType.TAG) new Thread(readTAGParameter).Start(ID);
        }

        private int sleepTime = 100;
        private void readNODEParameter(object obj) {
            if (obj is byte[]) {
                byte[] locaByteID = (byte[])obj;
                if (locaByteID == null) return;
                NODEid = locaByteID;               
                
                onReadJieDianServiseIP(NODEid,null);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;

                onReadJieDianServisePort(NODEid, null);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;

                onReadJieDianWifiName(NODEid,null);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;

                onReadJieDianWifiPassWord(NODEid, null);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;

                onReadNODEmodel(NODEid, 0x00);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;

                onReadNODE_IP(NODEid, null);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;

                onReadNODESubMask(NODEid, null);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;

                onRead_XinHaoQiangdu_(NODEid, 0x00);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;

                onRead_XinHaoQiangduXiShu(NODEid, 0x00);
                Thread.Sleep(sleepTime);
                if (!XWUtils.byteBTBettow(NODEid, locaByteID, NODEid.Length)) return;

                onReadNODEGateWay(NODEid, null);
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
            }
        }

    }
}
