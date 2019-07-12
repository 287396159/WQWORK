using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using SoundPlayer = System.Media.SoundPlayer;
using System.Media;
using System.Windows.Forms;
namespace PersionAutoLocaSys
{
    class SoundOperation
    {
        public static bool isSoundPlay = false;
        public static int PlayCount = 0;
        public static int SoundPlayTime = 0;
        /// <param name="iFrequency">声音频率（从37Hz到32767Hz）。在windows95中忽略</param>  
        /// <param name="iDuration">声音的持续时间，以毫秒为单位。</param>  
        [DllImport("Kernel32.dll")] //引入命名空间 using System.Runtime.InteropServices;  
        public static extern bool Beep(int frequency, int duration);
        /// <summary>
        /// 播放系统默认的警报声
        /// </summary>
        public static void DefaultSoundPlay(int Time)
        {
            PlayCount = 1;
            isSoundPlay = true;
            new Thread((object Param)=>
            {
                for (int i = 0; i < PlayCount; i++)
                {
                    SoundPlayTime = Time;//单次播放的时间Time 
                    while (SoundPlayTime >= 0)
                    {
                        Beep(500, 700);
                        Thread.Sleep(3000);
                        SoundPlayTime -= 4;//休眠3s + 声音1s
                    }
                }
                isSoundPlay = false;
            }).Start();
        }
        public static void StopSoundPlay()
        {
            PlayCount = 0;
            isSoundPlay = false;
            SoundPlayTime = 0;
        }
    }

    public class DefinePlay
    {
        public static AxWMPLib.AxWindowsMediaPlayer axMediaplayer = null;
        private static System.Threading.Thread playthread = null;
        private const UInt32 waittime = 5000;
        private static UInt32 curwait = 0;
        private static UInt32 playtotalcount = 0;
        private static UInt32 curcount = 0;
        private static double curtime = 0; 
        private static double offtime = 0;
        
        public static void SinglePlay(AxWMPLib.AxWindowsMediaPlayer axplayer, string AudioAddress)
        {
            try
            {
                axMediaplayer = axplayer;
                curwait = 0;
                axplayer.URL = AudioAddress;
                axplayer.settings.autoStart = true;
            }catch(Exception ex)
            {throw ex;}
            while (axplayer.playState != WMPLib.WMPPlayState.wmppsPlaying && curwait < waittime)
            {
                System.Threading.Thread.Sleep(10);
                System.Windows.Forms.Application.DoEvents();
                curwait += 10;
            }
            if(axplayer.playState != WMPLib.WMPPlayState.wmppsPlaying)Close();
            axplayer.StatusChange += axsingleplayer_StatusChange;
        }
        public static void RePlay()
        { 
            //重新开始播放
            axMediaplayer.Ctlcontrols.currentPosition = 0;
            curcount = 0;
        }
        public static void Play(AxWMPLib.AxWindowsMediaPlayer axplayer, string AudioAddress, int PlayTime)
        {
            try
            {
                axMediaplayer = axplayer;
                curwait = 0;
                axplayer.URL = AudioAddress;
                axplayer.settings.autoStart = true;
                axplayer.settings.setMode("loop", true);
            }
            catch (Exception ex) 
            { 
                throw ex;
            }
            try
            {
                while (axplayer.playState != WMPLib.WMPPlayState.wmppsPlaying && curwait < waittime)
                {
                    System.Threading.Thread.Sleep(10);
                    System.Windows.Forms.Application.DoEvents();
                    curwait += 10;
                }
                if (axplayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
                {
                    Close();
                }
            }catch(Exception ex)
            {
                FileOperation.WriteLog("播放音频文件出错!错误原因：" + ex.ToString());
            }
            double totaltime = axplayer.currentMedia.duration;
            if (PlayTime < totaltime)
            {
                playthread = new Thread((object param) =>
                {
                    curtime = axplayer.Ctlcontrols.currentPosition;
                    while (curtime < PlayTime)
                    {
                        Thread.Sleep(10);
                        if (null == axplayer) break;
                        curtime = axplayer.Ctlcontrols.currentPosition;
                    }
                    Close();
                });
                playthread.Start();
            }
            else
            {
                //此时播放的时间要大于音乐的总时间
                //判断需要播放的次数
                playtotalcount = (UInt32)(PlayTime / totaltime);
                offtime = PlayTime % totaltime;
                axplayer.StatusChange += axplayer_StatusChange;
            }

        }
        //删除添加的控件
        public static void RemoveMediaPlayer(Form frm)
        {
            string StrControlType = "";
            for (int i = 0; i < frm.Controls.Count; i++)
            {
                StrControlType = frm.Controls[i].GetType().ToString();
                if ("AxWMPLib.AxWindowsMediaPlayer".Equals(StrControlType))
                {
                    frm.Controls.RemoveAt(i);
                }
            }
        }
        public static void Close()
        {
            if (axMediaplayer == null)
                return;
            curcount = 0;
            axMediaplayer.Ctlcontrols.stop();
            axMediaplayer.close();
            //axMediaplayer.Dispose();
            axMediaplayer = null;
            if (null != playthread)
            {
                try
                {
                    if (playthread.IsAlive)
                    {
                        playthread.Abort();
                    }
                }
                catch (Exception ex)
                { 
                    Console.WriteLine(ex.ToString());
                }
                finally 
                {
                    playthread = null;
                }
            }
        }
        private static void axplayer_StatusChange(object obj, EventArgs args)
        {
            switch (axMediaplayer.playState)
            {
                case WMPLib.WMPPlayState.wmppsMediaEnded:
                 
                    curcount++;
                    if (curcount < playtotalcount) { return; }
                    if (null != playthread) return;
                    playthread = new Thread((object param) =>
                    {
                        double totaltime = axMediaplayer.currentMedia.duration;
                        if (offtime < totaltime)
                        {
                            double curtime = axMediaplayer.Ctlcontrols.currentPosition;
                            while (curtime < offtime)
                            { 
                                Thread.Sleep(10);
                                if (null == axMediaplayer) break;
                                curtime = axMediaplayer.Ctlcontrols.currentPosition; 
                            }
                            Close();
                        }
                    });
                    playthread.Start();
                    break;
                case WMPLib.WMPPlayState.wmppsPlaying:

                    break;
                case WMPLib.WMPPlayState.wmppsWaiting:
         
                    break;
                default:
                    break;
            }
        }
        //单次播放的回调函数
        private static void axsingleplayer_StatusChange(object obj, EventArgs args)
        {
            switch (axMediaplayer.playState)
            {
                case WMPLib.WMPPlayState.wmppsMediaEnded:
          
                    Close();
                    break;
                case WMPLib.WMPPlayState.wmppsPlaying:
        
                    break;
                case WMPLib.WMPPlayState.wmppsWaiting:
            
                    break;
                default:
                    break;
            }
        }
    }
    /*
    /// <summary> 
    /// clsMci 的摘要说明。 
        /// </summary> 
    public class clsMCI
    {
        public clsMCI()
        {
        }
        //定义API函数使用的字符串变量  
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        private string Name = "";
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        private string durLength = "";
        [MarshalAs(UnmanagedType.LPTStr, SizeConst = 128)]
        private string TemStr = "";
        int ilong;
        //定义播放状态枚举变量 
        public enum State
        {
            mPlaying = 1,
            mPuase = 2,
            mStop = 3
        };
        //结构变量 
        public struct structMCI
        {
            public bool bMut;
            public int iDur;
            public int iPos;
            public int iVol;
            public int iBal;
            public string iName;
            public State state;
        };
        public structMCI mc = new structMCI();
        //取得播放文件属性 
        public string FileName
        {
            get
            {
                return mc.iName;
            }
            set
            {
                try
                {
                    TemStr = "";
                    TemStr = TemStr.PadLeft(127, Convert.ToChar(" "));
                    Name = Name.PadLeft(260, Convert.ToChar(" "));
                    mc.iName = value;
                    ilong = APIClass.GetShortPathName(mc.iName, Name, Name.Length);
                    Name = GetCurrPath(Name);
                    Name = "open " + Convert.ToChar(34) + Name + Convert.ToChar(34) + " alias media";
                    ilong = APIClass.mciSendString("close all", TemStr, TemStr.Length, 0);
                    ilong = APIClass.mciSendString(Name, TemStr, TemStr.Length, 0);
                    ilong = APIClass.mciSendString("set media time format milliseconds", TemStr, TemStr.Length, 0);
                    mc.state = State.mStop;
                }
                catch
                {
                }
            }
        }
        //播放 
        public void play()
        {
            TemStr = "";
            TemStr = TemStr.PadLeft(127, Convert.ToChar(" "));
            APIClass.mciSendString("play media", TemStr, TemStr.Length, 0);
            mc.state = State.mPlaying;
        }
        //停止 
        public void StopT()
        {
            TemStr = "";
            TemStr = TemStr.PadLeft(128, Convert.ToChar(" "));
            ilong = APIClass.mciSendString("close media", TemStr, 128, 0);
            ilong = APIClass.mciSendString("close all", TemStr, 128, 0);
            mc.state = State.mStop;
        }
        public void Puase()
        {
            TemStr = "";
            TemStr = TemStr.PadLeft(128, Convert.ToChar(" "));
            ilong = APIClass.mciSendString("pause media", TemStr, TemStr.Length, 0);
            mc.state = State.mPuase;
        }
        private string GetCurrPath(string name)
        {
            if (name.Length < 1) return "";
            name = name.Trim();
            name = name.Substring(0, name.Length - 1);
            return name;
        }
        //总时间 
        public int Duration
        {
            get
            {
                durLength = "";
                durLength = durLength.PadLeft(128, Convert.ToChar(" "));
                APIClass.mciSendString("status media length", durLength, durLength.Length, 0);
                durLength = durLength.Trim();
                if (durLength == "") return 0;
                return (int)(Convert.ToDouble(durLength) / 1000f);
            }
        }

        //当前时间 
        public int CurrentPosition
        {
            get
            {
                durLength = "";
                durLength = durLength.PadLeft(128, Convert.ToChar(" "));
                APIClass.mciSendString("status media position",durLength,durLength.Length,0);
                mc.iPos = (int)(Convert.ToDouble(durLength)/1000f);
                return mc.iPos;
            }
        }
    }
    public class APIClass
    {
            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern int GetShortPathName(
             string lpszLongPath,
             string shortFile,
             int cchBuffer
            );

            [DllImport("winmm.dll", EntryPoint = "mciSendString", CharSet = CharSet.Auto)]
            public static extern int mciSendString(
             string lpstrCommand,
             string lpstrReturnString,
             int uReturnLength,
             int hwndCallback
            );
   }*/
}

