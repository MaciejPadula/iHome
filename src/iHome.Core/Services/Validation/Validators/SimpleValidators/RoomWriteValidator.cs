using iHome.Model;
using iHome.Microservices.Authorization.Contract;
using iHome.Microservices.Authorization.Contract.Models.Response;

namespace iHome.Core.Services.Validation.Validators.SimpleValidators;

public class RoomWriteValidator : SimpleValidatorBase, IValidator
{
    private readonly IRoomAuthService _roomAuthService;

    public RoomWriteValidator(IRoomAuthService roomAuthService)
    {
        _roomAuthService = roomAuthService;
    }

    public override bool IsMatched(ValidatorType type) =>
        type == ValidatorType.RoomWrite;

    protected override Task<AuthResponse> AuthorizationMethod(Guid id, string userId) =>
        _roomAuthService.CanWriteRoom(new() { RoomId = id, UserId = userId });
}
