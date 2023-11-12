using iHome.Model;
using iHome.Microservices.Authorization.Contract;
using iHome.Microservices.Authorization.Contract.Models.Response;

namespace iHome.Core.Services.Validation.Validators.SimpleValidators;

public class DeviceWriteValidator : SimpleValidatorBase, IValidator
{
    private readonly IDeviceAuthService _deviceAuthService;

    public DeviceWriteValidator(IDeviceAuthService deviceAuthService)
    {
        _deviceAuthService = deviceAuthService;
    }

    public override bool IsMatched(ValidatorType type) =>
        type == ValidatorType.DeviceWrite;

    protected override Task<AuthResponse> AuthorizationMethod(Guid id, string userId) =>
        _deviceAuthService.CanWriteDevice(new() { DeviceId = id, UserId = userId });
}
