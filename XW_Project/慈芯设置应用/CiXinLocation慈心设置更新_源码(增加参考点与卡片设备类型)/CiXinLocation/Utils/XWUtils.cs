using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SerialportSample
{
    class XWUtils{

        /// <summary>
        /// 字符串转int32
        /// </summary>
        /// <returns>错误返回-1</returns>
        public static Int32 stringToInt32(string int32String) {
            Int32 i32 = -1;
            try {
                i32 = Int32.Parse(int32String);
            }
            catch { }           
            return i32;
        }


        /// <summary>
        /// 字符串转byte
        /// </summary>
        /// <param name="byte32String"></param>
        /// <returns>错误返回-1</returns>
        public static byte stringToByte(string byte32String){
            byte by = 0;
            try{
                by = byte.Parse(deleteFir0(byte32String));
            }
            catch { }
            return by;
        }


     public static string deleteFir0(string byte32String){
            if ("0".Equals(byte32String)) return byte32String;
            if (byte32String.Substring(0, 1).Equals("0")) {
                return deleteFir0(byte32String.Substring(1, byte32String.Length - 1));
            }
            return byte32String;
        }

        /// <summary>
        /// 16进制字符串转byte
        /// </summary>
        /// <param name="byte32String"></param>
        /// <returns>错误返回-1</returns>
        public static byte hexStrToByte(string byte32String){
            byte by = 0;
            try{
                by = Convert.ToByte(byte32String,16);
            }
            catch { }
            return by;
        }


        /// <summary>
        /// 字符串转int,错误返回0
        /// </summary>
        /// <param name="int32String"></param>
        /// <returns>错误返回0</returns>
        public static int stringToInt(string int32String){
            int in1 = 0;
            try{
                in1 = int.Parse(int32String);
            }
            catch { }
            return in1;
        }


        /// <summary>
        /// 字符串转int,错误返回-1
        /// </summary>
        /// <param name="int32String"></param>
        /// <returns>错误返回0</returns>
        public static int stringToInt1(string int32String)
        {
            int in1 = -1;
            try
            {
                in1 = int.Parse(int32String);
            }
            catch { }
            return in1;
        }


        /// <summary>
        /// 字符串转int,错误返回-1
        /// </summary>
        /// <param name="int32String"></param>
        /// <returns>错误返回0</returns>
        public static double stringToDouble1(string int32String)
        {
            double in1 = -1;
            try
            {
                in1 = double.Parse(int32String);
            } catch { }
            return in1;
        }


        /// <summary>
        /// 计算校验位
        /// </summary>
        /// <param name="buf">数据包</param>
        /// <returns></returns>
        public static byte getCheckBit(byte[] buf) {
            byte check = 0;
            foreach(byte item in buf){
                check += item;
            }
            return check;
        }


        /// <summary>
        /// 指定部位计算校验位
        /// </summary>
        /// <param name="buf">数据包</param>
        /// <param name="index">初始下标</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static byte getCheckBit(byte[] buf,int index,int length){
            byte check = 0;
            if(index >= length) return check;
            for (int i = index; i < length; i++){
                check += buf[i];
            }
            return check;
        }
      

        public static int strToInt(string intString) {
            try{
                return int.Parse(intString);
            }
            catch {
                return -10086;
            }
        }


        /// <summary>
        /// 解析文件路径，获得hex文件名
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string hexFail(string item)
        {
            string[] spl = item.Split('\\');
            string hex = "";
            if (spl[spl.Length - 1].EndsWith(".hex"))
            {
                hex = spl[spl.Length - 1];
                //.Replace(".hex","");
            }
            return hex;
        }

        
        public static string[] getSubStr(string msg) {
            string[] str = msg.Split(':');
            return str;
        }


        /// <summary>
        /// 获取IP4地址的字符串
        /// </summary>
        /// <returns></returns>
        public static string GetIpAddress(){
            //string hostName = Dns.GetHostName();   //获取本机名
            //IPHostEntry localhost = Dns.GetHostByName(hostName);    //方法已过期，可以获取IPv4的地址
            //IPHostEntry localhost = Dns.GetHostEntry(hostName);   //获取IPv6地址
            //IPAddress localaddr = localhost.AddressList[0];

            return GetAddressIP();
        }


        /// <summary>
        /// 获取IP4地址的字符串
        /// </summary>
        /// <returns></returns>
        public static string GetAddressIP() {
            string AddressIP = "";
            IPAddress _IPAddress = GetAddress();
            if (_IPAddress != null) AddressIP = _IPAddress.ToString();
            return AddressIP;
        }


        public static IPAddress GetAddress() {
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList){
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork"){
                    return _IPAddress;//设定全局的IP
                }
            }
            return null;
        }


        /// <summary>
        /// 获取当前工作目录的完全限定路径。
        /// </summary>
        /// <returns></returns>
        public static String getAddress() {
            string msg = "";
            try { 
                msg = System.Environment.CurrentDirectory;
            }
            catch { }
            return msg;
        }


        /// <summary>
        /// 字符串转IP
        /// </summary>
        /// <param name="ip">一定是255.255.255.255的类型</param>
        /// <returns></returns>
        public static byte[] getIP4byte(string ip){
            byte[] ip4Byte = new byte[4] { 0, 0, 0, 0 };
            string[] ip4Str = getIP4(ip);
            if (null == ip4Str) return null;
            for (int i = 0; i < ip4Str.Length; i++)
            {
                int btCount = stringToInt1(ip4Str[i]);
                try{
                    ip4Byte[i] = byte.Parse(deleteFir0(ip4Str[i]));
                }catch{
                    return null;
                }               
            }
            return ip4Byte;
        }

        public  static string[] getIP4(string ip)
        { //将 192.168.1.1格式分割成4份
            string[] splStr = null;
            try {
                splStr = ip.Split('.');
            }
            catch{}
            if (splStr == null || splStr.Length != 4) splStr = null;
            return splStr;
        }

        //将串口号变成两个byte
        public static byte[] getComTime(string port) {            
             return getComTime(port, 1, 65535);
        }

        //将串口号变成两个byte
        public static byte[] getComZeroTime(string port) 
        {
            return getComTime(port, 0, 65535);
        }

        //将串口号变成两个byte
        public static byte[] getComTime(string port,int small,int max)
        {
            int comInt = XWUtils.stringToInt1(port);
            if (comInt == -1) return null;
            if (comInt < small || comInt > max) return null;
            byte[] comByte = new byte[2];
            comByte[0] = (byte)(comInt / 256);
            comByte[1] = (byte)(comInt % 256);
            return comByte;
        }

        /// <summary>
        /// 将两位ID号的字符串转为byte[],
        /// </summary>
        /// <param name="byteID">例如0102,大于4位则取前4位</param>
        /// <returns></returns>
        public static byte[] getByteID(string byteID)
        { //将 liang
            if (byteID.Length < 4) return null;
            byte[] byteBt = new byte[2];
            try {
                byteBt[0] = hexStrToByte(byteID.Substring(0, 2));
                byteBt[1] = hexStrToByte(byteID.Substring(2, 2));
            }
            catch {
                return null;
            }           
            return byteBt;
        }


        /// <summary>
        /// 获取切割 （ 首项字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string getSplFirst(string data)
        {
            return getSplpart(data, '(', 0);
        }


        /// <summary>
        /// 获取切割 最后一项字符串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="flag">切割字符</param>
        /// <returns></returns>
        public static string getSplTypeEnd(string data, char flag)
        {
            return getSplpart(data, flag, 1);
        }


        /// <summary>
        /// 获取切割 首项字符串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string getSplFirst(string data, char flag)
        {
            return getSplpart(data,flag,0);
        }


        /// <summary>
        /// 获取切割指定位置字符串
        /// </summary>
        /// <param name="data">源数据</param>
        /// <param name="flag">切割的标志位</param>
        /// <param name="index">获取切割的部位</param>
        /// <returns></returns>
        public static string getSplpart(string data,char flag,int index)
        { //将 liang
            string splone = null;
            try
            {
                string[] splStr = data.Split(flag);
                splone = splStr[index];
            }
            catch { }
            return splone;
        }


        /// <summary>
        /// 比较两个数组的内容是否相等
        /// </summary>
        /// <param name="socurBT"></param>
        /// <param name="srcBt"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool byteBTBettow(byte[] socurBT,byte[] srcBt,int length)
        { //将 liang
            if (socurBT == null || srcBt == null) return false;
            if (srcBt.Length != length || srcBt.Length != length) return false;
            for (int i = 0; i < length;i++ )
            {
                if (srcBt[i] != socurBT[i]) return false;
            }
            return true;
        }


    }
}
