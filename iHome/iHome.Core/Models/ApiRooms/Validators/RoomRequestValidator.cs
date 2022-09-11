using FluentValidation;
using iHome.Models.Requests;

namespace iHome.Core.Models.RoomsApi.Validators
{
    public class RoomRequestValidator : AbstractValidator<RoomRequest>
    {
        public RoomRequestValidator()
        {
            RuleFor(x => x.RoomName).MinimumLength(3);
            RuleFor(x => x.RoomDescription).NotNull();
        }
    }
}
