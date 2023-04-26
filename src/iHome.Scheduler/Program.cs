using iHome.Infrastructure.Firebase.Helpers;
using iHome.Infrastructure.SQL.Helpers;
using iHome.Scheduler.Contexts;
using iHome.Scheduler.Infrastructure.Helpers;
using iHome.Scheduler.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();

services.AddLogging(builder =>
{
    builder
        .AddConsole()
        .AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Error)
        .SetMinimumLevel(LogLevel.Information);
});

services.AddDataContexts("");
services.AddFirebaseRepositories("", "");
services.AddSchedulesService();

services.AddTransient<ISchedulesProvider, SchedulesProvider>();
services.AddTransient<ISchedulesUpdater, SchedulesUpdater>();

services.AddSingleton<WorkerContext>();
services.AddSingleton<IScheduleWorker, ScheduleWorker>();

var worker = services.BuildServiceProvider().GetService<IScheduleWorker>();

if (worker == null)
{
    return;
}

await worker.Start();
