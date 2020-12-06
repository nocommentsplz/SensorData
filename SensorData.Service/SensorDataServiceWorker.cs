using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SensorData.Service.Settings;
using SensorData.SharedComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
        ISensorValueReportGenerator _reportGenerator;
        SensorValueReportSettings _reportSettings;

        public SensorDataServiceWorker(ILogger<SensorDataServiceWorker> logger, IConfiguration configuration,
            ISensorValueReportGenerator reportGenerator, ISensorDataServerRegistry sensorDataServerRegistry)
        {
            _logger = logger;
            _configuration = configuration;
            _sensorDataServerRegistry = sensorDataServerRegistry;
            _reportGenerator = reportGenerator;

            _reportSettings = _configuration.GetSection("SensorValueReportSettings").Get<SensorValueReportSettings>();
            _reportGenerator.Settings = _reportSettings;
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
                var workers = new List<Task>();

                workers.Add(SaveReportToFile(stoppingToken));
                workers.Add(DisplayReportInConsole(stoppingToken));

                await Task.WhenAll(workers.ToArray());
            }
        }

        private async Task SaveReportToFile(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_reportSettings.ReportInFileIntervalInSeconds * 1000, stoppingToken);
                _reportGenerator.SaveReportToFile();
            }
        }

        private async Task DisplayReportInConsole(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_reportSettings.ReportInConsoleIntervalInSeconds * 1000, stoppingToken);
                var report = _reportGenerator.GenerateReport();
                if (report.Any())
                {
                    string reportText = JsonSerializer.Serialize(report, new JsonSerializerOptions() { WriteIndented = true });
                    _logger.LogInformation($"Generating report at {DateTimeOffset.Now}\n${reportText}");
                }
            }
        }
    }
}
