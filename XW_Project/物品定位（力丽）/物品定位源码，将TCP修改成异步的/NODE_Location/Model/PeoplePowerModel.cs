using MoveableListLib.Bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiXinLocation.Model
{
    class PeoplePowerModel
    {
        private static PeoplePowerModel peopleModel;     
        private static object obje = new object();
        private int powerValue = 0;  // 人员权限值


        private int jurisdiction = 50;//值越大，权限越小；  
      
        private string count;
        private string password;
        private int congjiValue = 105;
        private bool isConnect = false;              

        private PeoplePowerModel(){}

        public static PeoplePowerModel getPeoplePowerModel()
        {
            if (peopleModel == null)
            {
                lock (obje)
                {
                    if (peopleModel == null) peopleModel = new PeoplePowerModel();
                }
            }
            return peopleModel;
        }

        public bool textPeoplePower(string count ,string password) 
        {
            List<PeopleBean> peoples  = FileModel.getFlModel().ChFlBean.Peoples.ToList();
            var peopleCounts = peoples.Where(a => a.Name.Equals(count) &&　a.PassWord.Equals(password));
            if (peopleCounts.Count() == 0) return false;
            PeopleBean people = peopleCounts.First();
            powerValue = people.PowerValue;
            jurisdiction = people.Jurisdiction;
            return true;
        }

        public bool IsConnect
        {
            get { return isConnect; }
            set { isConnect = value; }
        }

        public void congJiPeoplePower(string count, string password)
        {
            powerValue = AllPower;
            jurisdiction = CongjiValue;
            this.count = count;
            this.password = password;
        }

        public int CongjiValue
        {
            get { return congjiValue; }
            set { congjiValue = value; }
        }

        public int PowerValue
        {
            get { return powerValue; }
            set { powerValue = value; }
        }

        public string Count
        {
            get { return count; }
            set { count = value; }
        }

        public void startCheck() 
        {
            powerValue |= 1;
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public int Jurisdiction
        {
            get { return jurisdiction; }
            set { jurisdiction = value; }
        }

        /// <summary>
        /// 是否有软件开启与关闭权限
        /// </summary>
        /// <returns></returns>
        public bool isHaveExeOC() 
        {
            return (powerValue & exeOC) > 0;
        }

        /// <summary>
        /// 是否有监控权限
        /// </summary>
        /// <returns></returns>
        public bool isMonitor()
        {
            return (powerValue & monitor) > 0;
        }

        /// <summary>
        /// 是否有参数打開或者關閉
        /// </summary>
        /// <returns></returns>
        public bool isCanshuOC()
        {
            return (powerValue & canshuOC) > 0;
        }

        /// <summary>
        /// 是否有报警权限
        /// </summary>
        /// <returns></returns>
        public bool isWarnDeal()
        {
            return (powerValue & warnDeal) > 0;
        }

        /// <summary>
        /// 是否有查询追踪权限
        /// </summary>
        /// <returns></returns>
        public bool isSerchTag()
        {
            return (powerValue & serchTag) > 0;
        }

        /// <summary>
        /// 是否有历史数据取得与输出,,與Excel或者txt導入與到出
        /// </summary>
        /// <returns></returns>
        public bool isHisData()
        {
            return (powerValue & hisData) > 0;
        }

        /// <summary>
        /// 是否有人员操作權限
        /// </summary>
        /// <returns></returns>
        public bool isPeopleOperation()
        {
            return (powerValue & peopleOperation) > 0;
        }

        /// <summary>
        /// 是否有動態顯示權限
        /// </summary>
        /// <returns></returns>
        public bool isLocationShow() 
        {
            return (powerValue & locationShow) > 0;
        }

        /// <summary>
        /// 是否有參數更改權限
        /// </summary>
        /// <returns></returns>
        public bool isCanshuChange() 
        {
            return (powerValue & canshuChange) > 0;
        }
        /// <summary>
        /// 是否有所有权限
        /// </summary>
        /// <returns></returns>
        public bool isAllPower()
        {
            return powerValue > allPower;
        }

        
        private int exeOC = 1;     // 0001
        /// <summary>
        /// 软件开启与关闭权限
        /// </summary>
        public int ExeOC
        {
            get { return exeOC; }
        }
        
        private int monitor = 2;   // 0010
        /// <summary>
        /// 监控权限
        /// </summary>
        public int Monitor
        {
            get { return monitor; }
        }
       
        private int canshuOC = 4; // 0100
        /// <summary>
        /// 参数打開關閉权限
        /// </summary>
        public int CanshuOC
        {
            get { return canshuOC; }
        }
        
        private int warnDeal = 8;  // 1000
        /// <summary>
        /// 报警权限
        /// </summary>
        public int WarnDeal
        {
            get { return warnDeal; }
        }
        
        private int serchTag = 16; // 0001 0000
        /// <summary>
        /// 查询追踪权限
        /// </summary>
        public int SerchTag
        {
            get { return serchTag; }
        }
        
        private int hisData = 32;  // 0010 0000
        /// <summary>
        /// 历史数据取得与输出,與Excel或者txt導入與到出
        /// </summary>
        public int HisData
        {
            get { return hisData; }
        }
        
        private int locationShow = 64;
        /// <summary>
        /// 動態定位顯示
        /// </summary>
        public int LocationShow
        {
            get { return locationShow; }
        }
        
        private int canshuChange = 128;//1000 0000
        /// <summary>
        /// 參數更改
        /// </summary>
        public int CanshuChange
        {
            get { return canshuChange; }
        }
        
        private int peopleOperation = 256; //0001 0000 0000        
        /// <summary>
        /// 人员操作权限
        /// </summary>
        public int PeopleOperation
        {
            get { return peopleOperation; }
        }

        /// <summary>
        /// 所有权限
        /// </summary>
        private int allPower = 1023;//11 1111 1111
        public int AllPower
        {
            get { return allPower; }
        }
    }
}
