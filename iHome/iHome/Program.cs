using iHome;
using iHome.Controllers;
using Newtonsoft.Json;

PageController pageController = new PageController("ihome.database.windows.net", "rootAdmin", "VcuraBEFKR6@3PX", "iHome");

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorPages(options => {
    options.RootDirectory = "/wwwroot";
});
builder.Services.AddControllersWithViews();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

app.MapPost("/Register", (HttpContext httpContext) =>
{
    String login = httpContext.Request.Form["Login"];
    String password = httpContext.Request.Form["Password"];
    return "{ \"code\" :"+pageController.GetUserController().RegisterUser(login, password)+" }";
});
app.MapPost("/Login", (HttpContext httpContext) =>
{
    String login = httpContext.Request.Form["Login"];
    String password = httpContext.Request.Form["Password"];

    return pageController.GetUserController().LoginUser(login, password);
});
app.MapPost("/ValidateLogin", (HttpContext httpContext) =>
{
    Guid guid = Guid.Parse(httpContext.Request.Form["AuthKey"]);
    return pageController.GetUserController().CheckSession(guid);
});
//app.MapGet("/test", () => JsonConvert.SerializeObject(pageController.GetUserController().GetAllUsers()));
app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();