using SensorData.SharedComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.Service
{
    public class SensorCommandObject : TcpCommandObject
    {
        public SensorCommandObject() { }

        public SensorCommandObject(byte sensorId, byte value)
        {
            CreateRawCommandDataBuffer(SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength + 2);
            RawCommandDataBuffer[SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength] = sensorId;
            RawCommandDataBuffer[SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength + 1] = value;
        }

        public byte SensorId { get { return RawCommandDataBuffer[SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength + 1]; } }

        public byte Value { get { return RawCommandDataBuffer[SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength]; } }
    }
}
