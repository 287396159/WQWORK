using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PrecisePosition
{
    class RecordOperation
    {
        private static Object obj_lock = new object();
        //保存记录文件的数据流
        private static FileStream savefstr = null;
        //保存记录文件的路径
        private static string recordpath = Environment.CurrentDirectory + @"\" + "Record";
        private const byte tagpacklen = 23;
        //数据保存协议
        class TTPro
        { //0xfd(1byte)+0x01(1byte)+ID(3byte)+Type(1byte)+RD(3byte)+Sig(1byte)+Batt(1byte)+NoExe(2byte)+Sleep(2byte)+ReportDT(7byte)+index(1byte)+cs(1byte)+0xfc(1byte)
            public static byte Pro_head = 0xfd;//包头
            public static byte Pro_end = 0xfc;//包尾
            public static byte Pro_type = 0x01;//包类型
            public static int ID_index = 2;
        }
        //开启文件流
        private static void Open(string dirname, string filename)
        {
            string savefpath = recordpath + @"\" + dirname + @"\" + filename;
            //判断记录目录是否存在
            if (!Directory.Exists(recordpath))
                Directory.CreateDirectory(recordpath);
            //判断里面的年+月+日目录是否存在
            if (!Directory.Exists(recordpath + @"\" + dirname))
                Directory.CreateDirectory(recordpath + @"\" + dirname);
            //判断里面的小时文件是否存在
            if (!File.Exists(savefpath))
            {
                lock (obj_lock)
                {
                    savefstr = File.Create(savefpath);
                }
            }
            if (null != savefstr)
            {//判断当前流是否正确
                if (!savefpath.Equals(savefstr.Name))
                {
                    lock (obj_lock)
                    {
                        savefstr = File.Open(savefpath, FileMode.Append, FileAccess.Write);
                    }
                }
            }
            else
            {
                lock (obj_lock)
                {
                    savefstr = new FileStream(recordpath + @"\" + dirname + @"\" + filename, FileMode.Append, FileAccess.Write);
                }
            }
        }
        //保存记录文件
        public static void SaveRecord(byte[] bytes)
        {
            //年+月+日
            string svdirname = (bytes[13] << 8 | bytes[14]).ToString().PadLeft(4, '0') + bytes[15].ToString().PadLeft(2, '0') + bytes[16].ToString().PadLeft(2, '0');
            //小时
            string svfname = bytes[17].ToString().PadLeft(2, '0') + ".dat";
            try
            {
                Open(svdirname, svfname);
                lock (obj_lock)
                {
                    savefstr.Write(bytes, 0, bytes.Length);
                    //保证每次能写进去，避免流缓存还没满而没有写进去
                    savefstr.Flush();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //关闭文件流
        public static void Close()
        {
            if (null != savefstr)
            {
                lock (obj_lock)
                {
                    savefstr.Close();
                    savefstr = null;
                }
            }
        }
        //得到文件
        public static void GetRecord(string strfile, ref List<CardImg> tags)
        {
            lock (obj_lock)
            {
                savefstr = new FileStream(strfile, FileMode.Open, FileAccess.Read);
                byte[] bytes = new byte[tagpacklen];
                CardImg tpk = null;
                int place = 1;

                int curlen = savefstr.Read(bytes, 0, bytes.Length);
                //每次读取的字节数必须为tagpacklen长度
                while (curlen >= tagpacklen)
                {   //0xfd(1byte)+0x01(1byte)+ID(3byte)+Type(1byte)+RD(3byte)+Sig(1byte)+Batt(1byte)+NoExe(2byte)+Sleep(2byte)+ReportDT(7byte)+index(1byte)+cs(1byte)+0xfc(1byte)
                    tpk = new CardImg();

                    tags.Add(tpk);
                    savefstr.Seek(tagpacklen * place, SeekOrigin.Begin);
                    curlen = savefstr.Read(bytes, 0, bytes.Length);
                    place++;
                }
                if (null != savefstr)
                {
                    savefstr.Close();
                }
                savefstr = null;
            }
        }
        //TagPack => bytes
        public static byte[] ParseTagPack(CardImg tpk)
        {
            //数据包格式:0xfd(1byte)+0x01(1byte)+ID(2byte)+Type(1byte)+RD(2byte)+Sig(1byte)+Batt(1byte)+NoExe(2byte)+Sleep(2byte)+ReportDT(7byte)+index(1byte)+cs(1byte)+0xfc(1byte)
            byte[] bytes = new byte[tagpacklen];
            bytes[0] = 0xfd;
            bytes[1] = 0x01;

            bytes[21] = 0;
            for (int i = 0; i < 21; i++)
                bytes[21] += bytes[i];
            bytes[22] = 0xfc;
            return bytes;
        }
    }
}
