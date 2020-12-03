using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.SharedComponents
{
    public interface ICommunicationServer<TCommandObject>
        where TCommandObject : class, ICommandObject, new()
    {
        public bool Initialized { get; }
        public bool Initialize();
        public void Shutdown();
    }
}
