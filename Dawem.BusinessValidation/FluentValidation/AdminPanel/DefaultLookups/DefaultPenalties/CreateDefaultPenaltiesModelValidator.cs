using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultPenalties;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.AdminPanel.DefaultLookups.DefaultPenalties
{
    public class CreateDefaultPenaltiesModelValidator : AbstractValidator<CreateDefaultPenaltiesDTO>
    {
        public CreateDefaultPenaltiesModelValidator()
        {

            //RuleFor(model => model.Name).NotNull().
            //       WithMessage(LeillaKeys.SorryYouMustEnterPenaltiesName);
            //RuleFor(model => model.DefaultType).IsInEnum().
            //       WithMessage(LeillaKeys.SorryYouMustEnterCorrectVacationType);
        }
    }
}
