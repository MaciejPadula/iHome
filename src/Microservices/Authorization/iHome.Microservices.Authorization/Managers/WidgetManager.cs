using iHome.Microservices.Authorization.Contract.Models.Request;
using iHome.Microservices.Authorization.Infrastructure.Repositories;

namespace iHome.Microservices.Authorization.Managers;

public interface IWidgetManager
{
    Task<bool> CanRead(WidgetAuthRequest request);
    Task<bool> CanWrite(WidgetAuthRequest request);
}

public class WidgetManager : IWidgetManager
{
    private readonly IWidgetRepository _widgetRepository;

    public WidgetManager(IWidgetRepository widgetRepository)
    {
        _widgetRepository = widgetRepository;
    }

    public Task<bool> CanRead(WidgetAuthRequest request) =>
        _widgetRepository.UserHasReadAccess(request.WidgetId, request.UserId);

    public Task<bool> CanWrite(WidgetAuthRequest request) =>
        _widgetRepository.UserHasWriteAccess(request.WidgetId, request.UserId);
}
