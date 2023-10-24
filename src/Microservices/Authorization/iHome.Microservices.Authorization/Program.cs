using iHome.Microservices.Authorization.Contract;
using Web.Infrastructure.Microservices.Server.Builders;
using iHome.Microservices.Authorization.Infrastructure;
using iHome.Infrastructure.Sql.Helpers;
using iHome.Microservices.Authorization.Controllers;
using iHome.Microservices.Authorization.Managers;

var builder = new MicroserviceBuilder(args);

builder.Services.AddDbConnectionFactory(opt =>
{
    opt.ConnectionString = builder.Configuration["ConnectionStrings:SqlConnectionString"] ?? string.Empty;
});
builder.Services.AddRepositories();
builder.Services.AddScoped<IRoomManager, RoomManager>();
builder.Services.AddScoped<IDeviceManager, DeviceManager>();
builder.Services.AddScoped<IScheduleManager, ScheduleManager>();
builder.Services.AddScoped<IWidgetManager, WidgetManager>();

builder.RegisterMicroservice<IRoomAuthService, RoomAuthController>();
builder.RegisterMicroservice<IDeviceAuthService, DeviceAuthController>();
builder.RegisterMicroservice<IScheduleAuthService, ScheduleAuthController>();
builder.RegisterMicroservice<IWidgetAuthService, WidgetAuthController>();

builder.ConfigureApp(app =>
{
    app.UseHttpsRedirection();
    app.UseAuthorization();
});

var app = builder.Build();
app.Run();
