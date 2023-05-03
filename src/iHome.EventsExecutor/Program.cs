using iHome.EventsExecutor;
using iHome.Infrastructure.Firebase.Helpers;
using iHome.Infrastructure.Queue.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();

services.AddDataQueueReader(configuration["Azure:StorageConnectionString"]);
services.AddFirebaseRepositories(configuration["Firebase:Url"], string.Empty);
services.AddScoped<Worker>();

var worker = services.BuildServiceProvider().GetService<Worker>();

if (worker == null)
{
    return;
}

await worker.Start();