using Azure.Storage.Queues;
using iHome.Infrastructure.Queue.Models;
using iHome.Infrastructure.Queue.Service;
using iHome.Infrastructure.Queue.Service.Read;
using iHome.Infrastructure.Queue.Service.Write;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace iHome.Infrastructure.Queue.Helpers;

public static class DataUpdateDependencyInjectionExtensions
{
    private const string QueueName = "device-data-events";
    private static QueueClient? _queueClient;

    public static IServiceCollection AddDataQueueReader(this IServiceCollection services, string? azureConnectionString)
    {
        services.TryAddScoped<IQueueReader<DataUpdateModel>>(_ => 
            new AzureQueueReader<DataUpdateModel>(CreateQueueClient(azureConnectionString)));

        return services;
    }

    public static IServiceCollection AddDataQueueWriter(this IServiceCollection services, string? azureConnectionString)
    {
        services.TryAddScoped<IQueueWriter<DataUpdateModel>>(_ =>
            new AzureQueueWriter<DataUpdateModel>(CreateQueueClient(azureConnectionString)));

        return services;
    }

    public static IServiceCollection AddDataUpdateQueue(this IServiceCollection services, string? azureConnectionString)
    {
        services.AddDataQueueReader(azureConnectionString);
        services.AddDataQueueWriter(azureConnectionString);
        services.AddScoped<IQueueFullAccess<DataUpdateModel>, QueueFullAccess<DataUpdateModel>>();

        return services;
    }

    private static QueueClient CreateQueueClient(string? azureConnectionString)
    {
        _queueClient ??= new QueueClient(azureConnectionString, QueueName);

        return _queueClient;
    }
}
