using Dawem.Models.Dtos.Requests.Vacation;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Requests.Vacation
{
    public class CreateRequestVacationDTOValidator : AbstractValidator<CreateRequestVacationDTO>
    {
        public CreateRequestVacationDTOValidator()
        {
            /*RuleFor(model => model.EmployeeId).NotNull()
                .When(m => m.ForEmployee)
                .WithMessage(LeillaKeys.SorryYouMustChooseEmployeeForRequestVacation);

            RuleFor(model => model.VacationTypeId).GreaterThan(0)
               .WithMessage(LeillaKeys.SorryYouMustChooseVacationType);

            RuleFor(model => model.DateFrom).Must(d => d != default)
               .WithMessage(LeillaKeys.SorryYouMustEnterDateFromForVacationRequest);

            RuleFor(model => model.DateTo).Must(d => d != default)
               .WithMessage(LeillaKeys.SorryYouMustEnterDateToForVacationRequest);

            RuleFor(model => model).Must(d => d.DateTo >= d.DateFrom)
               .WithMessage(LeillaKeys.SorryDateToMustGreaterThanOrEqualDateFrom);*/



        }
    }
}
