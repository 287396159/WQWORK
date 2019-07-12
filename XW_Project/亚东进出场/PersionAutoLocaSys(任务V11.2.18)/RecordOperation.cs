using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PersionAutoLocaSys
{
        //原始记录资料操作
        class RecordOperation
        {
            private static Object obj_lock = new object();
            //保存记录文件的数据流
            private static FileStream savefstr = null;
            //保存记录文件的路径
            private static string recordpath = Environment.CurrentDirectory + @"\" + "Record";
            private const byte tagpacklen = 23;
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
                        //在重新开启一个流前，需要把前面的一个流文件关闭掉
                        //否则可能存在下一次访问前面一个文件时报错“文件流被占用...”
                        try
                        {
                            if(null != savefstr)
                                savefstr.Close();
                        }
                        catch (Exception)
                        {
                        }
                        finally
                        {
                            savefstr = null;
                        }
                        savefstr = File.Create(savefpath);
                        GC.Collect();
                    }
                }
                if (null != savefstr)
                {//判断当前流是否正确
                    if (!savefpath.Equals(savefstr.Name))
                    {
                        lock (obj_lock)
                        {
                            //关闭前面一个文件流
                            try
                            {
                                savefstr.Close();
                            }
                            catch (Exception)
                            {
                            }
                            finally 
                            {
                                savefstr = null;
                            }
                            try
                            {
                                savefstr = File.Open(savefpath, FileMode.Append, FileAccess.Write);
                                GC.Collect();
                            }catch(Exception ex)
                            {
                                FileOperation.WriteLog("重新打開文件失敗!失败原因:"+ex.ToString());
                            }
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
                        if (null == savefstr) return;
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
            public static void GetRecord(DateTime startdt,DateTime enddt,string strtagid,string strfile, ref List<TagPack> tags)
            {
                string strcurid = "";
                DateTime curdatetime;
                lock (obj_lock)
                {
                    savefstr = new FileStream(strfile, FileMode.Open, FileAccess.Read);
                    byte[] bytes = new byte[tagpacklen];
                    TagPack tpk = null;
                    int place = 1;

                    int curlen = savefstr.Read(bytes, 0, bytes.Length);
                    //每次读取的字节数必须为tagpacklen长度
                    while (curlen >= tagpacklen)
                    {   //0xfd(1byte)+0x01(1byte)+ID(3byte)+Type(1byte)+RD(3byte)+Sig(1byte)+Batt(1byte)+NoExe(2byte)+Sleep(2byte)+ReportDT(7byte)+index(1byte)+cs(1byte)+0xfc(1byte)
                        strcurid = bytes[2].ToString("X2") + bytes[3].ToString("X2");
                        curdatetime = new DateTime((bytes[13] << 8 | bytes[14]), bytes[15], bytes[16], bytes[17], bytes[18], bytes[19]);
                        if (strcurid.Equals(strtagid) && DateTime.Compare(startdt, curdatetime) < 0
                            && DateTime.Compare(enddt,curdatetime) > 0)
                        {
                            tpk = new TagPack();
                            tpk.TD[0] = bytes[2];
                            tpk.TD[1] = bytes[3];

                            tpk.isAlarm = bytes[4];
                            tpk.RD_New[0] = bytes[5];
                            tpk.RD_New[1] = bytes[6];
                            tpk.SigStren = bytes[7];
                            tpk.Bat = bytes[8];
                            tpk.ResTime = (int)(bytes[9] << 8 | bytes[10]);
                            tpk.Sleep = (int)(bytes[11] << 8 | bytes[12]);
                            tpk.ReportTime = new DateTime((bytes[13] << 8 | bytes[14]), bytes[15], bytes[16], bytes[17], bytes[18], bytes[19]);
                            tpk.index = bytes[20];
                            tags.Add(tpk);
                        }
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
            public static byte[] ParseTagPack(TagPack tpk)
            {
                //数据包格式:0xfd(1byte)+0x01(1byte)+ID(2byte)+Type(1byte)+RD(2byte)+Sig(1byte)+Batt(1byte)+NoExe(2byte)+Sleep(2byte)+ReportDT(7byte)+index(1byte)+cs(1byte)+0xfc(1byte)
                byte[] bytes = new byte[tagpacklen];
                bytes[0] = 0xfd;
                bytes[1] = 0x01;
                System.Buffer.BlockCopy(tpk.TD, 0, bytes, 2, 2);
                bytes[4] = tpk.isAlarm;
                System.Buffer.BlockCopy(tpk.RD_New, 0, bytes, 5, 2);
                bytes[7] = tpk.SigStren;
                bytes[8] = tpk.Bat;
                bytes[9] = (byte)(tpk.ResTime >> 8);
                bytes[10] = (byte)(tpk.ResTime);
                bytes[11] = (byte)(tpk.Sleep >> 8);
                bytes[12] = (byte)(tpk.Sleep);
                //年(2byte)+月(1byte)+日(1byte)+时(1byte)+分(1byte)+秒(1byte)
                bytes[13] = (byte)(tpk.ReportTime.Year >> 8);
                bytes[14] = (byte)(tpk.ReportTime.Year);
                bytes[15] = (byte)tpk.ReportTime.Month;
                bytes[16] = (byte)tpk.ReportTime.Day;
                bytes[17] = (byte)tpk.ReportTime.Hour;
                bytes[18] = (byte)tpk.ReportTime.Minute;
                bytes[19] = (byte)tpk.ReportTime.Second;
                bytes[20] = tpk.index;
                bytes[21] = 0;
                for (int i = 0; i < 21; i++)
                    bytes[21] += bytes[i];
                bytes[22] = 0xfc;
                return bytes;
            }
    }
}
