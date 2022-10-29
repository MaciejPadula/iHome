using iHome.Core.Logic.UserInfo;
using iHome.Core.Models.Application;
using iHome.Core.Services.DevicesService;
using iHome.Core.Services.RoomsService;
using iHome.Hubs;
using iHome.Logic.Notificator;
using Microsoft.OpenApi.Models;

namespace iHome.Backend.Services
{
    public static class IServiceExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IUserInfo, UserInfo>()
                .AddScoped<IDevicesService, DevicesService>()
                .AddScoped<IRoomsService, RoomsService>()
                .AddScoped<INotificator, Notificator>();
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection services, ConfigurationManager config)
        {
            return services.Configure<ApplicationSettings>(config);
        }

        public static IServiceCollection AddSignalRHubs(this IServiceCollection services)
        {
            return services.AddScoped<RoomsHub>();
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "iHome Rooms API", Version = "v1" });
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "Using the Authorization header with the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securitySchema);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                });
            });
        }

        public static IServiceCollection ConfigureCors(this IServiceCollection services, string myPolicy)
        {
            return services.AddCors(options =>
            {
                options.AddPolicy(myPolicy,
                    policy =>
                    {
                        policy
                            .SetIsOriginAllowed(_ => true)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });
        }
    }
}
