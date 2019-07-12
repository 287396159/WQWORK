using CiXinLocation.bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CiXinLocation.Model
{
    class QuyuAutoChange
    {
        private String currentCenJi = "";
        private String currentQuyuID = "";
        private String currentNODEID = "";
        private String currentTagID = "";
        private String quyuText = "";
        private String cengjiText = "";
        private List<CenJiBean> fileCenJiData;
        private List<CanKaoDianBean> canKaoDians;
        private QuyuChangeLinster quyuLinster;
        
        public List<CanKaoDianBean> CanKaoDians
        {
            get { return canKaoDians; }
            set { canKaoDians = value; }
        }


        public List<CenJiBean> FileCenJiData
        {
            get { return fileCenJiData; }
            set { fileCenJiData = value; }
        }


        public QuyuAutoChange() 
        {
            FileCenJiData = FileModel.getFlModel().CenJiData.ToList();
            CanKaoDians = FileModel.getFlModel().ChFlBean.CanKaoDians.ToList();
        }

        public void setOnChangeLinster(QuyuChangeLinster quyuLinster)
        {
            this.quyuLinster = quyuLinster;
        }

        /// <summary>
        /// 设置当前节点的ID。
        /// </summary>
        /// <param name="nodeID"></param>
        public void setCurrentNODEID(byte[] nodeID) 
        {
            if (nodeID.Length != 2) return;
            if (CanKaoDians == null) return;
            new Thread(() => 
            {
                String nodeStr = nodeID[0].ToString("X2") + nodeID[1].ToString("X2");
                for (int i = 0; i < CanKaoDians.Count; i++)
                {
                    if (!CanKaoDians[i].Id.Equals(nodeStr)) continue;
                    setcurrentID(CanKaoDians[i]);
                    break;
                }
            }).Start();            
        }


        private void setcurrentID(CanKaoDianBean can) 
        {
            currentNODEID = can.Id;
            currentQuyuID = can.QuYuID;

            for (int i = 0; i < FileCenJiData.Count;i++)
            {
                CenJiBean cenJi = FileCenJiData[i];
                for (int j = 0; j < cenJi.QuYuBeans.Count; j++) 
                {
                    QuYuBean quyuBean = cenJi.QuYuBeans[j];
                    if (!currentQuyuID.Equals(quyuBean.QuyuID)) continue;
                    quyuText = getIDNameBuder(quyuBean.QuyuName,quyuBean.QuyuID).ToString();
                    cengjiText = getIDNameBuder(cenJi.CenJiName, cenJi.ID).ToString();
                    if (quyuLinster != null) 
                    {
                        quyuLinster.onCengJiChange(cengjiText);
                        quyuLinster.onQuyuChange(quyuText);
                    } 
                    return;
                }
            }
        }


        private StringBuilder getIDNameBuder(String name,string id)
        {
            StringBuilder buder = new StringBuilder();
            buder.Append(name);
            buder.Append("(");
            buder.Append(id);
            buder.Append(")");
            return buder;
        }
    }

    public interface QuyuChangeLinster 
    {
        void onQuyuChange(String changeText);
        void onCengJiChange(String changeText);
    }
}
