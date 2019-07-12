using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiXinLocation.CiXinInterface
{
    interface ScourDataInterface
    {
        /// <summary>
        /// 接收到的源数据
        /// </summary>
        /// <param name="bt">byte数组的数据</param>
        /// <param name="socurDataBuf">byte字符串形式源数据</param>
        void backData(byte[] bt,StringBuilder socurDataBuf);
    }
}
