using SensorData.Service.CommandObjects.Api;
using SensorData.SharedComponents;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace SensorData.Service
{
    class ApiCommandObjectProcessor : TcpCommandObjectProcessor<ApiCommandObject>
    {
        private static Dictionary<byte, Func<IIndividualCommandObjectProcessor<ApiCommandObject>>> _commandRegister
            = new Dictionary<byte, Func<IIndividualCommandObjectProcessor<ApiCommandObject>>>()
            {
                { SensorDataOverTcpProtocol.ApiCommands.StartSensorDataService, ()=> new StartSensorDataServiceCommandObjectProcessor()  },
                { SensorDataOverTcpProtocol.ApiCommands.StopSensorDataService, ()=> new StopSensorDataServiceCommandObjectProcessor() },
                { SensorDataOverTcpProtocol.ApiCommands.FetchSensorValue, ()=> new FetchSensorValueCommandObjectProcessor()  }
            };

        protected override IIndividualCommandObjectProcessor<ApiCommandObject> GetIndividualCommandObjectProcessor(ApiCommandObject commandObject)
        {
            if (!_commandRegister.ContainsKey(commandObject.Command))
            {
                throw new Exception($"IIndividualCommandObjectProcessor not registered for the command {commandObject.Command}");
            }

            return _commandRegister[commandObject.Command]();
        }
    }
}
