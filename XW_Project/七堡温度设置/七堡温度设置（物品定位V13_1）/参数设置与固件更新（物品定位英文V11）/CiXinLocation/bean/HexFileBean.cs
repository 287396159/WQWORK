using CiXinLocation.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CiXinLocation.bean
{
    public class HexFileBean
    {
        private string path;
        private UInt32 fileLength;
        private byte[] g_bin = null;
        private string fileInfor;//Hex文件解析的消息
        private byte[] timeVersion ;
        private byte[] typeByte ;


        public HexFileBean() { }

        public HexFileBean(string path) {
            this.path = path;
        }


        public HexFileBean(string path, UInt32 fileLength, byte[] g_bin) {
            this.path = path;
            this.fileLength = fileLength;
            this.g_bin = g_bin;
        }

        public byte[] TimeVersion {
            get { return timeVersion;}
            set {                 
                timeVersion =value;
                Array.Reverse(timeVersion);
            }
        }

        public byte[] TypeByte
        {
            get { return typeByte; }
            set { typeByte =value; }
        }

        /// <summary>
        /// 主要的设备类型，参考点，节点，卡片
        /// </summary>
        public byte mainTypeByte
        {
            get {
                if (typeByte == null) return 0;
                return typeByte[1]; }
        }

        /// <summary>
        /// 主要的设备类型，参考点，节点
        /// </summary>
        public DrivaceType mainDrivaceType
        {
            get {
                if (mainTypeByte == 0x04) return DrivaceType.USB_DANGLE;
                if (mainTypeByte == 0x03) return DrivaceType.TAG;
                else if (mainTypeByte == 0x02) return DrivaceType.LOCATION;
                else if (mainTypeByte == 0x01) return DrivaceType.NODE;
                else return DrivaceType.NODRIVACE;
            }
        }


        public byte Index
        {
            get
            {
                if (typeByte == null || typeByte.Length < 2) return 0;
                return typeByte[0];
            }
        }

        /// <summary>
        /// hex文件地址和文件名
        /// </summary>
        public string Path { //Hex文件地址
            get { return path; }
            set { path = value; }
        }

        /// <summary>
        /// Hex文件名
        /// </summary>
        public string FileName
        { //Hex文件地址
            get {
                string name = "";
                try { 
                    name = System.IO.Path.GetFileNameWithoutExtension(Path);
                }
                catch {
                    name = System.IO.Path.GetFileName(Path);
                }
                return name;
            }
        }

        /// <summary>
        /// HEX文件地址，没有文件名。若出错或者对象为空，返回“”字符串
        /// </summary>
        public string FilePath {
            get
            {
                string name = "";
                try
                {
                    name = System.IO.Path.GetDirectoryName(Path);
                }catch {}
                return name;
            }
        }


        public string FileInfor
        {
            get { return fileInfor; }
            set { fileInfor = value; }
        }

        public UInt32 FileLength {//Hex文件的长度
            get { return fileLength; }
        }

        public byte[] G_bin {//Hex文件的bin数据
            get { return g_bin; }
        }

        public string VersionMessage {            
            get {
                return CiXinUtils.VersionMessage(timeVersion, typeByte);  
            }
        }

        private string getTypeName(byte mainType,byte type)
        {
            return CiXinUtils.getTypeName(mainType, type);
        }

        public string LVItem0Text {
            get { return FileName + "(" + FilePath + ")"; }
        }

        public ListViewItem getlvItem() {
            if (Path == null || Path.Length < 2) return null;
            ListViewItem lvItem = new ListViewItem();
            lvItem.SubItems[0].Text = LVItem0Text;
            if (mainDrivaceType == DrivaceType.TAG) lvItem.SubItems.Add("0%");
            return lvItem;
        }

    }
}
