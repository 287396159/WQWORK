using CiXinLocation.bean;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CiXinLocation.Utils
{
    class HexFileRead {

        public delegate void sendDataEventHandler(HexFileBean hfInfor, int type);
        public sendDataEventHandler sendDataEventHandle;    //按类型发送消息

        HexFileBean hfBean;
       
        private byte[] timeVersion = new byte[] { 0, 0, 0, 0 };
        private byte[] typeByte = new byte[] { 0, 0, 0, 0 };


        void showMsg(string msg) {
            sendDataFrom(msg, 0xff);
        }

        void sendDataFrom(string msg, int type)
        {
            if (null == sendDataEventHandle) return;            
            hfBean.FileInfor = msg;
            sendDataEventHandle(hfBean, type);
        }


        public bool readHexFile(string g_hexfilepath)
        {
            hfBean = new HexFileBean();
            hfBean.Path = g_hexfilepath;

            if (null == g_hexfilepath || g_hexfilepath.Length < 1)
            {
                showMsg(StringUtils.hexFileAddErr);
                return false;
            }
            try {
                FileStream file = new FileStream(g_hexfilepath, FileMode.Open);
                int hexfilesize = (int)(file.Length);
                int g_binsize = (hexfilesize / 1024 + 3) * 1024;
                return readHexFile(file, hexfilesize, g_binsize);
            } catch { }
            return false;
        }


        public bool readHexFile(FileStream file, int hexfilesize, int g_binsize) {
            byte[] g_bin = null;
            UInt32 g_bin_len = 0;
            UInt32 g_bin_offset = 0;

            //开始解析hex文件
            if (file == null) return false;
            g_bin_len = 0;
            UInt32 baseaddr = 0;
            UInt32 dataaddr = 0;

            bool first_data = true;
            try {                
                byte[] hexbuf = new byte[hexfilesize];
                file.Seek(0, SeekOrigin.Begin);
                file.Read(hexbuf, 0, hexfilesize);
                file.Close();                
                g_bin = Enumerable.Repeat((byte)0xff, g_binsize).ToArray();//创建数据并且赋初值0xff//new byte[g_binsize];
                //Array.Clear(g_bin, 0, g_binsize);

                byte[] oneline = new byte[128];
                byte[] binbuf = new byte[128];
                UInt32 len;
                int hexindex = 0;
                g_bin_len = 0;
                g_bin_offset = 0;
                while (true)
                {
                    //读取一行
                    len = 0;
                    while (len < oneline.Length)
                    {
                        oneline[len++] = hexbuf[hexindex++];
                        if (oneline[len - 1] == 0x0A)
                        {
                            break;
                        }
                    }
                    if (len >= oneline.Length)
                    {
                        showMsg(StringUtils.hexFileToLong);
                        return false;
                    }
                    if (oneline[0] != ':' || oneline[len - 2] != 0x0D || oneline[len - 1] != 0x0A)
                    {
                        showMsg(StringUtils.hexFileStringErr);
                        return false;
                    }

                    //把文本文件转换成十六进制数据
                    int bl = 0;
                    for (int i = 1; i < len - 2; i++)
                    {
                        if (oneline[i] >= '0' && oneline[i] <= '9')
                            binbuf[bl] = (byte)((oneline[i] - '0') << 4);
                        else if (oneline[i] >= 'a' && oneline[i] <= 'f')
                            binbuf[bl] = (byte)((oneline[i] - 'a' + 10) << 4);
                        else if (oneline[i] >= 'A' && oneline[i] <= 'F')
                            binbuf[bl] = (byte)((oneline[i] - 'A' + 10) << 4);
                        else
                        {
                            showMsg(StringUtils.hexFileStrNoHex);
                            return false;
                        }
                        i++;
                        if (oneline[i] >= '0' && oneline[i] <= '9')
                            binbuf[bl] += (byte)((oneline[i] - '0'));
                        else if (oneline[i] >= 'a' && oneline[i] <= 'f')
                            binbuf[bl] += (byte)((oneline[i] - 'a' + 10));
                        else if (oneline[i] >= 'A' && oneline[i] <= 'F')
                            binbuf[bl] += (byte)((oneline[i] - 'A' + 10));
                        else
                        {
                            showMsg(StringUtils.hexFileStrNoHex);
                            return false;
                        }
                        bl++;
                    }
                    /*
                     *  第一个字节 0x10表示本行数据的长度；
                        第二、三字节 0x00 0x08表示本行数据的起始地址；
                        第四字节 0x00表示数据类型，数据类型有：0x00、0x01、0x02、0x03、0x04、0x05。
                        '00' Data Rrecord：用来记录数据，HEX文件的大部分记录都是数据记录
                        '01' End of File Record: 用来标识文件结束，放在文件的最后，标识HEX文件的结尾
                        '02' Extended Segment Address Record: 用来标识扩展段地址的记录
                        '03' Start Segment Address Record:开始段地址记录
                        '04' Extended Linear Address Record: 用来标识扩展线性地址的记录
                        '05' Start Linear Address Record:开始线性地址记录
                        然后是数据，最后一个字节 0x54为校验和。
                        校验和的算法为：计算0x54前所有16进制码的累加和(不计进位)，检验和 = 0x100 - 累加和
                     */
                    //判断十六进制数据的校验和
                    byte cs = 0;
                    for (int i = 0; i < bl - 1; i++)
                        cs += binbuf[i];
                    cs = (byte)(0x100 - cs);
                    if (cs != binbuf[bl - 1])
                    {
                        showMsg(StringUtils.hexFileCheckFail);
                        return false;
                    }
                    //判断长度
                    if (bl != binbuf[0] + 1 + 2 + 1 + 1)
                    {
                        showMsg(StringUtils.hexFileHangLenErr);
                        return false;
                    }
                    getHexVersionAndDay(binbuf);
                    //判断数据类型
                    if (binbuf[3] == 0x00)
                    {
                        //这是数据
                        dataaddr = baseaddr + (UInt32)((binbuf[1] << 8) | (binbuf[2]));
                        if (first_data)
                        {
                            first_data = false;
                            g_bin_offset = dataaddr;
                        }
                        for (int i = 0; i < binbuf[0]; i++)
                        {
                            if (dataaddr >= g_binsize)
                            {
                                return false;
                            }
                            g_bin[dataaddr] = binbuf[4 + i];
                            dataaddr++;
                        }
                    }
                    else if (binbuf[3] == 0x01)
                    {
                        //文件结尾
                        break;
                    }
                    else if (binbuf[3] == 0x04)
                    {
                        //这是在说明数据的基地址
                        //    baseaddr = (uint)((((binbuf[4] << 8) | (binbuf[5])) << 16));   //
                        baseaddr = (uint)((((0 << 8) | (binbuf[5])) << 16));
                    }
                    else if (binbuf[3] == 0x05)
                    {
                        //这里通常为4个Byte的数据:040000052000044192
                        //其数据可以理解为：0x20000441，这个是main函数的入口地址
                        //可以不管
                    }
                    else
                    {
                        showMsg(StringUtils.hexFileTypeErr);
                        return false;
                    }
                }
                //文件解析完毕
                g_bin_len = dataaddr;

                //把后面多余的全是0xFF的去掉
                len = 0;
                UInt32 j;
                for (len = 0; len < g_bin_len; len++)
                {
                    if (g_bin[len] == 0xFF)
                    {
                        for (j = len + 1; j < g_bin_len; j++)
                        {
                            if (g_bin[j] != 0xFF)
                            {
                                len = j;
                                break;
                            }
                        }
                        if (j == g_bin_len)
                            break;
                    }
                }
                g_bin_len = ((len / 64) + 1) * 64;

                g_bin_len = (g_bin_len - g_bin_offset);
                for (int i = 0; i < g_bin_len; i++)
                    g_bin[i] = g_bin[g_bin_offset + i];

                string path = hfBean.Path;
                hfBean = new HexFileBean(path, g_bin_len, g_bin);
                hfBean.TimeVersion = timeVersion;
                hfBean.TypeByte = typeByte;

                showMsg(StringUtils.hexFileReadSuccess + g_bin_len);
                return true;
            }
            catch (Exception ee) {
                Console.WriteLine(ee.ToString());
                return false;
            }
        }


        void getHexVersionAndDay(byte[] binbuf)
        {
            int length = binbuf[0];
            int address = binbuf[1] * 256 + binbuf[2];
            for (int i = 0; i < 4; i++)
            {
                int versionIndex = getAddByte(address, (0x4800) + i+4, length);
                if (-1 != versionIndex) {
                    timeVersion[i] = binbuf[versionIndex + 4];
                }
            }
            //typeByte
            for (int i = 0; i < 4; i++)
            {
                int versionIndex = getAddByte(address, (0x4800) + i, length);
                if (-1 != versionIndex)
                {
                    typeByte[i] = binbuf[versionIndex + 4];
                }
            }//for
        }

        bool typeByteChange()  //-Z(CODE)IMAGE_MARK=0x4800-0x4803       //-Z(CODE)IMAGE_VERSION=0x4804-0x4807
        {
            for (int i = 1; i < typeByte.Length; i++)
            {
                if (typeByte[i] - typeByte[0] != 0x80)
                {
                    typeByte[0] = 4;
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 寻找hex文件中的某一项
        /// </summary>
        /// <param name="addRess">hex读取的位置</param>
        /// <param name="length">长度 //第一版是4800，第二版现在位置A004,//第三版又成为4800</param>
        /// <returns></returns>
        private int getAddByte(Int32 address, int location, int length)
        {
            if (address <= location && address + length > location)
            {
                int ca = location - address;
                return ca;
            }
            return -1;
        }


    }
}
