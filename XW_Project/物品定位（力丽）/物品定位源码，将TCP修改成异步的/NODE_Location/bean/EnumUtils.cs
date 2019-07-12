using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiXinLocation.bean
{
    class EnumUtils
    {
    }

    public enum DrivaceTypeAll
    {
        NOTHING = 0,
        CARD = 1,
        CANKAODIAN = 2,
        NODE = 3,
        USB_DANGLE = 4,

        NODRIVACE = 5,
        LOCATION = 6,
        TAG = 7,
        TAG2 = 5,
    }

    enum ButtonTag 
    { 
        CHANGQU_UP = 1,
        CHANGQU_DOWN = 2,
        QUYU_UP = 3,
        QUYU_DOWN = 4,
    }
    enum CenQuType
    { 
        CENGJI = 1,
        QUYU = 2,
    }
    public enum SetCanKaoDianType //設置參考點的狀態
    {
        ADD = 1,
        DELETE = 2,
        RETURN = 3,
    }

    public enum LocationType //定位类型
    {
        NOTHING = 0,//没有
        SINGLE = 1,//单点
        DOUBLE = 2,//两点
        TREE = 3,//三点
    }

    public enum ListPageType 
    {
        FIRST = 0,//首页
        UP = 1,   //上一页
        DOWN = 2, //下一页
        LAST = 3,  //尾页
        NOWAY = 4 //什么都不是
    }

    public enum TCPSendType
    {
        PUTONG = 1, //普通
        FORWORD = 2,//实时装发数据
    }


    public enum DataPacketType
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
        SET_WITETIME = 0x016,

        SET_CARD_XinHaoQiangdu = 0X017,//设置信号阀值强度
        SET_CARD_XinHaoQiangduXiShu = 0X018,//设置信号阀值强度系数
    }

    public enum CardUpType //卡片数据的一些类型
    {
        HISTORY_DATA = 0X01,
        CURRENT_DATA = 0X02,
    }


}
