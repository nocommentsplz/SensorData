using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.SharedComponents.Settings
{
    public class TcpServerSettings
    {
        public int Port { get; set; }

        public int Backlog { get; set; }
    }
}
