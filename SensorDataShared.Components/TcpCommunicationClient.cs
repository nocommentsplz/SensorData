using SensorData.SharedComponents.Settings;
using SensorData.SharedComponents.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SensorData.SharedComponents
{
    public class TcpCommunicationClient<TCommandObject> : ITcpCommunicationClient<TCommandObject>
        where TCommandObject : class, ITcpCommandObject, new()
    {
        private Socket _clientSocket;
        private static object _connectLock = new object();

        public bool Connected { get; private set; }

        public bool Connect(TcpClientSettings settings)
        {
            if (!Connected)
            {
                lock (_connectLock)
                {
                    if (!Connected)
                    {
                        try
                        {
                            if (null != settings)
                            {
                                ExecuteConnectionRoutine(settings);
                                Connected = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            ExecuteCleanupRoutine();
                            Connected = false;
                            throw;
                        }
                    }
                }
            }

            return Connected;
        }

        private void ExecuteCleanupRoutine()
        {

        }

        private void ExecuteConnectionRoutine(TcpClientSettings settings)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, settings.ServerPort);

            _clientSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _clientSocket.Connect(localEndPoint);
        }

        public void Disconnect()
        {
            if (Connected)
            {
                lock (_connectLock)
                {
                    if (Connected)
                    {
                        try
                        {
                            ExecuteDisconnectionRoutine();
                            Connected = false;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            ExecuteCleanupRoutine();
                            Connected = false;
                            throw;
                        }
                    }
                }
            }
        }

        private void ExecuteDisconnectionRoutine()
        {
            _clientSocket.Disconnect(false);
            _clientSocket = null;
        }

        public int Send(TCommandObject commandObject)
        {
            if (!Connected)
            {
                throw new Exception("CommunicationClient is not yet connected. Call Connect prior to Send");
            }

            return _clientSocket.Send(commandObject.RawCommandDataBuffer);
        }

        public TCommandObject SendReceive(TCommandObject commandObject)
        {
            int numberOfBytesSent = Send(commandObject);
            if (0 >= numberOfBytesSent)
            {
                throw new Exception("Message sending failed");
            }

            return _clientSocket.ReceiveCommandObject<TCommandObject>();
        }
    }
}
