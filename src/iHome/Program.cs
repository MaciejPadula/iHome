using iHome.Core.Helpers;
using iHome.Logic;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddScoped<IUserAccessor, MockUserAccessor>()
    .AddDataContexts(
        options => options.UseSqlServer(builder.Configuration["ConnectionStrings:AzureSQL"]),
        options => options.UseCosmos(builder.Configuration["ConnectionStrings:AzureCosmos"] ?? string.Empty, builder.Configuration["Azure:Cosmos:Database"] ?? string.Empty)
    )
    .AddRoomService();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
