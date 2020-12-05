using SensorData.SharedComponents;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Linq;

namespace SensorData.Service.CommandObjects.Api
{
    class FetchSensorValueCommandObjectProcessor : IIndividualCommandObjectProcessor<ApiCommandObject>
    {
        public void ProcessCommand(ApiCommandObject commandObject, Socket socket)
        {
            if (commandObject.Command != SensorDataOverTcpProtocol.ApiCommands.FetchSensorValue)
            {
                throw new Exception("Wrong Command Object Processor");
            }

            IEnumerable<byte> sensorValues = SensorValueCollection.Instance.GetSensorValues(commandObject.SensorId);

            ApiCommandObject apiCommandObject = new ApiCommandObject(
                SensorDataOverTcpProtocol.ApiCommands.FetchSensorValueResponse, commandObject.SensorId, new byte[] { sensorValues.LastOrDefault() });

            socket.Send(apiCommandObject.RawCommandDataBuffer);
        }
    }
}
