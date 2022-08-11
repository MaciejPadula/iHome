using Auth0.AspNetCore.Authentication;
using iHome.Hubs;
using iHome.Logic.Database;
using iHome.Logic.UserInfo;
using iHome.Models.Application;
using iHome.Services.DatabaseService;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddAuth0WebAppAuthentication(options => {
        options.Domain = builder.Configuration["Auth0:Domain"];
        options.ClientId = builder.Configuration["Auth0:ClientId"];
        options.Scope = "openid profile email";
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "iHome Rooms API", Version = "v1" });
});

builder.Services.AddControllersWithViews();


builder.Services.Configure<ApplicationSettings>(builder.Configuration);
builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped<IUserInfo, UserInfo>();
builder.Services.AddScoped<IDatabaseService, AzureDatabaseService>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<RoomsHub>("/roomsHub");

string[] urls = builder.Configuration["Addresses"].Split("&");
for(int i = 0; i < urls.Length; ++i)
{
    app.Urls.Add(urls[i]);
}


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
