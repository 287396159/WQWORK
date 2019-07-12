using CiXinLocation.Utils;
using PersionAutoLocaSys.Bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersionAutoLocaSys.Utils
{
    /// <summary>
    /// 进出场历史数据存储到文件类
    /// </summary>
    class AdmissionExitFileUtils
    {
        private string filePath;//存储的文件

        private byte[] bt;//存储数据
        private AdmissionExitEnum adexUm = AdmissionExitEnum.PUTONG;

        public AdmissionExitFileUtils(){}


        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }


        public AdmissionExitEnum AdexUm
        {
            get { return adexUm; }
            set { adexUm = value; }
        }


        /// <summary>
        /// 设置文件目录
        /// </summary>
        public void setFile() 
        {
            DateTime dt = DateTime.Now;
            FilePath = getDateTimeStr(dt);
        }

        private String getDateTimeStr(DateTime dt) 
        {
            return getDateTimeStr(dt, adexUm);
        }

        private String getDateTimeStr(DateTime dt, AdmissionExitEnum adum) 
        {
            String path = "";
            //path = "History\\" + dt.Year + "with" + dt.Month + "\\" + dt.ToString("yyyyMMdd") + ".dat";
            if (adum == AdmissionExitEnum.PUTONG)
            {
                path = "History\\" + dt.Year + "" + dt.Month + "\\" + dt.ToString("yyyyMMdd") + ".dat";
            }
            else 
            {
                path = "History\\" + dt.Year + "with" + dt.Month + "\\" + dt.ToString("yyyyMMdd") + ".dat";
            }
            return path;
        }

        /// <summary>
        /// 存储AdmissionExit数据
        /// </summary>
        public void saveHisAdmissionExit()
        {
            setHisAdmissionExitBinary();
            setFile();
            saveBinaryAdmissionExitData();
        }

        /// <summary>
        /// 将上报的所有刷卡记录存储，
        /// </summary>
        public void saveHisAllAdmissionExit()
        {
            adexUm = AdmissionExitEnum.ALL;
            setHisAllAdmissionExitBinary();           
            setFile();
            saveBinaryAdmissionExitData();
        }

        /// <summary>
        /// 设置二进制数据
        /// </summary>
        public void setHisAdmissionExitBinary()
        {
            setHisAdmissionExitBinary(CommonCollection.admissionExits);
        }

        /// <summary>
        /// 设置保存的所有刷卡记录的二进制数据
        /// </summary>
        public void setHisAllAdmissionExitBinary()
        {
            setHisAdmissionExitBinary(CommonCollection.allAdmissionExits);
        }

        /// <summary>
        /// 设置二进制数据
        /// </summary>
        public void setHisAdmissionExitBinary(List<AdmissionExit> saves)
        {
            if(saves ==null)return;
            List<AdmissionExit> save = null;
            lock (saves)
            {
                save = saves.ToList();
                saves.Clear();
            }
            if (save == null || save.Count == 0) return;
            byte[] date = System.Text.Encoding.ASCII.GetBytes("data");
            byte[] over = System.Text.Encoding.ASCII.GetBytes("over");

            int admissExitLength = save[0].backAdminssionExitBinary().Length;
            int btLength = admissExitLength + 8;
            bt = new byte[save.Count * btLength]; //将存储的对象集合，打包成一个长数组
            for (int i = 0; i < save.Count; i++)
            {
                //if (bt.Length < admissExitLength * i) break;
                Array.Copy(date, 0, bt, btLength * i, date.Length);  //头部标志
                Array.Copy(save[i].backAdminssionExitBinary(), 0, bt, btLength * i + date.Length, admissExitLength); //数据体
                Array.Copy(over, 0, bt, btLength * i + date.Length + admissExitLength, over.Length); //尾部标志
            }
        }

        /// <summary>
        /// 存储二进制数据到文件中
        /// </summary>
        public void saveBinaryAdmissionExitData()
        {
            if (bt == null || bt.Length == 0) return;
            DataFileUtils daUtils = new DataFileUtils();
            daUtils.addDataInFile(bt, FilePath);
        }

        public List<AdmissionExit> getHistoryAllAdmissionExits(DateTime startHisDt, DateTime endHisDt)
        {
            AdexUm = AdmissionExitEnum.ALL;
            return getHistoryAdmissionExits(startHisDt, endHisDt);
        }

        public List<AdmissionExit> getHistoryAdmissionExits(DateTime startHisDt, DateTime endHisDt)
        {
            if (endHisDt.CompareTo(startHisDt) < 0) return null;            
            TimeSpan sp = endHisDt.Subtract(startHisDt);
            int days = sp.Days;
            List<AdmissionExit> allDayhisAdmissionExits = new List<AdmissionExit>();
            for (int i = 0; i <= days;i++ )  //隔一天需要取2天的数据。故此处i <= days
            {
                DateTime curDt = startHisDt.AddDays(i);
                List<AdmissionExit> hisAdmissionExits = getHistoryAdmissionExits(curDt);
                if (hisAdmissionExits != null) allDayhisAdmissionExits.AddRange(hisAdmissionExits);
            }
            return allDayhisAdmissionExits;
        }

        public List<AdmissionExit> getHistoryAdmissionExits(DateTime hisDt) 
        {
            getBinaryAdmissionExitData(hisDt);
            List<AdmissionExit> hisAdmissionExits = getHisAdmissionExitFormByte();
            return hisAdmissionExits;
        }

        /// <summary>
        /// 获取存储的含有AdmissionExit的二进制数据
        /// </summary>
        /// <param name="hisDt"></param>
        public void getBinaryAdmissionExitData(DateTime hisDt)
        {
            FilePath = getDateTimeStr(hisDt);
            DataFileUtils daUtils = new DataFileUtils();
            bt = daUtils.getDataFromFile(FilePath);
            //List<byte[]> admissionExits = getAdmissionExitBt(data);
        }

        /// <summary>
        /// 将二进制数据转换成AdmissionExit数据
        /// </summary>
        /// <returns></returns>
        public List<AdmissionExit> getHisAdmissionExitFormByte() 
        {
            if (bt == null) return null;
            List<byte[]> admissionExits = getAdmissionExitBt(bt);
            List<AdmissionExit> hisAdmissionExits = new List<AdmissionExit>();
            for (int i = 0; i < admissionExits.Count;i++ )
            {
                AdmissionExit hisAexit = new AdmissionExit(AdmissionExit.ADMISSION);
                hisAexit.setHisAdminssionExit(admissionExits[i]);
                hisAdmissionExits.Add(hisAexit);
            }
            return hisAdmissionExits;
        }

        /// <summary>
        /// 将内容拆分出一条条
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<byte[]> getAdmissionExitBt(byte[] data) 
        {
            List<byte[]> chcheBy = new List<byte[]>();
            int length = 0;
            while (length < data.Length - 8) 
            {
                if (data[length] != 'd' || data[1 + length] != 'a' || data[2 + length] != 't' || data[3 + length] != 'a') 
                {
                    length++;
                    continue;
                }
                int overIndex = length + 4;
                while (overIndex < data.Length - 4)
                {
                    if (data[overIndex] != 'o' || data[1 + overIndex] != 'v' || data[2 + overIndex] != 'e' || data[3 + overIndex] != 'r')
                    {
                        overIndex++;
                        continue;
                    }
                    break;
                }
                if (data[overIndex] != 'o' || data[1 + overIndex] != 'v' || data[2 + overIndex] != 'e' || data[3 + overIndex] != 'r')
                {
                    length++;
                    continue;
                }
                byte[] admissionExitData = new byte[overIndex - length - 4];
                Array.Copy(data, length + 4, admissionExitData, 0, admissionExitData.Length);
                chcheBy.Add(admissionExitData);
                length++;
            }

            return chcheBy;
        }

    }
}
