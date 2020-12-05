using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SensorData.DeviceStub.Settings;
using SensorData.SharedComponents;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SensorData.DeviceStub
{
    class Program
    {
        public static IConfiguration configuration;

        static async Task Main(string[] args)
        {

            byte deviceId = 0;
            if (args.Length > 0)
            {
                byte.TryParse(args[0], out deviceId);
            }

            while (deviceId <= 0 || deviceId > 255)
            {
                Console.Write("Enter Device Id (between 1 - 255) : ");
                byte.TryParse(Console.ReadLine(), out deviceId);
            }


            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            int serverPort;

            if (!int.TryParse(configuration.GetSection("SensorDataServiceSettings:ServerPort").Value, out serverPort))
            {
                Console.WriteLine("SensorDataServiceSettings:ServerPort contains bad value");
                return;
            }

            Console.WriteLine($"Device Stub Initializing with Id {deviceId}, Connecting to Port {serverPort}");

            ITcpCommunicationClient<SensorCommandObject> communicationClient = new TcpCommunicationClient<SensorCommandObject>();
            if (!communicationClient.Connect(new SensorDataServiceSettings() { ServerPort = serverPort }))
            {
                Console.WriteLine("Could not establish connection with server");
                return;
            }

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            _ = Task.Run(() =>
            {
                Console.WriteLine("Press ESC to stop");
                do
                {
                    while (!Console.KeyAvailable)
                    {
                        // Do something
                    }
                } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

                cancellationTokenSource.Cancel();
            });

            await Task.Run(async () =>
            {
                Random randomGenerator = new Random();
                while (!cancellationToken.IsCancellationRequested)
                {
                    byte[] data = { (byte)randomGenerator.Next(0, 255) };
                    communicationClient.Send(new SensorCommandObject(0xAA, deviceId, data));
                    Console.WriteLine(data[0]);
                    await Task.Delay(1000, cancellationToken);
                }
            });

            Console.ReadKey();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Add access to generic IConfigurationRoot
            services.AddSingleton<IConfiguration>(configuration);
        }
    }
}
