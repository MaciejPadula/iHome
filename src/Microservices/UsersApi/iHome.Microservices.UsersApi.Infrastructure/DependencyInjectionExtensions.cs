using iHome.Microservices.UsersApi.Contract;
using iHome.Microservices.UsersApi.Infrastructure.Models;
using iHome.Microservices.UsersApi.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Microservices.UsersApi.Infrastructure
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddAuthRepository(this IServiceCollection services,
            Action<Auth0ApiConfiguration> optionsPredicate)
        {
            var options = new Auth0ApiConfiguration();
            optionsPredicate(options);

            services.AddSingleton(options);
            services.AddHttpClient<IUserRepository, Auth0UserRepository>();
            services.AddScoped<IUserRepository, Auth0UserRepository>();

            return services;
        }
    }
}
