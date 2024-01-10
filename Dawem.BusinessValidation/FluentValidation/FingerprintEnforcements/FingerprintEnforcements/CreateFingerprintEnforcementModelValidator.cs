using Dawem.Enums.Generals;
using Dawem.Models.Dtos.FingerprintEnforcements.FingerprintEnforcements;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.AssignmentTypes
{
    public class CreateFingerprintEnforcementModelValidator : AbstractValidator<CreateFingerprintEnforcementModel>
    {
        public CreateFingerprintEnforcementModelValidator()
        {
            RuleFor(model => model.ForType)
                .IsInEnum()
                .WithMessage(LeillaKeys.SorryYouMustEnterForType);

            RuleFor(model => model.ForAllEmployees)
                .Must(f => f == null || f == false)
                .When(model => model.ForType != ForType.Employees)
                .WithMessage(LeillaKeys.SorryYouMustNotSetForAllEmployeesWhenTypeNotEmployees);

            RuleFor(model => model.FingerprintDate).Must(d => d != default)
               .WithMessage(LeillaKeys.SorryYouMustEnterFingerprintDate);

            RuleFor(model => model.AllowedTime)
                .GreaterThan(0)
                .WithMessage(LeillaKeys.SorryYouMustEnterAllowedTime);

            RuleFor(model => model.TimeType)
                .IsInEnum()
                .WithMessage(LeillaKeys.SorryYouMustChooseTimeType);

            RuleFor(model => model.Actions)
                .Must(a => a != null && a.Count > 0)
                .WithMessage(LeillaKeys.SorryYouMustChooseAtLeastOneAction);

            RuleFor(model => model.Employees)
               .Must(a => a != null && a.Count > 0)
               .When(model => model.ForType == ForType.Employees && (model.ForAllEmployees == null || !model.ForAllEmployees.Value))
               .WithMessage(LeillaKeys.SorryYouMustChooseAtLeastOneEmployee);

            RuleFor(model => model.Groups)
               .Must(a => a != null && a.Count > 0)
               .When(model => model.ForType == ForType.Groups)
               .WithMessage(LeillaKeys.SorryYouMustChooseAtLeastOneGroup);

            RuleFor(model => model.Departments)
               .Must(a => a != null && a.Count > 0)
               .When(model => model.ForType == ForType.Departments)
               .WithMessage(LeillaKeys.SorryYouMustChooseAtLeastOneDepartment);
        }
    }
}
