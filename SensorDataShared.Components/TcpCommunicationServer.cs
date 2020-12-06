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
    public class TcpCommunicationServer<TCommandObject> : ITcpCommunicationServer
        where TCommandObject : class, ITcpCommandObject, new()
    {
        private object _initializeLock = new object();
        private Socket _listenerSocket;
        private SocketCollection _clientSockets = new SocketCollection();
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;
        public TcpCommunicationServer()
        {

        }

        public bool Initialized { get; private set; }

        public TcpServerSettings Settings { get; set; }

        public ICommandObjectProcessor<TCommandObject> CommandObjectProcessor { private get; set; }

        protected virtual bool LogOnConnectDisconnect { get; }

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

            if (LogOnConnectDisconnect)
            {
                Console.WriteLine("Waiting for a connection...");
            }

            _listenerSocket.BeginAccept(new AsyncCallback(ApiAcceptCallback), _listenerSocket);

            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            ReadFromConnectedClients();
        }

        private void ApiAcceptCallback(IAsyncResult ar)
        {
            if (LogOnConnectDisconnect)
            {
                Console.WriteLine("Accepting a connection...");
            }

            try
            {
                Socket clientSocket = _listenerSocket.EndAccept(ar);
                _clientSockets.Add(clientSocket);

                if (LogOnConnectDisconnect)
                {
                    Console.WriteLine($"Total Number of Active Connections : {_clientSockets.Count}");
                    Console.WriteLine("Waiting for new connection...");
                }

                _listenerSocket.BeginAccept(new AsyncCallback(ApiAcceptCallback), _listenerSocket);
            }
            catch (ObjectDisposedException)
            {

            }
            catch (NullReferenceException)
            {

            }
        }

        private void ExecuteShutdownRoutine()
        {
            cancellationTokenSource.Cancel();

            while (_clientSockets.Count > 0)
            {
                foreach (Socket socket in _clientSockets.Items)
                {
                    socket.Close();
                    socket.Dispose();
                }

                _clientSockets.Clear();
            }

            _listenerSocket.Close();
            _listenerSocket.Dispose();
            _listenerSocket = null;
        }

        private void ReadFromConnectedClients()
        {
            Task.Run(async () =>
            {

                while (!cancellationToken.IsCancellationRequested)
                {
                    List<Socket> readList = new List<Socket>();
                    readList.AddRange(_clientSockets.Items);

                    if (readList.Any())
                    {
                        Socket.Select(readList, null, null, 1000);

                        foreach (Socket socket in readList)
                        {
                            try
                            {
                                TCommandObject apiCommandObject = socket.ReceiveCommandObject<TCommandObject>();
                                ProcessApiCommand(apiCommandObject, socket);
                            }
                            catch (SocketException ex)
                            {
                                _clientSockets.Remove(socket);
                                if (LogOnConnectDisconnect)
                                {
                                    Console.WriteLine(ex.Message);
                                    Console.WriteLine($"Total Number of Active Connections : {_clientSockets.Count}");
                                }
                            }
                        }
                    }
                    else
                    {
                        await Task.Delay(1000, cancellationToken);
                    }
                }
            });
        }

        private void ProcessApiCommand(TCommandObject apiCommandObject, Socket socket)
        {
            Task.Run(() =>
            {
                if (null != CommandObjectProcessor)
                {
                    CommandObjectProcessor.ProcessCommand(apiCommandObject, socket);
                }
            });
        }
    }
}
