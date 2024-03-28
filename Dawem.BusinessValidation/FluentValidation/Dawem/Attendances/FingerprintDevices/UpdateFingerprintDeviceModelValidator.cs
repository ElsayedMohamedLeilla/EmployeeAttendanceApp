using Dawem.Helpers;
using Dawem.Models.Dtos.Dawem.Attendances.FingerprintDevices;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Attendances.FingerprintDevices
{
    public class UpdateFingerprintDeviceModelValidator : AbstractValidator<UpdateFingerprintDeviceModel>
    {
        public UpdateFingerprintDeviceModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterId);
            RuleFor(model => model.Name).NotNull().
                    WithMessage(LeillaKeys.SorryYouMustEnterFingerprintDeviceName);
            RuleFor(model => model.IpAddress).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterFingerprintDeviceIpAddress);
            RuleFor(model => model.PortNumber).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterFingerprintDevicePortNumber);
            RuleFor(model => model.Model).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterFingerprintDeviceModel);
            RuleFor(model => model.SerialNumber).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterFingerprintDeviceSerialNumber);
            RuleFor(model => model.IpAddress).Must(OthersHelper.ValidateIPv4).
                   WithMessage(LeillaKeys.SorryEnteredIpAddressIsNotValid);
            RuleFor(model => model.PortNumber).Must(x => int.TryParse(x, out var val) && val > 0).
                   WithMessage(LeillaKeys.SorryEnteredPortNumberIsNotValid);
        }
    }
}
