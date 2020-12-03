using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.SharedComponents
{
    public class ApiCommandObject : TcpCommandObject
    {
        public ApiCommandObject() { }

        public ApiCommandObject(byte command)
        {
            CreateRawCommandDataBuffer(SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength + 1);
            RawCommandDataBuffer[SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength] = command;
        }

        public ApiCommandObject(byte command, byte sensorId)
        {
            CreateRawCommandDataBuffer(SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength + 2);
            RawCommandDataBuffer[SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength] = command;
            RawCommandDataBuffer[SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength + 1] = sensorId;
        }
        public ApiCommandObject(byte command, byte sensorId, byte[] data)
        {
            CreateRawCommandDataBuffer((short)(SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength + 2 + data.Length));
            RawCommandDataBuffer[SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength] = command;
            RawCommandDataBuffer[SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength + 1] = sensorId;
            data.CopyTo(RawCommandDataBuffer, SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength + 2);
        }

        public byte Command { get { return RawCommandDataBuffer[SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength]; } }

        public byte SensorId { get { return RawCommandDataBuffer[SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength + 1]; } }

        public byte[] Data
        {
            get
            {
                //2 = 1 (Command) + 1(SendorId)
                int numberOfBytesExcludingData = (SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength + 2);
                byte[] data = new byte[PacketSize - numberOfBytesExcludingData];
                Array.Copy(RawCommandDataBuffer, numberOfBytesExcludingData, data, 0, data.Length);
                return data;
            }
        }
    }
}
