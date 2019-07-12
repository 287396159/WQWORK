using CiXinLocation.Utils;
using PersionAutoLocaSys.Bean;
using PersionAutoLocaSys.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersionAutoLocaSys.Model
{
    class FileModel
    {
        private static FileModel fileMode;
        private static object obj = new object();
        private int saveAdExitCount = 0;
        private bool isSaveAdExitCount = false;

        private FileModel() { }

        public static FileModel fileInit() 
        {
            if (fileMode == null) 
            {
                lock (obj) 
                {
                    if (fileMode == null)
                        fileMode = new FileModel();
                }
            }
            return fileMode;
        }

        /// <summary>
        /// 将AdmissionExit存储到文件中
        /// </summary>
        public void saveHisAdmission()
        {
            AdmissionExitFileUtils aefUtils = new AdmissionExitFileUtils();
            aefUtils.saveHisAdmissionExit();
        }

        /// <summary>
        /// 存储所有刷卡记录
        /// </summary>
        public void saveHisAllAdmission()
        {
            AdmissionExitFileUtils aefUtils = new AdmissionExitFileUtils();
            aefUtils.saveHisAllAdmissionExit();
        }

        /// <summary>
        /// 获取文件中AdmissionExit数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<AdmissionExit> getHisAdmission(DateTime dt)
        {
            List<AdmissionExit> hisAdmissionExits = new List<AdmissionExit>();
            AdmissionExitFileUtils aefUtils = new AdmissionExitFileUtils();
            hisAdmissionExits.AddRange(aefUtils.getHistoryAdmissionExits(dt));
            return hisAdmissionExits;
        }

        /// <summary>
        /// 获取文件中AdmissionExit数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
         public List<AdmissionExit> getHisAdmission(DateTime start,DateTime endDt)
         {
             List<AdmissionExit> hisAdmissionExits = new List<AdmissionExit>();
             AdmissionExitFileUtils aefUtils = new AdmissionExitFileUtils();
             hisAdmissionExits.AddRange(aefUtils.getHistoryAdmissionExits(start, endDt));
             return hisAdmissionExits;
         }
        
        public List<AdmissionExit> getHisAllAdmission(DateTime start, DateTime endDt)
         {
             List<AdmissionExit> hisAllAdmissionExits = new List<AdmissionExit>();
             AdmissionExitFileUtils aefUtils = new AdmissionExitFileUtils();
             hisAllAdmissionExits.AddRange(aefUtils.getHistoryAllAdmissionExits(start, endDt));
             return hisAllAdmissionExits;
         }

        /// <summary>
        /// 一句需求，下面所有付出，都是虛無，想想都是好的，都不知道怎麼想的
                 //将进出场数据存储到相关的文件的一系列逻辑操作
        /// </summary>
        public void saveAdmissionExits()   
        {
            saveAdExitCount++;
            if (isSaveAdExitCount) //发现已有线程在存储数据中
            {
                saveAdExitCount++;
                return;
            }
            isSaveAdExitCount = true;            
            saveAdmissionExits(1);
            removeAdmissionExitTimeOut();
        }

        private void saveAdmissionExits(int aeCount)
        {
            saveAdExitCount = 0;
            lock (CommonCollection.admissionExits)
            {
                List<AdmissionExit> save = CommonCollection.admissionExits.ToList();
                DataFileUtils dFileUtils = new DataFileUtils();

                dFileUtils.SerializeCacheBean2(save, "AdmissionExit.dat");

                Object obj = null;
                dFileUtils.Deserialize("admissionExit.bat", ref obj);
            }
            if (saveAdExitCount > 0) saveAdmissionExits(1); //saveAdExitCount>0 说明，数据已经更新，需要再次存储一下
            else isSaveAdExitCount = false;
        }


        /// <summary>
        /// 删掉超时不要的数据，目前为3天前的数据
        /// </summary>
        private void removeAdmissionExitTimeOut() 
        {
            DateTime dayBefore = DateTime.Now.AddDays(-3);
            DateTime beforeTime = new DateTime(dayBefore.Year,dayBefore.Month,dayBefore.Day);
            UInt32 dTime = GetTimeStamp(beforeTime,true);
            if (CommonCollection.admissionExits.Count > 0) 
            {
                if (CommonCollection.admissionExits[0].Time > dTime) return;
            }
            List<AdmissionExit> save = CommonCollection.admissionExits.ToList();
            lock (CommonCollection.admissionExits)
            {
                for (int i = 0; i < save.Count;i++ )
                {
                    if (save[i].Time < dTime)
                        CommonCollection.admissionExits.Remove(save[i]);
                    else break;
                }            
            }

            if (save.Count > 0)
            {
                if (save[0].Time < dTime) saveAdmissionExits();  //都删掉了一下数据，更新一下数据库呗
            }
        }

        /// <summary>  
        /// 获取当前时间戳  
        /// </summary>  
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.</param>  
        /// <returns></returns>  
        public UInt32 GetTimeStamp(DateTime UtcNow, bool bflag)
        {
            TimeSpan ts = UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            UInt32 time = 0;
            if (bflag)
                time = Convert.ToUInt32(ts.TotalSeconds);
            else
                time = (UInt32)Convert.ToInt64(ts.TotalMilliseconds);
            return time;
        }

        /// <summary>
        /// 从文件中获取AdmissionExit数据，并且加载到CommonCollection.admissionExits中
        /// </summary>
        public void getSysAdmissionExit()
        {
            List<AdmissionExit> admissExits = getAdmissionExit();
            if(admissExits != null)
            {
                CommonCollection.admissionExits = admissExits;
            }
        }

        public List<AdmissionExit> getAdmissionExit() 
        {
            List<AdmissionExit> admissExits = new List<AdmissionExit>();
            DataFileUtils dFileUtils = new DataFileUtils();
            Object obj = null;
            dFileUtils.Deserialize("AdmissionExit.dat", ref obj);
            if (null == obj) return null;
            admissExits = obj as List<AdmissionExit>;
            return admissExits;
        }


        public bool setTagInforData()
        {
            DataFileUtils dFileUtils = new DataFileUtils();
            return dFileUtils.serializeObject(CommonCollection.Tags, "tag.dat");
        }

        public bool saveBackUpTagInforData()
        {
            DataFileUtils dFileUtils = new DataFileUtils();
            return dFileUtils.serializeObject(CommonCollection.Tags, "BackUp\\tag.dat");
        }

        public bool getBackUpCommCollTags()
        {
            Dictionary<string, Tag> cachTags = getTagInforData("BackUp\\tag.dat");
            if (cachTags == null) return false;
            CommonCollection.Tags = cachTags;
            return true;
        }

        public bool getCommCollTags()
        {
            Dictionary<string, Tag> cachTags = getTagInforData("tag.dat");
            if (cachTags == null) return false;
            CommonCollection.Tags = cachTags;
            return true;
        }

        /// <summary>
        /// 获取tag资料
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,Tag>  getTagInforData(String pathName)
        {
            Dictionary<string, Tag> tags = new Dictionary<string,Tag>();
            DataFileUtils dFileUtils = new DataFileUtils();
            Object obj = null;
            dFileUtils.Deserialize(pathName, ref obj);
            if (null == obj) return null;
            tags = obj as Dictionary<string, Tag>;
            return tags;
        }

    }
}
