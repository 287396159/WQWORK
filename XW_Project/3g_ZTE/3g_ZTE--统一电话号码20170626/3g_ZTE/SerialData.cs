using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3g_ZTE
{
    class SerialData 
    {
        //进入设置模式
        public static readonly byte[] inSettingState = new byte[6] { 0XFD, 0XA0, 0X00, 0X00, 0X0D, 0X0A };
        //退出设置模式
        public static readonly byte[] exitSettingState = new byte[6] { 0XFD, 0XA1, 0X00, 0X00, 0X0D, 0X0A };
        //设置单片机IO口输入协议头
        public static readonly byte[] inputSet = new byte[4] { 0XFD, 0XC0, 0X03, 0XD1 };
        //设置单片机IO口输出协议头
        public static readonly byte[] outputSet = new byte[4] { 0XFD, 0XC1, 0X00, 0X5C };
        //请求向单片机读取IO口输入的设置
        public static readonly byte[] readFlashInputSet = new byte[6] { 0XFD, 0XE0, 0X00, 0X00, 0X0D, 0X0A };
        //请求向单片机读取IO口输出的设置
        public static readonly byte[] readFlashOutputSet = new byte[6] { 0XFD, 0XE1, 0X00, 0X00, 0X0D, 0X0A };
        //IO口设置时成功的固定字符串
        public static readonly byte[] replySetSuccess = new byte[8] { 0X5A, 0X83, 0X00, 0X02, 0X00, 0X00, 0X0D, 0X0A };
        //对IO口设置时失败的固定字符串
        public static readonly byte[] replySetFail = new byte[8] { 0X5A, 0X83, 0X00, 0X02, 0X01, 0X01, 0X0D, 0X0A };

        public static bool configuration = false;
        public static bool CommandOK = false;
        //串口数据缓存
        public static Byte[] serialBuff = new Byte[1024];
        //IO input buff
        public static Byte[] currentDataBuff_IO_Input = new Byte[1024];
        //IO output buff
        public static Byte[] currentDataBuff_IO_Output = new Byte[128];
    }
}
