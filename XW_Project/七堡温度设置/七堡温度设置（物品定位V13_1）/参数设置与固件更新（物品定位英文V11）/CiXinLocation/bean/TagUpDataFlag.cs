using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiXinLocation.bean
{
    class TagUpDataFlag
    {
        private List<tag> tagS = new List<tag>();

        public TagUpDataFlag(int count) {
            for(int i = 0;i< count;i++){
                tagS.Add(new tag(i+1));
            }
        }

        public List<tag> Tags
        {
            get { return tagS; }
        }       
        
    }

    class tag
    {
        public tag(int tagBtnIndex)
        {
            this.tagBtnIndex = tagBtnIndex;
        }
        public int tagBtnIndex = 0;
        public bool isPath = false;
    }
}
