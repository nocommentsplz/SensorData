using Microsoft.Extensions.Configuration;
using SensorData.Service.CommandObjects.Api;
using SensorData.SharedComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.Service
{
    public class SensorDataServerRegistry : ISensorDataServerRegistry
    {
        private static readonly SensorDataServerRegistrar[] _registeredServerCreators =
        {
            new SensorDataServerRegistrar((IConfiguration configuration) =>  new ApiCommunicationServer(configuration)),
            new SensorDataServerRegistrar((IConfiguration configuration) =>  SensorCommunicationServer.CreateInstance(configuration), initializeOnCreate:false),
        };

        public IEnumerable<SensorDataServerRegistrar> RegisteredServerCreators { get => _registeredServerCreators; }
    }
}
