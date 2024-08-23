using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultVacationsTypes;
using Dawem.Models.Dtos.Dawem.Core.VacationsTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Core.VacationsTypes
{
    public class CreateDefaultVacationsTypeModelValidator : AbstractValidator<CreateDefaultVacationsTypeDTO>
    {
        public CreateDefaultVacationsTypeModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterVacationsTypeName);
            RuleFor(model => model.DefaultType).IsInEnum().
                   WithMessage(LeillaKeys.SorryYouMustEnterCorrectVacationType);
        }
    }
}
