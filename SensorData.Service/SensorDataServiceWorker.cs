using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SensorData.Service.Settings;
using SensorData.SharedComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SensorData.Service
{
    public class SensorDataServiceWorker : BackgroundService
    {
        private readonly ILogger<SensorDataServiceWorker> _logger;
        IConfiguration _configuration;
        Dictionary<Guid, ICommunicationServer> _servers;
        ISensorDataServerRegistry _sensorDataServerRegistry;

        public SensorDataServiceWorker(ILogger<SensorDataServiceWorker> logger,
            IConfiguration configuration, ISensorDataServerRegistry sensorDataServerRegistry)
        {
            _logger = logger;
            _configuration = configuration;
            _sensorDataServerRegistry = sensorDataServerRegistry;            
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _servers = new Dictionary<Guid, ICommunicationServer>();
            foreach (SensorDataServerRegistrar registrar in _sensorDataServerRegistry.RegisteredServerCreators)
            {
                ICommunicationServer server = registrar.ServerCreator(_configuration);
                if (registrar.InitializeOnCreate)
                {
                    server.Initialize();
                }

                _servers.Add(registrar.ServerId, server);
            }

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (ICommunicationServer server in _servers.Values)
            {
                server.Shutdown();
            }

            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("{time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
