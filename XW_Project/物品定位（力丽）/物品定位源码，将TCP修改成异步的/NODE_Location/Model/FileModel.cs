using CiXinLocation.bean;
using CiXinLocation.Utils;
using MoveableListLib.Bean;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CiXinLocation.Model
{

    /// <summary>
    /// 文件数据的操作
    /// </summary>
    class FileModel
    {      
        private FileModel() 
        {
            oldUpCardDatas = new List<CardUpDataBean>();
            hisFileDBeans = new List<HistoryFileDataBean>();
            chFlBean = new CacheFileBean();
            cenJiData = new List<CenJiBean>();
        }        

        /// <summary>
        /// 获取FileModel的对象
        /// </summary>
        /// <returns></returns>
        public static FileModel getFlModel() 
        {
            if (fileModel == null)
            {
                lock (obje)
                {
                    fileModel = new FileModel();
                }
            }
            return fileModel;
        }

        public List<CardUpDataBean> OldUpCardDatas
        {
            get { return oldUpCardDatas; }
            set { oldUpCardDatas = value; }
        }

        public List<HistoryFileDataBean> HisFileDBeans
        {
            get {
                if (hisFileDBeans == null) hisFileDBeans = new List<HistoryFileDataBean>();
                return hisFileDBeans; }
            set { hisFileDBeans = value; }
        }

        public List<CenJiBean> CenJiData 
        {
            get { return cenJiData; }
            set { cenJiData = value; }
        }

        public CacheFileBean ChFlBean
        {
            get { return chFlBean; }
            set { chFlBean = value; }
        }

        public string FileName2Lock
        {
            get { return fileName2Lock; }
        }

        public string FileNameLock
        {
            get { return fileNameLock; }
        }


        public void start() 
        {       
            getData();
            stratPeopleCount();
            getCFData();
            getHisFilInfore();
            peoplePowerValue();
        }

        /// <summary>
        /// 文件传输之后，读取一下问价中的对象值
        /// </summary>
        public void startFile()
        {
            getData();
            getCFData();
        }

        private void peoplePowerValue()
        {
            if (chFlBean == null) return;
            if (chFlBean.Peoples == null || chFlBean.Peoples.Count == 0) return;
            int isHaveDMATEK = 0;
            foreach (var people in chFlBean.Peoples)
            {
                if ("99123456".Equals(people.Id)) 
                {
                    isHaveDMATEK |= 1;
                    if (people.PowerValue != 0xffffff) people.PowerValue = 0xffffff;
                }
                if (people.Id.Equals("fg123456") && people.PowerValue != 0XFFFFFF) 
                {
                    people.PowerValue = 0xffffff;
                    isHaveDMATEK |= 2;
                }                   
            }
            if ((isHaveDMATEK & 1) == 0) chFlBean.Peoples.Add(addChaojiYonhu());
            if (isHaveDMATEK > 0) setCFData();
        }

        /// <summary>
        /// 添加超级用户
        /// </summary>
        private PeopleBean addChaojiYonhu()
        {
            PeopleBean pb = new PeopleBean();
            pb.Name = "DMATEK";
            pb.PassWord = "admin24585448";
            pb.Id = "99123456";
            pb.Jurisdiction = 1;
            pb.PowerValue = 0xffffff;
            return pb;
        }

        private void stratPeopleCount() 
        {
            if (CenJiData == null || CenJiData.Count == 0) return;
            foreach (CenJiBean cItem in CenJiData)
            {
                foreach (QuYuBean qItem in cItem.QuYuBeans)
                {
                    qItem.PepleCount = 0;
                }
            }
            setData();
        }
        
        public void getCFData()
        {
            lock(FileName2Lock)
            {
                DataFileUtils dFileUtils = new DataFileUtils();
                Object obj = null;
                dFileUtils.Deserialize(fileName2, ref obj);
                if (null == obj) return;
                chFlBean = obj as CacheFileBean;
                chFlBean.addCardValues(chFlBean.Cards.ToList());
            }            
        }

        public void setCFData()
        {
            lock (FileName2Lock)
            {
                DataFileUtils dFileUtils = new DataFileUtils();
                dFileUtils.serializeCacheFile(chFlBean, fileName2);
            }
        }
        
        public void getHisFilInfore()
        {
            DataFileUtils dFileUtils = new DataFileUtils();
            Object obj = null;
            dFileUtils.Deserialize(fileName3, ref obj);
            if (null == obj) return;
            hisFileDBeans = obj as List<HistoryFileDataBean>;
        }

        public void setHisFileInfore()
        {
            if (PeoplePowerModel.getPeoplePowerModel().Jurisdiction == PeoplePowerModel.getPeoplePowerModel().CongjiValue)
            {
                return;
            }
            DataFileUtils dFileUtils = new DataFileUtils();
            dFileUtils.serializeObject(hisFileDBeans, fileName3);
        }       

        public List<CardUpDataBean> getHisFileData(string fileName)
        {
            DataFileUtils dFileUtils = new DataFileUtils();
            Object obj = null;
            dFileUtils.Deserialize(fileName4+fileName, ref obj);
            if (null == obj) return null;
            List<CardUpDataBean> cardData = obj as List<CardUpDataBean>;
            return cardData;
        }

        /// <summary>
        /// 读取卡片的二进制数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <param type="type">0 = TCP发送出去的数据，1 = 取出来的原始数据</param>
        /// <returns></returns>
        public List<byte[]> getHisFileListDataData(string fileName,int type) 
        {
            DataFileUtils dFileUtils = new DataFileUtils();
            byte[] data = dFileUtils.getDataFromFile (fileName4 + fileName);
            List<byte[]> chcheBy = new List<byte[]>();
            int index_da = 0;
            int dataLength = 27; //取出数据到TCP发送的数据包长度
            while (true && data != null)
            {
                try 
                {
                    int ov_index;
                    getIndexOver(data, ref index_da, out ov_index, dataLength);
                    if (ov_index > data.Length) break;
                    if (ov_index - index_da - 4 < 1) 
                    {
                        index_da++;
                        continue;
                    }
                    byte[] cardData = new byte[ov_index - index_da - 4];
                    Array.Copy(data, index_da + 4, cardData, 0, cardData.Length);

                    if (cardData.Length > 21 && type == 0)
                    {
                        byte[] hisData = getChongZuData(cardData); ///将历史数据组装起来
                        chcheBy.Add(hisData);                      ///组装的数据保存再此处
                    }
                    else if (type == 1)
                    {
                        chcheBy.Add(cardData);    
                    }      
                    index_da = ov_index + 4;
                }
                catch(Exception e)
                {
                    index_da++;
                    Debug.Write("getHisFileListDataData.."+e.Message);
                }                               
            }
            return chcheBy;
        }

        public byte[] getChongZuData(byte[] cardData) 
        {
            byte[] hisData = new byte[22];
            Array.Copy(cardData, 0, hisData, 0, 17);
            Array.Copy(cardData, 18, hisData, 17, 3);
            hisData[20] = XWUtils.getCheckBit(hisData, 0, 21);
            hisData[21] = 0xfb;
            return hisData;
        }

        /// <summary>
        /// 取其中的一包数据
        /// </summary>
        /// <param name="data">数据源</param>
        /// <param name="dataIndex">“data”的index位置</param>
        /// <param name="overIndex">“over”的index位置</param>
        /// <param name="packLength">数据包长度，宜长不宜短</param>
        private void getIndexOver(byte[] data,ref int dataIndex,out int overIndex,int packLength) 
        {
            while (dataIndex < data.Length)
            {
                if(data[dataIndex] != 'd')
                {
                    dataIndex ++ ;
                    continue;
                }
                if (data[1 + dataIndex] != 'a' || data[2 + dataIndex] != 't' || data[3 + dataIndex] != 'a') 
                {
                    dataIndex++;
                    continue;
                }
                break;
            }
            overIndex = dataIndex + 4;
            while (overIndex < data.Length)
            {
                if (overIndex - dataIndex - 4 > packLength) throw new Exception("此项不存在");
                if (data[overIndex] != 'o')
                {
                    overIndex++;
                    continue;
                }
                if (data[1 + overIndex] != 'v' || data[2 + overIndex] != 'e' || data[3 + overIndex] != 'r')
                {
                    overIndex++;
                    continue;
                }
                break;
            }            
        }

        public void setHisFileData(string fileName)
        {
            if (PeoplePowerModel.getPeoplePowerModel().Jurisdiction == PeoplePowerModel.getPeoplePowerModel().CongjiValue)
            {
                return;
            }

            DataFileUtils dFileUtils = new DataFileUtils();
            List<byte[]> chcheBy = new List<byte[]>();
            try 
            {
                foreach (CardUpDataBean cBean in oldUpCardDatas)
                {
                    if (cBean == null) continue;
                    chcheBy.Add(cBean.getCardUpByte(0x03));
                }
            }
            catch { }            
            dFileUtils.addDataInFile(chcheBy, fileName4 + fileName);
        }

        public void getData() 
        {
            lock (FileNameLock)
            {
                DataFileUtils dFileUtils = new DataFileUtils();
                Object obj = null;
                dFileUtils.Deserialize(fileName, ref obj);
                if (null == obj) return;
                cenJiData = obj as List<CenJiBean>;
                if (getdata != null) getdata(COUNTADD);
            }
        }

        public void setData()
        {
            lock(FileNameLock)
            {
                DataFileUtils dFileUtils = new DataFileUtils();
                dFileUtils.serializeCache(cenJiData, fileName);
                if (getdata != null) getdata(COUNTADD);
            }            
        }

        public void setErrorAppData(string errorMsg)
        {
            DataFileUtils dFileUtils = new DataFileUtils();
            FileMode fMo = FileMode.Append;
            //if (dFileUtils.FileSize(fileName4 + "exception.txt") > 1024 * 1024 * 10) fMo = FileMode.Create;
            dFileUtils.addErrorMsgInFile("\r\n" + XwDataUtils.currentTimeToSe() + "\r\n", 
                fileName4+XwDataUtils.currentMonthTime() +"\\1495614534.txt", fMo);
            dFileUtils.addErrorMsgInFile(errorMsg, fileName4 + XwDataUtils.currentMonthTime() + "\\1495614534.txt", fMo);
        }

        public void setErrorData(string errorMsg)
        {
            DataFileUtils dFileUtils = new DataFileUtils();
            FileMode fMo = FileMode.Append;
            if (dFileUtils.FileSize(fileName4 + errorfileName) > 1024 * 1024 * 10) fMo = FileMode.Create;
            dFileUtils.addErrorMsgInFile("\r\n" + XwDataUtils.currentTimeToSe() + "\r\n",
               fileName4 + errorfileName, fMo);
            dFileUtils.addErrorMsgInFile(errorMsg + "\r\n", fileName4 + errorfileName, fMo);
        }

        /// <summary>
        /// 修改chFlBean的值
        /// </summary>
        /// <param name="cacheFlBean"></param>
        public void changeCacheData(CacheFileBean cacheFlBean)
        {
            if (cacheFlBean == null) return;
            if (chFlBean == null || cacheFlBean != null) ///豁出去了，再牛B的逻辑，扛不住主机随机的修改，抗不住客户任意的蹂躏
            {

                string locaIp = FileModel.getFlModel().ChFlBean.ServerIP_TCP;
                if (locaIp == null) locaIp = XWUtils.GetAddressIP();
                int poet = FileModel.getFlModel().ChFlBean.ServerPort_TCP;
                poet = poet == 0 ? 51234 : poet;

                chFlBean = cacheFlBean;

                FileModel.getFlModel().ChFlBean.ServerIP_TCP = locaIp;
                FileModel.getFlModel().ChFlBean.ServerPort_TCP = poet;
                return;
            } 

            chFlBean.LocaIP = chFlBean.LocaIP;

            if (chFlBean.LocaPort != cacheFlBean.LocaPort) chFlBean.LocaPort = cacheFlBean.LocaPort;
            if (chFlBean.LocaIP_TCP != cacheFlBean.LocaIP_TCP) chFlBean.LocaIP_TCP = cacheFlBean.LocaIP_TCP;
            if (chFlBean.LocaPort_TCP != cacheFlBean.LocaPort_TCP) chFlBean.LocaPort_TCP = cacheFlBean.LocaPort_TCP;
            if (chFlBean.ServerIP_TCP != cacheFlBean.ServerIP_TCP) chFlBean.ServerIP_TCP = cacheFlBean.ServerIP_TCP;
            if (chFlBean.ShowCanKaoDian != cacheFlBean.ShowCanKaoDian) chFlBean.ShowCanKaoDian = cacheFlBean.ShowCanKaoDian;
            if (chFlBean.ShowJingJiTag != cacheFlBean.ShowJingJiTag) chFlBean.ShowJingJiTag = cacheFlBean.ShowJingJiTag;
            if (chFlBean.ShowBlackTag != cacheFlBean.ShowBlackTag) chFlBean.ShowBlackTag = cacheFlBean.ShowBlackTag;
            if (chFlBean.BlackTime != cacheFlBean.BlackTime) chFlBean.BlackTime = cacheFlBean.BlackTime;
            if (chFlBean.NoReveTime != cacheFlBean.NoReveTime) chFlBean.NoReveTime = cacheFlBean.NoReveTime;
            if (chFlBean.BlackTimeText != cacheFlBean.BlackTimeText) chFlBean.BlackTimeText = cacheFlBean.BlackTimeText;
            if (chFlBean.CheckCText != cacheFlBean.CheckCText) chFlBean.CheckCText = cacheFlBean.CheckCText;        

            List<PeopleBean> cachePeoples = cacheFlBean.Peoples;
            foreach (var peopleItem in cachePeoples) //List<PeopleBean>同步
            {
                var valuePeoples = chFlBean.Peoples.Where(a => a.Id.Equals(peopleItem.Id));
                if (valuePeoples.Count() == 0) 
                {
                    chFlBean.Peoples.Add(peopleItem);
                    continue;
                }
                foreach (var valuePeopleItem in valuePeoples)
                {
                    changePeopleBean(peopleItem, valuePeopleItem);
                }
            }

            List<CardBean> cards = cacheFlBean.Cards;
            foreach (var cardItem in cards) //List<CardBean>同步
            {
                var valueCards = chFlBean.Cards.Where(a => a.Id.Equals(cardItem.Id));
                if (valueCards.Count() == 0)
                {
                    chFlBean.Cards.Add(cardItem);
                    continue;
                }
                foreach (var valueCardItem in valueCards)
                {
                    changeCard(cardItem, valueCardItem);
                }
            }

            List<CanKaoDianBean> canKaoDianBean = cacheFlBean.CanKaoDians;
            foreach (var canKaoDianBeanItem in canKaoDianBean) //List<CardBean>同步
            {
                var valueCanKaoDianBeans = chFlBean.CanKaoDians.Where(a => a.Id.Equals(canKaoDianBeanItem.Id));
                if (valueCanKaoDianBeans.Count() == 0)
                {
                    chFlBean.CanKaoDians.Add(canKaoDianBeanItem);
                    continue;
                }
                foreach (var valuecankaoItem in valueCanKaoDianBeans)
                {
                    cchangeCankaodian(valuecankaoItem, canKaoDianBeanItem);
                }
            }

            Dictionary<string, CardBean> cardDicS = cacheFlBean.CardDic;
            foreach (var cardDicSItem in cardDicS) //List<CardBean>同步
            {
                if (chFlBean.CardDic.ContainsKey(cardDicSItem.Key))
                {
                    changeCard(cardDicSItem.Value, chFlBean.CardDic[cardDicSItem.Key]);
                }
                else 
                {
                    chFlBean.CardDic.Add(cardDicSItem.Key, cardDicSItem.Value);
                }
            }

        //private List<NODEBean> nodes;  //当前力丽不需要这个数据，只是干放着，没有任何数据
        //private Dictionary<string, CardBean> cardDic;//Dictionary        
         //   if ()

        }

        private void cchangeCankaodian(CanKaoDianBean sour, CanKaoDianBean des) 
        {
            if (!sour.Id.Equals(des.Id)) return;
            if (des.Name != sour.Name) des.Name = sour.Name;
            if (des.QuYuname != sour.QuYuname) des.QuYuname = sour.QuYuname;
            if (des.CenJiname != sour.CenJiname) des.CenJiname = sour.CenJiname;
            if (des.QuYuID != sour.QuYuID) des.QuYuID = sour.QuYuID;
            if (des.CenJiID != sour.CenJiID) des.CenJiID = sour.CenJiID;
            if (des.POint != sour.POint) des.POint = sour.POint;            
            if (XWUtils.byteBTBettow(des.CanDianID,sour.CanDianID))Array.Copy(sour.CanDianID,0,des.CanDianID,0,sour.CanDianID.Length);
            if (des.PeopleCount != sour.PeopleCount) des.PeopleCount = sour.PeopleCount;
            if (XWUtils.byteBTBettowInt(des.ColWeiHei,sour.ColWeiHei))Array.Copy(sour.ColWeiHei,0,des.ColWeiHei,0,sour.ColWeiHei.Length);
            if (des.TimeReceive != sour.TimeReceive) des.TimeReceive = sour.TimeReceive;  
        }

        private void changeCard(CardBean sour, CardBean des) 
        {
            if (!sour.Id.Equals(des.Id)) return;
            if (des.Name != sour.Name) des.Name = sour.Name;
            if (des.FirstReceiveTime != sour.FirstReceiveTime) des.FirstReceiveTime = sour.FirstReceiveTime;
        }

        private void changePeopleBean(PeopleBean sour, PeopleBean des) 
        {
            if (!sour.Id.Equals(des.Id)) return;
            if (des.Name != sour.Name) des.Name = sour.Name;
            if (des.PassWord != sour.PassWord) des.PassWord = sour.PassWord;
            if (des.Jurisdiction != sour.Jurisdiction) des.Jurisdiction = sour.Jurisdiction;
            if (des.PowerValue != sour.PowerValue) des.PowerValue = sour.PowerValue;
         }

        /// <summary>
        /// 修改层级数据
        /// </summary>
        /// <param name="cenJiData"></param>
        public void changeData(List<CenJiBean> cenJiDatas)
        {
            if (cenJiDatas == null) return;
            List<CenJiBean> cacheCenJiData = cenJiDatas.ToList();
            if (cenJiData == null || cacheCenJiData.Count > 0) 
            {
                cenJiData = cacheCenJiData;
                return;
            }
            foreach (var item in cacheCenJiData)
            {
                var values = cenJiData.Where(a => a.ID.Equals(item.ID));
                if (values.Count() == 0)  //没有就加上
                {
                    cenJiData.Add(item);
                    continue;
                }
                foreach (var valueItem in values)//有就修改一下
                {
                    changeData(item, valueItem);
                }
            }
        }

        public void changeData(CenJiBean sour, CenJiBean des) 
        {
            if (!des.ID.Equals(sour.ID)) return;
            if (!des.CenJiName.Equals(sour.CenJiName)) des.CenJiName = sour.CenJiName;
            List<QuYuBean> sourQuYuBeans = sour.QuYuBeans;
            if (sourQuYuBeans == null) return;
            if (des.QuYuBeans == null) des.QuYuBeans = sourQuYuBeans;
            foreach (var sourItem in sourQuYuBeans)
            {
                var values = des.QuYuBeans.Where(a => a.QuyuID.Equals(sourItem.QuyuID));
                if (values.Count() == 0)  //没有就加上
                {
                    if (des.QuYuBeans == null) des.QuYuBeans = new List<QuYuBean>();
                    des.QuYuBeans.Add(sourItem);
                    continue;
                }
                foreach (var valueItem in values)//有就修改一下
                {
                    changeQuYuBeans(sourItem, valueItem);
                }
            }
        }

        private void changeQuYuBeans(QuYuBean sour, QuYuBean des) 
        {
            if (!sour.QuyuID.Equals(des.QuyuID)) return;
            des.QuyuName = sour.QuyuName;
            des.MapID = sour.MapID;
           // des.MapPath = sour.MapPath;
            des.PepleCount = sour.PepleCount;
            des.Begin_color = sour.Begin_color;
            des.End_color = sour.End_color;
        }


        public RestartBean getRestartData()
        {
            DataFileUtils dFileUtils = new DataFileUtils();
            Object obj = null;
            dFileUtils.Deserialize(fileName5, ref obj);
            if (null == obj) return null;
            RestartBean resBean = obj as RestartBean;
            return resBean;
        }

        public void setRestartData(RestartBean resBean)
        {
            DataFileUtils dFileUtils = new DataFileUtils();
            dFileUtils.serializeRestart(resBean, fileName5);           
        }

        public void deleteRestartData()
        {
            DataFileUtils dFileUtils = new DataFileUtils();
            dFileUtils.deleRestart( fileName5);
        }


        private static FileModel fileModel;
        private List<CenJiBean> cenJiData;
        private const string fileName = "data.dat";
        private const string fileName2 = "cacheData.dat";
        private const string fileName3 = "history_File_List.dat";
        private const string fileName4 = "HistoryFile\\";
        private const string fileName5 = "HistoryFile\\RestartBean.dat";
        private const string errorfileName = "errorData.txt";//报警的信息
        private static object obje = new object();
        private CacheFileBean chFlBean;
        private List<HistoryFileDataBean> hisFileDBeans;
        private List<CardUpDataBean> oldUpCardDatas;
        private string fileNameLock = "dataLock";        
        private string fileName2Lock = "cacheDataLock";       


        /// <summary>
        /// 最新数据的委托
        /// </summary>
        /// <param name="type"></param>
        public delegate void NewDataHandle(int type);
        public NewDataHandle getdata;
        public const int COUNTADD = 1;
        public const int PEOPLEADD = 2;

    }
}
