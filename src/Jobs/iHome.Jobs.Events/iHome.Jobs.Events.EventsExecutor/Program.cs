using iHome.Infrastructure.Queue.Helpers;
using iHome.Infrastructure.Queue.Models;
using iHome.Jobs.Events.EventsExecutor;
using iHome.Jobs.Events.EventsExecutor.Services;
using iHome.Microservices.Devices.Contract;
using Web.Infrastructure.Microservices.Client.Builders;
using Web.Infrastructure.Microservices.Client.Configuration.Extensions;
using Web.Infrastructure.Microservices.Client.Extensions;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;
        services.AddApplicationInsightsTelemetryWorkerService(config);

        services.AddQueueReader<DataUpdateModel>(opt =>
        {
            opt.QueueName = "device-data-events";
            opt.ConnectionString = config.GetValue<string>("Azure:StorageConnectionString");
        });
        services.AddConfigurationServiceLookup("Microservices");
        services.AddMicroserviceClient<IDeviceDataService>(o => { },
            MicroserviceClientConfigurationBuilder.GetLongRunningConfig());
        services.AddTransient<IScheduleDevicesProcessor, ScheduleDevicesProcessor>();
        services.AddTransient<IScheduleDevicesProvider, ScheduleDevicesProvider>();

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
