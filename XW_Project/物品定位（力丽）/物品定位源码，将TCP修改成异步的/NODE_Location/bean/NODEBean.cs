using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiXinLocation.bean
{
    [Serializable]
    public class NODEBean
    {
        private string name;
        private string id;

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

        public NODEBean() { }


    }
}
