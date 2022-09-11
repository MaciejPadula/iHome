using FluentValidation;
using iHome.Core.Models.ApiRooms;

namespace iHome.Core.Models.RoomsApi.Validators
{
    public class DeviceRequestValidator : AbstractValidator<Device>
    {
        public DeviceRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty();
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(3);
            RuleFor(x => x.RoomId)
                .NotNull()
                .NotEmpty();
        }
    }
}
