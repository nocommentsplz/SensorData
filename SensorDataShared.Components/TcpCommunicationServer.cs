using SensorData.SharedComponents.Settings;
using SensorData.SharedComponents.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SensorData.SharedComponents
{
    public class TcpCommunicationServer<TCommandObject> : ITcpCommunicationServer<TCommandObject>
        where TCommandObject : class, ITcpCommandObject, new()
    {
        private object _initializeLock = new object();
        private Socket _listenerSocket;
        private ConcurrentBag<Socket> _clientSockets = new ConcurrentBag<Socket>();
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;
        public TcpCommunicationServer()
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
        }

        public bool Initialized { get; private set; }
        public TcpServerSettings Settings { get; set; }

        public bool Initialize()
        {
            if (!Initialized)
            {
                lock (_initializeLock)
                {
                    if (!Initialized)
                    {
                        try
                        {
                            if (null != Settings)
                            {
                                ExecuteInitializationRoutine();
                                Initialized = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            ExecuteCleanupRoutine();
                            Initialized = false;
                            throw;
                        }
                    }
                }
            }

            return Initialized;
        }

        public void Shutdown()
        {
            if (Initialized)
            {
                lock (_initializeLock)
                {
                    if (Initialized)
                    {
                        try
                        {
                            ExecuteShutdownRoutine();
                            Initialized = false;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            ExecuteCleanupRoutine();
                            Initialized = false;
                            throw;
                        }
                    }
                }
            }
        }

        private void ExecuteCleanupRoutine()
        {

        }

        private void ExecuteInitializationRoutine()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Settings.Port);

            _listenerSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _listenerSocket.Bind(localEndPoint);
            _listenerSocket.Listen(Settings.Backlog);

            Console.WriteLine("Waiting for a connection...");
            _listenerSocket.BeginAccept(new AsyncCallback(ApiAcceptCallback), _listenerSocket);

            ReadFromConnectedClients();
        }

        private void ApiAcceptCallback(IAsyncResult ar)
        {
            Console.WriteLine("Accepting a connection...");

            Socket clientSocket = _listenerSocket.EndAccept(ar);
            _clientSockets.Add(clientSocket);

            Console.WriteLine("Waiting for new connection...");
            _listenerSocket.BeginAccept(new AsyncCallback(ApiAcceptCallback), _listenerSocket);
        }

        private void ExecuteShutdownRoutine()
        {

        }

        private void ReadFromConnectedClients()
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    List<Socket> readList = new List<Socket>();
                    readList.AddRange(_clientSockets);

                    if (readList.Any())
                    {
                        Socket.Select(readList, null, null, 1000000);

                        foreach (Socket socket in readList)
                        {
                            TCommandObject apiCommandObject = socket.ReceiveCommandObject<TCommandObject>();
                            ProcessApiCommand(apiCommandObject);
                        }
                    }
                    else
                    {
                        await Task.Delay(1000, cancellationToken);
                    }
                }
            });
        }

        private void ProcessApiCommand(TCommandObject apiCommandObject)
        {
            Task.Run(() =>
            {
                Console.WriteLine($"New Api Command Received, Packet Size:{apiCommandObject.PacketSize}");
            });
        }

        //private void ReadApiCommandPacketLength(Socket clientSocket, TCommandObject apiCommandObject)
        //{
        //    apiCommandObject.RawCommandDataBuffer = new byte[SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength];
        //    clientSocket.BeginReceive(apiCommandObject.RawCommandDataBuffer,
        //        0, SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength, 0, new AsyncCallback(ReadApiCommandPacketLengthCallback), apiCommandObject);
        //}

        //private void ReadApiCommandPacketLengthCallback(IAsyncResult ar)
        //{
        //    // Read rest of the api command data from the client socket.
        //    int bytesRead = _clientSocket.EndReceive(ar);
        //    TCommandObject apiCommandObject = (TCommandObject)ar.AsyncState;
        //    if (bytesRead > 0)
        //    {
        //        byte[] commandBuffer = new byte[SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength + apiCommandObject.PacketSize];
        //        apiCommandObject.RawCommandDataBuffer.CopyTo(commandBuffer, 0);
        //        apiCommandObject.RawCommandDataBuffer = commandBuffer;

        //        _clientSocket.BeginReceive(apiCommandObject.RawCommandDataBuffer, SensorDataOverTcpProtocol.NumberOfBytesToRepresentPacketLength,
        //            apiCommandObject.PacketSize, 0, new AsyncCallback(ReadApiCommandCallback), apiCommandObject);
        //    }
        //}

        //private void ReadApiCommandCallback(IAsyncResult ar)
        //{
        //    int bytesRead = _clientSocket.EndReceive(ar);
        //    TCommandObject apiCommandObject = (TCommandObject)ar.AsyncState;
        //    if (bytesRead > 0)
        //    {
        //        ProcessApiCommand(apiCommandObject);
        //        ReadApiCommand();
        //    }
        //}
    }
}
