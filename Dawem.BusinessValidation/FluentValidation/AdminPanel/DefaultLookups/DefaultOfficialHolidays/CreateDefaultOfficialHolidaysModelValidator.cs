using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultOfficialHolidaysTypes;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.AdminPanel.DefaultLookups.DefaultOfficialHolidaysTypes
{
    public class CreateDefaultOfficialHolidaysModelValidator : AbstractValidator<CreateDefaultOfficialHolidaysDTO>
    {
        public CreateDefaultOfficialHolidaysModelValidator()
        {

            //RuleFor(model => model.Name).NotNull().
            //       WithMessage(LeillaKeys.SorryYouMustEnterOfficialHolidaysTypeName);
            //RuleFor(model => model.DefaultType).IsInEnum().
            //       WithMessage(LeillaKeys.SorryYouMustEnterCorrectOfficialHolidayType);
        }
    }
}
