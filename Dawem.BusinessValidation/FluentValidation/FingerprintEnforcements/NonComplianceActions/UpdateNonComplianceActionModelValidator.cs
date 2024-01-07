using Dawem.Models.Dtos.FingerprintEnforcements.NonComplianceActions;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.NonComplianceActions
{
    public class UpdateNonComplianceActionModelValidator : AbstractValidator<UpdateNonComplianceActionModel>
    {
        public UpdateNonComplianceActionModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterNonComplianceActionId);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterNonComplianceActionName);

        }
    }
}
