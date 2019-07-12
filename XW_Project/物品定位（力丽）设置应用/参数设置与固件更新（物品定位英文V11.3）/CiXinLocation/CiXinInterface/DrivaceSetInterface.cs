using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiXinLocation.CiXinInterface
{
    /// <summary>
    /// 相关设备设置模块的接口
    /// </summary>
    public interface DrivaceSetInterface
    {

        /// <summary>
        /// 查找USB_Dangle的版本
        /// </summary>
        /// <param name="ID"></param>
        void onCheckUSB_DANGLE(byte[] version);

        /// <summary>
        /// 查找周围节点ID //两个字节的ID
        /// </summary>
        void onCheckJieDianID(byte[] ID);

        /// <summary>
        /// 查找周围节点的版本
        /// </summary>
        /// <param name="ID">节点的Id 2个Byte</param>
        /// <param name="type">子设备类型</param>
        /// <param name="version">4个byte 例如0x11071210，表示17年7月18版本号为1.0</param>
        void onCheckJieDianVersion(byte[] ID,byte type,byte[] version);

        /// <summary>
        /// 设置节点的ServiseIP
        /// </summary>
        /// <param name="ID">节点的Id 2个Byte</param>
        /// <param name="IP">设置成功的IP 4个Byte</param>
        void onSetJieDianServiseIP(byte[] ID, byte[] IP);

        /// <summary>
        /// 读取节点的ServiseIP
        /// </summary>
        /// <param name="ID">节点的Id 2个Byte</param>
        /// <param name="IP">读取到的IP 4个Byte</param>
        void onReadJieDianServiseIP(byte[] ID, byte[] IP);

        /// <summary>
        /// 设置节点的Servise Port
        /// </summary>
        /// <param name="ID">节点的Id 2个Byte</param>
        /// <param name="port">设置节点的端口 2个Byte</param>
        void onSetJieDianServisePort(byte[] ID, byte[] port);


        /// <summary>
        /// 读取节点的Servise Port
        /// </summary>
        /// <param name="ID">节点的Id 2个Byte</param>
        /// <param name="port">读取节点的端口 2个Byte</param>
        void onReadJieDianServisePort(byte[] ID, byte[] port);


        /// <summary>
        /// 设置节点的Wifi账号
        /// </summary>
        /// <param name="ID">节点ID 2byte</param>
        /// <param name="name">wifi名称，不带中文，Ascii表示 固定32Byte,0x00补足wifi不够32Byte</param>
        void onSetJieDianWifiName(byte[] ID, byte[] name);

        /// <summary>
        /// 读取节点的Wifi账号
        /// </summary>
        /// <param name="ID">节点ID 2byte</param>
        /// <param name="name">wifi名称，不带中文，Ascii表示 固定32Byte,0x00补足wifi不够32Byte</param>
        void onReadJieDianWifiName(byte[] ID, byte[] name);


        /// <summary>
        /// 设置节点的Wifi密码
        /// </summary>
        /// <param name="ID">节点ID 2byte</param>
        /// <param name="password">wifi密码，不带中文，Ascii表示 固定32Byte,0x00补足wifi不够32Byte</param>
        void onSetJieDianWifiPassWord(byte[] ID, byte[] password);

        /// <summary>
        /// 设置节点的Wifi密码
        /// </summary>
        /// <param name="ID">节点ID 2byte</param>
        /// <param name="password">wifi密码，不带中文，Ascii表示 固定32Byte,0x00补足wifi不够32Byte</param>
        void onReadJieDianWifiPassWord(byte[] ID, byte[] password);

        /// <summary>
        /// 查找周围参考点ID //两个字节的ID
        /// </summary>
        void onCheckCanKaoDianID(byte[] ID);


        /// <summary>
        /// 查找周围参考点的版本
        /// </summary>
        /// <param name="ID">参考点ID 2byte</param>
        /// /// <param name="type">子设备类型</param>
        /// <param name="version">4个byte 例如0x11071210，表示17年7月18版本号为1.0</param>
        void onCheckCanKaoDianVersion(byte[] ID,byte type,byte[] version);


        /// <summary>
        /// 查找周围卡片ID //两个字节的ID
        /// </summary>
        void onCheckTagID(byte[] ID);
        

        /// <summary>
        /// 查找周围卡片的版本
        /// </summary>
        /// <param name="ID">卡片ID 2byte</param>
        /// <param name="type">子设备类型</param>
        /// <param name="version">4个byte 例如0x11071210，表示17年7月18版本号为1.0</param>
        void onCheckTagVersion(byte[] ID,byte type, byte[] version);


        /// <summary>
        /// 设置卡片的定位封包上报间隔时间
        /// </summary>
        /// <param name="ID">卡片ID 2Byte</param>
        /// <param name="upTime">卡片上报时间 2Byte</param>
        void onSetTagUpTime(byte[] ID,byte[] upTime);

        /// <summary>
        /// 设置卡片的定位封包上报间隔时间,此协议乃是V4.2灯控定位，上报时间仅一个字节
        /// </summary>
        /// <param name="ID">卡片ID 2Byte</param>
        /// <param name="upTime">卡片上报时间 1Byte</param>
        void onSetTagUpTime(byte[] ID, byte upTime);

        /// <summary>
        /// 查找周围卡片ID //两个字节的ID
        /// </summary>
        //void onSetTagSleepTime(byte[] ID, byte sleepTime,byte[] TimeOut);


        /// <summary>
        /// 读取卡片的定位封包上报间隔时间
        /// </summary>
        /// <param name="ID">卡片ID 2Byte</param>
        /// <param name="upTime">卡片上报时间 2Byte</param>
        void onReadTagUpTime(byte[] ID, byte[] upTime);

        /// <summary>
        /// 读取卡片的定位封包上报间隔时间
        /// </summary>
        /// <param name="ID">卡片ID 2Byte</param>
        /// <param name="upTime">卡片上报时间 Byte</param>
        void onReadTagUpTime(byte[] ID, byte upTime);

        /// <summary>
        /// 设置卡片的发射功率
        /// </summary>
        /// <param name="ID">卡片ID 2Byte</param>
        /// <param name="RF">发射功率，范围1~16</param>
        void onSetTagRF(byte[] ID, byte RF);

        /// <summary>
        /// 读取卡片的发射功率
        /// </summary>
        /// <param name="ID">卡片ID 2Byte</param>
        /// <param name="RF">发射功率，范围1~16</param>
        void onReadTagRF(byte[] ID, byte RF);

        /// <summary>
        /// 节点复位
        /// </summary>
        /// <param name="ID"></param>
        void onNODEfuwei(byte[] ID);

        /// <summary>
        /// 设置节点模式，model = 1 ->DHCP  ||  model = 0 ->静态模式
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="model"></param>
        void onSetNODEmodel(byte[] ID, byte model);

        /// <summary>
        /// 读取节点模式，model = 1 ->DHCP  ||  model = 0 ->静态模式
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="model"></param>
        void onReadNODEmodel(byte[] ID, byte model);

        /// <summary>
        /// 设置节点IP
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="IP"></param>
        void onSetNODE_IP(byte[] ID, byte[] IP);

        /// <summary>
        /// 读取节点IP
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="IP"></param>
        void onReadNODE_IP(byte[] ID, byte[] IP);

        /// <summary>
        /// 设置节点的submask
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="IP"></param>
        void onSetNODESubMask(byte[] ID, byte[] IP);

        /// <summary>
        /// 读取节点的submask
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="IP"></param>
        void onReadNODESubMask(byte[] ID, byte[] IP);

        /// <summary>
        /// 设置节点的GateWay
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="IP"></param>
        void onSetNODEGateWay(byte[] ID, byte[] IP);

        /// <summary>
        /// 读取节点的GateWay
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="IP"></param>
        void onReadNODEGateWay(byte[] ID, byte[] IP);

        /// <summary>
        /// 清空USB——Dangle中缓存的Hex数据
        /// </summary>
        void clearTag();

        /// <summary>
        /// 设置信号强度阀值
        /// </summary>
        void onSet_XinHaoQiangdu_(byte[] ID, byte Threshold);

        /// <summary>
        /// 读取信号强度阀值
        /// </summary>
        void onRead_XinHaoQiangdu_(byte[] ID, byte Threshold);

        /// <summary>
        /// 设置信号强度系数
        /// </summary>
        void onSet_XinHaoQiangduXiShu_(byte[] ID, byte k1);

        /// <summary>
        /// 设置信号强度系数
        /// </summary>
        void onRead_XinHaoQiangduXiShu(byte[] ID, byte k1);

        void onCheckVersionBack();
    }
}
