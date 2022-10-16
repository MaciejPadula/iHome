using iHome.Backend.Middleware;
using iHome.Backend.Services;
using iHome.Hubs;
using Microsoft.AspNetCore.SignalR;

if (!Directory.Exists("wwwroot"))
    Directory.CreateDirectory("wwwroot");

var builder = WebApplication.CreateBuilder(args);

const string myPolicy = "Angular";

builder.Services
    .ConfigureSwagger()
    .ConfigureCors(myPolicy)
    .ConfigureAuth0Authentication(builder.Configuration);

builder.Services
    .AddControllers();

builder.Services
    .AddConfiguration(builder.Configuration)
    .AddAzureSqlServer(builder.Configuration["AzureConnectionString"])
    .AddApiServices()
    .AddSignalRHubs()
    .AddSignalR();

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
