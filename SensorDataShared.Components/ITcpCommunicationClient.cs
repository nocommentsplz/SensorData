using SensorData.SharedComponents.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.SharedComponents
{
    public interface ITcpCommunicationClient<TCommandObject> : ICommunicationClient<TCommandObject>
        where TCommandObject : class, ITcpCommandObject, new()
    {
        bool Connected { get; }
        bool Connect(TcpClientSettings settings);
        void Disconnect();
    }
}
