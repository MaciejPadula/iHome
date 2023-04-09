using iHome.Infrastructure.SQL.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Infrastructure.SQL.Helpers;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDataContexts(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        return services.AddDbContext<SqlDataContext>(o => o.UseSqlServer(connectionString));
    }
}
