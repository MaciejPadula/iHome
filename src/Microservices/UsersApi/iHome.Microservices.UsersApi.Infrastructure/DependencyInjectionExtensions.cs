using iHome.Core.Services.Users;
using iHome.Microservices.UsersApi.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Microservices.UsersApi.Infrastructure
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHttpClient<IUserManagementService, Auth0UserManagementService>();
            services.AddScoped<IUserManagementService, Auth0UserManagementService>();

            return services;
        }
    }
}
