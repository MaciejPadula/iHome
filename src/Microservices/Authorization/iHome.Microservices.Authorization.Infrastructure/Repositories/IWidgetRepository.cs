namespace iHome.Microservices.Authorization.Infrastructure.Repositories;

public interface IWidgetRepository
{
    Task<bool> UserHasWriteAccess(Guid widgetId, string userId);
    Task<bool> UserHasReadAccess(Guid widgetId, string userId);
}
