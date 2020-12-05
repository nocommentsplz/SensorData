using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace SensorData.SharedComponents
{
    public interface ICommandObjectProcessor<TCommandObject>
        where TCommandObject : class, ICommandObject, new()
    {
        void ProcessCommand(TCommandObject commandObject, Socket socket);
    }
}
