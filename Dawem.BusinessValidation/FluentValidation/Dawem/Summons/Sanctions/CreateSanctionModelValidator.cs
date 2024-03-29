using Dawem.Models.Dtos.Dawem.Summons.Sanctions;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Summons.Sanctions
{
    public class CreateSanctionModelValidator : AbstractValidator<CreateSanctionModel>
    {
        public CreateSanctionModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterSanctionName);

            RuleFor(model => model.Type).IsInEnum().
                   WithMessage(LeillaKeys.SorryYouMustEnterSanctionType);

            RuleFor(model => model.WarningMessage)
                .NotNull().WithMessage(LeillaKeys.SorryYouMustEnterSanctionWarningMessage);

        }
    }
}
