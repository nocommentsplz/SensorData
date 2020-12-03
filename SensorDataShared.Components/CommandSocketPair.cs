using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace SensorData.SharedComponents
{
    class CommandSocketPair<TCommandObject>
        where TCommandObject : class, ICommandObject, new()
    {
        CommandSocketPair(TCommandObject commandObject, Socket socket)
        {
            CommandObject = commandObject;
            Socket = socket;
            ID = new Guid().ToString();
        }

        public TCommandObject CommandObject { get; private set; }

        public Socket Socket { get; private set; }

        public string ID { get; private set; }
    }
}
