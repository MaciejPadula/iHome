using iHome.Microservices.OpenAI.Contract;
using iHome.Microservices.OpenAI.Controllers;
using iHome.Microservices.OpenAI.Infrastructure;
using Web.Infrastructure.Microservices.Server.Builders;

var builder = new MicroserviceBuilder(args);

builder.Services.AddSuggestionService(builder.Configuration["OpenAI:ApiToken"]);

builder.RegisterMicroservice<ISuggestionsService, SuggestionsController>();

builder.ConfigureApp(app =>
{
    app.UseHttpsRedirection();
    app.UseAuthorization();
});

var app = builder.Build();
app.Run();
