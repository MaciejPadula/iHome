using iHome.AzureFunctions;
using iHome.Core.Logic.Database;
using iHome.Core.Models.Application;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;

[assembly: FunctionsStartup(typeof(Startup))]
namespace iHome.AzureFunctions
{
    public class Startup : FunctionsStartup
    {
        ApplicationSettings _applicationSettings;
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.ConfigurationBuilder
                .AddEnvironmentVariables();

            _applicationSettings = builder.ConfigurationBuilder.Build().Get<ApplicationSettings>();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(_applicationSettings.AzureConnectionString));
        }
    }
}
