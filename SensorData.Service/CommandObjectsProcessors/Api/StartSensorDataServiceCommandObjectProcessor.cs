﻿using SensorData.SharedComponents;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace SensorData.Service.CommandObjects.Api
{
    class StartSensorDataServiceCommandObjectProcessor : IIndividualCommandObjectProcessor<ApiCommandObject>
    {
        public void ProcessCommand(ApiCommandObject commandObject, Socket socket)
        {
            if (commandObject.Command != SensorDataOverTcpProtocol.ApiCommands.StartSensorDataService)
            {
                throw new Exception("Wrong Command Object Processor");
            }

            SensorCommunicationServer.Instance.Initialize();
        }
    }
}
