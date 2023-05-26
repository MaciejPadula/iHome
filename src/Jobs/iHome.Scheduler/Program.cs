using iHome.Infrastructure.Queue.Helpers;
using iHome.Infrastructure.SQL.Helpers;
using iHome.Scheduler.Contexts;
using iHome.Scheduler.Infrastructure.Helpers;
using iHome.Scheduler.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();

services.AddLogging(builder =>
{
    builder
        .AddConsole()
        .AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Error)
        .SetMinimumLevel(LogLevel.Information);
});

services.AddDataContexts(configuration["ConnectionStrings:AzureSQL"]);
services.AddDataQueueWriter(configuration["Azure:StorageConnectionString"]);
services.AddSchedulesService();

services.AddTransient<ISchedulesProvider, SchedulesProvider>();

services.AddSingleton<WorkerContext>();
services.AddSingleton<IScheduleWorker, ScheduleWorker>();

var worker = services.BuildServiceProvider().GetService<IScheduleWorker>();

if (worker == null)
{
    return;
}

await worker.Start();
