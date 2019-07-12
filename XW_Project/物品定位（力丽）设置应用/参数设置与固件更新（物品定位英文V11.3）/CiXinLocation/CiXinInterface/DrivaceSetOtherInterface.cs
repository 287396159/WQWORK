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
        
    }
}
