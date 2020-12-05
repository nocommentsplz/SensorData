using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace SensorData.SharedComponents
{
    public class SocketCollection
    {
        private readonly List<Socket> _socketList;
        private static object _lock = new object();

        public SocketCollection()
        {
            _socketList = new List<Socket>();
        }

        public void Add(Socket socket)
        {
            lock (_lock)
            {
                _socketList.Add(socket);
            }
        }

        public void Remove(Socket socket)
        {
            lock (_lock)
            {
                _socketList.Remove(socket);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _socketList.Clear();
            }
        }

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _socketList.Count;
                }
            }
        }

        public IEnumerable<Socket> Items
        {
            get
            {
                lock (_lock)
                {
                    List<Socket> copy = new List<Socket>();
                    copy.AddRange(_socketList);
                    return copy;
                }
            }
        }
    }
}
