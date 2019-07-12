using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CiXinLocation.Utils
{
    class StringUtils {

        enum Language{
            english = 0, //英文
            chinese = 1,//中文
        }
        private static Language lau = Language.english;
        public static int id_length = 2;
        public static int version_length = 4;
        public static String addHandle = "第";
        public static String addressWei = "行地址与当前选择的设备类型一致。\r\n不添加该Hex文件";
        public static String prompt = "提示";
        public static String quZhiFanWei = "取值范围：1~255";
        public static String quZhiFanWei2 = "取值范围：0.01~2.55";
        public static String staticModel = "静态模式";
        public static String dynmicModel = "动态模式";       
        public static String commError = "串口操作失敗";
        public static String commOpen = "请打开串口";
        public static String open = "打開";
        public static String close = "關閉";
        public static String comNothing = "串口號為空";
        public static String errorID = "未选择ID";
        public static String errorIP = "IP格式不正确，正确格式参考：192.168.1.101";
        public static String errorPort = "格式错误";
        public static String driType = "设备类型";
        public static String wifiLength = "WiFi只能在32个字符内";
        public static String changeDrivace = "当前更新设备";
        public static String currentDrivace = "当前设置设备";
        public static String errorHexFile = "当前文件的设备类型不正确";
        public static String hexFileNoType = "当前选择的设备与选择的文件类型不一致。";
        public static String updataSuccess = "更新成功";
        public static String updataFile = "更新失败";
        public static String updataSpeed = "更新进度";
        public static String hexFileInfor = "hex文件版本信息";
        public static String hexFileSize = "hex文件大小";
        public static String year = "年";
        public static String month = "月";
        public static String day = "日";
        public static String node = "节点";
        public static String cankaodian = "参考点";
        public static String card = "卡片";
        public static String USB_Dangle = "USB_Dangle";
        public static String USB_DangleInfor = "USB_dangle版本:";
        public static String isNewVersin = "文件与设备版本一致";
        public static String isSuccess = "成功";// "success";
        public static String isFail = "失败"; //"fail");
        public static String operationFailed = "操作失败";
        public static String hexFileAddErr = "文件地址错误";
        public static String hexFileToLong = "讀取到的hex文件的一行太長，解析文件失敗！";
        public static String hexFileStringErr = "讀取到的hex文件的一行內容的開始和結尾字符不符合要求，解析文件失敗！";
        public static String hexFileStrNoHex = "文件中存在無法轉換的字符，解析文件失敗！";
        public static String hexFileCheckFail = "計算校驗和失敗，解析文件失敗！";
        public static String hexFileHangLenErr = "行的數據長度不符合，解析文件失敗！";
        public static String hexFileTypeErr = "hex文件中的數據類型不为00、01、04、05，超出本程式的解析範圍，解析文件失敗！";
        public static String hexFileReadSuccess = "Hex文件讀取成功，文件大小";
        public static String findCom = "检测到";
        public static String Com_Remove = "被拔出！！";

        private static void chInEn() {

            findCom = "Detected that ";
            Com_Remove = " was pulled out";
            prompt = "prompt";
            addHandle = "the ";
            addressWei = " row address is in line with the type of device currently selected. \r\n does not add the Hex file";
            quZhiFanWei = "Range of values: 1~255";
            quZhiFanWei2 = "Range of values: 0.01~2.55";
            staticModel = "static mode";
            dynmicModel = "dynamic model";
            isSuccess = "success";
            isFail = "fail";
            isNewVersin = "The file is identical to the device version";
            hexFileNoType = "The currently selected device is out of line with the selected file type.";
            commError = "Serial operation failed";
            commOpen = "open the serial,please";
            open = "open";
            close = "close";
            comNothing = "Serial number is empty";
            errorID = "Unselected";
            errorIP = "The IP format is incorrect. The correct format reference: 192.168.1.101";
            errorPort = "Format error";
            wifiLength = "WiFi can only be in 32 characters";
            changeDrivace = "Current update device";
            currentDrivace = "Current setting device";
            errorHexFile = "The device type of the current file is incorrect";
            updataSuccess = "successful";
            updataFile = "failed";
            updataSpeed = "progress";
            hexFileInfor = "Size";
            hexFileSize = "Information";//
            year = "/";//"year";
            month = "/";//"month";
            day = "/";//"day";
            driType = "Type";
            node = "node";
            cankaodian = "reference point";
            card = "tag";
            USB_DangleInfor = "USB_dangle version:";
            operationFailed = "operation  failed";
            hexFileAddErr = "File address error";
            hexFileToLong = "The line from the hex file read is too long, and the parsing file failed!";
            hexFileStringErr = "The start and end characters of a line of content read from the hex file do not meet the requirements, and the parsing file fails!";
            hexFileStrNoHex = "There are characters that cannot be converted in the file. The parsing file failed!";
            hexFileCheckFail = "The checksum failed. The parsing file failed!";
            hexFileHangLenErr = "The row data length does not match. The parsing file failed!";
            hexFileTypeErr = "The data types in the hex file are not 00, 01, 04, and 05. Beyond the scope of this program, the parsing file failed!";
            hexFileReadSuccess = "Hex file read successfully, file size:"; 
        }

        public static Hashtable ceTable = new Hashtable();
        private static void startHashTable()
        {
            if (ceTable == null) ceTable = new Hashtable();
        }


        /// <summary>
        /// 中文转英文的切换。通过键值对的形式转换
        /// </summary>
        public static void getCEnglishHashTable() {
            if (lau == Language.chinese) return;
            chInEn();
            startHashTable();

            ceTable.Add("搜索", "search");
            ceTable.Add("成功", "success");
            ceTable.Add("失败", "fail");
            ceTable.Add("复位", "reset");
            ceTable.Add("清除", "clear");
            ceTable.Add("刷新", "Refresh");
            ceTable.Add("打开", "open");
            ceTable.Add("浏览", "browse");
            ceTable.Add("更新", "update");
            ceTable.Add("进度", "progress");
            ceTable.Add("设置", "setting");
            ceTable.Add("开始更新", "StartUpdating");
            ceTable.Add("当前设置设备:", "Current setting device:");
            ceTable.Add("当前更新设备:", "Current update device:");
            ceTable.Add("节点固件更新", "Node firmware update");
            ceTable.Add("USB_Dangle信息", "USB_Dangle information");
            ceTable.Add("串口操作", "Serial operation");
            ceTable.Add("串口号:", "Serial：");
            ceTable.Add("正在努力打开中...", "Trying to open it...");
            ceTable.Add(" 节点设备列表", "Node device list");
            ceTable.Add("节点ID", "node ID");
            ceTable.Add("版本", "version");
            ceTable.Add("更新进度", "progress");
            ceTable.Add("设置ServiseIP", "Set ServiseIP");
            ceTable.Add("设置Servise端口", "Set ServisePort");
            ceTable.Add("设置wifi名称", "Set WiFi name");
            ceTable.Add("设置wifi密码", "Set WiFi password");
            ceTable.Add("读取ServiseIP", "read serviseIP");
            ceTable.Add("读取Servise端口", "read ServisePort");
            ceTable.Add("读取wifi名称", "read WiFi name");
            ceTable.Add("读取wifi密码", "read WiFi password");
            ceTable.Add("文件名", "file name");
            ceTable.Add("hex文件大小:", "File size:");
            ceTable.Add("hex文件版本信息:", "File version information:");
            ceTable.Add("hex文件地址:", "file address:");
            ceTable.Add("节点", "node");
            ceTable.Add("参考点", "reference point");
            ceTable.Add(" 参考点设备列表", "Reference point device list");
            ceTable.Add(" 参考点固件更新", "Reference point firmware update");
            ceTable.Add("参考点ID", "reference ID");
            ceTable.Add("卡片ID", "card ID");
            ceTable.Add("卡片", "card");
            ceTable.Add("卡片更新", "update");
            ceTable.Add(" 卡片设备列表", "Card device list");
            ceTable.Add(" 卡片固件更新", "Card firmware update");
            ceTable.Add("关闭更新", "Close update");
            ceTable.Add("清空Dangle", "clear Dangle");
            ceTable.Add("清空", "clear");
            ceTable.Add("传送进度", "schedule");
            ceTable.Add("卡片设置", "Card setting");
            ceTable.Add("节点设置", "Node setting");
            ceTable.Add("设置上报时间", "Set reporting time");
            ceTable.Add("设置卡片功率", "Set card power");
            ceTable.Add("读取上报时间", "Read report time");
            ceTable.Add("读取卡片功率", "Read card power");
            ceTable.Add("静态模式", "static mode");
            ceTable.Add("动态模式", "dynamic model");
            ceTable.Add("USB_dangle版本：", "USB_dangle Version：");
            ceTable.Add("设置节点模式", "set node mode");
            ceTable.Add("读取节点模式", "read node mode");
            ceTable.Add("设置节点IP", "set node IP");
            ceTable.Add("读取节点IP", "read node IP");
            ceTable.Add("设置SubMask", "set SubMask");
            ceTable.Add("读取SubMask", "read SubMask");
            ceTable.Add("设置GateWay", "set GateWay");
            ceTable.Add("读取GateWay", "read GateWay");

            ceTable.Add("设置接收阈值", "set threshold");
            ceTable.Add("读取接收阈值", "read threshold");
            ceTable.Add("设置强度系数", "set coefficient");
            ceTable.Add("读取强度系数", "read coefficient");
            
            ceTable.Add("取值范围:0~65535", "Range of values: 0~65535");
            ceTable.Add("取值范围:1~255", "Range of values:1~255");
            ceTable.Add("取值范围:0~255", "Range of values:0~255");
            ceTable.Add("取值范围:0.01~2.55", "Range of values: 0.01~2.55");
            //public static String quZhiFanWei = "取值范围：1~255";
            //public static String quZhiFanWei2 = "取值范围：0.01~2.55";
            ceTable.Add("值越小功率越大", "The smaller the value, the greater the power");
        }
          

        /// <summary>
        /// listView的操作
        /// </summary>
        /// <param name="listView"></param>
        public static void setLvLanguage(ListView listView)
        {
            foreach (ColumnHeader ch in listView.Columns)
            {
                string text = ch.Text;
                if (text == null) continue;
                if (!ceTable.ContainsKey(text)) continue;
                ch.Text = (string)ceTable[text];
            }
        }


        public static void setComboBox(ComboBox comBox) { 
            if(comBox.Items.Count <= 0)return;
            int count = comBox.Items.Count;
            for (int i = 0; i < count;i++ )
            {
                if (!(comBox.Items[i] is string)) continue;
                string text = (string)comBox.Items[i];
                if (text == null) continue;
                if (!ceTable.ContainsKey(text)) continue;
                comBox.Items[i] = (string)ceTable[text];
            }
        }

        /// <summary>
        /// 遍历窗体控件，并且操作所有控件的Text
        /// </summary>
        /// <param name="parent"></param>
        public static void PrintCtrlName(Control parent)
        {
            if (lau == Language.chinese) return;
            foreach (Control ctrl in parent.Controls)
            {
                //遍历所有ListView...
                if (ctrl is ListView){
                    ListView t = (ListView)ctrl;
                    setLvLanguage(t);
                }
                if (ctrl is ComboBox) {
                    ComboBox comBox = (ComboBox)ctrl;
                    setComboBox(comBox);
                }
                string text = ctrl.Text;
                if (text != null && text.Length > 0 && ceTable.ContainsKey(text))
                {
                    ctrl.Text = (string)ceTable[text];
                }
                if (ctrl.Controls.Count > 0)
                {
                    PrintCtrlName(ctrl);
                }
            }
        }

    }
}
