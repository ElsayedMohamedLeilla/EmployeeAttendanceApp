using Dawem.Models.Dtos.Employees.HolidayTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.HolidayTypes
{
    public class CreateHolidayTypeModelValidator : AbstractValidator<CreateHolidayTypeModel>
    {
        public CreateHolidayTypeModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterTaskTypeName);

        }
    }
}
