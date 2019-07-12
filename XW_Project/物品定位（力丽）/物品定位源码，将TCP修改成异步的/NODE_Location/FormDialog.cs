using CiXinLocation.bean;
using CiXinLocation.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CiXinLocation
{
    public partial class FormDialog : Form
    {
        private List<CardBean> saveCards;
        private List<CanKaoDianBean> saveCanKaoDians;

        private List<CardBean> cardNameChongFu;  //卡片名称重复了
        private List<CardBean> changeCardNames;

        private List<CanKaoDianBean> changeCanKaoDianNames;
        private List<CanKaoDianBean> nodeNameChongFu; //節點名称重复了


        private DrivaceTypeAll drivaceType;
        private int drivaceAllCount = -1;        

        public FormDialog()
        {
            InitializeComponent();
        }

        public FormDialog(DrivaceTypeAll drivaceType):this()
        {
            this.drivaceType = drivaceType;
        }

        /// <summary>
        /// 導入設備的總數量
        /// </summary>
        public int DrivaceAllCount
        {
            get { return drivaceAllCount; }
            set { drivaceAllCount = value; }
        }

        /// <summary>
        /// saveCards为null时，创建集合，不会null就不处理
        /// </summary>
        private void CreateSaveCards()
        {
            if (saveCards == null) saveCards = new List<CardBean>();
        }

        /// <summary>
        /// saveCanKaoDians为null时，创建集合，不会null就不处理
        /// </summary>
        private void CreateSaveCanKaoDians() 
        {
            if (saveCanKaoDians == null) saveCanKaoDians = new List<CanKaoDianBean>();
        }

        /// <summary>
        /// changeCardNames为null时，创建集合，不会null就不处理
        /// </summary>
        private void CreateChangeCards()
        {
            if (changeCardNames == null) changeCardNames = new List<CardBean>();
        }

        /// <summary>
        /// changeCanKaoDianNames为null时，创建集合，不会null就不处理
        /// </summary>
        private void CreateChangeCanKaoDians()
        {
            if (changeCanKaoDianNames == null) changeCanKaoDianNames = new List<CanKaoDianBean>();
        }


        /// <summary>
        /// changeCardNames为null时，创建集合，不会null就不处理
        /// </summary>
        private void CreateChongFuCards()
        {
            if (cardNameChongFu == null) cardNameChongFu = new List<CardBean>();
        }

        /// <summary>
        /// changeCanKaoDianNames为null时，创建集合，不会null就不处理
        /// </summary>
        private void CreateChongFuCanKaoDians()
        {
            if (nodeNameChongFu == null) nodeNameChongFu = new List<CanKaoDianBean>();
        }
        /// <summary>
        /// 保存修改的设备名称和ID
        /// </summary>
        /// <param name="cards"></param>
        public void saveCardID(List<CardBean> cards) 
        {
            if(cards == null || cards.Count == 0) return;
            DrivaceAllCount = cards.Count;
            CreateSaveCards();
            CreateChangeCards();

            removeChongFuCard(ref cards);

            Dictionary<string, CardBean> cardDictionary = FileModel.getFlModel().ChFlBean.CardDic;

            if (cardDictionary == null || cardDictionary.Count == 0) 
            {
                saveCards = cards.ToList();
                return;
            }
            int cardCount = cards.Count;
            for (int i = 0; i < cardCount;i++ )
            {
                if (!cardDictionary.ContainsKey(cards[i].Id)) saveCards.Add(cards[i]);
                else if (cardDictionary.ContainsKey(cards[i].Id) && !cardDictionary[cards[i].Id].Name.Equals(cards[i].Name))
                    changeCardNames.Add(cards[i]);
            }     
       
        }


        //取出卡片重复的名称
        private void removeChongFuCard(ref List<CardBean> cards) 
        {
            CreateChongFuCards();
            Dictionary<string, CardBean> caedName = new Dictionary<string, CardBean>(); //以名稱為key的集合
            List<string> nameLists = new List<string>();
            foreach (var keyItem in cards)
            {
                string name = keyItem.Name;
                if (!caedName.ContainsKey(name)) caedName.Add(name, keyItem);
                else 
                {
                    nameLists.Add(name);
                    cardNameChongFu.Add(keyItem);
                } 
                //else cardNameChongFu.Add(keyItem);
            }
            foreach (var str in nameLists) 
            {
                if (caedName.ContainsKey(str)) cardNameChongFu.Add(caedName[str]);
                caedName.Remove(str);
            }
            Dictionary<string, CardBean> cardDictionary = new Dictionary<string,CardBean>(FileModel.getFlModel().ChFlBean.CardDic);
            foreach (var nameItem in cardDictionary)
            {
                string nameItemName = nameItem.Value.Name;
                string nameItemID = nameItem.Value.Id;
                if (caedName.ContainsKey(nameItemName)) 
                {
                    string caedNameName = caedName[nameItemName].Name;
                    string caedNameID = caedName[nameItemName].Id;
                    if (!caedNameID.Equals(nameItemID)) //name相同，ID不同，說明name重複
                    {
                        cardNameChongFu.Add(caedName[nameItemName]);
                        caedName.Remove(caedNameName);
                    }
                }
            }
            cards = caedName.Values.ToList();
        }


        /// <summary>
        /// 给ListView添加Item
        /// </summary>
        private void listViewAddItem() 
        {
            CreateSaveCards();
            CreateChangeCards();
            List<ListViewItem> liItems = new List<ListViewItem>();
            int changeCount = changeCardNames.Count;
            for (int k = 0; k < changeCount; k++)
            {
                liItems.Add(getListItems(changeCardNames[k].Name, changeCardNames[k].Id,"OK"));
            }
            int saveCout = saveCards.Count;
            for (int j = 0; j < saveCout; j++)
            {
                liItems.Add(getListItems(saveCards[j].Name, saveCards[j].Id, "OK"));
            }
            int chongFuCout = cardNameChongFu.Count;
            for (int i = 0; i < chongFuCout; i++)
            {
                liItems.Add(getListItems(cardNameChongFu[i].Name, cardNameChongFu[i].Id, "名稱重複"));
            }
            listView1.Items.AddRange(liItems.ToArray());
        }

        public void saveNODEID(List<CanKaoDianBean> canKaoDians)
        {
            if (canKaoDians == null || canKaoDians.Count == 0) return;
            DrivaceAllCount = canKaoDians.Count;

            CreateSaveCanKaoDians();
            CreateChangeCanKaoDians();
            removeChongFuCanKaoDian(ref canKaoDians);
            List<CanKaoDianBean> canBeans = FileModel.getFlModel().ChFlBean.CanKaoDians.ToList();

            if (canBeans == null || canBeans.Count == 0)
            {
                saveCanKaoDians = canKaoDians.ToList();
                return;
            }
            int cardCount = canKaoDians.Count;
            int nodeBeanCount = canBeans.Count;
            for (int i = 0; i < cardCount; i++)
            {
                for (int j = 0; j < nodeBeanCount;j++ )
                {
                    if (!canBeans[j].Id.Equals(canKaoDians[i].Id)) continue;
                    if (!canBeans[j].Name.Equals(canKaoDians[i].Name))
                        changeCanKaoDianNames.Add(canKaoDians[i]);
                    break;
                }
            }
        //    listViewAddNODEItem();
        }


        private void removeChongFuCanKaoDian(ref List<CanKaoDianBean> canKaoDians)
        {
            CreateChongFuCanKaoDians();
            Dictionary<string, CanKaoDianBean> canKaoDianName = new Dictionary<string, CanKaoDianBean>(); //以名稱為key的集合
            List<string> nameLists = new List<string>();
            foreach (var keyItem in canKaoDians)
            {
                string name = keyItem.Name;
                if (!canKaoDianName.ContainsKey(name)) canKaoDianName.Add(name, keyItem);
                else
                {
                    nameLists.Add(name);
                    nodeNameChongFu.Add(keyItem);
                }
                //else cardNameChongFu.Add(keyItem);
            }
            foreach (var str in nameLists)
            {
                if (canKaoDianName.ContainsKey(str)) nodeNameChongFu.Add(canKaoDianName[str]);
                canKaoDianName.Remove(str);
            }
            Dictionary<string, CardBean> cardDictionary = new Dictionary<string,CardBean>(FileModel.getFlModel().ChFlBean.CardDic);
            foreach (var nameItem in cardDictionary)
            {
                string nameItemName = nameItem.Value.Name;
                string nameItemID = nameItem.Value.Id;
                if (canKaoDianName.ContainsKey(nameItemName))
                {
                    string caedNameName = canKaoDianName[nameItemName].Name;
                    string caedNameID = canKaoDianName[nameItemName].Id;
                    if (!caedNameID.Equals(nameItemID)) //name相同，ID不同，說明name重複
                    {
                        nodeNameChongFu.Add(canKaoDianName[nameItemName]);
                        canKaoDianName.Remove(caedNameName);
                    }
                }
            }
            canKaoDians = canKaoDianName.Values.ToList();
        }

        /// <summary>
        /// 给ListView添加Item
        /// </summary>
        private void listViewAddNODEItem()
        {
            CreateSaveCanKaoDians();
            CreateChangeCanKaoDians();

            List<ListViewItem> liItems = new List<ListViewItem>();
            /*int saveCout = saveCanKaoDians.Count;
            for (int j = 0; j < saveCout; j++)
            {
                liItems.Add(getListItems(saveCanKaoDians[j].Name, saveCanKaoDians[j].Id));
            }*/
            int changeCount = changeCanKaoDianNames.Count;
            for (int k = 0; k < changeCount; k++)
            {
                liItems.Add(getListItems(changeCanKaoDianNames[k].Name, changeCanKaoDianNames[k].Id, "OK"));
            }
            int chongFuCount = nodeNameChongFu.Count;
            for (int i = 0; i < chongFuCount; i++)
            {
                liItems.Add(getListItems(nodeNameChongFu[i].Name, nodeNameChongFu[i].Id, "名稱重複"));
            }
            listView1.Items.AddRange(liItems.ToArray());
        }

        private ListViewItem getListItems(string name, string id,string msg)
        {
            ListViewItem liItem = new ListViewItem();
            liItem.SubItems[0].Text = id;
            liItem.SubItems.Add(name);
            liItem.SubItems.Add(msg);
            return liItem;
        }

        private void button1_Click(object sender, EventArgs e) // 確定
        {
            if (drivaceType == DrivaceTypeAll.CARD)
            {
                startChangeCard();
            }
            else if (drivaceType == DrivaceTypeAll.CANKAODIAN)
            {
                startChangeNODE();
            }
            else 
            {
                this.DialogResult = DialogResult.None;
                this.Close();
                return;//此处退出，为了不走下面的语句，要不然，下面是文件操作，是耗时的。
            } 
            FileModel.getFlModel().setCFData();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void startChangeCard() //开始修改卡片名称或者增加卡片，并保存
        {
            if (saveCards == null || changeCardNames == null) return;
            int changeCardCount = changeCardNames.Count;
            for (int j = 0; j < changeCardCount; j++)
            {
                FileModel.getFlModel().ChFlBean.setCardValues(changeCardNames[j]);
            }
            int saveCardCount = saveCards.Count;
            for (int i = 0; i < saveCardCount; i++)
            {
                FileModel.getFlModel().ChFlBean.addCardValues(saveCards[i].Id, saveCards[i].Name);
            }
        }

        private void startChangeNODE()
        {
            if (saveCanKaoDians == null || changeCanKaoDianNames == null) return;

            int changeCardCount = changeCanKaoDianNames.Count;
            List<CanKaoDianBean> canBeans = FileModel.getFlModel().ChFlBean.CanKaoDians.ToList();
            int canCount = canBeans.Count;
            for (int j = 0; j < changeCardCount; j++)
            {
                for (int i = 0; i < canCount;i++ )
                {
                    if (!canBeans[i].Id.Equals(changeCanKaoDianNames[j].Id)) continue;
                    FileModel.getFlModel().ChFlBean.CanKaoDians[i].Name = changeCanKaoDianNames[j].Name;
                    break;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) // 取消
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormDialog_Load(object sender, EventArgs e)
        {
            if (DrivaceAllCount == -1) 
            {
                new Thread(messageThread).Start();
            //    return;
            }
            if (drivaceType == DrivaceTypeAll.CARD)
            {
                this.Text = "卡片導入信息";
                listViewAddItem();
                label2.Text = "導入數量：" + (changeCardNames.Count + saveCards.Count);
            }
            else if (drivaceType == DrivaceTypeAll.CANKAODIAN)
            {
                this.Text = "節點導入信息";
                listViewAddNODEItem();
                label2.Text = "導入數量：" + (changeCanKaoDianNames.Count);// + saveCanKaoDians.Count
            }
            label1.Text = "文檔總數量：" + DrivaceAllCount;            
        }

        private void messageThread() 
        {
            string driMsg = drivaceType == DrivaceTypeAll.CARD ? "卡片信息" : "節點信息";
            driMsg = drivaceType == DrivaceTypeAll.CANKAODIAN ? "設備信息":driMsg;
            MessageBox.Show("文件導入錯誤，請確認該文件是否有" + driMsg);
            this.Invoke((EventHandler)(delegate
            { //放入主線程 
                this.Close();
            }));           
        }
    }
}
