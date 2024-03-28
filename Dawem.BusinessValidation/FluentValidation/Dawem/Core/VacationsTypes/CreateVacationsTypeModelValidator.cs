using Dawem.Models.Dtos.Dawem.Core.VacationsTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Core.VacationsTypes
{
    public class CreateVacationsTypeModelValidator : AbstractValidator<CreateVacationsTypeDTO>
    {
        public CreateVacationsTypeModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterVacationsTypeName);
            RuleFor(model => model.DefaultType).IsInEnum().
                   WithMessage(LeillaKeys.SorryYouMustEnterCorrectVacationType);
        }
    }
}
