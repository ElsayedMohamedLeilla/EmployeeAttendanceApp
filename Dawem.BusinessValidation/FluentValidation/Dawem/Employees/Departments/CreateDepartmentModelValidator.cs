using Dawem.Models.Dtos.Dawem.Employees.Departments;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Employees.Departments
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
