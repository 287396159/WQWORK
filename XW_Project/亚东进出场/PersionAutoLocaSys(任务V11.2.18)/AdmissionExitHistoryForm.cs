using Common;
using PersionAutoLocaSys.Bean;
using PersionAutoLocaSys.Model;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PersionAutoLocaSys
{
    public partial class AdmissionExitHistoryForm : Form
    {
        List<AdmissionExit> hisAdmissionExits;
        List<AdmissionExit> hisAllAdmissionExits;

        public AdmissionExitHistoryForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)  //查詢記錄
        {
            DateTime startTm = DateTime.Parse(startTP.Text);
            DateTime endTm = DateTime.Parse(EndTP.Text);

            if (!tabControl1.SelectedTab.Name.Equals("tabPage1"))
            {
                setAlladmissionExit(startTm, endTm);
                setPuTongadmissionExit(startTm, endTm);                
            }
            else 
            {
                setPuTongadmissionExit(startTm, endTm);
                setAlladmissionExit(startTm, endTm);
            }                    
        }

        private void setPuTongadmissionExit(DateTime startTm, DateTime endTm) 
        {
            hisAdmissionExits = FileModel.fileInit().getHisAdmission(startTm, endTm);
            if (hisAdmissionExits == null) return;

            if (checkBox1.Checked) setHisAdmissionList(hisAdmissionExits); //入场
            else if (checkBox2.Checked) setHisExitList(hisAdmissionExits); //出厂
            else setHisAdmiExitList(hisAdmissionExits);
        }

        private void setAlladmissionExit(DateTime startTm, DateTime endTm)
        {
            hisAllAdmissionExits = FileModel.fileInit().getHisAllAdmission(startTm, endTm);
            setHisAllAdmiExitList(hisAllAdmissionExits);
        }

        private void AdmissionExitHistoryForm_Load(object sender, EventArgs e)
        {
            startTP.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            EndTP.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            startTP.Text = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day).ToString();
            EndTP.Text = DateTime.Now.ToString();
        }

        private void setHisAdmissionList(List<AdmissionExit> admissionExits)
        {
            List<AdmissionExit> admissions = admissionExits.Where(a => a.Model.Equals(AdmissionExit.ADMISSION)).ToList();
            daTagListView.Items.Clear();
            int length = admissions.Count;
            for (int i = 0; i < length; i++)
            {
                ListViewItem item = new ListViewItem();
                item.Text = admissions[i].TagID;
                item.Name = admissions[i].TagID;
                item.SubItems.Add(admissions[i].Name);

                item.SubItems.Add(XwDataUtils.dataFromTimeStamp((Int32)admissions[i].Time)); //入场时间
                item.SubItems.Add("--");
                String workIDs = "";
                for (int j = 0; j < 16; j++)
                {
                    workIDs += admissions[i].WorkIDbyte[j].ToString("X2");
                }
                item.SubItems.Add(workIDs);
                daTagListView.Items.Add(item);
            }
        }

        private void setHisExitList(List<AdmissionExit> admissionExits)
        {
            List<AdmissionExit> exits = admissionExits.Where(a => a.Model.Equals(AdmissionExit.EXIT)).ToList();
            daTagListView.Items.Clear();
            int length = exits.Count;
            for (int i = 0; i < length; i++)
            {
                ListViewItem item = new ListViewItem();
                item.Text = exits[i].TagID;
                item.Name = exits[i].TagID;
                item.SubItems.Add(exits[i].Name);

                item.SubItems.Add("--");
                item.SubItems.Add(XwDataUtils.dataFromTimeStamp((Int32)exits[i].Time)); //出厂时间                
                String workIDs = "";
                for (int j = 0; j < 16; j++)
                {
                    workIDs += exits[i].WorkIDbyte[j].ToString("X2");
                }
                item.SubItems.Add(workIDs);
                daTagListView.Items.Add(item);
            }
        }

        /// <summary>
        /// 设置历史出厂入场数据给列表
        /// </summary>
        private void setHisAdmiExitList(List<AdmissionExit> admissionExits)
        {
            List<AdmissionExit> admissions = admissionExits.Where(a => a.Model.Equals(AdmissionExit.ADMISSION)).ToList();
            List<AdmissionExit> exits = admissionExits.Where(a => a.Model.Equals(AdmissionExit.EXIT)).ToList();

            daTagListView.Items.Clear();
            int length = admissions.Count;
            for (int i = 0; i < length; i++)
            {
                ListViewItem item = new ListViewItem();
                item.Text = admissions[i].TagID;
                item.Name = admissions[i].TagID;
                item.SubItems.Add(admissions[i].Name);

                String startTime = XwDataUtils.dataFromTimeStamp((Int32)admissions[i].Time);
                item.SubItems.Add(startTime); //入场时间

                String endTime = "";
                for (int j = 0; j < exits.Count;j++ )
                {
                    if (exits[j].TagID.Equals(admissions[i].TagID) && exits[j].Time > admissions[i].Time)
                       // && admissions[i].Time + 72000 > exits[j].Time) //72000是秒 = 20小时，20小时内应该有一套上下班的
                    {
                        endTime = XwDataUtils.dataFromTimeStamp((Int32)exits[j].Time);
                        break;
                    }                                              
                }
                item.SubItems.Add(endTime);//出厂时间
                String workIDs = "";
                if (admissions[i].WorkIDbyte != null) 
                {
                    for (int j = 0; j < 16; j++)
                    {
                        workIDs += admissions[i].WorkIDbyte[j].ToString("X2");
                    }
                }                
                item.SubItems.Add(workIDs);
                daTagListView.Items.Add(item);
            }
        }

        /// <summary>
        /// 設置所有的歷史進出場數據
        /// </summary>
        /// <param name="admissionExits"></param>
        private void setHisAllAdmiExitList(List<AdmissionExit> admissionExits)
        {
            listView1.Items.Clear();
            int length = admissionExits.Count;
            for (int i = 0; i < length; i++)
            {
                ListViewItem item = new ListViewItem();
                item.Text = admissionExits[i].TagID;
                item.Name = admissionExits[i].TagID;
                item.SubItems.Add(admissionExits[i].Name);
                item.SubItems.Add(admissionExits[i].Model);
                item.SubItems.Add(XwDataUtils.dataFromTimeStamp((Int32)admissionExits[i].Time)); //進出厂时间                
                String workIDs = "";
                for (int j = 0; j < 16; j++)
                {
                    workIDs += admissionExits[i].WorkIDbyte[j].ToString("X2");
                }
                item.SubItems.Add(workIDs);
                listView1.Items.Add(item);
            }
        }

        /// <summary>
        /// 判断给入的参数是否是界面相关选中时间
        /// 
        /// </summary>
        /// <param name="admissExit"></param>
        /// <returns></returns>
        private bool isData(AdmissionExit admissExit)
        {
            if (checkBox1.Checked)
            {
                return admissExit.Model.Equals(AdmissionExit.ADMISSION);
            }
            else if (checkBox2.Checked)
            {
                return admissExit.Model.Equals(AdmissionExit.EXIT);
            }
            else
            {
                return false;
            }
        }

        private void button2_Click(object sender, EventArgs e) //匯出excel
        {
            if (hisAdmissionExits == null)
            {
                MessageBox.Show("數據不存在");
                return;
            } 

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "xls files   (*.xls)|*.xls";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                new Thread(saveCardXlsFileThread).Start(saveFileDialog1.FileName);
            } 
        }

        /// <summary>
        /// 普通的進出場數據
        /// </summary>
        /// <param name="obj"></param>
        private void saveCardXlsFileThread(object obj)
        {
            string filePath = (string)obj;
            saveCardXlsFile(filePath);
        }

        public void saveCardXlsFile(string filePath)
        {
            try
            {
                if (hisAdmissionExits == null) 
                {
                    MessageBox.Show("數據發生錯誤！！");
                    return;
                }

                List<AdmissionExit> admissions = hisAdmissionExits.Where(a => a.Model.Equals(AdmissionExit.ADMISSION)).ToList();
                List<AdmissionExit> exits = hisAdmissionExits.Where(a => a.Model.Equals(AdmissionExit.EXIT)).ToList();

                List<ImportAdmissExit> importAdmissionExit = new List<ImportAdmissExit>(); //合并一下进出场数据
                int length = admissions.Count;
                for (int i = 0; i < length; i++)
                {
                    ImportAdmissExit importData = new ImportAdmissExit();
                    importData.setAdmissData(admissions[i]);

                    String endTime = "";
                    for (int j = 0; j < exits.Count; j++)
                    {
                        if (exits[j].TagID.Equals(admissions[i].TagID) && exits[j].Time > admissions[i].Time)
                        // && admissions[i].Time + 72000 > exits[j].Time) //72000是秒 = 20小时，20小时内应该有一套上下班的
                        {
                            endTime = XwDataUtils.dataFromTimeStamp((Int32)exits[j].Time);
                            break;
                        }
                    }
                    importData.ExitTime = endTime;
                    importAdmissionExit.Add(importData);
                }


                Dictionary<string, string> cellheader = new Dictionary<string, string> { 
                    { "Name", "名稱" },
                    { "TagID", "卡片ID" },    
                    { "WorkID", "進出場ID" },                       
                    { "AdmissTime", "進場時間" },   
                    { "ExitTime", "出場時間" }, 
                    
                };
                string urlPath = ExcelHelper.EntityListToExcel2003(cellheader, importAdmissionExit, filePath, "AdmissionExit", ExcelWeithEnum.ADMISSIONEXIT_PUTONG);
            }
            catch (Exception ex)
            {
                Debug.Write("saveCardXlsFile" + ex.Message);
            }
        }

        /// <summary>
        /// 所有進出場數據
        /// </summary>
        /// <param name="obj"></param>
        private void saveAllCardXlsFileThread(object obj)
        {
            string filePath = (string)obj;
            saveAllaeCardXlsFile(filePath);
        }

        public void saveAllaeCardXlsFile(string filePath)
        {
            try
            {
                if (hisAllAdmissionExits == null)
                {
                    MessageBox.Show("數據發生錯誤！！");
                    return;
                }

                Dictionary<string, string> cellheader = new Dictionary<string, string> { 
                    { "Name", "名稱" },
                    { "TagID", "卡片ID" },    
                    { "WorkIDStr", "進出場ID" },   
                    { "AeTime", "進出場時間" },   
                    { "Model", "進出場模式" }, 
                };
                string urlPath = ExcelHelper.EntityListToExcel2003(cellheader, hisAllAdmissionExits, filePath, "AdmissionExit", ExcelWeithEnum.ADMISSIONEXIT_ALL);
            }
            catch (Exception ex)
            {
                Debug.Write("saveCardXlsFile" + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (hisAllAdmissionExits == null)
            {
                MessageBox.Show("數據不存在");
                return;
            }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "xls files   (*.xls)|*.xls";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                new Thread(saveAllCardXlsFileThread).Start(saveFileDialog1.FileName);
            } 
        }

    }
}
