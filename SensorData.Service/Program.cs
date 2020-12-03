using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SensorData.Service.Settings;
using SensorData.SharedComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensorData.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.LoadSettings(hostContext.Configuration);
                    services.AddCommunicationServerComponents();
                    services.AddHostedService<SensorDataServiceWorker>();
                });
    }
}
