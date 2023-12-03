using Dawem.Helpers;
using Dawem.Models.Dtos.Attendances.FingerprintDevices;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Attendances.FingerprintDevices
{
    public class CreateFingerprintDeviceModelValidator : AbstractValidator<CreateFingerprintDeviceModel>
    {
        public CreateFingerprintDeviceModelValidator()
        {
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
