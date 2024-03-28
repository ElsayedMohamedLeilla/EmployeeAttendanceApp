using Dawem.Models.Dtos.Dawem.Requests.Permissions;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Requests.Permissions
{
    public class CreateRequestPermissionModelDTOValidator : AbstractValidator<CreateRequestPermissionModelDTO>
    {
        public CreateRequestPermissionModelDTOValidator()
        {
            RuleFor(model => model.EmployeeId).NotNull()
                .When(m => m.ForEmployee)
                .WithMessage(LeillaKeys.SorryYouMustChooseEmployeeForRequestPermission);

            RuleFor(model => model.PermissionTypeId).GreaterThan(0)
               .WithMessage(LeillaKeys.SorryYouMustChoosePermissionType);

            RuleFor(model => model.DateFrom).Must(d => d != default)
               .WithMessage(LeillaKeys.SorryYouMustEnterDateAndTimeFromForPermissionRequest);

            RuleFor(model => model.DateTo).Must(d => d != default)
               .WithMessage(LeillaKeys.SorryYouMustEnterDateAndTimeToForPermissionRequest);

            RuleFor(model => model).Must(d => d.DateTo >= d.DateFrom)
               .WithMessage(LeillaKeys.SorryDateToMustGreaterThanOrEqualDateFrom);

        }
    }
}
