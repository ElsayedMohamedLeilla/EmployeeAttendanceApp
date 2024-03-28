using Dawem.Models.Dtos.Dawem.Employees.Departments;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Employees.Departments
{
    public class UpdateDepartmentModelValidator : AbstractValidator<UpdateDepartmentModel>
    {
        public UpdateDepartmentModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterDepartmentId);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterDepartmentName);

        }
    }
}
