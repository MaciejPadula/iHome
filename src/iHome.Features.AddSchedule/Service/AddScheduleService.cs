using iHome.Features.AddSchedule.Service.AddSchedule;
using iHome.Features.AddSchedule.Service.GetSuggestedDevices;
using iHome.Features.AddSchedule.Service.GetSuggestedTime;
using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator;

namespace iHome.Features.AddSchedule.Service;

internal class AddScheduleService : IAddScheduleService
{
    private readonly IMediator _mediator;

    public AddScheduleService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task AddSchedule(string name, int day, string time, string userId)
    {
        await _mediator.HandleCommandAsync(new AddScheduleCommand
        {
            Name = name,
            Day = day,
            Time = time,
            UserId = userId
        });
    }

    public async Task<List<DeviceDto>> GetSuggestedDevices(string name, string time, string userId)
    {
        var query = await _mediator.HandleQueryAsync(new GetSuggestedDevicesQuery
        {
            Name = name,
            Time = time,
            UserId = userId
        });

        return query.Result;
    }

    public async Task<TimeModel> GetSuggestedTime(string name)
    {
        var query = await _mediator.HandleQueryAsync(new GetSuggestedTimeQuery
        {
            Name = name
        });

        return query.Result;
    }
}
