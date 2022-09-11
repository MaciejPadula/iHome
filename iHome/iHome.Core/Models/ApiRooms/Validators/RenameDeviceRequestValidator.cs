using FluentValidation;
using iHome.Models.Requests;

namespace iHome.Core.Models.RoomsApi.Validators
{
    public class RenameDeviceRequestValidator : AbstractValidator<RenameDeviceRequest>
    {
        public RenameDeviceRequestValidator()
        {
            RuleFor(x => x.DeviceName)
                .NotEmpty()
                .MinimumLength(3);
            RuleFor(x => x.DeviceId).NotNull();
        }
    }
}
