using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace CiXinLocation.bean
{
    /// <summary>
    /// 区域
    /// </summary>
    [Serializable]
    public class QuYuBean
    {

        public QuYuBean(){}

        private string quyuName;
        private string quyuID;
        private string quyuType = "一般区域";
        private string mapID;
        private string mapPath;
        private Color begin_color;
        private Color end_color;
        private int pepleCount;
  

        public Color Begin_color 
        {
            get { return begin_color;}
            set { begin_color = value; }
        }

        public Color End_color
        {
            get { return end_color; }
            set { end_color = value; }
        }

        public string MapPath
        {
            get
            {
                if (quyuName == null || "".Equals(quyuName)) return "";
                if (isPath(mapPath)) return mapPath;
                string imagePath = Environment.CurrentDirectory + "\\" + mapID;
                if (isPath(imagePath)) return imagePath;
                imagePath = Environment.CurrentDirectory + "\\image\\" + mapID;
                if (isPath(imagePath)) return imagePath;
                imagePath = Environment.CurrentDirectory + "\\IMAGE\\" + mapID;
                if (isPath(imagePath)) return imagePath;
                imagePath = Environment.CurrentDirectory + "\\Image\\" + mapID;
                return imagePath;
            }
            set { mapPath = value; }
        }

        private bool isPath(string path) 
        {
            return File.Exists(path);
        }


        public string MapID
        {
            get { return mapID; }
            set { mapID = value; }
        }

        public string QuyuType
        {
            get { return quyuType; }
            set { quyuType = value; }
        }

        public string QuyuID
        {
            get { return quyuID; }
            set { quyuID = value; }
        }

        public string QuyuName 
        {
            get { return quyuName; }
            set { quyuName = value; }
        }

        public int PepleCount
        {
            get { return pepleCount; }
            set { pepleCount = value; }
        }


    }
}
