using Dawem.Models.Dtos.Provider;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.Department
{
    public class CreateDepartmentModelValidator : AbstractValidator<CreateDepartmentModel>
    {
        public CreateDepartmentModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterDepartmentName);

        }
    }
}
