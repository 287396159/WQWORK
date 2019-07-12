using CiXinLocation.bean;
using MoveableListLib;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CiXinLocation.Model
{
    public class LocationViewModel:FromBaseModel
    {

        public delegate void MoveViewHandle(Control col, Point pot);
        public delegate void AddOrRemoveViewHandle(Control col, int viewType);
        public MoveViewHandle moveHandle;
        public AddOrRemoveViewHandle adReView;
        public delegate void CardCountHandle(Dictionary<string, List<CardUpDataBean>> NodeCardUpBean);
        public CardCountHandle cardCountHandle;
        private List<CanKaoDianBean> ckds; ///本区域的参考点ID
        private List<TagLocationPointBean> tagInfor;
        private List<TagLocationPointBean> tagInforCache;
        private Dictionary<string, TagLocationPointBean> viewTagInfor;
        private Panel panel1;
        private ListView listView1;
        private ListView listView2;
        private List<CardUpDataBean> cardBean;
        private Label LAB_COUNT;
        private List<CardUpDataBean> allCard = null;
        private int tyByteIndex = 1;
        private string serchCardID = ""; //搜尋的卡片ID       
        private Dictionary<string, CanKaoDianBean> mainCanDictionarys;
        private List<CardUpDataBean> cardHisBean;
        private Dictionary<string, CanKaoDianUpInfo> mainCanInfos;
        private Dictionary<string, CanKaoDianUpInfo> listViewCanInfos;
        private Dictionary<string, List<CardUpDataBean>> nodeCardUpBean;        
        private QuYuBean quYuBean;
        private string cengjiID;
        private List<SingleTagBean> sTagBeans;
        private LocationViewFrom viewFrom;
        int nodeTimeOver = 300;
        private UInt32 runTime = 0;
        private bool modelClose = false;


        public LocationViewModel(LocationViewFrom viewFrom,Panel panel1, ListView lsitView1, ListView listView2, Label LAB_COUNT) 
        {
            ckds = new List<CanKaoDianBean>();
            this.viewFrom = viewFrom;
            this.panel1 = panel1;
            this.listView1 = lsitView1;
            this.listView2 = listView2;
            this.LAB_COUNT = LAB_COUNT;

            tagInfor = new List<TagLocationPointBean>();
            tagInforCache = new List<TagLocationPointBean>();
            viewTagInfor = new Dictionary<string, TagLocationPointBean>();
            nodeCardUpBean = new Dictionary<string, List<CardUpDataBean>>();
            cardHisBean = new List<CardUpDataBean>();
            cardBean = new List<CardUpDataBean>();
            sTagBeans = new List<SingleTagBean>();
            if (listViewCanInfos == null) listViewCanInfos = new Dictionary<string, CanKaoDianUpInfo>();

            if (PeoplePowerModel.getPeoplePowerModel().Jurisdiction == PeoplePowerModel.getPeoplePowerModel().CongjiValue)
            {
                nodeTimeOver = 600;
            }
        }        

        private void addOrMoveTag(Control col, int viewType)
        {
            if (moveHandle != null) adReView(col, viewType);
        }

        private void moveViewHandle(Control col, Point pot) 
        {
            if (moveHandle != null) moveHandle(col, pot);
        }


        public string SerchCardID
        {
            get { return serchCardID; }
            set 
            { 
                serchCardID = value; 
                CengjiIDChange();
            }
        }


        public string CengjiID
        {
            get { return cengjiID; }
            set { cengjiID = value; }
        }


        public QuYuBean QUYUBean
        {
            get { return quYuBean; }
            set { quYuBean = value; }
        }

        public List<CanKaoDianBean> Ckds
        {
            get { return ckds; }
            set { ckds = value; }
        }

        public Dictionary<string, List<CardUpDataBean>> NodeCardUpBean
        {
            get { return nodeCardUpBean; }
            set { nodeCardUpBean = value; }
        }

        public List<CardUpDataBean> CardBean
        {
            get { return cardBean; }
            set { cardBean = value; }
        }

        public void CengjiIDChange() 
        {
            if (serchCardID.Length > 0) 
            {
                if (tyByteIndex > 0)
                    listView1.Items.Clear();                 
            }
        }

        public bool setSerchCARDID(string cardID)
        {
            var cardBns = cardBean.Where(a => a.TagIdStr.Equals(cardID));
            if (cardBns.Count() > 0)
            {
                SerchCardID = cardID.ToUpper();
                return true;
            }
            else 
            {
                SerchCardID = "";
            }
            return false;
        }


        public void setListViewCanInfos(List<CanKaoDianBean> ckdBean) 
        {
            if (ckdBean == null || ckdBean.Count == 0) return;
            if (listViewCanInfos == null) listViewCanInfos = new Dictionary<string, CanKaoDianUpInfo>();
            foreach (var item in ckdBean)
            {
                if (!listViewCanInfos.ContainsKey(item.Id))
                {
                    CanKaoDianUpInfo cInro = new CanKaoDianUpInfo();
                    cInro.CkdId = item.Id;
                    Array.Copy(item.CanDianID, 0, cInro.CID,0,2);
                    cInro.Name = item.Name;
                    cInro.UpTime = XwDataUtils.GetTimeStamp();
                    listViewCanInfos.Add(item.Id, cInro);                   
                }                   
            }          
            setMainCanDictionarys(ckdBean.ToList());
        }

        public void setMainCanDictionarys(List<CanKaoDianBean> ckdBean) 
        {
            if (ckdBean == null || ckdBean.Count == 0) return;
            if (mainCanDictionarys == null) mainCanDictionarys = new Dictionary<string, CanKaoDianBean>();
            foreach (var item in ckdBean)
            {
                if (!mainCanDictionarys.ContainsKey(item.Id))
                {
                    item.TimeReceive = XwDataUtils.GetTimeStamp();
                    mainCanDictionarys.Add(item.Id,item);
                }
            }
        }

        public void loadNewData() 
        {
            if (tagInfor != null) tagInfor.Clear();
            if (cardBean != null) cardBean.Clear();
            if (listView1 != null) listView1.Items.Clear();
        }

        /// <summary>
        /// 存储参考点与卡片的关系，通过相关判断，改变位置关系
        /// </summary>
        /// <param name="ckdItem"></param>
        /// <param name="ID"></param>
        private void addTagBean(CanKaoDianBean ckdItem,byte[] ID,int index)
        {
            addTagBean(ckdItem.CanDianID, ID, index);
        }

        /// <summary>
        /// 存储参考点与卡片的关系，通过相关判断，改变位置关系
        /// </summary>
        /// <param name="ckdItem"></param>
        /// <param name="ID"></param>
        private void addTagBean(byte[] canID, byte[] ID,int index)
        {
            string IDStr = ID[0].ToString("X2") + ID[1].ToString("X2");
            try
            {
                var singTag = sTagBeans.Where(a => { return a.tagHave(ID); }).First(); //LinQ查找符合条件的元素
               
                if (!singTag.CaedRemoveCount.ContainsKey(IDStr))
                { //谨慎操作，防止异常
                    int[] singInt = { 0, index };
                    singTag.CaedRemoveCount.Add(IDStr, singInt);
                }                                                    
                if (XWUtils.byteBTBettow(canID, singTag.CanID))
                {
                    singTag.CaedRemoveCount[IDStr][0] = 0;
                    singTag.CaedRemoveCount[IDStr][1] = index;
                }
                else
                {
                    if (index != singTag.CaedRemoveCount[IDStr][1]) 
                    {
                        singTag.CaedRemoveCount[IDStr][0]++;
                        singTag.CaedRemoveCount[IDStr][1] = index;
                    }
                    if (singTag.CaedRemoveCount[IDStr][0] >= FileModel.getFlModel().ChFlBean.CheckC) //超过参考点的检查次数
                    {
                        singTag.deleCard(ID);
                        addTagInSingle(canID, ID, index);
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                addTagInSingle(canID, ID, index);
            }
            catch (Exception e) { }
        }

        private void addTagInSingle(byte[] canID, byte[] ID,int dengx)
        {
            if (canID == null) return;
            var singTags = sTagBeans.Where(a => XWUtils.byteBTBettow(canID, a.CanID));
            if (singTags.Count() > 0) 
            {
                try {
                    SingleTagBean singTag = singTags.First();
                    singTag.addCard(ID, dengx);
                }
                catch { }
                return;
            } 
            SingleTagBean sBean = new SingleTagBean();
            sBean.CanID = canID;
            sBean.addCard(ID, dengx);
            sTagBeans.Add(sBean);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID">卡片ID</param>
        /// <returns></returns>
        private byte[] getCanKaoDianIDInSingleTag(byte[] ID) 
        {
            byte[] port1ID = null;
            try
            {
                string IDStr = ID[0].ToString("X2") + ID[1].ToString("X2");
                var singTag = sTagBeans.Where(a => { return a.tagHave(ID); }).ToList().First(); //LinQ查找符合条件的元素
                port1ID = singTag.CanID;
            } 
            catch (Exception e)
            {
                Debug.WriteLine("LocationViewModel.getCanKaoDianIDInSingleTag"+e.Message);
            }
            return port1ID;
        }

        private Object objHis = new object();
        public void onHisData(CardUpDataBean showUpCardData, int type)
        {
            lock (objHis)
            {
                cardHisBean.Add(showUpCardData);
            }             
        }      

        /// <summary>
        /// 接收主数据分析好参考点的数据，拿来就用
        /// </summary>
        /// <param name="showUpCardDatas">处理好的类数据</param>
        public void distributionCKDData(Dictionary<string, CanKaoDianBean> canDictionarys) 
        {
            if (canDictionarys == null) return;
            if (mainCanDictionarys == null) 
            {
                mainCanDictionarys = new Dictionary<string, CanKaoDianBean>(canDictionarys);
                return;
            }
            foreach (var mainCanitem in mainCanDictionarys)
            {
                if (canDictionarys.ContainsKey(mainCanitem.Key))
                    mainCanitem.Value.TimeReceive = canDictionarys[mainCanitem.Key].TimeReceive;
            }
        }

        UInt32 hisTime = 0;
        private UInt32 getCurrenHisTime()
        {
            if (IsDealHistory)
                return hisTime;
            else return XwDataUtils.GetTimeStamp();
        }

        /// <summary>
        /// 此时是历史轨迹功能使用时刻
        /// false不是，true是
        /// </summary>
        public bool IsDealHistory
        {
            get { return isDealHistory; }
            set { isDealHistory = value; }
        }

        /// <summary>
        /// 此时是历史轨迹功能使用时刻
        /// false不是，true是
        /// </summary>
        private bool isDealHistory = false;

        /// <summary>
        /// 接收主数据分析好的数据，拿来就用，通过委托转过来的数据
        /// </summary>
        /// <param name="showUpCardDatas">处理好的类数据</param>
        public void distributionData(List<CardUpDataBean> showUpCardDatas)
        {
            if (showUpCardDatas == null || modelClose) return;
            if (tyByteIndex == 3)
            {
                allPageCount = showUpCardDatas.Count;
                drawListCache(showUpCardDatas);
                cardBean.Clear();
                cardBean.AddRange(showUpCardDatas);
            }
            else 
            {
                distributionDataView(showUpCardDatas);
                distributionPaintView(showUpCardDatas);
            }                
            if (allCard == null) return;
            var cards = showUpCardDatas.Where(a => a.TagIdStr.Equals(serchCardID)); //添加搜索的条目
            if (cards.Count() > 0)
            {
                allCard.Insert(0, cards.First());
            }
        }         

        public void distributionPaintView(List<CardUpDataBean> showUpCardDatas)
        {           
            tagInforCache.Clear();                  
            CardUpDataBean cardSerchBean = null;
            if (serchCardID != null && !serchCardID.Equals(""))
            {
                int index = getCardBeanIndex(showUpCardDatas, XWUtils.hexStrToInt1(serchCardID));
                if (index >= 0) 
                {
                    cardSerchBean = showUpCardDatas[index];
                    if (cardSerchBean.Port1IDHistory[0] == 0 && cardSerchBean.Port1IDHistory[1] == 0)
                        Array.Copy(cardSerchBean.Port1IDHistory, 0, cardSerchBean.Port1IDHistory, 0, 2);
                }
            }
            foreach (CanKaoDianBean ckdItem in ckds)
            {
                int canCOunt = 0;
                if (cardSerchBean != null && XWUtils.byteBTBettow(ckdItem.CanDianID, cardSerchBean.Port1IDHistory, ckdItem.CanDianID.Length))
                {
                    if (addTagInfo(cardSerchBean, ckdItem, canCOunt)) //添加数据
                        canCOunt++;
                }
                if (nodeCardUpBean == null) continue;
                if (nodeCardUpBean.ContainsKey(ckdItem.Id)) 
                {
                    if (nodeCardUpBean[ckdItem.Id] == null) continue;
                    Dictionary<string, List<CardUpDataBean>> NodeCardUpCacheBean = new Dictionary<string, List<CardUpDataBean>>(nodeCardUpBean);
                    foreach (CardUpDataBean cBean in NodeCardUpCacheBean[ckdItem.Id])
                    {
                        if (cBean.FirstReceiveTime + FileModel.getFlModel().ChFlBean.NoReveTime < getCurrenRunTime()) continue;
                        if (addTagInfo(cBean, ckdItem, canCOunt)) //添加数据
                            canCOunt++;
                        if (canCOunt >= 3) break;
                    }    
                }                
            }
            tagInfor.Clear();
            tagInfor.AddRange(tagInforCache.ToList()); 
        }

        private int getCardBeanIndex(List<CardUpDataBean> dealUpCardDatas, int tagIDIndex)
        {
            if (dealUpCardDatas == null || dealUpCardDatas.Count == 0 || tagIDIndex == -1) return -1;

            int low = 0;
            int hei = dealUpCardDatas.Count;
            while (hei > low)
            {
                int mid = (low + hei) / 2;
                if (tagIDIndex == dealUpCardDatas[mid].TagID_Int) return mid;
                else if (tagIDIndex > dealUpCardDatas[mid].TagID_Int)
                {
                    if (low != mid)
                        low = mid;
                    else low++;
                }
                else
                {
                    if (hei != mid)
                        hei = mid;
                    else hei--;
                }
            }
            return -2;
        }

        private bool addTagInfo(CardUpDataBean cBean, CanKaoDianBean ckdItem,int canCOunt) 
        {
            Point[] locaP = new Point[3];
            int[] colWeiHei = new int[2];

            byte[] port1ID = null;// getCanKaoDianIDInSingleTag(cBean.TagId);
            if (port1ID == null) port1ID = cBean.Port1IDHistory;
            if (!XWUtils.byteBTBettow(port1ID, ckdItem.CanDianID, ckdItem.CanDianID.Length)) return false;

            if (locaP[1].X == 0 && locaP[1].Y == 0) { } //这个判断是否单点，两点定位，就没有这个必要了
            colWeiHei = ckdItem.ColWeiHei;            
            locaP[0] = ckdItem.POint;
            byte[] ressByte = new byte[3] { cBean.Port1Rssi, cBean.Port2Rssi, cBean.Port3Rssi };
            tagDeal(locaP, ressByte, cBean, colWeiHei, canCOunt);
            return true;
        }

        //界面裡面的數據解析，不是所有的數據
        public void distributionDataView(List<CardUpDataBean> showUpCardDatas)
        {           
            if (cengjiID == null || quYuBean == null) return;
            Dictionary<string, List<CardUpDataBean>> cacheNodeCardUpBean = new Dictionary<string, List<CardUpDataBean>>();
            foreach (CanKaoDianBean ckdItem in ckds)
            {
                List<CardUpDataBean> cardBeans = new List<CardUpDataBean>();
                cacheNodeCardUpBean.Add(ckdItem.Id, cardBeans);
            }
            List<CardUpDataBean> cacheBean = new List<CardUpDataBean>();           
            foreach (CardUpDataBean cBean in showUpCardDatas)
            {
                if (cacheNodeCardUpBean.ContainsKey(cBean.Port1IDHistoryStr)) 
                {
                    if (cacheNodeCardUpBean[cBean.Port1IDHistoryStr] == null)
                        cacheNodeCardUpBean[cBean.Port1IDHistoryStr] = new List<CardUpDataBean>();
                    cacheNodeCardUpBean[cBean.Port1IDHistoryStr].Add(cBean);
                    cacheBean.Add(cBean);
                }
            }
            try 
            {
                allPageCount = cacheBean.Count;
                drawListCache(cacheBean.ToList());
                cardBean.Clear();
                cardBean.AddRange(cacheBean);
                nodeCardUpBean.Clear();
                foreach (var item in cacheNodeCardUpBean)
                {
                    nodeCardUpBean.Add(item.Key,item.Value);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("LocationViewModel.distributionDataView="+e.Message);
            }           
        }

        /// <summary>
        /// Tag處理，為TAG賦值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="locaP"></param>
        /// <param name="ressByte"></param>
        /// <param name="cBean"></param>
        /// <param name="colWeiHei"></param>
        private void tagDeal(Point[] locaP, byte[] ressByte, CardUpDataBean cBean, int[] colWeiHei, int canCOunt) 
        {
            Point tagLocaP = new Point();
            LocationType lType = LocationType.NOTHING;

            if (locaP[1].X == 0 && locaP[1].Y == 0 ) //单点定位
            {
                tagLocaP = new Point(locaP[0].X ,locaP[0].Y);
                lType = LocationType.SINGLE;
            }
            else if (locaP[2].X == 0 && locaP[2].Y == 0) //两点定位
            {
                tagLocaP.Y = (locaP[0].Y + locaP[1].Y) * ressByte[0] / (ressByte[0] + ressByte[1]);
                tagLocaP.X = (locaP[0].X + locaP[1].X) * ressByte[0] / (ressByte[0] + ressByte[1]);
                lType = LocationType.DOUBLE;
            }
            else 
            {
                tagLocaP.Y = ((locaP[0].Y + locaP[1].Y) * ressByte[0] / (ressByte[0] + ressByte[1])+
                    (locaP[0].Y + locaP[2].Y) * ressByte[0] / (ressByte[0] + ressByte[2]))/2;

                tagLocaP.X = ((locaP[0].X + locaP[2].X) * ressByte[0] / (ressByte[0] + ressByte[2])+
                    (locaP[0].X + locaP[1].X) * ressByte[0] / (ressByte[0] + ressByte[1]))/2;
                lType = LocationType.TREE;
            }
             
            bool haveTag = false;
            foreach (TagLocationPointBean tpBean in tagInforCache)
            {                
                if (!XWUtils.byteBTBettow(cBean.TagId, tpBean.CardID)) continue;
                haveTag = true;
                tpBean.Point = tagLocaP;
                tpBean.LcType = lType;
                tpBean.ShowTime = cBean.FirstReceiveTime;
                tpBean.MType = cBean.MType;
                tpBean.CountIndex = canCOunt;
                tpBean.SleepTime = cBean.SleepTime;
                tpBean.Gonglv = cBean.SendGongLv;
                if (FileModel.getFlModel().ChFlBean.ShowJingJiTag && cBean.MType == 2)
                    tpBean.MColor = 3;
                else if (cBean.SensorTime >= FileModel.getFlModel().ChFlBean.BlackTime)
                    tpBean.MColor = 1;
                else tpBean.MColor = 2;
            }
            if (!haveTag && getCurrenRunTime() - cBean.FirstReceiveTime < FileModel.getFlModel().ChFlBean.NoReveTime) 
            {
                TagLocationPointBean tpBean = getTagLocationPoint(cBean);
                tpBean.Point = tagLocaP;
                tpBean.LcType = lType;
                tpBean.CountIndex = canCOunt;
                if (colWeiHei != null) tpBean.ColWeiHei = colWeiHei;
                tagInforCache.Add(tpBean);
            }
        }

        private TagLocationPointBean getTagLocationPoint(CardUpDataBean cBean) 
        {
            TagLocationPointBean tpBean = new TagLocationPointBean();
            tpBean.Name = arrayTagName(cBean.TagId);//cBean.TagId[0].ToString("X2") + cBean.TagId[1].ToString("X2");
            
            tpBean.ShowTime = cBean.FirstReceiveTime;
            tpBean.MType = cBean.MType;
            tpBean.CardID = cBean.TagId;
            tpBean.SleepTime = cBean.SleepTime;
            tpBean.Gonglv = cBean.SendGongLv;

            if (FileModel.getFlModel().ChFlBean.ShowJingJiTag && cBean.MType == 2)
                tpBean.MColor = 3;
            else if (cBean.SensorTime >= FileModel.getFlModel().ChFlBean.BlackTime)
                tpBean.MColor = 1;
            else tpBean.MColor = 2;
            return tpBean;
        }


        /// <summary>
        /// 加载数据  ,LocationViewFrom locaForm
        /// </summary>
        /// <param name="dataType">1 = 列表模式 ， <0 图像模式</param>
        public void draw(int dataType) 
        {
            tyByteIndex = dataType;
            if(IsDealHistory)
            {
            //    List<CardUpDataBean> cardHisBeanCache = cardHisBean.ToList();
            //    cardHisBean.Clear();
            //    drawHisList(cardHisBeanCache);
            }
            if (!IsDealHistory && (dataType == 1 || dataType == 3) && allCard != null)
            {                
                var allCards = allCard.Where(a => a.FirstReceiveTime + FileModel.getFlModel().ChFlBean.NoReveTime > getCurrenRunTime()) .ToList();
                drawList(allCards);                                             
            }else if (dataType < 0) 
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(drawImage));    //参数可选               
            }
            if (IsDealHistory)
            {
                return;
            }
            ThreadPool.QueueUserWorkItem(new WaitCallback(changPortColor));    //参数可选

            Dictionary<string, List<CardUpDataBean>> cacheNodeCardUpBean = new Dictionary<string, List<CardUpDataBean>>(nodeCardUpBean);
            if (cardCountHandle != null) cardCountHandle(cacheNodeCardUpBean);
        }

        /// <summary>
        /// type = 1,表示只有ID和名称。type代表上报的信息
        /// </summary>
        /// <param name="canDianInfo"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private ListViewItem getListViewItemIn2(CanKaoDianUpInfo canDianInfo,int type)
        {       
            string version = "";
            string Drivatetype = "";
            string time = "";
            if (type == 2 && canDianInfo != null && canDianInfo.Version[0] != 0 && canDianInfo.Version[1] != 0) 
            {
                version = Loca_NodeUtils.getWuPinLocaVersion(canDianInfo.Version);
                time = XwDataUtils.currentTimeToSe(canDianInfo.UpTime);
                Drivatetype = Loca_NodeUtils.getDrivaceNODEType(canDianInfo.NodeType);
            }
            ListViewItem lItem = new ListViewItem();
            lItem.SubItems[0].Text = canDianInfo.CkdId;
            lItem.SubItems.Add(canDianInfo.Name);
            lItem.SubItems.Add(Drivatetype);
            lItem.SubItems.Add(version);
            lItem.SubItems.Add("在線");
            lItem.SubItems.Add(time);
            return lItem;
        }

        private int page = 1; // 显示当前的页数
        private int pageCount = 30; // 每页显示数量
        private int allPageCount = 0;//總頁數的總數量
        private bool isLoadData = false;
        public void changePageInList(ListPageType lisPage,int pageCount) 
        {
            isLoadData = true;
            this.pageCount = pageCount;
            if (lisPage == ListPageType.FIRST) page = 1;
            else if (lisPage == ListPageType.DOWN) page++;
            else if (lisPage == ListPageType.UP)
            {
                if (page > 1) page--;
                else if (page == -1001) page = allPageCount - 1;
            }
            else if (lisPage == ListPageType.LAST) 
            {
                page = -1001;
            }
            pageChage(allPageCount);
            listView1.Items.Clear();
            isLoadData = false;
        }

        public void setPage(int setPages) 
        {
            string msg = "";
            if (setPages > getAllPage())
                msg = "超過總頁數";
            else if (setPages < 1) msg = "頁數不能小於1";
            if (msg.Length > 0) 
            {
                MessageBox.Show(msg);
                return;
            }
            page = setPages;
            listView1.Items.Clear();
        }

        public int getPage()
        {
            if (page == -1001) return getAllPage();
            return page;
        }

        public int getAllPage() 
        {
            return (allPageCount/pageCount) + 1;
        }

        public void setTimeDate(UInt32 time)
        {
            hisTime = time;
        }

        public void setPortInfo()
        {
            if (listViewCanInfos == null) return;
            foreach (var item in listViewCanInfos)
            {
                ListViewItem lItem = getListViewItemIn2(item.Value,1);
                if (lItem != null) listView2.Items.Add(lItem);
            }
            LAB_COUNT.Text = listViewCanInfos.Count.ToString();
        }


        private void changePortInfo() 
        {
            Dictionary<string, CanKaoDianUpInfo> nCanInfos = new Dictionary<string, CanKaoDianUpInfo>(mainCanInfos);
            viewFrom.Invoke((EventHandler)(delegate //放入主線程
            { 
                for (int i = 0; i < listView2.Items.Count;i++ )
                {
                    ListViewItem lViewItem = listView2.Items[i];
                    if (mainCanInfos.ContainsKey(lViewItem.SubItems[0].Text))
                    {
                        changeView2Item(lViewItem, mainCanInfos[lViewItem.SubItems[0].Text]);
                        nCanInfos.Remove(lViewItem.SubItems[0].Text);
                    }                   
                }
                foreach (var item in nCanInfos)
                {
                    if (listViewCanInfos.ContainsKey(item.Key)) continue;
                    ListViewItem lItem = getListViewItemIn2(item.Value, 2);
                    if (lItem != null) listView2.Items.Add(lItem);
                    listViewCanInfos.Add(item.Key, new CanKaoDianUpInfo(item.Value));
                }
                if (nCanInfos.Count > 0) LAB_COUNT.Text = listViewCanInfos.Count.ToString();
            }));                       
        }

        private void changePortInfo(string nodeID)
        {         
            viewFrom.Invoke((EventHandler)(delegate //放入主線程
            {
                if (!mainCanInfos.ContainsKey(nodeID)) return;

                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    ListViewItem lViewItem = listView2.Items[i];
                    if (nodeID.Equals(lViewItem.SubItems[0].Text))
                    {
                        changeView2Item(lViewItem, mainCanInfos[nodeID]);
                        return;
                    }
                }
                ListViewItem lItem = getListViewItemIn2(mainCanInfos[nodeID], 2);
                if (lItem != null) listView2.Items.Add(lItem);
                if (!listViewCanInfos.ContainsKey(nodeID))
                    listViewCanInfos.Add(nodeID, new CanKaoDianUpInfo(mainCanInfos[nodeID]));

                if (listViewCanInfos.Count > 0) LAB_COUNT.Text = listViewCanInfos.Count.ToString();
            }));          
        }

        private void changePortInfo(string cankaoDianID,string onlineMsg)//专门修改在线和离线的
        {            
            if (listView2 == null) return;
            viewFrom.Invoke((EventHandler)(delegate //放入主線程
            {     
                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    ListViewItem lViewItem = listView2.Items[i];
                    if (!cankaoDianID.Equals(lViewItem.SubItems[0].Text)) continue;
                    if (!lViewItem.SubItems[4].Text.Equals(onlineMsg)) 
                    {
                        lViewItem.SubItems[4].Text = onlineMsg;
                        //if ("離線".Equals(onlineMsg))
                        //    warnPort(cankaoDianID);
                    } 
                    break;
                }
            }));             
        }

        private void warnPort(string cankaoDianID,long uoTime) 
        {
            if (cankaoDianID == null || IsDealHistory) return;
            WarnMessage.getWarnMessage().
                addNODEUnanswerWranMsgs(cankaoDianID, (uint)nodeTimeOver, "", "", uoTime);
        }

        private void changeView2Item(ListViewItem lViewItem,CanKaoDianUpInfo infoCan) 
        {
            if(infoCan == null) return;
            string ckdID = infoCan.CkdId;
            if (listViewCanInfos.ContainsKey(ckdID) && infoCan.Version != null && infoCan.Version[0] != 0 && infoCan.Version[1] != 0)
            {
                if (!lViewItem.SubItems[5].Text.Equals(XwDataUtils.currentTimeToSe(infoCan.UpTime)) && CloseUDPtime <= OpenUDPtime)
                 {
                    listViewCanInfos[ckdID].UpTime = infoCan.UpTime;
                    lViewItem.SubItems[5].Text = XwDataUtils.currentTimeToSe(infoCan.UpTime);
                 }
                if (!lViewItem.SubItems[3].Text.Equals(Loca_NodeUtils.getWuPinLocaVersion(infoCan.Version))) 
                 {
                     Array.Copy(infoCan.Version, 0, listViewCanInfos[ckdID].Version, 0, 2);
                     lViewItem.SubItems[3].Text = Loca_NodeUtils.getWuPinLocaVersion(infoCan.Version);
                 }
                if (!lViewItem.SubItems[2].Text.Equals(Loca_NodeUtils.getDrivaceNODEType(infoCan.NodeType))) 
                 {
                     Array.Copy(infoCan.NodeType, 0, listViewCanInfos[ckdID].NodeType, 0, 2);
                     lViewItem.SubItems[2].Text = Loca_NodeUtils.getDrivaceNODEType(infoCan.NodeType);
                 }                     
            }
            if (infoCan.UpTime + nodeTimeOver < XwDataUtils.GetTimeStamp() && lViewItem.SubItems[4].Text.Equals("在線"))
                lViewItem.SubItems[4].Text = "離線";
            else if (infoCan.UpTime + nodeTimeOver > XwDataUtils.GetTimeStamp() && lViewItem.SubItems[4].Text.Equals("離線")) 
                lViewItem.SubItems[4].Text = "在線";
        }


        public void changeCanKaiDianIDtIME(string nodeID, Dictionary<string, CanKaoDianUpInfo> canKaoDianInfor) //byte[] canKaoID,long newTime 
        {           
            if (canKaoDianInfor == null) return;
            if (mainCanInfos == null)
            {
                mainCanInfos = new Dictionary<string, CanKaoDianUpInfo>(canKaoDianInfor);
                changePortInfo();
                return;
            }
            if (nodeID == null) 
            {
                return;
            }
            if(!canKaoDianInfor.ContainsKey(nodeID)) return;
            if (mainCanInfos.ContainsKey(nodeID))
            {
                if (mainCanInfos[nodeID].UpTime < canKaoDianInfor[nodeID].UpTime)
                    mainCanInfos[nodeID].UpTime = canKaoDianInfor[nodeID].UpTime;
                if (mainCanInfos[nodeID].NodeType != canKaoDianInfor[nodeID].NodeType)
                    mainCanInfos[nodeID].NodeType = canKaoDianInfor[nodeID].NodeType;
                if (mainCanInfos[nodeID].Version != canKaoDianInfor[nodeID].Version)
                    mainCanInfos[nodeID].Version = canKaoDianInfor[nodeID].Version;
            }
            else
                mainCanInfos.Add(nodeID,canKaoDianInfor[nodeID]);
            changePortInfo(nodeID);
        }

        
        private void changPortColorThread()//
        {
            if (listViewCanInfos == null || mainCanDictionarys == null) return;
            Dictionary<string, CanKaoDianUpInfo> mainCanims = new Dictionary<string, CanKaoDianUpInfo>(listViewCanInfos);
            
            foreach (Control col in panel1.Controls)
            {
                if (!(col is CanKaoDianView)) continue;
                CanKaoDianView cdView = (CanKaoDianView)col;
                string id = (string)cdView.Tag;
                long timeCha = 121;
                if (mainCanims.ContainsKey(id))
                    timeCha = XwDataUtils.GetTimeStamp() - mainCanims[id].UpTime;
                else if (mainCanDictionarys.ContainsKey(id))
                    timeCha = XwDataUtils.GetTimeStamp() - mainCanDictionarys[id].TimeReceive;

                if (cdView.btnBackColor() != Color.Blue && timeCha <= nodeTimeOver)
                {
                    cdView.mainBtnBackColor(Color.Blue);
                    changePortInfo(id, "在線");
                }
                else if (cdView.btnBackColor() != Color.Black && (timeCha > nodeTimeOver))
                {
                    cdView.mainBtnBackColor(Color.Black);
                    changePortInfo(id, "離線");
                }
                mainCanims.Remove(id);
            }

            foreach(var item in mainCanims)
            {
                string mag = "在線";
                long timeCha = XwDataUtils.GetTimeStamp() - item.Value.UpTime;
                if (timeCha > nodeTimeOver) mag = "離線";
                changePortInfo(item.Key, mag);
            }
        }

        private void changPortColor(object OBJ)
        {
            try 
            {
                changPortColorThread();
            } 
            catch(Exception e) 
            {
                Debug.WriteLine("LocationViewModel.changPortColor"+e.Message);
            }            
        }

        private void drawListCache( List<CardUpDataBean> allCards) //列表模式加载数据
        {
            try
            {
                if (allCards == null) return;
                int pageD = pageChage(allCards.Count);
                if (pageD == -1001) pageD = getAllPage();
                allCard = allCards.Skip((pageD - 1) * pageCount).Take(pageCount).ToList();                
                //drawList();
            }catch(Exception e)
            {
                Debug.WriteLine("LocationViewModel.drawListCache=" + e.Message);
            }
        }

        private int pageChage(int listCount) 
        {
            if (listCount == 0) return 1;
            if (page == -1001) return page;
            if(page <= 1) return 1;             
            if ((page - 1) * pageCount > listCount)
            {
                page--;
                pageChage(listCount);
            } 
            return page;            
        }

        public void setHis_tagID(bool isLoadAddData, string tagID) 
        {
            this.isLoadAddData = isLoadAddData;
            this.tagID = tagID;
        }

        private bool isLoadAddData = true;
        private string tagID;

        private bool isDealHisData(List<CardUpDataBean> allCard) 
        {
            if(allCard.Count <= 0) return false;
            CardUpDataBean cBean = allCard[0];
            string count = cBean.TotalCount.ToString();
            if (listView1.Items.Count <= 0) 
            {
                listView1.Items.Insert(0, addItems(cBean));
                return false;
            }
            string listCount = listView1.Items[0].SubItems[16].Text;
            if (cBean.TagIdStr.Equals(tagID) && !count.Equals(listCount))
            {
                listView1.Items.Insert(0, addItems(cBean));
                if (listView1.Items.Count > pageCount)
                {
                    for (int i = pageCount; i < listView1.Items.Count; i++)
                    {
                        listView1.Items.RemoveAt(i);
                    }
                }
                return true;
            }
            return false;
        }


        public void setListTopIndex() 
        {
            topIndex = listView1.TopItem.Index;
        }


        public void drawHisList(List<CardUpDataBean> allCard)
        {
            if (allCard == null) return;

            foreach (CardUpDataBean cBean in allCard)
            {
                insertItem(cBean);
            }
        }

        private int topIndex = 0;
        private void drawList(List<CardUpDataBean> allCard) 
        {
            if (allCard == null) return;
            if (!isLoadAddData) 
            {
                isDealHisData(allCard);
                return;     
            }

            if (allCard.Count == 0) 
            {
                listView1.Items.Clear();
                return;
            }

            if (listView1.Items.Count > pageCount || listView1.Items.Count > allCard.Count)
            {
                listView1.Items.Clear();
                Console.Write("\r\n=====");
            }
            if (!"".Equals(serchCardID) && listView1.Items.Count > 0 && !listView1.Items[0].SubItems[0].Text.Equals(serchCardID))
            {             
                listView1.Items.Clear();
                Console.Write("\r\n==+++++" + ("".Equals(serchCardID)));
            }

            foreach (CardUpDataBean cBean in allCard)
            {
                if (isLoadData) return;
                bool isHavetag = false;
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (isLoadData) return;
                    if (!listView1.Items[i].SubItems[1].Text.Equals(cBean.TagIdStr)) continue;
                    changeItem(i, cBean);
                    isHavetag = true;
                    break;
                }
                if (!isHavetag) 
                addItem(cBean);
            }
            if (listView1.Items.Count > 0 && listView1.Items[0].SubItems[1].Text.Equals(serchCardID)) 
            {
                ListViewItem lvItem = listView1.Items[0];
                if (lvItem.SubItems[0].ForeColor == Color.Red) return;
                for (int j = 0; j < lvItem.SubItems.Count; j++)
                {
                    lvItem.SubItems[j].ForeColor = Color.Red;
                }
            }            
        }

        private void insertItem(CardUpDataBean cBean)
        {
            listView1.Items.Insert(0,addItems(cBean));
        }

        private void addItem(CardUpDataBean cBean) 
        {
            listView1.Items.Add(addItems(cBean));
        }

        private ListViewItem addItems(CardUpDataBean cBean)
        {
            ListViewItem lvItem = new ListViewItem();
            lvItem.SubItems[0].Text = arrayTagName(cBean.TagId);
            lvItem.SubItems.Add(cBean.TagIdStr);
            lvItem.SubItems.Add(arrayCnKaoDianName(cBean.Port1IDHistory));
            lvItem.SubItems.Add(cBean.Port1IDHistoryStr);
            lvItem.SubItems.Add(cBean.Port1RssiHistory.ToString());
            lvItem.SubItems.Add(arrayCnKaoDianName(cBean.Port2IDHistory));
            lvItem.SubItems.Add(cBean.Port2IDHistoryStr);
            lvItem.SubItems.Add(cBean.Port2RssiHistory.ToString());
            lvItem.SubItems.Add(arrayCnKaoDianName(cBean.Port3IDHistory));
            lvItem.SubItems.Add(cBean.Port3IDHistoryStr);
            lvItem.SubItems.Add(cBean.Port3RssiHistory.ToString());

            string locaType = "普通定位";
            lvItem.SubItems.Add(locaType);
            lvItem.SubItems.Add(cBean.Battery.ToString());
            lvItem.SubItems.Add(cBean.SensorTime.ToString());
            lvItem.SubItems.Add(XwDataUtils.currentTimeToSe(cardFirRecvTm(cBean.FirstReceiveTime)));
            UInt32 tm = getCurrenRunTime() < cardFirRecvTm(cBean.FirstReceiveTime) ? 0 : getCurrenRunTime() - cardFirRecvTm(cBean.FirstReceiveTime);
            lvItem.SubItems.Add(tm.ToString());
            lvItem.SubItems.Add(cBean.TotalCount.ToString());
            lvItem.SubItems.Add(cBean.LostCount.ToString());
            string str = "TAG";
            if (cBean.DrivaceType == 0x04)
            {
                str = "pTAG";
            }
            else if (cBean.DrivaceType == 0x05)
            {
                str = "eTAG";
            }
            lvItem.SubItems.Add(str);
            lvItem.SubItems.Add(cBean.SleepTime.ToString());
            lvItem.SubItems.Add(cBean.SendGongLv.ToString());
            if (cBean.DrivaceType == 0x04)
            {
                byte LED_Staus = cBean.LEDStaus;
                if (LED_Staus == 0) str = "滅";
                else if (LED_Staus == 1) str = "亮";
                else if (LED_Staus == 2) str = "閃爍";
                else str = "";
            }
            else if (cBean.DrivaceType == 0x05)
            {
                byte LED_StausRED = (byte)(cBean.LEDStaus & 3);	  // 3  = 二进制：00 00 00 11;
                byte LED_StausGreen = (byte)(cBean.LEDStaus & 12);// 12 = 二进制：00 00 11 00;
                byte LED_StausBlue = (byte)(cBean.LEDStaus & 48); // 48 = 二进制：00 11 00 00;

                str = "R_" + getLEDstatus(LED_StausRED) + " G_" + getLEDstatus(LED_StausGreen) + " B_"
                    + getLEDstatus(LED_StausBlue);
            }
            lvItem.SubItems.Add(str);
            return lvItem;
        }

        private UInt32 cardFirRecvTm(UInt32 cardTime)
        {
            if (CloseUDPtime > OpenUDPtime && getCurrenRunTime() >= CloseUDPtime) 
            {
                UInt32 closeTime =  (getCurrenRunTime() - CloseUDPtime)*2;
                cardTime -= closeTime;
            }
            return cardTime;
        }

        //寻找参考点名字
        private string arrayCnKaoDianName(byte[] canKaoDianID) 
        {
            if (canKaoDianID[0] == 0 && canKaoDianID[1] == 0) 
                return canKaoDianID[0].ToString("X2") + canKaoDianID[1].ToString("X2");

            List<CanKaoDianBean> canKaoDians = FileModel.getFlModel().ChFlBean.CanKaoDians;
            var canDs = canKaoDians.Where(a => XWUtils.byteBTBettow(canKaoDianID, a.CanDianID)).ToList();
            if (canDs.Count() > 0) return canDs.First().Name;
            else return canKaoDianID[0].ToString("X2") + canKaoDianID[1].ToString("X2");
        }

        //寻找卡片名字
        private string arrayTagName(byte[] tagID) 
        {
            Dictionary<string, CardBean> cardDic = new Dictionary<string,CardBean>(FileModel.getFlModel().ChFlBean.CardDic);
            if (cardDic != null && cardDic.ContainsKey(tagID[0].ToString("X2") + tagID[1].ToString("X2")))
                return cardDic[tagID[0].ToString("X2") + tagID[1].ToString("X2")].Name;
            else return tagID[0].ToString("X2") + tagID[1].ToString("X2");
        }

        private void changeItem(int item_index, CardUpDataBean cBean) 
        {
            if (item_index >= listView1.Items.Count) return;
            
            listView1.Items[item_index].SubItems[0].Text = arrayTagName(cBean.TagId);
            listView1.Items[item_index].SubItems[2].Text = arrayCnKaoDianName(cBean.Port1IDHistory);
            listView1.Items[item_index].SubItems[3].Text = cBean.Port1IDHistoryStr;
            listView1.Items[item_index].SubItems[4].Text = cBean.Port1Rssi.ToString();
            listView1.Items[item_index].SubItems[5].Text = arrayCnKaoDianName(cBean.Port2IDHistory);
            listView1.Items[item_index].SubItems[6].Text = cBean.Port2IDHistoryStr;
            listView1.Items[item_index].SubItems[7].Text = cBean.Port2Rssi.ToString();
            listView1.Items[item_index].SubItems[8].Text = arrayCnKaoDianName(cBean.Port3IDHistory);
            listView1.Items[item_index].SubItems[9].Text = cBean.Port3IDHistoryStr;
            listView1.Items[item_index].SubItems[10].Text = cBean.Port3Rssi.ToString();


            string locaType = "普通定位";
            listView1.Items[item_index].SubItems[11].Text = locaType;
            listView1.Items[item_index].SubItems[12].Text = cBean.Battery.ToString();
            listView1.Items[item_index].SubItems[13].Text = cBean.SensorTime.ToString();
            listView1.Items[item_index].SubItems[14].Text = XwDataUtils.currentTimeToSe(cardFirRecvTm(cBean.FirstReceiveTime));

            UInt32 tm = getCurrenRunTime() < cardFirRecvTm(cBean.FirstReceiveTime) ? 0 : getCurrenRunTime() - cardFirRecvTm(cBean.FirstReceiveTime);
            listView1.Items[item_index].SubItems[15].Text = tm.ToString();
            listView1.Items[item_index].SubItems[16].Text = cBean.TotalCount.ToString();
            listView1.Items[item_index].SubItems[17].Text = cBean.LostCount.ToString();           

            string str = "TAG";
            if (cBean.DrivaceType == 0x04)
            {
                str = "pTAG";
            }
            else if (cBean.DrivaceType == 0x05)
            {
                str = "eTAG";
            }
            listView1.Items[item_index].SubItems[18].Text = str;
            listView1.Items[item_index].SubItems[19].Text = cBean.SleepTime.ToString();
            listView1.Items[item_index].SubItems[20].Text = cBean.SendGongLv.ToString();
            if (cBean.DrivaceType == 0x04)
            {
                byte LED_Staus = cBean.LEDStaus;
                if (LED_Staus == 0) str = "滅";
                else if (LED_Staus == 1) str = "亮";
                else if (LED_Staus == 2) str = "閃爍";
                else str = "";
            }
            else if (cBean.DrivaceType == 0x05)
            {
                byte LED_StausRED = (byte)(cBean.LEDStaus & 3);	  // 3  = 二进制：00 00 00 11;
                byte LED_StausGreen = (byte)(cBean.LEDStaus & 12);// 12 = 二进制：00 00 11 00;
                byte LED_StausBlue = (byte)(cBean.LEDStaus & 48); // 48 = 二进制：00 11 00 00;

                str = "R_" + getLEDstatus(LED_StausRED) + " G_" + getLEDstatus(LED_StausGreen) + " B_"
                    + getLEDstatus(LED_StausBlue);
            }            
            listView1.Items[item_index].SubItems[21].Text = str;
            if (listView1.Items[item_index].SubItems[0].ForeColor == Color.Red) 
            {
                if (item_index == 0 && cBean.TagIdStr.Equals(serchCardID)) return; 

                for (int i = 0; i < listView1.Items[item_index].SubItems.Count;i++ ) 
                {
                    listView1.Items[item_index].SubItems[i].ForeColor = Color.Black;
                }
            }
        }

        private string getLEDstatus(byte status){
	        string sta = "滅";
	        if (status == 1)
	        {
                sta = "亮";
	        }else if (status == 2)
	        {
		        sta = "閃爍";
	        }
	        return sta;
        }

        private void drawImage(object OBJ)  //图像模式加载数据,目前是阻塞的
        {
            try 
            {
                if (serchCardID != null && !serchCardID.Equals("") && tagInfor != null)
                {
                    var tps = tagInfor.Where(a => a.CardName.Equals(serchCardID));
                    if (tps.Count() > 0) 
                    {
                        drawCardInpanel(tps.First());
                        return;
                    }
                }
                var tagInfors = tagInfor.ToList();
                List<string> keyCardID = viewTagInfor.Keys.ToList();
                foreach (TagLocationPointBean tpBean in tagInfors)
                {
                    if (tpBean.CountIndex > 3) continue;
                    drawCardInpanel(tpBean);
                    keyCardID.Remove(tpBean.CardName);
                }
                foreach (var item in keyCardID)
                {
                    if(viewTagInfor.ContainsKey(item))
                        removeView(viewTagInfor[item],2);
                }
            }           
            catch { }         
        }

        private void drawCardInpanel(TagLocationPointBean tpBean) 
        {
            bool haveTag = false;
            Panel mPal = null;
            if (removeView(tpBean,1)) return;
            foreach (Control col in panel1.Controls)
            {
                if (col is Panel && col.Tag is TagLocationPointBean)
                {
                    TagLocationPointBean tagBean = (TagLocationPointBean)col.Tag;
                    if (!tagBean.Name.Equals(tpBean.Name)) continue;
                    if (tagBean.MColor == tpBean.MColor)
                        moveViewHandle(col, getPoint(tpBean));
                    else mPal = (Panel)col;
                    moveLab(col.Location, tpBean);
                    haveTag = true;
                    break;
                }
            }
            if (mPal != null)
            {
                addOrMoveTag(mPal, 2);
                drawPan(tpBean); //意思是要变色了
            }
            if (!haveTag) drawCard(tpBean);
        }

        public TagLocationPointBean getTagLocationPointBean(string tagID) 
        {
            TagLocationPointBean tpBean = null;
            var tpBeanItem = tagInfor.Where(a => a.CardName.Equals(tagID));
            if (tpBeanItem.Count() > 0) tpBean = tpBeanItem.FirstOrDefault();
            else 
            {
                var allcardBeans = cardBean.Where(a => a.TagIdStr.Equals(tagID));
                if (allcardBeans.Count() > 0) tpBean = getTagLocationPoint(allcardBeans.FirstOrDefault());
            }           
            if (tpBean != null) tpBean.Name = arrayTagName(tpBean.CardID);
            return tpBean;
        } 

        private void moveLab(Point palPo, TagLocationPointBean tpBean) 
        {
            foreach (Control col in panel1.Controls)
            {
                if (col is Label && col.Text.Equals(tpBean.Name))
                {
                    Point pal = getPoint(tpBean);
                    moveViewHandle(col, new Point(pal.X - col.Width / 2 + 7, pal.Y - col.Height));
                    return;
                }   
            }            
        }

        /// <summary>
        /// 移除View
        /// </summary>
        /// <param name="tpBean"></param>
        /// <param name="type"> 判断类型，= 1 表示需要判断超时时间， =2表示直接删除，不需要超时时间</param>
        /// <returns></returns>
        private bool removeView(TagLocationPointBean tpBean,int type) 
        {
            if (type == 1 && getCurrenRunTime() - tpBean.ShowTime < FileModel.getFlModel().ChFlBean.NoReveTime) return false;
       
            Panel movePal = null;
            Label moveLab = null;
            foreach (Control col in panel1.Controls)
            {
                if (col is Panel && col.Tag is TagLocationPointBean)
                {
                    TagLocationPointBean tagBean = (TagLocationPointBean)col.Tag;
                    if (!tagBean.Name.Equals(tpBean.Name)) continue;
                     movePal = (Panel)col;
                     if (movePal != null && moveLab != null) break;
                }
                if (col is Label && col.Text.Equals(tpBean.Name)) 
                {
                      moveLab = (Label)col;
                      if (movePal != null && moveLab != null) break;
                }
            }
            if (movePal != null) addOrMoveTag(movePal, 2); //panel1.Controls.Remove(movePal);
            if (moveLab != null) addOrMoveTag(moveLab, 2); //panel1.Controls.Remove(moveLab);
            if (tpBean.CardName != null) viewTagInfor.Remove(tpBean.CardName);
            return true;
        }//

        private void DELETagInSingle(byte[] ID)
        {
            if (ID == null) return;
            var singTags = sTagBeans.Where(a => a.tagHave(ID));
            if (singTags.Count() == 0) return;
            foreach (var sTagItem in singTags)
            {
                sTagItem.deleCard(ID);
            }
        }

        private void drawCard(TagLocationPointBean tpBean)
        {
            if (getCurrenRunTime() - tpBean.ShowTime > FileModel.getFlModel().ChFlBean.NoReveTime) return;//长时间未移动的不显示
            drawPan(tpBean);

            Label teLabe = new Label();
            teLabe.Text = tpBean.Name;// tpBean.Name;
            teLabe.AutoSize = true;
            teLabe.Font = new System.Drawing.Font("宋体", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            Point pal = getPoint(tpBean);
            Point pal2 = new Point(pal.X - teLabe.PreferredWidth / 2 + 7, pal.Y - teLabe.PreferredHeight);
            teLabe.Location = pal2;                      

            addOrMoveTag(teLabe, 1);
        }


        //画上圆点
        private void drawPan(TagLocationPointBean tpBean) 
        {
            Panel creat = new Panel();
            creat.Tag = tpBean;
            creat.Width = 10;
            creat.Height = 10;
            creat.Paint += new System.Windows.Forms.PaintEventHandler(this.createPanel_Paint);
            Point cardPoint = getPoint(tpBean);            
            creat.Location = cardPoint;
            addOrMoveTag(creat, 1);
            if (!viewTagInfor.ContainsKey(tpBean.CardName))
                viewTagInfor.Add(tpBean.CardName, tpBean);
        }

        private Point getTagLoPoint(int coutIndex) //byte[] ID,
        {
            Point pt = new Point(0,0);
            pt = SingleTagBean.getStaticCaPoint(coutIndex);
            //var singTag = sTagBeans.Where(a => { return a.tagHave(ID); }); //LinQ查找符合条件的元素
            //if (singTag.Count() > 0) pt = singTag.First().getPint(ID);            
            return pt;
        }

        private Point getPoint(TagLocationPointBean tpBean) 
        {
            Point po;
            if (tpBean.ColWeiHei != null && tpBean.ColWeiHei.Length == 2)
                po = new Point(tpBean.Point.X * panel1.Width / tpBean.ColWeiHei[0],
                        tpBean.Point.Y * panel1.Height / tpBean.ColWeiHei[1]);//tpBean.Point;
            else po = tpBean.Point;

            Point pt = getTagLoPoint(tpBean.CountIndex);
            po.X += pt.X;
            po.Y += pt.Y;
            return po;
        }

        private void createPanel_Paint(object sender, PaintEventArgs e)
        {
            if (!(sender is Panel)) return;
            Panel obPanel = (Panel)sender;
            if (!(obPanel.Tag is TagLocationPointBean)) return;
            TagLocationPointBean tagBean = (TagLocationPointBean)obPanel.Tag;

            Rectangle rect = new Rectangle(0, 0, 8, 8);//定义矩形,参数为起点横纵坐标以及其长和宽           
            //单色填充
            SolidBrush b1 = new SolidBrush(Color.Green);//定义单色画刷  
            if (tagBean.MColor == 1) b1.Color = Color.Black;
            if (tagBean.MColor == 3) b1.Color = Color.Red; //紧急定位
            if (tagBean.MColor == 4) b1.Color = Color.Pink;

            e.Graphics.FillEllipse(b1, rect);//填充这个矩形
        }

        public void setRunTimeDate(UInt32 time)
        {
            runTime = time;
        }

        private UInt32 getCurrenRunTime()
        {
            if (IsDealHistory)
                return runTime;
            else return XwDataUtils.GetTimeStamp();
        }

        public override void close() 
        {
            modelClose = true;
            new Thread(closeThread).Start();
        }

        public void closeThread()
        {
            try 
            {
                Thread.Sleep(1000);

                if (ckds != null)
                {
                    ckds.Clear();
                    ckds = null;
                }
                if (tagInfor != null)
                {
                    tagInfor.Clear();
                    tagInfor = null;
                }
                if (tagInforCache != null)
                {
                    tagInforCache.Clear();
                    tagInforCache = null;
                }
                if (viewTagInfor != null)
                {
                    viewTagInfor.Clear();
                    viewTagInfor = null;
                }
                if (panel1 != null)
                {
                    panel1 = null;
                }
                if (listView1 != null)
                {
                    listView1 = null;
                }
                if (listView2 != null)
                {
                    listView2 = null;
                }
                if (LAB_COUNT != null)
                {
                    LAB_COUNT = null;
                }
                if (cardBean != null)
                {
                    cardBean.Clear();
                    cardBean = null;
                }
                if (allCard != null)
                {
                    allCard.Clear();
                    allCard = null;
                }
                if (serchCardID != null)
                {
                    serchCardID = null;
                }
                if (mainCanDictionarys != null)
                {
                    mainCanDictionarys.Clear();
                    mainCanDictionarys = null;
                }
                if (mainCanInfos != null)
                {
                    mainCanInfos.Clear();
                    mainCanInfos = null;
                }
                if (listViewCanInfos != null)
                {
                    listViewCanInfos.Clear();
                    listViewCanInfos = null;
                }
                if (nodeCardUpBean != null)
                {
                    nodeCardUpBean.Clear();
                    nodeCardUpBean = null;
                }
                if (quYuBean != null)
                {
                    quYuBean = null;
                }
                if (cengjiID != null)
                {
                    cengjiID = null;
                }
                if (sTagBeans != null)
                {
                    sTagBeans.Clear();
                    sTagBeans = null;
                }
                if (viewFrom != null)
                {
                    viewFrom = null;
                } 
            }
            catch { }           
        }

        /// <summary>
        /// 返回MODEL的标志位
        /// </summary>
        /// <returns></returns>
        public override string TAG()
        {
            return "LocationViewModel";
        }


    }
}
