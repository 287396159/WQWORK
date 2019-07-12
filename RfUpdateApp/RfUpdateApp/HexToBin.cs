using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RfUpdateApp
{
    public enum ConvertStatus : int
    {
        ConvertOk,          //转换成功
        DataToLong,         //行数据长度过长
        DataToShort,        //行数据长度过短
        DataNoColon,        //行数据没有冒号
        DataTypeError,      //行数据类型错误
        DataLengthError,    //行数据长度与标记的不匹配
        DataCheckError,     //CRC校验和错误
        HexNoEnd            //文件没有结束符
    }

    public class HexToBin
    {
        private const int LineHexDataMaxLength = 256;
        private const int LineHexDataMinLength = 11;


        public byte LineLength
        {
            get;
            set;
        }

        public byte LineType
        {
            get;
            set;
        }

        public UInt16 FirstAddr
        {
            get;
            set;
        }

        public UInt16  LineStartAddr
        {
            get;
            set;
        }
        //用于保存移位后的扩展段地址或扩展性地址 即基地址  
        public UInt32 BaseAddr
        {
            get;
            set;
        }

        public byte[] LineBinData
        {
            get;
            set;
        }

        public HexToBin()
        {
            this.LineBinData = new byte[256];
            this.LineLength = 0;
            this.LineStartAddr = 0;
            this.LineType = 0;
            this.BaseAddr = 0;
            this.FirstAddr = 0xFFFF;
        }
        /// <summary>
        /// 每行字符形式的hex数据转换为hex
        /// </summary>
        /// <param name="strLineHexData"></param>
        /// <returns></returns>
        public ConvertStatus CovertLineHexToBin(string strLineHexData)
        {
            //行数据过长
            if (strLineHexData.Length > LineHexDataMaxLength)
                return ConvertStatus.DataToLong;
            //行数据过短
            if (strLineHexData.Length < LineHexDataMinLength)
                return ConvertStatus.DataToShort;
            //第一个字符不是冒号
            if (strLineHexData[0] != ':')
                return ConvertStatus.DataNoColon;
            //行数据的长度应该为奇数
            if (strLineHexData.Length % 2 == 0)
                return ConvertStatus.DataLengthError;

            //bin行数据长度
            this.LineLength  = Convert.ToByte(strLineHexData.Substring(1, 2), 16);
            //bin行数据储存起始地址
            this.LineStartAddr = (UInt16)((UInt16)(Convert.ToByte(strLineHexData.Substring(3, 2), 16)) * 256 + (UInt16)(Convert.ToByte(strLineHexData.Substring(5, 2), 16)));
            //bin行数据类型
            this.LineType = Convert.ToByte(strLineHexData.Substring(7, 2), 16);
            //将hex数据转换为bin数据
            int j = 0;
            for (int i = 0; i < strLineHexData.Length - 9;)
            {
                this.LineBinData[j] = Convert.ToByte(strLineHexData.Substring(9 + i, 2), 16);
                i += 2;
                j++;
            }
            //标识的长度和实际的不一样 还有最后一个校验字节
            if (j != this.LineLength + 1)
                return ConvertStatus.DataLengthError;

            //数据校验检查 0x01 + 取反（其他数据）= 最后一个字节的校验位
            byte cs = 0; int k = 0;
            for (k = 0; k < (strLineHexData.Length - 3) / 2; k++)   //除去校验位和开始的冒号
            {
                cs += Convert.ToByte(strLineHexData.Substring(1 + 2 * k, 2), 16);
            }
            byte dataCs = Convert.ToByte(strLineHexData.Substring(1 + 2 * k, 2), 16);
            cs = (byte)~cs;
            cs += 0x01;
            if(cs != dataCs)
            {
                return ConvertStatus.DataCheckError;
            }

            return ConvertStatus.ConvertOk;
        }
        /// <summary>
        /// HEX转换为bin
        /// </summary>
        /// <param name="strHexFile"></hex所有字符串>
        /// <param name="binFile"></存放的bin数组>
        /// <param name="binSize"></存放的bin文件的大小>
        /// <returns></returns>
        public ConvertStatus HexFileToBinFile(string strHexFile, ref byte[] binFile, ref UInt32 binSize)
        {
            UInt32 addrStart = 0;
            UInt32 addrBase = 0;

         
            for (int i = 0; i < binFile.Length; i++)
            {
                binFile[i] = 0xFF;
            }
            
            //将其分割为一行一行的数据。
            string[] strLineData = strHexFile.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < strLineData.Length; i++)
            {
                ConvertStatus res = CovertLineHexToBin(strLineData[i]);
                if (res != ConvertStatus.ConvertOk)
                {
                    return res;
                }
                else
                {
                    switch (this.LineType)
                    { 
                        //数据记录
                        case 0:
                            if (this.FirstAddr == 0xFFFF)
                            {
                                this.FirstAddr = this.LineStartAddr;
                            }
                            addrStart = (UInt32)(addrBase + this.LineStartAddr - this.FirstAddr);    //每行的起始地址 需要加上基地址
                            Console.WriteLine(addrStart);
                            for (int j = 0; j < this.LineLength; j++)
                            {
                                binFile[j + addrStart] = this.LineBinData[j];  
                            }
                            binSize = (UInt32)(this.LineLength + addrStart);
                            Console.WriteLine("binSize"+binSize);
                            break;
                        //数据结束
                        case 1:
                            Console.WriteLine("binSizeOKOK" + binSize);
                            return ConvertStatus.ConvertOk;
                        //扩展段地址记录
                        case 2:
                            if (this.LineLength != 2)                                   //扩展地址只有两个字节
                                return ConvertStatus.DataTypeError;
                            if (i == 0)  //首次仅仅记录基地址
                            {
                                this.BaseAddr = ((UInt32)(((UInt32)this.LineBinData[0] << 8) + (UInt32)this.LineBinData[1]) << 4);
                            }
                            else
                            {
                                if (((((UInt32)this.LineBinData[0] << 8) + (UInt32)this.LineBinData[1]) << 4) < this.BaseAddr)
                                    return ConvertStatus.DataTypeError;
                                addrBase = (((((UInt32)this.LineBinData[0] << 8) + (UInt32)this.LineBinData[1]) << 4) - this.BaseAddr);
                            }
                            break;
                        //开始段地址记录
                        case 3:
                            break;
                        //扩展线性地址记录
                        case 4:
                            if (this.LineLength != 2) 
                                return ConvertStatus.DataTypeError;
                            if (i == 0)  //首次仅仅记录基地址
                            {
                                this.BaseAddr = ((UInt32)(((UInt32)this.LineBinData[0] << 8) + (UInt32)this.LineBinData[1]) << 16);
                            }
                            else
                            {
                                if (((UInt32)(((UInt32)this.LineBinData[0] << 8) + (UInt32)this.LineBinData[1]) << 16) < this.BaseAddr)
                                    return ConvertStatus.DataTypeError;
                                addrBase = (((((UInt32)this.LineBinData[0] << 8) + (UInt32)this.LineBinData[1]) << 16) - this.BaseAddr);
                            }
                            break;
                        //开始线性地址记录
                        case 5:
                            break;
                        default:
                            return ConvertStatus.DataTypeError;
                    }
                }
            }

            return ConvertStatus.HexNoEnd;
        }
    }

    
}
