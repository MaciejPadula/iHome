using iHome.Jobs.Events.Scheduler.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ScheduleWorker>();
    })
    .Build();

host.Run();
