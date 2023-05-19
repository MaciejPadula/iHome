using iHome.Microservices.UsersApi.Infrastructure;
using iHome.Microservices.UsersApi.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<Auth0ApiConfiguration>(builder.Configuration.GetSection("Auth0"));
builder.Services.AddServices();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
