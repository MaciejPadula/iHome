using iHome.Core.Logic;
using iHome.Core.Services;
using iHome.Core.Services.Validation;
using iHome.Core.Services.Validation.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Core.Helpers;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        // logic
        services.AddTransient<ITimeModelParser, TimeModelParser>();

        // validation
        services.AddScoped<IValidationService, ValidationService>();

        // services
        services.AddScoped<IDevicesForSchedulingAccessor, DevicesForSchedulingAccessor>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IWidgetService, WidgetService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IScheduleService, ScheduleService>();
        services.AddScoped<ISuggestionService, SuggestionService>();

        return services;
    }
}
