using Dawem.Models.Dtos.Employees.Departments;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.Departments
{
    public class CreateDepartmentModelValidator : AbstractValidator<CreateDepartmentModel>
    {
        public CreateDepartmentModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterDepartmentName);

            
        }
    }
}
