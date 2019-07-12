using CiXinLocation.bean;
using CiXinLocation.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiXinLocation.CiXinInterface
{
    interface ComDataInterface
    {
        /// <summary>
        /// 端口号。返回的是失败的信息
        /// </summary>
        string open();

        /// <summary>
        /// 串口开闭状态，开=true,关=false
        /// </summary>
        /// <returns></returns>
        bool comType();
        /// <summary>
        /// 基本参数的设定打开。返回的是失败的信息
        /// </summary>
        /// <param name="port">端口号</param>
        /// <param name="serialPort">串口号，或者IP</param>
        string open(string port, string serialPort);

        /// <summary>
        /// 基本参数的设定打开。返回的是失败的信息
        /// </summary>
        /// <param name="port">波特率</param>
        /// <param name="serialPort">串口号</param>
        string open(int port, string serialPort);

        void close();

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        void sendData(byte[] buf,int index,int length,string ip);

        void sendData(SendDataType sType);

        /// <summary>
        /// 类关闭调用
        /// </summary>
        void classClose();
    }


    /// <summary>
    /// 没办法，接口不能定义委托，只能再搞这个接口，用来回调数据
    /// </summary>
    public interface ComReadDataInterface
    {
        /// <summary>
        /// 读取串口中数据
        /// </summary>
        /// <param name="buf"></param>
        void revePortsData(byte[] buf, string ip);

        void revePortsData(SendDataType sType);

    }
}
