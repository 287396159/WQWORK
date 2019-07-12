using CiXinLocation.bean;
using CiXinLocation.Utils;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiXinLocation.Model
{
    /// <summary>
    /// 文件读取后数据格式为：data+id+version+type = 4+2+4+4 共14个字节
    /// </summary>
    class DmatekFileModel
    {

        private ConfigFileUtils configUtils;
        private List<ID_Version> mID_Versions;

        /// <summary>
        /// 文件存储中，每个基础包的长度
        /// </summary>
        private int IDLENGTH = 14;

        public DmatekFileModel() { 
            configUtils = new ConfigFileUtils();
            configUtils.setCurrentApplicationPatn();
            configUtils.getFile(configUtils.CurrentPath, "cixin.tmetk");
            if (mID_Versions == null) mID_Versions = new List<ID_Version>();
            dmatekRead();
        }


        public ID_Version getVersion(DrivaceType dtype, byte[] ID)
        { //通过ID查找 文件中存储的对应设备类型的ID
            if (ID == null) return null;
            foreach (ID_Version idVersion in mID_Versions)
            {
                if (dtype != idVersion.typeDrivaceType) continue;
                if (idVersion.Id[0] == ID[0] && idVersion.Id[1] == ID[1]) return idVersion;
            }
            return null;
        }


        public void writeVersion(DrivaceType dtype, byte[] ID, byte[] version, byte type, byte channel)
        {
            if (ID == null || version == null) return;
            foreach (ID_Version idVersion in mID_Versions){
                if (dtype != idVersion.typeDrivaceType) continue;
                if (XWUtils.byteBTBettow(ID, idVersion.Id, ID.Length))
                    if (XWUtils.byteBTBettow(version, idVersion.Version, version.Length)) return;
                    else {
                        idVersion.Version = version;
                        idVersion.Type[0] = type;
                        idVersion.Channel = channel;
                        dmatekWrite();
                        return;
                    }
            }
            if (version.Length != 4) return;
            ID_Version newIdVersion = new ID_Version(ID);
            newIdVersion.typeDrivaceType = dtype;
            newIdVersion.Version = version;
            newIdVersion.Type[0] = type;
            newIdVersion.Channel = channel;
            mID_Versions.Add(newIdVersion);
            dmatekWrite();
        }

        private void dmatekWrite() {
            clear();
            foreach (ID_Version idVersion in mID_Versions)
            {
                dmatekWrite(idVersion);
            }
        }

        /// <summary>
        /// 写数据到文件中
        /// </summary>
        /// <param name="writeData"> 写入的数据 </param>
        public void dmatekWrite(ID_Version writeID_Version)
        {
            if (writeID_Version == null || writeID_Version.Version == null) return;
            //byte[] id_data = new byte[IDLENGTH];
            if (mID_Versions == null) return;
            configUtils.addDataInFile(writeID_Version.toArray());
        }

        private void clear() {
            if (configUtils != null) configUtils.clearData();
        }

        public void dmatekRead() { 
            byte[] mData = configUtils.getDataFromFile();
            if (mData.Length % IDLENGTH != 0) return ;
            int id_count = mData.Length / IDLENGTH;
            for (int i = 0; i < id_count;i++ ) {
                byte[] data = new byte[IDLENGTH];
                Array.Copy(mData, i * IDLENGTH, data, 0, IDLENGTH);
                ID_Version idversion = getID_VersionFromIDdata(data);
                if(idversion != null) mID_Versions.Add(idversion);
            }
        }


        private ID_Version getID_VersionFromIDdata(byte[] id_data)
        {
            if (id_data == null || id_data.Length != IDLENGTH) return null;
            if (id_data[0] != 'd' && id_data[1] != 'a' && id_data[2] != 't' && id_data[3] != 'a') return null;
            byte[] id = new byte[2];
            byte[] version_dt = new byte[4];
            byte[] type_dt = new byte[4];
            
            Array.Copy(id_data,4,id,0,id.Length);
            Array.Copy(id_data, 4+id.Length, version_dt, 0, version_dt.Length);
            Array.Copy(id_data, 4 + id.Length + version_dt.Length, type_dt, 0, type_dt.Length);

            ID_Version id_version = new ID_Version(type_dt, id);
            id_version.Version = version_dt;
            return id_version;
        }


        public void closeModel() {
            mID_Versions.Clear();
        }

    }
}

