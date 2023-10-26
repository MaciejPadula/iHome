using iHome.Microservices.OpenAI.Contract;
using iHome.Microservices.OpenAI.Controllers;
using iHome.Microservices.OpenAI.Infrastructure;
using iHome.Microservices.OpenAI.Services;
using Web.Infrastructure.Microservices.Server.Builders;

var builder = new MicroserviceBuilder(args);

builder.Services.AddSuggestionsProviders(builder.Configuration["OpenAI:ApiToken"]);
builder.Services.AddScoped<ISuggestionsManager, SuggestionsManager>();

builder.RegisterMicroservice<ISuggestionsService, SuggestionsController>();

builder.ConfigureApp(app =>
{
    app.UseHttpsRedirection();
    app.UseAuthorization();
});

var app = builder.Build();
app.Run();
