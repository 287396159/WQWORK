using CiXinLocation.bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiXinLocation.CiXinInterface
{
    interface MainFromInterface
    {
        /// <summary>
        /// 设置节点的ServiseIP
        /// </summary>
        /// <param name="IP">设置成功的IP 4个Byte</param>
        void onSetJieDianServiseIP( byte[] IP,byte[] port);

        /// <summary>
        /// 读取节点的ServiseIP
        /// </summary>
        /// <param name="IP">读取到的IP 4个Byte</param>
        void onReadJieDianServiseIP(byte[] IP, byte[] port);


        /// <summary>
        /// 设置波特率
        /// </summary>
        /// <param name="port">设置节点的端口 2个Byte</param>
        void onSetBoTeLvPort(byte port);

        /// <summary>
        /// 读取节点的Servise Port
        /// </summary>
        /// <param name="port">读取节点的端口 2个Byte</param>
        void onReadBoTeLvPort(byte port);
    }

    interface DrivaceSetOtherInterface : MainFromInterface
    {
        /// <summary>
        /// 读取所有的参数
        /// </summary>
        void readAllParameter();
    }
}
