using Dawem.Models.Dtos.Employees.Departments;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.Departments
{
    public class CreatePlanModelValidator : AbstractValidator<CreatePlanModel>
    {
        public CreatePlanModelValidator()
        {

            RuleFor(model => model.NameAr).
                NotNull().
                WithMessage(LeillaKeys.SorryYouMustEnterPlanName);

            RuleFor(model => model.MinNumberOfEmployees).
                Must(n => n > 0).
                WithMessage(LeillaKeys.SorryYouMustEnterPlanMinNumberOfEmployees);

            RuleFor(model => model.MaxNumberOfEmployees).
                Must(x => x > 0).
                WithMessage(LeillaKeys.SorryYouMustEnterPlanMaxNumberOfEmployees);

            RuleFor(model => model.EmployeeCost).
                Must(x => x > 0).
                WithMessage(LeillaKeys.SorryYouMustEnterPlanEmployeeCost);

            RuleFor(model => model).
                Must(x => x.MinNumberOfEmployees <= x.MaxNumberOfEmployees).
                WithMessage(LeillaKeys.SorryPlanMinNumberOfEmployeesMustLessThanOrEqualMaxNumberOfEmployees);

        }
    }
}
