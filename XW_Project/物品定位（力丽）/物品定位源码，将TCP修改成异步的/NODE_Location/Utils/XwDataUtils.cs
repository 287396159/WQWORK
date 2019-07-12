using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialportSample
{
    class XwDataUtils
    {

        /// <summary>  
        /// 获取当前时间戳  
        /// </summary>  
        /// <returns>获取10位long型时间戳</returns>  
        public static UInt32 GetTimeStamp(){
            return GetTimeStamp(true);
        }


        /// <summary>  
        /// 获取当前时间戳  
        /// </summary>  
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.</param>  
        /// <returns></returns>  
        public static UInt32 GetTimeStamp(bool bflag){
            return GetTimeStamp(DateTime.UtcNow,bflag);            
        }

        public static UInt32 GetTimeStamp(DateTime UtcNow){
            return GetTimeStamp(UtcNow, true);
        }

        /// <summary>  
        /// 获取当前时间戳  
        /// </summary>  
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.</param>  
        /// <returns></returns>  
        public static UInt32 GetTimeStamp(DateTime UtcNow,bool bflag) {
            TimeSpan ts = UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            UInt32 time = 0;
            if (bflag)
                time = Convert.ToUInt32(ts.TotalSeconds);
            else
                time = (UInt32)Convert.ToInt64(ts.TotalMilliseconds);
            return time;
        }


        /// <summary>
        /// 获取当前时间的13位长度的时间戳
        /// </summary>
        /// <returns></returns>
        public static Int64 GetLongTimeStamp() {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            Int64 time = Convert.ToInt64(ts.TotalMilliseconds);
            return time;
        }


        /// <summary>
        /// 时间戳转换成时间
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        public static string dataFromTimeStamp(Int32 unixTimeStamp){
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddSeconds(unixTimeStamp);
            return dt.ToString("yyyy年MM月dd HH:mm:ss");
            //System.Console.WriteLine(dt.ToString("yyyy年MM月dd HH:mm:ss"));
        }


        /// <summary>
        /// 出厂时间戳
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        public static Int32 FactoryTimeTimeStamp(){
            TimeSpan ts = new DateTime(2017, 4, 1,0,0,0) - new DateTime(1970, 1, 1, 0, 0, 0);
            Int32 time = Convert.ToInt32(ts.TotalSeconds);
            return time;
        }


        public static string FactoryTime(){
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddSeconds(FactoryTimeTimeStamp());

            //DateTime dt = new DateTime();
            return dt.ToString("yyyy年MM月dd日");
        }


        public static string currentTime() {

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddSeconds(GetTimeStamp());

            //DateTime dt = new DateTime();
            return dt.ToString("yyyy年MM月dd日");
        }

        public static string currentMonthTime()
        {

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddSeconds(GetTimeStamp());           
            //DateTime dt = new DateTime();
            return dt.ToString("yyyy年MM月");
        }


        public static bool oneDayTime(long time,long time2)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt1 = startTime.AddSeconds(time);
            DateTime dt2 = startTime.AddSeconds(time2);
            //DateTime dt = new DateTime();
            return dt1.Day == dt2.Day;
        }


        public static string currentTimeToSe()
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddSeconds(GetTimeStamp());
            return dt.ToString("yyyy年MM月dd HH:mm:ss");
        }


        public static string currentTimeToSe(long time)
        {
            return currentTimeToSe(time, "yyyy年MM月dd HH:mm:ss");
            //System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            //DateTime dt = startTime.AddSeconds(time);
            //return dt.ToString();
        }

        public static string currentTimeToSe(long time,string format)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddSeconds(time);
            return dt.ToString(format);
        }

        /// <summary>
        /// 10位数字的时间戳转换成4个字节
        /// </summary>
        /// <param name="firstReceiveTime"></param>
        /// <returns></returns>
        public static byte[] firstTimeByte(long firstReceiveTime)
        {            
            byte[] timeByte = new byte[4];
            timeByte[0] = (byte)(firstReceiveTime / 0x1000000);
            timeByte[1] = (byte)(firstReceiveTime % 0x1000000 / 0x10000);
            timeByte[2] = (byte)(firstReceiveTime % 0x10000 / 0x100);
            timeByte[3] = (byte)(firstReceiveTime % 0x100);
            return timeByte;            
        }


    }
}
