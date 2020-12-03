using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SensorData.Api.Settings;
using SensorData.SharedComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.Api
{
    static class ServiceCollectionExtensions
    {
        internal static void AddCommunicationClientComponents(this IServiceCollection services)
        {
            services.AddSingleton<ITcpCommunicationClient<ApiCommandObject>, TcpCommunicationClient<ApiCommandObject>>();
        }

        internal static void LoadSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var sensorDataServiceSettingsSection = configuration.GetSection("SensorDataServiceSettings");
            services.Configure<SensorDataServiceSettings>(sensorDataServiceSettingsSection);
        }
    }
}
