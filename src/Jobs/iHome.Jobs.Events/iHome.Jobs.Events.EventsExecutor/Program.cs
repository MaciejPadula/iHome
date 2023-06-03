using iHome.Infrastructure.Queue.Helpers;
using iHome.Jobs.Events.EventsExecutor;
using iHome.Microservices.Devices.Contract;
using Web.Infrastructure.Microservices.Client.Extensions;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;

        services.AddDataQueueWriter(config.GetValue<string>("Azure:StorageConnectionString"));
        services.AddMicroserviceClient<IDeviceDataService>("http://172.30.0.3:5002");
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
