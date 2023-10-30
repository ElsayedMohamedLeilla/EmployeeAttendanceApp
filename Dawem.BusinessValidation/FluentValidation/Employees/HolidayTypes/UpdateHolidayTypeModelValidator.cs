using Dawem.Models.Dtos.Employees.HolidayType;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.HolidayTypes
{
    public class UpdateHolidayTypeModelValidator : AbstractValidator<UpdateHolidayTypeModel>
    {
        public UpdateHolidayTypeModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(DawemKeys.SorryYouMustEnterHolidayTypeId);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterHolidayTypeName);

        }
    }
}
