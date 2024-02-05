using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Summons.Summons;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Summons.Summons
{
    public class CreateSummonModelValidator : AbstractValidator<CreateSummonModel>
    {
        public CreateSummonModelValidator()
        {
            RuleFor(model => model.ForType)
                .IsInEnum()
                .WithMessage(LeillaKeys.SorryYouMustEnterForType);

            RuleFor(model => model.ForAllEmployees)
                .Must(f => f == null || f == false)
                .When(model => model.ForType != ForType.Employees)
                .WithMessage(LeillaKeys.SorryYouMustNotSetForAllEmployeesWhenTypeNotEmployees);

            RuleFor(model => model.DateAndTime).Must(d => d != default)
               .WithMessage(LeillaKeys.SorryYouMustEnterSummonDateAndTime);

            RuleFor(model => model.AllowedTime)
                .GreaterThan(0)
                .WithMessage(LeillaKeys.SorryYouMustEnterAllowedTime);

            RuleFor(model => model.TimeType)
                .IsInEnum()
                .WithMessage(LeillaKeys.SorryYouMustChooseTimeType);

            RuleFor(model => model.NotifyWays)
                .Must(a => a != null && a.Count > 0)
                .WithMessage(LeillaKeys.SorryYouMustChooseAtLeastOneNotifyWay);

            RuleFor(model => model.Sanctions)
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
