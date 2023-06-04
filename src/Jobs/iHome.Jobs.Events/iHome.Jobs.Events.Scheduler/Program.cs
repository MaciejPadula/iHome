using iHome.Infrastructure.Queue.Helpers;
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

        services.AddDataQueueWriter(config.GetValue<string>("Azure:StorageConnectionString"), ServiceLifetime.Singleton);
        services.AddDbContext<SqlDataContext>(o => o.UseSqlServer(config.GetValue<string>("ConnectionStrings:AzureSQL")), ServiceLifetime.Singleton);

        services.AddTransient<IDateTimeProvider, DefaultDateTimeProvider>();
        services.AddTransient<ISchedulesProvider, SchedulesProvider>();
        services.AddTransient<IScheduleRepository, EFScheduleRepository>();

        services.AddHostedService<ScheduleWorker>();
    })
    .Build();

host.Run();
