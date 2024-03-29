using Dawem.Domain.Entities.Core;
using Dawem.Helpers;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Core.Groups
{
    public class UpdateGroupModelValidator : AbstractValidator<Zone>
    {
        public UpdateGroupModelValidator()
        {
            RuleFor(model => model.Name).NotNull().
                   WithMessage(AmgadKeys.SorryYouMustEnterZoneName);

            RuleFor(model => model.Latitude).GreaterThan(0).
                 WithMessage(LeillaKeys.SorryYouMustEnterTheLatitude);
            RuleFor(model => model.Longitude).GreaterThan(0).
                   WithMessage(LeillaKeys.SorryYouMustEnterTheLongitude);
            RuleFor(model => model).Must(lat => lat.Latitude.IsValidLatitude()).
                   WithMessage(LeillaKeys.SorryYouMustEnterCorrectLatitude);
            RuleFor(model => model).Must(lat => lat.Longitude.IsValidLongitude()).
                   WithMessage(LeillaKeys.SorryYouMustEnterCorrectLongitude);

        }
    }
}
