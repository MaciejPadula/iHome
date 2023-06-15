using iHome.Infrastructure.Sql.Helpers;
using iHome.Microservices.Widgets.Contract;
using iHome.Microservices.Widgets.Controllers;
using iHome.Microservices.Widgets.Infrastructure;
using Web.Infrastructure.Microservices.Server.Builders;

var builder = new MicroserviceBuilder(args);

builder.Services.AddDbConnectionFactory(builder.Configuration["ConnectionStrings:SqlConnectionString"] ?? string.Empty);
builder.Services.AddRepositories();

builder.RegisterMicroservice<IWidgetDeviceManagementService, WidgetDeviceManagementController>();
builder.RegisterMicroservice<IWidgetManagementService, WidgetManagementController>();

builder.ConfigureApp(app =>
{
    app.UseHttpsRedirection();
    app.UseAuthorization();
});

var app = builder.Build();
app.Run();
