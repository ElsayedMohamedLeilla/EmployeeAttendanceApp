using Dawem.Models.Dtos.Summons.Sanctions;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Summons.Sanctions
{
    public class UpdateSanctionModelValidator : AbstractValidator<UpdateSanctionModel>
    {
        public UpdateSanctionModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterSanctionId);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterSanctionName);

            RuleFor(model => model.Type).IsInEnum().
                  WithMessage(LeillaKeys.SorryYouMustEnterSanctionType);

            RuleFor(model => model.WarningMessage)
                .NotNull()
                .WithMessage(LeillaKeys.SorryYouMustEnterSanctionWarningMessage);

        }
    }
}
