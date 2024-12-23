using Dawem.Models.Requests.Justifications;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Requests.Justifications
{
    public class CreateRequestOvertimeDTOValidator : AbstractValidator<CreateRequestOvertimeDTO>
    {
        public CreateRequestOvertimeDTOValidator()
        {
            RuleFor(model => model.EmployeeId).NotNull()
                .When(m => m.ForEmployee)
                .WithMessage(AmgadKeys.SorryYouMustChooseEmployeeForRequestJustification);

            RuleFor(model => model.OvertimeTypeId).GreaterThan(0)
               .WithMessage(LeillaKeys.SorryYouMustChooseOvertimeType);

            RuleFor(model => model.OvertimeDate).Must(d => d != default)
               .WithMessage(LeillaKeys.SorryYouMustEnterOvertimeDate);

            RuleFor(model => model.DateFrom).Must(d => d != default)
               .WithMessage(LeillaKeys.SorryYouMustEnterDateFromForOvertimeRequest);

            RuleFor(model => model.DateTo).Must(d => d != default)
               .WithMessage(LeillaKeys.SorryYouMustEnterDateToForOvertimeRequest);

            RuleFor(model => model).Must(d => d.DateTo > d.DateFrom)
               .WithMessage(LeillaKeys.SorryOvertimeDateToMustGreaterThanDateFrom);

        }
    }
}
