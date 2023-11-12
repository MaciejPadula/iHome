using iHome.Model;
using iHome.Microservices.Authorization.Contract;
using iHome.Microservices.Authorization.Contract.Models.Response;

namespace iHome.Core.Services.Validation.Validators.SimpleValidators;

public class RoomReadValidator : SimpleValidatorBase, IValidator
{
    private readonly IRoomAuthService _roomAuthService;

    public RoomReadValidator(IRoomAuthService roomAuthService)
    {
        _roomAuthService = roomAuthService;
    }

    public override bool IsMatched(ValidatorType type) =>
        type == ValidatorType.RoomRead;

    protected override Task<AuthResponse> AuthorizationMethod(Guid id, string userId) =>
        _roomAuthService.CanReadRoom(new() { RoomId = id, UserId = userId });
}
