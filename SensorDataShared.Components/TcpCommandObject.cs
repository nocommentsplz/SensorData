using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.SharedComponents
{
    public class TcpCommandObject : ITcpCommandObject
    {
        public Int16 PacketSize
        {
            get
            {
                if (null == RawCommandDataBuffer) 
                    throw new Exception("RawCommandDataBuffer is not set when PacketSize is retrieved");

                return BitConverter.ToInt16(RawCommandDataBuffer, 0);                
            }
        }

        public byte[] RawCommandDataBuffer { get; set; }

        protected void CreateRawCommandDataBuffer(short packetSize)
        {
            RawCommandDataBuffer = new byte[packetSize];
            BitConverter.GetBytes(packetSize).CopyTo(RawCommandDataBuffer, 0);
        }
    }
}
