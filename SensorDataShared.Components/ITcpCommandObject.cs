using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.SharedComponents
{
    public interface ITcpCommandObject : ICommandObject
    {
        Int16 PacketSize { get; }
    }
}
