using iHome.Microservices.Devices.Infrastructure;
using iHome.Microservices.Devices.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<FirebaseSettings>(builder.Configuration.GetSection("Firebase"));
builder.Services.AddRepositories();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
