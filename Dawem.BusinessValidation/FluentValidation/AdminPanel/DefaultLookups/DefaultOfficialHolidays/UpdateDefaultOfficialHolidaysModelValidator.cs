using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultOfficialHolidaysTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.AdminPanel.DefaultLookups.DefaultOfficialHolidaysTypes
{
    public class UpdateDefaultOfficialHolidaysModelValidator : AbstractValidator<UpdateDefaultOfficialHolidaysDTO>
    {
        public UpdateDefaultOfficialHolidaysModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(AmgadKeys.SorryYouMustEnterHolidayId);

            //RuleFor(model => model.Name).NotNull().
            //       WithMessage(LeillaKeys.SorryYouMustEnterOfficialHolidaysTypeName);

            //RuleFor(model => model.DefaultType).IsInEnum().
            //       WithMessage(LeillaKeys.SorryYouMustEnterCorrectOfficialHolidayType);

        }
    }
}
