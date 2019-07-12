using CiXinLocation.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CiXinLocation.bean
{
    class ID_Version
    {
        private DrivaceType drivaceType;

        private byte[] id;
        private byte[] version;
        private byte[] type;
        private byte channel;       

        public ID_Version( byte[] ID)
        {
            id = ID;
        }

        public ID_Version(byte[] type, byte[] ID)
        {
            this.type = type;
            id = ID;
        }

        public byte[] Id 
        {
            get { return id; }
            set { }
        }

        public byte Channel
        {
            get { return channel; }
            set { channel = value; }
        }

        public byte[] Version 
        {
            get { return version; }
            set { version = value; }
        }

        public DrivaceType DrivaceType
        {
            get { return drivaceType; }
            set { drivaceType = value; }
        }

        public byte[] Type
        {
            get { return type; }
            set { }
        }

        /// <summary>
        /// 主要的设备类型，参考点，节点，卡片
        /// </summary>
        public byte mainTypeByte
        {
            get
            {
                if (type == null) return 0;
                return type[1];
            }
        }


        /// <summary>
        /// 主要从存储的Type中读取DrivaceType
        /// </summary>
        public DrivaceType typeDrivaceType
        {
            get {
                if (mainTypeByte == 0x03) return DrivaceType.TAG;
                else if (mainTypeByte == 0x02) return DrivaceType.LOCATION;
                else if (mainTypeByte == 0x01) return DrivaceType.NODE;
                else if (mainTypeByte == 0x04) return DrivaceType.USB_DANGLE;
                else return DrivaceType.NODRIVACE;
            }
            set {
                if (Type == null) type = new byte[4];
                if(value == DrivaceType.TAG) type[1] = 3;
                if(value == DrivaceType.LOCATION)type[1] = 2;
                if(value == DrivaceType.NODE)type[1] = 1;
                if (value == DrivaceType.USB_DANGLE) type[1] = 4;
            }
        }


        public string IDtostring() {
            return Id[0].ToString("X2") + Id[1].ToString("X2");
        }

        public bool idEqulse(ID_Version id_version) {
            if (id_version == null) return false;
            if (Id == null || id_version.Id == null) return false;
            if (Id.Length != 2 || id_version.Id.Length != 2) return false;
            if (typeDrivaceType != id_version.typeDrivaceType) return false;//|| Type[0] != id_version.Type[0]
            //if (Type != id_version.Type) return false;
            //在设备类型一样的情况下，设备ID相等，才相等
            if (Id[0] == id_version.Id[0] && Id[1] == id_version.Id[1]) return true;
            return false;
        }


        public ListViewItem getlvItem(){
            if (Id == null || Id.Length != 2) return null;
            ListViewItem lvItem = new ListViewItem();
            lvItem.SubItems[0].Text = IDtostring();
            lvItem.SubItems.Add(getVersion());//getVersion().ToString("X2")
            lvItem.SubItems.Add("");//getVersion().ToString("X2")
            return lvItem;
        }


        public string getVersion(){
            return CiXinUtils.getVersion(Version);
        }

        public string getSunType
        {
            get{
                if (Type == null) return "";
                return CiXinUtils.getSunTypeName(Type[1], Type[0]);
            }           
        }

        public byte[] toArray() {
            if (type == null) return null;
            int length_idversion = id.Length + Version.Length + type.Length;
            byte[] ID_versionByte = new byte[length_idversion];
            Array.Copy(Id, 0, ID_versionByte, 0, Id.Length);
            Array.Copy(Version, 0, ID_versionByte,Id.Length,Version.Length);
            Array.Copy(Type, 0, ID_versionByte, Id.Length + Version.Length, type.Length);
            return ID_versionByte;
        }

    }
}
