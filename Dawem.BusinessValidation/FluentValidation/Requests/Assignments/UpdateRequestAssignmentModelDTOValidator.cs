using Dawem.Models.Dtos.Requests.Assignments;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Requests.Assignments
{
    public class UpdateRequestAssignmentModelDTOValidator : AbstractValidator<UpdateRequestAssignmentModelDTO>
    {
        public UpdateRequestAssignmentModelDTOValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterRequestId);

            RuleFor(model => model.EmployeeId).NotNull()
                .When(m => m.ForEmployee)
                .WithMessage(LeillaKeys.SorryYouMustChooseEmployeeForRequestAssignment);

            RuleFor(model => model.AssignmentTypeId).GreaterThan(0)
               .WithMessage(LeillaKeys.SorryYouMustChooseAssignmentType);

            RuleFor(model => model.DateFrom).Must(d => d != default)
               .WithMessage(LeillaKeys.SorryYouMustEnterDateFromForAssignmentRequest);

            RuleFor(model => model.DateTo).Must(d => d != default)
               .WithMessage(LeillaKeys.SorryYouMustEnterDateToForAssignmentRequest);

            RuleFor(model => model).Must(d => d.DateTo >= d.DateFrom)
              .WithMessage(LeillaKeys.SorryDateToMustGreaterThanOrEqualDateFrom);

        }
    }
}
