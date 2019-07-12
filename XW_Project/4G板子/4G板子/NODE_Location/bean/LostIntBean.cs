using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiXinLocation.bean
{
    /// <summary>
    /// 丢包的Int属性
    /// </summary>
    class LostIntBean
    {
        private int locationtHeoreticalValue = 0;//理论值
        private int locationCount = 3;           //参考点数量
        private int lostCount = 0;               //丢包统计
        private int locationReve = 0;            //接收的数量
        private int jumpPoint = 0;               //跳点

        public LostIntBean() {         
        }

        /// <summary>
        /// 理论值
        /// </summary>
        public int LocationtHeoreticalValue {
            get { return locationtHeoreticalValue; }
            set { locationtHeoreticalValue = value; }
        }

        /// <summary>
        /// 参考点数量
        /// </summary>
        public int LocationCount
        {
            get { return locationCount; }
            set { locationCount = value; }
        }

        /// <summary>
        /// 丢包统计
        /// </summary>
        public int LostCount
        {
            get { return lostCount; }
            set { lostCount = value; }
        }

        /// <summary>
        /// 接收的数量
        /// </summary>
        public int LocationReve
        {
            get { return locationReve; }
            set { locationReve = value; }
        }

        /// <summary>
        /// 跳点
        /// </summary>
        public int JumpPoint
        {
            get { return jumpPoint; }
            set { jumpPoint = value; }
        }


    }
}
