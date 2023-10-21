using Dawem.Models.Dtos.Provider;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Authentication
{
    public class CreateEmployeeModelValidator : AbstractValidator<CreateEmployeeModel>
    {
        public CreateEmployeeModelValidator()
        {
            RuleFor(model => model.DepartmentId).GreaterThan(0).
                    WithMessage(DawemKeys.SorryYouMustChooseDepartment);
            RuleFor(model => model.Name).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterEmployeeName);
            RuleFor(model => model.JoiningDate).GreaterThan(default(DateTime)).
                   WithMessage(DawemKeys.SorryYouMustEnterEmployeeName);
        }
    }
}
