using Dawem.Models.Dtos.Requests.Vacation;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Requests.Vacation
{
    public class CreateRequestVacationDTOValidator : AbstractValidator<CreateRequestVacationDTO>
    {
        public CreateRequestVacationDTOValidator()
        {
            RuleFor(model => model.EmployeeId).NotNull()
                .When(m => m.ForEmployee)
                .WithMessage(AmgadKeys.SorryYouMustChooseEmployeeForRequestVacation);

            RuleFor(model => model.VacationTypeId).GreaterThan(0)
               .WithMessage(AmgadKeys.SorryYouMustChooseVacationType);

            RuleFor(model => model.DateFrom).Must(d => d != default)
               .WithMessage(AmgadKeys.SorryYouMustEnterDateFromForVacationRequest);

            RuleFor(model => model.DateTo).Must(d => d != default)
               .WithMessage(AmgadKeys.SorryYouMustEnterDateToForVacationRequest);

            RuleFor(model => model).Must(d => d.DateTo >= d.DateFrom)
               .WithMessage(AmgadKeys.SorryDateToMustGreaterThanOrEqualDateFrom);

          

        }
    }
}
