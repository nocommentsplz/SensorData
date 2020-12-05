using SensorData.SharedComponents.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.SharedComponents
{
    public interface ITcpCommunicationServer : ICommunicationServer
    {
        public TcpServerSettings Settings { get; set; }
    }
}
