using iHome.Core.Services;
using iHome.Core.Services.Validation;
using iHome.Core.Services.Validation.Validators.SimpleValidators;
using Microsoft.Extensions.DependencyInjection;

namespace iHome.Core.Helpers;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        // validation
        services.AddScoped<IValidator, RoomReadValidator>();
        services.AddScoped<IValidator, RoomWriteValidator>();

        services.AddScoped<IValidator, DeviceReadValidator>();
        services.AddScoped<IValidator, DeviceWriteValidator>();

        services.AddScoped<IValidator, WidgetReadValidator>();
        services.AddScoped<IValidator, WidgetWriteValidator>();

        services.AddScoped<IValidator, ScheduleAccessValidator>();

        services.AddScoped<IValidationService, ValidationService>();


        // services
        services.AddScoped<IDevicesForSchedulingAccessor, DevicesForSchedulingAccessor>();
        services.AddScoped<IWidgetService, WidgetService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IScheduleService, ScheduleService>();
        services.AddScoped<ISuggestionService, SuggestionService>();

        return services;
    }
}
