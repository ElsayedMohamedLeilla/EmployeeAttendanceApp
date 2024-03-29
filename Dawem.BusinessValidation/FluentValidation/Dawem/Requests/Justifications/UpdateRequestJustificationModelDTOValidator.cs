using Dawem.Models.Requests.Justifications;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Requests.Justifications
{
    public class UpdateRequestJustificationModelDTOValidator : AbstractValidator<UpdateRequestJustificationDTO>
    {
        public UpdateRequestJustificationModelDTOValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterRequestId);

            RuleFor(model => model.EmployeeId).NotNull()
                .When(m => m.ForEmployee)
                .WithMessage(AmgadKeys.SorryYouMustChooseEmployeeForRequestJustification);

            RuleFor(model => model.JustificationTypeId).GreaterThan(0)
               .WithMessage(AmgadKeys.SorryYouMustChooseJustificationType);

            RuleFor(model => model.DateFrom).Must(d => d != default)
               .WithMessage(AmgadKeys.SorryYouMustEnterDateFromForJustificationRequest);

            RuleFor(model => model.DateTo).Must(d => d != default)
               .WithMessage(AmgadKeys.SorryYouMustEnterDateToForJustificationRequest);

            RuleFor(model => model).Must(d => d.DateTo >= d.DateFrom)
              .WithMessage(LeillaKeys.SorryDateToMustGreaterThanOrEqualDateFrom);


        }
    }
}
