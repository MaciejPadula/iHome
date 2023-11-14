using iHome.Model;
using iHome.Repository;
using iHome.Shared.Logic;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Features.AddSchedule.Service.GetSuggestedDevices;

internal class GetSuggestedDevicesQueryHandler : IAsyncQueryHandler<GetSuggestedDevicesQuery>
{
    private readonly IScheduleSuggestionsProvider _scheduleSuggestionsProvider;
    private readonly ITimeModelParser _timeModelParser;

    public GetSuggestedDevicesQueryHandler(IScheduleSuggestionsProvider scheduleSuggestionsProvider, ITimeModelParser timeModelParser)
    {
        _scheduleSuggestionsProvider = scheduleSuggestionsProvider;
        _timeModelParser = timeModelParser;
    }

    public async Task HandleAsync(GetSuggestedDevicesQuery query)
    {
        var devicesIds = await _scheduleSuggestionsProvider.GetDevices(query.Name, _timeModelParser.Parse(query.Time));
        //query.UserId;
        var devices = new List<DeviceDto>();
        query.Result = devices;
    }
}
