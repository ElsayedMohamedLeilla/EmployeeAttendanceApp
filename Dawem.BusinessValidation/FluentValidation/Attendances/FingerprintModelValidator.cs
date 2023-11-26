using Dawem.Helpers;
using Dawem.Models.Dtos.Attendances;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Attendances
{
    public class FingerprintModelValidator : AbstractValidator<FingerprintModel>
    {
        public FingerprintModelValidator()
        {
            RuleFor(model => model.Latitude).GreaterThan(0).
                   WithMessage(LeillaKeys.SorryYouMustEnterTheLatitude);
            RuleFor(model => model.Longitude).GreaterThan(0).
                   WithMessage(LeillaKeys.SorryYouMustEnterTheLongitude);
            RuleFor(model => model).Must(lat => lat.Latitude.IsValidLatitude()).
                   WithMessage(LeillaKeys.SorryYouMustEnterCorrectLatitude);
            RuleFor(model => model).Must(lat => lat.Longitude.IsValidLongitude()).
                   WithMessage(LeillaKeys.SorryYouMustEnterCorrectLongitude);
            RuleFor(model => model.Type)
                .IsInEnum()
                .WithMessage(LeillaKeys.SorryYouMustEnterFingerPrintType);
            
        }
    }
}
