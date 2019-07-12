using CiXinLocation.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiXinLocation.bean
{
    class ID_DType
    {
        private string id;
        private int id_index;
        private DrivaceType mDType;

        public ID_DType(int index, DrivaceType dType)
        {
            id_index = index;
            mDType = dType;
        }

        public ID_DType(string id, DrivaceType dType)
        {
            this.id = id;
            mDType = dType;
        }

        public int Id_index {
            get { return id_index;}
        }

        public string Id
        {
            get { return id; }
        }

        public DrivaceType MDType {
            get { return mDType; }
        }

    }
}
