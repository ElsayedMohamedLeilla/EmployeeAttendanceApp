using Dawem.Models.Dtos.Summons.Sanctions;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Summons.Sanctions
{
    public class CreateSanctionModelValidator : AbstractValidator<CreateSanctionModel>
    {
        public CreateSanctionModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterSanctionName);

            /*RuleFor(model => model.Type).IsInEnum().
                   WithMessage(LeillaKeys.SorryYouMustEnterSanctionType);*/

            RuleFor(model => model.WarningMessage)
                .NotNull().WithMessage(LeillaKeys.SorryYouMustEnterSanctionWarningMessage);

        }
    }
}
