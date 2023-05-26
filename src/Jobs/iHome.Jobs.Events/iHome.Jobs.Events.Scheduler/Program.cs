using iHome.Infrastructure.Queue.Helpers;
using iHome.Jobs.Events.Contexts;
using iHome.Jobs.Events.Infrastructure.Contexts;
using iHome.Jobs.Events.Infrastructure.Helpers;
using iHome.Jobs.Events.Infrastructure.Repositories;
using iHome.Jobs.Events.Scheduler.Services;
using iHome.Jobs.Events.Services;
using Microsoft.EntityFrameworkCore;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;

        services.AddSingleton<WorkerContext>();
        services.AddDataQueueWriter(config.GetValue<string>("Azure:StorageConnectionString"));
        services.AddDbContext<SqlDataContext>(o => o.UseSqlServer(config.GetValue<string>("ConnectionStrings:AzureSQL")));
        services.AddTransient<IDateTimeProvider, DefaultDateTimeProvider>();
        services.AddScoped<ISchedulesProvider, SchedulesProvider>();
        services.AddScoped<IScheduleRepository, EFScheduleRepository>();
        services.AddHostedService<ScheduleWorker>();
    })
    .Build();

host.Run();
