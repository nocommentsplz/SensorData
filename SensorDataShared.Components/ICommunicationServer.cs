using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.SharedComponents
{
    public interface ICommunicationServer
    {
        bool Initialized { get; }

        bool Initialize();

        void Shutdown();
    }
}
