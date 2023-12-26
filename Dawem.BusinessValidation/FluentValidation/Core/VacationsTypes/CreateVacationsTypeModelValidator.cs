using Dawem.Models.Dtos.Core.VacationsTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Core.VacationsTypes
{
    public class CreateVacationsTypeModelValidator : AbstractValidator<CreateVacationsTypeDTO>
    {
        public CreateVacationsTypeModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterVacationsTypeName);
            RuleFor(model => model.Type).IsInEnum().
                   WithMessage(LeillaKeys.SorryYouMustEnterCorrectVacationType);
        }
    }
}
