using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Others.VacationBalances;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.VacationBalances
{
    public class CreateVacationBalanceValidator : AbstractValidator<CreateVacationBalanceModel>
    {
        public CreateVacationBalanceValidator()
        {
            RuleFor(model => model.ForType)
                .IsInEnum()
                .WithMessage(LeillaKeys.SorryYouMustEnterForType);

            RuleFor(model => model.EmployeeId)
                .Must(s => s > 0)
                .When(s => s.ForType == ForType.Employees)
                .WithMessage(LeillaKeys.SorryYouMustChooseEmployeeWhenForTypeIsEmployee);

            RuleFor(model => model.GroupId)
               .Must(s => s > 0)
               .When(s => s.ForType == ForType.Groups)
               .WithMessage(LeillaKeys.SorryYouMustChooseGroupWhenForTypeIsGroup);

            RuleFor(model => model.DepartmentId)
               .Must(s => s > 0)
               .When(s => s.ForType == ForType.Departments)
               .WithMessage(LeillaKeys.SorryYouMustChooseDepartmentWhenForTypeIsDepartment);

            RuleFor(model => model.Year)
                .Must(s => s > 0)
                .WithMessage(LeillaKeys.SorryYouMustChooseYear);

            RuleFor(model => model.VacationType)
                .IsInEnum()
                .WithMessage(AmgadKeys.SorryYouMustChooseVacationType);

            RuleFor(model => model.Balance)
                .Must(d => d > 0)
                .WithMessage(LeillaKeys.SorryYouMustEnterVacationBalance);
        }
    }

}
