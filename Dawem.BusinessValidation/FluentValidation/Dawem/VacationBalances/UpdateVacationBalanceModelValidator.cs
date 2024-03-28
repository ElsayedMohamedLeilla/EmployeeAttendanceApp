using Dawem.Models.Dtos.Dawem.Others.VacationBalances;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.VacationBalances
{
    public class UpdateVacationBalanceModelValidator : AbstractValidator<UpdateVacationBalanceModel>
    {
        public UpdateVacationBalanceModelValidator()
        {
            RuleFor(model => model.Id)
                .Must(s => s > 0)
                .WithMessage(LeillaKeys.SorryYouMustEnterVacationBalanceId);

            RuleFor(model => model.EmployeeId)
                .Must(s => s > 0)
                .WithMessage(LeillaKeys.SorryYouMustChooseEmployee);

            RuleFor(model => model.Year)
                .Must(s => s > 0)
                .WithMessage(LeillaKeys.SorryYouMustChooseYear);

            RuleFor(model => model.DefaultVacationType)
                .IsInEnum()
                .WithMessage(AmgadKeys.SorryYouMustChooseVacationType);

            RuleFor(model => model.Balance)
                .Must(d => d > 0)
                .WithMessage(LeillaKeys.SorryYouMustEnterVacationBalance);
        }
    }
}
