using iHome.Model;
using iHome.Microservices.Authorization.Contract;
using iHome.Microservices.Authorization.Contract.Models.Response;

namespace iHome.Core.Services.Validation.Validators.SimpleValidators;

public class WidgetReadValidator : SimpleValidatorBase, IValidator
{
    private readonly IWidgetAuthService _widgetAuthService;

    public WidgetReadValidator(IWidgetAuthService widgetAuthService)
    {
        _widgetAuthService = widgetAuthService;
    }

    public override bool IsMatched(ValidatorType type) =>
        type == ValidatorType.WidgetRead;

    protected override Task<AuthResponse> AuthorizationMethod(Guid id, string userId) =>
        _widgetAuthService.CanReadWidget(new() { WidgetId = id, UserId = userId });
}
