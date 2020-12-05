using SensorData.SharedComponents;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace SensorData.Service.CommandObjects.Api
{
    class SensorValueCommandObjectProcessor : IIndividualCommandObjectProcessor<SensorCommandObject>
    {
        public void ProcessCommand(SensorCommandObject commandObject, Socket socket)
        {
            if (commandObject.Command != SensorDataOverTcpProtocol.SensorCommands.SensorValue)
            {
                throw new Exception("Wrong Command Object Processor");
            }

            Console.WriteLine($"Sensor Value, Sensor Id : {commandObject.SensorId}, Value : {commandObject.Data[0]}");
            SensorValueCollection.Instance.AddSensorValue(commandObject.SensorId, commandObject.Data[0]);
        }
    }
}
