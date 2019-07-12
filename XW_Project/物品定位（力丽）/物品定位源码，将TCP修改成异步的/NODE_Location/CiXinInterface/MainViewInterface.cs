using CiXinLocation.bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiXinLocation.CiXinInterface
{
    /// <summary>
    /// 主界面修改View參數
    /// </summary>
    interface MainViewInterface
    {

        /// <summary>
        /// 总人数数量
        /// </summary>
        /// <param name="count"></param>
        void onPeopleAllCount(int count);

        /// <summary>
        /// 区域人员数量
        /// </summary>
        /// <param name="cenJiPeopleCount"></param>
        void onQuyuPeopleCount( List<CenJiBean> cenJiPeopleCount);

        /// <summary>
        /// 修改区域名称
        /// </summary>
        /// <param name="cenjiID"></param>
        /// <param name="quyuID"></param>
        /// <param name="changeName"></param>
        void onQuYuName(string cenjiID, string quyuID, string changeName);

        /// <summary>
        /// 修改层级名称
        /// </summary>
        /// <param name="cenjiID"></param>
        /// <param name="cenJiName"></param>
        void onCenJiName(string cenjiID, string cenJiName);

        void message(string msg,int type);
    }
}
