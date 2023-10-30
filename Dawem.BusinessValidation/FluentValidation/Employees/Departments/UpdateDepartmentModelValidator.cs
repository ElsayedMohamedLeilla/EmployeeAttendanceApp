using Dawem.Models.Dtos.Employees.Department;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.Departments
{
    public class UpdateDepartmentModelValidator : AbstractValidator<UpdateDepartmentModel>
    {
        public UpdateDepartmentModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(DawemKeys.SorryYouMustEnterDepartmentId);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterDepartmentName);

        }
    }
}
