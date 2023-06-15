using iHome.Infrastructure.Sql.Helpers;
using iHome.Microservices.Devices.Contract;
using iHome.Microservices.Devices.Controllers;
using iHome.Microservices.Devices.Infrastructure;
using iHome.Microservices.Devices.Infrastructure.Models;
using Web.Infrastructure.Microservices.Server.Builders;

var builder = new MicroserviceBuilder(args);

builder.Services.AddDbConnectionFactory(builder.Configuration["ConnectionStrings:SqlConnectionString"] ?? string.Empty);
builder.Services.Configure<FirebaseSettings>(builder.Configuration.GetSection("Firebase"));
builder.Services.AddRepositories();

builder.RegisterMicroservice<IDeviceDataService, DeviceDataController>();
builder.RegisterMicroservice<IDeviceManagementService, DeviceManagementController>();

builder.ConfigureApp(app =>
{
    app.UseHttpsRedirection();
    app.UseAuthorization();
});

var app = builder.Build();

app.Run();
