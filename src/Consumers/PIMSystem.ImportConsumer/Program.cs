using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MetroBus;
using MetroBus.Microsoft.Extensions.DependencyInjection;
using PIMSystem.ImportConsumer.Consumers;
using System.IO;

namespace PIMSystem.ImportConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            new HostBuilder()
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(basePath: Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json", optional: true);
                config.AddEnvironmentVariables();
            })
            .ConfigureServices((hostContext, services) =>
            {
                string rabbitMqUri = hostContext.Configuration.GetValue<string>("RabbitMqUri");
                string rabbitMqUserName = hostContext.Configuration.GetValue<string>("RabbitMqUserName");
                string rabbitMqPassword = hostContext.Configuration.GetValue<string>("RabbitMqPassword");
                string productImportedQueueName = hostContext.Configuration.GetValue<string>("ProductImportedQueueName");
                string categoryImportedQueueName = hostContext.Configuration.GetValue<string>("CategoryImportedQueueName");

                services.AddMetroBus(x =>
                {
                    x.AddConsumer<ProductImportedConsumer>();
                    x.AddConsumer<CategoryImportedConsumer>();
                });

                services.AddSingleton(provider => MetroBusInitializer.Instance
                    .UseRabbitMq(rabbitMqUri, rabbitMqUserName, rabbitMqPassword)
                    .RegisterConsumer<ProductImportedConsumer>(productImportedQueueName)
                    .RegisterConsumer<CategoryImportedConsumer>(categoryImportedQueueName)
                        .UseRetryPolicy()
                        .UseIncrementalRetryPolicy(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10))
                        .Then()
                    .UseCircuitBreaker(10, 5, TimeSpan.FromSeconds(10))
                    .Build())
                    .BuildServiceProvider();
                services.AddHostedService<BusService>();
            })
            .RunConsoleAsync()
            .Wait();
        }
    }
}
