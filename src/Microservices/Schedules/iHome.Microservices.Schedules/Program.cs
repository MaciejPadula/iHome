using iHome.Infrastructure.Sql.Helpers;
using iHome.Microservices.Schedules.Contract;
using iHome.Microservices.Schedules.Controllers;
using iHome.Microservices.Schedules.Infrastructure;
using iHome.Microservices.Schedules.Providers;
using Web.Infrastructure.Microservices.Server.Builders;

var builder = new MicroserviceBuilder(args);

builder.Services.AddDbConnectionFactory(builder.Configuration["ConnectionStrings:SqlConnectionString"] ?? string.Empty);
builder.Services.AddRepositories();
builder.Services.AddScoped<IDeviceForSchedulingTypesProvider, DeviceForSchedulingTypesProvider>();

builder.RegisterMicroservice<IScheduleDeviceManagementService, ScheduleDeviceManagementController>();
builder.RegisterMicroservice<IScheduleManagementService, ScheduleManagementController>();

builder.ConfigureApp(app =>
{
    app.UseHttpsRedirection();
    app.UseAuthorization();
});

var app = builder.Build();
app.Run();
