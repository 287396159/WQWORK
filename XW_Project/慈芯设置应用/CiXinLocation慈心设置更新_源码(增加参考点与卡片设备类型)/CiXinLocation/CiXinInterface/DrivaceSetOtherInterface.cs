using CiXinLocation.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiXinLocation.CiXinInterface
{
    interface DrivaceSetOtherInterface : DrivaceSetInterface
    {
        /// <summary>
        /// 读取所有的参数
        /// </summary>
        void readAllParameter(DrivaceType driType,byte[] ID);

        /// <summary>
        /// 设置Channel值
        /// </summary>
        /// <param name="channel"></param>
        void setChannel(byte channel);

        /// <summary>
        /// 獲取Channel值
        /// </summary>
        /// <param name="channel"></param>
        byte getChannel();
    }
}
