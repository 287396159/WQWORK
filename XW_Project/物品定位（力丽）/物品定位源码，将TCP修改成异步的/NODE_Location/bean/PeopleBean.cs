using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoveableListLib.Bean
{
    [Serializable]
    public class PeopleBean
    {        
        private string name;
        private string id;
        private string passWord;
        private int jurisdiction;//权限
        private int powerValue; //权限值


        public int PowerValue
        {
            get { return powerValue; }
            set { powerValue = value; }
        }

        public string Name 
        {
            get { return name; }
            set { name = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
         
        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }

        public int Jurisdiction
        {
            get { return jurisdiction; }
            set { jurisdiction = value; }
        }

        public string JurisdictionStr
        {
            get 
            {
                if (Jurisdiction == 0) return "系統管理員";
                else if (Jurisdiction == 1) return "超級用戶";
                else if (Jurisdiction == 2) return "管理員";
                else if (Jurisdiction == 3) return "一般用戶";
                else if (Jurisdiction == 4) return "從機用戶";
                return "";
            }            
        }

        public static int JurisdictionStrToInt(string JurisdictionStr)
        {
            if (JurisdictionStr.Equals("系統管理員")) return 0;
            else if (JurisdictionStr.Equals("超級用戶")) return 1;
            else if (JurisdictionStr.Equals("管理員")) return 2;
            else if (JurisdictionStr.Equals("一般用戶")) return 3;
            else if (JurisdictionStr.Equals("從機用戶")) return 4;
            else return  -1;
        }

    }
}
