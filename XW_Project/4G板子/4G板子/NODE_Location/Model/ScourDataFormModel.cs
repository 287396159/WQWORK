using CiXinLocation.bean;
using CiXinLocation.CiXinInterface;
using SerialportSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiXinLocation.Model
{
    class ScourDataFormModel : FromBaseModel
    {
        private ScourDataInterface scourDataInter;
        private string tag = "";

        public ScourDataFormModel(ScourDataInterface scourDataInter)
        {
            receVeByteHandle = 0xfe;
            receVeByteend = 0xfd;
            this.scourDataInter = scourDataInter;
            tag = XwDataUtils.GetTimeStamp(false).ToString() + new Random().Next(1024).ToString() + "ScourDataFormModel";
        }

        public override void reveData(SendDataType sType)
        { 
        
        }

        public override void reveData(byte[] buf, string ip)
        {
            //if (buf.Length != 15) return;
            StringBuilder sBuilder = new StringBuilder();
            foreach(byte item in buf){
                sBuilder.Append(item.ToString("X2"));
                sBuilder.Append(" ");                
            }
            if (scourDataInter != null) scourDataInter.backData(buf, sBuilder);
            sBuilder.Clear();            
        }

        public override void close()
        {
            if (scourDataInter != null) scourDataInter = null;
        }

        public override string TAG()
        {
            return tag;
        }

       

    }
}
