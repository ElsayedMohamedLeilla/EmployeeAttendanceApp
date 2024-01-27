using Dawem.Models.Dtos.FingerprintEnforcements.NonComplianceActions;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.NonComplianceActions
{
    public class CreateNonComplianceActionModelValidator : AbstractValidator<CreateNonComplianceActionModel>
    {
        public CreateNonComplianceActionModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterNonComplianceActionName);

            RuleFor(model => model.Type).IsInEnum().
                   WithMessage(LeillaKeys.SorryYouMustEnterNonComplianceActionType);

            RuleFor(model => model.WarningMessage)
                .NotNull().WithMessage(LeillaKeys.SorryYouMustEnterNonComplianceActionWarningMessage);

        }
    }
}
