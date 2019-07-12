using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Collections;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Runtime.InteropServices;
namespace PrecisePositionLibrary
{
    /// <summary>
    /// 实现三点的精准定位
    /// </summary>
    public class PrecisePosition
    {
        private PrecisePosition() { }
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        private static extern bool PostMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern bool SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);
        [StructLayout(LayoutKind.Sequential)]
        public struct TagPlace 
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
             public byte[] ID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
             public byte[] GroupID;
             public double X, Y, Z;
             public byte Battery;
             public int index;
             public byte LocalType;
             public ushort NoExeTime;
             public ushort SleepTime;
             public short GsensorX, GsensorY, GsensorZ;
             [MarshalAs(UnmanagedType.ByValArray,SizeConst = 2)]
             public byte[] ReferID1;
             [MarshalAs(UnmanagedType.ByValArray,SizeConst = 2)]
             public byte[] ReferID2;
             [MarshalAs(UnmanagedType.ByValArray,SizeConst = 2)]
             public byte[] ReferID3;
             [MarshalAs(UnmanagedType.ByValArray,SizeConst = 2)]
             public byte[] ReferID4;
             [MarshalAs(UnmanagedType.ByValArray,SizeConst = 2)]
             public byte[] ReferID5;
             public double Dis1, Dis2, Dis3, Dis4, Dis5;
             public int SigQuality1, SigQuality2, SigQuality3, SigQuality4, SigQuality5;
             public double ResidualValue1, ResidualValue2, ResidualValue3, ResidualValue4, ResidualValue5;
        }
        public struct BasicReport
        {    public byte[] ID;
             public ushort SleepTime;
             public uint Version;
        }
        //此时消息处理时间
        public static int tick = 0;
        public static Point[] InterTemp = new Point[InterTempMaxLen];
        private const int InterTempMaxLen = 6;
        public static int CurInterNum = 0;
        public static int MapWidth = 719;
        public static int MapHeight = 642;
        private const int FixedWaiting = 800;
        public const byte bigversion = 3;//大版本
        public const byte smallversion = 1;//小版本
        private static IPEndPoint CurEndPoint = null;
        private static System.Net.Sockets.UdpClient client = null;
        private static Thread ListenNetNode = null;
        private static Thread ScanDeviceDis = null;
        private static bool isScanDis=false;
        private static int ScanTime = 10;
        private static int TagSleepParam = 5;
        private static int TagConst = 50;
        private static int BsSleepParam = 3;
        private static int BsConst = 30;
        private static byte[] Buffer = new byte[2048];
        private static int BufferLen = 0;
        private static IntPtr CurHandler = IntPtr.Zero;
        private static ReportMode CurReportMode = ReportMode.UnKnown;
        private static PosititionMode CurPositionMode = PosititionMode.UnKnown;
        private static AfewDPos adpos = AfewDPos.Pos2Dim;
        public enum ReportMode
        {
            ImgMode,
            ListMode,
            UnKnown
        }
        //定位模式
        public enum PosititionMode 
        {
            SigQuality,
            Closestdistance,
            UnKnown
        }
        public enum AfewDPos
        { 
            Pos3Dim,
            Pos2Dim
        }
        /// <summary>
        /// 当前的所有基站
        /// </summary>
        private static ConcurrentDictionary<string, BasicStation> BasicStations = new ConcurrentDictionary<string, BasicStation>();
        private static ConcurrentDictionary<string, TagInfo> BasicTagInfos = new ConcurrentDictionary<string, TagInfo>();
        private static ConcurrentDictionary<string, BsInfo> BsInfos = new ConcurrentDictionary<string, BsInfo>();
        /// <summary>
        /// 当前接收到的所有数据包
        /// </summary>
        public static ConcurrentDictionary<string, TagPack> TagPacks = new ConcurrentDictionary<string, TagPack>();
        /// <summary>
        /// 初始化基站讯息
        /// </summary>
        /// <param name="BStations">提供的基站数组</param>
        /// <returns></returns>
        public static bool InitBasicStations(List<BsInfo> BStations)
        {
            
            if (null == BStations)
            {
                return false;
            }
            if (BStations.Count == 0)
            {
                return false;
            }
            BsInfos.Clear();
            string StrBsId = "";
            foreach (BsInfo bs in BStations)
            {
                if (null == bs)
                    continue;
                StrBsId = bs.ID[0].ToString("X2") + bs.ID[1].ToString("X2");
                try
                {
                    BsInfos.TryAdd(StrBsId, bs);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 初始化网络讯息
        /// </summary>
        public static bool InitNet(IPAddress ip, int Port)
        {
            if (null == ip)
            {
                return false;
            }
            if (Port <= 0 || Port > 65535)
            {
                return false;
            }
            try
            {
                CurEndPoint = new IPEndPoint(ip, Port);
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw e;
            }
            return true;
        }
        /// <summary>
        /// 设置检测设备是否发生异常的参数
        /// 判断Tag是否断开的时间间隔为SleepParam*TagSleep + TagParam
        /// 判断基站是否断开的时间间隔为BsSleepParam*BsSleep + BsParam
        /// 注：默认调用Start时就开启了设备检测功能，使用的是默认参数
        /// </summary>
        /// <param name="scantime">扫描的间隔时间(单位为秒)</param>
        /// <param name="tspparam">tag的扫描间隔时间参数1=>SleepParam</param>
        /// <param name="tgcst">tag的扫描间隔时间参数1=>TagParam</param>
        /// <param name="ndspparam">基站的扫描间隔时间参数1=>BsSleepParam</param>
        /// <param name="ndcst">基站的扫描间隔时间参数2=>BsParam</param>
        private static void SetScanDeviceDisParam(int scantime, int tspparam, int tgcst, int ndspparam, int ndcst)
        {
            isScanDis = true;
            ScanTime = scantime;
            TagSleepParam = tspparam;
            TagConst = tgcst;
            BsSleepParam = ndspparam;
            BsConst = ndcst;
        }
        /// <summary>
        /// 关闭检测设备是否异常的功能
        /// </summary>
        private static void StopScanFunction()
        {
            isScanDis = false;
            ScanTime = 10;
            TagSleepParam = 5;
            TagConst = 50;
            BsSleepParam = 3;
            BsConst = 30;
        }
        
        /// <summary>
        /// 开始监控网络数据
        /// </summary>
        /// <param name="CurHandler"></param>
        /// <param name="CurReportMode"></param>
        /// <param name="mPositionMode"></param>
        /// <returns></returns>
        public static bool Start(IntPtr CurHandler, ReportMode CurReportMode,PosititionMode mPositionMode,AfewDPos adpos)
        {
            if (null == CurEndPoint)
            {
                return false;
            }
            try
            {
                PrecisePosition.CurHandler = CurHandler;
                PrecisePosition.CurReportMode = CurReportMode;
                PrecisePosition.CurPositionMode = mPositionMode;
                PrecisePosition.adpos = adpos;
                client = new System.Net.Sockets.UdpClient(CurEndPoint);
            }
            catch (ArgumentNullException e1)
            { 
                throw e1;
            }
            catch (System.Net.Sockets.SocketException e2)
            { 
                throw e2;
            }
            //开启一个线程监听端口的连接
            ListenNetNode = new Thread(ListenLocalPort);
            ListenNetNode.Start();
            //--开启检测设备是否断开连接--
            isScanDis = true;
            ScanDeviceDis = new Thread(ScanDeviceDisFun);
            ScanDeviceDis.Start();
            return true;
        }

        /// <summary>
        /// 初始化卡片高度
        /// </summary>
        /// <param name="tginfos"></param>
        /// <returns></returns>
        public static bool InitTag(TagInfo[] tginfos)
        {
            if (null == tginfos)
            {
                return false;
            }
            if (tginfos.Length <= 0)
            {
                return false;
            }
            BasicTagInfos.Clear();
            string strtgId = "";
            foreach (TagInfo tg in tginfos)
            {
                if (null == tg)
                {
                    continue;
                }
                strtgId = tg.Id[0].ToString("X2") + tg.Id[1].ToString("X2");
                try
                {
                    BasicTagInfos.TryAdd(strtgId, tg);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 停止监控时发生
        /// </summary>
        public static void Stop()
        {
            if (null != client)
            {
                try
                {
                    client.Close();
                }
                catch (Exception)
                {

                }
                finally
                {
                    client = null;
                }
            }
            if (null != ListenNetNode)
            {
                if (ListenNetNode.IsAlive)
                {
                    try
                    {
                        ListenNetNode.Abort();
                    }
                    catch (Exception)
                    {

                    }
                }
                ListenNetNode = null;
            }
            if (null != ScanDeviceDis)
            {
                if (ScanDeviceDis.IsAlive)
                {
                    try
                    {
                        ScanDeviceDis.Abort();
                    }
                    catch (Exception)
                    {

                    }
                }
                ScanDeviceDis = null;
            }
            TagPacks.Clear();
            BasicStations.Clear();
        }
        /// <summary>
        /// 检测设备是否发生异常
        /// </summary>
        private static void ScanDeviceDisFun()
        {
            while (isScanDis)
            {
                Thread.Sleep(ScanTime*1000);
                //检测基站是否发生故障
                foreach(KeyValuePair<string,BasicStation> bstt in BasicStations)
                {
                    if (null == bstt.Value)
                    {
                        continue;
                    }
                    if (Environment.TickCount - bstt.Value.TickCount < (bstt.Value.SleepTime * BsSleepParam + BsConst) * 1000)
                    { 
                        bstt.Value.isState = true; 
                        continue;
                    }
                    //说明此时这个设备断开了连接
                    bstt.Value.isState = false;
                    IntPtr pointer = GetBasicStationPtr(bstt.Value);
                    if (pointer == IntPtr.Zero)
                    {
                        continue;
                    }
                    PostMessage(CurHandler, TPPID.WM_DEVICE_DIS, TPPID.WBASIC_TYPE, pointer);
                }
                foreach (KeyValuePair<string, TagPack> tgpk in TagPacks)
                {
                    if (null == tgpk.Value) continue;
                    if (Environment.TickCount - tgpk.Value.TickCount < (tgpk.Value.St * TagSleepParam + TagConst) * 1000)
                    { 
                        tgpk.Value.isState = true; 
                        continue;
                    }
                    tgpk.Value.isState = false;
                    IntPtr pointer = GetTagPackPtr(tgpk.Value, new Point(tgpk.Value.x, tgpk.Value.y, tgpk.Value.z));
                    if (pointer == IntPtr.Zero)
                    {
                        continue;
                    }
                    PostMessage(CurHandler, TPPID.WM_DEVICE_DIS, TPPID.WTAG_TYPE, pointer);
                }
            }
        }
        /// <summary>
        /// 网络数据处理函数
        /// </summary>
        private static void ListenLocalPort()
        {
            IPEndPoint RemoteEndPoint = new IPEndPoint(IPAddress.Any, IPEndPoint.MinPort);
            byte[] TempBytes = new byte[2048];
            int Index = -1;
            byte cs = 0;
            string StrTagID = "", StrBsstID = "";
            BasicStation Bsst;
            IntPtr pointer;
            TagPack CurTagPack = null;
            BufferLen = 0;
            BsInfo brf = null;
            ushort d = 0;
            while (null != client) {
                try {
                    TempBytes = client.Receive(ref RemoteEndPoint);
                } catch(Exception ex) { 
                    Console.WriteLine(ex.ToString());
                }
                if(TempBytes.Length > 0) {
                    Array.Copy(TempBytes, 0, Buffer, BufferLen, TempBytes.Length);
                    BufferLen += TempBytes.Length;
                }
                tick = Environment.TickCount;
                while (BufferLen >= 13) {
                    Index = Find(TPPID.Head);
                    if (Index < 0) { 
                        //没有找到头
                        BufferLen = 0;
                        continue;
                    }
                    /*
                     *   FC(0) + 01(1) + SerialNum[1 byte](2) + 
                     *   LocateType[1 byte](3) + TagId[2 byte](4-5) + 
                     *   Battery[1 byte](6) + GsensorTime[2 byte](7-8) + 
                     *   TagSleepTime[2 byte](9-10) + ReportRouterNum[1 byte](11) + 
                     *   RouterId[2 byte](12-13) + Distance[2 byte](14-15) + 
                     *   Priority[1 byte](16) + CS(17) + FB(18)
                     * */
                    if(Buffer[Index+1]==TPPID.TagType && Buffer[Index+TPPID.TagPackLen-1]==TPPID.End) { //Tag数据包
                        cs = 0;
                        for (int i = Index; i < Index + TPPID.TagPackLen - 2; i++)
                        {
                            cs += Buffer[i];
                        }
                        if (cs == Buffer[Index + TPPID.TagPackLen - 2])
                        {
                            StrTagID = Buffer[Index + 4].ToString("X2") + Buffer[Index + 5].ToString("X2");
                            if (TagPacks.ContainsKey(StrTagID))
                            {
                                if (TagPacks.TryGetValue(StrTagID, out CurTagPack))
                                {
                                    if (CurTagPack.index == Buffer[Index + 2])
                                    {//序列号相同，说明是其他基站也上报上来的数据包
                                        BasicReportTag curbrtag = null;
                                        if (!CurTagPack.CurBasicReport.ContainsKey(Buffer[Index + 12].ToString("X2") + Buffer[Index + 13].ToString("X2")))
                                        {
                                            curbrtag = new BasicReportTag();
                                            System.Buffer.BlockCopy(Buffer, Index + 12, curbrtag.Id, 0, 2);

                                            string strgroupid = curbrtag.Id[0].ToString("X2") + curbrtag.Id[1].ToString("X2");
                                            BsInfo bs = null;
                                            if (BsInfos.TryGetValue(strgroupid, out bs))
                                            {//设置基站讯息 
                                                System.Buffer.BlockCopy(bs.GroupID, 0, curbrtag.GroupID, 0, 2);
                                            }

                                            if (adpos == AfewDPos.Pos3Dim)
                                            {
                                                curbrtag.distanse = (ushort)(Buffer[Index + 14] << 8 | Buffer[Index + 15]);
                                            }
                                            else {
                                                d = (ushort)(Buffer[Index + 14] << 8 | Buffer[Index + 15]);
                                                if (BsInfos.TryGetValue(curbrtag.Id[0].ToString("X2") + curbrtag.Id[1].ToString("X2"), out brf))
                                                {
                                                    if (brf.Place.z < 0)
                                                    {
                                                        curbrtag.distanse = d;
                                                    }
                                                    else
                                                    {
                                                        TagInfo mytginfo = null;
                                                        double p = 0d;
                                                        if (BasicTagInfos.TryGetValue(CurTagPack.ID[0].ToString("X2") + CurTagPack.ID[1].ToString("X2"), out mytginfo))
                                                        {
                                                            p = Math.Pow(d, 2) - Math.Pow(Math.Abs(brf.Place.z - mytginfo.height), 2);
                                                        }
                                                        else
                                                        {
                                                            p = Math.Pow(d, 2) - Math.Pow(brf.Place.z, 2);
                                                        }
                                                        if (p >= 0)
                                                        {
                                                            curbrtag.distanse = Math.Pow(p, 0.5);
                                                        }
                                                        else
                                                        {
                                                            curbrtag.distanse = 30;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    curbrtag.distanse = d;
                                                }
                                            }

                                            curbrtag.Priority = Buffer[Index + 16];
                                            CurTagPack.CurBasicReport.TryAdd(curbrtag.Id[0].ToString("X2") + curbrtag.Id[1].ToString("X2"), curbrtag);
                                            CurTagPack.CurBasicNum++;
                                        }
                                        else
                                        {
                                            CurTagPack.CurBasicReport.TryGetValue(Buffer[Index + 12].ToString("X2") + Buffer[Index + 13].ToString("X2"), out curbrtag);
                                            if (null != curbrtag)
                                            {

                                                if (adpos == AfewDPos.Pos3Dim)
                                                {
                                                    curbrtag.distanse = (ushort)(Buffer[Index + 14] << 8 | Buffer[Index + 15]);
                                                }
                                                else
                                                {
                                                    d = (ushort)(Buffer[Index + 14] << 8 | Buffer[Index + 15]);
                                                    if (BsInfos.TryGetValue(curbrtag.Id[0].ToString("X2") + curbrtag.Id[1].ToString("X2"), out brf))
                                                    {
                                                        if (brf.Place.z < 0)
                                                        {
                                                            curbrtag.distanse = d;
                                                        }
                                                        else
                                                        {
                                                            TagInfo mytginfo = null;
                                                            double p = 0d;
                                                            if (BasicTagInfos.TryGetValue(CurTagPack.ID[0].ToString("X2") + CurTagPack.ID[1].ToString("X2"), out mytginfo))
                                                            {
                                                                p = Math.Pow(d, 2) - Math.Pow(Math.Abs(brf.Place.z - mytginfo.height), 2);
                                                            }
                                                            else
                                                            {
                                                                p = Math.Pow(d, 2) - Math.Pow(brf.Place.z, 2);
                                                            }
                                                            if (p >= 0)
                                                            {
                                                                curbrtag.distanse = Math.Pow(p, 0.5);
                                                            }
                                                            else
                                                            {
                                                                curbrtag.distanse = 30;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        curbrtag.distanse = d;
                                                    }
                                                }
                                                curbrtag.Priority = Buffer[Index + 16];
                                            }
                                            string strgroupid = curbrtag.Id[0].ToString("X2") + curbrtag.Id[1].ToString("X2");
                                            BsInfo bs = null;
                                            if (BsInfos.TryGetValue(strgroupid, out bs))
                                            {//设置基站讯息 
                                                System.Buffer.BlockCopy(bs.GroupID, 0, curbrtag.GroupID, 0, 2);
                                            }
                                        }
                                        //计算间隔时间
                                        if (CurTagPack.CurBasicNum >= CurTagPack.TotalbasicNum || Environment.TickCount - CurTagPack.TickCount > CurTagPack.St * 100 + FixedWaiting)
                                        {
                                            try
                                            {
                                                SetTagPoint(CurTagPack);
                                            }
                                            catch (Exception)
                                            {
                                            }
                                            CurTagPack.isReport = true;
                                        }
                                    }
                                    else
                                    {//序列号不相同，上一次定位已经完成，开始重新计算坐标
                                        if (!CurTagPack.isReport)
                                        {
                                            try
                                            {
                                                SetTagPoint(CurTagPack);
                                            }
                                            catch (Exception)
                                            {

                                            }
                                        }
                                        CurTagPack.isReport = false;
                                        CurTagPack.CurBasicReport.Clear();
                                        CurTagPack.CurBasicNum = 0;
                                        CurTagPack.TotalbasicNum = Buffer[Index + 11];

                                        BasicReportTag curbrtag = new BasicReportTag();
                                        System.Buffer.BlockCopy(Buffer, Index + 12, curbrtag.Id, 0, 2);
                                        string strgroupid = curbrtag.Id[0].ToString("X2") + curbrtag.Id[1].ToString("X2");
                                        BsInfo bs = null;
                                        if (BsInfos.TryGetValue(strgroupid, out bs))
                                        {//设置基站讯息 
                                            System.Buffer.BlockCopy(bs.GroupID, 0, curbrtag.GroupID, 0, 2);
                                        }

                                        if (adpos == AfewDPos.Pos3Dim)
                                        {
                                            curbrtag.distanse = (ushort)(Buffer[Index + 14] << 8 | Buffer[Index + 15]);
                                        }
                                        else
                                        {
                                            d = (ushort)(Buffer[Index + 14] << 8 | Buffer[Index + 15]);
                                            if (BsInfos.TryGetValue(curbrtag.Id[0].ToString("X2") + curbrtag.Id[1].ToString("X2"), out brf))
                                            {
                                                if (brf.Place.z < 0)
                                                {
                                                    curbrtag.distanse = d;
                                                }
                                                else
                                                {
                                                    TagInfo mytginfo = null;
                                                    double p = 0d;
                                                    if (BasicTagInfos.TryGetValue(CurTagPack.ID[0].ToString("X2") + CurTagPack.ID[1].ToString("X2"), out mytginfo))
                                                    {
                                                        p = Math.Pow(d, 2) - Math.Pow(Math.Abs(brf.Place.z - mytginfo.height), 2);
                                                    }
                                                    else
                                                    {
                                                        p = Math.Pow(d, 2) - Math.Pow(brf.Place.z, 2);
                                                    }
                                                    if (p >= 0)
                                                    {
                                                        curbrtag.distanse = Math.Pow(p, 0.5);
                                                    }
                                                    else
                                                    {
                                                        curbrtag.distanse = 30;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                curbrtag.distanse = d;
                                            }
                                        }
                                        curbrtag.Priority = Buffer[Index + 16];
                                        CurTagPack.CurBasicReport.TryAdd(curbrtag.Id[0].ToString("X2") + curbrtag.Id[1].ToString("X2"), curbrtag);
                                        CurTagPack.CurBasicNum++;
                                        CurTagPack.TickCount = Environment.TickCount;
                                    }
                                    CurTagPack.LocalType = Buffer[Index + 3];
                                    CurTagPack.Battery = Buffer[Index + 6];
                                    CurTagPack.NoExeTime = (ushort)(Buffer[Index + 7] << 8 | Buffer[Index + 8]);
                                    CurTagPack.St = (ushort)(Buffer[Index + 9] << 8 | Buffer[Index + 10]);
                                    CurTagPack.TotalbasicNum = Buffer[Index + 11];
                                    CurTagPack.index = Buffer[Index + 2];
                                    CurTagPack.ReportDT = DateTime.Now;
                                }
                            }
                            else
                            {
                                CurTagPack = new TagPack();
                                System.Buffer.BlockCopy(Buffer, Index + 4, CurTagPack.ID, 0, 2);
                                CurTagPack.TotalbasicNum = Buffer[Index + 11];

                                BasicReportTag curbrtag = new BasicReportTag();
                                System.Buffer.BlockCopy(Buffer, Index + 12, curbrtag.Id, 0, 2);

                                //获取当前基站的组别
                                string strgroupid = curbrtag.Id[0].ToString("X2") + curbrtag.Id[1].ToString("X2");
                                BsInfo bs = null;
                                if (BsInfos.TryGetValue(strgroupid, out bs))
                                {//设置基站讯息 
                                    System.Buffer.BlockCopy(bs.GroupID, 0, curbrtag.GroupID, 0, 2);
                                }
                                //计算出Tag与基站的高度差
                                if (adpos == AfewDPos.Pos3Dim)
                                {
                                    curbrtag.distanse = (ushort)(Buffer[Index + 14] << 8 | Buffer[Index + 15]);
                                }
                                else
                                {
                                    d = (ushort)(Buffer[Index + 14] << 8 | Buffer[Index + 15]);
                                    if (BsInfos.TryGetValue(curbrtag.Id[0].ToString("X2") + curbrtag.Id[1].ToString("X2"), out brf))
                                    {
                                        if (brf.Place.z < 0)
                                        {
                                            curbrtag.distanse = d;
                                        }
                                        else
                                        {
                                            TagInfo mytginfo = null;
                                            double p = 0d;
                                            if (BasicTagInfos.TryGetValue(CurTagPack.ID[0].ToString("X2") + CurTagPack.ID[1].ToString("X2"), out mytginfo))
                                            {
                                                p = Math.Pow(d, 2) - Math.Pow(Math.Abs(brf.Place.z - mytginfo.height), 2);
                                            }
                                            else
                                            {
                                                p = Math.Pow(d, 2) - Math.Pow(brf.Place.z, 2);
                                            }
                                            if (p >= 0)
                                            {
                                                curbrtag.distanse = Math.Pow(p, 0.5);
                                            }
                                            else
                                            {
                                                curbrtag.distanse = 30;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        curbrtag.distanse = d;
                                    }
                                }
                                CurTagPack.LocalType = Buffer[Index + 3];
                                curbrtag.Priority = Buffer[Index + 16];
                                CurTagPack.CurBasicReport.TryAdd(curbrtag.Id[0].ToString("X2") + curbrtag.Id[1].ToString("X2"), curbrtag);
                                CurTagPack.CurBasicNum++;
                                CurTagPack.Battery = Buffer[Index + 6];
                                CurTagPack.St = (ushort)(Buffer[Index + 9] << 8 | Buffer[Index + 10]);
                                CurTagPack.index = Buffer[Index + 2];
                                CurTagPack.NoExeTime = (ushort)(Buffer[Index + 7] << 8 | Buffer[Index + 8]);

                                CurTagPack.TickCount = Environment.TickCount;
                                TagPacks.TryAdd(StrTagID, CurTagPack);
                            }
                            //校验错误，舍弃该包数据
                            Array.Copy(Buffer, Index + TPPID.TagPackLen, Buffer, Index, BufferLen);
                            BufferLen -= TPPID.TagPackLen;
                        }
                        else
                        {
                            //说明校验位错误
                            Array.Copy(Buffer, Index + 1, Buffer, 0, BufferLen);
                            BufferLen -= (Index + 1);
                        }
                    }
                    else if (Buffer[Index + 1] == TPPID.TagStillType && Buffer[Index + TPPID.TagStillLen + Buffer[Index + TPPID.TagPackNumPlace] * 4] == TPPID.End)
                    {
                        /*  
                         *   FC(0) + 03(1) + SerialNum[1 byte](2) + 
                         *   LocateType[1 byte](3) + TagId[2 byte](4-5) + 
                         *   Battery[1 byte](6) + GsensorTime[2 byte](7-8) + 
                         *   TagSleepTime[2 byte](9-10) +  GsensorX[2byte](11-12) + GsensorY[2byte](13-14) + GsensorZ[2byte](15-16)  + 
                         *   RouterNum[1 byte](17) + 
                         *   RouterId1[2 byte](18-19) + Distance1[2 byte](20-21) + 
                         *   ... + CS + FB
                         * */
                        cs = 0;
                        for (int i = Index; i < (Index + TPPID.TagStillLen + Buffer[Index + TPPID.TagPackNumPlace] * 4 - 1); i++)
                            cs += Buffer[i];
                        if (cs == Buffer[Index + TPPID.TagStillLen + Buffer[Index + TPPID.TagPackNumPlace] * 4 - 1])
                        {
                            StrTagID = Buffer[Index + 4].ToString("X2") + Buffer[Index + 5].ToString("X2");
                            if (TagPacks.ContainsKey(StrTagID))
                            {
                                if (TagPacks.TryGetValue(StrTagID, out CurTagPack))
                                {
                                    if (CurTagPack.index != Buffer[Index + 2])
                                    {//说明不是同一包数据，说明是新的一包数据
                                        CurTagPack.CurBasicReport.Clear();
                                        BasicReportTag curbrtag = null;
                                        for (int i = 0; i < Buffer[Index + TPPID.TagPackNumPlace]; i++)
                                        {
                                            curbrtag = new BasicReportTag();
                                            System.Buffer.BlockCopy(Buffer, Index + TPPID.TagPackNumPlace + 1 + i * 4, curbrtag.Id, 0, 2);
                                            //基站的ID
                                            string strgroupid = curbrtag.Id[0].ToString("X2") + curbrtag.Id[1].ToString("X2");
                                            BsInfo bs = null;
                                            if (BsInfos.TryGetValue(strgroupid, out bs))
                                            {//设置基站讯息 
                                                System.Buffer.BlockCopy(bs.GroupID, 0, curbrtag.GroupID, 0, 2);
                                            }
                                            if (adpos == AfewDPos.Pos3Dim)
                                            {
                                                curbrtag.distanse = (ushort)(Buffer[Index + TPPID.TagPackNumPlace + 3 + i * 4] << 8 |
                                                                Buffer[Index + TPPID.TagPackNumPlace + 4 + i * 4]);
                                            }
                                            else
                                            {
                                                d = (ushort)(Buffer[Index + TPPID.TagPackNumPlace + 3 + i * 4] << 8 |
                                                         Buffer[Index + TPPID.TagPackNumPlace + 4 + i * 4]);
                                                if (BsInfos.TryGetValue(curbrtag.Id[0].ToString("X2") + curbrtag.Id[1].ToString("X2"), out brf))
                                                {
                                                    if (brf.Place.z < 0) curbrtag.distanse = d;
                                                    else
                                                    {
                                                        TagInfo mytginfo = null;
                                                        double p = 0d;
                                                        if (BasicTagInfos.TryGetValue(CurTagPack.ID[0].ToString("X2") + CurTagPack.ID[1].ToString("X2"), out mytginfo))
                                                        {
                                                            p = Math.Pow(d, 2) - Math.Pow(Math.Abs(brf.Place.z - mytginfo.height), 2);
                                                        }
                                                        else
                                                        {
                                                            p = Math.Pow(d, 2) - Math.Pow(brf.Place.z, 2);
                                                        }
                                                        if (p >= 0) curbrtag.distanse = Math.Pow(p, 0.5);
                                                        else curbrtag.distanse = 30;
                                                    }
                                                }
                                                else curbrtag.distanse = d;
                                            }
                                            curbrtag.Priority = (byte)i;
                                            CurTagPack.CurBasicReport.TryAdd(curbrtag.Id[0].ToString("X2") + curbrtag.Id[1].ToString("X2"), curbrtag);
                                        }
                                    }
                                    else
                                    {//说明是同一包数据,它的基站信息相同的，所以不用处理

                                    }
                                    CurTagPack.LocalType = Buffer[Index + 3];
                                    CurTagPack.Battery = Buffer[Index + 6];
                                    CurTagPack.CurBasicNum = Buffer[Index + 11 + 6];
                                    CurTagPack.TotalbasicNum = Buffer[Index + 11 + 6];
                                    CurTagPack.index = Buffer[Index + 2];
                                    CurTagPack.NoExeTime = (ushort)(Buffer[Index + 7] << 8 | Buffer[Index + 8]);
                                    CurTagPack.St = (ushort)(Buffer[Index + 9] << 8 | Buffer[Index + 10]);

                                    CurTagPack.GsensorX = (short)(Buffer[Index + 11] << 8 | Buffer[Index + 12]);
                                    CurTagPack.GsensorY = (short)(Buffer[Index + 13] << 8 | Buffer[Index + 14]);
                                    CurTagPack.GsensorZ = (short)(Buffer[Index + 15] << 8 | Buffer[Index + 16]);
                                    try { 
                                        SetTagPoint(CurTagPack);
                                    }
                                    catch (Exception) {

                                    }
                                }
                            }
                            else
                            {
                                CurTagPack = new TagPack();
                                System.Buffer.BlockCopy(Buffer, Index + 4, CurTagPack.ID, 0, 2);
                                BasicReportTag curbrtag = null;
                                for (int i = 0; i < Buffer[Index + TPPID.TagPackNumPlace]; i++)
                                {
                                    curbrtag = new BasicReportTag();
                                    System.Buffer.BlockCopy(Buffer, Index + TPPID.TagPackNumPlace + 1 + i * 4, curbrtag.Id, 0, 2);

                                    string strgroupid = curbrtag.Id[0].ToString("X2") + curbrtag.Id[1].ToString("X2");
                                    BsInfo bs = null;
                                    if (BsInfos.TryGetValue(strgroupid, out bs))
                                    {//设置基站讯息 
                                        System.Buffer.BlockCopy(bs.GroupID, 0, curbrtag.GroupID, 0, 2);
                                    }

                                    if (adpos == AfewDPos.Pos3Dim)
                                    {
                                        curbrtag.distanse = (ushort)(Buffer[Index + TPPID.TagPackNumPlace + 3 + i * 4] << 8 |
                                                        Buffer[Index + TPPID.TagPackNumPlace + 4 + i * 4]);
                                    }
                                    else
                                    {
                                        d = (ushort)(Buffer[Index + TPPID.TagPackNumPlace + 3 + i * 4] << 8 |
                                                         Buffer[Index + TPPID.TagPackNumPlace + 4 + i * 4]);
                                        if (BsInfos.TryGetValue(curbrtag.Id[0].ToString("X2") + curbrtag.Id[1].ToString("X2"), out brf))
                                        {
                                            if (brf.Place.z < 0) curbrtag.distanse = d;
                                            else
                                            {
                                                TagInfo mytginfo = null;
                                                double p = 0d;
                                                if (BasicTagInfos.TryGetValue(CurTagPack.ID[0].ToString("X2") + CurTagPack.ID[1].ToString("X2"), out mytginfo))
                                                {
                                                    p = Math.Pow(d, 2) - Math.Pow(Math.Abs(brf.Place.z - mytginfo.height), 2);
                                                }
                                                else
                                                {
                                                    p = Math.Pow(d, 2) - Math.Pow(brf.Place.z, 2);
                                                }
                                                if (p >= 0) curbrtag.distanse = Math.Pow(p, 0.5);
                                                else curbrtag.distanse = 30;
                                            }
                                        }
                                        else curbrtag.distanse = d;
                                    }
                                    curbrtag.Priority = (byte)i;
                                    CurTagPack.CurBasicReport.TryAdd(curbrtag.Id[0].ToString("X2") + curbrtag.Id[1].ToString("X2"), curbrtag);
                                }
                                CurTagPack.LocalType = Buffer[Index + 3];
                                CurTagPack.Battery = Buffer[Index + 6];
                                CurTagPack.CurBasicNum = Buffer[Index + 11 + 6];
                                CurTagPack.TotalbasicNum = Buffer[Index + 11 + 6];
                                CurTagPack.index = Buffer[Index + 2];
                                CurTagPack.NoExeTime = (ushort)(Buffer[Index + 7] << 8 | Buffer[Index + 8]);
                                CurTagPack.St = (ushort)(Buffer[Index + 9] << 8 | Buffer[Index + 10]);

                                CurTagPack.GsensorX = (short)(Buffer[Index + 11] << 8 | Buffer[Index + 12]);
                                CurTagPack.GsensorY = (short)(Buffer[Index + 13] << 8 | Buffer[Index + 14]);
                                CurTagPack.GsensorZ = (short)(Buffer[Index + 15] << 8 | Buffer[Index + 16]);

                                try 
                                { 
                                    SetTagPoint(CurTagPack);
                                }
                                catch (Exception)
                                {
                                }
                                TagPacks.TryAdd(CurTagPack.ID[0].ToString("X2") + CurTagPack.ID[1].ToString("X2"), CurTagPack);
                            }
                            System.Buffer.BlockCopy(Buffer, Index + TPPID.TagStillLen + 1 + Buffer[Index + TPPID.TagPackNumPlace] * 4, Buffer, Index, BufferLen);
                            BufferLen -= (TPPID.TagStillLen + 1 + CurTagPack.TotalbasicNum * 4);
                        }
                        else
                        {
                            //说明校验位错误
                            Array.Copy(Buffer, Index + 1, Buffer, 0, BufferLen);
                            BufferLen -= (Index + 1);
                        }
                    }
                    else if (Buffer[Index + 1] == TPPID.RouterType && Buffer[Index + TPPID.RouterPackLen - 1] == TPPID.End)
                    {//Router数据包
                        cs = 0;
                        for (int i = Index; i < Index + TPPID.RouterPackLen - 2; i++) 
                            cs += Buffer[i];
                        if (cs == Buffer[Index + TPPID.RouterPackLen - 2])
                        {
                            //判断当前的小版本是否相同
                            string strversion = Buffer[Index + TPPID.RouterVersionLen + 3].ToString("X2").Substring(1, 1);
                            if (!strversion.Equals(smallversion.ToString()))
                            {
                                int Id = (short)(Buffer[Index + 2] << 8 | Buffer[Index + 3]);
                                PostMessage(CurHandler, TPPID.WM_VERSION_ERR, TPPID.WSMALLVERSION, (IntPtr)Id);
                            }
                            strversion = Buffer[Index + TPPID.RouterVersionLen + 3].ToString("X2").Substring(0, 1);
                            //判断当前大版本是否正确
                            if (!strversion.Equals(bigversion.ToString()))
                            {
                                int Id = (short)(Buffer[Index + 2] << 8 | Buffer[Index + 3]);
                                PostMessage(CurHandler, TPPID.WM_VERSION_ERR, TPPID.WBIGVERSION, (IntPtr)Id);
                            }
                            StrBsstID = Buffer[Index + 2].ToString("X2") + Buffer[Index + 3].ToString("X2");
                            if (BasicStations.ContainsKey(StrBsstID))
                            {
                                BasicStations.TryGetValue(StrBsstID, out Bsst);
                                if (null != Bsst)
                                {
                                    Bsst.SleepTime = (ushort)(Buffer[Index + 4] << 8 | Buffer[Index + 5]);
                                    Bsst.Version = (uint)(Buffer[Index + 6] << 24 | Buffer[Index + 7] << 16 | Buffer[Index + 8] << 8 | Buffer[Index + 9]);
                                    Bsst.TickCount = Environment.TickCount;
                                    //向上位抛消息
                                    pointer = GetBasicStationPtr(Bsst);
                                    if (pointer != IntPtr.Zero)
                                    {
                                        PostMessage(CurHandler, TPPID.WM_TAG_PACK, TPPID.WBASIC_TYPE, pointer);
                                    }
                                }
                            }
                            else
                            {
                                Bsst = new BasicStation();
                                Array.Copy(Buffer, Index + 2, Bsst.ID, 0, 2);
                                Bsst.SleepTime = (ushort)(Buffer[Index + 4] << 8 | Buffer[Index + 5]);
                                Bsst.Version = (uint)(Buffer[Index + 6] << 24 | Buffer[Index + 7] << 16 | Buffer[Index + 8] << 8 | Buffer[Index + 9]);
                                Bsst.TickCount = Environment.TickCount;
                                BasicStations.TryAdd(StrBsstID, Bsst);
                                pointer = GetBasicStationPtr(Bsst);
                                if (IntPtr.Zero != pointer)
                                { 
                                    PostMessage(CurHandler, TPPID.WM_TAG_PACK, TPPID.WBASIC_TYPE, pointer);
                                }
                            }
                            //校验错误，舍弃该包数据
                            Array.Copy(Buffer, Index + TPPID.RouterPackLen, Buffer, Index, BufferLen);
                            BufferLen -= TPPID.RouterPackLen;
                        }
                        else
                        {
                            //说明校验位错误
                            Array.Copy(Buffer, Index + 1, Buffer, 0, BufferLen);
                            BufferLen -= (Index + 1);
                        }
                    }
                    else
                    {
                        //可能其他数据包上报上来，而它的数据包中存在包头Head
                        //舍弃Index之前的数据包 
                        Array.Copy(Buffer, Index + 1, Buffer, 0, BufferLen);
                        BufferLen -= (Index + 1);
                    }
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="brtag"></param>
        /// <param name="bestthree"></param>
        /// <param name="bsrptags"></param>
        private static void GetOtherBasicReportTag(List<BasicReportTag> brtag,BasicReportTag[] bestthree,ref BasicReportTag[] bsrptags)
        {
            int i = 0, len = 0;
            string[] strs = new string[bestthree.Length];
            for (i = 0; i < bestthree.Length; i++)
            {
                strs[i] = bestthree[i].Id[0].ToString("X2") + bestthree[i].Id[1].ToString("X2");
            }
            len = (brtag.Count >= 5) ? 2 : (brtag.Count - bestthree.Length);
            if (len <= 0) return;
            bsrptags = new BasicReportTag[len];
            int index = 0;
            bool flag = false;
            for (i = 0; i < brtag.Count; i++)
            {
                string strid = brtag[i].Id[0].ToString("X2") + brtag[i].Id[1].ToString("X2");
                flag = false;
                for (int j = 0; j < strs.Length; j++)
                {
                    if (strid.Equals(strs[j]))
                    {
                        flag = true; break;
                    }
                }
                if (!flag)
                {
                    bsrptags[index++] = brtag[i];
                }
            }
        }
        /// <summary>
        /// 获取集合中指定Id的数据
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="strid"></param>
        /// <returns></returns>
        private static AnchorDataFormat GetIterateData(List<AnchorDataFormat> lists, string strid)
        {
            string strcurid = "";
            foreach (AnchorDataFormat anchordatas in lists)
            {
                strcurid = anchordatas.IterateData.ID[0].ToString("X2") + anchordatas.IterateData.ID[1].ToString("X2");
                if (strcurid.Equals(strid))
                {
                    return anchordatas;
                }
            }
            return null;
        }
        /// <summary>
        /// 泰勒展开定位算法
        /// </summary>
        /// <param name="pa">定位基站A坐标</param>
        /// <param name="pb">定位基站B坐标</param>
        /// <param name="pc">定位基站C坐标</param>
        /// <param name="ra">距离A的半径</param>
        /// <param name="rb">距离B的半径</param>
        /// <param name="rc">距离C的半径</param>
        private static Point TaylorExpandsLocation(Point pa, Point pb, Point pc, double ra, double rb, double rc)
        {
            double x1 = 0, y1 = 0, x2 = 0, y2 = 0, x3 = 0, y3 = 0, r1 = 0, r2 = 0, r3 = 0, xo = 0, yo = 0;
            x1 = pa.x; y1 = pa.y; x2 = pb.x; y2 = pb.y; x3 = pc.x; y3 = pc.y; r1 = ra; r2 = rb; r3 = rc;
            IterateDataFormat optimumData = new IterateDataFormat(-1, -1, -1);
            double[,] m = new double[,]{{1, -2 * x1, -2 * y1},
                                        {1, -2 * x2, -2 * y2},
                                        {1, -2 * x3, -2 * y3},
                                        };
            try
            {
                Matrix A = new Matrix(m);
                double[,] n = new double[,]{{ r1 * r1 - x1 * x1 - y1 * y1},
                                        { r2 * r2 - x2 * x2 - y2 * y2},
                                        { r3 * r3 - x3 * x3 - y3 * y3},
                                        };
                Matrix b = new Matrix(n);
                Matrix At = A.Matrixtran();
                Matrix K = At * A;
                Matrix A1 = K.InverseMatrix();
                Matrix X = new Matrix();
                X = A1 * At * b;

                double x10 = 0.0, y10 = 0.0;
                xo = X[1, 0];
                yo = X[2, 0];
                x10 = xo; y10 = yo;

                List<IterateDataFormat> iterateData = new List<IterateDataFormat> { };
                iterateData.Add(new IterateDataFormat(x1, y1, r1));
                iterateData.Add(new IterateDataFormat(x2, y2, r2));
                iterateData.Add(new IterateDataFormat(x3, y3, r3));
                IterateDataFormat initData = new IterateDataFormat(xo, yo, 0);

                optimumData = Iterate.TaylorIterate2(initData, iterateData, 5);
                if (optimumData.X == double.MaxValue && optimumData.Y == double.MaxValue && optimumData.R == double.MaxValue)
                {
                    optimumData = Iterate.ManualIterate2(initData, iterateData, 100);
                    optimumData = Iterate.ManualIterate2(optimumData, iterateData, 80);
                    optimumData = Iterate.ManualIterate2(optimumData, iterateData, 60);
                    optimumData = Iterate.ManualIterate2(optimumData, iterateData, 40);
                    optimumData = Iterate.ManualIterate2(optimumData, iterateData, 20);
                    optimumData = Iterate.ManualIterate2(optimumData, iterateData, 10);
                    optimumData = Iterate.ManualIterate2(optimumData, iterateData, 5);
                    optimumData = Iterate.ManualIterate2(optimumData, iterateData, 2);
                    optimumData = Iterate.ManualIterate2(optimumData, iterateData, 1);
                }
            }
            catch (Exception)
            {
                optimumData.X = -1;
                optimumData.Y = -1;
            }
            xo = optimumData.X;
            yo = optimumData.Y;
            return new Point(xo, yo,0);
        }
        /// <summary>
        /// 求两圆的交点坐标
        /// </summary>
        /// <returns></returns>
        private static TwoCirInterMode TwoCircularInterCoor(Point A, Point B, double Ra, double Rb, out Point[] Point)
        {
            Point = new Point[2];
            #region
            /*
             * 两条直线的交线方程为:2(X1 - X2)X + 2（Y1 - Y2）Y = 
             * R2^2 - R1^2 + X1^2 - X2^2 + Y1^2 - Y2^2;
             */
            #endregion
            double K1 = Math.Pow(Rb, 2) - Math.Pow(Ra, 2) +
                        Math.Pow(A.x, 2) - Math.Pow(B.x, 2) +
                        Math.Pow(A.y, 2) - Math.Pow(B.y, 2);
            if (A.y == B.y) A.y = A.y + 1;
            if (A.x == B.x) A.x = A.x + 1;
            double K2 = K1 / (2 * (A.y - B.y));
            double K3 = (A.x - B.x) / (A.y - B.y);
            #region
            /*此时的直线方程为 Y = K2 - K3*X;
             * 将直线方程带入圆的方程中 (X-Xa)^2 + (Y-Ya)^2 = Ra^2;
             * 得到的一元二次方程为:(X-Xa)^2 + (K2 - K3X-Ya)^2 = Ra^2;
            */
            #endregion
            double K4 = K2 - A.y;
            #region
            /*(X-Xa)^2 + (K4 - K3X)^2 = Ra^2;
             * 化简得到:(K3^2+1)X^2 - 2(Xa+K3*K4)*X + Xa^2 + K4^2 - Ra^2 = 0
             */
            #endregion
            double K5 = Math.Pow(K3, 2) + 1;
            double K6 = -2 * (A.x + K3 * K4);
            double K7 = Math.Pow(A.x, 2) + Math.Pow(K4, 2) - Math.Pow(Ra, 2);
            #region
            //等式化简得到 K5 * X^2 + 2*K6*X + (K7 - Ra^2)=0
            //此时计算X的坐标
            #endregion
            double K8 = Math.Pow(K6, 2) - 4 * K5 * K7;
            double P1 = Math.Sqrt(K8);
            double X1, X2, Y1, Y2;
            #region
            //计算出X坐标后将X坐标带入直线方程即可计算出Y坐标
            // Y = K2 - K3*X
            #endregion
            TwoCirInterMode CurTwoCirInterMode = TwoCirInterMode.UnKnown;
            if (K8 < 0)
            {//说明没有交点
                X1 = X2 = Y1 = Y2 = -1;
                CurTwoCirInterMode = TwoCirInterMode.No;
            }
            else if (K8 == 0)
            {//说明只有一个交点
                X1 = X2 = (-2 * K6 + P1) / (2 * K5);
                Y1 = Y2 = K2 - K3 * X1;
                CurTwoCirInterMode = TwoCirInterMode.TT1;
            }
            else
            {//说明有两个交点
                X1 = (-K6 + P1) / (2 * K5);
                X2 = (-K6 - P1) / (2 * K5);
                Y1 = K2 - K3 * X1;
                Y2 = K2 - K3 * X2;
                CurTwoCirInterMode = TwoCirInterMode.TT2;
            }
            Point[0] = new Point(X1, Y1);
            Point[1] = new Point(X2, Y2);
            return CurTwoCirInterMode;
        }
        /// <summary>
        /// 求两圆圆心连线与两圆的交点，取出靠内的两个焦点
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="Ra"></param>
        /// <param name="Rb"></param>
        private static void P2P_LineInterCoor(Point A, Point B, double Ra, double Rb, out Point[] P2P_LineInterCoors)
        {
            P2P_LineInterCoors = new Point[2];
            /*  一、判断方式
             * 1、判断两圆的位置关系，
             * 2、是外面相离还是里面相离
             * 3、外面相离的话,取中间两个点
             * 4、里面相离的话，取大圆上靠近小圆的点 A，再以A点为基础取小圆上靠近A的点
             * 二、判断方式
             * 取大圆上靠近小圆的点 A，再以A点为基础取小圆上靠近A的点
             */

            //A、B两圆圆心的距离
            double Dab = Distance2Point(A, B);
            double Sab = Math.Abs(Ra - Rb);
            double Tab = Ra + Rb;
            #region
            /*已知两圆的圆心求直线方程
                 * K = (Ya-Yb)/(Xa-Xb)
                 * B = Ya - K*Xa
                 * 直线方程为:Y = K*X + B
                 */
            #endregion
            if (A.x == B.x) A.x = A.x + 1;
            double K1 = (A.y - B.y) / (A.x - B.x);
            double K2 = A.y - K1 * A.x;
            #region
            /*
                 * 将直线方程带入A圆方程，计算与本圆的交点
                 * （X-Xa）^2+(K1*X + K2 - Ya)^2 = R^2
                 */
            #endregion
            double K3 = K2 - A.y;
            #region
            /*将参数带入上式中可得: (X-Xa)^2+(K1*X + K3)^2 = R^2
                 * 化简可得：（K1^2 + 1）*X^2 + 2(K1*K3 - Xa)*X + Xa^2 + K3^2 - Ra^2 = 0
                 * 计算出两个坐标
                 */
            #endregion
            double K4 = Math.Pow(2 * (K1 * K3 - A.x), 2) - 4 * (Math.Pow(K1, 2) + 1) * (Math.Pow(A.x, 2) + Math.Pow(K3, 2) - Math.Pow(Ra, 2));
            //此时K4一定是大于0，否则就有问题了
            if (K4 < 0) return;
            double X1, X2, Y1, Y2;
            X1 = (-2 * (K1 * K3 - A.x) + Math.Sqrt(K4)) / (2 * (Math.Pow(K1, 2) + 1));
            X2 = (-2 * (K1 * K3 - A.x) - Math.Sqrt(K4)) / (2 * (Math.Pow(K1, 2) + 1));
            Y1 = K1 * X1 + K2;
            Y2 = K1 * X2 + K2;
            //计算与B圆的交点的其中一个交点
            /*
             * 将直线方程代入到B圆的方程中：
             * （X-Xb）^2+(K1*X + K2 - Yb)^2 = Rb^2
             */
            double K5 = K2 - B.y;
            double K6 = Math.Pow(2 * (K1 * K5 - B.x), 2) - 4 * (Math.Pow(K1, 2) + 1) * (Math.Pow(B.x, 2) + Math.Pow(K5, 2) - Math.Pow(Rb, 2));
            double X3, X4, Y3, Y4;
            X3 = (-2 * (K1 * K5 - B.x) + Math.Sqrt(K6)) / (2 * (Math.Pow(K1, 2) + 1));
            X4 = (-2 * (K1 * K5 - B.x) - Math.Sqrt(K6)) / (2 * (Math.Pow(K1, 2) + 1));
            Y3 = K1 * X3 + K2;
            Y4 = K1 * X4 + K2;
            Point P1, P2, P3, P4;
            P1 = new Point(X1, Y1);
            P2 = new Point(X2, Y2);
            P3 = new Point(X3, Y3);
            P4 = new Point(X4, Y4);
            //判断哪一个坐标位于 两圆心中间位置
            double MaxX = A.y, MaxY = A.y;
            //double MinX, MinY;
            //if (Mode == 0)
            {//两个圆内相离
                if (Ra > Rb)
                {//A是大圆
                    //得到这两个点距离靠近B的那个点
                    double da1 = Distance2Point(B, P1);
                    double da2 = Distance2Point(B, P2);
                    if (da1 < da2) P2P_LineInterCoors[0] = P1;
                    else P2P_LineInterCoors[0] = P2;

                    double db1 = Distance2Point(P2P_LineInterCoors[0], P3);
                    double db2 = Distance2Point(P2P_LineInterCoors[0], P4);
                    if (db1 < db2) P2P_LineInterCoors[1] = P3;
                    else P2P_LineInterCoors[1] = P4;
                }
                else
                { //B是大圆
                    double db3 = Distance2Point(A, P3);
                    double db4 = Distance2Point(A, P4);
                    if (db3 < db4) P2P_LineInterCoors[1] = P3;
                    else P2P_LineInterCoors[1] = P4;
                    double dba1 = Distance2Point(P2P_LineInterCoors[1], P1);
                    double dba2 = Distance2Point(P2P_LineInterCoors[1], P2);
                    if (dba1 < dba2) P2P_LineInterCoors[0] = P1;
                    else P2P_LineInterCoors[0] = P2;
                }
            }
        }

        private static double GetArea(Point p0, Point p1, Point p2)
        {
            double area = 0;
            area = p0.x * p1.y + p1.x * p2.y + p2.x * p0.y
                 - p1.x * p0.y - p2.x * p1.y - p0.x * p2.y;
            return area / 2;  // 另外在求解的过程中，不需要考虑点的输入顺序是顺时针还是逆时针，相除后就抵消了。
        }

        /// <summary>
        /// 求多边形的重心
        /// </summary>
        /// <param name="Px"></param>
        /// <returns></returns>
        private static Point GetPolygonCenterOfGravity(Point[] Px)
        {
            double sum_x = 0, sum_y = 0, sum_area = 0, area = 0, G_x = -1, G_y = -1;
            if (Px.Length < 3) return PException.P1;
            for (int i = 2; i < Px.Length; ++i)
            {
                area = GetArea(Px[0], Px[1], Px[i]);
                sum_area += area;
                sum_x += (Px[0].x + Px[i - 1].x + Px[i].x) * area;
                sum_y += (Px[0].y + Px[i - 1].y + Px[i].y) * area;
            }
            G_x = sum_x / sum_area / 3;
            G_y = sum_y / sum_area / 3;
            Point G_P = new Point(G_x, G_y);
            return G_P;
        }
        private static Point P3Planscan_8Mode(Point A, Point B, Point C, double Ra, double Rb, double Rc)
        {
            Point p = null;
            ArrayList PolygonPes = new ArrayList();
            double d1, d2;
            //判断A、B两圆的位置关系
            Point[] PinterAB = null;
            TwoCirInterMode curABcirmode = TwoCircularInterCoor(A, B, Ra, Rb, out PinterAB);
            switch (curABcirmode)
            {
                case TwoCirInterMode.No:
                    #region
                    /*1、取A、B两圆圆心的连线
                         *2、求连线与两圆的交点
                         *3、分别取去两圆交点中靠的较近的两个点
                    */
                    #endregion
                    P2P_LineInterCoor(A, B, Ra, Rb, out PinterAB);
                    PolygonPes.Add(PinterAB[0]);
                    PolygonPes.Add(PinterAB[1]);
                    break;
                case TwoCirInterMode.TT1:
                    PolygonPes.Add(PinterAB[0]);
                    break;
                case TwoCirInterMode.TT2:
                    #region
                    /*
                     *  1、相交于两个点
                     *  2、计算一个点与圆边的距离
                     *  3、找出距离最小的交点
                     */
                    #endregion
                    d1 = Distance2Point(PinterAB[0], C);
                    d2 = Distance2Point(PinterAB[1], C);
                    d1 = Math.Abs(d1 - Rc);
                    d2 = Math.Abs(d2 - Rc);
                    if (d1 < d2)
                    {//取PinterAB[0]
                        PolygonPes.Add(PinterAB[0]);
                    }
                    else
                    {//取PinterAB[1]
                        PolygonPes.Add(PinterAB[1]);
                    }
                    break;
                case TwoCirInterMode.UnKnown:
                    break;
            }
            //判断A、C两圆的位置关系
            Point[] PinterAC = null;
            TwoCirInterMode curACcirmode = TwoCircularInterCoor(A, C, Ra, Rc, out PinterAC);
            switch (curACcirmode)
            {
                case TwoCirInterMode.No:
                    P2P_LineInterCoor(A, C, Ra, Rc, out PinterAC);
                    PolygonPes.Add(PinterAC[0]);
                    PolygonPes.Add(PinterAC[1]);
                    break;
                case TwoCirInterMode.TT1:
                    PolygonPes.Add(PinterAC[0]);
                    break;
                case TwoCirInterMode.TT2:
                    d1 = Distance2Point(PinterAC[0], B);
                    d2 = Distance2Point(PinterAC[1], B);
                    d1 = Math.Abs(d1 - Rb);
                    d2 = Math.Abs(d2 - Rb);
                    if (d1 < d2) PolygonPes.Add(PinterAC[0]);
                    else PolygonPes.Add(PinterAC[1]);
                    break;
                case TwoCirInterMode.UnKnown:
                    break;
            }
            //判断B、C两圆的位置关系
            Point[] PinterBC = null;
            TwoCirInterMode curBCcirmode = TwoCircularInterCoor(B, C, Rb, Rc, out PinterBC);
            switch (curBCcirmode)
            {
                case TwoCirInterMode.No:
                    P2P_LineInterCoor(B, C, Rb, Rc, out PinterBC); PolygonPes.Add(PinterBC[0]); PolygonPes.Add(PinterBC[1]); break;
                case TwoCirInterMode.TT1:
                    PolygonPes.Add(PinterBC[0]);
                    break;
                case TwoCirInterMode.TT2:
                    d1 = Distance2Point(PinterBC[0], A);
                    d2 = Distance2Point(PinterBC[1], A);
                    d1 = Math.Abs(d1 - Ra);
                    d2 = Math.Abs(d2 - Ra);
                    if (d1 < d2) PolygonPes.Add(PinterBC[0]);
                    else PolygonPes.Add(PinterBC[1]);
                    break;
                case TwoCirInterMode.UnKnown:
                    break;
            }
            if (curABcirmode == TwoCirInterMode.TT2 &&
                curACcirmode == TwoCirInterMode.TT2 &&
                curBCcirmode == TwoCirInterMode.TT2 &&
                PolygonPes.Count == 3)
            {//说明此时两两相交
                #region
                /*
                 * 找去选取的3个点中距离最短的边
                 * 再在其他点中找去距离最短边，距离最短边中点最短的点
                 */
                #endregion
                object[] temps = PolygonPes.ToArray();
                PolygonPes.Clear();
                double p1, p2, p3, mx = 0, my = 0;
                p1 = Distance2Point((Point)temps[0], (Point)temps[1]);
                p2 = Distance2Point((Point)temps[0], (Point)temps[2]);
                p3 = Distance2Point((Point)temps[1], (Point)temps[2]);
                if (p1 < p2 && p1 < p3)
                {//最小的是p1
                    mx = (((Point)temps[0]).x + ((Point)temps[1]).x) / 2;
                    my = (((Point)temps[0]).y + ((Point)temps[1]).y) / 2;
                    PolygonPes.Add(temps[0]); PolygonPes.Add(temps[1]);
                }
                else if (p2 < p1 && p2 < p3)
                {//最小的是p2
                    mx = (((Point)temps[0]).x + ((Point)temps[2]).x) / 2;
                    my = (((Point)temps[0]).y + ((Point)temps[2]).y) / 2;
                    PolygonPes.Add(temps[0]); PolygonPes.Add(temps[2]);
                }
                else if (p3 < p1 && p3 < p2)
                {//最小的是p3
                    mx = (((Point)temps[1]).x + ((Point)temps[2]).x) / 2;
                    my = (((Point)temps[1]).y + ((Point)temps[2]).y) / 2;
                    PolygonPes.Add(temps[1]); PolygonPes.Add(temps[2]);
                }
                Point middl_p = new Point(mx, my);
                //计算其他各个交点与中点的距离
                double b1, b2, b3, b4, b5, b6;
                b1 = b2 = b3 = b4 = b5 = b6 = double.MaxValue;
                if (PinterAB[0].x != ((Point)PolygonPes[0]).x && PinterAB[0].y != ((Point)PolygonPes[0]).y && PinterAB[0].x != ((Point)PolygonPes[1]).x && PinterAB[0].y != ((Point)PolygonPes[1]).y) b1 = Distance2Point(PinterAB[0], middl_p);
                if (PinterAB[1].x != ((Point)PolygonPes[0]).x && PinterAB[1].y != ((Point)PolygonPes[0]).y && PinterAB[1].x != ((Point)PolygonPes[1]).x && PinterAB[1].y != ((Point)PolygonPes[1]).y) b2 = Distance2Point(PinterAB[1], middl_p);
                if (PinterAC[0].x != ((Point)PolygonPes[0]).x && PinterAC[0].y != ((Point)PolygonPes[0]).y && PinterAC[0].x != ((Point)PolygonPes[1]).x && PinterAC[0].y != ((Point)PolygonPes[1]).y) b3 = Distance2Point(PinterAC[0], middl_p);
                if (PinterAC[1].x != ((Point)PolygonPes[0]).x && PinterAC[1].y != ((Point)PolygonPes[0]).y && PinterAC[1].x != ((Point)PolygonPes[1]).x && PinterAC[1].y != ((Point)PolygonPes[1]).y) b4 = Distance2Point(PinterAC[1], middl_p);
                if (PinterBC[0].x != ((Point)PolygonPes[0]).x && PinterBC[0].y != ((Point)PolygonPes[0]).y && PinterBC[0].x != ((Point)PolygonPes[1]).x && PinterBC[0].y != ((Point)PolygonPes[1]).y) b5 = Distance2Point(PinterBC[0], middl_p);
                if (PinterBC[1].x != ((Point)PolygonPes[0]).x && PinterBC[1].y != ((Point)PolygonPes[0]).y && PinterBC[1].x != ((Point)PolygonPes[1]).x && PinterBC[1].y != ((Point)PolygonPes[1]).y) b6 = Distance2Point(PinterBC[1], middl_p);
                //判断哪一个最近
                double Mind = double.MaxValue;
                if (Mind > b1) Mind = b1; if (Mind > b2) Mind = b2; if (Mind > b3) Mind = b3;
                if (Mind > b4) Mind = b4; if (Mind > b5) Mind = b5; if (Mind > b6) Mind = b6;
                if (Mind == b1)
                {
                    PolygonPes.Add(PinterAB[0]);
                }
                else if (Mind == b2)
                {
                    PolygonPes.Add(PinterAB[1]);
                }
                else if (Mind == b3)
                {
                    PolygonPes.Add(PinterAC[0]);
                }
                else if (Mind == b4)
                {
                    PolygonPes.Add(PinterAC[1]);
                }
                else if (Mind == b5)
                {
                    PolygonPes.Add(PinterBC[0]);
                }
                else if (Mind == b6)
                {
                    PolygonPes.Add(PinterBC[1]);
                }
            }
            Point[] TempPes = new Point[PolygonPes.Count];
            int i = 0;
            for (i = 0; i < PolygonPes.Count; i++)
            {
                TempPes[i] = (Point)PolygonPes[i];
            }
            p = GetPolygonCenterOfGravity(TempPes);
            if (p.x < 0 || p.y < 0)
            {//预测点超出地图
                return PException.P2;
            }
            if (Double.IsNaN(p.x) || Double.IsNaN(p.y) ||
                Double.IsNegativeInfinity(p.x) || Double.IsNegativeInfinity(p.y) ||
                Double.IsPositiveInfinity(p.x) || Double.IsPositiveInfinity(p.y)
                )
            {
                return PException.P1;
            }
            CurInterNum = PolygonPes.Count;
            for (i = 0; i < CurInterNum; i++)
            {
                InterTemp[i] = TempPes[i];
            }
            return p;
        }
        private static Point CurPoint = null;
        private static List<IterateDataFormat> ResidualList = null;

        private static bool TwoDimCoorCalculation(TagPack cdImg, Grouping gp)
        {
            //选择到组别以后就可以对组别中的基站进行选择了
            BasicReportTag[] bestreporttags = null;
            GetBestThreeStation(gp.bsgd, out bestreporttags);
            if (null == bestreporttags || bestreporttags.Length < 3)
            {
                return false;
            }
            BsInfo p1 = null, p2 = null, p3 = null;
            BsInfos.TryGetValue(bestreporttags[0].Id[0].ToString("X2") + bestreporttags[0].Id[1].ToString("X2"),out p1);
            BsInfos.TryGetValue(bestreporttags[1].Id[0].ToString("X2") + bestreporttags[1].Id[1].ToString("X2"), out p2);
            BsInfos.TryGetValue(bestreporttags[2].Id[0].ToString("X2") + bestreporttags[2].Id[1].ToString("X2"), out p3);
            if (p1 == null || p2 == null || p3 == null)
            {
                return false;
            }
            try
            {
                CurPoint = TaylorExpandsLocation(p1.Place, p2.Place, p3.Place, bestreporttags[0].distanse, bestreporttags[1].distanse, bestreporttags[2].distanse);
                if (CurPoint.x <= 0 || CurPoint.y <= 0)
                {
                    CurPoint = P3Planscan_8Mode(p1.Place, p2.Place, p3.Place, bestreporttags[0].distanse, bestreporttags[1].distanse, bestreporttags[2].distanse);
                }
            }
            catch (Exception)
            {
                return false;
            }
            string strid = "";
            BasicReportTag bstg = null;
            foreach (BasicReportTag bsrepor in bestreporttags)
            {
                strid = bsrepor.Id[0].ToString("X2") + bsrepor.Id[1].ToString("X2");
                if (cdImg.CurBasicReport.TryGetValue(strid, out bstg))
                {
                    bstg.ResidualValue = 1;
                }
            }
            return true;
        }
        private class Grouping
        { 
            public byte[] GroupID = new byte[2];
            public List<BasicReportTag> bsgd = new List<BasicReportTag>();
        }
        private static Dictionary<string, Grouping> Groupings = new Dictionary<string, Grouping>();
        private static bool ThreeDimCoorCalculation(TagPack cdImg, Grouping gp)
        {
            if (null == ResidualList)
            {
                ResidualList = new List<IterateDataFormat>();
            }
            else
            {
                ResidualList.Clear();
            }
            //遍历Tag上报上来的基站讯息
            foreach (BasicReportTag brtag in gp.bsgd)
            {
                if (null == brtag)
                {
                    continue;
                }
                if (ResidualList.Count >= 3)
                {
                    //主要添加前面的四个点
                    break;
                }
                BsInfo bs = null;
                if (BsInfos.TryGetValue(brtag.Id[0].ToString("X2") + brtag.Id[1].ToString("X2"), out bs))
                {
                    IterateDataFormat p = new IterateDataFormat(brtag.Id, bs.Place.x, bs.Place.y, bs.Place.z, brtag.distanse);
                    ResidualList.Add(p);
                }
            }
            try
            {
                if (ResidualList.Count < 3)
                {
                    return false;
                }
                IterateDataFormat rsdlformat = Get_LeastCoordinates(ResidualList);
                if (null == rsdlformat)
                {
                    return false;
                }
                CurPoint = new Point(rsdlformat.X, rsdlformat.Y, rsdlformat.Z);
                if (ResidualList.Count == 3)
                {//如果只是选择三个基站就不显示高度了
                    CurPoint.z = Double.MinValue;
                }
                BasicReportTag reporttag = null;
                //重新设置基站的ResidualValue值，已经选择的设置为1，没有选择的设置为0
                foreach (IterateDataFormat it in ResidualList)
                {
                    string strid = it.ID[0].ToString("X2") + it.ID[1].ToString("X2");
                    if (cdImg.CurBasicReport.TryGetValue(strid, out reporttag))
                    {
                        reporttag.ResidualValue = 1;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 计算Tag的地址
        /// </summary>
        private static void SetTagPoint(TagPack cdImg)
        {
            if (CurReportMode == ReportMode.ImgMode)
            {
                Groupings.Clear();
                //重新设置
                List<BasicReportTag> bstags = new List<BasicReportTag>();
                foreach (KeyValuePair<string, BasicReportTag> brt in cdImg.CurBasicReport)
                {
                    if (null == brt.Value)
                        continue;
                    if (brt.Value.distanse == UInt16.MaxValue)
                        continue;
                    if (BsInfos.ContainsKey(brt.Key))
                    {
                        BasicReportTag brptg = new BasicReportTag();
                        CopyBasicReportTag(brt.Value, brptg);
                        bstags.Add(brptg);
                    }
                }
                if (CurPositionMode == PosititionMode.Closestdistance) {
                    BubbleSort_ClosestDistance(ref bstags);
                } else {
                    BubbleSort_QualityPriority(ref bstags);
                }
                Grouping grsp = null;
                // 给所有上报上来的基站按组别分组，哪一组基站最多就以当前组为卡片所处的组别
                foreach (BasicReportTag brtag in bstags) {
                    string strgroupid = brtag.GroupID[0].ToString("X2") + brtag.GroupID[1].ToString("X2");
                    if (!Groupings.ContainsKey(strgroupid)) {
                        grsp = new Grouping();
                        System.Buffer.BlockCopy(brtag.GroupID, 0, grsp.GroupID, 0, 2);
                        grsp.bsgd.Add(brtag);
                        Groupings.Add(strgroupid, grsp);
                    } else {
                        Groupings.TryGetValue(strgroupid, out grsp);
                        grsp.bsgd.Add(brtag);
                    }
                }
                int num = 0;
                string groupid = "";
                //选择基站最多的组别
                foreach (KeyValuePair<string, Grouping> grp in Groupings) {
                    if (grp.Value.bsgd.Count > num) {
                        num = grp.Value.bsgd.Count;
                        groupid = grp.Key;
                    }
                }
                Grouping gp = null;
                Groupings.TryGetValue(groupid, out gp);
                if (null == gp)
                    return;
                if (gp.bsgd.Count >= 3) {
                    System.Buffer.BlockCopy(gp.GroupID, 0, cdImg.GroupID, 0, 2);
                    bool isMark = false;
                    if (adpos == AfewDPos.Pos3Dim)
                    {//三维的方式
                        isMark = ThreeDimCoorCalculation(cdImg, gp);
                    }
                    else
                    {//两维的方式
                        isMark = TwoDimCoorCalculation(cdImg, gp);
                    }
                    if (!isMark)
                        return;
                    if (CurPoint.x <= 0 || CurPoint.y <= 0)
                    {
                        return;
                    }
                    if (CurPoint.x == Double.MaxValue || CurPoint.y == Double.MaxValue || CurPoint.z == Double.MaxValue)
                    {
                        return;
                    }
                    IntPtr pointer = GetTagLocalPackPtr(cdImg, new Point(CurPoint.x, CurPoint.y, CurPoint.z));
                    if (pointer != IntPtr.Zero)
                    {
                        PostMessage(CurHandler, TPPID.WM_TAG_PACK, TPPID.WPARAM_TYPE, pointer);
                    }
                } else {
                    IntPtr ptr = getTagMessagePtr(cdImg, gp);
                    PostMessage(CurHandler, TPPID.WM_TAG_PACK, TPPID.WREFERSMSG_TYPE, ptr);
                }
            }
            else if(CurReportMode == ReportMode.ListMode)
            {
                BasicReportTag[] bestthree = null;
                GetBasicPorts(cdImg.CurBasicReport,false,out bestthree);
                if (null == bestthree || bestthree.Length <= 0)
                {
                    return;
                }
                if (bestthree.Length == 1)
                {
                    System.Buffer.BlockCopy(bestthree[0].Id, 0, cdImg.RD1, 0, 2);
                    cdImg.Dis1 = bestthree[0].distanse;
                    cdImg.SigQuality1 = bestthree[0].Priority;
                    cdImg.RD2[0] = 0; cdImg.RD2[1] = 0;
                    cdImg.RD3[0] = 0; cdImg.RD3[1] = 0;
                    cdImg.Dis2 = int.MaxValue;
                    cdImg.Dis3 = int.MaxValue;
                    cdImg.SigQuality2 = int.MaxValue;
                    cdImg.SigQuality3 = int.MaxValue;
                }
                else if (bestthree.Length == 2)
                {
                    System.Buffer.BlockCopy(bestthree[0].Id, 0, cdImg.RD1, 0, 2);
                    System.Buffer.BlockCopy(bestthree[1].Id, 0, cdImg.RD2, 0, 2);
                    cdImg.Dis1 = bestthree[0].distanse;
                    cdImg.Dis2 = bestthree[1].distanse;
                    cdImg.SigQuality1 = bestthree[0].Priority;
                    cdImg.SigQuality2 = bestthree[1].Priority;
                    cdImg.RD3[0] = 0; cdImg.RD3[1] = 0;
                    cdImg.Dis3 = int.MaxValue;
                    cdImg.SigQuality3 = int.MaxValue;
                }
                else
                {
                    System.Buffer.BlockCopy(bestthree[0].Id, 0, cdImg.RD1, 0, 2);
                    System.Buffer.BlockCopy(bestthree[1].Id, 0, cdImg.RD2, 0, 2);
                    System.Buffer.BlockCopy(bestthree[2].Id, 0, cdImg.RD3, 0, 2);
                    cdImg.Dis1 = bestthree[0].distanse;
                    cdImg.Dis2 = bestthree[1].distanse;
                    cdImg.Dis3 = bestthree[2].distanse;
                    cdImg.SigQuality1 = bestthree[0].Priority;
                    cdImg.SigQuality2 = bestthree[1].Priority;
                    cdImg.SigQuality3 = bestthree[2].Priority;
                }
                BasicReportTag[] bsrptags = null;
                GetOtherBasicReportTag(cdImg.CurBasicReport.Values.ToList<BasicReportTag>(), bestthree, ref bsrptags);
                IntPtr pointer = GetTagPackPtr(cdImg, new Point(-1,-1, -1),bsrptags);
                if (pointer != IntPtr.Zero)
                { 
                    PostMessage(CurHandler, TPPID.WM_TAG_PACK, TPPID.WPARAM_TYPE, pointer);
                }  
            }
        }

        private static IntPtr getTagMessagePtr(TagPack tagPack, Grouping group)
        {
            TagPlace tagPlace = new TagPlace();
            tagPlace.ID = tagPack.ID;
            tagPlace.Battery = tagPack.Battery;
            tagPlace.index = tagPack.index;
            tagPlace.LocalType = tagPack.LocalType;
            tagPlace.NoExeTime = tagPack.NoExeTime;
            tagPlace.SleepTime = tagPack.St;
            tagPlace.GroupID = group.GroupID;

            if (group.bsgd.Count == 1)
            { // 当只有1个参考点时
                tagPlace.ReferID1 = group.bsgd[0].Id;
                tagPlace.Dis1 = group.bsgd[0].distanse;
                tagPlace.SigQuality1 = group.bsgd[0].Priority;

            }
            else if (group.bsgd.Count == 2)
            { // 当有2个参考点时
                tagPlace.ReferID1 = group.bsgd[0].Id;
                tagPlace.Dis1 = group.bsgd[0].distanse;
                tagPlace.SigQuality1 = group.bsgd[0].Priority;

                tagPlace.ReferID2 = group.bsgd[1].Id;
                tagPlace.Dis2 = group.bsgd[1].distanse;
                tagPlace.SigQuality2 = group.bsgd[1].Priority;
            }
            IntPtr record = IntPtr.Zero;
            try
            {
                int marLen = Marshal.SizeOf(typeof(TagPlace));
                record = Marshal.AllocHGlobal(marLen);
                Marshal.StructureToPtr(tagPlace, record, false);
            }
            catch (Exception)
            {

            }
            return record;
        }

        /// <summary>
        /// 找出各个条件都满足，且位置最好的基站
        /// </summary>
        /// <param name="BasicReports"></param>
        /// <param name="bsrts"></param>
        private static void GetBasicPorts(ConcurrentDictionary<string, BasicReportTag> BasicReports,bool flag,out BasicReportTag[] bsrts)
        {
            List<BasicReportTag> bstags = new List<BasicReportTag>();
            foreach (KeyValuePair<string, BasicReportTag> brt in BasicReports)
            {
                if (null == brt.Value) 
                    continue;
                if (brt.Value.distanse == UInt16.MaxValue)
                    continue;
                //判断是否有上传基站讯息
                if (flag)
                {
                    if (BsInfos.ContainsKey(brt.Key))
                    {
                        BasicReportTag brptg = new BasicReportTag();
                        CopyBasicReportTag(brt.Value, brptg);
                        bstags.Add(brptg);
                    }
                }
                else
                {
                    BasicReportTag brptg = new BasicReportTag();
                    CopyBasicReportTag(brt.Value, brptg);
                    bstags.Add(brptg);
                }
            }
            if (CurPositionMode == PosititionMode.Closestdistance)
            {
                BubbleSort_ClosestDistance(ref bstags);
            }
            else
            {
                BubbleSort_QualityPriority(ref bstags);
            }
            if (CurReportMode == ReportMode.ImgMode)
            {
                if (bstags.Count < 3)
                { 
                    bsrts = new BasicReportTag[bstags.Count];
                    for (int i = 0; i < bstags.Count; i++)
                    { 
                        bsrts[i] = bstags[i];
                    }
                }
                else GetBestThreeStation(bstags, out bsrts);
            }
            else if (CurReportMode == ReportMode.ListMode)
            {
                if (bstags.Count < 3)
                {
                    bsrts = new BasicReportTag[bstags.Count];
                    for (int i = 0; i < bstags.Count; i++)
                    {
                        bsrts[i] = bstags[i];
                    }

                }
                else
                {
                    bsrts = new BasicReportTag[3];
                    bsrts[0] = bstags[0];
                    bsrts[1] = bstags[1];
                    bsrts[2] = bstags[2];
                }
            }
            else bsrts = null;
        }
        /// <summary>
        /// 找去位置最好的三个基站
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="beststts"></param>
        private static void GetBestThreeStation(List<BasicReportTag> basicstations, out BasicReportTag[] beststts)
        {
            string strid1 = "", strid2 = "", strid3 = "";
            double CurAngle = 0;
            int record = 2;
            BsInfo port1=null, port2=null, port3=null;
            if (BsInfos.Count < 3)
            {
                beststts = null;
                return;
            }
            if (basicstations.Count < 3) 
            {
                beststts = null;
                return;
            }
            strid1 = basicstations[0].Id[0].ToString("X2") + basicstations[0].Id[1].ToString("X2");
            strid2 = basicstations[1].Id[0].ToString("X2") + basicstations[1].Id[1].ToString("X2");
            BsInfos.TryGetValue(strid1, out port1);
            BsInfos.TryGetValue(strid2, out port2);
            if (null == port1 || null == port2)
            {
                beststts = null; 
                return;
            }
            for (int i = 2; i < basicstations.Count; i++)
            {
                strid3 = basicstations[i].Id[0].ToString("X2") + basicstations[i].Id[1].ToString("X2");
                BsInfos.TryGetValue(strid3, out port3);
                if (null != port3)
                {
                    CurAngle = GetTheBiggestAngle(port1.Place, port2.Place, port3.Place);
                    if (CurAngle <= Math.PI / 1.2)
                    {
                        record = i;
                        break;
                    }
                }
            }
            beststts = new BasicReportTag[3];
            beststts[0] = basicstations[0];
            beststts[1] = basicstations[1];
            beststts[2] = basicstations[record];
        }
        /// <summary>
        /// 得到最大的夹角
        /// </summary>
        /// <param name="pa"></param>
        /// <param name="pb"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        private static double GetTheBiggestAngle(Point pa, Point pb, Point pc)
        {
            //求三边的长
            double c = Distance2Point(pa, pb);
            double a = Distance2Point(pb, pc);
            double b = Distance2Point(pa, pc);
            double CosA = 0, CosB = 0, CosC = 0, A = 0, B = 0, C = 0;
            //三角形余弦定理
            CosA = (Math.Pow(c, 2) + Math.Pow(b, 2) - Math.Pow(a, 2)) / (2 * b * c);
            CosB = (Math.Pow(a, 2) + Math.Pow(c, 2) - Math.Pow(b, 2)) / (2 * a * c);
            CosC = (Math.Pow(a, 2) + Math.Pow(b, 2) - Math.Pow(c, 2)) / (2 * a * b);
            A = Math.Acos(CosA);
            B = Math.Acos(CosB);
            C = Math.Acos(CosC);
            if (A > B && A > C)
            {
                return A;
            }
            else if (B > A && B > C)
            {
                return B;
            }
            else
            {
                return C;
            }
        }
        private static void BubbleSort_QualityPriority(ref List<BasicReportTag> basictags)
        {
            BasicReportTag tempbasic = new BasicReportTag();
            for (int i = basictags.Count - 1; i >= 0; --i)
            {
                for (int j = 0; j < i; ++j)
                {
                    if (basictags[j].Priority > basictags[j + 1].Priority)
                    {
                        CopyBasicReportTag(basictags[j], tempbasic);
                        CopyBasicReportTag(basictags[j + 1], basictags[j]);
                        CopyBasicReportTag(tempbasic, basictags[j + 1]);
                    }
                }
            }
        }
        //Closest mode of distance
        private static void BubbleSort_ClosestDistance(ref List<BasicReportTag> basictags)
        {
            BasicReportTag tempbasic = new BasicReportTag();
            for (int i = basictags.Count - 1; i >= 0; --i)
            {
                for (int j = 0; j < i; ++j)
                {
                    if (basictags[j].distanse > basictags[j + 1].distanse)
                    {
                        CopyBasicReportTag(basictags[j], tempbasic);
                        CopyBasicReportTag(basictags[j + 1], basictags[j]);
                        CopyBasicReportTag(tempbasic, basictags[j + 1]);
                    }
                }
            }
        }
        private static void CopyBasicReportTag(BasicReportTag brtag, BasicReportTag copytag)
        {
            System.Buffer.BlockCopy(brtag.Id, 0, copytag.Id, 0, 2);
            System.Buffer.BlockCopy(brtag.GroupID,0,copytag.GroupID,0,2);
            copytag.Priority = brtag.Priority;
            copytag.distanse = brtag.distanse;
        }
        /// <summary>
        /// 将基站上报上来的信息封装到结构体中
        /// </summary>
        /// <param name="bsst"></param>
        /// <returns></returns>
        private static IntPtr GetBasicStationPtr(BasicStation bsst)
        {
            IntPtr Record = IntPtr.Zero;
            BasicReport brpt = new BasicReport();
            brpt.ID = bsst.ID;
            brpt.SleepTime = bsst.SleepTime;
            brpt.Version = bsst.Version;
            try
            {Record = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BasicReport)));
             Marshal.StructureToPtr(brpt, Record, false);
            }catch(Exception){}
            return Record;
        }

        private static IntPtr GetTagLocalPackPtr(TagPack tp, Point Place)
        {
            TagPlace CurTagPlace = new TagPlace();
            CurTagPlace.ID = tp.ID;
            
            CurTagPlace.GroupID = tp.GroupID;
            BasicReportTag[] basictags = tp.CurBasicReport.Values.ToArray<BasicReportTag>();
            for (int i = 0; i < basictags.Length; i ++ )
            {
                switch(i)
                {
                    case 0:
                        CurTagPlace.ReferID1 = basictags[i].Id;
                        CurTagPlace.Dis1 = basictags[i].distanse;
                        CurTagPlace.ResidualValue1 = basictags[i].ResidualValue;
                        CurTagPlace.SigQuality1 = basictags[i].Priority;
                        break;
                    case 1:
                        CurTagPlace.ReferID2 = basictags[i].Id;
                        CurTagPlace.Dis2 = basictags[i].distanse;
                        CurTagPlace.ResidualValue2 = basictags[i].ResidualValue;
                        CurTagPlace.SigQuality2 = basictags[i].Priority;
                        break;
                    case 2:
                        CurTagPlace.ReferID3 = basictags[i].Id;
                        CurTagPlace.Dis3 = basictags[i].distanse;
                        CurTagPlace.ResidualValue3 = basictags[i].ResidualValue;
                        CurTagPlace.SigQuality3 = basictags[i].Priority;
                        break;
                    case 3:
                        CurTagPlace.ReferID4 = basictags[i].Id;
                        CurTagPlace.Dis4 = basictags[i].distanse;
                        CurTagPlace.ResidualValue4 = basictags[i].ResidualValue;
                        CurTagPlace.SigQuality4 = basictags[i].Priority;
                        break;
                    case 4:
                        CurTagPlace.ReferID5 = basictags[i].Id;
                        CurTagPlace.Dis5 = basictags[i].distanse;
                        CurTagPlace.ResidualValue5 = basictags[i].ResidualValue;
                        CurTagPlace.SigQuality5 = basictags[i].Priority;
                        break;
                }
            }
            CurTagPlace.LocalType = tp.LocalType;
            CurTagPlace.index = tp.index;
            CurTagPlace.Battery = tp.Battery;
            CurTagPlace.NoExeTime = tp.NoExeTime;
            CurTagPlace.SleepTime = tp.St;
            CurTagPlace.GsensorX = tp.GsensorX;
            CurTagPlace.GsensorY = tp.GsensorY;
            CurTagPlace.GsensorZ = tp.GsensorZ;
            CurTagPlace.X = Place.x;
            CurTagPlace.Y = Place.y;
            CurTagPlace.Z = Place.z;
            IntPtr record = IntPtr.Zero;
            try
            {
                int MarLen = Marshal.SizeOf(typeof(TagPlace));
                record = Marshal.AllocHGlobal(MarLen);
                Marshal.StructureToPtr(CurTagPlace, record, false);
            }
            catch (Exception)
            {
                
            }
            return record;
        }
        /// <summary>
        /// 将tag上报上来的数据包封装到结构体中
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="Place"></param>
        /// <returns></returns>
        private static IntPtr GetTagPackPtr(TagPack tp,Point Place)
        {
            TagPlace CurTagPlace = new TagPlace();
            CurTagPlace.ID = tp.ID;
            CurTagPlace.ReferID1 = tp.RD1; CurTagPlace.Dis1 = tp.Dis1; CurTagPlace.SigQuality1 = tp.SigQuality1; CurTagPlace.ResidualValue1 = tp.ResidualValue1;
            CurTagPlace.ReferID2 = tp.RD2; CurTagPlace.Dis2 = tp.Dis2; CurTagPlace.SigQuality2 = tp.SigQuality2; CurTagPlace.ResidualValue2 = tp.ResidualValue2;
            CurTagPlace.ReferID3 = tp.RD3; CurTagPlace.Dis3 = tp.Dis3; CurTagPlace.SigQuality3 = tp.SigQuality3; CurTagPlace.ResidualValue3 = tp.ResidualValue3;
            CurTagPlace.ReferID4 = tp.RD4; CurTagPlace.Dis4 = tp.Dis4; CurTagPlace.SigQuality4 = tp.SigQuality4; CurTagPlace.ResidualValue4 = tp.ResidualValue4;
            CurTagPlace.ReferID5 = tp.RD5; CurTagPlace.Dis5 = tp.Dis5; CurTagPlace.SigQuality5 = tp.SigQuality5; CurTagPlace.ResidualValue5 = tp.ResidualValue5;

            CurTagPlace.GroupID = tp.GroupID;
            
            CurTagPlace.LocalType = tp.LocalType;
            CurTagPlace.index = tp.index;
            CurTagPlace.Battery = tp.Battery;
            CurTagPlace.NoExeTime = tp.NoExeTime;
            CurTagPlace.SleepTime = tp.St;
            CurTagPlace.X = Place.x;
            CurTagPlace.Y = Place.y;
            CurTagPlace.Z = Place.z;
            IntPtr record = IntPtr.Zero;
            try
            {
                int MarLen = Marshal.SizeOf(typeof(TagPlace));
                record = Marshal.AllocHGlobal(MarLen);
                Marshal.StructureToPtr(CurTagPlace, record, false);
            }catch(Exception)
            {
            
            }
            return record;
        }

        private static IntPtr GetTagPackPtr(TagPack tp, Point Place, BasicReportTag[] bsrptags)
        {
            TagPlace CurTagPlace = new TagPlace();
            CurTagPlace.ID = tp.ID;
            CurTagPlace.ReferID1 = tp.RD1; CurTagPlace.Dis1 = tp.Dis1;
            CurTagPlace.SigQuality1 = tp.SigQuality1;
            CurTagPlace.ReferID2 = tp.RD2; CurTagPlace.Dis2 = tp.Dis2;
            CurTagPlace.SigQuality2 = tp.SigQuality2;
            CurTagPlace.ReferID3 = tp.RD3; CurTagPlace.Dis3 = tp.Dis3;
            CurTagPlace.SigQuality3 = tp.SigQuality3;
            CurTagPlace.GsensorX = tp.GsensorX;
            CurTagPlace.GsensorY = tp.GsensorY;
            CurTagPlace.GsensorZ = tp.GsensorZ;
            CurTagPlace.LocalType = tp.LocalType;
            CurTagPlace.index = tp.index;
            CurTagPlace.Battery = tp.Battery;
            CurTagPlace.NoExeTime = tp.NoExeTime;
            CurTagPlace.SleepTime = tp.St;
            CurTagPlace.X = Place.x;
            CurTagPlace.Y = Place.y;
            if(CurReportMode == ReportMode.ListMode)
            {//当前是列表模式
                if (null != bsrptags && bsrptags.Length > 0)
                {
                    if (bsrptags.Length >= 2)
                    {
                        CurTagPlace.ReferID4 = new byte[2]; CurTagPlace.ReferID5 = new byte[2];
                        System.Buffer.BlockCopy(bsrptags[0].Id, 0, CurTagPlace.ReferID4, 0, 2);
                        CurTagPlace.Dis4 = bsrptags[0].distanse;
                        System.Buffer.BlockCopy(bsrptags[1].Id, 0, CurTagPlace.ReferID5, 0, 2);
                        CurTagPlace.Dis5 = bsrptags[1].distanse;

                        CurTagPlace.SigQuality4 = bsrptags[0].Priority;
                        CurTagPlace.SigQuality5 = bsrptags[1].Priority;
                    }
                    else if (bsrptags.Length == 1)
                    {
                        CurTagPlace.ReferID4 = new byte[2];
                        System.Buffer.BlockCopy(bsrptags[0].Id, 0, CurTagPlace.ReferID4, 0, 2);
                        CurTagPlace.Dis4 = bsrptags[0].distanse;
                        CurTagPlace.SigQuality4 = bsrptags[0].Priority;
                        CurTagPlace.Dis5 = int.MaxValue;
                        CurTagPlace.SigQuality5 = int.MaxValue;
                    }
                }
                else
                { 

                        CurTagPlace.Dis5 = int.MaxValue;
                        CurTagPlace.SigQuality5 = int.MaxValue;
                        CurTagPlace.Dis4 = int.MaxValue;
                        CurTagPlace.SigQuality4 = int.MaxValue;
                }
            }

            IntPtr record = IntPtr.Zero;
            try
            {
                int MarLen = Marshal.SizeOf(typeof(TagPlace));
                record = Marshal.AllocHGlobal(MarLen);
                Marshal.StructureToPtr(CurTagPlace, record, false);
            }
            catch (Exception)
            {
            }
            return record;
        }

        public static void FreeHGLOBAL(IntPtr Hander)
        {
            Marshal.FreeHGlobal(Hander);
        }
        private static int Find(byte ch)
        {
            for (int i = 0; i < BufferLen; i++)
            {
                if (ch == Buffer[i]) return i;
            }
            return -1;
        }
        /// <summary>
        /// 求两点之间的距离
        /// </summary>
        /// <param name="A1"></param>
        /// <param name="A2"></param>
        /// <returns></returns>
        private static double Distance2Point(Point A1, Point A2)
        {
            double Distance = 0;
            double d1 = Math.Pow(A1.x - A2.x, 2);
            double d2 = Math.Pow(A1.y - A2.y, 2);
            Distance = Math.Sqrt(d1 + d2);
            return Distance;
        }
        /// <summary>
        /// 求三角形的重心
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        private static Point TriangleGravity(Point A, Point B, Point C)
        {
            Point G = new Point();
            G.x = (A.x + B.x + C.x) / 3;
            G.y = (A.y + B.y + C.y) / 3;
            return G;
        }
        /// <summary>
        /// 判断指定的字符串是否在数组中
        /// </summary>
        /// <param name="strvalue"></param>
        /// <param name="StrFileNames"></param>
        /// <returns></returns>
        private static bool isExistStr(string strvalue, string[] StrFileNames)
        {
           foreach(string str in StrFileNames)
           {
             if(null == str)continue;
             if(str.Equals(strvalue))return true;
           }
            return false;
        }

        //使用最小二乘法获取坐标值
        private static IterateDataFormat GetXY(List<AnchorDataFormat> anchorGroupList)
        {
            double xo = 0;
            double yo = 0;
            List<IterateDataFormat> iterateData = new List<IterateDataFormat> { };
            for (int i = 0; i < anchorGroupList.Count; i++)
            {
                iterateData.Add(anchorGroupList[i].IterateData);
            }
            try
            {
                double[,] a = new double[iterateData.Count, 3];

                for (int i = 0; i < iterateData.Count; i++)
                {
                    a[i, 0] = 1;
                    a[i, 1] = -2 * iterateData[i].X;
                    a[i, 2] = -2 * iterateData[i].Y;
                }
                Matrix A = new Matrix(a);

                double[,] b = new double[iterateData.Count, 1];
                for (int i = 0; i < iterateData.Count; i++)
                {
                    b[i, 0] = iterateData[i].R * iterateData[i].R - iterateData[i].X * iterateData[i].X - iterateData[i].Y * iterateData[i].Y;
                }
                Matrix B = new Matrix(b);

                Matrix At = A.Matrixtran();

                Matrix AtA = At * A;
                Matrix A1 = null;
                try
                {
                    A1 = AtA.InverseMatrix();
                }catch
                {
                    Console.WriteLine("222");
                }
                Matrix X = A1 * At * B;

                xo = X[1, 0];
                yo = X[2, 0];
            }
            catch
            {
                
                //寻找半径最小的基站作为估计的卡片位置
                double minR = double.MaxValue;
                for (int i = 0; i < iterateData.Count; i++)
                {
                    if (iterateData[i].R <= minR)
                    {
                        minR = iterateData[i].R;
                        xo = iterateData[i].X;
                        yo = iterateData[i].Y;
                    }
                }
            }
                IterateDataFormat initData = new IterateDataFormat(xo, yo, 0);
                IterateDataFormat optimumData;
                optimumData = Iterate.TaylorIterate(initData, iterateData, 5);
                if (optimumData.X == double.MaxValue && optimumData.Y == double.MaxValue && optimumData.R == double.MaxValue)
                {
                    optimumData = Iterate.ManualIterate(initData, iterateData, 100);
                    optimumData = Iterate.ManualIterate(optimumData, iterateData, 80);
                    optimumData = Iterate.ManualIterate(optimumData, iterateData, 60);
                    optimumData = Iterate.ManualIterate(optimumData, iterateData, 40);
                    optimumData = Iterate.ManualIterate(optimumData, iterateData, 20);
                    optimumData = Iterate.ManualIterate(optimumData, iterateData, 10);
                    optimumData = Iterate.ManualIterate(optimumData, iterateData, 5);
                    optimumData = Iterate.ManualIterate(optimumData, iterateData, 2);
                    optimumData = Iterate.ManualIterate(optimumData, iterateData, 1);
                }
                xo = optimumData.X;
                yo = optimumData.Y;
                
            
            return new IterateDataFormat(xo, yo, 0);
        }
        /// <summary>
        /// 定位三维坐标
        /// </summary>
        /// <param name="iters"></param>
        /// <returns></returns>
        private static IterateDataFormat Get_LeastCoordinates(List<IterateDataFormat> iters)
        {
            double xo = 0;
            double yo = 0;
            double zo = 0;
            try
            {
                double[,] a = new double[iters.Count, 4];
                for (int i = 0; i < iters.Count; i++)
                {
                    a[i, 0] = 1;
                    a[i, 1] = -2 * iters[i].X;
                    a[i, 2] = -2 * iters[i].Y;
                    a[i, 3] = -2 * iters[i].Z;
                }
                Matrix A = new Matrix(a);
                double[,] b = new double[iters.Count,1];
                for (int i = 0; i < iters.Count; i++)
                {
                    b[i, 0] = iters[i].R * iters[i].R - iters[i].X * iters[i].X - iters[i].Y * iters[i].Y - iters[i].Z * iters[i].Z;
                }
                Matrix B = new Matrix(b);
                Matrix At = A.Matrixtran();
                Matrix AtA = At * A;
                Matrix A1 = AtA.InverseMatrix();
                Matrix X = A1 * At * B;
                double[,] g = new double[4, 3];
                g[0, 0] = 1; g[0, 1] = 0; g[0, 2] = 0;
                g[1, 0] = 0; g[1, 1] = 1; g[1, 2] = 0;
                g[2, 0] = 0; g[2, 1] = 0; g[2, 2] = 1;
                g[3, 0] = 1; g[3, 1] = 1; g[3, 2] = 1;
                Matrix G = new Matrix(g);
                double[,] h = new double[4, 1];
                h[0, 0] = X[1, 0] * X[1, 0];
                h[1, 0] = X[2, 0] * X[2, 0];
                h[2, 0] = X[3, 0] * X[3, 0];
                h[3, 0] = X[0, 0];
                Matrix H = new Matrix(h);
                Matrix GT = G.Matrixtran();
                Matrix GTG = GT * G;
                Matrix G1 = GTG.InverseMatrix();
                Matrix X1 = G1 * GT * H;
                if (X1[0, 0] < 0 || X1[1, 0] < 0 || X1[2, 0] < 0)
                {
                    xo = X[1, 0];
                    yo = X[2, 0];
                    zo = X[3, 0];
                }
                else
                {
                    if (X[1, 0] >= 0) xo = Math.Sqrt(X1[0, 0]);
                    else xo = -Math.Sqrt(X1[0, 0]);

                    if (X[2, 0] >= 0) yo = Math.Sqrt(X1[1, 0]);
                    else yo = -Math.Sqrt(X1[1, 0]);

                    if (X[3, 0] >= 0) zo = Math.Sqrt(X1[2, 0]);
                    else zo = -Math.Sqrt(X1[2, 0]);
                }
            }
            catch
            {
                //寻找半径最小的基站作为估计的卡片位置
                double minR = double.MaxValue;
                for (int i = 0; i < iters.Count; i++)
                {
                    if (iters[i].R <= minR)
                    {
                        minR = iters[i].R;
                        xo = iters[i].X;
                        yo = iters[i].Y;
                        zo = iters[i].Z;
                    }
                }
            }

            //迭代求出最优解
            IterateDataFormat initData = new IterateDataFormat(xo, yo, zo, 0);
            IterateDataFormat optimumData;
            //使用泰勒级数迭代
            optimumData = Iterate.TaylorIterate(initData, iters, 5);
            //无法迭代出最优解，使用手动迭代
            if (optimumData.X == double.MaxValue && optimumData.Y == double.MaxValue && optimumData.R == double.MaxValue && optimumData.Z == double.MaxValue)
            {
                //选取合适的步数
                double step = 0;
                for (int i = 0; i < iters.Count; i++)
                {
                    double dis = Math.Sqrt((xo - iters[i].X) * (xo - iters[i].X) + (yo - iters[i].Y) * (yo - iters[i].Y) + (zo - iters[i].Z) * (zo - iters[i].Z)) + iters[i].R;
                    if (dis >= step)
                    {
                        step = dis;
                    }
                }
                step = (int)step;
                optimumData = initData;
                for ( ; ; )
                {
                    optimumData = Iterate.ManualIterate(optimumData, iters, step);
                    step = step / 4;
                    step = (int)step;
                    if (step <= 1)
                    {
                        break;
                    }
                }
            }
            xo = optimumData.X;
            yo = optimumData.Y;
            zo = optimumData.Z;
            return new IterateDataFormat(xo, yo, zo, 0);
        }

        /// <summary>
        /// 使用残差法计算坐标
        /// </summary>
        /// <param name="ReportPorts"></param>
        /// <returns></returns>
        private static ResidualFormat Get_OptimalPoint(List<IterateDataFormat> iterateData)
        {
            List<ResidualFormat> ResidualList = new List<ResidualFormat>{ };
            List<AnchorDataFormat> anchorDataList = new List<AnchorDataFormat> { };
            if (iterateData.Count < 3)
            {
                return null;
            }
            //给每个节点分配一个类型，最多支持6个基站
            for (int i = 0; i < iterateData.Count; i++)
            {
                if (i >= 6)
                    break;
                anchorDataList.Add(new AnchorDataFormat(iterateData[i], 0, i));
            }
            //三个数据组合
            if (anchorDataList.Count >= 3)
            {
                for (int i = 0; i < anchorDataList.Count; i++)
                {
                    for (int j = i + 1; j < anchorDataList.Count; j++)
                    {
                        for (int l = j + 1; l < anchorDataList.Count; l++)
                        {
                            List<AnchorDataFormat> data = new List<AnchorDataFormat>{ };
                            data.Add(anchorDataList[i]);
                            data.Add(anchorDataList[j]);
                            data.Add(anchorDataList[l]);
                            ResidualList.Add(new ResidualFormat(data));
                        }
                    }
                }
            }
            //四个数据组合
            if (anchorDataList.Count >= 4)
            {
                for (int k = 0; k < anchorDataList.Count; k++)
                {
                    for (int i = k + 1; i < anchorDataList.Count; i++)
                    {
                        for (int j = i + 1; j < anchorDataList.Count; j++)
                        {
                            for (int l = j + 1; l < anchorDataList.Count; l++)
                            {
                                List<AnchorDataFormat> data = new List<AnchorDataFormat> { };
                                data.Add(anchorDataList[k]);
                                data.Add(anchorDataList[i]);
                                data.Add(anchorDataList[j]);
                                data.Add(anchorDataList[l]);
                                ResidualList.Add(new ResidualFormat(data));
                            }
                        }
                    }
                }
            }
            //五个数据组合
            if (anchorDataList.Count >= 5)
            {
                for (int m = 0; m < anchorDataList.Count; m++)
                {
                    for (int k = m + 1; k < anchorDataList.Count; k++)
                    {
                        for (int i = k + 1; i < anchorDataList.Count; i++)
                        {
                            for (int j = i + 1; j < anchorDataList.Count; j++)
                            {
                                for (int l = j + 1; l < anchorDataList.Count; l++)
                                {
                                    List<AnchorDataFormat> data = new List<AnchorDataFormat> { };
                                    data.Add(anchorDataList[m]);
                                    data.Add(anchorDataList[k]);
                                    data.Add(anchorDataList[i]);
                                    data.Add(anchorDataList[j]);
                                    data.Add(anchorDataList[l]);
                                    ResidualList.Add(new ResidualFormat(data));
                                }
                            }
                        }
                    }
                }
            }

            //六个数据组合
            if (anchorDataList.Count >= 6)
            {
                for (int n = 0; n < anchorDataList.Count; n++)
                {
                    for (int m = n + 1; m < anchorDataList.Count; m++)
                    {
                        for (int k = m + 1; k < anchorDataList.Count; k++)
                        {
                            for (int i = k + 1; i < anchorDataList.Count; i++)
                            {
                                for (int j = i + 1; j < anchorDataList.Count; j++)
                                {
                                    for (int l = j + 1; l < anchorDataList.Count; l++)
                                    {
                                        List<AnchorDataFormat> data = new List<AnchorDataFormat> { };
                                        data.Add(anchorDataList[n]);
                                        data.Add(anchorDataList[m]);
                                        data.Add(anchorDataList[k]);
                                        data.Add(anchorDataList[i]);
                                        data.Add(anchorDataList[j]);
                                        data.Add(anchorDataList[l]);
                                        ResidualList.Add(new ResidualFormat(data));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < ResidualList.Count; i++)
            {
                ResidualList[i].OptimalPoint = GetXY(ResidualList[i].AnchorDataList);
                ResidualList[i].Residual = Get_Residual(ResidualList[i].OptimalPoint, ResidualList[i].AnchorDataList);
                //将每个基站组合所对应的残差加到每个基站组合中的各个成员
                for (int j = 0; j < ResidualList[i].AnchorDataList.Count; j++)
                {
                    for (int k = 0; k < anchorDataList.Count; k++)
                    {
                        if (anchorDataList[k].Type == ResidualList[i].AnchorDataList[j].Type)
                        {
                            anchorDataList[k].Residual += ResidualList[i].Residual;
                            break;
                        }
                    }
                }
            }
            //将基站按照残差从小到大排序
            for (int j = anchorDataList.Count - 1; j > 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    if (anchorDataList[i].Residual > anchorDataList[i + 1].Residual)
                    {
                        AnchorDataFormat anchorDataTemp = anchorDataList[i];
                        anchorDataList[i] = anchorDataList[i + 1];
                        anchorDataList[i + 1] = anchorDataTemp;
                    }
                }
            }
            for (int i = 0; i < ResidualList.Count; i++)
            {
                if (ResidualList[i].AnchorDataList.Count == 3)
                {
                    //使用残差最小的三个基站作为最优解
                    if (ResidualList[i].AnchorDataList.Contains(anchorDataList[0]) && ResidualList[i].AnchorDataList.Contains(anchorDataList[1]) && ResidualList[i].AnchorDataList.Contains(anchorDataList[2]))
                    {
                        if (IsSuitablePoint(ResidualList[i].OptimalPoint, ResidualList[i].AnchorDataList, anchorDataList) == true)
                        {
                            return ResidualList[i];
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            //将基站组合按照残差从小到大排序
            for (int i = ResidualList.Count - 1; i > 0; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    if (ResidualList[j].Residual > ResidualList[j + 1].Residual)
                    {
                        ResidualFormat residualTemp = ResidualList[j];
                        ResidualList[j] = ResidualList[j + 1];
                        ResidualList[j + 1] = residualTemp;
                    }
                }
            }

            for (int i = 0; i < ResidualList.Count; i++)
            {
                if (IsSuitablePoint(ResidualList[i].OptimalPoint, ResidualList[i].AnchorDataList, anchorDataList) == true)
                {
                    return ResidualList[i];
                }
            }

            return ResidualList[0];
        }


        //获取残差
        private static double Get_Residual(IterateDataFormat point, List<AnchorDataFormat> anchorData)
        {
            double value = 0;

            for (int i = 0; i < anchorData.Count; i++)
            {
                value += Math.Pow(anchorData[i].IterateData.R - Math.Sqrt((anchorData[i].IterateData.X - point.X) * (anchorData[i].IterateData.X - point.X) + (anchorData[i].IterateData.Y - point.Y) * (anchorData[i].IterateData.Y - point.Y)), 2);
            }
            value = value / anchorData.Count;
            return value;
        }

        private static  bool IsSuitablePoint(IterateDataFormat point, List<AnchorDataFormat> useAnchorDataList, List<AnchorDataFormat> totalAnchorDataList)
        {
            int i = 0;
            int j = 0;

            if (useAnchorDataList.Count == totalAnchorDataList.Count) return true;

            for (i = 0; i < totalAnchorDataList.Count; i++)
            {
                for (j = 0; j < useAnchorDataList.Count; j++)
                {
                    if (useAnchorDataList[j].Type == totalAnchorDataList[i].Type)
                        break;
                }
                //存在其他基站
                if (j == useAnchorDataList.Count)
                {
                    if (Math.Sqrt(Math.Pow((totalAnchorDataList[i].IterateData.X - point.X), 2) + Math.Pow((totalAnchorDataList[i].IterateData.Y - point.Y), 2)) > totalAnchorDataList[i].IterateData.R)
                        return false;
                }

            }

            return true;
        }
    }
    //卡尔曼滤波
    [Serializable]
    public class Kalman
    {
        //系统状态
        public double X
        {
            get;
            set;
        }
        //根据上一状态预测的协方差
        public double P
        {
            get;
            set;
        }
        //过程噪声协方差
        public double Q
        {
            get;
            set;
        }
        //测量噪声协方差
        public double R
        {
            get;
            set;
        }
        //卡尔曼增益
        public double Gain
        {
            set;
            get;
        }
        public Kalman(double x)
        {
            this.X = x;     //系统状态初始化
            this.P = 0.1;     //协方差初始化，一般不为0，可以选择一个较小的适当的数
            this.Q = 0.2;     //过程噪声初始化
            this.R = 1;     //测量噪声初始化
        }
        public Kalman(double x, double p, double q, double r)
        {
            this.X = x;     //系统状态初始化
            this.P = p;     //协方差初始化，一般不为0，可以选择一个较小的适当的数
            this.Q = q;     //过程噪声初始化
            this.R = r;     //测量噪声初始化
        }

        public double Kalman_Filter(double mesure)
        {
            this.X = this.X;                                    //获取上一次预测的结果
            this.P = this.P + this.Q;                           //预测下一时刻的协方差
            this.Gain = this.P / (this.P + this.R);             //更新卡尔曼增益
            this.X = this.X + this.Gain * (mesure - this.X);    //得到现在状态的最优化估算值
            this.P = (1 - this.Gain) * this.P;                  //更新这一时刻的协方差
            return this.X;
        }
    }
    [Serializable]
    public class Point
    {
        public double x { set; get; }
        public double y { set; get; }
        public double z { set; get; }
        public Point()
        {
            this.x = -1d;
            this.y = -1d;
            this.z = -1d;
        }
        public Point(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
    /// <summary>
    /// 基站类
    /// </summary>
    public class BasicStation
    {
        public byte[] ID = new byte[2];
        public ushort SleepTime;
        public uint Version;
        public int TickCount;
        public bool isState;
    }

    [Serializable]
    public class TagPack
    {
        public byte[] ID = new byte[2];
        public byte[] GroupID = new byte[2];
        public double x = 0, y = 0, z = 0;
        public byte[] RD1 = new byte[2];
        public byte[] RD2 = new byte[2];
        public byte[] RD3 = new byte[2];
        public byte[] RD4 = new byte[2];
        public byte[] RD5 = new byte[2];

        public Point Refer1Place = new Point();
        public Point Refer2Place = new Point();
        public Point Refer3Place = new Point();
        public Point Refer4Place = new Point();
        public Point Refer5Place = new Point();

        public double Dis1, Dis2, Dis3, Dis4, Dis5;
        public int SigQuality1, SigQuality2, SigQuality3, SigQuality4, SigQuality5;
        public double ResidualValue1, ResidualValue2, ResidualValue3, ResidualValue4, ResidualValue5;

        public short GsensorX, GsensorY, GsensorZ;

        public ConcurrentDictionary<string, BasicReportTag> CurBasicReport = new ConcurrentDictionary<string, BasicReportTag>();

        public byte LocalType = 0;//定位类型
        public byte Battery;
        public ushort NoExeTime;
        public ushort TotalbasicNum = 0;//当前需要上报的基站的总数量
        public byte CurBasicNum;//基站的个数
        public byte index;
        public int TickCount;
        public DateTime ReportDT = new DateTime();
        public ushort St = 1;//默认1s上报一次
        public bool isReport = false;
        public bool isState;
        public ArrayList InterPoint = new ArrayList();
    }
    [Serializable]
    public class BasicReportTag
    {
        public byte[] Id = new byte[2];
        //组别讯息
        public byte[] GroupID = new byte[2];
        public double distanse = -1;
        public byte Priority = 0;
        public double ResidualValue = 0;
        public int SigQuality = 0;

    }
    public class BsInfo
    {
        public byte[] ID = new byte[2];
        public byte[] GroupID = new byte[2];
        public Point Place = new Point();
    }

    public class TagInfo
    {
        public byte[] Id = new byte[2];
        public double height = -1d;//Tag的高度
    }

    enum TwoCirInterMode
    {
        UnKnown,//未知关系
        No,//两圆没有相交
        TT1,//相交于1点
        TT2//相交于2点
    }
    /// <summary>
    /// 各种异常点列举
    /// </summary>
    public class PException
    {
        public static Point P1 = new Point( 0, 0, -1); 
        public static Point P2 = new Point( 0, 0, -2); 
        public static Point P3 = new Point( 0, 0, -3);
    }
    /// <summary>
    /// 协议标识
    /// </summary>
    public class TPPID
    {
        public const byte Head = 0xFC;
        public const byte End = 0xFB;
        public const byte TagType = 0x01;
        public const byte TagStillType = 0x03;
        public const byte RouterType = 0x02;
        public const byte TagCommonLocal = 0;
        public const byte TagWarmLocal = 1;
        public const int TagStillLen = 13 + 6;
        public const int TagPackNumPlace = 11 + 6;
        public const int TagPackLen = 19;
        public const int RouterPackLen = 13;
        public const byte RouterVersionLen = 6;

        public const int WREFERSMSG_TYPE = 2;

        public const int WPARAM_TYPE = 0;
        public const int WBASIC_TYPE = 1;
        public const int WTAG_TYPE = 0;
        public const int WBIGVERSION = 0;
        public const int WSMALLVERSION = 1;
        public const int USER = 0x0400;
        public const int WM_TAG_PACK = USER + 500;
        public const int WM_DEVICE_DIS = USER + 501;
        public const int WM_VERSION_ERR = USER + 502;
    }

    class Iterate
    {
        /// <summary>
        /// 预测点到各个圆之间距离的平方和
        /// </summary>
        /// <param name="initData"></预测点的坐标>
        /// <param name="iterateData"></各个圆的参数>
        /// <returns></预测点到各个圆之间距离的平方和>
        static private double SumOfSquare(IterateDataFormat initData, List<IterateDataFormat> iterateData)
        {
            double squareSum = 0;
            for (int i = 0; i < iterateData.Count; i++)
            {
                squareSum += (Math.Abs(Math.Sqrt((initData.X - iterateData[i].X) * (initData.X - iterateData[i].X) + (initData.Y - iterateData[i].Y) * (initData.Y - iterateData[i].Y) + (initData.Z - iterateData[i].Z) * (initData.Z - iterateData[i].Z)) - iterateData[i].R) *
                              Math.Abs(Math.Sqrt((initData.X - iterateData[i].X) * (initData.X - iterateData[i].X) + (initData.Y - iterateData[i].Y) * (initData.Y - iterateData[i].Y) + (initData.Z - iterateData[i].Z) * (initData.Z - iterateData[i].Z)) - iterateData[i].R));
            }
            return squareSum;
        }
        /// <summary>
        /// 手动迭代法，推算出点到各个圆之间距离的平方和最小的点的坐标
        /// </summary>
        /// <param name="initData"></预测点的坐标>
        /// <param name="iterateData"></各个圆的参数>
        /// <param name="step"></步数>
        /// <returns></最优点的坐标>
        static public IterateDataFormat ManualIterate2(IterateDataFormat initData, List<IterateDataFormat> iterateData, double step)
        {
            IterateDataFormat lastData = new IterateDataFormat(initData.X, initData.Y, initData.R);
            double squareSumBase = 0;
            double squareSumTemp = 0;
            IterateDataFormat dataTemp = new IterateDataFormat(0, 0, 0);

            while (true)
            {
                squareSumBase = SumOfSquare(lastData, iterateData);
                dataTemp.X = lastData.X;
                dataTemp.Y = lastData.Y;

                //先对x坐标进行迭代
                dataTemp.X += step;
                //X坐标加，可以减少平方和
                squareSumTemp = SumOfSquare(dataTemp, iterateData);
                if (squareSumTemp < squareSumBase)
                {
                    while (true)
                    {
                        squareSumBase = squareSumTemp;
                        dataTemp.X += step;
                        squareSumTemp = SumOfSquare(dataTemp, iterateData);
                        if (squareSumTemp > squareSumBase)
                            break;
                    }
                    dataTemp.X -= step;
                }
                //X坐标减，可以减少平方和
                else
                {
                    dataTemp.X = lastData.X;
                    squareSumTemp = squareSumBase;
                    while (true)
                    {
                        squareSumBase = squareSumTemp;
                        dataTemp.X -= step;
                        squareSumTemp = SumOfSquare(dataTemp, iterateData);
                        if (squareSumTemp > squareSumBase)
                            break;
                    }
                    dataTemp.X += step;
                }

                //对Y坐标进行迭代
                dataTemp.Y += step;
                //Y坐标加，可以减少平方和
                squareSumTemp = SumOfSquare(dataTemp, iterateData);
                if (squareSumTemp < squareSumBase)
                {
                    while (true)
                    {
                        squareSumBase = squareSumTemp;
                        dataTemp.Y += step;
                        squareSumTemp = SumOfSquare(dataTemp, iterateData);
                        if (squareSumTemp > squareSumBase)
                            break;
                    }
                    dataTemp.Y -= step;
                }
                //Y坐标减，可以减少平方和
                else
                {
                    dataTemp.Y = lastData.Y;
                    squareSumTemp = squareSumBase;
                    while (true)
                    {
                        squareSumBase = squareSumTemp;
                        dataTemp.Y -= step;
                        squareSumTemp = SumOfSquare(dataTemp, iterateData);
                        if (squareSumTemp > squareSumBase)
                            break;
                    }
                    dataTemp.Y += step;
                }
                //迭代后的最优解与上次的一致，迭代结束
                if (dataTemp.X == lastData.X && dataTemp.Y == lastData.Y)
                {
                    return lastData;
                }
                lastData.X = dataTemp.X;
                lastData.Y = dataTemp.Y;
            }
        }
        /// <summary>
        /// 手动迭代法，推算出点到各个圆之间距离的平方和最小的点的坐标
        /// </summary>
        /// <param name="initData"></预测点的坐标>
        /// <param name="iterateData"></各个圆的参数>
        /// <param name="step"></步数>
        /// <returns></最优点的坐标>
        static public IterateDataFormat ManualIterate(IterateDataFormat initData, List<IterateDataFormat> iterateData, double step)
        {
            IterateDataFormat lastData = new IterateDataFormat(initData.X, initData.Y, initData.Z, initData.R);
            double squareSumBase = 0;
            double squareSumTemp = 0;
            IterateDataFormat dataTemp = new IterateDataFormat(0, 0, 0, 0);
            while (true)
            {
                squareSumBase = SumOfSquare(lastData, iterateData);
                dataTemp.X = lastData.X;
                dataTemp.Y = lastData.Y;
                dataTemp.Z = lastData.Z;
                //先对x坐标进行迭代
                dataTemp.X += step;
                //X坐标加，可以减少平方和
                squareSumTemp = SumOfSquare(dataTemp, iterateData);
                if (squareSumTemp < squareSumBase)
                {
                    while (true)
                    {
                        squareSumBase = squareSumTemp;
                        dataTemp.X += step;
                        squareSumTemp = SumOfSquare(dataTemp, iterateData);
                        if (squareSumTemp > squareSumBase)
                            break;
                    }
                    dataTemp.X -= step;
                }
                else
                {
                    dataTemp.X = lastData.X;
                    squareSumTemp = squareSumBase;
                    while (true)
                    {
                        squareSumBase = squareSumTemp;
                        dataTemp.X -= step;
                        squareSumTemp = SumOfSquare(dataTemp, iterateData);
                        if (squareSumTemp > squareSumBase)
                            break;
                    }
                    dataTemp.X += step;
                }
                //对Y坐标进行迭代
                dataTemp.Y += step;
                //Y坐标加，可以减少平方和
                squareSumTemp = SumOfSquare(dataTemp, iterateData);
                if (squareSumTemp < squareSumBase)
                {
                    while (true)
                    {
                        squareSumBase = squareSumTemp;
                        dataTemp.Y += step;
                        squareSumTemp = SumOfSquare(dataTemp, iterateData);
                        if (squareSumTemp > squareSumBase)
                            break;
                    }
                    dataTemp.Y -= step;
                }
                else
                {
                    dataTemp.Y = lastData.Y;
                    squareSumTemp = squareSumBase;
                    while (true)
                    {
                        squareSumBase = squareSumTemp;
                        dataTemp.Y -= step;
                        squareSumTemp = SumOfSquare(dataTemp, iterateData);
                        if (squareSumTemp > squareSumBase)
                            break;
                    }
                    dataTemp.Y += step;
                }
                dataTemp.Z += step;
                squareSumTemp = SumOfSquare(dataTemp, iterateData);
                if (squareSumTemp < squareSumBase)
                {
                    while (true)
                    {
                        squareSumBase = squareSumTemp;
                        dataTemp.Z += step;
                        squareSumTemp = SumOfSquare(dataTemp, iterateData);
                        if (squareSumTemp > squareSumBase)
                        {
                            break;
                        }
                    }
                    dataTemp.Z -= step;
                }
                else
                {
                    dataTemp.Z = lastData.Z;
                    squareSumTemp = squareSumBase;
                    while (true)
                    {
                        squareSumBase = squareSumTemp;
                        dataTemp.Z -= step;
                        squareSumTemp = SumOfSquare(dataTemp, iterateData);
                        if (squareSumTemp > squareSumBase)
                        {
                            break;
                        }
                    }
                    dataTemp.Z += step;
                }
                if (dataTemp.X == lastData.X && dataTemp.Y == lastData.Y && dataTemp.Z == lastData.Z)
                {
                    return lastData;
                }
                lastData.X = dataTemp.X;
                lastData.Y = dataTemp.Y;
                lastData.Z = dataTemp.Z;
            }
        }

        /// <summary>
        /// 泰勒级数展开法
        /// </summary>
        /// <param name="initData"></预测点的坐标>
        /// <param name="iterateData"></各个圆的参数>
        /// <param name="stopThreShold"></停止迭代的阀值>
        /// <returns></最优点的坐标>
        static public IterateDataFormat TaylorIterate2(IterateDataFormat initData, List<IterateDataFormat> iterateData, double stopThreShold)
        {
            //停止迭代的阀值必须为正数
            stopThreShold = Math.Abs(stopThreShold);
            IterateDataFormat lastData = new IterateDataFormat(initData.X, initData.Y, initData.R);
            double lastDelta = double.MaxValue;
            byte tick = 0;
            try
            {
                while (true)
                {
                    tick++;
                    List<double> rnoList = new List<double> { };
                    for (int i = 0; i < iterateData.Count; i++)
                    {
                        rnoList.Add(Math.Sqrt((iterateData[i].X - lastData.X) * (iterateData[i].X - lastData.X) + (iterateData[i].Y - lastData.Y) * (iterateData[i].Y - lastData.Y)));
                    }

                    double[,] h = new double[iterateData.Count, 1];
                    for (int i = 0; i < iterateData.Count; i++)
                    {
                        h[i, 0] = iterateData[i].R - rnoList[i];
                    }
                    Matrix Mh = new Matrix(h);

                    double[,] g = new double[iterateData.Count, 2];
                    for (int i = 0; i < iterateData.Count; i++)
                    {
                        g[i, 0] = (lastData.X - iterateData[i].X) / rnoList[i];
                        g[i, 1] = (lastData.Y - iterateData[i].Y) / rnoList[i];
                    }
                    Matrix Mg = new Matrix(g);

                    Matrix MgT = Mg.Matrixtran();
                    Matrix MgTMg = MgT * Mg;
                    Matrix MgTMg1 = MgTMg.InverseMatrix();
                    Matrix Mdelta = MgTMg1 * MgT * Mh;
                    //上一次的调整值小于这一次的调整值，表示函数没有极小值，发散不收敛，退出返回无解
                    if (lastDelta < Math.Abs(Mdelta[0, 0]) + Math.Abs(Mdelta[1, 0]))
                    {
                        lastData.X = double.MaxValue;
                        lastData.Y = double.MaxValue;
                        lastData.R = double.MaxValue;
                        return lastData;
                    }
                    lastDelta = Math.Abs(Mdelta[0, 0]) + Math.Abs(Mdelta[1, 0]);
                    lastData.X += Mdelta[0, 0];
                    lastData.Y += Mdelta[1, 0];
                    //如果DeltaX+DeltaY小于停止阀值，则停止迭代
                    if (lastDelta <= stopThreShold)
                    {
                        return lastData;
                    }
                    if (tick >= 10)
                    {
                        lastData.X = -1;
                        lastData.Y = -1;
                        return lastData;
                    }
                }
            }
            //无解
            catch
            {
                lastData.X = double.MaxValue;
                lastData.Y = double.MaxValue;
                lastData.R = double.MaxValue;
                return lastData;
            }
        }

        /// <summary>
        /// 泰勒级数展开法
        /// </summary>
        /// <param name="initData"></预测点的坐标>
        /// <param name="iterateData"></各个圆的参数>
        /// <param name="stopThreShold"></停止迭代的阀值>
        /// <returns></最优点的坐标>
        static public IterateDataFormat TaylorIterate(IterateDataFormat initData, List<IterateDataFormat> iterateData, double stopThreShold)
        {
            //停止迭代的阀值必须为正数
            stopThreShold = Math.Abs(stopThreShold);
            IterateDataFormat lastData = new IterateDataFormat(initData.X, initData.Y, initData.Z, initData.R);
            double lastDelta = double.MaxValue;
            int count = 0;
            try
            {
                while (true)
                {
                    List<double> rnoList = new List<double> { };
                    for (int i = 0; i < iterateData.Count; i++)
                    {
                        double rno = Math.Sqrt((iterateData[i].X - lastData.X) * (iterateData[i].X - lastData.X) + (iterateData[i].Y - lastData.Y) * (iterateData[i].Y - lastData.Y) + (iterateData[i].Z - lastData.Z) * (iterateData[i].Z - lastData.Z));
                        //rno不能为0
                        if (rno == 0)
                        {
                            lastData.X = double.MaxValue;
                            lastData.Y = double.MaxValue;
                            lastData.Z = double.MaxValue;
                            lastData.R = double.MaxValue;
                            return lastData;
                        }
                        rnoList.Add(rno);
                    }

                    double[,] h = new double[iterateData.Count, 1];
                    for (int i = 0; i < iterateData.Count; i++)
                    {
                        h[i, 0] = iterateData[i].R - rnoList[i];
                    }
                    Matrix Mh = new Matrix(h);

                    double[,] g = new double[iterateData.Count, 3];
                    for (int i = 0; i < iterateData.Count; i++)
                    {
                        g[i, 0] = (lastData.X - iterateData[i].X) / rnoList[i];
                        g[i, 1] = (lastData.Y - iterateData[i].Y) / rnoList[i];
                        g[i, 2] = (lastData.Z - iterateData[i].Z) / rnoList[i];
                    }
                    Matrix Mg = new Matrix(g);

                    Matrix MgT = Mg.Matrixtran();
                    Matrix MgTMg = MgT * Mg;
                    Matrix MgTMg1 = MgTMg.InverseMatrix();
                    Matrix Mdelta = MgTMg1 * MgT * Mh;

                    //上一次的调整值小于这一次的调整值，表示函数没有极小值，发散不收敛，退出返回无解
                    if (lastDelta <= Math.Abs(Mdelta[0, 0]) + Math.Abs(Mdelta[1, 0]) + Math.Abs(Mdelta[2, 0]))
                    {
                        lastData.X = double.MaxValue;
                        lastData.Y = double.MaxValue;
                        lastData.Z = double.MaxValue;
                        lastData.R = double.MaxValue;
                        return lastData;
                    }
                    lastDelta = Math.Abs(Mdelta[0, 0]) + Math.Abs(Mdelta[1, 0]) + Math.Abs(Mdelta[2, 0]);
                    lastData.X += Mdelta[0, 0];
                    lastData.Y += Mdelta[1, 0];
                    lastData.Z += Mdelta[2, 0];

                    //如果DeltaX+DeltaY小于停止阀值，则停止迭代
                    if (lastDelta <= stopThreShold)
                    {
                        return lastData;
                    }

                    count++;
                    //如果迭代次数超过20次，退出迭代
                    if (count > 20)
                    {
                        lastData.X = double.MaxValue;
                        lastData.Y = double.MaxValue;
                        lastData.Z = double.MaxValue;
                        lastData.R = double.MaxValue;
                        return lastData;
                    }
                }
            }
            //无解
            catch
            {
                lastData.X = double.MaxValue;
                lastData.Y = double.MaxValue;
                lastData.Z = double.MaxValue;
                lastData.R = double.MaxValue;
                return lastData;
            }
        }
    }
    
    class IterateDataFormat
    {
        public byte[] ID
        {
            set;
            get;
        }
        public double X
        {
            get;
            set;
        }
        public double Y
        {
            get;
            set;
        }
        public double Z
        {
            get;
            set;
        }
        public double R
        {
            get;
            set;
        }
        public IterateDataFormat(byte[] ID, double x, double y, double z, double r)
        {
            X = x;
            Y = y;
            Z = z;
            R = r;
            this.ID = ID;
        }
        public IterateDataFormat(byte[] ID, double x, double y, double r)
        {
            X = x;
            Y = y;
            R = r;
            this.ID = ID;
        }
        public IterateDataFormat(double x, double y, double z, double r)
        {
            X = x;
            Y = y;
            Z = z;
            R = r;
        }
        public IterateDataFormat(double x, double y, double r)
        {
            X = x;
            Y = y;
            R = r;
        }
    }
    //基站数据
    class AnchorDataFormat
    {
        //基站残差
        public double Residual
        {
            get;
            set;
        }
        public int Type
        {
            get;
            set;
        }
        public IterateDataFormat IterateData
        {
            get;
            set;
        }
        public AnchorDataFormat(IterateDataFormat iterateData, double residual, int type)
        {
            IterateData = new IterateDataFormat(iterateData.ID,iterateData.X, iterateData.Y, iterateData.R);
            Residual = residual;
            Type = type;
        }
    }
    class ResidualFormat
    {
        //不同基站组成的集合
        public List<AnchorDataFormat> AnchorDataList
        {
            get;
            set;
        }
        //基站组合算出的最优解
        public IterateDataFormat OptimalPoint
        {
            get;
            set;
        }
        //该基站组合所对应的残差
        public double Residual
        {
            get;
            set;
        }
        public ResidualFormat(List<AnchorDataFormat> anchorGroupList)
        {
            AnchorDataList = anchorGroupList;
        }
    }
    /// <summary>
    /// 矩阵   异常 512索引 1024无解 2046矩阵行列 
    /// </summary>
    class Matrix
    {
        private int m_row;//行
        private int m_col;//列
        private double[,] m_data;//数据
        /// <summary>元素
        /// </summary>
        /// <param name="ro"></param>
        /// <param name="co"></param>
        /// <returns></returns>
        public double this[int ro, int co]
        {
            get
            {
                if (ro >= m_row || co >= m_col || ro < 0 || co < 0) throw new Exception("512");
                return m_data[ro, co];
            }
            set
            {
                if (ro >= m_row || co >= m_col || ro < 0 || co < 0) throw new Exception("512");
                m_data[ro, co] = value;
            }
        }
        /// <summary>行数
        /// </summary>
        public int Row
        {
            get { return m_row; }
        }
        /// <summary>列数
        /// </summary>
        public int Column
        {
            get { return m_col; }
        }
        public Matrix()
        {
            m_row = 0; m_col = 0; m_data = new double[0, 0];
        }
        public Matrix(double[,] matrix)
        {
            m_row = matrix.GetLength(0);
            m_col = matrix.GetLength(1);
            m_data = matrix;
        }
        public Matrix(int ro, int co)
        {
            if (ro < 0 || co < 0) throw new Exception("512");
            m_row = ro;
            m_col = co;
            m_data = new double[ro, co];
        }
        //矩阵与矩阵相乘
        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (left.Column != right.Row) throw new Exception("2046");
            Matrix re = new Matrix(left.Row, right.Column);
            for (int i = 0; i < left.Row; i++)
            {
                for (int j = 0; j < right.Column; j++)
                {
                    for (int k = 0; k < left.Column; k++)
                    {
                        re[i, j] += left[i, k] * right[k, j];
                    }
                }
            }
            return re;
        }
        //矩阵相加
        public static Matrix operator +(Matrix left, Matrix right)
        {
            if (left.Row != right.Row || left.Column != right.Column)
                throw new Exception("2046");
            Matrix re = new Matrix(left.Row, left.Column);
            for (int i = 0; i < left.Row; i++)
            {
                for (int j = 0; j < left.Column; j++)
                {
                    re[i, j] = left[i, j] + right[i, j];
                }
            }
            return re;
        }
        //矩阵相减
        public static Matrix operator -(Matrix left, Matrix right)
        {
            if (left.Row != right.Row || left.Column != right.Column)
                throw new Exception("2046");
            Matrix re = new Matrix(left.Row, left.Column);
            for (int i = 0; i < left.Row; i++)
            {
                for (int j = 0; j < left.Column; j++)
                {
                    re[i, j] = left[i, j] - right[i, j];
                }
            }
            return re;
        }
        //矩阵与数相乘
        public static Matrix operator *(double factor, Matrix right)
        {
            Matrix re = new Matrix(right.Row, right.Column);
            for (int i = 0; i < right.Row; i++)
            {
                for (int j = 0; j < right.Column; j++)
                {
                    re[i, j] = right[i, j] * factor;
                }
            }
            return re;
        }
        public static Matrix operator *(Matrix left, double factor)
        {
            return factor * left;
        }
        /// <summary>转置
        /// </summary>
        /// <returns></returns>
        public Matrix Matrixtran()
        {
            Matrix re = new Matrix(this.m_col, this.m_row);
            for (int i = 0; i < this.m_row; i++)
            {
                for (int j = 0; j < this.m_col; j++)
                {
                    re[j, i] = this[i, j];
                }
            }
            return re;
        }
        /// <summary>行列式        //加边法
        /// </summary>
        /// <param name="Matrix"></param>
        /// <returns></returns>
        public double Matrixvalue()
        {
            if (this.m_row != this.m_col)
            {
                throw new Exception("2046");
            }
            int n = this.m_row;
            //一维矩阵行列式为它本身
            if (n == 1) 
                return this[0, 0];
            //二维矩阵行列式
            if (n == 2) 
                return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
            //多维矩阵行列式
            double dsum = 0, dSign = 1;
            for (int i = 0; i < n; i++)
            {
                Matrix tempa = new Matrix(n - 1, n - 1);
                for (int j = 0; j < n - 1; j++)
                {
                    for (int k = 0; k < n - 1; k++)
                    {
                        tempa[j, k] = this[j + 1, k >= i ? k + 1 : k];
                    }
                }
                dsum += this[0, i] * dSign * tempa.Matrixvalue();
                dSign = dSign * -1;
            }
            return dsum;
        }
        /// <summary>求逆
        /// </summary>
        /// <param name="Matrix"></param>
        /// <returns></returns>
        public Matrix InverseMatrix()
        {
            int row = this.m_row; int col = this.m_col;
            if (row != col)
                throw new Exception("2046");
            Matrix re = new Matrix(row, col);
            double val = this.Matrixvalue();
            if (Math.Abs(val) == 0)
            {
                throw new Exception("1024");
            }
            re = this.AdjointMatrix();
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    re[i, j] = re[i, j] / val;
                }
            }
            return re;

        }
        /// <summary>求伴随矩阵
        /// </summary>
        /// <param name="Matrix"></param>
        /// <returns></returns>
        public Matrix AdjointMatrix()
        {
            int row = this.m_row;
            Matrix re = new Matrix(row, row);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    Matrix temp = new Matrix(row - 1, row - 1);
                    for (int x = 0; x < row - 1; x++)
                    {
                        for (int y = 0; y < row - 1; y++)
                        {
                            temp[x, y] = this[x < i ? x : x + 1, y < j ? y : y + 1];
                        }
                    }
                    re[j, i] = ((i + j) % 2 == 0 ? 1 : -1) * temp.Matrixvalue();
                }
            }
            return re;
        }
    }
}
