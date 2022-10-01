using Auth0.AspNetCore.Authentication;
using iHome.Backend.Middleware;
using iHome.Core.Logic.Database;
using iHome.Core.Logic.UserInfo;
using iHome.Core.Models.Application;
using iHome.Core.Services.DevicesService;
using iHome.Core.Services.RoomsService;
using iHome.Hubs;
using iHome.Logic.Notificator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

if (!Directory.Exists("wwwroot"))
    Directory.CreateDirectory("wwwroot");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // If the request is for our hub...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/roomsHub")))
            {
                // Read the token out of the query string
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
}).AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
    options.Scope = "openid profile email";
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "iHome Rooms API", Version = "v1" });
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

    c.AddSecurityDefinition("Bearer", securitySchema);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { "Bearer" } }
    });
});

var myPolicy = "Angular";

builder.Services.AddCors(options =>
{
    options.AddPolicy(myPolicy,
        policy =>
        {
            policy
                .SetIsOriginAllowed(_ => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddControllers();

builder.Services
    .Configure<ApplicationSettings>(builder.Configuration)
    .AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration["AzureConnectionString"], sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure();
        });
    })
    .AddScoped<IUserInfo, UserInfo>()
    .AddScoped<IDevicesService, DevicesService>()
    .AddScoped<IRoomsService, RoomsService>()
    .AddScoped<INotificator, Notificator>()
    .AddScoped<RoomsHub>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "iHome Rooms API v1");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(myPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<RoomsHub>("/roomsHub");
app.Use(async (context, next) =>
{
    var hubContext = context.RequestServices
                            .GetRequiredService<IHubContext<RoomsHub>>();

    if (next != null)
    {
        await next.Invoke();
    }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Rooms}"
);

app.UseMiddleware<ErrorHandlerMiddleware>();

if (app.Environment.IsProduction())
{
    app.UseSpa(spa =>
    {
        spa.Options.SourcePath = "wwwroot";
    });
}


app.Run();
