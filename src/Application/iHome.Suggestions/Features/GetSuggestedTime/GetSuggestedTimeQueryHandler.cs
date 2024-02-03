using iHome.Microservices.OpenAI.Contract;
using iHome.Shared.Logic;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Suggestions.Features.GetSuggestedTime;

internal class GetSuggestedTimeQueryHandler : IAsyncQueryHandler<GetSuggestedTimeQuery>
{
    private readonly ISuggestionsService _suggestionsService;
    private readonly ITimeModelParser _timeModelParser;

    public GetSuggestedTimeQueryHandler(ISuggestionsService suggestionsService, ITimeModelParser timeModelParser)
    {
        _suggestionsService = suggestionsService;
        _timeModelParser = timeModelParser;
    }

    public async Task HandleAsync(GetSuggestedTimeQuery query)
    {
        var time = await _suggestionsService.GetSuggestedTimeByScheduleName(new()
        {
            ScheduleName = query.Name
        });

        query.Result = _timeModelParser.Parse(time.Time);
    }
}
