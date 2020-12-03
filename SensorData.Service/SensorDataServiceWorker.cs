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
        private readonly ITcpCommunicationServer<ApiCommandObject> _apiServer;
        private readonly ApiTcpServerSettings _apiSettings;

        public SensorDataServiceWorker(ILogger<SensorDataServiceWorker> logger,
            IOptions<ApiTcpServerSettings> apiSettings, ITcpCommunicationServer<ApiCommandObject> apiServer)
        {
            _logger = logger;
            _apiSettings = apiSettings.Value;
            _apiServer = apiServer;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _apiServer.Settings = _apiSettings;
            _apiServer.Initialize();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _apiServer.Shutdown();
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
