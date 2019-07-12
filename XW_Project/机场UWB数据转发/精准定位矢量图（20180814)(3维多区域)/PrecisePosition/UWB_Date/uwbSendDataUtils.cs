using PositionReceiveWin;
using PositionReceiveWin.Bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PrecisePosition.UWB_Date
{
    public class UwbSendDataUtils
    {
        private IPEndPoint uwb1point = new IPEndPoint(IPAddress.Parse("192.168.1.50"), 5000);
        private IPEndPoint uwb3point = new IPEndPoint(IPAddress.Parse("192.168.1.50"), 5000);
        private UdpClient client;

        public IPEndPoint UWB1point
        {
            get { return uwb1point; }
            set { uwb1point = value; }
        }
        public IPEndPoint UWB3point
        {
            get { return uwb3point; }
            set { uwb3point = value; }
        }
        public UdpClient Client
        {
            get { return client; }
            set { client = value; }
        }

        public UwbSendDataUtils() {}

        public UwbSendDataUtils(IPEndPoint point, IPEndPoint point3, UdpClient client) 
        {
            UWB1point = point;
            UWB3point = point;
            Client = client;
        }

        public void sendUWB1Data(UWBbean UWBbean) 
        {
            String uwbd = JSON.stringify(UWBbean);
            byte[] data = Encoding.UTF8.GetBytes(uwbd);
            sendNoPoint(data,UWB1point);
        }

        public void sendUWB3Data(UWBbean UWBbean)
        {
            String uwbd = JSON.stringify(UWBbean);
            byte[] data = Encoding.UTF8.GetBytes(uwbd);
            sendNoPoint(data, UWB3point);
        }

        private void sendNoPoint(byte[] buf, IPEndPoint uwbPoint)
        {
            if (client != null) 
            {
                try
                {
                    client.Send(buf, buf.Length, uwbPoint);
                    //client.SendTo();//client.SendTo(buf, point);       
                }
                catch (ObjectDisposedException oEx) 
                {
                    Console.Write(oEx.Message);
                }                
            }
                  
        }


    }
}
