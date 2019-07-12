using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiXinLocation.bean
{
    [Serializable]
    class HistoryFileDataBean
    {

        //文件名称
        private string fileName;            
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        //存储的开始时间
        private long startTime;
        public long StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        //存储的结束时间
        private long endTime;
        public long EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        //文件地址
        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
            
        //文件大小
        private long fileSize = 0;
        public long FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        //卡片ID
        private List<string> cardID;
        public List<string> CardID
        {
            get { return cardID; }
            set { cardID = value; }
        }

        //是否正在存储中
        private bool isCache;

        public bool IsCache
        {
            get { return isCache; }
            set { isCache = value; }
        }
        

        /*private List<FileData> fileDatas;
        private List<FileData> FileDatas
        {
            get { return fileDatas; }
            set { fileDatas = value; }
        }*/

        public HistoryFileDataBean()
        {
            //fileDatas = new List<FileData>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="day">day为月份，比如 2017年11月27日</param>
        public HistoryFileDataBean(long time,string month,long endTime)
        {
            FileName = time + ".dat";
            IsCache = true;
            FileSize = 0;
            StartTime = time;
            EndTime = endTime;
            FilePath = month;
        }
    }
}
