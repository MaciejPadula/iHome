using iHome.Core.Logic.Database;
using iHome.Core.Models.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace iHome.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            /*var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .Get<ApplicationSettings>();
            
             
             .Configure<ApplicationSettings>(options =>
                {
                    options.AzureConnectionString = config.AzureConnectionString;
                    options.Auth0ApiSecret = config.Auth0ApiSecret;
                })*/

            builder.Services
                .AddScoped<HttpClient>()
                .AddScoped<MainPage>();

            return builder.Build();
        }
    }
}