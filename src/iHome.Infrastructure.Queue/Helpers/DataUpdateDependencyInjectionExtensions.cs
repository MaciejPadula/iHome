using iHome.Infrastructure.Queue.DataUpdate;
using iHome.Infrastructure.Queue.DataUpdate.Read;
using iHome.Infrastructure.Queue.DataUpdate.Write;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace iHome.Infrastructure.Queue.Helpers
{
    public static class DataUpdateDependencyInjectionExtensions
    {
        public static IServiceCollection AddDataQueueReader(this IServiceCollection services, string? azureConnectionString)
        {
            services.TryAddScoped<IDataUpdateQueueReader>(provider => new AzureDataQueueReader(azureConnectionString ?? string.Empty));
            return services;
        }

        public static IServiceCollection AddDataQueueWriter(this IServiceCollection services, string? azureConnectionString)
        {
            services.TryAddScoped<IDataUpdateQueueWriter>(provider => new AzureDataUpdateQueueWriter(azureConnectionString ?? string.Empty));
            return services;
        }

        public static IServiceCollection AddDataUpdateQueue(this IServiceCollection services, string? azureConnectionString)
        {
            services.AddDataQueueReader(azureConnectionString);
            services.AddDataQueueWriter(azureConnectionString);
            services.AddScoped<IDataUpdateQueue, DataUpdateQueue>();

            return services;
        }
    }
}
