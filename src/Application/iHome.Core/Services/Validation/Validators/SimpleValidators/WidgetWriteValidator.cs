using iHome.Model;
using iHome.Microservices.Authorization.Contract;
using iHome.Microservices.Authorization.Contract.Models.Response;

namespace iHome.Core.Services.Validation.Validators.SimpleValidators;

public class WidgetWriteValidator : SimpleValidatorBase, IValidator
{
    private readonly IWidgetAuthService _widgetAuthService;

    public WidgetWriteValidator(IWidgetAuthService widgetAuthService)
    {
        _widgetAuthService = widgetAuthService;
    }

    public override bool IsMatched(ValidatorType type) =>
        type == ValidatorType.WidgetWrite;

    protected override Task<AuthResponse> AuthorizationMethod(Guid id, string userId) =>
        _widgetAuthService.CanWriteWidget(new() { WidgetId = id, UserId = userId });
}
