using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiXinLocation.Utils
{
   public enum DrivaceType
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
   public enum CommunicationMode
   {
       NOCOM = 0,
       SERIALPORT = 1,
       UDP = 2,
       TCPServer = 3,
       TCPClien = 4,
       TCPServer_loca = 5,//TcpServer自己给自己传的数据
       TCPServer_IP = 6,//TcpServer自己给自己传的数据
       TCPClien_loca = 7,//TCPClien自己给自己传的数据
       TCPClien_File = 8,//TCPClien文件传输
       TCPServer_File = 9,//TcpServer文件传输
       TCPServer_OUT = 10,//TcpServer文件協議传输，有相關的文檔，現在有7個協議
   }
}
