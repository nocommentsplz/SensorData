using Microsoft.Extensions.Configuration;
using SensorData.Service.Settings;
using SensorData.SharedComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.Service
{
    sealed class SensorCommunicationServer : TcpCommunicationServer<SensorCommandObject>
    {
        private SensorCommunicationServer(IConfiguration configuration)
        {
            CommandObjectProcessor = new SensorCommandObjectProcessor();
            Settings = configuration.GetSection("SensorTcpServerSettings").Get<SensorTcpServerSettings>();
        }

        private static bool _InstanceCreated = false;
        private static object _InstanceCreationLock = new object();
        private static SensorCommunicationServer _Instance;

        public static SensorCommunicationServer CreateInstance(IConfiguration configuration)
        {
            if (!_InstanceCreated)
            {
                lock (_InstanceCreationLock)
                {
                    if (!_InstanceCreated)
                    {
                        _Instance = new SensorCommunicationServer(configuration);
                    }
                }
            }

            return _Instance;
        }

        public static SensorCommunicationServer Instance
        {
            get
            {
                if (null == _Instance) throw new Exception("SensorCommunicationServer instance not yet created. Call GetInstance to create instance");
                return _Instance;
            }
        }
    }
}
