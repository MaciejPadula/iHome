using iHome.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Features.AddSchedule.Service.GetSuggestedTime;

internal class GetSuggestedTimeQueryHandler : IAsyncQueryHandler<GetSuggestedTimeQuery>
{
    private readonly IScheduleSuggestionsProvider _scheduleSuggestionsProvider;

    public GetSuggestedTimeQueryHandler(IScheduleSuggestionsProvider scheduleSuggestionsProvider)
    {
        _scheduleSuggestionsProvider = scheduleSuggestionsProvider;
    }

    public async Task HandleAsync(GetSuggestedTimeQuery query)
    {
        query.Result = await _scheduleSuggestionsProvider.GetTime(query.Name);
    }
}
