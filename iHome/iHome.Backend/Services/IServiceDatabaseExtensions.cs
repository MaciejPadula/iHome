using iHome.Core.Logic.Database;
using Microsoft.EntityFrameworkCore;

namespace iHome.Backend.Services
{
    public static class IServiceDatabaseExtensions
    {
        public static IServiceCollection AddAzureSqlServer(this IServiceCollection services, string connectionString)
        {
            return services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                });
            });
        }

        public static IServiceCollection AddAzureCosmosDb(this IServiceCollection services, string connectionString, string databaseName)
        {
            return services.AddDbContext<AppDbContext>(options =>
            {
                options.UseCosmos(connectionString, databaseName);
            });
        }
    }
}
