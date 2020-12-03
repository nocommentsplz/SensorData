using SensorData.SharedComponents.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.SharedComponents
{
    public interface ITcpCommunicationServer<TCommandObject> : ICommunicationServer<TCommandObject>
        where TCommandObject : class, ITcpCommandObject, new()
    {
        public TcpServerSettings Settings { get; set; }
    }
}
