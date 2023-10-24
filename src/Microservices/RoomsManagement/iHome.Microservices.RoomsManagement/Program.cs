using iHome.Infrastructure.Sql.Helpers;
using iHome.Microservices.RoomsManagement.Contract;
using iHome.Microservices.RoomsManagement.Controllers;
using iHome.Microservices.RoomsManagement.Infrastructure;
using Web.Infrastructure.Microservices.Server.Builders;

var builder = new MicroserviceBuilder(args);

builder.Services.AddDbConnectionFactory(opt =>
{
    opt.ConnectionString = builder.Configuration["ConnectionStrings:SqlConnectionString"] ?? string.Empty;
});
builder.Services.AddRepositories();

builder.RegisterMicroservice<IRoomManagementService, RoomManagementController>();
builder.RegisterMicroservice<IRoomSharingService, RoomSharingController>();

builder.ConfigureApp(app =>
{
    app.UseHttpsRedirection();
    app.UseAuthorization();
});

var app = builder.Build();
app.Run();
