using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiXinLocation.Utils
{
   public enum DrivaceTypeEnum
    {
         NODRIVACE = 0,
         NODE = 1,
         LOCATION = 2,
         TAG = 3,
         USB_DANGLE = 4,
         TAG2 = 5,
    }
   public enum CommunicationMode
   {
       NOCOM = 0,
       SERIALPORT = 1,
       UDP = 2,
       TCP = 3,
   }

    /// <summary>
    /// 写一个发送的类型吧。免得说不知道谁发给谁，类型有可能增加
    /// </summary>
   public enum SendMode
   {
       LOCA = 0,//自己本地传递
       OUT = 1, //数据是发送给外地
       ALL = 1, //都可能是
   }

   enum DataPacketType
   {
       SET_SERVISE_IP = 0x01,
       SET_SERVISE_PORT = 0x03,
       SET_WIFI_NAME = 0x05,
       SET_WIFI_PASSWORD = 0x07,
       SET_NODE_MODEL = 0x09,
       SET_NODE_IP = 0x0B,
       SET_SUBMASK = 0X0D,
       SET_GATEWAY = 0X0F,
       SET_CARD_UPTIME = 0X11,
       SET_CARD_POWER = 0X013,
       SET_CARD_CLOSE_UPDATA = 0X015,
       RESET = 0x014,
       SET_CARD_XinHaoQiangdu = 0X016,//设置信号阀值强度
       SET_CARD_XinHaoQiangduXiShu = 0X017,//设置信号阀值强度系数
       SET_CARD_STATICTIME = 0X18,
       SET_CARD_GSENSOR = 0X019,
       CanKaoRESET = 0x020,
       SET_WIFINODE_MODEL = 0X021,
       SET_WIFINODE_STATICIP = 0X022,
       SET_BOTELV = 0x023,
   }
}
