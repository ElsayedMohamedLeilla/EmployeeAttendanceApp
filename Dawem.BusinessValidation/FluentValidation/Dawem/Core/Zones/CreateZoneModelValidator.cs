﻿using Dawem.Helpers;
using Dawem.Models.Dtos.Dawem.Core.Zones;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Core.Zones
{
    public class CreateZoneModelValidator : AbstractValidator<CreateZoneDTO>
    {
        public CreateZoneModelValidator()
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
