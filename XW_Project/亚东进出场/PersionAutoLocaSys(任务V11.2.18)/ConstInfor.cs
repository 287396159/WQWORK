using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Collections;
using System.Net;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using MG3732_MSG;
using PersionAutoLocaSys.Bean;
using PersionAutoLocaSys.Model;
using SerialportSample;
namespace PersionAutoLocaSys
{
    /// <summary>
    /// 常量信息
    /// </summary>
    public class ConstInfor
    {
        //主标题栏的宽度，高度
        public const int MainTitle_Width = 916;
        public const int MainTitle_Height = 124;
        //Log的左边距，上边距
        public const int Log_left = 60;
        public const int Log_top = 40;
        public const int Tag_PC_left = 380;
        public const int Tag_PC_top = 30;
        //开始连接图片的左边距，上边距,宽高
        public const int BgCnnBtn_left = 720;
        public const int BgCnnBtn_top = 10;
        public const int BgCnn_Width = 128;
        public const int BgCnn_Height = 31;
        //设置按钮图片的左边距，上边距，宽高
        public const int BgSetBtn_left = 720;
        public const int BgSetBtn_top = 45;
        public const int BgSet_Width = 128;
        public const int BgSet_Height = 31;
        //查询按钮图片的左边距，上边距，宽高
        public const int SelecBtn_left = 720;
        public const int SelectBtn_top = 80;
        public const int Select_Width = 128;
        public const int Select_Height = 31;
        //信号动画
        public const int Sign_Speed = 6;//没秒移动的速度
        //横杠的高度及两个表格间的间距
        public const int Cross_Ban_H = 7;
        public const int TableSpace = 5;
        //主中心区的宽度，高度
        public const int MainCenter_Width = 915;
        public const int MainCenter_Height = 486;
        //Person表格距离左边框的距离，距离顶端的距离
        public const int Table_Left = 10;
        public const int Table_Top = Cross_Ban_H;
        //中心区的人员卡片的表格的信息
        public const int PsCell_First_Width = 200;
        public const int PsCell_RowNum = 4;//中心区的人员卡片的表格的一行的个数
        public const int PsCell_ColNum = 10;//中心区的人员卡片的表格的列数（为单数）
        //警报表格距离顶部的距离
        public const int Alarm_TB_Bottom = 106;
        public const int AlarmCell_First_Width = 200;
        public const int AlarmCell_RowNum = 4;
        public const int AlarmCell_ColNum = 2;
        public const int AlarmBtt = 20;//警报表格底部距离下端的距离
        //当参考点中不选择区域时显示的字符串
        public const string NoArea = "No Area";
        //当前区域不需要任何一个组时
        public const string NoGroup = "No Group";
        //区域类型的对应名称
        public const string StrSimpleArea = "一般區域";
        public const string StrControlArea = "管制區域";
        public const string StrDangerArea = "危險區域";
        //区域地图的长宽
        public const int AreaMapWidth = 666;
        public const int AreaMapHeight = 420;
        //参考点的长宽
        public const int RouterWidth = 40;
        public const int RouterHeight = 20;

        public const int DataNodeWidth = 50;
        public const int DataNodeHeight = 20;
        //Tag的半径
        public const int TagR = 8;
        //电池电量低于多少报警
        public const int BatterLow = 5;
        public const int MapWidth = 781;//629 739  745  781,
        public const int MapHeight = 479;//348 419  461   479
        //警告信息
        public const string StrAlarmInfor = "報警資訊";
        public const string StrInforNum = "數量";
        public const string StrAreaControl = "區域管制";
        public const string StrPersonHelp = "人員求救";
        public const string StrBattery = "電量不足";
        public const string StrRouterAdmin = "參考點";
        public const string StrPersonRs = "人員未移動";
        public const string StrAlarmMore = "...";
        //人员表格
        public const string StrPerDivArea = "人員分區";
        public const string StrPerNum = "人數";
        public const string StrPerTotalNum = "人員總數";
        //查询框 X 位置
        public const int QueryX_Place = 260;
        public const int QueryX_Width = 41;
        public const int QueryX_Height = 26;
        //查询框 查询按钮的位置
        public const int QueryBtn_Left = 185;
        public const int QueryBtn_Top = 71;
        public const int QueryBtn_Width = 72;
        public const int QueryBtn_Height = 35;
        //查询框 文本的位置
        public const int QueryLabel_Left = 20;
        public const int QueryLabel_Top = 48;

        //网络参数
        public const int Net_Ok = 0;
        public const int Ip_Invalid = 1;
        public const int Port_Invalid = 2;
        public const int NetNode_Fail = 3;
        public const int UdpClient_Fail = 4;
        public const int UdpClient_ReDefine = 5;

        //Tag封包
        public const byte Head_Loca = 0xFE;
        public const byte Type_Cmm = 0x03;
        public const byte Type_Alarm = 0x04;
        public const byte End_Loca = 0xFD;
        //Router封包
        public const byte Head_Refer = 0xFC;
        public const byte Type_Refer = 0x02;
        public const byte End_Refer = 0xFB;
        public const byte RouterLocalPack = 14;
        //DataNode封包
        public const byte Head_Node = 0xFC;
        public const byte Type_Node = 0x01;
        public const byte End_Node = 0xFB;
        public const byte NodePackLen = 12;
        //系统检测设备是否断开的默认参数
        public const int DefaultSysScanTime = 20;

        public const int DefaultTagDisParam1Time = 4;
        public const int DefaultTagDisParam2Time = 50;

        public const int DefaultRouterParam1Time = 4;
        public const int DefaultRouterParam2Time = 50;

        public const String DefaultSoundAlarm = "DefaultSound";

        public const String PersonHelpMsg = "@><@人員發生求救,请查看應用...";
        public const String AreaControlMsg1 = "@><@人員進入限制區域";
        public const String AreaControlMsg2 = ",請及時處理...";
        public const String PersonRisMsg1 = "@><@人員未移動超過指定時間(";
        public const String PersonRisMsg2 = "),請及時處理...";
        public const String TagBatteryLowMsg1 = "卡片電量(";
        public const String TagBatteryLowMsg2 = ")不足,請及時更換電池...";
        public const String TagDisMsg = "卡片斷開连接,請及時查看原因...";
        public const String ReferDisMsg = "參考點斷開連接,請及時查看原因...";
        public const String NodeDisMsg = "數據節點斷開連接,請及時查看原因...";
        //当判断短信发送失败时,重发次数
        public const int SendMsgFail_Count = 3;
        public const string DefaultUserName = "admin";
        public const string DefaultPW = "admin";

        public const string dmatekname = "DMATEK";
        public const string dmatekpsw = "dmatek1234";
        public const string StrAlwayWork = "AlwayWork";
        public const string StrNotWork = "NotWork";
        public const string StrUnKnown = "UnKnown";
        public const string StrLimitWork = "LimitWork";
        public const int DefaultStartH = 8;
        public const int DefaultStartM = 0;
        public const int DefaultEndH = 18;
        public const int DefaultEndM = 0;

        public const string DefaultSoundTime = "Loop";

        public static string StrAdminPerson = "管理人員";
        public static string StrSimplePerson = "一般人員";

        public static Point PersonAdminNameLoca = new Point(33, 61);
        public static Point PersonAdminPsLoca = new Point(33, 92);
        public static Point PersonAdminPerimLoca = new Point(9, 125);

        public static string StrPersonIDFormatErr = "對不起,人員ID格式有誤!";
        public static string StrSorryPerosnNotLess0 = "對不起，人員ID不能小於0";
        public static string StrSorryChoosePersonItemExist = "對不起，你選擇的人員項已經存在!";
        public static string StrSorryUpdateUserExist = "對不起，你更新的用戶不存在!";
        public static string StrSorryNotChooseNeedRemoveItem = "對不起,你還沒有選擇需要删除的项!";

        public const UInt16 FORMMESSAGE = 0x0112;
        public const UInt16 CLOSEMSGPARAM = 0xF060;
        public static IntPtr FORMMSGMOVE = (IntPtr)0xF012;

        public const int recnwtimeout = 100;

        public const int recrefertimeout = 50;
        //设置Router的数据包
        public const byte head1 = 0xFA;
        public const byte end1 = 0xFB;

        //通过Node设置周围节点及参考点讯息
        public const byte head2 = 0xFC;
        public const byte end2 = 0xFB;
        //版本及固件类型的位置
        public const int verplace = 18436;//0x4804
        public const int imgplace = 18432;//0x4800
        //基地址
        public const UInt16 baseaddr = 0x4000;


        public static string GetDevType(NodeType devtype,byte type)
        {
            string str = "";
            if (devtype == NodeType.DataNode)
            {
                switch (type)
                {
                    case 0x01:
                        str = "ZB2530-01PA_02PA_WIFI_V1.0(NODE)";
                        break;
                    case 0x02:
                        str = "ZB2530-LAN_V02.02(NODE)";
                        break;
                    case 0x03:
                        str = "ZB2530-LAN-04_V01.01(NODE)";
                        break;
                    case 0x04:
                        str = "ZB2530-WIFI-04_V01.01(NODE)";
                        break;
                    default:
                        break;
                }
            }
            else if (devtype == NodeType.ReferNode)
            {
                switch (type)
                {
                    case 0x01:
                        str = "ZB2530-01PA/02PA/WIFI_V1.0(REFER)";
                        break;
                    case 0x02:
                        str = "ZB2530-03_V1.0(REFER)";
                        break;
                    case 0x03:
                        str = "ZB2530-LAN/WIFI-04_V01.01(REFER)";
                        break;
                    case 0x04:
                        str = "ZB2530-04_V1.1(REFER)";
                        break;
                    default:
                        break;
                }
            }
            return str;
        }

    }
    /// <summary>
    /// 画出指定Tag的位置
    /// </summary>
    public class RtAroundTagPlace
    {  
        public static TagPack CurImageTag = null;
        public static int mode = 0;//当mode = 0时为一般模式，为1时为搜索模式
        //其中StrRouterID、RouterX，RouterY表示Router的ID
        public static String StrRouterID = null;
        //Num表示当前Router的周围的Tag的数量
        public static int Num = 0;
        //MyTag表示要画的Tag的ID
        public static string StrTagID = null;
        //卡片当前应该在的位置
        public static BasicRouter CurBasic = null;
        public static double scale;
        /// <summary>
        /// 清除Router的周围位置的占用情况
        /// </summary>
        /// <param name="MyArea"></param>
        public static void ClearAreaAllRouterStand(Area MyArea)
        {
            if (null == MyArea)
                return;
            if (null == MyArea.AreaRouter)
                return;
            foreach(KeyValuePair<string,BasicRouter> br in MyArea.AreaRouter)
            {
                if (null == br.Value) continue;
                br.Value.ClearAllPlace();
            }
        }
        /// <summary>
        /// 清除指定区域Tag上次的位置
        /// </summary>
        /// <param name="MyArea"></param>
        public static void ClearAreaAllTagOldRouter(Area MyArea)
        {
            if (null == MyArea)
                return;
            if (null == MyArea.AreaRouter)
                return;
            foreach (KeyValuePair<string, BasicRouter> br in MyArea.AreaRouter)
            {
                if (null == br.Value)
                {
                    continue;
                }
                try
                {
                    foreach (KeyValuePair<string, TagPack> tp in CommonCollection.TagPacks)
                    {
                        if (null == tp.Value)
                        {
                            continue;
                        }
                        if (tp.Value.RD_New[0] == br.Value.ID[0] && tp.Value.RD_New[1] == br.Value.ID[1])
                        {
                            tp.Value.RD_Old[0] = 0;
                            tp.Value.RD_Old[1] = 0;
                        }
                    }
                }catch(Exception)
                {
                }
            }
        }

        public static void DrawTag(Bitmap MyBitMap)
        {
            Graphics g = Graphics.FromImage(MyBitMap);
            Font MyFont = new Font("宋体", 10, FontStyle.Regular);
            Font TagIDNameFont = new Font("宋体", 8, FontStyle.Regular);
            Brush MyBrush = null;
            if (CommonBoxOperation.GetTagStatus(StrTagID) == 1)
            {
                MyBrush = Brushes.Red;
            }
            else if (CommonBoxOperation.GetTagStatus(StrTagID) == 0)
            {
                MyBrush = Brushes.Green;
            }
            else
            {
                return;
            }
            //根据Router的ID获取Router
            BasicRouter br = CommonBoxOperation.GetRouter(StrRouterID);
            if (null == br)
            {
                return;
            }
            if (null == StrTagID || "".Equals(StrTagID))
            {
                return;
            }
            String StrTagIDName = "";
            //获取指定ID的Tag的名称
            Tag MyTagName = CommonBoxOperation.GetTag(StrTagID);
            if (null != MyTagName)
            {
                if (null == MyTagName.Name || "".Equals(MyTagName.Name))
                {
                    StrTagIDName = StrTagID;
                }
                else
                {
                    StrTagIDName = MyTagName.Name + "(" + StrTagID + ")";
                }
            }
            else
            {
                StrTagIDName = StrTagID;
            }
            //根据获取指定的Router周围参考点的数量
            Num = CommonBoxOperation.GetRouterAroundNum(StrTagID, StrRouterID);
            //确定当前Tag是参考点上的第几个Tag
            switch (Num)
            {
                case 0:
                    g.FillEllipse(MyBrush, br.x - 8, br.y - 32, ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                    g.DrawString(StrTagIDName, TagIDNameFont, MyBrush, br.x - 5, br.y - 42);
                    break;
                case 1:
                    g.FillEllipse(MyBrush, br.x + 24, br.y - 8, ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                    g.DrawString(StrTagIDName, TagIDNameFont, MyBrush, br.x + 40, br.y - 15);
                    break;
                case 2:
                    g.FillEllipse(MyBrush, br.x - 8, br.y + 16, ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                    g.DrawString(StrTagIDName, TagIDNameFont, MyBrush, br.x - 5, br.y + 33);
                    break;
                case 3:
                    g.FillEllipse(MyBrush, br.x - 40, br.y - 8, ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                    g.DrawString(StrTagIDName, TagIDNameFont, MyBrush, br.x - 40, br.y - 18);
                    break;
                case 4:
                    g.FillEllipse(MyBrush, br.x + 24, br.y - 32, ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                    g.DrawString(StrTagIDName, TagIDNameFont, MyBrush, br.x + 35, br.y - 40);
                    break;
                case 5:
                    g.FillEllipse(MyBrush, br.x + 24, br.y + 16, ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                    g.DrawString(StrTagIDName, TagIDNameFont, MyBrush, br.x + 40, br.y + 25);
                    break;
                case 6:
                    g.FillEllipse(MyBrush, br.x - 40, br.y + 16, ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                    g.DrawString(StrTagIDName, TagIDNameFont, MyBrush, br.x - 40, br.y + 8);
                    break;
                case 7:
                    g.FillEllipse(MyBrush, br.x - 40, br.y - 32, ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                    g.DrawString(StrTagIDName, TagIDNameFont, MyBrush, br.x - 40, br.y - 42);
                    break;
                default:
                    MyBrush = Brushes.White;
                    g.FillEllipse(MyBrush, br.x - 8, br.y - 8, ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                    g.DrawString((Num - 7).ToString(), MyFont, Brushes.Black, br.x - 5, br.y - 5);
                    break;
            }
            Num++;
        }
        //右侧画3个Tag位置，考虑面板的放大
        public static void DrawTag3_Place(Bitmap MyBitMap,float wscale,float hscale)
        {
            if (null == MyBitMap)
            {
                return;
            }
            if (null == CurImageTag)
            {
                return;
            }
            Graphics g = Graphics.FromImage(MyBitMap);
            Font MyFont = new Font("宋体", 10, FontStyle.Regular);
            Font TagIDNameFont = new Font("宋体", 8, FontStyle.Regular);
            Brush MyBrush = null;
            //判断TagPack是否是警告数据包
            if (CurImageTag.ResTime > 60)
            {
                MyBrush = Brushes.Black;
            }
            else
            {
                MyBrush = Brushes.Green;
            }
            if (CurImageTag.isAlarm == 0x04)
            {
                MyBrush = Brushes.Red;
            }
            String StrTagID, StrTagName, StrTagIDName;
            StrTagID = CurImageTag.TD[0].ToString("X2") + CurImageTag.TD[1].ToString("X2");
            StrTagName = CommonBoxOperation.GetTagName(StrTagID);
            //获取指定ID的Tag的名称显示
            if (null != StrTagName)
            {
                if (!"".Equals(StrTagName))
                {
                    StrTagIDName = StrTagName + "(" + StrTagID + ")";
                }
                else
                {
                    StrTagIDName = StrTagID;
                }
            }
            else
            {
                StrTagIDName = StrTagID;
            }
            //需要把Tag画到哪一个参考点附近
            CurBasic = CommonBoxOperation.GetRouter(CurImageTag.RD_New[0].ToString("X2") + CurImageTag.RD_New[1].ToString("X2"));
            if (null == CurBasic)
            {
                return;
            }
            //得到Tag应该画到什么位置
            ReferAroundPosition MyRouterAroundPlace = CurBasic.GetOkPlace(CurImageTag.CurPlace);
            if (MyRouterAroundPlace == ReferAroundPosition.UnKnown)
            {
                return;
            }
            CurImageTag.CurPlace = MyRouterAroundPlace;
            switch (MyRouterAroundPlace)
            {
                case ReferAroundPosition.FirstPosition:
                    CurBasic.StandPlace(ReferAroundPosition.FirstPosition);
                    g.FillEllipse(MyBrush, ((float)CurBasic.x * wscale + 30), ((float)CurBasic.y * hscale - 45 + ConstInfor.TagR * 2), ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                    g.DrawString(StrTagIDName, TagIDNameFont, MyBrush, ((float)CurBasic.x * wscale + 32 + ConstInfor.TagR * 2), ((float)CurBasic.y * hscale - 41 + ConstInfor.TagR * 2));
                    break;
                case ReferAroundPosition.SecondPosition:
                    CurBasic.StandPlace(ReferAroundPosition.SecondPosition);
                    g.FillEllipse(MyBrush, ((float)CurBasic.x * wscale + 30), ((float)CurBasic.y * hscale - ConstInfor.TagR - 2 ), ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                    g.DrawString(StrTagIDName, TagIDNameFont, MyBrush, ((float)CurBasic.x * wscale + 32 + ConstInfor.TagR * 2), ((float)CurBasic.y * hscale - ConstInfor.TagR + 2));
                    break;
                case ReferAroundPosition.ThirdPosition:
                    CurBasic.StandPlace(ReferAroundPosition.ThirdPosition);
                    g.FillEllipse(MyBrush, ((float)CurBasic.x * wscale + 30), ((float)CurBasic.y * hscale - ConstInfor.TagR + 17), ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                    g.DrawString(StrTagIDName, TagIDNameFont, MyBrush, ((float)CurBasic.x * wscale + 32 + ConstInfor.TagR * 2), ((float)CurBasic.y * hscale - ConstInfor.TagR + 3  + 17));
                    break;
                case ReferAroundPosition.CenterPosition:
                    CurBasic.StandPlace(ReferAroundPosition.CenterPosition);
                    if (Num <= 3) return;
                    Brush TagColor, StrColor;
                    if (CurImageTag.isAlarm == 0x04)
                    {
                        TagColor = Brushes.Red;
                        StrColor = Brushes.White;
                    }
                    else
                    {
                        TagColor = Brushes.White;
                        StrColor = Brushes.Black;
                    }
                    if (Num - 3 < 10)
                    {
                        g.FillEllipse(TagColor, ((float)CurBasic.x * wscale - 8), (float)((float)CurBasic.y * hscale - 8), ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                        g.DrawString((Num - 3).ToString(), MyFont, StrColor, ((float)CurBasic.x * wscale - 5), (float)((float)CurBasic.y * hscale - 5));
                    }
                    else if (Num - 3 < 100)
                    {
                        g.FillEllipse(TagColor, ((float)CurBasic.x * wscale - 8), (float)((float)CurBasic.y * hscale - 8), ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                        g.DrawString((Num - 3).ToString(), MyFont, StrColor, ((float)CurBasic.x * wscale - 8), (float)((float)CurBasic.y * hscale - 5));
                    }
                    else
                    {
                        g.FillRectangle(TagColor, ((float)CurBasic.x * wscale - 10), (float)((float)CurBasic.y * hscale - 8), (ConstInfor.TagR +2) * 2,ConstInfor.TagR * 2);
                        g.DrawString((Num - 3).ToString(), new Font("宋体", 9, FontStyle.Regular), StrColor, ((float)CurBasic.x * wscale - 12), (float)((float)CurBasic.y * hscale - 5));
                    }
                    break;
            }
            CurImageTag.RD_Old[0] = CurImageTag.RD_New[0]; 
            CurImageTag.RD_Old[1] = CurImageTag.RD_New[1];
        }
        //右侧画五个Tag位置，考虑面板的放大
        public static void DrawTag_Place(Bitmap MyBitMap,float scale)
        {
            if (null == MyBitMap)
                return;
            if (null == CurImageTag)
                return;
            Graphics g = Graphics.FromImage(MyBitMap);
            Font MyFont = new Font("宋体", 10, FontStyle.Regular);
            Font TagIDNameFont = new Font("宋体", 8, FontStyle.Regular);
            Brush MyBrush = null;
            //判断TagPack是否是警告数据包
            if (CurImageTag.isAlarm == 0x05) MyBrush = Brushes.Green;
            else MyBrush = Brushes.Red;
            String StrTagID, StrTagName, StrTagIDName;
            StrTagID = CurImageTag.TD[0].ToString("X2") + CurImageTag.TD[1].ToString("X2");
            StrTagName = CommonBoxOperation.GetTagName(StrTagID);
            //获取指定ID的Tag的名称显示
            if (null != StrTagName)
            {
                if (!"".Equals(StrTagName)) StrTagIDName = StrTagName + "(" + StrTagID + ")";
                else StrTagIDName = StrTagID;
            }
            else
            {
                StrTagIDName = StrTagID;
            }
            //需要把Tag画到CurBasic周围
            CurBasic = CommonBoxOperation.GetRouter(CurImageTag.RD_New[0].ToString("X2") + CurImageTag.RD_New[1].ToString("X2"));
            if (null == CurBasic)
            {//没有设置参考点
                string msg = DateTime.Now.ToString() + " (" + CurImageTag.RD_New[0].ToString("X2") + CurImageTag.RD_New[1].ToString("X2") + ")参考点你还没有设置!";
                FileOperation.WriteLog(msg);
                return;
            }
            //得到Tag应该画到什么位置
            ReferAroundPosition MyRouterAroundPlace = GetTagOkPlace();
            //清楚掉Router当前Tag占的位置
            Num = CommonBoxOperation.GetRouterAroundNum(CurImageTag.RD_New[0].ToString("X2") + CurImageTag.RD_New[1].ToString("X2"));
            //先画出Tag的实际位置
            CurImageTag.CurPlace = MyRouterAroundPlace;

            switch (MyRouterAroundPlace)
            {
                case ReferAroundPosition.FirstPosition:
                    CurBasic.StandPlace(ReferAroundPosition.FirstPosition);
                    g.FillEllipse(MyBrush, (CurBasic.x + 15) * scale, (CurBasic.y - 32) * scale, ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                    g.DrawString(StrTagIDName, TagIDNameFont, MyBrush, (CurBasic.x + 10 + ConstInfor.TagR*2) * scale, (CurBasic.y - 30) * scale);
                    break;
                case ReferAroundPosition.SecondPosition:
                    CurBasic.StandPlace(ReferAroundPosition.SecondPosition);
                    g.FillEllipse(MyBrush, (CurBasic.x + 15) * scale, (CurBasic.y - 35 + ConstInfor.TagR * 2) * scale, ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                    g.DrawString(StrTagIDName, TagIDNameFont, MyBrush, (CurBasic.x + 10 + ConstInfor.TagR * 2) *scale, (CurBasic.y - 33 + ConstInfor.TagR * 2) * scale);
                    break;
                case ReferAroundPosition.ThirdPosition:
                    CurBasic.StandPlace(ReferAroundPosition.ThirdPosition);
                    g.FillEllipse(MyBrush, (CurBasic.x + 15) * scale, (CurBasic.y - 30 + ConstInfor.TagR * 3) * scale, ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                    g.DrawString(StrTagIDName, TagIDNameFont, MyBrush, (CurBasic.x + 10 + ConstInfor.TagR * 2) *scale, (CurBasic.y - 28 + ConstInfor.TagR * 3) * scale);
                    break;
                case ReferAroundPosition.CenterPosition:
                    CurBasic.StandPlace(ReferAroundPosition.CenterPosition);
                    if (Num <= 5)return;
                    MyBrush = Brushes.White;
                    g.FillEllipse(MyBrush, (CurBasic.x - 5) * scale, (float)(CurBasic.y - 5.2) * scale, ConstInfor.TagR * 2, ConstInfor.TagR * 2);
                    if (Num - 5 < 10)
                        g.DrawString((Num - 5).ToString(), MyFont, Brushes.Black, (CurBasic.x - 3) * scale, (float)(CurBasic.y - 4.2) * scale);
                    else
                        g.DrawString((Num - 5).ToString(), MyFont, Brushes.Black, (CurBasic.x - 5) * scale, (float)(CurBasic.y - 3.2) * scale);
                    break;
            }
            CurImageTag.RD_Old[0] = CurImageTag.RD_New[0]; CurImageTag.RD_Old[1] = CurImageTag.RD_New[1];;
        }
        public static void DrawTag_Place(Bitmap MyBitMap)
        {
            if (null == MyBitMap)
            {
                return;
            }
            if (null == CurImageTag)
            {
                return;
            }
            Graphics g = Graphics.FromImage(MyBitMap);

            Font MyFont = new Font("宋体", 10, FontStyle.Regular);
            Font TagIDNameFont = new Font("宋体", 8, FontStyle.Regular);

            Brush MyBrush = null;
            //判断TagPack是否是警告数据包
            if (CurImageTag.isAlarm == 0x05)
            {
                MyBrush = Brushes.Green;
            }
            else
            {
                MyBrush = Brushes.Red;
            }
            String StrTagID, StrTagName, StrTagIDName;
            StrTagID = CurImageTag.TD[0].ToString("X2") + CurImageTag.TD[1].ToString("X2");
            StrTagName = CommonBoxOperation.GetTagName(StrTagID);
            //获取指定ID的Tag的名称显示
            if (null != StrTagName)
            {
                if (!"".Equals(StrTagName))
                {
                    StrTagIDName = StrTagName + "(" + StrTagID + ")";
                }
                else
                {
                    StrTagIDName = StrTagID;
                }
            }
            else StrTagIDName = StrTagID;
            //需要把Tag画到CurBasic周围
            CurBasic = CommonBoxOperation.GetRouter(CurImageTag.RD_New[0].ToString("X2") + CurImageTag.RD_New[1].ToString("X2"));
            if (null == CurBasic)
            {//没有设置参考点
                string msg = DateTime.Now.ToString() + " (" + CurImageTag.RD_New[0].ToString("X2") + CurImageTag.RD_New[1].ToString("X2") + ")参考点你还没有设置!";
                FileOperation.WriteLog(msg);
                return;
            }
            //得到Tag应该画到什么位置
            ReferAroundPosition MyRouterAroundPlace = GetTagOkPlace();
            //清楚掉Router当前Tag占的位置
            Num = CommonBoxOperation.GetRouterAroundNum(CurImageTag.RD_New[0].ToString("X2") + CurImageTag.RD_New[1].ToString("X2"));
            //先画出Tag的实际位置
            CurImageTag.CurPlace = MyRouterAroundPlace;
            switch(MyRouterAroundPlace)
            {
                case ReferAroundPosition.FirstPosition:
                    CurBasic.StandPlace(ReferAroundPosition.FirstPosition);
                    break;
                case ReferAroundPosition.SecondPosition:
                    CurBasic.StandPlace(ReferAroundPosition.SecondPosition);
                    break;
                case ReferAroundPosition.ThirdPosition:
                    CurBasic.StandPlace(ReferAroundPosition.ThirdPosition);
                    break;
                case ReferAroundPosition.CenterPosition:
                    CurBasic.StandPlace(ReferAroundPosition.CenterPosition);
                    break;
            }
            CurImageTag.RD_Old[0] = CurImageTag.RD_New[0];CurImageTag.RD_Old[1] = CurImageTag.RD_New[1];
        }
        public static ReferAroundPosition GetTagOkPlace()
        {
            if(null == CurImageTag)return ReferAroundPosition.UnKnown;
            BasicRouter OldBasicRefer = CommonBoxOperation.GetRouter(CurImageTag.RD_Old[0].ToString("X2") + CurImageTag.RD_Old[1].ToString("X2"));
            //说明连续两次Tag都在这个参考点附近，那么此时Tag的位置不应该移动
            if (CurImageTag.RD_New[0] == CurImageTag.RD_Old[0] && CurImageTag.RD_New[1] == CurImageTag.RD_Old[1])
            {  
                //当前的Tag是否有位置站（修改）
                if (CurImageTag.CurPlace == ReferAroundPosition.UnKnown)
                {//说明此时没有占用任何位置 
                    return CurBasic.GetOkPlace(ReferAroundPosition.FirstPosition);
                }
                else
                { //说明此时位置已经被占用了
                    if (OldBasicRefer.GetStandStatus(CurImageTag.CurPlace))
                    {
                        return CurBasic.GetOkPlace(ReferAroundPosition.FirstPosition);
                    }else return CurImageTag.CurPlace;
                }
            }
            //两次参考点发生改变,清除掉原来参考点占用的位置
            if (null != OldBasicRefer) 
                OldBasicRefer.ClearPlaceStand(CurImageTag.CurPlace);
            return CurBasic.GetOkPlace(ReferAroundPosition.FirstPosition);
        }
    }
    /// <summary>
    /// 主窗口画图
    /// </summary>
    public class DrawIMG
    {
        public static Thread SigThread = null;
        public static Form1 Frm;
        public static void StartSig()
        {
            Stop();
            SigThread = new Thread(SigThreadFun);
            SigThread.Start();
        }
        /// <summary>
        /// 停止信号线程
        /// </summary>
        public static void Stop()
        {
            if (SigThread != null)
            {
                if (SigThread.IsAlive)
                {
                    SigThread.Abort();
                    SigThread = null;
                }
            }
        }
        /// <summary>
        /// 画出PC与Tag的图片
        /// </summary>
        /// <param name="g"></param>
        /// <param name="CurStatus"></param>
        public static void DrawPC_Tag(Graphics g, BtnStatus CurStatus)
        {
            switch (CurStatus)
            {
                case BtnStatus.Bt_start_No_Press:
                case BtnStatus.Bt_start_Press:
                    g.DrawImageUnscaled(new Bitmap(Properties.Resources.speed_disconnect), ConstInfor.Tag_PC_left, ConstInfor.Tag_PC_top);
                    break;
                case BtnStatus.Bt_stop_No_Press:
                case BtnStatus.Bt_stop_Press:
                    g.DrawImageUnscaled(new Bitmap(Properties.Resources.speed_connect), ConstInfor.Tag_PC_left, ConstInfor.Tag_PC_top);
                    break;
            }
        }
        /// <summary>
        /// 主窗口中连接按键按下的情况
        /// </summary>
        public static void Paint_CnnBtn(Graphics g, BtnStatus CurStatus)
        {
            switch (CurStatus)
            {
                case BtnStatus.Bt_start_No_Press:
                    g.DrawImageUnscaled(new Bitmap(Properties.Resources.start_up_603_30), ConstInfor.BgCnnBtn_left, ConstInfor.BgCnnBtn_top);
                    break;
                case BtnStatus.Bt_start_Press:
                    g.DrawImageUnscaled(new Bitmap(Properties.Resources.start_down_603_30), ConstInfor.BgCnnBtn_left, ConstInfor.BgCnnBtn_top);
                    break;
                case BtnStatus.Bt_stop_No_Press:
                    g.DrawImageUnscaled(new Bitmap(Properties.Resources.stop_up_603_30), ConstInfor.BgCnnBtn_left, ConstInfor.BgCnnBtn_top);
                    break;
                case BtnStatus.Bt_stop_Press:
                    g.DrawImageUnscaled(new Bitmap(Properties.Resources.stop_down_603_30), ConstInfor.BgCnnBtn_left, ConstInfor.BgCnnBtn_top);
                    break;
            }
        }
        /// <summary>
        /// 主窗口中设置按键按下的情况
        /// </summary>
        /// <param name="g"></param>
        /// <param name="CurStatus"></param>
        public static void Paint_SetBtn(Graphics g, BtnStatus CurStatus)
        {
            //开始连接监控按键按下时，设置按键变成灰色表示不可点击
            if (Frm.cnn_btn_status == BtnStatus.Bt_stop_No_Press)
                CurStatus = BtnStatus.Bt_start_Press;
            switch (CurStatus)
            {
                case BtnStatus.Bt_start_No_Press:
                    g.DrawImageUnscaled(new Bitmap(Properties.Resources.cd_2), ConstInfor.BgSetBtn_left, ConstInfor.BgSetBtn_top);
                    break;
                case BtnStatus.Bt_start_Press:
                    g.DrawImageUnscaled(new Bitmap(Properties.Resources.cd_1), ConstInfor.BgSetBtn_left, ConstInfor.BgSetBtn_top);
                    break;
            }
        }
        /// <summary>
        /// 主窗口中查询按键按下的情况
        /// </summary>
        /// <param name="g"></param>
        /// <param name="CurStatus"></param>
        public static void Paint_SelectBtn(Graphics g, BtnStatus CurStatus)
        {
            switch (CurStatus)
            {
                case BtnStatus.Bt_start_No_Press:
                    g.DrawImageUnscaled(new Bitmap(Properties.Resources.sel_up_01), ConstInfor.SelecBtn_left, ConstInfor.SelectBtn_top);
                    break;
                case BtnStatus.Bt_start_Press:
                    g.DrawImageUnscaled(new Bitmap(Properties.Resources.sel_down_01), ConstInfor.SelecBtn_left, ConstInfor.SelectBtn_top);
                    break;
            }
        }
        /// <summary>
        /// 信号动画效果的线程的执行方法
        /// </summary>
        public static void SigThreadFun()
        {
            Frm.Invoke(new Action(() =>
            {
                Frm.SigP1.Visible = true;
                Frm.SigP2.Visible = true;
            }));
            Bitmap MainBitMap = new Bitmap(126, 8);
            Graphics g = Graphics.FromImage(MainBitMap);
            Bitmap bitmap = Properties.Resources.speed_left;
            bitmap = new Bitmap(bitmap, 8, 8);
            int i = 0;
            int x = 0;
            Random MyRandom = new Random();
            double num = 0.0;
            Rectangle Rec = new Rectangle(0, 0, 13, 40);
            Brush MyBrush = new SolidBrush(Color.BlueViolet);
            while (Frm.MyUdpClient != null)
            {
                Thread.Sleep(100);
                if (i % 2 == 0)
                {
                    bitmap = Properties.Resources.speed_left;
                }
                else
                {
                    bitmap = Properties.Resources.speed_right;
                }
                bitmap = new Bitmap(bitmap, 8, 8);
                g.DrawImageUnscaled(bitmap, x, 0);
                try
                {
                    Frm.Sig_panel.CreateGraphics().DrawImageUnscaled(MainBitMap, 0, 0);
                    g.Clear(Color.White);
                }
                catch (Exception)
                {

                }
                x += ConstInfor.Sign_Speed;
                if (x >= 126)
                {
                    x = 0;
                    num = MyRandom.NextDouble();
                    if (num > 0.8)
                    {
                        try
                        {
                            Rec.Height = Convert.ToInt32((Frm.SigP1.Height / 2) * 1);
                        }
                        catch (Exception)
                        {
                            Rec.Height = 0;
                        }
                    }
                    else if (num > 0.6)
                    {
                        try
                        {
                            Rec.Height = Convert.ToInt32((Frm.SigP1.Height / 2) * 0.8);
                        }
                        catch (Exception)
                        {
                            Rec.Height = 0;
                        }
                    }
                    else if (num > 0.4)
                    {
                        try
                        {
                            Rec.Height = Convert.ToInt32((Frm.SigP1.Height / 2) * 0.6);
                        }
                        catch (Exception)
                        {
                            Rec.Height = 0;
                        }
                    }
                    else if (num > 0.2)
                    {
                        try
                        {
                            Rec.Height = Convert.ToInt32((Frm.SigP1.Height / 2) * 0.4);
                        }
                        catch (Exception)
                        {
                            Rec.Height = 0;
                        }
                    }
                    else
                    {
                        try
                        {
                            Rec.Height = Convert.ToInt32((Frm.SigP1.Height / 2) * 0.2);
                        }
                        catch (Exception)
                        {
                            Rec.Height = 0;
                        }
                    }
                    Rec.Y = Frm.SigP1.Height - Rec.Height;
                    try
                    {
                        Frm.SigP1.CreateGraphics().Clear(Color.FromArgb(235, 235, 235));
                        Frm.SigP1.CreateGraphics().FillRectangle(MyBrush, Rec); ;
                    }
                    catch (Exception)
                    {

                    }
                }
                i++;
            }
            Frm.Invoke(new Action(() =>
            {
                Frm.Sig_panel.CreateGraphics().Clear(Color.White);
                Frm.SigP1.CreateGraphics().Clear(Color.FromArgb(235, 235, 235));
                Frm.SigP2.CreateGraphics().Clear(Color.FromArgb(235, 235, 235));
                Frm.SigP1.Visible = true;
                Frm.SigP2.Visible = true;
            }));
        }
        public static void DrawMainCenter(Graphics g)
        {
            //画出绿色横杠
            int MainCenter_left = (Frm.MainCenter_Panel.Width - ConstInfor.MainCenter_Width) / 2;
            Bitmap B_1 = Properties.Resources.B_1;
            g.DrawImage(B_1, MainCenter_left + ConstInfor.Table_Left - 2, 0, ConstInfor.MainCenter_Width - ConstInfor.Table_Left * 2, B_1.Height);
            Bitmap B_2 = Properties.Resources.B_2;
            g.DrawImage(B_2, MainCenter_left + ConstInfor.Table_Left - 2, ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom, ConstInfor.MainCenter_Width - ConstInfor.Table_Left * 2, B_1.Height);
            //画出表格
            BrushPersonTB(g);
            DrawPersonTable(g);
            BrushAlarmTB(g);
            DrawAlarmTable(g);
            DrawGroup(g);

            DrawPersonStr(g);
            DrawAlarmStr(g);
            //画出警告的人数
            DrawAlarmNum(g, CommonBoxOperation.GetAlarmTagsNum(SpeceilAlarm.PersonHelp),
                CommonBoxOperation.GetAlarmTagsNum(SpeceilAlarm.AreaControl), 
                CommonBoxOperation.GetAlarmTagsNum(SpeceilAlarm.Resid), 
                CommonBoxOperation.GetAlarmTagsNum(SpeceilAlarm.BatteryLow) + 
                CommonBoxOperation.GetAlarmTagsNum(SpeceilAlarm.NodeDis) +
                CommonBoxOperation.GetAlarmTagsNum(SpeceilAlarm.TagDis) + 
                CommonBoxOperation.GetAlarmTagsNum(SpeceilAlarm.ReferDis));
            DrawPerson(g);
        }
        /// <summary>
        /// 画出组别的名称
        /// </summary>
        public static void DrawGroup(Graphics g)
        {
            Font MyFont = new Font("黑体", 17);
            Brush FontBrushNo = new SolidBrush(Color.Gray);
            Brush FontBrushSelected = new SolidBrush(Color.FromArgb(131, 131, 131));

            Brush BgBrush = new SolidBrush(Color.FromArgb(247, 247, 247));
            int CommCellWidth = (ConstInfor.MainCenter_Width - ConstInfor.PsCell_First_Width - ConstInfor.Table_Left * 2) / ConstInfor.PsCell_RowNum;
            int CommCelleHeight = (ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom - ConstInfor.Cross_Ban_H - 5) / (ConstInfor.PsCell_ColNum);
            Rectangle MyRectangle = new Rectangle(ConstInfor.PsCell_First_Width + ConstInfor.Table_Left + 1, ConstInfor.Cross_Ban_H, CommCellWidth - 1, CommCelleHeight);
            string StrName = "";
            Brush CurBrush = null;
            //初始化3个显示的组
            InitShowGroups();
            for (int i = 0; i < SysParam.GroupShows.Length; i++)
            {
                if (null == SysParam.GroupShows[i])
                {
                    StrName = ConstInfor.NoGroup;
                    CurBrush = FontBrushNo;
                }
                else
                {
                    if ("".Equals(SysParam.GroupShows[i].Name))
                    {
                        StrName = SysParam.GroupShows[i].ID[0].ToString("X2") + SysParam.GroupShows[i].ID[1].ToString("X2");
                    }
                    else
                    {
                        StrName = SysParam.GroupShows[i].Name;
                    }
                    if (SysParam.GroupShows[i].Selected)
                    {
                        CurBrush = FontBrushSelected;
                        MyRectangle.X += i * CommCellWidth;
                        g.FillRectangle(BgBrush, MyRectangle);
                    }
                    else
                    {
                        CurBrush = FontBrushNo;
                    }
                }
                //判断字符串的长度，从而设置其名称
                if (DrawIMG.GetLength(StrName) > 14)
                {
                    MessageBox.Show("區域名稱絕對長度不能大于14，其中一個漢字長度為2，默認取前14个字符，若第14个为汉字则取前13个字符!");
                    StrName = DrawIMG.Get14Char(StrName);
                }
                if (!ConstInfor.NoGroup.Equals(StrName))
                {
                    int len = GetLength(StrName);
                    g.DrawString(StrName, MyFont, CurBrush, CommCellWidth * i + ConstInfor.PsCell_First_Width + CommCellWidth / 2 - (float)(len*4.8), ConstInfor.Table_Top + 5);
                }
            }
        }
        /// <summary>
        /// 初始化GroupShows数组中的项
        /// </summary>
        /// <returns>
        ///     -1：表示GroupShows数组中的项全为空
        ///     >=0：选中的项
        /// </returns>
        public static int InitShowGroups()
        {
            //判断是否全为空
            if (IsAllNullGroups())
                return -1;
            int index = GetGroupsSelectedIndex();
            if (index < 0)
            {
                //说明没有选中的项，默认选择第一项,若第一项为空，则选择第二项，一次累推
                for (int i = 0; i < SysParam.GroupShows.Length; i++)
                {
                    if (null != SysParam.GroupShows[i])
                    {
                        SysParam.GroupShows[i].Selected = true;
                        return i;
                    }
                }
            }
            return index;
        }
        /// <summary>
        /// 将Groups中的项加载到ShowGroups中去
        /// </summary>
        /// <returns></returns>
        public static int LoadShowGroups()
        {
            lock (CommonCollection.Groups_Lock)
            {
                if (CommonCollection.Groups.Count <= 0)
                    return -1;
                if (IsAllNullGroups())
                {
                    int index = 0;
                    foreach (KeyValuePair<string, Group> group in CommonCollection.Groups)
                    {
                        if (null != group.Value)
                        {
                            if (index > 2)
                                break;
                            GroupShow MyGroupShow = new GroupShow();
                            MyGroupShow.ID = group.Value.ID;
                            MyGroupShow.Name = group.Value.Name;
                            SysParam.GroupShows[index] = MyGroupShow;
                            index++;
                        }
                    }
                    for (int i = 0; i < SysParam.GroupShows.Length; i++)
                    {
                        if (null != SysParam.GroupShows[i])
                        {
                            SysParam.GroupShows[0].Selected = true;
                            break;
                        }
                    }
                    if (index <= 0)
                        return -1;
                    else
                        return index;
                }
                //若存在不为空的项，判断不为空的项是否存在
            }
            return 0;
        }
        /// <summary>
        /// 将ShowGroup中的index项设置为选定，其他设置为未选定状态
        /// </summary>
        /// <param name="index"></param>
        /// <returns>-1:全空;-2:选择项为空;==index：设置成功</returns>
        public static int SetShowGroupSelected(int index)
        {
            if (IsAllNullGroups())
                return -1;
            for (int i = 0; i < SysParam.GroupShows.Length; i++)
            {
                if (null != SysParam.GroupShows[i])
                    SysParam.GroupShows[i].Selected = false;
            }
            if (null != SysParam.GroupShows[index])
            {
                SysParam.GroupShows[index].Selected = true;
                return index;
            }
            return -2;
        }

        /// <summary>
        /// 判断GroupShows数组中的项是否全为空
        /// </summary>
        /// <returns></returns>
        public static bool IsAllNullGroups()
        {
            int flag = 0;
            for (int i = 0; i < SysParam.GroupShows.Length; i++)
            {
                if (null == SysParam.GroupShows[i])
                    flag++;
                else
                    break;
            }
            if (flag == SysParam.GroupShows.Length)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断得到GroupShows数组中选中的项
        /// </summary>
        /// <returns>
        ///     >=0：表示选中的项
        ///     -1：表示没有选中项
        /// </returns>
        public static int GetGroupsSelectedIndex()
        {
            for (int i = 0; i < SysParam.GroupShows.Length; i++)
            {
                if (null != SysParam.GroupShows[i])
                {
                    if (SysParam.GroupShows[i].Selected)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        /// <summary>
        /// 获取选定的组信息
        /// </summary>
        /// <returns>-1：没有选定的组信息</returns>
        public static int GetSelectedGroup()
        {
            if (IsAllNullGroups())
                return -1;
            int index = 0;
            for (index = 0; index < SysParam.GroupShows.Length; index++)
            {
                if (null != SysParam.GroupShows[index])
                {
                    if (SysParam.GroupShows[index].Selected)
                    {
                        return index;
                    }
                }
            }
            return index;
        }

        public static void SetCurAreasClear()
        {
            for (int i = 0; i < SysParam.CurAreas.Length; i++)
            {
                SysParam.CurAreas[i] = null;
            }
        }
        /// <summary>
        /// 画出人员表格的信息
        /// </summary>
        /// <param name="g"></param>
        public static void DrawPersonTable(Graphics g)
        {
            //线的Pen
            Pen LinePen = new Pen(Color.FromArgb(211, 211, 211), 1);
            //得到普通表格的宽度和高度
            int CommCellWidth = (ConstInfor.MainCenter_Width - ConstInfor.PsCell_First_Width - ConstInfor.Table_Left * 2) / ConstInfor.PsCell_RowNum;
            int CommCelleHeight = (ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom - ConstInfor.Cross_Ban_H - 5) / (ConstInfor.PsCell_ColNum);
            Point[] Points = new Point[2];
            //画竖线
            for (int i = 0; i <= ConstInfor.PsCell_RowNum + 1; i++)
            {
                if (0 == i)
                {
                    Points[0].X = ConstInfor.Table_Left;
                    Points[0].Y = ConstInfor.Table_Top;
                    Points[1].X = ConstInfor.Table_Left;
                    Points[1].Y = ConstInfor.PsCell_ColNum * CommCelleHeight + ConstInfor.Table_Top + CommCelleHeight;
                }
                else if (1 == i)
                {
                    Points[0].X = ConstInfor.Table_Left + ConstInfor.PsCell_First_Width;
                    Points[0].Y = ConstInfor.Table_Top;
                    Points[1].X = ConstInfor.Table_Left + ConstInfor.PsCell_First_Width; ;
                    Points[1].Y = (ConstInfor.PsCell_ColNum - 1) * CommCelleHeight + ConstInfor.Table_Top + CommCelleHeight; ;
                }
                else if (ConstInfor.PsCell_RowNum + 1 == i)
                {
                    Points[0].X = ConstInfor.Table_Left + ConstInfor.PsCell_First_Width + ConstInfor.PsCell_RowNum * CommCellWidth - 1;
                    Points[0].Y = ConstInfor.Table_Top;
                    Points[1].X = ConstInfor.Table_Left + ConstInfor.PsCell_First_Width + ConstInfor.PsCell_RowNum * CommCellWidth - 1;
                    Points[1].Y = (ConstInfor.PsCell_ColNum - 1) * CommCelleHeight + ConstInfor.Table_Top + CommCelleHeight;
                }
                else
                {
                    Points[0].X = ConstInfor.Table_Left + ConstInfor.PsCell_First_Width + (i - 1) * CommCellWidth;
                    Points[0].Y = ConstInfor.Table_Top;
                    Points[1].X = ConstInfor.Table_Left + ConstInfor.PsCell_First_Width + (i - 1) * CommCellWidth;
                    Points[1].Y = (ConstInfor.PsCell_ColNum - 1) * CommCelleHeight + ConstInfor.Table_Top;
                }
                g.DrawLine(LinePen, Points[0], Points[1]);
            }
            //画横线
            for (int i = 0; i <= ConstInfor.PsCell_ColNum; i++)
            {
                Points[0].X = ConstInfor.Table_Left;
                Points[0].Y = ConstInfor.Table_Top + i * CommCelleHeight;
                Points[1].X = ConstInfor.Table_Left + ConstInfor.PsCell_First_Width + ConstInfor.PsCell_RowNum * CommCellWidth;
                Points[1].Y = ConstInfor.Table_Top + i * CommCelleHeight;
                g.DrawLine(LinePen, Points[0], Points[1]);
            }
        }
        /// <summary>
        /// 画出警告信息表格
        /// </summary>
        /// <param name="g"></param>
        public static void DrawAlarmTable(Graphics g)
        {
            Pen LinePen = new Pen(Color.FromArgb(211, 211, 211), 1);
            //得到普通表格的宽度和高度
            int CommCellWidth = (ConstInfor.MainCenter_Width - ConstInfor.AlarmCell_First_Width - ConstInfor.Table_Left * 2) / ConstInfor.AlarmCell_RowNum;
            int CommCelleHeight = (ConstInfor.Alarm_TB_Bottom - ConstInfor.AlarmBtt - ConstInfor.Cross_Ban_H) / (ConstInfor.AlarmCell_ColNum);
            Point[] Points = new Point[2];
            for (int i = 0; i <= ConstInfor.AlarmCell_RowNum + 2; i++)
            {
                if (0 == i)
                {
                    Points[0].X = ConstInfor.Table_Left;
                    Points[0].Y = ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H;
                    Points[1].X = ConstInfor.Table_Left;
                    Points[1].Y = ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H + CommCelleHeight * ConstInfor.AlarmCell_ColNum;
                }
                else if (1 == i)
                {
                    Points[0].X = ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width;
                    Points[0].Y = ConstInfor.Cross_Ban_H + ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom;
                    Points[1].X = ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width; ;
                    Points[1].Y = ConstInfor.Cross_Ban_H + ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + CommCelleHeight * ConstInfor.AlarmCell_ColNum;
                }
                else
                {
                    Points[0].X = ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + CommCellWidth * (i - 1);
                    Points[0].Y = ConstInfor.Cross_Ban_H + ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom;
                    Points[1].X = ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + CommCellWidth * (i - 1);
                    Points[1].Y = ConstInfor.Cross_Ban_H + ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + CommCelleHeight * ConstInfor.AlarmCell_ColNum;
                }
                g.DrawLine(LinePen, Points[0], Points[1]);
            }
            for (int i = 0; i <= ConstInfor.AlarmCell_ColNum + 1; i++)
            {
                Points[0].X = ConstInfor.Table_Left;
                Points[0].Y = ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H + CommCelleHeight * i; ;
                Points[1].X = ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + ConstInfor.AlarmCell_RowNum * CommCellWidth;
                Points[1].Y = ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H + CommCelleHeight * i;
                g.DrawLine(LinePen, Points[0], Points[1]);
            }
        }
        /// <summary>
        /// 将人员表格中的内容刷灰，刷白
        /// </summary>
        /// <param name="g"></param>
        public static void BrushPersonTB(Graphics g)
        {
            int CommCelleHeight = (ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom - ConstInfor.Cross_Ban_H - 5) / (ConstInfor.PsCell_ColNum);
            Brush Brush_Gray = new SolidBrush(Color.FromArgb(247, 247, 247));//刷灰色
            Brush Brush_White = new SolidBrush(Color.White);//刷白
            for (int i = 0; i < ConstInfor.PsCell_ColNum; i++)
            {
                if (i == 0)
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(234, 234, 234)), ConstInfor.Table_Left + 1, ConstInfor.Cross_Ban_H + 1 + CommCelleHeight + CommCelleHeight * (i - 1), ConstInfor.MainCenter_Width - ConstInfor.Table_Left * 2 - 4, CommCelleHeight - 2);
                }
                else
                {
                    if (i % 2 == 0 || i == (ConstInfor.PsCell_ColNum - 1))//刷白色
                    {
                        g.FillRectangle(Brush_White, ConstInfor.Table_Left + 1, ConstInfor.Cross_Ban_H + 1 + CommCelleHeight + CommCelleHeight * (i - 1), ConstInfor.MainCenter_Width - ConstInfor.Table_Left * 2 - 4, CommCelleHeight - 2);
                    }
                    else
                    { //刷灰色
                        g.FillRectangle(Brush_Gray, ConstInfor.Table_Left + 1, ConstInfor.Cross_Ban_H + 1 + CommCelleHeight * i, ConstInfor.MainCenter_Width - ConstInfor.Table_Left * 2 - 4, CommCelleHeight - 2);
                    }
                }
            }
        }
        /// <summary>
        /// 将警报表格中的内容刷灰，刷白
        /// </summary>
        /// <param name="g"></param>
        public static void BrushAlarmTB(Graphics g)
        {
            int CommCelleHeight = (ConstInfor.Alarm_TB_Bottom - ConstInfor.AlarmBtt - ConstInfor.Cross_Ban_H) / (ConstInfor.AlarmCell_ColNum);
            Brush Brush_Gray = new SolidBrush(Color.FromArgb(247, 247, 247));//刷灰色
            Brush Brush_White = new SolidBrush(Color.White);//刷白
            g.FillRectangle(Brush_Gray, ConstInfor.Table_Left + 1, ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H + 2, ConstInfor.MainCenter_Width - ConstInfor.Table_Left * 2 - 4, CommCelleHeight - 2);
            g.FillRectangle(Brush_White, ConstInfor.Table_Left + 1, ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H + CommCelleHeight + 1, ConstInfor.MainCenter_Width - ConstInfor.Table_Left * 2 - 4, CommCelleHeight - 2);
        }
        /// <summary>
        /// 画出人员表格中的列标题
        /// </summary>
        /// <param name="g"></param>
        /// 
        public static void DrawPersonStr(Graphics g)
        {
            Font MyFont = new Font("黑体", 17);
            Brush FontBrush = new SolidBrush(Color.Black);
            int CommCelleHeight = (ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom - ConstInfor.Cross_Ban_H - 8) / (ConstInfor.PsCell_ColNum);
            int CommCellWidth = (ConstInfor.MainCenter_Width - ConstInfor.AlarmCell_First_Width - ConstInfor.Table_Left * 2) / ConstInfor.AlarmCell_RowNum;

            //判断当前的组别个数
            if (CommonBoxOperation.GetGroupCount() <= 3)
            {
                SysParam.isRightArrowDown = true;
                SysParam.isLeftArrowDown = true;
            }
            for (int i = 0; i < 10; i++)
            {
                if (i == 0)
                {
                    switch (CommonBoxOperation.isShowGroupsMargin())
                    {
                        case 0:
                            SysParam.isLeftArrowDown = true;
                            break;
                        case 1:
                            SysParam.isRightArrowDown = true;
                            break;
                        default:
                            break;
                    }
                    if (SysParam.isLeftArrowDown)
                        g.DrawImageUnscaled(new Bitmap(Properties.Resources.jtou_L_2), ConstInfor.Table_Left + 80 - 12, ConstInfor.Table_Top + 5);
                    else
                        g.DrawImageUnscaled(new Bitmap(Properties.Resources.jtou_L_1), ConstInfor.Table_Left + 80 - 12, ConstInfor.Table_Top + 5);
                    if (SysParam.isRightArrowDown)
                        g.DrawImageUnscaled(new Bitmap(Properties.Resources.jtou_R_2), ConstInfor.Table_Left + CommCellWidth * 4 + 100 - 8, ConstInfor.Table_Top + 5);
                    else
                        g.DrawImageUnscaled(new Bitmap(Properties.Resources.jtou_R_1), ConstInfor.Table_Left + CommCellWidth * 4 + 100 - 8, ConstInfor.Table_Top + 5);
                }
                else
                    if (i == 9)
                    {
                        g.DrawString(ConstInfor.StrPerTotalNum, MyFont, FontBrush, ConstInfor.Table_Left + 40, ConstInfor.Table_Top + CommCelleHeight * 9 + 5);
                        continue;
                    }
                    else if (i % 2 == 1)
                        g.DrawString(ConstInfor.StrPerDivArea, MyFont, FontBrush, ConstInfor.Table_Left + 40, ConstInfor.Table_Top + CommCelleHeight * i + 5);
                    else
                        g.DrawString(ConstInfor.StrPerNum, MyFont, FontBrush, ConstInfor.Table_Left + 62, ConstInfor.Table_Top + CommCelleHeight * i + 5);
            }
        }
        /// <summary>
        ///  画出警告表格中的列标题
        /// </summary>
        /// <param name="g"></param>
        public static void DrawAlarmStr(Graphics g)
        {
            int CommCellWidth = (ConstInfor.MainCenter_Width - ConstInfor.AlarmCell_First_Width - ConstInfor.Table_Left * 2) / ConstInfor.AlarmCell_RowNum;
            int CommCelleHeight = (ConstInfor.Alarm_TB_Bottom - ConstInfor.AlarmBtt - ConstInfor.Cross_Ban_H) / (ConstInfor.AlarmCell_ColNum);
            Font MyFont = new Font("黑体", 17);
            Brush FontBrush = new SolidBrush(Color.Black);
            g.DrawString(ConstInfor.StrAlarmInfor, MyFont, FontBrush, ConstInfor.Table_Left + 40, ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H + 8);
            g.DrawString(ConstInfor.StrInforNum, MyFont, FontBrush, ConstInfor.Table_Left + 60, ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H + CommCelleHeight + 8);
            g.DrawString(ConstInfor.StrPersonHelp, MyFont, FontBrush, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + 36, ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H + 8);
            g.DrawString(ConstInfor.StrAreaControl, MyFont, FontBrush, ConstInfor.Table_Left + CommCellWidth + ConstInfor.AlarmCell_First_Width + 36, ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H + 8);
            g.DrawString(ConstInfor.StrPersonRs, MyFont, FontBrush, ConstInfor.Table_Left + CommCellWidth * 2 + ConstInfor.AlarmCell_First_Width + 24, ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H + 8);
            g.DrawString(ConstInfor.StrAlarmMore, MyFont, FontBrush, ConstInfor.Table_Left + CommCellWidth * 3 + ConstInfor.AlarmCell_First_Width + 68, ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H + 2);
        }
        /// <summary>
        /// 画出警告讯息的数量
        /// </summary>
        /// <param name="g"></param>
        public static void DrawAlarmNum(Graphics g, int num0, int num1, int num2, int num3)
        {
            int CommCellWidth = (ConstInfor.MainCenter_Width - ConstInfor.AlarmCell_First_Width - ConstInfor.Table_Left * 2) / ConstInfor.AlarmCell_RowNum;
            int CommCelleHeight = (ConstInfor.Alarm_TB_Bottom - ConstInfor.AlarmBtt - ConstInfor.Cross_Ban_H) / (ConstInfor.AlarmCell_ColNum);
            Font MyFont = new Font("黑体", 17);
            Font MyFont1 = new Font("黑体", 17);
            Font MyFont2 = new Font("黑体", 17);
            Font MyFont3 = new Font("黑体", 17);
            Font MyFont4 = new Font("黑体", 17);
            int Place_Top = ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H + CommCelleHeight + 8;
            int Place_Top1 = ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H + CommCelleHeight + 8;
            int Place_Top2 = ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H + CommCelleHeight + 8;
            int Place_Top3 = ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H + CommCelleHeight + 8;
            int Place_Top4 = ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H + CommCelleHeight + 8;
            Brush FontBrush = new SolidBrush(Color.Red);
            if (SysParam.AlarmFontChangeInt % 2 == 0)
            {
                SysParam.AlarmFontChangeInt = 0;
                if (num0 > 0)
                {
                    if (!SysParam.isPersonHelp)
                    {
                        MyFont1 = new Font("黑体", 26);
                        Place_Top1 -= 6;
                    }
                }
                if (num1 > 0)
                {
                    if (!SysParam.isAreaControl)
                    {
                        MyFont2 = new Font("黑体", 26);
                        Place_Top2 -= 6;
                    }
                }
                if (num2 > 0)
                {
                    if (!SysParam.isEmergy)
                    {
                        MyFont3 = new Font("黑体", 26);
                        Place_Top3 -= 6;
                    }
                }
                if (num3 > 0)
                {
                    MyFont4 = new Font("黑体",26);
                    Place_Top4 -= 6;
                }
            }
            if (num0 >= 0 && num0 < 10)
                g.DrawString(num0.ToString(), MyFont1, FontBrush, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + 76, Place_Top1);
            else if (num0 >= 10 && num0 < 100)
                g.DrawString(num0.ToString(), MyFont1, FontBrush, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + 71, Place_Top1);
            else if (num0 >= 100 && num0 < 1000)
                g.DrawString(num0.ToString(), MyFont1, FontBrush, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + 68, Place_Top1);
            else if (num0 >= 1000 && num0 < 10000)
                g.DrawString(num0.ToString(), MyFont1, FontBrush, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + 60, Place_Top1);
            else
                g.DrawString("數據超出", new Font("宋体", 16), Brushes.Red, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + 43, ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom + ConstInfor.Cross_Ban_H + CommCelleHeight + 8);
            //num1写入
            if (num1 >= 0 && num1 < 10)
                g.DrawString(num1.ToString(), MyFont2, FontBrush, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + CommCellWidth + 76, Place_Top2);
            else if (num1 >= 10 && num1 < 100)
                g.DrawString(num1.ToString(), MyFont2, FontBrush, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + CommCellWidth + 71, Place_Top2);
            else if (num1 >= 100 && num1 < 1000)
                g.DrawString(num1.ToString(), MyFont2, FontBrush, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + CommCellWidth + 68, Place_Top2);
            else if (num1 >= 1000 && num1 < 10000)
                g.DrawString(num1.ToString(), MyFont2, FontBrush, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + CommCellWidth + 60, Place_Top2);
            else
                g.DrawString("數據超出", new Font("宋体", 16), Brushes.Red, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + CommCellWidth + 43, Place_Top);
            //num2写入
            if (num2 >= 0 && num2 < 10)
                g.DrawString(num2.ToString(), MyFont3, FontBrush, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + CommCellWidth * 2 + 76, Place_Top3);
            else if (num2 >= 10 && num2 < 100)
                g.DrawString(num2.ToString(), MyFont3, FontBrush, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + CommCellWidth * 2 + 71, Place_Top3);
            else if (num2 >= 100 && num2 < 1000)
                g.DrawString(num2.ToString(), MyFont3, FontBrush, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + CommCellWidth * 2 + 65, Place_Top3);
            else if (num2 >= 1000 && num2 < 10000)
                g.DrawString(num2.ToString(), MyFont3, FontBrush, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + CommCellWidth * 2 + 59, Place_Top3);
            else
                g.DrawString("數據超出", new Font("宋体", 16), Brushes.Red, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + CommCellWidth * 2 + 43, Place_Top);
            //num3写入
            if (num3 >= 0 && num3 < 10)
                g.DrawString(num3.ToString(), MyFont4, FontBrush, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + CommCellWidth * 3 + 76, Place_Top4);
            else if (num3 >= 10 && num3 < 100)
                g.DrawString(num3.ToString(), MyFont4, FontBrush, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + CommCellWidth * 3 + 71, Place_Top4);
            else if (num3 >= 100 && num3 < 1000)
                g.DrawString(num3.ToString(), MyFont4, FontBrush, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + CommCellWidth * 3 + 65, Place_Top4);
            else if (num3 >= 1000 && num3 < 10000)
                g.DrawString(num3.ToString(), MyFont4, FontBrush, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + CommCellWidth * 3 + 59, Place_Top4);
            else
                g.DrawString("數據超出", new Font("宋体", 16), Brushes.Red, ConstInfor.Table_Left + ConstInfor.AlarmCell_First_Width + CommCellWidth * 3 + 43, Place_Top);
        }

        /// <summary>
        /// 画出人员表格中的数量
        /// </summary>
        /// <param name="g"></param>
        public static void DrawPerson(Graphics g)
        {
            int CommCellWidth = (ConstInfor.MainCenter_Width - ConstInfor.AlarmCell_First_Width - ConstInfor.Table_Left * 2) / ConstInfor.AlarmCell_RowNum;
            int CommCelleHeight = (ConstInfor.MainCenter_Height - ConstInfor.Alarm_TB_Bottom - ConstInfor.Cross_Ban_H - 5) / (ConstInfor.PsCell_ColNum);

            Font NumFont = new Font("黑体", 17);
            Brush NumBrush = new SolidBrush(Color.Red);
            int x, y, Total_X, Total_Y;
            string StrTitle;
            //获取当前选定的组信息
            SetCurAreasClear();
            int res = GetSelectedGroup();
            int index = 0;
            //给区域按ID排序并生成一个临时的list
            List<KeyValuePair<string, Area>> list = CommonCollection.Areas.OrderBy(c => c.Key).ToList();
            foreach (KeyValuePair<string, Area> area in list)
            {
                    if (area.Value.GroupID == null)
                        continue;
                    if (res < 0 || res >= SysParam.GroupShows.Length || (area.Value.GroupID[0] == SysParam.GroupShows[res].ID[0] && area.Value.GroupID[1] == SysParam.GroupShows[res].ID[1]))
                    {
                        SysParam.CurAreas[index] = area.Value;
                        if (null != area.Value.Name && !"".Equals(area.Value.Name))
                            StrTitle = area.Value.Name;
                        else
                            StrTitle = area.Key;
                        x = ConstInfor.Table_Left + ConstInfor.PsCell_First_Width + CommCellWidth * (index % 4);
                        y = ConstInfor.Table_Top + CommCelleHeight * (index / 4) * 2 + 8;
                        DrawPersonTitleStr(g, StrTitle, x, y + CommCelleHeight, area.Value.AreaType);
                        DrawPersonNum(g, CommonBoxOperation.GetNumber(area.Value), x, y + CommCelleHeight * 2 - 2, area.Value.AreaType);
                        index++;
                        if (index >= 16)
                            break;
                    }
            }
            
            Total_X = ConstInfor.Table_Left + ConstInfor.PsCell_First_Width;
            Total_Y = ConstInfor.Table_Top + CommCelleHeight * 4 * 2 + CommCelleHeight + 5;
            //lock (CommonCollection.TagPacks_Lock)
            try
            {
                DrawPersonTotalNum(g, CommonCollection.TagPacks.Count, Total_X, Total_Y);
            }catch(Exception)
            {
            }
        }
        /// <summary>
        /// 画出人员分区表格的总数量
        /// </summary>
        /// <param name="g"></param>
        /// <param name="num"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void DrawPersonTotalNum(Graphics g, int num, int x, int y)
        {
            Font NumFont = new Font("黑体", 20);
            Brush MyBrush = new SolidBrush(Color.Black);
            g.DrawString(num.ToString(), NumFont, MyBrush, x + 335 - 5 * GetLength(num.ToString()), y);
        }

        /// <summary>
        /// 画出人员分区表格的数量
        /// </summary>
        /// <param name="g"></param>
        /// <param name="num"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="type">区域类型，不同类型显示不同的颜色</param>
        public static void DrawPersonNum(Graphics g, int num, int x, int y, AreaType type)
        {
            Font NumFont = new Font("黑体", 17);
            Brush MyBrush = null;
            switch (type)
            {
                case AreaType.SimpleArea:MyBrush = new SolidBrush(Color.Black);break;
                case AreaType.ControlArea:MyBrush = new SolidBrush(Color.Blue);break;
                case AreaType.DangerArea:MyBrush = new SolidBrush(Color.Red);break;
            }
            int len = GetLength(num.ToString());
            if (len >= 10000)g.DrawString("數據超出", NumFont, MyBrush, x + 75 - GetLength("數據超出") * 5, y);
            else g.DrawString(num.ToString(), NumFont, MyBrush, x + 75 - len * 5, y);
        }
        /// <summary>
        /// 画出人员分区表格的标题栏的名称
        /// </summary>
        /// <param name="g"></param>
        /// <param name="str"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void DrawPersonTitleStr(Graphics g, String str, int x, int y, AreaType type)
        {
            Font TitleFont = new Font("黑体", 17);
            Brush MyBrush = null;
            switch (type)
            {
                case AreaType.SimpleArea:MyBrush = new SolidBrush(Color.Black);break;
                case AreaType.ControlArea:MyBrush = new SolidBrush(Color.Blue);break;
                case AreaType.DangerArea:MyBrush = new SolidBrush(Color.Red);break;
            }
            int len = GetLength(str);
            g.DrawString(str, TitleFont, MyBrush, x + 75 - len * 5, y);
        }

        /// <summary>
        /// 得到字符串的长度，其中数字和字母单位长度为1，汉字单位长度为2
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static int GetLength(String Str)
        {
            return Encoding.Default.GetByteCount(Str);
        }

        /// <summary>
        /// 获取字符串的前14个字节，这里的14是指将字符串转为byte数组时的长度
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static String Get14Char(String Str)
        {
            if (GetLength(Str) <= 14)
                return Str;
            int j = 0;
            for (int i = 0; i < Str.Length; i++)
            {
                char ch = Str[i];
                ushort sh = ch;
                //说明第i个字符串为汉字
                if (sh >= 0x4E00 && sh <= 0x9FA5)j += 2;else j++;
                //此时刚好为14个字节
                if (j >= 14)return Str.Substring(0, i);
            }
            return Str;
        }

        /// <summary>
        /// 初始化网络信息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="port"></param>
        public static int InitNet(string strIp, int port)
        {
            if (null == strIp)return ConstInfor.Ip_Invalid;
            if ("".Equals(strIp))return ConstInfor.Ip_Invalid;
            IPAddress MyIpAddress = null;
            if (!IPAddress.TryParse(strIp, out MyIpAddress))return ConstInfor.Ip_Invalid;
            if (port > System.Net.IPEndPoint.MaxPort || port < System.Net.IPEndPoint.MinPort)return ConstInfor.Port_Invalid;
            if (null == MyIpAddress)   return ConstInfor.Ip_Invalid;
            IPEndPoint Net_Node = null;
            try
            {
                Net_Node = new IPEndPoint(MyIpAddress, port);
            }
            catch (Exception)
            {
                return ConstInfor.NetNode_Fail;
            }
            if (null != Frm.MyUdpClient)
                return ConstInfor.UdpClient_ReDefine;
            try
            {
                Frm.MyUdpClient = new System.Net.Sockets.UdpClient(Net_Node);
            }
            catch (Exception)
            {
                return ConstInfor.UdpClient_Fail;
            }
            sendAndrodData();
            return ConstInfor.Net_Ok;
        }

        public static void sendAndrodData() 
        {
            if (CommonCollection.Androidendport == null) SysParam.getAndroidendport();
            if (CommonCollection.Androidendport == null) return;
            byte[] senAndroid = new byte[] { 0xfa, 0xd4, 0xce, 0xf9 };
            new Thread(() => 
            {
                int t = 5;
                CommonCollection.androidenSend = false;
                while (t > 0)
                {
                    if (CommonCollection.androidenSend == true) break;
                    //Frm.MyUdpClient.Send(senAndroid, senAndroid.Length, CommonCollection.Androidendport);
                    Frm.udpSendData(senAndroid, senAndroid.Length, CommonCollection.Androidendport);
                    t--;
                    Thread.Sleep(300);
                }               
            }).Start();           
        }

        /// <summary>
        /// 向原始资料库中添加新的资料
        /// </summary>
        public static void AddOriginal(TagPack tpk)
        {
            if (null == tpk) 
                return;
            DateTime dt = DateTime.Now;
            string StrDirName = FileOperation.Original + "\\" + dt.Year.ToString().PadLeft(4, '0') + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0') + ".dat";
            //string StrLogName = StrDirName + "\\" + dt.Hour.ToString() + ".dat";
            try
            {
                if (File.Exists(StrDirName))
                { 
                    CommonCollection.LogTags = FileOperation.GetOriginalData(StrDirName);
                }
                else
                { 
                    FileOperation.CreateDateTimeDir(FileOperation.Original, StrDirName);
                    return;
                }
            }
            catch (Exception ex)
            { 
                string msg = DateTime.Now.ToString() + " " + ex.ToString(); FileOperation.WriteLog(msg); 
            }
            if (null == CommonCollection.LogTags) return;
            int len = CommonCollection.LogTags.Count;
            if (len > 0)
            {
                if (CommonCollection.LogTags[len - 1].index == tpk.index)
                {
                    if (tpk.SigStren > CommonCollection.LogTags[len - 1].SigStren)
                    {
                        CommonCollection.LogTags.RemoveAt(len - 1);
                    }
                    else return;
                }
            }
            CommonCollection.LogTags.Add(tpk);
            FileOperation.SaveOriginalData(CommonCollection.LogTags, StrDirName);
        }

        /// <summary>
        /// 开启监听线程
        /// </summary>
        public static void StartListenerThread()
        {
            if (null == Frm.UdpListenerThread)
            {
                Frm.UdpListenerThread = new Thread(ParseClientData);
            }
            Frm.UdpListenerThread.Start();
        }
        public static int FindCharPlace(byte[] buff,byte ch,int start,int curlen)
        {
            for (int i = start; i < curlen; i++)
            {
                if (i < 0 || i >= curlen)
                    return -1;
                if (ch == buff[i])
                    return i;
            }
            return -1;
        }
        private static byte[] ReceiveCaching = new byte[2048];
        private static int CachingLen = 0;
        public static void ParseClientData()
        {
            IPEndPoint endport = new IPEndPoint(IPAddress.Any, System.Net.IPEndPoint.MinPort);
            string StrTagID = "",strid = "";
            int start = 0,len = 0,count = 0;
            byte cs = 0;
            nodenwparam nodeparam = null;
            referparam referparam = null;
            while (null != Frm.MyUdpClient)
            {
                byte[] bytes = null;
                try
                {
                    bytes = Frm.MyUdpClient.Receive(ref endport);
                    if(bytes.Length > 0)
                    {//接收网络数据，并将它放入到缓存里面去
                        System.Buffer.BlockCopy(bytes, 0, ReceiveCaching, CachingLen, bytes.Length);
                        CachingLen += bytes.Length;
                    }
                }
                catch (System.Net.Sockets.SocketException e)
                {
                    Console.WriteLine(e.ToString());
                }
                catch (System.Threading.ThreadAbortException){
                
                }
                catch (Exception ex)
                { 
                    string msg = DateTime.Now.ToString() + " " + ex.ToString(); 
                    FileOperation.WriteLog(msg);
                }
                //处理网络数据
                start = 0;
                try
                {
                    while (CachingLen >= 4)
                    {
                        //查找包头
                        start = FindCharPlace(ReceiveCaching, ConstInfor.Head_Loca, start, CachingLen);
                        if (start < 0)
                        {//说明没有找到定位数据包以及Rfer和Node上报的数据包头，此时只需要我们处理设置的数据包即可

                            start = 0;//再重新从头开始搜索
                            while (start >= 0)
                            {
                                start = FindCharPlace(ReceiveCaching, ConstInfor.head1, start, CachingLen);
                                if (start < 0)
                                {//任然没有找到包头,可以不必处理这一包数据了
                                    start = 0;
                                    while (start >= 0)
                                    {
                                        start = FindCharPlace(ReceiveCaching, ConstInfor.head2, start, CachingLen);
                                        if (start < 0)
                                        {
                                            Array.Clear(ReceiveCaching, 0, CachingLen);
                                            CachingLen = 0;
                                            break;
                                        }
                                        else
                                        {
                                            if (ReceiveCaching[start + 1] == 0x01)
                                            {//可能是搜索节点讯息
                                                /*查询周围的节点设备ID
                                                 * USB --> PC: FC + 01 + Count + ID + channel+ CS + FB
                                                 * */
                                                count = ReceiveCaching[start + 2];
                                                if (start + count * 3 + 4 > CachingLen)
                                                {
                                                    start += 1;
                                                }
                                                else
                                                {
                                                    if (ReceiveCaching[start + count * 3 + 4] == 0xFB)
                                                    {//是返回的节点讯息
                                                        cs = 0;
                                                        for (int i = start; i < start + count * 3 + 3; i++)
                                                        {
                                                            cs += ReceiveCaching[i];
                                                        }
                                                        if (cs == ReceiveCaching[start + count * 3 + 3])
                                                        {
                                                            SysParam.readmark = true;
                                                            for (int i = 0; i < count; i++)
                                                            {
                                                                nodenwparam ndparam = new nodenwparam();
                                                                System.Buffer.BlockCopy(ReceiveCaching, start + 3 + i * 3, ndparam.id, 0, 2);
                                                                ndparam.channel = ReceiveCaching[start + 5 + i * 3];
                                                                strid = ndparam.id[0].ToString("X2") + ndparam.id[1].ToString("X2");
                                                                SysParam.mdevices.nodedevs.TryAdd(strid, ndparam);
                                                            }
                                                        }
                                                        if (CachingLen >= (count * 3 + 5))
                                                        {
                                                            System.Buffer.BlockCopy(ReceiveCaching, start + count * 3 + 5, ReceiveCaching, start, CachingLen);
                                                            CachingLen -= (count * 3 + 5);
                                                        }
                                                        else
                                                        {
                                                            //从下一个位置开始查找
                                                            start += 1;                                                           
                                                        }
                                                    }
                                                    else
                                                    {//说明此时start位置不是包头 
                                                        start += 1;
                                                        
                                                    }
                                                }
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x02 && ReceiveCaching[start + 11] == 0xFB)
                                            {
                                                /*查询节点设备的固件版本号
                                                 * USB --> PC:FC + 02 + ID + Type + Version + channel + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 10; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 10])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        nodeparam.type = ReceiveCaching[start + 4];
                                                        nodeparam.version = (UInt32)(ReceiveCaching[start + 5] << 24 | ReceiveCaching[start + 6] << 16 | ReceiveCaching[start + 7] << 8 | ReceiveCaching[start + 8]);
                                                    }
                                                }
                                                dealPageOver(start, 12);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x05 && ReceiveCaching[start + 9] == 0xFB)
                                            {
                                                /*设置节点的Server IP
                                                 * USB --> PC:FC + 05 + ID + IP + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 8; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 8])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        System.Buffer.BlockCopy(ReceiveCaching, start + 4, nodeparam.serverip, 0, 4);
                                                    }
                                                }
                                                dealPageOver(start, 10);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x06 && ReceiveCaching[start + 9] == 0xFB)
                                            {
                                                /*读取节点的Server IP
                                                 * USB --> PC:FC + 06 + ID + IP + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 8; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 8])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        System.Buffer.BlockCopy(ReceiveCaching, start + 4, nodeparam.serverip, 0, 4);
                                                    }
                                                }
                                                dealPageOver(start, 10);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x07 && ReceiveCaching[start + 7] == 0xFB)
                                            {
                                                /*设置节点的Server Port
                                                 * USB --> PC:FC + 07 + ID + Port + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 6; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 6])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        nodeparam.port = (ushort)(ReceiveCaching[start + 4] << 8 | ReceiveCaching[start + 5]);
                                                    }
                                                }
                                                dealPageOver(start, 8);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x08 && ReceiveCaching[start + 7] == 0xFB)
                                            {
                                                /*读取节点的Server Port
                                                 * USB --> PC:FC + 08 + ID + Port + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 6; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 6])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        nodeparam.port = (ushort)(ReceiveCaching[start + 4] << 8 | ReceiveCaching[start + 5]);
                                                    }
                                                }
                                                dealPageOver(start, 8);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x09 && ReceiveCaching[start + 37] == 0xFB)
                                            {
                                                /*设置节点的Wifi AP名称
                                                 * USB --> PC:FC + 09 + ID + Name + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 36; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 36])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        System.Buffer.BlockCopy(ReceiveCaching, start + 4, nodeparam.wifiname, 0, 32);
                                                    }
                                                }
                                                dealPageOver(start, 38);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x0A && ReceiveCaching[start + 37] == 0xFB)
                                            {
                                                /*读取节点的Wifi AP名称
                                                 * USB --> PC:FC + 0A + ID + Name + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 36; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 36])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        System.Buffer.BlockCopy(ReceiveCaching, start + 4, nodeparam.wifiname, 0, 32);
                                                    }
                                                }
                                                dealPageOver(start, 38);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x0B && ReceiveCaching[start + 37] == 0xFB)
                                            {
                                                /*设置节点的Wifi AP密码
                                                 * USB --> PC:FC + 0B + ID + Password + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 36; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 36])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        System.Buffer.BlockCopy(ReceiveCaching, start + 4, nodeparam.wifipsw, 0, 32);
                                                    }
                                                }
                                                dealPageOver(start, 38);
               
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x0C && ReceiveCaching[start + 37] == 0xFB)
                                            {
                                                /*读取节点的Wifi AP密码
                                                 * USB --> PC:FC + 0C + ID + Password + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 36; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 36])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        System.Buffer.BlockCopy(ReceiveCaching, start + 4, nodeparam.wifipsw, 0, 32);
                                                    }
                                                }
                                                dealPageOver(start, 38);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x0D && ReceiveCaching[start + 5] == 0xFB)
                                            {
                                                /*控制节点复位
                                                 * USB --> PC:FC + 0D + ID + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 4; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 4])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                    }
                                                }
                                                dealPageOver(start, 6);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x0E && ReceiveCaching[start + 6] == 0xFB)
                                            {
                                                /*设置节点的IP模式
                                                 * USB --> PC:FC + 0E + ID + MODE + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 5; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 5])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        nodeparam.ipmode = ReceiveCaching[start + 4];
                                                    }
                                                }
                                                dealPageOver(start, 7);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x0F && ReceiveCaching[start + 6] == 0xFB)
                                            {
                                                /*读取节点的IP模式
                                                 * USB --> PC:FC + 0F + ID + MODE + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 5; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 5])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        nodeparam.ipmode = ReceiveCaching[start + 4];
                                                    }
                                                }
                                                dealPageOver(start, 7);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x10 && ReceiveCaching[start + 9] == 0xFB)
                                            {
                                                /*
                                                 * 设置节点的IP
                                                 * USB --> PC:FC + 10 + ID + IP + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 8; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 8])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        System.Buffer.BlockCopy(ReceiveCaching, start + 4, nodeparam.nodeip, 0, 4);
                                                    }
                                                }
                                                dealPageOver(start, 10);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x11 && ReceiveCaching[start + 9] == 0xFB)
                                            {
                                                /*读取节点的IP
                                                 * USB --> PC:FC + 11 + ID + IP + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 8; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 8])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        System.Buffer.BlockCopy(ReceiveCaching, start + 4, nodeparam.nodeip, 0, 4);
                                                    }
                                                }
                                                dealPageOver(start, 10);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x12 && ReceiveCaching[start + 9] == 0xFB)
                                            {
                                                /*设置节点的SubMask
                                                 * USB --> PC: FC + 12 + ID + IP + CS + FB
                                                 */
                                                cs = 0;
                                                for (int i = start; i < start + 8; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 8])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        System.Buffer.BlockCopy(ReceiveCaching, start + 4, nodeparam.submask, 0, 4);
                                                    }
                                                }
                                                dealPageOver(start, 10);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x13 && ReceiveCaching[start + 9] == 0xFB)
                                            {
                                                /*读取节点的SubMask
                                                 * USB --> PC: FC + 13 + ID + IP + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 8; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 8])
                                                {

                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        System.Buffer.BlockCopy(ReceiveCaching, start + 4, nodeparam.submask, 0, 4);
                                                    }

                                                }
                                                dealPageOver(start, 10);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x14 && ReceiveCaching[start + 9] == 0xFB)
                                            {
                                                /*设置节点的GateWay
                                                 * USB --> PC:FC + 14 + ID + IP + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 8; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 8])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        System.Buffer.BlockCopy(ReceiveCaching, start + 4, nodeparam.gateway, 0, 4);
                                                    }
                                                }
                                                dealPageOver(start, 10);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x15 && ReceiveCaching[start + 9] == 0xFB)
                                            {
                                                /*
                                                 * 读取节点的GateWay
                                                 * USB --> PC:FC + 15 + ID + IP + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 8; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 8])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        System.Buffer.BlockCopy(ReceiveCaching, start + 4, nodeparam.gateway, 0, 4);
                                                    }
                                                }
                                                dealPageOver(start, 10);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x16 && ReceiveCaching[start + 6] == 0xFB)
                                            {
                                                /*
                                                 * USB-->PC:FC + 16 + ID + Rssi + CS + FB
                                                 */
                                                cs = 0;
                                                for (int i = start; i < start + 5;i ++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 5])
                                                {
                                                    SysParam.readmark = true;
                                                    SysParam.WifiSsd = ReceiveCaching[start + 4];
                                                }
                                                dealPageOver(start, 7);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x41)
                                            {
                                                /*
                                                 * 查询周围的参考点设备ID
                                                 * USB --> PC:FC + 41 + Count + ID + CS + FB
                                                 * */
                                                count = ReceiveCaching[start + 2];
                                                if (start + count * 2 + 4 > CachingLen)
                                                {
                                                    start += 1;
                                                }
                                                else
                                                {
                                                    if (ReceiveCaching[start + count * 2 + 4] == 0xFB)
                                                    {
                                                        cs = 0;
                                                        for (int i = start; i < start + count * 2 + 3; i++)
                                                        {
                                                            cs += ReceiveCaching[i];
                                                        }
                                                        if (cs == ReceiveCaching[start + count * 2 + 3])
                                                        {
                                                            SysParam.readmark = true;
                                                            for (int i = 0; i < count; i++)
                                                            {
                                                                referparam = new PersionAutoLocaSys.referparam();
                                                                System.Buffer.BlockCopy(ReceiveCaching, start + 3 + i * 2, referparam.id, 0, 2);
                                                                strid = referparam.id[0].ToString("X2") + referparam.id[1].ToString("X2");
                                                                SysParam.mdevices.referdevs.TryAdd(strid, referparam);
                                                            }
                                                        }
                                                        if (CachingLen >= (count * 2 + 5))
                                                        {
                                                            System.Buffer.BlockCopy(ReceiveCaching, start + count * 2 + 5, ReceiveCaching, start, CachingLen);
                                                            CachingLen -= (count * 2 + 5);

                                                        }
                                                        else
                                                        {
                                                            start += 1;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        start += 1;
                                                    }
                                                }
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x42 && ReceiveCaching[start + 10] == 0xFB)
                                            {
                                                /*查询参考点设备的固件版本号
                                                 * USB --> PC:FC + 42 + ID + Type + Version + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 9; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 9])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.referdevs.TryGetValue(strid, out referparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        referparam.type = ReceiveCaching[start + 4];
                                                        referparam.version = (UInt32)(ReceiveCaching[start + 5] << 24 
                                                            | ReceiveCaching[start + 6] << 16 
                                                            | ReceiveCaching[start + 7] << 8 | ReceiveCaching[start + 8]);
                                                    }
                                                }
                                                dealPageOver(start, 11);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x45 && ReceiveCaching[start + 6] == 0xFB)
                                            {
                                                /*
                                                 * 设置参考点接收Tag定位信号强度阀值
                                                 * USB --> PC:FC + 45 + ID + Threshold + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 5; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 5])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.referdevs.TryGetValue(strid, out referparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        referparam.Sgthreshold = ReceiveCaching[start + 4];
                                                    }
                                                }
                                                dealPageOver(start, 7);         
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x46 && ReceiveCaching[start + 6] == 0xFB)
                                            {
                                                /*
                                                 * 读取参考点接收Tag定位信号强度阀值
                                                 * USB--->PC：FC +  46 + ID +Threshold + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 5; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 5])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.referdevs.TryGetValue(strid, out referparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        referparam.Sgthreshold = ReceiveCaching[start + 4];
                                                    }
                                                }
                                                dealPageOver(start, 7);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x47 && ReceiveCaching[start + 6] == 0xFB)
                                            {
                                                /*
                                                 * 设置参考点发送Tag定位包的信号强度系数k
                                                 * USB--->PC：FC + 47 + ID + k1 + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 5; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 5])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.referdevs.TryGetValue(strid, out referparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        referparam.Sgstrengthfac = ReceiveCaching[start + 4];
                                                    }
                                                }
                                                dealPageOver(start, 7);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x48 && ReceiveCaching[start + 6] == 0xFB)
                                            {
                                                /*
                                                 * 读取参考点发送Tag定位包的信号强度系数k
                                                 * USB--->PC：FC +  48 + ID + k1 + CS  + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 5; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 5])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.referdevs.TryGetValue(strid, out referparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        referparam.Sgstrengthfac = ReceiveCaching[start + 4];
                                                    }
                                                }
                                                dealPageOver(start, 7);
                      
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x49 && ReceiveCaching[start + 5] == 0xFB)
                                            {
                                                /*
                                                 * 控制参考点复位
                                                 * USB --> PC:FC + 49 + ID + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 4; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 4])
                                                {
                                                    SysParam.readmark = true;
                                                }
                                                dealPageOver(start, 6);
                           
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x4A && ReceiveCaching[start + 7] == 0xFB)
                                            {
                                                /*查询节点周围指定参考点的信号强度
                                                 * USB --> PC：FC + 4A + ID +Rssi1 + Rssi2 + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 6; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 6])
                                                {

                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.referdevs.TryGetValue(strid, out referparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        referparam.refertonodesig = ReceiveCaching[start + 5];
                                                        referparam.nodetorefersig = ReceiveCaching[start + 4];
                                                    }
                                                }
                                                dealPageOver(start, 8);
                      
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x17 && ReceiveCaching[start + 6] == 0xFB)
                                            {
                                                /*查询节点最后一次网络连接状态
                                                 * FC + 17 + ID + status + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 5;i ++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 5])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        nodeparam.status = ReceiveCaching[start + 4];
                                                    }
                                                }
                                                dealPageOver(start, 7);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x18 && ReceiveCaching[start + 6] == 0xFB)
                                            {
                                                /*设置Wifi节点的IP模式
                                                 * FC + 18 + ID + mode + CS + FB
                                                 * 
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 5;i ++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 5])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        nodeparam.ipmode = (byte)(ReceiveCaching[start + 4] == 0x02?0x1:0);
                                                    }
                                                }
                                                dealPageOver(start, 7);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x1A && ReceiveCaching[start + 9] == 0xFB)
                                            {
                                                /*设置节点使用Wifi是的静态IP
                                                 * FC + 1A + ID + IP + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 8; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 8])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        System.Buffer.BlockCopy(ReceiveCaching, start + 4, nodeparam.nodeip,0,4);
                                                    }
                                                }
                                                dealPageOver(start, 10);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x19 && ReceiveCaching[start + 6] == 0xFB)
                                            {
                                                /*读取节点使用Wifi是的静态IP
                                                 * FC + 19 + ID + IP + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 5; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 5])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        nodeparam.ipmode = (byte)(ReceiveCaching[start + 4] == 0x2 ? 0x01:0);
                                                    }
                                                }
                                                dealPageOver(start, 7);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x1B && ReceiveCaching[start + 9] == 0xFB)
                                            {
                                                /*读取节点使用Wifi是的静态IP
                                                 * FC + 1A + ID + IP + CS + FB
                                                 * */
                                                cs = 0;
                                                for (int i = start; i < start + 8; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 8])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        System.Buffer.BlockCopy(ReceiveCaching, start + 4, nodeparam.nodeip, 0, 4);
                                                    }
                                                }
                                                dealPageOver(start, 10);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0x1C && ReceiveCaching[start + 95] == 0xFB)
                                            {
                                                cs = 0;
                                                for (int i = start; i < start + 94; i++)
                                                {
                                                    cs += ReceiveCaching[i];
                                                }
                                                if (cs == ReceiveCaching[start + 94])
                                                {
                                                    strid = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                    if (SysParam.mdevices.nodedevs.TryGetValue(strid, out nodeparam))
                                                    {
                                                        SysParam.readmark = true;
                                                        System.Buffer.BlockCopy(ReceiveCaching,start + 4,SysParam.backcmds,0,90);
                                                    }
                                                }
                                                dealPageOver(start, 96);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0xCC && ReceiveCaching[start + 23] == 0xFB)
                                            {
                                                // 获取上下班时间          
                                                setAndroidendport(endport);              
                                                byte[] buffer = new byte[24];
                                                Array.Copy(ReceiveCaching, start, buffer, 0, 24);
                                                sendWorkData(start, endport);                                               
                                                dealUpWorkTime(buffer, endport);
                                                dealPageOver(start, 24);                                            
                                            }
                                            else if (ReceiveCaching[start + 1] == 0xCD && ReceiveCaching[start + 23] == 0xFB)
                                            {
                                                // 获取上下班时间
                                                // FC + CD + ID + tine + CS + FB 
                                                setAndroidendport(endport);    
                                                byte[] buffer = new byte[24];
                                                Array.Copy(ReceiveCaching, start, buffer, 0, 24);
                                                sendWorkData(start, endport);
                                                dealUpSleepTime(buffer, endport);
                                                dealPageOver(start, 24);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0xCE && ReceiveCaching[start + 4] == 0xFB)
                                            {
                                                setAndroidendport(endport);   
                                                sendPeopleID(ReceiveCaching[2], endport);
                                                dealPageOver(start, 5);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0xCF && ReceiveCaching[start + 5] == 0xFB)
                                            {
                                                setAndroidendport(endport);   
                                                sendPeopleInfo(ReceiveCaching, endport);
                                                dealPageOver(start, 6);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0xD4 && ReceiveCaching[start + 3] == 0xFB)
                                            {
                                                setAndroidendport(endport);   
                                                CommonCollection.androidenSend = true;
                                                dealPageOver(start, 4);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0xD5)
                                            {
                                                setAndroidendport(endport);   
                                                setAccountPassword(ReceiveCaching, endport);
                                                dealPageOver(start, 4);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0xD6 && ReceiveCaching[start + 3] == 0xFB)
                                            {
                                                setAndroidendport(endport);   
                                                setAllAccountPassword(ReceiveCaching, endport);
                                                dealPageOver(start, 4);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0xD7 && ReceiveCaching[start + 3] == 0xFB)
                                            {
                                                setAndroidendport(endport);   
                                                sendCurrentTime(ReceiveCaching, endport);
                                                dealPageOver(start, 4);
                                            }
                                            else if (ReceiveCaching[start + 1] == 0xD8 && ReceiveCaching[start + 19] == 0xFB)
                                            {
                                                setAndroidendport(endport);
                                                sendDaCardInfor(ReceiveCaching, endport);
                                                dealPageOver(start, 20);
                                            }
                                            else
                                            {
                                                dealPageOver(start, 2);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    #region 网络设置Refer和Node的参数
                                    if (ReceiveCaching[start + 1] == 0x00 && ReceiveCaching[start + 5] == 0xFB)
                                    {// Node -->PC：FA + 00 + ID(2byte) + CS + FB(6byte)
                                        cs = 0;
                                        for (int i = start; i < start + 4; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 4])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                        }
                                        dealPageOver(start, 6);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x01 && ReceiveCaching[start + 9] == 0xFB)
                                    {// Node -->PC：FA + 01 + ID(2byte) + IP(4byte) + CS + FB(10byte)
                                        cs = 0;
                                        for (int i = start; i < start + 8; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 8])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mntparm.serverip, 0, 4);
                                        }
                                        dealPageOverDeleteStart(start, 10);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x02 && ReceiveCaching[start + 9] == 0xFB)
                                    {// Node-->PC：FA + 02 + ID(2byte) + IP(4byte) + CS + FB(10byte)
                                        cs = 0;
                                        for (int i = start; i < start + 8; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 8])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mntparm.serverip, 0, 4);
                                        }
                                        dealPageOverDeleteStart(start, 8);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x03 && ReceiveCaching[start + 7] == 0xFB)
                                    {// Node -->PC：FA + 03 + ID(2byte) + Port(2byte) + CS + FB(8byte)
                                        cs = 0;
                                        for (int i = start; i < start + 6; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 6])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                            SysParam.mntparm.port = (UInt16)(ReceiveCaching[start + 4] << 8 | ReceiveCaching[start + 5]);
                                        }
                                        dealPageOverDeleteStart(start, 8);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x04 && ReceiveCaching[start + 7] == 0xFB)
                                    {// Node -->PC：FA + 04 + ID(2byte) + Port(2byte) + CS + FB(8byte)
                                        cs = 0;
                                        for (int i = start; i < start + 6; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 6])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                            SysParam.mntparm.port = (UInt16)(ReceiveCaching[start + 4] << 8 | ReceiveCaching[start + 5]);
                                        }
                                        dealPageOverDeleteStart(start, 8);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x05 && ReceiveCaching[start + 37] == 0xFB)
                                    { // Node -->PC：FA + 05 + ID(2byte) + Name(32byte) + CS + FB(38byte)
                                        cs = 0;
                                        for (int i = start; i < start + 36; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 36])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mntparm.wifiname, 0, 32);
                                        }
                                        dealPageOverDeleteStart(start, 38);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x06 && ReceiveCaching[start + 37] == 0xFB)
                                    {//Node -->PC：FA + 06 + ID(2byte) + Name(32byte) + CS + FB(38byte)
                                        cs = 0;
                                        for (int i = start; i < start + 36; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 36])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mntparm.wifiname, 0, 32);
                                        }
                                        dealPageOverDeleteStart(start, 38);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x07 && ReceiveCaching[start + 37] == 0xFB)
                                    {//Node -->PC：FA + 07 + ID(2byte) + PSW(32byte) + CS + FB(38byte)
                                        cs = 0;
                                        for (int i = start; i < start + 36; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 36])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mntparm.wifipsw, 0, 32);
                                        }
                                        dealPageOverDeleteStart(start, 38);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x08 && ReceiveCaching[start + 37] == 0xFB)
                                    {//Node -->PC：FA + 08 + ID(2byte) + PSW(32byte) + CS + FB(38byte)

                                        cs = 0;
                                        for (int i = start; i < start + 36; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 36])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mntparm.wifipsw, 0, 32);
                                        }
                                        dealPageOverDeleteStart(start, 38);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x09 && ReceiveCaching[start + 6] == 0xFB)
                                    {//Node -->PC：FA + 09 + ID(2byte) + Mode(1byte) + CS + FB(7byte)

                                        cs = 0;
                                        for (int i = start; i < start + 5; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 5])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                            SysParam.mntparm.ipmode = ReceiveCaching[start + 4];
                                        }
                                        dealPageOverDeleteStart(start, 7);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x0A && ReceiveCaching[start + 6] == 0xFB)
                                    {//Node -->PC：FA + 0A + ID(2byte) + Mode(1byte) + CS + FB(7byte)

                                        cs = 0;
                                        for (int i = start; i < start + 5; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 5])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                            SysParam.mntparm.ipmode = ReceiveCaching[start + 4];
                                        }
                                        dealPageOverDeleteStart(start, 7);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x0B && ReceiveCaching[start + 9] == 0xFB)
                                    {//Node -->PC：FA + 0B + ID(2byte) + IP(4byte) + CS + FB(10byte)

                                        cs = 0;
                                        for (int i = start; i < start + 8; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 8])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mntparm.nodeip, 0, 4);
                                        }
                                        dealPageOverDeleteStart(start, 10);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x0C && ReceiveCaching[start + 9] == 0xFB)
                                    {//Node -->PC：FA + 0C + ID(2byte) + IP(4byte) + CS + FB(10byte)

                                        cs = 0;
                                        for (int i = start; i < start + 8; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 8])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mntparm.nodeip, 0, 4);
                                        }
                                        dealPageOverDeleteStart(start, 10);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x0D && ReceiveCaching[start + 9] == 0xFB)
                                    {//Node -->PC：FA + 0D + ID(2byte) + Submask(4byte) + CS + FB(10byte)
                                        cs = 0;
                                        for (int i = start; i < start + 8; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 8])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mntparm.submask, 0, 4);
                                        }
                                        dealPageOverDeleteStart(start, 10);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x0E && ReceiveCaching[start + 9] == 0xFB)
                                    {// Node -->PC：FA + 0E + ID(2byte) + Submask(4byte) + CS + FB(10byte)

                                        cs = 0;
                                        for (int i = start; i < start + 8; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 8])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mntparm.submask, 0, 4);
                                        }
                                        dealPageOverDeleteStart(start, 10);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x0F && ReceiveCaching[start + 9] == 0xFB)
                                    {// Node -->PC：FA + 0F + ID(2byte) + GateWay(4byte) + CS + FB(10byte)

                                        cs = 0;
                                        for (int i = start; i < start + 8; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 8])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mntparm.gateway, 0, 4);
                                        }
                                        dealPageOverDeleteStart(start, 10);
                                    }
                               else if (ReceiveCaching[start + 1] == 0x10 && ReceiveCaching[start + 9] == 0xFB)
                               {// Node --> PC：FA + 10 + ID(2byte) + GateWay(4byte) + CS + FB(10byte)

                                   cs = 0;
                                   for (int i = start; i < start + 8; i++)
                                   {
                                       cs += ReceiveCaching[i];
                                   }
                                   if (cs == ReceiveCaching[start + 8])
                                   {
                                       SysParam.cbparm = true;
                                       System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                       System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mntparm.gateway, 0, 4);
                                        }
                                   dealPageOverDeleteStart(start, 10);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x11 && ReceiveCaching[start + 10] == 0xFB)
                                    {// PC --> Node：FA + 11 + ID(2byte) + type + Version + CS + FB(11byte)

                                        cs = 0;
                                        for (int i = start; i < start + 9; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 9])
                                        {
                                            SysParam.scandevtype = true;
                                            SysParam.mnodemsg = new NodeMsg();
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mnodemsg.ID, 0, 2);
                                            SysParam.mnodemsg.type = ReceiveCaching[4];
                                            SysParam.mnodemsg.mendpoint = endport;

                                            SysParam.mnodemsg.Version = (UInt32)(ReceiveCaching[start + 5] << 24 | ReceiveCaching[start + 6] << 16 | ReceiveCaching[start + 7] << 8 | ReceiveCaching[start + 8]);
                                        }
                                        dealPageOverDeleteStart(start, 11);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x12 && ReceiveCaching[start + 7] == 0xFB)
                                    {// Node --> PC：FA + 12 + ID + Addr + CS + FB(8byte)

                                        cs = 0;
                                        for (int i = start; i < start + 6; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 6])
                                        {
                                            if (SysParam.currentaddr == (UInt16)(bytes[4] << 8 | bytes[5]))
                                            {
                                                SysParam.cbparm = true;
                                                SysParam.mnodemsg = new NodeMsg();
                                                System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mnodemsg.ID, 0, 2);

                                            }
                                        }
                                        dealPageOverDeleteStart(start, 8);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x13 && ReceiveCaching[start + 6] == 0xFB)
                                    {//  Node --> PC：FA + 13 + ID + Status(1byte) + CS + FB(7byte)

                                        cs = 0;
                                        for (int i = start; i < start + 5; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 5])
                                        {
                                            SysParam.cbparm = true;
                                            SysParam.ischeckcs = (ReceiveCaching[start + 4] == 0x01 ? true : false);
                                            SysParam.mnodemsg = new NodeMsg();
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mnodemsg.ID, 0, 2);
                                        }
                                        dealPageOverDeleteStart(start, 7);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x14 && ReceiveCaching[start + 6] == 0xFB)
                                    {//Node --> PC：FA + 14 + ID + Channel(1byte) + CS + FB
                                        cs = 0;
                                        for (int i = start; i < start + 5; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 5])
                                        {
                                            SysParam.cbparm = true;
                                            SysParam.mntparm = new nodenwparam();
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                            SysParam.mntparm.channel = ReceiveCaching[start + 4];
                                        }
                                        dealPageOverDeleteStart(start, 7);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x20 && ReceiveCaching[start + 8] == 0xFB)
                                    {//Node--->PC:FA + 20 + NodeID(2byte) + ReferID(2byte) + SigThreshold(1byte) + CS +FB(9byte)

                                        cs = 0;
                                        for (int i = start; i < start + 7; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 7])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mfrprm.id, 0, 2);
                                            SysParam.mfrprm.Sgthreshold = ReceiveCaching[start + 6];
                                        }
                                        dealPageOverDeleteStart(start, 9);
                          
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x21 && ReceiveCaching[start + 8] == 0xFB)
                                    {//  Node--->PC:FA + 21 + NodeID(2byte) + ReferID(2byte) + SigThreshold(1byte) + CS +FB(9byte)

                                        cs = 0;
                                        for (int i = start; i < start + 7; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 7])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mfrprm.id, 0, 2);
                                            SysParam.mfrprm.Sgthreshold = ReceiveCaching[start + 6];
                                        }
                                        dealPageOverDeleteStart(start, 9);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x22 && ReceiveCaching[start + 8] == 0xFB)
                                    {// FA + 22 + NodeID(2byte) + ReferID(2byte) + SigThreshold(1byte) + CS +FB(9byte)

                                        cs = 0;
                                        for (int i = start; i < start + 7; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 7])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mfrprm.id, 0, 2);
                                            SysParam.mfrprm.Sgstrengthfac = ReceiveCaching[start + 6];
                                        }
                                        dealPageOverDeleteStart(start, 9);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x23 && ReceiveCaching[start + 8] == 0xFB)
                                    {// FA + 23 + NodeID(2byte) + ReferID(2byte) + k(1byte) + CS +FB(9byte)

                                        cs = 0;
                                        for (int i = start; i < start + 7; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 7])
                                        {
                                            SysParam.cbparm = true;
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mfrprm.id, 0, 2);
                                            SysParam.mfrprm.Sgstrengthfac = ReceiveCaching[start + 6];
                                        }
                                        dealPageOverDeleteStart(start, 9);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x24 && ReceiveCaching[start + 12] == 0xFB)
                                    {// FA + 24 + NodeID(2byte) + ID(2byte) + type(1byte) + Version(4byte) + CS + FB(13byte)

                                        cs = 0;
                                        for (int i = start; i < start + 11; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 11])
                                        {
                                            SysParam.scandevtype = true;
                                            SysParam.mnodemsg = new NodeMsg();

                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mnodemsg.BasicID, 0, 2);
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mnodemsg.ID, 0, 2);

                                            SysParam.mnodemsg.type = ReceiveCaching[start + 6];
                                            SysParam.mnodemsg.mendpoint = endport;

                                            SysParam.mnodemsg.Version = (UInt32)(ReceiveCaching[start + 7] << 24 | ReceiveCaching[start + 8] << 16 | ReceiveCaching[start + 9] << 8 | ReceiveCaching[start + 10]);
                                        }
                                        dealPageOverDeleteStart(start, 13);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x25 && ReceiveCaching[start + 9] == 0xFB)
                                    {// FA + 25 + NodeID(2byte) + ID(2byte) + Addr(2byte) + CS + FB(10byte)

                                        cs = 0;
                                        for (int i = start; i < start + 8; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 8])
                                        {
                                            if (SysParam.currentaddr == (UInt16)(ReceiveCaching[start + 6] << 8 | ReceiveCaching[start + 7]))
                                            {
                                                SysParam.cbparm = true;
                                                SysParam.mnodemsg = new NodeMsg();
                                                System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mnodemsg.BasicID, 0, 2);
                                                System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mnodemsg.ID, 0, 2);
                                            }
                                        }
                                        dealPageOverDeleteStart(start, 10);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x26 && ReceiveCaching[start + 8] == 0xFB)
                                    {// Node--->PC:FA + 26 + NodeID(2byte) + ID + Status(1byte) + CS + FB(9byte)

                                        cs = 0;
                                        for (int i = start; i < start + 7; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 7])
                                        {
                                            SysParam.cbparm = true;
                                            SysParam.ischeckcs = (ReceiveCaching[start + 6] == 0x01 ? true : false);
                                            SysParam.mnodemsg = new NodeMsg();
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mnodemsg.BasicID, 0, 2);
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mnodemsg.ID, 0, 2);
                                        }
                                        dealPageOverDeleteStart(start, 9);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x27 && ReceiveCaching[start + 5] == 0xFB)
                                    {//FA + 27 + ReferID(2byte) + CS + FB
                                        cs = 0;
                                        for (int i = start; i < start + 4; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 4])
                                        {
                                            SysParam.cbparm = true;
                                            SysParam.mfrprm = new referparam();
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mfrprm.id, 0, 2);
                                        }
                                        dealPageOverDeleteStart(start, 6);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x15 && ReceiveCaching[start + 6] == 0xFB)
                                    {
                                        cs = 0;
                                        for (int i = start; i < start + 5; i ++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 5])
                                        {//校验相同
                                            SysParam.cbparm = true;
                                            SysParam.mntparm = new nodenwparam();
                                            SysParam.mntparm.wifisig = ReceiveCaching[4];
                                        }
                                        dealPageOverDeleteStart(start, 7);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x16 && ReceiveCaching[start + 6] == 0xFB)
                                    {
                                        cs = 0;
                                        for (int i = start; i < start + 5; i ++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 5])
                                        {
                                            if (SysParam.mntparm.id[0] == ReceiveCaching[start + 2] && SysParam.mntparm.id[1] == ReceiveCaching[start + 3])
                                            {
                                                SysParam.cbparm = true;
                                            }
                                        }
                                        dealPageOverDeleteStart(start, 7);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x17 && ReceiveCaching[start + 6] == 0xFB)
                                    {
                                        cs = 0;
                                        for (int i = start; i < start + 5; i ++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 5])
                                        {
                                            SysParam.cbparm = true;
                                            SysParam.mntparm = new nodenwparam();
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                            
                                            SysParam.mntparm.ipmode = (byte)(ReceiveCaching[start + 4] == 0x2?0x1:0);
                                        }
                                        dealPageOverDeleteStart(start, 7);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x18 && ReceiveCaching[start + 9] == 0xFB)
                                    {
                                        cs = 0;
                                        for (int i = start; i < start + 8; i++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 8])
                                        {
                                            //查看是否是设置当前的节点
                                            if (SysParam.mntparm.id[0] == ReceiveCaching[start + 2] && SysParam.mntparm.id[1] == ReceiveCaching[start + 3])
                                            {
                                                SysParam.cbparm = true;
                                            }
                                        }
                                        dealPageOverDeleteStart(start, 10);
                                    }
                                    else if (ReceiveCaching[start + 1] == 0x19 && ReceiveCaching[start + 9] == 0xFB)
                                    {
                                        cs = 0;
                                        for (int i = start; i < start + 8; i ++)
                                        {
                                            cs += ReceiveCaching[i];
                                        }
                                        if (cs == ReceiveCaching[start + 8])
                                        {
                                            SysParam.cbparm = true;
                                            SysParam.mntparm = new nodenwparam();
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, SysParam.mntparm.id, 0, 2);
                                            System.Buffer.BlockCopy(ReceiveCaching, start + 4, SysParam.mntparm.nodeip,0,4);
                                        }
                                        dealPageOverDeleteStart(start, 10);
                                    }
                                    else
                                    {
                                        start += 1;
                                    }
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            //找到定位包的包头，数据包的指定位获取数据包的长度
                            //fe + 03/04 + id_k + gsensortime + battery + tagsleeptime + tagstaticsleeptime + index + refcount + id_c1 + rssi1 + ... + cs + fd
                            if (start + 1 >= CachingLen)
                            {
                                //说明此时start是最后一包数据了,start继续往后搜索
                                start += 1;
                            }
                            else
                            {
                                //判断以fe开头的包的类型
                                if (ReceiveCaching[start + 1] == ConstInfor.Type_Alarm || ReceiveCaching[start + 1] == ConstInfor.Type_Cmm)
                                {
                                    len = ReceiveCaching[start + 12] * 3 + 15;
                                    if (start + len >= ReceiveCaching.Length)
                                    {
                                        start += 1;
                                    }
                                    else
                                    {
                                        if (ReceiveCaching[start + len - 1] == ConstInfor.End_Loca)
                                        {
                                            cs = 0;
                                            for (int i = start; i < start + len - 2; i++)
                                            {
                                                cs += ReceiveCaching[i];
                                            }
                                            if (cs == ReceiveCaching[start + len - 2])
                                            {//校验成功
                                                //卡片上报的封包:fe + 03/04 + id_k + gsensortime + battery + tagsleeptime + tagstaticsleeptime + index + refcount + id_c1 + rssi1 + ... + cs + fd
                                                #region
                                                StrTagID = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                //Tag上报数据时，检查数据是否可以接收
                                                if (IsReceiveTag(StrTagID))
                                                {
                                                    #region 检查是否有Tag断开警报讯息已经处理
                                                    WarmInfo MyWm = CommonBoxOperation.GetWarmItem(StrTagID, SpeceilAlarm.TagDis);
                                                    if (null != MyWm && !MyWm.isHandler)
                                                    {
                                                        MyWm.ClearAlarmTime = DateTime.Now;
                                                        MyWm.isHandler = true;
                                                    }
                                                    #endregion
                                                    TagPack MyTagPack = null;
                                                    try
                                                    {
                                                        if (CommonCollection.TagPacks.TryGetValue(StrTagID, out MyTagPack))
                                                        {
                                                            #region Tag上报数据包
                                                            //判断是否需要使用未移动指定时间报警的功能
                                                            MyTagPack.isUseGSensor = IsNeedGSensor(StrTagID);
                                                            //新来的数据包序列号是否相同
                                                            if (MyTagPack.index == ReceiveCaching[start + 11])
                                                            {//fe + 03/04 + id_k + gsensortime + battery + tagsleeptime + tagstaticsleeptime + index + refcount + id_c1 + rssi1 + ... + cs + fd                                         //序列号相同时，添加新的基站到集合中
                                                                for (int i = 0; i < ReceiveCaching[start + 12]; i++)
                                                                {
                                                                    byte[] id = new byte[2];
                                                                    System.Buffer.BlockCopy(ReceiveCaching, start + 13 + i * 3, id, 0, 2);
                                                                    strid = id[0].ToString("X2") + id[1].ToString("X2");
                                                                    RefReport mrefreport = new RefReport(id, ReceiveCaching[start + 13 + i * 3 + 2]);
                                                                    //添加Refer讯息到集合中去
                                                                    if (!MyTagPack.BaseRefs.ContainsKey(strid))
                                                                    {
                                                                        MyTagPack.BaseRefs.Add(strid, mrefreport);
                                                                        MyTagPack.BasicRefsNum++;//表示当前的集合中保存的参考点的数量
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                #region 每次接收完Tag的定位包后，开始计算Tag此时是靠近哪一个参考点
                                                                if (MyTagPack.BaseRefs.Values.Count > 0)
                                                                {
                                                                    RefReport minref = MyTagPack.BaseRefs.Values.First();

                                                                    foreach (KeyValuePair<string, RefReport> mrep in MyTagPack.BaseRefs)
                                                                    {
                                                                        if (null == mrep.Value)
                                                                            continue;
                                                                        if (mrep.Value.Rssi < minref.Rssi)
                                                                        {
                                                                            minref = mrep.Value;
                                                                        }
                                                                    }
                                                                    if (null != minref)
                                                                    {
                                                                        string strlastbestrd = MyTagPack.RD_New[0].ToString("X2") + MyTagPack.RD_New[1].ToString("X2");
                                                                        string strcurbestrd = minref.Id[0].ToString("X2") + minref.Id[1].ToString("X2");
                                                                        //判断当前选择的优化方式
                                                                        if (SysParam.curOptimalMedol == OptimizationModel.HopTimes)
                                                                        {
                                                                            #region 连续两次跳点次数限制模式
                                                                            if (strlastbestrd.Equals(strcurbestrd))
                                                                            {
                                                                                MyTagPack.SigStren = minref.Rssi;
                                                                                MyTagPack.LastJumpId[0] = 0;
                                                                                MyTagPack.LastJumpId[1] = 0;
                                                                                MyTagPack.JumpNum = 0;
                                                                            }
                                                                            else
                                                                            {//发生一次跳点
                                                                                if (MyTagPack.JumpNum >= SysParam.PopTimes)
                                                                                {//连续跳点次数大于指定值时,才更新位置
                                                                                    MyTagPack.SigStren = minref.Rssi;
                                                                                    System.Buffer.BlockCopy(minref.Id, 0, MyTagPack.RD_New, 0, 2);
                                                                                    MyTagPack.LastJumpId[0] = 0; MyTagPack.LastJumpId[1] = 0;
                                                                                    MyTagPack.JumpNum = 0;
                                                                                }
                                                                                else
                                                                                {
                                                                                    //判断当前跳点的位置和上一次跳点位置是否相同
                                                                                    string lastJumpId = MyTagPack.LastJumpId[0].ToString("X2") + MyTagPack.LastJumpId[1].ToString("X2");
                                                                                    if ("0000".Equals(lastJumpId))
                                                                                    {//说明这一次是第一次发生跳点
                                                                                        MyTagPack.JumpNum++;
                                                                                        System.Buffer.BlockCopy(minref.Id, 0, MyTagPack.LastJumpId, 0, 2);
                                                                                    }
                                                                                    else
                                                                                    {//后面才发生跳点
                                                                                        if (lastJumpId.Equals(strcurbestrd))
                                                                                        {
                                                                                            MyTagPack.JumpNum++;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            MyTagPack.JumpNum = 0;
                                                                                            //同时改变上一次跳点位置
                                                                                            System.Buffer.BlockCopy(minref.Id, 0, MyTagPack.LastJumpId, 0, 2);
                                                                                        }
                                                                                    }

                                                                                }
                                                                            }
                                                                            #endregion
                                                                        }
                                                                        else
                                                                        {
                                                                            #region 信号强度差值优化模式
                                                                            if (MyTagPack.ResTime > 3)
                                                                            {//当前Tag处于静止状态
                                                                                if (minref.Rssi < MyTagPack.SigStren)
                                                                                {
                                                                                    MyTagPack.SigStren = minref.Rssi;
                                                                                    System.Buffer.BlockCopy(minref.Id, 0, MyTagPack.RD_New, 0, 2);
                                                                                }
                                                                            }
                                                                            else
                                                                            {//当前tag处于移动状态 
                                                                                //判断两次最优点的信号差值是否大于阀值3db

                                                                                if (strlastbestrd.Equals(strcurbestrd))
                                                                                {
                                                                                    MyTagPack.SigStren = minref.Rssi;
                                                                                    MyTagPack.limitlossnum = 0;

                                                                                }
                                                                                else
                                                                                {
                                                                                    //判断上一次的最强点是否丢包了
                                                                                    RefReport lastbestincur = null;
                                                                                    if (MyTagPack.BaseRefs.TryGetValue(strlastbestrd, out lastbestincur))
                                                                                    {//上一次最强点没有丢包
                                                                                        //此时我们可以判断两次信号强度差值大于阀值
                                                                                        /*
                                                                                         * 注意:这里的比较应该是拿上一次最强点在本次的信号强度与本次最优点的信号强度比较
                                                                                         * */
                                                                                        //发现上一次的最优点在本次的信号强度比本次最优点信号强度大于3db
                                                                                        if (lastbestincur.Rssi - minref.Rssi >= SysParam.RssiThreshold)
                                                                                        {
                                                                                            System.Buffer.BlockCopy(minref.Id, 0, MyTagPack.RD_New, 0, 2);
                                                                                            MyTagPack.SigStren = minref.Rssi;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            MyTagPack.SigStren = lastbestincur.Rssi;
                                                                                        }
                                                                                        MyTagPack.limitlossnum = 0;
                                                                                    }
                                                                                    else
                                                                                    {//上一次的最强点已经丢包 
                                                                                        if (MyTagPack.limitlossnum >= 1)
                                                                                        {//丢包已经丢了两次了，可能是参考点已经断开了，所有需要我们去更新
                                                                                            System.Buffer.BlockCopy(minref.Id, 0, MyTagPack.RD_New, 0, 2);
                                                                                            MyTagPack.SigStren = minref.Rssi;
                                                                                            MyTagPack.limitlossnum = 0;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            MyTagPack.limitlossnum++;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            #endregion
                                                                        }
                                                                    }
                                                                }
                                                                MyTagPack.BaseRefs.Clear();
                                                                MyTagPack.BasicRefsNum = 0;
                                                                #endregion
                                                                TagPack CurTagPack = CommonBoxOperation.CloneTagPack(MyTagPack);
                                                                try
                                                                {
                                                                    RecordOperation.SaveRecord(RecordOperation.ParseTagPack(MyTagPack));
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    FileOperation.WriteLog("保存记录文件出现异常，异常原因:" + ex.ToString());
                                                                }
                                                                MyTagPack.isAlarm = ReceiveCaching[start + 1];
                                                                MyTagPack.ResTime = (ReceiveCaching[start + 4] << 8 | ReceiveCaching[start + 5]);
                                                                MyTagPack.Bat = ReceiveCaching[start + 6];
                                                                MyTagPack.Sleep = (int)(ReceiveCaching[start + 7] << 8 | ReceiveCaching[start + 8]);
                                                                MyTagPack.StaticSleep = (int)(ReceiveCaching[start + 9] << 8 | ReceiveCaching[start + 10]);
                                                                MyTagPack.index = ReceiveCaching[start + 11];
                                                                for (int i = 0; i < ReceiveCaching[start + 12]; i++)
                                                                {
                                                                    byte[] id = new byte[2];
                                                                    System.Buffer.BlockCopy(ReceiveCaching, start + 13 + i * 3, id, 0, 2);
                                                                    strid = id[0].ToString("X2") + id[1].ToString("X2");
                                                                    RefReport mrefreport = new RefReport(id, ReceiveCaching[start + 13 + i * 3 + 2]);
                                                                    //添加Refer讯息到集合中去
                                                                    if (!MyTagPack.BaseRefs.ContainsKey(strid))
                                                                    {
                                                                        MyTagPack.BaseRefs.Add(strid, mrefreport);
                                                                        MyTagPack.BasicRefsNum++;
                                                                    }
                                                                }
                                                                if (MyTagPack.ResTime <= 0)
                                                                {//Tag有发生移动
                                                                    WarmInfo MyWarm = CommonBoxOperation.GetWarmItem(StrTagID, SpeceilAlarm.AreaControl);
                                                                    if (null != MyWarm && !MyWarm.isHandler)
                                                                    {//说明区域滞留控制发生改变
                                                                        if (!(MyWarm.RD[0].ToString("X2") + MyWarm.RD[1].ToString("X2")).Equals(MyTagPack.RD_New[0].ToString("X2") + MyTagPack.RD_New[1].ToString("X2")))
                                                                        {//说明此时靠近的Refer也发生改变，原来的区域滞留Referr认为已经处理了
                                                                            ((AreaAdmin)MyWarm).ClearAlarmTime = DateTime.Now;
                                                                            ((AreaAdmin)MyWarm).isHandler = true;
                                                                        }
                                                                    }
                                                                    //判断当前的Tag是否有解除卡片滞留警报资讯
                                                                    MyWarm = CommonBoxOperation.GetWarmItem(StrTagID, SpeceilAlarm.Resid);
                                                                    if (null != MyWarm && !MyWarm.isHandler)
                                                                    {
                                                                        //说明此时已经解除了滞留情况
                                                                        ((PersonRes)MyWarm).ClearAlarmTime = DateTime.Now;
                                                                        ((PersonRes)MyWarm).isHandler = true;
                                                                    }
                                                                }
                                                                MyTagPack.ReportTime = DateTime.Now;
                                                                MyTagPack.TickCount = Environment.TickCount;
                                                                //判断是否发生报警
                                                                JundgeAlarmInfo(MyTagPack);
                                                            }
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            #region Tag第一次上来数据包处理
                                                            //判断当前的Tag是否在设置的区域中
                                                            MyTagPack = new TagPack();
                                                            //获取Tag的ID讯息
                                                            System.Buffer.BlockCopy(ReceiveCaching, start + 2, MyTagPack.TD, 0, 2);
                                                            //讯号类型
                                                            MyTagPack.isAlarm = ReceiveCaching[start + 1];
                                                            //获取Gsensor讯息
                                                            MyTagPack.ResTime = ReceiveCaching[start + 4] << 8 | ReceiveCaching[start + 5];
                                                            //获取电池电量
                                                            MyTagPack.Bat = ReceiveCaching[start + 6];
                                                            //获取tag休眠时间
                                                            MyTagPack.Sleep = ReceiveCaching[start + 7] << 8 | ReceiveCaching[start + 8];
                                                            //获取tag静止时的休眠时间
                                                            MyTagPack.StaticSleep = ReceiveCaching[start + 9] << 8 | ReceiveCaching[start + 10];
                                                            MyTagPack.index = ReceiveCaching[start + 11];
                                                            MyTagPack.BasicRefsNum = 0;
                                                            for (int i = 0; i < ReceiveCaching[start + 12]; i++)
                                                            {
                                                                byte[] id = new byte[2];
                                                                System.Buffer.BlockCopy(ReceiveCaching, start + 13 + i * 3, id, 0, 2);
                                                                strid = id[0].ToString("X2") + id[1].ToString("X2");
                                                                RefReport mrefreport = new RefReport(id, ReceiveCaching[start + 13 + i * 3 + 2]);
                                                                //添加Refer讯息到集合中去
                                                                if (!MyTagPack.BaseRefs.ContainsKey(strid))
                                                                {
                                                                    MyTagPack.BaseRefs.Add(strid, mrefreport);
                                                                    MyTagPack.BasicRefsNum++;
                                                                }
                                                            }
                                                            #region 获取Tag的所有基站中信号最好的基站讯息
                                                            RefReport minref = MyTagPack.BaseRefs.Values.First();
                                                            foreach (KeyValuePair<string, RefReport> mrep in MyTagPack.BaseRefs)
                                                            {
                                                                if (null == mrep.Value)
                                                                {
                                                                    continue;
                                                                }
                                                                if (mrep.Value.Rssi < minref.Rssi)
                                                                {
                                                                    minref = mrep.Value;
                                                                }
                                                            }
                                                            if (null != minref)
                                                            {
                                                                System.Buffer.BlockCopy(minref.Id, 0, MyTagPack.RD_New, 0, 2);
                                                                MyTagPack.SigStren = minref.Rssi;
                                                            }
                                                            #endregion
                                                            //判断当前的Tag是否使用GSensor
                                                            MyTagPack.isUseGSensor = IsNeedGSensor(StrTagID);
                                                            MyTagPack.ReportTime = DateTime.Now;
                                                            MyTagPack.TickCount = Environment.TickCount;
                                                            JundgeAlarmInfo(MyTagPack);
                                                            //添加定位基站到集合
                                                            CommonCollection.TagPacks.TryAdd(StrTagID, MyTagPack);
                                                            #endregion
                                                        }
                                                    }
                                                    catch (Exception)
                                                    {

                                                    }
                                                }
                                                #endregion
                                            }
                                            dealPageOverDeleteStart(start, len);
                                        }
                                        else
                                        {
                                            start += 1;
                                        }
                                    }
                                }
                                else if (ReceiveCaching[start + 1] == ConstInfor.Type_Refer)
                                {
                                    if (start + 14 >= ReceiveCaching.Length)
                                    {
                                        start += 1;
                                    }
                                    else
                                    {
                                        if (ReceiveCaching[start + 14 - 1] == ConstInfor.End_Loca)
                                        {
                                            #region 处理Refer上报ID的数据包
                                            cs = 0;
                                            for (int i = start; i < start + 12; i++)
                                            {
                                                cs += ReceiveCaching[i];
                                            }
                                            if (cs == ReceiveCaching[start + 12])
                                            {
                                                String StrReportRouterID = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                WarmInfo MyWm = CommonBoxOperation.GetWarmItem(StrReportRouterID, SpeceilAlarm.ReferDis);
                                                if (null != MyWm && !MyWm.isHandler)
                                                {
                                                    MyWm.ClearAlarmTime = DateTime.Now;
                                                    MyWm.isHandler = true;
                                                    //从列表中删除这一条警报
                                                    if (SysParam.isDevCnnLoss)
                                                    {
                                                        if(null == MyWm.RDName || "".Equals(MyWm.RDName))
                                                        {
                                                            FileOperation.WriteLog(DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "----->參考點設備(ID: "+ StrReportRouterID +" )在" + MyWm.AlarmTime.Hour + ":" + MyWm.AlarmTime.Minute + ":" + MyWm.AlarmTime.Second + "時斷開連接," + MyWm.ClearAlarmTime.Hour + ":" + MyWm.ClearAlarmTime.Minute + ":" + MyWm.ClearAlarmTime.Second + "重新开始連接上...");
                                                        }
                                                        else
                                                        {
                                                            FileOperation.WriteLog(DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "----->參考點設備(ID: " + StrReportRouterID + " Name: " + MyWm.RDName + " )在" + MyWm.AlarmTime.Hour + ":" + MyWm.AlarmTime.Minute + ":" + MyWm.AlarmTime.Second + "時斷開連接," + MyWm.ClearAlarmTime.Hour + ":" + MyWm.ClearAlarmTime.Minute + ":" + MyWm.ClearAlarmTime.Second + "重新开始連接上...");
                                                        }

                                                        CommonCollection.WarmInfors.Remove(MyWm);
                                                        if (null != Frm.MyOterAlarmWin)
                                                        {
                                                            try
                                                            {
                                                                Frm.BeginInvoke(new Action(() =>
                                                                {
                                                                    Frm.MyOterAlarmWin.ReferDisListView.Items.Clear();
                                                                }));
                                                            }catch(Exception ex)
                                                            {

                                                                FileOperation.WriteLog("设备自动连接，清除警报列表时异常，异常原因：" + ex.ToString());
                                                            }
                                                        }
                                                    }
                                                }
                                                Router MyRouter = null;
                                                if (CommonCollection.Routers.TryGetValue(StrReportRouterID, out MyRouter))
                                                {
                                                    //FE + 02 + ID_c + Version + SleepTime + ID_j + CS + FD
                                                    MyRouter.SleepTime = (int)(ReceiveCaching[start + 8] << 8 | ReceiveCaching[start + 9]);
                                                    MyRouter.Version = (UInt32)(ReceiveCaching[start + 4] << 24 | ReceiveCaching[start + 5] << 16 | ReceiveCaching[start + 6] << 8 | ReceiveCaching[start + 7]);

                                                    System.Buffer.BlockCopy(ReceiveCaching, start + 10, MyRouter.BasicID, 0, 2);
                                                    strid = MyRouter.BasicID[0].ToString("X2") + MyRouter.BasicID[1].ToString("X2");
                                                    Router rt = null;
                                                    if (CommonCollection.Routers.TryGetValue(strid, out rt))
                                                    {
                                                        if (rt.CurType == NodeType.DataNode)
                                                        {
                                                            rt.mendpoint = endport;
                                                        }
                                                    }
                                                    MyRouter.mendpoint = endport;
                                                    MyRouter.CurType = NodeType.ReferNode;
                                                    MyRouter.ReportTime = DateTime.Now;
                                                    MyRouter.TickCount = Environment.TickCount;
                                                    MyRouter.status = true;
                                                    MyRouter.isHandler = false;
                                                }
                                                else
                                                {
                                                    MyRouter = new Router();
                                                    System.Buffer.BlockCopy(ReceiveCaching, start + 2, MyRouter.ID, 0, 2);
                                                    MyRouter.SleepTime = (int)(ReceiveCaching[start + 8] << 8 | ReceiveCaching[start + 9]);
                                                    System.Buffer.BlockCopy(ReceiveCaching, start + 10, MyRouter.BasicID, 0, 2);

                                                    strid = MyRouter.BasicID[0].ToString("X2") + MyRouter.BasicID[1].ToString("X2");
                                                    Router rt = null;
                                                    if (CommonCollection.Routers.TryGetValue(strid, out rt))
                                                    {
                                                        if (rt.CurType == NodeType.DataNode)
                                                        {
                                                            rt.mendpoint = endport;
                                                        }
                                                    }

                                                    MyRouter.mendpoint = endport;
                                                    MyRouter.Version = (UInt32)(ReceiveCaching[start + 4] << 24 | ReceiveCaching[start + 5] << 16 | ReceiveCaching[start + 6] << 8 | ReceiveCaching[start + 7]);
                                                    MyRouter.ReportTime = DateTime.Now;
                                                    MyRouter.TickCount = Environment.TickCount;
                                                    MyRouter.status = true;
                                                    MyRouter.isHandler = false;
                                                    MyRouter.CurType = NodeType.ReferNode;
                                                    CommonCollection.Routers.TryAdd(StrReportRouterID, MyRouter);
                                                }
                                            }
                                            #endregion
                                            dealPageOverDeleteStart(start, 14);
                                        }
                                        else
                                        {
                                            start += 1;
                                        }

                                    }
                                }
                                else if (ReceiveCaching[start + 1] == ConstInfor.Type_Node)
                                {
                                    if (start + 12 >= ReceiveCaching.Length)
                                    {
                                        start += 1;
                                    }
                                    else
                                    {
                                        if (ReceiveCaching[start + 12 - 1] == ConstInfor.End_Loca)
                                        {
                                            #region 处理Node上报ID的数据包
                                            cs = 0;
                                            for (int i = start; i < start + 10; i++)
                                            {
                                                cs += ReceiveCaching[i];
                                            }
                                            if (cs == ReceiveCaching[start + 10])
                                            {
                                                String StrReportRouterID = ReceiveCaching[start + 2].ToString("X2") + ReceiveCaching[start + 3].ToString("X2");
                                                WarmInfo MyWm = CommonBoxOperation.GetWarmItem(StrReportRouterID, SpeceilAlarm.NodeDis);
                                                if (null != MyWm && !MyWm.isHandler)
                                                {
                                                    MyWm.ClearAlarmTime = DateTime.Now;
                                                    MyWm.isHandler = true;
                                                    //基站上报以后，删除掉已经处理的警报
                                                    if (SysParam.isDevCnnLoss)
                                                    {
                                                        if (null == MyWm.RDName || "".Equals(MyWm.RDName))
                                                        {
                                                            FileOperation.WriteLog(DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "----->數據節點設備(ID: " + StrReportRouterID + " )在" + MyWm.AlarmTime.Hour + ":" + MyWm.AlarmTime.Minute + ":" + MyWm.AlarmTime.Second + "時斷開連接," + MyWm.ClearAlarmTime.Hour + ":" + MyWm.ClearAlarmTime.Minute + ":" + MyWm.ClearAlarmTime.Second + "重新开始連接上...");
                                                        }
                                                        else
                                                        {
                                                            FileOperation.WriteLog(DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "----->數據節點設備(ID: " + StrReportRouterID + " Name: " + MyWm.RDName + " )在" + MyWm.AlarmTime.Hour + ":" + MyWm.AlarmTime.Minute + ":" + MyWm.AlarmTime.Second + "時斷開連接," + MyWm.ClearAlarmTime.Hour + ":" + MyWm.ClearAlarmTime.Minute + ":" + MyWm.ClearAlarmTime.Second + "重新开始連接上...");
                                                        }
                                                        CommonCollection.WarmInfors.Remove(MyWm);
                                                        if (null != Frm.MyOterAlarmWin)
                                                        {
                                                            try
                                                            {
                                                                Frm.BeginInvoke(new Action(() =>
                                                                {
                                                                    Frm.MyOterAlarmWin.NodeDisListView.Items.Clear();
                                                                }));
                                                            }catch(Exception ex)
                                                            {
                                                                FileOperation.WriteLog("设备自动连接，清除警报列表时异常，异常原因：" + ex.ToString());
                                                            }
                                                        }
                                                    }
                                                }
                                                Router MyRouter = null;
                                                if (CommonCollection.Routers.TryGetValue(StrReportRouterID, out MyRouter))
                                                {
                                                    MyRouter.SleepTime = (int)(ReceiveCaching[start + 8] << 8 | ReceiveCaching[start + 9]);
                                                    MyRouter.Version = (UInt32)(ReceiveCaching[start + 4] << 24 | ReceiveCaching[start + 5] << 16 | ReceiveCaching[start + 6] << 8 | ReceiveCaching[start + 7]);
                                                    MyRouter.ReportTime = DateTime.Now;
                                                    MyRouter.CurType = NodeType.DataNode;
                                                    MyRouter.TickCount = Environment.TickCount;
                                                    MyRouter.status = true;
                                                    MyRouter.isHandler = false;
                                                    MyRouter.mendpoint = endport;
                                                }
                                                else
                                                {
                                                    MyRouter = new Router();
                                                    System.Buffer.BlockCopy(ReceiveCaching, start + 2, MyRouter.ID, 0, 2);
                                                    MyRouter.SleepTime = (int)(ReceiveCaching[start + 8] << 8 | ReceiveCaching[start + 9]);
                                                    MyRouter.Version = (UInt32)(ReceiveCaching[start + 4] << 24 | ReceiveCaching[start + 5] << 16 | ReceiveCaching[start + 6] << 8 | ReceiveCaching[start + 7]);
                                                    MyRouter.ReportTime = DateTime.Now;
                                                    MyRouter.CurType = NodeType.DataNode;
                                                    MyRouter.TickCount = Environment.TickCount;
                                                    MyRouter.status = true;
                                                    MyRouter.isHandler = false;
                                                    MyRouter.mendpoint = endport;
                                                    CommonCollection.Routers.TryAdd(StrReportRouterID, MyRouter);
                                                }
                                            }
                                            #endregion                                            
                                            dealPageOverDeleteStart(start,12);
                                        }
                                        else
                                        {
                                            start += 1;
                                        }
                                    }
                                }
                                else
                                {//说明类型有误，此时应该从下一个位置开始找start
                                    start += 1;
                                }
                            }
                        }
                    }
                }catch(Exception ex)
                {
                    FileOperation.WriteLog("数据包接收处理出现异常，异常原因为:"+ex.ToString());
                }
            }
        }

        /// <summary>
        /// fc 数据包处理完后，统一将缓存清理一下，鄙视一下前面员工，瞎搞，一大段下面的重复代码，垃圾
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        private static void dealPageOverDeleteStart(int start, int length)
        {
            if (CachingLen >= length)
            {
                System.Buffer.BlockCopy(ReceiveCaching, start + length, ReceiveCaching, start, CachingLen - start - length);
                CachingLen -= length;
            }
            else
            {
                start += 1;
            }
        }

        /// <summary>
        /// fc 数据包处理完后，统一将缓存清理一下，鄙视一下前面员工，瞎搞，一大段下面的重复代码，垃圾
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        private static void dealPageOver(int start,int length) 
        {
            if (CachingLen >= length)
            {
                System.Buffer.BlockCopy(ReceiveCaching, start + length, ReceiveCaching, start, CachingLen);
                CachingLen -= length;
            }
            else
            {
                start += 1;
            }
        }

        /// <summary>
        /// 通过打卡ID，获取卡片资料
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="endport"></param>
        private static void sendDaCardInfor(byte[] buffer, IPEndPoint endport)
        {
            byte[] buffers = new byte[20];
            Array.Copy(buffer, 0, buffers,0,20);
            new Thread(() => 
            {
                sendDaCardInforThread(buffers, endport);
            }).Start();            
        }

        private static void sendDaCardInforThread(byte[] buffer, IPEndPoint endport)
        {
            byte[] workID = new byte[16];
            Array.Copy(buffer, 2, workID, 0, 16);
            Dictionary<string, Tag> curTags = new Dictionary<string, Tag>(CommonCollection.Tags);
            Tag CurTag = null;
            foreach (var item in curTags)
            {
                Tag iTag = item.Value;
                if (XWUtils.byteBTBettow(iTag.workID, workID))
                {
                    CurTag = iTag;
                    break; ;
                }
            }
            byte[] data = getTagInfo(CurTag, 0xd8);
            //Frm.MyUdpClient.Send(data, data.Length, endport);
            Frm.udpSendData(data, data.Length, endport);
        }

        /// <summary>
        /// 发送当前的时间         
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="endport"></param>      
        private static void sendCurrentTime(byte[] buffer, IPEndPoint endport)
        {
            UInt32 currentTime = XwDataUtils.GetTimeStamp();
            byte[] timeBt = XwDataUtils.firstTimeByte(currentTime);
            byte[] sendDt = new byte[8];
            sendDt[0] = 0xfa;
            sendDt[1] = 0xd7;
            Array.Copy(timeBt, 0, sendDt, 2, timeBt.Length);
            sendDt[sendDt.Length - 2] = XWUtils.getCheckBit(sendDt, 0, sendDt.Length - 2);
            sendDt[sendDt.Length - 1] = 0xf9;
            if (sendDt != null) //Frm.MyUdpClient.Send(sendDt, sendDt.Length, endport);
                Frm.udpSendData(sendDt, sendDt.Length, endport);
        }

        /// <summary>
        /// 发送所有符合条件的账号密码
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="endport"></param>
        private static void setAllAccountPassword(byte[] buffer, IPEndPoint endport)
        {
            Dictionary<string, Person> Persons = new Dictionary<string, Person>(CommonCollection.Persons);
            
            foreach (var peoItem in Persons)
            {
                if (peoItem.Value.PersonAccess != PersonAccess.AdminPerson)
                {  //权限不够
                    continue;
                }
                byte[] account = XWUtils.getUTF8byte(peoItem.Value.Name);
                byte[] password = XWUtils.getUTF8byte(peoItem.Value.Ps);
                byte[] sendDt = new byte[8 + account.Length + password.Length];
                sendDt[0] = 0xfa;
                sendDt[1] = 0xd6;
                sendDt[2] = (byte)((account.Length / 0x100) % 0x100); sendDt[3] = (byte)(account.Length % 0x100);
                sendDt[4] = (byte)((password.Length / 0x100) % 0x100); sendDt[5] = (byte)(password.Length % 0x100);
                Array.Copy(account, 0, sendDt, 6, account.Length);
                Array.Copy(password, 0, sendDt, 6 + account.Length, password.Length);
                sendDt[sendDt.Length - 2] = XWUtils.getCheckBit(sendDt, 0, sendDt.Length - 2);
                sendDt[sendDt.Length - 1] = 0xf9;
                if (sendDt != null) Frm.udpSendData(sendDt, sendDt.Length, endport);
            }
        }

        /// <summary>
        /// 将账号密码发送
        /// </summary>
        private static void setAccountPassword(byte[] buffer, IPEndPoint endport) 
        {
            if(buffer.Length < 8) return;
            int accountLength = buffer[2] << 8 | buffer[3];
            int passWordLength = buffer[4] << 8 | buffer[5];
            byte[] account = new byte[accountLength];
            byte[] password = new byte[passWordLength];
            Array.Copy(buffer, 6, account, 0, accountLength);
            Array.Copy(buffer, 6 + accountLength, password, 0, passWordLength);
            String acc = XWUtils.getUTF8Str(account);
            String paWo = XWUtils.getUTF8Str(password);

            Dictionary<string, Person> Persons = new Dictionary<string,Person>(CommonCollection.Persons);

            byte[] sendDt = null;
            foreach(var peoItem in Persons)
            {
                if (!peoItem.Value.Name.Equals(acc)) continue;
                if (peoItem.Value.PersonAccess != PersonAccess.AdminPerson) 
                {  //权限不够
                    sendDt = getPassowrdResylt(buffer, accountLength, passWordLength, 2);
                    break;
                }
                if(peoItem.Value.Ps.Equals(paWo))
                { //正确
                    sendDt = getPassowrdResylt(buffer, accountLength, passWordLength, 0);
                }else
                {//错误
                    sendDt = getPasswordError(buffer, accountLength, account, peoItem.Value.Ps);
                }
                break;
            }
            if (sendDt == null) sendDt = getPassowrdResylt(buffer, accountLength, passWordLength, 3); // 找不到账号时
            if (sendDt != null) Frm.udpSendData(sendDt, sendDt.Length, endport);
        }

        private static byte[] getPassowrdResylt(byte[] buffer, int accountLength, int passWordLength,byte flag) 
        {
            byte[] sendDt = null;
            sendDt = new byte[accountLength + passWordLength+9];
            Array.Copy(buffer, 0, sendDt, 0, 6);
            sendDt[6] = flag;
            Array.Copy(buffer, 6, sendDt, 7, accountLength + passWordLength);
            sendDt[0] = 0xFa;
            sendDt[sendDt.Length - 2] = XWUtils.getCheckBit(sendDt, 0, sendDt.Length - 2);           
            sendDt[sendDt.Length - 1] = 0xf9;
            return sendDt;
        }

        /// <summary>
        /// 获取错误密码的流程，返回错误数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="accountLength"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static byte[] getPasswordError(byte[] buffer, int accountLength,byte[] account,String password) 
        {
            byte[] sendDt = null;
            byte[] passwordDt = XWUtils.getUTF8byte(password);
            sendDt = new byte[accountLength + passwordDt.Length + 9];
            Array.Copy(buffer, 0, sendDt, 0, 4);
            sendDt[4] = (byte)((passwordDt.Length / 0x100) % 0x100);
            sendDt[5] = (byte)(passwordDt.Length % 0x100);
            sendDt[6] = 1;
            Array.Copy(account, 0, sendDt, 7, accountLength);
            Array.Copy(passwordDt, 0, sendDt, 7 + accountLength, passwordDt.Length);
            sendDt[0] = 0xFa;
            sendDt[sendDt.Length - 2] = XWUtils.getCheckBit(sendDt, 0, sendDt.Length - 2);         
            sendDt[sendDt.Length - 1] = 0xf9;
            return sendDt;
        }

        /// <summary>
        /// 保存一份數據，表明這是Android打卡機的IP和內容
        /// </summary>
        /// <param name="endport"></param>
        private static void setAndroidendport(IPEndPoint endport) 
        {
            if (CommonCollection.Androidendport != null && CommonCollection.Androidendport.ToString().Equals(endport.ToString()))
            {
                return; //若是和保存的IPEndPoint一致，就不需要再往下操作了
            }
            CommonCollection.Androidendport = endport;
            SysParam.SaveAndroidendport();      
        }
       
        private static void sendPeopleInfo(byte[] buffer, IPEndPoint endport) 
        {
            // FC + CF + ID + CS + FB  
            // FA+ CF + tagID+workID+data+ CS + F9
            String StrTagID = buffer[2].ToString("X2") + buffer[3].ToString("X2");
            Tag CurTag = null;
            try
            {
                CommonCollection.Tags.TryGetValue(StrTagID, out CurTag);
            }
            catch (Exception)
            {
                return;
            }
            /*byte[] name = Encoding.UTF8.GetBytes(CurTag.Name);//crBean.Name
            byte[] data = new byte[name.Length + 22];
            data[0] = 0xfa;
            data[1] = 0xCF;
            data[2] = buffer[2];
            data[3] = buffer[3];
            Array.Copy(CurTag.workID, 0, data, 4, 16);
            Array.Copy(name, 0, data, 20, name.Length);
            for (int k = 0; k < data.Length - 2; k++)
            {
                data[data.Length - 2] += data[k];
            }       
               data[data.Length - 1] = 0xf9;*/
            byte[] data = getTagInfo(CurTag,0xcf);
            new Thread(() =>{
                Frm.udpSendData(data, data.Length, endport);
            }).Start();            
        }


        private static byte[] getTagInfo(Tag CurTag,byte packType) 
        {
            String ne = CurTag == null ? "no" : CurTag.Name;
            byte[] name = Encoding.UTF8.GetBytes(ne);//crBean.Name   
            byte[] data = new byte[name.Length + 22];
            data[0] = 0xfa;
            data[1] = packType;
            if (CurTag != null)
            {                           
                data[2] = CurTag.ID[0];
                data[3] = CurTag.ID[1];
                Array.Copy(CurTag.workID, 0, data, 4, 16);
                Array.Copy(name, 0, data, 20, name.Length);
            }
            for (int k = 0; k < data.Length - 2; k++)
            {
                data[data.Length - 2] += data[k];
            }
            data[data.Length - 1] = 0xf9;
            return data;
        }

        /// <summary>
        /// 将存储的卡片ID按要求发送出去。
        /// </summary>
        /// <param name="page"></param>
        /// <param name="endport"></param>
        private static void sendPeopleID(int page, IPEndPoint endport) 
        {
            // FC + CE + page + CS + FB       
            //Dictionary<string, Tag> tagIDs = new Dictionary<string, Tag>(CommonCollection.Tags);
            int pageCh = page;//保存现场，后面要用到
            page = page == 0 ? 1 : page;//保证page是从1开始的。0时，page置为1；
            List<Tag> tagIDs = CommonCollection.Tags.Values.ToList();
            int index = (page - 1) * 255 + 1;
            index = page == 1? 0 : index;
            int lastIndex = tagIDs.Count < page * 256 ? tagIDs.Count : page * 256;

            List<byte> idBts = new List<byte>();
            for (int i = index; i < lastIndex; i++)
            {
                try //防止异常，下标出现错误，导致程序崩掉
                {
                    Tag tag = tagIDs[i];
                    idBts.Add(tag.ID[0]);
                    idBts.Add(tag.ID[1]);
                }
                catch { }                
            }
            byte[] dataID = idBts.ToArray();
            byte[] data = new byte[6 + dataID.Length];
            data[0] = 0xfa;
            data[1] = 0xCE;
            data[2] = (byte)pageCh;
            data[3] = (byte)(dataID.Length / 2);
            Array.Copy(dataID, 0, data, 4, dataID.Length);
            for (int k = 0; k < data.Length - 2; k++) 
            {
                data[data.Length - 2] += data[k];
            }
            data[data.Length - 1] = 0xf9;
            new Thread(() => 
            {
                Frm.udpSendData(data, data.Length, endport);
            }).Start();
           
        }


        /// <summary>
        /// 上下班打卡
        /// </summary>
        /// <param name="start"></param>
        /// <param name="endport"></param>
        private static void sendWorkData(int start, IPEndPoint endport) 
        {
            byte[] sendBuffer = new byte[25];
            Array.Copy(ReceiveCaching, start, sendBuffer, 0, 22);
            sendBuffer[0] = 0xfa;
            for (int i = 0; i < sendBuffer.Length - 2; i++)
            {
                sendBuffer[23] += sendBuffer[i];
            }
            sendBuffer[24] = 0xf9;
            new Thread(() => {
                Frm.udpSendData(sendBuffer, sendBuffer.Length, endport);
            }).Start();           
        }

        /// <summary>
        /// 获取卡片上班时间，时间值是下位传上来的
        /// time : hh-mm-ss
        /// </summary>
        /// <param name="buffer"></param>
        private static void dealUpWorkTime(byte[] buffer, IPEndPoint endport) 
        {
            // FC + CC + ID + tine + CS + FB      
            String StrTagID = getTagIDformBuffer(buffer);
            Tag CurTag = null;
            try {
                CommonCollection.Tags.TryGetValue(StrTagID, out CurTag);
            }
            catch (Exception) {
                return;
            }
            if (CurTag == null) return;
            byte[] data = getTagInfo(CurTag, 0xd0);
            Frm.udpSendData(data, data.Length, endport);
            DateTime dt = getBuffDateTime(buffer);
            setAllAdmissionOrExit(CurTag, buffer, AdmissionExit.ADMISSION);
            if (dt.Year < CurTag.StartWorkDT.Year  || DateTime.Compare(dt, CurTag.StartWorkDT) <= 0) return;
            if (dt.Year == CurTag.StartWorkDT.Year && dt.DayOfYear < CurTag.StartWorkDT.DayOfYear) return; //同一天就不用再算上班了
            if (DateTime.Compare(dt, CurTag.EndWorkDT) > 0)   //已经打过下班卡，再打上班卡就是上班了
            {
                startWork(CurTag, dt, buffer);
            }
            else if (DateTime.Compare(dt, CurTag.StartWorkDT) > 0) //没有打过下班卡，看看时间是否晚于上一次上班卡
            {
               if (dt.Year > CurTag.StartWorkDT.Year) //若果是跨年，则算一次上班打卡，没办法日期比较就是这么烦
               {
                   startWork(CurTag, dt, buffer);
               }
               else if (dt.DayOfYear > CurTag.StartWorkDT.DayOfYear) //若果是过了一天了，再打卡就是第二天的上班时间
               {
                   startWork(CurTag, dt, buffer);
               }
            }      
        }

        private static void startWork(Tag CurTag, DateTime dt, byte[] buffer) 
        {
            CurTag.StartWorkDT = dt;

            CurTag.EndWorkDT = maxDt;
            setAdmissionOrExit(CurTag, buffer, AdmissionExit.ADMISSION);
            SysParam.SaveTag(0);
        }

        /// <summary>
        /// 存储所有的相关入场和出厂资料
        /// </summary>
        /// <param name="CurTag"></param>
        /// <param name="buffer"></param>
        /// <param name="model"></param>
        private static void setAllAdmissionOrExit(Tag CurTag, byte[] buffer, String model)
        {
            if (buffer.Length < 22) return;
            UInt32 upWorkTime = (UInt32)(buffer[18] << 24 | buffer[19] << 16 | buffer[20] << 8 | buffer[21]);
            new Thread(() =>
            {
                AdmissionExit admExit = new AdmissionExit(model);
                admExit.Name = CurTag.Name;
                admExit.Time = upWorkTime;
                admExit.TagID = CurTag.ID[0].ToString("X2") + CurTag.ID[1].ToString("X2");
                admExit.TagIDbyte = CurTag.ID;
                admExit.WorkIDbyte = CurTag.workID;
                lock (CommonCollection.admissionExits)
                {
                    CommonCollection.allAdmissionExits.Add(admExit);

                    FileModel.fileInit().saveHisAllAdmission();
                }                
            }).Start();
        }


        /// <summary>
        /// 存储相关入场和出厂资料
        /// </summary>
        /// <param name="CurTag"></param>
        /// <param name="buffer"></param>
        /// <param name="model"></param>
        private static void setAdmissionOrExit(Tag CurTag, byte[] buffer, String model) 
        {
            if (buffer.Length < 22) return;
            UInt32 upWorkTime = (UInt32)(buffer[18] << 24 | buffer[19] << 16 | buffer[20] << 8 | buffer[21]);
            new Thread(() => {
                AdmissionExit admExit = new AdmissionExit(model);
                admExit.Name = CurTag.Name;
                admExit.Time = upWorkTime;
                admExit.TagID = CurTag.ID[0].ToString("X2") + CurTag.ID[1].ToString("X2");
                admExit.TagIDbyte = CurTag.ID;
                admExit.WorkIDbyte = CurTag.workID;
                lock (CommonCollection.admissionExits)
                {
                    CommonCollection.admissionExits.Add(admExit);
                    FileModel.fileInit().saveHisAdmission();
                }                               
            }).Start();
        }

        /// <summary>
        /// 通过上报的打卡的ID找到tag的ID
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private static String getTagIDformBuffer(byte[] buffer)
        {
            byte[] workID = new byte[16];
            Array.Copy(buffer, 2, workID, 0, 16);
            return getTagIDformWorkID(workID);
        }

        /// <summary>
        /// 通过打卡的ID找到tag的ID
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private static String getTagIDformWorkID(byte[] buffer) 
        {
            if (buffer.Length < 16) return"";
            List<Tag> tags = CommonCollection.Tags.Values.ToList();
            byte[] workIDBuff = new byte[16];          
            Array.Copy(buffer, 0, workIDBuff,0,16);

            String idStr = getBtString(workIDBuff);
            Console.WriteLine(idStr);
            for (int i = 0; i < tags.Count;i++ )
            {
                if (!isBettonBt(workIDBuff, tags[i].workID)) continue;
                String tagIDString = tags[i].ID[0].ToString("X2") + tags[i].ID[1].ToString("X2");
                return tagIDString;
            }
            return "";
        }

        private static String getBtString(byte[] buf) 
        {
            StringBuilder ser = new StringBuilder();
            for (int i = 0; i < buf.Length;i++ )
            {
                ser.Append(buf[i].ToString("X2"));
            }
            return ser.ToString();
        }

        public static DateTime maxDt;

        public static DateTime getMaxDate()
        {
            byte[] endBuf = new byte[4] {0xf6, 0x65, 0xc8, 0x7f };
            DateTime endDt = getDateTime(endBuf);
            return endDt;
        }

        private static DateTime getBuffDateTime(byte[] buffer)
        {
            byte[] endBuf = new byte[4];
            Array.Copy(buffer, 18, endBuf,0,4);
            return getDateTime(endBuf);
        }

        private static DateTime getDateTime(byte[] buf) 
        {
            // FC + CC + ID + tine + CS + FB 
            UInt32 upWorkTime = (UInt32)(buf[0] << 24 | buf[1] << 16 | buf[2] << 8 | buf[3]);
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddSeconds(upWorkTime);
            return dt;
        }

        private static void dealUpSleepTime(byte[] buffer, IPEndPoint endport)
        {
            // FC + CD + ID + tine + CS + FB 
            String StrTagID = getTagIDformBuffer(buffer);
            Tag CurTag = null;
            try
            {
                CommonCollection.Tags.TryGetValue(StrTagID, out CurTag);
            }
            catch (Exception)
            {
                return;
            }
            if (CurTag == null) return;
            byte[] data = getTagInfo(CurTag, 0xd1);
            Frm.udpSendData(data, data.Length, endport);
            DateTime dt = getBuffDateTime(buffer);
            setAllAdmissionOrExit(CurTag, buffer, AdmissionExit.EXIT);
            if (dt.Month == CurTag.EndWorkDT.Month && dt.Day == CurTag.EndWorkDT.Day) // 两者取最小值
            {
                if (CurTag.EndWorkDT.CompareTo(DateTime.Now) <= 0)
                    return;
            }             
            setAdmissionOrExit(CurTag, buffer, AdmissionExit.EXIT);
            CurTag.EndWorkDT = dt;
        }

        public static String getBUffIdStr(byte[] workID)
        {
            String workIDs = "";
            for (int i = 0; i < workID.Length; i++)
            {
                try 
                {
                    workIDs += workID[i].ToString("X2");
                }
                catch { }               
            }
            return workIDs;
        }

        public static byte[] getworkIDFormTextBox(String boxValue)
        {
            byte text = 0;
            byte[] workID = new byte[16];
            if (boxValue == null) return workID;
            for (int i = (boxValue.Length / 2) - 1; i >= 0; i--)   //字符串最后面的应该优先，因为高位可能没有
            {
                try
                {
                    text = Convert.ToByte(boxValue.Substring(i * 2, 2), 16);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    text = 0;
                }
                if(i < 16) workID[i] = text;
            }
            return workID;
        }

        /// <summary>
        /// 判断两个数组内容是否是相等，如果数组为null判定为false
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool isBettonBt(byte[] buf, byte[] value) 
        {
            if(buf == null || value == null) return false;
            if (buf.Length != value.Length) return false;
            for (int i = 0; i < buf.Length;i++ )
            {
                if (buf[i] != value[i]) return false;
            }
            return true;
        }


        /// <summary>
        /// 关闭网络数据
        /// </summary>
        public static void StopNet()
        {
            if (null == Frm.MyUdpClient)
                return;
            Frm.MyUdpClient.Close();
            Frm.MyUdpClient = null;
        }

        public static void StopThread()
        {
            try
            {
                StopNet();
                Delay(10);
                if (null != Frm.UdpListenerThread)
                {
                    if (Frm.UdpListenerThread.IsAlive)
                        Frm.UdpListenerThread.Abort();
                }
                if (null != SysParam.SendMsgThread)
                    SysParam.SendMsgThread.Abort();
                DefinePlay.RemoveMediaPlayer(Frm);
                if (null != DefinePlay.axMediaplayer)
                    DefinePlay.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                SysParam.SendMsgThread = null;
                Frm.UdpListenerThread = null;
            }
        }

        //判断当前的记录是否需要保存
        private static bool isSaveHistoryRecord()
        {
            //获得当前的年月日
            DateTime CurDT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            int count = CommonCollection.RecordDB.Count;
            if(count <= 0)return false;
            TagPack lastpk = CommonCollection.RecordDB[count - 1];
            DateTime lastDT = new DateTime(lastpk.ReportTime.Year, lastpk.ReportTime.Month, lastpk.ReportTime.Day, lastpk.ReportTime.Hour, 0, 0);
            if ((CurDT - lastDT).TotalHours >= 1) return true;
            return false;
        }

        private static void SaveHistoryRecord()
        { 
            //查看记录保存文件是否存在
            if (!Directory.Exists(FileOperation.Original))Directory.CreateDirectory(FileOperation.Original);
            int count = CommonCollection.RecordDB.Count;
            if (count <= 0) return;
            TagPack lastpk = CommonCollection.RecordDB[count - 1];
            //判断最后一包时间的文件夹是否存在
            string StrDT = lastpk.ReportTime.Year.ToString().PadLeft(4, '0') +
                           lastpk.ReportTime.Month.ToString().PadLeft(2, '0') +
                           lastpk.ReportTime.Day.ToString().PadLeft(2, '0');
            string StrHour = lastpk.ReportTime.Hour.ToString().PadLeft(2, '0');

            List<TagPack> Oldtags = null;
            object obj = null;
            if (!Directory.Exists(FileOperation.Original + "\\" + StrDT))
            {//目录中不存在“年+月+天”的文件夹
                Directory.CreateDirectory(FileOperation.Original + "\\" + StrDT);
                Oldtags = new List<TagPack>();
            }
            else
            {
                if (File.Exists(FileOperation.Original + "\\" + StrDT + "\\" + StrHour + ".dat"))
                    DeserializeObject(out obj, FileOperation.Original + "\\" + StrDT + "\\" + StrHour + ".dat");
            }
            if (null != obj) Oldtags = obj as List<TagPack>;
            if (null == Oldtags) Oldtags = new List<TagPack>();
            TagPack tk = null;
            while (CommonCollection.RecordDB.Count > 0)
            {
                tk = CommonCollection.RecordDB[0];
                CommonCollection.RecordDB.RemoveAt(0);
                if (null == tk) continue;
                string StrDTH = tk.ReportTime.Year.ToString().PadLeft(4, '0') + tk.ReportTime.Month.ToString().PadLeft(2, '0')
                                + tk.ReportTime.Day.ToString().PadLeft(2, '0') + tk.ReportTime.Hour.ToString().PadLeft(2, '0');
                if (StrDTH.Equals(StrDT + StrHour)) Oldtags.Add(tk);
            }
            SeralizeObject(Oldtags, FileOperation.Original + "\\" + StrDT + "\\" + StrHour + ".dat");
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
            catch (Exception)
		    {
			}
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
            catch (Exception)
            {
                obj = null;
            }
            finally
            {
                if (null != fstr) 
				fstr.Close();
            }
        }
        /// <summary>
        /// 延时函数
        /// </summary>
        /// <param name="ms"></param>
        public static void Delay(int ms)
        {
            int TickCount = Environment.TickCount;
            while (Environment.TickCount - TickCount < ms)
            {
                Application.DoEvents();
            }
        }

        /// <summary>
        /// 判断当前的Tag是否可以接收
        /// </summary> 
        /// <param name="StrTagID"></param>
        /// <returns></returns>
        public static bool IsReceiveTag(string StrTagID)
        {
            Tag CurTag = null;
            try
            {
                CommonCollection.Tags.TryGetValue(StrTagID, out CurTag);
            }catch(Exception){}
            if (null == CurTag)
                return true;
            switch (CurTag.CurTagWorkTime)
            {
                case WorkTime.AlwaysWork:
                    return true;
                case WorkTime.LimitTime:
                    DateTime StartDT = CurTag.StartWorkDT;
                    DateTime EndDT = CurTag.EndWorkDT;
                    //DateTime StartDT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                     //                   CurTag.StartWorkDT.Hour, CurTag.StartWorkDT.Minute, 0);
                    //DateTime EndDT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                     //                   CurTag.EndWorkDT.Hour, CurTag.EndWorkDT.Minute, 59);    
                    if (DateTime.Compare(DateTime.Now, StartDT) >= 0 && DateTime.Compare(DateTime.Now, EndDT) < 0)
                        return true;
                    break;
                case WorkTime.NoWork:
                    return false;
                default: 
                    return true;
            }
            return false;
        }


        /// <summary>
        /// 判断当前的G-Sensor是否要工作
        /// </summary>
        /// <param name="StrTagID"></param>
        /// <returns></returns>
        public static bool IsNeedGSensor(string StrTagID)
        {
            return IsReceiveTag(StrTagID);  
            
            Tag CurTag = null;
            try
            {
                CommonCollection.Tags.TryGetValue(StrTagID, out CurTag);
            }
            catch (Exception) 
            { 

            }
            if (null == CurTag)
                return true;
            switch (CurTag.CurGSWorkTime)
            {
                case WorkTime.AlwaysWork:
                    return true;
                case WorkTime.LimitTime:
                    {
                        DateTime StartDT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                        CurTag.StartGSDT.Hour, CurTag.StartGSDT.Minute, 0);
                        DateTime EndDT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                           CurTag.EndGSDT.Hour, CurTag.EndGSDT.Minute, 59);
                        if (DateTime.Compare(DateTime.Now, StartDT) >= 0 && DateTime.Compare(DateTime.Now, EndDT) < 0)
                            return true;
                    }
                    break;
                case WorkTime.NoWork:
                    return false;
                default:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断该包数据是否应该发生报警
        /// 若应该发生报警，则把它添加到报警的集合中
        /// </summary>
        /// <param name="MyTagPack"></param>
        public static void JundgeAlarmInfo(TagPack MyTagPack)
        {
            String StrTagID = MyTagPack.TD[0].ToString("X2") + MyTagPack.TD[1].ToString("X2");
            //区域管制
            Tag tag = CommonBoxOperation.GetTag(StrTagID);
            String StrRouterID = MyTagPack.RD_New[0].ToString("X2") + MyTagPack.RD_New[1].ToString("X2");
            string StrRouterName = CommonBoxOperation.GetRouterName(StrRouterID);
            Area CurArea = CommonBoxOperation.GetAreaFromRouterID(StrRouterID);
            if(SysParam.isLowBattery)
            {
                #region 产生低电量警报
                WarmInfo MyWarmInfo = null;
                if (MyTagPack.Bat < SysParam.LowBattery)
                {
                    //低电量的讯息只需要保存一次即可
                    MyWarmInfo = CommonBoxOperation.GetWarmItem(StrTagID, SpeceilAlarm.BatteryLow);
                    if (null == MyWarmInfo || MyWarmInfo.isHandler)
                    {
                        MyWarmInfo = new BattLow();
                        System.Buffer.BlockCopy(MyTagPack.TD, 0, ((BattLow)MyWarmInfo).TD,0,2);
                        System.Buffer.BlockCopy(MyTagPack.RD_New, 0, ((BattLow)MyWarmInfo).RD,0,2);
                        if (null != StrRouterName && !"".Equals(StrRouterName))
                        {
                            MyWarmInfo.RDName = StrRouterName;
                        }
                        if (null != CurArea)
                        {
                            System.Buffer.BlockCopy(CurArea.ID, 0, MyWarmInfo.AD,0,2);
                            MyWarmInfo.AreaName = CurArea.Name;
                        }
                        ((BattLow)MyWarmInfo).Batt = MyTagPack.Bat;
                        ((BattLow)MyWarmInfo).BasicBatt = SysParam.LowBattery;
                        if (null != tag)
                        {
                            ((BattLow)MyWarmInfo).TagName = tag.Name;
                        }
                        ((BattLow)MyWarmInfo).AlarmTime = MyTagPack.ReportTime;
                        MyWarmInfo.ClearAlarmTime = MyTagPack.ReportTime;
                        ((BattLow)MyWarmInfo).isHandler = false;
                        try
                        {
                            CommonCollection.WarmInfors.Add(MyWarmInfo);
                        }catch(Exception)
                        {
                        }
                        if (SysParam.IsSoundAlarm && SysParam.isSoundBatteryLow)
                        {
                            if (SysParam.SoundName == ConstInfor.DefaultSoundAlarm)
                            {
                                if (!SoundOperation.isSoundPlay)
                                    SoundOperation.DefaultSoundPlay(SysParam.SoundTime);
                                else SoundOperation.PlayCount++;
                            }
                            else
                            {
                                Frm.BeginInvoke(new Action(() =>
                                {
                                    try
                                    {
                                        if (null == DefinePlay.axMediaplayer)
                                        {
                                            AxWMPLib.AxWindowsMediaPlayer axplayer = new AxWMPLib.AxWindowsMediaPlayer();
                                            axplayer.Visible = false;
                                            axplayer.BeginInit();
                                            Frm.Controls.Add(axplayer);
                                            axplayer.EndInit();
                                            if (SysParam.SoundTime == 0)
                                            {
                                                DefinePlay.SinglePlay(axplayer, FileOperation.SoundPath + "\\" + SysParam.SoundName);
                                            }
                                            else
                                            {
                                                DefinePlay.Play(axplayer, FileOperation.SoundPath + "\\" + SysParam.SoundName, SysParam.SoundTime);
                                            }
                                        }
                                        else
                                        {
                                            DefinePlay.RePlay();
                                        }
                                    }
                                    catch (Exception ex)
                                    { 
                                        FileOperation.WriteLog(DateTime.Now + " " + ex.ToString());
                                    }
                                }));
                            }
                        }
                    }
                }
                else
                {
                    MyWarmInfo = CommonBoxOperation.GetWarmItem(StrTagID, SpeceilAlarm.BatteryLow);
                    if (null != MyWarmInfo && !MyWarmInfo.isHandler)
                    {
                        MyWarmInfo.isHandler = true;
                        MyWarmInfo.ClearAlarmTime = DateTime.Now;
                    }
                }
                #endregion
            }
            //人员发生滞留
            if (null != tag && tag.IsStopAlarm)
            {
                #region 产生人员发生滞留警报
                if (MyTagPack.ResTime >= tag.StopTime && MyTagPack.isUseGSensor)
                {
                    WarmInfo MyWarmInfo = CommonBoxOperation.GetWarmItem(StrTagID,SpeceilAlarm.Resid);
                    if (null == MyWarmInfo || MyWarmInfo.isHandler)
                    {
                        MyWarmInfo = new PersonRes();
                        System.Buffer.BlockCopy(MyTagPack.TD, 0, ((PersonRes)MyWarmInfo).TD, 0, 2);
                        System.Buffer.BlockCopy(MyTagPack.RD_New, 0, MyWarmInfo.RD, 0, 2);
                        if (null != StrRouterName && !"".Equals(StrRouterName))
                        {
                            MyWarmInfo.RDName = StrRouterName;
                        }
                        if (null != CurArea)
                        {

                            System.Buffer.BlockCopy(CurArea.ID, 0, MyWarmInfo.AD, 0, 2);
                            MyWarmInfo.AreaName = CurArea.Name;
                        }
                        MyWarmInfo.AlarmTime = DateTime.Now;
                        MyWarmInfo.ClearAlarmTime = MyWarmInfo.AlarmTime;
                        MyWarmInfo.isHandler = false;
                        MyWarmInfo.isClear = false;
                        ((PersonRes)MyWarmInfo).ResTime = MyTagPack.ResTime;
                        ((PersonRes)MyWarmInfo).BasicResTime = tag.StopTime;

                        if (null != tag)
                        {
                            ((PersonRes)MyWarmInfo).TagName = tag.Name;
                        }
                        try
                        {
                            CommonCollection.WarmInfors.Add(MyWarmInfo);
                        }catch(Exception)
                        {
                        }
                        if (SysParam.IsSoundAlarm && SysParam.isSoundPersonRes)
                        {
                            if (SysParam.SoundName == ConstInfor.DefaultSoundAlarm)
                            {
                                if (!SoundOperation.isSoundPlay)
                                {
                                    SoundOperation.DefaultSoundPlay(SysParam.SoundTime);
                                }
                                else
                                {
                                    SoundOperation.PlayCount++;
                                }
                            }
                            else
                            {
                                Frm.BeginInvoke(new Action(() =>
                                {
                                    try
                                    {
                                        if (null == DefinePlay.axMediaplayer)
                                        {
                                            AxWMPLib.AxWindowsMediaPlayer axplayer = new AxWMPLib.AxWindowsMediaPlayer();
                                            axplayer.Visible = false;
                                            axplayer.BeginInit();
                                            Frm.Controls.Add(axplayer);
                                            axplayer.EndInit();
                                            if (SysParam.SoundTime == 0)
                                            {
                                                DefinePlay.SinglePlay(axplayer, FileOperation.SoundPath + "\\" + SysParam.SoundName);
                                            }
                                            else
                                            {
                                                DefinePlay.Play(axplayer, FileOperation.SoundPath + "\\" + SysParam.SoundName, SysParam.SoundTime);
                                            }
                                        }
                                        else
                                        {
                                            DefinePlay.RePlay();
                                        }
                                    }
                                    catch (Exception ex)
                                    { 
                                        FileOperation.WriteLog(DateTime.Now + " " + ex.ToString());
                                    }
                                }));
                            }
                        }
                    }
                    else if (null != MyWarmInfo && !MyWarmInfo.isHandler)
                    {   //说明滞留警报已经存在，并且没有处理
                        ((PersonRes)MyWarmInfo).ResTime = MyTagPack.ResTime;
                    }
                }
                #endregion
            }
            #region 判断是否有限制警报
            bool Mark = false;
            if (null != tag)
            {
                //判断当前tag是否靠近了不能靠近的参考点
                foreach(string strEnterID in tag.TagReliableList)
                {
                    if (StrRouterID.Equals(strEnterID))
                    {
                        Mark = true;
                        break;
                    }
                }
            }
            else
            {
                //当没有设置Tag的相关参数时表示这个Tag可以靠近所有的参考点
                Mark = true;
            }
            #endregion
            //卡片进入不应该进入的参考点
            if (!Mark)
            {
                #region 产生进入不能进入的Refer区域警报
                WarmInfo warif = CommonBoxOperation.GetWarmItem(StrTagID, SpeceilAlarm.AreaControl);
                if (null == warif || warif.isHandler)
                {//已经处理或者还没产生
                    WarmInfo MyWarmInfo = new AreaAdmin();
                    System.Buffer.BlockCopy(MyTagPack.TD, 0, ((AreaAdmin)MyWarmInfo).TD,0,2);
                    System.Buffer.BlockCopy(MyTagPack.RD_New, 0, MyWarmInfo.RD,0,2);
                    if (null != StrRouterName && !"".Equals(StrRouterName))
                    {
                        MyWarmInfo.RDName = StrRouterName;
                    }
                    
                    if (null != CurArea)
                    {
                        System.Buffer.BlockCopy(CurArea.ID, 0, MyWarmInfo.AD,0,2);
                        MyWarmInfo.AreaName = CurArea.Name;
                        ((AreaAdmin)MyWarmInfo).AreaType = CurArea.AreaType;
                    }
                    ((AreaAdmin)MyWarmInfo).AlarmTime = MyTagPack.ReportTime;
                    ((AreaAdmin)MyWarmInfo).ClearAlarmTime = MyTagPack.ReportTime;
                    if (null != tag)
                    { 
                        ((AreaAdmin)MyWarmInfo).TagName = tag.Name; 
                    }
                    ((AreaAdmin)MyWarmInfo).isHandler = false;
                    try
                    {
                        CommonCollection.WarmInfors.Add(MyWarmInfo);
                    }
                    catch (Exception)
                    {

                    }
                    if (SysParam.IsSoundAlarm && SysParam.isSoundAreaControl)
                    {
                        if (SysParam.SoundName == ConstInfor.DefaultSoundAlarm)
                        {
                            if (!SoundOperation.isSoundPlay)
                            {
                                SoundOperation.DefaultSoundPlay(SysParam.SoundTime);
                            }
                            else
                            {
                                SoundOperation.PlayCount++;
                            }
                        }
                        else
                        {
                            Frm.BeginInvoke(new Action(() =>
                            {
                                try
                                {
                                    if (null == DefinePlay.axMediaplayer)
                                    {
                                        AxWMPLib.AxWindowsMediaPlayer axplayer = new AxWMPLib.AxWindowsMediaPlayer();
                                        axplayer.Visible = false;
                                        axplayer.BeginInit();
                                        Frm.Controls.Add(axplayer);
                                        axplayer.EndInit();
                                        if (SysParam.SoundTime == 0) DefinePlay.SinglePlay(axplayer, FileOperation.SoundPath + "\\" + SysParam.SoundName);
                                        else DefinePlay.Play(axplayer, FileOperation.SoundPath + "\\" + SysParam.SoundName, SysParam.SoundTime);
                                    }
                                    else
                                    {
                                        DefinePlay.RePlay();
                                    }
                                }
                                catch (Exception ex)
                                { FileOperation.WriteLog(DateTime.Now + " " + ex.ToString()); }
                            }));
                        }
                    }
                }
                #endregion
            }
            //发生人员求救报警资讯
            if (MyTagPack.isAlarm == 0x04)
            {
                #region 产生人员求救警报，只要有人员求救的警报就添加进去
                //根据Tag的ID获取
                WarmInfo MyWarmInfo = new PersonHelp();
                System.Buffer.BlockCopy(MyTagPack.TD, 0, ((PersonHelp)MyWarmInfo).TD,0,2);
                System.Buffer.BlockCopy(MyTagPack.RD_New, 0, MyWarmInfo.RD,0,2);

                if (null != StrRouterName && !"".Equals(StrRouterName))
                {
                    MyWarmInfo.RDName = StrRouterName;
                }
                if (null != CurArea)
                {
                    System.Buffer.BlockCopy(CurArea.ID,0,MyWarmInfo.AD,0,2);
                    MyWarmInfo.AreaName = CurArea.Name;
                }
                MyWarmInfo.AlarmTime = MyTagPack.ReportTime;
                MyWarmInfo.ClearAlarmTime = MyTagPack.ReportTime;
                ((PersonHelp)MyWarmInfo).isHandler = false;
                if (null != tag)
                {
                    ((PersonHelp)MyWarmInfo).TagName = tag.Name;
                }
                try
                {
                    CommonCollection.WarmInfors.Add(MyWarmInfo);
                }catch(Exception)
                {

                }
                if (SysParam.IsSoundAlarm && SysParam.isSoundPersonHelp)
                {
                    if (SysParam.SoundName == ConstInfor.DefaultSoundAlarm)
                    {
                        if (!SoundOperation.isSoundPlay)
                        {
                            SoundOperation.DefaultSoundPlay(SysParam.SoundTime);
                        }
                        else
                        {
                            SoundOperation.PlayCount++;
                        }
                    }
                    else
                    {
                        Frm.BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                if (null == DefinePlay.axMediaplayer)
                                {
                                    AxWMPLib.AxWindowsMediaPlayer axplayer = new AxWMPLib.AxWindowsMediaPlayer();
                                    axplayer.Visible = false;
                                    axplayer.BeginInit();
                                    Frm.Controls.Add(axplayer);
                                    axplayer.EndInit();
                                    if (SysParam.SoundTime == 0)
                                    {
                                        DefinePlay.SinglePlay(axplayer, FileOperation.SoundPath + "\\" + SysParam.SoundName);
                                    }
                                    else
                                    {
                                        DefinePlay.Play(axplayer, FileOperation.SoundPath + "\\" + SysParam.SoundName, SysParam.SoundTime);
                                    }
                                }
                                else
                                {
                                    DefinePlay.RePlay();
                                }
                             }catch (Exception ex)
                             { 
                                 FileOperation.WriteLog("异步委托播放音频出现异常!异常原因: " + ex.ToString());
                             }
                        }));
                    }
                }
                #endregion
            }
        }
    }
        /// <summary>
        /// 画区域地图
        /// </summary>
        class DrawAreaMap
        {
            /// <summary>
            /// 将地图画到Panel的面板中
            /// </summary>
            /// <param name="mypanel"></param>
            /// <param name="bitmap"></param>
            public static void DrawMap(Panel mypanel, string strpath)
            {
                if (strpath == null || "".Equals(strpath))
                {
                    DrawNoMap(mypanel);
                    return;
                }
                try
                {
                    Bitmap bitmap = new Bitmap(FileOperation.MapPath+"\\"+strpath, true);
                    bitmap = new Bitmap(bitmap, mypanel.Width, mypanel.Height);
                    mypanel.CreateGraphics().DrawImageUnscaled(bitmap, 0, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            public static void DrawMap(Panel mypanel, Bitmap bitmap, string strpath)
            {
                Graphics g = Graphics.FromImage(bitmap);
                if (strpath == null || "".Equals(strpath))
                {
                    g.Clear(Color.White);
                    return;
                }
                try
                {
                    Bitmap Mybitmap = new Bitmap(FileOperation.MapPath+"\\"+strpath, true);
                    g.DrawImage(Mybitmap, 0, 0, bitmap.Width, bitmap.Height);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            /// <summary>
            /// 没有地图
            /// </summary>
            /// <param name="mypanel"></param>
            public static void DrawNoMap(Panel mypanel)
            {
                mypanel.CreateGraphics().DrawImageUnscaled(new Bitmap(Properties.Resources.NoArea), 0, 0, mypanel.Width, mypanel.Height);
            }

            /// <summary>
            /// 将该区域上的所有参考点添加到地图上
            /// mode:0为设置模式，1为浏览模式
            /// scale:面板放大
            /// </summary>
            /// <param name="panel"></param>
            public static void DrawBasicRouter(Bitmap bitmap, string AreaID, int mode,float wscale,float hscale)
            {
                if (null == AreaID || "".Equals(AreaID))
                    return;
                Area area = null;
                //画出该区域上的所有参考点
                if (!CommonCollection.Areas.TryGetValue(AreaID, out area))
                {
                    return;
                }
                string StrRouterName;
                foreach (KeyValuePair<string, BasicRouter> Br in area.AreaRouter)
                {
                    StrRouterName = Br.Value.Name;
                    if (mode == 1)
                    {
                        if (Br.Value.isVisible)
                        {
                            DrawSingleRouter(bitmap, Br.Value, Br.Value.x * wscale, Br.Value.y * hscale);
                        }
                    }
                    else
                    {
                        DrawSingleRouter(bitmap, Br.Value, Br.Value.x * wscale, Br.Value.y * hscale);
                    }
                }
                string StrNodeName;
                foreach (KeyValuePair<string, BasicNode> Bn in area.AreaNode)
                {
                    StrNodeName = Bn.Value.Name;
                    if (mode == 1)
                    {
                        if (Bn.Value.isVisible)
                        {
                            DrawSingleNode(bitmap, Bn.Value, Bn.Value.x * wscale, Bn.Value.y * hscale);
                        }
                    }
                    else
                    {
                        DrawSingleNode(bitmap, Bn.Value, Bn.Value.x * wscale, Bn.Value.y * hscale);
                    }
                }
            }

            /// <summary>
            /// 将该区域上的所有参考点添加到地图上
            /// mode:0为设置模式，1为浏览模式
            /// </summary>
            /// <param name="panel"></param>
            public static void DrawBasicRouter(Bitmap bitmap, string AreaID, int mode)
            {
                if (null == AreaID || "".Equals(AreaID))
                    return;
                Area area = null;
                //画出该区域上的所有参考点
                if (!CommonCollection.Areas.TryGetValue(AreaID, out area))return;
                string StrRouterName;
                foreach (KeyValuePair<string, BasicRouter> Br in area.AreaRouter)
                {
                    StrRouterName = Br.Value.Name;
                    if (mode == 1)
                    {
                        if (Br.Value.isVisible)
                        {

                            DrawSingleRouter(bitmap, Br.Value,Br.Value.x,Br.Value.y);
                        }
                    }
                    else
                    {
                        DrawSingleRouter(bitmap, Br.Value, Br.Value.x, Br.Value.y);
                    }
                }
                string StrNodeName;
                foreach (KeyValuePair<string, BasicNode> Bn in area.AreaNode)
                {
                    StrNodeName = Bn.Value.Name;
                    if (mode == 1)
                    {
                        if (Bn.Value.isVisible)
                        {
                            DrawSingleNode(bitmap, Bn.Value,Bn.Value.x,Bn.Value.y);
                        }
                    }
                    else
                    {
                        DrawSingleNode(bitmap, Bn.Value, Bn.Value.x, Bn.Value.y);
                    }
                }
            }

            public static void DrawSingleRouter(Bitmap bitmap,BasicRouter router,float x,float y)
            {
                Brush routerbrush = Brushes.Blue;
                Graphics g = Graphics.FromImage(bitmap);
                string str = router.Name;
                if (null == str || "".Equals(str))
                {
                    str = router.ID[0].ToString("X2") + router.ID[1].ToString("X2");
                }
                if (!router.isReport)
                {
                    routerbrush = Brushes.Black;
                }
                else
                {
                    Router mrter = null;
                    if (CommonCollection.Routers.TryGetValue(router.ID[0].ToString("X2") + router.ID[1].ToString("X2"), out mrter))
                    {
                        if (!mrter.status && mrter.CurType == NodeType.ReferNode)
                        {
                            routerbrush = Brushes.Black;
                        }
                    }
                }
                StringFormat strformat = new StringFormat();
                strformat.Alignment = StringAlignment.Far;
                g.FillRectangle(routerbrush, x - (ConstInfor.RouterWidth / 2), y - (ConstInfor.RouterHeight / 2), ConstInfor.RouterWidth, ConstInfor.RouterHeight);
                g.DrawString(str, new Font("宋体", 10, FontStyle.Regular), routerbrush, x - 20, y - (ConstInfor.RouterHeight / 2) + 3, strformat);
                g.DrawString("參考點", new Font("宋体", 9, FontStyle.Regular), Brushes.White, x - 19, y - 6);
            }

            /// <summary>
            /// 向地图中添加单个参考点
            /// </summary>
            /// <param name="bitmap"></param>
            /// <param name="ID"></param>
            /// <param name="X"></param>
            /// <param name="Y"></param>
            /// 
            public static void DrawSingleNode(Bitmap bitmap,BasicNode node,float x,float y)
            {
                Brush nodebrush = Brushes.Red;
                Graphics g = Graphics.FromImage(bitmap);
                string str = node.Name;
                if (null == str || "".Equals(str))
                {
                    str = node.ID[0].ToString("X2") + node.ID[1].ToString("X2");
                }
                if (!node.isReport)
                {
                    nodebrush = Brushes.Black;
                }
                else
                {
                    Router mrter = null;
                    if (CommonCollection.Routers.TryGetValue(node.ID[0].ToString("X2") + node.ID[1].ToString("X2"), out mrter))
                    {
                        if (!mrter.status && mrter.CurType == NodeType.DataNode)
                        {
                            nodebrush = Brushes.Black;
                        }
                    }
                }

                StringFormat strformat = new StringFormat();
                strformat.Alignment = StringAlignment.Far;
                g.FillRectangle(nodebrush, x - (ConstInfor.DataNodeWidth / 2), y - (ConstInfor.DataNodeHeight / 2), ConstInfor.DataNodeWidth, ConstInfor.DataNodeHeight);
                g.DrawString(str, new Font("宋体", 10, FontStyle.Regular), nodebrush, x - 25, y - (ConstInfor.DataNodeHeight / 2) + 3, strformat);
                g.DrawString("數據節點", new Font("宋体", 9, FontStyle.Regular), Brushes.White, x - 25, y - 6);
            }

            /// <summary>
            /// 根据参考点的ID获取参考点的名称
            /// </summary>
            /// <param name="ID"></param>
            /// <returns></returns>
            public static string GetRouterName(String StrID)
            {
                BasicRouter Br = null;
                    foreach (KeyValuePair<string, Area> area in CommonCollection.Areas)
                    {
                        if (null == area.Value.AreaRouter)
                            continue;
                        Br = null;
                        area.Value.AreaRouter.TryGetValue(StrID, out Br);
                        if (null == Br)
                            continue;
                        return Br.Name;

                    }
                
                return null;
            }
            /// <summary>
            /// 参考点的ID获取指定区域参考点的名称
            /// </summary>
            /// <param name="StrID"></param>
            /// <param name="StrAreaID"></param>
            /// <returns></returns>
            public static string GetRouterName(string StrID, byte[] AreaID)
            {
                if (null == AreaID)
                    return null;
                String StrAreaID = AreaID[0].ToString("X2") + AreaID[1].ToString("X2");
                Area MyArea = null;
                    CommonCollection.Areas.TryGetValue(StrAreaID, out MyArea);
                if (null == MyArea)
                    return null;
                if (null == MyArea.AreaRouter)
                    return null;
                BasicRouter Bscr = null;
                MyArea.AreaRouter.TryGetValue(StrID, out Bscr);
                if (null == Bscr)
                    return null;
                return Bscr.Name;
            }
            public static Bitmap GetBitmap(string MapPath)
            {
                if ("".Equals(MapPath))
                    return null;
                Bitmap bitmap = null;
                try
                {
                    bitmap = new Bitmap(MapPath, false);
                    bitmap = new Bitmap(bitmap, ConstInfor.MapWidth, ConstInfor.MapHeight);
                }
                catch (Exception)
                {
                }
                return bitmap;
            }
        }
    }