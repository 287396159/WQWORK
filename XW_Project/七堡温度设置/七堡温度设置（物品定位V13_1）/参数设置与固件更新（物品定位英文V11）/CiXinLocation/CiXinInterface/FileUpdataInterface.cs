using CiXinLocation.bean;
using CiXinLocation.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CiXinLocation.CiXinInterface
{
    public interface FileUpdataInterface
    {
        /// <summary>
        /// 文件属性自带的设备类型
        /// </summary>
        /// <returns></returns>
        DrivaceType getType();

        /// <summary>
        /// 子设备类型
        /// </summary>
        /// <returns></returns>
        byte sunDeviceType();

        void fileInformation(HexFileBean hfInfor);//Hex文件的相关信息

        void sendBinData(DrivaceType dType, byte[] ID, byte[] Addr); //发送解析hex文件的Bin数据,int hashCode
        void backBinData(byte[] ID, byte[] Addr); //下位机回送的数据

        void checkBinData(byte[] ID);//发送检查Bin数据校验码
        void backCheckBinData(byte[] ID, byte status);//下位机回复校验码
        void updataResult(byte[] ID, byte status);//0失败，1成功。
        
        void upDataTag(byte Enable);//Enable = 0 关闭。Enable = 1打开
        void upDataTag(byte[] ID, byte[] Addr, byte[] len);
        void clearTag();

        void askUSB_dangleUpData();//byte[] len, byte[] CheckSum 
       
    }

    public interface FileUpdataModelInterface : FileUpdataInterface
    {
        void stop();
        void start();
        void start(byte Index);
    }


    public interface LabelChangeInterface { 
        /// <summary>
        /// 改变lab的值
        /// </summary>
        /// <param name="lab"></param>
        /// <param name="text"></param>
        void setUpdataResultLabelMain(Label lab,string text);
        void enableFalse(Label lab);
    }
}
