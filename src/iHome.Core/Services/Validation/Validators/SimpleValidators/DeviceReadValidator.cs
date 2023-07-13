using iHome.Core.Models;
using iHome.Microservices.Authorization.Contract;
using iHome.Microservices.Authorization.Contract.Models.Response;

namespace iHome.Core.Services.Validation.Validators.SimpleValidators;

public class DeviceReadValidator : SimpleValidatorBase, IValidator
{
    private readonly IDeviceAuthService _deviceAuthService;

    public DeviceReadValidator(IDeviceAuthService deviceAuthService)
    {
        _deviceAuthService = deviceAuthService;
    }

    public override bool IsMatched(ValidatorType type) =>
        type == ValidatorType.DeviceRead;

    protected override Task<AuthResponse> AuthorizationMethod(Guid id, string userId) =>
        _deviceAuthService.CanReadDevice(new() { DeviceId = id, UserId = userId });
}
