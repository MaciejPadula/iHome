using iHome.Microservices.UsersApi.Contract;
using iHome.Microservices.UsersApi.Controllers;
using iHome.Microservices.UsersApi.Infrastructure;
using iHome.Microservices.UsersApi.Infrastructure.Models;
using Web.Infrastructure.Microservices.Server.Builders;

var builder = new MicroserviceBuilder(args);

builder.Services.Configure<Auth0ApiConfiguration>(builder.Configuration.GetSection(Auth0ApiConfiguration.Key));
builder.Services.AddServices();
builder.Services.AddControllers();

builder.RegisterMicroservice<IUserManagementService, UserManagementController>();

builder.ConfigureApp(app =>
{
    app.UseHttpsRedirection();
    app.UseAuthorization();
});

var app = builder.Build();
app.Run();
