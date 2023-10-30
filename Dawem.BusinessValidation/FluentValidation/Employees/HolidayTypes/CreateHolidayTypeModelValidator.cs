using Dawem.Models.Dtos.Employees.HolidayType;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.HolidayTypes
{
    public class CreateHolidayTypeModelValidator : AbstractValidator<CreateHolidayTypeModel>
    {
        public CreateHolidayTypeModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterTaskTypeName);

        }
    }
}
