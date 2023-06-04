using iHome.Infrastructure.Queue.Helpers;
using iHome.Jobs.Events.EventsExecutor;
using iHome.Microservices.Devices.Contract;
using Web.Infrastructure.Microservices.Client.Configuration.Extensions;
using Web.Infrastructure.Microservices.Client.Extensions;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;

        services.AddDataQueueReader(config.GetValue<string>("Azure:StorageConnectionString"), ServiceLifetime.Singleton);
        services.AddConfigurationServiceLookup("Microservices");
        services.AddMicroserviceClient<IDeviceDataService>(ServiceLifetime.Singleton);

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
