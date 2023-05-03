using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace iHome.Backend.Services
{
    public static class IServiceAuthenticationExtensions
    {
        public static Auth0WebAppAuthenticationBuilder ConfigureAuth0Authentication(this IServiceCollection services, ConfigurationManager config)
        {
            return services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = $"https://{config["Auth0:Domain"]}/";
                    options.Audience = config["Auth0:Audience"];
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/roomsHub")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                })
                .AddAuth0WebAppAuthentication(options =>
                {
                    options.Domain = config["Auth0:Domain"];
                    options.ClientId = config["Auth0:ClientId"];
                    options.Scope = "openid profile email";
                });
        }
    }
}
