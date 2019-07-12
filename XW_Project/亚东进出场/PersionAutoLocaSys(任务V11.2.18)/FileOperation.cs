using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Office.Core;
using Microsoft.Office.Interop;
using Microsoft.Office.Tools;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace PersionAutoLocaSys
{
    class FileOperation
    {
        private static Encoding encoder = Encoding.UTF8;
        static FileStream MyFileStream = null;
        public const int MaxLen = 1000;//数组大小
        public  const String ConfigFileName = "Config.ini";
        public  const String PortFileName = "Port.ini";
        public  const String AreaFileName = "Area.ini";
        public  const String GroupFileName = "Group.ini";
        public  const String SafeFileName = "Safe.ini";
        public  const String PersonFileName = "Person.ini";
        public  const String OtherFileName = "Other.ini";
        public const String TagFileName = "Tag.ini";
        public static String TagPath = Environment.CurrentDirectory + "\\" + TagFileName;
        public static String ConfigPath = Environment.CurrentDirectory + "\\" + ConfigFileName;
        public static String PortPath = Environment.CurrentDirectory + "\\" + PortFileName;
        public static String AreaPath = Environment.CurrentDirectory + "\\" + AreaFileName;
        public static String GroupPath = Environment.CurrentDirectory + "\\" + GroupFileName;
        public static String SafePath = Environment.CurrentDirectory + "\\" + SafeFileName;
        public static String PersonPath = Environment.CurrentDirectory + "\\" + PersonFileName;
        public static String OtherPath = Environment.CurrentDirectory + "\\" + OtherFileName;
        //加上一個備份路徑
        public static String BackUpDir = Environment.CurrentDirectory + "\\BackUp";
        public static String BackUpTagPath = Environment.CurrentDirectory + "\\BackUp\\" + TagFileName;
        public static String BackUpConfigPath = Environment.CurrentDirectory + "\\BackUp\\" + ConfigFileName;
        public static String BackUpPortPath = Environment.CurrentDirectory + "\\BackUp\\" + PortFileName;
        public static String BackUpAreaPath = Environment.CurrentDirectory + "\\BackUp\\" + AreaFileName;
        public static String BackUpGroupPath = Environment.CurrentDirectory + "\\BackUp\\" + GroupFileName;
        public static String BackUpSafePath = Environment.CurrentDirectory + "\\BackUp\\" + SafeFileName;
        public static String BackUpPersonPath = Environment.CurrentDirectory + "\\BackUp\\" + PersonFileName;
        public static String BackUpOtherPath = Environment.CurrentDirectory + "\\BackUp\\" + OtherFileName;

        public static String StrPersonOperSeg = "PERSONOPER";
        public static String StrOpertimeKey = "OPERATIONTIME";
        public static String GroupName = "NAME";
        public static String PortSeg = "PORT";
        public static String PortName = "NAME";
        public static String PortAreaID = "AREA";
        public static String AreaSeg = "AREA";
        public static String AreaID = "ID";
        public static String AreaName = "NAME";
        public static String AreaGroupID = "GROUPID";
        public static String AreaType = "TYPE";
        public static String AreaMapPath = "MAP";
        public static String AreaRouter = "ROUTERID";
        public static String AreaRouterName = "RNAME";
        public static String AreaRouterVisible = "RVISIBLE";
        public static String AreaRouterX = "RX";
        public static String AreaRouterY = "RY";
        public static String AreaNode = "NODEID";
        public static String AreaNodeName = "NNAME";
        public static String AreaNodeVisible = "NVISIBLE";
        public static String AreaNodeX = "NX";
        public static String AreaNodeY = "NY";
        public static String TagSeg = "TAG";
        public static String TagName = "NAME";
        public static String TagIsStopTime = "ISSTOPTIME";
        public static String TagStopTime = "STOPSECOND";

        public static String TagWorkTime = "WORKTIME";
        public static String TagStartTimeH = "STARTHOUR";
        public static string TagStartTimeM = "STARTMINUTE";
        public static String TagEndTimeH = "ENDHOUR";
        public static string TagEndTimeM = "ENDMINUTE";

        public static String TagGSWorkTime = "GSWK";
        public static String TagGSStartTimeH = "GSSTHOUR";
        public static string TagGSStartTimeM = "GSSTMINUTE";
        public static String TagGSEndTimeH = "GSEDHOUR";
        public static string TagGSEndTimeM = "GSEDMINUTE";
        public static String TagWorkStartTimeStamp = "StartTimeStamp";
        public static String TagWorkEndTimeStamp = "EndTimeStamp";
        public static String TagWorkID = "WORKID";

        public static String TagSimpleArea = "SIMPLE";
        public static String TagControlArea = "CONTROL";
        public static String TagDanglerArea = "DANGER";
        public const String NetSeg = "SERVER";
        public const String NetKey_IP = "IP";
        public const String NetKey_Port = "PORT";
        public const String MapSeg = "MAP";
        public const String MapKey_Path = "Path";
        public const String RealWidth = "RealWidth";
        public const String RealHeight = "RealHeight";
        public const String AlarmSeg = "ALARM";
        public const String AlarmPersonHelp = "PRSHELP";
        public const String AlarmEme = "EMERY";
        public const String AlarmAreaControl = "AREADMIN";
        public const String AlarmLowBattery = "LOWBATTERY";
        public const String AlarmStop = "STOP";
        public const String AlarmStopTime = "STOPTIME";
        public const String AlarmLowBatteryValue = "BATTERYVALUE";
        public const String AlarmNoControl = "NoControl";
        public const String AlarmNoControlCount = "CONTROLVALUE";
        public const String DeviceExceDis = "DEVICEEXCEPTION";
        public const String SysScanTimeTab = "SYSSCANTAB";
        public const String TagDisTimeParam1 = "TAGDISPARAM1";
        public const String TagDisTimeParam2 = "TAGDISPARAM2";
        public const String ReferDisTimeParam1 = "REFERDISPARAM1";
        public const String ReferDisTimeParam2 = "REFERDISPARAM2";
        public const String isSound = "ISSOUND";
        public const String SoundName = "SOUNDNAME";
        public const String SoundTime = "SOUNDTIME";
        public const String OptimizedParam = "OPTIMIZED";
        public const String Debug = "DEBUG";
        public const String DebugKey = "ISDEBUG";

        public const String AutoClearDevCnnAlarm = "AUTOCLEARCLEAR";

        public const String SettingArraoundDevices = "SETTINGOTHERDEVICES";
        public const String OptimizedMedol = "MEDOL";
        public const String OptimizedValue = "SIGTHRESHOLD";

        public static String BackUpMapPath = Environment.CurrentDirectory + "\\BackUp\\AreaIMG";
        public static String MapPath = Environment.CurrentDirectory + "\\AreaIMG";
        public static String SoundPath = Environment.CurrentDirectory + "\\Sound";

        //浏览视图中保存参考点
        public static String SavePortsPath = Environment.CurrentDirectory + "\\SavePorts.ini";
        public const String Loca_X = "X";
        public const String Loca_Y = "Y";
        public static String LogPath = Environment.CurrentDirectory + "\\Log";

        public static String Original = Environment.CurrentDirectory + "\\Record";
        public static String WarmMsg = Environment.CurrentDirectory + "\\WarmMsg";
        public static String WarmName = "warm.dat";
        //清除参数
        public static String ClearSeg = "CLEAR";
        public static String IsClearData = "CLRFLAG";
        public static String ClearTime = "CLEARTIME";

        public static String AutoClearHandle = "AUTOHANDLE";
        public static String AutoClearHandleTime = "AUTOHANDLETIME";

        public static string IsClearWarm = "CLEARWARMFLAG";
        public static string ClearWarmTime = "CLEARWARMTIME";

        public static String PersonMsgPath = Environment.CurrentDirectory + "\\PersonInfo.ini";
        
        public static String PersonName = "NAME";
        public static String PersonPhone = "PHONE";
        public static String ComSeg = "COM";
        public static String ComName = "COMNAME";
        public static String MsgPersonHelp = "PERSONHELP";
        public static String MsgAreaAdmin = "AREAADMIN";
        public static String MsgPersonRes = "PERSONRES";
        public static String MsgTagBatteryLow = "BATTERYLOW";
        public static String MsgDeviceDis = "DEVICEDIS";

  
        public static String PersonPassWord = "PASSWORD";
        public static String PersonAccess = "ACCESS";

        public const string SafeSeg = "SAFE";
        public const string UserName = "USER";
        public const string UserPass = "PASS";


        public static string OperRecordPath = Environment.CurrentDirectory  + "\\OperRecord";

        public static bool Open(String FileStr, bool Create)
        {
            //判断文件是否存在
            if (!File.Exists(FileStr))
            {
                if (!Create)
                {
                    return false;
                }
                try
                {
                    MyFileStream = File.Create(FileStr);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            try
            {
            
                MyFileStream = new FileStream(FileStr, FileMode.Open, FileAccess.ReadWrite);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 从一个目录将其内容复制到另一目录
        /// </summary>
        /// <param name="directorySource">源目录</param>
        /// <param name="directoryTarget">目标目录</param>
       public static void CopyFolderTo(string directorySource, string directoryTarget)
        {
            //检查是否存在目的目录  
            if (!Directory.Exists(directoryTarget))
            {
                Directory.CreateDirectory(directoryTarget);
            }
            //先来复制文件  
            DirectoryInfo directoryInfo = new DirectoryInfo(directorySource);
            FileInfo[] files = directoryInfo.GetFiles();
            //复制所有文件  
            foreach (FileInfo file in files)
            {
                try
                {
                    file.CopyTo(Path.Combine(directoryTarget, file.Name));
                }catch{
                }
            }
            /*最后复制目录  
            DirectoryInfo[] directoryInfoArray = directoryInfo.GetDirectories();
            foreach (DirectoryInfo dir in directoryInfoArray)
            {
                CopyFolderTo(Path.Combine(directorySource, dir.Name), Path.Combine(directoryTarget, dir.Name));
            }*/
        }
        public static bool Append(String FileStr, bool Create)
        {
            //判断文件是否存在
            if (!File.Exists(FileStr))
            {
                if (!Create)
                {
                    return false;
                }
                try
                {
                    MyFileStream = File.Create(FileStr);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            try
            {

                MyFileStream = new FileStream(FileStr, FileMode.Append, FileAccess.Write);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 清除集合中的内容
        /// </summary>
        public static void ClearBox()
        {
            try
            {
                CommonCollection.TagPacks.Clear();
            }catch(Exception)
            {
            }
            CommonCollection.Routers.Clear();
            try
            {
                CommonCollection.AlarmTags.Clear();
            }catch(Exception)
            {

            }
            try
            {
                #region 保存上次产生的记录
                List<WarmInfo> listbox = null;
                string warmpath = FileOperation.WarmMsg + @"\" + FileOperation.WarmName;
                if (File.Exists(warmpath))
                {
                    listbox = FileOperation.GetWarmData(warmpath);
                }
                if (null == listbox)
                {
                    listbox = new List<WarmInfo>();
                }
                listbox.AddRange(CommonCollection.WarmInfors.ToArray());
                if (listbox.Count > 0)
                {
                    if (!File.Exists(warmpath))
                    {
                        FileOperation.CreateDirFile(FileOperation.WarmMsg, warmpath);
                    }
                    FileOperation.SaveWarmData(listbox, warmpath);
                }
                #endregion
                CommonCollection.WarmInfors.Clear();
            }catch(Exception)
            {
            }
            foreach(KeyValuePair<string,Area> area in CommonCollection.Areas)
            {
                if (null == area.Value)
                {
                    continue;
                }
                foreach(KeyValuePair<string,BasicRouter> mrouter in area.Value.AreaRouter)
                {
                    if (null == mrouter.Value)
                    {
                        continue;
                    }
                    mrouter.Value.isReport = false;
                }
                foreach (KeyValuePair<string, BasicNode> mnode in area.Value.AreaNode)
                {
                    if (null == mnode.Value)
                    {
                        continue;
                    }
                    mnode.Value.isReport = false;
                }
            }
        }
        public static bool WriteLog(String msg)
        {
            String stryear = "", strmonth = "", strday = "", strfilename = "";
            stryear = DateTime.Now.Year.ToString();
            strmonth = DateTime.Now.Month.ToString().PadLeft(2, '0');
            strday = DateTime.Now.Day.ToString().PadLeft(2,'0');
            StringBuilder strbuilder = new StringBuilder();
            strbuilder.Append(stryear);strbuilder.Append(strmonth);
            strbuilder.Append(strday);strbuilder.Append(".txt");
            strfilename = strbuilder.ToString();
            
            //判断文件夹是否存在
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
            if (!Append(LogPath + @"\" + strfilename, true))
            {
                return false;
            }
            if (null == MyFileStream)
            {
                return false;
            }
            if (!msg.EndsWith("\r\n"))
            {
                msg += "\r\n";
            }
            byte[] bytes = encoder.GetBytes(msg);
            try
            {
                MyFileStream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Close(LogPath + @"\" + strfilename);
            }
            return true;
        }

        //关闭文件流
        public static bool Close(String FileStr)
        {
            if (!File.Exists(FileStr))
            {
                return true;
            }
            if (MyFileStream != null)
            {
                try
                {
                    MyFileStream.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    MyFileStream = null;
                }
                if (MyFileStream == null)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool GetAllSegment(String FileStr, out ArrayList ArrayStrs)
        {
            ArrayStrs = null;
            if (!Open(FileStr, true))
            {
                return false;
            }
            FileInfo fl = new FileInfo(FileStr);
            int len = (int)fl.Length + 1;
            //得到所有分组信息
            byte[] bytes = new byte[len];
            MyFileStream.Read(bytes, 0, len);
            if (!Close(FileStr))
            {
                return false;
            }
            //将字节转化为字符串
            String Str = encoder.GetString(bytes, 0, bytes.Length);
            String _Str;
            //.ini中的字符串
            ArrayStrs = new ArrayList();
            int start, end;
            int endindex = Str.IndexOf('\0');
            start = Str.IndexOf('[', 0, endindex);
            //找到‘[’中括号
            while (start >= 0)
            {//找到‘[’括号
                end = Str.IndexOf(']', start, endindex - start);
                _Str = Str.Substring(start + 1, end - start - 1);
                ArrayStrs.Add(_Str);
                start = Str.IndexOf('[', end, endindex - end);
            }
            return true;
        }

        //得到Segment区中的所有Key值
        public static bool GetAllKey(String FileStr, String segment, ArrayList ArrayStrs)
        {
            if (!Open(FileStr, true))
                return false;
            FileInfo fl = new FileInfo(FileStr);
            int len = (int)fl.Length + 1;
            byte[] bytes = new byte[len];
            MyFileStream.Read(bytes, 0, len);
            if (!Close(FileStr))
                return false;
            String Str = encoder.GetString(bytes, 0, bytes.Length);
            //  得到所有Key
            String _Str = "[" + segment + "]";
            int start, index;
            //在所有字符串中找到segment键值的字符串
            int endindex = Str.IndexOf('\0');
            start = Str.IndexOf(_Str, 0, endindex);
            if (start >= 0)
            {//找到了Segment的值
                Str = Str.Substring(start, endindex - start);
                start = Str.IndexOf("\r\n");
                if (start >= 0)
                {//找到第一个换行符
                    Str = Str.Substring(start + 2, Str.Length - start - 2);
                    start = Str.IndexOf("\r\n");
                    start = Str.IndexOf('[', start, Str.Length - start);
                    if (start >= 0)
                    {
                        Str = Str.Substring(0, start);
                    }
                    //得到所有的键值对
                    start = 0;
                    start = Str.IndexOf('=', 0, Str.Length);
                    index = 0;
                    while (start >= 0)
                    {
                        _Str = Str.Substring(0, start);
                        ArrayStrs.Add(_Str);
                        Str = Str.Substring(start + 1, Str.Length - start - 1);
                        start = Str.IndexOf("\r\n");
                        Str = Str.Substring(start + 2, Str.Length - start - 2);
                        start = Str.IndexOf('=', 0, Str.Length);
                        index++;
                    }                    
                    return true;
                }
            }
            return false;
        }
        public static String GetValue(String FileStr, String segment, String key)
        {//得到键值
            ArrayList StrArray = new ArrayList();
            String value;
            if (GetAllKey(FileStr, segment, StrArray))
            {//得到所有的键值
                if (!Open(FileStr, true))
                    return null;
                FileInfo fl = new FileInfo(FileStr);
                int len = (int)fl.Length + 1;
                byte[] bytes = new byte[len];
                MyFileStream.Read(bytes, 0, len);
                if (!Close(FileStr))
                    return null;
                String Str_Seg, Str_Key;
                String Str = encoder.GetString(bytes, 0, bytes.Length);
                int Strlen = Str.IndexOf('\0');
                for (int i = 0; i < StrArray.Count; i++)
                {
                    if (key.Equals(StrArray[i]))
                    {
                        Strlen = Str.Length < Strlen ? Str.Length : Strlen;
                        Str_Seg = "[" + segment + "]";
                        Str_Key = key + "=";
                        Str = Str.Substring(Str.IndexOf(Str_Seg), Strlen - Str.IndexOf(Str_Seg));
                        Str = Str.Substring(Str.IndexOf(Str_Key) + Str_Key.Length, Str.Length - Str.IndexOf(Str_Key) - Str_Key.Length);
                        value = Str.Substring(0, Str.IndexOf("\r\n"));
                        return value;
                    }
                }
            }
            return null;
        }
        public static bool SetValue(String FileStr, String segment, String key, String value)
        {
            ArrayList StrArray = new ArrayList();
            if (!Open(FileStr, true))
                return false;
            FileInfo fl = new FileInfo(FileStr);
            int len = (int)fl.Length + 1;
            byte[] bytes = new byte[len];
            MyFileStream.Read(bytes, 0, len);
            if(!Close(FileStr))
                return false;
            String Str = encoder.GetString(bytes, 0, bytes.Length);
            String StrFile = Str;
            int i, start, index_Key, index_right;
            if (GetAllKey(FileStr, segment, StrArray))
            {
                String _Str = "[" + segment + "]";
                String Str_Key = key + "=";
                for (i = 0; i < StrArray.Count; i++)
                {
                    if (StrArray[i] != null)
                    {
                        if (key.Equals(StrArray[i]) && !StrArray[i].Equals(""))
                        {//找到键值
                            start = Str.IndexOf(_Str, 0);
                            //找到左端的位置
                            index_Key = Str.IndexOf(Str_Key, start);
                            //找到右端的位置
                            index_right = Str.IndexOf("\r\n", index_Key);
                            StrFile = Str.Substring(0, index_Key + Str_Key.Length)
                                + value + Str.Substring(index_right, Str.Length - index_right);
                            break;
                        }
                    }
                }
                if (i >= StrArray.Count)
                {//说明在该段中没有这个键值,重新添加键值
                    start = Str.IndexOf(_Str, 0);
                    int endindex = Str.IndexOf('\0');
                    index_right = Str.IndexOf("[", start + _Str.Length, endindex - start - _Str.Length);
                    //找到段位
                    if (index_right < 0)
                    {
                        StrFile = Str.Substring(0,endindex) + key + "=" + value + "\r\n";
                    }
                    else
                    {
                        _Str = key + "=" + value + "\r\n";
                        StrFile = Str.Substring(0, index_right) + _Str + Str.Substring(index_right, Str.Length - index_right);
                    }
                }
            }
            else
            {
                //没有段值
                //找到文件结尾
                StrFile = Str.Substring(0, Str.IndexOf('\0')) + "[" + segment + "]\r\n" + key + "=" + value + "\r\n";
            }
            if (!Open(FileStr, true))
                return false;
            bytes = encoder.GetBytes(StrFile);
            MyFileStream.Write(bytes, 0, bytes.Length);
            if (!Close(FileStr))
                return false;
            return true;
        }
        //清除文件中的所有项
        public static bool Clear(String FilePath)
        {
            if (!Open(FilePath, true))
                return false;
            MyFileStream.SetLength(0);
            if (!Close(FilePath))
                return false;
            return true;
        }

        public static bool CreateFile(String MapPath,bool Create)
        {
            if (!Directory.Exists(MapPath))
            {
                if (Create)
                {
                    DirectoryInfo MyDirectoryInfo = null;
                    try
                    {
                        MyDirectoryInfo = Directory.CreateDirectory(MapPath);
                    }
                    catch (Exception ex)
                    {
                        FileOperation.WriteLog(DateTime.Now+":"+ex.ToString());
                        return false;
                    }
                }
                else
                    return false;
            }
            return true;
        }


        //读取数据
        public static byte[] getDataFromFile(string path)
        {
            string filePath = Environment.CurrentDirectory + "\\Log\\" + path;
            if (!File.Exists(filePath))
            {
                //   Console.WriteLine("{0} already exists!", filePath);
                return null;
            }
            byte[] imageBuffer = null;
            FileStream fs = null;
            BinaryReader br = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                br = new BinaryReader(fs);
                long bufSize = br.BaseStream.Length;
                imageBuffer = new byte[bufSize];
                br.Read(imageBuffer, 0, Convert.ToInt32(bufSize));
                //br.Dispose();
            }
            catch { }
            finally
            {
                if (br != null) br.Close();
                if (fs != null) fs.Dispose();
                if (fs != null) fs.Close();
            }
            return imageBuffer;
        }

        //存储二进制对象
        public static void addMsgInFile(string msg, string path, FileMode model)
        {
            string filePath = Environment.CurrentDirectory + "\\Log\\" + path;
            if (!File.Exists(filePath))
            {
                createFile(filePath);
            }
            FileStream fs = null;
            BinaryWriter w = null;
            try
            {
                fs = new FileStream(filePath, model, FileAccess.Write);
                w = new BinaryWriter(fs, System.Text.Encoding.Default);
                w.Write(msg);
            }
            catch { }
            finally
            {
                if (w != null) w.Flush();
                if (fs != null) fs.Flush();
                if (w != null) w.Close();
                if (fs != null) fs.Close();
            }
        }

        public static void createFile(string fileName)
        {
            FileStream fs = null;
            try
            {
                FileInfo fi = new FileInfo(fileName);
                var di = fi.Directory;
                if (!di.Exists)
                    di.Create();
                fs = File.Create(fileName);
            }
            catch { }
            finally
            {
                if (fs != null)
                {
                    fs.Flush();
                    fs.Close();//创建完文件，就得关闭，否则就是站着茅坑不拉屎
                }
            }
        }

        /// <summary>
        /// 向MapPath文件夹中添加图片
        /// </summary>
        public static bool SetMap(String AbsMapPath,string StrMapName)
        {
            if (!CreateFile(MapPath, true))
                return false;
            if (isFileExist(StrMapName, 0)) return true;
            try
            {
                File.Copy(AbsMapPath, MapPath + "\\" + StrMapName, true);
            }
            catch (Exception ex)
            {
                string msg = DateTime.Now.ToString() + "复制图片出现异常：\'" + ex.ToString() + "\'";
                FileOperation.WriteLog(msg);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 判断指定的图片是否存在
        /// </summary>
        /// <param name="StrIMGName"></param>
        /// <param name="Type">0:图片文件
        /// 1：音频文件</param>
        /// <returns></returns>
        public static bool isFileExist(String StrIMGName,int Type)
        {
            if(null == StrIMGName || "".Equals(StrIMGName))
                return false;
            String FilePath = null;
            if (Type == 0)
                FilePath = MapPath;
            else if (Type == 1)
                FilePath = SoundPath;
            if (null == FilePath)
                return false;
            if (!Directory.Exists(FilePath))
                return false;
            String[] StrFiles = Directory.GetFiles(FilePath);
            if (null == StrFiles)
                return false;
            String StrName = null;
            foreach (String str in StrFiles)
            {
                StrName = GetFileName(str, Type);
                if (null == StrName || "".Equals(StrName))
                    continue;
                if (StrIMGName.Equals(StrName))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 获取文件名称
        /// </summary>
        /// <param name="AbsMapPath"></param>
        /// <param name="Type">0：图片文件
        ///     1：音频文件
        /// </param>
        /// <returns></returns>
        public static String GetFileName(String AbsPath,int Type)
        {
            int index = -1;
            if (Type == 0)
            {
                if (AbsPath.EndsWith(".bmp") || AbsPath.EndsWith(".jpg") || AbsPath.EndsWith(".png") )
                {
                    index = AbsPath.LastIndexOf("\\");
                }
            }
            else if (Type == 1)
            {
                if (AbsPath.EndsWith(".mp3") || AbsPath.EndsWith(".wav"))
                {
                    index = AbsPath.LastIndexOf("\\");
                }
            }
            if (index <= 0)
                return null;
            String MapName = AbsPath.Substring(index + 1, AbsPath.Length - index - 1);
            return MapName;
        }
        /// <summary>
        /// 清理图片集合中的图片
        /// </summary>
        /// <param name="StrIMGNames"></param>
        /// <returns></returns>
        public static bool ClearIMGFILE(String[] StrIMGNames)
        {
            if (null == StrIMGNames)
                return false;
            String[] StrFiles = null;
            try
            {
               StrFiles = Directory.GetFiles(MapPath);
            }catch(Exception)
            {
            }
            if (null == StrFiles)
                return true;
            String StrImageName = "";
            foreach (String Str in StrFiles)
            {
                if (null == Str)
                    continue;
                StrImageName = GetFileName(Str, 0);
                if (null == StrImageName || "".Equals(StrImageName))
                    continue;
                if (!JundgeStrInComm(StrIMGNames, StrImageName))
                {
                    try
                    {
                        File.Delete(MapPath + "//" + StrImageName);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            return true;
        }

        public static bool JundgeStrInComm(String[] StrIMGNames,String Str)
        {
            if (null == StrIMGNames)
                return false;
            if (StrIMGNames.Length <= 0)
                return false;
            if (null == Str)
                return false;
            foreach (String str in StrIMGNames)
            { 
                if(null == str)
                    return false;
                if (str.Equals(Str))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 添加音频文件
        /// </summary>
        /// <returns></returns>
        public static bool SetSound(String AbsSound)
        {
            if (!CreateFile(SoundPath, true))
            {
                return false;
            }
            //获取AbsMapPath文件的文件名
            String SoundName = GetFileName(AbsSound, 1);
            if (null == SoundName || "".Equals(SoundName))
                return false;
            ClearSounds();
            if (isFileExist(SoundName, 1))
                return true;
            //每次在复制之前将文件中的其他音频文件全部删除
            try
            {
                File.Copy(AbsSound, SoundPath + "\\" + SoundName);
            }
            catch (Exception ex)
            {
                string msg = DateTime.Now.ToString() + "复制音频出现异常：\'" + ex.ToString() + "\'";
                FileOperation.WriteLog(msg);
                return false;
            }
            return true;
        }
        public static void ClearSounds()
        {
            if (!Directory.Exists(SoundPath))
                return;
            String[] StrSoundNames = Directory.GetFiles(SoundPath);
            if (null == StrSoundNames)
                return;
            String SoundName = null;
            foreach (String strSoundName in StrSoundNames)
            {
                SoundName = GetFileName(strSoundName, 1);
                if (null == SoundName || "".Equals(SoundName))
                    continue;
                try
                {
                    File.Delete(SoundPath + "\\" + SoundName);
                }catch(Exception)
                {
                    continue;    
                }
            }
        
        }
        /// <summary>
        /// 得到音频文件
        /// </summary>
        /// <returns></returns>
        public static String GetSound()
        {
            if (!Directory.Exists(SoundPath))
                return null;
            String[] StrFileNames = Directory.GetFiles(SoundPath);
            if (null == StrFileNames)
                return null;
            if (StrFileNames.Length <=0)
            {
                return null;
            }
            else
            {
                return GetFileName(StrFileNames[0], 1);
            }
        }
        /// <summary>
        /// 创建时间文件夹
        /// </summary>
        public static void CreateDateTimeDir(string StrLogName)
        {
            if (null == StrLogName || "".Equals(StrLogName)) return;

            if (!Directory.Exists(Original)) 
                Directory.CreateDirectory(Original);
            if (CreateDirFile(StrLogName))
            {
                FileStream fstr = null;
                try
                {
                    fstr = File.Open(StrLogName, FileMode.Open, FileAccess.ReadWrite);
                    //fstr = File.Create(StrLogName.ToString());
                    //序列化一个空的List到文件中
                    List<TagPack> tpks = new List<TagPack>();
                    BinaryFormatter MyBf = new BinaryFormatter();
                    MyBf.Serialize(fstr, tpks);
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (null != fstr) fstr.Close();
                    fstr = null;
                }
            }
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="strpath"></param>
        public static void SeralizeObject(Object obj, string strpath)
        {
            FileStream fstr = null;
            try
            {
                fstr = new FileStream(strpath, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fstr, obj);
            }
            catch (Exception) { }
            finally
            {
                if (null != fstr)
                    fstr.Close();
            }
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="strpath"></param>
        public static void DeserializeObject(out Object obj, string strpath)
        {
            FileStream fstr = null;
            try
            {
                fstr = new FileStream(strpath, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                obj = bf.Deserialize(fstr);
                fstr.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                obj = null;
            }
            finally
            {
                if (null != fstr) fstr.Close();
            }
        }


        /// <summary>
        /// 创建时间文件夹
        /// </summary>
        public static void CreateDateTimeDir(string StrDir,string StrLogName)
        {

            if (null == StrDir || "".Equals(StrDir)) return;
            if (null == StrLogName || "".Equals(StrLogName)) return;
            if (CreateDirFile(StrDir, StrLogName))
            {
                FileStream fstr = null;
                try
                {
                    fstr = File.Open(StrLogName, FileMode.Open, FileAccess.ReadWrite);
                    //fstr = File.Create(StrLogName.ToString());
                    //序列化一个空的List到文件中
                    List<TagPack> tpks = new List<TagPack>();
                    BinaryFormatter MyBf = new BinaryFormatter();
                    MyBf.Serialize(fstr, tpks);
                }
                catch (Exception)
                {
                }
                finally {
                    if (null != fstr) fstr.Close();
                    fstr = null;
                }
            }
        }
        //保存数据包到文件中
        public static void SaveOriginalData(List<TagPack> MyTags,string StrLogName)
        {
            if (null == MyTags)return;
            if (null == StrLogName || "".Equals(StrLogName))return;
            BinaryFormatter MyBf = new BinaryFormatter();
            FileStream fstr = null;
            try
            {
                fstr = File.Open(StrLogName, FileMode.Open, FileAccess.Write);
                fstr.Position = 0;
                MyBf.Serialize(fstr, MyTags);
            }
            catch (System.Runtime.Serialization.SerializationException)
            {
                try
                {
                    if (File.Exists(StrLogName))
                    {
                        File.Delete(StrLogName);
                    }
                }
                catch (Exception) { }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
            finally {
                if (null != fstr)fstr.Close();
                fstr = null;
            }
        }
        public static List<TagPack> GetOriginalData(string StrLogName)
        {
            if (null == StrLogName || "".Equals(StrLogName)) return null;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fstr = null;
            List<TagPack> LogTags = null;
            if (!File.Exists(StrLogName)) return null;
            try
            {
             fstr = File.Open(StrLogName, FileMode.Open, FileAccess.Read);
             fstr.Position = 0;
             LogTags = (List<TagPack>)bf.Deserialize(fstr);
            }
            catch (Exception EX)
            {
                Console.WriteLine(EX.ToString());
            }
            finally 
            {if (null != fstr) fstr.Close();fstr = null;}
            return LogTags;
        }
        /// <summary>
        /// 保存警报讯息
        /// </summary>
        public static void SaveWarmData(List<WarmInfo> MyWarms,string StrLogName)
        {
            if (null == MyWarms) 
                return;
            if (null == StrLogName || "".Equals(StrLogName)) 
                return;
            BinaryFormatter MyBf = new BinaryFormatter();
            FileStream fstr = null;
            try
            {
                fstr = File.Open(StrLogName, FileMode.Open, FileAccess.Write);
                fstr.Position = 0;
                MyBf.Serialize(fstr, MyWarms);
            }
            catch (System.Runtime.Serialization.SerializationException)
            {
                //序列化失败(防止序列化一半时失败，之后反序列化将一直不成功)
                try
                {
                    if (File.Exists(StrLogName))
                    {
                        File.Delete(StrLogName);
                    }
                }catch(Exception){}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (null != fstr) fstr.Close();
                fstr = null;
            }
        }
        /// <summary>
        /// 得到原始的警报讯息
        /// </summary>
        /// <returns></returns>
        public static List<WarmInfo> GetWarmData(string StrLogName)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fstr = null;
            List<WarmInfo> LogTags = null;
            if (!File.Exists(StrLogName)) 
                return new List<WarmInfo>();
            try
            {
                fstr =  File.Open(StrLogName, FileMode.Open, FileAccess.Read);
                fstr.Position = 0;
                LogTags = (List<WarmInfo>)bf.Deserialize(fstr);
            }
            catch (Exception e1)
            {
                MessageBox.Show("得到原始的警报讯息出现异常!异常原因:"+e1.ToString());
            }
            finally
            {
                if (null != fstr)
                {
                    fstr.Close();
                }
                fstr = null; 
            }
            return LogTags;
        }

        /// <summary>
        /// 得到所有的文件夹名称
        /// </summary>
        /// <param name="StrPath"></param>
        /// <returns></returns>
        public static string[] GetAllDirName(string StrPath)
        {
            if (null == StrPath || "".Equals(StrPath)) 
                return null;
            string[] SrtDirs = null;
            try
            {
                SrtDirs = Directory.GetDirectories(StrPath).OrderBy(Strs => new DirectoryInfo(Strs).CreationTime).ToArray();
            }catch(Exception)
            {
                return null;
            }
            return SrtDirs;
        }
        /// <summary>
        /// 得到文件夹下的文件名称
        /// </summary>
        /// <param name="StrPath"></param>
        /// <returns></returns>
        public static string[] GetAllFileName(string StrPath)
        {
            if (null == StrPath || "".Equals(StrPath)) return null;
            string[] StrFiles = null;
            try {
                StrFiles = Directory.GetFiles(StrPath).OrderBy
                    (fls => new FileInfo(fls).CreationTime).ToArray();
            }
            catch(Exception)
            {return null;}
            return StrFiles;
        }
        //判断指定的文件是否存在
        public static bool CreateDirFile(string StrDir,string StrFile)
        {
            if (null == StrDir || "".Equals(StrDir) || null == StrFile || "".Equals(StrFile))
                return false;
            FileStream CrtStreanm = null;
            try
            {
                if (!Directory.Exists(StrDir))
                {
                    Directory.CreateDirectory(StrDir);
                }
                if (!File.Exists(StrFile))
                {
                    CrtStreanm = File.Create(StrFile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            finally {
                if (null != CrtStreanm)
                {
                    CrtStreanm.Close();
                }
                CrtStreanm = null;
            }
            return true;
        }
        /// <summary>
        /// 清除文件中内容
        /// </summary>
        /// <param name="StrFile"></param>
        public static void ClearFileContent(string StrFile)
        {
            if (null == StrFile || "".Equals(StrFile)) return;
            if (File.Exists(StrFile))
            {
                FileStream fs = null;
                try
                {
                    fs = File.Open(StrFile, FileMode.Open, FileAccess.Write);
                    fs.SetLength(0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally {
                    if (null != fs) fs.Close();
                    fs = null;
                }
            }
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="StrFile"></param>
        /// <returns></returns>
        public static bool CreateDirFile(string StrFile)
        {
            if (null == StrFile || "".Equals(StrFile)) return false;
            if (!File.Exists(StrFile))
            {
                FileStream cf = null;
                try
                {
                    cf = File.Create(StrFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
                finally {
                    if (null != cf) cf.Close();
                    cf = null;
                }
                return true;
            }
            return true;
        }

        /// <summary>
        ///向指定文件中写入文本信息
        /// </summary>
        /// <param name="StrFilePath"></param>
        /// <param name="Msg"></param>
        public static void WriteDataFile(string StrFilePath,string Msg)
        {
            if (null == StrFilePath || "".Equals(StrFilePath)) return;
            if ("".Equals(Msg)) return;
            FileStream ws = null;
            try
            {
                ws = File.Open(StrFilePath, FileMode.Append, FileAccess.Write);
                byte[] bytes = Encoding.Default.GetBytes(Msg);
                ws.Write(bytes,0,bytes.Length);
                ws.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }
            finally {
                if (null != ws) ws.Close();
                ws = null;
            }
        }
    }
    /// <summary>
    /// 利用第三方组件操作Execel
    /// </summary>
    public class NpoiLib
    {
        public HSSFWorkbook hssfworkbook;
        public ISheet sheet1;
        public int currentRow, currentcolumn;
        public NpoiLib(string sheetname)
        {
            hssfworkbook = new HSSFWorkbook();
            //預設情況下一個workbook必須至少一個sheet，不然打開會錯誤
            sheet1 = hssfworkbook.CreateSheet(sheetname);
            currentRow = 0;
            currentcolumn = 0;
        }
        //注意，這種方法使用是要確定row沒有被寫入過值
        public bool writeToCell(ISheet sheet, int row, int column, string str)
        {
            IRow row1;
            if (sheet.GetRow(row) == null)
            {
                row1 = sheet.CreateRow(row);
            }
            else
            {
                row1 = sheet.GetRow(row);
            }

            try
            {
                row1.CreateCell(column).SetCellValue(str);
            }
            catch (Exception ex)
            {
                FileOperation.WriteLog("向Excel单元格中写入数据出现错误！错误原因:"+ex.ToString());
                return false;
            }
            return true;
        }
        /// <summary>
        /// 设置列的宽度
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="ColumnIndex"></param>
        /// <param name="Width"></param>
        public void SetColumnWidth(ISheet sheet,int ColumnIndex,int Width)
        {
            sheet.SetColumnWidth(ColumnIndex, Width);
        }
       
        //注意，這種方法使用是要確定row有資料，type可以根據不同的類型進行區別
        public string readToCell(ISheet sheet, int row, int column, int type)
        {
            string tr = null;
            try
            {
                tr = sheet.GetRow(row).GetCell(column).StringCellValue;
            }
            catch (Exception)
            {
                return tr;
            }
            return tr;
        }
        public void WriteToFile(string filename)
        {
            //Write the stream data of workbook to the root directory
            FileStream file = new FileStream(@filename, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
        }
    }
    /*
    class ExcelOperation
    {
        private static Excel.Application ExcelApp;
        private static Excel.Workbooks ExcelWorkBooks;
        private static Excel.Workbook ExcelWorkBook;
        private static Excel.Worksheet WorkSheet;
        public static void CreateWorkBook(int SheetIndex)
        {
            ExcelApp = new Excel.Application();
            ExcelWorkBooks = ExcelApp.Workbooks;
            ExcelWorkBook = ExcelWorkBooks.Add(true);
            
            if (ExcelWorkBook.Worksheets.Count > 0)
                WorkSheet = ExcelWorkBook.Worksheets[SheetIndex];
        }
        public static void SaveData(int RowIndex, int ColumnIndex, string StrCell)
        {
            if (null == WorkSheet) return;
            if (null == StrCell) return;
            if (RowIndex < 1 || ColumnIndex < 1) return;
            Excel.Range CurRange = WorkSheet.Cells[RowIndex, ColumnIndex];
            CurRange.Value = StrCell;
            CurRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        }
        public static void SetCellWidth(int Start,int End,int Width)
        {
            if (Start < 1 || End < 1 || Width < 0) return;
            if (Start > End) return;
            Excel.Range MyRange = WorkSheet.Cells[Start, End];
            MyRange.ColumnWidth = Width;
            MyRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        }
        public static bool SaveAsExcel(string FilePath)
        {
            if (null == ExcelWorkBook) return false;
            if (null == FilePath || "".Equals(FilePath)) return false;
            try
            {
                ExcelWorkBook.SaveAs(FilePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public static void Close()
        {
            if (null != ExcelWorkBook) ExcelWorkBook.Close();
            if (null != ExcelWorkBooks) ExcelWorkBooks.Close();
            if (null != ExcelApp) ExcelApp.Quit();
            ExcelWorkBook = null;
            ExcelWorkBooks = null;
            ExcelApp = null;
            GC.Collect();
        }
    }*/
}
