﻿using Dawem.Models.Dtos.Dawem.Core.Holidays;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Core.Holidays
{
    public class UpdateHolidayModelValidator : AbstractValidator<UpdateHolidayDTO>
    {
        public UpdateHolidayModelValidator()
        {
            RuleFor(model => model.Name).NotNull().
                 WithMessage(AmgadKeys.SorryYouMustEnterHolidayName);

            RuleFor(model => model.DateType)
           .IsInEnum().WithMessage(AmgadKeys.SorryDateTypeMustBe0ForGregorianDateOr1HijriDate);

            RuleFor(model => model.StartDate).Must(d => d != default)
             .WithMessage(AmgadKeys.SorryYouMustEnterStartDateForHoliday);

            RuleFor(model => model.EndDate).Must(d => d != default)
            .WithMessage(AmgadKeys.SorryYouMustEnterEndDateForHoliday);

        }
    }
}
