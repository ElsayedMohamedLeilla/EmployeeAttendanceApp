using Dawem.Helpers;
using Dawem.Models.Dtos.Dawem.Attendances;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Attendances
{
    public class FingerprintModelValidator : AbstractValidator<FingerprintModel>
    {
        public FingerprintModelValidator()
        {
            //RuleFor(model => model.Latitude).NotNull().
            //       WithMessage(LeillaKeys.SorryYouMustEnterTheLatitude);
            //RuleFor(model => model.Longitude).NotNull().
            //       WithMessage(LeillaKeys.SorryYouMustEnterTheLongitude);

            RuleFor(model => model).Must(lat => lat.Latitude.IsValidLatitude()).
                   WithMessage(LeillaKeys.SorryYouMustEnterCorrectLatitude);
            RuleFor(model => model).Must(lat => lat.Longitude.IsValidLongitude()).
                   WithMessage(LeillaKeys.SorryYouMustEnterCorrectLongitude);
            /*RuleFor(model => model.Type)
                .IsInEnum()
                .WithMessage(LeillaKeys.SorryYouMustEnterFingerPrintType);*/

        }
    }
}
