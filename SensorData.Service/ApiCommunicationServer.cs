using Microsoft.Extensions.Configuration;
using SensorData.Service.Settings;
using SensorData.SharedComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.Service
{
    class ApiCommunicationServer : TcpCommunicationServer<ApiCommandObject>
    {
        internal ApiCommunicationServer(IConfiguration configuration)
        {
            CommandObjectProcessor = new ApiCommandObjectProcessor();
            Settings = configuration.GetSection("ApiTcpServerSettings").Get<ApiTcpServerSettings>();
        }

        protected override bool LogOnConnectDisconnect { get => false; }
    }
}
