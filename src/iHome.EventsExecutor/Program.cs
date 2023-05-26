using iHome.EventsExecutor;
using iHome.Infrastructure.Queue.Helpers;
using iHome.Microservices.Devices.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Infrastructure.Microservices.Client.Extensions;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();

services.AddDataQueueReader(configuration["Azure:StorageConnectionString"]);
services.AddMicroserviceClient<IDeviceDataService>("https://localhost:7062");
services.AddScoped<Worker>();

var worker = services.BuildServiceProvider().GetService<Worker>();

if (worker == null)
{
    return;
}

await worker.Start();