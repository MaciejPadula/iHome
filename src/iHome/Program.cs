using iHome.Core.Helpers;
using iHome.Infrastructure.Firebase.Helpers;
using iHome.Infrastructure.SQL.Helpers;
using iHome.Logic;
using iHome.Microservices.OpenAI.Contract;
using iHome.Microservices.RoomsManagement.Contract;
using iHome.Microservices.UsersApi.Contract;
using iHome.Microservices.Widgets.Contract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using Web.Infrastructure.Microservices.Client.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserAccessor, HttpContextUserAccessor>();
builder.Services.AddMicroserviceClient<IRoomManagementService>("https://localhost:7019");
builder.Services.AddMicroserviceClient<IRoomSharingService>("https://localhost:7019");
builder.Services.AddMicroserviceClient<IUserManagementService>("https://localhost:7094");
builder.Services.AddMicroserviceClient<IWidgetManagementService>("https://localhost:7206");
builder.Services.AddMicroserviceClient<IWidgetDeviceManagementService>("https://localhost:7206");
builder.Services.AddMicroserviceClient<ISuggestionsService>("https://localhost:7018");

builder.Services.AddDataContexts(builder.Configuration["ConnectionStrings:AzureSQL"]);
builder.Services.AddFirebaseRepositories(builder.Configuration["Firebase:Url"], builder.Configuration["Firebase:AuthToken"]);

builder.Services.AddCoreServices();


builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo { Title = "iHome", Version = "v1" });
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "Using the Authorization header with the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    o.AddSecurityDefinition("Bearer", securitySchema);

    o.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                });
});

builder.Services.AddControllersWithViews();

string domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = domain;
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json", "iHome V1"));
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
