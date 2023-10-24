using iHome.Infrastructure.Queue.Helpers;
using iHome.Infrastructure.Queue.Models;
using iHome.Jobs.Events.Infrastructure.Contexts;
using iHome.Jobs.Events.Infrastructure.Helpers;
using iHome.Jobs.Events.Infrastructure.Repositories;
using iHome.Jobs.Events.Scheduler.Logic;
using iHome.Jobs.Events.Scheduler.Services;
using iHome.Jobs.Events.Services;
using Microsoft.EntityFrameworkCore;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;
        services.AddApplicationInsightsTelemetryWorkerService(config);
        services.AddQueueWriter<DataUpdateModel>(opt =>
        {
            opt.QueueName = "device-data-events";
            opt.ConnectionString = config.GetValue<string>("Azure:StorageConnectionString");
        });
        services.AddDbContext<SqlDataContext>(o => o.UseSqlServer(config.GetValue<string>("ConnectionStrings:AzureSQL")));

        services.AddTransient<IScheduleRunningConditionChecker, ScheduleRunningConditionChecker>();
        services.AddTransient<IDateTimeProvider, DefaultDateTimeProvider>();
        services.AddTransient<ISchedulesProvider, SchedulesProvider>();
        services.AddTransient<IScheduleHistoryRepository, EFScheduleHistoryRepository>();
        services.AddTransient<IScheduleRepository, EFScheduleRepository>();

        services.AddHostedService<ScheduleWorker>();
    })
    .Build();

await host.RunAsync();
