using Dawem.Models.Dtos.Dawem.Employees.HolidayTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Employees.HolidayTypes
{
    public class UpdateHolidayTypeModelValidator : AbstractValidator<UpdateHolidayTypeModel>
    {
        public UpdateHolidayTypeModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterHolidayTypeId);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterHolidayTypeName);

        }
    }
}
