using SensorData.Service.CommandObjects.Api;
using SensorData.SharedComponents;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace SensorData.Service
{
    class SensorCommandObjectProcessor : TcpCommandObjectProcessor<SensorCommandObject>
    {
        private static Dictionary<byte, Func<IIndividualCommandObjectProcessor<SensorCommandObject>>> _commandRegister
            = new Dictionary<byte, Func<IIndividualCommandObjectProcessor<SensorCommandObject>>>()
            {
                { SensorDataOverTcpProtocol.SensorCommands.SensorValue, ()=> new SensorValueCommandObjectProcessor()  },
            };


        protected override IIndividualCommandObjectProcessor<SensorCommandObject> GetIndividualCommandObjectProcessor(SensorCommandObject commandObject)
        {
            if (!_commandRegister.ContainsKey(commandObject.Command))
            {
                throw new Exception($"IIndividualCommandObjectProcessor not registered for the command {commandObject.Command}");
            }

            return _commandRegister[commandObject.Command]();
        }
    }
}

