using Microsoft.Extensions.Configuration;
using SensorData.SharedComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.Service
{
    public class SensorDataServerRegistrar
    {
        public SensorDataServerRegistrar(Func<IConfiguration, ICommunicationServer> serverCreator, bool initializeOnCreate = true)
        {
            ServerCreator = serverCreator;
            InitializeOnCreate = initializeOnCreate;
            ServerId = Guid.NewGuid();
        }

        public Func<IConfiguration, ICommunicationServer> ServerCreator { get; private set; }
        public bool InitializeOnCreate { get; private set; }
        public Guid ServerId { get; }
    }
}
