using iHome.Microservices.UsersApi.Contract;
using iHome.Microservices.UsersApi.Controllers;
using iHome.Microservices.UsersApi.Infrastructure;
using iHome.Microservices.UsersApi.Infrastructure.Models;
using iHome.Microservices.UsersApi.Services;
using Web.Infrastructure.Microservices.Server.Builders;

var builder = new MicroserviceBuilder(args);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAuthRepository(opt =>
{
    opt.ApiUrl = builder.Configuration[$"{Auth0ApiConfiguration.Key}:{nameof(Auth0ApiConfiguration.ApiUrl)}"]
        ?? throw new NullReferenceException();

    opt.ApiToken = builder.Configuration[$"{Auth0ApiConfiguration.Key}:{nameof(Auth0ApiConfiguration.ApiToken)}"]
        ?? throw new NullReferenceException();
});
builder.Services.AddControllers();

builder.RegisterMicroservice<IUserManagementService, UserManagementController>();

builder.ConfigureApp(app =>
{
    app.UseHttpsRedirection();
    app.UseAuthorization();
});

var app = builder.Build();
app.Run();
