using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace SensorData.SharedComponents.Utilities
{
    public static class SocketExtensions
    {
        public static byte[] ReceiveBytes(this Socket socket, short packetSize)
        {
            int numberOfBytesReceived = 0;
            byte[] buffer = new byte[packetSize];
            while (numberOfBytesReceived < packetSize)
            {
                numberOfBytesReceived += socket.Receive(buffer,
                    numberOfBytesReceived, packetSize - numberOfBytesReceived, 0);
            }

            return buffer;
        }

        public static TCommandObject ReceiveCommandObject<TCommandObject>(this Socket socket)
            where TCommandObject : class, ITcpCommandObject, new()
        {
            byte[] receivePacketSizeBuffer = socket.ReceiveBytes(SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength);
            Int16 packetSize = BitConverter.ToInt16(receivePacketSizeBuffer);
            byte[] receivePacketDataBuffer = socket.ReceiveBytes((Int16)(packetSize - SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength));

            byte[] completeMessageData = new byte[packetSize];
            receivePacketSizeBuffer.CopyTo(completeMessageData, 0);
            receivePacketDataBuffer.CopyTo(completeMessageData, SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength);

            TCommandObject receiveCommandObject = new TCommandObject();
            receiveCommandObject.RawCommandDataBuffer = completeMessageData;

            return receiveCommandObject;
        }
    }
}
