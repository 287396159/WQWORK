using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Ports;
using System.Threading;
using MG3732_MSG;
using System.Collections.Concurrent;
using PersionAutoLocaSys.Bean;
using PersionAutoLocaSys.Model;
namespace PersionAutoLocaSys
{
    /// <summary>
    /// 常用的集合对象
    /// </summary>
    public class CommonCollection
    {
        //组集合
        public static Object Groups_Lock = new Object();
        public static Dictionary<string, Group> Groups = new Dictionary<string, Group>();
        //区域集合
        public static ConcurrentDictionary<string, Area> Areas = new ConcurrentDictionary<string, Area>();
        //Tag的集合
        public static object Tags_Lock = new object();
        //接收数据包容器
        public static Dictionary<string,Tag> Tags = new Dictionary<string,Tag>();
        //人员管理集合
        public static Dictionary<string, Person> Persons = new Dictionary<string, Person>();
        public static ConcurrentDictionary<string, TagPack> TagPacks = new ConcurrentDictionary<string, TagPack>();
        //保存每次上报的记录
        public static System.Object RecordDBObj = new System.Object();
        public static List<TagPack> RecordDB = new List<TagPack>();
        //人员警报集合
        public static Dictionary<string, AlarmTag> AlarmTags = new Dictionary<string, AlarmTag>();
        //Router上报上来的信息集合
        public static ConcurrentDictionary<string, Router> Routers = new ConcurrentDictionary<string, Router>();
        //数据包上报集合（用于保存原始数据）
        public static List<TagPack> LogTags = new List<TagPack>();
        //用于保存原始的警报讯息
        public static List<WarmInfo> LogWarms = new List<WarmInfo>();
        //警报信息集合
        public static object warm_lock = new object();
        public static List<WarmInfo> WarmInfors = new List<WarmInfo>();
        public static List<PhonePerson> PhonePersons = new List<PhonePerson>();

        //操作记录
        public static List<PersonOperation> personOpers = new List<PersonOperation>();

        public static IPEndPoint Androidendport;
        public static bool androidenSend = false;

        /// <summary>
        /// 进出厂的保存
        /// </summary>
        public static List<AdmissionExit> admissionExits = new List<AdmissionExit>();

        /// <summary>
        /// 所有的进出场
        /// </summary>
        public static List<AdmissionExit> allAdmissionExits = new List<AdmissionExit>();
        //public static Dictionary<string, AdmissionExit> admissionExits = new Dictionary<string, AdmissionExit>();
    }

    public class PhonePerson
    {
       public int ID = 0;
       public string Name="";
       public string PhoneNumber = "";
    }

    //人员类型
    public class Person
    {
        public byte[] ID = new byte[2];
        public string Name { get; set;}
        public string Ps { get; set; }
        public PersonAccess PersonAccess = PersonAccess.SimplePerson;
        public Person()
	    { 
			ID[0] = 0;
		 	ID[1] = 0;
		  	Name = "";
		   	Ps = "";
		}
        public Person(byte[] ID, string Name, string Ps, int type)
        {
            this.ID = ID; 
			this.Name = Name; 
			this.Ps = Ps;
            if (type == 1) 
			{
				PersonAccess = PersonAccess.AdminPerson;
			}
			else 
			{
				PersonAccess = PersonAccess.SimplePerson;
			}
        }
    }

    //人员权限
    public enum PersonAccess
    {
        SimplePerson,
        AdminPerson,
        dmatekPerson,
    }
    public enum NodeType
    { 
        ReferNode,
        DataNode,
        UnKnown
    }
    [Serializable]
    public class PersonOperation
    { 
        public byte[] ID;
        public DateTime operdt;
        public OperType mopertype;
        public PersonOperation()
        {
            ID = new byte[2];
            operdt = DateTime.Now;
        }
        public PersonOperation(byte[] id,OperType mopertype)
        {
            ID = new byte[2];
            System.Buffer.BlockCopy(id, 0, ID,0,2);
            this.mopertype = mopertype;
            operdt = DateTime.Now;
        }
    }

    //操作类型
    public enum OperType
    { 
        UnKnown,

        OpenForm,
        CloseForm,

        OpenListen,
        CloseListen,

        EnterSetting,
        LeaveSetting,

        SearchTag,
        SearchRefer,
        SearchNode,

        HandlePersonHelpAlarm,
        DelePersonHelpAlarm,

        HandleAreaAdminAlarm,
        DeleAreaAdminAlarm,

        HandlePersonStopAlarm,
        DelePersonStopAlarm,

        HandleBattLackAlarm,
        DeleBattLackAlarm,

        HandleTagExcepAlarm,
        DeleTagExcepAlarm,

        HandleReferExcepAlarm,
        DeleReferExcepAlarm,

        HandleNodeExcepAlarm,
        DeleNodeExcepAlarm,

        ViewTraceAnalysis,      
        ViewAlarmRecord,
        PersonOperation,
        NodeSettingDevice,

		LoginIn,//登录
		LoginOut,//退出登录
		EnterNodeParam,//进入Node网络参数设置
        LeaveNodeParam,//退出Node参数设置
		EnterReferParam,//进入Refer参数设置
		LeaveReferParam,//退出Refer参数设置
        DebugSetAroundDeviceParam//调试模式下设置周围设备/搜索周围设备
   
	}

    public class RefReport
    {
        public byte[] Id = new byte[2];
        public byte Rssi;
        public RefReport() { }
        public RefReport(byte[] id,byte Rssi)
        {
            this.Id = id;
            this.Rssi = Rssi;
        }
    }

    //Router上报信息上来
    public class Router
    { 
        public byte[] ID = new byte[2];
        public int SleepTime = 0;
        public UInt32 Version;
        public DateTime ReportTime;
        public int TickCount = 0;
        public bool status;//true：连接状态；false：断开连接状态
        public bool isHandler;
        public NodeType CurType = NodeType.UnKnown;
        public byte[] BasicID = new byte[2];
        public IPEndPoint mendpoint = new IPEndPoint(IPAddress.Any,IPEndPoint.MinPort);
    }

    //主显示的页面中，默认显示的组，只能显示3个
    public class GroupShow
    { 
        public byte[] ID = new byte[2];
        public string Name = "";//组的名称
        public bool Selected;//当前的组是否被选中
    }
    //特殊报警
    public enum SpeceilAlarm
    {
        PersonHelp,
        //Emergy,
        AreaControl,
        BatteryLow,
        TagDis,
        ReferDis,
        NodeDis,
        Resid,//滞留
        UnKnown
    }

    /// <summary>
    /// 网络参数
    /// </summary>
    public class NetParam
    {
        public static string Ip = null;
        public static int Port = 51234;
    }
    /// <summary>
    /// 组类
    /// 每个组最多只能有16个区域，超过16个区域，
    /// 需要分到下一个组，因为主页面上每组只能显示16个区域
    /// </summary>
    public class Group
    { 
        public byte[] ID = new byte[2];
        public string Name;
    }
    /// <summary>
    /// 区域类
    /// </summary>
    public class Area
    {
        public byte[] ID = new byte[2];//区域ID
        public string Name;//区域名称
        public byte[] GroupID = new byte[2];//所属组的ID
        public AreaType AreaType;//区域类型
        public AreaMap AreaBitMap = new AreaMap();//区域地图
        //区域参考点容器
        public Dictionary<string, BasicRouter> AreaRouter = new Dictionary<string, BasicRouter>();
        //区域数据节点容器
        public Dictionary<string, BasicNode> AreaNode = new Dictionary<string, BasicNode>();
        public int CurNum;//区域当前人数，默认为0
    }
    /// <summary>
    /// 区域地图包括地图的路径
    /// </summary>
    [Serializable]
    public class AreaMap
    {
        public string MapPath;
        public Bitmap MyBitmap;
    }
    //表示Router周围位置占用情况
    [Serializable]
    public struct ReferLocaInfo 
	{
        public bool ExistTop;
        public bool ExistUpperRight;
        public bool ExistRight;
        public bool ExistLowRight;
        public bool ExistLow;
        public bool ExistLowLeft;
        public bool ExistLeft;
        public bool ExistUpperLeft;
        public int CenterNum;//中间的人数
    }
    public struct NodeLocaInfo
    { 
        public bool ExistFirstPosition;
        public bool ExistSecondPosition;
        public bool ExistThirdPosition;
        public int CenterNum;//中间的人数
    }
    public class Devices
    {

        public ConcurrentDictionary<string, referparam> referdevs = new ConcurrentDictionary<string, referparam>();
        public ConcurrentDictionary<string, nodenwparam> nodedevs = new ConcurrentDictionary<string,nodenwparam>();
    }
    //网络参数类
    public class nodenwparam
    {
        public byte[] id = new byte[2];
        public byte[] serverip = new byte[4];//Server的IP
        public UInt16 port = 0;//Server端口
        public byte ipmode = 1;//IP模式
        public byte channel = 0;
        public byte[] nodeip = new byte[4];//节点IP
        public byte[] submask = new byte[4];//节点掩码
        public byte[] gateway = new byte[4];//节点网关
        public byte[] wifiname = new byte[32];
        public byte[] wifipsw = new byte[32];
        public byte status = 0;
        public byte wifisig = 0;
        public byte type = byte.MaxValue;
        public UInt32 version = UInt32.MaxValue;
    }
    //参考点参数
    public class referparam
    { 
        public byte[] id = new byte[2];
        public byte Sgthreshold;
        public byte Sgstrengthfac;
        public byte type = byte.MaxValue;
        public UInt32 version = UInt32.MaxValue;
        public byte nodetorefersig;
        public byte refertonodesig;
    }

    public class NodeMsg : Router
    {
        public byte type = 0;//0x02:表示ZB2530_Lan_V02.02
    }
    [Serializable]
    public class BasicNode
    { 
        public byte[] ID = new byte[2];
        public string Name = "";
        public int x=0;
        public int y=0;
        public bool isVisible = false;
        public bool isReport = false;
        public int NoReportTick = 0;
    }
    /// <summary>
    /// 参考点基类
    /// </summary>
    [Serializable]
    public class BasicRouter
    {
        public byte[] ID = new byte[2];
        public string Name = "";
        public int x = 0;
        public int y = 0;
        public bool isVisible = false;//在地图上是否可见
        //表示当前的Router哪些位置被占用
        public NodeLocaInfo CurLocaInfor;
        public int Num = 0;
        public int NoReportTick = 0;
        public bool isReport = false;

        public ReferAroundPosition GetFreePlace()
        {
            if (!CurLocaInfor.ExistFirstPosition)
                return ReferAroundPosition.FirstPosition;
            if (!CurLocaInfor.ExistSecondPosition)
                return ReferAroundPosition.SecondPosition;
            if (!CurLocaInfor.ExistThirdPosition)
                return ReferAroundPosition.ThirdPosition;
            return ReferAroundPosition.CenterPosition;
        }

        public ReferAroundPosition GetOkPlace(ReferAroundPosition MyRouterPlace)
        {
            if (RtAroundTagPlace.CurImageTag.RD_Old[0] != RtAroundTagPlace.CurImageTag.RD_New[0] ||
                RtAroundTagPlace.CurImageTag.RD_Old[1] != RtAroundTagPlace.CurImageTag.RD_New[1])
            {//说明两次Tag的位置有跳动
                return GetFreePlace();
            }
            if (RtAroundTagPlace.CurBasic.GetStandStatus(MyRouterPlace))
            {
                return GetFreePlace();
            }
            else
            {
                if (MyRouterPlace == ReferAroundPosition.CenterPosition)
                    return GetFreePlace();
                else return MyRouterPlace;
            }
        }
        public ReferAroundPosition GetLastPlace(ReferAroundPosition MyRouterPlace)
        {
            switch (MyRouterPlace)
            {
                case ReferAroundPosition.FirstPosition:
                    return ReferAroundPosition.SecondPosition;
                case ReferAroundPosition.SecondPosition:
                    return ReferAroundPosition.ThirdPosition;
                case ReferAroundPosition.ThirdPosition:
                    return ReferAroundPosition.CenterPosition;
                default:
                    return ReferAroundPosition.UnKnown;
            }
        }
        public bool GetStandStatus(ReferAroundPosition MyRouterPlace)
        {
            switch (MyRouterPlace)
            {
                case ReferAroundPosition.FirstPosition:
                    return CurLocaInfor.ExistFirstPosition;
                case ReferAroundPosition.SecondPosition:
                    return CurLocaInfor.ExistSecondPosition;
                case ReferAroundPosition.ThirdPosition:
                    return CurLocaInfor.ExistThirdPosition;
            }
            return false;
        }
        public void StandPlace(ReferAroundPosition MyRouterPlace)
        {
            switch (MyRouterPlace)
            {
                case ReferAroundPosition.FirstPosition:
                    CurLocaInfor.ExistFirstPosition = true;
                    break;
                case ReferAroundPosition.SecondPosition:
                    CurLocaInfor.ExistSecondPosition = true;
                    break;
                case ReferAroundPosition.ThirdPosition:
                    CurLocaInfor.ExistThirdPosition = true;
                    break;
                case ReferAroundPosition.CenterPosition:
                    Num++;
                    break;
            }
        }
        public void ClearAllPlace()
        {
            CurLocaInfor.ExistFirstPosition = false;
            CurLocaInfor.ExistSecondPosition = false;
            CurLocaInfor.ExistThirdPosition = false;
            CurLocaInfor.CenterNum = 0;
        }
        public void ClearPlaceStand(ReferAroundPosition MyRouterPlace)
        {
            switch (MyRouterPlace)
            {
                case ReferAroundPosition.FirstPosition:CurLocaInfor.ExistFirstPosition = false;break;
                case ReferAroundPosition.SecondPosition: CurLocaInfor.ExistSecondPosition = false;break;
                case ReferAroundPosition.ThirdPosition: CurLocaInfor.ExistThirdPosition = false;break;
                case ReferAroundPosition.CenterPosition:if (Num > 0)Num--;break;
            }
        }
    }

    [Serializable]
    public class Tag
    {
        public byte[] ID = new byte[2];
        public byte[] workID = new byte[16];
        //Tag的名称
        public string Name;
        //是否停滞报警
        public bool IsStopAlarm = false;
        //停滞报警时间
        public int StopTime;
        //可进的区域（以参考点ID为区分）
        public List<string> TagReliableList = new List<string>();

        public WorkTime CurTagWorkTime = WorkTime.UnKnown;
        public DateTime StartWorkDT, EndWorkDT;
        public WorkTime CurGSWorkTime = WorkTime.UnKnown;
        public DateTime StartGSDT, EndGSDT;
    }

    public enum WorkTime 
    {
        AlwaysWork,
        LimitTime,
        NoWork,
        UnKnown
    }

    //画在界面上的Tag类
    public class ImageTag
    {
        public byte[] ID = new byte[2];//Tag的ID
        public byte[] RD = new byte[2];//上次Router的ID
        public int TagStatus = -1;//表示卡片的状态，0：普通；1：警告
        public string TagName = "";//Tag的名称
        //表示当前的Tag位于Router的什么位置
        //ReferAroundPosition CurPlace = ReferAroundPosition.UnKnown;
    }

    public enum ReferAroundPosition
    {
        FirstPosition,
        SecondPosition,
        ThirdPosition,
        CenterPosition,
        UnKnown
    }

    /// <summary>
    /// 卡片的区域选择，
    /// </summary>
    [Serializable]
    public class AreaSelect
    {
        public bool SimpleArea{set;get;}
        public bool ControlArea { set; get; }
        public bool DangerArea { set; get; }
        public void Clear()
        {
            SimpleArea = false;
            ControlArea = false;
            DangerArea = false;
        }
        public string GetAreasStr()
        {
            string StrAreas = "";
            if (SimpleArea)
            {
                StrAreas += ConstInfor.StrSimpleArea + "、";
            }
            if (ControlArea)
            {
                StrAreas += ConstInfor.StrControlArea + "、";
            }
            if (DangerArea)
            {
                StrAreas += ConstInfor.StrDangerArea + "、";
            }
            if (StrAreas.EndsWith("、"))
            {
                StrAreas = StrAreas.Substring(0, StrAreas.Length-1);
            }
            return StrAreas;
        }
    }

    /// <summary>
    /// 区域类型
    /// </summary>
    [Serializable]
    public enum AreaType
    {
        SimpleArea,
        ControlArea,
        DangerArea
    }

    /// <summary>
    /// 按钮的状态
    /// </summary>
    public enum BtnStatus
    {
        Bt_start_No_Press,
        Bt_start_Press,//按键按下
        Bt_stop_No_Press,//按键松开
        Bt_stop_Press,
        Bt_Double
    }
    [Serializable]
    public class TagPack
    { 
        public byte[] TD = new byte[2];//Tag的ID
        public byte[] RD_New = new byte[2];//Refer的ID(新)
        public byte[] RD_Old = new byte[2];//Refer的ID(旧)
       // public byte[] tempId = new byte[2];
       // public byte tempssi = 0;

        public byte[] ND = new byte[2];//数据节点的ID
        public byte isAlarm;//是否警报，其中0表示正常，1表示警报
        public byte SigStren;
        public byte Bat;//电池电量
        public byte index;//数据包的序列号
        public int Sleep;//Tag的休眠时间
        public DateTime ReportTime;//数据包上报
        public int TickCount;
        public bool isVisble;//该Tag在图形显示时是否可见
        public int LostCount;//丢包数量
        public int JumpNum = 0;
        public byte[] LastJumpId = new byte[2]; 

        public int ResTime;//没有运动
        // 表示当前的Tag在Router上的位置
        public ReferAroundPosition CurPlace = ReferAroundPosition.UnKnown;
        public bool isUseGSensor = true;

        //新增加的Tag的讯息
        public int StaticSleep;//Tag静止时上报的间隔时间
        public int BasicRefsNum;//当前基站的数量
        public Dictionary<string, RefReport> BaseRefs = new Dictionary<string, RefReport>();//保存Tag定位包携带的Refer讯息
        public int limitlossnum = 0;


    }
    /// <summary>
    /// 警告讯息的抽象类
    /// </summary>
    [Serializable]
    public class WarmInfo
    {
        public byte[] RD = new byte[2];
        public byte[] AD = new byte[2];
        public String RDName;
        public String AreaName;
        public DateTime AlarmTime = new DateTime();//警告时间
        public DateTime ClearAlarmTime = new DateTime();//警报解除时间
        public bool isHandler;//是否处理
        public bool isSendMsg = false;//是否发送短信
        public bool isClear;//是否清除掉
    }
    /// <summary>
    /// 人员求救报警类
    /// </summary>
    [Serializable]
    public class PersonHelp : WarmInfo
    {
        public byte[] TD = new byte[2];//抽象类中的属性
        public string TagName;//Tag的名称
    }
    /// <summary>
    /// 区域管制类
    /// </summary>
    [Serializable]
    public class AreaAdmin : WarmInfo
    {
        public byte[] TD = new byte[2];//抽象类中的属性
        public string TagName;//Tag的名称
        //可进区域选择
        public AreaSelect TagAreaSt;
        public AreaType AreaType;//区域类型
    }
    /// <summary>
    /// 人员滞留
    /// </summary>
    [Serializable]
    public class PersonRes : WarmInfo
    {
        public byte[] TD = new byte[2];
        public string TagName;//Tag的名称
        public int ResTime;
        public int BasicResTime;
    }
    /// <summary>
    /// 低电量报警
    /// </summary>
    [Serializable]
    public class BattLow : WarmInfo
    {
        public byte[] TD = new byte[2];//抽象类中的属性
        public string TagName;//Tag的名称
        public byte Batt;
        public byte BasicBatt;//系统设置的低电量标准
    }
    /// <summary>
    /// Tag断开连接
    /// </summary>
    [Serializable]
    public class TagDis : WarmInfo
    {
        public byte[] TD = new byte[2];//抽象类中的属性
        // Tag的名称
        public string TagName;
        public int SleepTime;
    }
    /// <summary>
    /// 参考点断开连接
    /// </summary>
    [Serializable]
    public class ReferDis:WarmInfo
    {
        public int SleepTime;//休眠时间
    }
    /// <summary>
    /// 数据节点断开连接
    /// </summary>
    [Serializable]
    public class NodeDis:WarmInfo
    {
        public int SleepTime;
    }

    public class AlarmTag
    { 
        public byte[] TD = new byte[2];
        public byte[] RD = new byte[2];
        public DateTime AlarmTime;//警告时间
        public int AlarmCount;//警告次数
        public bool isHandler;//是否处理
        public SpeceilAlarm MySpeceilAlarm = SpeceilAlarm.UnKnown;//警报的种类
        //若是滞留报警的话，会有添加的滞留时间信息
        public int StopTime;
        //若是低电量报警的话，会有添加的低电量信息
        public int LowBattery;
    }


    /// <summary>
    /// 对通用的集合对象中的数据进行操作
    /// </summary>
    public class CommonBoxOperation
    {
        /// <summary>
        /// 获取Tag的名称
        /// </summary>
        /// <param name="StrTagID"></param>
        /// <returns></returns>
        public static String GetTagName(string StrTagID)
        {
            if (null == StrTagID || "".Equals(StrTagID))
                return null;
            Tag MyTag = null;
            lock (CommonCollection.Tags_Lock)
            {
                CommonCollection.Tags.TryGetValue(StrTagID, out MyTag);
            }
            if (MyTag == null)
                return null;
            return MyTag.Name;
        }
        /// <summary>
        /// 获取Tags中存在指定可进Refer的Tag
        /// </summary>
        /// <param name="strReferID"></param>
        /// <returns></returns>
        public static Tag GetExistRefer(string strReferID)
        {
            List<Tag> tags = CommonCollection.Tags.Values.ToList<Tag>();
            foreach (Tag tag in tags)
            {
                if (null == tag)
                {
                    continue;
                }
                foreach(string strreferid in tag.TagReliableList)
                {
                    if (strreferid.Equals(strReferID))
                    {
                        return tag;
                    }
                }
            }
            return null;
        }

        //得到系统管理员
        public static Person GetAdminPerson()
        {
                foreach (KeyValuePair<string, Person> ps in CommonCollection.Persons)
                {
                    if (null == ps.Value) continue;
                    if (ps.Value.PersonAccess == PersonAccess.AdminPerson)
                    {
                        return ps.Value;
                    }
                }
            return null;
        }
        /// <summary>
        /// 获取Tag的设置信息
        /// </summary>
        /// <param name="StrTagID"></param>
        /// <returns></returns>
        public static Tag GetTag(string StrTagID)
        {
            Tag MyTag = null;
            lock (CommonCollection.Tags_Lock)
            {
                CommonCollection.Tags.TryGetValue(StrTagID, out MyTag);
            }
            return MyTag;
        }
        /// <summary>
        /// 获取某一区域的Tag的数量
        /// </summary>
        public static int GetNumber(Area area)
        {
            String StrAreaID = area.ID[0].ToString("X2") + area.ID[1].ToString("X2");
            String StrRouterID;
            int Count = 0;
            try{
                foreach (KeyValuePair<string, TagPack> tp in CommonCollection.TagPacks)
                {
                    StrRouterID = tp.Value.RD_New[0].ToString("X2") + tp.Value.RD_New[1].ToString("X2");
                    if (null == area.AreaRouter)
                    return 0;
                    if(area.AreaRouter.ContainsKey(StrRouterID))
                        Count++;
                }
            }catch(Exception)
            {
            }
            return Count;
        }
        public static string GetRouterName(string StrRouterID)
        {
            BasicRouter Br = GetRouter(StrRouterID);
            if (null == Br)
                return null;
            else
                return Br.Name;
        }
        //得到节点的名称
        public static string GetNodeName(string StrNodeID)
        {
            BasicNode bsn = GetNode(StrNodeID);
            if (null == bsn) return null;
            else return bsn.Name;
        }
        //根据ID得到节点
        public static BasicNode GetNode(string StrNodeID)
        {
            if (null == StrNodeID)
                return null;
            BasicNode bsn = null;
                foreach (KeyValuePair<string, Area> MyArea in CommonCollection.Areas)
                {
                    if (null == MyArea.Value.AreaNode)
                        continue;
                    bsn = null;
                    MyArea.Value.AreaNode.TryGetValue(StrNodeID, out bsn);
                    if (null == bsn)
                    {
                        continue;
                    }
                    break;
                }
            
            return bsn;
        }
        /// <summary>
        /// 根据Tag的名称得到Tag
        /// </summary>
        /// <param name="tagname"></param>
        /// <returns></returns>
        public static Tag GetTagFromName(string tagname)
        {
            List<Tag> tags = CommonCollection.Tags.Values.ToList<Tag>();
            foreach (Tag tag in tags)
            {
                if (tag.Name.Equals(tagname))
                {
                    return tag;
                }
            }
            return null;
        }
        public static TagPack CloneTagPack(TagPack OldTpk)
        {
            if (null == OldTpk) return null;
            TagPack NewTagPack = new TagPack();
            NewTagPack.TD[0] = OldTpk.TD[0]; NewTagPack.TD[1] = OldTpk.TD[1];
            NewTagPack.RD_New[0] = OldTpk.RD_New[0]; NewTagPack.RD_New[1] = OldTpk.RD_New[1];
            NewTagPack.Bat = OldTpk.Bat;NewTagPack.CurPlace = OldTpk.CurPlace;NewTagPack.index = OldTpk.index;
            NewTagPack.isAlarm = OldTpk.isAlarm;NewTagPack.isVisble = OldTpk.isVisble;NewTagPack.LostCount = OldTpk.LostCount;
            NewTagPack.ND[0] = OldTpk.ND[0]; NewTagPack.ND[1] = OldTpk.ND[1];
            NewTagPack.ResTime = OldTpk.ResTime;
            NewTagPack.RD_Old[0] = OldTpk.RD_Old[0]; NewTagPack.RD_Old[1] = OldTpk.RD_Old[1];
            NewTagPack.ReportTime = OldTpk.ReportTime;NewTagPack.ResTime = OldTpk.ResTime;
            NewTagPack.SigStren = OldTpk.SigStren;NewTagPack.Sleep = OldTpk.Sleep;
            return NewTagPack;
        }
        /// <summary>
        /// 根据Router的ID获取Router
        /// </summary>
        /// <param name="StrRouterID"></param>
        /// <returns></returns>
        public static BasicRouter GetRouter(String StrRouterID)
        {
            if (null == StrRouterID)
                return null;
            BasicRouter basRt = null;
            foreach (KeyValuePair<string, Area> MyArea in CommonCollection.Areas)
            {
                if (null == MyArea.Value)
                    continue;
                if (null == MyArea.Value.AreaRouter)
                    continue;
                MyArea.Value.AreaRouter.TryGetValue(StrRouterID, out basRt);
                if (null == basRt)
                    continue;
                break;
            }
            return basRt;
        }

        public static Area GetAreaFromNodeID(string StrNodeID)
        {
            foreach (KeyValuePair<string, Area> area in CommonCollection.Areas)
            {
                if (null == area.Value)
                    continue;
                foreach (KeyValuePair<string, BasicNode> br in area.Value.AreaNode)
                {
                    if (null == br.Value)
                        continue;
                    if ((br.Key).Equals(StrNodeID))
                    {
                        return area.Value;
                    }
                }
            }
            return null;  
        }

        /// <summary>
        /// 根据Router的ID得到Router所在的区域信息
        /// </summary>
        /// <param name="StrRouterID"></param>
        /// <returns></returns>
        public static Area GetAreaFromRouterID(String StrRouterID)
        {
            foreach (KeyValuePair<string, Area> area in CommonCollection.Areas)
            {
                if (null == area.Value)
                    continue;
                foreach (KeyValuePair<string, BasicRouter> br in area.Value.AreaRouter)
                {
                    if (null == br.Value)
                        continue;
                    if ((br.Key).Equals(StrRouterID))
                    {
                        return area.Value;
                    }
                }
            }
            return null;         
        }
        /// <summary>
        /// 获取Tag在Router上的序号
        /// </summary>
        /// <param name="StrRouterID"></param>
        /// <returns></returns>
        public static int GetRouterAroundNum(String StrTagID,String StrRouterID)
        {
            int num = 0;
            String StrRtID; 
            try{ 
                foreach(KeyValuePair<string,TagPack> Tp in CommonCollection.TagPacks)
                { 
                    //获取到Router的ID
                    StrRtID = Tp.Value.RD_New[0].ToString("X2") + Tp.Value.RD_New[1].ToString("X2");
                    if (StrRtID.Equals(StrRouterID))
                    {
                        if (Tp.Key.Equals(StrTagID))
                        {
                            return num;
                        }
                        num++;
                    }
                    else
                        continue;
                }
            }catch(Exception)
            {
            }
            return num;
        }

        /// <summary>
        /// 获取Router当前参考点的数量
        /// </summary>
        /// <param name="StrRouterID"></param>
        /// <returns></returns>
        public static int GetRouterAroundNum(String StrRouterID)
        {
            int num = 0;
            String StrRtID;
            try{
                foreach (KeyValuePair<string, TagPack> Tp in CommonCollection.TagPacks)
                {
                    //获取到Router的ID
                    StrRtID = Tp.Value.RD_New[0].ToString("X2") + Tp.Value.RD_New[1].ToString("X2");
                    if (StrRtID.Equals(StrRouterID))
                    {
                        num++;
                    }
                    else
                        continue;
                }
            }catch(Exception)
            {
            }
            return num;
        }
        /// <summary>
        /// 根据Tag的ID获取Tag的当前状态，其中-1:表示异常，0：表示正常状态，1:表示警报状态
        /// </summary>
        /// <param name="StrTagID"></param>
        /// <returns></returns>
        public static int GetTagStatus(String StrTagID)
        {
            if (null == StrTagID)
                return -1;
            TagPack TgPk = null;
            try{
                CommonCollection.TagPacks.TryGetValue(StrTagID, out TgPk);
                if (null == TgPk)
                    return -1;
                if (TgPk.isAlarm == 1)
                    return 1;
                else
                    return 0;
            }catch(Exception)
            {
                return -1;
            }
        }
        /// <summary>
        /// 判断Tag是否在指定的区域
        /// </summary>
        /// <param name="StrTagID"></param>
        /// <param name="StrAreaID"></param>
        /// <returns></returns>
        public static bool JudgeTagArea(String StrTagID,String StrAreaID)
        {
            if (null == StrTagID || "".Equals(StrTagID))
                return false;
            TagPack Tp = null;
            String StrRouterID = "";
            try{
                CommonCollection.TagPacks.TryGetValue(StrTagID, out Tp);
                if (null == Tp)
                {
                    return false;
                }
                StrRouterID = Tp.RD_New[0].ToString("X2") + Tp.RD_New[1].ToString("X2");
            }catch(Exception)
            {
            }
            Area MyArea = null;
                CommonCollection.Areas.TryGetValue(StrAreaID, out MyArea);
                if (MyArea == null)
                    return false;
                if (null == MyArea.AreaRouter)
                    return false;
                if (MyArea.AreaRouter.ContainsKey(StrRouterID))
                    return true;
            
            return false;
        }
        /// <summary>
        /// 根据Router的ID获取BasicRouter对象
        /// </summary>
        /// <param name="StrRouterID"></param>
        /// <returns></returns>
        public static BasicRouter GetBasicRouter(string StrRouterID)
        {
            if (null == StrRouterID)
                return null;
   
                foreach(KeyValuePair<string,Area> area in CommonCollection.Areas)
                {
                    if (null == area.Value)
                        continue;
                    if (null == area.Value.AreaRouter)
                        continue;
                    BasicRouter MyBasicRouter = null;
                    area.Value.AreaRouter.TryGetValue(StrRouterID, out MyBasicRouter);
                    if (null == MyBasicRouter)
                        continue;
                    else
                        return MyBasicRouter;
                }
            
            return null;

        }

        /// <summary>
        /// 根据Router的ID获取Router所在的区域
        /// </summary>
        /// <param name="StrRouterID"></param>
        /// <returns></returns>
        public static Area GetRouterArea(String StrRouterID)
        { 

                foreach(KeyValuePair<string,Area> MyArea in CommonCollection.Areas)
                {
                    if (null == MyArea.Value.AreaRouter)
                        continue;
                    if (MyArea.Value.AreaRouter.ContainsKey(StrRouterID))
                        return MyArea.Value;
                }
            
            return null;
        }
        /// <summary>
        /// 获取指定类型的警报未处理的Tag的数量
        /// </summary>
        /// <param name="MySpeceilAlarm"></param>
        /// <returns></returns>
        public static int GetAlarmNoHandleTagsNum(SpeceilAlarm MySpeceilAlarm)
        {
            int count = 0;
            try
            {
                foreach (WarmInfo wminfo in CommonCollection.WarmInfors)
                {
                    if (null == wminfo) continue;
                    if (wminfo.isHandler) continue;
                    string ClassName = wminfo.GetType().Name;
                    switch (MySpeceilAlarm)
                    {
                        case SpeceilAlarm.PersonHelp:
                            if ("PersonHelp".Equals(ClassName))
                            {
                                count++;
                            }
                            break;
                        case SpeceilAlarm.AreaControl:
                            if ("AreaAdmin".Equals(ClassName))
                            {
                                count++;
                            }
                            break;
                        case SpeceilAlarm.Resid:
                            if ("PersonRes".Equals(ClassName))
                            {
                                count++;
                            }
                            break;
                        case SpeceilAlarm.BatteryLow:
                            if ("BattLow".Equals(ClassName))
                            {
                                count++;
                            }
                            break;
                        case SpeceilAlarm.NodeDis:
                            if ("NodeDis".Equals(ClassName))
                            {
                                count++;
                            }
                            break;
                        case SpeceilAlarm.ReferDis:
                            if ("ReferDis".Equals(ClassName))
                            {
                                count++;
                            }
                            break;
                        case SpeceilAlarm.TagDis:
                            if ("TagDis".Equals(ClassName))
                            {
                                count++;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }catch(Exception)
            {
            }
            return count;
        }
         /// <summary>
            /// 获取对应人员警报讯息的数量
         /// </summary>
         /// <param name="ss"></param>
         /// <returns></returns>
        public static int GetAlarmTagsNum(SpeceilAlarm MySpeceilAlarm)
        {
            int count = 0;
            try
            {
                foreach (WarmInfo wminfo in CommonCollection.WarmInfors)
                {
                    if (null == wminfo) continue;
                    if (wminfo.isClear) continue;
                    string ClassName = wminfo.GetType().Name;
                    switch (MySpeceilAlarm)
                    {
                        case SpeceilAlarm.PersonHelp:
                            if ("PersonHelp".Equals(ClassName))
                            {
                                count++;
                            }
                            break;
                        case SpeceilAlarm.AreaControl:
                            if ("AreaAdmin".Equals(ClassName))
                            {
                                count++;
                            }
                            break;
                        case SpeceilAlarm.Resid:
                            if ("PersonRes".Equals(ClassName))
                            {
                                count++;
                            }
                            break;
                        case SpeceilAlarm.BatteryLow:
                            if ("BattLow".Equals(ClassName))
                            {
                                count++;
                            }
                            break;
                        case SpeceilAlarm.NodeDis:
                            if ("NodeDis".Equals(ClassName))
                            {
                                count++;
                            }
                            break;
                        case SpeceilAlarm.ReferDis:
                            if ("ReferDis".Equals(ClassName))
                            {
                                count++;
                            }
                            break;
                        case SpeceilAlarm.TagDis:
                            if ("TagDis".Equals(ClassName))
                            {
                                count++;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }catch(Exception)
            {
            
            }
            return count;
        }
        /// <summary>
        /// 获取特殊人员报警资讯的数量
        /// </summary>
        /// <returns></returns>
        public static int GetAlarmTagsNum()
        {
            int count = 0;
            try{
                foreach (KeyValuePair<string, AlarmTag> alarmtag in CommonCollection.AlarmTags)
                {
                    if (null == alarmtag.Value)
                        continue;
                    if (alarmtag.Value.isHandler)
                        continue;
                    if (SysParam.isPersonHelp && alarmtag.Value.MySpeceilAlarm == SpeceilAlarm.PersonHelp)
                        count++;
                    else if (SysParam.isAreaControl && alarmtag.Value.MySpeceilAlarm == SpeceilAlarm.AreaControl)
                        count++;
                    else if (SysParam.isEmergy && alarmtag.Value.MySpeceilAlarm == SpeceilAlarm.Resid)
                        count++;
                }
            }catch(Exception)
            {
            }
            return count;
         }

        /// <summary>
        /// 将某一项Tag设置为处理
        /// </summary>
        public static bool  SetAlarmTagHandler(String StrAlarmTagID)
        {
            if (null == StrAlarmTagID)
                return false;
                
            AlarmTag MyAlarmTag = null;
            CommonCollection.AlarmTags.TryGetValue(StrAlarmTagID, out MyAlarmTag);
            if (null == MyAlarmTag)
            {
              return false;
            }else MyAlarmTag.isHandler = true;
            return true;
        }
        /// <summary>
        /// 将Router设置为处理
        /// </summary>
        /// <param name="StrRouterID"></param>
        /// <returns></returns>
        public static bool SetAlarmRouterHandler(String StrRouterID)
        {
            if (null == StrRouterID)
                return false;
            Router rt = null;

                if (CommonCollection.Routers.TryGetValue(StrRouterID, out rt))
                {
                    rt.isHandler = true;
                    return true;
                }
            
            return false;
        }

        public static bool IsAlarmRouterHandler(String StrRouterID)
        {
            if (null == StrRouterID)
                return false;
            Router rt = null;

            CommonCollection.Routers.TryGetValue(StrRouterID, out rt);
            if (null != rt)
                return rt.isHandler;
            return false;
        }
        /// <summary>
        /// 设置指定的Tag可见,其他的Tag不可见
        /// </summary>
        /// <param name="StrTagID"></param>
        /// <returns></returns>
        public static void SetTagVisible(String StrTagID)
        {
            if (null == StrTagID || "".Equals(StrTagID))
                return;
            try{
                foreach (KeyValuePair<string, TagPack> tp in CommonCollection.TagPacks)
                {
                    if (StrTagID.Equals(tp.Key))
                        tp.Value.isVisble = true;
                    else
                        tp.Value.isVisble = false;
                }
            }catch(Exception)
            {
            }
            return;
        }
        //设置所有的Tag都可见
        public static void SetTagAllVisible()
        {
            try{
                foreach (KeyValuePair<string, TagPack> tp in CommonCollection.TagPacks)
                    tp.Value.isVisble = true;
            }catch(Exception)
            {
            
            }
        }
        /// <summary>
        /// 获取当前区域卡片的数量
        /// </summary>
        /// <returns></returns>
        public static int GetCurAreaTagNum(String StrAreaID)
        {
            int num = 0;
            String StrRouterID = "";
            try{
                foreach (KeyValuePair<string, TagPack> tp in CommonCollection.TagPacks)
                {
                    StrRouterID = tp.Value.RD_New[0].ToString("X2") + tp.Value.RD_New[1].ToString("X2");
                      Area MyArea = GetRouterArea(StrRouterID);
                      if (null != MyArea)
                      {
                          if ((MyArea.ID[0].ToString("X2") + MyArea.ID[1].ToString("X2")).Equals(StrAreaID))
                              num++;
                      }
                }
            }catch(Exception)
            {
            }
            return num;
        }
        //测试函数
        public static int numb = 0;
        public static byte JumpTag = 0;
        public static BasicRouter[] MyBasicRouters = new BasicRouter[3];
        public static TagPack[] MyTagPacks = new TagPack[2];
        public static int mode = 0;
        public static void Test1Tag()
        {
            if (mode <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    TagPack Altag = new TagPack();
                    Altag.TD[0] = 0x00;
                    Altag.TD[1] = (byte)i;
                    Altag.RD_New[0] = 0x00;
                    Altag.RD_New[1] = 0x00;

                    //lock (CommonCollection.TagPacks_Lock)
                        CommonCollection.TagPacks.TryAdd(Altag.TD[0].ToString("X2") + Altag.TD[1].ToString("X2"), Altag);
                }
                mode++;
            }
            TagPack MyTag = null;

            CommonCollection.TagPacks.TryGetValue("0009", out MyTag);
            if (null != MyTag)
            {
                if (MyTag.RD_New[0] == 0x00 && MyTag.RD_New[1] == 0x00)
                {
                    MyTag.RD_New[0] = 0x00;
                    MyTag.RD_New[1] = 0x00;
                }
                else
                {
                    MyTag.RD_New[0] = 0x00;
                    MyTag.RD_New[1] = 0x00;
                }
            }
        }
        public static void TestTag()
        {
            byte[] bt = new byte[2];
            {
                if (CommonCollection.TagPacks.Count > 40)
                {
                    if (JumpTag != 0)
                    {
                        if (numb % 4 == 0)
                        {
                            TagPack Altag = null;
                            try{
                                String Str = "00" + JumpTag.ToString("X2");
                                CommonCollection.TagPacks.TryGetValue(Str, out Altag);
                                if (Altag != null)
                                {
                                    Altag.RD_New[0] = 0x00; Altag.RD_New[1] = 0x05;
                                }
                            }catch(Exception)
                            {
                            }
                        }
                        else
                        {
                            TagPack Altag = null;
                            try{
                                String Str = "00" + JumpTag.ToString("X2");
                                CommonCollection.TagPacks.TryGetValue(Str, out Altag);
                                if (Altag != null)
                                {
                                    Altag.RD_New[0] = 0x00; Altag.RD_New[1] = 0x01;
                                }
                            }catch(Exception)
                            {
                            
                            }
                        }
                    }
                    JumpTag++; numb++;
                    return;
                }
            }
            numb++;
            if (numb % 2 == 0)
            {
                Random MyRandom = new Random();
                MyRandom.NextBytes(bt);
                TagPack Tp = new TagPack();
                if (bt[0] > 0 && bt[0] < 43)
                {

                    Tp.TD[0] = 0x00; Tp.TD[1] = bt[1];
                    Tp.RD_New[0] = 0x00; Tp.RD_New[1] = 0x01;
                }
                else if (bt[0] > 50 && bt[0] < 100)
                {
                    Tp.TD[0] = 0x00; Tp.TD[1] = bt[1];
                    Tp.RD_New[0] = 0x00; Tp.RD_New[1] = 0x02;
                }
                else
                {
                    Tp.TD[0] = 0x00; Tp.TD[1] = bt[1];
                    Tp.RD_New[0] = 0x00; Tp.RD_New[1] = 0x03;
                    if (JumpTag == 0)
                    {
                        JumpTag = bt[1];
                    }
                }
                if (bt[0] % 3 == 0)
                    Tp.isAlarm = 1;
                if(RtAroundTagPlace.mode == 0)
                    Tp.isVisble = true;
                else if(RtAroundTagPlace.mode == 1)
                    Tp.isVisble = false;
                String StrID = Tp.TD[0].ToString("X2") + Tp.TD[1].ToString("X2");
                try{
                    if (!CommonCollection.TagPacks.ContainsKey(StrID))
                    {
                        CommonCollection.TagPacks.TryAdd(StrID, Tp);
                    }
                }catch(Exception)
                {
                }
                //让JumpTag每隔几次跳动一次
                if (JumpTag != 0)
                {
                    if (numb % 4 == 0)
                    {
                        TagPack Altag = null;
                        try{
                            String Str = "00" + JumpTag.ToString("X2");
                            CommonCollection.TagPacks.TryGetValue(Str, out Altag);
                            if (Altag != null)
                            {
                                Altag.RD_New[0] = 0x00; Altag.RD_New[1] = 0x05;
                            }
                        }catch(Exception)
                        {
                        
                        }
                    }
                    else
                    {
                        TagPack Altag = null;
                        try{
                            String Str = "00" + JumpTag.ToString("X2");
                            CommonCollection.TagPacks.TryGetValue(Str, out Altag);
                            if (Altag != null)
                            {
                                Altag.RD_New[0] = 0x00; Altag.RD_New[1] = 0x01;
                            }
                        }catch(Exception)
                        {
                        }
                    }
                }
                //警告讯息需要添加到警告容器中
                if(Tp.isAlarm == 1)
                {
                    AlarmTag MyAlarmTag = null;
                    try{
                        CommonCollection.AlarmTags.TryGetValue(StrID, out MyAlarmTag);
                        if (null == MyAlarmTag)
                        {
                            MyAlarmTag = new AlarmTag();
                            MyAlarmTag.TD = Tp.TD;
                            MyAlarmTag.RD = Tp.RD_New;
                            MyAlarmTag.AlarmTime = DateTime.Now;
                            MyAlarmTag.AlarmCount = 1;
                            if (CommonCollection.AlarmTags.Count%2 == 0)
                                MyAlarmTag.MySpeceilAlarm = SpeceilAlarm.PersonHelp;
                            else
                                MyAlarmTag.MySpeceilAlarm = SpeceilAlarm.Resid;
                            MyAlarmTag.isHandler = false;
                            CommonCollection.AlarmTags.Add(StrID, MyAlarmTag);
                        }
                        else
                        {
                            MyAlarmTag.AlarmTime = DateTime.Now;
                            MyAlarmTag.AlarmCount = 1;
                            MyAlarmTag.isHandler = false;
                        }
                    }catch(Exception)
                    {
                    
                    }
                }
            }
        }
        /// <summary>
        /// 对组别中的显示项进行更新,默认让前三项显示
        /// </summary>
        public static void UpdateShowGroups() 
        {
            //将GroupShows数组中的所有项都置为空
            for (int i = 0; i < SysParam.GroupShows.Length; i++)
            {
                SysParam.GroupShows[i] = null;
            }
                int index = 0,rec = 0;
                foreach (KeyValuePair<string, Group> MyGroup in CommonCollection.Groups)
                {
	                if (rec >= SysParam.GroupShows.Length)
	                {
	                    return;
	                }
                    if (index >= SysParam.GroupIndex)
                    {
                        GroupShow MyGroupShow = new GroupShow();
                        MyGroupShow.ID = MyGroup.Value.ID;
                        MyGroupShow.Name = MyGroup.Value.Name;
                        MyGroupShow.Selected = false;
                        SysParam.GroupShows[rec] = MyGroupShow;
                        rec++;
                    }
                    index++;
                }
        }
        /// <summary>
        /// 向左移动ShowGroups
        /// </summary>
        public static int MoveLeftShowGroupsItem()
        {
            if (SysParam.GroupIndex > 0)
            {
                SysParam.GroupIndex--;
                UpdateShowGroups();
                if (null != SysParam.GroupShows[0])
                    SysParam.GroupShows[0].Selected = true;
                return 0;
            }
            else
                return -1;
        }
        /// <summary>
        /// 判断显示的组别是否在左边缘
        /// </summary>
        /// <returns>0：左边缘；1：右边缘</returns>
        public static int isShowGroupsMargin()
        {
            KeyValuePair<string, Group>[] MyGroups = null;
            lock(CommonCollection.Groups_Lock)
                MyGroups = CommonCollection.Groups.ToArray<KeyValuePair<string, Group>>();
            if (null == MyGroups || MyGroups.Length == 0)
                return -1;
            if (null == SysParam.GroupShows[0] || null == MyGroups[0].Value)
                return -1;
            if (MyGroups[0].Key.Equals(SysParam.GroupShows[0].ID[0].ToString("X2") + SysParam.GroupShows[0].ID[1].ToString("X2")))
                return 0;
            int len = MyGroups.Length;
            if (null == SysParam.GroupShows[2] || null == MyGroups[len - 1].Value)
                return -1;
            if (MyGroups[len - 1].Key.Equals(SysParam.GroupShows[2].ID[0].ToString("X2") + SysParam.GroupShows[2].ID[1].ToString("X2")))
                return 1;
            return -1;
        }

        /// <summary>
        /// 向右移动ShowGroups
        /// </summary>
        public static int MoveRightShowGroupsItem()
        {
            if (SysParam.GroupIndex < CommonCollection.Groups.Count - 3)
            {
                SysParam.GroupIndex++;
                UpdateShowGroups();
                if (null != SysParam.GroupShows[2])
                    SysParam.GroupShows[2].Selected = true;
                return 0;
            }
            else
                return -1;
        }

        public static int GetGroupCount()
        {
            lock (CommonCollection.Groups_Lock)
            {
                return CommonCollection.Groups.Count;
            }
        }
        /// <summary>
        /// 初始化位置参考点周围的位置占用情况
        /// </summary>
        /// <param name="MyReferLocaInfo"></param>
        public static void InitReferLocaInfor(ReferLocaInfo MyReferLocaInfo)
        {
            MyReferLocaInfo.CenterNum = 0;
            MyReferLocaInfo.ExistLeft = false;
            MyReferLocaInfo.ExistLow = false;
            MyReferLocaInfo.ExistLowLeft = false;
            MyReferLocaInfo.ExistLowRight = false;
            MyReferLocaInfo.ExistRight = false;
            MyReferLocaInfo.ExistTop = false;
            MyReferLocaInfo.ExistUpperLeft = false;
            MyReferLocaInfo.ExistUpperRight = false;
        }
        /// <summary>
        /// 初始化所有参考点的周围位置占用情况
        /// </summary>
        public static void InitAllReferLocaInfo()
        { 
                foreach(KeyValuePair<string,Area> area in CommonCollection.Areas)
                {
                    if (null == area.Value)continue;
                    if (null == area.Value.AreaRouter)continue;
                    foreach(KeyValuePair<string,BasicRouter> br in area.Value.AreaRouter)
                    {
                        if (null == br.Value)continue;
                        br.Value.CurLocaInfor.CenterNum = 0;
                        br.Value.CurLocaInfor.ExistFirstPosition = false;
                        br.Value.CurLocaInfor.ExistSecondPosition = false;
                        br.Value.CurLocaInfor.ExistThirdPosition = false;
                    }
                }
                try
                { 
                    foreach(KeyValuePair<string,TagPack> tp in CommonCollection.TagPacks)
                    {
                        if (null == tp.Value)continue;
                        tp.Value.RD_Old[0] = 0x00;
                        tp.Value.RD_Old[1] = 0x00;
                    }
                }catch(Exception)
                {
                }
        }

        public static void ClearIMGFile()
        {
            ArrayList StrIMGFileNames = new ArrayList();

                foreach(KeyValuePair<string,Area> MyArea in CommonCollection.Areas)
                {
                    if (null == MyArea.Value)
                        continue;
                    if (null == MyArea.Value.AreaBitMap)
                        continue;
                    if (null == MyArea.Value.AreaBitMap.MapPath || "".Equals(MyArea.Value.AreaBitMap.MapPath))
                        continue;
                    StrIMGFileNames.Add(MyArea.Value.AreaBitMap.MapPath);
                }
            
            String[] Strs = new String[StrIMGFileNames.Count];
            int i = 0;
            foreach (Object obj in StrIMGFileNames)
            {
                Strs[i] = obj.ToString();
                i++;
            }
            FileOperation.ClearIMGFILE(Strs);
        }
        /// <summary>
        /// 获取警报集合中匹配的最后一项
        /// </summary>
        /// <param name="StrTID">节点或卡片的ID</param>
        /// <param name="CurAlarmType"></param>
        /// <returns></returns>
        
        public static WarmInfo GetWarmItem(string StrID, SpeceilAlarm CurAlarmType)
        {
            if (null == StrID || "".Equals(StrID))
            {
                return null;
            }
            if (CurAlarmType == SpeceilAlarm.UnKnown)
            {
                return null;
            }
            WarmInfo[] allwarms = CommonCollection.WarmInfors.ToArray();
            for (int i = allwarms.Length - 1; i >= 0;i--)
            {
                if (null == allwarms[i])
                {
                    continue;
                }
                string classname = allwarms[i].GetType().Name;
                switch (CurAlarmType)
                {
                    case SpeceilAlarm.PersonHelp:
                        if ("PersonHelp".Equals(classname))
                        { //说明当前的类为人员求救类
                            string StrCurID = ((PersonHelp)allwarms[i]).TD[0].ToString("X2") + ((PersonHelp)allwarms[i]).TD[1].ToString("X2");
                            if (StrID.Equals(StrCurID))
                            {
                                return allwarms[i];
                            }
                        }
                        break;
                    case SpeceilAlarm.AreaControl:
                        if ("AreaAdmin".Equals(classname))
                        { 
                            string StrCurID = ((AreaAdmin)allwarms[i]).TD[0].ToString("X2") + ((AreaAdmin)allwarms[i]).TD[1].ToString("X2");
                            if (StrID.Equals(StrCurID))
                            {
                                return allwarms[i];
                            }
                        }
                        break;
                    case SpeceilAlarm.Resid:
                        if ("PersonRes".Equals(classname))
                        { 
                            string StrCurID = ((PersonRes)allwarms[i]).TD[0].ToString("X2") + ((PersonRes)allwarms[i]).TD[1].ToString("X2");
                            if (StrID.Equals(StrCurID))
                            {
                                return allwarms[i];
                            }
                        }
                        break;
                    case SpeceilAlarm.NodeDis:
                        if ("NodeDis".Equals(classname))
                        {
                            string StrCurID = ((NodeDis)allwarms[i]).RD[0].ToString("X2") + ((NodeDis)allwarms[i]).RD[1].ToString("X2");
                            if (StrID.Equals(StrCurID))
                            {
                                return allwarms[i];
                            }
                        }
                        break;
                    case SpeceilAlarm.ReferDis:
                        if ("ReferDis".Equals(classname))
                        { 
                            string StrCurID = ((ReferDis)allwarms[i]).RD[0].ToString("X2") + ((ReferDis)allwarms[i]).RD[1].ToString("X2");
                            if (StrID.Equals(StrCurID))
                            {
                                return allwarms[i];
                            }
                        }
                        break;
                    case SpeceilAlarm.TagDis:
                        if ("TagDis".Equals(classname))
                        {
                            string StrCurID = ((TagDis)allwarms[i]).TD[0].ToString("X2") + ((TagDis)allwarms[i]).TD[1].ToString("X2");
                            if (StrID.Equals(StrCurID))
                            {
                                return allwarms[i];
                            }
                        }
                        break;
                    case SpeceilAlarm.BatteryLow:
                        if ("BattLow".Equals(classname))
                        { 
                            string StrCurID = ((BattLow)allwarms[i]).TD[0].ToString("X2") + ((BattLow)allwarms[i]).TD[1].ToString("X2");
                            if (StrID.Equals(StrCurID))
                            {
                                return allwarms[i];
                            }
                        }
                        break;
                }
            }
            return null;
        }

    }
    /// <summary>
    /// 系统参数设置操作
    /// </summary>
    public class SysParam
    {
        public static ParamSet Pst = null;
        public static bool isDrag = false;
        public static bool isNode = false;
        public static bool isMove = false;//判断是否有移动
        //判断箭头是否按下
        public static bool isLeftArrowDown = false;
        public static bool isRightArrowDown = false;
        public static BasicRouter DragBasicRouter = null;
        public static BasicNode DragBasicNode = null;
        //主窗口中默认显示的3个组
        public static int GroupIndex = 0;
        public static GroupShow[] GroupShows = new GroupShow[3];
        //用于保存当前页面上显示的当前区域信息，最多为16个
        public static Area[] CurAreas = new Area[16];
        //特殊报警设置项
        public static bool isPersonHelp = false;
        public static bool isEmergy = false;
        public static bool isAreaControl = false;
        //判断是否有跟踪的项
        public static bool isTracking = false;
        //低电量报警
        public static bool isLowBattery = true;
        public static byte LowBattery = 5;
        //停滞时间报警
        //每隔5s检测一次Tag是否断开
        public static int Measure_Interval = ConstInfor.DefaultSysScanTime;
        public static int TagDisParam1 = ConstInfor.DefaultTagDisParam1Time;
        public static int TagDisParam2 = ConstInfor.DefaultTagDisParam2Time;
        public static int RouterParam1 = ConstInfor.DefaultRouterParam1Time;
        public static int RouterParam2 = ConstInfor.DefaultRouterParam2Time;
        public static bool IsSoundAlarm = true;
        public static String SoundName = ConstInfor.DefaultSoundAlarm;
        //是否进行声音报警
        public static bool isSoundPersonHelp = true;
        public static bool isSoundAreaControl = true;
        public static bool isSoundPersonRes = true;
        public static bool isSoundBatteryLow = true;
        public static bool isSoundDeviceTrouble = true;
       
        public static bool isQueryDrag = false;
        public static int oldX = -1;
        public static int oldY = -1;
        public static int AlarmFontChangeInt = 0;
        //两次进入限制区域相隔时间参数
        public static int LimitAreaSpanTime = 30;
        //清理原始数据参数
        public static bool isClearData = true;
        public static int ClearTime = 30;//天
        //是否短信發送警報信息
        //串口名称
        public static string ComName = "";
        public static bool isMsgSend = false;
        public static bool isSendPersonHelpMsg = false;
        public static bool isSendAreaControlMsg = false;
        public static bool isSendPersonResMsg = false;
        public static bool isSendBatteryLowMsg = false;
        public static bool isSendDeviceTrouble = false;
        //定时清理已处理的警告讯息
        public static bool isOnTimeClearHandlerWarm = true;
        public static int OnTimes = 7;
        public static int SoundTime = 0;

        //连续n次发生跳动Tag才变换位置
       // public static int JumpLimitNum = 2;
        //当前的用户ID
        public static Person CurPerson = null;
        //清理时间
        public static int cleardaysnum = 30;

        public static bool scandevtype = false;
        public static NodeMsg mnodemsg = null;
        public static int tickcount = 0;
        public static int sendcount = 3;

        public static bool cbparm = false;
        public static int currentaddr = 0;

        public static nodenwparam mntparm = new nodenwparam();
        public static bool ischeckcs = false;
        public static referparam mfrprm = new referparam();


        public static bool readmark = false;
        public static byte[] backcmds = new byte[90];

        public static byte WifiSsd = 0;
        public static Devices mdevices = new Devices();
        public static int CurrentTickCount = 0;
        public static int AutoClearTickCount = 0;
        //优化模式,默认是以信号强度差值优化
        public static OptimizationModel curOptimalMedol = OptimizationModel.SigStrengthValue;
        public static int RssiThreshold = 3;
        public static int PopTimes = 2;
        //自动清理已经处理的警告讯息

        public static bool isClearHandleAlarm = true;
        public static int AutoClearHandleAlarmTime = 3600;//默认一个3600s = 1h

        public static bool isDebug = false;
        public static bool isSettingArroundDevices = false;

        public static bool isDevCnnLoss = true;
        /// <summary>
        /// 初始化网络
        /// </summary>
        public static void InitNetParam()
        {
            Pst.IPComBox.Items.Clear();
            IPAddress[] Ips = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress Ip in Ips)
            {
                if (Ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    Pst.IPComBox.Items.Add(Ip);
                }
            }
            Pst.IPComBox.SelectedIndex = 0;
            Pst.PortTB.Text = NetParam.Port.ToString();
            Pst.NetPanel.Location = new Point(132, 12);
        }

        public static int GetListMaxID(ListView lv, int mode)
        {
            string StrID;
            int MaxValue = 0, CurValue = 0;
            byte[] ID=new byte[2];
            foreach (ListViewItem item in lv.Items)
            {
                StrID = item.Text;
                if(mode == 0)
                {
                    try
                    {
                        CurValue = Convert.ToInt32(StrID);
                    }
                    catch (Exception)
                    { 
                        continue;
                    }
                    if (CurValue > MaxValue) 
                        MaxValue = CurValue;
                }
                else
                {
                    try
                    {
                        ID[0] = Convert.ToByte(StrID.Substring(0, 2), 16);
                        ID[1] = Convert.ToByte(StrID.Substring(2, 2), 16);
                        CurValue = (int)(ID[0] << 8 | ID[1]);
                    }
                    catch (Exception)
                    { 
                        continue;
                    }
                    if (CurValue > MaxValue)
                        MaxValue = CurValue;
                }
            }
            return MaxValue;
        }
        /// <summary>
        /// 保存人员信息 
        /// </summary>
        public static void LoadPerson()
        {
            ArrayList MyList = null;
            if (!FileOperation.GetAllSegment(FileOperation.SafePath, out MyList))return;
            if (null == MyList) return;
            if (MyList.Count <= 0)
            { 
                //添加一个Name:Admin,PSW:Admin的管理人员
                Person ps = new Person();
                ps.Name = "Admin"; 
                ps.Ps = "Admin";
                ps.PersonAccess = PersonAccess.AdminPerson;
                CommonCollection.Persons.Add(ps.ID[0].ToString("X2").PadLeft(2, '0') + ps.ID[1].ToString("X2").PadLeft(2, '0'), ps);
            }

            foreach (string PersonID in MyList)
            {
                string StrPersonID = PersonID.ToString();
                Person ps = new Person();
                try
                {
                    ps.ID[0] = Convert.ToByte(StrPersonID.Substring(0, 2), 16);
                    ps.ID[1] = Convert.ToByte(StrPersonID.Substring(2, 2), 16);
                }
                catch (Exception)
                {
                    continue;
                }
                ps.Name = FileOperation.GetValue(FileOperation.SafePath, StrPersonID, FileOperation.PersonName);
                string strpass = FileOperation.GetValue(FileOperation.SafePath, StrPersonID, FileOperation.PersonPassWord);
                if (null == strpass) continue;
                ps.Ps = Encryption.Decrypt(strpass);
                string StrType = FileOperation.GetValue(FileOperation.SafePath, StrPersonID, FileOperation.PersonAccess);
                if (null == StrType) continue;
                if (ConstInfor.StrAdminPerson.Equals(StrType)) ps.PersonAccess = PersonAccess.AdminPerson;
                else ps.PersonAccess = PersonAccess.SimplePerson;
                CommonCollection.Persons.Add(ps.ID[0].ToString("X2").PadLeft(2, '0') + ps.ID[1].ToString("X2").PadLeft(2, '0'), ps);
            }
            //判断人员集合中是否有管理员，若没有采用默认的管理员
            SysParam.CurPerson = CommonBoxOperation.GetAdminPerson();
            if (null == SysParam.CurPerson)
            {
                byte[] ID = new byte[2];
                Person Prs = new Person(ID, "Admin", "Admin", 1);
                CommonCollection.Persons.Add(Prs.ID[0].ToString("X2").PadLeft(2, '0') + Prs.ID[1].ToString("X2").PadLeft(2, '0'), Prs);
            }
        }
        /// <summary>
        /// 初始化区域面板
        /// </summary>
        public static void InitAreaPara()
        {
            Pst.AreaPanel.Location = new Point(132, 12);

            Pst.AreaGroupCB.Items.Clear();
            List<KeyValuePair<string, Group>> tempgroups = null;
            lock (CommonCollection.Groups_Lock)
            {
              tempgroups = CommonCollection.Groups.OrderBy(c => c.Key).ToList();
            }
            foreach (KeyValuePair<string, Group> group in tempgroups)
            {
                AddNameID(group.Key, group.Value.Name, Pst.AreaGroupCB);
            }
            Pst.AreaGroupCB.Items.Add(ConstInfor.NoGroup);
            //默认选择第一个组别
            if (Pst.AreaGroupCB.Items.Count > 0)
            {
                if (Pst.AreaGroupCB.SelectedIndex < 0)
                {
                    Pst.AreaGroupCB.SelectedIndex = 0;
                }
            }
        }
        /// <summary>
        /// 初始化参考点面板
        /// </summary>
        public static void InitRouterPara()
        {
            Pst.RouterPanel.Location = new Point(132, 12);
            Pst.RouterAreaCB.Items.Clear();
            List<KeyValuePair<string, Area>> areas = CommonCollection.Areas.OrderBy(k => k.Key).ToList();
            foreach (KeyValuePair<string, Area> area in areas)
            {
                //判断当前的参考点是否有名称
                AddNameID(area.Key, area.Value.Name, Pst.RouterAreaCB);
            }
            Pst.RouterAreaCB.Items.Add(ConstInfor.NoArea);
            if (Pst.RouterAreaCB.Items.Count > 0)
            {
                Pst.RouterAreaCB.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// 初始化其它的相关参数
        /// </summary>
        public static void InitOtherParam()
        {
            if (SysParam.isLowBattery)
            {
                Pst.LowBatteryCB.Checked = true;
                Pst.LowBattryTX.Enabled = false;
                Pst.LowBattryTX.Text = SysParam.LowBattery.ToString();
            }
            else
            {
                Pst.LowBattryTX.Enabled = true;
                Pst.LowBattryTX.Text = "";
                Pst.LowBatteryCB.Checked = false;
            }
            Pst.SysScanTime_TB.Text = SysParam.Measure_Interval + "";
            Pst.TagDisTimeParam1TB.Text = SysParam.TagDisParam1 + "";
            Pst.TagDisTimeParam2TB.Text = SysParam.TagDisParam2 + "";
            Pst.RouterDisTimeParam1TB.Text = SysParam.RouterParam1 + "";
            Pst.RouterDisTimeParam2TB.Text = SysParam.RouterParam2 + "";
            if (SysParam.IsSoundAlarm)
            {
                Pst.SoundSelectedCbB.Visible = true;
                if (ConstInfor.DefaultSoundAlarm.Equals(SysParam.SoundName))
                {
                    Pst.SoundSelectedCbB.SelectedIndex = 0;
                    Pst.LoadSoungBtn.Visible = false;
                    Pst.label36.Visible = false;
                    Pst.SoundTimeTB.Visible = false;
                }
                else
                {
                    Pst.SoundSelectedCbB.SelectedIndex = 1;
                    Pst.LoadSoungBtn.Visible = true;
                    Pst.label36.Text = "导入的音频为：" + SysParam.SoundName;
                    Pst.label36.Visible = true;
                    Pst.LoadSoungBtn.Visible = true;
                }
                Pst.IsSoundAlarmCB.Checked = true;
                Pst.label45.Visible = true;
                Pst.label45.Visible = true;
                Pst.SoundTimeTB.Visible = true;
                Pst.label46.Visible = true;

                Pst.SoundDeviceThroubleCB.Visible = true;
                Pst.SoundBatteryLowCB.Visible = true;
                Pst.SoundAreaControlCB.Visible = true;
                Pst.SoundPersonResCB.Visible = true;
                Pst.soundPersonHelpCB.Visible = true;

                if (isSoundDeviceTrouble)
                {
                    Pst.SoundDeviceThroubleCB.Checked = true;
                }
                else
                {
                    Pst.SoundDeviceThroubleCB.Checked = false;
                }

                if (isSoundBatteryLow)
                {
                    Pst.SoundBatteryLowCB.Checked = true;
                }
                else
                {
                    Pst.SoundBatteryLowCB.Checked = false;
                }

                if (isSoundAreaControl)
                {
                    Pst.SoundAreaControlCB.Checked = true;
                }
                else
                {
                    Pst.SoundAreaControlCB.Checked = false;
                }

                if (isSoundPersonRes)
                {
                    Pst.SoundPersonResCB.Checked = true;
                }
                else
                {
                    Pst.SoundPersonResCB.Checked = false;
                }

                if (isSoundPersonHelp)
                {
                    Pst.soundPersonHelpCB.Checked = true;
                }
                else
                {
                    Pst.soundPersonHelpCB.Checked = false;
                }
            }
            else
            {
                Pst.IsSoundAlarmCB.Checked = false;
                Pst.SoundSelectedCbB.Visible = false;
                Pst.LoadSoungBtn.Visible = false;
                Pst.label36.Visible = false;
                Pst.label45.Visible = false;
                Pst.SoundTimeTB.Visible = false;
                Pst.label46.Visible = false;

                Pst.SoundDeviceThroubleCB.Visible = false;
                Pst.SoundBatteryLowCB.Visible = false;
                Pst.SoundAreaControlCB.Visible = false;
                Pst.SoundPersonResCB.Visible = false;
                Pst.soundPersonHelpCB.Visible = false;
            }
            if (SysParam.isClearData)
            {
                Pst.ClearTimeTB.Text = SysParam.ClearTime.ToString();
                Pst.IsClearTimerTabCb.Checked = true;
            }
            else
            {
                Pst.IsClearTimerTabCb.Checked = false;
            }
            if (SysParam.isOnTimeClearHandlerWarm)
            {
                Pst.OnTimeHandlerWarmTimesTB.Text = SysParam.OnTimes.ToString();
                Pst.isOnTimeClearHandlerWarmCB.Checked = true;
            }
            else
            {
                Pst.isOnTimeClearHandlerWarmCB.Checked = false;
            }
            //定时处理警报讯息
            if (SysParam.isClearHandleAlarm)
            {
                Pst.AutoClearTimesTB.Text = SysParam.AutoClearHandleAlarmTime.ToString();
                Pst.AutoClearHandleCB.Checked = true;
            }
            else
            {
                Pst.AutoClearHandleCB.Checked = false;
            }
        }
        /// <summary>
        /// 向ComBox中添加内容
        /// 例如:ID = 0001，Name = "A生活区" =>A生活区(0001)
        /// ID = 0001，Name = "" => 0001
        /// </summary>
        public static void AddNameID(String ID, String Name, ComboBox CB)
        {
            if ("".Equals(Name))
            {
                CB.Items.Add(ID);
            }
            else
			{
                CB.Items.Add(Name+"("+ID+")");
            }
        }
        /// <summary>
        /// 根据Name+ID的字符串获取区域信息
        /// </summary>
        /// <param name="StrID_Name"></param>
        /// <returns></returns>
        public static Area GetArea_IDName(string StrID_Name)
        { 
            string StrID = SysParam.GetStrID(StrID_Name);
            return GetArea_ID(StrID);
        }
        /// <summary>
        /// 根据区域的ID获取该区域
        /// </summary>
        /// <param name="StrAreaID"></param>
        /// <returns></returns>
        public static Area GetArea_ID(string StrAreaID)
        {
            if (null == StrAreaID)
            {
                return null;
            }
            Area CurArea = null;
       
                CommonCollection.Areas.TryGetValue(StrAreaID, out CurArea);
            
            return CurArea;
        }
        /// <summary>
        /// 初始化Tag面板
        /// </summary>
        public static void InitTagPara()
        {
            Pst.TagPanel.Location = new Point(132, 12);
        }
        public static void InitAlarmPara()
        {
            Pst.AlarmPanel.Location = new Point(132,12);
        }
        /// <summary>
        /// 初始化显示面板
        /// </summary>
        public static void InitShowPara()
        {
            Pst.ShowPanel.Location = new Point(132, 12);
            Pst.ShowGroupCB.Items.Clear();
            lock (CommonCollection.Groups_Lock)
            {
                foreach(KeyValuePair<string,Group> group in CommonCollection.Groups)
                {
                    AddNameID(group.Key, group.Value.Name, Pst.ShowGroupCB);
                }
            }
            Pst.ShowGroupCB.Items.Add(ConstInfor.NoGroup);
            
            if (Pst.ShowGroupCB.Items.Count > 0)
            {
                if (Pst.ShowGroupCB.SelectedIndex <= 0)
                {
                    Pst.ShowGroupCB.SelectedIndex = 0;
                }
            }
            //获取当前的组别
            Group MyGroup = GetGroup_IDName(Pst.ShowGroupCB.Text);
            Pst.ShowAreaCB.Items.Clear();
            if (null != MyGroup)
            {
                List<KeyValuePair<string, Area>> tempareas = CommonCollection.Areas.OrderBy(c => c.Key).ToList();
                foreach (KeyValuePair<string, Area> area in tempareas)
                {
                    if (null != area.Value.GroupID)
                    {
                        //判断当前的参考点是否有名称
                        if (area.Value.GroupID[0] == MyGroup.ID[0] && area.Value.GroupID[1] == MyGroup.ID[1])
                        {
                            AddNameID(area.Key, area.Value.Name, Pst.ShowAreaCB);
                        }
                    }
                }
            }
                
            
            Pst.ShowAreaCB.Items.Add(ConstInfor.NoArea);
            if (Pst.ShowAreaCB.SelectedIndex < 0)
            {
                Pst.ShowAreaCB.SelectedIndex = 0;
            }
            //获取对应的区域
            Area ar = GetArea_IDName(Pst.ShowAreaCB.Text);
            if (ar != null)
            {
                string StrAreaID = ar.ID[0].ToString("X2") + ar.ID[1].ToString("X2");
               Pst.MapPathTB.Text = ar.AreaBitMap.MapPath;
               //画出地图
               DrawAreaMap.DrawMap(Pst.ShowAreaMapPanel,Pst.AreaBitmap,Pst.MapPathTB.Text);
               //画出参考点
               DrawAreaMap.DrawBasicRouter(Pst.AreaBitmap, StrAreaID,0);
               //将图片画到地图上
               Pst.ShowAreaMapPanel_Paint(null, null);
            }
            else
            {
               Pst.MapPathTB.Text = "";
               DrawAreaMap.DrawMap(Pst.ShowAreaMapPanel,Pst.AreaBitmap,null);
               DrawAreaMap.DrawBasicRouter(Pst.AreaBitmap,null,0);
               Pst.ShowAreaMapPanel_Paint(null, null);
            }
        }
        public static Thread SendMsgThread = null;
        public static System.Object SendMsgList_Lock = new System.Object();
        public static List<WarmInfo> SendMsgList = new List<WarmInfo>();
        public static void ScanSendMsg()
        {
            string ClassName = "", StrTagID = "", StrReferID = "",
                   StrTagName = "", Msg = "", StrReferName = "";
            while (null != SendMsgThread)
            {
                Thread.Sleep(10000);
                WarmInfo[] SafeSendMsgList = null;
                lock (SendMsgList_Lock)
                    SafeSendMsgList = SendMsgList.ToArray();
                if (SafeSendMsgList.Length <= 0)
                {
                    continue;
                }
                try
                {
                    if (!MG3732_SendMSG_PUD.OpenSerial(SysParam.ComName))
                    {
                        continue;
                    }
                    if (!MG3732_SendMSG_PUD.CurSM_Param.isSIMMSGMode)
                    {
                        MG3732_SendMSG_PUD.CurSM_Param.isSIMMSGMode=MG3732_SendMSG_PUD.isSIMMSGMode();
                    }
                    if (!MG3732_SendMSG_PUD.CurSM_Param.isSIMMSGMode)
                    {
                        continue;
                    }
                    for (int i = 0; i < SafeSendMsgList.Length; i++)
                    {
                        WarmInfo CurWarm = (WarmInfo)SafeSendMsgList[i];
                        ClassName = CurWarm.GetType().Name;
                        //只要SendMsgList集合中存在的警告的讯息都需要发送简讯
                        if ("PersonHelp".Equals(ClassName))
                            StrTagID = ((PersonHelp)SafeSendMsgList[i]).TD[0].ToString("X2") + ((PersonHelp)SafeSendMsgList[i]).TD[1].ToString("X2");
                        else if ("AreaAdmin".Equals(ClassName))
                            StrTagID = ((AreaAdmin)SafeSendMsgList[i]).TD[0].ToString("X2") + ((AreaAdmin)SafeSendMsgList[i]).TD[1].ToString("X2");
                        else if ("PersonRes".Equals(ClassName))
                            StrTagID = ((PersonRes)SafeSendMsgList[i]).TD[0].ToString("X2") + ((PersonRes)SafeSendMsgList[i]).TD[1].ToString("X2");
                        else if ("BattLow".Equals(ClassName))
                            StrTagID = ((BattLow)SafeSendMsgList[i]).TD[0].ToString("X2") + ((BattLow)SafeSendMsgList[i]).TD[1].ToString("X2");
                        else if ("TagDis".Equals(ClassName))
                            StrTagID = ((TagDis)SafeSendMsgList[i]).TD[0].ToString("X2") + ((TagDis)SafeSendMsgList[i]).TD[1].ToString("X2");
                        else if ("ReferDis".Equals(ClassName))
                            StrReferID = ((WarmInfo)SafeSendMsgList[i]).RD[0].ToString("X2") + ((WarmInfo)SafeSendMsgList[i]).RD[1].ToString("X2");
                        else if ("NodeDis".Equals(ClassName))
                            StrReferID = ((WarmInfo)SafeSendMsgList[i]).RD[0].ToString("X2") + ((WarmInfo)SafeSendMsgList[i]).RD[1].ToString("X2");
                        if (null != StrTagID && !"".Equals(StrTagID))
                        {
                            StrTagName = CommonBoxOperation.GetTagName(StrTagID);
                            if (null == StrTagName || "".Equals(StrTagName)) Msg = StrTagID;
                            else Msg = StrTagName + "(" + StrTagID + ")";
                        }
                        if (null != StrReferID && !"".Equals(StrReferID))
                        {
                            StrReferName = CommonBoxOperation.GetRouterName(StrReferID);
                            if (null == StrReferName || "".Equals(StrReferName)) Msg = StrReferID;
                            else Msg = StrReferName + "(" + StrReferID + ")";
                        }
                        if ("PersonHelp".Equals(ClassName))
                        {
                            Msg += ConstInfor.PersonHelpMsg;
                        }
                        else
                        if ("AreaAdmin".Equals(ClassName))
                        {
                            Msg += ConstInfor.AreaControlMsg1;
                            string StrCurArea = "";
                            if (null != ((AreaAdmin)SafeSendMsgList[i]).AreaName && !"".Equals(((AreaAdmin)SafeSendMsgList[i]).AreaName)) StrCurArea = ((AreaAdmin)SafeSendMsgList[i]).AreaName;
                            else StrCurArea = ((AreaAdmin)SafeSendMsgList[i]).AD[0].ToString("X2") + ((AreaAdmin)SafeSendMsgList[i]).AD[1].ToString("X2");
                            Msg += StrCurArea;
                        }
                        else
                        if ("PersonRes".Equals(ClassName))
                        {
                            Msg += ConstInfor.PersonRisMsg1;
                            Msg += ((PersonRes)SafeSendMsgList[i]).BasicResTime + " s";
                            Msg += ConstInfor.PersonRisMsg2;
                        }
                        else
                        if ("BattLow".Equals(ClassName))
                        {
                           Msg += ConstInfor.TagBatteryLowMsg1;
                           Msg += ((BattLow)SafeSendMsgList[i]).Batt + "%";
                           Msg += ConstInfor.TagBatteryLowMsg2;
                        }
                        else
                        if ("TagDis".Equals(ClassName))
                        {
                            Msg += ConstInfor.TagDisMsg;
                        }
                        else
                        if ("ReferDis".Equals(ClassName))
                        {
                            Msg += ConstInfor.ReferDisMsg;
                        }
                        else
                        if ("NodeDis".Equals(ClassName))
                        {
                             Msg += ConstInfor.NodeDisMsg;
                        }
                        if (MG3732_SendMSG_PUD.CurSM_Param.SCALen < 0)
                        {
                            MG3732_SendMSG_PUD.isSendSelectCenterAddress();
                        }
                        if (MG3732_SendMSG_PUD.CurSM_Param.SCALen > 0)
                        {
                            int CurIndex = 0,sendCount = 0;
                            try
                            {
                                while (CurIndex < CommonCollection.PhonePersons.Count)
                                {
                                    if (sendCount >= ConstInfor.SendMsgFail_Count) { CurIndex++; continue; };
                                    if (MG3732_SendMSG_PUD.SendMsg(Msg, CommonCollection.PhonePersons[CurIndex].PhoneNumber))
                                    {   //每次设置发送完信息后，清理掉缓存中的数据
                                        sendCount = 0;CurIndex++; continue;
                                    }
                                    sendCount++;
                                    Thread.Yield(); Thread.Sleep(20);
                                    MG3732_SendMSG_PUD.ClearBuffer();
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    //认为短信全部发送成功
                    SendMsgList.Clear();
                }
                catch (Exception ex)
                {
                    string msg = "发送短信出现异常:" + ex.ToString();
                    FileOperation.WriteLog(msg);
                }
                finally 
                {
               
                    MG3732_SendMSG_PUD.CloseSerial();
                }
            }
        }
        public static void SendPhoneMsg(WarmInfo CurWarm)
        {
            if (CurWarm.isSendMsg)
            {
                return;
            }
            try
            {
                MG3732_SendMSG_PUD.OpenSerial(SysParam.ComName);
                if (!MG3732_SendMSG_PUD.CurSM_Param.isSIMMSGMode)
                {
                    MG3732_SendMSG_PUD.CurSM_Param.isSIMMSGMode = MG3732_SendMSG_PUD.isSIMMSGMode();
                }
                if (MG3732_SendMSG_PUD.CurSM_Param.isSIMMSGMode)
                {
                    string StrTagName = "",Msg = "",
                           StrTagID="",StrReferID="",
                           StrReferName="";
                    string ClassName =  CurWarm.GetType().Name;
                    if ("PersonHelp".Equals(ClassName))
                    {
                        if (!SysParam.isSendPersonHelpMsg) return;
                        StrTagID = ((PersonHelp)CurWarm).TD[0].ToString("X2") + ((PersonHelp)CurWarm).TD[1].ToString("X2");
                    }
                    else
                    if ("AreaAdmin".Equals(ClassName))
                    {
                        if (!SysParam.isSendAreaControlMsg) return;
                        StrTagID = ((AreaAdmin)CurWarm).TD[0].ToString("X2") + ((AreaAdmin)CurWarm).TD[1].ToString("X2");
                    }
                    else
                    if ("PersonRes".Equals(ClassName))
                    {
                        if (!SysParam.isSendPersonResMsg) return;
                        StrTagID = ((PersonRes)CurWarm).TD[0].ToString("X2") + ((PersonRes)CurWarm).TD[1].ToString("X2");    
                    }
                    else
                    if ("BattLow".Equals(ClassName))
                    {
                        if (!SysParam.isSendBatteryLowMsg) return;
                        StrTagID = ((BattLow)CurWarm).TD[0].ToString("X2") + ((BattLow)CurWarm).TD[1].ToString("X2");     
                    }
                    else
                    if ("TagDis".Equals(ClassName))
                    {
                        if (!SysParam.isSendDeviceTrouble) return;
                        StrTagID = ((TagDis)CurWarm).TD[0].ToString("X2") + ((TagDis)CurWarm).TD[1].ToString("X2");    
                    }
                    else
                    if ("ReferDis".Equals(ClassName))
                    {
                        if (!SysParam.isSendDeviceTrouble) return;
                        StrReferID = CurWarm.RD[0].ToString("X2") + CurWarm.RD[1].ToString("X2");            
                    }
                    else
                    if ("NodeDis".Equals(ClassName))
                    {
                        if (!SysParam.isSendDeviceTrouble) return;
                        StrReferID = CurWarm.RD[0].ToString("X2") + CurWarm.RD[1].ToString("X2");
                    }
                    if (null != StrTagID && !"".Equals(StrTagID))
                    {   StrTagName = CommonBoxOperation.GetTagName(StrTagID);
                        if (null == StrTagName || "".Equals(StrTagName)) Msg = StrTagID;
                        else Msg = StrTagName + "(" + StrTagID + ")";
                    }
                    if (null != StrReferID && !"".Equals(StrReferID))
                    {
                        StrReferName = CommonBoxOperation.GetRouterName(StrReferID);
                        if (null == StrReferName || "".Equals(StrReferName)) Msg = StrReferID;
                        else Msg = StrReferName + "(" + StrReferID + ")";
                    }
                    if ("PersonHelp".Equals(ClassName))
                    {
                        Msg += ConstInfor.PersonHelpMsg;
                    }
                    else
                    if ("AreaAdmin".Equals(ClassName))
                    {
                        Msg += ConstInfor.AreaControlMsg1;
                        string StrCurArea = "";
                        if (null != ((AreaAdmin)CurWarm).AreaName && !"".Equals(((AreaAdmin)CurWarm).AreaName)) StrCurArea = ((AreaAdmin)CurWarm).AreaName;
                        else StrCurArea = ((AreaAdmin)CurWarm).AD[0].ToString("X2") + ((AreaAdmin)CurWarm).AD[1].ToString("X2");
                        Msg += StrCurArea;
                    }
                    else
                    if ("PersonRes".Equals(ClassName))
                    {
                        Msg += ConstInfor.PersonRisMsg1;
                        Msg += ((PersonRes)CurWarm).BasicResTime + " s";
                        Msg += ConstInfor.PersonRisMsg2;
                    }
                    else
                    if ("BattLow".Equals(ClassName))
                    {
                        Msg += ConstInfor.TagBatteryLowMsg1;
                        Msg += ((BattLow)CurWarm).Batt + "%";
                        Msg += ConstInfor.TagBatteryLowMsg2;         
                    }
                    else
                    if ("TagDis".Equals(ClassName))
                    {
                        Msg += ConstInfor.TagDisMsg;              
                    }
                    else
                    if ("ReferDis".Equals(ClassName))
                    {
                        Msg += ConstInfor.ReferDisMsg;                  
                    }
                   else
                    if ("NodeDis".Equals(ClassName))
                    {
                        Msg += ConstInfor.NodeDisMsg;
                    }
                    #region
                    /*
                    switch (WarmType)
                    {
                        case SpeceilAlarm.PersonHelp:
                            Msg += ConstInfor.PersonHelpMsg;
                            break;
                        case SpeceilAlarm.AreaControl:
                            Msg += ConstInfor.AreaControlMsg1;
                            string StrCurArea = "";
                            if (null != ((AreaAdmin)CurWarm).AreaName && !"".Equals(((AreaAdmin)CurWarm).AreaName))StrCurArea = ((AreaAdmin)CurWarm).AreaName;
                            else StrCurArea = ((AreaAdmin)CurWarm).AD[0].ToString("X2") + ((AreaAdmin)CurWarm).AD[1].ToString("X2");
                            Msg += StrCurArea;
                            break;
                        case SpeceilAlarm.Resid:
                            Msg += ConstInfor.PersonRisMsg1;
                            Msg += ((PersonRes)CurWarm).BasicResTime + " s";
                            Msg += ConstInfor.PersonRisMsg2;
                            break;
                        case SpeceilAlarm.BatteryLow:
                            Msg += ConstInfor.TagBatteryLowMsg1;
                            Msg += ((BattLow)CurWarm).Batt + "%";
                            Msg += ConstInfor.TagBatteryLowMsg2;
                            break;
                        case SpeceilAlarm.TagDis:
                            Msg += ConstInfor.TagDisMsg;
                            break;
                        case SpeceilAlarm.ReferDis:
                            Msg += ConstInfor.ReferDisMsg;
                            break;
                        case SpeceilAlarm.NodeDis:
                            Msg += ConstInfor.NodeDisMsg;
                            break;
                        default: return;
                    }
                    */
                    #endregion
                    if (MG3732_SendMSG_PUD.CurSM_Param.SCALen < 0)
                    {
                        MG3732_SendMSG_PUD.isSendSelectCenterAddress();
                    }
                    if (MG3732_SendMSG_PUD.CurSM_Param.SCALen > 0)
                    {
                        #region
                        int CurIndex = 0,sendCount = 0;
                        try
                        {
                            while (CurIndex < CommonCollection.PhonePersons.Count)
                            {
                                if (sendCount >= ConstInfor.SendMsgFail_Count) { CurIndex++; continue;};
                                if (MG3732_SendMSG_PUD.SendMsg(Msg, CommonCollection.PhonePersons[CurIndex].PhoneNumber))
                                {
                                    //每次设置发送完信息后，清理掉缓存中的数据
                                    sendCount=0; CurIndex++; continue;
                                }
                                sendCount++;
                                Thread.Yield();Thread.Sleep(20);
                                MG3732_SendMSG_PUD.ClearBuffer();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("><异常！");
                        }
                        finally{
                            CurWarm.isSendMsg = true;
                        }
                        #endregion
                    }
                }
            }
            catch (Exception)
            {
                string msg = "发送短信出现异常!";
                FileOperation.WriteLog(msg);
            }
            finally
            {
                MG3732_SendMSG_PUD.CloseSerial();
            }
        }

        /// <summary>
        /// 还原显示面板
        /// </summary>
        public static void RestoreShow()
        {
            if (Pst.ShowAreaCB.Items.Count <= 0)return;
            String StrID_Name =  Pst.ShowAreaCB.Text;
            Area ar = GetArea_IDName(StrID_Name);
            if (ar != null)
            {
                string StrAreaID = ar.ID[0].ToString("X2") + ar.ID[1].ToString("X2");
                //画出地图
                DrawAreaMap.DrawMap(Pst.ShowAreaMapPanel,Pst.AreaBitmap,Pst.MapPathTB.Text);
                //画出参考点
                DrawAreaMap.DrawBasicRouter(Pst.AreaBitmap,StrAreaID,0);
                //将图片画到地图上
                Pst.ShowAreaMapPanel.CreateGraphics().DrawImage(Pst.AreaBitmap, 0, 0);
            }else
			{
                DrawAreaMap.DrawMap(Pst.ShowAreaMapPanel, Pst.AreaBitmap, null);
                DrawAreaMap.DrawBasicRouter(Pst.AreaBitmap, null,0);
                Pst.ShowAreaMapPanel.CreateGraphics().DrawImage(Pst.AreaBitmap, 0, 0);
            }
        }
        /// <summary>
        /// 设置设置面板的当前状态
        /// </summary>
        /// <param name="index"></param>
        public static void SetPanelStatus(int index)
        {
            switch (index)
            {
                case 0:
                    Pst.PersonAdminPanel.Visible = true;
                    Pst.GroupPanel.Visible = false;
                    Pst.AreaPanel.Visible = false;
                    Pst.NetPanel.Visible = false;
                    Pst.RouterPanel.Visible = false;
                    Pst.TagPanel.Visible = false;
                    Pst.ShowPanel.Visible = false;
                    Pst.AlarmPanel.Visible = false;
                    Pst.UpdatePersonListView();
                    break;
                case 1:
                    Pst.PersonAdminPanel.Visible = false;
                    Pst.GroupPanel.Visible = true;
                    Pst.AreaPanel.Visible = false;
                    Pst.NetPanel.Visible = false;
                    Pst.RouterPanel.Visible = false;
                    Pst.TagPanel.Visible = false;
                    Pst.ShowPanel.Visible = false;
                    Pst.AlarmPanel.Visible = false;
                    Pst.UpdateGroupListView();
                    break;
                case 2:
                    Pst.PersonAdminPanel.Visible = false;
                    Pst.GroupPanel.Visible = false;
                    Pst.AreaPanel.Visible = true;
                    Pst.NetPanel.Visible = false;
                    Pst.RouterPanel.Visible = false;
                    Pst.TagPanel.Visible = false;
                    Pst.ShowPanel.Visible = false;
                    Pst.AlarmPanel.Visible = false;
                    Pst.AreaIDTX.Text = "";
                    Pst.AreaNameTX.Text = "";
                    Pst.AreaMapPathTX.Text = "";
                    Pst.AreaMapPanel_Paint(null, null);
                    InitAreaPara();
                    Pst.UpdateAraeListView();
                    break;
                case 3:
                    Pst.PersonAdminPanel.Visible = false;
                    Pst.GroupPanel.Visible = false;
                    Pst.AreaPanel.Visible = false;
                    Pst.NetPanel.Visible = true;
                    Pst.RouterPanel.Visible = false;
                    Pst.TagPanel.Visible = false;
                    Pst.ShowPanel.Visible = false;
                    Pst.AlarmPanel.Visible = false;
                    break;
                case 4:
                    Pst.PersonAdminPanel.Visible = false;
                    Pst.GroupPanel.Visible = false;
                    Pst.AreaPanel.Visible = false;
                    Pst.NetPanel.Visible = false;
                    Pst.RouterPanel.Visible = false;
                    Pst.TagPanel.Visible = false;
                    Pst.ShowPanel.Visible = true;
                    Pst.AlarmPanel.Visible = false;
                    Pst.ShowAreaMapPanel.Width = ConstInfor.MapWidth;
                    Pst.ShowAreaMapPanel.Height = ConstInfor.MapHeight;
                    InitShowPara();
                    break;
                case 5:
                    Pst.PersonAdminPanel.Visible = false;
                    Pst.GroupPanel.Visible = false;
                    Pst.AreaPanel.Visible = false;
                    Pst.NetPanel.Visible = false;
                    Pst.RouterPanel.Visible = true;
                    Pst.TagPanel.Visible = false;
                    Pst.ShowPanel.Visible = false;
                    Pst.AlarmPanel.Visible = false;
                    Pst.RouterIDTX.Text = "";
                    Pst.RouterNameTX.Text = "";
                    Pst.RouterAreaCB.SelectedIndex = 0;
                    //将Area中的参考点加载到列表中
                    SysParam.UpdateRouterLv();
                    SysParam.InitRouterPara();
                    break;
                case 6:
                    Pst.PersonAdminPanel.Visible = false;
                    Pst.GroupPanel.Visible = false;
                    Pst.AreaPanel.Visible = false;
                    Pst.NetPanel.Visible = false;
                    Pst.RouterPanel.Visible = false;
                    Pst.ShowPanel.Visible = false;
                    Pst.TagPanel.Visible = true;
                    Pst.AlarmPanel.Visible = false;
                    Pst.TagIDTB.Text = "";
                    Pst.TagNameTB.Text = "";
                    Pst.isStopAlarmCB.Checked = false;

                    Pst.GSensorCB.SelectedIndex = 2;
                    Pst.WorkTimeTypeCB.SelectedIndex = 2;

                    Pst.AddCheckControl(Pst.ReferTreeView);
                    Pst.UpdateTagListView();
                    break;
                case 7:
                    Pst.PersonAdminPanel.Visible = false;
                    Pst.GroupPanel.Visible = false;
                    Pst.AreaPanel.Visible = false;
                    Pst.NetPanel.Visible = false;
                    Pst.RouterPanel.Visible = false;
                    Pst.ShowPanel.Visible = false;
                    Pst.TagPanel.Visible = false;
                    Pst.AlarmPanel.Visible = true;

                    InitAllParams();
                    break;
            }
        }

        public static void InitAllParams()
        {
            if (int.MaxValue == SysParam.SoundTime)
            {
                Pst.SoundTimeTB.SelectedIndex = 1;
            }
            else
            {
                Pst.SoundTimeTB.SelectedIndex = 0;
            }

            SysParam.InitOtherParam();
            string[] PortNames = SerialPort.GetPortNames();
            Pst.ComNamesCB.Items.Clear();
            Pst.ComNamesCB.Items.Add("");

            foreach (string PortName in PortNames)
            {
                Pst.ComNamesCB.Items.Add(PortName);
            }

            if (!SysParam.isMsgSend)
            {
                InitNoCom();
            }
            else
            {
                InitYesCom();
            }
            
        }

        /// <summary>
        /// 不选择串口时
        /// </summary>
        public static void InitNoCom()
        {
            Pst.ComNamesCB.SelectedIndex = 0;
            Pst.MsgPersonHelpCB.Enabled = false; Pst.MsgAreaControlCB.Enabled = false;
            Pst.MsgPersonResCB.Enabled = false; Pst.MsgBatteryLowCB.Enabled = false;
            Pst.MsgDeviceThroubleCB.Enabled = false;
            Pst.MsgPersonHelpCB.Checked = false; Pst.MsgAreaControlCB.Checked = false;
            Pst.MsgPersonResCB.Checked = false; Pst.MsgBatteryLowCB.Checked = false;
            Pst.MsgDeviceThroubleCB.Checked = false;
        }
        //选择串口时
        public static void InitYesCom()
        {
            for (int i = 0; i < Pst.ComNamesCB.Items.Count; i++)
            {
                if (Pst.ComNamesCB.Items[i].ToString().Equals(SysParam.ComName))
                {
                    Pst.ComNamesCB.SelectedIndex = i;
                }
            }
            Pst.MsgPersonHelpCB.Enabled = true; Pst.MsgAreaControlCB.Enabled = true;
            Pst.MsgPersonResCB.Enabled = true; Pst.MsgBatteryLowCB.Enabled = true;
            Pst.MsgDeviceThroubleCB.Enabled = false;
            
            if (SysParam.isSendPersonHelpMsg) Pst.MsgPersonHelpCB.Checked = true;
            if (SysParam.isSendAreaControlMsg) Pst.MsgAreaControlCB.Checked = true;
            if (SysParam.isSendPersonResMsg) Pst.MsgPersonResCB.Checked = true;
            if (SysParam.isSendBatteryLowMsg) Pst.MsgBatteryLowCB.Checked = true;
            if (SysParam.isSendDeviceTrouble) Pst.MsgDeviceThroubleCB.Checked = true;
        }

        /// <summary>
        /// 参考点重新保存时，去刷新区域信息
        /// </summary>
        public static void ReferChangeUpdateArea()
        {
            string StrRouterID = "";
            foreach(KeyValuePair<string,Area> MyArea in CommonCollection.Areas)
            {
                if (null == MyArea.Value.AreaRouter)
                {
                    continue;
                }
                foreach (KeyValuePair<string, BasicRouter> Br in MyArea.Value.AreaRouter)
                {
                    StrRouterID = Br.Key;
                    if (Pst.RouterListView.Items.Count <=0)
                    {
                        Br.Value.Name = "";
                    }
                    ListViewItem item = Pst.RouterListView.FindItemWithText(StrRouterID,false,0);
                    if (null == item)
                    {
                        Br.Value.Name = "";
                        continue;
                    }
                    Area area = GetArea_IDName(item.SubItems[2].Text);
                    if (MyArea.Value.ID[0] == area.ID[0] && MyArea.Value.ID[1] == area.ID[1])
                    {//两个区域相同
                        Br.Value.Name = item.SubItems[1].Text;
                    }
                    else
                    {
                        Br.Value.Name = "";
                    }
                }
            }
        }
        /// <summary>
        /// 導入人員訊息資料
        /// </summary>
        /// <returns></returns>
        public static bool ImportBackUpPerson() 
        {
            if (!Directory.Exists(FileOperation.BackUpDir)){//文件目录不存在时
                MessageBox.Show("對不起,人員資料沒有備份過!");
                return false;   
            }
            if (!File.Exists(FileOperation.BackUpSafePath)){//文件不存在
                MessageBox.Show("對不起,人員資料沒有備份過!");
                return false;
            }
            //文件存在时给出提醒
            if (MessageBox.Show("導入備份文件后，源文件會被替換掉，確定要導入嗎?", "提醒", MessageBoxButtons.OKCancel) == DialogResult.Cancel){
                return false;
            }
            try
            {
                File.Copy(FileOperation.BackUpSafePath, FileOperation.SafePath,true);
            }
            catch {
                return false;
            }
            return true;
        }

        public static bool ImportBackUpGroup() {
            if (!Directory.Exists(FileOperation.BackUpDir))
            {//文件目录不存在时
                MessageBox.Show("對不起,組別資料沒有備份過!");
                return false;
            }
            if (!File.Exists(FileOperation.BackUpGroupPath))
            {//文件不存在
                MessageBox.Show("對不起,組別資料沒有備份過!");
                return false;
            }
            //文件存在时给出提醒
            if (MessageBox.Show("導入備份文件后，源文件會被替換掉，確定要導入嗎?", "提醒", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return false;
            }
            try
            {
                File.Copy(FileOperation.BackUpGroupPath, FileOperation.GroupPath, true);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool ImportBackUpArea()
        {
            if (!Directory.Exists(FileOperation.BackUpDir)){//文件目录不存在时
                MessageBox.Show("對不起,區域資料沒有備份過!");
                return false;
            }
            if (!File.Exists(FileOperation.BackUpAreaPath)){//文件不存在
                MessageBox.Show("對不起,區域資料沒有備份過!");
                return false;
            }
            //文件存在时给出提醒
            if (MessageBox.Show("導入備份文件后，源文件會被替換掉，確定要導入嗎?", "提醒", MessageBoxButtons.OKCancel) == DialogResult.Cancel){
                return false;
            }
            try{
                //先备份图片讯息
                FileOperation.CopyFolderTo(FileOperation.BackUpMapPath, FileOperation.MapPath);
                //在备份区域讯息
                File.Copy(FileOperation.BackUpAreaPath, FileOperation.AreaPath, true);
            }
            catch{
                return false;
            }
            return true;
        }

        public static bool ImportBackUpTag() {
            if (!Directory.Exists(FileOperation.BackUpDir))
            {//文件目录不存在时
                MessageBox.Show("對不起,Tag資料沒有備份過!");
                return false;
            }
            if (!File.Exists(FileOperation.BackUpTagPath))
            {//文件不存在
                MessageBox.Show("對不起,Tag資料沒有備份過!");
                return false;
            }
            //文件存在时给出提醒
            if (MessageBox.Show("導入備份文件后，源文件會被替換掉，確定要導入嗎?", "提醒", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return false;
            }
            try
            {
                SysParam.LoadPersonTagInfo(1);  
                //File.Copy(FileOperation.BackUpTagPath, FileOperation.TagPath, true);
            }
            catch {
                return false;
            }
            return true;
        }

        public static bool SavePerson(int type)
        { 
            //保存人员信息
            if (type == 0)
            {
                if (!FileOperation.Clear(FileOperation.SafePath))
                {
                    return false;
                }
            }
            else { 
                if(!Directory.Exists(FileOperation.BackUpDir))
                {
                    Directory.CreateDirectory(FileOperation.BackUpDir);
                }
                if (!FileOperation.Clear(FileOperation.BackUpSafePath))
                {
                    return false;
                }
            }
            try
            {
                foreach (KeyValuePair<string, Person> person in CommonCollection.Persons)
                {
                    if (type == 0)
                    {
                        FileOperation.SetValue(FileOperation.SafePath, person.Key, FileOperation.PersonName, person.Value.Name);
                        FileOperation.SetValue(FileOperation.SafePath, person.Key, FileOperation.PersonPassWord, Encryption.Encrypti(person.Value.Ps));
                        if (person.Value.PersonAccess == PersonAccess.AdminPerson)
                        {
                            FileOperation.SetValue(FileOperation.SafePath, person.Key, FileOperation.PersonAccess, ConstInfor.StrAdminPerson);
                        }
                        else
                        {
                            FileOperation.SetValue(FileOperation.SafePath, person.Key, FileOperation.PersonAccess, ConstInfor.StrSimplePerson);
                        }
                    }
                    else
                    {
                        FileOperation.SetValue(FileOperation.BackUpSafePath, person.Key, FileOperation.PersonName, person.Value.Name);
                        FileOperation.SetValue(FileOperation.BackUpSafePath, person.Key, FileOperation.PersonPassWord, Encryption.Encrypti(person.Value.Ps));
                        if (person.Value.PersonAccess == PersonAccess.AdminPerson)
                        {
                            FileOperation.SetValue(FileOperation.BackUpSafePath, person.Key, FileOperation.PersonAccess, ConstInfor.StrAdminPerson);
                        }
                        else
                        {
                            FileOperation.SetValue(FileOperation.BackUpSafePath, person.Key, FileOperation.PersonAccess, ConstInfor.StrSimplePerson);
                        }
                    }

                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        ///保存区域集合中的项
        /// </summary>
        public static bool SaveAreaInfo(int type)
        {
            string StrAreaType = "";
            string StrAreaMap = "";
            int index = 0;
            //清除掉文件中的内容
            if (type == 0)
            {
                if (!FileOperation.Clear(FileOperation.AreaPath))
                {
                    return false;
                }
            }
            else
            {
                if(!Directory.Exists(FileOperation.BackUpDir))
                {
                    Directory.CreateDirectory(FileOperation.BackUpDir);
                }
                if (!FileOperation.Clear(FileOperation.BackUpAreaPath))
                {
                    return false;
                }
            }
            try
            {
                foreach (KeyValuePair<string, Area> area in CommonCollection.Areas)
                {
                    if (type == 0)
                    {
                        FileOperation.SetValue(FileOperation.AreaPath, area.Key, FileOperation.AreaName, area.Value.Name);
                    }
                    else {
                        FileOperation.SetValue(FileOperation.BackUpAreaPath, area.Key, FileOperation.AreaName, area.Value.Name);
                    }
                    switch (area.Value.AreaType)
                    {
                        case AreaType.ControlArea:
                            StrAreaType = ConstInfor.StrControlArea;
                            break;
                        case AreaType.DangerArea:
                            StrAreaType = ConstInfor.StrDangerArea;
                            break;
                        case AreaType.SimpleArea:
                            StrAreaType = ConstInfor.StrSimpleArea;
                            break;
                    }
                    if (type == 0)
                    {
                        FileOperation.SetValue(FileOperation.AreaPath, area.Key, FileOperation.AreaType, StrAreaType);
                    }
                    else
                    {
                        FileOperation.SetValue(FileOperation.BackUpAreaPath, area.Key, FileOperation.AreaType, StrAreaType);
                    }
                    if (null != area.Value.AreaBitMap)
                    {
                        StrAreaMap = area.Value.AreaBitMap.MapPath;
                    }
                    else
                    {
                        StrAreaMap = "";
                    }
                    if (type == 0)
                    {
                        FileOperation.SetValue(FileOperation.AreaPath, area.Key, FileOperation.AreaMapPath, StrAreaMap);
                    }
                    else
                    {
                        FileOperation.SetValue(FileOperation.BackUpAreaPath, area.Key, FileOperation.AreaMapPath, StrAreaMap);
                    }
                    //保存组的ID
                    string StrAreaGroupID = "";
                    if (null != area.Value.GroupID)
                    {
                        StrAreaGroupID = area.Value.GroupID[0].ToString("X2") + area.Value.GroupID[1].ToString("X2");
                    }
                    if (type == 0)
                    {
                        FileOperation.SetValue(FileOperation.AreaPath, area.Key, FileOperation.AreaGroupID, StrAreaGroupID);
                    }
                    else {
                        FileOperation.SetValue(FileOperation.BackUpAreaPath, area.Key, FileOperation.AreaGroupID, StrAreaGroupID);
                    }
                    
                    index = 0;
                    if (null != area.Value.AreaRouter)
                    {
                        foreach (KeyValuePair<string, BasicRouter> Br in area.Value.AreaRouter)
                        {
                            if (type == 0)
                            {
                                FileOperation.SetValue(FileOperation.AreaPath, area.Key, FileOperation.AreaRouter + "_" + index.ToString(), Br.Key);
                                FileOperation.SetValue(FileOperation.AreaPath, area.Key, FileOperation.AreaRouterName + "_" + index.ToString(), Br.Value.Name);
                                FileOperation.SetValue(FileOperation.AreaPath, area.Key, FileOperation.AreaRouterVisible + "_" + index.ToString(), Br.Value.isVisible.ToString());
                                FileOperation.SetValue(FileOperation.AreaPath, area.Key, FileOperation.AreaRouterX + "_" + index.ToString(), Br.Value.x.ToString());
                                FileOperation.SetValue(FileOperation.AreaPath, area.Key, FileOperation.AreaRouterY + "_" + index.ToString(), Br.Value.y.ToString());
                            }
                            else
                            {
                                FileOperation.SetValue(FileOperation.BackUpAreaPath, area.Key, FileOperation.AreaRouter + "_" + index.ToString(), Br.Key);
                                FileOperation.SetValue(FileOperation.BackUpAreaPath, area.Key, FileOperation.AreaRouterName + "_" + index.ToString(), Br.Value.Name);
                                FileOperation.SetValue(FileOperation.BackUpAreaPath, area.Key, FileOperation.AreaRouterVisible + "_" + index.ToString(), Br.Value.isVisible.ToString());
                                FileOperation.SetValue(FileOperation.BackUpAreaPath, area.Key, FileOperation.AreaRouterX + "_" + index.ToString(), Br.Value.x.ToString());
                                FileOperation.SetValue(FileOperation.BackUpAreaPath, area.Key, FileOperation.AreaRouterY + "_" + index.ToString(), Br.Value.y.ToString());
                            }

                            index++;
                        }
                    }

                    index = 0;
                    if (null != area.Value.AreaNode)
                    {
                        foreach (KeyValuePair<string, BasicNode> Bn in area.Value.AreaNode)
                        {
                            if (type == 0)
                            {
                                FileOperation.SetValue(FileOperation.AreaPath, area.Key, FileOperation.AreaNode + "_" + index.ToString(), Bn.Key);
                                FileOperation.SetValue(FileOperation.AreaPath, area.Key, FileOperation.AreaNodeName + "_" + index.ToString(), Bn.Value.Name);
                                FileOperation.SetValue(FileOperation.AreaPath, area.Key, FileOperation.AreaNodeVisible + "_" + index.ToString(), Bn.Value.isVisible.ToString());
                                FileOperation.SetValue(FileOperation.AreaPath, area.Key, FileOperation.AreaNodeX + "_" + index.ToString(), Bn.Value.x.ToString());
                                FileOperation.SetValue(FileOperation.AreaPath, area.Key, FileOperation.AreaNodeY + "_" + index.ToString(), Bn.Value.y.ToString());
                            }
                            else {
                                FileOperation.SetValue(FileOperation.BackUpAreaPath, area.Key, FileOperation.AreaNode + "_" + index.ToString(), Bn.Key);
                                FileOperation.SetValue(FileOperation.BackUpAreaPath, area.Key, FileOperation.AreaNodeName + "_" + index.ToString(), Bn.Value.Name);
                                FileOperation.SetValue(FileOperation.BackUpAreaPath, area.Key, FileOperation.AreaNodeVisible + "_" + index.ToString(), Bn.Value.isVisible.ToString());
                                FileOperation.SetValue(FileOperation.BackUpAreaPath, area.Key, FileOperation.AreaNodeX + "_" + index.ToString(), Bn.Value.x.ToString());
                                FileOperation.SetValue(FileOperation.BackUpAreaPath, area.Key, FileOperation.AreaNodeY + "_" + index.ToString(), Bn.Value.y.ToString());
                            }
                            index++;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 保存所有的訊息
        /// </summary>
        public static void SaveAllInfor()
        {
            StringBuilder strbuilder = new StringBuilder();
            int tick;
            float interval;
            tick = Environment.TickCount;
            if (SysParam.SavePerson(0))
            {
                
                FileOperation.WriteLog(DateTime.Now.ToString() + "=>人員訊息保存成功!");
                interval = (float)(Environment.TickCount - tick) / 1000;
                strbuilder.Append("人員訊息保存成功...(" + String.Format("{0:N2}", interval) + " s)\r\n");
            }
            else
            {
                FileOperation.WriteLog(DateTime.Now.ToString() + "=>人員訊息保存失敗!");
                strbuilder.Append("人員訊息保存失敗..." + "\r\n");
            }
            tick = Environment.TickCount;
            if (SysParam.SaveAreaInfo(0))
            {
                FileOperation.WriteLog(DateTime.Now.ToString() + "=>區域訊息保存成功!");
                interval = (float)(Environment.TickCount - tick) / 1000;
                strbuilder.Append("區域訊息保存成功...(" + String.Format("{0:N2}", interval) + " s)\r\n");
            }
            else
            {
                FileOperation.WriteLog(DateTime.Now.ToString() + "=>區域訊息保存失敗!");
                strbuilder.Append("區域訊息保存失敗..." + "\r\n");
            }
            tick = Environment.TickCount;
           
            if (SysParam.SaveTag(0))
            //if ( FileModel.fileInit().setTagInforData())
            {
                FileOperation.WriteLog(DateTime.Now.ToString() + "=>卡片訊息保存成功!");
                interval = (float)(Environment.TickCount - tick) / 1000;
                strbuilder.Append("卡片訊息保存成功...(" + String.Format("{0:N2}", interval) + " s)\r\n");
            }
            else
            {
                FileOperation.WriteLog(DateTime.Now.ToString() + "=>卡片訊息保存失敗!");
                strbuilder.Append("卡片訊息保存失敗..." + "\r\n");
            }
            tick = Environment.TickCount;
            if (SysParam.SaveNet())
            {

                FileOperation.WriteLog(DateTime.Now.ToString() + "=>網絡訊息保存成功!");
                interval = (float)(Environment.TickCount - tick) / 1000;
                strbuilder.Append("網絡訊息保存成功...(" + String.Format("{0:N2}", interval) + " s)\r\n");
            }
            else
            {
                FileOperation.WriteLog(DateTime.Now.ToString() + "=>網絡訊息保存失敗!");
                strbuilder.Append("網絡訊息保存失敗..." + "\r\n");
            }
            tick = Environment.TickCount;
            if (SysParam.SaveGroup(0))
            {
                FileOperation.WriteLog(DateTime.Now.ToString() + "=>組別訊息保存成功!");
                interval = (float)(Environment.TickCount - tick) / 1000;
                strbuilder.Append("組別訊息保存成功...(" + String.Format("{0:N2}", interval) + " s)\r\n");
            }
            else
            {
                FileOperation.WriteLog(DateTime.Now.ToString() + "=>組別訊息保存失敗!");
                strbuilder.Append("組別訊息保存失敗..." + "\r\n");
            }
            //保存添加的人员信息
            tick = Environment.TickCount;
            if (SysParam.SavePersonMsg())
            {
                FileOperation.WriteLog(DateTime.Now.ToString() + "=>人員資料訊息保存成功!");
                interval = (float)(Environment.TickCount - tick) / 1000;
                strbuilder.Append("人員資料訊息保存成功...(" + String.Format("{0:N2}", interval) + " s)\r\n");
            }
            else
            {
                FileOperation.WriteLog(DateTime.Now.ToString() + "=>人員資料訊息保存失敗!");
                strbuilder.Append("人員資料訊息保存失敗..." + "\r\n");
            }
            SysParam.SaveAlarmParam();
            MessageBox.Show(strbuilder.ToString(), "保存訊息提醒", MessageBoxButtons.OK);
        }

        /// <summary>
        /// 保存卡片信息
        /// </summary>
        public static bool SaveTag(int type)
        {
            if (type == 0)
                return FileModel.fileInit().setTagInforData();
            else if (type == 1)
                return FileModel.fileInit().saveBackUpTagInforData();
            /*if (type == 0)
            {
                if (!FileOperation.Clear(FileOperation.TagPath)) {
                    return false;
                }
            }*/
            else if (type == 3)
            {
                if (!Directory.Exists(FileOperation.BackUpDir))
                {
                    Directory.CreateDirectory(FileOperation.BackUpDir);
                }
                if (!FileOperation.Clear(FileOperation.BackUpTagPath))
                {
                    return false;
                }
            }
            else return false;

            foreach (KeyValuePair<string, Tag> tag in CommonCollection.Tags) {
                try
                {
                    String filePath = "";
                    /*if (type == 0)
                    {
                        filePath = FileOperation.TagPath;
                        //FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagName, tag.Value.Name);
                    }*/
                    if (type == 3)
                    {
                        filePath = FileOperation.BackUpTagPath;
                    }
                    FileOperation.SetValue(filePath, tag.Key, FileOperation.TagName, tag.Value.Name);
                    FileOperation.SetValue(filePath, tag.Key, FileOperation.TagWorkTime, ConstInfor.StrLimitWork);
                    UInt32 tWorkSTamp = GetTimeStamp(tag.Value.StartWorkDT, true);
                    UInt32 tWorkETamp = GetTimeStamp(tag.Value.EndWorkDT, true);
                    FileOperation.SetValue(filePath, tag.Key, FileOperation.TagWorkStartTimeStamp, tWorkSTamp.ToString());
                    FileOperation.SetValue(filePath, tag.Key, FileOperation.TagWorkEndTimeStamp, tWorkETamp.ToString());
                    String tagStr = DrawIMG.getBUffIdStr(tag.Value.workID);
                    FileOperation.SetValue(filePath, tag.Key, FileOperation.TagWorkID, tagStr);
                    //FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagGSStartTimeH, tag.Value.StartGSDT.Hour + "");
                    //FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagGSStartTimeM, tag.Value.StartGSDT.Minute + "");
                    //FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagGSEndTimeH, tag.Value.EndGSDT.Hour + "");
                    //FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagGSEndTimeM, tag.Value.EndGSDT.Minute + "");

                    switch (tag.Value.CurGSWorkTime)
                    {
                        case WorkTime.AlwaysWork:
                            FileOperation.SetValue(filePath, tag.Key, FileOperation.TagGSWorkTime, ConstInfor.StrAlwayWork);
                            /*if (type == 0)
                            {
                                FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagGSWorkTime, ConstInfor.StrAlwayWork);
                            }
                            else {
                                FileOperation.SetValue(FileOperation.BackUpTagPath, tag.Key, FileOperation.TagGSWorkTime, ConstInfor.StrAlwayWork);
                            }*/
                            break;
                        case WorkTime.LimitTime:

                            FileOperation.SetValue(filePath, tag.Key, FileOperation.TagGSWorkTime, ConstInfor.StrLimitWork);
                            FileOperation.SetValue(filePath, tag.Key, FileOperation.TagGSStartTimeH, tag.Value.StartGSDT.Hour + "");
                            FileOperation.SetValue(filePath, tag.Key, FileOperation.TagGSStartTimeM, tag.Value.StartGSDT.Minute + "");
                            FileOperation.SetValue(filePath, tag.Key, FileOperation.TagGSEndTimeH, tag.Value.EndGSDT.Hour + "");
                            FileOperation.SetValue(filePath, tag.Key, FileOperation.TagGSEndTimeM, tag.Value.EndGSDT.Minute + "");

                            /*if (type == 0)
                            {
                                FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagGSWorkTime, ConstInfor.StrLimitWork);
                                FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagGSStartTimeH, tag.Value.StartGSDT.Hour + "");
                                FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagGSStartTimeM, tag.Value.StartGSDT.Minute+"");
                                FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagGSEndTimeH, tag.Value.EndGSDT.Hour + "");
                                FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagGSEndTimeM, tag.Value.EndGSDT.Minute + "");
                            }
                            else {
                                FileOperation.SetValue(FileOperation.BackUpTagPath, tag.Key, FileOperation.TagGSWorkTime, ConstInfor.StrLimitWork);
                                FileOperation.SetValue(FileOperation.BackUpTagPath,tag.Key,FileOperation.TagGSStartTimeH,tag.Value.StartGSDT.Hour+"");
                                FileOperation.SetValue(FileOperation.BackUpTagPath,tag.Key, FileOperation.TagGSStartTimeM, tag.Value.StartGSDT.Minute + "");
                                FileOperation.SetValue(FileOperation.BackUpTagPath, tag.Key, FileOperation.TagGSEndTimeH, tag.Value.EndGSDT.Hour+"");
                                FileOperation.SetValue(FileOperation.BackUpTagPath, tag.Key, FileOperation.TagGSEndTimeM,tag.Value.EndGSDT.Minute+"");
                            }*/
                            break;
                        case WorkTime.NoWork:
                            FileOperation.SetValue(filePath, tag.Key, FileOperation.TagGSWorkTime, ConstInfor.StrNotWork);
                            /*if (type == 0)
                            {
                                FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagGSWorkTime, ConstInfor.StrNotWork);
                            }
                            else {
                                FileOperation.SetValue(FileOperation.BackUpTagPath, tag.Key, FileOperation.TagGSWorkTime, ConstInfor.StrNotWork);
                            }*/
                            break;
                        case WorkTime.UnKnown:
                            FileOperation.SetValue(filePath, tag.Key, FileOperation.TagGSWorkTime, ConstInfor.StrUnKnown);
                            /*if (type == 0)
                            {
                                FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagGSWorkTime, ConstInfor.StrUnKnown);
                            }
                            else {
                                FileOperation.SetValue(FileOperation.BackUpTagPath, tag.Key, FileOperation.TagGSWorkTime, ConstInfor.StrUnKnown);
                            }*/
                            break;
                    }
                    /*switch (tag.Value.CurTagWorkTime)
                    {
                        case WorkTime.AlwaysWork:
                            if (type == 0)
                            {
                                FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagWorkTime, ConstInfor.StrAlwayWork);
                            }
                            else {
                                FileOperation.SetValue(FileOperation.BackUpTagPath, tag.Key, FileOperation.TagWorkTime, ConstInfor.StrAlwayWork);
                            }
                            
                            break;
                        case WorkTime.LimitTime:
                            if (type == 0)
                            {
                                FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagWorkTime, ConstInfor.StrLimitWork);
                                FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagStartTimeH, tag.Value.StartWorkDT.Hour + "");
                                FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagStartTimeM, tag.Value.StartWorkDT.Minute + "");
                                FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagEndTimeH, tag.Value.EndWorkDT.Hour + "");
                                FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagEndTimeM, tag.Value.EndWorkDT.Minute + "");
                            }
                            else {
                                FileOperation.SetValue(FileOperation.BackUpTagPath, tag.Key, FileOperation.TagWorkTime, ConstInfor.StrLimitWork);
                                FileOperation.SetValue(FileOperation.BackUpTagPath, tag.Key, FileOperation.TagStartTimeH, tag.Value.StartWorkDT.Hour + "");
                                FileOperation.SetValue(FileOperation.BackUpTagPath, tag.Key, FileOperation.TagStartTimeM, tag.Value.StartWorkDT.Minute + "");
                                FileOperation.SetValue(FileOperation.BackUpTagPath, tag.Key, FileOperation.TagEndTimeH, tag.Value.EndWorkDT.Hour + "");
                                FileOperation.SetValue(FileOperation.BackUpTagPath, tag.Key, FileOperation.TagEndTimeM, tag.Value.EndWorkDT.Minute + "");
                            }

                            break;
                        case WorkTime.NoWork:
                            if (type == 0)
                            {
                                FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagWorkTime, ConstInfor.StrNotWork);
                            }
                            else {
                                FileOperation.SetValue(FileOperation.BackUpTagPath, tag.Key, FileOperation.TagWorkTime, ConstInfor.StrNotWork);
                            }
                           
                            break;
                        case WorkTime.UnKnown:
                            if (type == 0)
                            {
                                FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagWorkTime, ConstInfor.StrUnKnown);
                            }
                            else {
                                FileOperation.SetValue(FileOperation.BackUpTagPath, tag.Key, FileOperation.TagWorkTime, ConstInfor.StrUnKnown);
                            }
                            break;
                    }*/
                    if (tag.Value.IsStopAlarm)
                    {
                        FileOperation.SetValue(filePath, tag.Key, FileOperation.TagIsStopTime, "True");
                        FileOperation.SetValue(filePath, tag.Key, FileOperation.TagStopTime, tag.Value.StopTime + "");
                        /*if (type == 0)
                        {
                            FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagIsStopTime, "True");
                            FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagStopTime, tag.Value.StopTime + "");
                        }
                        else {
                            FileOperation.SetValue(FileOperation.BackUpTagPath, tag.Key, FileOperation.TagIsStopTime, "True");
                            FileOperation.SetValue(FileOperation.BackUpTagPath, tag.Key, FileOperation.TagStopTime, tag.Value.StopTime + "");
                        }*/
                    }
                    else
                    {
                        FileOperation.SetValue(filePath, tag.Key, FileOperation.TagIsStopTime, "False");
                        FileOperation.SetValue(filePath, tag.Key, FileOperation.TagStopTime, tag.Value.StopTime + "");
                        /*if (type == 0)
                        {
                            FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagIsStopTime, "False");
                            FileOperation.SetValue(FileOperation.TagPath, tag.Key, FileOperation.TagStopTime, tag.Value.StopTime + "");
                        }
                        else {
                            FileOperation.SetValue(FileOperation.BackUpTagPath, tag.Key, FileOperation.TagIsStopTime, "False");
                            FileOperation.SetValue(FileOperation.BackUpTagPath, tag.Key, FileOperation.TagStopTime, tag.Value.StopTime + "");
                        }*/
                    }
                    for (int i = 0; i < tag.Value.TagReliableList.Count; i++)
                    {
                        FileOperation.SetValue(filePath, tag.Key, "RD_" + i, tag.Value.TagReliableList.ElementAt(i));
                        /*if (type == 0)
                        {
                            FileOperation.SetValue(FileOperation.TagPath, tag.Key, "RD_" + i, tag.Value.TagReliableList.ElementAt(i));
                        }
                        else {
                            FileOperation.SetValue(FileOperation.BackUpTagPath, tag.Key, "RD_" + i, tag.Value.TagReliableList.ElementAt(i));
                        }*/
                    }
                }
                catch (Exception ex)
                {
                    FileOperation.WriteLog("保存Tag相关参数出现异常，异常原因:" + ex.ToString());
                    return false;
                }
            }
            return true;
        }

        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.</param>  
        /// <returns></returns>  
        public static UInt32 GetTimeStamp(DateTime UtcNow, bool bflag)
        {
            TimeSpan ts = UtcNow - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            UInt32 time = 0;
            if (bflag)
                time = Convert.ToUInt32(ts.TotalSeconds);
            else
                time = (UInt32)Convert.ToInt64(ts.TotalMilliseconds);
            return time;
        }


        /// <summary>
        /// 保存网络讯息
        /// </summary>
        public static bool SaveNet()
        {
            if (null == NetParam.Ip)
                return false;
            if (FileOperation.Clear(FileOperation.ConfigPath))
            {
                FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.NetSeg, FileOperation.NetKey_IP, NetParam.Ip.ToString());
                FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.NetSeg, FileOperation.NetKey_Port, NetParam.Port.ToString());
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 保存发送短信的人员信息
        /// </summary>
        public static bool SavePersonMsg()
        {
            if (FileOperation.Clear(FileOperation.PersonMsgPath))
            {
                try
                {
                    string StrID = "";
                    foreach (PhonePerson psn in CommonCollection.PhonePersons)
                    {
                        StrID = psn.ID.ToString().PadLeft(2, '0');
                        FileOperation.SetValue(FileOperation.PersonMsgPath, StrID, FileOperation.PersonName, psn.Name);
                        FileOperation.SetValue(FileOperation.PersonMsgPath, StrID, FileOperation.PersonPhone, psn.PhoneNumber);
                    }
                }catch{
                    return false;
                }
                return true;
            }
            else {
                return false;
            }
        }

       
        /// <summary>
        /// 保存人员权限信息,分管理人员和使用人员
        /// </summary>
        public static void SavePersonnelAccess()
        {
            if (FileOperation.Clear(FileOperation.SafePath))
            {
                foreach(KeyValuePair<string,Person> person in CommonCollection.Persons)
                {
                    FileOperation.SetValue(FileOperation.SafePath, person.Key, FileOperation.PersonName,person.Value.Name);
                    FileOperation.SetValue(FileOperation.SafePath, person.Key, FileOperation.PersonPassWord, person.Value.Ps);
                    if (person.Value.PersonAccess == PersonAccess.AdminPerson) FileOperation.SetValue(FileOperation.SafePath, person.Key, FileOperation.PersonAccess, ConstInfor.StrAdminPerson);
                    else FileOperation.SetValue(FileOperation.SafePath, person.Key, FileOperation.PersonAccess, ConstInfor.StrSimplePerson);
                }
            }
        }
        /// <summary>
        /// 保存组信息
        /// </summary>
        public static bool SaveGroup(int type)
        {
            if (type == 0){//保存組別訊息
                if (!FileOperation.Clear(FileOperation.GroupPath))
                {
                    return false;
                }
            }else {
                if (!Directory.Exists(FileOperation.BackUpDir))
                {
                    Directory.CreateDirectory(FileOperation.BackUpDir);
                }
                if (!FileOperation.Clear(FileOperation.BackUpGroupPath))
                {
                    return false;
                }
            }
            try
            {
                foreach (KeyValuePair<string, Group> group in CommonCollection.Groups)
                {
                    if (type == 0){
                        FileOperation.SetValue(FileOperation.GroupPath, group.Key, FileOperation.GroupName, group.Value.Name);
                    }
                    else {
                        FileOperation.SetValue(FileOperation.BackUpGroupPath, group.Key, FileOperation.GroupName, group.Value.Name);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 保存设置警告的相关参数
        /// </summary>
        public static void SaveAlarmParam()
        {
            //在加载网络信息时已经清楚了相关信息
            if(isPersonHelp)FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.AlarmPersonHelp,"True");
            else FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.AlarmPersonHelp,"False");
            if(isEmergy)FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.AlarmEme,"True");
            else FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.AlarmEme,"False");
            if(isAreaControl)FileOperation.SetValue(FileOperation.ConfigPath,FileOperation.AlarmSeg,FileOperation.AlarmAreaControl,"True");
            else FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.AlarmAreaControl,"False");
            if (isLowBattery)
            {FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.AlarmLowBattery, "True");
             FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.AlarmLowBatteryValue, LowBattery.ToString());
            }else FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.AlarmLowBattery, "False");
            /*保存警告的声音*/
            if (IsSoundAlarm)
            {
                FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.isSound, "True");
                if (isSoundPersonHelp){
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.MsgPersonHelp, "True");
                }
                else {
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.MsgPersonHelp, "False");
                }

                if (isSoundPersonRes){
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.MsgPersonRes, "True");
                }
                else {
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.MsgPersonRes, "False");
                }

                if (isSoundAreaControl)
                {
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.MsgAreaAdmin, "True");
                }
                else {
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.MsgAreaAdmin, "False");
                }

                if (isSoundBatteryLow)
                {
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.MsgTagBatteryLow, "True");
                }
                else {
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.MsgTagBatteryLow, "False");
                }

                if (isSoundDeviceTrouble)
                {
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.MsgDeviceDis, "True");
                }
                else {
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.MsgDeviceDis, "False");
                }
            }
            else
            {
                FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.isSound, "False");
            }
            FileOperation.SetValue(FileOperation.ConfigPath,FileOperation.AlarmSeg,FileOperation.SoundName,SysParam.SoundName);
            FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.DeviceExceDis, FileOperation.SysScanTimeTab, Measure_Interval.ToString());
            FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.DeviceExceDis, FileOperation.TagDisTimeParam1, TagDisParam1.ToString());
            FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.DeviceExceDis, FileOperation.TagDisTimeParam2, TagDisParam2.ToString());
            FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.DeviceExceDis, FileOperation.ReferDisTimeParam1, RouterParam1.ToString());
            FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.DeviceExceDis, FileOperation.ReferDisTimeParam2, RouterParam2.ToString());
            //保存清理参数
            if (isClearData)
            {
                FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.ClearSeg, FileOperation.IsClearData, "True");
                FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.ClearSeg, FileOperation.ClearTime, ClearTime.ToString());
            }
            else
            {
                FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.ClearSeg, FileOperation.IsClearData, "False");
            }

            if (isOnTimeClearHandlerWarm)
            {
                FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.ClearSeg, FileOperation.IsClearWarm, "True");
                FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.ClearSeg, FileOperation.ClearWarmTime, OnTimes.ToString());
            }
            else
            {
                FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.ClearSeg, FileOperation.IsClearWarm, "False");
            }
            //是否清理已经被处理的警告讯息
            if(isClearHandleAlarm)
            {
                FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.ClearSeg, FileOperation.AutoClearHandle, "True");
                FileOperation.SetValue(FileOperation.ConfigPath,FileOperation.ClearSeg,FileOperation.AutoClearHandleTime,AutoClearHandleAlarmTime+"");
            }
            else
            {
                FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.ClearSeg, FileOperation.AutoClearHandle, "False");
            }
            if (isMsgSend)
            {
                FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.ComSeg, FileOperation.ComName, ComName);

                if (isSendPersonHelpMsg)
                {
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.ComSeg, FileOperation.MsgPersonHelp, "True");
                }
                else
                {
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.ComSeg, FileOperation.MsgPersonHelp, "False");
                }

                if (isSendAreaControlMsg)
                {
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.ComSeg, FileOperation.MsgAreaAdmin, "True");
                }
                else
                {
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.ComSeg, FileOperation.MsgAreaAdmin, "False");
                }

                if (isSendPersonResMsg)
                {
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.ComSeg, FileOperation.MsgPersonRes, "True");
                }
                else
                {
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.ComSeg, FileOperation.MsgPersonRes, "False");
                }

                if (isSendBatteryLowMsg)
                {
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.ComSeg, FileOperation.MsgTagBatteryLow, "True");
                }
                else
                {
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.ComSeg, FileOperation.MsgTagBatteryLow, "False");
                }

                if (isSendDeviceTrouble)
                {
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.ComSeg, FileOperation.MsgDeviceDis, "True");
                }
                else
                {
                    FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.ComSeg, FileOperation.MsgDeviceDis, "False");
                }
            }
            FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.SoundTime, SysParam.SoundTime + "");
            //保存优化方式
            if (curOptimalMedol == OptimizationModel.HopTimes)
            {
                FileOperation.SetValue(FileOperation.ConfigPath,FileOperation.OptimizedParam,FileOperation.OptimizedMedol,"1");
                //保存优化的跳点次数
                FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.OptimizedParam, FileOperation.OptimizedValue, PopTimes+"");
            }
            else
            {
                FileOperation.SetValue(FileOperation.ConfigPath,FileOperation.OptimizedParam,FileOperation.OptimizedMedol,"0");
                //保存优化的信号强度差的阀值
                FileOperation.SetValue(FileOperation.ConfigPath, FileOperation.OptimizedParam, FileOperation.OptimizedValue, RssiThreshold+"");
            }

            if (SysParam.isDebug)
            {
                FileOperation.SetValue(FileOperation.OtherPath, FileOperation.Debug, FileOperation.DebugKey, "True");
                if (SysParam.isSettingArroundDevices)
                {
                    FileOperation.SetValue(FileOperation.OtherPath, FileOperation.Debug, FileOperation.SettingArraoundDevices, "True");
                }
                else
                {
                    FileOperation.SetValue(FileOperation.OtherPath, FileOperation.Debug, FileOperation.SettingArraoundDevices, "False");
                }
            }
            else
            {
                FileOperation.SetValue(FileOperation.OtherPath, FileOperation.Debug, FileOperation.DebugKey, "False");
            }

            //保存自动清理已经连接上的设备
            if (SysParam.isDevCnnLoss)
            {
                FileOperation.SetValue(FileOperation.OtherPath, FileOperation.Debug, FileOperation.AutoClearDevCnnAlarm, "True");
            }
            else
            {
                FileOperation.SetValue(FileOperation.OtherPath, FileOperation.Debug, FileOperation.AutoClearDevCnnAlarm, "False");
            }
        }
       

        public static void SaveAndroidendport()
        {
            if (CommonCollection.Androidendport == null) return;
            FileMode fMo = FileMode.Create;
            FileOperation.addMsgInFile("data:"+CommonCollection.Androidendport.ToString(), "adnroid.txt", fMo);
        }

        public static void getAndroidendport()
        {
            byte[] dt = FileOperation.getDataFromFile("adnroid.txt");
            //System.Text.Encoding.ASCII.GetBytes(datas)
            if (dt == null) return;
            String[] ipPort = Encoding.ASCII.GetString(dt,0,dt.Length).Split(':');            
            try {
                String ip = ipPort[1];
                String port = ipPort[2];
                int servicePort = 4567;
                servicePort = int.Parse(port);
                IPEndPoint Androidendport = new IPEndPoint(IPAddress.Parse(ip), servicePort);
                CommonCollection.Androidendport = Androidendport;
            }
            catch {
                return;
            }           
        }

        /// <summary>
        /// 将原来的数据包原始资料和现在的数据包资料合并,并进行序列化
        /// </summary>
        public static void SaveRecord()
        {
            //应用打开什么时候，就以这个时间为基准
            TagPack StartTP = null;
            if (CommonCollection.RecordDB.Count > 0) StartTP = CommonCollection.RecordDB[0];
           if (null == StartTP) return;
            DateTime StartDT = StartTP.ReportTime;//开始时间
            string StrLogName = FileOperation.Original + "\\" + StartDT.Year.ToString().PadLeft(2,'0') + StartDT.Month.ToString().PadLeft(2,'0') + StartDT.Day.ToString().PadLeft(2,'0') + ".dat";
            try
            {
                if (File.Exists(StrLogName))CommonCollection.LogTags = FileOperation.GetOriginalData(StrLogName);
                else FileOperation.CreateDateTimeDir(StrLogName);
                foreach (TagPack tpk in CommonCollection.RecordDB)
                    CommonCollection.LogTags.Add(tpk);
            }
            catch (Exception)
            {
            }
            finally {
                FileOperation.SaveOriginalData(CommonCollection.LogTags, StrLogName);
            }
        }


        /// <summary>
        /// 保存操作记录
        /// </summary>
        public static void SaveOperationRecord()
        {
            FileOperation.SetValue(FileOperation.OtherPath, FileOperation.StrPersonOperSeg, FileOperation.StrOpertimeKey, cleardaysnum + "");
            //判断操作记录文件夹是否存在
            if(!Directory.Exists(FileOperation.OperRecordPath))
            {
                Directory.CreateDirectory(FileOperation.OperRecordPath);
            }
            string strdt = "";
            foreach (PersonOperation peroper in CommonCollection.personOpers)
            {
                if (null == peroper)
                    continue;
                strdt = peroper.operdt.Year.ToString().PadLeft(4, '0') + peroper.operdt.Month.ToString().PadLeft(2, '0') + peroper.operdt.Day.ToString().PadLeft(2, '0')+".dat";
                if (File.Exists(FileOperation.OperRecordPath + "\\" + strdt))
                {//存在将他序列化出来再将其加上去
                    Object obj = null;
                    FileOperation.DeserializeObject(out obj, FileOperation.OperRecordPath + "\\" + strdt);
                    List<PersonOperation> curperopers = obj as List<PersonOperation>;
                    if (null == curperopers)
                        continue;
                    curperopers.Add(peroper);
                    FileOperation.SeralizeObject(curperopers, FileOperation.OperRecordPath + "\\" + strdt);
                }
                else
                {
                    List<PersonOperation> curperopers = new List<PersonOperation>();
                    curperopers.Add(peroper);
                    FileOperation.SeralizeObject(curperopers, FileOperation.OperRecordPath + "\\" + strdt);
                }
            }
        }
        /// <summary>
        /// 清除人员操作记录
        /// </summary>
        public static void ClearPersonOperRecords()
        {
            string strdt;
            int start, end , year, month, day;
            DateTime mdtd;
            List<string> strdelefiles = new List<string>();
            if (!Directory.Exists(FileOperation.OperRecordPath))
            {
                return;
            }
            string[] strfiles =  Directory.GetFiles(FileOperation.OperRecordPath);
            if (null == strfiles)
            {
                return;
            }
            foreach (string str in strfiles)
            {
                start = str.LastIndexOf("\\");
                end = str.LastIndexOf(".dat");
                if (end - start - 1 != 8)
                {
                    continue;
                }
                strdt = str.Substring(start + 1, end - start - 1);
                try 
                {
                    year = Convert.ToInt32(strdt.Substring(0,4));
                    month = Convert.ToInt32(strdt.Substring(4,2));
                    day = Convert.ToInt32(strdt.Substring(6,2));
                    mdtd = new DateTime(year,month,day,DateTime.Now.Hour,DateTime.Now.Minute,DateTime.Now.Second);
                }catch(Exception)
                {
                    continue;
                }
                double days = (DateTime.Now - mdtd).TotalDays;
                if(days> SysParam.cleardaysnum)
                {
                    strdelefiles.Add(str);
                }
            }
            foreach (string str in strdelefiles)
            {
                File.Delete(str);
            }
        }
        //清除警告资讯
        public static void ClearWarmData()
        {
            string StrWarmDir = FileOperation.WarmMsg;
            string StrWarmPath = StrWarmDir + "\\" + FileOperation.WarmName;
            List<WarmInfo> ClearWms = null;
            try
            {
                if (Directory.Exists(StrWarmDir) && File.Exists(StrWarmPath))
                {
                    ClearWms = FileOperation.GetWarmData(StrWarmPath);
                }
            }
            catch (Exception)
            {
            }
            if (null == ClearWms) return;
            ArrayList RemoveData = new ArrayList();
            TimeSpan TS;
            int DayNum;
            DateTime CurDT = DateTime.Now;
            foreach (WarmInfo wm in ClearWms)
            {
                if (null == wm) continue;
                TS = CurDT - wm.AlarmTime;
                DayNum = Convert.ToInt32(TS.TotalDays);
                if (DayNum >= SysParam.OnTimes)
                {
                    RemoveData.Add(wm);
                }
            }
            foreach (object obj in RemoveData)
            {
                ClearWms.Remove((WarmInfo)obj);
            }
            //将这个集合中的数据替换原来的数据
            FileOperation.SaveWarmData(ClearWms, StrWarmPath);
            RemoveData.Clear();
        }
        /// <summary>
        /// 清理日誌文件
        /// 注：日誌文件每隔day天清理一次
        /// </summary>
        public static void ClearLog(int day)
        { 
            if(!Directory.Exists(FileOperation.LogPath))
            {
                return;
            }
            string stryear = "", strmonth = "", strday = "";
            int year = 0, month = 0, dy = 0;
            //生成当前的时间
            DateTime LastDateTime, StartDateTime;
            StartDateTime = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,0,0,0);
            List<string> deletelist = new List<string>();
            //获取日志目录中的所有文件
            string[] strfiles = Directory.GetFiles(FileOperation.LogPath);
            foreach (string file in strfiles)
            { 
                //获取文件名
                int index = file.LastIndexOf(@"\");
                int end = file.LastIndexOf(".txt");
                if (end - index - 1 < 0 || index < 0 || end < 0)
                    continue;
                string filename = file.Substring(index + 1, end - index - 1);
                if(filename.Length != 8)
                    continue;
                stryear = filename.Substring(0, 4);
                strmonth = filename.Substring(4, 2);
                strday = filename.Substring(6, 2);
                try
                {
                    year = Convert.ToInt32(stryear);
                    month = Convert.ToInt32(strmonth);
                    dy = Convert.ToInt32(strday);
                }catch(Exception)
                {
                    continue;
                }
                LastDateTime = new DateTime(year, month, dy, 0, 0, 0);
                if ( Convert.ToInt32((StartDateTime - LastDateTime).Days) > day)
                {//可以删除
                    deletelist.Add(file);
                }
            }

            foreach (String str in deletelist)
            {
                File.Delete(str);
            }

        }
        /// <summary>
        /// 清理掉原始资讯
        /// </summary>
        public static void ClearOrigionalAlarm()
        {
            TimeSpan TS;
            int DayNum = 0;
            ArrayList RemoveData = new ArrayList();
            //清除原始信息的资料
            string[] StrDirs = FileOperation.GetAllDirName(FileOperation.Original);
            if (null == StrDirs) return;
            //取出年月日
            string StrYear, StrMonth, StrDay;
            int year,month,day;
            DateTime CurODT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime StartDT;
            foreach (string dir in StrDirs)
            {
                if (null == dir)
                {
                    continue;
                }
                string fileName = dir.Substring(dir.LastIndexOf('\\') + 1, dir.Length - dir.LastIndexOf('\\') - 1);
                StrYear = fileName.Substring(0, 4);
                StrMonth = fileName.Substring(4, 2);
                StrDay = fileName.Substring(6, 2);
                try 
                {
                    year = Convert.ToInt32(StrYear);month = Convert.ToInt32(StrMonth);day = Convert.ToInt32(StrDay);
                    StartDT = new DateTime(year, month,day,0,0,0);
                }catch(Exception)
                {
                    continue;
                }
                TS = CurODT - StartDT;
                DayNum = Convert.ToInt32(TS.TotalDays);
                if (DayNum >= SysParam.ClearTime)
                {
                    RemoveData.Add(dir);
                }
            }
            foreach (object obj in RemoveData)
            {
                try
                {
                    string sss = obj.ToString();
                    if (Directory.Exists(obj.ToString()))
                    {
                        Directory.Delete(obj.ToString(), true);
                    }
                }catch(Exception)
                {
                    continue;
                }
            }
        }
        /// <summary>
        /// 保存原始的资讯信息
        /// </summary>
        public static void SaveOriginalAlarm()
        {
            string StrWarmDir = FileOperation.WarmMsg;
            string StrWarmPath = StrWarmDir + "\\"+FileOperation.WarmName;
            try 
            {
                //首先判断当前的.dat文件是否存在
                if (!Directory.Exists(StrWarmDir) || !File.Exists(StrWarmPath))
                {
                    if (!FileOperation.CreateDirFile(StrWarmDir, StrWarmPath))
                    {//生成.dat文件失败
                        return;
                    }
                    FileOperation.SaveWarmData(CommonCollection.WarmInfors, StrWarmPath);
                }
                else
                { 
                    //说明当前的文件已经存在与文件夹中了,先将原始的警报讯息添加进去
                    CommonCollection.LogWarms = FileOperation.GetWarmData(StrWarmPath);
                    if (null != CommonCollection.LogWarms)
                    foreach(WarmInfo wm in CommonCollection.WarmInfors)
                        CommonCollection.LogWarms.Add(wm);
                    FileOperation.SaveWarmData(CommonCollection.LogWarms,StrWarmPath);
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        /// <summary>
        /// 加载警告的相关参数
        /// </summary>
        public static void LoadAlarmParam()
        {
            string StrPersonHelp = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.AlarmPersonHelp);
            if ("True".Equals(StrPersonHelp))
            {
                isPersonHelp = true;
            }
            else
            {
                isPersonHelp = false;
            }
            string StrEmery = FileOperation.GetValue(FileOperation.ConfigPath,FileOperation.AlarmSeg,FileOperation.AlarmEme);
            if ("True".Equals(StrEmery))
            {
                isEmergy = true;
            }
            else
            {
                isEmergy = false;
            }
            string StrAreaAdmin = FileOperation.GetValue(FileOperation.ConfigPath,FileOperation.AlarmSeg,FileOperation.AlarmAreaControl);
            if ("True".Equals(StrAreaAdmin))
            {
                isAreaControl = true;
            }
            else
            {
                isAreaControl = false;
            }
            string StrLowBattery = FileOperation.GetValue(FileOperation.ConfigPath,FileOperation.AlarmSeg,FileOperation.AlarmLowBattery);
            if ("True".Equals(StrLowBattery))
            {
                string StrLowBatteryValue = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.AlarmLowBatteryValue);
                try
                {
                    LowBattery = Convert.ToByte(StrLowBatteryValue);
                    isLowBattery = true;
                }
                catch (Exception)
                {
                    isLowBattery = false;
                }
            }
            else
            {
                isLowBattery = false;
            }
            string StrIsSound = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.isSound);
            if ("True".Equals(StrIsSound))
            {
                IsSoundAlarm = true;
                SoundName = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.SoundName);

                String strpersonhelp = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.MsgPersonHelp);
                if ("True".Equals(strpersonhelp))
                {
                    SysParam.isSoundPersonHelp = true;
                }
                else
                {
                    SysParam.isSoundPersonHelp = false;
                }

                String strpersonres = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.MsgPersonRes);
                if ("True".Equals(strpersonres))
                {
                    SysParam.isSoundPersonRes = true;
                }
                else {
                    SysParam.isSoundPersonRes = false;
                }

                String strbatt = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.MsgTagBatteryLow);
                if ("True".Equals(strbatt))
                {
                    SysParam.isSoundBatteryLow = true;
                }
                else
                {
                    SysParam.isSoundBatteryLow = false;
                }

                String strareaadmin = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.MsgAreaAdmin);
                if ("True".Equals(strareaadmin))
                {
                    SysParam.isSoundAreaControl = true;
                }
                else {
                    SysParam.isSoundAreaControl = false;
                }

                String strdevdis = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.MsgDeviceDis);
                if ("True".Equals(strdevdis)){
                    SysParam.isSoundDeviceTrouble = true;
                }
                else {
                    SysParam.isSoundDeviceTrouble = false;
                }
            }
            else
            {
                IsSoundAlarm = false;
            }
            string StrSoundTime = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.AlarmSeg, FileOperation.SoundTime);
            if (null == StrSoundTime)
            {
                SysParam.SoundTime = 0;
            }
            else
            {
                try
                {
                    SysParam.SoundTime = Convert.ToInt32(StrSoundTime);
                }
                catch (Exception)
                {
                    SysParam.SoundTime = 0;
                }
            }
            string StrSysScanTab = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.DeviceExceDis,FileOperation.SysScanTimeTab);
            if (null != StrSysScanTab && !"".Equals(StrSysScanTab))
            {
                try
                {
                    Measure_Interval = Convert.ToInt32(StrSysScanTab);
                }catch(Exception)
                {
                    Measure_Interval = 20;
                }
            }
            string StrTagDisParam1 = FileOperation.GetValue(FileOperation.ConfigPath,FileOperation.DeviceExceDis,FileOperation.TagDisTimeParam1);
            string StrTagDisParam2 = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.DeviceExceDis, FileOperation.TagDisTimeParam2);
            if (null != StrTagDisParam1 && null != StrTagDisParam2 && !"".Equals(StrTagDisParam1) && !"".Equals(StrTagDisParam2))
            {
                try 
                {
                    TagDisParam1 = Convert.ToInt32(StrTagDisParam1);
                    TagDisParam2 = Convert.ToInt32(StrTagDisParam2);
                }catch(Exception)
                {
                    TagDisParam1 = 4;
                    TagDisParam2 = 50;
                }
            }
            string StrReferDisParam1 = FileOperation.GetValue(FileOperation.ConfigPath,FileOperation.DeviceExceDis,FileOperation.ReferDisTimeParam1);
            string StrReferDisParam2 = FileOperation.GetValue(FileOperation.ConfigPath,FileOperation.DeviceExceDis,FileOperation.ReferDisTimeParam2);
            if (null != StrReferDisParam1 && null != StrReferDisParam2 && !"".Equals(StrReferDisParam1) && !"".Equals(StrReferDisParam2))
            {
                try 
                {
                    RouterParam1 = Convert.ToInt32(StrReferDisParam1);
                    RouterParam2 = Convert.ToInt32(StrReferDisParam2);
                }catch(Exception)
                {
                    RouterParam1 = 4;
                    RouterParam2 = 50;
                }
            }
            string StrIsClearData = FileOperation.GetValue(FileOperation.ConfigPath,FileOperation.ClearSeg,FileOperation.IsClearData);
            if ("True".Equals(StrIsClearData))
            {
                string StrClearTime = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.ClearSeg, FileOperation.ClearTime);
                try
                {
                    SysParam.ClearTime = Convert.ToInt32(StrClearTime);
                    SysParam.isClearData = true;
                }
                catch (Exception)
                {
                    ClearTime = 180;
                    SysParam.isClearData = true;
                }
            }
            else
            {
                SysParam.isClearData = false;
            }

            string StrIsClearWarm = FileOperation.GetValue(FileOperation.ConfigPath,FileOperation.ClearSeg,FileOperation.IsClearWarm);
            if ("True".Equals(StrIsClearWarm))
            {
                string StrClearTime = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.ClearSeg, FileOperation.ClearWarmTime);
                try
                {
                    SysParam.isOnTimeClearHandlerWarm = true;
                    SysParam.OnTimes = Convert.ToInt32(StrClearTime);
                }
                catch (Exception)
                {
                    SysParam.isOnTimeClearHandlerWarm = true;
                    SysParam.OnTimes = 7;
                }
            }
            else
            {
                SysParam.isOnTimeClearHandlerWarm = false;
            }
            string StrIsAutoClearHandle = FileOperation.GetValue(FileOperation.ConfigPath,FileOperation.ClearSeg,FileOperation.AutoClearHandle);
            if ("True".Equals(StrIsAutoClearHandle))
            {
                SysParam.isClearHandleAlarm = true;
                string StrAutoClearHandleTime = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.ClearSeg, FileOperation.AutoClearHandleTime);
                try
                {
                    SysParam.AutoClearHandleAlarmTime = Convert.ToInt32(StrAutoClearHandleTime);
                }
                catch (Exception)
                {
                    SysParam.AutoClearHandleAlarmTime = 3600;
                }
            }
            else
            {
                SysParam.isClearHandleAlarm = false;
            }
            string strComName = FileOperation.GetValue(FileOperation.ConfigPath,FileOperation.ComSeg,FileOperation.ComName);
            if (null != strComName && !"".Equals(strComName))
            {
                SysParam.isMsgSend = true;
                SysParam.ComName = strComName;
                string StrIsMsgType = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.ComSeg, FileOperation.MsgPersonHelp);
                if ("True".Equals(StrIsMsgType))
                {
                    SysParam.isSendPersonHelpMsg = true;
                }
                else 
                {
                    SysParam.isSendPersonHelpMsg = false;
                }
                StrIsMsgType = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.ComSeg, FileOperation.MsgAreaAdmin);
                if ("True".Equals(StrIsMsgType))
                {
                    SysParam.isSendAreaControlMsg = true;
                }
                else
                {
                    SysParam.isSendAreaControlMsg = false;
                }
                StrIsMsgType = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.ComSeg, FileOperation.MsgPersonRes);
                if ("True".Equals(StrIsMsgType))
                {
                    SysParam.isSendPersonResMsg = true;
                }
                else
                {
                    SysParam.isSendPersonResMsg = false;
                }
                StrIsMsgType = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.ComSeg, FileOperation.MsgTagBatteryLow);
                if ("True".Equals(StrIsMsgType))
                {
                    SysParam.isSendBatteryLowMsg = true;
                }
                else
                {
                    SysParam.isSendBatteryLowMsg = false;
                }

                StrIsMsgType = FileOperation.GetValue(FileOperation.ConfigPath, FileOperation.ComSeg, FileOperation.MsgDeviceDis);
                if ("True".Equals(StrIsMsgType))
                {
                    SysParam.isSendDeviceTrouble = true;
                }
                else
                {
                    SysParam.isSendDeviceTrouble = false;
                }
            }
            string strOptimizedmedol = FileOperation.GetValue(FileOperation.ConfigPath,FileOperation.OptimizedParam,FileOperation.OptimizedMedol);
            if ("1".Equals(strOptimizedmedol))
            {//连续跳点次数
                SysParam.curOptimalMedol = OptimizationModel.HopTimes;
                try
                {
                    string strOptimizedValue = FileOperation.GetValue(FileOperation.ConfigPath,FileOperation.OptimizedParam,FileOperation.OptimizedValue);
                    SysParam.PopTimes = Convert.ToInt32(strOptimizedValue);
                }catch(Exception)
                {
                    SysParam.PopTimes = 2;
                }
            }
            else
            {// 信号强度差值
                SysParam.curOptimalMedol = OptimizationModel.SigStrengthValue;
                try 
                {
                    string strOptimizedValue = FileOperation.GetValue(FileOperation.ConfigPath,FileOperation.OptimizedParam,FileOperation.OptimizedValue);
                    SysParam.RssiThreshold = Convert.ToInt32(strOptimizedValue);
                }catch(Exception)
                {
                    SysParam.RssiThreshold = 3;
                }
            }
            string stropertime = FileOperation.GetValue(FileOperation.OtherPath, FileOperation.StrPersonOperSeg, FileOperation.StrOpertimeKey);
            if (null == stropertime || "".Equals(stropertime))
            {
                SysParam.cleardaysnum = 30;
            }
            else
            {
                try
                {
                    SysParam.cleardaysnum = Convert.ToInt32(stropertime);
                }
                catch (Exception)
                {
                    SysParam.cleardaysnum = 30;
                }
            }
            //加载调试设置
            string strisdebug = FileOperation.GetValue(FileOperation.OtherPath,FileOperation.Debug,FileOperation.DebugKey);
            if ("True".Equals(strisdebug))
            {
                SysParam.isDebug = true;
                string strsettingdevices = FileOperation.GetValue(FileOperation.OtherPath,FileOperation.Debug,FileOperation.SettingArraoundDevices);
                if ("True".Equals(strsettingdevices))
                {
                    SysParam.isSettingArroundDevices = true;
                }else
                {
                    SysParam.isSettingArroundDevices = false;
                }
            }
            else
            {
                SysParam.isDebug = false;
            }
            
            string strautoclear = FileOperation.GetValue(FileOperation.OtherPath, FileOperation.Debug, FileOperation.AutoClearDevCnnAlarm);
            if ("True".Equals(strautoclear))
            {
                SysParam.isDevCnnLoss = true;
            }
            else
            {
                SysParam.isDevCnnLoss = false;
            }
        }


        /// <summary>
        /// 加载区域的相关信息
        /// </summary>
        public static void LoadAreaInfo()
        {
            ArrayList MyList = null;
            if(!FileOperation.GetAllSegment(FileOperation.AreaPath,out MyList))return;
            if (null == MyList)return;
            string StrAreaID, StrAreaType, 
                   StrAreaMapPath, StrAreaRouterID, 
                   StrAreaRouterName, StrAreaRouterVisible,
                   StrAreaRouterX, StrAreaRouterY;
            //MyList为所有区域的ID
            for (int i = 0; i < MyList.Count;i++)
            {
                Area MyArea = new Area();
                StrAreaID = MyList[i].ToString();
                if (StrAreaID.Length != 4)continue;
                try 
                {
                    MyArea.ID[0] = Convert.ToByte(StrAreaID.Substring(0,2),16);
                    MyArea.ID[1] = Convert.ToByte(StrAreaID.Substring(2,2),16);
                }catch(Exception)
                {
                    continue;
                }
                MyArea.Name = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(),FileOperation.AreaName);
                //加载组信息

                String StrAreaGroupID = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaGroupID);
                if (null == StrAreaGroupID)
                    continue;
                if (StrAreaGroupID.Length != 4){
                    MyArea.GroupID = null;
                }
                try {
                    MyArea.GroupID[0] = Convert.ToByte(StrAreaGroupID.Substring(0,2),16);
                    MyArea.GroupID[1] = Convert.ToByte(StrAreaGroupID.Substring(2,2),16);
                }catch(Exception){
                    MyArea.GroupID = null;
                }
                StrAreaType = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(),FileOperation.AreaType);
                switch(StrAreaType)
				{
                    case ConstInfor.StrControlArea:
                        MyArea.AreaType = AreaType.ControlArea;
                         break;
                    case ConstInfor.StrDangerArea:
                         MyArea.AreaType = AreaType.DangerArea;
                         break;
                    default:
                         MyArea.AreaType = AreaType.SimpleArea;
                         break;
                }
                StrAreaMapPath = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaMapPath);
                if (null != StrAreaMapPath && !"".Equals(StrAreaMapPath))
                {
                    MyArea.AreaBitMap.MapPath = StrAreaMapPath;
                    try
                    {
                        Bitmap MyBitmap = new Bitmap(FileOperation.MapPath + "\\" + MyArea.AreaBitMap.MapPath, true);
                        MyBitmap = new Bitmap(MyBitmap, ConstInfor.MapWidth, ConstInfor.MapHeight);
                        MyArea.AreaBitMap.MyBitmap = MyBitmap;
                    }
                    catch (Exception)
                    {

                    }
                }
                else
                {
                    MyArea.AreaBitMap = null;
                }
                MyArea.CurNum = 0;
                //加载区域中的参考点
                int index = 0;
                StrAreaRouterID = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaRouter + "_" + index.ToString());
                while (!"".Equals(StrAreaRouterID) && null != StrAreaRouterID)
                {
                    if (StrAreaRouterID.Length != 4)
                    {
                        StrAreaRouterID = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaRouter + "_" + (++index).ToString());
                        continue;
                    }
                    byte[] ID = new byte[2];
                    try 
                    {
                        ID[0] = Convert.ToByte(StrAreaRouterID.Substring(0, 2), 16);
                        ID[1] = Convert.ToByte(StrAreaRouterID.Substring(2, 2), 16);
                    }catch(Exception)
                    {
                        StrAreaRouterID = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaRouter + "_" + (++index).ToString());
                        continue;
                    }
                    BasicRouter Rr = new BasicRouter();
                    Rr.ID = ID;
                    StrAreaRouterName = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaRouterName + "_" + index.ToString());
                    Rr.Name = StrAreaRouterName;
                    StrAreaRouterVisible = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaRouterVisible + "_" + index.ToString());
                    if ("True".Equals(StrAreaRouterVisible))Rr.isVisible = true;
                    else Rr.isVisible = false;
                    StrAreaRouterX = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaRouterX + "_" + index.ToString());
                    StrAreaRouterY = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaRouterY + "_" + index.ToString());
                    try 
                    {
                        Rr.x = Convert.ToInt32(StrAreaRouterX);
                        Rr.y = Convert.ToInt32(StrAreaRouterY);
                    }catch(Exception)
                    {
                        StrAreaRouterID = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaRouter + "_" + (++index).ToString());continue;
                    }
                    MyArea.AreaRouter.Add(StrAreaRouterID, Rr);
                    StrAreaRouterID = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaRouter + "_" + (++index).ToString());
                }

                index = 0;
                StrAreaRouterID = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaNode + "_" + index.ToString());
                while (!"".Equals(StrAreaRouterID) && null != StrAreaRouterID)
                {
                    if (StrAreaRouterID.Length != 4)
                    {
                        StrAreaRouterID = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaNode + "_" + (++index).ToString());
                        continue;
                    }
                    byte[] ID = new byte[2];
                    try
                    {
                        ID[0] = Convert.ToByte(StrAreaRouterID.Substring(0, 2), 16);
                        ID[1] = Convert.ToByte(StrAreaRouterID.Substring(2, 2), 16);
                    }
                    catch (Exception)
                    {
                        StrAreaRouterID = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaNode + "_" + (++index).ToString());
                        continue;
                    }
                    BasicNode Bn = new BasicNode();
                    Bn.ID = ID;
                    StrAreaRouterName = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaNodeName + "_" + index.ToString());
                    Bn.Name = StrAreaRouterName;
                    StrAreaRouterVisible = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaNodeVisible + "_" + index.ToString());
                    if ("True".Equals(StrAreaRouterVisible)) Bn.isVisible = true;
                    else Bn.isVisible = false;
                    StrAreaRouterX = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaNodeX + "_" + index.ToString());
                    StrAreaRouterY = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaNodeY + "_" + index.ToString());
                    try
                    {
                        Bn.x = Convert.ToInt32(StrAreaRouterX);
                        Bn.y = Convert.ToInt32(StrAreaRouterY);
                    }
                    catch (Exception)
                    {StrAreaRouterID = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaNode + "_" + (++index).ToString());continue;}
                    MyArea.AreaNode.Add(StrAreaRouterID, Bn);
                    StrAreaRouterID = FileOperation.GetValue(FileOperation.AreaPath, MyList[i].ToString(), FileOperation.AreaNode + "_" + (++index).ToString());
                }
                CommonCollection.Areas.TryAdd(StrAreaID,MyArea);
            }
        }

        /// <summary>
        /// 加载人员卡片资讯
        /// </summary>
        public static void LoadPersonTagInfo(int type)
        {
            ArrayList MyList = null;
            String path = FileOperation.BackUpTagPath;
            if (type != 1) path = FileOperation.TagPath;
            if (!FileOperation.GetAllSegment(path, out MyList))
            {
                return;
            }
            if (null == MyList)
            {
                return;
            }
            if (type == 0 || type == 1) CommonCollection.Tags = new Dictionary<string, Tag>();

            String StrTagID = "",StrIsStopTime = "",StrStopTime = "";
            for (int i = 0; i < MyList.Count;i++)
            {
                Tag tag = new Tag();
                StrTagID = MyList[i].ToString();
                if (StrTagID.Length != 4)continue;
                try
                {
                    tag.ID[0] = Convert.ToByte(StrTagID.Substring(0,2),16);
                    tag.ID[1] = Convert.ToByte(StrTagID.Substring(2,2),16);
                }catch(Exception)
                {continue;}

                tag.Name = FileOperation.GetValue(path, StrTagID, FileOperation.TagName);

                string StrWorkTime = null;
                StrWorkTime = FileOperation.GetValue(path, StrTagID, FileOperation.TagGSWorkTime);
                if (ConstInfor.StrAlwayWork.Equals(StrWorkTime))
                {
                    tag.CurGSWorkTime = WorkTime.AlwaysWork;
                }
                else if (ConstInfor.StrNotWork.Equals(StrWorkTime))
                {
                    tag.CurGSWorkTime = WorkTime.NoWork;
                }
                else if (ConstInfor.StrLimitWork.Equals(StrWorkTime))
                {
                    tag.CurGSWorkTime = WorkTime.LimitTime;

                    string StrStartH = FileOperation.GetValue(path, StrTagID, FileOperation.TagGSStartTimeH);
                    string StrStartM = FileOperation.GetValue(path, StrTagID, FileOperation.TagGSStartTimeM);

                    string StrEndH = FileOperation.GetValue(path, StrTagID, FileOperation.TagGSEndTimeH);
                    string StrEndM = FileOperation.GetValue(path, StrTagID, FileOperation.TagGSEndTimeM);
                    int StartH = -1, StartM = -1, EndH = -1, EndM = -1;
                    if (StartH < 0 || StartH > 23 || EndH < 0 || EndH > 23 || StartM < 0 || StartM > 59 || EndM < 0 || EndM > 59)
                    {
                        tag.StartGSDT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, ConstInfor.DefaultStartH, ConstInfor.DefaultStartM, 0);
                        tag.EndGSDT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, ConstInfor.DefaultEndH, ConstInfor.DefaultEndM, 0);
                    }
                    else
                    {
                        tag.StartGSDT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, StartH, StartM, 0);
                        tag.EndGSDT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, EndH, EndM, 0);
                    }
                }

                string StrStartTime = FileOperation.GetValue(path, StrTagID, FileOperation.TagWorkStartTimeStamp); //改一下，这是工作时间
                string StrEndTime = FileOperation.GetValue(path, StrTagID, FileOperation.TagWorkEndTimeStamp);
                UInt32 StartT = 0, EndT = 0;
                try
                {
                    StartT = Convert.ToUInt32(StrStartTime);
                    EndT = Convert.ToUInt32(StrEndTime);
                    if (EndT == 0) EndT = 4133865599;
                }
                catch(Exception)
                {}
                if(StartT >= 0)
                {
                    System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                    DateTime startDt = startTime.AddSeconds(StartT);
                    System.DateTime endTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                    DateTime endDt = endTime.AddSeconds(EndT);
                    tag.StartWorkDT = startDt;
                    tag.EndWorkDT = endDt;
                }
                StrWorkTime = FileOperation.GetValue(path, StrTagID, FileOperation.TagWorkTime);
                if (ConstInfor.StrAlwayWork.Equals(StrWorkTime))
                {
                    tag.CurTagWorkTime = WorkTime.AlwaysWork;
                }
                else if (ConstInfor.StrNotWork.Equals(StrWorkTime))
                {
                    tag.CurTagWorkTime = WorkTime.NoWork;
                }
                else if (ConstInfor.StrLimitWork.Equals(StrWorkTime))
                {
                    tag.CurTagWorkTime = WorkTime.LimitTime;
                }
                /*  string StrStartH = FileOperation.GetValue(FileOperation.TagPath, StrTagID, FileOperation.TagStartTimeH);
                    string StrStartM = FileOperation.GetValue(FileOperation.TagPath, StrTagID, FileOperation.TagStartTimeM);

                    string StrEndH = FileOperation.GetValue(FileOperation.TagPath, StrTagID, FileOperation.TagEndTimeH);
                    string StrEndM = FileOperation.GetValue(FileOperation.TagPath, StrTagID, FileOperation.TagEndTimeM);
                    int StartH = -1, StartM = -1, EndH = -1, EndM = -1;
                    if (null != StrStartH && null != StrStartM && null != StrEndH && null != StrEndM)
                    {
                        try
                        {
                            StartH = Convert.ToInt32(StrStartH); StartM = Convert.ToInt32(StrStartM);
                            EndH = Convert.ToInt32(StrEndH); EndM = Convert.ToInt32(StrEndM);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (StartH < 0 || StartH > 23 || EndH < 0 || EndH > 23 || StartM < 0 || StartM > 59 || EndM < 0 || EndM > 59)
                    {
                        tag.StartWorkDT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, ConstInfor.DefaultStartH, ConstInfor.DefaultStartM, 0);
                        tag.EndWorkDT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, ConstInfor.DefaultEndH, ConstInfor.DefaultEndM, 0);
                    }*/
                    else
                    {
                        DateTime zuoTian = DateTime.Now.AddDays(-1);
                        tag.StartWorkDT = new DateTime(zuoTian.Year, zuoTian.Month, zuoTian.Day, 0, 0, 0);
                        tag.EndWorkDT = new DateTime(zuoTian.Year, zuoTian.Month, zuoTian.Day, 23, 59, 0);
                    }
                //}
                
                /*
                if (null != StrStartH && null != StrStartM && null != StrEndH && null != StrEndM)
                {
                    try
                    {
                        StartH = Convert.ToInt32(StrStartH); StartM = Convert.ToInt32(StrStartM);
                        EndH = Convert.ToInt32(StrEndH); EndM = Convert.ToInt32(StrEndM);
                    }
                    catch (Exception)
                    {
                    }
                }*/

                StrIsStopTime = FileOperation.GetValue(path, StrTagID, FileOperation.TagIsStopTime);
                if ("True".Equals(StrIsStopTime))
                {
                    tag.IsStopAlarm = true;
                    StrStopTime = FileOperation.GetValue(path, StrTagID, FileOperation.TagStopTime);
                    try
                    {
                        tag.StopTime = Convert.ToInt32(StrStopTime);
                    }catch(Exception)
                    {//若出现异常 => 
                        tag.IsStopAlarm = false;
                    }
                }
                else
                {
                    tag.IsStopAlarm = false;
                }
                int ReferNum = 0;
                foreach (KeyValuePair<string, Area> area in CommonCollection.Areas)
                    ReferNum += area.Value.AreaRouter.Count;

                ArrayList AreaIDs = new ArrayList();
                FileOperation.GetAllKey(path, StrTagID, AreaIDs);
                foreach (string StrAreaId in AreaIDs)
                {
                    if (null == StrAreaId) continue;
                    if ("".Equals(StrAreaId)) continue;
                    int index = StrAreaId.IndexOf("_");
                    if (index >= 0)
                    {
                        string head = StrAreaId.Substring(0, index);
                        if("RD".Equals(head))
                        {
                            string StrReferID = FileOperation.GetValue(path, StrTagID, StrAreaId);
                            tag.TagReliableList.Add(StrReferID);
                        }
                    }
                }
                string workIDString = FileOperation.GetValue(path, StrTagID, FileOperation.TagWorkID);
                tag.workID = DrawIMG.getworkIDFormTextBox(workIDString);
                
                if (CommonCollection.Tags.ContainsKey(StrTagID))
                {
                    CommonCollection.Tags[StrTagID] = tag;
                }
                else 
                {
                    CommonCollection.Tags.Add(StrTagID, tag);
                }
               
            }
        }
        /// <summary>
        /// 加载网络资讯
        /// </summary>
        public static void LoadNetInfo()
        {
          string StrIP=FileOperation.GetValue(FileOperation.ConfigPath,FileOperation.NetSeg,FileOperation.NetKey_IP);
          if (null != StrIP)
              NetParam.Ip = StrIP; 
          string StrPort = FileOperation.GetValue(FileOperation.ConfigPath,FileOperation.NetSeg,FileOperation.NetKey_Port);
          int Port = 0;
          try { 
            Port = Convert.ToInt32(StrPort);
          }catch(Exception)
          {
              return; 
          }
          NetParam.Port = Port;
        }
        /// <summary>
        /// 加载发送短信的用户信息
        /// </summary>
        public static void LoadPersonMsg()
        { 
            ArrayList MyList = null;
            if (!FileOperation.GetAllSegment(FileOperation.PersonMsgPath, out MyList))return;
            if (null == MyList)return;
            string StrPersonID="";
            for (int i = 0; i < MyList.Count;i++)
            {
                StrPersonID = MyList[i].ToString();
                PhonePerson PPson = new PhonePerson();
                try 
                {PPson.ID = Convert.ToInt32(StrPersonID);}
                catch(Exception)
                {continue;}
                PPson.Name = FileOperation.GetValue(FileOperation.PersonMsgPath, StrPersonID,FileOperation.PersonName);
                PPson.PhoneNumber = FileOperation.GetValue(FileOperation.PersonMsgPath,StrPersonID,FileOperation.PersonPhone);
                CommonCollection.PhonePersons.Add(PPson);
            }
        }
        /// <summary>
        /// 加载组信息
        /// </summary>
        public static void LoadGroup()
        {
            ArrayList MyList = null;
            if (!FileOperation.GetAllSegment(FileOperation.GroupPath, out MyList))return;
            if (null == MyList)return;
            for (int i = 0; i < MyList.Count;i++)
            {
                string StrGroupID = MyList[i].ToString();
                if (StrGroupID.Length != 4)
                {
                    continue;
                }
                Group group = new Group();
                try {
                    group.ID[0] = Convert.ToByte(StrGroupID.Substring(0,2),16);
                    group.ID[1] = Convert.ToByte(StrGroupID.Substring(2,2),16);
                }catch(Exception)
                {
                    continue;
                }
                group.Name = FileOperation.GetValue(FileOperation.GroupPath, StrGroupID, FileOperation.GroupName);
                CommonCollection.Groups.Add(StrGroupID, group);
            }
            //每次更新到列表中时，重新对它进行排序
            List<KeyValuePair<string, Group>> gps = CommonCollection.Groups.OrderBy(c => c.Key).ToList();
            CommonCollection.Groups.Clear();
            foreach (KeyValuePair<string, Group> grp in gps)
            {
                CommonCollection.Groups.Add(grp.Key, grp.Value);
            }
        }
        /// <summary>
        /// 将区域中的参考点增加到参考点列表中，从而修改参考点的名称
        /// </summary>
        public static void UpdateRouterLv()
        {
            String StrGroupID;
            Pst.RouterListView.Items.Clear();
            List<KeyValuePair<string, Area>> areas = CommonCollection.Areas.OrderBy(c => c.Key).ToList();
            foreach (KeyValuePair<string, Area> MyArea in areas)
            {
                foreach (KeyValuePair<string, BasicRouter> Brt in MyArea.Value.AreaRouter)
                {
                    ListViewItem Item = new ListViewItem();
                    Item.Text = Brt.Key;
                    Item.Name = Brt.Key;

                    if (null != Brt.Value.Name) Item.SubItems.Add(Brt.Value.Name);
                    else Item.SubItems.Add("");
                    Item.SubItems.Add("參考點");
                    if (Brt.Value.isVisible) Item.SubItems.Add("Yes");
                    else Item.SubItems.Add("No");

                    if (null == MyArea.Value.GroupID || (MyArea.Value.GroupID[0] == 0 && MyArea.Value.GroupID[1] == 0)) StrGroupID = ConstInfor.NoGroup;
                    else StrGroupID = MyArea.Value.GroupID[0].ToString("X2") + MyArea.Value.GroupID[1].ToString("X2");
                    Group Group = null;
                    lock (CommonCollection.Groups_Lock) CommonCollection.Groups.TryGetValue(StrGroupID, out Group);
                    if (null == Group)
                    {
                        Item.SubItems.Add(StrGroupID);
                    }
                    else
                    {
                        if ("".Equals(Group.Name))
                        {
                            Item.SubItems.Add(StrGroupID);
                        }
                        else
                        {
                            Item.SubItems.Add(Group.Name + "(" + StrGroupID + ")");
                        }
                    }
                    if (null == MyArea.Value.Name || "".Equals(MyArea.Value.Name))
                    {
                        Item.SubItems.Add(MyArea.Key);
                    }
                    else
                    {
                        Item.SubItems.Add(MyArea.Value.Name + "(" + MyArea.Key + ")");
                    }
                    Pst.RouterListView.Items.Add(Item);
                }
            }
            foreach (KeyValuePair<string, Area> MyArea in areas)
            {
                foreach (KeyValuePair<string, BasicNode> Bn in MyArea.Value.AreaNode)
                {
                    ListViewItem Item = new ListViewItem();
                    Item.Text = Bn.Key;
                    Item.Name = Bn.Key;//每一个数据节点前面加上一个"FF",以防键值冲突

                    if (null != Bn.Value.Name)
                    {
                        Item.SubItems.Add(Bn.Value.Name);
                    }
                    else
                    {
                        Item.SubItems.Add("");
                    }
                    Item.SubItems.Add("數據節點");
                    if (Bn.Value.isVisible)
                    {
                        Item.SubItems.Add("Yes");
                    }
                    else
                    {
                        Item.SubItems.Add("No");
                    }

                    if (null == MyArea.Value.GroupID || (MyArea.Value.GroupID[0] == 0 && MyArea.Value.GroupID[1] == 0))
                    {
                        StrGroupID = ConstInfor.NoGroup;
                    }
                    else
                    {
                        StrGroupID = MyArea.Value.GroupID[0].ToString("X2") + MyArea.Value.GroupID[1].ToString("X2");
                    }
                    Group Group = null;
                    lock (CommonCollection.Groups_Lock)
                    {
                        CommonCollection.Groups.TryGetValue(StrGroupID, out Group);
                    }
                    if (null == Group)
                    {
                        Item.SubItems.Add(StrGroupID);
                    }
                    else
                    {
                        if ("".Equals(Group.Name)) Item.SubItems.Add(StrGroupID);
                        else Item.SubItems.Add(Group.Name + "(" + StrGroupID + ")");
                    }
                    if (null == MyArea.Value.Name || "".Equals(MyArea.Value.Name)) Item.SubItems.Add(MyArea.Key);
                    else Item.SubItems.Add(MyArea.Value.Name + "(" + MyArea.Key + ")");
                    Pst.RouterListView.Items.Add(Item);
                }
            }
            Pst.RouterListView.Sorting = SortOrder.Ascending;
            Pst.RouterListView.Sort();
            Pst.RouterListView.ListViewItemSorter = new ListViewItemComparer(0, Pst.RouterListView.Sorting);
            Pst.CancalSortingView(Pst.curSortColumn);
            Pst.UpdateSortingView(0, Pst.RouterListView.Sorting);
            Pst.curSortColumn = 0;
        }
        /// <summary>
        /// 更改区域参考点的名称
        /// </summary>
        public static void UpdateAreaRouterName(string StrRouterID,string StrRouterName,bool isVisible)
        {
                foreach(KeyValuePair<string,Area> MyArea in CommonCollection.Areas)
                {
                    if (null == MyArea.Value.AreaRouter)
                        continue;
                    BasicRouter Bsr = null;
                    MyArea.Value.AreaRouter.TryGetValue(StrRouterID, out Bsr);

                    if (Bsr == null)
                        continue;
                    Bsr.Name = StrRouterName;
                    Bsr.isVisible = isVisible;
                    break;
                }
        }

        public static void UpdateAreaDataNodeName(string StrRouterID,string StrRouterName,bool isVisible)
        {
                foreach (KeyValuePair<string, Area> MyArea in CommonCollection.Areas)
                {
                    if (null == MyArea.Value.AreaRouter) continue;
                    BasicNode Bn = null;
                    MyArea.Value.AreaNode.TryGetValue(StrRouterID, out Bn);
                    if (Bn == null)
                        continue;

                    Bn.Name = StrRouterName;
                    Bn.isVisible = isVisible;
                    break;
                }
            
        }

        public static TagPack GetPackTag(String StrTagID)
        {
            if (null == StrTagID)
                return null;
            TagPack MyTag = null;
            try{
                CommonCollection.TagPacks.TryGetValue(StrTagID, out MyTag);
            }catch(Exception)
            {
            }
            return MyTag;
        }
        public static TagPack GetPackTag_Name(String StrName)
        {
            if (null == StrName)
                return null;
            String StrTagID = null;
            lock (CommonCollection.Tags_Lock)
            {
                foreach(KeyValuePair<string,Tag> tag in CommonCollection.Tags)
                {
                    if (null == tag.Value)
                        continue;
                    if (StrName.Equals(tag.Value.Name))
                    {
                        StrTagID = tag.Key;
                        break;
                    }
                }
            }
            return GetPackTag(StrTagID);
        }
        /// <summary>
        /// 根据组的ID得到组的相关信息
        /// </summary>
        /// <returns></returns>
        public static Group GetGroup_ID(String StrID)
        {
            if (null == StrID)
                return null;
            Group CurGroup = null;
            lock (CommonCollection.Groups_Lock)
            {
                CommonCollection.Groups.TryGetValue(StrID, out CurGroup);
            }
            return CurGroup;
        }
        public static Group GetGroup_IDName(string StrID_Name)
        {
            string StrID = GetStrID(StrID_Name);
            return GetGroup_ID(StrID);
        }
        /// <summary>
        /// 根据组别的ID得到区域的个数
        /// </summary>
        /// <param name="StrGroupID"></param>
        /// <returns></returns>
        public static int GetAreaNumInGroup(string StrGroupIDNAME)
        {
            if (null == StrGroupIDNAME)
                return -1;
            if ("".Equals(StrGroupIDNAME))
                return 0;
            int index = 0;
            foreach (ListViewItem item in Pst.AreaListView.Items)
            {
                if (StrGroupIDNAME.Equals(item.SubItems[3].Text.Trim()))
                {
                    index++;
                }
            }
            return index;
        }
        /// <summary>
        /// 根据Name+"("+ID+")"获取ID
        /// </summary>
        /// <param name="StrGroupIDNAME"></param>
        /// <returns></returns>
        public static string GetStrID(string StrIdName)
        {
            string StrID;
            StrIdName = StrIdName.Trim();
            int index1, index2;
            //判断其结尾是否是反括号
            if (StrIdName.EndsWith(")"))
            {
                index1 = StrIdName.IndexOf("(");
                index2 = StrIdName.IndexOf(")");
                if (index1 >= StrIdName.Length || index2 >= StrIdName.Length || index1 < 0 || index2 < 0)
                {
                    return null;
                }
                StrID = StrIdName.Substring(index1 + 1,4);
            }
            else
            {
                StrID = StrIdName;
            }
          
            if (StrID.Length != 4)
            {
                //可能是Type:ID格式
                index1 = StrID.IndexOf(":");
                if (index1 < 0)
                    return null;
                if (index1 + 4 < StrID.Length)
                    StrID = StrID.Substring(index1 + 1,4);
                else
                    return null;
                return GetStrID(StrID);
            }
            byte id1, id2;
            try {
                id1 = Convert.ToByte(StrID.Substring(0,2),16);
                id2 = Convert.ToByte(StrID.Substring(2,2),16);
            }catch(Exception)
            {
                return null;
            }
            return StrID;
        }
        /// <summary>
        /// 根据PackTag的名称获取PackTag的
        /// </summary>
        /// <param name="Str"></param>
        /// <param name="MyTagPack"></param>
        /// <returns></returns>
        public static bool TryGetPackTag(string StrIDORName, out TagPack MyTagPack)
        {
            //先根据ID查找
            if (CommonCollection.TagPacks.TryGetValue(StrIDORName, out MyTagPack))
            {
                return true;
            }
            //根据名称查找
            lock (CommonCollection.Tags_Lock)
            {
                foreach (KeyValuePair<string, Tag> MyTag in CommonCollection.Tags)
                {
                    if (MyTag.Value.Name.Equals(StrIDORName)) StrIDORName = MyTag.Key;
                }
            }
            try
            {
                //先根据ID看是否能够获得
                if (CommonCollection.TagPacks.TryGetValue(StrIDORName, out MyTagPack))
                    return true;
            }
            catch (Exception)
			{
			}
            return false;
        }
        /// <summary>
        /// 判断指定的TagID的Tag是否在某个特殊警告集合中
        /// </summary>
        /// <returns></returns>
        public static bool isTagExistAlarmBox(string StrTagID)
        {
            if (null == StrTagID)
                return false;
            string StrCurTagID;
            try
            {
                foreach (WarmInfo warm in CommonCollection.WarmInfors)
                {
                    string StrClassName = warm.GetType().Name;
                    if ("PersonHelp".Equals(StrClassName))
                    {
                        StrCurTagID = ((PersonHelp)warm).TD[0].ToString("X2") + ((PersonHelp)warm).TD[1].ToString("X2");
                        if (StrTagID.Equals(StrCurTagID)) return true;
                    }
                    else if ("AreaAdmin".Equals(StrClassName))
                    {
                        StrCurTagID = ((AreaAdmin)warm).TD[0].ToString("X2") + ((AreaAdmin)warm).TD[1].ToString("X2");
                        if (StrTagID.Equals(StrCurTagID)) return true;
                    }
                    else if ("PersonRes".Equals(StrClassName))
                    {
                        StrCurTagID = ((PersonRes)warm).TD[0].ToString("X2") + ((PersonRes)warm).TD[1].ToString("X2");
                        if (StrTagID.Equals(StrCurTagID)) return true;
                    }
                    else if ("BattLow".Equals(StrClassName))
                    {
                        StrCurTagID = ((BattLow)warm).TD[0].ToString("X2") + ((BattLow)warm).TD[1].ToString("X2");
                        if (StrTagID.Equals(StrCurTagID)) return true;
                    }
                    else if ("TagDis".Equals(StrClassName))
                    {
                        StrCurTagID = ((TagDis)warm).TD[0].ToString("X2") + ((TagDis)warm).TD[1].ToString("X2");
                        if (StrTagID.Equals(StrCurTagID)) return true;
                    }
                }
            }
            catch (Exception)
            {

            }
            return false;
        }
        public static void SearchListViewStr(ListView listview,String Str)
        {
            ListViewItem item = null;
            int index = -1;
            while (index < listview.Items.Count - 1)
            {

                item = listview.FindItemWithText(Str, true, index + 1);
                if (null == item)
                    break;
                item.BackColor = Color.Red;
                index = item.Index;
                item.EnsureVisible();
            }
        }
        public static void ClearListViewWhiteItem(ListView listview)
        {
            foreach (ListViewItem item in listview.Items)
            {
                if (item.BackColor == Color.Red)
                {
                    item.BackColor = Color.White;
                }
            }
        }
        //获取警报集合中的项
        public static WarmInfo GetWarmItem(string StrTagID, SpeceilAlarm CurAlarmType)
        {
            if (null == StrTagID || "".Equals(StrTagID)) return null;
            if (CurAlarmType == SpeceilAlarm.UnKnown) return null;
            try
            {
                foreach (WarmInfo winf in CommonCollection.WarmInfors)
                {
                    if (null == winf) continue;
                    string ClassName = winf.GetType().Name;
                    switch (CurAlarmType)
                    {
                        case SpeceilAlarm.PersonHelp:
                            if ("PersonHelp".Equals(ClassName))
                            { //说明当前的类为人员求救类
                                string StrCurID = ((PersonHelp)winf).TD[0].ToString("X2") + ((PersonHelp)winf).TD[1].ToString("X2");
                                if (StrTagID.Equals(StrCurID))
                                {
                                    return winf;
                                }
                            }
                            break;
                        case SpeceilAlarm.AreaControl:
                            if ("AreaAdmin".Equals(ClassName))
                            { //说明当前的类为人员求救类
                                string StrCurID = ((AreaAdmin)winf).TD[0].ToString("X2") + ((AreaAdmin)winf).TD[1].ToString("X2");
                                if (StrTagID.Equals(StrCurID))
                                {
                                    return winf;
                                }
                            }
                            break;
                        case SpeceilAlarm.Resid:
                            if ("PersonRes".Equals(ClassName))
                            { //说明当前的类为人员求救类
                                string StrCurID = ((PersonRes)winf).TD[0].ToString("X2") + ((PersonRes)winf).TD[1].ToString("X2");
                                if (StrTagID.Equals(StrCurID))
                                {
                                    return winf;
                                }
                            }
                            break;
                        case SpeceilAlarm.NodeDis:
                            if ("NodeDis".Equals(ClassName))
                            { //说明当前的类为人员求救类
                                string StrCurID = ((NodeDis)winf).RD[0].ToString("X2") + ((NodeDis)winf).RD[1].ToString("X2");
                                if (StrTagID.Equals(StrCurID))
                                {
                                    return winf;
                                }
                            }
                            break;
                        case SpeceilAlarm.ReferDis:
                            if ("ReferDis".Equals(ClassName))
                            { //说明当前的类为人员求救类
                                string StrCurID = ((ReferDis)winf).RD[0].ToString("X2") + ((ReferDis)winf).RD[1].ToString("X2");
                                if (StrTagID.Equals(StrCurID))
                                {
                                    return winf;
                                }
                            }
                            break;
                        case SpeceilAlarm.TagDis:
                            if ("TagDis".Equals(ClassName))
                            { //说明当前的类为人员求救类
                                string StrCurID = ((TagDis)winf).TD[0].ToString("X2") + ((TagDis)winf).TD[1].ToString("X2");
                                if (StrTagID.Equals(StrCurID))
                                {
                                    return winf;
                                }
                            }
                            break;
                        case SpeceilAlarm.BatteryLow:
                            if ("BattLow".Equals(ClassName))
                            { //说明当前的类为人员求救类
                                string StrCurID = ((BattLow)winf).TD[0].ToString("X2") + ((BattLow)winf).TD[1].ToString("X2");
                                if (StrTagID.Equals(StrCurID))
                                {
                                    return winf;
                                }
                            }
                            break;
                        default:
                            return null;
                    }

                }
            }catch(Exception)
            {
            }
            return null;
        }

    }
}
                            