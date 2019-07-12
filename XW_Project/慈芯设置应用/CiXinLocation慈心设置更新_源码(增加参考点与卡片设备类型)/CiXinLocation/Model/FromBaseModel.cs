using SerialportSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CiXinLocation.Model
{
    public class FromBaseModel {

        public delegate void SendDataHandle(byte[] buf);
        public SendDataHandle sendDataHandle;

        public byte sendByteHandle = 0xf2;  //发送包头
        public byte sendByteend = 0xf1;     //发送包尾
        public byte receVeByteHandle = 0xfc;//接收包头
        public byte receVeByteend = 0xfb;   //接收包尾


        public FromBaseModel() {             
        }

        private void sendDataModel(object objbuf){
            if (!(objbuf is byte[])) return;
            if (sendDataHandle == null) return;
            sendDataHandle((byte[])objbuf);                        
        }


        int i_send = 0;
        /// <summary>
        /// 计算校验，并且发送数据
        /// </summary>
        /// <param name="buf"></param>
        public void addCSAndsendData(byte[] buf)
        { 
            if(buf == null || buf.Length < 2) return;
            buf[buf.Length - 2] = XWUtils.getCheckBit(buf,0,buf.Length -2);
            new Thread(sendDataModel).Start(buf);
            //int time = Environment.TickCount;
            //Console.WriteLine("sendDataModel{0},{1}", time, i_send);
            //i_send++;

        }


        private int index_buf = 0;
        private byte[] bytesBuf = new byte[288];
        /// <summary>
        /// 接收数据开始处
        /// </summary>
        /// <param name="buf"></param>
        public void revePortsData(byte[] buf) {
            if (buf == null || buf.Length < 1) return;
            //if (index_buf == 0 && buf[0] != receVeByteHandle) return;
            if (buf[0] == receVeByteHandle && buf[buf.Length - 1] == receVeByteend)
            {
                if (buf[buf.Length - 2] == XWUtils.getCheckBit(buf, 0, buf.Length - 2))
                {
                    index_buf = 0;
                    reveData(buf);
                    return;
                }
            } 
            if(index_buf == 0) Array.Clear(bytesBuf, 0, bytesBuf.Length);//先清理，后面才好添加数据
            if (bytesBuf.Length <= buf.Length + 38) {
                index_buf = 0;
                Array.Clear(bytesBuf, 0, bytesBuf.Length);
                if (bytesBuf.Length < buf.Length) return;
            }
            else if (index_buf + buf.Length >= bytesBuf.Length) {
                Array.Copy(bytesBuf, index_buf - 38, bytesBuf, 0, 38);
                index_buf = 38;
                Array.Clear(bytesBuf, 38, bytesBuf.Length - 38);
            }            
            Array.Copy(buf, 0, bytesBuf, index_buf, buf.Length);
            findPack(bytesBuf);
                    
            index_buf += buf.Length;
            if (index_buf >= bytesBuf.Length) index_buf = 0;
        }

        /// <summary>
        /// 将缓存的数据中符合要求的数据包解析出来
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="index"></param>
        private void findPack(byte[] buf){            
            for (int i = 0; i < buf.Length;i++ ) {
                if (buf[i] != 0xfc) continue; //只要包头
                for (int j = i; j < buf.Length; j++) {
                    if (buf[j] != 0xfb) continue;//只要包尾
                    byte[] packData = new byte[j - i + 1];
                    Array.Copy(buf, i, packData, 0, packData.Length);//截取一段包头与包尾                  
                    if (checkPacket(packData)) Array.Clear(buf,i,j-i); //校验，并且清理掉
                }
            }         
        }


        private bool checkPacket(byte[] checkBuf) {
            byte check = XWUtils.getCheckBit(checkBuf, 0, checkBuf.Length-2);
            if (checkBuf[checkBuf.Length - 2] == check) {
                reveData(checkBuf);
                return true;
            }
            return false;
        }


        public virtual void reveData(byte[] buf) {  }
        public virtual void close() { }

        /// <summary>
        /// 返回MODEL的标志位
        /// </summary>
        /// <returns></returns>
        public virtual string TAG() { return "FromBaseModel"; }

    }
}
  