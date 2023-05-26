namespace iHome.Microservices.Widgets.Infrastructure.Repositories;

public interface IWidgetDeviceRepository
{
    Task Add(Guid widgetId, Guid deviceId);
    Task<IEnumerable<Guid>> GetDeviceIdsByWidgetId(Guid widgetId);
    Task Remove(Guid widgetId, Guid deviceId);
}
