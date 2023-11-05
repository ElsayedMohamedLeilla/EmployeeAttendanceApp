using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.Employees
{
    public class UpdateEmployeeModelValidator : AbstractValidator<UpdateEmployeeModel>
    {
        public UpdateEmployeeModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterEmployeeId);
            RuleFor(model => model.DepartmentId).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustChooseDepartment);
            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterEmployeeName);
            RuleFor(model => model.JoiningDate).GreaterThan(default(DateTime)).
                   WithMessage(LeillaKeys.SorryYouMustEnterEmployeeName);
        }
    }
}
