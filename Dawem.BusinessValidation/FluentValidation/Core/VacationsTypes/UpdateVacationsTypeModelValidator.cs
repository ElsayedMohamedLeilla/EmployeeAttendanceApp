using Dawem.Models.Dtos.Core.VacationsTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Core.VacationsTypes
{
    public class UpdateVacationsTypeModelValidator : AbstractValidator<UpdateVacationsTypeDTO>
    {
        public UpdateVacationsTypeModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterVacationsTypeId);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterVacationsTypeName);

            RuleFor(model => model.Type).IsInEnum().
                   WithMessage(LeillaKeys.SorryYouMustEnterCorrectVacationType);

        }
    }
}
