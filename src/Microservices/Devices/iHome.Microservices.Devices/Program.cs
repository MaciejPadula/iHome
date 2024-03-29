using iHome.Infrastructure.Sql.Helpers;
using iHome.Infrastructure.Firestore.Helpers;
using iHome.Microservices.Devices.Contract;
using iHome.Microservices.Devices.Controllers;
using iHome.Microservices.Devices.Infrastructure;
using iHome.Microservices.Devices.Infrastructure.Models;
using Web.Infrastructure.Microservices.Server.Builders;
using iHome.Microservices.Devices.Managers;
using iHome.Microservices.Devices.Providers;
using iHome.Microservices.Devices.Handlers;

var builder = new MicroserviceBuilder(args);

builder.Services.AddDbConnectionFactory(opt =>
{
    opt.ConnectionString = builder.Configuration["ConnectionStrings:SqlConnectionString"]
        ?? throw new ArgumentException("SqlConnectionString");
});
builder.Services.AddFirestoreConnectionFactory(opt =>
{
    var config = builder.Configuration.GetSection("FirestoreConfig").Get<ServiceAccountSettings>()
        ?? throw new ArgumentException("FirestoreConfig");

    opt.ProjectId = config.ProjectId;
    opt.JsonCredentials = config;
});
builder.Services.AddRepositories();
builder.Services.AddScoped<IDeviceDataHandler, DeviceDataHandler>();
builder.Services.AddScoped<IDeviceManager, DeviceManager>();
builder.Services.AddScoped<IDeviceDataSetter, DeviceDataSetter>();

builder.RegisterMicroservice<IDeviceDataService, DeviceDataController>();
builder.RegisterMicroservice<IDeviceManagementService, DeviceManagementController>();

builder.ConfigureApp(app =>
{
    app.UseHttpsRedirection();
    app.UseAuthorization();
});

var app = builder.Build();

app.Run();
