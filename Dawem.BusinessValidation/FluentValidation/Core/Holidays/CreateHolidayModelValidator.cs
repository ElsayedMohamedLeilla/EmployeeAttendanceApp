using Dawem.Models.Dtos.Core.Holidays;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Core.Holidays
{
    public class CreateHolidayModelValidator : AbstractValidator<CreateHolidayDTO>
    {
        public CreateHolidayModelValidator()
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
