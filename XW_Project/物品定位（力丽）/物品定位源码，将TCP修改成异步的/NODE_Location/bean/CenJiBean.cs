using MoveableListLib.Bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiXinLocation.bean
{
    /// <summary>
    /// 层级
    /// </summary>
    [Serializable]
    public class CenJiBean
    {

        public CenJiBean()
        {
            //chFlBean = new CacheFileBean();
            quYuBeans = new List<QuYuBean>();
        }
      
        private string cenJiName;
        private string iD;
        private List<QuYuBean> quYuBeans;

        public string CenJiName 
        {
            get { return cenJiName; }
            set { cenJiName = value; }
        }

        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }

        public List<QuYuBean> QuYuBeans
        {
            get { return quYuBeans; }
            set { quYuBeans = value; }
        }

        public bool isHaveQuYuBeanName(string quyuName) 
        {
            if (quYuBeans == null || quYuBeans.Count == 0) return false;
            var quItem = quYuBeans.Where(item => item.QuyuName.Equals(quyuName));
            if (quItem.Count() > 0) return true;
            else return false;
        }

        public bool isHaveQuYuBeanID(string quyuID)
        {
            if (quYuBeans == null || quYuBeans.Count == 0) return false;
            var quItem = quYuBeans.Where(item => item.QuyuID.Equals(quyuID));
            if (quItem.Count() > 0) return true;
            else return false;
        }


    }
}
