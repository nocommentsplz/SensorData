using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SensorData.Service.Settings;
using SensorData.SharedComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.Service
{
    static class ServiceCollectionExtensions
    {
        internal static void AddCommunicationServerComponents(this IServiceCollection services)
        {
            services.AddTransient<ITcpCommunicationServer<ApiCommandObject>, TcpCommunicationServer<ApiCommandObject>>();
        }

        internal static void LoadSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var apiTcpServerSettingsSection = configuration.GetSection("ApiTcpServerSettings");
            services.Configure<ApiTcpServerSettings>(apiTcpServerSettingsSection);
        }
    }
}
